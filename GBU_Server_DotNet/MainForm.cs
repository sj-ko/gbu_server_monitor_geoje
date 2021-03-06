﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Security;
using gx;
using cm;
using System.Linq;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Configuration;
using System.Diagnostics;

namespace GBU_Server_Monitor
{
    public partial class MainForm : Form
    {
        [Serializable]
        public class SettingContainer
        {
            public Setting globalSetting;
            public List<ANPRCam> camList;
        };

        public Setting setting;

        private const int NUM_OF_CAM = 100; //5;

        private System.Threading.Timer timer;
        private AutoResetEvent timerEvent;

        //private FileSystemWatcher[] watcher = new FileSystemWatcher[NUM_OF_CAM];

        private bool playStatus = false;

        private const string AXXON_HTTP_URL_1 = "http://localhost:8088/gbu/live/media/snapshot/KSJCJ-NOTE-PC/DeviceIpint.";
        private const string AXXON_HTTP_URL_2 = "/SourceEndpoint.video:0:0";

        public class PLATE_FOUND
        {
            public int no;
            public int cam;
            public DateTime dateTime;
            public string plateStr;
            public string imageFilePath;
        };

        [Serializable]
        public class ANPRCam
        {
            public int camid;
            public string name;
            //public string rtspUrl;
            //public double latitude;
            //public double longitude;
            public string address;
            public string username;
            public string password;
            public TreeNode node;
            public GMapOverlay markersOverlay;
            public long recognizedTime { get; set; }
            public PointLatLng position { get; set; }

            // add cam manufacturer
            public Constants.MANUFACTURER manufacturer;
        };

        struct Route
        {
            public string plate;
            public int camid;
            public DateTime dateTime;
            public ANPRCam anprCamObj;
        };

        private List<PLATE_FOUND> _plateList;
        private int _plateListIdx = 0;

        public List<ANPRCam> _anprCamList { get; private set; }

        public Database dbManager;

        //public string savepath = @"D:\anprtest";

        private TreeNode _selectedNode;

        private ImageImpoter[] imageImporter = new ImageImpoter[NUM_OF_CAM];
        private Client[] anprClient = new Client[NUM_OF_CAM];

        private BackgroundWorker playWorker = new BackgroundWorker();
        private BackgroundWorker stopWorker = new BackgroundWorker();
        private PopUp popUp;

        public MainForm()
        {
            InitializeComponent();
        }

        ~MainForm()
        {
            Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _anprCamList = new List<ANPRCam>();
            dbManager = new Database();
            _plateList = new List<PLATE_FOUND>();

            playWorker.WorkerReportsProgress = true;
            playWorker.WorkerSupportsCancellation = true;
            playWorker.DoWork += new DoWorkEventHandler(playWorker_DoWork);
            playWorker.ProgressChanged += new ProgressChangedEventHandler(playWorker_ProgressChanged);
            playWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(playWorker_RunWorkerCompleted);

            stopWorker.WorkerReportsProgress = true;
            stopWorker.WorkerSupportsCancellation = true;
            stopWorker.DoWork += new DoWorkEventHandler(stopWorker_DoWork);
            stopWorker.ProgressChanged += new ProgressChangedEventHandler(stopWorker_ProgressChanged);
            stopWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(stopWorker_RunWorkerCompleted);

            listView_result.View = View.Details;
            listView_result.FullRowSelect = true;
            listView_result.GridLines = true;

            listView_result.Columns.Add("카메라", 50, HorizontalAlignment.Left);
            listView_result.Columns.Add("시간", 200, HorizontalAlignment.Left);
            listView_result.Columns.Add("차량번호", 100, HorizontalAlignment.Left);

            comboBox_manufacturer.Items.Add("기타");
            comboBox_manufacturer.Items.Add("AXIS");
            comboBox_manufacturer.Items.Add("HIKVISION");
            comboBox_manufacturer.Items.Add("한화테크윈");
            comboBox_manufacturer.Items.Add("HONEYWELL");

            InitSetting();

            // Initialize map:
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gMapControl1.Position = new PointLatLng(34.8544319, 128.6288147); // for geoje
            gMapControl1.CanDragMap = true;
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.CacheLocation = Environment.CurrentDirectory + @"\GMapCache";

            loadGlobalSetting();
        }

        private void Btn_Disconnect_Click(object sender, EventArgs e)
        {
            
        }

        private void InitSetting()
        {
            setting = new Setting();
            setting.PropertyChanged += setting_PropertyChanged;
        }

        private void setting_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //UpdateFormUIValue();
        }

        private void Stop()
        {
            if (timer != null)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite); // stop timer
                timer.Dispose();
                timer = null;
            }

            int progress = 0;
            for (int i = 0; i < _anprCamList.Count; i++)
            {
                if (imageImporter[i] != null)
                {
                    imageImporter[i].Stop();
                    imageImporter[i].ANPRDetected -= MainForm_ANPRDetected;
                    imageImporter[i].ANPRStatus -= MainForm_ANPRStatus;
                    imageImporter[i] = null;

                    progress = (int)((float)((float)i / (float)_anprCamList.Count) * 100);
                    stopWorker.ReportProgress(progress);
                }

                if (anprClient[i] != null)
                {
                    anprClient[i].Stop();
                    anprClient[i] = null;
                }
            }

            /*

            for (int i = 0; i < NUM_OF_CAM; i++)
            {
                if (watcher[i] != null)
                {
                    watcher[i].EnableRaisingEvents = false;
                    watcher[i].Dispose();
                    watcher[i] = null;
                }
            }*/

            //_plateListIdx = 0;
            //_plateList.Clear();
        }

        private void MediaTimerCallBack(Object obj)
        {
            this.BeginInvoke(new Action(() =>
                {
                    foreach (ANPRCam cam in _anprCamList)
                    {
                        if ((DateTime.Now.Ticks - cam.recognizedTime) / 10000f > 3000f && cam.recognizedTime > 0) // 3 sec
                        {
                            cam.markersOverlay.Markers.Clear();
                            GMarkerGoogle marker = new GMarkerGoogle(cam.position, GMarkerGoogleType.green);
                            cam.markersOverlay.Markers.Add(marker);
                        }
                    }
                }
            ));

        }

        private static Bitmap ResizeBitmap(Bitmap sourceBMP, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
                g.DrawImage(sourceBMP, 0, 0, width, height);
            return result;
        }

        private void OnChangedANPRPath(object source, FileSystemEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(e.FullPath + " changed");
            //TextBox[] textBoxes = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11, textBox12, textBox13, textBox14, textBox15, textBox16, textBox17, textBox18, textBox19, textBox20 };
            //PictureBox[] pictureBoxes = new PictureBox[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20 };

            string lastLine = null;

            //try
            //{
                lastLine = File.ReadLines(e.FullPath).Last();

                string[] logresults = lastLine.Split(',');
                this.BeginInvoke(new Action(() =>
                    {
                        //pictureBox_result.ImageLocation = logresults[3];
                        //textBox_result.Text = "카메라 " + logresults[0] + " : " + findANPRCamName(Convert.ToInt32(logresults[0], 10)) + " 차량번호 : " + logresults[1];

                        DateTime time = new DateTime();
                        string[] itemStr = { logresults[0], logresults[2], logresults[1] };
                        ListViewItem item = new ListViewItem(itemStr);
                        listView_result.Items.Add(item);
                        listView_result.Items[listView_result.Items.Count - 1].EnsureVisible();

                        PLATE_FOUND found = new PLATE_FOUND();

                        found.no = _plateListIdx;
                        found.cam = Convert.ToInt32(logresults[0], 10);
                        found.dateTime = Convert.ToDateTime(logresults[2]);
                        found.imageFilePath = logresults[3];
                        found.plateStr = logresults[1];

                        _plateList.Add(found);
                        _plateListIdx++;

                        foreach (ANPRCam cam in _anprCamList)
                        {
                            int id = Convert.ToInt32(logresults[0], 10);
                            if (cam.camid == id)
                            {
                                cam.recognizedTime = DateTime.Now.Ticks;
                                cam.position = cam.markersOverlay.Markers[0].Position;
                                cam.markersOverlay.Markers.Clear();
                                GMarkerGoogle marker = new GMarkerGoogle(cam.position, GMarkerGoogleType.red);
                                cam.markersOverlay.Markers.Add(marker);
                            }
                        }
                    }
                ));

            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine("OnChangedANPRPath() error : " + ex.ToString());
            //}

        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void button_PlayStop_Click(object sender, EventArgs e)
        {
            if (playStatus == true)
            {
                // Rev 1
                //System.Diagnostics.Process.Start(@"taskkill", @"/f /im GBU_Server_DotNet*");
                // ---
                // Rev 2 - Axxon Module
                
                // ---
                popUp = new PopUp();
                popUp.SetMessage("연결 끊는 중...");
                stopWorker.RunWorkerAsync();
                popUp.ShowDialog();

                // workaround for camname reset
                foreach (ANPRCam cam in _anprCamList)
                {
                    cam.node.Text = "카메라 " + cam.camid + " - " + cam.name;
                }
            }
            else
            {
                // Rev 1 
                /*
                for (int i = 0; i < _anprCamList.Count; i++)
                {
                    System.Diagnostics.Process.Start(setting.serverPath + @"\GBU_Server_DotNet.exe", setting.configPath + @"\ch" + _anprCamList[i].camid + ".cfg --autostart --enlarge");
                }
                */
                // ---
                // Rev 2 - Axxon Module
                popUp = new PopUp();
                popUp.SetMessage("연결 중...");
                playWorker.RunWorkerAsync();
                popUp.ShowDialog();

                // ---

                timerEvent = new AutoResetEvent(true);
                timer = new System.Threading.Timer(MediaTimerCallBack, null, 100, 200);


                /*for (int i = 0; i < _anprCamList.Count; i++) // NUM_OF_CAM -> to be changed _anprCamList.Count
                {
                    watcher[i] = new FileSystemWatcher();
                    string path = setting.savePath + "\\ch" + _anprCamList[i].camid;
                    if (Directory.Exists(path))
                    {
                        watcher[i].Path = path;
                        watcher[i].NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                        watcher[i].Filter = "*.jpg";
                        watcher[i].Changed += new FileSystemEventHandler(OnChangedANPRPath);
                        watcher[i].EnableRaisingEvents = true;
                    }
                }*/

                
            }
        }

        void playWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int progress = 0;
            for (int i = 0; i < _anprCamList.Count; i++)
            {
                //string axxonUrl = "http://192.168.0.16/Streaming/channels/1/picture"; // AXXON_HTTP_URL_1 + _anprCamList[i].camid + AXXON_HTTP_URL_2;
                if (setting.mode == 0)
                {
                    imageImporter[i] = new ImageImpoter();
                    imageImporter[i].ANPRDetected += MainForm_ANPRDetected;
                    imageImporter[i].ANPRStatus += MainForm_ANPRStatus;
                    imageImporter[i].SavePath = setting.savePath;

                    // adjust address by manufacturer
                    string address = _anprCamList[i].address;

                    switch (_anprCamList[i].manufacturer)
                    {
                        case Constants.MANUFACTURER.AXIS:
                            address = "http://" + _anprCamList[i].address + "/jpg/image.jpg";
                            break;
                        case Constants.MANUFACTURER.HIKVISION:
                            address = "http://" + _anprCamList[i].address + "/Streaming/channels/1/picture";
                            break;
                        case Constants.MANUFACTURER.HANWHA_TECHWIN:
                            address = "http://" + _anprCamList[i].address + "/cgi-bin/video.cgi?msubmenu=jpg";
                            break;
                        case Constants.MANUFACTURER.HONEYWELL:
                            address = "http://" + _anprCamList[i].address + "/cgi-bin/webra_fcgi.fcgi?api=get_jpeg_raw&chno=1";
                            break;

                        case (int)Constants.MANUFACTURER.UNKNOWN:
                        default:
                            break;
                    }
                    //

                    imageImporter[i].InitCamera(_anprCamList[i].camid, address, setting.importInterval, _anprCamList[i].username, _anprCamList[i].password, setting.anprTimeout);
                    imageImporter[i].Play();
                }
                else
                {
                    anprClient[i] = new Client();
                    anprClient[i].InitConnection(setting.serverAddr);
                    anprClient[i].Play();
                }

                progress = (int)((float)((float)i / (float)_anprCamList.Count) * 100);
                playWorker.ReportProgress(progress);
            }
        }

        void MainForm_ANPRStatus(int channel, int eventstatus)
        {
            string statusMsg = "";
            switch (eventstatus)
            {
                case 0: default:
                    statusMsg = "";
                    break;
                case 1:
                    statusMsg = " [외부 파일 읽는 중]";
                    break;
                case 2:
                    statusMsg = " [외부 파일 차번 인식 중]";
                    break;
                case 3:
                    statusMsg = " [외부 파일 차번 인식 완료]";
                    break;
            }

            foreach (ANPRCam cam in _anprCamList)
            {
                if (cam.camid == channel)
                {
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        cam.node.Text = "카메라 " + cam.camid + " - " + cam.name + statusMsg;
                    }
                    ));
                }
            }



        }

        void playWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            popUp.progressBar.Value = e.ProgressPercentage;
        }

        void playWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            popUp.Close();

            button_PlayStop.Text = "끊기";

            button_camAdd.Enabled = false;
            button_camDel.Enabled = false;
            button_addFile.Enabled = false;
            button_SaveGlobalSetting.Enabled = false;

            playStatus = true;
        }

        void stopWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Stop();
        }

        void stopWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            popUp.progressBar.Value = e.ProgressPercentage;
        }

        void stopWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            popUp.Close();

            button_PlayStop.Text = "연결";

            button_camAdd.Enabled = true;
            button_camDel.Enabled = true;
            button_addFile.Enabled = true;
            button_SaveGlobalSetting.Enabled = true;

            playStatus = false;
        }

        void MainForm_ANPRDetected(int channel, DateTime dateTime, string plateStr, string imagePath)
        {
            this.BeginInvoke(new Action(() =>
            {
                //pictureBox_result.ImageLocation = logresults[3];
                //textBox_result.Text = "카메라 " + logresults[0] + " : " + findANPRCamName(Convert.ToInt32(logresults[0], 10)) + " 차량번호 : " + logresults[1];

                DateTime time = new DateTime();
                string[] itemStr = { channel.ToString(), dateTime.ToString(), plateStr };
                ListViewItem item = new ListViewItem(itemStr);
                listView_result.Items.Add(item);
                listView_result.Items[listView_result.Items.Count - 1].EnsureVisible();

                PLATE_FOUND found = new PLATE_FOUND();

                found.no = _plateListIdx;
                found.cam = channel;
                found.dateTime = dateTime;
                found.imageFilePath = imagePath;
                found.plateStr = plateStr;

                _plateList.Add(found);
                _plateListIdx++;

                foreach (ANPRCam cam in _anprCamList)
                {
                    int id = channel;
                    if (cam.camid == id)
                    {
                        cam.recognizedTime = DateTime.Now.Ticks;
                        cam.position = cam.markersOverlay.Markers[0].Position;
                        cam.markersOverlay.Markers.Clear();
                        GMarkerGoogle marker = new GMarkerGoogle(cam.position, GMarkerGoogleType.red);
                        cam.markersOverlay.Markers.Add(marker);
                    }
                }
            }));
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.Owner = this;
            searchWindow.Init();
            searchWindow.Show();
        }

        private void button_About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("GBU ANPR Monitor " + Application.ProductVersion + "\n" + "For Geoje City"
                + "\n\n" + "(C) 2017 GBU Datalinks Co. Ltd.");
        }

        private void button_Exit_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(@"taskkill", @"/f /im GBU_Server_DotNet*");
            popUp = new PopUp();
            popUp.SetMessage("연결 끊는 중...");
            stopWorker.RunWorkerAsync();
            popUp.ShowDialog();
            //gMapControl1.Dispose(); // freeze?
            Application.Exit();
        }

        private void button_SwitchMonitor_Click(object sender, EventArgs e)
        {
            if (Screen.AllScreens.Length > 1)
            {
                Screen myScreen = Screen.FromControl(this);
                Screen otherScreen = Screen.AllScreens.FirstOrDefault(s => !s.Equals(myScreen)) ?? myScreen;

                this.StartPosition = FormStartPosition.Manual;

                Point p = new Point();

                p.X = otherScreen.WorkingArea.Left;
                p.Y = otherScreen.WorkingArea.Top;

                this.Location = p;
            }

        }

        private void button_Configure_Click(object sender, EventArgs e)
        {
            ConfigureWindow configureWindow = new ConfigureWindow();
            configureWindow.Owner = this;
            configureWindow.Init();
            configureWindow.ShowDialog();
        }

        private void button_camAdd_Click(object sender, EventArgs e)
        {
            if (textBox_camID.Text.Length < 1 || textBox_name.Text.Length < 1)
            {
                MessageBox.Show("카메라 ID와 RTSP 주소를 확인해 주세요.");
                return;
            }

            // if camid exists, replace it
            foreach (ANPRCam existCam in _anprCamList)
            {
                if (existCam.camid == Convert.ToInt32(textBox_camID.Text, 10))
                {
                    // exist!
                    existCam.node.Text = "카메라 " + textBox_camID.Text + " - " + textBox_name.Text;
                    existCam.position = gMapControl1.Position;
                    existCam.markersOverlay.Markers[0].Position = existCam.position;
                    existCam.name = textBox_name.Text;
                    existCam.address = textBox_address.Text;
                    existCam.username = textBox_camUsername.Text;
                    existCam.password = textBox_camPassword.Text;
                    existCam.manufacturer = (Constants.MANUFACTURER)comboBox_manufacturer.SelectedIndex;
                    return;
                }
            }

            TreeNode camNode = new TreeNode("카메라 " + textBox_camID.Text + " - " + textBox_name.Text, 0, 0);
            treeView1.Nodes.Add(camNode);

            ANPRCam newcam = new ANPRCam();
            newcam.camid = Convert.ToInt32(textBox_camID.Text, 10);
            newcam.name = textBox_name.Text;
            newcam.address = textBox_address.Text;
            newcam.username = textBox_camUsername.Text;
            newcam.password = textBox_camPassword.Text;
            newcam.manufacturer = (Constants.MANUFACTURER)comboBox_manufacturer.SelectedIndex;
            newcam.node = camNode;
            newcam.markersOverlay = new GMapOverlay("markers");
            gMapControl1.Overlays.Add(newcam.markersOverlay);
            GMarkerGoogle marker = new GMarkerGoogle(gMapControl1.Position, GMarkerGoogleType.green);
            newcam.markersOverlay.Markers.Add(marker);
            newcam.position = gMapControl1.Position;

            _anprCamList.Add(newcam);
        }

        private void button_camDel_Click(object sender, EventArgs e)
        {
            if (_selectedNode != null)
            {
                ANPRCam targetCam = new ANPRCam();

                foreach (ANPRCam cam in _anprCamList)
                {
                    if (cam.node.Equals(_selectedNode))
                    {
                        targetCam = cam;
                        break;
                    }
                }

                if (targetCam.markersOverlay != null)
                {
                    _anprCamList.Remove(targetCam);
                    targetCam.markersOverlay.Markers.Clear();
                    gMapControl1.Overlays.Remove(targetCam.markersOverlay);
                    treeView1.Nodes.Remove(_selectedNode);
                }
            }
            else
            {
                MessageBox.Show("삭제할 카메라를 선택해 주세요.");
                return;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _selectedNode = treeView1.SelectedNode;

            ANPRCam targetCam = new ANPRCam();

            foreach (ANPRCam cam in _anprCamList)
            {
                if (cam.node.Equals(_selectedNode))
                {
                    targetCam = cam;
                    break;
                }
            }

            gMapControl1.Position = targetCam.position;

            // set values in setting UI
            textBox_camID.Text = targetCam.camid.ToString();
            textBox_name.Text = targetCam.name;
            textBox_address.Text = targetCam.address;
            textBox_camUsername.Text = targetCam.username;
            textBox_camPassword.Text = targetCam.password;
            comboBox_manufacturer.SelectedIndex = (int)targetCam.manufacturer;
            //
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            
        }

        private string findANPRCamName(int id)
        {
            string name = "";

            foreach (ANPRCam cam in _anprCamList)
            {
                if (id == cam.camid)
                {
                    name = cam.name;
                }
            }

            return name;
        }

        private void listView_result_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listView_result.FocusedItem.Index;
            string routeResult = "";

            pictureBox_result.ImageLocation = _plateList[index].imageFilePath;
            textBox_result.Text = "카메라 " + _plateList[index].cam + " : " + findANPRCamName(_plateList[index].cam) + " 차량번호 : " + _plateList[index].plateStr;

            // 경로추적
            List<Route> routeList = new List<Route>();
            traceRoute(_plateList[index].plateStr, ref routeList);

            foreach (Route route in routeList)
            {
                routeResult += route.anprCamObj.name + " [" + route.dateTime.ToString() + "] -> ";
            }

            textBox_traceRoute.Text = routeResult;
        }

        private void traceRoute(string plateStr, ref List<Route> routeList)
        {
            foreach (PLATE_FOUND found in _plateList)
            {
                foreach (ANPRCam cam in _anprCamList)
                {
                    if (plateStr.Equals(found.plateStr) && cam.camid == found.cam)
                    {
                        Route route = new Route();
                        route.camid = found.cam;
                        route.dateTime = found.dateTime;
                        route.plate = found.plateStr;
                        route.anprCamObj = cam;
                        routeList.Add(route);
                    }
                }
            }
        }

        private void button_Statictics_Click(object sender, EventArgs e)
        {
            StatisticsWindow statisticsWindow = new StatisticsWindow();
            statisticsWindow.Owner = this;
            statisticsWindow.Init();
            statisticsWindow.ShowDialog();
        }

        // save ANPRCam, Setting obj
        private void saveGlobalSetting()
        {
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.ConnectionStrings["aesPassword"].ConnectionString);
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;

            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CBC;

            var settingContainer = new SettingContainer();
            settingContainer.globalSetting = setting;
            settingContainer.camList = _anprCamList;

            try
            {
                BinaryFormatter binFmt = new BinaryFormatter();
                using (FileStream fs = new FileStream("setting_global.dat", FileMode.Create))
                {
                    using (CryptoStream cs = new CryptoStream(fs, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        binFmt.Serialize(cs, settingContainer);
                        cs.Close();
                    }
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("setting save error : " + e.Message);
            }
        }

        // load ANPRCam, Setting obj
        private void loadGlobalSetting()
        {
            if (!File.Exists("setting_global.dat"))
            {
                return;
            }

            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.ConnectionStrings["aesPassword"].ConnectionString);
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;

            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CBC;

            var settingContainer = new SettingContainer();

            try
            {
                BinaryFormatter binFmt = new BinaryFormatter();
                using (FileStream rdr = new FileStream("setting_global.dat", FileMode.Open))
                {
                    using (CryptoStream cs = new CryptoStream(rdr, AES.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        settingContainer = (SettingContainer)binFmt.Deserialize(cs);
                        setting = settingContainer.globalSetting;
                        _anprCamList = settingContainer.camList;
                        cs.Close();
                    }
                    rdr.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Setting file open error : " + e.Message);
            }

            foreach (ANPRCam cam in _anprCamList)
            {
                treeView1.Nodes.Add(cam.node);
                gMapControl1.Overlays.Add(cam.markersOverlay);
            }
        }

        private void button_SaveGlobalSetting_Click(object sender, EventArgs e)
        {
            saveGlobalSetting();
        }

        private void button_Minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox_result_Click(object sender, EventArgs e)
        {
            
        }

        private void button_addFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "AVI|*.avi|MP4|*.mp4|*.MOV|*.mov";
            fileDialog.Title = "설정 파일 열기";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = fileDialog.FileName;
                textBox_address.Text = path;
            }
        }

        private void pictureBox_result_DoubleClick(object sender, EventArgs e)
        {
            if (pictureBox_result.ImageLocation != null)
            {
                ProcessStartInfo _processStartInfo = new ProcessStartInfo();
                _processStartInfo.WorkingDirectory = @"%SystemRoot%\System32\";
                _processStartInfo.FileName = @"rundll32.exe";
                _processStartInfo.Arguments = "\"" + @"C:\Program Files\Windows Photo Viewer\PhotoViewer.dll" + "\"" + ", ImageView_Fullscreen " + Path.GetFullPath(pictureBox_result.ImageLocation);
                _processStartInfo.CreateNoWindow = false;
                Process myProcess = Process.Start(_processStartInfo);
            }
        }

        private void comboBox_manufacturer_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox_manufacturer.SelectedIndex)
            {
                case (int)Constants.MANUFACTURER.AXIS:
                    
                    break;
                case (int)Constants.MANUFACTURER.HIKVISION:

                    break;
                case (int)Constants.MANUFACTURER.HANWHA_TECHWIN:

                    break;
                case (int)Constants.MANUFACTURER.HONEYWELL:

                    break;
                case (int)Constants.MANUFACTURER.UNKNOWN:

                default:
                    break;
            }
            
        }
    }
}
