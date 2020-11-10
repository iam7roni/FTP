using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace FtpServer
{
    class StoreCommandHandler:  FtpCommand
	{

        //This command causes the server to accept data transferred via the data 
        //connection and to store that data as a file at the server site. 
        //If the file specified exists already on the server in the requested path, then the file shall be overwritten. 
        //A new file is created on the server if the file specified does not already exist.
        
        public StoreCommandHandler(FTPCommandHandler ftpCommandHandler)
            : base("STOR", ftpCommandHandler)
		{
		}

        public override string OnProcess(string param, Client client)
		{
            param = param.Split('\r')[0];
            string sFilePath = ftpCommandHandler.GetPath(param);
            if (sFilePath[sFilePath.Length - 1] != '\\')
            {
                if ( File.Exists(sFilePath))
                {
                    return GetMessage(553, "File already exists.");
                }
                FtpReplySocket replySocket = new FtpReplySocket(client.serverControl);
                if (!replySocket.Loaded)
                {
                    return GetMessage(425, "Error in establishing data connection.");
                }
                FileHelpers.OpenFile(sFilePath, true);
                if (FileHelpers.m_theFile == null)
                {
                    return GetMessage(550, "Couldn't open file");
                }
                client.serverControl.serverForm.Invoke((MethodInvoker)delegate
                {
                    Font font = new Font("Tahoma", 8, FontStyle.Regular);
                    client.serverControl.serverForm.textBox1.SelectionFont = font;
                    client.serverControl.serverForm.textBox1.SelectionColor = Color.Green;
                    client.serverControl.serverForm.textBox1.SelectedText = "Starting data uploading transfer, please wait..." + Environment.NewLine;
                });
                SocketHelpers.Send(client.client, "150 Opening connection for data transfer.\r\n");
                const int m_nBufferSize = 65536;
                byte[] abData = new byte[m_nBufferSize];
                int nReceived = replySocket.Receive(abData);
                while (nReceived > 0)
                {
                    FileHelpers.Write(abData, nReceived);
                    nReceived = replySocket.Receive(abData);
                }
                FileHelpers.Close();
                replySocket.Close();
                return GetMessage(226, "File uploaded succeeded.");  
            }
            return GetMessage(550, "This isn't file.");
        }
    }    
}
