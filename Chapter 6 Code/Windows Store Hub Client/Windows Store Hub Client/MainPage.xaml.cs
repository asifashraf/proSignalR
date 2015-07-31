using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Windows_Store_Hub_Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public HubConnection _hubConnection;
        public IHubProxy _auctionProxy;
        public MainPage()
        {
            InitializeComponent();
            btnCurrentBid.Click += btnCurrentBid_Click;
            btnMakeBid.Click += btnMakeBid_Click;
            SetupHub();
        }
        private async void SetupHub()
        {
            _hubConnection = new Microsoft.AspNet.SignalR.Client.HubConnection("http://localhost:1331/");
            _auctionProxy = _hubConnection.CreateHubProxy("AuctionHub");
            _auctionProxy.Subscribe("UpdateBid").Received += UpdateBid_auctionProxy;
            _auctionProxy.Subscribe("CloseBid").Received += CloseBid_auctionProxy;
            _auctionProxy.Subscribe("CloseBidWin").Received += CloseBidWin_auctionProxy;
            await _hubConnection.Start();
        }
        void UpdateBid(dynamic bid, int formObject)
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
        void UpdateButtons(bool enabled)
        {
            btnCurrentBid.IsEnabled = enabled;
            btnMakeBid.IsEnabled = enabled;
        }
        async void UpdateBid_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            () =>
            {
                UpdateBid(obj[0], 0);
                UpdateButtons(true);
            });
        }
        async void CloseBid_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            () =>
            {
                UpdateButtons(false);
            });
        }
        async void CloseBidWin_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            () =>
            {
                UpdateButtons(false);
                UpdateBid(obj[0], 1);
            });
        }
        private void btnCurrentBid_Click(object sender, RoutedEventArgs e)
        {
            _auctionProxy.Invoke("MakeCurrentBid");
        }
        private void btnMakeBid_Click(object sender, RoutedEventArgs e)
        {
            _auctionProxy.Invoke<string>("MakeBid", this.txtBid.Text);
        }
    }
}