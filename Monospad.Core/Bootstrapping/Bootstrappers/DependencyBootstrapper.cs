using Monospad.Core.Providers;
using Monospad.Core.Providers.Impl;
using Monospad.Core.Services;
using Monospad.Core.Services.Impl;
using Monospad.Core.Services.Interceptors;
using Taga.Framework.Hosting;
using Taga.Framework.Hosting.Configuration;
using Taga.Framework.Hosting.Impl;
using Taga.Framework.Hosting.Owin;
using Taga.Framework.IoC;
using Taga.Framework.Json;
using Taga.Framework.Json.Newtonsoft;
using Taga.Orm.UnitOfWork;
using Taga.Orm.UnitOfWork.Impl;

namespace Monospad.Core.Bootstrapping.Bootstrappers
{
    class DependencyBootstrapper : IBootstrapper
    {
        public void Bootstrap(IDependencyContainer container)
        {
            // Framework "er"s
            container.RegisterSingleton<IHttpRequestHandler, HttpRequestHandler>();
            container.RegisterSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
            container.RegisterSingleton<IRouteResolver, RouteResolver>();
            container.RegisterSingleton<IParameterResolver, ParameterResolver>();
            container.RegisterSingleton<IActionInvoker, ActionInvoker>();
            container.RegisterSingleton<IOwinHandler, GenericOwinHandler>();
            container.RegisterSingleton<IServiceConfigBuilder, ServiceConfigBuilder>();

            // Database
            container.RegisterTransient<IUnitOfWork, UnitOfWork>();
            container.RegisterSingleton<IUnitOfWorkStack, UnitOfWorkStack>();
            container.RegisterSingleton<IRepository, Repository>();

            // Interceptors
            container.RegisterSingleton<IActionInterceptorBuilder, MonospadActionInterceptorBuilder>();

            // Providers
            container.RegisterSingleton<IAuthProvider, AuthProvider>();
            container.RegisterSingleton<ICryptoProvider, CryptoProvider>();
            container.RegisterSingleton<IMailProvider, MailProvider>();
            container.RegisterSingleton<IServiceRouteProvider, MonospadRouteProvider>();

            // Auth
            container.RegisterSingleton<IUserService, UserService>();
            container.RegisterSingleton<INoteService, NoteService>();
        }
    }
}