using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace LauncherUpdate
{
    public partial class Updater : Form
    {
        public string downloadUrl = "http://rtpl.dynu.com:3414/projectponyville/patches/launcher/Launcher.exe";

        public Updater()
        {
            //InitializeComponent();

            if (File.Exists(Application.StartupPath + "/Launcher.exe"))
            {
                File.Delete(Application.StartupPath + "/Launcher.exe");
            }

            WebClient dl = new WebClient();
            dl.DownloadFile(downloadUrl, Application.StartupPath + "/Launcher.exe");
            Process.Start(Application.StartupPath + "/Launcher.exe");

            Environment.Exit(0);
        }
    }
}