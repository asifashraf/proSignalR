using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using GroupBrush.Web.Unity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security.Cookies;
using System.Web.Http;
using GroupBrush.Entity;
using Microsoft.WindowsAzure;
using Microsoft.Owin.StaticFiles;
using System.Collections.Generic;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles.Infrastructure;

[assembly: OwinStartup(typeof(GroupBrush.Web.Startup))]

namespace GroupBrush.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string strUseRedis = CloudConfigurationManager.GetSetting("UseRedis") ?? "false";
            bool useRedis = bool.Parse(strUseRedis);
            var dependencyResolver = new UnityDependencyResolver();
            UnityWireupConfiguration.WireUp(dependencyResolver);
            GlobalHost.DependencyResolver = dependencyResolver;
            var options = new CookieAuthenticationOptions()
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new PathString("/"),
                LogoutPath = new PathString("/")
            };
            app.UseCookieAuthentication(options);
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Equals("/") ||
                context.Request.Path.Value.StartsWith("/public", StringComparison.CurrentCultureIgnoreCase))
                {
                    await next();
                }
                else if (context.Request.User == null || !context.Request.User.Identity.IsAuthenticated)
                {
                    context.Response.StatusCode = 401;
                }
                else
                {
                    await next();
                }
            });
            HttpConfiguration webApiConfiguration = new HttpConfiguration();
            webApiConfiguration.DependencyResolver = dependencyResolver;
            webApiConfiguration.MapHttpAttributeRoutes();
            app.UseWebApi(webApiConfiguration);
            RedisConfiguration redisConfiguration = dependencyResolver.Resolve<RedisConfiguration>();
            if (redisConfiguration.UseRedis)
            {
                GlobalHost.DependencyResolver.UseRedis(redisConfiguration.HostName, redisConfiguration.Port, redisConfiguration.Password, redisConfiguration.EventKey);
            }
            app.MapSignalR();
            var sharedOptions = new SharedOptions()
            {
                RequestPath = new PathString(string.Empty),
                FileSystem =
                    new PhysicalFileSystem(".//public//content")
            };
            app.UseDefaultFiles(new Microsoft.Owin.StaticFiles.DefaultFilesOptions(sharedOptions)
            {
                DefaultFileNames = new List<string>() { "index.html" }
            });
            app.UseStaticFiles("/public");
            app.UseStaticFiles("/content");
            app.UseStaticFiles("/scripts");
            app.UseStaticFiles("/styles");
            app.UseStaticFiles(new StaticFileOptions(sharedOptions));
        }
    }
}
