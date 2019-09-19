namespace WEF.ModelGenerator
{
    partial class SQLForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLForm));
            this.cnnTxt = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbl_execute = new System.Windows.Forms.Label();
            this.contextMenuStripCopy = new CCWin.SkinControl.SkinContextMenuStrip();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStripCopy.SuspendLayout();
            this.SuspendLayout();
            // 
            // cnnTxt
            // 
            this.cnnTxt.Dock = System.Windows.Forms.DockStyle.Top;
            this.cnnTxt.Location = new System.Drawing.Point(0, 0);
            this.cnnTxt.Name = "cnnTxt";
            this.cnnTxt.ReadOnly = true;
            this.cnnTxt.Size = new System.Drawing.Size(848, 21);
            this.cnnTxt.TabIndex = 999;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStripCopy;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(848, 344);
            this.dataGridView1.TabIndex = 10;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(848, 693);
            this.splitContainer1.SplitterDistance = 345;
            this.splitContainer1.TabIndex = 11;
            // 
            // lbl_execute
            // 
            this.lbl_execute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbl_execute.AutoSize = true;
            this.lbl_execute.Location = new System.Drawing.Point(5, 723);
            this.lbl_execute.Name = "lbl_execute";
            this.lbl_execute.Size = new System.Drawing.Size(29, 12);
            this.lbl_execute.TabIndex = 12;
            this.lbl_execute.Text = "就绪";
            // 
            // contextMenuStripCopy
            // 
            this.contextMenuStripCopy.Arrow = System.Drawing.Color.Black;
            this.contextMenuStripCopy.Back = System.Drawing.Color.White;
            this.contextMenuStripCopy.BackRadius = 4;
            this.contextMenuStripCopy.Base = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.contextMenuStripCopy.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.contextMenuStripCopy.Fore = System.Drawing.Color.Black;
            this.contextMenuStripCopy.HoverFore = System.Drawing.Color.White;
            this.contextMenuStripCopy.ItemAnamorphosis = true;
            this.contextMenuStripCopy.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.contextMenuStripCopy.ItemBorderShow = true;
            this.contextMenuStripCopy.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.contextMenuStripCopy.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.contextMenuStripCopy.ItemRadius = 4;
            this.contextMenuStripCopy.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.contextMenuStripCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStripCopy.ItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.contextMenuStripCopy.Name = "contextMenuStripCopy";
            this.contextMenuStripCopy.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.contextMenuStripCopy.Size = new System.Drawing.Size(101, 26);
            this.contextMenuStripCopy.SkinAllColor = true;
            this.contextMenuStripCopy.TitleAnamorphosis = true;
            this.contextMenuStripCopy.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.contextMenuStripCopy.TitleRadius = 4;
            this.contextMenuStripCopy.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.copyToolStripMenuItem.Text = "复制";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // SQLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 740);
            this.Controls.Add(this.lbl_execute);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.cnnTxt);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SQLForm";
            this.Text = "SQLForm";
            this.Load += new System.EventHandler(this.SQLForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStripCopy.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox cnnTxt;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lbl_execute;
        private CCWin.SkinControl.SkinContextMenuStrip contextMenuStripCopy;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    }
}