namespace WEF.ModelGenerator.DbSelect
{
    partial class DBMsAccess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBMsAccess));
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnectionString = new CCWin.SkinControl.SkinAlphaWaterTextBox();
            this.rbconnstring = new System.Windows.Forms.RadioButton();
            this.rbdatabaseselect = new System.Windows.Forms.RadioButton();
            this.txtfilepath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.panelDB = new System.Windows.Forms.Panel();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.skinWaterTextBox1 = new CCWin.SkinControl.SkinWaterTextBox();
            this.panelDB.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 245);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "密码:";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.BackAlpha = 10;
            this.txtConnectionString.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtConnectionString.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtConnectionString.Enabled = false;
            this.txtConnectionString.Location = new System.Drawing.Point(122, 359);
            this.txtConnectionString.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtConnectionString.Size = new System.Drawing.Size(419, 121);
            this.txtConnectionString.TabIndex = 7;
            this.txtConnectionString.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtConnectionString.WaterFont = new System.Drawing.Font("微软雅黑", 8.5F);
            this.txtConnectionString.WaterText = "";
            // 
            // rbconnstring
            // 
            this.rbconnstring.AutoSize = true;
            this.rbconnstring.Location = new System.Drawing.Point(26, 323);
            this.rbconnstring.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbconnstring.Name = "rbconnstring";
            this.rbconnstring.Size = new System.Drawing.Size(111, 19);
            this.rbconnstring.TabIndex = 6;
            this.rbconnstring.Text = "连接字符串:";
            this.rbconnstring.UseVisualStyleBackColor = true;
            // 
            // rbdatabaseselect
            // 
            this.rbdatabaseselect.AutoSize = true;
            this.rbdatabaseselect.Checked = true;
            this.rbdatabaseselect.Location = new System.Drawing.Point(26, 159);
            this.rbdatabaseselect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbdatabaseselect.Name = "rbdatabaseselect";
            this.rbdatabaseselect.Size = new System.Drawing.Size(81, 19);
            this.rbdatabaseselect.TabIndex = 1;
            this.rbdatabaseselect.TabStop = true;
            this.rbdatabaseselect.Text = "数据库:";
            this.rbdatabaseselect.UseVisualStyleBackColor = true;
            this.rbdatabaseselect.CheckedChanged += new System.EventHandler(this.rbdatabaseselect_CheckedChanged);
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
            // fileDialog
            // 
            this.fileDialog.FileName = "*.mdb";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(31, 119);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(249, 25);
            this.txtPassword.TabIndex = 5;
            // 
            // panelDB
            // 
            this.panelDB.Controls.Add(this.txtUserName);
            this.panelDB.Controls.Add(this.txtPassword);
            this.panelDB.Controls.Add(this.button1);
            this.panelDB.Controls.Add(this.txtfilepath);
            this.panelDB.Location = new System.Drawing.Point(122, 122);
            this.panelDB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelDB.Name = "panelDB";
            this.panelDB.Size = new System.Drawing.Size(419, 191);
            this.panelDB.TabIndex = 7;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(31, 78);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(249, 25);
            this.txtUserName.TabIndex = 4;
            this.txtUserName.Text = "admin";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(332, 503);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 39);
            this.button2.TabIndex = 8;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(440, 503);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 39);
            this.button3.TabIndex = 9;
            this.button3.Text = "取消";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 203);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "帐号:";
            // 
            // skinWaterTextBox1
            // 
            this.skinWaterTextBox1.Location = new System.Drawing.Point(122, 73);
            this.skinWaterTextBox1.Name = "skinWaterTextBox1";
            this.skinWaterTextBox1.Size = new System.Drawing.Size(280, 25);
            this.skinWaterTextBox1.TabIndex = 11;
            this.skinWaterTextBox1.Text = "MSAcessConnection";
            this.skinWaterTextBox1.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinWaterTextBox1.WaterText = "配置名称";
            // 
            // DBMsAccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 608);
            this.Controls.Add(this.skinWaterTextBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panelDB);
            this.Controls.Add(this.rbdatabaseselect);
            this.Controls.Add(this.rbconnstring);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DBMsAccess";
            this.Text = "Access连接";
            this.panelDB.ResumeLayout(false);
            this.panelDB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private CCWin.SkinControl.SkinAlphaWaterTextBox txtConnectionString;
        private System.Windows.Forms.RadioButton rbconnstring;
        private System.Windows.Forms.RadioButton rbdatabaseselect;
        private System.Windows.Forms.TextBox txtfilepath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog fileDialog;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Panel panelDB;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label2;
        private CCWin.SkinControl.SkinWaterTextBox skinWaterTextBox1;
    }
}