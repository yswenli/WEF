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
using WEF.MvcPager;
using WEF.Section;

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


        #region 查询

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public Search<TEntity> Search()
        {
            return new Search<TEntity>(DBContext.Db, _trans);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public new Search<TEntity> Search(string tableName)
        {
            return new Search<TEntity>(DBContext.Db, tableName, _trans);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public TEntity First(Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return Search().First(lambdaWhere);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public TEntity First(string tableName, Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return Search(tableName).First(lambdaWhere);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public TEntity Single(Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return Search().Single(lambdaWhere);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public TEntity Single(string tableName, Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return Search(tableName).Single(lambdaWhere);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public List<TEntity> ToList(Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return Search().ToList(lambdaWhere);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public List<TEntity> ToList(string tableName, Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return Search(tableName).ToList(lambdaWhere);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderAsc"></param>
        /// <returns></returns>
        public PagedList<TEntity> ToPagedList(Expression<Func<TEntity, bool>> lambdaWhere,
            int pageIndex = 1,
            int pageSize = 12,
            string orderBy = "ID",
            bool orderAsc = true)
        {
            return Search().Where(lambdaWhere).ToPagedList(pageIndex, pageSize, orderBy, orderAsc);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="lambdaWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderAsc"></param>
        /// <returns></returns>
        public PagedList<TEntity> ToPagedList(string tableName, Expression<Func<TEntity, bool>> lambdaWhere,
            int pageIndex = 1,
            int pageSize = 12,
            string orderBy = "ID",
            bool orderAsc = true)
        {
            return Search(tableName).Where(lambdaWhere).ToPagedList(pageIndex, pageSize, orderBy, orderAsc);
        }

        #endregion

        #region 添加

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
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(string tableName, TEntity entity)
        {
            return DBContext.Insert(_trans, tableName, entity);
        }
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
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Insert(string tableName, params TEntity[] entities)
        {
            return DBContext.Insert(_trans, tableName, entities);
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
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Insert(string tableName, IEnumerable<TEntity> entities)
        {
            return DBContext.Insert(_trans, tableName, entities);
        }
        #endregion


        #region 更新
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
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(string tableName, TEntity entity)
        {
            return DBContext.Update(_trans, tableName, entity);
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
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Update(string tableName, params TEntity[] entities)
        {
            return DBContext.Update(_trans, tableName, entities);
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
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Update(string tableName, List<TEntity> entities)
        {
            return DBContext.Update(_trans, tableName, entities.ToArray());
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
        /// 更新
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update(string tableName, TEntity entity, Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return DBContext.Update(_trans, tableName, entity, lambdaWhere);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="lambadaSelect"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update(string tableName, Expression<Func<TEntity, object>> lambadaSelect, Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return DBContext.Update(_trans, tableName, lambadaSelect, JoinOn.None, lambdaWhere);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update(Dictionary<Field, object> fieldValue, Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return DBContext.Update<TEntity>(_trans, EntityCache.GetTableName<TEntity>(), fieldValue, JoinOn.None, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update(string tableName, Dictionary<Field, object> fieldValue, Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return DBContext.Update<TEntity>(_trans, tableName, fieldValue, JoinOn.None, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
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
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete(string tableName, TEntity entity)
        {
            return DBContext.Delete(_trans, tableName, entity);
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
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Delete(string tableName, IEnumerable<TEntity> entities)
        {
            return DBContext.Delete(_trans, tableName, entities);
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
        /// 删除
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Delete(string tableName, List<TEntity> entities)
        {
            return DBContext.Delete(_trans, tableName, entities);
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
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Delete(string tableName, Expression<Func<TEntity, bool>> lambdaWhere)
        {
            return DBContext.Delete(_trans, tableName, lambdaWhere);
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

        #region 行转列

        /// <summary>
        /// 行转列
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="pivotInfo"></param>
        /// <param name="customerWhereExpression"></param>
        /// <param name="pivotTableName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<Model> ToPivotList<Model, TEntity2>(PivotInfo<TEntity2> pivotInfo,
            WhereExpression customerWhereExpression,
            string pivotTableName,
            int pageIndex,
            int pageSize,
            string orderBy = null,
            bool asc = true)
            where TEntity2 : Entity
        {
            var tableName = EntityCache.GetTableName<TEntity2>();
            var groupBys = ExpressionToOperation<TEntity2>.ToSelect(tableName, pivotInfo.GroupBys).Select(q => q.FieldName);
            var whereExpresstion = ExpressionToOperation<TEntity2>.ToWhereOperation(pivotInfo.WhereLambada);
            return ToPivotList<Model>(tableName,
                groupBys,
                pivotInfo.ColumnNames,
                ExpressionToOperation<TEntity2>.ToSelect("", pivotInfo.TypeFieldName).First().FieldName,
                ExpressionToOperation<TEntity2>.ToSelect("", pivotInfo.ValueFieldName).First().FieldName,
                whereExpresstion,
                customerWhereExpression,
                pivotTableName,
                pageIndex,
                pageSize,
                orderBy,
                asc);
        }
        /// <summary>
        /// 行转列
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="pivotInfo"></param>
        /// <param name="customerWhereExpression"></param>
        /// <param name="pivotTableName"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public List<Model> ToPivotList<Model>(PivotInfo<TEntity> pivotInfo,
            WhereExpression customerWhereExpression,
            string pivotTableName,
            string orderBy = null,
            bool asc = true)
        {
            return ToPivotList<Model, TEntity>(pivotInfo, customerWhereExpression, pivotTableName, 1, 1000, orderBy, asc).Data;
        }
        /// <summary>
        /// 行转列
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="pivotInfo"></param>
        /// <param name="customerWhereExpression"></param>
        /// <param name="pivotTableName"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public List<Model> ToPivotList<Model, TEntity2>(PivotInfo<TEntity2> pivotInfo,
           WhereExpression customerWhereExpression,
           string pivotTableName,
            string orderBy = null,
            bool asc = true)
           where TEntity2 : Entity
        {
            return ToPivotList<Model, TEntity2>(pivotInfo, customerWhereExpression, pivotTableName, 1, 1000, orderBy, asc).Data;
        }

        /// <summary>
        /// 行转列
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="pivotInfo"></param>
        /// <param name="whereSQL"></param>
        /// <param name="pivotTableName"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public List<Model> ToPivotList<Model>(PivotInfo<TEntity> pivotInfo,
            string whereSQL = null,
            string pivotTableName = null,
            string orderBy = null,
            bool asc = true)
        {
            if (string.IsNullOrEmpty(whereSQL))
                return ToPivotList<Model>(pivotInfo,
                    WhereExpression.All,
                    pivotTableName,
                    orderBy,
                    asc);
            return ToPivotList<Model>(pivotInfo,
                    new WhereExpression(whereSQL),
                    pivotTableName,
                    orderBy,
                    asc);
        }

        /// <summary>
        /// 行转列
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="pivotInfo"></param>
        /// <param name="whereLambada"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public List<Model> ToPivotList<Model>(PivotInfo<TEntity> pivotInfo,
            Expression<Func<Model, bool>> whereLambada,
            string orderBy = null,
            bool asc = true)
            where Model : class, new()
        {
            var where = ExpressionToOperation<Model>.ToWhereOperation(whereLambada);
            return ToPivotList<Model>(pivotInfo, where, typeof(Model).Name, orderBy, asc);
        }
        /// <summary>
        /// 行转列
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="pivotInfo"></param>
        /// <param name="whereLambada"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public List<Model> ToPivotList<Model, TEntity2>(PivotInfo<TEntity2> pivotInfo,
            Expression<Func<Model, bool>> whereLambada,
            string orderBy = null,
            bool asc = true)
            where Model : class, new()
            where TEntity2 : Entity
        {
            var where = ExpressionToOperation<Model>.ToWhereOperation(whereLambada);
            return ToPivotList<Model, TEntity2>(pivotInfo, where, typeof(Model).Name, orderBy, asc);
        }

        #endregion
    }
}
