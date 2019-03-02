/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2017
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：1a0dd623-eae1-428c-8095-d971d079c8ab
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：WEF.Section
 * 类名称：FromSection
 * 创建时间：2017/7/26 15:31:40
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web.Caching;
using WEF.Common;
using WEF.Db;
using WEF.Expressions;
using WEF.Provider;

namespace WEF
{
    /// <summary>
    /// 查询
    /// </summary>    
    public class Search : ISearch
    {
        #region 变量
        /// <summary>
        /// 
        /// </summary>
        protected WhereClip where = WhereClip.All;
        /// <summary>
        /// 
        /// </summary>
        protected WhereClip havingWhere = WhereClip.All;
        /// <summary>
        /// 
        /// </summary>
        protected OrderByClip orderBy = OrderByClip.None;
        /// <summary>
        /// 
        /// </summary>
        protected GroupByClip groupBy = GroupByClip.None;
        /// <summary>
        /// 
        /// </summary>
        protected string tableName;
        ///// <summary>
        ///// 
        ///// </summary>
        protected string asName;
        /// <summary>
        /// 
        /// </summary>
        //public List<string> asNames = new List<string>();//2016-05-10新增
        /// <summary>
        /// 
        /// </summary>
        protected List<Parameter> parameters = new List<Parameter>();
        /// <summary>
        /// 
        /// </summary>
        protected List<Field> fields = new List<Field>();
        /// <summary>
        /// 
        /// </summary>
        protected DbProvider dbProvider;
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, KeyValuePair<string, WhereClip>> joins = new Dictionary<string, KeyValuePair<string, WhereClip>>();
        /// <summary>
        /// 
        /// </summary>
        protected Database database;
        /// <summary>
        /// 
        /// </summary>
        protected string distinctString;
        /// <summary>
        /// 
        /// </summary>
        protected string prefixString;



        /// <summary>
        /// 开始项
        /// </summary>
        protected int startIndex;
        /// <summary>
        /// 结束项
        /// </summary>
        protected int endIndex;


        /// <summary>
        /// 缓存超时时间
        /// </summary>
        protected int? timeout;


        /// <summary>
        /// 缓存依赖
        /// </summary>
        protected CacheDependency cacheDep = null;


        /// <summary>
        /// 
        /// </summary>
        protected string typeTableName;

        /// <summary>
        /// 是否重新加载
        /// </summary>
        protected bool isRefresh = false;

        /// <summary>
        /// 是否已经执行过分页
        /// </summary>
        protected bool isPageFromSection = false;

        /// <summary>
        /// 事务   -- 查询
        /// </summary>
        protected DbTransaction trans;

        /// <summary>
        /// 
        /// </summary>
        protected int Identity
        {
            get; set;
        }
        #endregion

        #region 属性
        //2015-08-12恢复注释
        /// <summary>
        /// DbProvider。
        /// </summary>
        public DbProvider DbProvider
        {
            get
            {
                return dbProvider;
            }
        }
        //2015-08-12新增
        /// <summary>
        /// DbProvider。
        /// </summary>
        public Database Database
        {
            get
            {
                return database;
            }
        }
        /// <summary>
        /// 设置 distinct
        /// </summary>
        internal string DistinctString
        {
            set
            {
                distinctString = value;
            }
        }

        /// <summary>
        /// 前置值如 Top N
        /// </summary>
        internal string PrefixString
        {
            set
            {
                prefixString = value;
            }
        }
        /// <summary>
        /// limit
        /// </summary>
        private string _limitString;
        /// <summary>
        /// limit 
        /// </summary>
        public string LimitString
        {
            set
            {
                _limitString = value;
            }
            get
            {
                return _limitString;
            }
        }

        /// <summary>
        /// 记录数sql语句 count
        /// </summary>
        public string CountSqlString
        {
            get
            {
                StringBuilder sql = new StringBuilder();

                if (GroupByClip.IsNullOrEmpty(groupBy) && string.IsNullOrEmpty(distinctString))
                {
                    sql.Append(" SELECT count(*) as r_cnt FROM ");
                    sql.Append(FromString);
                    if (!WhereClip.IsNullOrEmpty(where))
                    {
                        sql.Append(where.WhereString);
                    }
                }
                else
                {

                    sql.Append("SELECT count(*) as r_cnt FROM (");

                    sql.Append(SqlNoneOrderbyString);

                    sql.Append(") tmp__table");
                }

                return sql.ToString();
            }
        }

        /// <summary>
        /// 没有没有排序字段
        /// </summary>
        public string SqlNoneOrderbyString
        {
            get
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT ");
                sql.Append(distinctString);
                sql.Append(" ");
                sql.Append(prefixString);
                sql.Append(" ");
                sql.Append(ColumnsString);
                sql.Append(" FROM ");
                sql.Append(FromString);
                sql.Append(" ");

                if (!WhereClip.IsNullOrEmpty(where))
                {
                    sql.Append(where.WhereString);
                }
                if (!GroupByClip.IsNullOrEmpty(groupBy))
                {
                    sql.Append(GroupByString);
                    if (!WhereClip.IsNullOrEmpty(havingWhere))
                    {
                        sql.Append(" HAVING ");
                        sql.Append(havingWhere.ToString());
                    }
                }
                return sql.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal string FromString
        {
            get
            {
                StringBuilder fromstring = new StringBuilder();


                //处理ACCESS 的多表联合查询
                if (database.DbProvider is MsAccessProvider)
                {
                    fromstring.Append('(', joins.Count);
                    fromstring.Append(tableName);
                    foreach (KeyValuePair<string, KeyValuePair<string, WhereClip>> kv in joins)
                    {
                        fromstring.Append(" ");
                        fromstring.Append(kv.Value.Key);
                        fromstring.Append(" ");
                        fromstring.Append(kv.Key);
                        fromstring.Append(" ON ");
                        fromstring.Append(kv.Value.Value.ToString());
                        fromstring.Append(")");
                    }

                }
                else
                {
                    fromstring.Append(tableName);
                    foreach (KeyValuePair<string, KeyValuePair<string, WhereClip>> kv in joins)
                    {
                        fromstring.Append(" ");
                        fromstring.Append(kv.Value.Key);
                        fromstring.Append(" ");
                        fromstring.Append(kv.Key);
                        fromstring.Append(" ON ");
                        fromstring.Append(kv.Value.Value.ToString());
                    }
                }

                return fromstring.ToString();
            }

        }

        /// <summary>
        /// 连接信息
        /// </summary>
        internal Dictionary<string, KeyValuePair<string, WhereClip>> Joins
        {
            get
            {
                return joins;
            }
            set
            {
                joins = value;
            }
        }

        /// <summary>
        /// 获取 sql语句
        /// </summary>
        public string SqlString
        {
            get
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT ");
                sql.Append(distinctString);
                sql.Append(" ");
                sql.Append(prefixString);
                sql.Append(" ");
                sql.Append(ColumnsString);
                sql.Append(" FROM ");
                sql.Append(FromString);
                sql.Append(" ");

                if (!WhereClip.IsNullOrEmpty(where))
                {
                    sql.Append(where.WhereString);
                }
                if (!GroupByClip.IsNullOrEmpty(groupBy))
                {
                    sql.Append(GroupByString);
                    if (!WhereClip.IsNullOrEmpty(havingWhere))
                    {
                        sql.Append(" HAVING ");
                        sql.Append(havingWhere.ToString());
                    }
                }

                sql.Append(OrderByString);
                sql.Append(" ");
                sql.Append(LimitString);
                return sql.ToString();
            }
        }

        /// <summary>
        /// 返回  表名
        /// </summary>
        public string TableName
        {
            get
            {
                return tableName;
            }
            internal set
            {
                tableName = value;

                this.joins = new Dictionary<string, KeyValuePair<string, WhereClip>>();
            }
        }


        /// <summary>
        /// 返回  排序
        /// </summary>
        public OrderByClip OrderByClip
        {
            get
            {
                return orderBy;
            }
            internal set
            {
                orderBy = value;
            }
        }

        /// <summary>
        /// 返回排序字符串  例如：orderby id desc
        /// </summary>
        public string OrderByString
        {
            get
            {
                if (OrderByClip.IsNullOrEmpty(orderBy))
                    return string.Empty;

                if (tableName.IndexOf('(') >= 0 || tableName.IndexOf(')') >= 0 || tableName.IndexOf(" FROM ", StringComparison.OrdinalIgnoreCase) >= 0 || tableName.IndexOf(" AS ", StringComparison.OrdinalIgnoreCase) >= 0)
                    return orderBy.RemovePrefixTableName().OrderByString;
                return orderBy.OrderByString;
            }
        }

        /// <summary>
        /// 返回 分组
        /// </summary>
        public GroupByClip GroupByClip
        {
            get
            {
                return groupBy;
            }
            internal set
            {
                groupBy = value;
            }
        }

        /// <summary>
        /// 返回排序字符串 例如：group by id
        /// </summary>
        public string GroupByString
        {
            get
            {
                if (GroupByClip.IsNullOrEmpty(groupBy))
                    return string.Empty;
                if (tableName.IndexOf('(') >= 0 || tableName.IndexOf(')') >= 0 || tableName.IndexOf(" FROM ", StringComparison.OrdinalIgnoreCase) >= 0 || tableName.IndexOf(" AS ", StringComparison.OrdinalIgnoreCase) >= 0)
                    return groupBy.RemovePrefixTableName().GroupByString;
                return groupBy.GroupByString;
            }
        }

        /// <summary>
        /// 返回 条件
        /// </summary>
        public WhereClip GetWhereClip()
        {
            return where;
        }

        /// <summary>
        /// 返回 参数  (包含where 和 from)
        /// </summary>
        public List<Parameter> Parameters
        {
            get
            {
                List<Parameter> ps = new List<Parameter>();

                if (!WhereClip.IsNullOrEmpty(where))
                    ps.AddRange(where.Parameters);

                //处理groupby的having
                if (!GroupByClip.IsNullOrEmpty(groupBy) && !WhereClip.IsNullOrEmpty(havingWhere))
                    ps.AddRange(havingWhere.Parameters);

                ps.AddRange(parameters);

                return ps;
            }
            internal set
            {
                this.parameters = value;
            }


        }

        /// <summary>
        /// 返回  选择列
        /// </summary>
        public string ColumnsString
        {
            get
            {
                if (fields.Count == 0)
                    return "*";

                StringBuilder columns = new StringBuilder();
                foreach (Field filed in fields)
                {
                    columns.Append(",");
                    columns.Append(filed.FullName);
                }

                return columns.ToString().Substring(1);
            }
        }


        /// <summary>
        /// 查询的字段
        /// </summary>
        public List<Field> Fields
        {
            get
            {
                return this.fields;
            }
        }

        public string TypeTableName
        {
            get
            {
                return typeTableName;
            }

            set
            {
                this.typeTableName = value;
            }
        }

        public int? Timeout
        {
            get
            {
                return timeout;
            }

            set
            {
                this.timeout = value;
            }
        }

        public CacheDependency CacheDep
        {
            get
            {
                return cacheDep;
            }

            set
            {
                this.cacheDep = value;
            }
        }

        public bool IsRefresh
        {
            get
            {
                return isRefresh;
            }

            set
            {
                this.isRefresh = value;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        public Search(Database database, string tableName)
            : this(database, tableName, "", (DbTransaction)null)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        /// <param name="asName"></param>
        public Search(Database database, string tableName, string asName)
            : this(database, tableName, asName, (DbTransaction)null)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        /// <param name="asName"></param>
        /// <param name="trans"></param>
        public Search(Database database, string tableName, string asName, DbTransaction trans)
        {
            Check.Require(database, "database", Check.NotNull);
            Check.Require(tableName, "tableName", Check.NotNullOrEmpty);

            this.trans = trans;
            this.dbProvider = database.DbProvider;
            this.database = database;
            this.tableName = tableName;
            this.asName = asName;
            //asNames.Add(tableName + "|" +asName);
            this.TypeTableName = tableName.Trim(dbProvider.LeftToken, dbProvider.RightToken);
        }

        #endregion

        #region 操作


        /// <summary>
        /// 是否开启缓存
        /// </summary>
        /// <returns></returns>
        protected bool isTurnonCache()
        {
            if (null == dbProvider.CacheConfig)
                return false;

            return dbProvider.CacheConfig.Enable;

        }

        /// <summary>
        /// 判断是否用户自定义缓存策略
        /// </summary>
        /// <returns></returns>
        protected bool isCustomerCache()
        {
            return (Timeout.HasValue || null != CacheDep);
        }


        /// <summary>
        /// 设置缓存有效期  单位：秒
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public ISearch SetCacheTimeOut(int timeout)
        {
            this.Timeout = timeout;
            return this;
        }

        /// <summary>
        /// 设置缓存依赖
        /// </summary>
        /// <param name="dep"></param>
        /// <returns></returns>
        public ISearch SetCacheDependency(CacheDependency dep)
        {
            this.CacheDep = dep;
            return this;
        }


        /// <summary>
        /// 重新加载
        /// </summary>
        /// <returns></returns>
        public ISearch Refresh()
        {
            IsRefresh = true;
            return this;
        }


        /// <summary>
        /// whereclip
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ISearch Where(WhereClip where)
        {
            this.where = where;
            return this;
        }


        /// <summary>
        /// groupby
        /// </summary>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        public ISearch GroupBy(GroupByClip groupBy)
        {
            this.groupBy = groupBy;
            return this;
        }


        /// <summary>
        /// having条件
        /// </summary>
        /// <param name="havingWhere"></param>
        /// <returns></returns>
        public ISearch Having(WhereClip havingWhere)
        {
            this.havingWhere = havingWhere;
            return this;
        }

        /// <summary>
        /// groupby
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public ISearch GroupBy(params Field[] fields)
        {
            if (null == fields || fields.Length <= 0)
                return this;
            var tempgroupby = fields.Aggregate(GroupByClip.None, (current, f) => current && f.GroupBy);
            this.groupBy = tempgroupby;
            return this;
        }

        /// <summary>
        /// orderby
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public ISearch OrderBy(OrderByClip orderBy)
        {
            this.orderBy = orderBy;
            return this;
        }


        /// <summary>
        /// orderby
        /// </summary>
        /// <param name="orderBys"></param>
        /// <returns></returns>
        public ISearch OrderBy(params OrderByClip[] orderBys)
        {
            if (null == orderBys || orderBys.Length <= 0)
                return this;
            var temporderby = orderBys.Aggregate(OrderByClip.None, (current, ob) => current && ob);
            this.orderBy = temporderby;
            return this;
        }


        /// <summary>
        /// select field
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public ISearch Select(params Field[] fields)
        {
            this.fields.Clear();
            return AddSelect(fields);
        }


        /// <summary>
        /// select sql
        /// </summary>
        /// <param name="fromSection"></param>
        /// <returns></returns>
        public ISearch AddSelect(ISearch fromSection)
        {
            return AddSelect(fromSection, null);
        }

        /// <summary>
        /// select sql
        /// </summary>
        /// <param name="fromSection"></param>
        /// <param name="aliasName">别名</param>
        /// <returns></returns>
        public ISearch AddSelect(ISearch fromSection, string aliasName)
        {
            if (null == fromSection)
                return this;

            Check.Require(fromSection.Fields.Count == 1 && !fromSection.Fields[0].PropertyName.Equals("*"), "fromSection's fields must be only one!");

            this.fields.Add(new Field(string.Concat("(", fromSection.SqlString, ")")).As(aliasName));

            this.parameters.AddRange(fromSection.Parameters);

            return this;
        }


        /// <summary>
        /// add select field
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        internal ISearch AddSelect(params Field[] fields)
        {
            if (null != fields && fields.Length > 0)
            {
                foreach (Field field in fields)
                {
                    Field f = this.fields.Find(fi => fi.Name.Equals(field.Name) && fi.TableName.Equals(field.TableName));
                    if (Field.IsNullOrEmpty(f))
                        this.fields.Add(field);
                }
            }
            return this;
        }

        /// <summary>
        /// Distinct
        /// </summary>
        /// <returns></returns>
        public ISearch Distinct()
        {
            this.distinctString = " DISTINCT ";
            return this;
        }

        /// <summary>
        /// Top
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public ISearch Top(int topCount)
        {
            return From(1, topCount);
        }


        /// <summary>
        /// Page
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageIndex">第几页</param>
        /// <returns></returns>
        public ISearch Page(int pageSize, int pageIndex)
        {
            return From(pageSize * (pageIndex - 1) + 1, pageIndex * pageSize);
        }


        /// <summary>
        /// From startIndex to endIndex
        /// </summary>
        /// <param name="startIndex">开始记录数</param>
        /// <param name="endIndex">结束记录数</param>
        /// <returns></returns>
        public ISearch From(int startIndex, int endIndex)
        {
            this.startIndex = startIndex;

            this.endIndex = endIndex;

            isPageFromSection = false;

            return this;
        }


        /// <summary>
        /// 格式化sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        protected string formatSql(string sql, ISearch from)
        {
            string tempSql = DataUtils.FormatSQL(sql, from.DbProvider.LeftToken, from.DbProvider.RightToken);
            List<Parameter> listPara = from.Parameters;
            foreach (Parameter p in listPara)
            {
                tempSql = tempSql.Replace(p.ParameterName, p.ParameterValue == null ? string.Empty : p.ParameterValue.ToString());
            }
            return tempSql;
        }

        #endregion

        #region 查询


        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Count(GetPagedFromSection());
        }

        /// <summary>
        /// 获取记录数(内部使用)
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        internal int Count(ISearch from)
        {
            //string cacheKey = string.Concat(dbProvider.ConnectionStringsName, "COUNT", "|", formatSql(from.CountSqlString, from));
            string cacheKey = string.Format("{0}COUNT|{1}", dbProvider.ConnectionStringsName, formatSql(from.CountSqlString, from));
            object cacheValue = getCache(cacheKey);
            if (null != cacheValue)
            {
                return (int)cacheValue;
            }

            DbCommand dbCommand = database.GetSqlStringCommand(from.CountSqlString);
            database.AddCommandParameter(dbCommand, from.Parameters.ToArray());
            int returnValue;
            if (trans == null)
                returnValue = DataUtils.ConvertValue<int>(database.ExecuteScalar(dbCommand));
            else
                returnValue = DataUtils.ConvertValue<int>(database.ExecuteScalar(dbCommand, trans));

            setCache<int>(returnValue, cacheKey);

            return returnValue;
        }


        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        protected object getCache(string cacheKey)
        {
            if (IsRefresh)
                return null;

            object cacheValue = WEF.Cache.Cache.Default.GetCache(cacheKey);

            return cacheValue;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cacheKey"></param>
        protected void setCache<T>(T value, string cacheKey)
        {
            if (isCustomerCache())
                WEF.Cache.Cache.Default.AddCacheDependency(cacheKey, value, Timeout.HasValue ? Timeout.Value : 0, CacheDep);
            else
            {
                if (isTurnonCache())
                {
                    string entityCacheKey = string.Concat(dbProvider.ConnectionStringsName, TypeTableName);
                    if (dbProvider.EntitiesCache.ContainsKey(entityCacheKey))
                    {
                        int? temptimeOut = dbProvider.EntitiesCache[entityCacheKey].TimeOut;
                        if (temptimeOut.HasValue)
                        {
                            WEF.Cache.Cache.Default.AddCacheSlidingExpiration(cacheKey, value, temptimeOut.Value);
                        }
                        else
                        {
                            WEF.Cache.Cache.Default.AddCacheDependency(cacheKey, value, 0, new CacheDependency(dbProvider.EntitiesCache[entityCacheKey].FilePath));
                        }
                    }
                }
            }
        }


        /// <summary>
        /// To DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet ToDataSet()
        {
            ISearch from = GetPagedFromSection();
            string cacheKey = string.Format("{0}DataSet|{1}", dbProvider.ConnectionStringsName, formatSql(from.SqlString, from));
            object cacheValue = getCache(cacheKey);
            if (null != cacheValue)
            {
                return (DataSet)cacheValue;
            }

            DataSet ds;
            if (trans == null)
                ds = database.ExecuteDataSet(CreateDbCommand(from));
            else
                ds = database.ExecuteDataSet(CreateDbCommand(from), trans);

            setCache<DataSet>(ds, cacheKey);

            return ds;
        }

        /// <summary>
        /// 获取分页过的FromSection
        /// </summary>
        /// <returns></returns>
        public ISearch GetPagedFromSection()
        {
            if (startIndex > 0 && endIndex > 0 && !isPageFromSection)
            {
                isPageFromSection = true;
                return dbProvider.CreatePageFromSection(this, startIndex, endIndex);
            }
            return this;
        }

        /// <summary>
        /// 创建  查询的DbCommand
        /// </summary>
        /// <returns></returns>
        protected DbCommand CreateDbCommand(ISearch from)
        {
            var dbCommand = database.GetSqlStringCommand(from.SqlString);
            database.AddCommandParameter(dbCommand, from.Parameters.ToArray());
            return dbCommand;
        }

        /// <summary>
        /// To DataReader
        /// </summary>
        /// <returns></returns>
        public IDataReader ToDataReader()
        {
            return ToDataReader(GetPagedFromSection());
        }

        /// <summary>
        ///  To DataReader
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public IDataReader ToDataReader(ISearch from)
        {
            return trans == null
                ? database.ExecuteReader(CreateDbCommand(@from))
                : database.ExecuteReader(CreateDbCommand(@from), trans);
        }

        /// <summary>
        /// To DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            return this.ToDataSet().Tables[0];
        }

        /// <summary>
        /// To Scalar
        /// </summary>
        /// <returns></returns>
        public object ToScalar()
        {
            Check.Require(this.fields.Count == 1, "fields must be one!");
            Check.Require(!this.fields[0].PropertyName.Trim().Equals("*"), "fields cound not be * !");

            ISearch from = GetPagedFromSection();
            string cacheKey = string.Format("{0}Scalar|{1}", dbProvider.ConnectionStringsName, formatSql(from.SqlString, from));
            object cacheValue = getCache(cacheKey);
            if (null != cacheValue)
            {
                return cacheValue;
            }

            object returnValue;

            if (trans == null)
                returnValue = database.ExecuteScalar(CreateDbCommand(from));
            else
                returnValue = database.ExecuteScalar(CreateDbCommand(from), trans);

            setCache<object>(returnValue, cacheKey);

            return returnValue;

        }

        /// <summary>
        /// To Scalar
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public TResult ToScalar<TResult>()
        {
            return DataUtils.ConvertValue<TResult>(this.ToScalar());
        }


        #endregion

        #region 连接 join


        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <param name="joinType"></param>
        /// <returns></returns>
        protected ISearch join(string tableName, string userName, WhereClip where, JoinType joinType)
        {
            if (string.IsNullOrEmpty(tableName) || WhereClip.IsNullOrEmpty(where))
                return this;

            tableName = dbProvider.BuildTableName(tableName, userName);

            if (!joins.ContainsKey(tableName))
            {
                string joinString = string.Empty;
                switch (joinType)
                {
                    case JoinType.InnerJoin:
                        joinString = "INNER JOIN";
                        break;
                    case JoinType.LeftJoin:
                        joinString = "LEFT OUTER JOIN";
                        break;
                    case JoinType.RightJoin:
                        joinString = "RIGHT OUTER JOIN";
                        break;
                    case JoinType.CrossJoin:
                        joinString = "CROSS JOIN";
                        break;
                    case JoinType.FullJoin:
                        joinString = "FULL OUTER JOIN";
                        break;
                    default:
                        joinString = "INNER JOIN";
                        break;
                }

                joins.Add(tableName, new KeyValuePair<string, WhereClip>(joinString, where));

                if (where.Parameters.Count > 0)
                    parameters.AddRange(where.Parameters);
            }

            return this;
        }


        /// <summary>
        /// Inner Join
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ISearch InnerJoin(string tableName, WhereClip where, string userName = null)
        {
            return join(tableName, userName, where, JoinType.InnerJoin);
        }



        /// <summary>
        /// Left Join
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ISearch LeftJoin(string tableName, WhereClip where, string userName = null)
        {
            return join(tableName, userName, where, JoinType.LeftJoin);
        }



        /// <summary>
        /// Right Join
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ISearch RightJoin(string tableName, WhereClip where, string userName = null)
        {
            return join(tableName, userName, where, JoinType.RightJoin);
        }


        /// <summary>
        /// Cross Join
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ISearch CrossJoin(string tableName, WhereClip where, string userName = null)
        {
            return join(tableName, userName, where, JoinType.CrossJoin);
        }



        /// <summary>
        /// Full Join
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ISearch FullJoin(string tableName, WhereClip where, string userName = null)
        {
            return join(tableName, userName, where, JoinType.FullJoin);
        }

        #endregion

        #region Union

        /// <summary>
        /// Union
        /// </summary>
        /// <param name="fromSection"></param>
        /// <returns></returns>
        public ISearch Union(ISearch fromSection)
        {
            StringBuilder tname = new StringBuilder();

            tname.Append("(");

            tname.Append(this.SqlNoneOrderbyString);

            tname.Append(" UNION ");

            tname.Append(fromSection.SqlNoneOrderbyString);

            tname.Append(") tempuniontable ");

            ISearch tmpfromSection = new Search(this.database, tname.ToString());
            tmpfromSection.TypeTableName = this.TypeTableName;
            tmpfromSection.Timeout = this.Timeout;
            tmpfromSection.CacheDep = this.CacheDep;
            tmpfromSection.IsRefresh = this.IsRefresh;
            tmpfromSection.Parameters.AddRange(this.Parameters);
            tmpfromSection.Parameters.AddRange(fromSection.Parameters);
            return tmpfromSection;
        }

        /// <summary>
        /// Union All
        /// </summary>
        /// <param name="fromSection"></param>
        /// <returns></returns>
        public ISearch UnionAll(ISearch fromSection)
        {
            StringBuilder tname = new StringBuilder();

            tname.Append("(");

            tname.Append(this.SqlNoneOrderbyString);

            tname.Append(" UNION ALL ");

            tname.Append(fromSection.SqlNoneOrderbyString);

            tname.Append(") tempuniontable ");

            ISearch tmpfromSection = new Search(this.database, tname.ToString());
            tmpfromSection.TypeTableName = this.TypeTableName;
            tmpfromSection.Timeout = this.Timeout;
            tmpfromSection.CacheDep = this.CacheDep;
            tmpfromSection.IsRefresh = this.IsRefresh;
            tmpfromSection.Parameters.AddRange(this.Parameters);
            tmpfromSection.Parameters.AddRange(fromSection.Parameters);
            return tmpfromSection;
        }

        #endregion
    }
}
