using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WEF;
using WEF.ModelGenerator.Common;
using WEF.Common;
using System.Threading.Tasks;
using System.Linq;

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

                this.Invoke(new Action(() =>
                {
                    cnnTxt.Text = DBObjectHelper.GetCnnString(ConnectionModel, DatabaseName);

                    #region structure

                    columnsdt = dbObject.GetColumnInfoList(DatabaseName, TableName);

                    gridColumns.DataSource = columnsdt;

                    #endregion

                    cbPrimarykey.Items.Clear();

                    if (null != primarykeydt && primarykeydt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in primarykeydt.Rows)
                        {
                            cbPrimarykey.Items.Add(dr["ColumnName"].ToString());
                        }

                        cbPrimarykey.SelectedIndex = 0;
                    }



                    txtClassName.Text = TableName.Trim().Replace(' ', '_');
                    txtnamespace.Text = UtilsHelper.ReadNamespace();
                }));
                LoadForm.HideLoading(1);
            });
        }



        /// <summary>
        /// 添加主键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPrimarykey_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = gridColumns.SelectedRows;
            if (null != rows && rows.Count > 0)
            {
                foreach (DataGridViewRow row in rows)
                {
                    object temp = row.Cells[1].Value;

                    if (!cbPrimarykey.Items.Contains(temp))
                    {
                        cbPrimarykey.Items.Add(temp);
                    }
                }

                cbPrimarykey.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 删除主键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemovePrimarykey_Click(object sender, EventArgs e)
        {
            if (cbPrimarykey.SelectedIndex >= 0)
            {
                cbPrimarykey.Items.RemoveAt(cbPrimarykey.SelectedIndex);
            }

            if (cbPrimarykey.Items.Count > 0)
                cbPrimarykey.SelectedIndex = 0;
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

        #region 代码页快捷菜单

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtContent.SelectAll();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtContent.Text);
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
       
    }
}
