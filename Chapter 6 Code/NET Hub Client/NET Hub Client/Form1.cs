using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NET_Hub_Client
{
    public partial class frmAuctionClient : Form
    {
        public HubConnection _hubConnection;
        public IHubProxy _auctionProxy;
        delegate void UpdateBid(dynamic bid, int formObject);
        delegate void UpdateButtons(bool enabled);
        UpdateBid _updateDelegate;
        UpdateButtons _updateButtonsDelegate;
        public frmAuctionClient()
        {
            InitializeComponent();
            SetupHub();
        }
        private async void SetupHub()
        {
            _updateDelegate = new UpdateBid(UpdateBidMethod);
            _updateButtonsDelegate = new UpdateButtons(UpdateButtonsMethod);
            _hubConnection = new Microsoft.AspNet.SignalR.Client.HubConnection("http://localhost:1331");
            _auctionProxy = _hubConnection.CreateHubProxy("AuctionHub");
            _auctionProxy.Subscribe("UpdateBid").Received += UpdateBid_auctionProxy;
            _auctionProxy.Subscribe("CloseBid").Received += CloseBid_auctionProxy;
            _auctionProxy.Subscribe("CloseBidWin").Received += CloseBidWin_auctionProxy;
            await _hubConnection.Start();
        }
        void UpdateBidMethod(dynamic bid, int formObject)
        {
            if (bid != null)
            {
                lblName.Text = bid.Name;
                lblDescr.Text = bid.Description;
                lblBid.Text = bid.BidPrice;
                lblTime.Text = bid.TimeLeft;
                if (formObject > 0)
                {
                    lstWins.Items.Add(bid.Name + " at " + bid.BidPrice);
                }
            }
        }
        void UpdateButtonsMethod(bool enabled)
        {
            btnCurrentBid.Enabled = enabled;
            btnMakeBid.Enabled = enabled;
        }

        void UpdateBid_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            this.Invoke(_updateDelegate, obj[0], 0);
            this.Invoke(_updateButtonsDelegate, true);
        }
        void CloseBid_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            this.Invoke(_updateButtonsDelegate, false);
        }
        void CloseBidWin_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            this.Invoke(_updateButtonsDelegate, false);
            this.Invoke(_updateDelegate, obj[0], 1);
        }
        private void btnCurrentBid_Click(object sender, EventArgs e)
        {
            _auctionProxy.Invoke("MakeCurrentBid");
        }
        private void btnMakeBid_Click(object sender, EventArgs e)
        {
            _auctionProxy.Invoke<string>("MakeBid", txtBid.Text);
        }
    }
}
