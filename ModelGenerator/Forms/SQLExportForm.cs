using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Forms
{
    public partial class SQLExportForm : Form
    {
        public SQLExportForm()
        {
            InitializeComponent();

            ExcelHelper.OnStart += ExcelHelper_OnStart;
            ExcelHelper.OnRunning += ExcelHelper_OnRunning;
            ExcelHelper.OnStop += ExcelHelper_OnStop;


            CsvHelper.OnStart += CsvHelper_OnStart;
            CsvHelper.OnRunning += CsvHelper_OnRunning;
            CsvHelper.OnStop += CsvHelper_OnStop;
        }

        private void ExcelHelper_OnStart()
        {
            progressBar1.Invoke(new Action(() =>
            {
                progressBar1.Visible = true;
            }));

        }
        private void ExcelHelper_OnRunning(long arg1, long arg2)
        {
            progressBar1.Invoke(new Action(() =>
            {
                if (arg2 != 0)
                    progressBar1.Value = (int)(arg1 / arg2);
            }));
        }
        private void ExcelHelper_OnStop()
        {
            progressBar1.Invoke(new Action(() =>
            {
                progressBar1.Visible = false;
            }));
        }

        private void CsvHelper_OnStart()
        {
            progressBar1.Invoke(new Action(() =>
            {
                progressBar1.Visible = true;
            }));
        }
        private void CsvHelper_OnRunning(long arg1, long arg2)
        {
            progressBar1.Invoke(new Action(() =>
            {
                if (arg2 != 0)
                    progressBar1.Value = (int)(arg1 / arg2);
            }));
        }
        private void CsvHelper_OnStop()
        {
            progressBar1.Invoke(new Action(() =>
            {
                progressBar1.Visible = false;
            }));
        }

        public DataTable DataTable
        {
            get; set;
        }

        public ConnectionModel Connection
        {
            get; set;
        }

        public string TableName
        {
            get; set;
        }

        public SQLExportForm(DataTable dataTable) : this()
        {
            DataTable = dataTable;
        }

        public SQLExportForm(ConnectionModel cnn, string tableName) : this()
        {
            Connection = cnn;
            TableName = tableName;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var text = button1.Text;

            saveFileDialog1.Filter = "Excel文件|*.xls";
            saveFileDialog1.FileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                button1.Enabled = false;
                button1.Text = "正在导出Excel";
                progressBar1.Visible = true;

                try
                {
                    var fileName = saveFileDialog1.FileName;

                    if (DataTable != null)
                    {
                        await ExcelHelper.DataTableToExcelAsync(DataTable, fileName);
                    }
                    else
                    {
                        await ExcelHelper.DataTableToExcel(Connection, TableName, fileName);
                    }

                    MessageBox.Show("已成功将数据导出到Excel文件中");

                    button1.Enabled = true;

                    button1.Text = text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败：" + ex.Message);
                }

            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var text = button1.Text;
           

            saveFileDialog1.Filter = "csv文件|*.csv";
            saveFileDialog1.FileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.csv";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                button2.Enabled = false;
                button2.Text = "正在导出Csv";
                progressBar1.Visible = true;

                try
                {
                    var fileName = saveFileDialog1.FileName;

                    if (DataTable != null)
                    {
                        await CsvHelper.CSV(DataTable, fileName);
                    }
                    else
                    {
                        await CsvHelper.CSV(Connection, TableName, fileName);
                    }

                    MessageBox.Show("已成功将数据导出到csv文件中");

                    button2.Enabled = true;

                    button2.Text = text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败：" + ex.Message);
                }
            }
        }
    }
}
