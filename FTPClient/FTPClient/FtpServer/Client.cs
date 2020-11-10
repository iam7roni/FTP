using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace FtpServer
{
    class Client
    {
        public TcpClient client;
        public NetworkStream stream;
        List<string> files = new List<string>();
        public ServerControl serverControl;
        public string User;

        public Client(TcpClient client, ServerControl serverControl)
        {
            // TODO: Complete member initialization
            this.client = client;
            this.serverControl = serverControl;
            Thread sessionThread = new Thread(new ThreadStart(Session));
            sessionThread.Start();
        }

        private void Session()
        {
                    // Get a stream object for reading and writing
                    stream = client.GetStream();
                    string Mes = "220 FTP Server Ready\r\n";
                    stream.Write(Encoding.ASCII.GetBytes(Mes), 0, Mes.Length); 
                    // Buffer for reading data
                    byte[] bytes = new byte[256];
                    string data = null;
                    bool runFlag = true;
                    // Loop to receive all the data sent by the client. 
                    while (runFlag)
                    {
                        int recLen = stream.Read(bytes, 0, bytes.Length);
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, recLen);
                        if (data.Equals(""))
                            runFlag = false;
                        else
                        {
                            // Process the data sent by the client.
                            data = data.ToUpper();
                            string[] param = data.Split(' ');
                            serverControl.ftpCommandHandler.Execute(param, this);
                        }
                    }
                    // Shutdown and end connection
                    client.Close();
        }
       
    }
}
