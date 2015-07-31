using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Self_Host.Startup))]

namespace Self_Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.MapSignalR<SamplePersistentConnection>("/SamplePC");
            app.Run((context) =>
            {
                if (context.Request.Path.Value.Equals("/", StringComparison.
                CurrentCultureIgnoreCase))
                {
                    context.Response.ContentType = "text/html";
                    string result = System.IO.File.ReadAllText(System.Environment.
                    CurrentDirectory + "\\index.html");
                    return context.Response.WriteAsync(result);
                }
                if (context.Request.Path.Value.StartsWith("/scripts/", StringComparison.
                CurrentCultureIgnoreCase))
                {
                    context.Response.ContentType = "text/javascript";
                    //The requested should be verified but adding for simplicity of example.
                    string result = System.IO.File.ReadAllText(System.Environment.
                    CurrentDirectory + context.Request.Path.Value);
                    return context.Response.WriteAsync(result);
                }
                return Task.FromResult<object>(null);
            });
        }
    }
}
