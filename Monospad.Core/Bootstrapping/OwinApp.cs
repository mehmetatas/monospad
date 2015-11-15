using System;
using System.Collections.Generic;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.StaticFiles.ContentTypes;
using Owin;
using TagKid.Framework.Hosting.Owin;
using TagKid.Framework.IoC;

namespace Monospad.Core.Bootstrapping
{
    public class OwinApp
    {
        public static void SelfHost(int port)
        {
            using (WebApp.Start<OwinApp>($"http://localhost:{port}"))
            {
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
            }
        }

        public static void ConfigureIIS(IAppBuilder app)
        {
            Bootstrapper.StartApp();

            var handler = DependencyContainer.Current.Resolve<IOwinHandler>();
            
            app.UseStaticFiles();

            app.Map("/api", builder =>
            {
                builder.Run(handler.HandleRequestAsync);
            });
        }

        public void Configuration(IAppBuilder app)
        {
            Bootstrapper.StartApp();

            var handler = DependencyContainer.Current.Resolve<IOwinHandler>();

            var fileServerOptions = new FileServerOptions
            {
                EnableDefaultFiles = true,
                DefaultFilesOptions = { DefaultFileNames = { "index.html" } },
                FileSystem = new PhysicalFileSystem("www"),
                StaticFileOptions =
                {
                    ContentTypeProvider = new FileExtensionContentTypeProvider(
                        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                        {
                            {".css", "text/css"},
                            {".html", "text/html"},
                            {".js", "application/javascript"}
                        })
                }
            };

            app.UseFileServer(fileServerOptions);

            app.Map("/api", builder =>
            {
                builder.Run(handler.HandleRequestAsync);
            });

            app.Use<OwinSpaMiddleware>("www\\index.html");
        }
    }
}