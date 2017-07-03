namespace GBU_Server_Monitor
{
    partial class MainForm
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
            this.button_PlayStop = new System.Windows.Forms.Button();
            this.button_About = new System.Windows.Forms.Button();
            this.button_Exit = new System.Windows.Forms.Button();
            this.listView_result = new System.Windows.Forms.ListView();
            this.pictureBox21 = new System.Windows.Forms.PictureBox();
            this.button_SwitchMonitor = new System.Windows.Forms.Button();
            this.button_Configure = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.textBox_camID = new System.Windows.Forms.TextBox();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.button_camAdd = new System.Windows.Forms.Button();
            this.button_camDel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox_result = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_traceRoute = new System.Windows.Forms.TextBox();
            this.button_Statictics = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_address = new System.Windows.Forms.TextBox();
            this.textBox_camUsername = new System.Windows.Forms.TextBox();
            this.textBox_camPassword = new System.Windows.Forms.TextBox();
            this.button_SaveGlobalSetting = new System.Windows.Forms.Button();
            this.button_Minimize = new System.Windows.Forms.Button();
            this.button_addFile = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox_manufacturer = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_result)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_PlayStop
            // 
            this.button_PlayStop.Location = new System.Drawing.Point(13, 12);
            this.button_PlayStop.Name = "button_PlayStop";
            this.button_PlayStop.Size = new System.Drawing.Size(75, 36);
            this.button_PlayStop.TabIndex = 47;
            this.button_PlayStop.Text = "연결";
            this.button_PlayStop.UseVisualStyleBackColor = true;
            this.button_PlayStop.Click += new System.EventHandler(this.button_PlayStop_Click);
            // 
            // button_About
            // 
            this.button_About.Location = new System.Drawing.Point(1747, 12);
            this.button_About.Name = "button_About";
            this.button_About.Size = new System.Drawing.Size(75, 36);
            this.button_About.TabIndex = 49;
            this.button_About.Text = "정보";
            this.button_About.UseVisualStyleBackColor = true;
            this.button_About.Click += new System.EventHandler(this.button_About_Click);
            // 
            // button_Exit
            // 
            this.button_Exit.Location = new System.Drawing.Point(1828, 12);
            this.button_Exit.Name = "button_Exit";
            this.button_Exit.Size = new System.Drawing.Size(75, 36);
            this.button_Exit.TabIndex = 50;
            this.button_Exit.Text = "종료";
            this.button_Exit.UseVisualStyleBackColor = true;
            this.button_Exit.Click += new System.EventHandler(this.button_Exit_Click);
            // 
            // listView_result
            // 
            this.listView_result.Location = new System.Drawing.Point(1511, 81);
            this.listView_result.Name = "listView_result";
            this.listView_result.ShowItemToolTips = true;
            this.listView_result.Size = new System.Drawing.Size(392, 800);
            this.listView_result.TabIndex = 51;
            this.listView_result.UseCompatibleStateImageBehavior = false;
            this.listView_result.SelectedIndexChanged += new System.EventHandler(this.listView_result_SelectedIndexChanged);
            // 
            // pictureBox21
            // 
            this.pictureBox21.ImageLocation = "gbudatalinks_logo.png";
            this.pictureBox21.Location = new System.Drawing.Point(12, 916);
            this.pictureBox21.Name = "pictureBox21";
            this.pictureBox21.Size = new System.Drawing.Size(123, 96);
            this.pictureBox21.TabIndex = 52;
            this.pictureBox21.TabStop = false;
            // 
            // button_SwitchMonitor
            // 
            this.button_SwitchMonitor.Location = new System.Drawing.Point(208, 12);
            this.button_SwitchMonitor.Name = "button_SwitchMonitor";
            this.button_SwitchMonitor.Size = new System.Drawing.Size(75, 36);
            this.button_SwitchMonitor.TabIndex = 73;
            this.button_SwitchMonitor.Text = "모니터전환";
            this.button_SwitchMonitor.UseVisualStyleBackColor = true;
            this.button_SwitchMonitor.Click += new System.EventHandler(this.button_SwitchMonitor_Click);
            // 
            // button_Configure
            // 
            this.button_Configure.Location = new System.Drawing.Point(289, 12);
            this.button_Configure.Name = "button_Configure";
            this.button_Configure.Size = new System.Drawing.Size(75, 36);
            this.button_Configure.TabIndex = 74;
            this.button_Configure.Text = "설정";
            this.button_Configure.UseVisualStyleBackColor = true;
            this.button_Configure.Click += new System.EventHandler(this.button_Configure_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(13, 81);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(426, 672);
            this.treeView1.TabIndex = 76;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(61, 773);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(56, 12);
            this.label21.TabIndex = 77;
            this.label21.Text = "카메라 ID";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(48, 799);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(69, 12);
            this.label22.TabIndex = 78;
            this.label22.Text = "카메라 이름";
            // 
            // textBox_camID
            // 
            this.textBox_camID.Location = new System.Drawing.Point(123, 766);
            this.textBox_camID.Name = "textBox_camID";
            this.textBox_camID.Size = new System.Drawing.Size(80, 21);
            this.textBox_camID.TabIndex = 79;
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(123, 793);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(312, 21);
            this.textBox_name.TabIndex = 80;
            // 
            // button_camAdd
            // 
            this.button_camAdd.Location = new System.Drawing.Point(28, 882);
            this.button_camAdd.Name = "button_camAdd";
            this.button_camAdd.Size = new System.Drawing.Size(107, 23);
            this.button_camAdd.TabIndex = 81;
            this.button_camAdd.Text = "카메라 추가/변경";
            this.button_camAdd.UseVisualStyleBackColor = true;
            this.button_camAdd.Click += new System.EventHandler(this.button_camAdd_Click);
            // 
            // button_camDel
            // 
            this.button_camDel.Location = new System.Drawing.Point(141, 882);
            this.button_camDel.Name = "button_camDel";
            this.button_camDel.Size = new System.Drawing.Size(107, 23);
            this.button_camDel.TabIndex = 82;
            this.button_camDel.Text = "카메라 삭제";
            this.button_camDel.UseVisualStyleBackColor = true;
            this.button_camDel.Click += new System.EventHandler(this.button_camDel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(475, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 12);
            this.label4.TabIndex = 91;
            this.label4.Text = "차량 이미지";
            // 
            // pictureBox_result
            // 
            this.pictureBox_result.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_result.Location = new System.Drawing.Point(477, 100);
            this.pictureBox_result.Name = "pictureBox_result";
            this.pictureBox_result.Size = new System.Drawing.Size(357, 203);
            this.pictureBox_result.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_result.TabIndex = 87;
            this.pictureBox_result.TabStop = false;
            this.pictureBox_result.Click += new System.EventHandler(this.pictureBox_result_Click);
            this.pictureBox_result.DoubleClick += new System.EventHandler(this.pictureBox_result_DoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ImageLocation = "geoje.png";
            this.pictureBox1.Location = new System.Drawing.Point(160, 914);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(14, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 92;
            this.pictureBox1.TabStop = false;
            // 
            // gMapControl1
            // 
            this.gMapControl1.Bearing = 0F;
            this.gMapControl1.CanDragMap = true;
            this.gMapControl1.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMapControl1.GrayScaleMode = false;
            this.gMapControl1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMapControl1.LevelsKeepInMemmory = 5;
            this.gMapControl1.Location = new System.Drawing.Point(477, 328);
            this.gMapControl1.MarkersEnabled = true;
            this.gMapControl1.MaxZoom = 15;
            this.gMapControl1.MinZoom = 12;
            this.gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl1.Name = "gMapControl1";
            this.gMapControl1.NegativeMode = false;
            this.gMapControl1.PolygonsEnabled = true;
            this.gMapControl1.RetryLoadTile = 0;
            this.gMapControl1.RoutesEnabled = true;
            this.gMapControl1.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMapControl1.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapControl1.ShowTileGridLines = false;
            this.gMapControl1.Size = new System.Drawing.Size(959, 553);
            this.gMapControl1.TabIndex = 93;
            this.gMapControl1.Zoom = 12D;
            this.gMapControl1.Load += new System.EventHandler(this.gMapControl1_Load);
            // 
            // textBox_result
            // 
            this.textBox_result.Location = new System.Drawing.Point(477, 301);
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.Size = new System.Drawing.Size(357, 21);
            this.textBox_result.TabIndex = 94;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(884, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 95;
            this.label1.Text = "경로 추적";
            // 
            // textBox_traceRoute
            // 
            this.textBox_traceRoute.Location = new System.Drawing.Point(886, 100);
            this.textBox_traceRoute.Multiline = true;
            this.textBox_traceRoute.Name = "textBox_traceRoute";
            this.textBox_traceRoute.Size = new System.Drawing.Size(550, 222);
            this.textBox_traceRoute.TabIndex = 96;
            // 
            // button_Statictics
            // 
            this.button_Statictics.Location = new System.Drawing.Point(94, 12);
            this.button_Statictics.Name = "button_Statictics";
            this.button_Statictics.Size = new System.Drawing.Size(75, 36);
            this.button_Statictics.TabIndex = 97;
            this.button_Statictics.Text = "통계 / 검색";
            this.button_Statictics.UseVisualStyleBackColor = true;
            this.button_Statictics.Click += new System.EventHandler(this.button_Statictics_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 823);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 12);
            this.label2.TabIndex = 98;
            this.label2.Text = "카메라 주소";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(74, 851);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 12);
            this.label3.TabIndex = 99;
            this.label3.Text = "접속 ID";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(254, 851);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 100;
            this.label5.Text = "암호";
            // 
            // textBox_address
            // 
            this.textBox_address.Location = new System.Drawing.Point(123, 820);
            this.textBox_address.Name = "textBox_address";
            this.textBox_address.Size = new System.Drawing.Size(312, 21);
            this.textBox_address.TabIndex = 101;
            // 
            // textBox_camUsername
            // 
            this.textBox_camUsername.Location = new System.Drawing.Point(123, 848);
            this.textBox_camUsername.Name = "textBox_camUsername";
            this.textBox_camUsername.Size = new System.Drawing.Size(100, 21);
            this.textBox_camUsername.TabIndex = 102;
            // 
            // textBox_camPassword
            // 
            this.textBox_camPassword.Location = new System.Drawing.Point(302, 848);
            this.textBox_camPassword.Name = "textBox_camPassword";
            this.textBox_camPassword.Size = new System.Drawing.Size(133, 21);
            this.textBox_camPassword.TabIndex = 103;
            // 
            // button_SaveGlobalSetting
            // 
            this.button_SaveGlobalSetting.Location = new System.Drawing.Point(508, 13);
            this.button_SaveGlobalSetting.Name = "button_SaveGlobalSetting";
            this.button_SaveGlobalSetting.Size = new System.Drawing.Size(76, 35);
            this.button_SaveGlobalSetting.TabIndex = 104;
            this.button_SaveGlobalSetting.Text = "설정 저장";
            this.button_SaveGlobalSetting.UseVisualStyleBackColor = true;
            this.button_SaveGlobalSetting.Click += new System.EventHandler(this.button_SaveGlobalSetting_Click);
            // 
            // button_Minimize
            // 
            this.button_Minimize.Location = new System.Drawing.Point(1666, 12);
            this.button_Minimize.Name = "button_Minimize";
            this.button_Minimize.Size = new System.Drawing.Size(75, 36);
            this.button_Minimize.TabIndex = 105;
            this.button_Minimize.Text = "최소화";
            this.button_Minimize.UseVisualStyleBackColor = true;
            this.button_Minimize.Click += new System.EventHandler(this.button_Minimize_Click);
            // 
            // button_addFile
            // 
            this.button_addFile.Location = new System.Drawing.Point(341, 882);
            this.button_addFile.Name = "button_addFile";
            this.button_addFile.Size = new System.Drawing.Size(94, 23);
            this.button_addFile.TabIndex = 106;
            this.button_addFile.Text = "외부파일연결";
            this.button_addFile.UseVisualStyleBackColor = true;
            this.button_addFile.Click += new System.EventHandler(this.button_addFile_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(238, 769);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 12);
            this.label6.TabIndex = 107;
            this.label6.Text = "카메라 제조사";
            // 
            // comboBox_manufacturer
            // 
            this.comboBox_manufacturer.FormattingEnabled = true;
            this.comboBox_manufacturer.Location = new System.Drawing.Point(325, 765);
            this.comboBox_manufacturer.Name = "comboBox_manufacturer";
            this.comboBox_manufacturer.Size = new System.Drawing.Size(110, 20);
            this.comboBox_manufacturer.TabIndex = 108;
            this.comboBox_manufacturer.SelectedIndexChanged += new System.EventHandler(this.comboBox_manufacturer_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1920, 1040);
            this.Controls.Add(this.comboBox_manufacturer);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button_addFile);
            this.Controls.Add(this.button_Minimize);
            this.Controls.Add(this.button_SaveGlobalSetting);
            this.Controls.Add(this.textBox_camPassword);
            this.Controls.Add(this.textBox_camUsername);
            this.Controls.Add(this.textBox_address);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_Statictics);
            this.Controls.Add(this.textBox_traceRoute);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_result);
            this.Controls.Add(this.gMapControl1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pictureBox_result);
            this.Controls.Add(this.button_camDel);
            this.Controls.Add(this.button_camAdd);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.textBox_camID);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.button_Configure);
            this.Controls.Add(this.button_SwitchMonitor);
            this.Controls.Add(this.pictureBox21);
            this.Controls.Add(this.listView_result);
            this.Controls.Add(this.button_Exit);
            this.Controls.Add(this.button_About);
            this.Controls.Add(this.button_PlayStop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "GBU ANPR Monitor";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_result)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_PlayStop;
        private System.Windows.Forms.Button button_About;
        private System.Windows.Forms.Button button_Exit;
        private System.Windows.Forms.PictureBox pictureBox21;
        private System.Windows.Forms.Button button_SwitchMonitor;
        private System.Windows.Forms.Button button_Configure;
        public System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox textBox_camID;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Button button_camAdd;
        private System.Windows.Forms.Button button_camDel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox_result;
        private System.Windows.Forms.PictureBox pictureBox1;
        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_traceRoute;
        public System.Windows.Forms.ListView listView_result;
        private System.Windows.Forms.Button button_Statictics;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_address;
        private System.Windows.Forms.TextBox textBox_camUsername;
        private System.Windows.Forms.TextBox textBox_camPassword;
        private System.Windows.Forms.Button button_SaveGlobalSetting;
        private System.Windows.Forms.Button button_Minimize;
        private System.Windows.Forms.Button button_addFile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_manufacturer;
    }
}

