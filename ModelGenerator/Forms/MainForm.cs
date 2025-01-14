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
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using CCWin;

using TxtReplaceTool;

using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Forms;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator
{
    public partial class MainForm : Skin_Mac
    {
        LeftPanelForm _leftPannelForm;

        SQLTemplateForm _sqlTemplateForm;

        CollectForm _collectForm;

        public MainForm()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Font;

            dockPanel.Theme = vS2015LightTheme1;

            this.SizeChanged += MainForm_SizeChanged;

            MessageQueue.Instance.OnMessage += Instance_OnMessage;
            MessageQueue.Instance.OnComplete += Instance_OnComplete;
        }



        private void Instance_OnMessage(string msg)
        {
            this.Invoke(() =>
            {
                toolStripStatusLabel2.Text = msg;
            });
        }

        private void Instance_OnComplete()
        {
            this.Invoke(() =>
            {
                toolStripStatusLabel2.Text = "就绪";
            });
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            dockPanel.Size = new Size(this.Size.Width - 10, this.Size.Height - 82);
            toolStripStatusLabel2.Width = statusStrip1.Width - toolStripStatusLabel1.Width - 15;
        }

        #region 打开数据库视图

        private void MainForm_Load(object sender, EventArgs e)
        {
            MessageQueue.Instance.SubMsg("正在初始化界面...");

            MainForm_SizeChanged(null, null);

            new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(500);

                this.Invoke(() =>
                {
                    _leftPannelForm = new LeftPanelForm();

                    if (!DockContentHelper.Load(dockPanel))
                    {
                        MessageQueue.Instance.SubMsg("正在加载服务器配置...");
                        _leftPannelForm.OnNewContentForm += lp_newcontentForm;
                        _leftPannelForm.OnNewSqlForm += lp_newsqlForm;
                        _leftPannelForm.OnNewDataForm += _leftPannelForm_OnNewDataForm;
                        _leftPannelForm.Show(dockPanel);
                        _leftPannelForm.DockTo(dockPanel, DockStyle.Left);
                    }

                    MessageQueue.Instance.SubMsg("正在加载模板配置...");

                    _sqlTemplateForm = new SQLTemplateForm();
                    _sqlTemplateForm.Show(dockPanel);
                    _sqlTemplateForm.DockTo(dockPanel, DockStyle.Right);

                    MessageQueue.Instance.SubMsg("正在加载内容项配置...");

                    _collectForm = new CollectForm();
                    _collectForm.Show(dockPanel);
                    _collectForm.DockTo(dockPanel, DockStyle.Right);

                    MessageQueue.Instance.SubMsg("就绪");
                });
            })).Start();
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
            contentForm.Show(dockPanel);
        }

        void lp_newsqlForm(ConnectionModel conModel)
        {
            SQLForm s = new SQLForm(conModel.TableName);
            s.Text = "(" + conModel.Database + ")SQL查询窗口";
            s.ConnectionModel = conModel;
            s.Show(dockPanel);
        }

        private void _leftPannelForm_OnNewDataForm(ConnectionModel conModel)
        {
            DataForm s = new DataForm(conModel);
            s.Text = "(" + conModel.Database + conModel.TableName + ")数据编辑窗口";
            s.ConnectionModel = conModel;
            s.Show(dockPanel);
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
            var sqlForm = dockPanel.ActiveContent as SQLForm;

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
            if (DataSyncForm.Instance.Visible)
            {
                if (DataSyncForm.Instance.WindowState == FormWindowState.Minimized)
                {
                    DataSyncForm.Instance.WindowState = FormWindowState.Normal;
                }
            }
            else
            {
                DataSyncForm.Instance.Show(this);
            }

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
        /// base64转码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void base64ToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Base64Form().ShowDialog(this);
        }





        /// <summary>
        /// 清理垃圾
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FileCleaner.FileClearForm().ShowDialog(this);
        }


        TxtReplaceForm _txtReplaceForm = new TxtReplaceForm();
        /// <summary>
        /// 文件文本查找替换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileFinderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_txtReplaceForm.IsDisposed)
                _txtReplaceForm.ShowDialog(this);
        }
        /// <summary>
        /// base64转码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void base64ConvertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Base64Form().ShowDialog(this);
        }
        /// <summary>
        /// xsd代码生成工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xSDToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new XsdGeneratorForm().ShowDialog(this);
        }

        /// <summary>
        /// log读取工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void log读取工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ApiLogForm().ShowDialog(this);
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
        /// <summary>
        /// ocr工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oCR工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new OCRForm().ShowDialog(this);
        }
        /// <summary>
        /// 超大sql文件执行工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sqlFileFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SQLFileForm().ShowDialog(this);
        }

        /// <summary>
        /// 二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void qRCodeToolToolStripMenuItem_Click(object sender, EventArgs e)
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
            DockContentHelper.Save(dockPanel);
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
                DockContentHelper.Save(dockPanel);

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
