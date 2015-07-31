using System;
using System.Drawing;

using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Client;

namespace iOS
{
    [Register("UniversalView")]
    public class UniversalView : UIView
    {
        public UniversalView()
        {
            Initialize();
        }

        public UniversalView(RectangleF bounds)
            : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.Red;
        }
    }

    [Register("MYViewController")]
    public class MYViewController : UIViewController
    {
        HubConnection _hubConnection;
        IHubProxy _auctionProxy;
        List<string> winningBids = new List<string>();
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
            this.InvokeOnMainThread(delegate
            {
                btnCurrentBid.Enabled = enabled;
                btnMakeBid.Enabled = enabled;
            });
        }
        void UpdateBid_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            this.InvokeOnMainThread(delegate
            {
                UpdateBidMethod(obj[0], 0);
                UpdateButtonsMethod(true);
            });
        }
        void CloseBid_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            this.InvokeOnMainThread(delegate
            {
                UpdateButtonsMethod(false);
            });
        }
        void CloseBidWin_auctionProxy(IList<Newtonsoft.Json.Linq.JToken> obj)
        {
            this.InvokeOnMainThread(delegate
            {
                UpdateButtonsMethod(false);
                UpdateBidMethod(obj[0], 1);
            });
        }
        UIButton btnCurrentBid;
        UIButton btnMakeBid;
        UITextField txtBid;
        UILabel lblName;
        UILabel lblDescr;
        UILabel lblBid;
        UILabel lblTime;
        UILabel lblWins;
        public override void ViewDidLoad()
        {
            StartHub();
            base.ViewDidLoad();
            View.Frame = UIScreen.MainScreen.Bounds;
            View.BackgroundColor = UIColor.LightGray;
            View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth |
            UIViewAutoresizing.FlexibleHeight;
            txtBid = new UITextField(new RectangleF(260, 120, 140, 30))
            {
                BackgroundColor
                    = UIColor.White
            };
            lblName = new UILabel(new RectangleF(20, 20, 200, 30));
            lblBid = new UILabel(new RectangleF(260, 20, 200, 30));
            lblDescr = new UILabel(new RectangleF(20, 60, 200, 30));
            lblTime = new UILabel(new RectangleF(260, 60, 200, 30));
            lblWins = new UILabel(new RectangleF(20, 170, 380, 150))
            {
                BackgroundColor =
                    UIColor.White,
                LineBreakMode = UILineBreakMode.WordWrap,
                Lines = 0
            };
            btnCurrentBid = UIButton.FromType(UIButtonType.System);
            btnCurrentBid.Frame = new RectangleF(20, 120, 100, 30);
            btnCurrentBid.SetTitle("Current Bid", UIControlState.Normal);
            btnCurrentBid.BackgroundColor = UIColor.Gray;
            btnMakeBid = UIButton.FromType(UIButtonType.RoundedRect);
            btnMakeBid.SetTitle("Make Bid", UIControlState.Normal);
            btnMakeBid.Frame = new RectangleF(140, 120, 100, 30);
            btnMakeBid.BackgroundColor = UIColor.Gray;
            btnCurrentBid.TouchUpInside += (object sender, EventArgs e) =>
            {
                _auctionProxy.Invoke("MakeCurrentBid");
            };
            btnMakeBid.TouchUpInside += (object sender, EventArgs e) =>
            {
                _auctionProxy.Invoke<string>("MakeBid", txtBid.Text);
            };
            View.AddSubview(btnCurrentBid);
            View.AddSubview(btnMakeBid);
            View.AddSubview(txtBid);
            View.AddSubview(lblName);
            View.AddSubview(lblDescr);
            View.AddSubview(lblBid);
            View.AddSubview(lblTime);
            View.AddSubview(lblWins);
        }
    }
}