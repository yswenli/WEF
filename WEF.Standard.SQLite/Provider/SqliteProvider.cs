/*
* 描述： 详细描述类能干什么
* 创建人：wenli
* 创建时间：2016-02-25 11:32:11
*/
/*
*修改人：wenli
*修改时间：2016-02-25 11:32:11
*修改内容：xxxxxxx
*/

using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

using WEF.Common;

namespace WEF.Provider
{
    /// <summary>
    /// sqlite
    /// </summary>
    public class SqliteProvider : DbProvider
    {

        public SqliteProvider(string connectionString)
            : base(connectionString, System.Data.SQLite.SQLiteFactory.Instance, '[', ']', '@')
        {
            this.DatabaseType = DatabaseType.Sqlite3;
        }

        public override string RowAutoID
        {
            get { return "select last_insert_rowid();"; }
        }

        public override bool SupportBatch
        {
            get { return true; }
        }

        public override void PrepareCommand(DbCommand cmd)
        {
            base.PrepareCommand(cmd);

            foreach (DbParameter p in cmd.Parameters)
            {

                if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue)
                {
                    continue;
                }

                object value = p.Value;
                if (value == DBNull.Value)
                {
                    continue;
                }
                Type type = value.GetType();
                SQLiteParameter sqliteParam = (SQLiteParameter)p;

                if (type == typeof(Guid))
                {
                    sqliteParam.DbType = DbType.String;
                    sqliteParam.Size = 32;
                    continue;
                }

                if ((p.DbType == DbType.Time || p.DbType == DbType.DateTime) && type == typeof(TimeSpan))
                {
                    sqliteParam.DbType = DbType.Double;
                    sqliteParam.Value = ((TimeSpan)value).TotalDays;
                    continue;
                }

                switch (p.DbType)
                {
                    case DbType.Time:
                        sqliteParam.DbType = DbType.DateTime;
                        p.Value = value.ToString();
                        break;
                    case DbType.Object:
                        sqliteParam.DbType = DbType.String;
                        p.Value = SerializationManager.Serialize(value);
                        break;
                }
            }

            cmd.CommandText = cmd.CommandText
                .Replace("substring(", "substr(")
                .Replace("len(", "length(")
                .Replace("getdate()", "datetime('now')")
                .Replace("isnull(", "ifnull(");
        }


        /// <summary>
        /// 创建分页查询
        /// </summary>
        /// <param name="fromSection"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public override Search CreatePageFromSection(Search fromSection, int startIndex, int endIndex)
        {
            Check.Require(startIndex, "startIndex", Check.GreaterThanOrEqual<int>(1));
            Check.Require(endIndex, "endIndex", Check.GreaterThanOrEqual<int>(1));
            Check.Require(startIndex <= endIndex, "startIndex must be less than endIndex!");
            Check.Require(fromSection, "fromSection", Check.NotNullOrEmpty);
            //Check.Require(fromSection.OrderByClip, "query.OrderByClip", Check.NotNullOrEmpty);

            fromSection.LimitString = string.Concat(" limit ", (startIndex - 1).ToString(), ",", (endIndex - startIndex + 1).ToString());


            return fromSection;
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override bool IsTableExist(string tableName)
        {
            using (var cnn = new SQLiteConnection(ConnectionString))
            {
                using (var cmd = cnn.CreateCommand())
                {
                    cmd.CommandText = $".tables {tableName};";
                    cnn.Open();
                    var data = cmd.ExecuteScalar()?.ToString() ?? "";
                    return (!string.IsNullOrEmpty(data) && data == tableName);
                }
            }
        }
    }
}
