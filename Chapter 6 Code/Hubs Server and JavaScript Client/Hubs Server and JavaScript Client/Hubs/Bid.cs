using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hubs_Server_and_JavaScript_Client.Hubs
{
    public class Bid
    {
        public Bid Clone()
        {
            return (Bid)MemberwiseClone();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public double BidPrice { get; set; }
        public int TimeLeft { get; set; }
        public string ConnectionId { get; set; }
    }
}