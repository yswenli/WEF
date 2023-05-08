/****************************************************************************
*Copyright (c) 2023 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.ModelGenerator.Forms
*文件名： FirewallForm
*版本号： V1.0.0.0
*唯一标识：9adc3fd2-48e8-4ab3-a18a-1e417b39ca0b
*当前的用户域：WALLE
*创建人： WALLE
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2023/5/8 13:38:46
*描述：
*
*=================================================
*修改标记
*修改时间：2023/5/8 13:38:46
*修改人： yswen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;

namespace WEF.ModelGenerator.Forms
{
    public partial class FirewallForm : Form
    {
        public FirewallForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ips = textBox1.Text;

            if (string.IsNullOrEmpty(ips))
            {
                MessageBox.Show(this, "ip内容不能为空", "防火墙设置工具", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            button1.Enabled = false;

            LoadForm.ShowLoading(this);

            var isWhiteList = radioButton1.Checked;

            Task.Run(() =>
            {
                if (isWhiteList)
                {
                    WindowFirewallHelper.SetWhiteList(ips);
                }
                else
                {
                    WindowFirewallHelper.SetBlackList(ips);
                }

                LoadForm.HideLoading(this);

                this.Invoke(() =>
                {
                    button1.Enabled = true;
                });
            });

        }
    }
}
