namespace WEF.ModelGenerator.DbSelect
{
    partial class DBMySql
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBMySql));
            this.txtport = new System.Windows.Forms.TextBox();
            this.cbbServer = new System.Windows.Forms.ComboBox();
            this.cbbDatabase = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.skinWaterTextBox1 = new CCWin.SkinControl.SkinWaterTextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtport
            // 
            this.txtport.Location = new System.Drawing.Point(312, 19);
            this.txtport.Name = "txtport";
            this.txtport.Size = new System.Drawing.Size(40, 21);
            this.txtport.TabIndex = 2;
            this.txtport.Text = "3306";
            // 
            // cbbServer
            // 
            this.cbbServer.FormattingEnabled = true;
            this.cbbServer.Location = new System.Drawing.Point(107, 20);
            this.cbbServer.Name = "cbbServer";
            this.cbbServer.Size = new System.Drawing.Size(163, 20);
            this.cbbServer.TabIndex = 1;
            this.cbbServer.Text = "127.0.0.1";
            // 
            // cbbDatabase
            // 
            this.cbbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbDatabase.Enabled = false;
            this.cbbDatabase.Location = new System.Drawing.Point(106, 121);
            this.cbbDatabase.Name = "cbbDatabase";
            this.cbbDatabase.Size = new System.Drawing.Size(246, 20);
            this.cbbDatabase.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 43;
            this.label4.Text = "数据库(&D)：";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(107, 51);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(246, 21);
            this.txtUserName.TabIndex = 3;
            this.txtUserName.Text = "root";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 38;
            this.label2.Text = "登录名(&L)：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 37;
            this.label1.Text = "服务器名称(&S)：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(106, 86);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(246, 21);
            this.txtPassword.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 40;
            this.label3.Text = "密码(&P)：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(272, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 46;
            this.label5.Text = "端口：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(117, 422);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 32);
            this.button1.TabIndex = 8;
            this.button1.Text = "连接测试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(223, 422);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 32);
            this.button2.TabIndex = 9;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(304, 422);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 32);
            this.button3.TabIndex = 10;
            this.button3.Text = "取消";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(20, 217);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 16);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "从连接字符串";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbbDatabase);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtport);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbbServer);
            this.panel1.Controls.Add(this.txtUserName);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Location = new System.Drawing.Point(20, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(363, 156);
            this.panel1.TabIndex = 52;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.skinWaterTextBox1);
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(20, 239);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(363, 168);
            this.panel2.TabIndex = 53;
            // 
            // skinWaterTextBox1
            // 
            this.skinWaterTextBox1.Location = new System.Drawing.Point(8, 3);
            this.skinWaterTextBox1.Multiline = true;
            this.skinWaterTextBox1.Name = "skinWaterTextBox1";
            this.skinWaterTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.skinWaterTextBox1.Size = new System.Drawing.Size(345, 158);
            this.skinWaterTextBox1.TabIndex = 7;
            this.skinWaterTextBox1.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinWaterTextBox1.WaterText = "请输mysql连接字符串";
            // 
            // DBMySql
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 471);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DBMySql";
            this.ShowInTaskbar = false;
            this.Text = "连接MySql服务器";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtport;
        public System.Windows.Forms.ComboBox cbbServer;
        public System.Windows.Forms.ComboBox cbbDatabase;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtUserName;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtPassword;
        public System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private CCWin.SkinControl.SkinWaterTextBox skinWaterTextBox1;
    }
}