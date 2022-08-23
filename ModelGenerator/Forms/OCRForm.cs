/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.ModelGenerator.Forms
*文件名： OCRForm
*版本号： V1.0.0.0
*唯一标识：6fe103dd-17f7-4775-a1eb-80aefd455dd3
*当前的用户域：WALLE
*创建人： yswen
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/7/8 10:13:39
*描述：
*
*=================================================
*修改标记
*修改时间：2022/7/8 10:13:39
*修改人： yswen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using CCWin;

using OpenCvSharp;
using OpenCvSharp.Aruco;

using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR.Models.Online;

namespace WEF.ModelGenerator.Forms
{
    public partial class OCRForm : Skin_Mac
    {
        FullOcrModel _model;

        public OCRForm()
        {
            InitializeComponent();

            DownloadModelAsync(() =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        comboBox1.SelectedIndex = 0;

                        label1.Text = "";
                    }));
                }
                else
                {

                    comboBox1.SelectedIndex = 0;

                    label1.Text = "";
                }
            });


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var imageFile = openFileDialog1.FileName;
                try
                {
                    pictureBox1.Image = Image.FromFile(imageFile);
                    pictureBox1.Tag = imageFile;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("请选择正确的图片文件，Err:" + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fileName = pictureBox1.Tag?.ToString() ?? null;

            if (string.IsNullOrEmpty(fileName)) return;

            button2.Enabled = false;
            label1.Text = "正在识别中...";

            Task.Factory.StartNew(() =>
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    using (PaddleOcrAll all = new PaddleOcrAll(_model)
                    {
                        AllowRotateDetection = false, /* 允许识别有角度的文字 */
                        Enable180Classification = false, /* 允许识别旋转角度大于90度的文字 */
                    })
                    {
                        using (Mat src = Cv2.ImDecode(GetData(fileName), ImreadModes.Color))
                        //using (Mat src = Cv2.ImDecode(data, ImreadModes.Color))
                        {
                            PaddleOcrResult result = all.Run(src);
                            int index = 0;
                            var sb = new StringBuilder();
                            foreach (PaddleOcrResultRegion region in result.Regions)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    //if (region.Score >= 0.7)
                                    //{
                                    //sb.AppendLine($"Index:{index},Text: {region.Text},Score:{region.Score}, RectCenter:{region.Rect.Center}, RectSize:{region.Rect.Size}, Angle:{region.Rect.Angle} {Environment.NewLine}");
                                    sb.AppendLine($"Index:{index},Text: {region.Text}");
                                    DrawRectangle(region, index);
                                    index += 1;
                                    //}
                                }));
                            }
                            sw.Stop();
                            this.Invoke(new Action(() =>
                            {
                                textBox1.Text += sb.ToString();
                                button2.Enabled = true;
                                label1.Text = $"操作完成，用时:{sw.ElapsedMilliseconds}ms";
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("识别出现异常，Err:" + ex.Message);
                    sw.Stop();
                    this.Invoke(new Action(() =>
                    {
                        button2.Enabled = true;
                        label1.Text = $"操作完成，用时:{sw.ElapsedMilliseconds}ms";
                    }));
                }
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = String.Empty;
            string fileName = pictureBox1.Tag?.ToString() ?? null;
            if (string.IsNullOrEmpty(fileName)) return;
            pictureBox1.Image = Image.FromFile(fileName);
        }

        /// <summary>
        /// 划线
        /// </summary>
        /// <param name="region"></param>
        /// <param name="index"></param>
        void DrawRectangle(PaddleOcrResultRegion region, int index)
        {
            var pen = new Pen(Color.Red, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            using (var g = pictureBox1.CreateGraphics())
            {
                var size = region.Rect.Size;
                if (region.Rect.Angle >= 80 && region.Rect.Angle <= 110)
                {
                    var x = region.Rect.Center.X - size.Height / 2;
                    var y = region.Rect.Center.Y - size.Width / 2;
                    g.DrawRectangle(pen, x, y, size.Height, size.Width);
                    g.DrawString(index.ToString(), new Font("微软雅黑", 10, FontStyle.Bold), Brushes.Red, x + 3, y + 3);
                }
                else
                {
                    var x = region.Rect.Center.X - size.Width / 2;
                    var y = region.Rect.Center.Y - size.Height / 2;
                    g.DrawRectangle(pen, x, y, size.Width, size.Height);
                    g.DrawString(index.ToString(), new Font("微软雅黑", 10, FontStyle.Bold), Brushes.Red, x + 3, y + 3);
                }
                g.Save();
            };
        }

        /// <summary>
        /// 灰度
        /// </summary>
        /// <param name="img1"></param>
        void ToGrey(Bitmap img1)
        {
            try
            {
                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        Color pixelColor = img1.GetPixel(i, j);
                        //计算灰度值
                        int grey = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);
                        Color newColor = Color.FromArgb(grey, grey, grey);
                        img1.SetPixel(i, j, newColor);
                    }
                }
            }
            catch { }
        }
        /// <summary>
        /// 二值化
        /// </summary>
        /// <param name="img1"></param>
        void Thresholding(Bitmap img1)
        {
            try
            {

                int[] histogram = new int[256];
                int minGrayValue = 255, maxGrayValue = 0;
                //求取直方图
                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        Color pixelColor = img1.GetPixel(i, j);
                        histogram[pixelColor.R]++;
                        if (pixelColor.R > maxGrayValue) maxGrayValue = pixelColor.R;
                        if (pixelColor.R < minGrayValue) minGrayValue = pixelColor.R;
                    }
                }
                //迭代计算阀值
                int threshold = -1;
                int newThreshold = (minGrayValue + maxGrayValue) / 2;
                for (int iterationTimes = 0; threshold != newThreshold && iterationTimes < 100; iterationTimes++)
                {
                    threshold = newThreshold;
                    int lP1 = 0;
                    int lP2 = 0;
                    int lS1 = 0;
                    int lS2 = 0;
                    //求两个区域的灰度的平均值
                    for (int i = minGrayValue; i < threshold; i++)
                    {
                        lP1 += histogram[i] * i;
                        lS1 += histogram[i];
                    }
                    int mean1GrayValue = (lP1 / lS1);
                    for (int i = threshold + 1; i < maxGrayValue; i++)
                    {
                        lP2 += histogram[i] * i;
                        lS2 += histogram[i];
                    }
                    int mean2GrayValue = (lP2 / lS2);
                    newThreshold = (mean1GrayValue + mean2GrayValue) / 2;
                }
                //计算二值化
                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        Color pixelColor = img1.GetPixel(i, j);
                        if (pixelColor.R > threshold) img1.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                        else img1.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 获取照片内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        byte[] GetData(string fileName)
        {
            using (Bitmap bitmap = new Bitmap(fileName, true))
            {
                //ToGrey(bitmap);
                //Thresholding(bitmap);
                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }

            }
        }

        byte[] GetData()
        {
            using (var ms = new MemoryStream())
            {
                pictureBox1.Image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                var selected = comboBox1.Items[comboBox1.SelectedIndex].ToString();
                switch (selected)
                {
                    case "V2":
                        _model = GetModel(1);
                        break;
                    case "V3":
                        _model = GetModel(2);
                        break;
                    default:
                        _model = GetModel(0);
                        break;
                }
                //_model.EnsureAll();
            }
        }



        Dictionary<int, FullOcrModel> _dic = null;

        async void DownloadModelAsync(Action done = null)
        {
            _dic = new Dictionary<int, FullOcrModel>();
            _dic.Add(0, await OnlineFullModels.ChineseServerV2.DownloadAsync());
            _dic.Add(1, await OnlineFullModels.ChineseV2.DownloadAsync());
            _dic.Add(2, await OnlineFullModels.ChineseV3.DownloadAsync());
            done?.Invoke();
        }

        FullOcrModel GetModel(int type)
        {
            return _dic[type];
        }
    }
}
