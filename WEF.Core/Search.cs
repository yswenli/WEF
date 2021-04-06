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
 * 类名称：Search
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

using WEF.Common;
using WEF.Db;
using WEF.Expressions;
using WEF.Provider;

namespace WEF
{
    /// <summary>
    /// DBContext的查询对象
    /// </summary>    
    public class Search : ISearch
    {
        #region 变量
        /// <summary>
        /// 
        /// </summary>
        protected WhereOperation _where = WhereOperation.All;
        /// <summary>
        /// 
        /// </summary>
        protected WhereOperation _havingWhere = WhereOperation.All;
        /// <summary>
        /// 
        /// </summary>
        protected OrderByOperation _orderBy = OrderByOperation.None;
        /// <summary>
        /// 
        /// </summary>
        protected GroupByOperation _groupBy = GroupByOperation.None;
        /// <summary>
        /// 
        /// </summary>
        protected string _tableName;
        ///// <summary>
        ///// 
        ///// </summary>
        protected string _asName;


        protected List<Parameter> _parameters = new List<Parameter>();
        /// <summary>
        /// 
        /// </summary>
        protected List<Field> _fields = new List<Field>();
        /// <summary>
        /// 
        /// </summary>
        protected DbProvider _dbProvider;
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, KeyValuePair<string, WhereOperation>> _joins = new Dictionary<string, KeyValuePair<string, WhereOperation>>();
        /// <summary>
        /// 
        /// </summary>
        protected Database _database;
        /// <summary>
        /// 
        /// </summary>
        protected string _distinctString;
        /// <summary>
        /// 
        /// </summary>
        protected string _prefixString;



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
        protected int Identity { get; set; }
        #endregion

        #region 属性
        /// <summary>
        /// DbProvider。
        /// </summary>
        public DbProvider DbProvider
        {
            get { return _dbProvider; }
        }
        /// <summary>
        /// DbProvider。
        /// </summary>
        public Database Database
        {
            get { return _database; }
        }
        /// <summary>
        /// 设置 distinct
        /// </summary>
        internal string DistinctString
        {
            set
            {
                _distinctString = value;
            }
        }

        /// <summary>
        /// 前置值如 Top N
        /// </summary>
        internal string PrefixString
        {
            set
            {
                _prefixString = value;
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
            get { return _limitString; }
        }

        /// <summary>
        /// 记录数sql语句 count
        /// </summary>
        public string CountSqlString
        {
            get
            {
                StringBuilder sql = new StringBuilder();

                if (GroupByOperation.IsNullOrEmpty(_groupBy) && string.IsNullOrEmpty(_distinctString))
                {
                    sql.Append(" SELECT count(*) as r_cnt FROM ");
                    sql.Append(FromString);
                    if (!WhereOperation.IsNullOrEmpty(_where))
                    {
                        sql.Append(_where.WhereString);
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
        internal string SqlNoneOrderbyString
        {
            get
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT ");
                sql.Append(_distinctString);
                sql.Append(" ");
                sql.Append(_prefixString);
                sql.Append(" ");
                sql.Append(ColumnsString);
                sql.Append(" FROM ");
                sql.Append(FromString);
                sql.Append(" ");

                if (!WhereOperation.IsNullOrEmpty(_where))
                {
                    sql.Append(_where.WhereString);
                }
                if (!GroupByOperation.IsNullOrEmpty(_groupBy))
                {
                    sql.Append(GroupByString);
                    if (!WhereOperation.IsNullOrEmpty(_havingWhere))
                    {
                        sql.Append(" HAVING ");
                        sql.Append(_havingWhere.ToString());
                    }
                }
                return sql.ToString();
            }
        }

        /// <summary>
        /// FromString
        /// </summary>
        internal string FromString
        {
            get
            {
                StringBuilder fromstring = new StringBuilder();

                //处理ACCESS 的多表联合查询
                if (_database.DbProvider is MsAccessProvider)
                {
                    fromstring.Append('(', _joins.Count);
                    fromstring.Append(_tableName);
                    foreach (KeyValuePair<string, KeyValuePair<string, WhereOperation>> kv in _joins)
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
                    fromstring.Append(_tableName);
                    foreach (KeyValuePair<string, KeyValuePair<string, WhereOperation>> kv in _joins)
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
        internal Dictionary<string, KeyValuePair<string, WhereOperation>> Joins
        {
            get
            {
                return _joins;
            }
            set
            {
                _joins = value;
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
                sql.Append(_distinctString);
                sql.Append(" ");
                sql.Append(_prefixString);
                sql.Append(" ");
                sql.Append(ColumnsString);
                sql.Append(" FROM ");
                sql.Append(FromString);
                sql.Append(" ");

                if (!WhereOperation.IsNullOrEmpty(_where))
                {
                    sql.Append(_where.WhereString);
                }
                if (!GroupByOperation.IsNullOrEmpty(_groupBy))
                {
                    sql.Append(GroupByString);
                    if (!WhereOperation.IsNullOrEmpty(_havingWhere))
                    {
                        sql.Append(" HAVING ");
                        sql.Append(_havingWhere.ToString());
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
                return _tableName;
            }
            internal set
            {
                _tableName = value;

                this._joins = new Dictionary<string, KeyValuePair<string, WhereOperation>>();
            }
        }


        /// <summary>
        /// 返回  排序
        /// </summary>
        public OrderByOperation OrderByClip
        {
            get
            {
                return _orderBy;
            }
            internal set
            {
                _orderBy = value;
            }
        }

        /// <summary>
        /// 返回排序字符串  例如：orderby id desc
        /// </summary>
        public string OrderByString
        {
            get
            {
                if (OrderByOperation.IsNullOrEmpty(_orderBy))
                    return string.Empty;

                if ((_tableName.IndexOf('(') >= 0 || _tableName.IndexOf(')') >= 0 || _tableName.IndexOf(" FROM ", StringComparison.OrdinalIgnoreCase) >= 0 || _tableName.IndexOf(" AS ", StringComparison.OrdinalIgnoreCase) >= 0)
                    && !FromString.Contains(" LEFT OUTER JOIN ") //2018-04-09 新增一个&&条件
                    )
                    return _orderBy.RemovePrefixTableName().OrderByString;
                return _orderBy.OrderByString;
            }
        }

        /// <summary>
        /// 返回 分组
        /// </summary>
        public GroupByOperation GroupByClip
        {
            get
            {
                return _groupBy;
            }
            internal set
            {
                _groupBy = value;
            }
        }

        /// <summary>
        /// 返回排序字符串 例如：group by id
        /// </summary>
        public string GroupByString
        {
            get
            {
                if (GroupByOperation.IsNullOrEmpty(_groupBy))
                    return string.Empty;
                if (_tableName.IndexOf('(') >= 0 || _tableName.IndexOf(')') >= 0 || _tableName.IndexOf(" FROM ", StringComparison.OrdinalIgnoreCase) >= 0 || _tableName.IndexOf(" AS ", StringComparison.OrdinalIgnoreCase) >= 0)
                    return _groupBy.RemovePrefixTableName().GroupByString;
                return _groupBy.GroupByString;
            }
        }

        /// <summary>
        /// 返回 条件
        /// </summary>
        public WhereOperation GetWhereClip()
        {
            return _where;
        }

        /// <summary>
        /// 返回 参数  (包含where 和 from)
        /// </summary>
        public List<Parameter> Parameters
        {
            get
            {
                List<Parameter> ps = new List<Parameter>();

                if (!WhereOperation.IsNullOrEmpty(_where))
                    ps.AddRange(_where.Parameters);

                //处理groupby的having
                if (!GroupByOperation.IsNullOrEmpty(_groupBy) && !WhereOperation.IsNullOrEmpty(_havingWhere))
                    ps.AddRange(_havingWhere.Parameters);

                ps.AddRange(_parameters);

                return ps;
            }
            internal set
            {
                this._parameters = value;
            }


        }

        /// <summary>
        /// 返回  选择列
        /// </summary>
        public string ColumnsString
        {
            get
            {
                if (_fields.Count == 0)
                    return "*";

                StringBuilder columns = new StringBuilder();
                foreach (Field filed in _fields)
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
                return this._fields;
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
            this._dbProvider = database.DbProvider;
            this._database = database;
            this._tableName = tableName;
            this._asName = asName;
            this.typeTableName = tableName.Trim(_dbProvider.LeftToken, _dbProvider.RightToken);
        }

        #endregion

        #region 操作


        /// <summary>
        /// 设置缓存有效期  单位：秒
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public Search SetCacheTimeOut(int timeout)
        {
            this.timeout = timeout;
            return this;
        }


        /// <summary>
        /// 重新加载
        /// </summary>
        /// <returns></returns>
        public Search Refresh()
        {
            isRefresh = true;
            return this;
        }


        /// <summary>
        /// whereclip
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search Where(WhereOperation where)
        {
            this._where = where;
            return this;
        }


        /// <summary>
        /// groupby
        /// </summary>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        public Search GroupBy(GroupByOperation groupBy)
        {
            this._groupBy = groupBy;
            return this;
        }


        /// <summary>
        /// having条件
        /// </summary>
        /// <param name="havingWhere"></param>
        /// <returns></returns>
        public Search Having(WhereOperation havingWhere)
        {
            this._havingWhere = havingWhere;
            return this;
        }

        /// <summary>
        /// groupby
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public Search GroupBy(params Field[] fields)
        {
            if (null == fields || fields.Length <= 0) return this;
            var tempgroupby = fields.Aggregate(GroupByOperation.None, (current, f) => current && f.GroupBy);
            this._groupBy = tempgroupby;
            return this;
        }

        /// <summary>
        /// orderby
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public Search OrderBy(OrderByOperation orderBy)
        {
            this._orderBy = orderBy;
            return this;
        }


        /// <summary>
        /// orderby
        /// </summary>
        /// <param name="orderBys"></param>
        /// <returns></returns>
        public Search OrderBy(params OrderByOperation[] orderBys)
        {
            if (null == orderBys || orderBys.Length <= 0) return this;
            var temporderby = orderBys.Aggregate(OrderByOperation.None, (current, ob) => current && ob);
            this._orderBy = temporderby;
            return this;
        }


        /// <summary>
        /// select field
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public Search Select(params Field[] fields)
        {
            this._fields.Clear();
            return AddSelect(fields);
        }


        /// <summary>
        /// select sql
        /// </summary>
        /// <param name="Search"></param>
        /// <returns></returns>
        public Search AddSelect(Search Search)
        {
            return AddSelect(Search, null);
        }

        /// <summary>
        /// select sql
        /// </summary>
        /// <param name="Search"></param>
        /// <param name="aliasName">别名</param>
        /// <returns></returns>
        public Search AddSelect(Search Search, string aliasName)
        {
            if (null == Search)
                return this;

            Check.Require(Search.Fields.Count == 1 && !Search.Fields[0].PropertyName.Equals("*"), "Search's fields must be only one!");

            this._fields.Add(new Field(string.Concat("(", Search.SqlString, ")")).As(aliasName));

            this._parameters.AddRange(Search.Parameters);

            return this;
        }


        /// <summary>
        /// add select field
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        internal Search AddSelect(params Field[] fields)
        {
            if (null != fields && fields.Length > 0)
            {
                foreach (Field field in fields)
                {
                    Field f = this._fields.Find(fi => fi.Name.Equals(field.Name) && fi.TableName.Equals(field.TableName));
                    if (Field.IsNullOrEmpty(f))
                        this._fields.Add(field);
                }
            }
            return this;
        }

        /// <summary>
        /// Distinct
        /// </summary>
        /// <returns></returns>
        public Search Distinct()
        {
            this._distinctString = " DISTINCT ";
            return this;
        }

        /// <summary>
        /// Top
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public Search Top(int topCount)
        {
            return From(1, topCount);
        }


        /// <summary>
        /// Page
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageIndex">第几页</param>
        /// <returns></returns>
        public Search Page(int pageSize, int pageIndex)
        {
            return From(pageSize * (pageIndex - 1) + 1, pageIndex * pageSize);
        }


        /// <summary>
        /// From startIndex to endIndex
        /// </summary>
        /// <param name="startIndex">开始记录数</param>
        /// <param name="endIndex">结束记录数</param>
        /// <returns></returns>
        public Search From(int startIndex, int endIndex)
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
        protected string formatSql(string sql, Search from)
        {
            string tempSql = DataUtils.FormatSQL(sql, from._dbProvider.LeftToken, from._dbProvider.RightToken);
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
        internal int Count(Search from)
        {
            DbCommand dbCommand = _database.GetSqlStringCommand(from.CountSqlString);

            _database.AddCommandParameter(dbCommand, from.Parameters.ToArray());

            int returnValue;
            if (trans == null)
                returnValue = DataUtils.ConvertValue<int>(_database.ExecuteScalar(dbCommand));
            else
                returnValue = DataUtils.ConvertValue<int>(_database.ExecuteScalar(dbCommand, trans));

            return returnValue;
        }





        /// <summary>
        /// To DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet ToDataSet()
        {
            Search from = GetPagedFromSection();

            DataSet ds;
            if (trans == null)
                ds = _database.ExecuteDataSet(CreateDbCommand(from));
            else
                ds = _database.ExecuteDataSet(CreateDbCommand(from), trans);

            return ds;
        }

        /// <summary>
        /// 获取分页过的FromSection
        /// </summary>
        /// <returns></returns>
        internal Search GetPagedFromSection()
        {
            if (startIndex > 0 && endIndex > 0 && !isPageFromSection)
            {
                isPageFromSection = true;
                return _dbProvider.CreatePageFromSection(this, startIndex, endIndex);
            }
            return this;
        }
        

        /// <summary>
        /// 创建  查询的DbCommand
        /// </summary>
        /// <returns></returns>
        protected DbCommand CreateDbCommand(Search from)
        {
            var dbCommand = _database.GetSqlStringCommand(from.SqlString);
            _database.AddCommandParameter(dbCommand, from.Parameters.ToArray());
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
        protected IDataReader ToDataReader(Search from)
        {
            return trans == null
                ? _database.ExecuteReader(CreateDbCommand(@from))
                : _database.ExecuteReader(CreateDbCommand(@from), trans);
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
            Check.Require(this._fields.Count == 1, "fields must be one!");
            Check.Require(!this._fields[0].PropertyName.Trim().Equals("*"), "fields cound not be * !");

            Search from = GetPagedFromSection();


            object returnValue;

            if (trans == null)
                returnValue = _database.ExecuteScalar(CreateDbCommand(from));
            else
                returnValue = _database.ExecuteScalar(CreateDbCommand(from), trans);

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
        protected Search join(string tableName, string userName, WhereOperation where, JoinType joinType)
        {
            if (string.IsNullOrEmpty(tableName) || WhereOperation.IsNullOrEmpty(where))
                return this;

            tableName = _dbProvider.BuildTableName(tableName, userName);

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

                if (_joins.ContainsKey(tableName))
                {
                    var index = (_joins.Keys.Count(d => d.StartsWith(tableName)) + 1).ToString();
                    var realTableName = tableName.Substring(1, tableName.Length - 2);
                    tableName += " as " + tableName.Insert(tableName.Length - 1, index);
                    where.expressionString = where.expressionString.Replace(realTableName, realTableName + index);
                }


                _joins.Add(tableName, new KeyValuePair<string, WhereOperation>(joinString, where));

                if (where.Parameters.Count > 0)
                    _parameters.AddRange(where.Parameters);
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
        public Search InnerJoin(string tableName, WhereOperation where, string userName = null)
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
        public Search LeftJoin(string tableName, WhereOperation where, string userName = null)
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
        public Search RightJoin(string tableName, WhereOperation where, string userName = null)
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
        public Search CrossJoin(string tableName, WhereOperation where, string userName = null)
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
        public Search FullJoin(string tableName, WhereOperation where, string userName = null)
        {
            return join(tableName, userName, where, JoinType.FullJoin);
        }

        #endregion

        #region Union

        /// <summary>
        /// Union
        /// </summary>
        /// <param name="Search"></param>
        /// <returns></returns>
        public Search Union(Search Search)
        {
            StringBuilder tname = new StringBuilder();

            tname.Append("(");

            tname.Append(this.SqlNoneOrderbyString);

            tname.Append(" UNION ");

            tname.Append(Search.SqlNoneOrderbyString);

            tname.Append(") tempuniontable ");

            Search tmpSearch = new Search(this._database, tname.ToString());
            tmpSearch.typeTableName = this.typeTableName;
            tmpSearch.timeout = this.timeout;
            tmpSearch.isRefresh = this.isRefresh;


            tmpSearch._parameters.AddRange(this.Parameters);
            tmpSearch._parameters.AddRange(Search.Parameters);

            return tmpSearch;
        }

        /// <summary>
        /// Union All
        /// </summary>
        /// <param name="Search"></param>
        /// <returns></returns>
        public Search UnionAll(Search Search)
        {
            StringBuilder tname = new StringBuilder();

            tname.Append("(");

            tname.Append(this.SqlNoneOrderbyString);

            tname.Append(" UNION ALL ");

            tname.Append(Search.SqlNoneOrderbyString);

            tname.Append(") tempuniontable ");

            Search tmpSearch = new Search(this._database, tname.ToString());
            tmpSearch.typeTableName = this.typeTableName;
            tmpSearch.timeout = this.timeout;
            tmpSearch.isRefresh = this.isRefresh;

            tmpSearch._parameters.AddRange(this.Parameters);
            tmpSearch._parameters.AddRange(Search.Parameters);

            return tmpSearch;
        }

        #endregion
    }
}
