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

            comboBox_Channel.SelectedIndex = 0;

            dbManager = form.dbManager;
        }

        private void Search_button_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Search_button_search_Click(object sender, EventArgs e)
        {
            DataTable result = new DataTable();

            //DataTable DBresult = new DataTable();

            _plateList.Clear();
            _plateListIdx = 0;

            Search_listView1.Items.Clear();
            
            // result from DB
            //dbManager.SearchPlate(search_textBox_search.Text, ref DBresult);
            // result from local file
            dbManager.SearchPlateForFile(comboBox_Channel.SelectedIndex - 1 ,search_textBox_search.Text, ref result);

            // read DB result
            //foreach (DataRow dr in DBresult.Rows)
            //{
            //   Console.WriteLine(Convert.ToString(dr["CARL_YMDHNS"]) + " , " + Convert.ToString(dr["CARL_CARNO"]).ToString() + " , " + Convert.ToString(dr["CARL_OK"]) + " , " + Convert.ToString(dr["CARL_CID"]));
            //}

            // read local file result and add list
            foreach (DataRow dr in result.Rows)
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
            }

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



    }
}
