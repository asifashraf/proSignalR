using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Client;

namespace Android
{
    [Activity(Label = "Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        Button btnCurrentBid;
        Button btnMakeBid;
        EditText txtBid;
        TextView lblName;
        TextView lblDescr;
        TextView lblBid;
        TextView lblTime;
        TextView lblWins;
        HubConnection _hubConnection;
        IHubProxy _auctionProxy;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            btnCurrentBid = FindViewById<Button>(Resource.Id.btnCurrentBid);
            btnCurrentBid.Click += delegate { _auctionProxy.Invoke("MakeCurrentBid"); };
            btnMakeBid = FindViewById<Button>(Resource.Id.btnMakeBid);
            btnMakeBid.Click += delegate { _auctionProxy.Invoke<string>("MakeBid", txtBid.Text); };
            txtBid = FindViewById<EditText>(Resource.Id.txtBid);
            lblName = FindViewById<TextView>(Resource.Id.lblName);
            lblDescr = FindViewById<TextView>(Resource.Id.lblDescr);
            lblBid = FindViewById<TextView>(Resource.Id.lblBid);
            lblTime = FindViewById<TextView>(Resource.Id.lblTime);
            lblWins = FindViewById<TextView>(Resource.Id.lblWins);
            StartHub();
        }
        async void StartHub()
        {
            _hubConnection = new HubConnection("http://192.168.1.148:1331/signalr");
            _auctionProxy = _hubConnection.CreateHubProxy("AuctionHub");
            _auctionProxy.Subscribe("UpdateBid").Received += UpdateBid_auctionProxy;
            _auctionProxy.Subscribe("CloseBid").Received += CloseBid_auctionProxy;
            _auctionProxy.Subscribe("CloseBidWin").Received += CloseBidWin_auctionProxy;
            await _hubConnection.Start();
        }
        void UpdateBidMethod(Newtonsoft.Json.Linq.JToken bid, int formObject)
        {
            if (bid != null && bid.HasValues)
            {
                lblName.Text = (string)bid["Name"];
                lblDescr.Text = (string)bid["Description"];
                lblBid.Text = (string)bid["BidPrice"];
                lblTime.Text = "Time Left: " + (string)bid["TimeLeft"];
                if (formObject > 0)
                {
                    string win = bid["Name"] + " at " + bid["BidPrice"] + "\r\n";
                    lblWins.Text += win;
                }
            }
        }
        void UpdateButtonsMethod(bool enabled)
        {
            this.RunOnUiThread(delegate
            {
                btnCurrentBid.Enabled = enabled;
                btnMakeBid.Enabled = enabled;
            });
        }
        void UpdateBid_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            this.RunOnUiThread(delegate
            {
                UpdateBidMethod(obj[0], 0);
                UpdateButtonsMethod(true);
            });
        }
        void CloseBid_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            this.RunOnUiThread(delegate
            {
                UpdateButtonsMethod(false);
            });
        }
        void CloseBidWin_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            this.RunOnUiThread(delegate
            {
                UpdateButtonsMethod(false);
                UpdateBidMethod(obj[0], 1);
            });
        }
    }
}