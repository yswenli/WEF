using System;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.DbSelect
{
    public partial class DBMsAccess : CCWin.Skin_Mac
    {
        public DBMsAccess()
        {
            InitializeComponent();
        }


        ConnectionModel _connModel = new ConnectionModel();

        public DBMsAccess(ConnectionModel cm) : this()
        {
            _connModel = cm;
        }


        /// <summary>
        /// 文件选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            fileDialog.FileName = string.Empty;
            fileDialog.Filter = "Access 文件|*.mdb;*.accdb;";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtfilepath.Text = fileDialog.FileName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbdatabaseselect_CheckedChanged(object sender, EventArgs e)
        {
            if (rbdatabaseselect.Checked)
            {
                panelDB.Enabled = true;
                txtConnectionString.Enabled = false;
            }
            else
            {
                panelDB.Enabled = false;
                txtConnectionString.Enabled = true;
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



        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (rbconnstring.Checked)
            {
                if (string.IsNullOrEmpty(txtConnectionString.Text))
                {
                    MessageBox.Show("请填写连接字符串!");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtfilepath.Text))
                {
                    MessageBox.Show("请选择Access文件!");
                    return;
                }

            }


            if (rbdatabaseselect.Checked)
            {

                StringBuilder cstring = new StringBuilder();

                string exstension = txtfilepath.Text.Substring(txtfilepath.Text.LastIndexOf('.') + 1);
                if (exstension.ToLower().Equals("mdb"))
                {
                    cstring.Append("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=");
                }
                else if (exstension.ToLower().Equals("accdb"))
                {
                    cstring.Append("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=");
                }

                cstring.Append(txtfilepath.Text);
                cstring.Append(";");
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    cstring.Append("User Id=");
                    cstring.Append(txtUserName.Text);
                    cstring.Append(";Password=");
                    cstring.Append(txtPassword.Text);
                    cstring.Append(";");
                }

                _connModel.ConnectionString = cstring.ToString();
                _connModel.Name = skinWaterTextBox1.Text;
                _connModel.Database = _connModel.Name.Substring(_connModel.Name.LastIndexOf('\\') + 1);
            }
            else
            {
                _connModel.ConnectionString = txtConnectionString.Text;


                string constring = txtConnectionString.Text;
                string templeftstring = string.Empty;
                string temprightstring = string.Empty;
                if (constring.IndexOf('/') > 0)
                {
                    templeftstring = constring.Substring(0, constring.LastIndexOf('/'));
                    temprightstring = constring.Substring(constring.LastIndexOf('/') + 1);
                }
                else if (constring.IndexOf('\\') > 0)
                {
                    templeftstring = constring.Substring(0, constring.LastIndexOf('\\'));
                    temprightstring = constring.Substring(constring.LastIndexOf('\\') + 1);
                }
                else
                {
                    MessageBox.Show("连接字符串格式不正确!");
                    return;
                }

                if (temprightstring.IndexOf(';') > 0)
                {
                    temprightstring = temprightstring.Substring(0, temprightstring.IndexOf(';'));
                }

                templeftstring = templeftstring.Substring(templeftstring.IndexOf('=') + 1);


                _connModel.Name = templeftstring + temprightstring;
                _connModel.Database = temprightstring;
            }

            try
            {

                using (OleDbConnection oledbConn = new OleDbConnection(_connModel.ConnectionString))
                {
                    oledbConn.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败!\n\r" + ex.Message);
                return;

            }

            _connModel.DbType = DatabaseType.MsAccess.ToString();
            if (_connModel.ID == Guid.Empty)
            {
                _connModel.ID = Guid.NewGuid();
            }

            UtilsHelper.UpdateConnection(_connModel);

            this.DialogResult = DialogResult.OK;

            this.Close();
        }
    }
}
