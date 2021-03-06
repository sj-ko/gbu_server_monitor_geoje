﻿namespace GBU_Server_Monitor
{
    partial class ConfigureWindow
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
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.Configure_groupBox3 = new System.Windows.Forms.GroupBox();
            this.Configure_textbox_savepath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Configure_textbox_anprtimeout = new System.Windows.Forms.TextBox();
            this.Configure_textbox_importinterval = new System.Windows.Forms.TextBox();
            this.Configure_radioButton_serverMode = new System.Windows.Forms.RadioButton();
            this.Configure_radioButton_clientMode = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.Configure_textBox_serverAddr = new System.Windows.Forms.TextBox();
            this.Configure_groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(175, 317);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 0;
            this.button_OK.Text = "확인";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(268, 317);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 1;
            this.button_Cancel.Text = "취소";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // Configure_groupBox3
            // 
            this.Configure_groupBox3.Controls.Add(this.Configure_textbox_savepath);
            this.Configure_groupBox3.Controls.Add(this.label7);
            this.Configure_groupBox3.Location = new System.Drawing.Point(12, 110);
            this.Configure_groupBox3.Name = "Configure_groupBox3";
            this.Configure_groupBox3.Size = new System.Drawing.Size(331, 86);
            this.Configure_groupBox3.TabIndex = 9;
            this.Configure_groupBox3.TabStop = false;
            this.Configure_groupBox3.Text = "영상 저장 경로";
            // 
            // Configure_textbox_savepath
            // 
            this.Configure_textbox_savepath.Location = new System.Drawing.Point(78, 33);
            this.Configure_textbox_savepath.Name = "Configure_textbox_savepath";
            this.Configure_textbox_savepath.Size = new System.Drawing.Size(224, 21);
            this.Configure_textbox_savepath.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "경로";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 225);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "ANPR Timeout";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "Image Interval";
            // 
            // Configure_textbox_anprtimeout
            // 
            this.Configure_textbox_anprtimeout.Location = new System.Drawing.Point(132, 216);
            this.Configure_textbox_anprtimeout.Name = "Configure_textbox_anprtimeout";
            this.Configure_textbox_anprtimeout.Size = new System.Drawing.Size(118, 21);
            this.Configure_textbox_anprtimeout.TabIndex = 14;
            // 
            // Configure_textbox_importinterval
            // 
            this.Configure_textbox_importinterval.Location = new System.Drawing.Point(132, 249);
            this.Configure_textbox_importinterval.Name = "Configure_textbox_importinterval";
            this.Configure_textbox_importinterval.Size = new System.Drawing.Size(118, 21);
            this.Configure_textbox_importinterval.TabIndex = 15;
            // 
            // Configure_radioButton_serverMode
            // 
            this.Configure_radioButton_serverMode.AutoSize = true;
            this.Configure_radioButton_serverMode.Location = new System.Drawing.Point(28, 28);
            this.Configure_radioButton_serverMode.Name = "Configure_radioButton_serverMode";
            this.Configure_radioButton_serverMode.Size = new System.Drawing.Size(75, 16);
            this.Configure_radioButton_serverMode.TabIndex = 16;
            this.Configure_radioButton_serverMode.TabStop = true;
            this.Configure_radioButton_serverMode.Text = "서버 모드";
            this.Configure_radioButton_serverMode.UseVisualStyleBackColor = true;
            // 
            // Configure_radioButton_clientMode
            // 
            this.Configure_radioButton_clientMode.AutoSize = true;
            this.Configure_radioButton_clientMode.Location = new System.Drawing.Point(190, 28);
            this.Configure_radioButton_clientMode.Name = "Configure_radioButton_clientMode";
            this.Configure_radioButton_clientMode.Size = new System.Drawing.Size(111, 16);
            this.Configure_radioButton_clientMode.TabIndex = 17;
            this.Configure_radioButton_clientMode.TabStop = true;
            this.Configure_radioButton_clientMode.Text = "클라이언트 모드";
            this.Configure_radioButton_clientMode.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "DB 서버 주소";
            // 
            // Configure_textBox_serverAddr
            // 
            this.Configure_textBox_serverAddr.Location = new System.Drawing.Point(90, 64);
            this.Configure_textBox_serverAddr.Name = "Configure_textBox_serverAddr";
            this.Configure_textBox_serverAddr.Size = new System.Drawing.Size(225, 21);
            this.Configure_textBox_serverAddr.TabIndex = 19;
            // 
            // ConfigureWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(355, 352);
            this.Controls.Add(this.Configure_textBox_serverAddr);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Configure_radioButton_clientMode);
            this.Controls.Add(this.Configure_radioButton_serverMode);
            this.Controls.Add(this.Configure_textbox_importinterval);
            this.Controls.Add(this.Configure_textbox_anprtimeout);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Configure_groupBox3);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_OK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigureWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "설정";
            this.Configure_groupBox3.ResumeLayout(false);
            this.Configure_groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.GroupBox Configure_groupBox3;
        private System.Windows.Forms.TextBox Configure_textbox_savepath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Configure_textbox_anprtimeout;
        private System.Windows.Forms.TextBox Configure_textbox_importinterval;
        private System.Windows.Forms.RadioButton Configure_radioButton_serverMode;
        private System.Windows.Forms.RadioButton Configure_radioButton_clientMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Configure_textBox_serverAddr;
    }
}