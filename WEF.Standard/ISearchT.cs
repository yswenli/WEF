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
 * 创建说明：ISearchT
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using WEF.Common;
using WEF.Expressions;
using WEF.MvcPager;

namespace WEF
{
    /// <summary>
    /// ISearchT
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISearch<T> where T : Entity
    {
        Search<T> AddSelect(Search fromSection);
        Search<T> AddSelect(Search fromSection, string aliasName);
        int Count();
        int Count(Expression<Func<T, object>> lambdaSelect);
        Search<T> CrossJoin<TEntity>(WhereOperation where) where TEntity : Entity;
        Search<T> Distinct();
        T First();
        Model First<Model>() where Model : class;
        T FirstDefault();
        Search<T> From(int startIndex, int endIndex);
        Search<T> FullJoin<TEntity>(WhereOperation where) where TEntity : Entity;
        Search<T> GroupBy(Expression<Func<T, object>> lambdaGroupBy);
        Search<T> GroupBy(GroupByOperation groupBy);
        Search<T> GroupBy(params Field[] fields);
        Search<T> Having(Expression<Func<T, bool>> lambdaHaving);
        Search<T> Having(Where where);
        Search<T> Having(WhereOperation havingWhere);
        Search<T> InnerJoin(Search fs);
        Search<T> InnerJoin<TEntity>(Expression<Func<T, TEntity, bool>> lambdaWhere, string asName = "") where TEntity : Entity;
        Search<T> InnerJoin<TEntity>(WhereOperation where, string asName = "", string asName2 = "") where TEntity : Entity;
        Search<T> LeftJoin<TEntity>(Expression<Func<T, TEntity, bool>> lambdaWhere) where TEntity : Entity;
        Search<T> LeftJoin<TEntity>(WhereOperation where) where TEntity : Entity;
        Search<T> OrderBy(Expression<Func<T, object>> lambdaOrderBy);
        Search<T> OrderBy(IEnumerable<string> asc, IEnumerable<string> desc);
        Search<T> OrderBy(OrderByOperation orderBy);
        Search<T> OrderBy(params Field[] f);
        Search<T> OrderBy(params OrderByOperation[] orderBys);
        Search<T> OrderByDescending(Expression<Func<T, object>> lambdaOrderBy);
        Search<T> OrderByDescending(params Field[] f);
        Search<T> Page(int pageIndex, int pageSize);
        Search<T> Refresh();
        Search<T> RightJoin<TEntity>(WhereOperation where) where TEntity : Entity;
        Search<T> Select(Expression<Func<T, bool>> lambdaSelect);
        Search<T> Select(params Expression<Func<T, object>>[] lambdaSelects);
        Search<T> Select(params Field[] fields);
        Search<T> Select<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere);
        Search<T> Select<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, object>> lambdaSelect);
        Search<T> Select<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere);
        Search<T> Select<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, object>> lambdaSelect);
        Search<T> Select<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere);
        Search<T> Select<T2, T3, T4>(Expression<Func<T, T2, T3, T4, object>> lambdaSelect);
        Search<T> Select<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere);
        Search<T> Select<T2, T3>(Expression<Func<T, T2, T3, object>> lambdaSelect);
        Search<T> Select<T2>(Expression<Func<T, T2, bool>> lambdaWhere);
        Search<T> Select<T2>(Expression<Func<T, T2, object>> lambdaSelect);
        Search<T> SetCacheTimeOut(int timeout);
        IEnumerable<T> ToEnumerable();
        T ToFirst();
        Model ToFirst<Model>() where Model : class;
        T ToFirstDefault();
        List<T> ToList();
        List<Model> ToList<Model>();
        Search<T> Top(int topCount);
        PagedList<T> ToPagedList(Expression<Func<T, bool>> lambdaWhere, int pageIndex, int pageSize, string orderBy, bool asc);
        PagedList<T> ToPagedList(int pageIndex, int pageSize, string order, bool asc);
        PagedList<T> ToPagedList(Where where, int pageIndex, int pageSize, string orderBy, bool asc);
        PagedList<Model> ToPagedList<Model>(Expression<Func<T, bool>> lambdaWhere, int pageIndex, int pageSize, string orderBy, bool asc);
        PagedList<Model> ToPagedList<Model>(int pageIndex, int pageSize, string order, bool asc);
        PagedList<Model> ToPagedList<Model>(Where where, int pageIndex, int pageSize, string orderBy, bool asc);
        Search<T> Union(Search fromSection);
        Search<T> UnionAll(Search fromSection);
        Search<T> Where(params Expression<Func<T, bool>>[] lambdaWheres);
        Search<T> Where(Where whereParam);
        Search<T> Where(Where<T> whereParam);
        Search<T> Where(WhereOperation where);
        Search<T> Where<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere);
        Search<T> Where<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere);
        Search<T> Where<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere);
        Search<T> Where<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere);
        Search<T> Where<T2>(Expression<Func<T, T2, bool>> lambdaWhere);
    }
}