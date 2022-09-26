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

            skinWaterTextBox1.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    var result = 0;

                    if (fileName.EndsWith(".xls") || fileName.EndsWith(".xlsx"))
                    {
                        dt = ExcelHelper.ImportFromFile(fileName);
                    }
                    else if (fileName.EndsWith(".csv"))
                    {
                        dt = CsvHelper.ImportToDataTable(fileName);
                    }
                    else if (fileName.EndsWith(".sql"))
                    {
                        var context = DBObjectHelper.GetDBContext(Connection);
                        var sqls = FileHelper.ReadTxt(fileName);
                        result = context.ExecuteNonQuery(sqls);
                        this.Invoke(() => MessageBox.Show($"操作完成，已成功导入{result}条"));
                        return;
                    }
                    else
                    {
                        this.Invoke(() => MessageBox.Show(this, "暂不支持的文件类型"));
                        return;
                    }

                    var dBContext = DBObjectHelper.GetDBContext(Connection);

                    result = dBContext.BulkInsert(TableName, dt);

                    this.Invoke(() => MessageBox.Show(this, $"操作完成，已成功导入{result}条"));
                }
                catch (Exception ex)
                {
                    this.Invoke(() => MessageBox.Show(this, "导入数据失败，" + (ex.Message.Length > 500 ? ex.Message.Substring(0, 500) : ex.Message)));
                }
                finally
                {
                    this.Invoke(() =>
                    {
                        skinWaterTextBox1.Enabled = true;
                        button1.Enabled = true;
                        button2.Enabled = true;
                    });
                }
            });


        }
    }
}
