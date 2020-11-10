using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace FtpServer
{
    class RetrCommandHandler:  FtpCommand
	{
        //This command requests that the server transfers a copy of the file specified. 
        //This transfer is to be done an the data port provided by the connection. 
        //The status and contents of the file at the server remains unchanged.


        public RetrCommandHandler(FTPCommandHandler ftpCommandHandler)
            : base("RETR", ftpCommandHandler)
		{
		}

        public override string OnProcess(string param, Client client)
		{
            param = param.Split('\r')[0];
            string sFilePath = ftpCommandHandler.GetPath(param);
            if (sFilePath[sFilePath.Length - 1] != '\\')
            {
                if (!File.Exists(sFilePath))
                {
                    return GetMessage(550, "File does not exist.");
                }
                FtpReplySocket replySocket = new FtpReplySocket(client.serverControl);
                if (!replySocket.Loaded)
                {
                    return GetMessage(550, "Unable to establish data connection");
                }
               
                FileHelpers.OpenFile(sFilePath, false);
                if (FileHelpers.m_theFile == null)
                {
                    return GetMessage(550, "Couldn't open file");
                }
                client.serverControl.serverForm.Invoke((MethodInvoker)delegate
                {
                    Font font = new Font("Tahoma", 8, FontStyle.Regular);
                    client.serverControl.serverForm.textBox1.SelectionFont = font;
                    client.serverControl.serverForm.textBox1.SelectionColor = Color.Green;
                    client.serverControl.serverForm.textBox1.SelectedText = "Starting data downloading transfer, please wait..." + Environment.NewLine;
                });
                SocketHelpers.Send(client.client, "150 Starting data transfer, please wait...\r\n");
                const int m_nBufferSize = 65536;
                byte[] abBuffer = new byte[m_nBufferSize];
                int nRead = FileHelpers.Read(abBuffer, m_nBufferSize);
                while (nRead > 0 && replySocket.Send(abBuffer, nRead))
                {
                    nRead = FileHelpers.Read(abBuffer, m_nBufferSize);
                }
                FileHelpers.Close();
                replySocket.Close();
                return GetMessage(226, "File download succeeded.");  
            }
            return GetMessage(550, "This isn't file.");          
        }
    }    
}
