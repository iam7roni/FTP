using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FtpServer
{
    /// <summary>
    /// ChangeWorkingDirectory
    /// </summary> 

    //This command allows the user to work with a different directory for file storage or retrieval 
    //without altering his login or accounting information. 
    //Transfer parameters remain unchanged. A pathname should be specified after this command.

    class CwdCommandHandler : FtpCommand
    {
        public CwdCommandHandler(FTPCommandHandler ftpCommandHandler)
            : base("CWD", ftpCommandHandler)
        {

        }

        public override string OnProcess(string Dir, Client client)
        {
            Dir = Dir.Split('\r')[0];
            Dir = Dir.Replace('/', '\\');

            if (!Dir.Equals("\\"))
                client.serverControl.CurrentDirectory = Path.Combine(client.serverControl.RootDirectory, Dir);

            if (!System.IO.Directory.Exists(client.serverControl.CurrentDirectory))
            {
                return GetMessage(550, "Not a valid directory.");
            }

            return GetMessage(250, string.Format("CWD Successful ({0})", client.serverControl.CurrentDirectory.Replace("\\", "/")));
        }
    }
}
