using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace FtpServer
{
    class PasvCommandHandler:  FtpCommand
	{
        
        //This command requests the server "listen" on a data port and to wait for a connection 
        //rather than to initiate one upon a transfer command thus making the Transfer Mode Passive. 
        //The response to this command includes the host and port address the server is listening on in most cases these have defaults.
        
        const int m_nPort = 2001;

        public PasvCommandHandler(FTPCommandHandler ftpCommandHandler)
            : base("PASV", ftpCommandHandler)
		{
			
		}

        public override string OnProcess(string param, Client client)
		{
            if (client.serverControl.socketPasv == null)
            {
                TcpListener listener = new TcpListener(IPAddress.Any, m_nPort);
                if (listener == null)
                {
                    return GetMessage(550, string.Format("Couldn't start listener on port {0}", m_nPort));
                }
                SendPasvReply(client);
                listener.Start();
                client.serverControl.socketPasv = listener.AcceptTcpClient();
                listener.Stop();
                return "";
            }
            else
            {
                SendPasvReply(client);
                return "";
            }
		}

        private void SendPasvReply(Client client)
        {
            string sIpAddress = SocketHelpers.GetLocalAddress().ToString();
            sIpAddress = sIpAddress.Replace('.', ',');
            sIpAddress += ',';
            sIpAddress += "0";
            sIpAddress += ',';
            sIpAddress += m_nPort.ToString();
            sIpAddress += ',';
            SocketHelpers.Send(client.client, string.Format("227 ({0})\r\n", sIpAddress));
        }
    }    
}
