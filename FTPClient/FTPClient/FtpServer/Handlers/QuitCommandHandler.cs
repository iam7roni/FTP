using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtpServer
{
    class QuitCommandHandler:  FtpCommand
	{

        //This command terminates the connection
        
        public QuitCommandHandler(FTPCommandHandler ftpCommandHandler)
            : base("QUIT", ftpCommandHandler)
		{
			
		}

        public override string OnProcess(string param, Client client)
		{
            return GetMessage(220, "Goodbye");
        }
    }
}
