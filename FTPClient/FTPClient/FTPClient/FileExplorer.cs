using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using Microsoft.VisualBasic.FileIO;


namespace FTPClient
{
    /* Class  :FileExplorer
     * Author : 
     * Date   : 
     * Discription : This class use to create the tree view and load 
     *               directories and files in to the tree
     *          
     */
    class FileExplorer
    {
        public FileExplorer()
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
                TreeNode desktop = new TreeNode();
                desktop.Text = "Desktop";
                desktop.Tag = "Desktop";
                desktop.Nodes.Add("");
                treeView.Nodes.Add(desktop);
                TreeNode myDocuments = new TreeNode();
                myDocuments.Text = "My Documents";
                myDocuments.Nodes.Add("");
                desktop.Nodes.Add(myDocuments);
                TreeNode myComputer = new TreeNode();
                myComputer.Text = "My Computer";
                desktop.Nodes.Add(myComputer);

                // Get driveInfo
                foreach (DriveInfo drv in DriveInfo.GetDrives())
                {
                    TreeNode fChild = new TreeNode();
                    if (drv.DriveType == DriveType.CDRom)
                    {
                        fChild.ImageIndex = 1;
                        fChild.SelectedImageIndex = 1;
                    }
                    else if (drv.DriveType == DriveType.Fixed)
                    {
                        fChild.ImageIndex = 0;
                        fChild.SelectedImageIndex = 0;
                    }
                    fChild.Text = drv.Name;
                    fChild.Nodes.Add("");
                    myComputer.Nodes.Add(fChild);
                    returnValue = true;
                }

            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;
            
        }

        private string GetPath(TreeNode parentNode)
        {
            string path = "";
            Char[] arr = { '\\' };
            string[] nameList = parentNode.FullPath.Split(arr);

            if (parentNode.Text == "Desktop")  // To fill Desktop
            {
                path = SpecialDirectories.Desktop + "\\";

                for (int i = 1; i < nameList.Length; i++)
                {
                    path = path + nameList[i] + "\\";
                }

            }
            else   // for other Directories
            {
                if (nameList.Length > 2 && nameList[1] == "My Computer")
                {
                    path = nameList[2] + "\\";
                    for (int i = 4; i < nameList.Length; i++)
                    {
                        path = path + nameList[i] + "\\";
                    }
                }
                else
                {
                    if ( nameList[1] == "My Documents")
                    {
                        switch (nameList[nameList.Length - 1])
                        {
                            case "My Documents":
                                path = SpecialDirectories.MyDocuments + "\\";
                                break;
                            case "My Music":
                                path = SpecialDirectories.MyMusic + "\\";
                                break;
                            case "My Pictures":
                                path = SpecialDirectories.MyPictures + "\\";
                                break;
                            default:
                                path = SpecialDirectories.MyDocuments + "\\" + nameList[2] + "\\";
                                for (int i = 3; i < nameList.Length; i++)
                                {
                                    path = path + nameList[i] + "\\";
                                }
                                break;
                        }
                    }
                }
            }
            return path;
        }


        /* Method :EnumerateDirectory
         * Author : Chandana Subasinghe
         * Date   : 10/03/2006
         * Discription : This is use to Enumerate directories and files
         *          
         */
        public TreeNode EnumerateDirectory(TreeNode parentNode)
        {          
            try
            {
                DirectoryInfo rootDir = new DirectoryInfo(GetPath(parentNode));
                parentNode.Nodes[0].Remove();
                foreach (DirectoryInfo dir in rootDir.GetDirectories())
                {                    
                    TreeNode node = new TreeNode();
                    node.Text = dir.Name;
                    node.Nodes.Add("");
                    parentNode.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                //TODO : 
            }
           
            return parentNode;
        }

        internal void EnumerateFiles(TreeNode treeNode, ListView listView1, ImageList imageList1)
        {
            if (treeNode.Text == "My Computer")
                return;
            DirectoryInfo rootDir = new DirectoryInfo(GetPath(treeNode));
            listView1.Items.Clear();
            //Fill files
            foreach (FileInfo file in rootDir.GetFiles())
            {
                if (file.Extension == ".lnk" || file.Extension == ".LNK")
                    continue;
                string [] fields = { file.Name, file.Length.ToString(), file.CreationTime.ToShortDateString() };
                // Set a default icon for the file.
                Icon iconForFile = SystemIcons.WinLogo;

                ListViewItem item = new ListViewItem(fields, 1);
                iconForFile = Icon.ExtractAssociatedIcon(file.FullName);

                // Check to see if the image collection contains an image 
                // for this extension, using the extension as a key. 
                if (!imageList1.Images.ContainsKey(file.Extension))
                {
                    // If not, add the image to the image list.
                    iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(file.FullName);
                    imageList1.Images.Add(file.Extension, iconForFile);
                }
                item.ImageKey = file.Extension;
                listView1.Items.Add(item);
            }
        }
    }
}
