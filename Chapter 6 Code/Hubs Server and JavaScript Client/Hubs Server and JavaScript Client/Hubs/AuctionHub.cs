using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Hubs_Server_and_JavaScript_Client.Hubs
{
    public class AuctionHub : Hub
    {
        public AuctionHub()
        {
            BidManager.Start();
        }
        public override Task OnConnected()
        {
            Clients.Caller.CloseBid();
            Clients.All.UpdateBid(BidManager.CurrentBid);
            return base.OnConnected();
        }
        public void MakeCurrentBid()
        {
            BidManager.CurrentBid.BidPrice += 1;
            BidManager.CurrentBid.ConnectionId = this.Context.ConnectionId;
            Clients.All.UpdateBid(BidManager.CurrentBid);
        }
        public void MakeBid(double bid)
        {
            if (bid < BidManager.CurrentBid.BidPrice)
            {
                return;
            }
            BidManager.CurrentBid.BidPrice = bid;
            BidManager.CurrentBid.ConnectionId = this.Context.ConnectionId;
            Clients.All.UpdateBid(BidManager.CurrentBid);
        }
    }
}