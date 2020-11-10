using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FtpServer
{
    class DeleCommandHandler:  FtpCommand
	{

        //This command causes the file specified to be deleted on the server. 
        //An extra level of protection (such as the query, "Do you really wish to delete?") 
        //can be provided by many of the existing FTP clients.

        public DeleCommandHandler(FTPCommandHandler ftpCommandHandler)
            : base("DELE", ftpCommandHandler)
		{
			
		}

        public override string OnProcess(string param, Client client)
		{
            param = param.Split('\r')[0];
            string dir = ftpCommandHandler.GetPath(param);

            if (dir[dir.Length - 1] != '\\')
            {
                if (!File.Exists(dir))
                {
                    return GetMessage(550, "File does not exist.");
                }
                try
                {
                    File.Delete(dir);
                }
                catch (Exception)
                {
                    return GetMessage(550, "Couldn't delete file.");
                }
                return GetMessage(250, "File deleted successfully");
            }
            return GetMessage(550, "This isn't file.");
        }
    }
}
