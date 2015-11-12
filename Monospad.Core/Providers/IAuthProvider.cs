using TagKid.Framework.WebApi;

namespace Monospad.Core.Providers
{
    public interface IAuthProvider
    {
        void AuthRoute(RouteContext ctx);
    }
}
