using Microsoft.Owin;
using Monospad.Core.Bootstrapping;
using Monospad.Owin.Web;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Monospad.Owin.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            OwinApp.ConfigureIIS(app);
        }
    }
}