using Monospad.Core.Models.Messages;
using TagKid.Framework.WebApi;

namespace Monospad.Core.Services
{
    public interface IUserService
    {
        Response Signup(SignupRequest request);
        Response Signin(SigninRequest request);
        Response SigninWithToken(SigninWithTokenRequest request);
        Response Signout(SignoutRequest request);
        Response RecoverPassword(RecoverPasswordRequest request);
    }
}
