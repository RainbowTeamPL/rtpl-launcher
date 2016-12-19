using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ProjectPonyvilleLauncher.Enums.Enums;
using static ProjectPonyvilleLauncher.Functions.Functions;
using static ProjectPonyvilleLauncher.Servers.Servers;

namespace ProjectPonyvilleLauncher
{
    public partial class Form1 : Form
    {
        public bool isGameInstalled = false;
        public bool isDownloading = false;
        public string regVersion = "0";

        public bool isOffline = false; //TODO: -offline switch

        public static bool rebuildIndex = false;

        public GameState gameState = GameState.NotInstalled;
        public Process[] updater = Process.GetProcessesByName("LauncherUpdate");
        public UpdateState updateState = UpdateState.Idle;

        public static string installDir = @"C:\Program Files\RainbowTeamPL\";
        public string defDir;

        public string percentageString = "0%";
        public string downloadedbytes = "0 MB/0 MB";
        public string speed = "0 kb/s";
        public string currFileName = "";
        public int percentage = 0;
        private Stopwatch _sw = new Stopwatch();
        //private Thread dl = null;

        private string _urlAddress;
        private string _location;
        public uint downloadedPatches = 0;
        public byte[] localMD5;

        //private Game _game;

        public static Game currGame = Game.Unknown;

        // The stopwatch which we will be using to calculate the download speed
        private System.Windows.Forms.Timer _timer1;

        private bool _cleaned;
        public static bool _restart;
        private string _archivePassword;

        public bool is64 = Environment.Is64BitProcess;
        public bool force32bitBuild = false;

        public bool bTryInstallPrerequisites { get; private set; }

        private Queue<string> _downloadqueue = new Queue<string>();

        public FileAttributes local = new FileAttributes();
        public FileAttributes remote = new FileAttributes();

        private bool _doonce = false;
        private bool _isInstalling = false;

        private List<string> changedItems = new List<string>();

        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;

            SetAccessRule(Application.StartupPath);

            defDir = installDir;

            GameSelection gameSelection = new GameSelection();
            gameSelection.ShowDialog();

            if (!Directory.Exists(Application.StartupPath + @"\Temp"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Temp");
            }

            GetGameInstallDir();

            string[] servers = new string[3];
            servers[0] = GlobalVariables.server1;
            servers[1] = GlobalVariables.server2;
            servers[2] = GlobalVariables.server3;

            CheckEmptyServer();

            switch (Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL\\ProjectPonyville", "Server", "rtpl.dynu.com")))
            {
                case "rtpl.dynu.com":
                    _urlAddress = servers[0];
                    break;

                case "marcinbebenek.capriolo.pl":
                    _urlAddress = servers[1];
                    break;

                case "rtpl.byethost12.com":
                    _urlAddress = servers[2];
                    break;
            }

            InitializeComponent();

            label1.Text = currGame.ToString() + " Launcher";

            //_game = currGame;
            //Console.WriteLine(_game);

            //if (currGame == Game.ProjectPonyville)
            //{
            if (File.Exists(installDir + @"\" + currGame.ToString() + @"\" + currGame.ToString() + ".exe"))
            {
                isGameInstalled = true;
            }

            GetRegVersion();
            //}

            GetServers();

            _archivePassword = schema.password;

            GetChangelog();

            GetVersion();

            local.id = regVersion;

            remote = GetRemoteFA();

            local = TryGetLocalFA();

            DownloadPromoImages();

            gameState = GameState.NotInstalled;
            UpdateBtnText();

            checkGameFilesDifference();

            bool isGameDifferent = changedItems.Any();

            if (isGameInstalled)
            {
                if (isGameDifferent)
                {
                    gameState = GameState.NotUpdated;
                }
                else
                {
                    gameState = GameState.ReadyToPlay;
                }
                UpdateBtnText();
            }
            else
            {
                gameState = GameState.NotInstalled;
                UpdateBtnText();
            }

            Console.Write("regv " + regVersion);
            Console.Write("verl " + VersionLabel.Text);
        }

        private void checkGameFilesDifference()
        {
            if (local.Files.Keys == null)
            {
                local = TryGetLocalFA();
            }
            if (remote.Files.Keys == null)
            {
                remote = GetRemoteFA();
            }

            foreach (var key in remote.Files.Keys)
            {
                try
                {
                    if (!remote.Files[key].Equals(local.Files[key]))
                    {
                        changedItems.Add(key);
                    }
                }
                catch { }
            }
        }

        private FileAttributes TryGetLocalFA()
        {
            if (File.Exists(Application.StartupPath + @"\Tools\" + currGame.ToString() + "rdindex.json"))
            {
                try
                {
                    string localJSON = File.ReadAllText(Application.StartupPath + @"\Tools\" + currGame.ToString() + "rdindex.json");
                    FileAttributes lfa = JsonConvert.DeserializeObject<FileAttributes>(localJSON) as FileAttributes;

                    return lfa;
                }
                catch { }
            }
            else
            {
                GenerateIndex().Wait();

                if (!_doonce)
                {
                    _doonce = true;
                    local = TryGetLocalFA();
                }
            }
            return new FileAttributes();
        }

        private void GetGameInstallDir()
        {
            installDir = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL", "installDir", defDir));
        }

        private void DownloadPromoImages()
        {
            WebClient webClient3 = new WebClient();

            if (File.Exists(Application.StartupPath + @"/Temp/img.jpg"))
            {
                File.Delete(Application.StartupPath + @"/Temp/img.jpg");
            }

            try
            {
                webClient3.DownloadFileCompleted += new AsyncCompletedEventHandler(WebClient3_DownloadFileCompleted);
                webClient3.DownloadFileAsync(new Uri("https://derpicdn.net/img/view/2016/9/15/1249483__safe_oc_fallout+equestria_oc-colon-littlepip_oc-colon-blackjack_artist-colon-oo00set00oo.png"), Application.StartupPath + "/Temp/img.jpg");
            }
            catch (WebException ex)
            {
                Console.WriteLine("Error {0}", ex);

                //button1.Visible = false;
                //button1.Enabled = false;
            }
        }

        private void GetVersion()
        {
            //MOVED TO UPDATER

            //WebClient webClient2 = new WebClient();
            //try
            //{
            //    //webClient2.DownloadFile(GlobalVariables.server1 + "/api/v1/version/get", Application.StartupPath + "/Temp/version.v");
            //    webClient2.DownloadFile(GlobalVariables.github + "/version.txt", Application.StartupPath + "/Temp/version.v");
            //}
            //catch (WebException ex)
            //{
            //    Console.WriteLine("Error {0}", ex);
            //    File.WriteAllText(Application.StartupPath + "/Temp/version.v", "OFFLINE");
            //    InstallBtn.Enabled = false;
            //}

            try
            {
                VersionLabel.Text = File.ReadAllText(Application.StartupPath + "/Temp/version.v");
            }
            catch { }

            //VersionLabel.Text = regVersion;
        }

        private void GetChangelog()
        {
            using (CustomWebClient webClient = new CustomWebClient())
            {
                try
                {
                    webClient.DownloadFile(GlobalVariables.github + "/" + currGame.ToString() + "/changelog.txt", Application.StartupPath + @"\Temp\changelog.tmp");
                }
                catch (WebException ex)
                {
                    Console.WriteLine("Error: {0}", ex);
                    File.WriteAllText(Application.StartupPath + @"\Temp\changelog.tmp", "UNDER MAINTENANCE");
                }
            }
            ChangelogTextBox.Text = File.ReadAllText(Application.StartupPath + @"\Temp\changelog.tmp");
        }

        private void WebClient3_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                promoImage1.Image = CreateNonIndexedImage(@Application.StartupPath + @"\Temp\img.jpg");
                //promoImage1.Enabled = false;

                //pictureBox1.ImageLocation = GlobalVariables.server1 + "/img.jpg";
                //pictureBox1.ImageLocation = "https://derpicdn.net/img/view/2016/9/15/1249483__safe_oc_fallout+equestria_oc-colon-littlepip_oc-colon-blackjack_artist-colon-oo00set00oo.png";
                //button1.Image = CreateNonIndexedImage(Application.StartupPath + "/Temp/img.bmp");
            }
            catch
            {
            }
        }

        private void GetRegVersion()
        {
            regVersion = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL\\" + currGame.ToString(), "Version", "0"));
            force32bitBuild = Convert.ToBoolean(Registry.GetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL\\" + currGame.ToString(), "force32bitBuild", false));
        }

        private delegate void SetTextCallback(string text);

        public void UnZip(string file, string location)
        {
            Thread.Sleep(1000);

            switch (is64)
            {
                case true:
                    SevenZip.SevenZipBase.SetLibraryPath(Application.StartupPath + @"\Tools\7z.dll"); //64-bit system
                    break;

                case false:
                    SevenZip.SevenZipBase.SetLibraryPath(Application.StartupPath + @"\Tools\7z_x86.dll"); //32-bit system
                    break;
            }

            Console.WriteLine(is64);

            //SevenZip.SevenZipBase.SetLibraryPath(Application.StartupPath + @"\Tools\7z.dll"); //old 64bit only

            var zip = new SevenZip.SevenZipExtractor(file, _archivePassword); //added password
            zip.EventSynchronization = SevenZip.EventSynchronizationStrategy.AlwaysAsynchronous;

            try
            {
                zip.ExtractArchive(location);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                Application.Exit();
            }
        }

        #region leftover

        /*public void Patch()
        {
            Download_Tools();
            int patchCount = CountPatches();
            ustate = UpdateState.Downloading;

            if (patchCount > 0)
            {
                for (int i = 1; i <= patchCount; i++)
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(GlobalVariables.server + String.Format("/patches/Patch{0}.MD5", i), String.Format("/Temp/Patch{0}.MD5", i));

                    while (PatchLoop(i) == false)
                    {
                        PatchLoop(i);
                    }
                    downloadedPatches++;
                }
                BeginPatching();
                UnzipGame();
            }
        }*/

        #endregion leftover

        private void UnzipGame()
        {
        }

        #region leftover

        /*private void BeginPatching()
        {
            for (int i = 1; i <= downloadedPatches; i++)
            {
                currFileName = string.Format("Patch{0}.patch", i);
                Process.Start(Application.StartupPath + "/Tools/xdelta.exe",
                    String.Format("patch \"" +
                    Application.StartupPath + "/Temp/Patch{0}.patch\" \"" +
                    Application.StartupPath + "/Temp/CD_Major.zip\" \"" +
                    Application.StartupPath + "/Temp/CD_Patched{1}.zip\"", i, i + 1)).WaitForExit();
            }
        }*/

        /*private bool PatchLoop(int i)
        {
            location = String.Format(Application.StartupPath + "/Temp/Patch{0}.patch", i);
            urlAddress = String.Format(GlobalVariables.server + "/patches/Patch{0}.patch", i);
            currFileName = String.Format("Patch{0}.patch", i);
            DownloadFile();
            string remoteMD5 = File.ReadAllText(String.Format(Application.StartupPath + "/Temp/Patch{0}.MD5", i));
            MD5Patch(i);

            if (remoteMD5 == ByteArrayToString(localMD5))
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/

        /*private void MD5Patch(int i)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(Application.StartupPath + String.Format("/Temp/Patch{0}.patch", i)))
                    {
                        localMD5 = md5.ComputeHash(stream);
                        Console.WriteLine(ByteArrayToString(localMD5));
                        stream.Close();
                    }
                }
            }
            catch
            {
                MD5Patch(i);
            }
        }*/

        /*private static int CountPatches()
        {
            string result = "";
            WebClient webClient = new WebClient();
            webClient.DownloadFile(GlobalVariables.server + "/PatchCount.php", Application.StartupPath + "/Temp/PatchCount.tmp");
            result = File.ReadAllText(Application.StartupPath + "/Temp/PatchCount.tmp");
            int patchCount = Convert.ToInt32(result);
            return patchCount;
        }*/

        #endregion leftover

        private static void Download_Tools()
        {
            if (Directory.Exists(Application.StartupPath + @"\Tools"))
            {
                if (!File.Exists(Application.StartupPath + @"\Tools\tools.exe"))
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile("https://github.com/RainbowTeamPL/rtpl-launcher/raw/master/_StaticDownload/Tools/tools.exe", Application.StartupPath + "/Tools/tools.exe");
                    Process.Start(Application.StartupPath + @"\Tools\tools.exe", "-y -gm2 -InstallPath=\"" + Application.StartupPath + "/Tools/\"").WaitForExit();
                }
            }
            else
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Tools");
                Download_Tools();
            }

            if (!File.Exists(Application.StartupPath + @"\SevenZipSharp.dll"))
            {
                File.Copy(Application.StartupPath + @"\Tools\SevenZipSharp.dll", Application.StartupPath + @"\SevenZipSharp.dll");
            }
        }

        public void DownloadFile()
        {
            isDownloading = true;
            HttpWebRequest req = HttpWebRequest.Create(_urlAddress) as HttpWebRequest;
            HttpWebResponse response;
            string resUri;
            response = req.GetResponse() as HttpWebResponse;
            resUri = response.ResponseUri.AbsoluteUri;

            using (CustomWebClient webClient = new CustomWebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                // The variable that will be holding the url address (making sure it starts with http://)
                //Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);

                // Start the stopwatch which we will be using to calculate the download speed
                _sw.Start();

                try
                {
                    // Start downloading the file
                    webClient.DownloadFileAsync(new Uri(resUri), _location);
                    Console.WriteLine(resUri);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void InitTimer()
        {
            _timer1 = new System.Windows.Forms.Timer();
            _timer1.Tick += new EventHandler(timer1_Tick);
            _timer1.Interval = 1000; // in miliseconds
            _timer1.Start();
        }

        #region leftover

        //public void SetText(string value)
        //{
        //    switch (GlobalVariables.type)
        //    {
        //        case 1:
        //            if (this.label1.InvokeRequired)
        //            {
        //                SetTextCallback d = new SetTextCallback(SetText);
        //                this.Invoke(d, new object[] { value });
        //            }
        //            else
        //            {
        //                this.label1.Text = value;
        //            }

        // break;

        // case 2: if (this.label2.InvokeRequired) { SetTextCallback d = new
        // SetTextCallback(SetText); this.Invoke(d, new object[] { value }); } else {
        // this.label2.Text = value; } break;

        //        case 3:
        //            if (this.label3.InvokeRequired)
        //            {
        //                SetTextCallback d = new SetTextCallback(SetText);
        //                this.Invoke(d, new object[] { value });
        //            }
        //            else
        //            {
        //                this.label3.Text = value;
        //            }
        //            break;
        //    }
        //}

        #endregion leftover

        public void UpdateBtnText()
        {
            switch (gameState)
            {
                case GameState.NotInstalled:
                    InstallBtn.Text = "Install";
                    break;

                case GameState.NotUpdated:
                    InstallBtn.Text = "Update";
                    break;

                case GameState.ReadyToPlay:
                    InstallBtn.Text = "Play";
                    break;
            }
        }

        public void UpdateStateText()
        {
            switch (updateState)
            {
                case UpdateState.Idle:
                    CurrAction.Text = "";
                    break;

                case UpdateState.Downloading:
                    CurrAction.Text = "Downloading " + currFileName + " " + downloadedbytes + " (" + speed + ") " + percentageString;
                    progressBar1.Value = percentage;
                    break;

                case UpdateState.Unzipping:
                    CurrAction.Text = "Unpacking";
                    break;

                case UpdateState.Patching:
                    CurrAction.Text = "Patching";
                    break;

                case UpdateState.ReadyToPlay:
                    CurrAction.Text = "Game is ready to play";
                    break;

                case UpdateState.InstallingPrerequisites:
                    CurrAction.Text = "Installing Prerequisites";
                    break;
            }
        }

        public void ProjectPonyvilleMajor()
        {
            Download_Tools();

            if (File.Exists(installDir + @"\" + currGame.ToString() + @"\" + currGame.ToString() + ".exe"))
            {
                try
                {
                    Directory.Delete(installDir + @"\" + currGame.ToString(), true);
                }
                catch { }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowNewFolderButton = true;
                fbd.Description = "Select installation folder for " + currGame.ToString() + ":";
                fbd.SelectedPath = defDir;
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;

                var res = fbd.ShowDialog();

                if (res == DialogResult.Cancel)
                {
                    Application.Exit();
                }

                installDir = fbd.SelectedPath;
                SetAccessRule(installDir);
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL", "installDir", installDir);
            }

            string[] servers = new string[3];
            servers[0] = GlobalVariables.server1;
            servers[1] = GlobalVariables.server2;
            servers[2] = GlobalVariables.server3;

            CheckEmptyServer();

            switch (Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL\\ProjectPonyville", "Server", "rtpl.dynu.com")))
            {
                case "rtpl.dynu.com":
                    _urlAddress = servers[0];
                    break;

                case "marcinbebenek.capriolo.pl":
                    _urlAddress = servers[1];
                    break;

                case "rtpl.byethost12.com":
                    _urlAddress = servers[2];
                    break;
            }

            remote = GetRemoteFA();

            List<string> list = new List<string>(remote.Files.Keys);

            for (int i = 0; i < remote.Files.Count; i++)
            {
                _downloadqueue.Enqueue(list[i]);
            }

            DownloadItem(); //Start Queue

            //if (force32bitBuild)
            //{
            //    urlAddress = urlAddress + "/patches/ProjectPonyville32.7z";
            //    currFileName = "ProjectPonyville32.7z";
            //}
            //else
            //{
            //    switch (is64)
            //    {
            //        case true:
            //            urlAddress = urlAddress + "/patches/ProjectPonyville.7z";
            //            currFileName = "ProjectPonyville.7z";
            //            break;

            //        case false:
            //            urlAddress = urlAddress + "/patches/ProjectPonyville32.7z";
            //            currFileName = "ProjectPonyville32.7z";
            //            break;
            //    }
            //}                     //CHANGED THE WAY ITS DOWNLOADED

            //urlAddress = urlAddress + "/patches/ProjectPonyville.7z";

            //location = Application.StartupPath + @"\Temp\ProjectPonyville.7z"; //CHANGE HERE

            //currFileName = "ProjectPonyville.7z";
            //this.dl = new Thread(new ThreadStart(this.DownloadFile));
            //this.dl.Start();

            //DownloadFileWUnzip(); //NO MORE UNZIPPING

            //while (isDownloading = true)
            //}
            //    Thread.Sleep(1000);
            //}
            //UnZip(Application.StartupPath + "\\Temp\\ProjectPonyville.7z", Application.StartupPath + "\\ProjectPonyville\\");

            //DownloadFile("http://127.0.0.1/canterlotdefense/patches/CD_Major.7z", Application.StartupPath + "\\Temp\\CD_Major.7z");
            //GlobalVariables.urlAdress = "http://127.0.0.1/canterlotdefense/patches/CD_Major.7z";
            //GlobalVariables.location = Application.StartupPath + "\\Temp\\CD_Major.7z";

            //DownloadForm dlform = new DownloadForm();
            //dlform.Show();
            //DownloadFile("http://marcinbebenek.capriolo.pl/tf/sound/rainbowteampl/events/dispencer/dispencersong.mp3", Application.StartupPath + "\\Temp\\CD_Major.zip");
        }

        private void DownloadItem()
        {
            if (_downloadqueue.Any())
            {
                string nextItem = _downloadqueue.Dequeue();
                currFileName = nextItem;

                updateState = UpdateState.Downloading;
                UpdateStateText();

                string uri = _urlAddress + @"/" + currGame.ToString() + nextItem.Replace(@"\", "/");

                if (!Directory.Exists(installDir + @"\" + currGame.ToString()))
                {
                    try
                    {
                        Directory.CreateDirectory(installDir + @"\" + currGame.ToString());
                    }
                    catch { }
                }

                BuildFolderHierarchy();

                string dir = Path.GetFullPath(installDir + @"\" + currGame.ToString() + nextItem);
                //Console.WriteLine(dir);

                using (CustomWebClient webClient = new CustomWebClient())
                {
                    _sw.Start();

                    //webClient.Headers["Accept-Encoding"] = "gzip,deflate"; //Removed to fix file corruption
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(OnGetDownloadedFileCompleted);
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    webClient.DownloadFileAsync(new Uri(uri), dir);
                }

                return;
            }

            ContinueInstalling(1);
        }

        private void BuildFolderHierarchy() //Creates empty folder structures in Temp //now in installDir
        {
            for (int i = 0; i < remote.Folders.Count; i++)
            {
                try
                {
                    Directory.CreateDirectory(installDir + @"\" + currGame.ToString() + remote.Folders[i]);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ContinueInstalling(int stage)
        {
            switch (stage)
            {
                case 1:
                    GenerateIndexV();
                    break;

                case 2:
                    if (updateState == UpdateState.Unzipping)
                    {
                        MoveTempToInstall();
                    }
                    break;

                case 3:
                    FinishInstall();
                    break;

                default:
                    break;
            }

            //throw new NotImplementedException();
        }

        private void FinishInstall()
        {
            if (bTryInstallPrerequisites == true)
            {
                TryInstallPrerequisites();
            }

            progressBar1.Style = ProgressBarStyle.Continuous;

            if (currGame == Game.ProjectPonyville)
            {
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL\\ProjectPonyville", "Version", VersionLabel.Text);
                GetRegVersion();
            }

            updateState = UpdateState.Idle;
            UpdateStateText();

            updateState = UpdateState.ReadyToPlay;
            UpdateStateText();

            gameState = GameState.ReadyToPlay;
            UpdateBtnText();
            UnlockButton();
        }

        private async void MoveTempToInstall()
        {
            updateState = UpdateState.Unzipping;
            CurrAction.Text = "Installing Files...";

            await MoveTempToInstallTask();
        }

        private Task MoveTempToInstallTask()
        {
            return Task.Run(() =>
            {
                //Not used now, downloading to installDir to prevent exceptions

                //try
                //{
                //    Directory.Move(Application.StartupPath + @"\Temp\ProjectPonyville", installDir);
                //}
                //catch (IOException ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}

                updateState = UpdateState.Idle;

                ContinueInstalling(3);
            });
        }

        private async void GenerateIndexV()
        {
            updateState = UpdateState.Unzipping;
            progressBar1.Style = ProgressBarStyle.Marquee;

            CurrAction.Text = "Generating Index File...";

            await GenerateIndex();
        }

        private Task GenerateIndex()
        {
            return Task.Run(() =>
             {
                 try
                 {
                     FileAttributes fa = new FileAttributes();

                     DirectoryInfo di = new DirectoryInfo(installDir + @"\ProjectPonyville\");
                     DirectoryInfo[] Folders = di.GetDirectories("*", SearchOption.AllDirectories);

                     List<FileInfo> Files = new List<FileInfo>();

                     string root = di.FullName.Replace(installDir + @"\ProjectPonyville", "");

                     for (int i = 0; i < Folders.Length; i++)
                     {
                         fa.Folders.Add(Folders[i].FullName.Replace(installDir + @"\ProjectPonyville", ""));
                     }

                     for (int i = 0; i < di.GetFiles().Length; i++) //Loop for root folder
                     {
                         string file = "\\" + di.GetFiles("*.*", SearchOption.TopDirectoryOnly)[i].Name;
                         string hash = ByteArrayToString(MD5Hash(di.GetFiles("*.*", SearchOption.TopDirectoryOnly)[i]));

                         if (!fa.Files.ContainsKey(file))
                         {
                             fa.Files.Add(file, hash);
                         }
                     }

                     for (int i = 0; i < Folders.Length; i++)
                     {
                         //Thread.Sleep(1);

                         for (int j = 0; j < Folders[i].GetFiles("*.*", SearchOption.AllDirectories).Length; j++)
                         {
                             Thread.Sleep(1);
                             string file = Folders[i].GetFiles("*.*", SearchOption.AllDirectories)[j].DirectoryName.Replace(installDir + @"\ProjectPonyville", "") + "\\" + Folders[i].GetFiles("*.*", SearchOption.AllDirectories)[j].Name;
                             string hash = ByteArrayToString(MD5Hash(Folders[i].GetFiles("*.*", SearchOption.AllDirectories)[j]));

                             if (!fa.Files.ContainsKey(file))
                             {
                                 fa.Files.Add(file, hash);
                             }

                             string jsonOutput = JsonConvert.SerializeObject(fa, Formatting.Indented);
                             File.WriteAllText(Application.StartupPath + @"\Tools\" + currGame.ToString() + "rdindex.json", jsonOutput);
                             //Files.Add(Folders[i].GetFiles("*.*", SearchOption.AllDirectories)[j]);
                         }
                     }

                     if (_isInstalling)
                     {
                         ContinueInstalling(2);
                     }
                 }
                 catch { }
             });
        }

        private async void OnGetDownloadedFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Thread.Sleep(1);
            _sw.Stop();
            _sw.Reset();
            DownloadItem();
        }

        public FileAttributes GetRemoteFA()
        {
            FileAttributes rfa = new FileAttributes();

            using (CustomWebClient webClient = new CustomWebClient())
            {
                try
                {
                    string remoteJSON = webClient.DownloadString(_urlAddress + "/" + currGame.ToString() + "/rdindex.json");
                    rfa = JsonConvert.DeserializeObject(remoteJSON, typeof(FileAttributes)) as FileAttributes;
                }
                catch (WebException ex)
                {
                    MessageBox.Show("Error getting data from remote: \r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }

            return rfa;
        }

        private void CheckEmptyServer()
        {
            if (Registry.GetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL\\ProjectPonyville", "Server", null) == null)
            {
                MessageBox.Show("Please select server in Settings.");

                using (Settings set = new Settings())
                {
                    set.ShowDialog(this);
                }
            }

            if (Registry.GetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL\\ProjectPonyville", "Server", null) == null)
            {
                CheckEmptyServer();
            }
        }

        private void DownloadFileWUnzip()
        {
            isDownloading = true;
            HttpWebRequest req = HttpWebRequest.Create(_urlAddress) as HttpWebRequest;
            HttpWebResponse response;
            string resUri;
            response = req.GetResponse() as HttpWebResponse;
            resUri = response.ResponseUri.AbsoluteUri;

            WebClient webClient = new WebClient();
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(CompletedWUnzip);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                // The variable that will be holding the url address (making sure it starts with http://)
                //Uri URL = urlAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ? new Uri(urlAddress) : new Uri("http://" + urlAddress);

                // Start the stopwatch which we will be using to calculate the download speed
                _sw.Start();

                try
                {
                    // Start downloading the file
                    webClient.DownloadFileAsync(new Uri(resUri), _location);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void CompletedWUnzip(object sender, AsyncCompletedEventArgs e)
        {
            // Reset the stopwatch.
            _sw.Reset();

            if (e.Cancelled == true)
            {
                //MessageBox.Show("Download has been canceled.");
            }
            else
            {
                //MessageBox.Show("Download completed!");
                //Close();
                currFileName = "";
                isDownloading = false;
            }

            updateState = UpdateState.Unzipping;
            UpdateStateText();
            progressBar1.Style = ProgressBarStyle.Marquee;

            await UnZipThread();

            if (bTryInstallPrerequisites == true)
            {
                TryInstallPrerequisites();
            }

            progressBar1.Style = ProgressBarStyle.Continuous;

            if (currGame == Game.ProjectPonyville)
            {
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL\\ProjectPonyville", "Version", VersionLabel.Text);
                GetRegVersion();
            }

            updateState = UpdateState.Idle;
            UpdateStateText();

            updateState = UpdateState.ReadyToPlay;
            UpdateStateText();

            gameState = GameState.ReadyToPlay;
            UpdateBtnText();
            UnlockButton();
        }

        private Task UnZipThread()
        {
            if (currGame == Game.ProjectPonyville)
            {
                return Task.Run(() => { UnZip(Application.StartupPath + @"\Temp\ProjectPonyville.7z", installDir + @"\ProjectPonyville\"); });
            }
            return null;
        }

        #region leftover

        /*private void MD5Major()
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(Application.StartupPath + "/Temp/CD_Major.zip"))
                    {
                        localMD5 = md5.ComputeHash(stream);
                        Console.WriteLine(ByteArrayToString(localMD5));
                        stream.Close();
                    }
                }
            }
            catch
            {
                MD5Major();
            }
        }*/

        #endregion leftover

        private void timer1_Tick(object sender, EventArgs e)
        {
            //webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            //UpdateStateText();
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            // Reset the stopwatch.
            _sw.Reset();

            if (e.Cancelled == true)
            {
                //MessageBox.Show("Download has been canceled.");
            }
            else
            {
                //MessageBox.Show("Download completed!");
                //Close();
                currFileName = "";
                isDownloading = false;
            }
        }

        // The event that will fire whenever the progress of the WebClient is changed
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            updateState = UpdateState.Downloading;
            progressBar1.Style = ProgressBarStyle.Continuous;
            // Calculate download speed and output it to labelSpeed.
            speed = string.Format("{0} kb/s", (e.BytesReceived / 1024d / _sw.Elapsed.TotalSeconds).ToString("0.00"));

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

            //CurrAction.Text = "Downloading " + currFileName + ", " + string.Format("{0} MB / {1} MB",
            //    (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
            //    (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00")) + " (" + string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00")) + ") " + e.ProgressPercentage.ToString() + "%";

            CurrAction.Text = "Downloading: " + currFileName + ", " + downloadedbytes + " (" + speed + ") " + percentageString;

            //progressBar1.Value = e.ProgressPercentage;
            progressBar1.Value = percentage;

            //label1.Text = downloadedbytes;
            //label2.Text = speed;
            //label3.Text = percentageString;
            //progressBar1.Value = percentage;
            //GlobalVariables.type = 1;
            //SetText(downloadedbytes);
            //GlobalVariables.type = 2;
            //SetText(speed);
            //GlobalVariables.type = 3;
            //SetText(percentageString);
            //UpdateStateText();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CurrAction.Text = "";

            if (updater.Length > 0)
            {
                for (int i = 0; i <= updater.Length; i++)
                {
                    updater[i].Kill();
                }
            }

            if (currGame == Game.Unknown)
            {
                Close();
            }

            if (!Directory.Exists(Application.StartupPath + @"\Temp"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Temp");
            }
        }

        private void Install()
        {
            if (!Directory.Exists(Application.StartupPath + @"\Temp"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\Temp");
            }

            bTryInstallPrerequisites = true;
            _isInstalling = true;

            if (currGame == Game.ProjectPonyville)
            {
                ProjectPonyvilleMajor();
            }
        }

        private void TryInstallPrerequisites()
        {
            updateState = UpdateState.InstallingPrerequisites;
            UpdateStateText();

            if (currGame == Game.ProjectPonyville)
            {
                ProcessStartInfo prereq = new ProcessStartInfo(Application.StartupPath + @"\ProjectPonyville\_Redist\install_redist.cmd");
                try
                {
                    Process.Start(prereq).WaitForExit();
                }
                catch { }
            }
        }

        private void InstallBtn_Click(object sender, EventArgs e)
        {
            switch (gameState)
            {
                case GameState.NotInstalled:
                    Install();
                    BlockButton();
                    break;

                case GameState.NotUpdated:
                    UpdateGame();
                    BlockButton();
                    break;

                case GameState.ReadyToPlay:
                    UnlockButton();
                    PlayGame();
                    break;
            }
        }

        private void BlockButton()
        {
            InstallBtn.Enabled = false;
        }

        private void UnlockButton()
        {
            InstallBtn.Enabled = true;
        }

        private void PlayGame()
        {
            if (currGame == Game.ProjectPonyville)
            {
                ProcessStartInfo game = new ProcessStartInfo(installDir + @"\ProjectPonyville\ProjectPonyville.exe", "-game");
                Process.Start(game);
            }

            this.WindowState = FormWindowState.Minimized;
        }

        private void UpdateGame()
        {
            updateState = UpdateState.Patching;

            for (int i = 0; i < changedItems.Count; i++)
            {
                _downloadqueue.Enqueue(changedItems[0]);
                changedItems.RemoveAt(0); //TODO: POSSIBLE LOGIC BREAK FIXED(wrong index numbers)
            }

            _isInstalling = true;

            DownloadItem();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            using (Settings set = new Settings())
            {
                set.ShowDialog(this);
            }

            if (rebuildIndex)
            {
                _isInstalling = false;
                GenerateIndexV();
            }
        }

        // The event that will trigger when the WebClient is completed

        public static Image CreateNonIndexedImage(string path)
        {
            using (var sourceImage = Image.FromFile(path))
            {
                var targetImage = new Bitmap(sourceImage.Width, sourceImage.Height,
                  PixelFormat.Format32bppArgb);
                using (var canvas = Graphics.FromImage(targetImage))
                {
                    canvas.DrawImageUnscaled(sourceImage, 0, 0);
                }
                return targetImage;
            }
        }

        [DllImport("Kernel32.dll", EntryPoint = "CopyMemory")]
        private extern static void CopyMemory(IntPtr dest, IntPtr src, uint length);

        public static Image CreateIndexedImage(string path)
        {
            using (var sourceImage = (Bitmap)Image.FromFile(path))
            {
                var targetImage = new Bitmap(sourceImage.Width, sourceImage.Height,
                  sourceImage.PixelFormat);
                var sourceData = sourceImage.LockBits(
                  new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                  ImageLockMode.ReadOnly, sourceImage.PixelFormat);
                var targetData = targetImage.LockBits(
                  new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                  ImageLockMode.WriteOnly, targetImage.PixelFormat);
                CopyMemory(targetData.Scan0, sourceData.Scan0,
                  (uint)sourceData.Stride * (uint)sourceData.Height);
                sourceImage.UnlockBits(sourceData);
                targetImage.UnlockBits(targetData);
                targetImage.Palette = sourceImage.Palette;
                return targetImage;
            }
        }

        private void promoImage1_Click(object sender, EventArgs e)
        {
            Process.Start("https://derpicdn.net/img/view/2016/9/15/1249483__safe_oc_fallout+equestria_oc-colon-littlepip_oc-colon-blackjack_artist-colon-oo00set00oo.png");
            this.WindowState = FormWindowState.Minimized;
        }

        private void Cleanup()
        {
            if (File.Exists(Application.StartupPath + @"\Temp\img.jpg"))
            {
                try
                {
                    File.Delete(Application.StartupPath + @"\Temp\img.jpg");
                }
                catch { }
            }

            if (File.Exists(Application.StartupPath + @"\Temp\changelog.tmp"))
            {
                File.Delete(Application.StartupPath + @"\Temp\changelog.tmp");
            }

            if (File.Exists(Application.StartupPath + @"\Temp\version.v"))
            {
                File.Delete(Application.StartupPath + @"\Temp\version.v");
            }

            _cleaned = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_restart)
            {
                try //for development only to prevent exception
                {
                    ProcessStartInfo p = new ProcessStartInfo(Application.StartupPath + @"\LauncherUpdate.exe");
                    Process.Start(p);
                }
                catch { }

                _restart = false;
            }

            if (!_cleaned)
            {
                e.Cancel = true;
                Cleanup();

                try
                {
                    Close();
                }
                catch { }
            }
            //Close();
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
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

        private static bool HashesAreEqual(string first, string second)
        {
            if (first == second)
            {
                return true;
            }
            return false;
        }

        private void VersionLabel_Click(object sender, EventArgs e)
        {
            updateState = UpdateState.ReadyToPlay;
            gameState = GameState.ReadyToPlay;

            UnlockButton();

            UpdateBtnText();
        }
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
                (request as HttpWebRequest).UserAgent = "Launcher";
            }
            return request;
        }
    }
}