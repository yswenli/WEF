/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using WEF.Common;

namespace WEF.Provider
{

    /// <summary>
    /// SqlServer 
    /// </summary>
    public class SqlServerProvider : DbProvider
    {
        public SqlServerProvider(string connectionString)
            : this(connectionString, SqlClientFactory.Instance)
        {
        }

        public SqlServerProvider(string connectionString, DbProviderFactory dbFactory)
            : base(connectionString, dbFactory, '[', ']', '@')
        {
        }

        public override string RowAutoID
        {
            get
            {
                return "select scope_identity()";
            }
        }

        public override bool SupportBatch
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Builds the name of the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public override string BuildParameterName(string name)
        {
            string nameStr = name.Trim(leftToken, rightToken);
            if (nameStr[0] != paramPrefixToken)
            {
                return nameStr.Insert(0, new string(paramPrefixToken, 1));
            }
            //剔除参数中的“.” 2016-04-08 added
            return nameStr.Replace(".", "_");
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
                SqlParameter sqlParam = (SqlParameter)p;

                if (type == typeof(Guid))
                {
                    sqlParam.SqlDbType = SqlDbType.UniqueIdentifier;
                    continue;
                }

                switch (p.DbType)
                {
                    case DbType.Binary:
                        if (((byte[])value).Length > 8000)
                        {
                            sqlParam.SqlDbType = SqlDbType.Image;
                        }
                        break;
                    case DbType.Time:
                        sqlParam.SqlDbType = SqlDbType.DateTime;
                        break;
                    case DbType.DateTime:
                        sqlParam.SqlDbType = SqlDbType.DateTime;
                        break;
                    case DbType.AnsiString:
                        if (value.ToString().Length > 8000)
                        {
                            sqlParam.SqlDbType = SqlDbType.Text;
                        }
                        break;
                    case DbType.String:
                        if (value.ToString().Length > 4000)
                        {
                            sqlParam.SqlDbType = SqlDbType.NText;
                        }
                        break;
                    case DbType.Object:
                        sqlParam.SqlDbType = SqlDbType.NText;
                        p.Value = SerializationManager.Serialize(value);
                        break;
                }

                if (sqlParam.SqlDbType == SqlDbType.DateTime && type == typeof(TimeSpan))
                {
                    sqlParam.Value = new DateTime(1900, 1, 1).Add((TimeSpan)value);
                    continue;
                }
            }

            ////replace TONUMBER to cast
            //int pos = cmd.CommandText.IndexOf("TO_NUMBER(");
            //if (pos > 0)
            //{
            //    int bracketCount = 1;
            //    string columnName = null;
            //    int i = pos + "TO_NUMBER(".Length;
            //    for (; i < cmd.CommandText.Length; ++i)
            //    {
            //        if (cmd.CommandText[i] == ')')
            //            --bracketCount;
            //        else if (cmd.CommandText[i] == '(')
            //            ++bracketCount;

            //        if (bracketCount == 0)
            //            break;
            //    }
            //    columnName = cmd.CommandText.Substring(pos + "TO_NUMBER(".Length, i - pos - "TO_NUMBER(".Length);
            //    string newSql = cmd.CommandText.Substring(0, pos) + string.Format("cast(cast({0} as money) as float)", columnName);
            //    if (i < cmd.CommandText.Length - 1)
            //        newSql += cmd.CommandText.Substring(i + 1);
            //    cmd.CommandText = newSql;
            //}




        }
    }
}
