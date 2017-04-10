using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GBU_Server_Monitor
{
    public partial class StatisticsWindow : Form
    {
        private Database dbManager;
        private int _camCount = 0;

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

            _camCount = form._anprCamList.Count;

            comboBox_Channel.Items.Add("전체");
            for (int i = 0; i < _camCount; i++ )
            {
                comboBox_Channel.Items.Add(form._anprCamList[i].camid + " - " + form._anprCamList[i].name);
            }

            comboBox_Channel.SelectedIndex = 0;

            dbManager = form.dbManager;
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

            DataTable result = new DataTable();
            int ch = comboBox_Channel.SelectedIndex;

            listView_Result.Items.Clear();

            // single channel search
            if (ch != 0)
            {
                string target = comboBox_Channel.GetItemText(comboBox_Channel.Items[ch]);
                string[] targetArr = target.Split(' ');
                int camid = Convert.ToInt32(targetArr[0], 10);
                //dbManager.SearchPlateByDate(camid, targetDateTime, ref result);
                dbManager.SearchPlateByRange(camid, startDate, startTime, endDate, endTime, ref result);

                string[] itemStr = { target, Convert.ToString(result.Rows.Count) };
                ListViewItem item = new ListViewItem(itemStr);
                listView_Result.Items.Add(item);
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
                    dbManager.SearchPlateByRange(camid, startDate, startTime, endDate, endTime, ref result);

                    string[] itemStr = { target, Convert.ToString(result.Rows.Count) };
                    ListViewItem item = new ListViewItem(itemStr);
                    listView_Result.Items.Add(item);
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

        }


    }
}
