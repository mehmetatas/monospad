using Monospad.Core.Bootstrapping.Bootstrappers;
using Monospad.Core.Models.Messages;
using Taga.Framework.Hosting;

namespace Monospad.Core.Services
{
    public interface IUserService
    {
        [NoAuth]
        Response Signup(SignupRequest request);

        [NoAuth]
        Response Signin(SigninRequest request);

        [NoAuth]
        Response SigninWithToken(SigninWithTokenRequest request);

        [NoAuth]
        Response Signout(SignoutRequest request);

        [NoAuth]
        Response RecoverPassword(RecoverPasswordRequest request);

        Response ResetPassword(ResetPasswordRequest request);

        Response ChangePassword(ChangePasswordRequest request);
    }
}
