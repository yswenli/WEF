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
        string ColumnsString
        {
            get;
        }
        string CountSqlString
        {
            get;
        }
        Database Database
        {
            get;
        }
        DbProvider DbProvider
        {
            get;
        }
        List<Field> Fields
        {
            get;
        }
        GroupByClip GroupByClip
        {
            get;
        }
        string GroupByString
        {
            get;
        }
        string LimitString
        {
            get;
            set;
        }
        OrderByClip OrderByClip
        {
            get;
        }
        string OrderByString
        {
            get;
        }
        List<Parameter> Parameters
        {
            get;
        }
        string SqlString
        {
            get;
        }
        string TableName
        {
            get;
        }

        string SqlNoneOrderbyString
        {
            get;
        }

        string TypeTableName
        {
            get;
            set;
        }

        int? Timeout
        {
            get;
            set;
        }

        CacheDependency CacheDep
        {
            get;
            set;
        }

        bool IsRefresh
        {
            get;
            set;
        }

        ISearch AddSelect(ISearch fromSection);
        ISearch AddSelect(ISearch fromSection, string aliasName);
        int Count();
        ISearch CrossJoin(string tableName, WhereClip where, string userName = null);
        ISearch Distinct();
        ISearch From(int startIndex, int endIndex);
        ISearch FullJoin(string tableName, WhereClip where, string userName = null);
        ISearch GetPagedFromSection();
        WhereClip GetWhereClip();
        ISearch GroupBy(params Field[] fields);
        ISearch GroupBy(GroupByClip groupBy);
        ISearch Having(WhereClip havingWhere);
        ISearch InnerJoin(string tableName, WhereClip where, string userName = null);
        ISearch LeftJoin(string tableName, WhereClip where, string userName = null);
        ISearch OrderBy(params OrderByClip[] orderBys);
        ISearch OrderBy(OrderByClip orderBy);
        ISearch Page(int pageIndex, int pageSize);
        ISearch Refresh();
        ISearch RightJoin(string tableName, WhereClip where, string userName = null);
        ISearch Select(params Field[] fields);
        ISearch SetCacheDependency(CacheDependency dep);
        ISearch SetCacheTimeOut(int timeout);
        IDataReader ToDataReader();
        IDataReader ToDataReader(ISearch from);
        DataSet ToDataSet();
        DataTable ToDataTable();
        ISearch Top(int topCount);
        object ToScalar();
        TResult ToScalar<TResult>();
        ISearch Union(ISearch fromSection);
        ISearch UnionAll(ISearch fromSection);
        ISearch Where(WhereClip where);
    }
}