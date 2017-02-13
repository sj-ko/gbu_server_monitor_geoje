using System;
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

namespace GBU_Server_Monitor
{
    public partial class MainForm : Form
    {
        public Setting setting;

        private const int NUM_OF_CAM = 100; //5;

        private System.Threading.Timer timer;
        private AutoResetEvent timerEvent;

        private FileSystemWatcher[] watcher = new FileSystemWatcher[NUM_OF_CAM];

        private bool playStatus = false;

        public class PLATE_FOUND
        {
            public int no;
            public int cam;
            public DateTime dateTime;
            public string plateStr;
            public string imageFilePath;
        };

        public class ANPRCam
        {
            public int camid;
            public string name;
            //public string rtspUrl;
            //public double latitude;
            //public double longitude;
            public TreeNode node;
            public GMapOverlay markersOverlay;
            public long recognizedTime { get; set; }
            public PointLatLng position { get; set; }
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

            listView_result.View = View.Details;
            listView_result.FullRowSelect = true;
            listView_result.GridLines = true;

            listView_result.Columns.Add("카메라", 50, HorizontalAlignment.Left);
            listView_result.Columns.Add("시간", 100, HorizontalAlignment.Left);
            listView_result.Columns.Add("차량번호", 100, HorizontalAlignment.Left);

            InitCamera();

            dbManager.SavePath = setting.savePath;

            // Initialize map:
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gMapControl1.Position = new PointLatLng(34.8544319, 128.6288147);
            gMapControl1.CanDragMap = true;
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.CacheLocation = Environment.CurrentDirectory + @"\GMapCache";
        }

        private void Btn_Disconnect_Click(object sender, EventArgs e)
        {
            
        }

        private void InitCamera()
        {
            setting = new Setting();
            setting.PropertyChanged += camera_PropertyChanged;
        }

        private void camera_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //UpdateFormUIValue();

            dbManager.SavePath = setting.savePath;
        }

        private void Stop()
        {
            if (timer != null)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite); // stop timer
                timer.Dispose();
                timer = null;
            }

            for (int i = 0; i < NUM_OF_CAM; i++)
            {
                if (watcher[i] != null)
                {
                    watcher[i].EnableRaisingEvents = false;
                    watcher[i].Dispose();
                    watcher[i] = null;
                }
            }

            //_plateListIdx = 0;
            //_plateList.Clear();
        }

        private void MediaTimerCallBack(Object obj)
        {
            this.BeginInvoke(new Action(() =>
                {
                    foreach (ANPRCam cam in _anprCamList)
                    {
                        if ((DateTime.Now.Ticks - cam.recognizedTime) / 10000f > 3000f && cam.recognizedTime > 0)
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
                System.Diagnostics.Process.Start(@"taskkill", @"/f /im GBU_Server_DotNet*");
                Stop();

                button_PlayStop.Text = "연결";

                button_camAdd.Enabled = true;
                button_camDel.Enabled = true;

                playStatus = false;
            }
            else
            {
                for (int i = 0; i < _anprCamList.Count; i++)
                {
                    System.Diagnostics.Process.Start(setting.serverPath + @"\GBU_Server_DotNet.exe", setting.configPath + @"\ch" + _anprCamList[i].camid + ".cfg --autostart --enlarge");
                }

                timerEvent = new AutoResetEvent(true);
                timer = new System.Threading.Timer(MediaTimerCallBack, null, 100, 200);


                for (int i = 0; i < NUM_OF_CAM; i++)
                {
                    watcher[i] = new FileSystemWatcher();
                    string path = setting.savePath + "\\ch" + i;
                    if (Directory.Exists(path))
                    {
                        watcher[i].Path = path;
                        watcher[i].NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                        watcher[i].Filter = "anprresult.txt";
                        watcher[i].Changed += new FileSystemEventHandler(OnChangedANPRPath);
                        watcher[i].EnableRaisingEvents = true;
                    }
                }

                button_PlayStop.Text = "끊기";

                button_camAdd.Enabled = false;
                button_camDel.Enabled = false;

                playStatus = true;
            }
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
            System.Diagnostics.Process.Start(@"taskkill", @"/f /im GBU_Server_DotNet*");
            Stop();
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

            TreeNode camNode = new TreeNode("카메라 " + textBox_camID.Text + " - " + textBox_name.Text, 0, 0);
            treeView1.Nodes.Add(camNode);

            ANPRCam newcam = new ANPRCam();
            newcam.camid = Convert.ToInt32(textBox_camID.Text, 10);
            newcam.name = textBox_name.Text;
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

                _anprCamList.Remove(targetCam);
                targetCam.markersOverlay.Markers.Clear();
                gMapControl1.Overlays.Remove(targetCam.markersOverlay);
                treeView1.Nodes.Remove(_selectedNode);
                
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

    }
}
