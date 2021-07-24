namespace WEF.ModelGenerator
{
    partial class ContentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContentForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tp1 = new System.Windows.Forms.TabPage();
            this.gridColumns = new System.Windows.Forms.DataGridView();
            this.contextMenuCopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设为主键ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消主键ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cbToupperFrstword = new System.Windows.Forms.CheckBox();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtnamespace = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbPrimarykey = new System.Windows.Forms.ComboBox();
            this.tp2 = new System.Windows.Forms.TabPage();
            this.txtContent = new System.Windows.Forms.RichTextBox();
            this.contextMenuStripSave = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.cnnTxt = new System.Windows.Forms.TextBox();
            this.saveEntity = new System.Windows.Forms.SaveFileDialog();
            this.button4 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tp1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridColumns)).BeginInit();
            this.contextMenuCopy.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tp2.SuspendLayout();
            this.contextMenuStripSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tp1);
            this.tabControl1.Controls.Add(this.tp2);
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 30);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(960, 806);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabControl1.Click += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tp1
            // 
            this.tp1.Controls.Add(this.gridColumns);
            this.tp1.Controls.Add(this.groupBox1);
            this.tp1.ImageIndex = 0;
            this.tp1.Location = new System.Drawing.Point(4, 4);
            this.tp1.Margin = new System.Windows.Forms.Padding(4);
            this.tp1.Name = "tp1";
            this.tp1.Padding = new System.Windows.Forms.Padding(4);
            this.tp1.Size = new System.Drawing.Size(952, 777);
            this.tp1.TabIndex = 0;
            this.tp1.Text = "生成设置";
            this.tp1.UseVisualStyleBackColor = true;
            // 
            // gridColumns
            // 
            this.gridColumns.AllowUserToAddRows = false;
            this.gridColumns.AllowUserToDeleteRows = false;
            this.gridColumns.AllowUserToOrderColumns = true;
            this.gridColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridColumns.ContextMenuStrip = this.contextMenuCopy;
            this.gridColumns.Location = new System.Drawing.Point(4, 4);
            this.gridColumns.Margin = new System.Windows.Forms.Padding(4);
            this.gridColumns.Name = "gridColumns";
            this.gridColumns.RowHeadersWidth = 51;
            this.gridColumns.RowTemplate.Height = 23;
            this.gridColumns.Size = new System.Drawing.Size(941, 394);
            this.gridColumns.TabIndex = 4;
            // 
            // contextMenuCopy
            // 
            this.contextMenuCopy.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.设为主键ToolStripMenuItem,
            this.取消主键ToolStripMenuItem});
            this.contextMenuCopy.Name = "contextMenuCopy";
            this.contextMenuCopy.Size = new System.Drawing.Size(139, 76);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.copyToolStripMenuItem.Text = "复制";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // 设为主键ToolStripMenuItem
            // 
            this.设为主键ToolStripMenuItem.Name = "设为主键ToolStripMenuItem";
            this.设为主键ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.设为主键ToolStripMenuItem.Text = "设为主键";
            this.设为主键ToolStripMenuItem.Click += new System.EventHandler(this.设为主键ToolStripMenuItem_Click);
            // 
            // 取消主键ToolStripMenuItem
            // 
            this.取消主键ToolStripMenuItem.Name = "取消主键ToolStripMenuItem";
            this.取消主键ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.取消主键ToolStripMenuItem.Text = "取消主键";
            this.取消主键ToolStripMenuItem.Click += new System.EventHandler(this.取消主键ToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.cbToupperFrstword);
            this.groupBox1.Controls.Add(this.txtClassName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtnamespace);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbPrimarykey);
            this.groupBox1.Location = new System.Drawing.Point(3, 406);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(941, 366);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "生成配置";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(378, 275);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(121, 44);
            this.button3.TabIndex = 8;
            this.button3.Text = "生成Json";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(120, 275);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(121, 44);
            this.button2.TabIndex = 8;
            this.button2.Text = "实体代码预览";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(249, 275);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 44);
            this.button1.TabIndex = 7;
            this.button1.Text = "保存代码文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(120, 220);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(134, 19);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "生成贫血实体类";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // cbToupperFrstword
            // 
            this.cbToupperFrstword.AutoSize = true;
            this.cbToupperFrstword.Checked = true;
            this.cbToupperFrstword.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbToupperFrstword.Location = new System.Drawing.Point(121, 193);
            this.cbToupperFrstword.Margin = new System.Windows.Forms.Padding(4);
            this.cbToupperFrstword.Name = "cbToupperFrstword";
            this.cbToupperFrstword.Size = new System.Drawing.Size(104, 19);
            this.cbToupperFrstword.TabIndex = 6;
            this.cbToupperFrstword.Text = "首字母大写";
            this.cbToupperFrstword.UseVisualStyleBackColor = true;
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(121, 147);
            this.txtClassName.Margin = new System.Windows.Forms.Padding(4);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(407, 25);
            this.txtClassName.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 151);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "类名:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 55);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "命名空间:";
            // 
            // txtnamespace
            // 
            this.txtnamespace.Location = new System.Drawing.Point(121, 52);
            this.txtnamespace.Margin = new System.Windows.Forms.Padding(4);
            this.txtnamespace.Name = "txtnamespace";
            this.txtnamespace.Size = new System.Drawing.Size(812, 25);
            this.txtnamespace.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 103);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "主键:";
            // 
            // cbPrimarykey
            // 
            this.cbPrimarykey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPrimarykey.FormattingEnabled = true;
            this.cbPrimarykey.Location = new System.Drawing.Point(121, 100);
            this.cbPrimarykey.Margin = new System.Windows.Forms.Padding(4);
            this.cbPrimarykey.Name = "cbPrimarykey";
            this.cbPrimarykey.Size = new System.Drawing.Size(207, 23);
            this.cbPrimarykey.TabIndex = 1;
            // 
            // tp2
            // 
            this.tp2.Controls.Add(this.txtContent);
            this.tp2.ImageIndex = 1;
            this.tp2.Location = new System.Drawing.Point(4, 4);
            this.tp2.Margin = new System.Windows.Forms.Padding(4);
            this.tp2.Name = "tp2";
            this.tp2.Padding = new System.Windows.Forms.Padding(4);
            this.tp2.Size = new System.Drawing.Size(952, 777);
            this.tp2.TabIndex = 1;
            this.tp2.Text = "实体代码预览";
            this.tp2.UseVisualStyleBackColor = true;
            // 
            // txtContent
            // 
            this.txtContent.ContextMenuStrip = this.contextMenuStripSave;
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtContent.Location = new System.Drawing.Point(4, 4);
            this.txtContent.Margin = new System.Windows.Forms.Padding(0);
            this.txtContent.Name = "txtContent";
            this.txtContent.ReadOnly = true;
            this.txtContent.Size = new System.Drawing.Size(944, 769);
            this.txtContent.TabIndex = 0;
            this.txtContent.Text = "请点击生成设置中的预览按钮";
            // 
            // contextMenuStripSave
            // 
            this.contextMenuStripSave.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripSave.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem1,
            this.selectAllToolStripMenuItem,
            this.保存ToolStripMenuItem});
            this.contextMenuStripSave.Name = "contextMenuStripSave";
            this.contextMenuStripSave.Size = new System.Drawing.Size(224, 76);
            // 
            // copyToolStripMenuItem1
            // 
            this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            this.copyToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem1.Size = new System.Drawing.Size(223, 24);
            this.copyToolStripMenuItem1.Text = "复制";
            this.copyToolStripMenuItem1.Click += new System.EventHandler(this.copyToolStripMenuItem1_Click);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(223, 24);
            this.selectAllToolStripMenuItem.Text = "全选";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(223, 24);
            this.保存ToolStripMenuItem.Text = "保存代码文件";
            this.保存ToolStripMenuItem.Click += new System.EventHandler(this.保存ToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "pz.ICO");
            this.imageList1.Images.SetKeyName(1, "cs.ICO");
            // 
            // cnnTxt
            // 
            this.cnnTxt.ContextMenuStrip = this.contextMenuCopy;
            this.cnnTxt.Dock = System.Windows.Forms.DockStyle.Top;
            this.cnnTxt.Location = new System.Drawing.Point(0, 0);
            this.cnnTxt.Margin = new System.Windows.Forms.Padding(4);
            this.cnnTxt.Name = "cnnTxt";
            this.cnnTxt.ReadOnly = true;
            this.cnnTxt.Size = new System.Drawing.Size(960, 25);
            this.cnnTxt.TabIndex = 7;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(536, 144);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(159, 30);
            this.button4.TabIndex = 9;
            this.button4.Text = "快捷生成业务代码";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // ContentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 839);
            this.Controls.Add(this.cnnTxt);
            this.Controls.Add(this.tabControl1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.Name = "ContentForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ContentForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tp1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridColumns)).EndInit();
            this.contextMenuCopy.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tp2.ResumeLayout(false);
            this.contextMenuStripSave.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp1;
        private System.Windows.Forms.TabPage tp2;
        private System.Windows.Forms.RichTextBox txtContent;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSave;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveEntity;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox cnnTxt;
        private System.Windows.Forms.DataGridView gridColumns;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbToupperFrstword;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtnamespace;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbPrimarykey;
        private System.Windows.Forms.ContextMenuStrip contextMenuCopy;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 设为主键ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消主键ToolStripMenuItem;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button4;
    }
}