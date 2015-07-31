using Microsoft.Owin;
using Owin;
using Persistent_Connection_Server_and_JavaScript_Client.PersistentConnections;

[assembly: OwinStartupAttribute(typeof(Persistent_Connection_Server_and_JavaScript_Client.Startup))]
namespace Persistent_Connection_Server_and_JavaScript_Client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR<SamplePersistentConnection>("/SamplePC");
            ConfigureAuth(app);
        }
    }
}
