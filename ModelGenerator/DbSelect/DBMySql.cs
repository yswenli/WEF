using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;
using WEF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WEF.ModelGenerator.DbSelect
{
    public partial class DBMySql : CCWin.Skin_Mac
    {
        public DBMySql()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 测试连接
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

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                MessageBox.Show("登陆名不能为空!");
                return;
            }

            try
            {
                WEF.DbDAL.IDbObject dbObejct = new WEF.DbDAL.MySql.DbObject(false, cbbServer.Text, txtUserName.Text, txtPassword.Text, txtport.Text);
                DataTable DBNameTable = dbObejct.GetDBList();
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

            }
            catch (Exception ex)
            {

                MessageBox.Show("连接失败!\n\r" + ex.Message);
                cbbDatabase.Enabled = false;

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

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                MessageBox.Show("登陆名不能为空!");
                return;
            }
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
                dbObejct.OpenDB();
            }
            catch (Exception ex)
            {

                MessageBox.Show("连接失败!\n\r" + ex.Message);
                return;

            }

            Connection connectionModel = new Connection();
            connectionModel.Database = cbbDatabase.SelectedIndex == 0 ? "all" : cbbDatabase.Text;
            connectionModel.ID = Guid.NewGuid();
            connectionModel.Name = cbbServer.Text + "(MySql)[" + connectionModel.Database + "]";
            connectionModel.ConnectionString = tempconnectionstring;
            connectionModel.DbType = DatabaseType.MySql.ToString();



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
    }
}
