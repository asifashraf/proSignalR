using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverLight_Hub_Client
{
    public partial class MainPage : UserControl
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
            _hubConnection = new Microsoft.AspNet.SignalR.Client.HubConnection("http://localhost:1331/");
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
                lblName.Text = (string)bid["Name"];
                lblDescr.Text = (string)bid["Description"];
                lblBid.Text = (string)bid["BidPrice"];
                lblTime.Text = (string)bid["TimeLeft"];
                if (formObject > 0)
                {
                    lstWins.Items.Add((string)bid["Name"] + " at " + (string)bid["BidPrice"]);
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

