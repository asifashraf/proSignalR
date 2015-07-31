using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
namespace Chapter3.Code
{
    [HubName("firstHub")]
    public class Chapter3Hub : Hub
    {
        public void BroadcastMessage(Person person)
        {
            Clients.All.displayText(person.Name, person.Message);
            Clients.Group(Clients.Caller.GroupName).displayText(person.Name, person.Message);
        }

        public Task Join(string groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }

        public Task Leave(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }
    }
}
