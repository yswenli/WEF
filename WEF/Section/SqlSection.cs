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
using System.Data;
using System.Data.Common;
using WEF.Common;

namespace WEF.Section
{

    /// <summary>
    /// 执行sql语句
    /// </summary>
    public class SqlSection : Section
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        public SqlSection(DBContext dbContext, string sql)
            : base(dbContext)
        {

            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            this._dbCommand = dbContext.Db.GetSqlStringCommand(sql);
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
        /// <param name="value"
        /// <param name="dbType"></param>
        /// <returns></returns>
        public SqlSection AddParameter(params DbParameter[] parameters)
        {
            _dbContext.Db.AddParameter(this._dbCommand, parameters);
            return this;
        }


        /// <summary>
        /// 添加参数
        /// </summary>
        public SqlSection AddInParameter(string parameterName, DbType dbType, object value)
        {
            return AddInParameter(parameterName, dbType, 0, value);
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"
        /// <param name="dbType"></param>
        /// <returns></returns>
        public SqlSection AddInParameter(string parameterName, DbType dbType, int size, object value)
        {
            Check.Require(parameterName, "parameterName", Check.NotNullOrEmpty);
            Check.Require(dbType, "dbType", Check.NotNullOrEmpty);

            _dbContext.Db.AddInParameter(this._dbCommand, parameterName, dbType, size, value);
            return this;
        }

        #endregion
    }
}
