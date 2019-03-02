using System;
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
        public DBSqlServer()
        {
            InitializeComponent();
        }

        private void DBSqlServer_Load(object sender, EventArgs e)
        {
            cbbServerType.SelectedIndex = 2;
            cbbShenFenRZ.SelectedIndex = 0;
            cbbDatabase.SelectedIndex = 0;
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

            WEF.DbDAL.IDbObject dbObejct = new WEF.DbDAL.SQL2000.DbObject(cbbShenFenRZ.SelectedIndex == 0, cbbServer.Text, txtUserName.Text, txtPassword.Text);

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
                        LoadForm.HideLoading();
                        MessageBox.Show("连接成功!");
                    }));
                }
                catch (Exception ex)
                {
                    LoadForm.HideLoading();
                    MessageBox.Show("连接失败!\n\r" + ex.Message);
                }
            });

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
                    LoadForm.HideLoading();

                    this.Invoke(new Action(() =>
                    {
                        Connection connectionModel = new Connection();
                        connectionModel.Database = cbbDatabase.SelectedIndex == 0 ? "all" : cbbDatabase.Text;
                        connectionModel.ID = Guid.NewGuid();
                        connectionModel.Name = cbbServer.Text + "(" + cbbServerType.Text + ")[" + connectionModel.Database + "]";
                        if (cbbServerType.SelectedIndex == 0)
                        {
                            connectionModel.DbType = DatabaseType.SqlServer.ToString();
                        }
                        else
                        {
                            connectionModel.DbType = DatabaseType.SqlServer9.ToString();
                        }
                        connectionModel.ConnectionString = tempconnectionstring;

                        UtilsHelper.AddConnection(connectionModel);

                        this.DialogResult = DialogResult.OK;

                        this.Close();
                    }));
                }
                catch (Exception ex)
                {
                    LoadForm.HideLoading();
                    MessageBox.Show("连接失败!\n\r" + ex.Message);
                }
            });






        }
    }
}
