namespace WEF.Standard.DevelopTools.DbSelect
{
    partial class DBOracle
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBOracle));
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cbbService = new System.Windows.Forms.ComboBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chbConnectString = new System.Windows.Forms.CheckBox();
            this.txtConnectString = new CCWin.SkinControl.SkinAlphaWaterTextBox();
            this.panelServer = new System.Windows.Forms.Panel();
            this.cbbServer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.skinWaterTextBox1 = new CCWin.SkinControl.SkinWaterTextBox();
            this.panelServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(375, 518);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 40);
            this.button3.TabIndex = 9;
            this.button3.Text = "取消";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(267, 518);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 40);
            this.button2.TabIndex = 8;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(105, 178);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(333, 25);
            this.txtPassword.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 181);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 15);
            this.label5.TabIndex = 24;
            this.label5.Text = "口  令:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 126);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 15);
            this.label4.TabIndex = 23;
            this.label4.Text = "用户名:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(120, 518);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 40);
            this.button1.TabIndex = 7;
            this.button1.Text = "连接测试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbbService
            // 
            this.cbbService.FormattingEnabled = true;
            this.cbbService.Location = new System.Drawing.Point(105, 66);
            this.cbbService.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbbService.Name = "cbbService";
            this.cbbService.Size = new System.Drawing.Size(333, 23);
            this.cbbService.TabIndex = 2;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(105, 122);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(333, 25);
            this.txtUserName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 70);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "服务名:";
            // 
            // chbConnectString
            // 
            this.chbConnectString.AutoSize = true;
            this.chbConnectString.Location = new System.Drawing.Point(52, 343);
            this.chbConnectString.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbConnectString.Name = "chbConnectString";
            this.chbConnectString.Size = new System.Drawing.Size(104, 19);
            this.chbConnectString.TabIndex = 5;
            this.chbConnectString.Text = "连接字符串";
            this.chbConnectString.UseVisualStyleBackColor = true;
            this.chbConnectString.CheckedChanged += new System.EventHandler(this.chbConnectString_CheckedChanged);
            // 
            // txtConnectString
            // 
            this.txtConnectString.BackAlpha = 10;
            this.txtConnectString.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtConnectString.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtConnectString.Enabled = false;
            this.txtConnectString.Location = new System.Drawing.Point(52, 371);
            this.txtConnectString.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtConnectString.Multiline = true;
            this.txtConnectString.Name = "txtConnectString";
            this.txtConnectString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtConnectString.Size = new System.Drawing.Size(423, 129);
            this.txtConnectString.TabIndex = 6;
            this.txtConnectString.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtConnectString.WaterFont = new System.Drawing.Font("微软雅黑", 8.5F);
            this.txtConnectString.WaterText = "请输入连接字符串";
            // 
            // panelServer
            // 
            this.panelServer.Controls.Add(this.cbbServer);
            this.panelServer.Controls.Add(this.cbbService);
            this.panelServer.Controls.Add(this.txtUserName);
            this.panelServer.Controls.Add(this.label2);
            this.panelServer.Controls.Add(this.txtPassword);
            this.panelServer.Controls.Add(this.label1);
            this.panelServer.Controls.Add(this.label5);
            this.panelServer.Controls.Add(this.label4);
            this.panelServer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelServer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.panelServer.Location = new System.Drawing.Point(35, 100);
            this.panelServer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelServer.Name = "panelServer";
            this.panelServer.Size = new System.Drawing.Size(467, 216);
            this.panelServer.TabIndex = 32;
            // 
            // cbbServer
            // 
            this.cbbServer.FormattingEnabled = true;
            this.cbbServer.Location = new System.Drawing.Point(105, 18);
            this.cbbServer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbbServer.Name = "cbbServer";
            this.cbbServer.Size = new System.Drawing.Size(333, 23);
            this.cbbServer.TabIndex = 1;
            this.cbbServer.Text = "127.0.0.1:1521";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "服务器:";
            // 
            // skinWaterTextBox1
            // 
            this.skinWaterTextBox1.Location = new System.Drawing.Point(35, 57);
            this.skinWaterTextBox1.Name = "skinWaterTextBox1";
            this.skinWaterTextBox1.Size = new System.Drawing.Size(467, 25);
            this.skinWaterTextBox1.TabIndex = 55;
            this.skinWaterTextBox1.Text = "OracleConnection";
            this.skinWaterTextBox1.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinWaterTextBox1.WaterText = "配置名称";
            // 
            // DBOracle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 586);
            this.Controls.Add(this.skinWaterTextBox1);
            this.Controls.Add(this.txtConnectString);
            this.Controls.Add(this.chbConnectString);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panelServer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DBOracle";
            this.Text = "连接到服务器";
            this.panelServer.ResumeLayout(false);
            this.panelServer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbbService;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbConnectString;
        private CCWin.SkinControl.SkinAlphaWaterTextBox txtConnectString;
        private System.Windows.Forms.Panel panelServer;
        private System.Windows.Forms.ComboBox cbbServer;
        private System.Windows.Forms.Label label2;
        private CCWin.SkinControl.SkinWaterTextBox skinWaterTextBox1;
    }
}