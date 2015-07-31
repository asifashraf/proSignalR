using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hubs_Server_and_JavaScript_Client.Startup))]
namespace Hubs_Server_and_JavaScript_Client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
