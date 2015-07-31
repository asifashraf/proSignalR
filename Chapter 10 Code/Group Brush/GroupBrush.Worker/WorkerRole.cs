using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using GroupBrush.Web;
using Microsoft.Owin.Hosting;

namespace GroupBrush.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        IDisposable _webApp = null;
        public override void Run()
        {
            while (true)
            {
                Thread.Sleep(10000);
                Trace.TraceInformation("Working");
            }
        }
        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;
            RoleInstanceEndpoint signalREndpoint = null;
            if (RoleEnvironment.CurrentRoleInstance.InstanceEndpoints.TryGetValue("SignalREndpoint", out signalREndpoint))
            {
                _webApp = WebApp.Start<Startup>(string.Format("{0}://{1}", signalREndpoint.Protocol, signalREndpoint.IPEndpoint));
            }
            else
            {
                throw new KeyNotFoundException("Could not find SignalREndpoint");
            }
            return base.OnStart();
        }
        public override void OnStop()
        {
            if (_webApp != null)
            {
                _webApp.Dispose();
            }
            base.OnStop();
        }
    }
}
