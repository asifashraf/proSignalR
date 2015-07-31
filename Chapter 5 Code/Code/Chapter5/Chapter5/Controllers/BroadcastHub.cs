using Microsoft.AspNet.SignalR;

namespace Chapter5.Controllers
{
    public class BroadcastHub : Hub
    {
        public void BroadcastMessage(string message)
        {
            Clients.All.displayText(message);
        }
    }
}