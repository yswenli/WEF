namespace WEF.Standard.DevelopTools
{
    partial class QrCodeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QrCodeForm));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabEncode = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.attrPictureBox = new System.Windows.Forms.PictureBox();
            this.attributeBtn = new System.Windows.Forms.Button();
            this.cboCorrectionLevel = new System.Windows.Forms.ComboBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.btnEncode = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cboVersion = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboEncoding = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEncodeData = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.picEncode = new System.Windows.Forms.PictureBox();
            this.tabDecode = new System.Windows.Forms.TabPage();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnDecode = new System.Windows.Forms.Button();
            this.txtDecodedData = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.picDecode = new System.Windows.Forms.PictureBox();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.label7 = new System.Windows.Forms.Label();
            this.sizeLabel = new CCWin.SkinControl.SkinLabel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabMain.SuspendLayout();
            this.tabEncode.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.attrPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEncode)).BeginInit();
            this.tabDecode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDecode)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabEncode);
            this.tabMain.Controls.Add(this.tabDecode);
            this.tabMain.Location = new System.Drawing.Point(19, 41);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(471, 426);
            this.tabMain.TabIndex = 0;
            // 
            // tabEncode
            // 
            this.tabEncode.Controls.Add(this.sizeLabel);
            this.tabEncode.Controls.Add(this.panel1);
            this.tabEncode.Controls.Add(this.radioButton2);
            this.tabEncode.Controls.Add(this.radioButton1);
            this.tabEncode.Controls.Add(this.attrPictureBox);
            this.tabEncode.Controls.Add(this.attributeBtn);
            this.tabEncode.Controls.Add(this.cboCorrectionLevel);
            this.tabEncode.Controls.Add(this.btnPrint);
            this.tabEncode.Controls.Add(this.label5);
            this.tabEncode.Controls.Add(this.btnSave);
            this.tabEncode.Controls.Add(this.txtSize);
            this.tabEncode.Controls.Add(this.btnEncode);
            this.tabEncode.Controls.Add(this.label4);
            this.tabEncode.Controls.Add(this.cboVersion);
            this.tabEncode.Controls.Add(this.label3);
            this.tabEncode.Controls.Add(this.cboEncoding);
            this.tabEncode.Controls.Add(this.label2);
            this.tabEncode.Controls.Add(this.txtEncodeData);
            this.tabEncode.Controls.Add(this.label1);
            this.tabEncode.Controls.Add(this.picEncode);
            this.tabEncode.Location = new System.Drawing.Point(4, 22);
            this.tabEncode.Name = "tabEncode";
            this.tabEncode.Padding = new System.Windows.Forms.Padding(3);
            this.tabEncode.Size = new System.Drawing.Size(463, 400);
            this.tabEncode.TabIndex = 0;
            this.tabEncode.Text = "加密";
            this.tabEncode.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton4);
            this.panel1.Controls.Add(this.radioButton3);
            this.panel1.Location = new System.Drawing.Point(8, 351);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(127, 32);
            this.panel1.TabIndex = 14;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(57, 5);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(47, 16);
            this.radioButton4.TabIndex = 13;
            this.radioButton4.Text = "背景";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(4, 5);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(47, 16);
            this.radioButton3.TabIndex = 13;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "前景";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(65, 329);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 13;
            this.radioButton2.Text = "图标";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 329);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 13;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "图片";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // attrPictureBox
            // 
            this.attrPictureBox.BackColor = System.Drawing.Color.White;
            this.attrPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.attrPictureBox.Location = new System.Drawing.Point(241, 10);
            this.attrPictureBox.Name = "attrPictureBox";
            this.attrPictureBox.Size = new System.Drawing.Size(192, 192);
            this.attrPictureBox.TabIndex = 12;
            this.attrPictureBox.TabStop = false;
            // 
            // attributeBtn
            // 
            this.attributeBtn.Location = new System.Drawing.Point(118, 326);
            this.attributeBtn.Name = "attributeBtn";
            this.attributeBtn.Size = new System.Drawing.Size(75, 23);
            this.attributeBtn.TabIndex = 11;
            this.attributeBtn.Text = "附加图片";
            this.attributeBtn.UseVisualStyleBackColor = true;
            this.attributeBtn.Click += new System.EventHandler(this.attributeBtn_Click);
            // 
            // cboCorrectionLevel
            // 
            this.cboCorrectionLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCorrectionLevel.FormattingEnabled = true;
            this.cboCorrectionLevel.Items.AddRange(new object[] {
            "L",
            "M",
            "Q",
            "H"});
            this.cboCorrectionLevel.Location = new System.Drawing.Point(340, 265);
            this.cboCorrectionLevel.Name = "cboCorrectionLevel";
            this.cboCorrectionLevel.Size = new System.Drawing.Size(95, 20);
            this.cboCorrectionLevel.TabIndex = 10;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(341, 356);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(94, 38);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "打印";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(282, 268);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "当前级别";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(241, 356);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 38);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(340, 301);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(43, 21);
            this.txtSize.TabIndex = 8;
            this.txtSize.Text = "4";
            // 
            // btnEncode
            // 
            this.btnEncode.Location = new System.Drawing.Point(141, 356);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(94, 38);
            this.btnEncode.TabIndex = 1;
            this.btnEncode.Text = "生成";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(282, 304);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "大小";
            // 
            // cboVersion
            // 
            this.cboVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVersion.FormattingEnabled = true;
            this.cboVersion.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "40",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40"});
            this.cboVersion.Location = new System.Drawing.Point(70, 301);
            this.cboVersion.Name = "cboVersion";
            this.cboVersion.Size = new System.Drawing.Size(157, 20);
            this.cboVersion.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 305);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "版本";
            // 
            // cboEncoding
            // 
            this.cboEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEncoding.FormattingEnabled = true;
            this.cboEncoding.Items.AddRange(new object[] {
            "AlphaNumeric",
            "Numeric",
            "Byte"});
            this.cboEncoding.Location = new System.Drawing.Point(70, 266);
            this.cboEncoding.Name = "cboEncoding";
            this.cboEncoding.Size = new System.Drawing.Size(157, 20);
            this.cboEncoding.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 272);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "加密类型";
            // 
            // txtEncodeData
            // 
            this.txtEncodeData.Location = new System.Drawing.Point(69, 229);
            this.txtEncodeData.Name = "txtEncodeData";
            this.txtEncodeData.Size = new System.Drawing.Size(388, 21);
            this.txtEncodeData.TabIndex = 2;
            this.txtEncodeData.Text = "你好，world";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "内容";
            // 
            // picEncode
            // 
            this.picEncode.BackColor = System.Drawing.Color.White;
            this.picEncode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picEncode.ContextMenuStrip = this.contextMenuStrip1;
            this.picEncode.Location = new System.Drawing.Point(26, 9);
            this.picEncode.Name = "picEncode";
            this.picEncode.Size = new System.Drawing.Size(192, 192);
            this.picEncode.TabIndex = 0;
            this.picEncode.TabStop = false;
            // 
            // tabDecode
            // 
            this.tabDecode.Controls.Add(this.btnOpen);
            this.tabDecode.Controls.Add(this.btnDecode);
            this.tabDecode.Controls.Add(this.txtDecodedData);
            this.tabDecode.Controls.Add(this.label6);
            this.tabDecode.Controls.Add(this.picDecode);
            this.tabDecode.Location = new System.Drawing.Point(4, 22);
            this.tabDecode.Name = "tabDecode";
            this.tabDecode.Padding = new System.Windows.Forms.Padding(3);
            this.tabDecode.Size = new System.Drawing.Size(463, 400);
            this.tabDecode.TabIndex = 1;
            this.tabDecode.Text = "解码";
            this.tabDecode.UseVisualStyleBackColor = true;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(83, 203);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(40, 25);
            this.btnOpen.TabIndex = 7;
            this.btnOpen.Text = "...";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnDecode
            // 
            this.btnDecode.Location = new System.Drawing.Point(361, 208);
            this.btnDecode.Name = "btnDecode";
            this.btnDecode.Size = new System.Drawing.Size(94, 36);
            this.btnDecode.TabIndex = 4;
            this.btnDecode.Text = "解码";
            this.btnDecode.UseVisualStyleBackColor = true;
            this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click_1);
            // 
            // txtDecodedData
            // 
            this.txtDecodedData.Location = new System.Drawing.Point(8, 252);
            this.txtDecodedData.Multiline = true;
            this.txtDecodedData.Name = "txtDecodedData";
            this.txtDecodedData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDecodedData.Size = new System.Drawing.Size(449, 142);
            this.txtDecodedData.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "请选择二维码";
            // 
            // picDecode
            // 
            this.picDecode.BackColor = System.Drawing.Color.White;
            this.picDecode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picDecode.Location = new System.Drawing.Point(6, 6);
            this.picDecode.Name = "picDecode";
            this.picDecode.Size = new System.Drawing.Size(451, 192);
            this.picDecode.TabIndex = 3;
            this.picDecode.TabStop = false;
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(351, 474);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "developed by li.wen 2013";
            // 
            // sizeLabel
            // 
            this.sizeLabel.BackColor = System.Drawing.Color.Transparent;
            this.sizeLabel.BorderColor = System.Drawing.Color.White;
            this.sizeLabel.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.sizeLabel.Location = new System.Drawing.Point(26, 203);
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size(192, 23);
            this.sizeLabel.TabIndex = 15;
            this.sizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.saveToolStripMenuItem.Text = "保存";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // QrCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 492);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tabMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "QrCodeForm";
            this.Text = "二维码工具";
            this.Load += new System.EventHandler(this.QrCodeForm_Load);
            this.tabMain.ResumeLayout(false);
            this.tabEncode.ResumeLayout(false);
            this.tabEncode.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.attrPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEncode)).EndInit();
            this.tabDecode.ResumeLayout(false);
            this.tabDecode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDecode)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabEncode;
        private System.Windows.Forms.TabPage tabDecode;
        private System.Windows.Forms.PictureBox picEncode;
        private System.Windows.Forms.TextBox txtEncodeData;
        private System.Windows.Forms.ComboBox cboCorrectionLevel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboVersion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboEncoding;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEncode;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnDecode;
        private System.Windows.Forms.TextBox txtDecodedData;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox picDecode;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnOpen;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button attributeBtn;
        private System.Windows.Forms.PictureBox attrPictureBox;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private CCWin.SkinControl.SkinLabel sizeLabel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    }
}

