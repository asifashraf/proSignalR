using System;
using System.Windows.Forms;
public class MonoClient: Form
{
	Button btnSend;
	TextBox txtName;
	TextBox txtMessage;
	ListBox lstMessages;
	Microsoft.AspNet.SignalR.Client.Connection myConnection = new Microsoft.AspNet.SignalR.Client.Connection("http://localhost:1234/SignalR/");

	static public void Main ()
	{
		Application.Run (new MonoClient());
	}
	public MonoClient()
	{
		btnSend = new Button() { Text = "Send", Width = 75, Top = 5, Left = 175 };
		txtName = new TextBox() { Width = 75, Top = 5, Left =5 };
		txtMessage = new TextBox() { Width = 75, Top = 5, Left = 90 };
		lstMessages = new ListBox() { Width = 245, Top = 30, Left = 5 };
		btnSend.Click += btnSend_Click;
		myConnection.Received += myConnection_Received;
		this.Controls.Add(btnSend);
		this.Controls.Add(txtName);
		this.Controls.Add(txtMessage);
		this.Controls.Add(lstMessages);
		StartConnection();
	}
	async void StartConnection()
	{
		await myConnection.Start();
	}
	private void btnSend_Click(object sender, EventArgs e)
	{
		myConnection.Send(txtName.Text + ":" + txtMessage.Text);
	}
	void myConnection_Received(string obj)
	{
		lstMessages.Invoke(new Action(() => lstMessages.Items.Add(obj)));
	}
}