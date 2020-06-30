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

        public SQLExportForm(DataTable dataTable) : this()
        {
            DataTable = dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Excel文件|*.xls";
            saveFileDialog1.FileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExcelHelper.DataTableToExcel(DataTable, saveFileDialog1.FileName);

                    MessageBox.Show("已成功将数据导出到Excel文件中");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败：" + ex.Message);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "csv文件|*.csv";
            saveFileDialog1.FileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.csv";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    CsvHelper.CSV(saveFileDialog1.FileName, DataTable);

                    MessageBox.Show("已成功将数据导出到csv文件中");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败：" + ex.Message);
                }
            }
        }
    }
}
