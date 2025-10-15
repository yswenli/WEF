using System;
using System.Data.SQLite;
using System.Text;
using System.Windows.Forms;

using WEF.Standard.DevelopTools.Common;
using WEF.Standard.DevelopTools.Model;

namespace WEF.Standard.DevelopTools.DbSelect
{
    public partial class DbSqlite : CCWin.Skin_Mac
    {

        Model.ConnectionModel _connModel;
        public DbSqlite()
        {
            InitializeComponent();
            _connModel = new ConnectionModel()
            {
                ID = Guid.NewGuid()
            };
        }


        public DbSqlite(ConnectionModel cm) : this()
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
            fileDialog.Filter = "Sqlite 文件|*.*";

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
                    MessageBox.Show("请选择Sqlite数据库文件!");
                    return;
                }

            }

            if (_connModel.ID == Guid.Empty)
            {
                _connModel.ID = Guid.NewGuid();
            }

            if (rbdatabaseselect.Checked)
            {

                StringBuilder cstring = new StringBuilder("Data Source=");
                cstring.Append(txtfilepath.Text);
                cstring.Append(";");
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    cstring.Append("Password=");
                    cstring.Append(txtPassword.Text);
                    cstring.Append(";");
                }

                _connModel.ConnectionString = cstring.ToString();
                _connModel.Name = skinWaterTextBox1.Text;
                if (string.IsNullOrEmpty(_connModel.Name))
                    _connModel.Name = txtfilepath.Text;
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

                using (SQLiteConnection oledbConn = new SQLiteConnection(_connModel.ConnectionString))
                {
                    oledbConn.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接失败!\n\r" + ex.Message);
                return;

            }

            _connModel.DbType = DatabaseType.Sqlite3.ToString();

            UtilsHelper.UpdateConnection(_connModel);

            this.DialogResult = DialogResult.OK;

            this.Close();
        }
    }
}
