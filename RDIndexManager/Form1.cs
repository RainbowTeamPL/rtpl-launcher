using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RDIndexManager
{
    public partial class Form1 : Form
    {
        public string defaultPath = @"C:\_Git\rtpl-launcher-serverside\server\ProjectPonyville";
        public FileAttributes fa = new FileAttributes();
        private FolderBrowserDialog fbd = new FolderBrowserDialog();

        private string c_path = "";
        private string c_output = "";
        private bool c_exit = false;

        public Form1(string[] args)
        {
            InitializeComponent();

            SortArgs(args);
        }

        private void SortArgs(string[] args)
        {
            foreach (string arg in args)
            {
                if (arg.Contains("-path"))
                {
                    c_path = arg.Replace("-path", "");
                }

                if (arg.Contains("-o"))
                {
                    c_output = arg.Replace("-o", "");
                }

                if (arg.Contains("-exit"))
                {
                    c_exit = true;
                }
            }

            if (c_exit)
            {
                StartWorkingOnFA();
            }
        }

        private static byte[] MD5Hash(FileInfo first)
        {
            BufferedStream bs = new BufferedStream(first.OpenRead(), 1200000);
            byte[] firstHash = MD5.Create().ComputeHash(bs);

            return firstHash;
        }

        private static bool FilesAreEqual_Hash(FileInfo first, FileInfo second)
        {
            byte[] firstHash = MD5.Create().ComputeHash(first.OpenRead());
            byte[] secondHash = MD5.Create().ComputeHash(second.OpenRead());

            for (int i = 0; i < firstHash.Length; i++)
            {
                if (firstHash[i] != secondHash[i])
                    return false;
            }
            return true;
        }

        private void BrowseFolderBtn_Click(object sender, EventArgs e)
        {
            fbd.ShowNewFolderButton = false;
            fbd.Description = "Select installation folder for game:";
            fbd.SelectedPath = defaultPath;
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;

            var res = fbd.ShowDialog();

            if (res == DialogResult.Cancel)
            {
                //Application.Exit();
            }

            BrowseTextBox.Text = fbd.SelectedPath;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BrowseTextBox.Text = defaultPath;

            if (c_path != "")
            {
                BrowseTextBox.Text = c_path;
            }
        }

        private void MakeBtn_Click(object sender, EventArgs e)
        {
            StartWorkingOnFA();
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private void StartWorkingOnFA()
        {
            LogTextBox.Text = "";

            LogTextBox.Text += "--STARTED--\r\n";
            LogTextBox.Text += "Working directory: " + BrowseTextBox.Text + "\r\n";
            LogTextBox.Text += "Scanning for Directories...\r\n";

            DirectoryInfo di = new DirectoryInfo(BrowseTextBox.Text);
            DirectoryInfo[] Folders = di.GetDirectories("*", SearchOption.AllDirectories);

            List<FileInfo> Files = new List<FileInfo>();

            string root = BrowseTextBox.Text.Replace(Path.GetDirectoryName(BrowseTextBox.Text), "");

            for (int i = 0; i < Folders.Length; i++)
            {
                fa.Folders.Add(Folders[i].FullName.Replace(Path.GetDirectoryName(BrowseTextBox.Text + root), ""));
            }

            LogTextBox.Text += "Listing from folder: " + root + "\r\n";

            for (int i = 0; i < di.GetFiles().Length; i++) //Loop for root folder
            {
                string file = "\\" + di.GetFiles("*.*", SearchOption.TopDirectoryOnly)[i].Name;
                string hash = ByteArrayToString(MD5Hash(di.GetFiles("*.*", SearchOption.TopDirectoryOnly)[i]));
                LogTextBox.Text += "Adding File: " + file + "\r\n";
                LogTextBox.Text += "Hash: " + hash + "\r\n";

                if (!fa.Files.ContainsKey(file))
                {
                    fa.Files.Add(file, hash);
                }
            }

            for (int i = 0; i < Folders.Length; i++)
            {
                //Thread.Sleep(1);
                LogTextBox.Text += "Listing from folder: " + Folders[i].FullName.Replace(BrowseTextBox.Text, "") + "\r\n";

                for (int j = 0; j < Folders[i].GetFiles("*.*", SearchOption.AllDirectories).Length; j++)
                {
                    Thread.Sleep(1);
                    string file = Folders[i].GetFiles("*.*", SearchOption.AllDirectories)[j].DirectoryName.Replace(BrowseTextBox.Text, "") + "\\" + Folders[i].GetFiles("*.*", SearchOption.AllDirectories)[j].Name;
                    string hash = ByteArrayToString(MD5Hash(Folders[i].GetFiles("*.*", SearchOption.AllDirectories)[j]));
                    LogTextBox.Text += "Adding File: " + file + "\r\n";
                    LogTextBox.Text += "Hash: " + hash + "\r\n";

                    if (!fa.Files.ContainsKey(file))
                    {
                        fa.Files.Add(file, hash);
                    }

                    //Files.Add(Folders[i].GetFiles("*.*", SearchOption.AllDirectories)[j]);
                }
            }

            File.WriteAllText(Application.StartupPath + @"\log.log", LogTextBox.Text);

            string jsonOutput = JsonConvert.SerializeObject(fa, Formatting.Indented);

            if (c_output == "")
            {
                File.WriteAllText(Path.Combine(BrowseTextBox.Text, "rdindex.json"), jsonOutput);
            }
            else
            {
                File.WriteAllText(c_output, jsonOutput);
            }

            if (c_exit)
            {
                Application.Exit();
            }
        }
    }
}