using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.DbSelect
{
    public partial class DBSqlServer : CCWin.Skin_Mac
    {

        ConnectionModel _connectionModel;


        /// <summary>
        /// 新建
        /// </summary>
        public DBSqlServer()
        {
            InitializeComponent();

            cbbServerType.SelectedIndex = 0;
            cbbShenFenRZ.SelectedIndex = 0;
            cbbDatabase.SelectedIndex = 0;

            _connectionModel = new ConnectionModel()
            {
                ID = Guid.NewGuid()                 
            };

        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="cm"></param>
        public DBSqlServer(ConnectionModel cm)
        {
            InitializeComponent();

            _connectionModel = cm;

            if (_connectionModel.ID == Guid.Empty)
            {
                _connectionModel.ID = Guid.NewGuid();
            }

            cbbServerType.SelectedIndex = 0;

            var ci = ConnectionInfo.GetConnectionInfo(cm);
            skinWaterTextBox1.Text = cm.Name;
            cbbServer.Text = ci.Server;
            cbbDatabase.Text = ci.DataBase;
            txtUserName.Text = ci.UserName;
            txtPassword.Text = ci.Password;
            if (string.IsNullOrEmpty(ci.Password))
            {
                cbbShenFenRZ.SelectedIndex = 0;
            }
            else
            {
                cbbShenFenRZ.SelectedIndex = 1;
            }
            cbbDatabase.Text = ci.DataBase;
        }


        /// <summary>
        /// 取消 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        private string createConnectionString(string database)
        {
            StringBuilder connstring = new StringBuilder();
            connstring.Append("Data Source=");
            connstring.Append(cbbServer.Text);
            connstring.Append(";Initial Catalog=");
            connstring.Append(database);
            connstring.Append(";");
            if (cbbShenFenRZ.SelectedIndex == 0)
            {
                connstring.Append("Integrated Security=True");
            }
            else
            {
                connstring.Append("User Id=");
                connstring.Append(txtUserName.Text);
                connstring.Append(";Password=");
                connstring.Append(txtPassword.Text);
            }

            return connstring.ToString();
        }

        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbbShenFenRZ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbShenFenRZ.SelectedIndex == 0)
            {
                txtPassword.Enabled = false;
                txtUserName.Enabled = false;
            }
            else
            {
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
            }
        }


        /// <summary>
        /// 连接  测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("连接字符串不能为空!");
                    return;
                }

                LoadForm.ShowLoading(this);

                WEF.DbDAL.IDbObject dbObejct = null;

                try
                {
                    dbObejct = new WEF.DbDAL.SQL2000.DbObject(textBox1.Text);
                }
                catch (Exception ex)
                {
                    LoadForm.HideLoading(this);
                    MessageBox.Show($"连接测试失败：{ex.Message}");
                }

                if (dbObejct == null) return;

                cbbDatabase.Enabled = false;

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var DBNameTable = dbObejct.GetDBList();

                        this.Invoke(new Action(() =>
                        {
                            cbbDatabase.Items.Clear();
                            cbbDatabase.Items.Add("全部");
                            foreach (DataRow dr in DBNameTable.Rows)
                            {
                                cbbDatabase.Items.Add(dr[0].ToString());
                            }

                            cbbDatabase.Enabled = true;
                            cbbDatabase.SelectedIndex = 0;
                            LoadForm.HideLoading(this);
                            MessageBox.Show("连接成功!");

                            button2.Enabled = true;
                        }));
                    }
                    catch (Exception ex)
                    {
                        LoadForm.HideLoading(this);
                        MessageBox.Show("连接失败!\n\r" + ex.Message);
                    }
                });
            }
            else
            {
                if (string.IsNullOrEmpty(cbbServer.Text))
                {
                    MessageBox.Show("服务器不能为空!");
                    return;
                }

                if (cbbShenFenRZ.SelectedIndex == 1 && string.IsNullOrEmpty(txtUserName.Text))
                {
                    MessageBox.Show("登陆名不能为空!");
                    return;
                }


                LoadForm.ShowLoading(this);

                WEF.DbDAL.IDbObject dbObejct;

                if (cbbServerType.SelectedIndex == 0)
                {
                    dbObejct = new WEF.DbDAL.SQLServer.DbObject(cbbShenFenRZ.SelectedIndex == 0, cbbServer.Text, txtUserName.Text, txtPassword.Text);
                }
                else
                {
                    dbObejct = new WEF.DbDAL.SQL2000.DbObject(cbbShenFenRZ.SelectedIndex == 0, cbbServer.Text, txtUserName.Text, txtPassword.Text);
                }

                cbbDatabase.Enabled = false;

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var DBNameTable = dbObejct.GetDBList();

                        this.Invoke(new Action(() =>
                        {
                            cbbDatabase.Items.Clear();
                            cbbDatabase.Items.Add("全部");
                            foreach (DataRow dr in DBNameTable.Rows)
                            {
                                cbbDatabase.Items.Add(dr[0].ToString());
                            }

                            cbbDatabase.Enabled = true;
                            cbbDatabase.SelectedIndex = 0;
                            LoadForm.HideLoading(this);
                            MessageBox.Show("连接成功!");

                            button2.Enabled = true;
                        }));
                    }
                    catch (Exception ex)
                    {
                        LoadForm.HideLoading(this);
                        MessageBox.Show("连接失败!\n\r" + ex.Message);
                    }
                });
            }
        }


        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbbServer.Text))
            {
                MessageBox.Show("服务器不能为空!");
                return;
            }

            if (cbbShenFenRZ.SelectedIndex == 1 && string.IsNullOrEmpty(txtUserName.Text))
            {
                MessageBox.Show("登陆名不能为空!");
                return;
            }

            string tempconnectionstring = createConnectionString(cbbDatabase.SelectedIndex == 0 ? "master" : cbbDatabase.Text);

            Task.Factory.StartNew(() =>
            {
                LoadForm.ShowLoading(this);

                try
                {
                    using (SqlConnection conn = new SqlConnection(tempconnectionstring))
                    {
                        conn.Open();
                    }
                    LoadForm.HideLoading(this);

                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    this.Invoke(new Action(() =>
                    {
                        if (checkBox1.Checked)
                        {
                            var cnnStr = textBox1.Text;

                            var arr1 = cnnStr.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var item in arr1)
                            {
                                var arr2 = item.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                                try
                                {
                                    dic.Add(arr2[0].ToLower(), arr2[1].ToLower());
                                }
                                catch { }
                            }

                            _connectionModel.Database = dic["initial catalog"];
                            _connectionModel.Name = skinWaterTextBox1.Text;
                            if (string.IsNullOrEmpty(_connectionModel.Name))
                                _connectionModel.Name = dic["data source"] + "(" + DatabaseType.SqlServer9.ToString() + ")[" + _connectionModel.Database + "]";
                            if (cbbServerType.SelectedIndex == 0)
                            {
                                _connectionModel.DbType = DatabaseType.SqlServer9.ToString();
                            }
                            else
                            {
                                _connectionModel.DbType = DatabaseType.SqlServer.ToString();
                            }

                        }
                        else
                        {
                            _connectionModel.Database = cbbDatabase.SelectedIndex == 0 ? "all" : cbbDatabase.Text;

                            _connectionModel.Name = skinWaterTextBox1.Text;
                            if (string.IsNullOrEmpty(_connectionModel.Name))
                                _connectionModel.Name = cbbServer.Text + "(" + cbbServerType.Text + ")[" + _connectionModel.Database + "]";
                            if (cbbServerType.SelectedIndex == 0)
                            {
                                _connectionModel.DbType = DatabaseType.SqlServer9.ToString();
                            }
                            else
                            {
                                _connectionModel.DbType = DatabaseType.SqlServer.ToString();
                            }


                        }
                        _connectionModel.ConnectionString = tempconnectionstring;
                        UtilsHelper.UpdateConnection(_connectionModel);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }));

                }
                catch (Exception ex)
                {
                    LoadForm.HideLoading(this);
                    MessageBox.Show("连接失败!\n\r" + ex.Message);
                }
            });

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                panel2.Enabled = true;
                panel1.Enabled = false;
            }
            else
            {
                panel1.Enabled = true;
                panel2.Enabled = false;
            }
        }
    }
}
