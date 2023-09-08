/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2022
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

using WEF.Common;
using WEF.Db;
using WEF.Expressions;
using WEF.MvcPager;
using WEF.Provider;

namespace WEF
{
    /// <summary>
    /// DBContext的查询对象
    /// </summary>    
    public class Search
    {
        #region 变量
        /// <summary>
        /// 
        /// </summary>
        protected WhereExpression _where = WhereExpression.All;
        /// <summary>
        /// 
        /// </summary>
        protected WhereExpression _havingWhere = WhereExpression.All;
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
        protected JoinOn _joinOn = new JoinOn();
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
        /// 某种类型的数据库表名，例如：[User]或`User`
        /// </summary>
        protected string _typeTableName;

        /// <summary>
        /// 是否重新加载
        /// </summary>
        protected bool _isRefresh = false;

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
        public string DistinctString
        {
            set
            {
                _distinctString = value;
            }
        }

        /// <summary>
        /// 前置值如 Top N
        /// </summary>
        public string PrefixString
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
                StringPlus sql = new StringPlus();

                if (GroupByOperation.IsNullOrEmpty(_groupBy) && string.IsNullOrEmpty(_distinctString))
                {
                    sql.Append(" SELECT count(1) as r_cnt FROM ");
                    sql.Append(_joinOn.ToString(_tableName, _database.DbProvider.GetType().Name));
                    if (!WhereExpression.IsNullOrEmpty(_where))
                    {
                        sql.Append(_where.WhereString);
                    }
                }
                else
                {

                    sql.Append("SELECT count(1) as r_cnt FROM (");

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
                StringPlus sql = new StringPlus();

                sql.Append(" SELECT ");
                sql.Append(_distinctString);
                sql.Append(" ");
                sql.Append(_prefixString);
                sql.Append(" ");
                sql.Append(ColumnsString);
                sql.Append(" FROM ");
                sql.Append(_joinOn.ToString(_tableName, _database.DbProvider.GetType().Name));
                sql.Append(" ");

                if (!WhereExpression.IsNullOrEmpty(_where))
                {
                    sql.Append(_where.WhereString);
                }
                if (!GroupByOperation.IsNullOrEmpty(_groupBy))
                {
                    sql.Append(GroupByString);
                    if (!WhereExpression.IsNullOrEmpty(_havingWhere))
                    {
                        sql.Append(" HAVING ");
                        sql.Append(_havingWhere.ToString());
                    }
                }
                return sql.ToString();
            }
        }


        /// <summary>
        /// 获取 sql语句
        /// </summary>
        public string SqlString
        {
            get
            {
                StringPlus sql = new StringPlus();

                sql.Append(" SELECT ");
                sql.Append(_distinctString);
                sql.Append(" ");
                sql.Append(_prefixString);
                sql.Append(" ");
                sql.Append(ColumnsString);
                sql.Append(" FROM ");
                sql.Append(_joinOn.ToString(_tableName, _database.DbProvider.GetType().Name));
                sql.Append(" ");

                if (!WhereExpression.IsNullOrEmpty(_where))
                {
                    sql.Append(_where.WhereString);
                }
                if (!GroupByOperation.IsNullOrEmpty(_groupBy))
                {
                    sql.Append(GroupByString);
                    if (!WhereExpression.IsNullOrEmpty(_havingWhere))
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
            set
            {
                _tableName = value;
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
            set
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

                if ((_tableName.IndexOf('(') >= 0 || _tableName.IndexOf(')') >= 0
                    || _tableName.IndexOf(" FROM ", StringComparison.OrdinalIgnoreCase) >= 0
                    || _tableName.IndexOf(" AS ", StringComparison.OrdinalIgnoreCase) >= 0)
                    && !_joinOn.ToString(_tableName, _database.DbProvider.GetType().Name).Contains(" LEFT OUTER JOIN "))
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
        public WhereExpression GetWhereClip()
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

                if (!WhereExpression.IsNullOrEmpty(_where))
                    ps.AddRange(_where.Parameters);

                //处理groupby的having
                if (!GroupByOperation.IsNullOrEmpty(_groupBy) && !WhereExpression.IsNullOrEmpty(_havingWhere))
                    ps.AddRange(_havingWhere.Parameters);

                ps.AddRange(_parameters);

                return ps;
            }
            set
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

                StringPlus columns = new StringPlus();
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
        /// <param name="trans"></param>
        public Search(Database database, string tableName, DbTransaction trans)
           : this(database, tableName, null, trans)
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
            this._typeTableName = tableName.Trim(_dbProvider.LeftToken, _dbProvider.RightToken);
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
            _isRefresh = true;
            return this;
        }


        /// <summary>
        /// whereclip
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search Where(WhereExpression where)
        {
            this._where = where;
            return this;
        }
        /// <summary>
        /// where
        /// </summary>
        /// <param name="whereSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Search Where(string whereSql, params Parameter[] parameters)
        {
            this._where = new WhereExpression(whereSql, parameters);
            return this;
        }

        /// <summary>
        /// groupby
        /// </summary>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        public Search GroupBy(GroupByOperation groupBy)
        {
            this._groupBy = this._groupBy & groupBy;
            return this;
        }


        /// <summary>
        /// having条件
        /// </summary>
        /// <param name="havingWhere"></param>
        /// <returns></returns>
        public Search Having(WhereExpression havingWhere)
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
            this._orderBy = this._orderBy & temporderby;
            return this;
        }
        /// <summary>
        /// OrderBy
        /// </summary>
        /// <param name="asc"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public Search OrderBy(IEnumerable<string> asc, IEnumerable<string> desc)
        {
            var orderBys = new List<OrderByOperation>();

            foreach (var item in asc)
            {
                orderBys.Aggregate(OrderByOperation.None, (current, ob) => current && ob);

                orderBys.Add(new OrderByOperation(item, OrderByOperater.ASC));
            }

            foreach (var item in desc)
            {
                orderBys.Add(new OrderByOperation(item, OrderByOperater.DESC));
            }

            if (null == orderBys || !orderBys.Any()) return this;

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
        public Search AddSelect(params Field[] fields)
        {
            if (null != fields && fields.Length > 0)
            {
                foreach (Field field in fields)
                {
                    Field f = _fields.Find(fi => fi.Name.Equals(field.Name) && fi.TableName.Equals(field.TableName));
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
        /// <param name="search"></param>
        /// <returns></returns>
        public string FormatSql(string sql, Search search)
        {
            string tempSql = DataUtils.FormatSQL(sql, search._dbProvider.LeftToken, search._dbProvider.RightToken);
            List<Parameter> listPara = search.Parameters;
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
        /// <param name="search"></param>
        /// <returns></returns>
        internal int Count(Search search)
        {
            DbCommand dbCommand = _database.GetSqlStringCommand(search.CountSqlString);

            _database.AddCommandParameter(dbCommand, search.Parameters.ToArray());

            int returnValue;
            if (trans == null)
                returnValue = DataUtils.ConvertValue<int>(_database.ExecuteScalar(dbCommand));
            else
                returnValue = DataUtils.ConvertValue<int>(_database.ExecuteScalar(dbCommand, trans));

            return returnValue;
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <returns></returns>
        public long LongCount()
        {
            return LongCount(GetPagedFromSection());
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        internal long LongCount(Search search)
        {
            DbCommand dbCommand = _database.GetSqlStringCommand(search.CountSqlString);

            _database.AddCommandParameter(dbCommand, search.Parameters.ToArray());

            long returnValue;
            if (trans == null)
                returnValue = DataUtils.ConvertValue<long>(_database.ExecuteScalar(dbCommand));
            else
                returnValue = DataUtils.ConvertValue<long>(_database.ExecuteScalar(dbCommand, trans));

            return returnValue;
        }

        /// <summary>
        /// To DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet ToDataSet()
        {
            Search search = GetPagedFromSection();

            DataSet ds;
            if (trans == null)
                ds = _database.ExecuteDataSet(CreateDbCommand(search));
            else
                ds = _database.ExecuteDataSet(CreateDbCommand(search), trans);

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
        protected DbCommand CreateDbCommand(Search search)
        {
            var dbCommand = _database.GetSqlStringCommand(search.SqlString);
            _database.AddCommandParameter(dbCommand, search.Parameters.ToArray());
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
        /// <param name="search"></param>
        /// <returns></returns>
        protected IDataReader ToDataReader(Search search)
        {
            return trans == null
                ? _database.ExecuteReader(CreateDbCommand(search))
                : _database.ExecuteReader(CreateDbCommand(search), trans);
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

            Search search = GetPagedFromSection();


            object returnValue;

            if (trans == null)
                returnValue = _database.ExecuteScalar(CreateDbCommand(search));
            else
                returnValue = _database.ExecuteScalar(CreateDbCommand(search), trans);

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
        /// 连接,加载空会清空
        /// </summary>
        /// <param name="joinOn"></param>
        /// <returns></returns>
        public Search Join(JoinOn joinOn)
        {
            if (string.IsNullOrEmpty(joinOn._tableName))
            {
                _joinOn = new JoinOn();
            }
            else
            {
                _joinOn.Add(joinOn);
            }
            return this;
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <param name="joinType"></param>
        /// <returns></returns>
        public Search Join(string tableName, string userName, WhereExpression where, JoinType joinType)
        {
            if (string.IsNullOrEmpty(tableName) || WhereExpression.IsNullOrEmpty(where))
                return this;

            tableName = _dbProvider.BuildTableName(tableName, userName);

            _joinOn.Add(tableName, where, joinType);

            if (where.Parameters.Count > 0)
                _parameters.AddRange(where.Parameters);

            return this;
        }


        /// <summary>
        /// Inner Join
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search InnerJoin(string tableName, WhereExpression where, string userName = null)
        {
            return Join(tableName, userName, where, JoinType.InnerJoin);
        }



        /// <summary>
        /// Left Join
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search LeftJoin(string tableName, WhereExpression where, string userName = null)
        {
            return Join(tableName, userName, where, JoinType.LeftJoin);
        }



        /// <summary>
        /// Right Join
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search RightJoin(string tableName, WhereExpression where, string userName = null)
        {
            return Join(tableName, userName, where, JoinType.RightJoin);
        }


        /// <summary>
        /// Cross Join
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search CrossJoin(string tableName, WhereExpression where, string userName = null)
        {
            return Join(tableName, userName, where, JoinType.CrossJoin);
        }



        /// <summary>
        /// Full Join
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public Search FullJoin(string tableName, WhereExpression where, string userName = null)
        {
            return Join(tableName, userName, where, JoinType.FullJoin);
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
            StringPlus tname = new StringPlus();

            tname.Append("(");

            tname.Append(this.SqlNoneOrderbyString);

            tname.Append(" UNION ");

            tname.Append(Search.SqlNoneOrderbyString);

            tname.Append(") tempuniontable ");

            Search tmpSearch = new Search(this._database, tname.ToString());
            tmpSearch._typeTableName = this._typeTableName;
            tmpSearch.timeout = this.timeout;
            tmpSearch._isRefresh = this._isRefresh;


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
            StringPlus tname = new StringPlus();

            tname.Append("(");

            tname.Append(this.SqlNoneOrderbyString);

            tname.Append(" UNION ALL ");

            tname.Append(Search.SqlNoneOrderbyString);

            tname.Append(") tempuniontable ");

            Search tmpSearch = new Search(this._database, tname.ToString());
            tmpSearch._typeTableName = this._typeTableName;
            tmpSearch.timeout = this.timeout;
            tmpSearch._isRefresh = this._isRefresh;

            tmpSearch._parameters.AddRange(this.Parameters);
            tmpSearch._parameters.AddRange(Search.Parameters);

            return tmpSearch;
        }

        #endregion

    }
}
