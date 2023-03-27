using Ionic.Zip;
using libraryfile;
using Point_Blank_Launcher.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Point_Blank_Launcher
{
    public partial class Connection : Form
    {
        public Process[] processos, processos2;
        public WebClient wc = new WebClient(),
            lcUpdater = new WebClient();
        protected string Address = "http://newlauncher.rootpb.com/launcherupdate/";
        public Connection()
        {
            InitializeComponent();
            lcUpdater.DownloadFileCompleted += lcUpdater_DownloadCompleted;
            Shown += new EventHandler(this.Form1_Shown);
        }
        private string GetLinkAddress(string locationweb)
        {
            return (Address + locationweb);
        }
        private bool CreateDirectory(string directory)
        {
            if (Directory.Exists(directory))
                return true;
            try
            {
                Directory.CreateDirectory(directory);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async void Form1_Shown(object sender, EventArgs e)
        {
            processos = Process.GetProcessesByName("PBLauncher");
            processos2 = Process.GetProcessesByName("PointBlank");
            if (!File.Exists(Application.StartupPath + @"\PBLauncher.log"))
            {
                try
                {
                    using (StreamWriter createfile = new StreamWriter(Application.StartupPath + @"\PBLauncher.log"))
                    {
                        createfile.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("[ERROR] Code 6*2", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }
            }
            saveLog("Launcher started.");
            LoadLanguages();
            if (processos.Length > 1)
            {
                MessageBox.Show(Config.launcher_opened, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                Dispose();
                return;
            }
            else if (processos2.Length > 0)
            {
                MessageBox.Show(Config.game_opened, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                Dispose();
                return;
            }
            int status = 0;
            try
            {
                status = int.Parse(wc.DownloadString(GetLinkAddress("status/status.txt")));
            }
            catch
            {
                saveLog("[ERROR] Code 1*2");
                MessageBox.Show(Config.connection_fail, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                Dispose();
                return;
            }
            if (checkITENS())
            {
                try
                {
                    if (status == 1)
                    {
                        if (!File.Exists(Application.StartupPath + @"\UserFileList.dat") || GetMD5HashFromFile(Application.StartupPath + @"\UserFileList.dat") != wc.DownloadString(GetLinkAddress("updates/userfilelist_hash.txt")))
                        {
                            if (!CreateDirectory(Application.StartupPath + @"\_LauncherPatchFiles"))
                            {
                                saveLog("[ERROR] Code 6*3");
                                Close();
                                return;
                            }
                            try
                            {
                                await lcUpdater.DownloadFileTaskAsync(new Uri(GetLinkAddress("updates/launcher_files/UserFileList.zip")), Path.GetTempPath() + @"\UserFileList.zip");
                            }
                            catch
                            {
                                saveLog("[ERROR] Code 7*2");
                                MessageBox.Show("[ERROR] Code 7*2", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Close();
                            }
                        }
                        else
                        {
                            new Launcher(this, false).Show();
                            ShowInTaskbar = false;
                            Visible = false;
                        }
                    }
                    else if (status == 2)
                    {
                        MessageBox.Show(Config.maintenance, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Close();
                        Dispose();
                    }
                    else if (status == 0)
                    {
                        MessageBox.Show(Config.connection_blocked, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Close();
                        Dispose();
                    }
                    else
                    {
                        saveLog("[ERROR] Code 1*3");
                        MessageBox.Show(Config.connection_fail, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Close();
                        Dispose();
                    }
                }
                catch
                {
                    saveLog("[ERROR] Code 1*4");
                    MessageBox.Show(Config.connection_fail, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                    Dispose();
                }
            }
        }
        void lcUpdater_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                return;
            if (checkITENS())
            {
                //if (unzip(Application.StartupPath, Application.StartupPath + @"\_LauncherPatchFiles\UserFileList.zip"))
                //{
                Unzip(Path.GetTempPath(), Path.GetTempPath() + @"\UserFileList.zip");
                try
                {
                    Directory.Delete(Application.StartupPath + @"\_LauncherPatchFiles", true);
                }
                catch
                {
                    saveLog("[ERROR] Code 6*1");
                    Close();
                    return;
                }
                    new Launcher(this, true).Show();
                    ShowInTaskbar = false;
                    Visible = false;
                //}
                //else
                //{
                //    MessageBox.Show(Config.excract_error, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    Close();
                //}
            }
        }
        private void saveLog(string texto)
        {
            DateTime data = DateTime.Now;
            try
            {
                using (StreamWriter sw = new StreamWriter(Application.StartupPath + @"\PBLauncher.log", true))
                {
                    sw.WriteLine("[" + data.ToString("yyyy/MM/dd") + " | " + data.ToString("HH:mm") + "] " + texto);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch { }
        }
        public void LoadLanguages()
        {
            Settings configs = new Settings();
            if (configs.language == "Português (Brasil)")
                Config.LanguageBRAZILIAN();
            else if (configs.language == "English")
                Config.LanguageENGLISH();
            else if (configs.language == "Turkish")
                Config.LanguageTURKISH();
        }
        protected string GetMD5HashFromFile(string fileName)
        {
            string hash = "";
            try
            {
                using (var md5 = MD5.Create())
                using (var stream = File.OpenRead(fileName))
                {
                    hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                    stream.Close();
                }
            }
            catch
            { }
            return hash;
        }

        private void Connection_Load(object sender, EventArgs e)
        {

        }

        public bool Unzip(string TargetDir, string ZipToUnpack)
        {
            try
            {
                using (ZipFile zip1 = ZipFile.Read(ZipToUnpack))
                    foreach (ZipEntry e2 in zip1)
                        e2.Extract(TargetDir, ExtractExistingFileAction.OverwriteSilently);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool checkITENS()
        {
            if (!File.Exists(Application.StartupPath + @"\libraryfile.dll"))
            {
                saveLog("[ERROR] Code 9*1.");
                MessageBox.Show("[ERROR] Code 9*1", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                Dispose();
                return false;
            }
            else if (!File.Exists(Application.StartupPath + @"\DotNetZip.dll"))
            {
                saveLog("[ERROR] Code 9*2.");
                MessageBox.Show(Config.fail_to_load_dll + "'DotNetZip.dll'", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                Dispose();
                return false;
            }
            return true;
        }
    }
}