using Owin;

namespace Chapter5
{
    public class Startup
    {
        // The name *MUST* be Configuration
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
