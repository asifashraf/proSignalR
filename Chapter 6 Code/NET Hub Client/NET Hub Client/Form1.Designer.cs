namespace NET_Hub_Client
{
    partial class frmAuctionClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCurrentBid = new System.Windows.Forms.Button();
            this.btnMakeBid = new System.Windows.Forms.Button();
            this.txtBid = new System.Windows.Forms.TextBox();
            this.lstWins = new System.Windows.Forms.ListBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDescr = new System.Windows.Forms.Label();
            this.lblBid = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCurrentBid
            // 
            this.btnCurrentBid.Location = new System.Drawing.Point(12, 114);
            this.btnCurrentBid.Name = "btnCurrentBid";
            this.btnCurrentBid.Size = new System.Drawing.Size(75, 23);
            this.btnCurrentBid.TabIndex = 0;
            this.btnCurrentBid.Text = "Current Bid";
            this.btnCurrentBid.UseVisualStyleBackColor = true;
            this.btnCurrentBid.Click += new System.EventHandler(this.btnCurrentBid_Click);
            // 
            // btnMakeBid
            // 
            this.btnMakeBid.Location = new System.Drawing.Point(93, 114);
            this.btnMakeBid.Name = "btnMakeBid";
            this.btnMakeBid.Size = new System.Drawing.Size(75, 23);
            this.btnMakeBid.TabIndex = 1;
            this.btnMakeBid.Text = "Make Bid";
            this.btnMakeBid.UseVisualStyleBackColor = true;
            this.btnMakeBid.Click += new System.EventHandler(this.btnMakeBid_Click);
            // 
            // txtBid
            // 
            this.txtBid.Location = new System.Drawing.Point(174, 116);
            this.txtBid.Name = "txtBid";
            this.txtBid.Size = new System.Drawing.Size(100, 20);
            this.txtBid.TabIndex = 2;
            // 
            // lstWins
            // 
            this.lstWins.FormattingEnabled = true;
            this.lstWins.Location = new System.Drawing.Point(12, 149);
            this.lstWins.Name = "lstWins";
            this.lstWins.Size = new System.Drawing.Size(264, 82);
            this.lstWins.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(20, 22);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "Name";
            // 
            // lblDescr
            // 
            this.lblDescr.AutoSize = true;
            this.lblDescr.Location = new System.Drawing.Point(20, 73);
            this.lblDescr.Name = "lblDescr";
            this.lblDescr.Size = new System.Drawing.Size(60, 13);
            this.lblDescr.TabIndex = 5;
            this.lblDescr.Text = "Description";
            // 
            // lblBid
            // 
            this.lblBid.AutoSize = true;
            this.lblBid.Location = new System.Drawing.Point(211, 22);
            this.lblBid.Name = "lblBid";
            this.lblBid.Size = new System.Drawing.Size(22, 13);
            this.lblBid.TabIndex = 6;
            this.lblBid.Text = "Bid";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(211, 73);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(30, 13);
            this.lblTime.TabIndex = 7;
            this.lblTime.Text = "Time";
            // 
            // frmAuctionClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblBid);
            this.Controls.Add(this.lblDescr);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lstWins);
            this.Controls.Add(this.txtBid);
            this.Controls.Add(this.btnMakeBid);
            this.Controls.Add(this.btnCurrentBid);
            this.Name = "frmAuctionClient";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCurrentBid;
        private System.Windows.Forms.Button btnMakeBid;
        private System.Windows.Forms.TextBox txtBid;
        private System.Windows.Forms.ListBox lstWins;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDescr;
        private System.Windows.Forms.Label lblBid;
        private System.Windows.Forms.Label lblTime;
    }
}

