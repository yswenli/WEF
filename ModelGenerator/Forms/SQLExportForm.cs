using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            button1.Enabled = false;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var text = button1.Text;
            button1.Enabled = false;
            button1.Text = "正在导出Excel";
            progressBar1.Visible = true;

            saveFileDialog1.Filter = "Excel文件|*.xls";
            saveFileDialog1.FileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var fileName = saveFileDialog1.FileName;

                    if (DataTable != null)
                    {
                        await Task.Run(() =>
                        {
                            ExcelHelper.DataTableToExcel(DataTable, fileName);
                        });
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
            button2.Enabled = false;
            button2.Text = "正在导出Csv";
            progressBar1.Visible = true;

            saveFileDialog1.Filter = "csv文件|*.csv";
            saveFileDialog1.FileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.csv";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var fileName = saveFileDialog1.FileName;

                    if (DataTable != null)
                    {
                        await Task.Run(() =>
                        {
                            CsvHelper.CSV(DataTable, fileName);
                        });
                    }
                    else
                    {
                        await Task.Run(() =>
                        {
                            CsvHelper.CSV(Connection, TableName, fileName);
                        });
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
