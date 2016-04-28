using System;
using System.Collections.Generic;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.StaticFiles.ContentTypes;
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
            
            app.Map("/api", builder =>
            {
                builder.Run(handler.Invoke);
            });

            app.UseFileServer(new FileServerOptions
            {
                EnableDefaultFiles = true,
                DefaultFilesOptions = {DefaultFileNames = {"index.html"}},
                FileSystem = new PhysicalFileSystem("app"),
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
            });

            app.Use<OwinSpaMiddleware>("index.html");
        }
    }
}