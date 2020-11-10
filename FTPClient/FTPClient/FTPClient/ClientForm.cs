using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace FTPClient
{
    public partial class ClientForm : Form
    {
        FileExplorer fileExplorer = new FileExplorer();
        public ServerExplorer serverExplorer = new ServerExplorer();
        FTP ftp;
        string serverSideSelectedDir;   // server side 
        string mySelectedDir;           // client side 

        public ClientForm()
        {
            InitializeComponent();
            buttonLogin.Text = "Log In";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Create file tree            
            fileExplorer.CreateTree(this.treeViewMy);
        }

        #region  My Side  (Client)
        private void treeViewMy_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text == "")
            {
                TreeNode node = fileExplorer.EnumerateDirectory(e.Node);
                mySelectedDir = fileExplorer.GetPath(e.Node);
            }    
        }
      
        private void treeViewMy_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            fileExplorer.EnumerateFiles(e.Node, listView1, imageList1);
            mySelectedDir = fileExplorer.GetPath(e.Node);
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            uploadToolStripMenuItem.Enabled = ftp != null && ftp.IsLoggedIn;
        }

        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ftp.UploadFile(mySelectedDir + listView1.SelectedItems[0].Text);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
             DialogResult res= MessageBox.Show(this, "Are you sure you want to delete this file ? ", "Delete file", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
             if (res == DialogResult.Yes)
             {
                 if (listView1.SelectedItems.Count > 0)
                     DeleteFile(mySelectedDir + listView1.SelectedItems[0].Text);
                 else
                     RemoveDirectory(mySelectedDir);
             }
        }

        private void RemoveDirectory(string dir)
        {
 	            try
                {
                    Directory.Delete(dir);
                }
                catch (Exception e)
                {
                    this.textBox1.Text += "Couldn't delete directory on my computer.\r\n";
                }
        }

        private void DeleteFile(string dir)
        {
                try
                {
                    File.Delete(dir);
                }
                catch (Exception)
                {
                    this.textBox1.Text +="Couldn't delete file on my computer.\r\n";
                }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion     

        #region  Server Side
        private void contextMenuStrip2_Opened(object sender, EventArgs e)
        {
            downloadToolStripMenuItem.Enabled = ftp != null && ftp.IsLoggedIn;
            deleteServerToolStripMenuItem.Enabled = ftp != null && ftp.IsLoggedIn;
        }

        private void treeView2_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (ftp != null && ftp.IsLoggedIn )
            {
                string dir = GetPath(e.Node);
                ftp.ChangeWorkingDirectory(dir, e.Node);
            }
        }      

        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (ftp != null && ftp.IsLoggedIn)
            {
                string dir = GetPath(e.Node);
                ftp.ChangeWorkingDirectory(dir, e.Node);
                serverSideSelectedDir = dir;
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ftp.DownloadFile(listView2.SelectedItems[0].Text, "");
        }

        private void deleteServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
             DialogResult res= MessageBox.Show(this, "Are you sure you want to delete this file ? ", "Delete file", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
             if (res == DialogResult.Yes)
             {
                 if (listView2.SelectedItems.Count > 0)
                     ftp.DeleteFile(listView2.SelectedItems[0].Text);
                 else
                     ftp.RemoveDirectory(serverSideSelectedDir);
             }
        }      
        
        #endregion

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (buttonLogin.Text.Equals("Log In"))
            {
                ftp = new FTP(this);
                ftp.FtpLogin();
            }
            else
            {
                buttonLogin.Text = "Log In";
                password.Enabled = true;
                userName.Enabled = true;
                textBoxPort.Enabled = true;
                textBoxHost.Enabled = true;
                ftp.CloseConnection();              
            }           
        }

        public void Login()
        {
            buttonLogin.Text = "Log Out";
            password.Enabled = false;
            userName.Enabled = false;
            textBoxPort.Enabled = false;
            textBoxHost.Enabled = false;
        }

        public void TextBoxAppendtext(string msg)
        {
            this.textBox1.AppendText(msg);
            this.textBox1.SelectionStart = this.textBox1.Text.Length;
            this.textBox1.ScrollToCaret();
        }

        private string GetPath(TreeNode treeNode)
        {
            string dir = "";
            while (treeNode.Parent != null)
            {
                dir = treeNode.Text + "\\" + dir;
                treeNode = treeNode.Parent;
            }
            return dir;
        }       
    }
}
