using Monospad.Core.Providers;
using TagKid.Framework.Hosting;
using TagKid.Framework.Owin;

namespace Monospad.Core.Services.Interceptors
{
    public class MonospadActionInterceptorBuilder : IActionInterceptorBuilder
    {
        private readonly IAuthProvider _authProvider;

        public MonospadActionInterceptorBuilder(IAuthProvider authProvider)
        {
            _authProvider = authProvider;
        }

        public IActionInterceptor Build(RouteContext context)
        {
            return new MonospadActionInterceptor(
                new ValidationInterceptor(),
                new UnitOfWorkInterceptor(),
                new SecurityInterceptor(_authProvider));
        }
    }
}
