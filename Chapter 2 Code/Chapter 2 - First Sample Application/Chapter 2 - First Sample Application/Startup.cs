using Chapter_2___First_Sample_Application.PersistentConnections;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Chapter_2___First_Sample_Application.Startup))]
namespace Chapter_2___First_Sample_Application
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
