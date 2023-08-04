/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：c9935cdf-7d39-434f-a3f9-b3b3fb92bf68
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：WEF
 * 类名称：SearchT
 * 创建时间：2017/7/27 15:12:26
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Schema;

using WEF.Common;
using WEF.Db;
using WEF.Expressions;
using WEF.MvcPager;

namespace WEF
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <typeparam name="T"></typeparam>    
    public class Search<T> : Search where T : Entity
    {
        /// <summary>
        /// 查询
        /// </summary>
        public Search()
           : this(new DBContext().Db)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="database"></param>
        public Search(Database database)
            : this(database, (DbTransaction)null)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="database"></param>
        /// <param name="trans"></param>
        public Search(Database database, DbTransaction trans)
            : base(database, database.DbProvider.BuildTableName(EntityCache.GetTableName<T>(), EntityCache.GetUserName<T>()), "", trans)
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="database"></param>
        /// <param name="trans"></param>
        /// <param name="asName"></param>
        public Search(Database database, DbTransaction trans, string asName)
            : base(database, database.DbProvider.BuildTableName((string.IsNullOrEmpty(asName) ? EntityCache.GetTableName<T>() : asName), EntityCache.GetUserName<T>()), asName, trans)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        /// <param name="trans"></param>
        public Search(Database database, string tableName, DbTransaction trans)
            : base(database, database.DbProvider.BuildTableName((string.IsNullOrEmpty(tableName) ? EntityCache.GetTableName<T>() : tableName), EntityCache.GetUserName<T>()), null, trans)
        {

        }

        #region 连接  Join
        /// <summary>
        /// InnerJoin
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public Search<T> InnerJoin(Search fs)
        {
            return Join(EntityCache.GetTableName<T>(), EntityCache.GetUserName<T>(), _where, JoinType.InnerJoin);
        }
        /// <summary>
        /// Inner Join，条件中只能写表关联条件。Lambda写法：.InnerJoin&lt;Model2>((a, b) => a.ID == b.ID)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <param name="asName"></param>
        /// <param name="asName2"></param>
        /// <returns></returns>
        public Search<T> InnerJoin<TEntity>(WhereExpression where, string asName = "", string asName2 = "")
             where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where, JoinType.InnerJoin);
        }
        /// <summary>
        /// Inner Join，条件中只能写表关联条件。Lambda写法：.InnerJoin&lt;Model2>((a, b) => a.ID == b.ID)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <param name="asName"></param>
        /// <returns></returns>
        public Search<T> InnerJoin<TEntity>(Expression<Func<T, TEntity, bool>> lambdaWhere, string asName = "")
             where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), ExpressionToOperation<T>.ToJoinWhere(lambdaWhere), JoinType.InnerJoin);
        }
        /// <summary>
        /// Cross Join
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search<T> CrossJoin<TEntity>(WhereExpression where)
            where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where, JoinType.CrossJoin);
        }


        /// <summary>
        /// Count
        /// </summary>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, object>> lambdaSelect)
        {
            return Count(GetPagedFromSection(lambdaSelect));
        }

        /// <summary>
        /// LongCount
        /// </summary>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public long LongCount(Expression<Func<T, object>> lambdaSelect)
        {
            return LongCount(GetPagedFromSection(lambdaSelect));
        }

        /// <summary>
        /// 获取分页过的FromSection
        /// </summary>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        internal Search GetPagedFromSection(Expression<Func<T, object>> lambdaSelect)
        {
            if (startIndex > 0 && endIndex > 0 && !isPageFromSection)
            {
                isPageFromSection = true;
                var s = this.Select(lambdaSelect);
                return _dbProvider.CreatePageFromSection(s, startIndex, endIndex);
            }
            return this;
        }

        /// <summary>
        /// Right Join，条件中只能写表关联条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search<T> RightJoin<TEntity>(WhereExpression where)
            where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where, JoinType.RightJoin);
        }

        /// <summary>
        /// Left Join，条件中只能写表关联条件。经典写法：Model1._.ID == Model2._.ID
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search<T> LeftJoin<TEntity>(WhereExpression where)
             where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where, JoinType.LeftJoin);
        }

        /// <summary>
        /// Left Join，条件中只能写表关联条件。Lambda写法：.LeftJoin&lt;Model2>((d1,d2) => d1.ID == d2.ID)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="lambdaJoin"></param>
        /// <returns></returns>
        public Search<T> LeftJoin<TEntity>(Expression<Func<T, TEntity, bool>> lambdaJoin)
             where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(),
                EntityCache.GetUserName<TEntity>(),
                ExpressionToOperation<T>.ToJoinWhere(lambdaJoin),
                JoinType.LeftJoin);
        }

        #region 多表左链

        /// <summary>
        /// Left Join，条件中只能写表关联条件。Lambda写法：.LeftJoin&lt;Model2>((d1,d2) => d1.ID == d2.ID)
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="lambdaJoin"></param>
        /// <returns></returns>
        public Search<T, T2> LeftJoin2<T2>(Expression<Func<T, T2, bool>> lambdaJoin)
             where T2 : Entity
        {
            return (Search<T, T2>)base.Join(EntityCache.GetTableName<T2>(),
                EntityCache.GetUserName<T2>(),
                ExpressionToOperation<T>.ToJoinWhere(lambdaJoin),
                JoinType.LeftJoin);
        }

        #endregion


        /// <summary>
        /// Full Join，条件中只能写表关联条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search<T> FullJoin<TEntity>(WhereExpression where)
            where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where, JoinType.FullJoin);
        }

        /// <summary>
        /// Join其他的多表，
        /// 其中一表必须是已经join过的表，
        /// 条件中只能写表关联条件，
        /// 需要注意泛型参数顺序
        /// </summary>
        /// <typeparam name="TEntity1"></typeparam>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="joinWhere"></param>
        /// <param name="joinType"></param>
        /// <returns></returns>
        public Search<T> Join<TEntity1, TEntity2>(Expression<Func<TEntity1, TEntity2, bool>> joinWhere, JoinType joinType)
            where TEntity1 : Entity
            where TEntity2 : Entity
        {
            var where = ExpressionToOperation<TEntity1>.ToJoinWhere(joinWhere);
            return (Search<T>)base.Join(EntityCache.GetTableName<TEntity2>(), EntityCache.GetUserName<TEntity1>(), where, joinType);
        }

        /// <summary>
        /// 连接，条件中只能写表关联条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="joinWhere"></param>
        /// <param name="joinType"></param>
        /// <returns></returns>
        public Search<T> Join<TEntity>(Expression<Func<T, TEntity, bool>> joinWhere, JoinType joinType) where TEntity : Entity
        {
            return Join<TEntity>(ExpressionToOperation<T>.ToJoinWhere(joinWhere), joinType);
        }

        /// <summary>
        /// 连接，条件中只能写表关联条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <param name="joinType"></param>
        /// <returns></returns>
        private Search<T> Join<TEntity>(WhereExpression where, JoinType joinType) where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where, joinType);
        }


        /// <summary>
        /// 连接，条件中只能写表关联条件
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <param name="joinType"></param>
        /// <returns></returns>
        private Search<T> Join(string tableName, string userName, WhereExpression where, JoinType joinType)
        {
            return (Search<T>)base.Join(tableName, userName, where, joinType);
        }

        #endregion

        #region 私有方法


        /// <summary>
        ///  设置默认主键排序 
        /// </summary>
        private void setPrimarykeyOrderby()
        {

            Field[] primarykeys = EntityCache.GetPrimaryKeyFields<T>();

            OrderByOperation temporderBy;

            if (null != primarykeys && primarykeys.Length > 0)
            {
                temporderBy = new OrderByOperation(primarykeys[0]);
            }
            else
            {
                temporderBy = new OrderByOperation(EntityCache.GetFirstField<T>());
            }

            OrderBy(temporderBy);
        }

        #endregion

        #region 操作


        /// <summary>
        /// Having 
        /// </summary>
        public new Search<T> Having(WhereExpression havingWhere)
        {
            return (Search<T>)base.Having(havingWhere);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search<T> Having(Where where)
        {
            return (Search<T>)base.Having(where.ToWhereClip());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lambdaHaving"></param>
        /// <returns></returns>
        public Search<T> Having(Expression<Func<T, bool>> lambdaHaving)
        {
            return (Search<T>)base.Having(ExpressionToOperation<T>.ToWhereOperation(lambdaHaving));
        }

        /// <summary>
        /// where
        /// </summary>
        /// <param name="whereSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public new Search<T> Where(string whereSql, params Parameter[] parameters)
        {
            return (Search<T>)base.Where(whereSql, parameters);
        }

        /// <summary>
        /// whereclip
        /// </summary>
        public new Search<T> Where(WhereExpression where)
        {
            return (Search<T>)base.Where(where);
        }
        /// <summary>
        /// Where
        /// </summary>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        public Search<T> Where(Where<T> whereParam)
        {
            return (Search<T>)base.Where(whereParam.ToWhereClip());
        }
        /// <summary>
        /// Where
        /// </summary>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        public Search<T> Where(Where whereParam)
        {
            return (Search<T>)base.Where(whereParam.ToWhereClip());
        }

        /// <summary>
        /// Where
        /// </summary>
        /// <param name="lambdaWheres"></param>
        /// <returns></returns>
        public Search<T> Where(params Expression<Func<T, bool>>[] lambdaWheres)
        {
            if (lambdaWheres == null || !lambdaWheres.Any()) throw new Exception("where条件不能为空");

            foreach (var item in lambdaWheres)
            {
                if (item == null) continue;

                if (this._where == null)
                {
                    this._where = new WhereBuilder(_tableName, ExpressionToOperation<T>.ToWhereOperation(item)).ToWhereClip();
                }
                else
                    this._where = this._where.And(ExpressionToOperation<T>.ToWhereOperation(item));
            }

            return this;
        }

        /// <summary>
        /// 子查询
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public Search<T> SubQuery(Expression<Func<T, WhereExpression>> lambdaWhere)
        {
            if (lambdaWhere == null) return this;

            var we = ExpressionToOperation<T>.ConvertSubQuery(_tableName, lambdaWhere.Body);

            if (this._where == null)
            {
                this._where = new WhereBuilder(_tableName, we).ToWhereClip();
            }
            else
                this._where = this._where.And(we);

            return this;
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public Search<T> Where<T2>(Expression<Func<T, T2, bool>> lambdaWhere)
        {
            return Where(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public Search<T> Where<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere)
        {
            return Where(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public Search<T> Where<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere)
        {
            return Where(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        public Search<T> Where<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere)
        {
            return Where(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public Search<T> Where<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere)
        {
            return Where(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }

        /// <summary>
        /// Select
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T> Select<T2>(Expression<Func<T, T2, object>> lambdaSelect)
        {
            return (Search<T>)AddSelect(ExpressionToOperation<T>.ToSelect(_tableName, lambdaSelect));
        }
        /// <summary>
        /// Select
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T> Select<T2, T3>(Expression<Func<T, T2, T3, object>> lambdaSelect)
        {
            return (Search<T>)AddSelect(ExpressionToOperation<T>.ToSelect(_tableName, lambdaSelect));
        }
        /// <summary>
        /// Select
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T> Select<T2, T3, T4>(Expression<Func<T, T2, T3, T4, object>> lambdaSelect)
        {
            return (Search<T>)AddSelect(ExpressionToOperation<T>.ToSelect(_tableName, lambdaSelect));
        }
        /// <summary>
        /// Select
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T> Select<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, object>> lambdaSelect)
        {
            return (Search<T>)AddSelect(ExpressionToOperation<T>.ToSelect(_tableName, lambdaSelect));
        }
        /// <summary>
        /// Select
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T> Select<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, object>> lambdaSelect)
        {
            return (Search<T>)AddSelect(ExpressionToOperation<T>.ToSelect(_tableName, lambdaSelect));
        }

        /// <summary>
        /// groupby
        /// </summary>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        public new Search<T> GroupBy(GroupByOperation groupBy)
        {
            return (Search<T>)base.GroupBy(groupBy);
        }
        /// <summary>
        /// groupby
        /// </summary>
        public new Search<T> GroupBy(params Field[] fields)
        {
            return (Search<T>)base.GroupBy(fields);
        }
        /// <summary>
        /// groupby
        /// </summary>
        /// <param name="lambdaGroupBy"></param>
        /// <returns></returns>
        public Search<T> GroupBy(Expression<Func<T, object>> lambdaGroupBy)
        {
            return (Search<T>)base.GroupBy(ExpressionToOperation<T>.ToGroupByClip(lambdaGroupBy));
        }
        /// <summary>
        /// groupby
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="lambdaGroupBy"></param>
        /// <returns></returns>
        public Search<T> GroupBy<T2>(Expression<Func<T, T2, object>> lambdaGroupBy)
        {
            return (Search<T>)base.GroupBy(ExpressionToOperation<T>.ToSelect(null, lambdaGroupBy));
        }
        /// <summary>
        /// groupby
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="lambdaGroupBy"></param>
        /// <returns></returns>
        public Search<T> GroupBy<T2, T3>(Expression<Func<T, T2, T3, object>> lambdaGroupBy)
        {
            return (Search<T>)base.GroupBy(ExpressionToOperation<T>.ToSelect(null, lambdaGroupBy));
        }
        /// <summary>
        /// groupby
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="lambdaGroupBy"></param>
        /// <returns></returns>
        public Search<T> GroupBy<T2, T3, T4>(Expression<Func<T, T2, T3, T4, object>> lambdaGroupBy)
        {
            return (Search<T>)base.GroupBy(ExpressionToOperation<T>.ToSelect(null, lambdaGroupBy));
        }
        /// <summary>
        /// groupby
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="lambdaGroupBy"></param>
        /// <returns></returns>
        public Search<T> GroupBy<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, object>> lambdaGroupBy)
        {
            return (Search<T>)base.GroupBy(ExpressionToOperation<T>.ToSelect(null, lambdaGroupBy));
        }
        /// <summary>
        /// groupby
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="lambdaGroupBy"></param>
        /// <returns></returns>
        public Search<T> GroupBy<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, object>> lambdaGroupBy)
        {
            return (Search<T>)base.GroupBy(ExpressionToOperation<T>.ToSelect(null, lambdaGroupBy));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public Search<T> OrderBy(params Field[] f)
        {
            var gb = f.Aggregate(OrderByOperation.None, (current, field) => current && field.Asc);
            return (Search<T>)base.OrderBy(gb);
        }
        /// <summary>
        /// OrderByDescending
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public Search<T> OrderByDescending(params Field[] f)
        {
            var gb = f.Aggregate(OrderByOperation.None, (current, field) => current && field.Desc);
            return (Search<T>)base.OrderBy(gb);
        }

        /// <summary>
        /// OrderBy
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public new Search<T> OrderBy(OrderByOperation orderBy)
        {
            return (Search<T>)base.OrderBy(orderBy);
        }

        /// <summary>
        /// OrderBy
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public Search<T> OrderBy(string fieldName, OrderByOperater orderBy)
        {
            return OrderBy(new OrderByOperation(fieldName, orderBy));
        }

        /// <summary>
        /// OrderBy
        /// </summary>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public Search<T> OrderBy(Dictionary<string, OrderByOperater> pairs)
        {
            var orderByOperations = new List<OrderByOperation>();
            if (pairs != null && pairs.Any())
            {
                foreach (var item in pairs)
                {
                    orderByOperations.Add(new OrderByOperation(item.Key, item.Value));
                }
            }
            return (Search<T>)base.OrderBy(orderByOperations.ToArray());
        }

        /// <summary>
        /// OrderBy
        /// </summary>
        /// <param name="lambdaOrderBys"></param>
        /// <returns></returns>
        public Search<T> OrderBy(params Expression<Func<T, object>>[] lambdaOrderBys)
        {
            var orderByOperations = new List<OrderByOperation>();
            if (lambdaOrderBys != null && lambdaOrderBys.Any())
            {
                foreach (var item in lambdaOrderBys)
                {
                    orderByOperations.Add(ExpressionToOperation<T>.ToOrderByClip(item));
                }
            }
            return (Search<T>)base.OrderBy(orderByOperations.ToArray());
        }
        /// <summary>
        /// OrderBy
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="lambdaOrderBys"></param>
        /// <returns></returns>
        public Search<T> OrderBy<T2>(params Expression<Func<T2, object>>[] lambdaOrderBys)
        {
            var orderByOperations = new List<OrderByOperation>();
            if (lambdaOrderBys != null && lambdaOrderBys.Any())
            {
                foreach (var item in lambdaOrderBys)
                {
                    orderByOperations.Add(ExpressionToOperation<T2>.ToOrderByClip(item));
                }
            }
            return (Search<T>)base.OrderBy(orderByOperations.ToArray());
        }

        /// <summary>
        /// OrderByDescending
        /// </summary>
        /// <param name="lambdaOrderBys"></param>
        /// <returns></returns>
        public Search<T> OrderByDescending(params Expression<Func<T, object>>[] lambdaOrderBys)
        {
            var orderByOperations = new List<OrderByOperation>();
            if (lambdaOrderBys != null && lambdaOrderBys.Any())
            {
                foreach (var item in lambdaOrderBys)
                {
                    orderByOperations.Add(ExpressionToOperation<T>.ToOrderByDescendingClip(item));
                }
            }
            return (Search<T>)base.OrderBy(orderByOperations.ToArray());
        }

        /// <summary>
        /// OrderByDescending
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="lambdaOrderBys"></param>
        /// <returns></returns>
        public Search<T> OrderByDescending<T2>(params Expression<Func<T2, object>>[] lambdaOrderBys)
        {
            var orderByOperations = new List<OrderByOperation>();
            if (lambdaOrderBys != null && lambdaOrderBys.Any())
            {
                foreach (var item in lambdaOrderBys)
                {
                    orderByOperations.Add(ExpressionToOperation<T2>.ToOrderByDescendingClip(item));
                }
            }
            return (Search<T>)base.OrderBy(orderByOperations.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderBys"></param>
        /// <returns></returns>
        public new Search<T> OrderBy(params OrderByOperation[] orderBys)
        {
            return (Search<T>)base.OrderBy(orderBys);
        }
        /// <summary>
        /// select field
        /// </summary>
        public new Search<T> Select(params Field[] fields)
        {
            return (Search<T>)base.Select(fields);
        }

        /// <summary>
        /// Select
        /// </summary>
        /// <param name="lambdaSelects"></param>
        /// <returns></returns>
        public Search<T> Select(params Expression<Func<T, object>>[] lambdaSelects)
        {
            return (Search<T>)base.Select(ExpressionToOperation<T>.ToSelect(_tableName, lambdaSelects));
        }

        /// <summary>
        /// Distinct
        /// </summary>
        /// <returns></returns>
        public new Search<T> Distinct()
        {
            return (Search<T>)base.Distinct();
        }

        /// <summary>
        /// Top
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public new Search<T> Top(int topCount = 1)
        {
            return From(1, topCount);
        }


        /// <summary>
        /// Page
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public new Search<T> Page(int pageIndex, int pageSize)
        {
            return From(pageSize * (pageIndex - 1) + 1, pageIndex * pageSize);
        }


        /// <summary>
        /// 设置默认排序
        /// </summary>
        private void SetDefaultOrderby()
        {
            if (!OrderByOperation.IsNullOrEmpty(this.OrderByClip)) return;
            if (_fields.Count > 0)
            {
                if (_fields.Any(f => f.PropertyName.Trim().Equals("*")))
                {
                    setPrimarykeyOrderby();
                }

            }
            else
            {
                setPrimarykeyOrderby();
            }
        }

        /// <summary>
        /// From  1-10
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public new Search<T> From(int startIndex, int endIndex)
        {
            if (startIndex > 1)
            {
                SetDefaultOrderby();
            }
            return (Search<T>)base.From(startIndex, endIndex);
        }


        /// <summary>
        /// 设置缓存有效期  单位：秒
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public new Search<T> SetCacheTimeOut(int timeout)
        {
            this.timeout = timeout;
            return this;
        }

        /// <summary>
        /// 重新加载
        /// </summary>
        /// <returns></returns>
        public new Search<T> Refresh()
        {
            isRefresh = true;
            return this;
        }


        /// <summary>
        /// select sql
        /// </summary>
        /// <param name="fromSection"></param>
        /// <returns></returns>
        public new Search<T> AddSelect(Search fromSection)
        {
            return AddSelect(fromSection, null);
        }

        /// <summary>
        /// select sql
        /// </summary>
        /// <param name="fromSection"></param>
        /// <param name="aliasName">别名</param>
        /// <returns></returns>
        public new Search<T> AddSelect(Search fromSection, string aliasName)
        {
            return (Search<T>)base.AddSelect(fromSection, aliasName);
        }

        #endregion

        #region 查询

        private readonly string[] _notClass = new string[] { "String" };

        /// <summary>
        /// ToFirstDefault
        /// </summary>
        /// <returns></returns>
        public T ToFirstDefault()
        {
            T t = this.ToFirst();
            if (t == null)
            {
                t = DataUtils.Create<T>();
                //t.SetTableName(_tableName);
            }
            return t;
        }

        /// <summary>
        /// 同ToFirstDefault， 返回第一个实体  如果为null，则默认实例化一个
        /// </summary>
        /// <returns></returns>
        public T FirstDefault()
        {
            return ToFirstDefault();
        }

        /// <summary>
        /// 返回第一个实体，同ToFirst()。无数据返回Null。
        /// </summary>
        /// <returns></returns>
        public T First()
        {
            return ToFirst();
        }
        /// <summary>
        /// 返回第一个实体，同ToFirst()。无数据返回Null。
        /// </summary>
        /// <returns></returns>
        public Model First<Model>() where Model : class
        {
            return ToFirst<Model>();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public T First(Expression<Func<T, bool>> lambdaWhere)
        {
            var where = new Where<T>(lambdaWhere);

            Search search = this.Top(1).GetPagedFromSection().Where(where.ToWhereClip());

            T t = null;

            using (IDataReader reader = ToDataReader(search))
            {
                var result = EntityUtils.ReaderToList<T>(reader);

                if (result.Any())
                {
                    t = result.First();
                }
            }

            if (t != null)
            {
                //t.SetTableName(_tableName);
                t.ClearModifyFields();
            }

            return t;
        }

        /// <summary>
        /// 返回第一个实体 ，同First()。无数据返回Null。
        /// </summary>
        /// <returns></returns>
        public Model ToFirst<Model>() where Model : class
        {
            var typet = typeof(Model);

            if (typet == typeof(T))
            {
                return ToFirst() as Model;
            }

            Search from = this.Top(1).GetPagedFromSection();

            Model t = null;

            using (IDataReader reader = ToDataReader(from))
            {
                var result = EntityUtils.ReaderToList<Model>(reader);

                if (result.Any())
                {
                    t = result.First();

                    if (t != null)
                    {
                        var st = t as Entity;

                        if (st != null)
                        {
                            st.ClearModifyFields();
                            //st.SetTableName(_tableName);
                        }
                    }
                }
            }

            return t;
        }
        /// <summary>
        /// 返回第一个实体 ，同First()。无数据返回Null。
        /// </summary>
        /// <returns></returns>
        public T ToFirst()
        {
            Search search = this.Top(1).GetPagedFromSection();

            T t = null;

            using (IDataReader reader = ToDataReader(search))
            {
                var result = EntityUtils.ReaderToList<T>(reader);

                if (result.Any())
                {
                    t = result.First();
                }
            }

            if (t != null)
            {
                //t.SetTableName(_tableName);
                t.ClearModifyFields();
            }

            return t;
        }

        /// <summary>
        /// Single,有则返回，无则null，多于一个则error
        /// </summary>
        /// <returns></returns>
        public T Single()
        {
            return ToSingle();
        }

        /// <summary>
        /// ToSingle,有则返回，无则null，多于一个则error
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T ToSingle()
        {
            Search search = this.Top(2).GetPagedFromSection();

            T t = null;

            using (IDataReader reader = ToDataReader(search))
            {
                var result = EntityUtils.ReaderToList<T>(reader);

                if (result.Any())
                {
                    if (result.Count > 1) throw new Exception("There are multiple records for the acquired data");

                    t = result.First();
                }
            }

            if (t != null)
            {
                //t.SetTableName(_tableName);
                t.ClearModifyFields();
            }

            return t;
        }

        /// <summary>
        /// 有则返回，无则null，多于一个则error
        /// </summary>
        /// <param name="expressionWhere"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T Single(Expression<Func<T, bool>> expressionWhere)
        {
            Search search = this.Top(2).GetPagedFromSection().Where(new Where<T>(expressionWhere).ToWhereClip());

            T t = null;

            using (IDataReader reader = ToDataReader(search))
            {
                var result = EntityUtils.ReaderToList<T>(reader);

                if (result.Any())
                {
                    if (result.Count > 1) throw new Exception("There are multiple records for the acquired data");

                    t = result.First();
                }
            }

            if (t != null)
            {
                //t.SetTableName(_tableName);
                t.ClearModifyFields();
            }

            return t;
        }

        /// <summary>
        /// Single,有则返回，无则null，多于一个则error
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <returns></returns>
        public Model Single<Model>() where Model : class
        {
            return ToSingle<Model>();
        }

        /// <summary>
        /// ToSingle,有则返回，无则null，多于一个则error
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <returns></returns>
        public Model ToSingle<Model>() where Model : class
        {
            var typet = typeof(Model);
            if (typet == typeof(T))
            {
                return ToSingle() as Model;
            }

            var search = this.Top(2).GetPagedFromSection();

            Model m = null;

            using (IDataReader reader = ToDataReader(search))
            {
                var result = EntityUtils.ReaderToList<Model>(reader);

                if (result.Any())
                {
                    if (result.Count > 1) throw new Exception("There are multiple records for the acquired data");

                    m = result.First();

                    if (m != null)
                    {
                        var st = m as Entity;

                        if (st != null)
                        {
                            st.ClearModifyFields();
                            //st.SetTableName(_tableName);
                        }
                    }
                }
            }

            return m;
        }


        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Model> ToList<Model>()
        {
            var typet = typeof(Model);

            if (typet == typeof(T))
            {
                return ToList() as List<Model>;
            }

            var from = GetPagedFromSection();

            if (typet.IsClass && !_notClass.Contains(typet.Name))
            {
                List<Model> result = null;
                using (var reader = ToDataReader(from))
                {
                    result = EntityUtils.ReaderToList<Model>(reader);
                }
                return result;
            }
            if (!@from.Fields.Any())
            {
                throw new Exception(".ToList<" + typet.Name + ">()至少需要.Select()一个字段！");
            }
            if (@from.Fields.Count > 1)
            {
                throw new Exception(".ToList<" + typet.Name + ">()最多.Select()一个字段！");
            }

            var list = new List<Model>();

            using (var reader = ToDataReader(@from))
            {
                while (reader.Read())
                {
                    var t = DataUtils.ConvertValue<Model>(reader[@from.Fields[0].Name]);

                    var st = t as Entity;

                    if (st != null)
                    {
                        st.ClearModifyFields();
                        //st.SetTableName(_tableName);
                    }

                    list.Add(t);
                }
                reader.Close();
            }

            return list;

        }
        /// <summary>
        /// To List&lt;T>
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            var from = GetPagedFromSection();

            List<T> list;
            using (var reader = ToDataReader(from))
            {
                list = EntityUtils.ReaderToList<T>(reader);
            }

            foreach (var m in list)
            {
                if (m != null)
                {
                    m.ClearModifyFields();
                    //m.SetTableName(_tableName);
                }
            }
            return list;
        }

        /// <summary>
        /// tolist
        /// </summary>
        /// <param name="expressionWhere"></param>
        /// <returns></returns>
        public List<T> ToList(Expression<Func<T, bool>> expressionWhere)
        {
            var from = GetPagedFromSection().Where(new Where<T>(expressionWhere).ToWhereClip());

            List<T> list;
            using (var reader = ToDataReader(from))
            {
                list = EntityUtils.ReaderToList<T>(reader).ToList();
            }

            foreach (var m in list)
            {
                if (m != null)
                {
                    m.ClearModifyFields();
                    //m.SetTableName(_tableName);
                }
            }
            return list;
        }

        /// <summary>
        /// 返回懒加载数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> ToEnumerable()
        {
            var from = GetPagedFromSection();

            using (var reader = ToDataReader(from))
            {
                var info = new EntityUtils.CacheInfo
                {
                    Deserializer = EntityUtils.GetDeserializer(typeof(T), reader, 0, -1, false)
                };
                while (reader.Read())
                {
                    dynamic next = info.Deserializer(reader);
                    yield return (T)next;
                }
            }
        }
        /// <summary>
        /// 返回第一个实体  如果为null，则默认实例化一个
        /// </summary>
        /// <returns></returns>
        #endregion

        #region Union

        /// <summary>
        /// Union
        /// </summary>
        /// <param name="fromSection"></param>
        /// <returns></returns>
        public new Search<T> Union(Search fromSection)
        {
            StringPlus tname = new StringPlus();

            tname.Append("(");

            tname.Append(this.SqlNoneOrderbyString);

            tname.Append(" UNION ");

            tname.Append(fromSection.SqlNoneOrderbyString);

            tname.Append(")");

            tname.Append(" ");

            tname.Append(EntityCache.GetTableName<T>());


            Search<T> tmpfromSection = new Search<T>(this._database);
            tmpfromSection._tableName = tname.ToString();

            tmpfromSection._parameters.AddRange(this.Parameters);
            tmpfromSection._parameters.AddRange(fromSection.Parameters);

            return tmpfromSection;
        }

        /// <summary>
        /// Union All
        /// </summary>        
        /// <param name="fromSection"></param>
        /// <returns></returns>
        public new Search<T> UnionAll(Search fromSection)
        {
            StringPlus tname = new StringPlus();

            tname.Append("(");

            tname.Append(this.SqlNoneOrderbyString);

            tname.Append(" UNION ALL ");

            tname.Append(fromSection.SqlNoneOrderbyString);

            tname.Append(")");

            tname.Append(" ");

            tname.Append(EntityCache.GetTableName<T>());

            Search<T> tmpfromSection = new Search<T>(this._database);
            tmpfromSection._tableName = tname.ToString();

            tmpfromSection._parameters.AddRange(this.Parameters);
            tmpfromSection._parameters.AddRange(fromSection.Parameters);

            return tmpfromSection;
        }

        #endregion

        #region 字典

        /// <summary>
        /// 转换成字典
        /// </summary>
        /// <param name="keyExpress"></param>
        /// <returns></returns>
        public Dictionary<dynamic, T> ToDictionary(Expression<Func<T, dynamic>> keyExpress)
        {
            var keyFiled = ExpressionToOperation<T>.ToSelect(_tableName, keyExpress).First();
            var list = ToList();
            if (list != null && list.Count > 0)
            {
                var result = new Dictionary<dynamic, T>();
                foreach (var item in list)
                {
                    var key = DataUtils.GetPropertyValue(item, keyFiled.Name);
                    if (key != null)
                        result.Add(key, item);
                }
                return result;
            }
            return null;
        }
        /// <summary>
        /// 转换成字典
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="keyExpress"></param>
        /// <returns></returns>
        public Dictionary<dynamic, Model> ToDictionary<Model>(Expression<Func<T, dynamic>> keyExpress)
        {
            var keyFiled = ExpressionToOperation<T>.ToSelect(_tableName, keyExpress).First();
            var list = ToList<Model>();
            if (list != null && list.Count > 0)
            {
                var result = new Dictionary<dynamic, Model>();
                foreach (var item in list)
                {
                    var key = DataUtils.GetPropertyValue(item, keyFiled.Name);
                    if (key != null)
                        result.Add(key, item);
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 转换成字典
        /// </summary>
        /// <param name="keyExpress"></param>
        /// <param name="valExpress"></param>
        /// <returns></returns>
        public Dictionary<dynamic, dynamic> ToDictionary(Expression<Func<T, dynamic>> keyExpress, Expression<Func<T, dynamic>> valExpress)
        {
            var keyFiled = ExpressionToOperation<T>.ToSelect(_tableName, keyExpress).First();
            var valFiled = ExpressionToOperation<T>.ToSelect(_tableName, valExpress).First();
            var list = ToList();
            if (list != null && list.Count > 0)
            {
                var result = new Dictionary<dynamic, dynamic>();
                foreach (var item in list)
                {
                    var key = DataUtils.GetPropertyValue(item, keyFiled.Name);
                    if (key != null)
                    {
                        var val = DataUtils.GetPropertyValue(item, valFiled.Name);
                        result.Add(key, val);
                    }
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 转换成字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="keyExpress"></param>
        /// <param name="valExpress"></param>
        /// <returns></returns>
        public Dictionary<TKey, TVal> ToDictionary<TKey, TVal>(Expression<Func<T, dynamic>> keyExpress, Expression<Func<T, dynamic>> valExpress)
        {
            var data = ToDictionary(keyExpress, valExpress);
            if (data == null || data.Count < 1) return null;
            var result = new Dictionary<TKey, TVal>();
            foreach (var item in data)
            {
                result.Add(item.Key, item.Value);
            }
            return result;
        }
        #endregion

        #region 分页列表
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<T> ToPagedList(int pageIndex, int pageSize, string order, bool asc)
        {
            var total = this.Count();

            if (!string.IsNullOrEmpty(TableName) && !string.IsNullOrEmpty(order) && order.IndexOf(".") == -1)
            {
                order = $"{TableName}.{order}";
            }

            var list = this.OrderBy(new OrderByOperation(order, asc ? OrderByOperater.ASC : OrderByOperater.DESC)).Page(pageIndex, pageSize).ToList<T>();

            return new PagedList<T>(list, pageIndex, pageSize, total);
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderLambada"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<T> ToPagedList(int pageIndex, int pageSize, Expression<Func<T, object>> orderLambada, bool asc = true)
        {
            var total = Count();

            OrderByOperation order;

            if (asc)
            {
                order = ExpressionToOperation<T>.ToOrderByClip(orderLambada);
            }
            else
            {
                order = ExpressionToOperation<T>.ToOrderByDescendingClip(orderLambada);
            }

            var list = OrderBy(order)
                .Page(pageIndex, pageSize)
                .ToList<T>();

            return new PagedList<T>(list, pageIndex, pageSize, total);
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<T> ToPagedList(Expression<Func<T, bool>> lambdaWhere, int pageIndex, int pageSize, string orderBy, bool asc)
        {
            var total = this.Where(lambdaWhere).Count();

            var list = this.Where(lambdaWhere).OrderBy(new OrderByOperation(orderBy, asc ? OrderByOperater.ASC : OrderByOperater.DESC)).Page(pageIndex, pageSize).ToList<T>();

            return new PagedList<T>(list, pageIndex, pageSize, total);
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<T> ToPagedList(Where where, int pageIndex, int pageSize, string orderBy, bool asc)
        {
            var total = this.Where(where).Count();

            var list = this.Where(where).OrderBy(new OrderByOperation(orderBy, asc ? OrderByOperater.ASC : OrderByOperater.DESC)).Page(pageIndex, pageSize).ToList<T>();

            return new PagedList<T>(list, pageIndex, pageSize, total);
        }


        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<Model> ToPagedList<Model>(int pageIndex, int pageSize, string order, bool asc)
        {
            var total = this.Count();

            var list = this.OrderBy(new OrderByOperation(order, asc ? OrderByOperater.ASC : OrderByOperater.DESC)).Page(pageIndex, pageSize).ToList<Model>();

            return new PagedList<Model>(list, pageIndex, pageSize, total);
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<Model> ToPagedList<Model>(Expression<Func<T, bool>> lambdaWhere, int pageIndex, int pageSize, string orderBy, bool asc)
        {
            var total = this.Where(lambdaWhere).Count();

            var list = this.Where(lambdaWhere).OrderBy(new OrderByOperation(orderBy, asc ? OrderByOperater.ASC : OrderByOperater.DESC)).Page(pageIndex, pageSize).ToList<Model>();

            return new PagedList<Model>(list, pageIndex, pageSize, total);
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<Model> ToPagedList<Model>(Where where, int pageIndex, int pageSize, string orderBy, bool asc)
        {
            var total = this.Where(where).Count();

            var list = this.Where(where).OrderBy(new OrderByOperation(orderBy, asc ? OrderByOperater.ASC : OrderByOperater.DESC)).Page(pageIndex, pageSize).ToList<Model>();

            return new PagedList<Model>(list, pageIndex, pageSize, total);
        }

        #endregion


    }
}
