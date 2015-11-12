using Monospad.Core.Providers;
using Monospad.Core.Providers.Impl;
using Monospad.Core.Services;
using Monospad.Core.Services.Impl;
using Monospad.Core.Services.Interceptors;
using TagKid.Framework.IoC;
using TagKid.Framework.Json;
using TagKid.Framework.Json.Newtonsoft;
using TagKid.Framework.UnitOfWork;
using TagKid.Framework.UnitOfWork.Impl;
using TagKid.Framework.WebApi;
using TagKid.Framework.WebApi.Impl;

namespace Monospad.Core.Bootstrapping.Bootstrappers
{
    class DependencyBootstrapper : IBootstrapper
    {
        public void Bootstrap(IDependencyContainer container)
        {
            // Framework "er"s
            container.RegisterSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
            container.RegisterSingleton<IRouteResolver, RouteResolver>();
            container.RegisterSingleton<IParameterResolver, ParameterResolver>();
            container.RegisterSingleton<IActionInvoker, ActionInvoker>();
            container.RegisterSingleton<IHttpHandler, HttpHandler>();

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