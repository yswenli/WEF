/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/
using WEF.Common;
using WEF.Expressions;
using WEF.Section;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;

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
        private DbTransaction _trans;

        /// <summary>
        /// 连接
        /// </summary>
        private DbConnection _conn;

        /// <summary>
        /// DBContext
        /// </summary>
        DBContext DBContext { get; set; }

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

            //超时退出
            if (timeout > 0)
            {
                Task.Run(() =>
                {
                    int i = 0;
                    while (_conn.State == ConnectionState.Open)
                    {
                        Thread.Sleep(1000);
                        i++;
                        if (i > timeout)
                        {
                            Close();
                            throw new TimeoutException("事务执行超时");
                        }
                    }
                });

            }
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
            if (!_isCommitOrRollback)
            {
                _trans.Commit();
                _isCommitOrRollback = true;
            }
            Close();
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
            Close();
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
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (_isClose)
                return;

            if (!_isCommitOrRollback)
            {
                _isCommitOrRollback = true;

                _trans?.Rollback();
            }

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
            return DBContext.FromSqlWithdModel<T>(sql, inputParams).SetDbTransaction(_trans);
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
        /// <typeparam name="T"></typeparam>
        /// <param name="proName"></param>
        /// <param name="inputParams"></param>
        /// <returns></returns>
        public ProcSection FromPro<T>(string proName, T inputParams) where T : class, new()
        {
            return DBContext.FromProc<T>(proName, inputParams).SetDbTransaction(_trans);
        }

        #region 查询

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public Search<TEntity> Search<TEntity>()
            where TEntity : Entity
        {
            return new Search<TEntity>(DBContext.Db, _trans);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public Search<TEntity> Search<TEntity>(string asName)
            where TEntity : Entity
        {
            return new Search<TEntity>(DBContext.Db, _trans, asName);
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
        #endregion


        #region 更新

        /// <summary>
        /// 更新全部字段  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int UpdateAll<TEntity>(params TEntity[] entities)
            where TEntity : Entity
        {
            return DBContext.UpdateAll<TEntity>(_trans, entities);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int UpdateAll<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.UpdateAll<TEntity>(_trans, entities.ToArray());
        }
        /// <summary>
        /// 更新全部字段  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public int UpdateAll<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            return DBContext.UpdateAll<TEntity>(_trans, entity);
        }


        /// <summary>
        /// 更新全部字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int UpdateAll<TEntity>(TEntity entity, WhereOperation where)
            where TEntity : Entity
        {
            return DBContext.UpdateAll<TEntity>(_trans, entity, where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int UpdateAll<TEntity>(TEntity entity, Where where)
            where TEntity : Entity
        {
            return DBContext.UpdateAll<TEntity>(_trans, entity, where);
        }


        /// <summary>
        /// 更新  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Update<TEntity>(params TEntity[] entities)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, entities);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Update<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, entities.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Update<TEntity>(List<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, entities.ToArray());
        }
        /// <summary>
        /// 更新  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public int Update<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity entity, WhereOperation where)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, entity, where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity entity, Where where)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, entity, where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, entity, ExpressionToOperation<TEntity>.ToWhereOperation(entity.GetTableName(), lambdaWhere));
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Field field, object value, WhereOperation where)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, tableName, field, value, where);
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Field field, object value, Where where)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, tableName, field, value, where);
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Field field, object value, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, tableName, field, value, ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere));
        }
        /// <summary>
        /// 更新多个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Dictionary<Field, object> fieldValue, WhereOperation where)
              where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, tableName, fieldValue, where);
        }
        /// <summary>
        /// 更新多个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Dictionary<Field, object> fieldValue, Where where)
              where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, tableName, fieldValue, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Dictionary<Field, object> fieldValue, Expression<Func<TEntity, bool>> lambdaWhere)
              where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, tableName, fieldValue, ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere));
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Field[] fields, object[] values, WhereOperation where)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, tableName, fields, values, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Field[] fields, object[] values, Where where)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, tableName, fields, values, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Field[] fields, object[] values, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(_trans, tableName, fields, values, ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere));
        }

        #endregion


        #region 删除

        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            return DBContext.Delete<TEntity>(_trans, entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Delete<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.Delete<TEntity>(_trans, entities);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Delete<TEntity>(List<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.Delete<TEntity>(_trans, entities);
        }
        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public int Delete<TEntity>(params string[] pkValues)
            where TEntity : Entity
        {
            return DBContext.DeleteByPrimaryKey<TEntity>(_trans, pkValues);
        }
        public int Delete<TEntity>(params Guid[] pkValues)
            where TEntity : Entity
        {
            return DBContext.DeleteByPrimaryKey<TEntity>(_trans, pkValues);
        }
        public int Delete<TEntity>(params long[] pkValues)
            where TEntity : Entity
        {
            return DBContext.DeleteByPrimaryKey<TEntity>(_trans, pkValues);
        }
        public int Delete<TEntity>(params int[] pkValues)
            where TEntity : Entity
        {
            return DBContext.DeleteByPrimaryKey<TEntity>(_trans, pkValues);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete<TEntity>(string tableName, WhereOperation where)
            where TEntity : Entity
        {
            return DBContext.Delete<TEntity>(_trans, tableName, where);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete<TEntity>(string tableName, Where where)
            where TEntity : Entity
        {
            return DBContext.Delete<TEntity>(_trans, tableName, where.ToWhereClip());
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Delete<TEntity>(string tableName, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return DBContext.Delete<TEntity>(_trans, tableName, ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere));
        }
        #endregion


        #region 添加

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Insert<TEntity>(params TEntity[] entities)
            where TEntity : Entity
        {
            return DBContext.Insert(_trans, entities);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Insert<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.Insert<TEntity>(_trans, entities);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Insert<TEntity>(List<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.Insert<TEntity>(_trans, entities);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            return DBContext.Insert<TEntity>(_trans, entity);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public int Insert<TEntity>(string tableName, Field[] fields, object[] values)
            where TEntity : Entity
        {
            return DBContext.Insert<TEntity>(_trans, tableName, fields, values);
        }
        #endregion

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
                    this.Dispose();
                    return ex;
                }
                Thread.Sleep(sleep);
                count++;
                if (count >= tryCount)
                {
                    this.Dispose();
                    return ex;
                }
            }
        }

        #endregion
    }
}
