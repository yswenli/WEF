using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.Standard.DevelopTools.Common;
using WEF.Standard.DevelopTools.Model;

using WeifenLuo.WinFormsUI.Docking;

namespace WEF.Standard.DevelopTools.Forms
{
    public partial class DataForm : DockContent
    {
        DBContext _dbContext;

        DataTable _dataTable;

        public Model.ConnectionModel ConnectionModel
        {
            get; set;
        }

        public DataForm()
        {
            InitializeComponent();

            #region 防闪
            //设置窗体的双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            //利用反射设置DataGridView的双缓冲
            Type dgvType = this.dataGridView1.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(this.dataGridView1, true, null);
            #endregion


            label1.Dock = DockStyle.Fill;

            //dataGridView1.Dock = DockStyle.Fill;

            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
        }


        public DataForm(ConnectionModel cm) : this()
        {
            ConnectionModel = cm;
        }


        private void DataForm_Load(object sender, EventArgs e)
        {
            label1.Visible = true;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    _dbContext = DBObjectHelper.GetDBContext(ConnectionModel);

                    _dataTable = _dbContext.ReadDataSource(ConnectionModel.TableName);

                    if (_dataTable != null)
                    {
                        dataGridView1.Invoke(new Action(() => dataGridView1.DataSource = _dataTable));
                    }
                    else
                    {
                        dataGridView1.Invoke(new Action(() =>
                        {
                            MessageBox.Show(this, "加载数据失败");

                            Close();
                        }));
                    }
                }
                catch (Exception ex)
                {
                    dataGridView1.Invoke(new Action(() => MessageBox.Show(this, "加载数据失败：" + ex.Message)));
                }
                finally
                {
                    dataGridView1.Invoke(new Action(() => label1.Visible = false));
                }
            });
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public void Save()
        {
            if (MessageBox.Show(this, "确定要保存相关操作吗？") != DialogResult.OK)
            {
                return;
            }

            label1.Visible = true;

            Task.Run(() =>
            {
                try
                {
                    _dbContext.WriteDataSource(_dataTable);

                    InvokeHelper.Invoke(this, () => MessageBox.Show(this, "保存成功"));

                }
                catch (Exception ex)
                {
                    InvokeHelper.Invoke(this, () => MessageBox.Show(this, "更新数据发生异常：" + ex.Message));
                }
                finally
                {
                    InvokeHelper.Invoke(this, () => label1.Visible = false);
                }
            });
        }

        #region menus
        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rows = dataGridView1.SelectedRows;
            if (rows != null && rows.Count > 0)
            {
                dataGridView1.Rows.Remove(rows[0]);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        #endregion

        /// <summary>
        /// ctrl+s 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            ShortcutKeyHelper.Save(sender, e, Save);
        }
        /// <summary>
        /// ctrl+s 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataForm_KeyUp(object sender, KeyEventArgs e)
        {
            ShortcutKeyHelper.Save(sender, e, Save);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}
