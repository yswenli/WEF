using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;

using WeifenLuo.WinFormsUI.Docking;

namespace WEF.ModelGenerator.Forms
{
    public partial class DataForm : DockContent
    {
        DBContext _dbContext;

        DataTable _dataTable;

        bool _loading = true;

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

            dataGridView1.Dock = DockStyle.Fill;

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

                    _dataTable = _dbContext.GetDataSource(ConnectionModel.TableName);

                    if (_dataTable != null)
                    {
                        dataGridView1.Invoke(new Action(() => dataGridView1.DataSource = _dataTable));

                        _loading = false;
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


        private async void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (!_loading)
            {
                label1.Visible = true;
                try
                {
                    _dbContext.UpdateDataSource(_dataTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "添加数据发生异常：" + ex.Message);
                }
                finally
                {
                    label1.Visible = false;
                }
            }
        }


        private async void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            label1.Visible = true;
            try
            {
                _dbContext.UpdateDataSource(_dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "删除数据发生异常：" + ex.Message);
            }
            finally
            {
                label1.Visible = false;
            }
        }


        private async void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            label1.Visible = true;
            try
            {
                _dbContext.UpdateDataSource(_dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "更新数据发生异常：" + ex.Message);
            }
            finally
            {
                label1.Visible = false;
            }
        }
    }
}
