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

namespace NET_Persistent_Connection_Client
{
    public partial class Form1 : Form
    {
        Connection myConnection = new Connection("http://localhost:1290/SamplePC/");
        public Form1()
        {
            InitializeComponent();
            button1.Click += button1_Click;
            myConnection.Received += myConnection_Received;
            myConnection.Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            myConnection.Send(textBox1.Text + ":" + textBox2.Text);
        }
        void myConnection_Received(string obj)
        {
            listBox1.Invoke(new Action(() => listBox1.Items.Add(obj)));
        }
    }
}
