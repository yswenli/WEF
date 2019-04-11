using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WEF.DbDAL;
using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator
{
    public partial class BatchForm : CCWin.Skin_Mac
    {
        private string databaseName;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        private Connection connectionModel;
        public Connection ConnectionModel
        {
            get { return connectionModel; }
            set { connectionModel = value; }
        }

        IDbObject dbObject;

        Sysconfig sysconfigModel;



        public BatchForm(string _databaseName, Connection _ConnectionModel)
        {
            InitializeComponent();
            this.DatabaseName = _databaseName;
            this.ConnectionModel = _ConnectionModel;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BatchForm_Load(object sender, EventArgs e)
        {
            sysconfigModel = UtilsHelper.GetSysconfigModel();
            txtNamaspace.Text = sysconfigModel.Namespace;

            var index = connectionModel.Name.IndexOf("[");

            if (index < 0)
            {
                index = connectionModel.Name.IndexOf("(");
            }

            llServer.Text = connectionModel.Name.Substring(0, index);
            llDatabaseName.Text = DatabaseName;
            txtPath.Text = sysconfigModel.BatchDirectoryPath;


            DataTable tablesDT = null;
            if (ConnectionModel.DbType.Equals(DatabaseType.MsAccess.ToString()))
            {
                dbObject = new WEF.DbDAL.OleDb.DbObject(ConnectionModel.ConnectionString);
            }
            else if (ConnectionModel.DbType.Equals(DatabaseType.SqlServer.ToString()))
            {
                dbObject = new WEF.DbDAL.SQL2000.DbObject(ConnectionModel.ConnectionString);
            }
            else if (ConnectionModel.DbType.Equals(DatabaseType.SqlServer9.ToString()))
            {
                dbObject = new WEF.DbDAL.SQL2005.DbObject(ConnectionModel.ConnectionString);
            }
            else if (ConnectionModel.DbType.Equals(DatabaseType.Oracle.ToString()))
            {
                dbObject = new WEF.DbDAL.Oracle.DbObject(ConnectionModel.ConnectionString);
            }
            else if (ConnectionModel.DbType.Equals(DatabaseType.Sqlite3.ToString()))
            {
                dbObject = new WEF.DbDAL.SQLite.DbObject(ConnectionModel.ConnectionString);
            }
            else if (ConnectionModel.DbType.Equals(DatabaseType.MySql.ToString()))
            {
                dbObject = new WEF.DbDAL.MySql.DbObject(ConnectionModel.ConnectionString);
            }

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



            pbar.Maximum = lbright.Items.Count;


            button1.Enabled = false;
            panelbtns.Enabled = false;

            backgroundWorker1.RunWorkerAsync();

        }


        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            EntityCodeBuilder builder;

            foreach (object o in lbright.Items)
            {
                builder = new EntityCodeBuilder(o.ToString(), txtNamaspace.Text, o.ToString().Trim().Replace(" ", ""), UtilsHelper.GetColumnInfos(dbObject.GetColumnInfoList(DatabaseName, o.ToString())), tableview[o.ToString()], cbToupperFrstword.Checked);

                using (StreamWriter sw = new StreamWriter(Path.Combine(txtPath.Text, o.ToString().Trim().Replace(' ', '_') + ".cs"), false, Encoding.UTF8))
                {
                    sw.Write(builder.Builder());
                    sw.Flush();
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
            MessageBox.Show("生成成功!");
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
