using TagKid.Framework.Hosting;
using TagKid.Framework.IoC;
using TagKid.Framework.UnitOfWork;
using TagKid.Framework.Owin;
using TagKid.Framework.Owin.Configuration;

namespace Monospad.Core.Services.Interceptors
{
    public class UnitOfWorkInterceptor : IActionInterceptor
    {
        private IUnitOfWork _uow;

        public object BeforeCall(RouteContext ctx)
        {
            _uow = DependencyContainer.Current.Resolve<IUnitOfWork>();

            if (ctx.Method.HttpMethod != HttpMethod.Get)
            {
                _uow.BeginTransaction();
            }
            return null;
        }

        public void AfterCall(RouteContext ctx)
        {
            if (_uow != null)
            {
                _uow.Commit();
            }
        }

        public object OnException(RouteContext ctx)
        {
            if (_uow != null)
            {
                _uow.Rollback();
            }
            return null;
        }

        public void Dispose()
        {
            if (_uow != null)
            {
                _uow.Dispose();
            }
        }
    }
}
