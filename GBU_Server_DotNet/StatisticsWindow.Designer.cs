namespace GBU_Server_Monitor
{
    partial class StatisticsWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.comboBox_Channel = new System.Windows.Forms.ComboBox();
            this.listView_Result = new System.Windows.Forms.ListView();
            this.button_Search = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.listView_CarList = new System.Windows.Forms.ListView();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_search = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBox_searchAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "카메라";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "시작 날짜";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(101, 80);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(246, 21);
            this.dateTimePicker1.TabIndex = 2;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // comboBox_Channel
            // 
            this.comboBox_Channel.FormattingEnabled = true;
            this.comboBox_Channel.Location = new System.Drawing.Point(101, 15);
            this.comboBox_Channel.Name = "comboBox_Channel";
            this.comboBox_Channel.Size = new System.Drawing.Size(121, 20);
            this.comboBox_Channel.TabIndex = 3;
            this.comboBox_Channel.SelectedIndexChanged += new System.EventHandler(this.comboBox_Channel_SelectedIndexChanged);
            // 
            // listView_Result
            // 
            this.listView_Result.Location = new System.Drawing.Point(12, 247);
            this.listView_Result.Name = "listView_Result";
            this.listView_Result.Size = new System.Drawing.Size(336, 214);
            this.listView_Result.TabIndex = 4;
            this.listView_Result.UseCompatibleStateImageBehavior = false;
            this.listView_Result.SelectedIndexChanged += new System.EventHandler(this.listView_Result_SelectedIndexChanged);
            // 
            // button_Search
            // 
            this.button_Search.Location = new System.Drawing.Point(236, 190);
            this.button_Search.Name = "button_Search";
            this.button_Search.Size = new System.Drawing.Size(112, 23);
            this.button_Search.TabIndex = 5;
            this.button_Search.Text = "검색";
            this.button_Search.UseVisualStyleBackColor = true;
            this.button_Search.Click += new System.EventHandler(this.button_Search_Click);
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(236, 467);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(112, 23);
            this.button_OK.TabIndex = 6;
            this.button_OK.Text = "확인";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "시작 시간";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker2.Location = new System.Drawing.Point(101, 107);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.ShowUpDown = true;
            this.dateTimePicker2.Size = new System.Drawing.Size(246, 21);
            this.dateTimePicker2.TabIndex = 8;
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.Location = new System.Drawing.Point(101, 134);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(246, 21);
            this.dateTimePicker3.TabIndex = 9;
            this.dateTimePicker3.ValueChanged += new System.EventHandler(this.dateTimePicker3_ValueChanged);
            // 
            // dateTimePicker4
            // 
            this.dateTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker4.Location = new System.Drawing.Point(101, 161);
            this.dateTimePicker4.Name = "dateTimePicker4";
            this.dateTimePicker4.ShowUpDown = true;
            this.dateTimePicker4.Size = new System.Drawing.Size(246, 21);
            this.dateTimePicker4.TabIndex = 10;
            this.dateTimePicker4.ValueChanged += new System.EventHandler(this.dateTimePicker4_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "끝 날짜";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "끝 시간";
            // 
            // listView_CarList
            // 
            this.listView_CarList.Location = new System.Drawing.Point(385, 33);
            this.listView_CarList.Name = "listView_CarList";
            this.listView_CarList.Size = new System.Drawing.Size(554, 428);
            this.listView_CarList.TabIndex = 13;
            this.listView_CarList.UseCompatibleStateImageBehavior = false;
            this.listView_CarList.SelectedIndexChanged += new System.EventHandler(this.listView_CarList_SelectedIndexChanged);
            this.listView_CarList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_CarList_MouseDoubleClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(383, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "차량 목록";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(41, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "차량번호";
            // 
            // textBox_search
            // 
            this.textBox_search.Location = new System.Drawing.Point(100, 43);
            this.textBox_search.Name = "textBox_search";
            this.textBox_search.Size = new System.Drawing.Size(122, 21);
            this.textBox_search.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 232);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "카메라 별 인식 현황";
            // 
            // checkBox_searchAll
            // 
            this.checkBox_searchAll.AutoSize = true;
            this.checkBox_searchAll.Checked = true;
            this.checkBox_searchAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_searchAll.Location = new System.Drawing.Point(236, 45);
            this.checkBox_searchAll.Name = "checkBox_searchAll";
            this.checkBox_searchAll.Size = new System.Drawing.Size(104, 16);
            this.checkBox_searchAll.TabIndex = 18;
            this.checkBox_searchAll.Text = "전체 차량 검색";
            this.checkBox_searchAll.UseVisualStyleBackColor = true;
            this.checkBox_searchAll.CheckedChanged += new System.EventHandler(this.checkBox_searchAll_CheckedChanged);
            // 
            // StatisticsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(951, 502);
            this.Controls.Add(this.checkBox_searchAll);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox_search);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.listView_CarList);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateTimePicker4);
            this.Controls.Add(this.dateTimePicker3);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.button_Search);
            this.Controls.Add(this.listView_Result);
            this.Controls.Add(this.comboBox_Channel);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StatisticsWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "통계";
            this.Load += new System.EventHandler(this.StatisticsWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox comboBox_Channel;
        private System.Windows.Forms.ListView listView_Result;
        private System.Windows.Forms.Button button_Search;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private System.Windows.Forms.DateTimePicker dateTimePicker4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView listView_CarList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_search;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBox_searchAll;
    }
}