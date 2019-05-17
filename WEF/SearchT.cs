/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2019
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
using System.Text;
using System.Web.Caching;
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
    public class Search<T> : Search, ISearch<T> where T : Entity
    {

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
            : base(database, database.DbProvider.BuildTableName(EntityCache.GetTableName<T>(), EntityCache.GetUserName<T>()), asName, trans)
        {

        }

        #region 连接  Join
        public Search<T> InnerJoin(Search fs)
        {
            return Join(EntityCache.GetTableName<T>(), EntityCache.GetUserName<T>(), where, JoinType.InnerJoin);
        }
        /// <summary>
        /// Inner Join。Lambda写法：.InnerJoin&lt;Model2>((a, b) => a.ID == b.ID)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <param name="asName"></param>
        /// <param name="asName2"></param>
        /// <returns></returns>
        public Search<T> InnerJoin<TEntity>(WhereOperation where, string asName = "", string asName2 = "")
             where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where, JoinType.InnerJoin);
        }
        /// <summary>
        /// Inner Join。Lambda写法：.InnerJoin&lt;Model2>((a, b) => a.ID == b.ID)
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
        public Search<T> CrossJoin<TEntity>(WhereOperation where)
            where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where, JoinType.CrossJoin);
        }
        /// <summary>
        /// Right Join
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search<T> RightJoin<TEntity>(WhereOperation where)
            where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where, JoinType.RightJoin);
        }

        /// <summary>
        /// Left Join。经典写法：Model1._.ID == Model2._.ID
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search<T> LeftJoin<TEntity>(WhereOperation where)
             where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where, JoinType.LeftJoin);
        }
        /// <summary>
        /// Left Join。Lambda写法：.LeftJoin&lt;Model2>((d1,d2) => d1.ID == d2.ID)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public Search<T> LeftJoin<TEntity>(Expression<Func<T, TEntity, bool>> lambdaWhere)
             where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), ExpressionToOperation<T>.ToJoinWhere(lambdaWhere), JoinType.LeftJoin);
        }
        /// <summary>
        /// Full Join
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search<T> FullJoin<TEntity>(WhereOperation where)
            where TEntity : Entity
        {
            return Join(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where, JoinType.FullJoin);
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <param name="joinType"></param>
        /// <returns></returns>
        private Search<T> Join(string tableName, string userName, WhereOperation where, JoinType joinType)
        {
            return (Search<T>)base.join(tableName, userName, where, joinType);
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
        public new Search<T> Having(WhereOperation havingWhere)
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
        /// whereclip
        /// </summary>
        public new Search<T> Where(WhereOperation where)
        {
            return (Search<T>)base.Where(where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        public Search<T> Where(Where<T> whereParam)
        {
            return (Search<T>)base.Where(whereParam.ToWhereClip());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereParam"></param>
        /// <returns></returns>
        public Search<T> Where(Where whereParam)
        {
            return (Search<T>)base.Where(whereParam.ToWhereClip());
        }
        /// <summary>
        /// 
        /// </summary>
        //public Search<T> Where(Expression<Func<T, bool>> lambdaWhere)
        //{
        //    return Where(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        //}

        public Search<T> Where(params Expression<Func<T, bool>>[] lambdaWheres)
        {
            if (lambdaWheres == null || !lambdaWheres.Any()) throw new Exception("where条件不能为空");

            WhereBuilder whereBuilder = null;

            foreach (var item in lambdaWheres)
            {
                if (item == null) continue;

                if (whereBuilder == null)
                {
                    whereBuilder = new WhereBuilder(ExpressionToOperation<T>.ToWhereOperation(item));
                }
                else
                    whereBuilder.And(ExpressionToOperation<T>.ToWhereOperation(item));
            }

            return Where(whereBuilder.ToWhereClip());

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
        /// 
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public Search<T> Select<T2>(Expression<Func<T, T2, bool>> lambdaWhere)
        {
            return Where(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <returns></returns>
        public Search<T> Select<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere)
        {
            return Where(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <returns></returns>
        public Search<T> Select<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere)
        {
            return Where(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        public Search<T> Select<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere)
        {
            return Where(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <returns></returns>
        public Search<T> Select<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere)
        {
            return Where(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// groupby
        /// </summary>
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
        public Search<T> GroupBy(Expression<Func<T, object>> lambdaGroupBy)//new 
        {
            return (Search<T>)base.GroupBy(ExpressionToOperation<T>.ToGroupByClip(lambdaGroupBy));
        }
        #region 2015-09-08新增
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
        /// 
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public Search<T> OrderByDescending(params Field[] f)
        {
            var gb = f.Aggregate(OrderByOperation.None, (current, field) => current && field.Desc);
            return (Search<T>)base.OrderBy(gb);
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public new Search<T> OrderBy(OrderByOperation orderBy)
        {
            return (Search<T>)base.OrderBy(orderBy);
        }
        /// <summary>
        /// orderby
        /// </summary>
        public Search<T> OrderBy(Expression<Func<T, object>> lambdaOrderBy)//new 
        {
            return (Search<T>)base.OrderBy(ExpressionToOperation<T>.ToOrderByClip(lambdaOrderBy));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lambdaOrderBy"></param>
        /// <returns></returns>
        public Search<T> OrderByDescending(Expression<Func<T, object>> lambdaOrderBy)
        {
            return (Search<T>)base.OrderBy(ExpressionToOperation<T>.ToOrderByDescendingClip(lambdaOrderBy));
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
        /// 
        /// </summary>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T> Select(Expression<Func<T, object>> lambdaSelect)
        {
            return (Search<T>)base.Select(ExpressionToOperation<T>.ToSelect(lambdaSelect));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T> Select<T2>(Expression<Func<T, T2, object>> lambdaSelect)
        {
            return (Search<T>)base.Select(ExpressionToOperation<T>.ToSelect(lambdaSelect));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T> Select<T2, T3>(Expression<Func<T, T2, T3, object>> lambdaSelect)
        {
            return (Search<T>)base.Select(ExpressionToOperation<T>.ToSelect(lambdaSelect));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T> Select<T2, T3, T4>(Expression<Func<T, T2, T3, T4, object>> lambdaSelect)
        {
            return (Search<T>)base.Select(ExpressionToOperation<T>.ToSelect(lambdaSelect));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T> Select<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, object>> lambdaSelect)
        {
            return (Search<T>)base.Select(ExpressionToOperation<T>.ToSelect(lambdaSelect));
        }
        /// <summary>
        /// 
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
            return (Search<T>)base.Select(ExpressionToOperation<T>.ToSelect(lambdaSelect));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T> Select(Expression<Func<T, bool>> lambdaSelect)
        {
            return (Search<T>)base.Select(ExpressionToOperation<T>.ToSelect(lambdaSelect));
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
        public new Search<T> Top(int topCount)
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
            if (fields.Count > 0)
            {
                if (fields.Any(f => f.PropertyName.Trim().Equals("*")))
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
        /// 设置缓存依赖
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public new Search<T> SetCacheDependency(CacheDependency dep)
        {
            this.cacheDep = dep;
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
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public List<TResult> ToList<TResult>()
        {
            var typet = typeof(TResult);
            if (typet == typeof(T))
            {
                return ToList() as List<TResult>;
            }
            var from = GetPagedFromSection();
            if (typet.IsClass && !_notClass.Contains(typet.Name))
            {
                string cacheKey = String.Format("{0}List|{1}", dbProvider.ConnectionStringsName,
                    formatSql(@from.SqlString, @from));
                var cacheValue = getCache(cacheKey);

                if (null != cacheValue)
                {
                    return (List<TResult>)cacheValue;
                }
                List<TResult> list;
                using (var reader = ToDataReader(from))
                {
                    list = EntityUtils.ReaderToEnumerable<TResult>(reader).ToList();
                    reader.Close();
                }
                setCache(list, cacheKey);
                return list;
            }
            if (!@from.Fields.Any())
            {
                throw new Exception(".ToList<" + typet.Name + ">()至少需要.Select()一个字段！");
            }
            if (@from.Fields.Count > 1)
            {
                throw new Exception(".ToList<" + typet.Name + ">()最多.Select()一个字段！");
            }
            {
                var cacheKey = string.Concat(dbProvider.ConnectionStringsName, "List", "|",
                    formatSql(@from.SqlString, @from));
                var cacheValue = getCache(cacheKey);

                if (null != cacheValue)
                {
                    return (List<TResult>)cacheValue;
                }
                var list = new List<TResult>();
                using (var reader = ToDataReader(@from))
                {
                    while (reader.Read())
                    {
                        list.Add(DataUtils.ConvertValue<TResult>(reader[@from.Fields[0].Name]));
                    }
                    reader.Close();
                }
                setCache(list, cacheKey);
                return list;
            }
        }
        /// <summary>
        /// To List&lt;T>
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            var from = GetPagedFromSection();
            string cacheKey = String.Format("{0}List|{1}", dbProvider.ConnectionStringsName,
                formatSql(@from.SqlString, @from));
            var cacheValue = getCache(cacheKey);
            if (null != cacheValue)
            {
                return (List<T>)cacheValue;
            }
            List<T> list;
            using (var reader = ToDataReader(from))
            {
                list = EntityUtils.ReaderToEnumerable<T>(reader).ToList();
            }
            setCache(list, cacheKey);
            //2015-09-08
            foreach (var m in list)
            {
                m.ClearModifyFields();
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
        public T ToFirstDefault()
        {
            T t = this.ToFirst();
            if (t == null)
                t = DataUtils.Create<T>();
            return t;
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
        public TResult First<TResult>() where TResult : class
        {
            return ToFirst<TResult>();
        }
        /// <summary>
        /// 返回第一个实体 ，同First()。无数据返回Null。
        /// </summary>
        /// <returns></returns>
        public TResult ToFirst<TResult>() where TResult : class
        {
            var typet = typeof(TResult);
            if (typet == typeof(T))
            {
                return ToFirst() as TResult;
            }
            Search from = this.Top(1).GetPagedFromSection();

            string cacheKey = string.Format("{0}FirstT|{1}", dbProvider.ConnectionStringsName, formatSql(from.SqlString, from));
            object cacheValue = getCache(cacheKey);

            if (null != cacheValue)
            {
                return (TResult)cacheValue;
            }


            TResult t = null;
            using (IDataReader reader = ToDataReader(from))
            {
                var result = EntityUtils.ReaderToEnumerable<TResult>(reader).ToArray();
                if (result.Any())
                {
                    t = result.First();
                }
            }

            setCache<TResult>(t, cacheKey);
            return t;
        }
        /// <summary>
        /// 返回第一个实体 ，同First()。无数据返回Null。
        /// </summary>
        /// <returns></returns>
        public T ToFirst()
        {
            Search search = this.Top(1).GetPagedFromSection();

            string cacheKey = string.Format("{0}FirstT|{1}", dbProvider.ConnectionStringsName, formatSql(search.SqlString, search));
            object cacheValue = getCache(cacheKey);

            if (null != cacheValue)
            {
                return (T)cacheValue;
            }


            T t = null;
            using (IDataReader reader = ToDataReader(search))
            {
                var result = EntityUtils.ReaderToEnumerable<T>(reader).ToArray();
                if (result.Any())
                {
                    t = result.First();
                }
            }

            setCache<T>(t, cacheKey);

            if (t != null)
            {
                t.ClearModifyFields();
            }
            return t;
        }

        #endregion

        #region Union

        /// <summary>
        /// Union
        /// </summary>
        /// <param name="fromSection"></param>
        /// <returns></returns>
        public new Search<T> Union(Search fromSection)
        {
            StringBuilder tname = new StringBuilder();

            tname.Append("(");

            tname.Append(this.SqlNoneOrderbyString);

            tname.Append(" UNION ");

            tname.Append(fromSection.SqlNoneOrderbyString);

            tname.Append(")");

            tname.Append(" ");

            tname.Append(EntityCache.GetTableName<T>());


            Search<T> tmpfromSection = new Search<T>(this.database);
            tmpfromSection.tableName = tname.ToString();

            tmpfromSection.parameters.AddRange(this.Parameters);
            tmpfromSection.parameters.AddRange(fromSection.Parameters);

            return tmpfromSection;
        }

        /// <summary>
        /// Union All
        /// </summary>        
        /// <param name="fromSection"></param>
        /// <returns></returns>
        public new Search<T> UnionAll(Search fromSection)
        {
            StringBuilder tname = new StringBuilder();

            tname.Append("(");

            tname.Append(this.SqlNoneOrderbyString);

            tname.Append(" UNION ALL ");

            tname.Append(fromSection.SqlNoneOrderbyString);

            tname.Append(")");

            tname.Append(" ");

            tname.Append(EntityCache.GetTableName<T>());

            Search<T> tmpfromSection = new Search<T>(this.database);
            tmpfromSection.tableName = tname.ToString();

            tmpfromSection.parameters.AddRange(this.Parameters);
            tmpfromSection.parameters.AddRange(fromSection.Parameters);

            return tmpfromSection;
        }

        #endregion


        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<T> GetPagedList(int pageIndex, int pageSize, string order, bool asc)
        {
            var total = this.Count();
            var list = this.OrderBy(new OrderByOperation(order, asc ? OrderByOperater.ASC : OrderByOperater.DESC)).Page(pageIndex, pageSize).ToList<T>();
            var pagedlist = new PagedList<T>(pageIndex, pageSize, total);
            pagedlist.AddRange(list);
            return pagedlist;
        }

    }
}
