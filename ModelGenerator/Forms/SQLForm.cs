/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：1e7ab7e0-8733-46b2-a556-1fbb0ad96298
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WEF.ModelGenerator
 * 类名称：SQLForm
 * 文件名：SQLForm
 * 创建年份：2015
 * 创建时间：2015-09-23 14:54:06
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.Common;
using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Controls;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Forms
{
    public partial class SQLForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        string _currentSql = "";

        public SQLForm()
        {
            InitializeComponent();

            #region 加速datagridview数据加载速度
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            #endregion

            #region 防闪
            //设置窗体的双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            //利用反射设置DataGridView的双缓冲
            Type dgvType = this.dataGridView1.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(this.dataGridView1, true, null);
            #endregion

            AutoTextBox.TextBox.RegistFindForm();

            AutoTextBox.TextBox.ContextMenuStrip = contextMenuStrip1;
        }

        public SQLForm(string tableName) : this()
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                AutoTextBox.TextBox.Text = "select * from " + tableName;
            }
        }

        public ConnectionModel ConnectionModel
        {
            get; set;
        }

        Stopwatch stopwatch = new Stopwatch();

        AutoTextBox autoTextBox1 = null;

        public AutoTextBox AutoTextBox
        {
            get
            {
                if (autoTextBox1 == null)
                    autoTextBox1 = new AutoTextBox();
                return autoTextBox1;
            }
        }

        private void SQLForm_Load(object sender, System.EventArgs e)
        {
            cnnTxt.Text = DBObjectHelper.GetCnnString(ConnectionModel, ConnectionModel.Database);

            AutoTextBox.Dock = DockStyle.Fill;

            AutoTextBox.KeyDown += autoTextBox1_KeyUp;

            AutoTextBox.TabIndex = 0;

            splitContainer1.Panel1.Controls.Add(AutoTextBox);

            stopwatch.Start();
        }


        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoTextBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            ShortcutKeyHelper.AllSelect(sender, e);

            ShortcutKeyHelper.Run(sender, e, () => RunSql());
        }

        public void RunSql()
        {
            _currentSql = AutoTextBox.TextBox.Text;

            stopwatch.Restart();

            LoadForm.ShowLoading(this);

            Task.Factory.StartNew(() =>
            {
                try
                {
                    var dbObject = DBObjectHelper.GetDBObject(ConnectionModel);

                    if (string.IsNullOrEmpty(_currentSql))
                    {
                        LoadForm.HideLoading(this); ;

                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show(this, "sql内容不能为空!");
                        }));

                        return;
                    }

                    this.Invoke(new Action(() =>
                    {
                        if (!string.IsNullOrWhiteSpace(AutoTextBox.TextBox.SelectedText))
                        {
                            _currentSql = AutoTextBox.TextBox.SelectedText.Trim();

                            if (string.IsNullOrEmpty(_currentSql))
                            {
                                LoadForm.HideLoading(this); ;
                                MessageBox.Show(this, "sql内容不能为空!");
                                return;
                            }
                        }

                        dataGridView1.DataSource = null;

                    }));

                    if (_currentSql.StartsWith("select", StringComparison.InvariantCultureIgnoreCase))
                    {
                        DataSet ds = null;
                        try
                        {
                            int max = 1000;

                            ds = dbObject.Query(ConnectionModel.Database, _currentSql);

                            stopwatch.Stop();

                            if (ds != null && ds.Tables != null)
                            {
                                var dt = ds.Tables[0];

                                if (dt != null)
                                {
                                    var count = dt.Rows.Count;

                                    if (count > max)
                                    {
                                        for (int i = max; i < count; i++)
                                        {
                                            dt.Rows.RemoveAt(max);
                                        }
                                    }


                                    var dList = new List<int>();

                                    for (int i = 0; i < dt.Columns.Count; i++)
                                    {
                                        if (dt.Columns[i].DataType == typeof(DateTime))
                                        {
                                            dList.Add(i);
                                        }

                                        if (dt.Columns[i].DataType == typeof(byte[]))
                                        {
                                            foreach (DataRow dr in dt.Rows)
                                            {
                                                dr[i] = null;
                                            }
                                        }
                                    }

                                    dataGridView1.Invoke(new Action(() =>
                                    {
                                        dataGridView1.DataSource = dt;

                                        if (dList.Any())
                                        {
                                            foreach (var item in dList)
                                            {
                                                dataGridView1.Columns[item].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss.fff";
                                            }
                                        }

                                        lbl_execute.Text = $"当前显示{(max > count ? count : max)}行，影响数据行数：{count} 耗时：{stopwatch.Elapsed.TotalSeconds} s";
                                    }));
                                }
                            }
                            LoadForm.HideLoading(this); ;
                        }
                        catch (Exception ex)
                        {
                            LoadForm.HideLoading(this);
                            this.Invoke(new Action(() =>
                            {

                                if (ex.Message.IndexOf("Unable to convert MySQL date/time value to System.DateTime", StringComparison.InvariantCultureIgnoreCase) > -1)
                                {
                                    var msg = ex.Message + @"
解决办法：
1、将该字段的缺省值设置为null，而不是0000-00-00/0000-00-00 00:00:00的情况；
2、在链接MySQL的字符串中添加：Convert Zero Datetime=True
3、将该字段设置成字符串类型；";
                                    MessageBox.Show(this, $"查询发生异常，ex:{msg}");
                                }
                                else
                                {
                                    MessageBox.Show(this, $"查询发生异常，ex:" + ex.Message);
                                }
                            }));
                        }
                        finally
                        {
                            //释放内存资源
                            ds?.Tables[0]?.Dispose();
                            ds?.Dispose();
                            GC.Collect();
                        }
                    }
                    else if (_currentSql.StartsWith("print", StringComparison.InvariantCultureIgnoreCase))
                    {
                        try
                        {
                            var obj = dbObject.GetSingle(ConnectionModel.Database, _currentSql);

                            lbl_execute.Invoke(new Action(() =>
                            {
                                dataGridView1.DataSource = null;
                                lbl_execute.Text = $"影响数据行数：1 耗时：{stopwatch.Elapsed.TotalSeconds} s";
                                dataGridView1.DataSource = new { 结果 = obj?.ToString() ?? "" };
                            }));
                        }
                        finally
                        {
                            LoadForm.HideLoading(this);
                        }
                    }
                    else
                    {
                        try
                        {
                            var count = dbObject.ExecuteSql(ConnectionModel.Database, _currentSql);

                            stopwatch.Stop();

                            lbl_execute.Invoke(new Action(() =>
                            {
                                lbl_execute.Text = $"影响数据行数：{count} 耗时：{stopwatch.Elapsed.TotalSeconds} s";
                            }));

                            LoadForm.HideLoading(this); ;
                        }
                        catch (Exception ex)
                        {
                            LoadForm.HideLoading(this); ;
                            this.Invoke(new Action(() =>
                            {
                                MessageBox.Show(this, $"操作发生异常，ex:" + ex.Message);
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show(this, $"操作发生异常，ex:" + ex.Message);
                    }));
                }
            });
        }

        #region 快捷菜单

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cells = dataGridView1.SelectedCells;

            if (cells != null && cells.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (DataGridViewCell cell in cells)
                {
                    if (cell == cells[cells.Count - 1])
                    {
                        sb.Append($"{cell.Value}");
                    }
                    else
                    {
                        sb.Append($"{cell.Value},");
                    }
                }

                Clipboard.SetText(sb.ToString());
            }
        }

        private void exportDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentSql.StartsWith("select", StringComparison.InvariantCultureIgnoreCase))
            {
                Task.Run(() =>
                {
                    LoadForm.ShowLoading(this);

                    try
                    {
                        WEF.DbDAL.IDbObject dbObject = DBObjectHelper.GetDBObject(ConnectionModel);

                        var ds = dbObject.Query(ConnectionModel.Database, _currentSql);

                        if (ds != null && ds.Tables != null)
                        {
                            var dt = ds.Tables[0];
                            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                            {
                                LoadForm.HideLoading(this);
                                InvokeHelper.Invoke(this, () => new SQLExportForm(dt).ShowDialog());
                                return;
                            }
                        }
                        LoadForm.HideLoading(this);
                    }
                    catch (Exception ex)
                    {
                        LoadForm.HideLoading(this);
                        MessageBox.Show("导出数据时发生异常：" + ex.Message);
                    }
                });
            }
        }

        private void 生成JsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_currentSql.StartsWith("select", StringComparison.InvariantCultureIgnoreCase))
            {

                Task.Run(() =>
                {
                    LoadForm.ShowLoading(this);

                    try
                    {
                        WEF.DbDAL.IDbObject dbObject = DBObjectHelper.GetDBObject(ConnectionModel);

                        var ds = dbObject.Query(ConnectionModel.Database, _currentSql);

                        if (ds != null && ds.Tables != null)
                        {
                            var dt = ds.Tables[0];

                            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                            {
                                if (dt.Rows.Count >= 1000)
                                {
                                    MessageBox.Show("生成失败，当前数据量超过1000条");
                                    LoadForm.HideLoading(this);
                                    return;
                                }

                                var json = SerializeHelper.Serialize(dt);

                                LoadForm.HideLoading(this);

                                InvokeHelper.Invoke(this, () => new TextForm("WEF数据库工具", json, true).ShowDialog(this));

                                return;
                            }
                        }

                        LoadForm.HideLoading(this);
                    }
                    catch (Exception ex)
                    {
                        LoadForm.HideLoading(this);
                        MessageBox.Show("生成Json时发生异常：" + ex.Message);
                    }
                });


            }

        }

        /// <summary>
        /// 生成sql
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentSql)) return;
            if (_currentSql.StartsWith("select", StringComparison.InvariantCultureIgnoreCase))
            {

                Task.Run(() =>
                {
                    LoadForm.ShowLoading(this);

                    try
                    {
                        WEF.DbDAL.IDbObject dbObject = DBObjectHelper.GetDBObject(ConnectionModel);

                        var ds = dbObject.Query(ConnectionModel.Database, _currentSql);

                        if (ds != null && ds.Tables != null)
                        {
                            var dt = ds.Tables[0];

                            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                            {
                                if (dt.Rows.Count >= 2000)
                                {
                                    MessageBox.Show("生成失败，当前数据量超过2000条");
                                    return;
                                }

                                var sql = new Common.StringPlus();
                                var tableName = Common.StringPlus.GetTableName(_currentSql);
                                if (string.IsNullOrEmpty(tableName))
                                {
                                    tableName = "TableName";
                                }

                                foreach (DataRow dr in dt.Rows)
                                {
                                    var sp = new Common.StringPlus($"INSERT INTO {tableName}(");
                                    var columns = dt.Columns;
                                    foreach (DataColumn col in columns)
                                    {
                                        sp.Append(col.ColumnName);
                                        sp.Append(",");
                                    }
                                    sp.RemoveLast();
                                    sp.Append(")VALUES(");
                                    for (int i = 0; i < columns.Count; i++)
                                    {
                                        var val = "null";
                                        if (dr[i] != null)
                                        {
                                            val = dr[i].ToString();
                                        }
                                        sp.Append($"'{val}'");
                                        sp.Append(",");
                                    }
                                    sp.RemoveLast();
                                    sp.Append(")");
                                    sql.AppendLine(sp.ToString());
                                }

                                InvokeHelper.Invoke(this, () => new TextForm("WEF数据库工具", sql.ToString(), true).ShowDialog(this));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                        MessageBox.Show("生成Json时发生异常：" + ex.Message);
                    }
                    finally
                    {
                        LoadForm.HideLoading(this);
                    }
                });
            }

        }

        #endregion

        #region sql输入框中的快捷菜单
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var selectTxt = autoTextBox1.TextBox.SelectedText;
            if (!string.IsNullOrWhiteSpace(selectTxt))
            {
                Clipboard.SetText(selectTxt);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var text = Clipboard.GetText();
            var offset = autoTextBox1.TextBox.SelectionStart;
            if (!string.IsNullOrEmpty(text))
            {
                if (autoTextBox1.TextBox.Text.Length < 1)
                {
                    autoTextBox1.TextBox.Text = text;
                }
                else
                {
                    if (autoTextBox1.TextBox.Text.Length <= offset)
                    {
                        autoTextBox1.TextBox.Text += text;
                    }
                    else if (autoTextBox1.TextBox.Text.Length > offset)
                    {
                        var first = autoTextBox1.TextBox.Text.Substring(0, offset);
                        var last = autoTextBox1.TextBox.Text.Substring(offset);
                        autoTextBox1.TextBox.Text = first + text + last;
                    }
                }
            }
        }


        private void allSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(autoTextBox1.TextBox.Text))
            {
                return;
            }
            autoTextBox1.TextBox.SelectAll();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunSql();
        }


        #endregion


    }
}
