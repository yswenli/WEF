/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2019
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：1a0dd623-eae1-428c-8095-d971d079c8ab
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：WEF
 * 命名空间：WEF
 * 类名称：ISearch
 * 创建时间：2017/7/26 15:31:40
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System.Collections.Generic;
using System.Data;

using WEF.Common;
using WEF.Db;
using WEF.Expressions;
using WEF.Provider;

namespace WEF
{
    /// <summary>
    /// DBContext的查询对象
    /// </summary>
    public interface ISearch
    {
        string ColumnsString { get; }
        string CountSqlString { get; }
        Database Database { get; }
        DbProvider DbProvider { get; }
        List<Field> Fields { get; }
        GroupByOperation GroupByClip { get; }
        string GroupByString { get; }
        string LimitString { get; set; }
        OrderByOperation OrderByClip { get; }
        string OrderByString { get; }
        List<Parameter> Parameters { get; }
        string SqlString { get; }
        string TableName { get; }

        Search AddSelect(Search Search);
        Search AddSelect(Search Search, string aliasName);
        int Count();
        Search CrossJoin(string tableName, WhereOperation where, string userName = null);
        Search Distinct();
        Search From(int startIndex, int endIndex);
        Search FullJoin(string tableName, WhereOperation where, string userName = null);
        WhereOperation GetWhereClip();
        Search GroupBy(GroupByOperation groupBy);
        Search GroupBy(params Field[] fields);
        Search Having(WhereOperation havingWhere);
        Search InnerJoin(string tableName, WhereOperation where, string userName = null);
        Search LeftJoin(string tableName, WhereOperation where, string userName = null);
        Search OrderBy(OrderByOperation orderBy);
        Search OrderBy(params OrderByOperation[] orderBys);
        Search OrderBy(IEnumerable<string> asc, IEnumerable<string> desc);
        Search Page(int pageSize, int pageIndex);
        Search Refresh();
        Search RightJoin(string tableName, WhereOperation where, string userName = null);
        Search Select(params Field[] fields);
        Search SetCacheTimeOut(int timeout);
        IDataReader ToDataReader();
        DataSet ToDataSet();
        DataTable ToDataTable();
        Search Top(int topCount);
        object ToScalar();
        TResult ToScalar<TResult>();
        Search Union(Search Search);
        Search UnionAll(Search Search);
        Search Where(WhereOperation where);
    }
}