using System.Collections.Generic;
using System.Data;
using System.Web.Caching;
using WEF.Common;
using WEF.Db;
using WEF.Expressions;
using WEF.Provider;

namespace WEF
{
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
        Search Page(int pageSize, int pageIndex);
        Search Refresh();
        Search RightJoin(string tableName, WhereOperation where, string userName = null);
        Search Select(params Field[] fields);
        Search SetCacheDependency(CacheDependency dep);
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