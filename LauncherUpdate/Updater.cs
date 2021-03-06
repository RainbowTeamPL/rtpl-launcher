﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace LauncherUpdate
{
    public partial class Updater : Form
    {
        //public string downloadUrl = "http://rtpl.dynu.com:3414/projectponyville/patches/launcher/Launcher.exe";

        private const string JSONSchemaURL = "https://api.github.com/repos/RainbowTeamPL/rtpl-launcher/releases/latest";
        private const string JSONSchemaURL64 = "https://api.github.com/repos/RainbowTeamPL/rtpl-launcher-64/releases/latest";

        public bool is64 = Environment.Is64BitOperatingSystem;

        public string JSONSchemaString;

        public Updater()
        {
            if (!File.Exists(Application.StartupPath + @"\Newtonsoft.Json.dll"))
            {
                CustomWebClient dlldl = new CustomWebClient();

                try
                {
                    dlldl.DownloadFile("https://github.com/RainbowTeamPL/rtpl-launcher/raw/master/_StaticDownload/LauncherUpdate/Newtonsoft.Json.dll", Application.StartupPath + @"\Newtonsoft.Json.dll");
                }
                catch { }
            }

            CustomWebClient wc = new CustomWebClient();

            try
            {
                switch (is64)
                {
                    case true:
                        JSONSchemaString = wc.DownloadString(JSONSchemaURL64);
                        break;

                    case false:
                        JSONSchemaString = wc.DownloadString(JSONSchemaURL);
                        break;
                }

                //JSONSchemaString = wc.DownloadString(JSONSchemaURL);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            InitializeComponent();

            //Width = 10;
            Height = 10;
        }

        private void dl_Completed(object sender, AsyncCompletedEventArgs e)
        {
            Process.Start(Application.StartupPath + @"\Launcher.exe");
            Thread.SpinWait(1000);
            Environment.Exit(0);
        }

        private void Updater_Load(object sender, EventArgs e)
        {
            JSONSchema schema = JsonConvert.DeserializeObject<JSONSchema>(JSONSchemaString);

            if (!Directory.Exists(Application.StartupPath + @"\Temp"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Temp");
            }

            File.WriteAllText(Application.StartupPath + @"\Temp\version.v", schema.tag_name);

            BuildNumberLabel.Text = schema.assets[0].updated_at + " " + schema.tag_name;

            if (File.Exists(Application.StartupPath + @"\Launcher.exe"))
            {
                File.Delete(Application.StartupPath + @"\Launcher.exe");
            }

            Console.Write("schema: " + schema.assets[0].browser_download_url + " " + schema.assets[0].updated_at);

            WebClient dl = new WebClient();
            dl.DownloadFileCompleted += new AsyncCompletedEventHandler(dl_Completed);
            dl.DownloadFileAsync(new Uri(schema.assets[0].browser_download_url), Application.StartupPath + @"\Launcher.exe");
        }
    }

    public class JSONSchema
    {
        public IList<JSONAssets> assets { get; set; }

        public string tag_name { get; set; }
    }

    public class JSONAssets
    {
        //[JsonProperty("browser_download_url")]
        public string browser_download_url { get; set; }

        public string updated_at { get; set; }
    }

    internal class CustomWebClient : WebClient
    {
        /// <summary>
        /// Returns a <see cref="T:System.Net.WebRequest" /> object for the specified resource.
        /// </summary>
        /// <param name="address">A <see cref="T:System.Uri" /> that identifies the resource to request.</param>
        /// <returns>
        /// A new <see cref="T:System.Net.WebRequest" /> object for the specified resource.
        /// </returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).KeepAlive = false;
                (request as HttpWebRequest).ProtocolVersion = System.Net.HttpVersion.Version10;
                (request as HttpWebRequest).UserAgent = "LauncherUpdate";
            }
            return request;
        }
    }
}