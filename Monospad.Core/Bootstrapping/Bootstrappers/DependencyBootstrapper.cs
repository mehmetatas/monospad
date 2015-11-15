using Monospad.Core.Providers;
using Monospad.Core.Providers.Impl;
using Monospad.Core.Services;
using Monospad.Core.Services.Impl;
using Monospad.Core.Services.Interceptors;
using TagKid.Framework.Hosting;
using TagKid.Framework.Hosting.Impl;
using TagKid.Framework.Hosting.Owin;
using TagKid.Framework.IoC;
using TagKid.Framework.Json;
using TagKid.Framework.Json.Newtonsoft;
using TagKid.Framework.UnitOfWork;
using TagKid.Framework.UnitOfWork.Impl;

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

            // Database
            container.RegisterTransient<IUnitOfWork, UnitOfWork>();
            container.RegisterSingleton<IRepository, Repository>();

            // Interceptors
            container.RegisterSingleton<IActionInterceptorBuilder, MonospadActionInterceptorBuilder>();

            // Providers
            container.RegisterSingleton<IAuthProvider, AuthProvider>();
            container.RegisterSingleton<ICryptoProvider, CryptoProvider>();
            container.RegisterSingleton<IMailProvider, MailProvider>();

            // Auth
            container.RegisterSingleton<IUserService, UserService>();
            container.RegisterSingleton<INoteService, NoteService>();
        }
    }
}