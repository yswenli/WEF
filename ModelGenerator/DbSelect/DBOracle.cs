using System;
using System.Data.OracleClient;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.DbSelect
{
    public partial class DBOracle : CCWin.Skin_Mac
    {
        public DBOracle()
        {
            InitializeComponent();
        }

        WEF.DbDAL.IDbObject dbObject;

        ConnectionModel _connectionModel = new ConnectionModel();

        public DBOracle(ConnectionModel cm) : this()
        {
            _connectionModel = cm;
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (chbConnectString.Checked)
            {
                if (string.IsNullOrEmpty(txtConnectString.Text))
                {
                    MessageBox.Show("请填写连接字符串!");
                    return;
                }

                dbObject = new WEF.DbDAL.Oracle.DbObject(txtConnectString.Text);
            }
            else
            {
                if (string.IsNullOrEmpty(cbbService.Text))
                {
                    MessageBox.Show("请填写服务!");
                    return;
                }

                if (string.IsNullOrEmpty(txtUserName.Text))
                {
                    MessageBox.Show("请填写用户名!");
                    return;
                }

                dbObject = new WEF.DbDAL.Oracle.DbObject(false, cbbServer.Text, cbbService.Text, txtUserName.Text, txtPassword.Text);
            }

            try
            {


                using (OracleConnection connect = new OracleConnection(dbObject.DbConnectStr))
                {
                    connect.Open();
                }


                MessageBox.Show("连接成功!");

                isConnection = true;

                button2.Enabled = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show("连接失败!\n\r" + ex.Message);
                isConnection = false;

            }

        }

        bool isConnection = false;


        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            button1_Click(null, null);

            if (!isConnection)
            {
                return;
            }

            var serviceName = cbbService.Text;

            if (string.IsNullOrEmpty(serviceName))
            {
                var index1 = dbObject.DbConnectStr.IndexOf("SERVICE_NAME", StringComparison.OrdinalIgnoreCase);
                var str1 = dbObject.DbConnectStr.Substring(0, index1);
                var str2 = dbObject.DbConnectStr.Substring(index1);
                var index2 = str2.IndexOf("=");
                var str3 = str2.Substring(index2 + 1);
                var index3 = str3.IndexOf(")");
                var str4 = str3.Substring(0, index3);
                serviceName = str4;
            }

            _connectionModel.Database = serviceName;
            if (_connectionModel.ID == Guid.Empty)
                _connectionModel.ID = Guid.NewGuid();
            _connectionModel.Name = skinWaterTextBox1.Text;
            if (string.IsNullOrEmpty(_connectionModel.Name))
                _connectionModel.Name = serviceName + "(Oracle)";
            _connectionModel.ConnectionString = dbObject.DbConnectStr;
            _connectionModel.DbType = DatabaseType.Oracle.ToString();
            UtilsHelper.UpdateConnection(_connectionModel);

            this.DialogResult = DialogResult.OK;

            this.Close();
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


        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbConnectString_CheckedChanged(object sender, EventArgs e)
        {
            if (chbConnectString.Checked)
            {
                panelServer.Enabled = false;
                txtConnectString.Enabled = true;
            }
            else
            {
                panelServer.Enabled = true;
                txtConnectString.Enabled = false;
            }

        }
    }
}
