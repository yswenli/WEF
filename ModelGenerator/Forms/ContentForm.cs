using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SqlSugar;

using WEF.Common;
using WEF.CSharpBuilder;
using WEF.Db;
using WEF.DbDAL;
using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Forms;
using WEF.ModelGenerator.Model;

using WeifenLuo.WinFormsUI.Docking;


namespace WEF.ModelGenerator
{
    public partial class ContentForm : DockContent
    {
        public ContentForm()
        {
            InitializeComponent();
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private Model.ConnectionModel connectionModel;

        public Model.ConnectionModel ConnectionModel
        {
            set { connectionModel = value; }
            get { return connectionModel; }
        }

        private string tableName;
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        private bool isView = false;
        public bool IsView
        {
            get { return isView; }
            set { isView = value; }
        }

        private string databaseName;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        string content;

        DataTable columnsdt = null;

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentForm_Load(object sender, EventArgs e)
        {
            LoadForm.ShowLoading(this);

            if (ConnectionModel != null)
            {
                this.DatabaseName = ConnectionModel.Database;
                this.TableName = ConnectionModel.TableName;
                this.IsView = ConnectionModel.IsView;
            }


            Task.Factory.StartNew(() =>
            {
                IDbObject dbObject;
                DataTable primarykeydt;
                try
                {
                    if (ConnectionModel.DbType == "SqlServer")
                    {
                        ConnectionModel.DbType = "SqlServer9";
                    }

                    dbObject = DBObjectHelper.GetDBObject(ConnectionModel);

                    primarykeydt = dbObject.GetKeyName(DatabaseName, TableName);
                }
                catch
                {
                    if (ConnectionModel.DbType == "SqlServer9")
                    {
                        ConnectionModel.DbType = "SqlServer";
                    }

                    try
                    {
                        dbObject = DBObjectHelper.GetDBObject(ConnectionModel);

                        primarykeydt = dbObject.GetKeyName(DatabaseName, TableName);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        return;
                    }
                }

                if (primarykeydt.Rows.Count == 0)
                {
                    MessageBox.Show("当前表找不到任何主键，请选择上面的列表行来添加主键");
                }

                columnsdt = dbObject.GetColumnInfoList(DatabaseName, TableName);

                this.Invoke(new Action(() =>
                {
                    cnnTxt.Text = DBObjectHelper.GetCnnString(ConnectionModel, DatabaseName);

                    gridColumns.DataSource = columnsdt;

                    cbPrimarykey.Items.Clear();

                    if (null != primarykeydt && primarykeydt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in primarykeydt.Rows)
                        {
                            cbPrimarykey.Items.Add(dr["ColumnName"].ToString());
                        }
                        cbPrimarykey.SelectedIndex = 0;
                    }
                    var className = TableName.Trim();

                    if (TableName.IndexOf("_") > -1)
                    {
                        className = className.ConvertToPropertyName();
                    }

                    if (cbToupperFrstword.Checked)
                    {
                        className = className.ConvertToPropertyName();
                    }

                    if (className.StartsWith("DB", StringComparison.InvariantCultureIgnoreCase))
                    {
                        txtClassName.Text = className;
                    }
                    else
                    {
                        txtClassName.Text = $"DB{className}";
                    }

                    txtnamespace.Text = UtilsHelper.ReadNamespace();
                }));
                LoadForm.HideLoading(this);
            });
        }




        bool _isOk = false;


        /// <summary>
        /// 实体生成
        /// </summary>
        /// <param name="simple"></param>
        private void GenerateModel(bool simple = false)
        {
            _isOk = false;

            try
            {
                if (string.IsNullOrEmpty(txtnamespace.Text))
                {
                    MessageBox.Show("命名空间不能为空!");
                    return;
                }
                if (string.IsNullOrEmpty(txtClassName.Text))
                {
                    MessageBox.Show("类名不能为空!");
                    return;
                }

                UtilsHelper.WriteNamespace(txtnamespace.Text);

                List<ColumnInfo> columns = UtilsHelper.GetColumnInfos(columnsdt);

                foreach (ColumnInfo col in columns)
                {
                    col.IsPrimaryKey = false;

                    foreach (object o in cbPrimarykey.Items)
                    {
                        if (col.Name.Equals(o.ToString()))
                        {
                            col.IsPrimaryKey = true;
                            break;
                        }
                    }
                }

                EntityCodeBuilder builder = new EntityCodeBuilder(TableName, txtnamespace.Text, txtClassName.Text, columns, IsView, cbToupperFrstword.Checked, ConnectionModel.DbType);

                var cs = builder.Build(simple);

                if (string.IsNullOrEmpty(cs))
                {
                    txtContent.Text = "当前表无任何主键，请在详情界面上选择某行添加为主键后再执行此操作";
                }
                else
                {
                    txtContent.Text = cs;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }


            _isOk = true;
        }

        void GenerateSqlSugarModel()
        {
            _isOk = false;

            var sugarConfig = new ConnectionConfig()
            {
                ConfigId = "1111",
                ConnectionString = ConnectionModel.ConnectionString,
                IsAutoCloseConnection = true
            };

            switch (ConnectionModel.DbType)
            {
                case "SqlServer":
                case "SqlServer9":
                    sugarConfig.DbType = SqlSugar.DbType.SqlServer;
                    sugarConfig.ConnectionString = ConnectionModel.ConnectionString.Replace("master", ConnectionModel.Database);
                    break;
                case "MySql":
                    sugarConfig.DbType = SqlSugar.DbType.MySql;
                    break;
                case "Oracle":
                    sugarConfig.DbType = SqlSugar.DbType.Oracle;
                    break;
                case "MariaDB":
                    sugarConfig.DbType = SqlSugar.DbType.MySql;
                    break;
                case "Sqlite":
                    sugarConfig.DbType = SqlSugar.DbType.Sqlite;
                    break;
            }
            var sqlclient = new SqlSugarClient(sugarConfig);
            //var csFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates), $"WEF_{DateTime.Now.Ticks}");
            var dic= sqlclient.DbFirst
                .Where(TableName)
                .IsCreateAttribute()
                .IsCreateDefaultValue()
                .ToClassStringList(txtnamespace.Text);

            txtContent.Text = dic.Values.FirstOrDefault();

            _isOk = true;
        }

        /// <summary>
        /// 生成Json
        /// </summary>
        /// <returns></returns>
        private string GenerateJson()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("{");

            List<ColumnInfo> dbColumns = UtilsHelper.GetColumnInfos(columnsdt);

            var columns = DBToCSharp.DbtoCSColumns(dbColumns, ConnectionModel.DbType);

            bool isFirst = true;

            foreach (ColumnInfo column in columns)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    sb.Append(",");
                }

                if (column.DataTypeName == "string")
                {
                    sb.AppendLine($"\"{column.Name}\":\"\"");
                }
                else if (column.DataTypeName == "bool" || column.DataTypeName == "bool?")
                {
                    sb.AppendLine($"\"{column.Name}\":true");
                }
                else if (column.DataTypeName == "DateTime" || column.DataTypeName == "DateTime?")
                {
                    sb.AppendLine($"\"{column.Name}\":\"{DateTime.Now}\"");
                }
                else
                {
                    sb.AppendLine($"\"{column.Name}\":0");
                }
            }
            sb.AppendLine("}");

            return SerializeHelper.ExpandJson(sb.ToString());
        }

        #region 代码页快捷菜单

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtContent.SelectAll();
            txtContent.Focus();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var txt = txtContent.SelectedText;

            if (!string.IsNullOrEmpty(txt))
            {
                Clipboard.SetText(txt);
            }
        }

        /// <summary>
        /// 预览界面快捷菜单保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveEntity.FileName = txtClassName.Text;

            saveEntity.Filter = "CS 文件|*.cs";

            if (saveEntity.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(saveEntity.FileName, false, Encoding.UTF8))
                {
                    sw.Write(txtContent.Text);
                    sw.Close();
                }
            }

        }

        #endregion


        /// <summary>
        /// 点击预览界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                button2_Click(null, null);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                GenerateSqlSugarModel();
            }
            else
            {
                GenerateModel(checkBox1.Checked);
            }

            if (_isOk)
            {
                tabControl1.SelectedIndex = 1;
            }
            else
            {
                tabControl1.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 生成json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            var json = GenerateJson();
            new TextForm("生成Josn-WEF代码生成工具", json, true).ShowDialog(this);
        }

        //快捷业务代码生成
        private void button4_Click(object sender, EventArgs e)
        {
            var className = txtClassName.Text;
            if (!string.IsNullOrEmpty(className))
            {
                new TemplateToCodeFastForm(className).ShowDialog(this);
            }

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            saveEntity.FileName = txtClassName.Text;
            saveEntity.Filter = "CS 文件|*.cs";

            if (saveEntity.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(saveEntity.FileName, false, Encoding.UTF8))
                {
                    sw.Write(txtContent.Text);
                    sw.Close();
                }
            }
        }

        #region 快捷菜单
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (contextMenuCopy.SourceControl == cnnTxt)
            {
                cnnTxt.SelectAll();
                Clipboard.SetText(cnnTxt.Text);
            }
            else
            {
                var cells = gridColumns.SelectedCells;

                if (cells != null)
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
        }

        private void 设为主键ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cells = gridColumns.SelectedCells;

            if (cells != null && cells.Count > 0)
            {
                var selectVal = gridColumns.Rows[cells[0].RowIndex].Cells[1].Value;

                if (!cbPrimarykey.Items.Contains(selectVal))
                {
                    cbPrimarykey.Items.Add(selectVal);
                }

                cbPrimarykey.SelectedIndex = 0;
            }
        }

        private void 取消主键ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cells = gridColumns.SelectedCells;

            if (cells != null && cells.Count > 0)
            {
                var selectVal = gridColumns.Rows[cells[0].RowIndex].Cells[1].Value;

                if (cbPrimarykey.Items.Contains(selectVal))
                {
                    cbPrimarykey.Items.Remove(selectVal);
                }
            }

            if (cbPrimarykey.Items.Count > 0)
                cbPrimarykey.SelectedIndex = 0;
        }

        /// <summary>
        /// 生成markdown文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateMarkDownDocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridColumns.Rows != null && gridColumns.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"### 表名 {TableName}");
                sb.AppendLine("|字段|类型|可空|默认|注释|");
                sb.AppendLine("|:----    |:-------    |:--- |-- -|------      |");

                foreach (DataGridViewRow row in gridColumns.Rows)
                {
                    sb.AppendLine($"|{row.Cells["ColumnName"].Value}|{row.Cells["TypeName"].Value}({row.Cells["Length"].Value})|{row.Cells["cisNull"].Value}|{row.Cells["defaultVal"].Value}|{row.Cells["deText"].Value}|");
                }
                new TextForm("生成MarkDown文档-WEF代码生成工具", sb.ToString(), true).ShowDialog(this);
            }
        }
        /// <summary>
        /// 生成markdown文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            generateMarkDownDocToolStripMenuItem_Click(null, null);
        }
        #endregion


    }
}
