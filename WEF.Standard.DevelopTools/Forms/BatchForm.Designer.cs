﻿namespace WEF.Standard.DevelopTools
{
    partial class BatchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchForm));
            this.label1 = new System.Windows.Forms.Label();
            this.lbleft = new System.Windows.Forms.ListBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.llServer = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.llDatabaseName = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panelbtns = new System.Windows.Forms.Panel();
            this.chbView = new System.Windows.Forms.CheckBox();
            this.btnallto = new System.Windows.Forms.Button();
            this.btnallback = new System.Windows.Forms.Button();
            this.btnto = new System.Windows.Forms.Button();
            this.btnback = new System.Windows.Forms.Button();
            this.lbright = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbToupperFrstword = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.txtNamaspace = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.pbar = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panelbtns.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(451, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据库:";
            // 
            // lbleft
            // 
            this.lbleft.FormattingEnabled = true;
            this.lbleft.ItemHeight = 15;
            this.lbleft.Location = new System.Drawing.Point(87, 25);
            this.lbleft.Margin = new System.Windows.Forms.Padding(4);
            this.lbleft.Name = "lbleft";
            this.lbleft.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbleft.Size = new System.Drawing.Size(319, 259);
            this.lbleft.TabIndex = 1;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(148, 95);
            this.txtPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(709, 25);
            this.txtPath.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(385, 618);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 44);
            this.button1.TabIndex = 4;
            this.button1.Text = "导出";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.llServer);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.llDatabaseName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(21, 60);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(963, 78);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据库";
            // 
            // llServer
            // 
            this.llServer.AutoSize = true;
            this.llServer.Location = new System.Drawing.Point(84, 36);
            this.llServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llServer.Name = "llServer";
            this.llServer.Size = new System.Drawing.Size(0, 15);
            this.llServer.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 36);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "服务器:";
            // 
            // llDatabaseName
            // 
            this.llDatabaseName.AutoSize = true;
            this.llDatabaseName.Location = new System.Drawing.Point(540, 36);
            this.llDatabaseName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llDatabaseName.Name = "llDatabaseName";
            this.llDatabaseName.Size = new System.Drawing.Size(0, 15);
            this.llDatabaseName.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panelbtns);
            this.groupBox2.Controls.Add(this.lbright);
            this.groupBox2.Controls.Add(this.lbleft);
            this.groupBox2.Location = new System.Drawing.Point(21, 145);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(963, 304);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "表选择";
            // 
            // panelbtns
            // 
            this.panelbtns.Controls.Add(this.chbView);
            this.panelbtns.Controls.Add(this.btnallto);
            this.panelbtns.Controls.Add(this.btnallback);
            this.panelbtns.Controls.Add(this.btnto);
            this.panelbtns.Controls.Add(this.btnback);
            this.panelbtns.Location = new System.Drawing.Point(415, 25);
            this.panelbtns.Margin = new System.Windows.Forms.Padding(4);
            this.panelbtns.Name = "panelbtns";
            this.panelbtns.Size = new System.Drawing.Size(137, 260);
            this.panelbtns.TabIndex = 16;
            // 
            // chbView
            // 
            this.chbView.AutoSize = true;
            this.chbView.Location = new System.Drawing.Point(23, 5);
            this.chbView.Margin = new System.Windows.Forms.Padding(4);
            this.chbView.Name = "chbView";
            this.chbView.Size = new System.Drawing.Size(89, 19);
            this.chbView.TabIndex = 16;
            this.chbView.Text = "加载视图";
            this.chbView.UseVisualStyleBackColor = true;
            this.chbView.CheckedChanged += new System.EventHandler(this.chbView_CheckedChanged);
            // 
            // btnallto
            // 
            this.btnallto.Location = new System.Drawing.Point(20, 46);
            this.btnallto.Margin = new System.Windows.Forms.Padding(4);
            this.btnallto.Name = "btnallto";
            this.btnallto.Size = new System.Drawing.Size(100, 29);
            this.btnallto.TabIndex = 12;
            this.btnallto.Text = ">>";
            this.btnallto.UseVisualStyleBackColor = true;
            this.btnallto.Click += new System.EventHandler(this.btnallto_Click);
            // 
            // btnallback
            // 
            this.btnallback.Location = new System.Drawing.Point(20, 211);
            this.btnallback.Margin = new System.Windows.Forms.Padding(4);
            this.btnallback.Name = "btnallback";
            this.btnallback.Size = new System.Drawing.Size(100, 29);
            this.btnallback.TabIndex = 15;
            this.btnallback.Text = "<<";
            this.btnallback.UseVisualStyleBackColor = true;
            this.btnallback.Click += new System.EventHandler(this.btnallback_Click);
            // 
            // btnto
            // 
            this.btnto.Location = new System.Drawing.Point(20, 99);
            this.btnto.Margin = new System.Windows.Forms.Padding(4);
            this.btnto.Name = "btnto";
            this.btnto.Size = new System.Drawing.Size(100, 29);
            this.btnto.TabIndex = 13;
            this.btnto.Text = ">";
            this.btnto.UseVisualStyleBackColor = true;
            this.btnto.Click += new System.EventHandler(this.btnto_Click);
            // 
            // btnback
            // 
            this.btnback.Location = new System.Drawing.Point(20, 155);
            this.btnback.Margin = new System.Windows.Forms.Padding(4);
            this.btnback.Name = "btnback";
            this.btnback.Size = new System.Drawing.Size(100, 29);
            this.btnback.TabIndex = 14;
            this.btnback.Text = "<";
            this.btnback.UseVisualStyleBackColor = true;
            this.btnback.Click += new System.EventHandler(this.btnback_Click);
            // 
            // lbright
            // 
            this.lbright.FormattingEnabled = true;
            this.lbright.ItemHeight = 15;
            this.lbright.Location = new System.Drawing.Point(560, 25);
            this.lbright.Margin = new System.Windows.Forms.Padding(4);
            this.lbright.Name = "lbright";
            this.lbright.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbright.Size = new System.Drawing.Size(325, 259);
            this.lbright.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbToupperFrstword);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.txtNamaspace);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtPath);
            this.groupBox3.Location = new System.Drawing.Point(21, 456);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(963, 140);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "参数设置";
            // 
            // cbToupperFrstword
            // 
            this.cbToupperFrstword.AutoSize = true;
            this.cbToupperFrstword.Checked = true;
            this.cbToupperFrstword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbToupperFrstword.Location = new System.Drawing.Point(781, 42);
            this.cbToupperFrstword.Margin = new System.Windows.Forms.Padding(4);
            this.cbToupperFrstword.Name = "cbToupperFrstword";
            this.cbToupperFrstword.Size = new System.Drawing.Size(104, 19);
            this.cbToupperFrstword.TabIndex = 10;
            this.cbToupperFrstword.Text = "首字母大写";
            this.cbToupperFrstword.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(854, 92);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(31, 29);
            this.button2.TabIndex = 3;
            this.button2.Text = "…";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtNamaspace
            // 
            this.txtNamaspace.Location = new System.Drawing.Point(148, 40);
            this.txtNamaspace.Margin = new System.Windows.Forms.Padding(4);
            this.txtNamaspace.Name = "txtNamaspace";
            this.txtNamaspace.Size = new System.Drawing.Size(625, 25);
            this.txtNamaspace.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(84, 36);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 15);
            this.label3.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 44);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "命名空间:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(540, 36);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 15);
            this.label6.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 99);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "导出文件夹:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(524, 618);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 44);
            this.button3.TabIndex = 5;
            this.button3.Text = "关闭";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // pbar
            // 
            this.pbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbar.Location = new System.Drawing.Point(4, 669);
            this.pbar.Margin = new System.Windows.Forms.Padding(4);
            this.pbar.Name = "pbar";
            this.pbar.Size = new System.Drawing.Size(997, 29);
            this.pbar.TabIndex = 12;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // BatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 702);
            this.Controls.Add(this.pbar);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BatchForm";
            this.ShowInTaskbar = false;
            this.Text = "批量生成";
            this.Load += new System.EventHandler(this.BatchForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panelbtns.ResumeLayout(false);
            this.panelbtns.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbleft;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label llServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label llDatabaseName;
        private System.Windows.Forms.ListBox lbright;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtNamaspace;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnallback;
        private System.Windows.Forms.Button btnback;
        private System.Windows.Forms.Button btnto;
        private System.Windows.Forms.Button btnallto;
        private System.Windows.Forms.ProgressBar pbar;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel panelbtns;
        private System.Windows.Forms.CheckBox chbView;
        private System.Windows.Forms.CheckBox cbToupperFrstword;
    }
}