/****************************************************************************
*项目名称：WEF.ModelGenerator.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.ModelGenerator.Common
*类 名 称：DBObjectHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/7/6 14:04:17
*描述：
*=====================================================================
*修改时间：2020/7/6 14:04:17
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEF.DbDAL;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Common
{
    /// <summary>
    /// 数据库类别处理类
    /// </summary>
    public static class DBObjectHelper
    {
        /// <summary>
        /// 获取不同的数据库操作对象
        /// </summary>
        /// <param name="cnn"></param>
        /// <returns></returns>
        public static IDbObject GetDBObject(Connection cnn)
        {
            IDbObject dbObject = null;

            if (cnn.DbType.Equals(DatabaseType.SqlServer.ToString()))
            {
                dbObject = new WEF.DbDAL.SQL2000.DbObject(cnn.ConnectionString);
            }
            else if (cnn.DbType.Equals(DatabaseType.SqlServer9.ToString()))
            {
                dbObject = new WEF.DbDAL.SQL2005.DbObject(cnn.ConnectionString);
            }
            else if (cnn.DbType.Equals(DatabaseType.MsAccess.ToString()))
            {
                dbObject = new WEF.DbDAL.OleDb.DbObject(cnn.ConnectionString);
            }
            else if (cnn.DbType.Equals(DatabaseType.Oracle.ToString()))
            {
                dbObject = new WEF.DbDAL.Oracle.DbObject(cnn.ConnectionString);
            }
            else if (cnn.DbType.Equals(DatabaseType.Sqlite3.ToString()))
            {
                dbObject = new WEF.DbDAL.SQLite.DbObject(cnn.ConnectionString);
            }
            else if (cnn.DbType.Equals(DatabaseType.MySql.ToString()))
            {
                dbObject = new WEF.DbDAL.MySql.DbObject(cnn.ConnectionString);
            }
            else if (cnn.DbType.Equals(DatabaseType.MariaDB.ToString()))
            {
                dbObject = new WEF.DbDAL.MySql.DbObject(cnn.ConnectionString);
            }
            else if (cnn.DbType.Equals(DatabaseType.PostgreSQL.ToString()))
            {
                dbObject = new WEF.DbDAL.PostgreSQL.DbObject(cnn.ConnectionString);
            }
            else
            {
                throw new Exception($"暂不支持的数据类型[{cnn.DbType}]");
            }
            return dbObject;
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="dataBaseName"></param>
        /// <returns></returns>
        public static string GetCnnString(Connection cnn, string dataBaseName = "")
        {
            var cnnStr = cnn.ConnectionString;

            if (cnn.DbType.Equals(DatabaseType.SqlServer9.ToString()) || cnn.DbType.Equals(DatabaseType.SqlServer.ToString()))
            {
                if (!string.IsNullOrEmpty(dataBaseName) && cnnStr.IndexOf("Initial Catalog=master;") > -1)
                {
                    cnnStr = cnnStr.Replace("Initial Catalog=master;", $"Initial Catalog={dataBaseName};");
                }
            }
            else if (cnn.DbType.Equals(DatabaseType.MySql.ToString()))
            {
                if (!string.IsNullOrEmpty(dataBaseName) && cnnStr.IndexOf("database=;") > -1)
                {
                    cnnStr = cnnStr.Replace("database=;", $"database={dataBaseName};");
                }
            }
            else if (cnn.DbType.Equals(DatabaseType.MariaDB.ToString()))
            {
                if (!string.IsNullOrEmpty(dataBaseName) && cnnStr.IndexOf("database=;") > -1)
                {
                    cnnStr = cnnStr.Replace("database=;", $"database={dataBaseName};");
                }
            }
            else if (cnn.DbType.Equals(DatabaseType.PostgreSQL.ToString()))
            {
                if (!string.IsNullOrEmpty(dataBaseName) && cnnStr.IndexOf("Database=;") > -1)
                {
                    cnnStr = cnnStr.Replace("Database=;", $"Database={dataBaseName};");
                }
            }
            else
            {
                throw new Exception($"暂不支持的数据类型[{cnn.DbType}]");
            }

            return cnnStr;
        }
    }
}
