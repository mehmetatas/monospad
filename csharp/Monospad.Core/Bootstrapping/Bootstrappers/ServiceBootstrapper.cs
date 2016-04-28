using System;
using System.Reflection;
using Monospad.Core.Services;
using Taga.Framework.Hosting.Configuration;
using Taga.Framework.IoC;

namespace Monospad.Core.Bootstrapping.Bootstrappers
{
    class ServiceBootstrapper : IBootstrapper
    {
        public void Bootstrap(IDependencyContainer container)
        {
            container.Resolve<IServiceConfigBuilder>()
                .Register<INoteService>()
                .Register<IUserService>();
        }
    }

    public class MonospadRouteProvider : IServiceRouteProvider
    {
        public string GetServiceRouteName(Type type)
        {
            return ToCamelCase(type.Name
                .Substring(0, type.Name.Length - "Service".Length)
                .Substring(1));
        }

        public HttpMethod GetHttpMethod(MethodInfo method)
        {
            if (method.Name.StartsWith("Get"))
            {
                return HttpMethod.Get;
            }
            return HttpMethod.Post;
        }

        public string GetMethodRoute(MethodInfo method)
        {
            return ToCamelCase(method.Name);
        }

        public bool IsNoAuth(MethodInfo method)
        {
            return method.GetCustomAttribute<NoAuthAttribute>() != null;
        }

        private static string ToCamelCase(string name)
        {
            return char.ToLowerInvariant(name[0]) + name.Substring(1);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class NoAuthAttribute : Attribute
    {

    }
}
