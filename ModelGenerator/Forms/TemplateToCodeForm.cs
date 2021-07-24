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

using CCWin;

using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Forms
{
    public partial class TemplateToCodeForm : Skin_Mac
    {
        List<TemplateToCodeConfig> _templateToCodeConfigs = new List<TemplateToCodeConfig>();


        public TemplateToCodeForm()
        {
            InitializeComponent();
        }

        private void TemplateToCodeForm_Load(object sender, EventArgs e)
        {
            var data = TemplateHelper.Data;

            if (data == null || data.Count < 1)
            {
                MessageBox.Show(this, "未检测到任何代码模板，请先配置代码模板", "模板代码生成工具");
                this.Close();
                return;
            }

            skinComboBox1.Items.Clear();
            foreach (var item in data)
            {
                skinComboBox1.Items.Add(item.Name);
            }
            skinComboBox1.SelectedIndex = 0;
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filePath = openFileDialog1.FileName;

                if (File.Exists(filePath))
                {
                    skinWaterTextBox3.Text = filePath;
                }
            }
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            var config = new TemplateToCodeConfig();
            config.TemplateName = skinComboBox1.SelectedItem?.ToString() ?? "";
            config.NameSpace = skinWaterTextBox1.Text;
            config.ReplaceString = skinWaterTextBox2.Text;
            config.Suffix = skinComboBox2.SelectedItem?.ToString() ?? "";
            config.ProjectFilePath = skinWaterTextBox3.Text;

            if (string.IsNullOrEmpty(config.TemplateName))
            {
                MessageBox.Show(this, "请先配置代码模板", "模板代码生成工具");
                return;
            }
            if (string.IsNullOrEmpty(config.NameSpace))
            {
                MessageBox.Show(this, "请先配置代码命名空间", "模板代码生成工具");
                return;
            }
            if (string.IsNullOrEmpty(config.ReplaceString))
            {
                MessageBox.Show(this, "请先配置替换名", "模板代码生成工具");
                return;
            }
            if (string.IsNullOrEmpty(config.Suffix))
            {
                MessageBox.Show(this, "请先配置替换名后缀", "模板代码生成工具");
                return;
            }
            if (string.IsNullOrEmpty(config.ProjectFilePath))
            {
                MessageBox.Show(this, "请先配置将要加入的项目文件地址", "模板代码生成工具");
                return;
            }

            _templateToCodeConfigs.Add(config);
            skinListBox1.Items.Clear();
            foreach (var item in _templateToCodeConfigs)
            {
                var key = $"{item.NameSpace}.{item.ReplaceString}{item.Suffix}";
                skinListBox1.Items.Add(new CCWin.SkinControl.SkinListBoxItem(key));
            }
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (_templateToCodeConfigs.Count > 0)
                {
                    var data = TemplateHelper.Data;

                    if (data == null || data.Count == 0) return;

                    foreach (var templateToCodeConfig in _templateToCodeConfigs)
                    {
                        var templateData = data.FirstOrDefault(b => b.Name == templateToCodeConfig.TemplateName);

                        if (templateData == null) continue;

                        var templateContent = templateData.Content;

                        if (string.IsNullOrEmpty(templateContent)) continue;                        

                        var codeContent = templateContent
                            .Replace("[namespace]", templateToCodeConfig.NameSpace)
                            .Replace("[className]", templateToCodeConfig.ReplaceString);

                        var fileDir = Path.GetDirectoryName(templateToCodeConfig.ProjectFilePath);
                        var className = templateToCodeConfig.ReplaceString + templateToCodeConfig.Suffix;
                        var filePath = Path.Combine(fileDir, className + ".cs");
                        UtilsHelper.Write(filePath, codeContent);
                    }
                }
                MessageBox.Show(this, "操作完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "操作失败：" + ex.Message);
            }
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectItem = skinListBox1.SelectedItem;

            if (selectItem != null)
            {
                var key = selectItem.ToString();
                var config = _templateToCodeConfigs.FirstOrDefault(b => key == $"{b.NameSpace}.{b.ReplaceString}{b.Suffix}");
                if (config != null)
                {
                    foreach (var item in skinComboBox1.Items)
                    {
                        if(item.ToString()== config.TemplateName)
                        {
                            skinComboBox1.SelectedItem = item;
                        }
                    }

                    skinWaterTextBox1.Text = config.NameSpace;

                    skinWaterTextBox2.Text = config.ReplaceString;

                    foreach (var item in skinComboBox2.Items)
                    {
                        if (item.ToString() == config.Suffix)
                        {
                            skinComboBox2.SelectedItem = item;
                        }
                    }
                    skinWaterTextBox3.Text = config.ProjectFilePath;
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "确定要删除此配置吗？", "模板代码生成工具", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var selectItem = skinListBox1.SelectedItem;

                if (selectItem != null)
                {
                    var key = selectItem.ToString();
                    var config = _templateToCodeConfigs.FirstOrDefault(b => key == $"{b.NameSpace}.{b.ReplaceString}{b.Suffix}");
                    if (config != null)
                    {
                        _templateToCodeConfigs.Remove(config);

                        skinListBox1.Items.Clear();
                        foreach (var item in _templateToCodeConfigs)
                        {
                            var skey = $"{item.NameSpace}.{item.ReplaceString}{item.Suffix}";
                            skinListBox1.Items.Add(new CCWin.SkinControl.SkinListBoxItem(skey));
                        }
                    }
                }
            }
        }

        private void skinListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            modifyToolStripMenuItem_Click(null, null);
        }
    }
}
