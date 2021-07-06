using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.Common;
using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Forms;

namespace WEF.ModelGenerator
{
    public partial class ContentForm : WeifenLuo.WinFormsUI.Docking.DockContent
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
                var dbObject = DBObjectHelper.GetDBObject(ConnectionModel);

                DataTable primarykeydt = dbObject.GetKeyName(DatabaseName, TableName);

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

                    if (TableName.StartsWith("DB"))
                    {
                        txtClassName.Text = $"{TableName.Trim().Replace(" ", "").Replace("_", "")}";
                    }
                    else
                    {
                        txtClassName.Text = $"DB{TableName.Trim().Replace(" ", "").Replace("_", "")}";
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
        private void GenerateModel()
        {
            _isOk = false;

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

            var cs = builder.Builder();

            if (string.IsNullOrEmpty(cs))
            {
                txtContent.Text = "当前表无任何主键，请在详情界面上选择某行添加为主键后再执行此操作";
            }
            else
            {
                txtContent.Text = cs;
            }

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
                GenerateModel();

                if (_isOk)
                {
                    tabControl1.SelectedIndex = 1;
                }
                else
                {
                    tabControl1.SelectedIndex = 0;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerateModel();

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
            new TextForm("WEF代码生成工具", json, true).ShowDialog(this);
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

        #endregion


    }
}
