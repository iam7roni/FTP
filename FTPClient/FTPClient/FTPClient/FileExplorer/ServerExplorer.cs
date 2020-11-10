using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Microsoft.VisualBasic.FileIO;


namespace FTPClient
{
    /// <summary>
    /// This class use to create the tree view and load 
    ///       directories and files in to the tree
    /// </summary>
    public class ServerExplorer
    {
        public ServerExplorer()
        {

        }
       
        /// <summary>
        /// This is use to creat and build the tree
        /// </summary>
        /// <param name="treeView"></param>
        /// <returns></returns>
        public bool CreateTree(TreeView treeView)
        {
            bool returnValue = false;             
            try
            {
                // Create Desktop
                TreeNode rootDir = new TreeNode();
                rootDir.Text = "/";
                rootDir.Tag = "Root";
                rootDir.Nodes.Add("");
                treeView.Nodes.Add(rootDir);
                returnValue = true;
            }
            catch (Exception )
            {
                returnValue = false;
            }
            return returnValue;
            
        }
       
        /// <summary>
        /// This is use to Enumerate directories 
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public TreeNode EnumerateDirectory(TreeNode parentNode, string dir)
        {          
            try
            {
                
                string[] items = dir.Split('@');
                if (parentNode.Nodes.Count == 1 && parentNode.Nodes[0].Text.Equals(""))
                    parentNode.Nodes[0].Remove();
                else
                    parentNode.Nodes.Clear();
                foreach (string name in items)
                {
                    if (!name.Equals(""))
                    {
                        TreeNode node = new TreeNode();
                        string[] parts = name.Split('\\');
                        node.Text = parts[parts.Length - 1];
                        node.Nodes.Add("");
                        parentNode.Nodes.Add(node);
                    }
                }
            }
            catch (Exception )  {   /*TODO :*/        }           
            return parentNode;
        }
        /// <summary>
        /// This is use to Enumerate files
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="listView2"></param>
        /// <param name="imageList1"></param>
        /// <param name="files"></param>
        public void EnumerateFiles(TreeNode treeNode, ListView listView2, ImageList imageList1, string files)
        {
            listView2.Items.Clear();
            string[] items = files.Split('@');
            //Fill files
            foreach (string fileDetails in items)
            {
                if (fileDetails.Equals("") || fileDetails.Equals("\r\n"))
                    continue;
                string[] parts = fileDetails.Split('*');
                string[] fields = { parts[0], parts[1], parts[2] };
                // Set a default icon for the file.
                Icon iconForFile = SystemIcons.WinLogo;
                ListViewItem item = new ListViewItem(fields, 1);
                listView2.Items.Add(item);
            }
        }
    }
}
