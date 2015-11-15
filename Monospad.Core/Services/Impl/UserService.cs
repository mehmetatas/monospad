using System;
using System.Collections.Generic;
using DummyOrm.Db;
using Monospad.Core.Exceptions;
using Monospad.Core.Models.Database;
using Monospad.Core.Models.Messages;
using Monospad.Core.Providers;
using TagKid.Framework.Hosting;
using TagKid.Framework.UnitOfWork;
using TagKid.Framework.Owin;

namespace Monospad.Core.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;
        private readonly ICryptoProvider _crypto;
        private readonly IMailProvider _mail;

        public UserService(IRepository repository, ICryptoProvider crypto, IMailProvider mail)
        {
            _repository = repository;
            _crypto = crypto;
            _mail = mail;
        }

        public Response Signup(SignupRequest request)
        {
            var newUser = _repository.Select<User>()
                .FirstOrDefault(u => u.Email == request.Email);

            if (newUser != null)
            {
                throw Errors.User_EmailAlreadyExists;
            }

            newUser = new User
            {
                Email = request.Email,
                Password = _crypto.ComputeHash(request.Password),
                SignupDate = DateTime.UtcNow
            };

            _repository.Insert(newUser);

            var login = CreateLogin(newUser);

            object note = null;
            if (!string.IsNullOrEmpty(request.UnsavedNoteContent))
            {
                var newNote = _repository.SaveNote(request.UnsavedNoteContent, newUser);
                note = newNote.ToItem();
            }

            return Response.Success.WithData(new
            {
                Notes = new[] { note },
                login.Token
            });
        }

        public Response Signin(SigninRequest request)
        {
            var loggedInUser = _repository.Select<User>()
                .FirstOrDefault(u => u.Email == request.Email);

            if (loggedInUser == null)
            {
                throw Errors.User_InvalidEmailOrPassword;
            }

            var passHash = _crypto.ComputeHash(request.Password);

            if (passHash != loggedInUser.Password)
            {
                throw Errors.User_InvalidEmailOrPassword;
            }

            var login = _repository.Select<Login>()
                .FirstOrDefault(l => l.User.Id == loggedInUser.Id);

            if (login?.ExpireDate > DateTime.UtcNow)
            {
                // Extend login
                login.ExpireDate = DateTime.UtcNow.AddDays(Constants.LoginTokenValidDays);

                _repository.Update(login);
            }
            else
            {
                login = CreateLogin(loggedInUser);
            }

            // Get note list
            var notes = GetNoteList(loggedInUser.Id);

            // Create Note
            if (!string.IsNullOrEmpty(request.UnsavedNoteContent))
            {
                var newNote = _repository.SaveNote(request.UnsavedNoteContent, loggedInUser);
                notes.Insert(0, newNote);
            }

            return Response.Success.WithData(new
            {
                Notes = notes,
                login.Token
            });
        }

        public Response SigninWithToken(SigninWithTokenRequest request)
        {
            var login = _repository.Select<Login>()
                .FirstOrDefault(l => l.Token == request.Token);

            if (login == null || login.ExpireDate < DateTime.UtcNow)
            {
                throw Errors.User_TokenExpired;
            }
            
            // Extend login
            login.ExpireDate = DateTime.UtcNow.AddDays(Constants.LoginTokenValidDays);

            _repository.Update(login);
            
            // Get note list
            var notes = GetNoteList(login.User.Id);

            return Response.Success.WithData(new
            {
                Notes = notes
            });
        }

        public Response Signout(SignoutRequest request)
        {
            var login = _repository.Select<Login>()
                .FirstOrDefault(l => l.Token == request.Token);

            if (login == null || login.User.Id != MonospadContext.Current.User.Id)
            {
                throw Errors.User_LoginNotFound;
            }

            login.ExpireDate = DateTime.UtcNow;

            _repository.Update(login);

            return Response.Success;
        }

        public Response RecoverPassword(RecoverPasswordRequest request)
        {
            var user = _repository.Select<User>()
                .FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                throw Errors.User_UnregisteredEmailAddress;
            }

            var logins = _repository.Select<Login>()
                .Where(l => l.User.Id == user.Id && l.ExpireDate > DateTime.UtcNow)
                .ToList();

            foreach (var login in logins)
            {
                login.ExpireDate = DateTime.UtcNow;
                _repository.Update(login);
            }

            var recoveryLogin = CreateLogin(user, true);

            _mail.SendPasswordRecoveryMail(user.Email, recoveryLogin.Token);

            return Response.Success;
        }

        public Response ResetPassword(ResetPasswordRequest request)
        {
            var login = _repository.Select<Login>()
                .Join(l => l.User)
                .FirstOrDefault(l => l.Token == request.Token);

            if (login == null || !login.IsPasswordRecovery)
            {
                throw Errors.User_InvalidPasswordRecoveryToken;
            }

            if (login.ExpireDate < DateTime.UtcNow)
            {
                throw Errors.User_PasswordRecoveryTokenExpired;
            }

            login.ExpireDate = DateTime.UtcNow;
            login.User.Password = _crypto.ComputeHash(request.NewPassword);

            _repository.Update(login);
            _repository.Update(login.User);

            login = CreateLogin(login.User);
            
            return Response.Success.WithData(new
            {
                login.Token
            });
        }

        public Response ChangePassword(ChangePasswordRequest request)
        {
            var user = MonospadContext.Current.User;
            var login = MonospadContext.Current.Login;

            user.Password = _crypto.ComputeHash(request.NewPassword);
            _repository.Update(user);

            login.ExpireDate = DateTime.UtcNow;
            _repository.Update(login);

            login = CreateLogin(user);

            return Response.Success.WithData(new
            {
                login.Token
            });
        }

        private Login CreateLogin(User user, bool recovery = false)
        {
            var login = new Login
            {
                User = user,
                Token = Guid.NewGuid(),
                IsPasswordRecovery = recovery,
                ExpireDate = DateTime.UtcNow.AddDays(recovery ? Constants.PasswordRecoveryValidDays : Constants.LoginTokenValidDays)
            };

            _repository.Insert(login);

            return login;
        }

        private List<Note> GetNoteList(long userId)
        {
            _repository.DeleteMany<Note>(n => n.User.Id == userId && n.Title == null);

            return _repository.Select<Note>()
                .Include(n => new { n.Id, n.Title, n.Summary })
                .Where(n => n.User.Id == userId)
                .OrderByDesc(n => n.LastUpdateDate)
                .ToList();
        }
    }
}
