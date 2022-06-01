using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;

namespace WEF.ModelGenerator.Forms
{
    public partial class XsdGeneratorForm : CCWin.Skin_Mac
    {
        public XsdGeneratorForm()
        {
            InitializeComponent();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                var spaceName = textBox1.Text;

                if (string.IsNullOrEmpty(spaceName))
                {
                    MessageBox.Show(this, "命名空间为必填项");

                    tabControl1.SelectedIndex = 0;

                    return;
                }

                var xsdTxt = textBox2.Text;

                if (string.IsNullOrEmpty(xsdTxt))
                {
                    MessageBox.Show(this, "xsd内容为必填项");

                    tabControl1.SelectedIndex = 0;

                    return;
                }

                try
                {
                    textBox3.Text = XSDToCSharp.GeneratedCode(xsdTxt, spaceName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "生成代码出错:" + ex.Message);
                    tabControl1.SelectedIndex = 0;
                    return;
                }
            }

        }
    }
}
