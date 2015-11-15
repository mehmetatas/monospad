using Monospad.Core.Providers;
using TagKid.Framework.Exceptions;
using TagKid.Framework.Hosting;

namespace Monospad.Core.Services.Interceptors
{
    public class SecurityInterceptor : IActionInterceptor
    {
        private readonly IAuthProvider _authProvider;

        public SecurityInterceptor(IAuthProvider authProvider)
        {
            _authProvider = authProvider;
        }

        public object BeforeCall(RouteContext ctx)
        {
            _authProvider.AuthRoute(ctx);
            return null;
        }

        public void AfterCall(RouteContext ctx)
        {

        }

        public object OnException(RouteContext ctx)
        {
            var ex = ctx.Exception as Error ?? Errors.Unknown;
            return Response.Error(ex);
        }

        public void Dispose()
        {

        }
    }
}