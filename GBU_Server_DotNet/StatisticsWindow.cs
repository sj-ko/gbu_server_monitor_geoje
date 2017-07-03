using System;
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
    public partial class StatisticsWindow : Form
    {
        private Database dbManager;
        private int _camCount = 0;

        private List<DataTable> _resultTableList;

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

        private string _selectedPlateImageLocation;

        public StatisticsWindow()
        {
            InitializeComponent();
        }

        public void Init()
        {
            MainForm form = (MainForm)this.Owner;

            listView_Result.View = View.Details;
            listView_Result.FullRowSelect = true;
            listView_Result.GridLines = true;

            listView_Result.Columns.Add("카메라", 150, HorizontalAlignment.Left);
            listView_Result.Columns.Add("통과대수", 60, HorizontalAlignment.Left);

            listView_CarList.View = View.Details;
            listView_CarList.FullRowSelect = true;
            listView_CarList.GridLines = true;

            listView_CarList.Columns.Add("카메라", 90, HorizontalAlignment.Left);
            listView_CarList.Columns.Add("시간", 170, HorizontalAlignment.Left);
            listView_CarList.Columns.Add("차량번호", 100, HorizontalAlignment.Left);

            _camCount = form._anprCamList.Count;

            comboBox_Channel.Items.Add("전체");
            for (int i = 0; i < _camCount; i++ )
            {
                comboBox_Channel.Items.Add(form._anprCamList[i].camid + " - " + form._anprCamList[i].name);
            }

            comboBox_Channel.SelectedIndex = 0;

            dbManager = form.dbManager;

            _resultTableList = new List<DataTable>();

            if (checkBox_searchAll.Checked)
            {
                textBox_search.Enabled = false;
            }
            else
            {
                textBox_search.Enabled = true;
            }
        }

        private void comboBox_Channel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void StatisticsWindow_Load(object sender, EventArgs e)
        {
            dateTimePicker2.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dateTimePicker4.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            MainForm form = (MainForm)this.Owner;
            DateTime targetDateTime = dateTimePicker1.Value;

            DateTime startDate = dateTimePicker1.Value;
            DateTime startTime = dateTimePicker2.Value;
            DateTime endDate = dateTimePicker3.Value;
            DateTime endTime = dateTimePicker4.Value;

            // check date range
            DateTime startDT = startDate.Date + startTime.TimeOfDay;
            DateTime endDT = endDate.Date + endTime.TimeOfDay;
            if ((endDT - startDT).Ticks > new TimeSpan(30, 0, 0, 0).Ticks)
            {
                MessageBox.Show("검색 범위는 최대 30일 입니다.");
                return;
            }
            // 

            DataTable result = new DataTable();
            int ch = comboBox_Channel.SelectedIndex;

            listView_Result.Items.Clear();

            _resultTableList.Clear();

            // single channel search
            if (ch != 0)
            {
                string target = comboBox_Channel.GetItemText(comboBox_Channel.Items[ch]);
                string[] targetArr = target.Split(' ');
                int camid = Convert.ToInt32(targetArr[0], 10);
                //dbManager.SearchPlateByDate(camid, targetDateTime, ref result);
                if (checkBox_searchAll.Checked)
                {
                    dbManager.SearchPlateByRange(camid, startDate, startTime, endDate, endTime, ref result);
                }
                else
                {
                    dbManager.SearchPlateByRange(camid, textBox_search.Text, startDate, startTime, endDate, endTime, ref result);
                }

                string[] itemStr = { target, Convert.ToString(result.Rows.Count) };
                ListViewItem item = new ListViewItem(itemStr);
                listView_Result.Items.Add(item);
                _resultTableList.Add(result);
            }
            // entire search
            else
            {
                for (int i = 1; i < comboBox_Channel.Items.Count; i++)
                {
                    string target = comboBox_Channel.GetItemText(comboBox_Channel.Items[i]);
                    string[] targetArr = target.Split(' ');
                    int camid = Convert.ToInt32(targetArr[0], 10);
                    //dbManager.SearchPlateByDate(camid, targetDateTime, ref result);
                    if (checkBox_searchAll.Checked)
                    {
                        dbManager.SearchPlateByRange(camid, startDate, startTime, endDate, endTime, ref result);
                    }
                    else
                    {
                        dbManager.SearchPlateByRange(camid, textBox_search.Text, startDate, startTime, endDate, endTime, ref result);
                    }

                    string[] itemStr = { target, Convert.ToString(result.Rows.Count) };
                    ListViewItem item = new ListViewItem(itemStr);
                    listView_Result.Items.Add(item);
                    _resultTableList.Add(result);
                }
            }

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value >= dateTimePicker3.Value)
            {
                dateTimePicker3.Value = dateTimePicker1.Value;
                if (dateTimePicker2.Value >= dateTimePicker4.Value)
                {
                    dateTimePicker2.Value = dateTimePicker4.Value;
                }
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value == dateTimePicker3.Value)
            {
                if (dateTimePicker2.Value >= dateTimePicker4.Value)
                {
                    dateTimePicker2.Value = dateTimePicker4.Value;
                }
            }
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker3.Value <= dateTimePicker1.Value)
            {
                dateTimePicker1.Value = dateTimePicker3.Value;
                if (dateTimePicker4.Value <= dateTimePicker2.Value)
                {
                    dateTimePicker4.Value = dateTimePicker2.Value;
                }
            }
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value == dateTimePicker3.Value)
            {
                if (dateTimePicker4.Value <= dateTimePicker2.Value)
                {
                    dateTimePicker4.Value = dateTimePicker2.Value;
                }
            }
        }

        private void listView_Result_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listView_Result.FocusedItem.Index;

            listView_CarList.Items.Clear();

            _plateList.Clear();
            _plateListIdx = 0;

            if (_resultTableList[index] != null)
            {
                foreach (DataRow dr in _resultTableList[index].Rows)
                {
                    DateTime myDateTime = DateTime.ParseExact(dr["ANPRDATE"].ToString().Substring(0, 10) + " " + dr["ANPRTIME"].ToString(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    string[] itemStr = { Convert.ToString(dr["CAMID"]), myDateTime.ToString(), Convert.ToString(dr["PLATE"]) };
                    ListViewItem item = new ListViewItem(itemStr);
                    listView_CarList.Items.Add(item);

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

        private void listView_CarList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listView_CarList.FocusedItem.Index;
            _selectedPlateImageLocation = _plateList[index].imageFilePath;
        }

        private void listView_CarList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView_CarList.SelectedItems.Count == 1)
            {
                //MessageBox.Show(_selectedPlateImageLocation);
                ProcessStartInfo _processStartInfo = new ProcessStartInfo();
                _processStartInfo.WorkingDirectory = @"%SystemRoot%\System32\";
                _processStartInfo.FileName = @"rundll32.exe";
                _processStartInfo.Arguments = "\"" + @"C:\Program Files\Windows Photo Viewer\PhotoViewer.dll" + "\"" + ", ImageView_Fullscreen " + Path.GetFullPath(_selectedPlateImageLocation);
                _processStartInfo.CreateNoWindow = false;
                Process myProcess = Process.Start(_processStartInfo);
            }
        }

        private void checkBox_searchAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_searchAll.Checked)
            {
                textBox_search.Enabled = false;
            }
            else
            {
                textBox_search.Enabled = true;
            }
        }


    }
}
