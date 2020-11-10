namespace FTP_Server
{
    using System;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using System.WinForms;
    using System.Data;
	using System.Threading;
    /// <summary>
    ///    Summary description for Console.
    /// </summary>
    public delegate void OnMsgArrived(int ClientID,string IP);
		
    public class Console : System.WinForms.Form
    {
        /// <summary>
        ///    Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components;
		private System.WinForms.Button button3;
		private System.WinForms.Button button2;
		private System.WinForms.Button button1;
		private System.WinForms.RichTextBox richTextBox1;
		private static int hwndRichTextBox=0;
		private FTPServer server=new FTPServer();
		private Thread ClientThread=null;
		private static Hashtable clientTable=new Hashtable();
        public Console()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
			hwndRichTextBox=richTextBox1.Handle;
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
			this.button2 = new System.WinForms.Button ();
			this.button3 = new System.WinForms.Button ();
			this.button1 = new System.WinForms.Button ();
			this.richTextBox1 = new System.WinForms.RichTextBox ();
			//@this.TrayHeight = 0;
			//@this.TrayLargeIcon = false;
			//@this.SnapToGrid = false;
			//@this.TrayAutoArrange = true;
			button2.Location = new System.Drawing.Point (267, 232);
			button2.Size = new System.Drawing.Size (72, 24);
			button2.TabIndex = 2;
			button2.Anchor = System.WinForms.AnchorStyles.BottomRight;
			button2.Text = "Stop FTP";
			button2.Click += new System.EventHandler (this.button2_Click);
			button3.Location = new System.Drawing.Point (123, 232);
			button3.Size = new System.Drawing.Size (72, 24);
			button3.TabIndex = 3;
			button3.Anchor = System.WinForms.AnchorStyles.BottomRight;
			button3.Text = "Sessions";
			button3.Click += new System.EventHandler (this.button3_Click);
			button1.Location = new System.Drawing.Point (195, 232);
			button1.Size = new System.Drawing.Size (72, 24);
			button1.TabIndex = 1;
			button1.Anchor = System.WinForms.AnchorStyles.BottomRight;
			button1.Text = "Start FTP";
			button1.Click += new System.EventHandler (this.button1_Click);
			richTextBox1.Size = new System.Drawing.Size (332, 216);
			richTextBox1.TabIndex = 0;
			richTextBox1.Anchor = System.WinForms.AnchorStyles.All;
			richTextBox1.Location = new System.Drawing.Point (8, 8);
			this.Text = "Console";
			this.AutoScaleBaseSize = new System.Drawing.Size (5, 13);
			this.ClientSize = new System.Drawing.Size (344, 261);
			this.Closing += new System.ComponentModel.CancelEventHandler (this.Console_Closing);
			this.Controls.Add (this.button3);
			this.Controls.Add (this.button2);
			this.Controls.Add (this.button1);
			this.Controls.Add (this.richTextBox1);
		}

		
		protected void button3_Click (object sender, System.EventArgs e)
		{
			Session s=new Session();
			s.Show();
		}

		protected void button2_Click (object sender, System.EventArgs e)
		{
			if(server!=null)
			{
				clientTable.Clear();
				CloseFTP closeSever=new CloseFTP(server.EndSession);
				closeSever();
			}
		}
		
		protected void Console_Closing(object sender,System.ComponentModel.CancelEventArgs e)
		{
			//Close the Application.
			e.Cancel=false;
			this.Dispose();
			this.Close();
			try
			{
				if(server!=null)
				{
					server.Close=true;
					Application.DoEvents();
					server.Dispose();
					ClientThread.Abort();
				}	
			}
			catch(Exception ex)
			{

			}
		}
		protected void button1_Click (object sender, System.EventArgs e)
		{
			//Start the FTP Server
			ClientThread=server.Start();
		}
		/// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args) 
        {
            Application.Run(new Console());
        }

		public static void Connected(Thread oThread,string IP)
		{
			clientTable.Add(oThread,IP);
		}
		public static void Disconnected(Thread oThread,string IP)
		{
			if(clientTable.ContainsKey(oThread)	)
			{
				clientTable.Remove(oThread);
			}
		}
		public static void Disconnect(Thread oThread)
		{
			oThread.Interrupt();	
		}

		public static Hashtable Clients()
		{
			return clientTable;
		}

		public static void ClientConnect(int ClientID,string msg)
		{
			try
			{
				Control rtf=Console.FromChildHandle(hwndRichTextBox);
				rtf.Text+= ClientID + msg ;
			}
			catch(Exception e)
			{
				System.Console.WriteLine(e.ToString());
			}
		}
    }
}
