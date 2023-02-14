/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.ModelGenerator.Forms
*文件名： DesignForm
*版本号： V1.0.0.0
*唯一标识：3cf87fb9-a481-426e-9f74-f69b189318dc
*当前的用户域：WALLE
*创建人： yswen
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/6/30 14:40:27
*描述：
*
*=================================================
*修改标记
*修改时间：2022/6/30 14:40:27
*修改人： yswenli
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
namespace WEF.ModelGenerator.Forms
{
    partial class DesignForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KeyColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.NotNullColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.AutoIncrementColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.InfoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeight = 46;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.TypeColumn,
            this.LengthColumn,
            this.KeyColumn,
            this.NotNullColumn,
            this.AutoIncrementColumn,
            this.InfoColumn});
            this.dataGridView1.Location = new System.Drawing.Point(13, 79);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.dataGridView1.RowTemplate.Height = 37;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(1530, 975);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(99, 27);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(518, 39);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "表名";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1409, 24);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 39);
            this.button1.TabIndex = 3;
            this.button1.Text = "确 定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // NameColumn
            // 
            this.NameColumn.FillWeight = 109.188F;
            this.NameColumn.HeaderText = "名称";
            this.NameColumn.MaxInputLength = 150;
            this.NameColumn.MinimumWidth = 10;
            this.NameColumn.Name = "NameColumn";
            // 
            // TypeColumn
            // 
            this.TypeColumn.FillWeight = 109.188F;
            this.TypeColumn.HeaderText = "类别";
            this.TypeColumn.Items.AddRange(new object[] {
            "NVarChar",
            "DateTime",
            "Bit",
            "Int",
            "Float",
            "Double",
            "Money",
            "Image"});
            this.TypeColumn.MinimumWidth = 10;
            this.TypeColumn.Name = "TypeColumn";
            // 
            // LengthColumn
            // 
            this.LengthColumn.FillWeight = 44.8718F;
            this.LengthColumn.HeaderText = "长度";
            this.LengthColumn.MinimumWidth = 10;
            this.LengthColumn.Name = "LengthColumn";
            this.LengthColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LengthColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // KeyColumn
            // 
            this.KeyColumn.FillWeight = 44.8718F;
            this.KeyColumn.HeaderText = "主键";
            this.KeyColumn.MinimumWidth = 10;
            this.KeyColumn.Name = "KeyColumn";
            // 
            // NotNullColumn
            // 
            this.NotNullColumn.FillWeight = 44.8718F;
            this.NotNullColumn.HeaderText = "必填";
            this.NotNullColumn.MinimumWidth = 10;
            this.NotNullColumn.Name = "NotNullColumn";
            // 
            // AutoIncrementColumn
            // 
            this.AutoIncrementColumn.FillWeight = 44.8718F;
            this.AutoIncrementColumn.HeaderText = "自增长";
            this.AutoIncrementColumn.MinimumWidth = 10;
            this.AutoIncrementColumn.Name = "AutoIncrementColumn";
            this.AutoIncrementColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.AutoIncrementColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // InfoColumn
            // 
            this.InfoColumn.FillWeight = 200.188F;
            this.InfoColumn.HeaderText = "描述";
            this.InfoColumn.MaxInputLength = 200;
            this.InfoColumn.MinimumWidth = 10;
            this.InfoColumn.Name = "InfoColumn";
            // 
            // DesignForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1559, 1067);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DesignForm";
            this.Text = "DesignForm";
            this.Load += new System.EventHandler(this.DesignForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn TypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LengthColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn KeyColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn NotNullColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn AutoIncrementColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn InfoColumn;
    }
}