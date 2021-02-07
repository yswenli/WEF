/****************************************************************************
*项目名称：WEF.ModelGenerator.Forms
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：WEF.ModelGenerator.Forms
*类 名 称：SQLTemplateForm
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/2/19 9:35:47
*描述：
*=====================================================================
*修改时间：2019/2/19 9:35:47
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Diagnostics;
using System.Windows.Forms;
using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Forms;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator
{
    public partial class MainForm : CCWin.Skin_Mac
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region 打开数据库视图

        private void MainForm_Load(object sender, EventArgs e)
        {
            Init();
        }

        LeftPanelForm lp = new LeftPanelForm();

        SQLTemplateForm sqlTemplateForm = new SQLTemplateForm();

        /// <summary>
        /// 左侧加载数据库列表
        /// </summary>
        private void Init()
        {
            if (!DockContentHelper.Load(dpleft))
            {
                lp.newcontentForm += lp_newcontentForm;
                lp.newsqlForm += lp_newsqlForm;
                lp.Show(dpleft);
                lp.DockTo(dpleft, DockStyle.Left);

                sqlTemplateForm.Show(dpleft);
                sqlTemplateForm.DockTo(dpleft, DockStyle.Right);
            }
        }

        /// <summary>
        /// 创建生成
        /// </summary>
        /// <param name="conModel"></param>
        /// <param name="tableName"></param>
        void lp_newcontentForm(ConnectionModel conModel)
        {
            ContentForm s = new ContentForm();
            s.Text = "(" + conModel.Database + ")" + conModel.TableName;
            s.TableName = conModel.TableName;
            s.DatabaseName = conModel.Database;
            s.IsView = conModel.IsView;
            s.ConnectionModel = conModel;
            s.Show(dpleft);
        }

        void lp_newsqlForm(ConnectionModel conModel,string tableName)
        {
            SQLForm s = new SQLForm(tableName);
            s.Text = "(" + conModel.Database + ")SQL查询窗口";
            s.ConnectionModel = conModel;
            s.Show(dpleft);
        }

        /// <summary>
        /// 数据库视图打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 数据库视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lp.Show();
        }

        #endregion


        #region 菜单

        private void newDBConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lp.ShowDbSelect();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sqlForm = dpleft.ActiveContent as SQLForm;

            if (sqlForm != null)
            {
                sqlForm.RunSql();
            }

        }

        private void logsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lp.ShowLogs();
        }
        /// <summary>
        /// json工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jsonToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new JsonClassGeneratorForm().ShowDialog(this);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog(this);
        }

        #endregion

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/yswenli/WEF/releases");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DockContentHelper.Save(dpleft);
            e.Cancel = true;
            this.Hide();
        }

        #region 托盘
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void displayUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void shutDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DockContentHelper.Save(dpleft);

                if (MessageBox.Show("确定要退出WEF数据库工具吗？", "WEF数据库工具", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    notifyIcon1.Dispose();
                    Application.Exit();
                    Environment.Exit(0);
                }
            }
            catch { }
        }


        #endregion

       
    }
}
