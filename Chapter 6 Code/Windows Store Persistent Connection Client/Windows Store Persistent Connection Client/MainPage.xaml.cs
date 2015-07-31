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

namespace Windows_Store_Persistent_Connection_Client
{

    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// An empty page that can be used on its own or navigated to within a Frame.
        /// </summary>
        Connection myConnection = new Connection("http://localhost:1290/SamplePC/");
        public MainPage()
        {
            this.InitializeComponent();
            myConnection.Received += myConnection_Received;
            myConnection.Start().Wait();
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            myConnection.Send(txtName.Text + ": " + txtInput.Text);
        }
        private void myConnection_Received(string data)
        {
            UpdateList(data);
        }
        private async void UpdateList(string data)
        {
            await itemListBox.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            () =>
            {
                var item = new ListBoxItem() { Content = data };
                itemListBox.Items.Add(item);
            });
        }
    }
}