using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FtpServer
{
    public partial class ServerForm : Form
    {
        ServerControl server;

        public ServerForm()
        {
            InitializeComponent();
            server = new ServerControl(this);
            server.RootDirectory = textBoxSharedFolder.Text;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog opDialog = new FolderBrowserDialog();
            if (opDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxSharedFolder.Text = opDialog.SelectedPath;
                server.RootDirectory = opDialog.SelectedPath;
            }
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            bool found = false;
            foreach (string name in listBoxUsers.Items)
            {
                if (name.Equals(textBoxUserName.Text.ToUpper()))
                {
                    found = true; break;
                }
            }
            if (!found)
            {
                listBoxUsers.Items.Add(textBoxUserName.Text);
                server.users[textBoxUserName.Text.ToUpper()] = textBoxPassword.Text;
            }
            else
                MessageBox.Show("User again exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);            
        }

        private void buttonRemoveUser_Click(object sender, EventArgs e)
        {
            bool found = false;
            foreach (string name in listBoxUsers.Items)
            {
                if (name.Equals(textBoxUserName.Text))
                {
                    found = true; break;
                }
            }
            if (found)
            {
                listBoxUsers.Items.Remove(textBoxUserName.Text);
                server.users.Remove(textBoxUserName.Text);
            }
            else
                MessageBox.Show("User isn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
