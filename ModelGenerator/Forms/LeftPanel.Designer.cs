namespace WEF.ModelGenerator
{
    partial class LeftPanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LeftPanel));
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("数据库服务器");
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.Treeview = new System.Windows.Forms.TreeView();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStripTop = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripDatabase = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripOneDataBase = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刷新ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.批量生成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.执行SQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.生成代码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sQL查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripOneMongoDB = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewConnectStringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyFieldNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStripTop.SuspendLayout();
            this.contextMenuStripDatabase.SuspendLayout();
            this.contextMenuStripOneDataBase.SuspendLayout();
            this.contextMenuStripTable.SuspendLayout();
            this.contextMenuStripOneMongoDB.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(173, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.ToolTipText = "新建数据库连接";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "异常日志";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // Treeview
            // 
            this.Treeview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Treeview.ImageIndex = 0;
            this.Treeview.ImageList = this.imgList;
            this.Treeview.Location = new System.Drawing.Point(0, 25);
            this.Treeview.Name = "Treeview";
            treeNode2.Name = "节点0";
            treeNode2.Text = "数据库服务器";
            this.Treeview.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.Treeview.SelectedImageIndex = 0;
            this.Treeview.Size = new System.Drawing.Size(173, 339);
            this.Treeview.TabIndex = 1;
            this.Treeview.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tview_NodeMouseClick);
            this.Treeview.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tview_NodeMouseDoubleClick);
            this.Treeview.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tview_KeyUp);
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "server.ICO");
            this.imgList.Images.SetKeyName(1, "database.ICO");
            this.imgList.Images.SetKeyName(2, "file.ICO");
            this.imgList.Images.SetKeyName(3, "fileopen.ICO");
            this.imgList.Images.SetKeyName(4, "table.ICO");
            // 
            // contextMenuStripTop
            // 
            this.contextMenuStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem,
            this.刷新ToolStripMenuItem});
            this.contextMenuStripTop.Name = "contextMenuStripTop";
            this.contextMenuStripTop.Size = new System.Drawing.Size(101, 48);
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.新建ToolStripMenuItem.Text = "添加";
            this.新建ToolStripMenuItem.Click += new System.EventHandler(this.新建ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // contextMenuStripDatabase
            // 
            this.contextMenuStripDatabase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连接ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.刷新ToolStripMenuItem1,
            this.viewConnectStringToolStripMenuItem});
            this.contextMenuStripDatabase.Name = "contextMenuStripDatabase";
            this.contextMenuStripDatabase.Size = new System.Drawing.Size(161, 92);
            // 
            // 连接ToolStripMenuItem
            // 
            this.连接ToolStripMenuItem.Name = "连接ToolStripMenuItem";
            this.连接ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.连接ToolStripMenuItem.Text = "连接";
            this.连接ToolStripMenuItem.Click += new System.EventHandler(this.连接ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem1
            // 
            this.刷新ToolStripMenuItem1.Name = "刷新ToolStripMenuItem1";
            this.刷新ToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.刷新ToolStripMenuItem1.Text = "刷新";
            this.刷新ToolStripMenuItem1.Click += new System.EventHandler(this.刷新ToolStripMenuItem1_Click);
            // 
            // contextMenuStripOneDataBase
            // 
            this.contextMenuStripOneDataBase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新ToolStripMenuItem2,
            this.批量生成ToolStripMenuItem,
            this.执行SQLToolStripMenuItem});
            this.contextMenuStripOneDataBase.Name = "contextMenuStripOneDataBase";
            this.contextMenuStripOneDataBase.Size = new System.Drawing.Size(148, 70);
            // 
            // 刷新ToolStripMenuItem2
            // 
            this.刷新ToolStripMenuItem2.Name = "刷新ToolStripMenuItem2";
            this.刷新ToolStripMenuItem2.Size = new System.Drawing.Size(147, 22);
            this.刷新ToolStripMenuItem2.Text = "刷新";
            this.刷新ToolStripMenuItem2.Click += new System.EventHandler(this.刷新ToolStripMenuItem2_Click);
            // 
            // 批量生成ToolStripMenuItem
            // 
            this.批量生成ToolStripMenuItem.Name = "批量生成ToolStripMenuItem";
            this.批量生成ToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.批量生成ToolStripMenuItem.Text = "生成代码";
            this.批量生成ToolStripMenuItem.Click += new System.EventHandler(this.批量生成ToolStripMenuItem_Click);
            // 
            // 执行SQLToolStripMenuItem
            // 
            this.执行SQLToolStripMenuItem.Name = "执行SQLToolStripMenuItem";
            this.执行SQLToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.执行SQLToolStripMenuItem.Text = "SQL查询窗口";
            this.执行SQLToolStripMenuItem.Click += new System.EventHandler(this.执行SQLToolStripMenuItem_Click);
            // 
            // contextMenuStripTable
            // 
            this.contextMenuStripTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.生成代码ToolStripMenuItem,
            this.sQL查询ToolStripMenuItem,
            this.copyFieldNameToolStripMenuItem});
            this.contextMenuStripTable.Name = "contextMenuStripOneDataBase";
            this.contextMenuStripTable.Size = new System.Drawing.Size(181, 92);
            // 
            // 生成代码ToolStripMenuItem
            // 
            this.生成代码ToolStripMenuItem.Name = "生成代码ToolStripMenuItem";
            this.生成代码ToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.生成代码ToolStripMenuItem.Text = "查看详情";
            this.生成代码ToolStripMenuItem.Click += new System.EventHandler(this.生成代码ToolStripMenuItem_Click);
            // 
            // sQL查询ToolStripMenuItem
            // 
            this.sQL查询ToolStripMenuItem.Name = "sQL查询ToolStripMenuItem";
            this.sQL查询ToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.sQL查询ToolStripMenuItem.Text = "SQL查询窗口";
            this.sQL查询ToolStripMenuItem.Click += new System.EventHandler(this.sQL查询ToolStripMenuItem_Click);
            // 
            // contextMenuStripOneMongoDB
            // 
            this.contextMenuStripOneMongoDB.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStripOneMongoDB.Name = "contextMenuStripOneMongoDB";
            this.contextMenuStripOneMongoDB.Size = new System.Drawing.Size(101, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem1.Text = "刷新";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem2.Text = "查询";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // viewConnectStringToolStripMenuItem
            // 
            this.viewConnectStringToolStripMenuItem.Name = "viewConnectStringToolStripMenuItem";
            this.viewConnectStringToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewConnectStringToolStripMenuItem.Text = "查看连接字符串";
            this.viewConnectStringToolStripMenuItem.Click += new System.EventHandler(this.viewConnectStringToolStripMenuItem_Click);
            // 
            // copyFieldNameToolStripMenuItem
            // 
            this.copyFieldNameToolStripMenuItem.Name = "copyFieldNameToolStripMenuItem";
            this.copyFieldNameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copyFieldNameToolStripMenuItem.Text = "复制表名";
            this.copyFieldNameToolStripMenuItem.Click += new System.EventHandler(this.copyFieldNameToolStripMenuItem_Click);
            // 
            // LeftPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(173, 364);
            this.Controls.Add(this.Treeview);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LeftPanel";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Float;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "数据库视图";
            this.Load += new System.EventHandler(this.LeftPanel_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStripTop.ResumeLayout(false);
            this.contextMenuStripDatabase.ResumeLayout(false);
            this.contextMenuStripOneDataBase.ResumeLayout(false);
            this.contextMenuStripTable.ResumeLayout(false);
            this.contextMenuStripOneMongoDB.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        public System.Windows.Forms.TreeView Treeview;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTop;
        private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDatabase;
        private System.Windows.Forms.ToolStripMenuItem 连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOneDataBase;
        private System.Windows.Forms.ToolStripMenuItem 批量生成ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTable;
        private System.Windows.Forms.ToolStripMenuItem 生成代码ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem 执行SQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sQL查询ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOneMongoDB;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem viewConnectStringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyFieldNameToolStripMenuItem;
    }
}