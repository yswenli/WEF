using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using CCWin;

using WEF.Standard.DevelopTools.Common;
using WEF.Standard.DevelopTools.Model;

namespace WEF.Standard.DevelopTools.Forms
{
    public partial class TemplateForm : Skin_Mac
    {
        public TemplateForm()
        {
            InitializeComponent();
        }

        private void TemplateForm_Load(object sender, EventArgs e)
        {
            LoadData(TemplateHelper.Data);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinTreeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            modifyToolStripMenuItem_Click(null, null);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectNode = skinTreeView1.SelectedNode;

            if (selectNode == null) return;

            var name = selectNode.Text;

            var data = TemplateHelper.Data;

            if (data == null || data.Count == 0)
            {
                return;
            }

            var templateData = data.FirstOrDefault(b => b.Name == name);

            if (templateData == null) return;

            skinWaterTextBox1.Text = templateData.Name;

            skinWaterTextBox2.Text = templateData.Content;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "确定要删除此模板吗？", "模板管理", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var selectNode = skinTreeView1.SelectedNode;

                if (selectNode == null) return;

                var name = selectNode.Text;

                var data = TemplateHelper.Data;

                if (data == null || data.Count == 0)
                {
                    return;
                }

                var templateData = data.FirstOrDefault(b => b.Name == name);

                if (templateData == null) return;

                TemplateHelper.Data.Remove(templateData);
                TemplateHelper.Save();
                LoadData(TemplateHelper.Data);
                MessageBox.Show(this, "操作完成");
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var name = skinWaterTextBox1.Text;

                var content = skinWaterTextBox2.Text;

                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show(this, "模板名称不能为空", "模板管理");
                }

                var data = TemplateHelper.Data;


                if (data == null)
                {
                    data = new List<TemplateData>();
                }

                var templateData = data.FirstOrDefault(b => b.Name == name);

                if (templateData != null)
                {
                    TemplateHelper.Data.Remove(templateData);
                }

                templateData = new TemplateData()
                {
                    Name = name,
                    Content = content
                };
                TemplateHelper.Data.Add(templateData);
                TemplateHelper.Save();
                LoadData(TemplateHelper.Data);

                MessageBox.Show(this, "操作完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "操作失败:" + ex.Message);
            }
        }



        void LoadData(List<TemplateData> data)
        {
            if (data != null && data.Count > 0)
            {
                skinTreeView1.Nodes.Clear();

                foreach (var item in data)
                {
                    skinTreeView1.Nodes.Add(item.Name);
                }
            }
        }


    }
}
