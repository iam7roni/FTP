using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace FtpServer
{
    class FtpReplySocket
    {
        private TcpClient m_theSocket;
        const int m_nPort = 2001;

        public bool Loaded
        {
            get { return m_theSocket != null; }
        }

        public FtpReplySocket(ServerControl serverControl)
        {
            m_theSocket = OpenSocket(serverControl);
        }

        public bool Send(byte[] abData, int nSize)
        {
            return SocketHelpers.Send(m_theSocket, abData, 0, nSize);
        }

        public bool Send(char[] abChars, int nSize)
        {
            return SocketHelpers.Send(m_theSocket, Encoding.ASCII.GetBytes(abChars), 0, nSize);
        }

        public bool Send(string sMessage)
        {
            byte[] abData = Encoding.ASCII.GetBytes(sMessage);
            return Send(abData, abData.Length);
        }

        public int Receive(byte[] abData)
        {
            return m_theSocket.GetStream().Read(abData, 0, abData.Length);
        }

        static private TcpClient OpenSocket(ServerControl serverControl)
        {
            TcpClient socketPasv = serverControl.socketPasv;
            if (socketPasv != null)
            {
                serverControl.socketPasv = null;
                return socketPasv;
            }
            return SocketHelpers.CreateTcpClient("", m_nPort);
        }

        public void Close()
        {
            SocketHelpers.Close(m_theSocket);
            m_theSocket = null;
        }
    }
}
