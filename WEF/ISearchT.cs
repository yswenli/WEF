/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2017
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：d3e81a30-b46f-42d9-9a36-b518a9216b8b
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：WEF
 * 类名称：ISearchT
 * 创建时间：2017/7/27 15:14:23
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Web.Caching;
using WEF;
using WEF.Common;
using WEF.Db;
using WEF.Expressions;
using WEF.MvcPager;
using WEF.Provider;

namespace WEF
{
    public interface ISearch<T> where T : Entity
    {
        ISearch<T> AddSelect(ISearch fromSection);
        ISearch<T> AddSelect(ISearch fromSection, string aliasName);
        ISearch<T> CrossJoin<TEntity>(WhereClip where) where TEntity : Entity;

        int Count();

        WhereClip GetWhereClip();

        DataSet ToDataSet();
        DataTable ToDataTable();

        ISearch<T> Distinct();
        T First();
        T FirstDefault();
        TResult First<TResult>() where TResult : class;
        ISearch<T> From(int startIndex, int endIndex);
        ISearch<T> FullJoin<TEntity>(WhereClip where) where TEntity : Entity;
        ISearch<T> GroupBy(params Field[] fields);
        ISearch<T> GroupBy(GroupByClip groupBy);
        ISearch<T> GroupBy(Expression<Func<T, object>> lambdaGroupBy);
        ISearch<T> Having(WhereClip havingWhere);
        ISearch<T> Having(Where where);
        ISearch<T> Having(Expression<Func<T, bool>> lambdaHaving);
        ISearch<T> InnerJoin<TEntity>(Expression<Func<T, TEntity, bool>> lambdaWhere, string asName = "") where TEntity : Entity;
        ISearch<T> InnerJoin<TEntity>(WhereClip where, string asName = "", string asName2 = "") where TEntity : Entity;
        ISearch<T> LeftJoin<TEntity>(WhereClip where) where TEntity : Entity;
        ISearch<T> LeftJoin<TEntity>(Expression<Func<T, TEntity, bool>> lambdaWhere) where TEntity : Entity;
        ISearch<T> OrderBy(params OrderByClip[] orderBys);
        ISearch<T> OrderBy(params Field[] f);
        ISearch<T> OrderBy(OrderByClip orderBy);
        ISearch<T> OrderBy(Expression<Func<T, object>> lambdaOrderBy);
        ISearch<T> OrderByDescending(params Field[] f);
        ISearch<T> OrderByDescending(Expression<Func<T, object>> lambdaOrderBy);
        ISearch<T> Page(int pageIndex,int pageSize);

        PagedList<T> GetPagedList(int pageIndex, int pageSize, string order, bool asc);

        ISearch<T> Refresh();
        ISearch<T> RightJoin<TEntity>(WhereClip where) where TEntity : Entity;
        ISearch<T> Select(params Field[] fields);
        ISearch<T> Select(Expression<Func<T, object>> lambdaSelect);
        ISearch<T> Select(Expression<Func<T, bool>> lambdaSelect);
        ISearch<T> Select<T2>(Expression<Func<T, T2, object>> lambdaSelect);
        ISearch<T> Select<T2>(Expression<Func<T, T2, bool>> lambdaWhere);
        ISearch<T> Select<T2, T3>(Expression<Func<T, T2, T3, object>> lambdaSelect);
        ISearch<T> Select<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere);
        ISearch<T> Select<T2, T3, T4>(Expression<Func<T, T2, T3, T4, object>> lambdaSelect);
        ISearch<T> Select<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere);
        ISearch<T> Select<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, object>> lambdaSelect);
        ISearch<T> Select<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere);
        ISearch<T> Select<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, object>> lambdaSelect);
        ISearch<T> Select<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere);
        ISearch<T> SetCacheDependency(CacheDependency dep);
        ISearch<T> SetCacheTimeOut(int timeout);
        IEnumerable<T> ToEnumerable();
        T ToFirst();
        TResult ToFirst<TResult>() where TResult : class;
        T ToFirstDefault();
        List<T> ToList();
        List<TResult> ToList<TResult>();
        ISearch<T> Top(int topCount);
        ISearch<T> Union(ISearch fromSection);
        ISearch<T> UnionAll(ISearch fromSection);
        ISearch<T> Where(Where<T> whereParam);
        ISearch<T> Where(WhereClip where);
        ISearch<T> Where(Where whereParam);
        ISearch<T> Where(Expression<Func<T, bool>> lambdaWhere);
        ISearch<T> Where<T2>(Expression<Func<T, T2, bool>> lambdaWhere);
        ISearch<T> Where<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere);
        ISearch<T> Where<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere);
        ISearch<T> Where<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere);
        ISearch<T> Where<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere);
        ISearch GetPagedFromSection();

        object ToScalar();

        TResult ToScalar<TResult>();

        IDataReader ToDataReader();
        IDataReader ToDataReader(ISearch from);
    }
}
