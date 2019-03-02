using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using WEF;
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
                if (string.IsNullOrEmpty(cbbServer.Text))
                {
                    MessageBox.Show("请填写服务!");
                    return;
                }

                if (string.IsNullOrEmpty(txtUserName.Text))
                {
                    MessageBox.Show("请填写用户名!");
                    return;
                }

                dbObject = new WEF.DbDAL.Oracle.DbObject(false, cbbServer.Text, txtUserName.Text, txtPassword.Text);
            }

            try
            {


                using (OracleConnection connect = new OracleConnection(dbObject.DbConnectStr))
                {
                    connect.Open();
                }


                MessageBox.Show("连接成功!");
                isConnection = true;
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


            Connection connectionModel = new Connection();
            connectionModel.Database = cbbServer.Text;
            connectionModel.ID = Guid.NewGuid();
            connectionModel.Name = cbbServer.Text+"(Oracle)";
            connectionModel.ConnectionString = dbObject.DbConnectStr;
            connectionModel.DbType = DatabaseType.Oracle.ToString();
            UtilsHelper.AddConnection(connectionModel);

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
