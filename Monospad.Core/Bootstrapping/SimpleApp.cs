using System;
using TagKid.Framework.Hosting;
using TagKid.Framework.Hosting.Http;
using TagKid.Framework.IoC;

namespace Monospad.Core.Bootstrapping
{
    public class SimpleApp
    {
        public static void Start()
        {
            Console.WriteLine("starting...");
            Bootstrapper.StartApp();

            var listener = new SimpleHttpListener(DependencyContainer.Current.Resolve<IHttpRequestHandler>());
            listener.Start();
            Console.WriteLine("running...");
            Console.ReadLine();
            Console.WriteLine("stopping...");
            listener.Stop();
            Console.WriteLine("stopped!");
        }
    }
}
