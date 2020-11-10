using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace FtpServer
{
    public sealed class SocketHelpers
    {
        static public IPAddress GetLocalAddress()
        {
            IPHostEntry hostEntry = Dns.Resolve(Dns.GetHostName());

            if (hostEntry == null || hostEntry.AddressList.Length == 0)
            {
                return null;
            }

            return hostEntry.AddressList[0];
        }
        static public void Close(TcpClient socket)
        {
            if (socket == null) return;
            try
            {
                if (socket.GetStream() != null)
                {
                    try
                    {
                        socket.GetStream().Flush();
                    }
                    catch (SocketException) { }
                    try
                    {
                        socket.GetStream().Close();
                    }
                    catch (SocketException) { }
                }
            }
            catch (SocketException)
            {
            }
            catch (InvalidOperationException) { }
            try
            {
                socket.Close();
            }
            catch (SocketException) { }
        }
      
        #region SEND
        static public bool Send(TcpClient socket, byte[] abMessage)
        {
            return Send(socket, abMessage, 0, abMessage.Length);
        }
        static public bool Send(TcpClient socket, byte[] abMessage, int nStart)
        {
            return Send(socket, abMessage, nStart, abMessage.Length - nStart);
        }
        static public bool Send(TcpClient socket, byte[] abMessage, int nStart, int nLength)
        {
            bool fReturn = true;
            try
            {
                BinaryWriter writer = new BinaryWriter(socket.GetStream());
                writer.Write(abMessage, nStart, nLength);
                writer.Flush();
            }
            catch (IOException)
            {
                fReturn = false;
            }
            catch (SocketException)
            {
                fReturn = false;
            }
            return fReturn;
        }
        static public bool Send(TcpClient socket, string sMessage)
        {
            byte[] abMessage = System.Text.Encoding.ASCII.GetBytes(sMessage);
            return Send(socket, abMessage);
        }
        #endregion

        static public TcpClient CreateTcpClient(string sAddress, int nPort)
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient(sAddress, nPort);
            }
            catch (SocketException)
            {
                client = null;
            }
            return client;
        }
    }
}
