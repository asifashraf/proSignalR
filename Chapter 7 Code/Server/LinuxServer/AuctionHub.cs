using System;

namespace LinuxServer
{

	public class AuctionHub : Microsoft.AspNet.SignalR.Hub
	{
		public AuctionHub()
		{
			BidManager.Start();
		}
		public override System.Threading.Tasks.Task OnConnected()
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

