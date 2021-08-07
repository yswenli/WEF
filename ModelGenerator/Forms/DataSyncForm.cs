using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CCWin;

using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Forms
{
    public partial class DataSyncForm : Skin_Mac
    {

        #region static

        static DataSyncForm _instance;

        static DataSyncForm()
        {
            _instance = new DataSyncForm();
        }

        /// <summary>
        /// 获取数据传输
        /// </summary>
        public static DataSyncForm Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        List<DataSyncConfig> _configs;

        DataSyncForm()
        {
            InitializeComponent();

            LoadData();

            SyncDataHelper.OnLog += SyncDataHelper_OnLog;
        }

        private void SyncDataHelper_OnLog(string logTxt)
        {
            dataGridView2.Invoke(new Action(() =>
            {
                dataGridView2.Rows.Insert(0, logTxt);
            }));
        }

        void LoadData()
        {
            _configs = DataSyncConfig.Read();

            if (_configs == null)
            {
                _configs = new List<DataSyncConfig>();
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _configs;
        }

        private void DataSyncForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new DataSyncConfigForm(null).ShowDialog(this);
            LoadData();
        }

        #region menu
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cells = dataGridView1.SelectedCells;
            if (cells != null && cells.Count > 0)
            {
                var cell = cells[0];
                var row = dataGridView1.Rows[cell.RowIndex];
                var cid = row.Cells[0].Value?.ToString() ?? "";
                _configs = DataSyncConfig.Read();
                if (_configs == null)
                {
                    _configs = new List<DataSyncConfig>();
                }
                var config = _configs.FirstOrDefault(b => b.ID == cid);
                if (config != null)
                {
                    new DataSyncConfigForm(config).ShowDialog(this);
                    LoadData();
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cells = dataGridView1.SelectedCells;
            if (cells != null && cells.Count > 0)
            {
                var cell = cells[0];
                var row = dataGridView1.Rows[cell.RowIndex];
                var cid = row.Cells[0].Value?.ToString() ?? "";
                _configs = DataSyncConfig.Read();
                if (_configs == null)
                {
                    _configs = new List<DataSyncConfig>();
                }
                var config = _configs.FirstOrDefault(b => b.ID == cid);
                if (config != null)
                {
                    if (MessageBox.Show(this, "确定要删除吗？", "数据传输工具", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _configs.Remove(config);
                        DataSyncConfig.Save(_configs);
                        LoadData();
                    }
                }
            }
        }

        #endregion

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (button2.Text == "开始传输")
            {
                var cells = dataGridView1.SelectedCells;
                if (cells != null && cells.Count > 0)
                {
                    var cell = cells[0];
                    var row = dataGridView1.Rows[cell.RowIndex];
                    var cid = row.Cells[0].Value?.ToString() ?? "";
                    _configs = DataSyncConfig.Read();
                    if (_configs == null)
                    {
                        _configs = new List<DataSyncConfig>();
                    }
                    var config = _configs.FirstOrDefault(b => b.ID == cid);
                    if (config != null)
                    {
                        new DataSyncConfigForm(config).ShowDialog(this);
                        LoadData();
                    }
                }
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "开始传输")
            {
                button2.Text = "停止传输";
                contextMenuStrip1.Enabled = false;
                button1.Enabled = false;

                SyncDataHelper.Start();
            }
            else
            {
                SyncDataHelper.Stop();

                button2.Text = "开始传输";
                contextMenuStrip1.Enabled = true;
                button1.Enabled = true;
            }
        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedCells = dataGridView2.SelectedCells;
            if (selectedCells != null && selectedCells.Count > 0)
            {
                var cell = selectedCells[0];
                if (cell.ColumnIndex == 0)
                {
                    new TextForm("传输日志", cell.Value?.ToString() ?? "", true).ShowDialog(this);
                }
            }
        }
    }
}
