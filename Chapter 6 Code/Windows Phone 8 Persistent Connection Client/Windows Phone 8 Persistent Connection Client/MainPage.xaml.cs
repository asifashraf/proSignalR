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


namespace Windows_Phone_8_Persistent_Connection_Client
{
    public partial class MainPage : PhoneApplicationPage
    {
        Microsoft.AspNet.SignalR.Client.Connection myConnection = new Connection("http://192.168.1.148:1331/SamplePC/");
        public MainPage()
        {
            this.InitializeComponent();
            myConnection.Received += myConnection_Received;
            myConnection.Start();
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            myConnection.Send(txtName.Text + ": " + txtInput.Text);
        }
        private void myConnection_Received(string data)
        {
            UpdateList(data);
        }
        private void UpdateList(string data)
        {
            itemListBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                var item = new ListBoxItem() { Content = data };
                itemListBox.Items.Add(item);
            }));
        }
    }
}
