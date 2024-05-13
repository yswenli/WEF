/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2024
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

using WEF.Common;
using WEF.Expressions;

namespace WEF.Section
{

    /// <summary>
    /// 执行sql语句
    /// </summary>
    public class SqlSection : Section
    {
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public SqlSection(DBContext dbContext, string sql, params Parameter[] parameters)
            : base(dbContext, sql)
        {

            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            this._dbCommand = dbContext.Db.GetSqlStringCommand(sql);

            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    this.AddInParameter(item.ParameterName, item.ParameterDbType, item.ParameterSize, item.ParameterValue);
                }
            }
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <param name="keyValuePairs"></param>
        public SqlSection(DBContext dbContext, string sql, IEnumerable<KeyValuePair<string, object>> keyValuePairs)
            : base(dbContext, sql)
        {

            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            this._dbCommand = dbContext.Db.GetSqlStringCommand(sql);

            if (keyValuePairs != null)
                this.AddInParameter(keyValuePairs);
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        public SqlSection(DBContext dbContext, string sql, int pageIndex, int pageSize, string orderBy, bool asc = true)
            : base(dbContext, sql, pageIndex, pageSize)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            _dbCommand = dbContext.Db.GetSqlStringCommand(sql, pageIndex, pageSize, orderBy, asc);
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBys"></param>
        /// <param name="parameters"></param>
        public SqlSection(DBContext dbContext, string sql, int pageIndex, int pageSize, Dictionary<string, OrderByOperater> orderBys, params Parameter[] parameters)
           : base(dbContext, sql, pageIndex, pageSize)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            this._dbCommand = dbContext.Db.GetSqlStringCommand(sql, pageIndex, pageSize, orderBys);

            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    this.AddInParameter(item.ParameterName, item.ParameterDbType, item.ParameterSize, item.ParameterValue);
                }
            }
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBys"></param>
        /// <param name="keyValuePairs"></param>
        public SqlSection(DBContext dbContext, string sql, int pageIndex, int pageSize, Dictionary<string, OrderByOperater> orderBys, KeyValuePair<string, object>[] keyValuePairs)
           : base(dbContext, sql, pageIndex, pageSize)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            this._dbCommand = dbContext.Db.GetSqlStringCommand(sql, pageIndex, pageSize, orderBys);

            this.AddInParameter(keyValuePairs);
        }

        /// <summary>
        /// 设置事务
        /// </summary>
        /// <param name="tran"></param>
        /// <returns></returns>
        public SqlSection SetDbTransaction(DbTransaction tran)
        {
            this._dbTransaction = tran;
            return this;
        }

        #region 添加参数


        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlSection AddInParameter(string parameterName, DbType dbType, object value)
        {
            Check.Require(parameterName, "parameterName", Check.NotNullOrEmpty);
            Check.Require(dbType, "dbType", Check.NotNullOrEmpty);
            return AddInParameter(parameterName, dbType, 0, value);
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlSection AddInParameter(string parameterName, DbType dbType, int size, object value)
        {
            Check.Require(parameterName, "parameterName", Check.NotNullOrEmpty);
            Check.Require(dbType, "dbType", Check.NotNullOrEmpty);

            _dbContext.Db.AddInParameter(this._dbCommand, parameterName, dbType, size, value);
            _dbContext.Db.AddInParameter(this._dbCountCommand, parameterName, dbType, size, value);
            return this;
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlSection AddInParameter(string parameterName, object value)
        {
            Check.Require(parameterName, "parameterName", Check.NotNullOrEmpty);
            Check.Require(value, "value", Check.NotNullOrEmpty);
            return AddInParameter(parameterName, value.GetDbType(), 0, value);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public SqlSection AddInParameter(IEnumerable<KeyValuePair<string, object>> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                this.AddInParameter(keyValuePair.Key, keyValuePair.Value.GetDbType(), 0, keyValuePair.Value);
            }
            return this;
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public SqlSection AddInParameterWithModel<T>(T parameter) where T : class
        {
            Check.Require(parameter, "parameter", Check.NotNullOrEmpty);
            List<KeyValuePair<string, object>> keyValuePairs = new List<KeyValuePair<string, object>>();
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var value = DynamicCalls.GetPropertyGetter(property).Invoke(parameter);
                keyValuePairs.Add(new KeyValuePair<string, object>($"{property.Name}", value));
            }
            this.AddInParameter(keyValuePairs);
            return this;
        }

        /// <summary>
        /// 添加集合型参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public SqlSection AddInListParameter<T>(string parameterName, IEnumerable<T> list)
        {
            Check.Require(parameterName, "parameterName", Check.NotNullOrEmpty);
            if (list != null && list.Any())
            {
                var i = 0;
                foreach (var item in list)
                {
                    i += 1;
                    this.AddInParameter($"{parameterName}{i}", item);
                }
            }
            return this;
        }
        /// <summary>
        /// 添加集合型参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlSection AddInParameter(IEnumerable<Parameter> parameters)
        {
            if (parameters != null && parameters.Any())
            {
                foreach (var parameter in parameters)
                {
                    AddInParameter(parameter.ParameterName, parameter.ParameterDbType, parameter.ParameterSize, parameter.ParameterValue);
                }
            }
            return this;
        }
        #endregion

    }
}
