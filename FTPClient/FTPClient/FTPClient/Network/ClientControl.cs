using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace FTPClient
{
    class ClientControl
    {
        public ClientForm clientForm;
        private TcpClient client;
        NetworkStream stream;

        public ClientControl(ClientForm clientForm)
        {
            // TODO: Complete member initialization
            this.clientForm = clientForm;
            Thread serverThread = new Thread(new ThreadStart(ServerSession));
            serverThread.Start();
        }

        private void ServerSession()
        {
            try
            {
                // Set the TcpListener on port 
                client = new TcpClient();
                client.Connect(this.clientForm.textBoxHost.Text,  int.Parse(this.clientForm.textBoxPort.Text));
                stream = client.GetStream();
                this.clientForm.Invoke((MethodInvoker)delegate
                {
                    this.clientForm.TextBoxAppendtext("Connected to FTP Server...\r\n");
               //     this.clientForm.textBox1.Text += "\r\n";
                });
                // Buffer for reading data
                byte[] bytes = new byte[256];
                string data = null;
                bool runFlag = true;
                // Enter the listening loop. 
                while (runFlag)
                {
                    int recLen = stream.Read(bytes, 0, bytes.Length);
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, recLen);
                    // Process the data sent by the client.
                    data = data.ToUpper();
                    string[] param = data.Split(' ');
                    switch (param[0])  // CPMMAND
                    {
                        
                        case "EXIT":
                            runFlag = false;
                            break;
                    }
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show("SocketException: " + e, "Error");
            }
            finally
            {
                // Stop listening for new clients.
                client.Close();
            }
        }

        public void Send(string message)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);
        }
    }
}
