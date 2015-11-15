using Monospad.Core.Models.Messages;
using TagKid.Framework.Hosting;

namespace Monospad.Core.Services
{
    public interface IUserService
    {
        Response Signup(SignupRequest request);
        Response Signin(SigninRequest request);
        Response SigninWithToken(SigninWithTokenRequest request);
        Response Signout(SignoutRequest request);
        Response RecoverPassword(RecoverPasswordRequest request);
        Response ResetPassword(ResetPasswordRequest request);
        Response ChangePassword(ChangePasswordRequest request);
    }
}
