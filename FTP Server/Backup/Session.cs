namespace FTP_Server
{
    using System;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using System.WinForms;
	using System.Threading;
    /// <summary>
    ///    Summary description for Session.
    /// </summary>
    public class Session : System.WinForms.Form
    {
        /// <summary>
        ///    Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components;
		private System.WinForms.Button button1;
		private System.WinForms.ListBox listBox1;
		private Hashtable clientTable=new Hashtable();
        public Session()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
			clientTable=Console.Clients();
			foreach(Thread oThread in clientTable.Keys)
			{
				listBox1.Items.Add(oThread);
			}
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        ///    Clean up any resources being used.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            components.Dispose();
        }

        /// <summary>
        ///    Required method for Designer support - do not modify
        ///    the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container ();
			this.button1 = new System.WinForms.Button ();
			this.listBox1 = new System.WinForms.ListBox ();
			//@this.TrayHeight = 0;
			//@this.TrayLargeIcon = false;
			//@this.TrayAutoArrange = true;
			button1.Location = new System.Drawing.Point (192, 168);
			button1.Size = new System.Drawing.Size (88, 24);
			button1.TabIndex = 1;
			button1.Text = "Disconnect";
			button1.Click += new System.EventHandler (this.button1_Click);
			listBox1.Location = new System.Drawing.Point (8, 24);
			listBox1.Size = new System.Drawing.Size (272, 134);
			listBox1.TabIndex = 0;
			this.Text = "Session";
			this.MaximizeBox = false;
			this.AutoScaleBaseSize = new System.Drawing.Size (5, 13);
			this.BorderStyle = System.WinForms.FormBorderStyle.FixedDialog;
			this.MinimizeBox = false;
			this.ClientSize = new System.Drawing.Size (290, 199);
			this.Controls.Add (this.button1);
			this.Controls.Add (this.listBox1);
		}

		protected void button1_Click (object sender, System.EventArgs e)
		{
			if(listBox1.Items.Count>0)
			{
				Thread oThread=(Thread)listBox1.SelectedItem;
				Console.Disconnect(oThread);
				listBox1.Items.Remove(listBox1.SelectedItem);
			}
		}
	}
}
