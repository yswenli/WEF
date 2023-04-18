/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.ModelGenerator.Forms
*文件名： Base64Form
*版本号： V1.0.0.0
*唯一标识：4a0b637e-681f-4d9f-b902-4a8a57529b9d
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2021/11/8 11:56:31
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/11/8 11:56:31
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using WEF.ModelGenerator.Common;

using static WEF.ModelGenerator.LogShow;

namespace WEF.ModelGenerator.Forms
{
    public partial class Base64Form : CCWin.Skin_Mac
    {
        public Base64Form()
        {
            InitializeComponent();
        }

        private void Base64Form_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "png files (*.png)|*.png|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        private async void skinButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(skinWaterTextBox1.Text))
            {
                var str = skinWaterTextBox1.Text;
                skinWaterTextBox2.Text = await Task.Run(() =>
                {
                    return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
                });
            }
        }

        private async void skinButton2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(skinWaterTextBox2.Text))
            {
                var base64Str = skinWaterTextBox2.Text;

                skinWaterTextBox1.Text = await Task.Run(() =>
                {
                    return Encoding.UTF8.GetString(Convert.FromBase64String(base64Str));
                });
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var filePath = openFileDialog1.FileName;

                skinWaterTextBox6.Text = filePath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var filePath = openFileDialog1.FileName;

                skinWaterTextBox5.Text = filePath;
            }
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            var filePath = skinWaterTextBox6.Text;

            var base64FilePath = skinWaterTextBox5.Text;

            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(base64FilePath))
            {
                WEFMessageBox.Show(this, "地址不能为空", "Base64转码");
                return;
            }

            if (FileHelper.Exists(filePath))
            {
                if (FileHelper.GetLenth(filePath) > 50 * 1024 * 1024)
                {
                    WEFMessageBox.Show(this, "文件长度不能超过50M限制", "Base64转码");
                }
                else
                {
                    skinButton6.Enabled = false;

                    Task.Run(() =>
                    {
                        var bytes = FileHelper.Read(filePath);

                        if (bytes != null && bytes.Length > 0)
                        {
                            var base64Str = Convert.ToBase64String(bytes);

                            if (base64Str.Length > 10 * 1024)
                            {
                                this.Invoke(() =>
                                {
                                    var result = FileHelper.Write(base64FilePath, Encoding.UTF8.GetBytes(base64Str));

                                    if (result)
                                    {
                                        WEFMessageBox.Show(this, "文件保存成功", "Base64转码");
                                    }
                                    else
                                    {
                                        WEFMessageBox.Show(this, "文件保存失败", "Base64转码");
                                    }
                                });

                            }
                            else
                            {
                                this.Invoke(() =>
                                {
                                    skinButton6.Enabled = true;
                                });
                            }
                        }
                        else
                        {
                            this.Invoke(() =>
                            {
                                skinButton6.Enabled = true;
                            });
                        }
                    });

                }
            }
        }


        private async void skinButton5_Click(object sender, EventArgs e)
        {
            var filePath = skinWaterTextBox6.Text;

            var base64FilePath = skinWaterTextBox5.Text;

            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(base64FilePath))
            {
                WEFMessageBox.Show(this, "地址不能为空", "Base64转码");
                return;
            }

            if (!FileHelper.Exists(base64FilePath))
            {
                WEFMessageBox.Show(this, "base64文件不存在", "Base64转码");
                return;
            }

            var result = await Task.Run(() =>
            {
                var bytes = Convert.FromBase64String(FileHelper.ReadTxt(base64FilePath));
                return FileHelper.Write(filePath, bytes);
            });

            if (result)
            {
                WEFMessageBox.Show(this, "文件保存成功", "Base64转码");
            }
            else
            {
                WEFMessageBox.Show(this, "文件保存失败", "Base64转码");
            }
        }

        /// <summary>
        /// base64转换图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private async void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                WEFMessageBox.Show(this, "base64不能为空", "Base64转小图片");
                pictureBox1.Image = null;
                return;
            }
            await Task.Run(() =>
            {
                try
                {
                    var bytes = Convert.FromBase64String(textBox1.Text);
                    using (var ms = new MemoryStream(bytes))
                    {
                        pictureBox1.Image = Bitmap.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    pictureBox1.Image = null;
                }
            });
        }
    }
}
