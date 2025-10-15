using System;
using System.Windows.Forms;
namespace WEF.Standard.DevelopTools
{
    public partial class DatabaseSelect : CCWin.Skin_Mac
    {
        public DatabaseSelect()
        {
            InitializeComponent();
        }


        public static DatabaseType? databaseType = null;


        /// <summary>
        /// 选择数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.OK;
            if (rbSqlServer.Checked)
            {
                databaseType = DatabaseType.SqlServer9;
            }
            else if (rbOledb.Checked)
            {
                databaseType = DatabaseType.MsAccess;
            }
            else if (rbOracle.Checked)
            {
                databaseType = DatabaseType.Oracle;
            }
            else if (rbSQLite.Checked)
            {
                databaseType = DatabaseType.Sqlite3;
            }
            else if (rbMySql.Checked)
            {
                databaseType = DatabaseType.MySql;
            }
            else if (rbMariaDB.Checked)
            {
                databaseType = DatabaseType.MySql;
            }
            else if (rbMongoDB.Checked)
            {
                databaseType = DatabaseType.MongoDB;
            }
            else if (rbPostgreSql.Checked)
            {
                databaseType = DatabaseType.PostgreSQL;
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
