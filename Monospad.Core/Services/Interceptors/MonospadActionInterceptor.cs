using TagKid.Framework.WebApi;

namespace Monospad.Core.Services.Interceptors
{
    public class MonospadActionInterceptor : IActionInterceptor
    {
        private readonly IActionInterceptor[] _interceptors;

        public MonospadActionInterceptor(params IActionInterceptor[] interceptors)
        {
            _interceptors = interceptors;
        }

        public object BeforeCall(RouteContext ctx)
        {
            foreach (var interceptor in _interceptors)
            {
                var res = interceptor.BeforeCall(ctx);
                if (res != null)
                {
                    return res;
                }
            }
            return null;
        }

        public void AfterCall(RouteContext ctx)
        {
            foreach (var interceptor in _interceptors)
            {
                interceptor.AfterCall(ctx);
            }
        }

        public object OnException(RouteContext ctx)
        {
            foreach (var interceptor in _interceptors)
            {
                var res = interceptor.OnException(ctx);
                if (res != null)
                {
                    return res;
                }
            }
            return null;
        }

        public void Dispose()
        {
            foreach (var interceptor in _interceptors)
            {
                interceptor.Dispose();
            }
        }
    }
}
