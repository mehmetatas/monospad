using Monospad.Core.Models.Messages;
using Monospad.Core.Services;
using TagKid.Framework.IoC;
using TagKid.Framework.Owin.Configuration;

namespace Monospad.Core.Bootstrapping.Bootstrappers
{
    class ServiceBootstrapper : IBootstrapper
    {
        public void Bootstrap(IDependencyContainer container)
        {
            var builder = ServiceConfig.Builder();

            BuildNoteService(builder);
            BuildUserService(builder);

            builder.Build();
        }

        private void BuildNoteService(ControllerConfigurator builder)
        {
            builder.ControllerFor<INoteService>("note")
                .ActionFor(s => s.SaveNote(default(SaveNoteRequest)), "save", HttpMethod.Post)
                .ActionFor(s => s.GetContent(default(GetContentRequest)), "getContent", HttpMethod.Get)
                .ActionFor(s => s.DeleteNote(default(DeleteNoteRequest)), "delete", HttpMethod.Post);
        }

        private void BuildUserService(ControllerConfigurator builder)
        {
            builder.ControllerFor<IUserService>("user")
                .ActionFor(s => s.Signup(default(SignupRequest)), "signup", HttpMethod.Post).NoAuth()
                .ActionFor(s => s.Signin(default(SigninRequest)), "signin", HttpMethod.Post).NoAuth()
                .ActionFor(s => s.SigninWithToken(default(SigninWithTokenRequest)), "signinWithToken", HttpMethod.Post).NoAuth()
                .ActionFor(s => s.RecoverPassword(default(RecoverPasswordRequest)), "recoverPassword", HttpMethod.Post).NoAuth()
                .ActionFor(s => s.ResetPassword(default(ResetPasswordRequest)), "resetPassword", HttpMethod.Post).NoAuth()
                .ActionFor(s => s.ChangePassword(default(ChangePasswordRequest)), "changePassword", HttpMethod.Post)
                .ActionFor(s => s.Signout(default(SignoutRequest)), "signout", HttpMethod.Post);
        }
    }
}
