using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using CCWin;

using WEF.ModelGenerator.Common;

namespace WEF.ModelGenerator.Forms
{
    /// <summary>
    /// .net5框架集成快捷生成业务代码
    /// </summary>
    public partial class TemplateToCodeFastForm : Skin_Mac
    {
        public TemplateToCodeFastForm()
        {
            InitializeComponent();
        }

        public TemplateToCodeFastForm(string entityNames) : this()
        {
            skinWaterTextBox1.Text = entityNames;
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(folderBrowserDialog1.SelectedPath))
                {
                    skinWaterTextBox2.Text = folderBrowserDialog1.SelectedPath;
                }
            }
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(skinWaterTextBox1.Text))
            {
                MessageBox.Show(this, "实体名不能为空！");
                return;
            }

            if (string.IsNullOrEmpty(skinWaterTextBox2.Text))
            {
                MessageBox.Show(this, "主项目地址不能为空！");
                return;
            }

            try
            {
                var dir = new DirectoryInfo(skinWaterTextBox2.Text);
                var parent = dir.Parent.FullName;
                var entities = skinWaterTextBox1.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (entities != null && entities.Length > 0)
                {
                    foreach (var item in entities)
                    {
                        //
                        var space1 = dir.Name.Replace(".Api.Host", ".Data");

                        var template1 = TemplateHelper.Data.Where(b => b.Name == "Oceania.Common.Data").First();

                        var cs1 = template1.Content.Replace("[namespace]", space1).Replace("[className]", item);

                        var file1 = Path.Combine(parent, space1, item + "Operator.cs");

                        File.WriteAllText(file1, cs1);
                        //
                        var space2 = dir.Name.Replace(".Api.Host", ".Services");

                        var template2 = TemplateHelper.Data.Where(b => b.Name == "Oceania.Common.Services").First();

                        var cs2 = template2.Content.Replace("[namespace]", space2).Replace("[className]", item);

                        var file2 = Path.Combine(parent, space2, item + "Service.cs");

                        File.WriteAllText(file2, cs2);
                        //
                        var space3 = dir.Name.Replace(".Api.Host", ".Controllers");

                        var template3 = TemplateHelper.Data.Where(b => b.Name == "Oceania.Common.Controllers").First();

                        var cs3 = template3.Content.Replace("[namespace]", space1).Replace("[className]", item);

                        var file3 = Path.Combine(parent, space3, item + "Controller.cs");

                        File.WriteAllText(file3, cs3);
                    }

                    
                }
                MessageBox.Show(this, "操作完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "操作失败，" + ex.Message);
            }
        }


    }
}
