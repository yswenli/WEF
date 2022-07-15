namespace WEF.ModelGenerator.Forms
{
	public partial class Base64Form{
        private CCWin.SkinControl.SkinWaterTextBox skinWaterTextBox2;
        private CCWin.SkinControl.SkinButton skinButton2;
        private System.ComponentModel.IContainer components;
        private CCWin.SkinControl.SkinButton skinButton1;
        private CCWin.SkinControl.SkinWaterTextBox skinWaterTextBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox1;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Base64Form));
            this.skinGroupBox1 = new CCWin.SkinControl.SkinGroupBox();
            this.skinWaterTextBox2 = new CCWin.SkinControl.SkinWaterTextBox();
            this.skinButton2 = new CCWin.SkinControl.SkinButton();
            this.skinButton1 = new CCWin.SkinControl.SkinButton();
            this.skinWaterTextBox1 = new CCWin.SkinControl.SkinWaterTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.skinGroupBox3 = new CCWin.SkinControl.SkinGroupBox();
            this.skinButton5 = new CCWin.SkinControl.SkinButton();
            this.skinButton6 = new CCWin.SkinControl.SkinButton();
            this.button2 = new System.Windows.Forms.Button();
            this.skinWaterTextBox6 = new CCWin.SkinControl.SkinWaterTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.skinWaterTextBox5 = new CCWin.SkinControl.SkinWaterTextBox();
            this.skinGroupBox1.SuspendLayout();
            this.skinGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinGroupBox1
            // 
            this.skinGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.BorderColor = System.Drawing.SystemColors.ControlLight;
            this.skinGroupBox1.Controls.Add(this.skinWaterTextBox2);
            this.skinGroupBox1.Controls.Add(this.skinButton2);
            this.skinGroupBox1.Controls.Add(this.skinButton1);
            this.skinGroupBox1.Controls.Add(this.skinWaterTextBox1);
            this.skinGroupBox1.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinGroupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.skinGroupBox1.Location = new System.Drawing.Point(39, 78);
            this.skinGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.skinGroupBox1.Name = "skinGroupBox1";
            this.skinGroupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.skinGroupBox1.RectBackColor = System.Drawing.Color.White;
            this.skinGroupBox1.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox1.Size = new System.Drawing.Size(1422, 748);
            this.skinGroupBox1.TabIndex = 0;
            this.skinGroupBox1.TabStop = false;
            this.skinGroupBox1.Text = "字符串Base64转换";
            this.skinGroupBox1.TitleBorderColor = System.Drawing.Color.White;
            this.skinGroupBox1.TitleRectBackColor = System.Drawing.Color.White;
            this.skinGroupBox1.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // skinWaterTextBox2
            // 
            this.skinWaterTextBox2.Location = new System.Drawing.Point(770, 74);
            this.skinWaterTextBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.skinWaterTextBox2.MaxLength = 30000;
            this.skinWaterTextBox2.Multiline = true;
            this.skinWaterTextBox2.Name = "skinWaterTextBox2";
            this.skinWaterTextBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.skinWaterTextBox2.Size = new System.Drawing.Size(642, 664);
            this.skinWaterTextBox2.TabIndex = 3;
            this.skinWaterTextBox2.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinWaterTextBox2.WaterText = "Base64字符串";
            // 
            // skinButton2
            // 
            this.skinButton2.BackColor = System.Drawing.Color.Transparent;
            this.skinButton2.BaseColor = System.Drawing.SystemColors.Control;
            this.skinButton2.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.skinButton2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton2.DownBack = null;
            this.skinButton2.DownBaseColor = System.Drawing.SystemColors.ControlLightLight;
            this.skinButton2.Location = new System.Drawing.Point(668, 419);
            this.skinButton2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.skinButton2.MouseBack = null;
            this.skinButton2.MouseBaseColor = System.Drawing.SystemColors.ControlLight;
            this.skinButton2.Name = "skinButton2";
            this.skinButton2.NormlBack = null;
            this.skinButton2.Radius = 15;
            this.skinButton2.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinButton2.Size = new System.Drawing.Size(93, 88);
            this.skinButton2.TabIndex = 2;
            this.skinButton2.Text = "《";
            this.skinButton2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.skinButton2.UseVisualStyleBackColor = false;
            this.skinButton2.Click += new System.EventHandler(this.skinButton2_Click);
            // 
            // skinButton1
            // 
            this.skinButton1.BackColor = System.Drawing.Color.Transparent;
            this.skinButton1.BaseColor = System.Drawing.SystemColors.Control;
            this.skinButton1.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.skinButton1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton1.DownBack = null;
            this.skinButton1.DownBaseColor = System.Drawing.SystemColors.ControlDark;
            this.skinButton1.Location = new System.Drawing.Point(668, 310);
            this.skinButton1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.skinButton1.MouseBack = null;
            this.skinButton1.MouseBaseColor = System.Drawing.SystemColors.ControlLight;
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.NormlBack = null;
            this.skinButton1.Radius = 15;
            this.skinButton1.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinButton1.Size = new System.Drawing.Size(93, 88);
            this.skinButton1.TabIndex = 1;
            this.skinButton1.Text = "》";
            this.skinButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.skinButton1.UseVisualStyleBackColor = false;
            this.skinButton1.Click += new System.EventHandler(this.skinButton1_Click);
            // 
            // skinWaterTextBox1
            // 
            this.skinWaterTextBox1.Location = new System.Drawing.Point(10, 74);
            this.skinWaterTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.skinWaterTextBox1.MaxLength = 30000;
            this.skinWaterTextBox1.Multiline = true;
            this.skinWaterTextBox1.Name = "skinWaterTextBox1";
            this.skinWaterTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.skinWaterTextBox1.Size = new System.Drawing.Size(646, 664);
            this.skinWaterTextBox1.TabIndex = 0;
            this.skinWaterTextBox1.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinWaterTextBox1.WaterText = "请输入字符串";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Title = "Base64转换";
            // 
            // skinGroupBox3
            // 
            this.skinGroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinGroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox3.BorderColor = System.Drawing.SystemColors.ControlLight;
            this.skinGroupBox3.Controls.Add(this.button3);
            this.skinGroupBox3.Controls.Add(this.skinWaterTextBox5);
            this.skinGroupBox3.Controls.Add(this.skinButton5);
            this.skinGroupBox3.Controls.Add(this.skinButton6);
            this.skinGroupBox3.Controls.Add(this.button2);
            this.skinGroupBox3.Controls.Add(this.skinWaterTextBox6);
            this.skinGroupBox3.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinGroupBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.skinGroupBox3.Location = new System.Drawing.Point(39, 836);
            this.skinGroupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.skinGroupBox3.Name = "skinGroupBox3";
            this.skinGroupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.skinGroupBox3.RectBackColor = System.Drawing.Color.White;
            this.skinGroupBox3.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox3.Size = new System.Drawing.Size(1422, 333);
            this.skinGroupBox3.TabIndex = 2;
            this.skinGroupBox3.TabStop = false;
            this.skinGroupBox3.Text = "文件Base64转换";
            this.skinGroupBox3.TitleBorderColor = System.Drawing.Color.White;
            this.skinGroupBox3.TitleRectBackColor = System.Drawing.Color.White;
            this.skinGroupBox3.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // skinButton5
            // 
            this.skinButton5.BackColor = System.Drawing.Color.Transparent;
            this.skinButton5.BaseColor = System.Drawing.SystemColors.Control;
            this.skinButton5.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.skinButton5.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton5.DownBack = null;
            this.skinButton5.DownBaseColor = System.Drawing.SystemColors.ControlDark;
            this.skinButton5.Location = new System.Drawing.Point(668, 172);
            this.skinButton5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.skinButton5.MouseBack = null;
            this.skinButton5.MouseBaseColor = System.Drawing.SystemColors.ControlLight;
            this.skinButton5.Name = "skinButton5";
            this.skinButton5.NormlBack = null;
            this.skinButton5.Radius = 15;
            this.skinButton5.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinButton5.Size = new System.Drawing.Size(93, 88);
            this.skinButton5.TabIndex = 4;
            this.skinButton5.Text = "《";
            this.skinButton5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.skinButton5.UseVisualStyleBackColor = false;
            this.skinButton5.Click += new System.EventHandler(this.skinButton5_Click);
            // 
            // skinButton6
            // 
            this.skinButton6.BackColor = System.Drawing.Color.Transparent;
            this.skinButton6.BaseColor = System.Drawing.SystemColors.Control;
            this.skinButton6.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.skinButton6.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton6.DownBack = null;
            this.skinButton6.DownBaseColor = System.Drawing.SystemColors.ControlDark;
            this.skinButton6.Location = new System.Drawing.Point(668, 74);
            this.skinButton6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.skinButton6.MouseBack = null;
            this.skinButton6.MouseBaseColor = System.Drawing.SystemColors.ControlLight;
            this.skinButton6.Name = "skinButton6";
            this.skinButton6.NormlBack = null;
            this.skinButton6.Radius = 15;
            this.skinButton6.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinButton6.Size = new System.Drawing.Size(93, 88);
            this.skinButton6.TabIndex = 3;
            this.skinButton6.Text = "》";
            this.skinButton6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.skinButton6.UseVisualStyleBackColor = false;
            this.skinButton6.Click += new System.EventHandler(this.skinButton6_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(591, 162);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 56);
            this.button2.TabIndex = 1;
            this.button2.Text = "..";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // skinWaterTextBox6
            // 
            this.skinWaterTextBox6.Location = new System.Drawing.Point(10, 165);
            this.skinWaterTextBox6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.skinWaterTextBox6.Name = "skinWaterTextBox6";
            this.skinWaterTextBox6.Size = new System.Drawing.Size(584, 46);
            this.skinWaterTextBox6.TabIndex = 0;
            this.skinWaterTextBox6.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinWaterTextBox6.WaterText = "文件地址";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1351, 159);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(45, 56);
            this.button3.TabIndex = 6;
            this.button3.Text = "..";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // skinWaterTextBox5
            // 
            this.skinWaterTextBox5.Location = new System.Drawing.Point(770, 162);
            this.skinWaterTextBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.skinWaterTextBox5.Name = "skinWaterTextBox5";
            this.skinWaterTextBox5.Size = new System.Drawing.Size(584, 46);
            this.skinWaterTextBox5.TabIndex = 5;
            this.skinWaterTextBox5.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinWaterTextBox5.WaterText = "Base64文本文件地址";
            // 
            // Base64Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.ClientSize = new System.Drawing.Size(1490, 1204);
            this.Controls.Add(this.skinGroupBox3);
            this.Controls.Add(this.skinGroupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Base64Form";
            this.Text = "Base64转换-Developed by Mason";
            this.Load += new System.EventHandler(this.Base64Form_Load);
            this.skinGroupBox1.ResumeLayout(false);
            this.skinGroupBox1.PerformLayout();
            this.skinGroupBox3.ResumeLayout(false);
            this.skinGroupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        private CCWin.SkinControl.SkinGroupBox skinGroupBox3;
        private System.Windows.Forms.Button button3;
        private CCWin.SkinControl.SkinWaterTextBox skinWaterTextBox5;
        private CCWin.SkinControl.SkinButton skinButton5;
        private CCWin.SkinControl.SkinButton skinButton6;
        private System.Windows.Forms.Button button2;
        private CCWin.SkinControl.SkinWaterTextBox skinWaterTextBox6;
    }
}