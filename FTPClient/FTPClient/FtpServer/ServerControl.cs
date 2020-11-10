using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace FtpServer
{
    class ServerControl
    {
        TcpListener listener;
        int PORT = 2000;
        public ServerForm serverForm;
        Client  clientOb;
        public FTPCommandHandler ftpCommandHandler;
        public Hashtable users;
        public string RootDirectory;
        public string CurrentDirectory;
        public TcpClient socketPasv = null;
        //public TcpClient theSocket = null;


        public ServerControl(ServerForm serverForm)
        {
            // TODO: Complete member initialization
            this.serverForm = serverForm;
            this.CurrentDirectory = "";
            this.RootDirectory = "";
            ftpCommandHandler = new FTPCommandHandler(this);
            users = new Hashtable();
            foreach (string userName in serverForm.listBoxUsers.Items)
                users[userName.ToUpper()] = serverForm.textBoxPassword;
            Thread litenerThread = new Thread(new ThreadStart(ListenerSession));
            litenerThread.Start();
        }

        private void ListenerSession()
        {
            try
            {
                // Set the TcpListener on port 
                listener = new TcpListener(IPAddress.Any, PORT);
                // Start listening for client requests.
                listener.Start();
                this.serverForm.Invoke((MethodInvoker)delegate
                     {
                         this.serverForm.textBox1.SelectedText = "Start FTP Server..." + Environment.NewLine;
                         this.serverForm.textBox1.SelectedText = "Listening...waiting for authentication" + Environment.NewLine;
                    });

                // Enter the listening loop. 
                while (true)
                {
                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = listener.AcceptTcpClient();
                    clientOb = new Client(client, this);
                    this.serverForm.Invoke((MethodInvoker)delegate
                    {
                        Font font = new Font("Tahoma", 8, FontStyle.Regular);
                        this.serverForm.textBox1.SelectionFont = font;
                        this.serverForm.textBox1.SelectionColor = Color.Red;
                        this.serverForm.textBox1.SelectedText = "Client connected..." + Environment.NewLine;
                    });
                }
            }
            catch (SocketException e)
            {
                    MessageBox.Show( "SocketException: " +  e, "Error");
            }
            finally
            {
                // Stop listening for new clients.
                listener.Stop();
            }          
        }

       
    }
}
