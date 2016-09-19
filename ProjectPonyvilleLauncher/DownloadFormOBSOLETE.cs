using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace ProjectPonyvilleLauncher
{
    public partial class DownloadFormOBSOLETE : Form
    {
        public string percentageString = "0%";
        public string downloadedbytes = "0MB/0MB";
        public string speed = "0kb/s";
        public string currFileName = "";
        public int percentage = 0;
        private Stopwatch sw = new Stopwatch();
        private Thread dl = null;

        // The stopwatch which we will be using to calculate the download speed
        private System.Windows.Forms.Timer timer1;

        public DownloadFormOBSOLETE()
        {
            InitializeComponent();
        }

        private delegate void SetTextCallback(string text);

        public void DownloadFile()
        {
            string urlAddress = GlobalVariables.urlAdress;
            string location = GlobalVariables.location;

            HttpWebRequest req = HttpWebRequest.Create(urlAddress) as HttpWebRequest;
            HttpWebResponse response;
            string resUri;
            response = req.GetResponse() as HttpWebResponse;
            resUri = response.ResponseUri.AbsoluteUri;

            WebClient webClient = new WebClient();
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                // The variable that will be holding the url address (making sure it starts with http://)
                //Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);

                // Start the stopwatch which we will be using to calculate the download speed
                sw.Start();

                try
                {
                    // Start downloading the file

                    webClient.DownloadFileAsync(new Uri(resUri), location);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void InitTimer()
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // in miliseconds
            timer1.Start();
        }

        public void SetText(string value)
        {
            switch (GlobalVariables.type)
            {
                case 1:
                    if (this.label1.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetText);
                        this.Invoke(d, new object[] { value });
                    }
                    else
                    {
                        this.label1.Text = value;
                    }

                    break;

                case 2:
                    if (this.label2.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetText);
                        this.Invoke(d, new object[] { value });
                    }
                    else
                    {
                        this.label2.Text = value;
                    }
                    break;

                case 3:
                    if (this.label3.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetText);
                        this.Invoke(d, new object[] { value });
                    }
                    else
                    {
                        this.label3.Text = value;
                    }
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            //UpdateStateText();
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            // Reset the stopwatch.
            sw.Reset();

            if (e.Cancelled == true)
            {
                //MessageBox.Show("Download has been canceled.");
            }
            else
            {
                MessageBox.Show("Download completed!");
                //Close();
            }
        }

        // The event that will fire whenever the progress of the WebClient is changed
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Calculate download speed and output it to labelSpeed.
            speed = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            // Update the progressbar percentage only when the value is not the same.
            percentage = e.ProgressPercentage;

            // Show the percentage on our label.
            percentageString = e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of
            // the file we are currently downloading
            downloadedbytes = string.Format("{0} MB / {1} MB",
(e.BytesReceived / 1024d / 1024d).ToString("0.00"),
(e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
            //UpdateStateText();
            //CurrAction.Text = "Downloading " + string.Format("{0} MB / {1} MB",
            //    (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
            //    (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00")) + " (" + string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00")) + ") " + e.ProgressPercentage.ToString() + "%";
            //progressBar1.Value = e.ProgressPercentage;

            //label1.Text = downloadedbytes;
            //label2.Text = speed;
            //label3.Text = percentageString;
            //progressBar1.Value = percentage;
            GlobalVariables.type = 1;
            SetText(downloadedbytes);
            GlobalVariables.type = 2;
            SetText(speed);
            GlobalVariables.type = 3;
            SetText(percentageString);
        }

        private void DownloadForm_Load(object sender, EventArgs e)
        {
            //Thread.Sleep(1000);
            this.dl = new Thread(new ThreadStart(this.DownloadFile));
            this.dl.Start();
        }
    }
}