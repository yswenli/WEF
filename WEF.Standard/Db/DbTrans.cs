/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

using WEF.Common;
using WEF.Section;

namespace WEF.Db
{
    /// <summary>
    /// 事务
    /// </summary>    
    public class DbTrans : IDisposable
    {
        static object _locker = new object();

        /// <summary>
        /// 事务
        /// </summary>
        protected DbTransaction _trans;

        /// <summary>
        /// 连接
        /// </summary>
        protected DbConnection _conn;

        /// <summary>
        /// DBContext
        /// </summary>
        protected DBContext DBContext { get; set; }

        /// <summary>
        /// 判断是否有提交或回滚
        /// </summary>
        private bool _isCommitOrRollback = false;

        /// <summary>
        /// 是否关闭
        /// </summary>
        private bool _isClose = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="dbContext"></param>
        /// <param name="timeout">默认120s</param>
        public DbTrans(DbTransaction trans, DBContext dbContext, int timeout = 120)
        {
            Check.Require(trans, "trans", Check.NotNull);

            Monitor.Enter(_locker);

            _trans = trans;

            _conn = trans.Connection;

            DBContext = dbContext;

            if (_conn.State != ConnectionState.Open)
                _conn.Open();
        }

        /// <summary>
        /// 连接
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                return _conn;
            }
        }

        /// <summary>
        /// 事务级别
        /// </summary>
        public IsolationLevel IsolationLevel
        {
            get { return _trans.IsolationLevel; }
        }

        /// <summary>
        /// 提交
        /// </summary>
        public void Commit()
        {
            try
            {
                if (!_isCommitOrRollback)
                {
                    _trans.Commit();
                    _isCommitOrRollback = true;
                }
            }
            catch (Exception ex)
            {
                Rollback();
                throw ex;
            }
        }


        /// <summary>
        /// 回滚
        /// </summary>
        public void Rollback()
        {
            if (!_isCommitOrRollback)
            {
                _trans.Rollback();
                _isCommitOrRollback = true;
            }
        }


        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="dbTrans"></param>
        /// <returns></returns>
        public static implicit operator DbTransaction(DbTrans dbTrans)
        {
            return dbTrans._trans;
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Search<T> Search<T>() where T : Entity
        {
            return new Search<T>(DBContext.Db, _trans);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Search Search(string tableName)
        {
            return new Search(DBContext.Db, tableName, "", _trans);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert<T>(T entity) where T : Entity
        {
            return DBContext.Insert(_trans, entity);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert<T>(string tableName, T entity) where T : Entity
        {
            return DBContext.Insert(_trans, tableName, entity);
        }

        /// <summary>
        /// 添加多条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Insert<T>(List<T> entities) where T : Entity
        {
            return DBContext.Insert(_trans, entities);
        }
        /// <summary>
        /// 添加多条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Insert<T>(string tableName, List<T> entities) where T : Entity
        {
            return DBContext.Insert(_trans, tableName, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Update<T>(params T[] entities) where T : Entity
        {
            return DBContext.Update(_trans, entities);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Update<T>(string tableName, params T[] entities) where T : Entity
        {
            return DBContext.Update(_trans, tableName, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Update<T>(List<T> entities) where T : Entity
        {
            return DBContext.Update(_trans, entities);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Update<T>(string tableName, List<T> entities) where T : Entity
        {
            return DBContext.Update(_trans, tableName, entities);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<T>(T entity, Expression<Func<T, bool>> lambdaWhere) where T : Entity
        {
            return DBContext.Update(_trans, entity, lambdaWhere);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<T>(string tableName, T entity, Expression<Func<T, bool>> lambdaWhere) where T : Entity
        {
            return DBContext.Update(_trans, tableName, entity, lambdaWhere);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete<T>(T entity) where T : Entity
        {
            return Delete(entity.GetTableName(), entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete<T>(string tableName, T entity) where T : Entity
        {
            return DBContext.Delete(_trans, tableName, entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Delete<T>(List<T> entities) where T : Entity
        {
            return DBContext.Delete(_trans, entities);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Delete<T>(string tableName, List<T> entities) where T : Entity
        {
            return DBContext.Delete(_trans, tableName, entities);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Delete<T>(Expression<Func<T, bool>> lambdaWhere) where T : Entity
        {
            return DBContext.Delete(_trans, lambdaWhere);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Delete<T>(string tableName, Expression<Func<T, bool>> lambdaWhere) where T : Entity
        {
            return DBContext.Delete(_trans, tableName, lambdaWhere);
        }

        /// <summary>
        /// FromSql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql)
        {
            return DBContext.FromSql(sql).SetDbTransaction(_trans);
        }
        /// <summary>
        /// FromSql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="inputParams"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql, Dictionary<string, object> inputParams)
        {
            return DBContext.FromSqlWithdAutomatic(sql, inputParams.ToArray()).SetDbTransaction(_trans);
        }

        /// <summary>
        /// FromSql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="inputParams"></param>
        /// <returns></returns>
        public SqlSection FromSql<T>(string sql, T inputParams) where T : class, new()
        {
            return DBContext.FromSqlWithdModel(sql, inputParams).SetDbTransaction(_trans);
        }

        /// <summary>
        /// FromPro
        /// </summary>
        /// <param name="proName"></param>
        /// <returns></returns>
        public ProcSection FromPro(string proName)
        {
            return DBContext.FromProc(proName).SetDbTransaction(_trans);
        }
        /// <summary>
        /// FromPro
        /// </summary>
        /// <param name="proName"></param>
        /// <param name="inputParams"></param>
        /// <returns></returns>
        public ProcSection FromPro(string proName, Dictionary<string, object> inputParams)
        {
            return DBContext.FromProc(proName, inputParams).SetDbTransaction(_trans);
        }

        /// <summary>
        /// FromPro
        /// </summary>
        /// <param name="proName"></param>
        /// <param name="inputParams"></param>
        /// <returns></returns>
        public ProcSection FromPro<T>(string proName, T inputParams) where T : class, new()
        {
            return DBContext.FromProc(proName, inputParams).SetDbTransaction(_trans);
        }


        #region try

        /// <summary>
        /// 尝试提交，
        /// 自动提交、回滚 释放事务
        /// </summary>
        /// <param name="action"></param>
        /// <param name="tryCount"></param>
        /// <param name="sleep"></param>
        /// <returns></returns>
        public Exception TryCommit(Action<DbTrans> action, int tryCount = 3, int sleep = 3 * 1000)
        {
            var work = new Func<Exception>(() =>
            {
                Exception result = null;
                try
                {
                    action.Invoke(this);
                    this.Commit();
                }
                catch (Exception ex)
                {
                    this.Rollback();
                    result = ex;
                }
                return result;
            });

            var count = 0;

            while (true)
            {
                var ex = work.Invoke();
                if (ex == null)
                {
                    Close();
                    return ex;
                }
                Thread.Sleep(sleep);
                count++;
                if (count >= tryCount)
                {
                    Close();
                    return ex;
                }
            }
        }

        #endregion

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (_isClose)
                return;

            if (_conn.State != ConnectionState.Closed)
            {
                _conn.Close();
            }

            _trans?.Dispose();

            _isClose = true;

            Monitor.Exit(_locker);
        }


        #region IDisposable 成员

        /// <summary>
        /// 关闭连接并释放资源
        /// </summary>
        public void Dispose()
        {
            Commit();
            Close();
        }

        #endregion


    }
}
