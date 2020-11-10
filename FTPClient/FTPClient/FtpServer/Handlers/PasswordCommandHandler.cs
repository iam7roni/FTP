using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtpServer
{
    class PasswordCommandHandler:  FtpCommand
	{

        //This command comes immediately after the user command and completes the login and sets the access control privileges.
        
        public PasswordCommandHandler(FTPCommandHandler ftpCommandHandler)
                   : base("PASS", ftpCommandHandler)
		{
			
		}

        public override string OnProcess(string Password, Client client)
		{
            Password = Password.Split('\r')[0];
            if (client.serverControl.users[client.User].ToString().Equals(Password))
                return GetMessage(230, "Password ok, FTP server ready");
            return GetMessage(530, "Username or password incorrect");            
		}

    }
}
