using CCWin;
using Leigh.Wen.QRCode.Libs;
using Leigh.Wen.QRCode.Libs.Data;
using Leigh.Wen.QRCode.util;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WEF.ModelGenerator
{
    public partial class QrCodeForm : Skin_Mac
    {
        /// <summary>
        /// 外源图片像素集合
        /// </summary>
        Bitmap _attrBitmap;

        bool _asIcon = false;

        public QrCodeForm()
        {
            InitializeComponent();
        }

        private void QrCodeForm_Load(object sender, EventArgs e)
        {
            attrPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            picEncode.SizeMode = PictureBoxSizeMode.StretchImage;
            //
            cboEncoding.SelectedIndex = 2;
            cboVersion.SelectedIndex = 6;
            cboCorrectionLevel.SelectedIndex = 1;
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEncode_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txtEncodeData.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("内容不能为空");
                    return;
                }

                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                String encoding = cboEncoding.Text;
                if (encoding == "Byte")
                {
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                }
                else if (encoding == "AlphaNumeric")
                {
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                }
                else if (encoding == "Numeric")
                {
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
                }
                try
                {
                    int scale = Convert.ToInt16(txtSize.Text);
                    qrCodeEncoder.QRCodeScale = scale;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误的大小,ex:" + ex.Message);
                    return;
                }
                try
                {
                    int version = Convert.ToInt16(cboVersion.Text);
                    qrCodeEncoder.QRCodeVersion = version;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误的版本,ex:" + ex.Message);
                }

                string errorCorrect = cboCorrectionLevel.Text;
                if (errorCorrect == "L")
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                else if (errorCorrect == "M")
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                else if (errorCorrect == "Q")
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                else if (errorCorrect == "H")
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                //
                qrCodeEncoder.QRCodeBackgroundColor = System.Drawing.Color.White;
                qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;

                Image image;

                String data = txtEncodeData.Text;

                var qrBitMap = qrCodeEncoder.Encode(data);

                if (radioButton2.Checked)
                {
                    if (_attrBitmap != null && _attrBitmap.Width > 0)
                    {
                        try
                        {
                            if (qrBitMap != null && qrBitMap.Width > 0)
                            {
                                int left = (qrBitMap.Width - _attrBitmap.Width) / 2;
                                int top = (qrBitMap.Height - _attrBitmap.Height) / 2;
                                for (int i = 0; i < qrBitMap.Width; i++)
                                {
                                    for (int j = 0; j < qrBitMap.Height; j++)
                                    {
                                        if (i >= left && i < left + _attrBitmap.Width)
                                        {
                                            if (j >= top && j < top + _attrBitmap.Height)
                                            {
                                                var color = _attrBitmap.GetPixel(i - left, j - top);

                                                if (color.A != System.Drawing.Color.Transparent.A)
                                                {
                                                    qrBitMap.SetPixel(i, j, _attrBitmap.GetPixel(i - left, j - top));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    if (_attrBitmap != null && _attrBitmap.Width > 0)
                    {
                        var contrast = 200;

                        if (radioButton3.Checked)
                        {
                            var maxColorValue = qrCodeEncoder.QRCodeForegroundColor.R + qrCodeEncoder.QRCodeForegroundColor.B + qrCodeEncoder.QRCodeForegroundColor.G;

                            var qrForegroundArgb = qrCodeEncoder.QRCodeForegroundColor.ToArgb();

                            _attrBitmap = BitmapEx.Luminance(_attrBitmap, -50);

                            for (int i = 0; i < qrBitMap.Width; i++)
                            {
                                for (int j = 0; j < qrBitMap.Height; j++)
                                {
                                    if (qrBitMap.GetPixel(i, j).ToArgb() == qrForegroundArgb)
                                    {
                                        try
                                        {
                                            var attrColor = _attrBitmap.GetPixel(i, j);
                                            var attrColorValue = attrColor.R + attrColor.G + attrColor.B;
                                            var absDifference = Math.Abs(maxColorValue - attrColorValue);

                                            if (absDifference < contrast)
                                            {
                                                qrBitMap.SetPixel(i, j, _attrBitmap.GetPixel(i, j));
                                            }
                                            else
                                            {
                                                qrBitMap.SetPixel(i, j, _attrBitmap.GetPixel(i, j));
                                            }
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var maxColorValue = qrCodeEncoder.QRCodeForegroundColor.R + qrCodeEncoder.QRCodeForegroundColor.B + qrCodeEncoder.QRCodeForegroundColor.G;

                            var qrBackgroundArgb = qrCodeEncoder.QRCodeBackgroundColor.ToArgb();

                            _attrBitmap = BitmapEx.Luminance(_attrBitmap, 50);

                            for (int i = 0; i < qrBitMap.Width; i++)
                            {
                                for (int j = 0; j < qrBitMap.Height; j++)
                                {
                                    if (qrBitMap.GetPixel(i, j).ToArgb() == qrBackgroundArgb)
                                    {
                                        try
                                        {
                                            var attrColor = _attrBitmap.GetPixel(i, j);
                                            var attrColorValue = attrColor.R + attrColor.G + attrColor.B;
                                            var absDifference = Math.Abs(maxColorValue - attrColorValue);

                                            if (absDifference > contrast)
                                            {
                                                qrBitMap.SetPixel(i, j, _attrBitmap.GetPixel(i, j));
                                            }
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }
                    }


                }
                //像素处理

                image = qrBitMap;
                picEncode.Image = image;

                sizeLabel.Text = $"W:{this.picEncode.Image.Width}px,H:{this.picEncode.Image.Height}px";

            }
            catch (Exception ex)
            {
                MessageBox.Show("生成失败:" + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG Image|*.png";
            saveFileDialog1.Title = "Save";
            saveFileDialog1.FileName = string.Empty;
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                using (System.IO.FileStream fs =
                   (System.IO.FileStream)saveFileDialog1.OpenFile())
                {

                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:
                            this.picEncode.Image.Save(fs,
                               System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;

                        case 2:
                            this.picEncode.Image.Save(fs,
                               System.Drawing.Imaging.ImageFormat.Bmp);
                            break;

                        case 3:
                            this.picEncode.Image.Save(fs,
                               System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                        case 4:
                            this.picEncode.Image.Save(fs,
                               System.Drawing.Imaging.ImageFormat.Png);
                            break;
                    }
                }
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnSave_Click(btnSave, null);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printDialog1.Document = printDocument1;
            DialogResult r = printDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(picEncode.Image, 0, 0);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG Image|*.png|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = string.Empty;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String fileName = openFileDialog1.FileName;

                picDecode.Image = new Bitmap(fileName);

            }
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDecode_Click_1(object sender, EventArgs e)
        {
            try
            {
                QRCodeDecoder decoder = new QRCodeDecoder();
                String decodedString = decoder.decode(new QRCodeBitmapImage(new Bitmap(picDecode.Image)));
                txtDecodedData.Text = decodedString;
            }
            catch (Exception ex)
            {
                MessageBox.Show("解码失败，ex:" + ex.Message);
            }
        }
        //附加图片
        private void attributeBtn_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG Image|*.png|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = string.Empty;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _asIcon = radioButton2.Checked;
                String fileName = openFileDialog1.FileName;
                var x = 190;
                var y = 190;
                if (_asIcon)
                {
                    x = 30;
                    y = 30;
                }
                _attrBitmap = BitmapEx.GetThumbnailImage(fileName, x, y);
                attrPictureBox.Image = _attrBitmap;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                panel1.Enabled = true;
            }
            else
            {
                panel1.Enabled = false;
            }
        }

        
    }
}