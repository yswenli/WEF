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
using CCWin;

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Forms;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator
{
    public partial class MainForm : Skin_Mac
    {
        public MainForm()
        {
            InitializeComponent();
        }

        LeftPanelForm _leftPannelForm;

        SQLTemplateForm _sqlTemplateForm;

        #region 打开数据库视图

        private void MainForm_Load(object sender, EventArgs e)
        {
            _leftPannelForm = new LeftPanelForm();

            _sqlTemplateForm = new SQLTemplateForm();

            if (!DockContentHelper.Load(dpleft))
            {
                _leftPannelForm.OnNewContentForm += lp_newcontentForm;
                _leftPannelForm.OnNewSqlForm += lp_newsqlForm;
                _leftPannelForm.Show(dpleft);
                _leftPannelForm.DockTo(dpleft, DockStyle.Left);

                _sqlTemplateForm.Show(dpleft);
                _sqlTemplateForm.DockTo(dpleft, DockStyle.Right);
            }
        }

        /// <summary>
        /// 创建生成
        /// </summary>
        /// <param name="conModel"></param>
        /// <param name="tableName"></param>
        void lp_newcontentForm(ConnectionModel conModel)
        {
            ContentForm contentForm = new ContentForm();
            contentForm.Text = "(" + conModel.Database + ")" + conModel.TableName;
            contentForm.TableName = conModel.TableName;
            contentForm.DatabaseName = conModel.Database;
            contentForm.IsView = conModel.IsView;
            contentForm.ConnectionModel = conModel;
            contentForm.Show(dpleft);
        }

        void lp_newsqlForm(ConnectionModel conModel)
        {
            SQLForm s = new SQLForm(conModel.TableName);
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
            _leftPannelForm.Show();
        }

        #endregion


        #region 菜单
        /// <summary>
        /// 新连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newDBConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _leftPannelForm.ShowDbSelect();
        }

        /// <summary>
        /// f5 sql运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sqlForm = dpleft.ActiveContent as SQLForm;

            if (sqlForm != null)
            {
                sqlForm.RunSql();
            }
        }
        /// <summary>
        /// 模板管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void templateManageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TemplateForm().ShowDialog(this);
        }
        /// <summary>
        /// 生成业务代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateBusinessCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TemplateToCodeForm().ShowDialog(this);
        }

        private void templatetocodefastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new TemplateToCodeFastForm().ShowDialog(this);
        }

        private void dataSyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataSyncForm.Instance.Show(this);
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

        /// <summary>
        /// 二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void qRCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new QrCodeForm().ShowDialog(this);
        }

        #region 关于菜单
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog(this);
        }

        private void errorlogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _leftPannelForm.ShowLogs();
        }
        #endregion

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
            NotifyIcon.ShowBalloonTip(3000, "WEF数据库工具", "当前程序正在后台运行", ToolTipIcon.Info);
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
                    NotifyIcon.Dispose();
                    Application.Exit();
                    Environment.Exit(0);
                }
            }
            catch { }
        }








        #endregion

       
    }
}
