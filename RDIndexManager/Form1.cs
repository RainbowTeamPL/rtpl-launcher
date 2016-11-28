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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RDIndexManager
{
    public partial class Form1 : Form
    {
        public string defaultPath = @"C:\_Git\ProjectPonyville\ProjectPonyvilleGame\Build2";
        public FileAttributes fa = new FileAttributes();
        private FolderBrowserDialog fbd = new FolderBrowserDialog();

        public Form1()
        {
            InitializeComponent();
        }

        private static byte[] MD5Hash(FileInfo first)
        {
            byte[] firstHash = MD5.Create().ComputeHash(first.OpenRead());

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
            LogTextBox.Text += "Scanning for Directories...";

            DirectoryInfo di = new DirectoryInfo(BrowseTextBox.Text);
            DirectoryInfo[] Folders = di.GetDirectories("*", SearchOption.AllDirectories);

            List<FileInfo> Files = new List<FileInfo>();

            for (int i = 0; i < Folders.Length; i++)
            {
                //Thread.Sleep(1);
                LogTextBox.Text += "Listing from folder: " + Folders[i].FullName.Replace(BrowseTextBox.Text, "") + "\r\n";

                for (int j = 0; j < Folders[i].GetFiles("*.*", SearchOption.AllDirectories).Length; j++)
                {
                    ThreadPool.SetMaxThreads(3, 3);

                    for (int k = 0; k < j; k++)
                    {
                        string file = Folders[i].GetFiles("*.*", SearchOption.AllDirectories)[j].DirectoryName.Replace(BrowseTextBox.Text, "") + "\\" + Folders[i].GetFiles("*.*", SearchOption.AllDirectories)[j].Name.ToString();
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessFile), file);
                    }
                }

                //Files.Add(Folders[i].GetFiles("*.*", SearchOption.AllDirectories)[j]);
            }

            string jsonOutput = JsonConvert.SerializeObject(fa, Formatting.Indented);
            File.WriteAllText(Path.Combine(Application.StartupPath, "rdindex.json"), jsonOutput);
        }

        private void ProcessFile(object state)
        {
            Thread.Sleep(1);
            string file = state.ToString();
            string hash = ByteArrayToString(MD5Hash(new FileInfo(Path.Combine(BrowseTextBox.Text, file))));
            LogTextBox.Text += "Adding File: " + file + "\r\n";
            LogTextBox.Text += "Hash: " + hash + "\r\n";

            if (!fa.Files.ContainsKey(file))
            {
                fa.Files.Add(file, hash);
            }
        }
    }
}