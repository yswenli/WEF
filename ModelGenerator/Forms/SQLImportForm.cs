using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CCWin;

using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Forms
{
    public partial class SQLImportForm : Skin_Mac
    {
        public ConnectionModel Connection
        {
            get; set;
        }

        public string TableName
        {
            get; set;
        }

        public SQLImportForm()
        {
            InitializeComponent();
        }

        public SQLImportForm(ConnectionModel cnn, string tableName) : this()
        {
            Connection = cnn;
            TableName = tableName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                skinWaterTextBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = null;

            var fileName = skinWaterTextBox1.Text;

            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
            {
                return;
            }

            if (fileName.EndsWith("*.xls"))
            {
                dt = ExcelHelper.ImportFromFile(fileName);
            }
            else
            {
                dt = CsvHelper.ImportToDataTable(fileName);
            }

            try
            {
                DBContext dBContext = DBObjectHelper.GetDBContext(Connection);

                var result = dBContext.BulkInsert(TableName, dt);

                MessageBox.Show($"操作完成，已成功导入{result}条");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "导入数据失败，" + ex.Message);
            }
        }
    }
}
