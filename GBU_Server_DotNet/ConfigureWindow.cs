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
    public partial class ConfigureWindow : Form
    {
        public ConfigureWindow()
        {
            InitializeComponent();
        }

        public void Init()
        {
            MainForm form = (MainForm)this.Owner;
            Configure_textbox_savepath.Text = form.setting.savePath;
            Configure_textbox_serverPath.Text = form.setting.serverPath;
            Configure_textbox_configPath.Text = form.setting.configPath;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            MainForm form = (MainForm)this.Owner;
            form.setting.savePath = Configure_textbox_savepath.Text;
            form.setting.serverPath = Configure_textbox_serverPath.Text;
            form.setting.configPath = Configure_textbox_configPath.Text;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
