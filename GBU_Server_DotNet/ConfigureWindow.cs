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
            Configure_textbox_anprtimeout.Text = Convert.ToString(form.setting.anprTimeout, 10);
            Configure_textbox_importinterval.Text = Convert.ToString(form.setting.importInterval, 10);
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            MainForm form = (MainForm)this.Owner;
            form.setting.savePath = Configure_textbox_savepath.Text;
            form.setting.anprTimeout = Convert.ToInt32(Configure_textbox_anprtimeout.Text);
            form.setting.importInterval = Convert.ToInt32(Configure_textbox_importinterval.Text);
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
