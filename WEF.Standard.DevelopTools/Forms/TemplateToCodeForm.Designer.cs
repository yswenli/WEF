
namespace WEF.Standard.DevelopTools.Forms
{
    partial class TemplateToCodeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateToCodeForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.skinButton2 = new CCWin.SkinControl.SkinButton();
            this.skinButton1 = new CCWin.SkinControl.SkinButton();
            this.skinWaterTextBox3 = new CCWin.SkinControl.SkinWaterTextBox();
            this.skinWaterTextBox2 = new CCWin.SkinControl.SkinWaterTextBox();
            this.skinComboBox2 = new CCWin.SkinControl.SkinComboBox();
            this.skinComboBox1 = new CCWin.SkinControl.SkinComboBox();
            this.skinWaterTextBox1 = new CCWin.SkinControl.SkinWaterTextBox();
            this.skinListBox1 = new CCWin.SkinControl.SkinListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skinButton3 = new CCWin.SkinControl.SkinButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.skinButton2);
            this.groupBox1.Controls.Add(this.skinButton1);
            this.groupBox1.Controls.Add(this.skinWaterTextBox3);
            this.groupBox1.Controls.Add(this.skinWaterTextBox2);
            this.groupBox1.Controls.Add(this.skinComboBox2);
            this.groupBox1.Controls.Add(this.skinComboBox1);
            this.groupBox1.Controls.Add(this.skinWaterTextBox1);
            this.groupBox1.Location = new System.Drawing.Point(7, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(641, 306);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "配置";
            // 
            // skinButton2
            // 
            this.skinButton2.BackColor = System.Drawing.Color.Transparent;
            this.skinButton2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton2.DownBack = null;
            this.skinButton2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton2.Location = new System.Drawing.Point(529, 247);
            this.skinButton2.MouseBack = null;
            this.skinButton2.Name = "skinButton2";
            this.skinButton2.NormlBack = null;
            this.skinButton2.Size = new System.Drawing.Size(104, 41);
            this.skinButton2.TabIndex = 6;
            this.skinButton2.Text = "添加配置";
            this.skinButton2.UseVisualStyleBackColor = false;
            this.skinButton2.Click += new System.EventHandler(this.skinButton2_Click);
            // 
            // skinButton1
            // 
            this.skinButton1.BackColor = System.Drawing.Color.Transparent;
            this.skinButton1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton1.DownBack = null;
            this.skinButton1.Location = new System.Drawing.Point(587, 206);
            this.skinButton1.MouseBack = null;
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.NormlBack = null;
            this.skinButton1.Size = new System.Drawing.Size(46, 28);
            this.skinButton1.TabIndex = 5;
            this.skinButton1.Text = "..";
            this.skinButton1.UseVisualStyleBackColor = false;
            this.skinButton1.Click += new System.EventHandler(this.skinButton1_Click);
            // 
            // skinWaterTextBox3
            // 
            this.skinWaterTextBox3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinWaterTextBox3.Location = new System.Drawing.Point(6, 206);
            this.skinWaterTextBox3.Name = "skinWaterTextBox3";
            this.skinWaterTextBox3.Size = new System.Drawing.Size(585, 27);
            this.skinWaterTextBox3.TabIndex = 3;
            this.skinWaterTextBox3.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinWaterTextBox3.WaterText = "将要加入的项目文件地址";
            // 
            // skinWaterTextBox2
            // 
            this.skinWaterTextBox2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinWaterTextBox2.Location = new System.Drawing.Point(6, 118);
            this.skinWaterTextBox2.Name = "skinWaterTextBox2";
            this.skinWaterTextBox2.Size = new System.Drawing.Size(627, 27);
            this.skinWaterTextBox2.TabIndex = 2;
            this.skinWaterTextBox2.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinWaterTextBox2.WaterText = "替换名";
            // 
            // skinComboBox2
            // 
            this.skinComboBox2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.skinComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.skinComboBox2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinComboBox2.FormattingEnabled = true;
            this.skinComboBox2.Items.AddRange(new object[] {
            "Operator",
            "Service",
            "Controller"});
            this.skinComboBox2.Location = new System.Drawing.Point(6, 161);
            this.skinComboBox2.Name = "skinComboBox2";
            this.skinComboBox2.Size = new System.Drawing.Size(627, 28);
            this.skinComboBox2.TabIndex = 1;
            this.skinComboBox2.WaterText = "请选择代码模板";
            // 
            // skinComboBox1
            // 
            this.skinComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.skinComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.skinComboBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinComboBox1.FormattingEnabled = true;
            this.skinComboBox1.Location = new System.Drawing.Point(7, 34);
            this.skinComboBox1.Name = "skinComboBox1";
            this.skinComboBox1.Size = new System.Drawing.Size(626, 28);
            this.skinComboBox1.TabIndex = 1;
            this.skinComboBox1.WaterText = "请选择代码模板";
            // 
            // skinWaterTextBox1
            // 
            this.skinWaterTextBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinWaterTextBox1.Location = new System.Drawing.Point(6, 76);
            this.skinWaterTextBox1.Name = "skinWaterTextBox1";
            this.skinWaterTextBox1.Size = new System.Drawing.Size(627, 27);
            this.skinWaterTextBox1.TabIndex = 0;
            this.skinWaterTextBox1.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinWaterTextBox1.WaterText = "命名空间";
            // 
            // skinListBox1
            // 
            this.skinListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinListBox1.Back = null;
            this.skinListBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.skinListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.skinListBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinListBox1.FormattingEnabled = true;
            this.skinListBox1.ItemHeight = 25;
            this.skinListBox1.Location = new System.Drawing.Point(7, 356);
            this.skinListBox1.MouseColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.skinListBox1.Name = "skinListBox1";
            this.skinListBox1.RowBackColor2 = System.Drawing.SystemColors.ActiveCaption;
            this.skinListBox1.ScrollAlwaysVisible = true;
            this.skinListBox1.SelectedColor = System.Drawing.SystemColors.Highlight;
            this.skinListBox1.Size = new System.Drawing.Size(641, 279);
            this.skinListBox1.TabIndex = 1;
            this.skinListBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.skinListBox1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modifyToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(109, 52);
            // 
            // modifyToolStripMenuItem
            // 
            this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            this.modifyToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.modifyToolStripMenuItem.Text = "修改";
            this.modifyToolStripMenuItem.Click += new System.EventHandler(this.modifyToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.deleteToolStripMenuItem.Text = "删除";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // skinButton3
            // 
            this.skinButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.skinButton3.BackColor = System.Drawing.Color.Transparent;
            this.skinButton3.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton3.DownBack = null;
            this.skinButton3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton3.Location = new System.Drawing.Point(544, 651);
            this.skinButton3.MouseBack = null;
            this.skinButton3.Name = "skinButton3";
            this.skinButton3.NormlBack = null;
            this.skinButton3.Size = new System.Drawing.Size(104, 41);
            this.skinButton3.TabIndex = 7;
            this.skinButton3.Text = "生成代码";
            this.skinButton3.UseVisualStyleBackColor = false;
            this.skinButton3.Click += new System.EventHandler(this.skinButton3_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.csproj";
            this.openFileDialog1.Filter = "项目文件|*.csproj";
            this.openFileDialog1.Title = "请选择项目文件";
            // 
            // TemplateToCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 708);
            this.Controls.Add(this.skinButton3);
            this.Controls.Add(this.skinListBox1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TemplateToCodeForm";
            this.Text = "模板代码生成工具";
            this.Load += new System.EventHandler(this.TemplateToCodeForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private CCWin.SkinControl.SkinWaterTextBox skinWaterTextBox1;
        private CCWin.SkinControl.SkinComboBox skinComboBox1;
        private CCWin.SkinControl.SkinWaterTextBox skinWaterTextBox2;
        private CCWin.SkinControl.SkinWaterTextBox skinWaterTextBox3;
        private CCWin.SkinControl.SkinButton skinButton1;
        private CCWin.SkinControl.SkinButton skinButton2;
        private CCWin.SkinControl.SkinListBox skinListBox1;
        private CCWin.SkinControl.SkinButton skinButton3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private CCWin.SkinControl.SkinComboBox skinComboBox2;
    }
}