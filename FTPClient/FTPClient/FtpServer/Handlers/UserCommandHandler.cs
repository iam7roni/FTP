using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtpServer
{
    class UserCommandHandler :  FtpCommand
	{

        //The USER command is required by the server for access to its file system. 
        //This command will normally be the first command sent after the connection is made.
        //This has the effect of purging any user, password, and account information already 
        //supplied at the beginning of the login sequence.
        
        public UserCommandHandler(FTPCommandHandler ftpCommandHandler)
                   : base("USER", ftpCommandHandler)
		{
			
		}

        public override string OnProcess(string User, Client client)
		{
            User = User.Split('\r')[0];
            if (client.serverControl.users.ContainsKey(User))
            {
                client.User = User;
                return GetMessage(331, string.Format("User {0} logged in, needs password", User));
            }
  	        return GetMessage(505, string.Format("User {0} name wrong", User));

		}
    }
}
