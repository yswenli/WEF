using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

using WEF.Common;
using WEF.CSharpBuilder;
using WEF.DbDAL;
using WEF.Standard.DevelopTools.Common;
using WEF.Standard.DevelopTools.Model;

namespace WEF.Standard.DevelopTools
{
    public partial class BatchForm : CCWin.Skin_Mac
    {
        private string databaseName;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        private ConnectionModel connectionModel;
        public ConnectionModel ConnectionModel
        {
            get { return connectionModel; }
            set { connectionModel = value; }
        }

        IDbObject dbObject;

        Sysconfig sysconfigModel;



        public BatchForm(string _databaseName, ConnectionModel _connectionModel)
        {
            InitializeComponent();
            this.DatabaseName = _databaseName;
            this.ConnectionModel = _connectionModel;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BatchForm_Load(object sender, EventArgs e)
        {
            try
            {
                sysconfigModel = UtilsHelper.GetSysconfigModel();

                txtNamaspace.Text = sysconfigModel.Namespace;

                var connectionInfo = ConnectionInfo.GetConnectionInfo(connectionModel);

                llServer.Text = connectionInfo.Server;
                llDatabaseName.Text = DatabaseName;
                txtPath.Text = sysconfigModel.BatchDirectoryPath;


                DataTable tablesDT = null;

                dbObject = DBObjectHelper.GetDBObject(ConnectionModel);

                tablesDT = dbObject.GetTables(DatabaseName);

                DataRow[] drs = tablesDT.Select("", "name asc");

                if (null != drs && drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        lbleft.Items.Add(dr[0]);
                        tableview.Add(dr[0].ToString(), false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"初始化失败," + ex.Message);
                this.Close();
            }
        }


        Dictionary<string, bool> tableview = new Dictionary<string, bool>();

        /// <summary>
        /// 文件选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                txtPath.Text = folderBrowserDialog1.SelectedPath;

        }


        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 全部to
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnallto_Click(object sender, EventArgs e)
        {
            foreach (object o in lbleft.Items)
            {
                lbright.Items.Add(o);
            }

            lbleft.Items.Clear();
        }

        /// <summary>
        /// to
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnto_Click(object sender, EventArgs e)
        {
            if (lbleft.SelectedIndex != -1)
            {
                List<object> list = new List<object>();

                foreach (object o in lbleft.SelectedItems)
                {
                    lbright.Items.Add(o);

                    list.Add(o);
                }

                foreach (object o in list)
                {
                    lbleft.Items.Remove(o);
                }


            }
        }

        /// <summary>
        /// back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnback_Click(object sender, EventArgs e)
        {
            if (lbright.SelectedIndex != -1)
            {
                List<object> list = new List<object>();

                foreach (object o in lbright.SelectedItems)
                {
                    lbleft.Items.Add(o);

                    list.Add(o);
                }

                foreach (object o in list)
                {
                    lbright.Items.Remove(o);
                }


            }
        }

        /// <summary>
        /// allback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnallback_Click(object sender, EventArgs e)
        {
            foreach (object o in lbright.Items)
            {
                lbleft.Items.Add(o);
            }

            lbright.Items.Clear();
        }


        bool _isTitleCase = false;

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNamaspace.Text))
            {
                MessageBox.Show("命名空间不能为空!");
                return;
            }
            if (string.IsNullOrEmpty(txtPath.Text))
            {
                MessageBox.Show("请先选择要导出的文件夹!");
                return;
            }
            if (!Directory.Exists(txtPath.Text))
            {
                Directory.CreateDirectory(txtPath.Text);
            }

            if (lbright.Items.Count == 0)
            {
                MessageBox.Show("请先选择表!");
                return;
            }

            _isTitleCase = cbToupperFrstword.Checked;

            pbar.Maximum = lbright.Items.Count;

            button1.Enabled = false;

            panelbtns.Enabled = false;

            backgroundWorker1.RunWorkerAsync();

        }

        int _noneWorkCount = 0;

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            _noneWorkCount = 0;

            EntityCodeBuilder builder;

            foreach (object o in lbright.Items)
            {
                var className = o.ToString().Trim();

                if (className.IndexOf("_") > -1)
                {
                    className = className.Replace("_", " ");
                }

                if (_isTitleCase)
                {
                    className = className.ConvertToPropertyName();
                }

                className = className.Replace(" ", "");

                if (!className.StartsWith("DB", StringComparison.InvariantCultureIgnoreCase))
                {
                    className = $"DB{className}";
                }

                builder = new EntityCodeBuilder(o.ToString(), txtNamaspace.Text, className, UtilsHelper.GetColumnInfos(dbObject.GetColumnInfoList(DatabaseName, o.ToString())), tableview[o.ToString()], _isTitleCase);

                var cs = builder.Build();

                if (!string.IsNullOrEmpty(cs))
                {
                    using (StreamWriter sw = new StreamWriter(Path.Combine(txtPath.Text, className + ".cs"), false, Encoding.UTF8))
                    {
                        sw.Write(cs);
                        sw.Flush();
                    }
                }
                else
                {
                    _noneWorkCount++;
                }

                backgroundWorker1.ReportProgress(1);

                System.Threading.Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 报告进度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (pbar.Maximum > pbar.Value)
                pbar.Value += 1;
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Enabled = true;

            panelbtns.Enabled = true;

            sysconfigModel.Namespace = txtNamaspace.Text;

            UtilsHelper.WriteNamespace(txtNamaspace.Text);

            if (_noneWorkCount == 0)
            {
                MessageBox.Show("代码导出操作已完成!");
            }
            else
            {
                MessageBox.Show($"代码导出操作已完成，但其中有{_noneWorkCount}张表因无任何主键未能生成代码");
            }
        }


        /// <summary>
        /// 加载视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbView_CheckedChanged(object sender, EventArgs e)
        {
            chbView.Enabled = false;


            DataTable tablesDT = dbObject.GetVIEWs(DatabaseName);
            DataRow[] drs = tablesDT.Select("", "name asc");
            if (null != drs && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    lbleft.Items.Add(dr[0]);
                    tableview.Add(dr[0].ToString(), true);
                }
            }
        }


    }
}
