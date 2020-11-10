using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace FtpServer
{
    class FTPCommandHandler
    {
        private Hashtable m_theCommandHashTable = null;
        public ServerControl serverControl;

        #region Construction       
        public FTPCommandHandler(ServerControl serverControl)
        {
            // TODO: Complete member initialization
            this.serverControl = serverControl;
            m_theCommandHashTable = new Hashtable();           
			LoadCommands();
        }

		#endregion

        #region Methods

        private void LoadCommands()
        {
            AddCommand(new UserCommandHandler(this));
            AddCommand(new PasswordCommandHandler(this));
            AddCommand(new GetRootDirCommandHandler(this));
            AddCommand(new QuitCommandHandler(this));
            AddCommand(new CwdCommandHandler(this));
            AddCommand(new PasvCommandHandler(this));
            AddCommand(new ListCommandHandler(this));
            AddCommand(new PwdCommandHandler(this));
            AddCommand(new RetrCommandHandler(this));
            AddCommand(new DeleCommandHandler(this));
            AddCommand(new StoreCommandHandler(this));
            AddCommand(new RemoveDirectoryCommandHandler(this));
        }       

        private void AddCommand(FtpCommand handler)
        {
            m_theCommandHashTable.Add(handler.Command, handler);
        }

        public void Execute(string[] param, Client client)
        {
            FtpCommand command = m_theCommandHashTable[param[0]] as FtpCommand;
            if (command == null)
            {
                SocketHelpers.Send(client.client, "550 Unknown command\r\n");
                serverControl.serverForm.Invoke((MethodInvoker)delegate
                {
                    Font font = new Font("Tahoma", 8, FontStyle.Regular);
                    serverControl.serverForm.textBox1.SelectionFont = font;
                    serverControl.serverForm.textBox1.SelectionColor = Color.Green;
                    serverControl.serverForm.textBox1.SelectedText = "550 Unknown command" + Environment.NewLine;
                });
            }
            else
            {
                serverControl.serverForm.Invoke((MethodInvoker)delegate
                {
                    Font font = new Font("Tahoma", 8, FontStyle.Regular);
                    serverControl.serverForm.textBox1.SelectionFont = font;
                    serverControl.serverForm.textBox1.SelectionColor = Color.Green;
                    serverControl.serverForm.textBox1.SelectedText = "FtpClent command " + param[0] + Environment.NewLine;
                });
                string arg = param.Length < 2 ? "" : param[1];
                for (int i = 2; i < param.Length; i++)
                {
                    arg += " " + param[i];
                }
                string Mes = command.OnProcess(arg, client);
                string[] items = Mes.Split('^');
                SocketHelpers.Send(client.client, items[0]);
                if (items.Length > 1)
                    SocketHelpers.Send(client.client, items[1]);
            }
        }

        public string GetPath(string sPath)
        {
            if (sPath.Length == 0)
            {
                return serverControl.RootDirectory;
            }

            sPath = sPath.Replace('/', '\\');

            if ( sPath[sPath.Length -1] == '\\' )
               return Path.Combine(serverControl.RootDirectory, sPath);
            return Path.Combine(serverControl.CurrentDirectory, sPath);
        }

        #endregion
    }
}
