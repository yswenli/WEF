/****************************************************************************
*Copyright (c) 2023 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Db
*文件名： DbTransT
*版本号： V1.0.0.0
*唯一标识：55e5d991-564a-439a-b3e6-e82ef1a4cc3c
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2023/2/14 13:22:04
*描述：事务
*
*=================================================
*修改标记
*修改时间：2023/2/14 13:22:04
*修改人： yswenli
*版本号： V1.0.0.0
*描述：事务
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

using WEF.Common;
using WEF.Expressions;

namespace WEF.Db
{
    /// <summary>
    /// 事务
    /// </summary>
    public class DbTrans<TEntity> : DbTrans where TEntity : Entity
    {
        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="dbContext"></param>
        /// <param name="timeout"></param>
        public DbTrans(DbTransaction trans, DBContext dbContext, int timeout = 120) : base(trans, dbContext, timeout)
        {

        }

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="dbTrans"></param>
        /// <returns></returns>
        public static implicit operator DbTransaction(DbTrans<TEntity> dbTrans)
        {
            return dbTrans._trans;
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public Search<TEntity> Search()
        {
            return new Search<TEntity>(DBContext.Db, _trans);
        }

        #region 添加

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Insert(params TEntity[] entities)
        {
            return DBContext.Insert(_trans, entities);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entities"></param>
        public int Insert(IEnumerable<TEntity> entities)
        {
            return DBContext.Insert(_trans, entities);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entities"></param>
        public int Insert(List<TEntity> entities)
        {
            return DBContext.Insert(_trans, entities);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(TEntity entity)
        {
            return DBContext.Insert(_trans, entity);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public int Insert(Field[] fields, object[] values)
        {
            return DBContext.Insert<TEntity>(_trans, EntityCache.GetTableName<TEntity>(), fields, values);
        }
        #endregion


        #region 更新

        /// <summary>
        /// 更新全部字段  
        /// </summary>
        /// <param name="entities"></param>
        public int UpdateAll(params TEntity[] entities)
        {
            return DBContext.UpdateAll(_trans, entities);
        }
        /// <summary>
        /// 更新全部字段
        /// </summary>
        /// <param name="entities"></param>
        public int UpdateAll(IEnumerable<TEntity> entities)
        {
            return DBContext.UpdateAll(_trans, entities.ToArray());
        }
        /// <summary>
        /// 更新全部字段  
        /// </summary>
        /// <param name="entity"></param>
        public int UpdateAll(TEntity entity)
        {
            return DBContext.UpdateAll(_trans, entity);
        }
        /// <summary>
        /// 更新全部字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int UpdateAll(TEntity entity, WhereExpression where)
        {
            return DBContext.UpdateAll(_trans, entity, where);
        }
        /// <summary>
        /// 更新全部字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int UpdateAll(TEntity entity, Where where)
        {
            return DBContext.UpdateAll(_trans, entity, where);
        }
        /// <summary>
        /// 更新  
        /// </summary>
        /// <param name="entities"></param>
        public int Update(params TEntity[] entities)
        {
            return DBContext.Update(_trans, entities);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entities"></param>
        public int Update(List<TEntity> entities)
        {
            return DBContext.Update(_trans, entities.ToArray());
        }
        /// <summary>
        /// 更新  
        /// </summary>
        /// <param name="entity"></param>
        public int Update(TEntity entity)
        {
            return DBContext.Update(_trans, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update(TEntity entity, WhereExpression where)
        {
            return DBContext.Update(_trans, entity, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update(TEntity entity, Where where)
        {
            return DBContext.Update(_trans, entity, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update(TEntity entity, Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return DBContext.Update(_trans, entity, lambdaWhere);
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update(Field field, object value, WhereExpression where)
        {
            return DBContext.Update<TEntity>(_trans, EntityCache.GetTableName<TEntity>(), field, value, where);
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update(Field field, object value, Where where)
        {
            return DBContext.Update<TEntity>(_trans, EntityCache.GetTableName<TEntity>(), field, value, where);
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update(Field field, object value, Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return DBContext.Update<TEntity>(_trans, EntityCache.GetTableName<TEntity>(), field, value, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 更新多个值
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update(Dictionary<Field, object> fieldValue, WhereExpression where)
        {
            return DBContext.Update<TEntity>(_trans, EntityCache.GetTableName<TEntity>(), fieldValue, where);
        }
        /// <summary>
        /// 更新多个值
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update(Dictionary<Field, object> fieldValue, Where where)
        {
            return DBContext.Update<TEntity>(_trans, EntityCache.GetTableName<TEntity>(), fieldValue, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update(Dictionary<Field, object> fieldValue, Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return DBContext.Update<TEntity>(_trans, EntityCache.GetTableName<TEntity>(), fieldValue, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update(Field[] fields, object[] values, WhereExpression where)
        {
            return DBContext.Update<TEntity>(_trans, EntityCache.GetTableName<TEntity>(), fields, values, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update(Field[] fields, object[] values, Where where)
        {
            return DBContext.Update<TEntity>(_trans, EntityCache.GetTableName<TEntity>(), fields, values, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update(Field[] fields, object[] values, Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return DBContext.Update<TEntity>(_trans, EntityCache.GetTableName<TEntity>(), fields, values, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }

        #endregion


        #region 删除

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete(TEntity entity)
        {
            return DBContext.Delete(_trans, entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities"></param>
        public int Delete(IEnumerable<TEntity> entities)
        {
            return DBContext.Delete(_trans, entities);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities"></param>
        public int Delete(List<TEntity> entities)
        {
            return DBContext.Delete(_trans, entities);
        }
        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public int Delete(params string[] pkValues)
        {
            return DBContext.DeleteByPrimaryKey<TEntity>(_trans, pkValues);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public int Delete(params Guid[] pkValues)
        {
            return DBContext.DeleteByPrimaryKey<TEntity>(_trans, pkValues);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public int Delete(params long[] pkValues)
        {
            return DBContext.DeleteByPrimaryKey<TEntity>(_trans, pkValues);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public int Delete(params int[] pkValues)
        {
            return DBContext.DeleteByPrimaryKey<TEntity>(_trans, pkValues);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete(WhereExpression where)
        {
            return DBContext.Delete<TEntity>(_trans, EntityCache.GetTableName<TEntity>(), where);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete(Where where)
        {
            return DBContext.Delete<TEntity>(_trans, EntityCache.GetTableName<TEntity>(), where.ToWhereClip());
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Delete(Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return DBContext.Delete(_trans, lambdaWhere);
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
        public Exception TryCommit(Action<DbTrans<TEntity>> action, int tryCount = 3, int sleep = 3 * 1000)
        {
            var work = new Func<Exception>(() =>
            {
                Exception result = null;
                try
                {
                    action.Invoke(this);
                    Commit();
                }
                catch (Exception ex)
                {
                    Rollback();
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
    }
}
