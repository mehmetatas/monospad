using System;
using System.Web;
using Monospad.Core.Bootstrapping;

namespace Monospad.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrapper.StartApp();
        }
    }
}