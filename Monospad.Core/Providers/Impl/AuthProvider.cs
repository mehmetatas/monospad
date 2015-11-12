using DummyOrm.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using Monospad.Core.Exceptions;
using Monospad.Core.Models.Database;
using TagKid.Framework.UnitOfWork;
using TagKid.Framework.WebApi;

namespace Monospad.Core.Providers.Impl
{
    public class AuthProvider : IAuthProvider
    {
        private readonly IRepository _repo;

        public AuthProvider(IRepository repo)
        {
            _repo = repo;
        }

        public void AuthRoute(RouteContext ctx)
        {
            if (ctx.Method.NoAuth)
            {
                return;
            }

            var tokenGuid = Guid.Empty;

            IEnumerable<string> headerValues;

            if (ctx.Request.Headers.TryGetValues("monospad-auth-token", out headerValues))
            {
                var authToken = headerValues.FirstOrDefault();
                Guid.TryParse(authToken, out tokenGuid);
            }

            if (tokenGuid == Guid.Empty)
            {
                throw Errors.Auth_LoginRequired;
            }

            var login = _repo.Select<Login>()
                .Join(l => l.User)
                .FirstOrDefault(l => l.Token == tokenGuid);

            if (login == null)
            {
                throw Errors.Auth_LoginRequired;
            }

            if (login.ExpireDate < DateTime.UtcNow)
            {
                throw Errors.Auth_LoginTokenExpired;
            }

            MonospadContext.Current.User = login.User;
        }
    }
}
