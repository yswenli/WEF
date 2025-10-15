using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WEF.Standard.DevelopTools.Forms
{
    public partial class SQLFileForm : Form
    {
        long count = 0L;
        long lineCount = 0L;
        bool stop = false;

        public SQLFileForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var connStr = textBox1.Text;
            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show(this, "请输入数据库连接字符串");
                return;
            }

            var sqlFilePath = textBox2.Text;
            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show(this, "请输入sql文件地址");
                return;
            }

            textBox3.Text = "";

            foreach (Control item in this.Controls)
            {
                item.Enabled = false;
            }


            Handle(connStr, sqlFilePath);


        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="sqlFilePath"></param>
        /// <returns></returns>
        Task Handle(string connStr, string sqlFilePath)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    count = 0L;
                    lineCount = 0L;
                    stop = false;

                    using (var fs = File.Open(sqlFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (var sr = new StreamReader(fs))
                        {
                            LogInfo("正在读取文件");

                            Task.Factory.StartNew(() =>
                            {
                                while (!stop)
                                {
                                    Notice($"正在处理到第 {lineCount} 行，共执行sql {count} 次");
                                    Thread.Sleep(1000);
                                }
                                LogInfo($"已处理到第 {lineCount} 行，共执行sql {count} 次");
                            }, TaskCreationOptions.LongRunning);


                            var sql = new StringBuilder();
                            var line = "";
                            while (line != null)
                            {
                                line = sr.ReadLine();
                                lineCount++;
                                if (line == null)
                                {
                                    stop = true;
                                    LogInfo("任务已执行完毕");
                                    break;
                                }
                                else
                                {
                                    if (line.StartsWith("GO", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        try
                                        {
                                            using (var cnn = new SqlConnection(connStr))
                                            {
                                                cnn.Open();
                                                using (var cmd = cnn.CreateCommand())
                                                {
                                                    cmd.CommandText = sql.ToString();
                                                    cmd.CommandTimeout = 3600;
                                                    cmd.ExecuteNonQuery();
                                                    count++;
                                                }
                                                cnn.Close();
                                            }
                                        }
                                        catch { }
                                        sql.Clear();
                                    }
                                    else
                                    {
                                        sql.AppendLine(line);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogInfo($"读取文件出错：{ex.Message}");
                }
                finally
                {
                    this.Invoke(new Action(() =>
                    {
                        foreach (Control item in this.Controls)
                        {
                            item.Enabled = true;
                        }
                    }));
                }
            });


        }

        void LogInfo(string text)
        {
            if (this.InvokeRequired)
            {
                textBox3.Invoke(new Action(() =>
                {
                    if (textBox3.Text.Length > 30000)
                    {

                    }
                    textBox3.Text += text + Environment.NewLine;
                }));
            }
            else
            {
                textBox3.Text += text + Environment.NewLine;
            }
        }

        void Notice(string text)
        {
            if (this.InvokeRequired)
            {
                label3.Invoke(new Action(() =>
                {
                    label3.Text = text;
                }));
            }
            else
            {
                label3.Text = text;
            }
        }
    }
}
