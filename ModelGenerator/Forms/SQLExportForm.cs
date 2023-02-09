using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.Common;
using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Forms
{
    public partial class SQLExportForm : CCWin.Skin_Mac
    {
        ExcelHelper excelHelper;

        CsvHelper csvHelper;

        public SQLExportForm()
        {
            InitializeComponent();

            excelHelper = new ExcelHelper();
            excelHelper.OnStop += ExcelHelper_OnStop;

            csvHelper = new CsvHelper();

            csvHelper.OnStop += CsvHelper_OnStop;
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 5;
            //progressBar1.Visible = true;
        }

        private void ExcelHelper_OnStop()
        {
            this.Invoke(new Action(() =>
            {
                progressBar1.Visible = false;
                MessageBox.Show("已成功将数据导出到Excel文件中");
                button1.Enabled = true;
            }));
        }

        private void CsvHelper_OnStop()
        {
            this.Invoke(new Action(() =>
            {
                progressBar1.Visible = false;
                MessageBox.Show("已成功将数据导出到csv文件中");
                button2.Enabled = true;
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


        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Excel文件|*.xls";
            saveFileDialog1.FileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                button1.Enabled = false;
                progressBar1.Visible = true;

                try
                {
                    var fileName = saveFileDialog1.FileName;

                    if (DataTable != null)
                    {
                        excelHelper.DataTableToExcelAsync(DataTable, fileName);
                    }
                    else
                    {
                        excelHelper.DataTableToExcel(Connection, TableName, fileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败：" + ex.Message);
                    button1.Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "csv文件|*.csv";
            saveFileDialog1.FileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.csv";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                button2.Enabled = false;
                progressBar1.Visible = true;

                try
                {
                    var fileName = saveFileDialog1.FileName;

                    if (DataTable != null)
                    {
                        csvHelper.CSV(DataTable, fileName);
                    }
                    else
                    {
                        csvHelper.CSV(Connection, TableName, fileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败：" + ex.Message);
                    button2.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 导出到JSON
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                LoadForm.ShowLoading(this);
                try
                {
                    if (DataTable == null)
                    {
                        var dbObj = DBObjectHelper.GetDBObject(Connection);

                        DataTable = dbObj.GetTabData(TableName, TableName, 1000);
                    }

                    var json = SerializeHelper.Serialize(DataTable);

                    LoadForm.HideLoading(this);

                    InvokeHelper.Invoke(this, () => new TextForm("WEF数据库工具", json, true).ShowDialog(this));
                }
                catch (Exception ex)
                {
                    LoadForm.HideLoading(this);
                    MessageBox.Show("生成Json时发生异常：" + ex.Message);
                }
            });


        }
    }
}
