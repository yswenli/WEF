namespace WEF.ModelGenerator
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newDBConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.templateManageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateBusinessCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.templatetocodefastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataSyncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jsonToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qRCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorlogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sQL查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.displayUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vS2015LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.cleanUpGarbageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockPanel
            // 
            this.dockPanel.BackgroundImage = global::WEF.ModelGenerator.Properties.Resources._0616152d52f214c689f01819eefb836d;
            this.dockPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.dockPanel.DockBackColor = System.Drawing.SystemColors.Control;
            this.dockPanel.Location = new System.Drawing.Point(4, 71);
            this.dockPanel.Margin = new System.Windows.Forms.Padding(6);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(1836, 718);
            this.dockPanel.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(4, 800);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(19, 0, 1, 0);
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip1.Size = new System.Drawing.Size(1836, 26);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(383, 20);
            this.toolStripStatusLabel1.Text = "下载地址：https://github.com/yswenli/WEF/releases";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDBConnectToolStripMenuItem,
            this.runToolStripMenuItem,
            this.mToolStripMenuItem,
            this.dataSyncToolStripMenuItem,
            this.jsonToolToolStripMenuItem,
            this.qRCodeToolStripMenuItem,
            this.cleanUpGarbageToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(4, 32);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1836, 28);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // newDBConnectToolStripMenuItem
            // 
            this.newDBConnectToolStripMenuItem.Name = "newDBConnectToolStripMenuItem";
            this.newDBConnectToolStripMenuItem.Size = new System.Drawing.Size(90, 24);
            this.newDBConnectToolStripMenuItem.Text = "(&N)新连接";
            this.newDBConnectToolStripMenuItem.Click += new System.EventHandler(this.newDBConnectToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(129, 24);
            this.runToolStripMenuItem.Text = "(&R)执行SQL(F5)";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // mToolStripMenuItem
            // 
            this.mToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.templateManageToolStripMenuItem,
            this.generateBusinessCodeToolStripMenuItem,
            this.templatetocodefastToolStripMenuItem});
            this.mToolStripMenuItem.Name = "mToolStripMenuItem";
            this.mToolStripMenuItem.Size = new System.Drawing.Size(72, 24);
            this.mToolStripMenuItem.Text = "(&T)模板";
            // 
            // templateManageToolStripMenuItem
            // 
            this.templateManageToolStripMenuItem.Name = "templateManageToolStripMenuItem";
            this.templateManageToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.templateManageToolStripMenuItem.Text = "管理模板";
            this.templateManageToolStripMenuItem.Click += new System.EventHandler(this.templateManageToolStripMenuItem_Click);
            // 
            // generateBusinessCodeToolStripMenuItem
            // 
            this.generateBusinessCodeToolStripMenuItem.Name = "generateBusinessCodeToolStripMenuItem";
            this.generateBusinessCodeToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.generateBusinessCodeToolStripMenuItem.Text = "生成业务代码";
            this.generateBusinessCodeToolStripMenuItem.Click += new System.EventHandler(this.generateBusinessCodeToolStripMenuItem_Click);
            // 
            // templatetocodefastToolStripMenuItem
            // 
            this.templatetocodefastToolStripMenuItem.Name = "templatetocodefastToolStripMenuItem";
            this.templatetocodefastToolStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.templatetocodefastToolStripMenuItem.Text = "快捷生成业务代码";
            this.templatetocodefastToolStripMenuItem.Click += new System.EventHandler(this.templatetocodefastToolStripMenuItem_Click);
            // 
            // dataSyncToolStripMenuItem
            // 
            this.dataSyncToolStripMenuItem.Name = "dataSyncToolStripMenuItem";
            this.dataSyncToolStripMenuItem.Size = new System.Drawing.Size(102, 24);
            this.dataSyncToolStripMenuItem.Text = "(&S)数据传输";
            this.dataSyncToolStripMenuItem.Click += new System.EventHandler(this.dataSyncToolStripMenuItem_Click);
            // 
            // jsonToolToolStripMenuItem
            // 
            this.jsonToolToolStripMenuItem.Name = "jsonToolToolStripMenuItem";
            this.jsonToolToolStripMenuItem.Size = new System.Drawing.Size(101, 24);
            this.jsonToolToolStripMenuItem.Text = "(&J)Json工具";
            this.jsonToolToolStripMenuItem.Click += new System.EventHandler(this.jsonToolToolStripMenuItem_Click);
            // 
            // qRCodeToolStripMenuItem
            // 
            this.qRCodeToolStripMenuItem.Name = "qRCodeToolStripMenuItem";
            this.qRCodeToolStripMenuItem.Size = new System.Drawing.Size(120, 24);
            this.qRCodeToolStripMenuItem.Text = "(&Q)二维码工具";
            this.qRCodeToolStripMenuItem.Click += new System.EventHandler(this.qRCodeToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.errorlogToolStripMenuItem});
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.关于ToolStripMenuItem.Text = "(&A)关于";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.aboutToolStripMenuItem.Text = "about";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // errorlogToolStripMenuItem
            // 
            this.errorlogToolStripMenuItem.Name = "errorlogToolStripMenuItem";
            this.errorlogToolStripMenuItem.Size = new System.Drawing.Size(153, 26);
            this.errorlogToolStripMenuItem.Text = "errorlog";
            this.errorlogToolStripMenuItem.Click += new System.EventHandler(this.errorlogToolStripMenuItem_Click);
            // 
            // sQL查询ToolStripMenuItem
            // 
            this.sQL查询ToolStripMenuItem.Name = "sQL查询ToolStripMenuItem";
            this.sQL查询ToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.BalloonTipText = "正在后台运行";
            this.NotifyIcon.BalloonTipTitle = "WEF数据库工具";
            this.NotifyIcon.ContextMenuStrip = this.contextMenuStrip1;
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "WEF数据库工具";
            this.NotifyIcon.Visible = true;
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayUIToolStripMenuItem,
            this.shutDownToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(154, 52);
            // 
            // displayUIToolStripMenuItem
            // 
            this.displayUIToolStripMenuItem.Name = "displayUIToolStripMenuItem";
            this.displayUIToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.displayUIToolStripMenuItem.Text = "显示主界面";
            this.displayUIToolStripMenuItem.Click += new System.EventHandler(this.displayUIToolStripMenuItem_Click);
            // 
            // shutDownToolStripMenuItem
            // 
            this.shutDownToolStripMenuItem.Name = "shutDownToolStripMenuItem";
            this.shutDownToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.shutDownToolStripMenuItem.Text = "退出";
            this.shutDownToolStripMenuItem.Click += new System.EventHandler(this.shutDownToolStripMenuItem_Click);
            // 
            // cleanUpGarbageToolStripMenuItem
            // 
            this.cleanUpGarbageToolStripMenuItem.Name = "cleanUpGarbageToolStripMenuItem";
            this.cleanUpGarbageToolStripMenuItem.Size = new System.Drawing.Size(103, 24);
            this.cleanUpGarbageToolStripMenuItem.Text = "(&C)清理垃圾";
            this.cleanUpGarbageToolStripMenuItem.Click += new System.EventHandler(this.cleanUpGarbageToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1844, 830);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.dockPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "WEF 数据库工具";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newDBConnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sQL查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        public System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem displayUIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shutDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jsonToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qRCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem errorlogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem templateManageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateBusinessCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem templatetocodefastToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataSyncToolStripMenuItem;
        private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme1;
        private System.Windows.Forms.ToolStripMenuItem cleanUpGarbageToolStripMenuItem;
    }
}

