using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtpServer
{
    abstract class FtpCommand
    {
        #region Member Variables

        public string  Command;
        public FTPCommandHandler ftpCommandHandler = null;

        #endregion

        #region Construction

        protected FtpCommand(string sCommand, FTPCommandHandler ftpCommandHandler)
		{
			this.Command = sCommand;
            this.ftpCommandHandler = ftpCommandHandler;
		}

		#endregion


        public abstract string OnProcess(string sMessage,Client client);

        protected string GetMessage(int nReturnCode, string sMessage)
        {
            return string.Format("{0} {1}\r\n", nReturnCode, sMessage);
        }
       
    }
}
