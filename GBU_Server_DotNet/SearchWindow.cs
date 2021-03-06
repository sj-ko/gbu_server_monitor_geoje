﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace GBU_Server_Monitor
{
    public partial class SearchWindow : Form
    {
        private Database dbManager;

        public struct PLATE_FOUND
        {
            public int id;
            public int cam;
            public DateTime dateTime;
            public string plateStr;
            public string imageFilePath;
            //public Image snapshot;
        };

        private List<PLATE_FOUND> _plateList = new List<PLATE_FOUND>();
        private int _plateListIdx = 0;
        private int _camCount = 0;

        public SearchWindow()
        {
            InitializeComponent();
        }

        public void Init()
        {
            MainForm form = (MainForm)this.Owner;

            Search_listView1.View = View.Details;
            Search_listView1.FullRowSelect = true;
            Search_listView1.GridLines = true;

            Search_listView1.Columns.Add("카메라", 90, HorizontalAlignment.Left);
            Search_listView1.Columns.Add("시간", 170, HorizontalAlignment.Left);
            Search_listView1.Columns.Add("차량번호", 100, HorizontalAlignment.Left);

            _camCount = form._anprCamList.Count;

            comboBox_Channel.Items.Add("전체");
            for (int i = 0; i < _camCount; i++)
            {
                comboBox_Channel.Items.Add(form._anprCamList[i].camid + " - " + form._anprCamList[i].name);
            }

            comboBox_Channel.SelectedIndex = 0;

            dbManager = form.dbManager;
        }

        private void Search_button_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Search_button_search_Click(object sender, EventArgs e)
        {
            //DataTable result = new DataTable();

            DataTable DBresult = new DataTable();

            _plateList.Clear();
            _plateListIdx = 0;

            Search_listView1.Items.Clear();

            int ch = comboBox_Channel.SelectedIndex;

            // single channel search
            if (ch != 0)
            {
                string target = comboBox_Channel.GetItemText(comboBox_Channel.Items[ch]);
                string[] targetArr = target.Split(' ');
                int camid = Convert.ToInt32(targetArr[0], 10);

                // result from DB
                dbManager.SearchPlate(camid, search_textBox_search.Text, ref DBresult);

                // read DB result CAMID, ANPRDATE, ANPRTIME, PLATE, IMAGEPATH
                foreach (DataRow dr in DBresult.Rows)
                {
                    DateTime myDateTime = DateTime.ParseExact(dr["ANPRDATE"].ToString().Substring(0, 10) + " " + dr["ANPRTIME"].ToString(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    string[] itemStr = { Convert.ToString(dr["CAMID"]), myDateTime.ToString(), Convert.ToString(dr["PLATE"]) };
                    ListViewItem item = new ListViewItem(itemStr);
                    Search_listView1.Items.Add(item);

                    PLATE_FOUND plate = new PLATE_FOUND();
                    plate.cam = Convert.ToInt32(dr["CAMID"]);
                    plate.dateTime = myDateTime;
                    plate.id = _plateListIdx;
                    plate.plateStr = Convert.ToString(dr["PLATE"]);
                    plate.imageFilePath = Convert.ToString(dr["IMAGEPATH"]);

                    _plateList.Add(plate);
                    _plateListIdx++;
                }
            }
            // entire search
            else
            {
                for (int i = 1; i < comboBox_Channel.Items.Count; i++)
                {
                    string target = comboBox_Channel.GetItemText(comboBox_Channel.Items[i]);
                    string[] targetArr = target.Split(' ');
                    int camid = Convert.ToInt32(targetArr[0], 10);

                    // result from DB
                    dbManager.SearchPlate(camid, search_textBox_search.Text, ref DBresult);

                    // read DB result CAMID, ANPRDATE, ANPRTIME, PLATE, IMAGEPATH
                    foreach (DataRow dr in DBresult.Rows)
                    {
                        DateTime myDateTime = DateTime.ParseExact(dr["ANPRDATE"].ToString().Substring(0, 10) + " " + dr["ANPRTIME"].ToString(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        string[] itemStr = { Convert.ToString(dr["CAMID"]), myDateTime.ToString(), Convert.ToString(dr["PLATE"]) };
                        ListViewItem item = new ListViewItem(itemStr);
                        Search_listView1.Items.Add(item);

                        PLATE_FOUND plate = new PLATE_FOUND();
                        plate.cam = Convert.ToInt32(dr["CAMID"]);
                        plate.dateTime = myDateTime;
                        plate.id = _plateListIdx;
                        plate.plateStr = Convert.ToString(dr["PLATE"]);
                        plate.imageFilePath = Convert.ToString(dr["IMAGEPATH"]);

                        _plateList.Add(plate);
                        _plateListIdx++;
                    }
                }
            }
            // result from local file
            //dbManager.SearchPlateForFile(comboBox_Channel.SelectedIndex - 1 ,search_textBox_search.Text, ref result);

            // read local file result and add list
            /*foreach (DataRow dr in result.Rows)
            {
                string[] itemStr = { Convert.ToString(dr["camId"]), Convert.ToDateTime(dr["dateTime"]).ToString(), Convert.ToString(dr["plate"]) };
                ListViewItem item = new ListViewItem(itemStr);
                Search_listView1.Items.Add(item);

                PLATE_FOUND plate = new PLATE_FOUND();
                plate.cam = Convert.ToInt32(dr["camId"]);
                plate.dateTime = Convert.ToDateTime(dr["dateTime"]);
                plate.id = _plateListIdx;
                plate.plateStr = Convert.ToString(dr["plate"]);
                plate.imageFilePath = Convert.ToString(dr["imageFilePath"]);
                //plate.snapshot = byteArrayToImage((byte[])dr["image"]);


                _plateList.Add(plate);
                _plateListIdx++;
            }*/

        }

        private void search_textBox_search_TextChanged(object sender, EventArgs e)
        {
            // to be added...
        }

        private void Search_listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainForm form = (MainForm)this.Owner;

            int index = Search_listView1.FocusedItem.Index;
            pictureBox_searchImage.ImageLocation = _plateList[index].imageFilePath;
            
            string plate = _plateList[index].plateStr;

            foreach (ListViewItem item in form.listView_result.Items)
            {
                if (item.Text.Contains(plate))
                {
                    form.listView_result.EnsureVisible(item.Index);
                }
            }
            
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private void comboBox_Channel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void search_textBox_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search_button_search_Click(sender, e);
            }
        }

        private void pictureBox_searchImage_DoubleClick(object sender, EventArgs e)
        {
            // %SystemRoot%\System32\rundll32.exe "%ProgramFiles%\Windows Photo Viewer\PhotoViewer.dll", ImageView_Fullscreen %1 

            //string strCmdText;
            //strCmdText = "\"%ProgramFiles%\\Windows Photo Viewer\\PhotoViewer.dll\", ImageView_Fullscreen " + pictureBox_searchImage.ImageLocation;
            //System.Diagnostics.Process.Start("%SystemRoot%\\System32\\rundll32.exe", strCmdText);

            ProcessStartInfo _processStartInfo = new ProcessStartInfo();
            _processStartInfo.WorkingDirectory = @"%SystemRoot%\System32\";
            _processStartInfo.FileName = @"rundll32.exe";
            _processStartInfo.Arguments = "\"" + @"C:\Program Files\Windows Photo Viewer\PhotoViewer.dll" + "\"" + ", ImageView_Fullscreen " + Path.GetFullPath(pictureBox_searchImage.ImageLocation);
            _processStartInfo.CreateNoWindow = false;
            Process myProcess = Process.Start(_processStartInfo);
        }



    }
}
