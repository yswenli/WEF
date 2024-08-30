using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;

namespace TxtReplaceTool
{
    public partial class TxtReplaceForm : Form
    {
        List<string> _fileList = new List<string>();

        public TxtReplaceForm()
        {
            InitializeComponent();
        }



        private void TxtReplaceForm_Load(object sender, EventArgs e)
        {
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            toolStripProgressBar1.MarqueeAnimationSpeed = 10;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        /// <summary>
        /// 查找文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            var filePath = textBox1.Text;
            var filters = textBox2.Text;
            var str = textBox3.Text;

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show(this, "地址不能为空");
                return;
            }
            if (string.IsNullOrEmpty(filters))
            {
                MessageBox.Show(this, "文件类型不能为空");
                return;
            }

            this.Enabled = false;

            label5.Text = "正在查找文件...";
            toolStripProgressBar1.Visible = true;

            Task.Run(() =>
            {
                var stopWatch = Stopwatch.StartNew();
                var list = FileHelper.Find(str, filePath, filters);
                this.Invoke(new Action(() =>
                {
                    this.Enabled = true;
                    button2.Enabled = true;
                    if (list != null && list.Count > 0)
                    {
                        label5.Text = $"已找到{list.Count}个文件,共用时{stopWatch.Elapsed.TotalSeconds}s";                        
                        _fileList.Clear();
                        _fileList = list;
                        listBox1.Items.Clear();
                        foreach (var item in list)
                        {
                            listBox1.Items.Add(item);
                        }
                        button3.Enabled = true;
                        button4.Enabled = true;
                        textBox4.Enabled = true;
                        textBox5.Enabled = true;
                        checkBox1.Enabled = true;
                        checkBox2.Enabled = true;
                    }
                    else
                    {
                        label5.Text = $"找不到任何内容,共用时{stopWatch.Elapsed.TotalSeconds}s";
                        listBox1.Items.Clear();

                        button3.Enabled = false;
                        button4.Enabled = false;
                        textBox4.Enabled = false;
                        textBox5.Enabled = false;
                        checkBox1.Enabled = false;
                        checkBox2.Enabled = false;
                    }
                    toolStripProgressBar1.Visible = false;
                }));

            });
        }

        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (_fileList == null || _fileList.Count < 1)
            {
                MessageBox.Show(this, "文件列表不能为空");
                return;
            };

            var source = textBox3.Text;
            var target = textBox4.Text;

            if (string.IsNullOrEmpty(source))
            {
                MessageBox.Show(this, "查找内容不能为空");
                return;
            }

            if (string.IsNullOrEmpty(target))
            {
                MessageBox.Show(this, "替换内容不能为空");
                return;
            }

            if (MessageBox.Show(this, "请仔细检查操作内容，确认要执行替换操作吗?", "文件查找替换工具提示提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            var replaceFileName = false;

            if (MessageBox.Show(this, "是否同时替换文件名称", "文件查找替换工具提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                replaceFileName = true;
            }

            this.EnableControlsOrNot();

            label5.Text = "正在替换文件...";

            Task.Run(() =>
            {
                var stopWatch = Stopwatch.StartNew();

                var list = FileHelper.Replace(source, target, _fileList);

                if (list != null && list.Count > 0 && replaceFileName)
                {
                    foreach (var item in list)
                    {
                        if (File.Exists(item))
                        {
                            var fileName = item.Replace(source, target);
                            File.Move(item, fileName);
                        }
                    }
                }

                this.Invoke(new Action(() =>
                {
                    if (list == null || list.Count < 1)
                    {
                        label5.Text = $"替换操作失败,共用时{stopWatch.Elapsed.TotalSeconds}s";
                    }
                    else
                    {
                        label5.Text = $"替换操作已成功完成{list.Count}次,共用时{stopWatch.Elapsed.TotalSeconds}s";
                    }

                    this.EnableControlsOrNot();
                }));
            });
        }

        /// <summary>
        /// 追加文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (_fileList == null || _fileList.Count < 1)
            {
                MessageBox.Show(this, "文件列表不能为空");
                return;
            };
            var appentText = textBox5.Text;
            if (string.IsNullOrEmpty(appentText))
            {
                MessageBox.Show(this, "追加的内容不能为空");
                return;
            }
            var addStatus = 0;
            if (checkBox1.Checked)
            {
                addStatus = 1;

                if (checkBox2.Checked)
                {
                    addStatus = 3;
                }
            }
            else
            {
                if (checkBox2.Checked)
                {
                    addStatus = 2;
                }
            }
            if (addStatus == 0)
            {
                MessageBox.Show(this, "请选择要追加的位置");
                return;
            }

            this.EnableControlsOrNot();

            Task.Factory.StartNew(() =>
            {
                var stopWatch = Stopwatch.StartNew();

                var list = FileHelper.AppendText(appentText, _fileList);

                this.Invoke(new Action(() =>
                {
                    if (list == null || list.Count < 1)
                    {
                        label5.Text = $"追加操作失败,共用时{stopWatch.Elapsed.TotalSeconds}s";
                    }
                    else
                    {
                        label5.Text = $"追加操作已成功完成{list.Count}次,共用时{stopWatch.Elapsed.TotalSeconds}s";
                    }
                    this.EnableControlsOrNot();
                }));
            });

        }
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem != null)
                {
                    var filePath = listBox1.SelectedItem.ToString();
                    Process.Start(filePath);
                }
            }
            catch { }
        }

        /// <summary>
        /// 排除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.DeleteSelectedItems();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItems != null && listBox1.SelectedItems.Count > 0)
                {
                    if (MessageBox.Show(this, "确认要删除这些文件吗？", "文件查找替换工具", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        listBox1.DeleteSelectedItems((item) => FileHelper.Delete(item));
                    }
                }
            }
            catch { }
        }

    }
}
