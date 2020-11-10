using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtpServer
{
    class GetRootDirCommandHandler :  FtpCommand
	{

        public GetRootDirCommandHandler(FTPCommandHandler ftpCommandHandler)
                   : base("GETROOTDIR", ftpCommandHandler)
		{
			
		}

        public override string OnProcess(string User, Client client)
		{
            return GetMessage(230, string.Format("{0}", client.serverControl.serverForm.textBoxSharedFolder.Text));
		}
    }
}
