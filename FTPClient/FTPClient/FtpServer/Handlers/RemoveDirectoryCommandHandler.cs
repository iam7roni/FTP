using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FtpServer
{
    class RemoveDirectoryCommandHandler:  FtpCommand
	{

        //This command causes the directory or subdirectory specified to be removed. 
        //Typically only empty directories can be removed. 
        //This command is usually preceded by a DELE (delete) command.
        
        public RemoveDirectoryCommandHandler(FTPCommandHandler ftpCommandHandler)
            : base("RMD", ftpCommandHandler)
		{
			
		}

        public override string OnProcess(string param, Client client)
		{
            param = param.Split('\r')[0];
            string dir = ftpCommandHandler.GetPath(param);

            if (dir[dir.Length - 1] == '\\')
            {
                if (!Directory.Exists(dir))
                {
                    return GetMessage(550, "Directory does not exist.");
                }
                try
                {
                    Directory.Delete(dir);
                }
                catch (System.IO.IOException)
                {
                    return GetMessage(550, "Couldn't delete directory.");
                }
                return GetMessage(250, "Directory deleted successfully");
            }
            return GetMessage(550, "This isn't directory.");
        }
    }
}
