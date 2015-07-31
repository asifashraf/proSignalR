using Microsoft.Owin;
using Owin;
using Persistent_Connection_Server.PersistentConnections;

[assembly: OwinStartupAttribute(typeof(Persistent_Connection_Server.Startup))]
namespace Persistent_Connection_Server
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
