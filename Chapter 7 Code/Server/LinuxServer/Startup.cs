using System;
using Owin;
using Microsoft.Owin;

namespace LinuxServer
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.MapSignalR();
		}
	}
}

