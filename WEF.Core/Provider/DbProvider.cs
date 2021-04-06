/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2019
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using WEF.Common;
using WEF.Expressions;

namespace WEF.Provider
{
    /// <summary>
    /// DbProvider
    /// </summary>
    public abstract class DbProvider
    {


        #region Protected Members
        /// <summary>
        /// like符号。 --- 2015-09-07
        /// </summary>
        protected char likeToken;
        /// <summary>
        /// 【
        /// </summary>
        protected char leftToken;
        /// <summary>
        /// 参数前缀
        /// </summary>
        protected char paramPrefixToken;

        /// <summary>
        /// 】
        /// </summary>
        protected char rightToken;

        /// <summary>
        /// The db provider factory.
        /// </summary>
        protected DbProviderFactory dbProviderFactory;
        /// <summary>
        /// The db connection string builder
        /// </summary>
        protected DbConnectionStringBuilder dbConnStrBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DbProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The conn STR.</param>
        /// <param name="dbProviderFactory">The db provider factory.</param>
        /// <param name="leftToken">leftToken</param>
        /// <param name="paramPrefixToken">paramPrefixToken</param>
        /// <param name="rightToken">rightToken</param>
        protected DbProvider(string connectionString, DbProviderFactory dbProviderFactory, char leftToken, char rightToken, char paramPrefixToken)
        {
            dbConnStrBuilder = new DbConnectionStringBuilder();
            dbConnStrBuilder.ConnectionString = RepairedMySqlConnStr(connectionString);
            this.dbProviderFactory = dbProviderFactory;
            this.leftToken = leftToken;
            this.rightToken = rightToken;
            this.paramPrefixToken = paramPrefixToken;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DbProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The conn STR.</param>
        /// <param name="dbProviderFactory">The db provider factory.</param>
        /// <param name="leftToken">leftToken</param>
        /// <param name="paramPrefixToken">paramPrefixToken</param>
        /// <param name="rightToken">rightToken</param>
        /// <param name="likeToken">likeToken</param>
        protected DbProvider(string connectionString, DbProviderFactory dbProviderFactory, char leftToken, char rightToken, char paramPrefixToken, char likeToken = '%')
        {
            dbConnStrBuilder = new DbConnectionStringBuilder();
            dbConnStrBuilder.ConnectionString = RepairedMySqlConnStr(connectionString);
            this.dbProviderFactory = dbProviderFactory;
            this.leftToken = leftToken;
            this.rightToken = rightToken;
            this.paramPrefixToken = paramPrefixToken;
            this.likeToken = likeToken;
        }

        string RepairedMySqlConnStr(string mysqlConnStr)
        {
            if (!string.IsNullOrEmpty(mysqlConnStr))
            {
                if ((dbProviderFactory != null && dbProviderFactory.GetType().Name.IndexOf("mysql", StringComparison.OrdinalIgnoreCase) > -1)
                    || mysqlConnStr.IndexOf("port=3306", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    if (mysqlConnStr.IndexOf("Convert Zero Datetime=True", StringComparison.OrdinalIgnoreCase) == -1
                   && mysqlConnStr.IndexOf("Allow Zero Datetime=True", StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        if (mysqlConnStr.LastIndexOf(";") == mysqlConnStr.Length - 1)
                            mysqlConnStr += "Convert Zero Datetime=True; Allow Zero Datetime=True";
                        else
                            mysqlConnStr += ";Convert Zero Datetime=True; Allow Zero Datetime=True;";
                    }
                }
            }
            return mysqlConnStr;
        }
        #endregion


        #region Properties
        //2015-08-12新增
        /// <summary>
        /// 
        /// </summary>
        private DatabaseType databaseType;
        //2015-08-12新增
        /// <summary>
        /// ConnectionStrings 节点名称
        /// </summary>
        public DatabaseType DatabaseType
        {
            get
            {
                return databaseType;
            }
            set
            {
                databaseType = value;
            }
        }

        private string connectionStringsName;

        /// <summary>
        /// ConnectionStrings 节点名称
        /// </summary>
        public string ConnectionStringsName
        {
            get
            {
                return connectionStringsName;
            }
            set
            {
                connectionStringsName = value;
            }
        }


        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get
            {
                return dbConnStrBuilder.ConnectionString;
            }
        }

        /// <summary>
        /// Gets the db provider factory.
        /// </summary>
        /// <value>The db provider factory.</value>
        public DbProviderFactory DbProviderFactory
        {
            get
            {
                return dbProviderFactory;
            }
        }

        /// <summary>
        /// Gets the param prefix.
        /// </summary>
        /// <value>The param prefix.</value>
        public char ParamPrefix
        {
            get
            {
                return paramPrefixToken;
            }
        }

        /// <summary>
        /// Gets the left token of table name or column name.
        /// </summary>
        /// <value>The left token.</value>
        public char LeftToken
        {
            get
            {
                return leftToken;
            }
        }

        /// <summary>
        /// Gets the right token of table name or column name.
        /// </summary>
        /// <value>The right token.</value>
        public char RightToken
        {
            get
            {
                return rightToken;
            }
        }

        #endregion
       


        /// <summary>
        /// 自增长字段查询语句
        /// </summary>
        public abstract string RowAutoID
        {
            get;
        }

        /// <summary>
        /// 是否支持批量sql提交
        /// </summary>
        public abstract bool SupportBatch
        {
            get;
        }

        /// <summary>
        /// Builds the name of the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual string BuildParameterName(string name)
        {
            string nameStr = name.Trim(leftToken, rightToken);
            if (nameStr[0] != paramPrefixToken)
            {
                if ("@?:".Contains(nameStr[0].ToString()))
                {
                    nameStr = nameStr.Substring(1).Insert(0, new string(paramPrefixToken, 1));
                }
                else
                {
                    nameStr = nameStr.Insert(0, new string(paramPrefixToken, 1));
                }
            }
            return nameStr.Replace(".", "_");
        }

        /// <summary>
        /// Builds the name of the table.
        /// </summary>
        /// <param name="tableName">The name.</param>
        /// <param name="userName">The name.</param>
        /// <returns></returns>
        public virtual string BuildTableName(string tableName, string userName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return "";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return string.Concat(leftToken.ToString(), tableName.Trim(leftToken, rightToken), rightToken.ToString());
                }
                return string.Concat(leftToken.ToString(), userName.Trim(leftToken, rightToken), rightToken.ToString())
                    + "."
                    + string.Concat(leftToken.ToString(), tableName.Trim(leftToken, rightToken), rightToken.ToString());
            }
        }


        /// <summary>
        /// 创建分页查询
        /// </summary>
        /// <param name="fromSection"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public virtual Search CreatePageFromSection(Search fromSection, int startIndex, int endIndex)
        {
            Check.Require(startIndex, "startIndex", Check.GreaterThanOrEqual<int>(1));
            Check.Require(endIndex, "endIndex", Check.GreaterThanOrEqual<int>(1));
            Check.Require(startIndex <= endIndex, "startIndex must be less than endIndex!");
            Check.Require(fromSection, "fromSection", Check.NotNullOrEmpty);

            int pageSize = endIndex - startIndex + 1;
            if (startIndex == 1)
            {
                fromSection.PrefixString = string.Concat(" TOP ", pageSize.ToString());
            }
            else
            {

                if (OrderByOperation.IsNullOrEmpty(fromSection.OrderByClip))
                {
                    foreach (Field f in fromSection.Fields)
                    {
                        if (!f.PropertyName.Equals("*") && f.PropertyName.IndexOf('(') == -1)
                        {
                            fromSection.OrderBy(f.Asc);
                            break;
                        }
                    }
                }

                Check.Require(!OrderByOperation.IsNullOrEmpty(fromSection.OrderByClip), "query.OrderByClip could not be null or empty!");

                int count = fromSection.Count(fromSection);

                List<Parameter> list = fromSection.Parameters;

                if (endIndex > count)
                {
                    int lastnumber = count - startIndex + 1;
                    if (startIndex > count)
                        lastnumber = count % pageSize;

                    fromSection.PrefixString = string.Concat(" TOP ", lastnumber.ToString());

                    fromSection.OrderBy(fromSection.OrderByClip.ReverseOrderByOperation);

                    //

                    fromSection.TableName = string.Concat(" (", fromSection.SqlString, ") AS temp_table ");

                    fromSection.PrefixString = string.Empty;

                    fromSection.DistinctString = string.Empty;

                    fromSection.GroupBy(GroupByOperation.None);

                    fromSection.Select(Field.All);

                    fromSection.OrderBy(fromSection.OrderByClip.ReverseOrderByOperation);

                    fromSection.Where(WhereOperation.All);

                }
                else
                {

                    if (startIndex < count / 2)
                    {

                        fromSection.PrefixString = string.Concat(" TOP ", endIndex.ToString());

                        fromSection.TableName = string.Concat(" (", fromSection.SqlString, ") AS tempIntable ");

                        fromSection.PrefixString = string.Concat(" TOP ", pageSize.ToString());

                        fromSection.DistinctString = string.Empty;

                        fromSection.GroupBy(GroupByOperation.None);

                        fromSection.Select(Field.All);

                        fromSection.OrderBy(fromSection.OrderByClip.ReverseOrderByOperation);

                        fromSection.Where(WhereOperation.All);

                        //

                        fromSection.TableName = string.Concat(" (", fromSection.SqlString, ") AS tempOuttable ");

                        fromSection.PrefixString = string.Empty;

                        fromSection.OrderBy(fromSection.OrderByClip.ReverseOrderByOperation);
                    }
                    else
                    {
                        fromSection.PrefixString = string.Concat(" TOP ", (count - startIndex + 1).ToString());

                        fromSection.OrderBy(fromSection.OrderByClip.ReverseOrderByOperation);

                        fromSection.TableName = string.Concat(" (", fromSection.SqlString, ") AS tempIntable ");

                        fromSection.PrefixString = string.Concat(" TOP ", pageSize.ToString());

                        fromSection.DistinctString = string.Empty;

                        fromSection.GroupBy(GroupByOperation.None);

                        fromSection.Select(Field.All);

                        fromSection.OrderBy(fromSection.OrderByClip.ReverseOrderByOperation);

                        fromSection.Where(WhereOperation.All);
                    }

                }

                fromSection.Parameters = list;

            }

            return fromSection;

        }

        /// <summary>
        /// 调整DbCommand命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public virtual void PrepareCommand(DbCommand cmd)
        {
            bool isStoredProcedure = (cmd.CommandType == CommandType.StoredProcedure);

            if (!isStoredProcedure)
            {
                if (databaseType == DatabaseType.PostgreSQL)
                {
                    cmd.CommandText = string.Format(cmd.CommandText, "", "");
                }
                else
                {
                    cmd.CommandText = DataUtils.FormatSQL(cmd.CommandText, leftToken, rightToken);
                }

            }

            foreach (DbParameter p in cmd.Parameters)
            {

                if (!isStoredProcedure)
                {
                    if (cmd.CommandText.IndexOf(p.ParameterName, StringComparison.Ordinal) == -1)
                    {
                        cmd.CommandText = cmd.CommandText.Replace("@" + p.ParameterName.Substring(1), p.ParameterName);
                        cmd.CommandText = cmd.CommandText.Replace("?" + p.ParameterName.Substring(1), p.ParameterName);
                        cmd.CommandText = cmd.CommandText.Replace(":" + p.ParameterName.Substring(1), p.ParameterName);
                    }
                }

                if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue)
                {
                    continue;
                }

                object value = p.Value;

                DbType dbType = p.DbType;

                if (value == DBNull.Value)
                {
                    continue;
                }

                if (value == null)
                {
                    p.Value = DBNull.Value;
                    continue;
                }

                Type type = value.GetType();

                if (type.IsEnum)
                {
                    p.DbType = DbType.Int32;
                    p.Value = Convert.ToInt32(value);
                    continue;
                }

                if (dbType == DbType.Guid && type != typeof(Guid))
                {
                    p.Value = new Guid(value.ToString());
                    continue;
                }

                if (type == typeof(Boolean))
                {
                    p.Value = (((bool)value) ? 1 : 0);
                    continue;
                }
            }
        }
    }
}
