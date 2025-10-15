using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

using CCWin;

using WEF.Standard.DevelopTools.Common;
using WEF.Standard.DevelopTools.Model;

namespace WEF.Standard.DevelopTools.DbSelect
{
    public partial class DBMySql : Skin_Mac
    {
        ConnectionModel _connectionModel;
        public DBMySql()
        {
            InitializeComponent();
            _connectionModel = new ConnectionModel()
            {
                ID = Guid.NewGuid()
            };
        }


        public DBMySql(ConnectionModel cm)
        {
            InitializeComponent();

            _connectionModel = cm;           

            var ci = ConnectionInfo.GetConnectionInfo(cm);

            skinWaterTextBox2.Text = cm.Name;
            cbbServer.Text = ci.Server;
            txtport.Text = ci.Port.ToString();
            cbbDatabase.Text = ci.DataBase;
            txtUserName.Text = ci.UserName;
            txtPassword.Text = ci.Password;
        }


        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (string.IsNullOrEmpty(skinWaterTextBox1.Text))
                {
                    MessageBox.Show("连接字符串不能为空!");
                    return;
                }

                LoadForm.ShowLoadingAsync(this);

                WEF.DbDAL.IDbObject dbObejct = new WEF.DbDAL.MySql.DbObject(skinWaterTextBox1.Text);

                Task.Run(() =>
                {
                    try
                    {
                        DataTable dbNameTable = dbObejct.GetDBList();

                        LoadForm.HideLoadingAsync(this);

                        InvokeHelper.Invoke(this, () =>
                        {
                            cbbDatabase.Items.Clear();
                            cbbDatabase.Items.Add("全部");
                            foreach (DataRow dr in dbNameTable.Rows)
                            {
                                cbbDatabase.Items.Add(dr[0].ToString());
                            }
                            cbbDatabase.Enabled = true;
                            cbbDatabase.SelectedIndex = 0;
                            MessageBox.Show("连接成功!");
                            button2.Enabled = true;
                        });
                    }
                    catch (Exception ex)
                    {
                        LoadForm.HideLoadingAsync(this);
                        InvokeHelper.Invoke(this, () =>
                        {
                            MessageBox.Show("连接失败!\n\r" + ex.Message);
                            cbbDatabase.Enabled = false;
                        });
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

                if (string.IsNullOrEmpty(txtUserName.Text))
                {
                    MessageBox.Show("登陆名不能为空!");
                    return;
                }

                LoadForm.ShowLoadingAsync(this);

                WEF.DbDAL.IDbObject dbObejct = new WEF.DbDAL.MySql.DbObject(false, cbbServer.Text, txtUserName.Text, txtPassword.Text, txtport.Text);

                Task.Run(() =>
                {
                    try
                    {
                        DataTable DBNameTable = dbObejct.GetDBList();
                        LoadForm.HideLoadingAsync(this);

                        InvokeHelper.Invoke(this, () =>
                        {
                            cbbDatabase.Items.Clear();
                            cbbDatabase.Items.Add("全部");
                            foreach (DataRow dr in DBNameTable.Rows)
                            {
                                cbbDatabase.Items.Add(dr[0].ToString());
                            }

                            cbbDatabase.Enabled = true;
                            cbbDatabase.SelectedIndex = 0;
                            MessageBox.Show("连接成功!");
                            button2.Enabled = true;
                        });

                    }
                    catch (Exception ex)
                    {
                        LoadForm.HideLoadingAsync(this);
                        InvokeHelper.Invoke(this, () =>
                        {
                            MessageBox.Show("连接失败!\n\r" + ex.Message);
                            cbbDatabase.Enabled = false;
                        });
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
            if (checkBox1.Checked)
            {
                if (string.IsNullOrEmpty(skinWaterTextBox1.Text))
                {
                    MessageBox.Show("连接字符串不能为空!");
                    return;
                }

                Dictionary<string, string> keyValuePairs = skinWaterTextBox1.Text.ToConnectParmaDic();

                if (!keyValuePairs.ContainsKey("database"))
                {
                    MessageBox.Show("连接字符串中不包含database!\n\r");
                    return;
                }

                LoadForm.ShowLoadingAsync(this);

                WEF.DbDAL.MySql.DbObject dbObejct = new WEF.DbDAL.MySql.DbObject(skinWaterTextBox1.Text);

                try
                {
                    _ = dbObejct.GetDBList();
                }
                catch (Exception ex)
                {
                    LoadForm.HideLoadingAsync(this);
                    MessageBox.Show("连接失败!\n\r" + ex.Message);
                    return;
                }

                _connectionModel.Database = string.IsNullOrEmpty(keyValuePairs["database"]) ? "all" : keyValuePairs["database"];
                _connectionModel.Name = skinWaterTextBox2.Text;
                if (string.IsNullOrEmpty(_connectionModel.Name))
                    _connectionModel.Name = keyValuePairs["server"] + "(MySql)[" + _connectionModel.Database + "]";
                _connectionModel.ConnectionString = dbObejct.DbConnectStr;
                _connectionModel.DbType = DatabaseType.MySql.ToString();

                UtilsHelper.UpdateConnection(_connectionModel);

                this.DialogResult = DialogResult.OK;

                this.Close();

                LoadForm.HideLoadingAsync(this);
            }
            else
            {
                if (string.IsNullOrEmpty(cbbServer.Text))
                {
                    MessageBox.Show("服务器不能为空!");
                    return;
                }

                if (string.IsNullOrEmpty(txtUserName.Text))
                {
                    MessageBox.Show("登陆名不能为空!");
                    return;
                }

                LoadForm.ShowLoadingAsync(this);

                WEF.DbDAL.MySql.DbObject dbObejct;

                var dataBase = cbbDatabase.Items[cbbDatabase.SelectedIndex].ToString();

                if (!string.IsNullOrEmpty(dataBase) && dataBase != "全部")
                {
                    dbObejct = new WEF.DbDAL.MySql.DbObject(false, cbbServer.Text, txtUserName.Text, txtPassword.Text, txtport.Text, dataBase);
                }
                else
                {
                    dbObejct = new WEF.DbDAL.MySql.DbObject(false, cbbServer.Text, txtUserName.Text, txtPassword.Text, txtport.Text);
                }

                string tempconnectionstring = dbObejct.DbConnectStr;

                try
                {
                    _ = dbObejct.GetDBList();
                }
                catch (Exception ex)
                {
                    LoadForm.HideLoadingAsync(this);
                    MessageBox.Show("连接失败!\n\r" + ex.Message);
                    return;

                }


                _connectionModel.Database = cbbDatabase.SelectedIndex == 0 ? "all" : cbbDatabase.Text;
                _connectionModel.Name = skinWaterTextBox2.Text;
                if (string.IsNullOrEmpty(_connectionModel.Name))
                    _connectionModel.Name = cbbServer.Text + "(MySql)[" + _connectionModel.Database + "]";
                _connectionModel.ConnectionString = tempconnectionstring;
                _connectionModel.DbType = DatabaseType.MySql.ToString();

                UtilsHelper.UpdateConnection(_connectionModel);


                this.DialogResult = DialogResult.OK;

                this.Close();

                LoadForm.HideLoadingAsync(this);
            }

        }


        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                panel1.Enabled = false;
                panel2.Enabled = true;
            }
            else
            {
                panel1.Enabled = true;
                panel2.Enabled = false;
            }
        }
    }
}
