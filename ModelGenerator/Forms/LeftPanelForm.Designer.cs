namespace WEF.ModelGenerator
{
    partial class LeftPanelForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LeftPanelForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("数据库服务器");
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.Treeview = new System.Windows.Forms.TreeView();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStripTop = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripDatabase = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewConnectStringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripOneDataBase = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刷新ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.批量生成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.执行SQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.生成代码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sQL查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyFieldNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.dataOperateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripOneMongoDB = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.searchTextBox = new CCWin.SkinControl.SkinWaterTextBox();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
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
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(354, 30);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(46, 24);
            this.toolStripButton1.ToolTipText = "新建数据库连接";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // Treeview
            // 
            this.Treeview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Treeview.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Treeview.ImageIndex = 0;
            this.Treeview.ImageList = this.imgList;
            this.Treeview.Location = new System.Drawing.Point(0, 94);
            this.Treeview.Margin = new System.Windows.Forms.Padding(6);
            this.Treeview.Name = "Treeview";
            treeNode1.Name = "节点0";
            treeNode1.Text = "数据库服务器";
            this.Treeview.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.Treeview.SelectedImageIndex = 0;
            this.Treeview.Size = new System.Drawing.Size(342, 630);
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
            this.contextMenuStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem,
            this.刷新ToolStripMenuItem});
            this.contextMenuStripTop.Name = "contextMenuStripTop";
            this.contextMenuStripTop.Size = new System.Drawing.Size(329, 80);
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(328, 38);
            this.新建ToolStripMenuItem.Text = "添加数据库服务器";
            this.新建ToolStripMenuItem.Click += new System.EventHandler(this.新建ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(328, 38);
            this.刷新ToolStripMenuItem.Text = "刷新数据库服务器列表";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // contextMenuStripDatabase
            // 
            this.contextMenuStripDatabase.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripDatabase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连接ToolStripMenuItem,
            this.toolStripMenuItem5,
            this.刷新ToolStripMenuItem1,
            this.toolStripMenuItem9,
            this.viewConnectStringToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolStripMenuItem8,
            this.删除ToolStripMenuItem});
            this.contextMenuStripDatabase.Name = "contextMenuStripDatabase";
            this.contextMenuStripDatabase.Size = new System.Drawing.Size(257, 212);
            // 
            // 连接ToolStripMenuItem
            // 
            this.连接ToolStripMenuItem.Name = "连接ToolStripMenuItem";
            this.连接ToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            this.连接ToolStripMenuItem.Text = "连接";
            this.连接ToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            this.editToolStripMenuItem.Text = "编辑";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem1
            // 
            this.刷新ToolStripMenuItem1.Name = "刷新ToolStripMenuItem1";
            this.刷新ToolStripMenuItem1.Size = new System.Drawing.Size(300, 38);
            this.刷新ToolStripMenuItem1.Text = "刷新";
            this.刷新ToolStripMenuItem1.Click += new System.EventHandler(this.refreshToolStripMenuItem1_Click);
            // 
            // viewConnectStringToolStripMenuItem
            // 
            this.viewConnectStringToolStripMenuItem.Name = "viewConnectStringToolStripMenuItem";
            this.viewConnectStringToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            this.viewConnectStringToolStripMenuItem.Text = "查看连接字符串";
            this.viewConnectStringToolStripMenuItem.Click += new System.EventHandler(this.viewConnectStringToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(297, 6);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // contextMenuStripOneDataBase
            // 
            this.contextMenuStripOneDataBase.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripOneDataBase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新ToolStripMenuItem2,
            this.toolStripMenuItem10,
            this.执行SQLToolStripMenuItem,
            this.toolStripMenuItem11,
            this.批量生成ToolStripMenuItem,
            this.toolStripMenuItem12,
            this.createTableToolStripMenuItem});
            this.contextMenuStripOneDataBase.Name = "contextMenuStripOneDataBase";
            this.contextMenuStripOneDataBase.Size = new System.Drawing.Size(231, 174);
            // 
            // 刷新ToolStripMenuItem2
            // 
            this.刷新ToolStripMenuItem2.Name = "刷新ToolStripMenuItem2";
            this.刷新ToolStripMenuItem2.Size = new System.Drawing.Size(230, 38);
            this.刷新ToolStripMenuItem2.Text = "刷新";
            this.刷新ToolStripMenuItem2.Click += new System.EventHandler(this.刷新ToolStripMenuItem2_Click);
            // 
            // 批量生成ToolStripMenuItem
            // 
            this.批量生成ToolStripMenuItem.Name = "批量生成ToolStripMenuItem";
            this.批量生成ToolStripMenuItem.Size = new System.Drawing.Size(230, 38);
            this.批量生成ToolStripMenuItem.Text = "生成代码";
            this.批量生成ToolStripMenuItem.Click += new System.EventHandler(this.批量生成ToolStripMenuItem_Click);
            // 
            // 执行SQLToolStripMenuItem
            // 
            this.执行SQLToolStripMenuItem.Name = "执行SQLToolStripMenuItem";
            this.执行SQLToolStripMenuItem.Size = new System.Drawing.Size(230, 38);
            this.执行SQLToolStripMenuItem.Text = "SQL查询窗口";
            this.执行SQLToolStripMenuItem.Click += new System.EventHandler(this.执行SQLToolStripMenuItem_Click);
            // 
            // createTableToolStripMenuItem
            // 
            this.createTableToolStripMenuItem.Name = "createTableToolStripMenuItem";
            this.createTableToolStripMenuItem.Size = new System.Drawing.Size(230, 38);
            this.createTableToolStripMenuItem.Text = "创建表";
            this.createTableToolStripMenuItem.Click += new System.EventHandler(this.createTableToolStripMenuItem_Click);
            // 
            // contextMenuStripTable
            // 
            this.contextMenuStripTable.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.生成代码ToolStripMenuItem,
            this.sQL查询ToolStripMenuItem,
            this.toolStripMenuItem6,
            this.toolStripMenuItem4,
            this.toolStripMenuItem3,
            this.dataOperateToolStripMenuItem,
            this.exportDataToolStripMenuItem,
            this.importDataToolStripMenuItem,
            this.toolStripMenuItem7,
            this.modifyTableToolStripMenuItem,
            this.copyFieldNameToolStripMenuItem,
            this.deleteTableToolStripMenuItem,
            this.toolStripMenuItem13});
            this.contextMenuStripTable.Name = "contextMenuStripOneDataBase";
            this.contextMenuStripTable.Size = new System.Drawing.Size(301, 414);
            // 
            // 生成代码ToolStripMenuItem
            // 
            this.生成代码ToolStripMenuItem.Name = "生成代码ToolStripMenuItem";
            this.生成代码ToolStripMenuItem.Size = new System.Drawing.Size(300, 38);
            this.生成代码ToolStripMenuItem.Text = "查看表结构";
            this.生成代码ToolStripMenuItem.Click += new System.EventHandler(this.生成代码ToolStripMenuItem_Click);
            // 
            // sQL查询ToolStripMenuItem
            // 
            this.sQL查询ToolStripMenuItem.Name = "sQL查询ToolStripMenuItem";
            this.sQL查询ToolStripMenuItem.Size = new System.Drawing.Size(280, 38);
            this.sQL查询ToolStripMenuItem.Text = "SQL查询窗口";
            this.sQL查询ToolStripMenuItem.Click += new System.EventHandler(this.sQL查询ToolStripMenuItem_Click);
            // 
            // copyFieldNameToolStripMenuItem
            // 
            this.copyFieldNameToolStripMenuItem.Name = "copyFieldNameToolStripMenuItem";
            this.copyFieldNameToolStripMenuItem.Size = new System.Drawing.Size(280, 38);
            this.copyFieldNameToolStripMenuItem.Text = "复制表名";
            this.copyFieldNameToolStripMenuItem.Click += new System.EventHandler(this.copyFieldNameToolStripMenuItem_Click);
            // 
            // modifyTableToolStripMenuItem
            // 
            this.modifyTableToolStripMenuItem.Name = "modifyTableToolStripMenuItem";
            this.modifyTableToolStripMenuItem.Size = new System.Drawing.Size(280, 38);
            this.modifyTableToolStripMenuItem.Text = "修改表";
            this.modifyTableToolStripMenuItem.Click += new System.EventHandler(this.modifyTableToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(277, 6);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(280, 38);
            this.toolStripMenuItem4.Text = "快捷生成业务代码";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(277, 6);
            // 
            // dataOperateToolStripMenuItem
            // 
            this.dataOperateToolStripMenuItem.Name = "dataOperateToolStripMenuItem";
            this.dataOperateToolStripMenuItem.Size = new System.Drawing.Size(280, 38);
            this.dataOperateToolStripMenuItem.Text = "编辑数据";
            this.dataOperateToolStripMenuItem.Click += new System.EventHandler(this.dataOperateToolStripMenuItem_Click);
            // 
            // exportDataToolStripMenuItem
            // 
            this.exportDataToolStripMenuItem.Name = "exportDataToolStripMenuItem";
            this.exportDataToolStripMenuItem.Size = new System.Drawing.Size(280, 38);
            this.exportDataToolStripMenuItem.Text = "导出数据";
            this.exportDataToolStripMenuItem.Click += new System.EventHandler(this.exportDataToolStripMenuItem_Click);
            // 
            // importDataToolStripMenuItem
            // 
            this.importDataToolStripMenuItem.Name = "importDataToolStripMenuItem";
            this.importDataToolStripMenuItem.Size = new System.Drawing.Size(280, 38);
            this.importDataToolStripMenuItem.Text = "导入数据";
            this.importDataToolStripMenuItem.Click += new System.EventHandler(this.importDataToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(277, 6);
            // 
            // deleteTableToolStripMenuItem
            // 
            this.deleteTableToolStripMenuItem.Name = "deleteTableToolStripMenuItem";
            this.deleteTableToolStripMenuItem.Size = new System.Drawing.Size(280, 38);
            this.deleteTableToolStripMenuItem.Text = "删除表";
            this.deleteTableToolStripMenuItem.Click += new System.EventHandler(this.deleteTableToolStripMenuItem_Click);
            // 
            // contextMenuStripOneMongoDB
            // 
            this.contextMenuStripOneMongoDB.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripOneMongoDB.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStripOneMongoDB.Name = "contextMenuStripOneMongoDB";
            this.contextMenuStripOneMongoDB.Size = new System.Drawing.Size(137, 80);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(136, 38);
            this.toolStripMenuItem1.Text = "刷新";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(136, 38);
            this.toolStripMenuItem2.Text = "查询";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchTextBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.searchTextBox.Location = new System.Drawing.Point(0, 30);
            this.searchTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(354, 39);
            this.searchTextBox.TabIndex = 5;
            this.searchTextBox.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.searchTextBox.WaterText = "查询";
            this.searchTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchTextBox_KeyUp);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(297, 6);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(253, 6);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(297, 6);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(297, 6);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(297, 6);
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(297, 6);
            // 
            // LeftPanelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 728);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.Treeview);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "LeftPanelForm";
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
        private System.Windows.Forms.ToolStripMenuItem 执行SQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sQL查询ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOneMongoDB;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem viewConnectStringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyFieldNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteTableToolStripMenuItem;
        private CCWin.SkinControl.SkinWaterTextBox searchTextBox;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem importDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem dataOperateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modifyTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem12;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem13;
    }
}