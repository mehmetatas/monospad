using Owin;
using Taga.Framework.Hosting.Owin;
using Taga.Framework.IoC;

namespace Monospad.Core.Bootstrapping
{
    public class OwinApp
    {
        public static void ConfigureIIS(IAppBuilder app)
        {
            Bootstrapper.StartApp();

            var handler = DependencyContainer.Current.Resolve<IOwinHandler>();
            
            app.UseStaticFiles();

            app.Map("/api", builder =>
            {
                builder.Run(handler.Invoke);
            });
        }
    }
}