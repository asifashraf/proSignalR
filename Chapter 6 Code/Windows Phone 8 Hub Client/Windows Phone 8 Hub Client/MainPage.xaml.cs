using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.AspNet.SignalR.Client;


namespace Windows_Phone_8_Hub_Client
{
    public partial class MainPage : PhoneApplicationPage
    {
        public HubConnection _hubConnection;
        public IHubProxy _auctionProxy;
        delegate void UpdateBid(dynamic bid, int formObject);
        delegate void UpdateButtons(bool enabled);
        UpdateBid _updateDelegate;
        UpdateButtons _updateButtonsDelegate;
        public MainPage()
        {
            InitializeComponent();
            btnCurrentBid.Click += btnCurrentBid_Click;
            btnMakeBid.Click += btnMakeBid_Click;
            SetupHub();
        }
        private async void SetupHub()
        {
            _updateDelegate = new UpdateBid(UpdateBidMethod);
            _updateButtonsDelegate = new UpdateButtons(UpdateButtonsMethod);
            _hubConnection = new HubConnection("http://192.168.1.148:1331/");
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
            btnCurrentBid.IsEnabled = enabled;
            btnMakeBid.IsEnabled = enabled;
        }
        void UpdateBid_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            Dispatcher.BeginInvoke(_updateDelegate, obj[0], 0);
            Dispatcher.BeginInvoke(_updateButtonsDelegate, true);
        }
        void CloseBid_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            Dispatcher.BeginInvoke(_updateButtonsDelegate, false);
        }
        void CloseBidWin_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            Dispatcher.BeginInvoke(_updateButtonsDelegate, false);
            Dispatcher.BeginInvoke(_updateDelegate, obj[0], 1);
        }
        private void btnCurrentBid_Click(object sender, EventArgs e)
        {
            _auctionProxy.Invoke("MakeCurrentBid");
        }
        private void btnMakeBid_Click(object sender, EventArgs e)
        {
            _auctionProxy.Invoke<string>("MakeBid", this.txtBid.Text);
        }

    }
}
