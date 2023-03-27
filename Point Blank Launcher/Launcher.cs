using Ionic.Zip;
using libraryfile;
using Point_Blank_Launcher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Point_Blank_Launcher
{
    public partial class Launcher : Form
    {
        public WebClient web = new WebClient(), wc = new WebClient(), launchUpdate = new WebClient(),
            verifUpd = new WebClient();
        protected string Address = "http://newlauncher.rootpb.com/launcherupdate/";
        public bool loadingVerif, loadingUpda, closing, useTemp;
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int size, string lpFileName);
        public Connection conc;
        public Launcher(Connection c, bool tempRequest)
        {
            conc = c;
            useTemp = tempRequest;
            InitializeComponent();
            launchUpdate.DownloadFileCompleted += launchUpdate_DownloadCompleted;
            verifUpd.DownloadProgressChanged += verifUpd_DownloadProgressChanged;
            //openZpt();
        }
        void form_Closing(object sender, FormClosedEventArgs e)
        {
            conc.Close();
            //sw.Close();
            closing = true;
        }
        void launchUpdate_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                return;
            try
            {
                Process.Start(Application.StartupPath + @"\_LauncherPatchFiles\PBLauncherUpdater.exe", "ok");
            }
            catch
            {
                saveLog("[ERROR] Erro 10*1");
                MessageBox.Show("Failed to open launcher updater.", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Close();
        }
        void verifUpd_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Bar1SetProgress(e.BytesReceived, e.TotalBytesToReceive, false);
        }
        void unzip_ExtractProgressChanged(object sender, ExtractProgressEventArgs e)
        {
            try
            {
                if (e.TotalBytesToTransfer != 0)
                    Bar1SetProgress(e.BytesTransferred, e.TotalBytesToTransfer, false);
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
        private List<XMLModel> parse(string path)
        {
            List<XMLModel> xmlModel = new List<XMLModel>();
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    if (fileStream.Length == 0)
                        saveLog("[ERROR] Code 4*1");
                    else
                    {
                        xmlDocument.Load(fileStream);
                        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
                        {
                            if ("list".Equals(xmlNode1.Name))
                            {
                                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                                {
                                    if ("file".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = (XmlNamedNodeMap)xmlNode2.Attributes;
                                        xmlModel.Add(new XMLModel(xml.GetNamedItem("local").Value));
                                    }
                                }
                            }
                        }
                    }
                    fileStream.Dispose();
                    fileStream.Close();
                }
            }
            catch
            {
            }
            return xmlModel;
        }
        public bool checkStatus()
        {
            int status = 0;
            try
            {
                status = int.Parse(wc.DownloadString(GetLinkAddress("status/status.txt")));
            }
            catch
            {
            }
            return status == 0 || status == 2 ? false : true;
        }
        public bool checkIntegrity(out bool fail)
        {
            fail = false;
            bool coisa = true;
            SortedList<string, string> arqs = LoadXML(Application.StartupPath + @"\UserFileList.dat");
            if (arqs != null)
            {
                saveLog("[Integrity count: " + arqs.Count + "]");
                WindowState = FormWindowState.Minimized;
                Verif.Enabled = false;
                Verif.BackgroundImage = Resources.CHECK_APERTADO;
                Start.Enabled = false;
                Start.BackgroundImage = Resources.START_APERTADO;
                List<string> arq2 = new List<string>();
                //label1.Text = Config.verif_files;
                string dire = Application.StartupPath;
                for (int i = 0; i < arqs.Count; i++)
                {
                    Application.DoEvents();
                    string key = arqs.Keys[i], value = "";
                    if (arqs.TryGetValue(key, out value))
                    {
                        if (!File.Exists(dire + key))
                        {
                            string v = getId(dire + key);
                            if (v == "")
                            {
                                MessageBox.Show("[ERRO] Code 11*2", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                saveLog("[ERRO] Code 11*2 (" + key + ")");
                                Close();
                                fail = true;
                                return false;
                            }
                            if (v != value)//getMD5HashFromFile(dire + key) != value)
                                arq2.Add(key);
                        }
                    }
                    if (closing)
                        break;
                }
                Verif.Enabled = true;
                Verif.BackgroundImage = Resources.CHECK_NORMAL;
                Start.BackgroundImage = Resources.START_NORMAL;
                Start.Enabled = true;
                //label1.Text = Config.game_is_ok;
                if (arq2.Count > 0)
                    coisa = false;
            }
            return coisa;
        }
        public string LoadLanguages()
        {
            Settings configs = new Settings();
            if (configs.language == "Português (Brasil)")
                Config.LanguageBRAZILIAN();
            else if (configs.language == "English")
                Config.LanguageENGLISH();
            else if (configs.language == "Turkish")
                Config.LanguageTURKISH();
            return configs.language;
        }
        private string ConvertFileName(string file)
        {
            if (file.Length > 50)
                file = file.Substring(0, 50) + "...";
            return file;
        }
        private string GetLinkAddress(string locationweb)
        {
            return (Address + locationweb);
        }
        private void Launcher_Load(object sender, EventArgs e)
        {
            string lg = LoadLanguages();
            /*languages.Items.Clear();
            languages.Items.Add(lg);
            foreach (string lgS in Config.items)
            {
                if (!languages.Items.Contains(lgS))
                    languages.Items.Add(lgS);
            }
            saveLog("Connection successful.");
            //label1.Text = Config.status;
            label2.Text = Config.file;
            label3.Text = Config.total;*/
            WindowState = FormWindowState.Normal;
            Visible = true;
            Start.Enabled = false;
            string version_lc = "";
            try
            {
                version_lc = web.DownloadString(GetLinkAddress("updates/launcher_hash.txt"));
            }
            catch
            {
                MessageBox.Show("[ERROR] Code 6*12", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                saveLog("[ERROR] Code 6*12");
                Close();
                return;
            } 
            string LauncherMD5 = getMD5HashFromFile(Application.StartupPath + @"\PBLauncher.exe");
            if (Directory.Exists(Application.StartupPath + @"\_LauncherPatchFiles"))
            {
                try
                {
                    Directory.Delete(Application.StartupPath + @"\_LauncherPatchFiles", true);
                }
                catch
                {
                    saveLog("[ERROR] Code 6*8");
                    MessageBox.Show("[ERROR] Code 6*8", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }
            }
            //if (version_lc == LauncherMD5)
            //{
            if (useTemp)
            {
                //label1.Text = Config.update;
                Verif.Enabled = false;
                Verif.BackgroundImage = Resources.CHECK_APERTADO;
                Start.Enabled = false;
                Start.BackgroundImage = Resources.START_APERTADO;
                Button_Update.Visible = true;
                Button_Update.BringToFront();
                Button_Update.Enabled = true;
            }
            else
            {
                //label1.Text = Config.game_is_ok;
                Start.Enabled = true;
                Verif.Enabled = true;
                XMLCleanerLoad();
            }
            //}
            //else
            //{
            //    MessageBox.Show(Config.update_launch, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    saveLog("[WARNING] Launcher update notice.");
            //    try
            //    {
            //        Directory.CreateDirectory(Application.StartupPath + @"\_LauncherPatchFiles");
            //    }
            //    catch
            //    {
            //        saveLog("[ERROR] Code 6*9");
            //        MessageBox.Show("[ERROR] Code 6*9", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        Close();
            //        return;
            //    }
            //    try
            //    {
            //        launchUpdate.DownloadFileAsync(new Uri(GetLinkAddress("updates/launcher_files/Updater.exe")), Application.StartupPath + @"\_LauncherPatchFiles\Updater.exe");
            //    }
            //    catch
            //    {
            //        saveLog("[ERROR] Code 3*1");
            //        MessageBox.Show(Config.upd_lc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        Close();
            //    }
            //}
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Config.leave, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { Close(); closing = true; }
        }
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            closeBut.Image = Resources.CloseZz;
        }
        public void XMLCleanerLoad()
        {
            if (File.Exists(Application.StartupPath + @"\removes.xml"))
            {
                List<XMLModel> items = parse(Application.StartupPath + @"\removes.xml");
                //label1.Text = Config.clear_files;
                Verif.Enabled = false;
                Verif.BackgroundImage = Resources.CHECK_APERTADO;
                fileName.Visible = true;
                for (int i = 0; i < items.Count; i++)
                {
                    XMLModel xml = items[i];
                    if (File.Exists(Application.StartupPath + xml.local))
                    {
                        fileName.Text = ConvertFileName(xml.local);
                        try
                        {
                            File.Delete(Application.StartupPath + xml.local);
                        }
                        catch
                        {
                            saveLog("[ERROR] Code 6*10");
                            MessageBox.Show("[ERROR] Code 6*10", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    Bar2SetProgress(i + 1, items.Count);
                }
                try
                {
                    File.Delete(Application.StartupPath + @"\removes.xml");
                }
                catch
                {
                    saveLog("[ERROR] Code 6*11");
                    MessageBox.Show("[ERROR] Code 6*11", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //label1.Text = Config.game_is_ok;
                fileName.Visible = false;
                Start.Enabled = true;
                Verif.Enabled = true;
                Start.BackgroundImage = Resources.START_NORMAL;
                Verif.BackgroundImage = Resources.CHECK_NORMAL;
            }
        }
        private void pictureBox1_MouseMove(object sender, EventArgs e)
        {
            closeBut.Image = Resources.CloseZz;
        }
        private void Start_MouseLeave(object sender, EventArgs e)
        {
            Start.BackgroundImage = Resources.START_NORMAL;
        }
        private bool Unzip(string TargetDir, string ZipToUnpack, bool showProgress)
        {
            try
            {
                using (ZipFile zip1 = ZipFile.Read(ZipToUnpack))
                {
                    if (showProgress)
                        zip1.ExtractProgress += unzip_ExtractProgressChanged;
                    int TotalFiles = 0;
                    foreach (ZipEntry e2 in zip1)
                        if (!e2.IsDirectory)
                            TotalFiles++;
                    if (showProgress)
                        fileName.Visible = true;
                    Application.DoEvents();
                    for (int i = 0; i < zip1.Count; i++)
                    {
                        ZipEntry e2 = zip1[i];
                        if (!e2.IsDirectory && showProgress)
                        {
                            fileName.Text = ConvertFileName(e2.FileName.Contains("/") ? e2.FileName.Substring(e2.FileName.LastIndexOf("/") + 1) : e2.FileName);
                            Bar2SetProgress(++i, TotalFiles);
                        }
                        e2.Extract(TargetDir, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
                return true;
            }
            catch
            {
                MessageBox.Show(Config.excract_error1, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        private void Start_MouseMove(object sender, MouseEventArgs e)
        {
            Start.BackgroundImage = Resources.START_COM_MOUSE;
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
        private async void Button_Update_Click(object sender, EventArgs e)
        {
            loadingUpda = true;
            Button_Update.BackgroundImage = Resources.UPDATE_APERTADO;
            Button_Update.Enabled = false;
            fileName.Text = "";
            fileName.Visible = true;
            //label1.Text = Config.searching_att;
            Application.DoEvents();
            if (closing) return;
            string tempPath = Path.GetTempPath();
            if (File.Exists(tempPath + @"\UserFileList.dat"))
            {
                SortedList<string, string> arqs = LoadXML(tempPath + @"\UserFileList.dat"),
                    arqs2;
                if (!File.Exists(Application.StartupPath + @"\UserFileList.dat"))
                    arqs2 = new SortedList<string,string>();
                else 
                    arqs2 = LoadXML(Application.StartupPath + @"\UserFileList.dat");
                List<string> news = new List<string>();
                foreach (string filePath in arqs.Keys)
                {
                    string value;
                    if (arqs.TryGetValue(filePath, out value) && (!arqs2.ContainsKey(filePath) || !arqs2.ContainsValue(value)))
                    {
                        news.Add(filePath);
                        saveLog("Update file: " + filePath);
                    }
                }
                if (!CreateDirectory(Application.StartupPath + @"\_DownloadPatchFiles"))
                {
                    saveLog("[ERROR] Code 6*6");
                    MessageBox.Show(Config.upd_gc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                    return;
                }
                saveLog("The client have " + news.Count + " invalid items.");
                Bar2SetProgress(0, 100);
                //label1.Text = Config.downl_file;
                int prog = 0;
                foreach (string p in news)
                {
                    Application.DoEvents();
                    string file = p.Substring(1), coisa = "";
                    int idx = file.LastIndexOf(@"\");
                    if (idx != -1)
                    {
                        coisa = @"\_DownloadPatchFiles\" + file.Substring(0, idx);
                        if (Directory.Exists(Application.StartupPath + @"\_DownloadPatchFiles"))
                        {
                            if (!CreateDirectory(Application.StartupPath + coisa))
                            {
                                saveLog("[ERROR] Code 6*7");
                                MessageBox.Show(Config.upd_gc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                Close();
                                return;
                            }
                        }
                        else
                        {
                            saveLog("[ERROR] Code 3*12");
                            MessageBox.Show(Config.upd_gc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Close();
                            return;
                        }
                    }
                    string name = p.Substring(p.LastIndexOf(@"\") + 1);
                    fileName.Text = ConvertFileName(name);
                    try
                    {
                        await verifUpd.DownloadFileTaskAsync(new Uri(GetLinkAddress("updates/client_files/" + file + ".zip")), Application.StartupPath + @"\_DownloadPatchFiles\" + file + ".zip");
                    }
                    catch
                    {
                        saveLog("[ERROR] Code 3*2; by: " + name);
                        MessageBox.Show(Config.upd_gc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Close();
                        closing = true;
                    }
                    if (closing) return;
                    Bar2SetProgress(++prog, news.Count);
                }
                Bar2SetProgress(0, 100);
                prog = 0;
                //label1.Text = Config.excract_file;
                if (!Directory.Exists(Application.StartupPath + @"\_DownloadPatchFiles"))
                {
                    saveLog("[ERROR] Code 3*14");
                    MessageBox.Show(Config.upd_gc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                    return;
                }
                foreach (string p in news)
                {
                    Application.DoEvents();
                    string file = p.Substring(1), coisa = "";
                    fileName.Text = ConvertFileName(p.Substring(p.LastIndexOf(@"\") + 1) + ".zip");
                    int idx = file.LastIndexOf(@"\");
                    if (idx != -1)
                    {
                        coisa = @"\" + file.Substring(0, idx);
                        if (!CreateDirectory(Application.StartupPath + coisa))
                        {
                            saveLog("[ERROR] Code 3*13");
                            MessageBox.Show(Config.upd_gc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Close();
                            return;
                        }
                    }
                    bool success = Unzip(Application.StartupPath + @"\" + coisa, Application.StartupPath + @"\_DownloadPatchFiles\" + file + ".zip", false);
                    if (!success)
                    {
                        saveLog("[ERROR] Code 3*2; By: " + coisa);
                        Close();
                        closing = true;
                    }
                    if (closing) return;
                    Bar2SetProgress(++prog, news.Count);
                }
                try
                {
                    Directory.Delete(Application.StartupPath + @"\_DownloadPatchFiles", true);
                }
                catch
                {
                    saveLog("[ERROR] Code 6*7");
                    MessageBox.Show(Config.upd_gc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                    return;
                }
                Bar2SetProgress(1, 1);
                fileName.Visible = false;
                Verif.Enabled = true;
                Verif.BackgroundImage = Resources.CHECK_NORMAL;
                Start.BackgroundImage = Resources.START_NORMAL;
                Start.Enabled = true;
                Start.BringToFront();
                news = null;
                //label1.Text = Config.game_is_ok;
                if (useTemp && !Unzip(Application.StartupPath, tempPath + @"\UserFileList.zip", false))
                {
                    saveLog("[ERROR] Code 9*9");
                    Close();
                    return;
                }
                useTemp = false;
            }
        }
        private void Button_Update_MouseLeave(object sender, EventArgs e)
        {
            if (!loadingUpda)
                Button_Update.BackgroundImage = Resources.UPDATE_NORMAL;
        }
        private void Button_Update_MouseMove(object sender, MouseEventArgs e)
        {
            Button_Update.BackgroundImage = Resources.UPDATE_COM_MOUSE;
        }
        public void Bar1SetProgress(long received, long maximum, bool progress)
        {
            ArchiveBar.Width = (int)((received * 389) / maximum);
        }

        public void Bar2SetProgress(ulong received, ulong maximum)
        {
            TotalBar.Width = (int)((received * 930) / maximum);
        }
        public void Bar2SetProgress(int received, int maximum)
        {
            TotalBar.Width = ((received * 930) / maximum);
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            minimBut.BackgroundImage = Resources.miniZZ;
        }

        private void pictureBox4_MouseMove(object sender, MouseEventArgs e)
        {
            minimBut.BackgroundImage = Resources.miniZZ;
        }
        private void Verif_MouseLeave(object sender, EventArgs e)
        {
            if (!loadingVerif)
                Verif.BackgroundImage = Resources.CHECK_NORMAL;
        }

        private void Verif_MouseMove(object sender, MouseEventArgs e)
        {
            Verif.BackgroundImage = Resources.CHECK_COM_MOUSE;
        }
        public bool Modpack(string[] items)
        {
            bool found = false;
            foreach (string f in items)
            {
                if (closing) return false;
                int idx = f.LastIndexOf(@"\");
                string f2 = f.Substring(idx);
                if (f2.Contains("(") || f2.Contains(")"))
                {
                    found = true;
                    try
                    {
                        File.Delete(f);
                    }
                    catch { return false; }
                }
            }
            if (found)
            {
                WindowState = FormWindowState.Normal;
                Activate();
                //label1.Text = Config.need_check;
                Verif.Enabled = true;
                Verif.BackgroundImage = Resources.CHECK_NORMAL;
                Start.Enabled = false;
                Start.BackgroundImage = Resources.START_APERTADO;
                MessageBox.Show(Config.need_check, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else return true;
        }
        private void Start_Click(object sender, EventArgs e)
        {
            if (checkStatus())
            {
                bool fail = false;//;
                bool integ = true;//checkIntegrity(out fail);
                if (fail) return;
                if (integ)
                {
                    string[] files = new string[0];
                    try
                    {
                        files = Directory.GetFiles(Application.StartupPath, "*", SearchOption.AllDirectories);
                        saveLog("[Integrity failed: " + files.Length + "]");
                    }
                    catch
                    {
                        saveLog("[ERRO] Code 7*1");
                        Close();
                        return;
                    }

                    bool mod = true;//Modpack(files);
                    if (mod)
                    {
                        try
                        {
                            Process.Start(Application.StartupPath + @"\PointBlank.exe", Config.password);
                        }
                        catch
                        {
                            MessageBox.Show("Failed to open a program needed for the game. (5*2)", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            saveLog("[ERROR] Code 5*2");
                        }
                        Close();
                    }
                }
                else
                {
                    WindowState = FormWindowState.Normal;
                    Activate();
                    //label1.Text = Config.need_check;
                    Verif.Enabled = true;
                    Verif.BackgroundImage = Resources.CHECK_NORMAL;
                    Start.Enabled = false;
                    Start.BackgroundImage = Resources.START_APERTADO;
                    MessageBox.Show(Config.need_check, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show(Config.maintenance, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
        }
        public int a, b;
        private void launcher_MouseDown(object sender, MouseEventArgs e)
        {
            a = Panel.MousePosition.X - Location.X;
            b = Panel.MousePosition.Y - Location.Y;
        }
        private void launcher_MouseMove(object sender, MouseEventArgs e)
        {
            Point newPoint = new Point();
            if (e.Button == MouseButtons.Left)
            {
                newPoint = Panel.MousePosition;
                newPoint.X = newPoint.X - (a);
                newPoint.Y = newPoint.Y - (b);
                Location = newPoint;
            }
        }

        private async void Verif_Click(object sender, EventArgs e)
        {
            loadingVerif = true;
            SortedList<string, string> arqs = LoadXML((useTemp ? Path.GetTempPath() : Application.StartupPath) + @"\UserFileList.dat");
            if (arqs != null)
            {
                Verif.Enabled = false;
                Verif.BackgroundImage = Resources.CHECK_APERTADO;
                Start.Enabled = false;
                Start.BackgroundImage = Resources.START_APERTADO;
                List<string> arq2 = new List<string>();
                //label1.Text = Config.verif_files;
                fileName.Visible = true;
                for (int i = 0; i < arqs.Count; i++)
                {
                    Application.DoEvents();
                    string key = arqs.Keys[i], value = "", dire = Application.StartupPath;
                    fileName.Text = ConvertFileName(key.Substring(key.LastIndexOf(@"\") + 1));
                    if (arqs.TryGetValue(key, out value))
                    {
                        string v = getId(dire + key);
                        if (File.Exists(dire + key) && v == "")
                        {
                            MessageBox.Show("[ERRO] Code 11*1", "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            saveLog("[ERRO] Code 11*11 (" + key + ")");
                            Close();
                            return;
                        }
                        bool Exists = File.Exists(dire + key);
                        if (!Exists || v != value)
                        {
                            arq2.Add(key);
                            saveLog("File verified: " + key + "; by: " + (!Exists ? 0 : 1));
                        }
                    }
                    //MessageBox.Show("P: " + key);
                    Bar2SetProgress(i, arqs.Count);
                    if (closing)
                    { return; }
                }
                Bar2SetProgress(0, 100);
                //label1.Text = Config.downl_file;
                int prog = 0;
                if (!CreateDirectory(Application.StartupPath + @"\_DownloadPatchFiles"))
                {
                    saveLog("[ERROR] Code 3*5");
                    MessageBox.Show(Config.upd_gc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                    return;
                }
                foreach (string p in arq2)
                {
                    Application.DoEvents();
                    string file = p.Substring(1), coisa = "";
                    int idx = file.LastIndexOf(@"\");
                    if (idx != -1)
                    {
                        coisa = @"\_DownloadPatchFiles\" + file.Substring(0, idx);
                        if (Directory.Exists(Application.StartupPath + @"\_DownloadPatchFiles"))
                        {
                            if (!CreateDirectory(Application.StartupPath + coisa))
                            {
                                saveLog("[ERROR] Code 6*7");
                                MessageBox.Show(Config.upd_gc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                Close();
                                return;
                            }
                        }
                        else
                        {
                            saveLog("[ERROR] Code 3*11");
                            MessageBox.Show(Config.upd_gc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Close();
                            return;
                        }
                    }
                    string name = p.Substring(p.LastIndexOf(@"\") + 1);
                    fileName.Text = ConvertFileName(name);
                    try
                    {
                        await verifUpd.DownloadFileTaskAsync(new Uri(GetLinkAddress("updates/client_files/" + file + ".zip")), Application.StartupPath + @"\_DownloadPatchFiles\" + file + ".zip");
                    }
                    catch (Exception ex)
                    {
                        saveLog("[ERROR] Code 3*2; by: " + name);
                        //MessageBox.Show("[" + ex.ToString() + "]\r\n" + Config.upd_gc_error, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //Close();
                    }
                    if (closing) return;
                    Bar2SetProgress(++prog, arq2.Count);
                }
                Bar2SetProgress(0, 100);
                prog = 0;
                //label1.Text = Config.excract_file;
                if (!Directory.Exists(Application.StartupPath + @"\_DownloadPatchFiles"))
                {
                    saveLog("[ERROR] Code 3*6");
                    MessageBox.Show(Config.upd_gc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                    return;
                }
                foreach (string p in arq2)
                {
                    Application.DoEvents();
                    string file = p.Substring(1), coisa = "";
                    fileName.Text = ConvertFileName(p.Substring(p.LastIndexOf(@"\") + 1) + ".zip");
                    int idx = file.LastIndexOf(@"\");
                    if (idx != -1)
                    {
                        coisa = @"\" + file.Substring(0, idx);
                        if (!CreateDirectory(Application.StartupPath + coisa))
                        {
                            saveLog("[ERROR] Code 3*10");
                            MessageBox.Show(Config.upd_gc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Close();
                            return;
                        }
                    }
                    bool success = Unzip(Application.StartupPath + @"\" + coisa, Application.StartupPath + @"\_DownloadPatchFiles\" + file + ".zip", false);
                    if (!success)
                        saveLog("[ERROR] Code 3*2; By: " + coisa);
                    if (closing) return;
                    Bar2SetProgress(++prog, arq2.Count);
                }
                try
                {
                    Directory.Delete(Application.StartupPath + @"\_DownloadPatchFiles", true);
                }
                catch
                {
                    saveLog("[ERROR] Code 6*7");
                    MessageBox.Show(Config.upd_gc_error2, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                    return;
                }
                Bar2SetProgress(1, 1);
                fileName.Visible = false;
                Verif.Enabled = true;
                Verif.BackgroundImage = Resources.CHECK_NORMAL;
                Start.BackgroundImage = Resources.START_NORMAL;
                Start.Enabled = true;
                //label1.Text = Config.game_is_ok;
                arq2 = null;
            }
            arqs = null;
            if (useTemp)
                Unzip(Application.StartupPath, Path.GetTempPath() + @"\UserFileList.zip", false);
            useTemp = false;
            loadingVerif = false;
        }
        public string getId(string dir)
        {
            try
            {
                return Directory.GetLastWriteTimeUtc(dir).ToString("ssMMHHmmddyyyy");
            }
            catch
            {
                saveLog("[err: '" + dir + "']");
                return "";
            }
        }
        protected string getMD5HashFromFile(Stream coisa)
        {
            string hash = "";
            try
            {
                using (var md5 = MD5.Create())
                using (var stream = coisa)
                {
                    hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                    stream.Close();
                }
            }
            catch
            { }
            return hash;
        }
        protected string getMD5HashFromFile(string fileName)
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
        private static SortedList<string, string> LoadXML(string path)
        {
            SortedList<string, string> files = new SortedList<string, string>();
            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    if (fileStream.Length != 0)
                    {
                        xmlDocument.Load(fileStream);
                        for (XmlNode xmlNode1 = xmlDocument.FirstChild; xmlNode1 != null; xmlNode1 = xmlNode1.NextSibling)
                        {
                            if ("list".Equals(xmlNode1.Name))
                            {
                                for (XmlNode xmlNode2 = xmlNode1.FirstChild; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
                                {
                                    if ("f".Equals(xmlNode2.Name))
                                    {
                                        XmlNamedNodeMap xml = xmlNode2.Attributes;
                                        string n = xml.GetNamedItem("n").Value;
                                        if (!files.ContainsKey(n))
                                            files.Add(n, xml.GetNamedItem("m").Value);
                                    }
                                }
                            }
                        }
                    }
                    fileStream.Dispose();
                    fileStream.Close();
                }
            }
            catch
            { }
            return files;
        }

       /* private void languages_SelectedIndexChanged(object sender, EventArgs e)
        {
           /* Settings configs = new Settings();
            if (configs.language != (string)languages.SelectedItem)
            {
                configs.language = (string)languages.SelectedItem;
                configs.Save();
                languages.Enabled = false;
                MessageBox.Show(Config.change_lang, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show(Config.language_error, "PBLauncher", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }*/
    }
}
 