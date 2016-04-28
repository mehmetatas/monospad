using Taga.Framework.Hosting;
using Taga.Framework.Validation;

namespace Monospad.Core.Services.Interceptors
{
    public class ValidationInterceptor : IActionInterceptor
    {
        public object BeforeCall(RouteContext ctx)
        {
            if (ctx.Parameters.Length == 1)
            {
                ValidationManager.Validate(ctx.Parameters[0]);
            }

            return null;
        }

        public void AfterCall(RouteContext ctx)
        {

        }

        public object OnException(RouteContext ctx)
        {
            return null;
        }

        public void Dispose()
        {

        }
    }
}