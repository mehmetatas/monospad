using TagKid.Framework.Hosting;

namespace Monospad.Core.Providers
{
    public interface IAuthProvider
    {
        void AuthRoute(RouteContext ctx);
    }
}
