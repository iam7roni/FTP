using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace FtpServer
{
    class ListCommandHandler :  FtpCommand
	{

        //Causes a list of files in the specified path to be transferred to the user. 
        //If the pathname specified is a file then current information on that file will be sent. 
        //If no path is specified the contents of the user's current working or default directory will be used. 
        //This List is in a readable form and provides a list of files and their attributes.

        public ListCommandHandler(FTPCommandHandler ftpCommandHandler)
                   : base("LIST", ftpCommandHandler)
		{
			
		}

        public override string OnProcess(string Dir, Client client)
		{
            try
            {
                string[] asDirectories = Directory.GetDirectories(client.serverControl.CurrentDirectory);
                string mes = "Dir#";
                foreach (string dir in asDirectories)
                    mes += dir + "@";
                DirectoryInfo rootDir = new DirectoryInfo(client.serverControl.CurrentDirectory);
                mes += "#Files#";
                foreach (FileInfo file in rootDir.GetFiles())
                    mes += file.Name + "*" + file.Length + "*" + file.CreationTime.ToShortDateString() + "*" + file.Extension + "@";
                return GetMessage(226, "LIST successful.\r\n^" + mes);
            }
            catch (Exception )
            {
                return GetMessage(227, "LIST unsuccessful.");
            }
        }     
    }
}
