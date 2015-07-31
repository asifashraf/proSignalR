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

namespace SilverLight_Persistent_Connection_Client
{
    public partial class MainPage : UserControl
    {
        Connection myConnection = new Connection("http://localhost:1290/SamplePC/");
        public MainPage()
        {
            InitializeComponent();
            btnSend.Click += btnSend_Click;
            myConnection.Received += myConnection_Received;
            myConnection.Start();
        }
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            myConnection.Send(txtName.Text + ": " + txtMessage.Text);
        }
        void myConnection_Received(string obj)
        {
            Dispatcher.BeginInvoke(() => { lstConvo.Items.Add(obj); });
        }
    }
}
