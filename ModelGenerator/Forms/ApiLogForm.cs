/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.ModelGenerator.Forms
*文件名： ApiLogForm
*版本号： V1.0.0.0
*唯一标识：9ae8c072-cd3c-4ef9-9433-92f6189af33e
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2022/4/14 21:29:16
*描述：
*
*=====================================================================
*修改标记
*修改时间：2022/4/14 21:29:16
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.Common;
using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Forms
{
    public partial class ApiLogForm : Form
    {
        List<ApiLogInfo> _apiLogInfos = new List<ApiLogInfo>();

        public ApiLogForm()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Now.Date.AddDays(-1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = String.Join(";", openFileDialog1.FileNames);

                LoadData(openFileDialog1.FileNames);
            }
        }

        Task LoadData(string[] files)
        {
            this.Enabled = false;

            return Task.Factory.StartNew(() =>
            {
                if (files != null && files.Length > 0)
                {
                    _apiLogInfos.Clear();

                    foreach (var file in files)
                    {
                        var arr = File.ReadAllLines(file, Encoding.UTF8);

                        if (arr == null || arr.Length < 1) continue;

                        foreach (var json in arr)
                        {
                            try
                            {
                                var instance = SerializeHelper.Deserialize<ApiLogInfo>(json);
                                if (instance != null)
                                {
                                    _apiLogInfos.Add(instance);
                                }
                            }
                            catch { }
                        }

                        if (_apiLogInfos != null && _apiLogInfos.Count > 0)
                        {
                            this.Invoke(() =>
                            {
                                dataGridView1.DataSource = null;
                                dataGridView1.DataSource = _apiLogInfos;
                                toolStripStatusLabel1.Text = $"已成功加载数据{_apiLogInfos.Count}条";
                            });
                        }
                    }
                }
                this.Invoke(() =>
                {
                    this.Enabled = true;
                });
            });

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            var from = dateTimePicker1.Value;

            var url = textBox2.Text;

            var cost = 0;

            int.TryParse(textBox3.Text, out cost);

            var keywords = textBox4.Text;

            Task.Factory.StartNew(() =>
            {
                if (_apiLogInfos != null && _apiLogInfos.Count > 0)
                {
                    var data = _apiLogInfos.Where(q => q.Created >= from);

                    if (!string.IsNullOrEmpty(url))
                    {
                        data = data.Where(q => q.Url.IndexOf(url, StringComparison.InvariantCultureIgnoreCase) > -1);
                    }

                    data = data.Where(q => q.Cost >= cost);

                    if (!string.IsNullOrEmpty(keywords))
                    {
                        data = data.Where(q => q?.CallIp?.IndexOf(keywords, StringComparison.InvariantCultureIgnoreCase) > -1
                        || q?.Exception?.Message?.IndexOf(keywords, StringComparison.InvariantCultureIgnoreCase) > -1
                        || q?.Header?.IndexOf(keywords, StringComparison.InvariantCultureIgnoreCase) > -1
                        || q?.Input?.IndexOf(keywords, StringComparison.InvariantCultureIgnoreCase) > -1
                        || q?.Output?.IndexOf(keywords, StringComparison.InvariantCultureIgnoreCase) > -1
                        || q?.RequestMethod?.IndexOf(keywords, StringComparison.InvariantCultureIgnoreCase) > -1
                        || q?.Description?.IndexOf(keywords, StringComparison.InvariantCultureIgnoreCase) > -1);
                    }

                    var result = data.ToList();

                    this.Invoke(() =>
                    {
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = result;
                        toolStripStatusLabel1.Text = $"已成功加载数据{result.Count}条";
                    });
                }
                this.Invoke(() =>
                {
                    this.Enabled = true;
                });
            });
        }

        private void ApiLogForm_KeyUp(object sender, KeyEventArgs e)
        {
            ShortcutKeyHelper.Enter(this, e, () =>
            {
                button2_Click(null, null);
            });
        }
    }
}
