using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtpServer
{
    class PwdCommandHandler :  FtpCommand
	{

        //Prints the name of the current working directory on return.
        
        public PwdCommandHandler(FTPCommandHandler ftpCommandHandler)
                   : base("PWD", ftpCommandHandler)
		{
			
		}

        public override string OnProcess(string param, Client client)
		{
            string sDirectory = client.serverControl.CurrentDirectory;
            sDirectory = sDirectory.Replace('\\', '/');
            return GetMessage(257, string.Format("\"{0}\" PWD Successful.", sDirectory));

		}
    }    
}
