﻿namespace WEF.Standard.DevelopTools.DbSelect
{
    partial class DbSqlite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbSqlite));
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panelDB = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtfilepath = new System.Windows.Forms.TextBox();
            this.rbdatabaseselect = new System.Windows.Forms.RadioButton();
            this.rbconnstring = new System.Windows.Forms.RadioButton();
            this.txtConnectionString = new CCWin.SkinControl.SkinAlphaWaterTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.skinWaterTextBox1 = new CCWin.SkinControl.SkinWaterTextBox();
            this.panelDB.SuspendLayout();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(455, 530);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 40);
            this.button3.TabIndex = 8;
            this.button3.Text = "取消";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(347, 530);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 40);
            this.button2.TabIndex = 7;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panelDB
            // 
            this.panelDB.Controls.Add(this.txtPassword);
            this.panelDB.Controls.Add(this.button1);
            this.panelDB.Controls.Add(this.txtfilepath);
            this.panelDB.Location = new System.Drawing.Point(136, 137);
            this.panelDB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelDB.Name = "panelDB";
            this.panelDB.Size = new System.Drawing.Size(419, 159);
            this.panelDB.TabIndex = 15;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(31, 91);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(249, 25);
            this.txtPassword.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(291, 29);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 29);
            this.button1.TabIndex = 3;
            this.button1.Text = "请选择";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtfilepath
            // 
            this.txtfilepath.Location = new System.Drawing.Point(31, 31);
            this.txtfilepath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtfilepath.Name = "txtfilepath";
            this.txtfilepath.ReadOnly = true;
            this.txtfilepath.Size = new System.Drawing.Size(249, 25);
            this.txtfilepath.TabIndex = 2;
            // 
            // rbdatabaseselect
            // 
            this.rbdatabaseselect.AutoSize = true;
            this.rbdatabaseselect.Checked = true;
            this.rbdatabaseselect.Location = new System.Drawing.Point(40, 174);
            this.rbdatabaseselect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbdatabaseselect.Name = "rbdatabaseselect";
            this.rbdatabaseselect.Size = new System.Drawing.Size(81, 19);
            this.rbdatabaseselect.TabIndex = 1;
            this.rbdatabaseselect.TabStop = true;
            this.rbdatabaseselect.Text = "数据库:";
            this.rbdatabaseselect.UseVisualStyleBackColor = true;
            this.rbdatabaseselect.CheckedChanged += new System.EventHandler(this.rbdatabaseselect_CheckedChanged);
            // 
            // rbconnstring
            // 
            this.rbconnstring.AutoSize = true;
            this.rbconnstring.Location = new System.Drawing.Point(40, 329);
            this.rbconnstring.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbconnstring.Name = "rbconnstring";
            this.rbconnstring.Size = new System.Drawing.Size(111, 19);
            this.rbconnstring.TabIndex = 5;
            this.rbconnstring.Text = "连接字符串:";
            this.rbconnstring.UseVisualStyleBackColor = true;
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.BackAlpha = 10;
            this.txtConnectionString.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtConnectionString.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtConnectionString.Enabled = false;
            this.txtConnectionString.Location = new System.Drawing.Point(167, 329);
            this.txtConnectionString.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtConnectionString.Size = new System.Drawing.Size(388, 176);
            this.txtConnectionString.TabIndex = 6;
            this.txtConnectionString.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtConnectionString.WaterFont = new System.Drawing.Font("微软雅黑", 8.5F);
            this.txtConnectionString.WaterText = "请输入连接字符串";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 231);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "密码:";
            // 
            // fileDialog
            // 
            this.fileDialog.FileName = "*.mdb";
            // 
            // skinWaterTextBox1
            // 
            this.skinWaterTextBox1.Location = new System.Drawing.Point(40, 74);
            this.skinWaterTextBox1.Name = "skinWaterTextBox1";
            this.skinWaterTextBox1.Size = new System.Drawing.Size(515, 25);
            this.skinWaterTextBox1.TabIndex = 57;
            this.skinWaterTextBox1.Text = "SqliteConnection";
            this.skinWaterTextBox1.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinWaterTextBox1.WaterText = "配置名称";
            // 
            // DbSqlite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 601);
            this.Controls.Add(this.skinWaterTextBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panelDB);
            this.Controls.Add(this.rbdatabaseselect);
            this.Controls.Add(this.rbconnstring);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DbSqlite";
            this.Text = "Sqlite连接";
            this.panelDB.ResumeLayout(false);
            this.panelDB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panelDB;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtfilepath;
        private System.Windows.Forms.RadioButton rbdatabaseselect;
        private System.Windows.Forms.RadioButton rbconnstring;
        private CCWin.SkinControl.SkinAlphaWaterTextBox txtConnectionString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog fileDialog;
        private CCWin.SkinControl.SkinWaterTextBox skinWaterTextBox1;
    }
}