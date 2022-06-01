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
using System.Text;
using System.Threading.Tasks;

using WEF.ModelGenerator.Common;

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

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var filePath = openFileDialog1.FileName;

                skinWaterTextBox3.Text = filePath;
            }
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            var filePath = skinWaterTextBox3.Text;

            if (!string.IsNullOrEmpty(filePath) && FileHelper.Exists(filePath))
            {
                if (FileHelper.GetLenth(filePath) > 50 * 1024 * 1024)
                {
                    WEFMessageBox.Show(this, "文件长度不能超过50M限制", "Base64转码");
                }
                else
                {
                    skinButton4.Enabled = false;

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
                                    if (WEFMessageBox.Show(this, "当前字符串过长，是否需要另存为txt文件？", "Base64转码",System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                        {
                                            var fileName = saveFileDialog1.FileName;

                                            var result = FileHelper.Write(fileName, Encoding.UTF8.GetBytes(base64Str));

                                            if (result)
                                            {
                                                WEFMessageBox.Show(this, "文件保存成功", "Base64转码");
                                            }
                                            else
                                            {
                                                WEFMessageBox.Show(this, "文件保存失败", "Base64转码");
                                            }                                            
                                        }
                                        skinButton4.Enabled = true;
                                    }
                                });
                                
                            }
                            else
                            {
                                this.Invoke(() =>
                                {
                                    skinWaterTextBox4.Text = base64Str;
                                    skinButton4.Enabled = true;
                                });
                            }
                        }
                        else
                        {
                            this.Invoke(() =>
                            {
                                skinButton4.Enabled = true;
                            });
                        }
                    });


                }
            }
        }

        private async void skinButton3_Click(object sender, EventArgs e)
        {
            var base64Str = skinWaterTextBox4.Text;

            if (!string.IsNullOrEmpty(base64Str))
            {
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var fileName = saveFileDialog1.FileName;

                    var result = await Task.Run(() =>
                    {
                        var bytes = Convert.FromBase64String(base64Str);
                        return FileHelper.Write(fileName, bytes);
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
            }
        }
    }
}
