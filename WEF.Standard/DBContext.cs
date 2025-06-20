/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2024
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：80082f38-5128-45aa-9217-d181a0bb7f92
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：WEF
 * 命名空间：WEF
 * 类名称：DBContext
 * 创建时间：2017/7/26 14:34:40
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using WEF.Batcher;
using WEF.Common;
using WEF.Db;
using WEF.Expressions;
using WEF.Provider;
using WEF.Section;

namespace WEF
{
    /// <summary>
    /// WEF核心类，数据操作上下文
    /// </summary>
    public sealed partial class DBContext : IDisposable
    {
        const string CONTEXTKEY = "DBContext.Current";

        private Database _db;

        private CommandCreator _cmdCreator;

        #region 属性

        /// <summary>
        /// 左边  
        /// <example>例如:sqlserver   的    [</example>
        /// </summary>
        public string LeftToken
        {
            get
            {
                return _db.DbProvider.LeftToken.ToString();
            }
        }


        /// <summary>
        /// 右边
        /// <example>例如:sqlserver   的    ]</example>
        /// </summary>
        public string RightToken
        {
            get
            {
                return _db.DbProvider.RightToken.ToString();
            }
        }

        /// <summary>
        /// 参数前缀
        /// <example>例如:sqlserver 的     @</example>
        /// </summary>
        public string ParamPrefix
        {
            get
            {
                return _db.DbProvider.ParamPrefix.ToString();
            }
        }

        /// <summary>
        /// 获取当前上下文的DBContext
        /// </summary>
        public static DBContext Current
        {
            get
            {
                return (DBContext)CallContext.GetData(CONTEXTKEY);
            }
        }

        #endregion

        #region batch

        /// <summary>
        /// 开始批处理
        /// </summary>
        /// <returns></returns>
        public IBatcher<T> CreateBatch<T>() where T : Entity
        {
            return DipBuilder.CreateBatcher<T>(_db);
        }
        #endregion

        #region 构造函数



        /// <summary>
        /// 构造函数    使用默认  DBContext.Default
        /// </summary>
        /// <param name="timeout"></param>
        public DBContext(int timeout = 120)
        {
            _db = Database.Default;
            _db.TimeOut = timeout;
            _cmdCreator = new CommandCreator(_db);
            CallContext.SetData(CONTEXTKEY, this);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connStrName">config文件中connectionStrings节点的name</param>
        /// <param name="timeout"></param>
        public DBContext(string connStrName, int timeout = 120)
        {
            _db = new Database(ProviderFactory.CreateDbProvider(connStrName), timeout);
            _db.DbProvider.ConnectionStringsName = connStrName;
            _cmdCreator = new CommandCreator(_db);
            CallContext.SetData(CONTEXTKEY, this);
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">已知的Database</param>
        /// <param name="timeout"></param>
        public DBContext(Database db, int timeout = 120)
        {
            _db = db;
            _db.TimeOut = timeout;
            _cmdCreator = new CommandCreator(_db);
            CallContext.SetData(CONTEXTKEY, this);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dt">数据库类别</param>
        /// <param name="connStr">连接字符串</param>
        /// <param name="timeout"></param>
        public DBContext(DatabaseType dt, string connStr, int timeout = 120)
        {
            DbProvider provider = DbProviderCreator.Create(dt, connStr);
            _db = new Database(provider, timeout);
            _cmdCreator = new CommandCreator(_db);
            CallContext.SetData(CONTEXTKEY, this);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="assemblyName">程序集</param>
        /// <param name="className">类名</param>
        /// <param name="connStr">连接字符串</param>
        /// <param name="timeout"></param>
        public DBContext(string assemblyName, string className, string connStr, int timeout = 120)
        {
            DbProvider provider = ProviderFactory.CreateDbProvider(assemblyName, className, connStr);
            if (provider == null)
            {
                throw new NotSupportedException(string.Format("Cannot construct DbProvider by specified parameters: {0}, {1}, {2}",
                    assemblyName, className, connStr));
            }

            _db = new Database(provider, timeout);

            _cmdCreator = new CommandCreator(_db);

            CallContext.SetData(CONTEXTKEY, this);
        }

        #endregion

        #region 查询


        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public Search<TEntity> Search<TEntity>(string tableName = "")
            where TEntity : Entity
        {
            return new Search<TEntity>(_db, null, tableName);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Search Search(string tableName)
        {
            return new Search(_db, tableName);
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Search<TEntity> Search<TEntity>(TEntity entity)
           where TEntity : Entity
        {
            return new Search<TEntity>(_db, null, entity.GetTableName());
        }


        /// <summary>
        /// Sum
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Sum<TEntity>(Field field, WhereExpression where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Sum()).Where(where).ToScalar();
        }
        /// <summary>
        /// Sum
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Sum<TEntity>(Field field, Where where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Sum()).Where(where).ToScalar();
        }
        /// <summary>
        /// Sum
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public object Sum<TEntity>(Field field, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Sum()).Where(lambdaWhere).ToScalar();
        }
        /// <summary>
        /// Max
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Max<TEntity>(Field field, WhereExpression where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Max()).Where(where).ToScalar();
        }
        /// <summary>
        /// Max
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Max<TEntity>(Field field, Where where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Max()).Where(where).ToScalar();
        }
        /// <summary>
        /// Max
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public object Max<TEntity>(Field field, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Max()).Where(lambdaWhere).ToScalar();
        }
        /// <summary>
        /// Min
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Min<TEntity>(Field field, WhereExpression where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Min()).Where(where).ToScalar();
        }
        /// <summary>
        /// Min
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Min<TEntity>(Field field, Where where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Min()).Where(where).ToScalar();
        }
        /// <summary>
        /// Min
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public object Min<TEntity>(Field field, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Min()).Where(lambdaWhere).ToScalar();
        }
        /// <summary>
        /// Avg
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Avg<TEntity>(Field field, WhereExpression where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Avg()).Where(where).ToScalar();
        }
        /// <summary>
        /// Avg
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public object Avg<TEntity>(Field field, Where where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Avg()).Where(where).ToScalar();
        }
        /// <summary>
        /// Avg
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public object Avg<TEntity>(Field field, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Avg()).Where(lambdaWhere).ToScalar();
        }
        /// <summary>
        /// Sum
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Sum<TEntity, TResult>(Field field, WhereExpression where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Sum()).Where(where).ToScalar<TResult>();
        }
        /// <summary>
        /// Sum
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Sum<TEntity, TResult>(Field field, Where where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Sum()).Where(where).ToScalar<TResult>();
        }
        /// <summary>
        /// Sum
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public TResult Sum<TEntity, TResult>(Field field, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Sum()).Where(lambdaWhere).ToScalar<TResult>();
        }
        /// <summary>
        /// Max
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Max<TEntity, TResult>(Field field, WhereExpression where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Max()).Where(where).ToScalar<TResult>();
        }
        /// <summary>
        /// Max
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Max<TEntity, TResult>(Field field, Where where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Max()).Where(where).ToScalar<TResult>();
        }
        /// <summary>
        /// Max
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public TResult Max<TEntity, TResult>(Field field, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Max()).Where(lambdaWhere).ToScalar<TResult>();
        }
        /// <summary>
        /// Min
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Min<TEntity, TResult>(Field field, WhereExpression where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Min()).Where(where).ToScalar<TResult>();
        }
        /// <summary>
        /// Min
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Min<TEntity, TResult>(Field field, Where where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Min()).Where(where).ToScalar<TResult>();
        }
        /// <summary>
        /// Min
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public TResult Min<TEntity, TResult>(Field field, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Min()).Where(lambdaWhere).ToScalar<TResult>();
        }
        /// <summary>
        /// Avg
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Avg<TEntity, TResult>(Field field, WhereExpression where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Avg()).Where(where).ToScalar<TResult>();
        }
        /// <summary>
        /// Avg
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public TResult Avg<TEntity, TResult>(Field field, Where where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Avg()).Where(where).ToScalar<TResult>();
        }
        /// <summary>
        /// Avg
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="field"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public TResult Avg<TEntity, TResult>(Field field, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Avg()).Where(lambdaWhere).ToScalar<TResult>();
        }
        /// <summary>
        /// 判断是否存在记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists<TEntity>(WhereExpression where)
            where TEntity : Entity
        {
            using (IDataReader dataReader = Search<TEntity>().Where(where).Top(1).Select(EntityCache.GetFirstField<TEntity>()).ToDataReader())
            {
                if (dataReader.Read())
                {
                    return true;
                }

                dataReader.Close();
            }

            return false;

        }

        /// <summary>
        /// 判断是否存在记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public bool Exists<TEntity>(string tableName, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Exists<TEntity>(ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 判断是否存在记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists<TEntity>(Where where)
            where TEntity : Entity
        {
            return Exists<TEntity>(where.ToWhereClip());
        }
        /// <summary>
        /// Count
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count<TEntity>(Field field, WhereExpression where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Count()).Where(where).ToScalar<int>();
        }
        /// <summary>
        /// Count
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count<TEntity>(Field field, Where where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Count()).Where(where).ToScalar<int>();
        }
        /// <summary>
        /// Count
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Count<TEntity>(string tableName, Field field, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Search<TEntity>(tableName).Select(field.Count()).Where(ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere)).ToScalar<int>();
        }
        /// <summary>
        /// Count
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count<TEntity>(WhereExpression where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(Field.All.Count()).Where(where).ToScalar<int>();
        }
        /// <summary>
        /// Count
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count<TEntity>(Where where)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(Field.All.Count()).Where(where).ToScalar<int>();
        }
        /// <summary>
        /// Count
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Count<TEntity>(Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(Field.All.Count()).Where(ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere)).ToScalar<int>();
        }
        #endregion

        #region Database

        /// <summary>
        /// Registers the SQL logger.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void RegisterSqlLogger(LogHandler handler)
        {
            _db.OnLog += handler;
        }

        /// <summary>
        /// Unregisters the SQL logger.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void UnregisterSqlLogger(LogHandler handler)
        {
            _db.OnLog -= handler;
        }

        /// <summary>
        /// Gets the db.
        /// </summary>
        /// <value>The db.</value>
        public Database Db
        {
            get
            {
                return _db;
            }
        }


        /// <summary>
        /// Builds the name of the db param.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The name of the db param</returns>
        public string BuildDbParamName(string name)
        {
            Check.Require(name, "name", Check.NotNullOrEmpty);

            return _db.DbProvider.BuildParameterName(name);
        }


        #endregion

        #region 事务

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public DbTrans BeginTransaction(int timeout = 120)
        {
            return new DbTrans(_db.BeginTransaction(), this, timeout);
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public DbTrans BeginTransaction(DbTransType type, int timeout = 120)
        {
            return new DbTrans(_db.BeginTransaction(type), this, timeout);
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public DbTrans<T> BeginTransaction<T>(int timeout = 120) where T : Entity
        {
            return new DbTrans<T>(_db.BeginTransaction(), this, timeout);
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public DbTrans<T> BeginTransaction<T>(DbTransType type, int timeout = 120) where T : Entity
        {
            return new DbTrans<T>(_db.BeginTransaction(type), this, timeout);
        }



        /// <summary>
        /// Closes the transaction.
        /// </summary>
        /// <param name="tran">The tran.</param>
        public void CloseTransaction(DbTransaction tran)
        {
            _db.CloseConnection(tran);
        }

        #endregion


        #region 更新操作

        /// <summary>
        /// 更新  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public int Update<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            if (!entity.IsModify())
                return 0;

            WhereExpression where = DataUtils.GetPrimaryKeyWhere(entity);

            Check.Require(!WhereExpression.IsNullOrEmpty(where), "entity must have the primarykey!");

            return Update(entity, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public List<int> Update<TEntity>(params TEntity[] entities)
            where TEntity : Entity
        {
            if (null == entities || entities.Length == 0)
                return null;
            var result = new List<int>();
            using (DbTrans trans = BeginTransaction<TEntity>())
            {
                result.Add(Update(trans, entities));
            }
            return result;
        }

        /// <summary>
        /// 更新  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public List<int> Update<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return Update(entities.ToArray());
        }
        /// <summary>
        /// 更新  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public async Task<List<int>> UpdateAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return await UpdateAsync(entities.ToArray());
        }
        /// <summary>
        /// 更新  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public List<int> Update<TEntity>(List<TEntity> entities)
            where TEntity : Entity
        {
            return Update(entities.ToArray());
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity entity, WhereExpression where)
            where TEntity : Entity
        {
            return !entity.IsModify()
                ? 0
                : ExecuteNonQuery(_cmdCreator.CreateUpdateCommand(entity, @where));
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <param name="joinOn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, Dictionary<Field, object> data, JoinOn joinOn, WhereExpression where)
           where TEntity : Entity
        {
            return Update<TEntity>(tran, tableName, data.Keys.ToArray(), data.Values.ToArray(), JoinOn.None, where);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <param name="joinOn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(DbTransaction tran, string tableName, Dictionary<Field, object> data, JoinOn joinOn, WhereExpression where)
           where TEntity : Entity
        {
            return await UpdateAsync<TEntity>(tran, tableName, data.Keys.ToArray(), data.Values.ToArray(), JoinOn.None, where);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Dictionary<Field, object> data, WhereExpression where)
          where TEntity : Entity
        {
            return Update<TEntity>(null, tableName, data, JoinOn.None, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="joinOn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity entity, JoinOn joinOn, WhereExpression where)
           where TEntity : Entity
        {
            return !entity.IsModify()
                ? 0
                : ExecuteNonQuery(_cmdCreator.CreateUpdateCommand(entity, joinOn, @where));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity1"></typeparam>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="entity"></param>
        /// <param name="joinOn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity1, TEntity2>(TEntity1 entity, JoinOn<TEntity1, TEntity2> joinOn, WhereExpression where)
           where TEntity1 : Entity
        {
            return !entity.IsModify()
                ? 0
                : ExecuteNonQuery(_cmdCreator.CreateUpdateCommand(entity, joinOn, @where));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity1"></typeparam>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="entity"></param>
        /// <param name="joinOn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity1, TEntity2>(TEntity1 entity, JoinOn<TEntity1, TEntity2> joinOn, WhereExpression where)
           where TEntity1 : Entity
        {
            return !entity.IsModify()
                ? 0
                : await ExecuteNonQueryAsync(_cmdCreator.CreateUpdateCommand(entity, joinOn, @where));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update(entity, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return await UpdateAsync(entity, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="lambadaSelect"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(Expression<Func<TEntity, object>> lambadaSelect, Expression<Func<TEntity, bool>> lambdaWhere)
                    where TEntity : Entity
        {
            return Update(EntityCache.GetTableName<TEntity>(), lambadaSelect, lambdaWhere);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="lambadaSelect"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(Expression<Func<TEntity, object>> lambadaSelect, Expression<Func<TEntity, bool>> lambdaWhere)
                    where TEntity : Entity
        {
            return await UpdateAsync(EntityCache.GetTableName<TEntity>(), lambadaSelect, lambdaWhere);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="lambadaSelect"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Expression<Func<TEntity, object>> lambadaSelect, Expression<Func<TEntity, bool>> lambdaWhere)
                    where TEntity : Entity
        {
            return Update(null, tableName, lambadaSelect, lambdaWhere);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="lambadaSelect"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(string tableName, Expression<Func<TEntity, object>> lambadaSelect, Expression<Func<TEntity, bool>> lambdaWhere)
                    where TEntity : Entity
        {
            return await UpdateAsync(null, tableName, lambadaSelect, lambdaWhere);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="lambadaSelect"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, Expression<Func<TEntity, object>> lambadaSelect, Expression<Func<TEntity, bool>> lambdaWhere)
                   where TEntity : Entity
        {
            return Update(tran, tableName, lambadaSelect, JoinOn.None, lambdaWhere);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="lambadaSelect"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(DbTransaction tran, string tableName, Expression<Func<TEntity, object>> lambadaSelect, Expression<Func<TEntity, bool>> lambdaWhere)
                   where TEntity : Entity
        {
            return await UpdateAsync(tran, tableName, lambadaSelect, JoinOn.None, lambdaWhere);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="lambadaSelect"></param>
        /// <param name="joinOn"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, Expression<Func<TEntity, object>> lambadaSelect, JoinOn joinOn, Expression<Func<TEntity, bool>> lambdaWhere)
                  where TEntity : Entity
        {
            return Update<TEntity>(tran, tableName, lambadaSelect.GetFieldVals(tableName), joinOn, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="lambadaSelect"></param>
        /// <param name="joinOn"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(DbTransaction tran, string tableName, Expression<Func<TEntity, object>> lambadaSelect, JoinOn joinOn, Expression<Func<TEntity, bool>> lambdaWhere)
                  where TEntity : Entity
        {
            return await UpdateAsync<TEntity>(tran, tableName, lambadaSelect.GetFieldVals(tableName), joinOn, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity entity, Where where)
            where TEntity : Entity
        {
            return Update(entity, where.ToWhereClip());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entity"></param>
        public int Update<TEntity>(DbTransaction tran, TEntity entity)
            where TEntity : Entity
        {
            if (!entity.IsModify())
                return 0;
            return Update(tran, entity.GetTableName(), entity);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, TEntity entity)
            where TEntity : Entity
        {
            if (!entity.IsModify())
                return 0;

            WhereExpression where = DataUtils.GetPrimaryKeyWhere(entity);

            Check.Require(!WhereExpression.IsNullOrEmpty(where), "entity must have the primarykey!");

            return Update(tran, tableName, entity, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entities"></param>
        public int Update<TEntity>(DbTransaction tran, params TEntity[] entities)
            where TEntity : Entity
        {

            if (null == entities || entities.Length == 0)
                return 0;
            return Update(tran, entities.First().GetTableName(), entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entities"></param>
        public async Task<int> UpdateAsync<TEntity>(DbTransaction tran, params TEntity[] entities)
            where TEntity : Entity
        {

            if (null == entities || entities.Length == 0)
                return 0;
            return await UpdateAsync(tran, entities.First().GetTableName(), entities);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, params TEntity[] entities)
           where TEntity : Entity
        {

            if (null == entities || entities.Length == 0)
                return 0;
            int count = 0;
            foreach (TEntity entity in entities)
            {
                if (!entity.IsModify())
                    continue;
                count += Update(tran, tableName, entity, DataUtils.GetPrimaryKeyWhere(entity));
            }
            return count;

        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(DbTransaction tran, string tableName, params TEntity[] entities)
           where TEntity : Entity
        {

            if (null == entities || entities.Length == 0)
                return 0;
            int count = 0;
            foreach (TEntity entity in entities)
            {
                if (!entity.IsModify())
                    continue;
                count += await UpdateAsync(tran, tableName, entity, DataUtils.GetPrimaryKeyWhere(entity));
            }
            return count;

        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entities"></param>
        public int Update<TEntity>(DbTransaction tran, List<TEntity> entities)
            where TEntity : Entity
        {
            return Update(tran, entities.First().GetTableName(), entities);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, List<TEntity> entities)
           where TEntity : Entity
        {
            return Update(tran, tableName, entities.ToArray());
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entities"></param>
        public int Update<TEntity>(DbTransaction tran, IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return Update(tran, entities.ToArray());
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, TEntity entity, WhereExpression where)
            where TEntity : Entity
        {
            if (!entity.IsModify())
                return 0;
            return ExecuteNonQuery(_cmdCreator.CreateUpdateCommand(entity, where), tran);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, TEntity entity, WhereExpression where)
           where TEntity : Entity
        {
            if (!entity.IsModify())
                return 0;
            return ExecuteNonQuery(_cmdCreator.CreateUpdateCommand(tableName, entity, where), tran);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(DbTransaction tran, string tableName, TEntity entity, WhereExpression where)
           where TEntity : Entity
        {
            if (!entity.IsModify())
                return 0;
            return await ExecuteNonQueryAsync(_cmdCreator.CreateUpdateCommand(tableName, entity, where), tran);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, TEntity entity, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update(tran, entity.GetTableName(), entity, lambdaWhere);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, TEntity entity, Expression<Func<TEntity, bool>> lambdaWhere)
           where TEntity : Entity
        {
            return Update(tran, tableName, entity, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }

        /// <summary>
        /// 更新
        /// </summary>
        public int Update<TEntity>(DbTransaction tran, TEntity entity, Where where)
            where TEntity : Entity
        {
            return Update(tran, entity, where.ToWhereClip());
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Field field, object value, WhereExpression where)
            where TEntity : Entity
        {
            if (Field.IsNullOrEmpty(field))
                return 0;

            return ExecuteNonQuery(_cmdCreator.CreateUpdateCommand<TEntity>(tableName, new Field[] { field }, new object[] { value }, null, where));
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Field field, object value, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(tableName, field, value, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Field field, object value, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(tableName, field, value, where.ToWhereClip());
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, Field field, object value, WhereExpression where)
            where TEntity : Entity
        {
            if (Field.IsNullOrEmpty(field))
                return 0;

            return Update<TEntity>(tran, tableName, new Field[] { field }, new object[] { value }, JoinOn.None, where);
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, Field field, object value, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, tableName, field, value, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, Field field, object value, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, tableName, field, value, where.ToWhereClip());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Field[] fields, object[] values, WhereExpression where)
            where TEntity : Entity
        {
            return Update<TEntity>(null, tableName, fields, values, JoinOn.None, where);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Field[] fields, object[] values, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(tableName, fields, values, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Field[] fields, object[] values, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(tableName, fields, values, where.ToWhereClip());
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="joinOn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, Field[] fields, object[] values, JoinOn joinOn, WhereExpression where)
            where TEntity : Entity
        {
            if (null == fields || fields.Length == 0)
                return 0;
            if (tran == null)
                return ExecuteNonQuery(_cmdCreator.CreateUpdateCommand<TEntity>(tableName, fields, values, joinOn, where));
            return ExecuteNonQuery(_cmdCreator.CreateUpdateCommand<TEntity>(tableName, fields, values, joinOn, where), tran);
        }

        // <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="joinOn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(DbTransaction tran, string tableName, Field[] fields, object[] values, JoinOn joinOn, WhereExpression where)
            where TEntity : Entity
        {
            if (null == fields || fields.Length == 0)
                return 0;
            if (tran == null)
                return await ExecuteNonQueryAsync(_cmdCreator.CreateUpdateCommand<TEntity>(tableName, fields, values, joinOn, where));
            return await ExecuteNonQueryAsync(_cmdCreator.CreateUpdateCommand<TEntity>(tableName, fields, values, joinOn, where), tran);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, Field[] fields, object[] values, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, tableName, fields, values, JoinOn.None, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, Field[] fields, object[] values, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, tableName, fields, values, JoinOn.None, where.ToWhereClip());
        }
        #endregion

        #region 删除操作


        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            return Delete(entity.GetTableName(), entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete<TEntity>(string tableName, TEntity entity)
            where TEntity : Entity
        {
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", tableName, ") is readonly!"));

            WhereExpression where = DataUtils.GetPrimaryKeyWhere(entity);

            Check.Require(!WhereExpression.IsNullOrEmpty(where), "entity must have the primarykey!");

            return Delete<TEntity>(tableName, where);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Delete<TEntity>(Expression<Func<TEntity, bool>> lambdaWhere)
             where TEntity : Entity
        {
            return Delete(EntityCache.GetTableName<TEntity>(), lambdaWhere);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> lambdaWhere)
             where TEntity : Entity
        {
            return await DeleteAsync(EntityCache.GetTableName<TEntity>(), lambdaWhere);
        }


        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Delete<TEntity>(DbTransaction tran, TEntity entity)
            where TEntity : Entity
        {
            var tableName = entity.GetTableName();

            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", tableName, ") is readonly!"));

            return Delete<TEntity>(tran, tableName, DataUtils.GetPrimaryKeyWhere(entity));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entities"></param>
        public int Delete<TEntity>(DbTransaction tran, params TEntity[] entities)
            where TEntity : Entity
        {
            return Delete(tran, EntityCache.GetTableName<TEntity>(), entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Delete<TEntity>(DbTransaction tran, string tableName, params TEntity[] entities)
            where TEntity : Entity
        {
            if (entities == null || entities.Length < 1) return 0;
            var listKey = new List<object>();
            var where = new Where();
            var f = entities.First().GetPrimaryKeyFields().First();
            foreach (var entity in entities)
            {
                if (entity == null) continue;
                listKey.Add(DataUtils.GetPropertyValue(entity, f.Name));
            }
            where.And(f.In(listKey));
            return Delete<TEntity>(tran, tableName, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entities"></param>
        public int Delete<TEntity>(DbTransaction tran, List<TEntity> entities)
            where TEntity : Entity
        {
            return Delete(tran, entities.ToArray());
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Delete<TEntity>(DbTransaction tran, string tableName, List<TEntity> entities)
           where TEntity : Entity
        {
            return Delete(tran, tableName, entities.ToArray());
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entities"></param>
        public int Delete<TEntity>(DbTransaction tran, IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return Delete(tran, entities.First().GetTableName(), entities);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Delete<TEntity>(DbTransaction tran, string tableName, IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return Delete(tran, tableName, entities.ToArray());
        }

        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(string tableName, params int[] pkValues)
            where TEntity : Entity
        {
            return DeleteByPrimaryKey<TEntity>(tableName, pkValues.ToArray());
        }
        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(string tableName, params Guid[] pkValues)
            where TEntity : Entity
        {
            return DeleteByPrimaryKey<TEntity>(tableName, pkValues.ToArray());
        }
        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(string tableName, params long[] pkValues)
            where TEntity : Entity
        {
            return DeleteByPrimaryKey<TEntity>(tableName, pkValues.ToArray());
        }
        /// <summary>
        /// 删除
        /// </summary>
        public int Delete<TEntity>(string tableName, params string[] pkValues)
            where TEntity : Entity
        {
            return DeleteByPrimaryKey<TEntity>(tableName, pkValues.ToArray());
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Delete<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return Delete(EntityCache.GetTableName<TEntity>(), entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return await DeleteAsync(EntityCache.GetTableName<TEntity>(), entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Delete<TEntity>(string tableName, IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            var arr = entities as TEntity[] ?? entities.ToArray();
            var eCount = arr.Length;
            switch (eCount)
            {
                case 0:
                    return 0;
                case 1:
                    return Delete(tableName, arr.First());
                default:
                    var listKey = new List<object>();
                    var where = new Where();
                    var f = arr.First().GetPrimaryKeyFields().First();
                    foreach (var entity in arr)
                    {
                        listKey.Add(DataUtils.GetPropertyValue(entity, f.Name));
                    }
                    where.And(f.In(listKey));
                    return Delete<TEntity>(tableName, where);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync<TEntity>(string tableName, IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            var arr = entities as TEntity[] ?? entities.ToArray();
            var eCount = arr.Length;
            switch (eCount)
            {
                case 0:
                    return 0;
                case 1:
                    return await DeleteAsync(tableName, arr.First());
                default:
                    var listKey = new List<object>();
                    var where = new Where();
                    var f = arr.First().GetPrimaryKeyFields().First();
                    foreach (var entity in arr)
                    {
                        listKey.Add(DataUtils.GetPropertyValue(entity, f.Name));
                    }
                    where.And(f.In(listKey));
                    return await DeleteAsync<TEntity>(tableName, where);
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        internal int DeleteByPrimaryKey<TEntity>(string tableName, Array pkValues)
            where TEntity : Entity
        {
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", tableName, ") is readonly!"));


            return ExecuteNonQuery(_cmdCreator.CreateDeleteCommand(tableName, EntityCache.GetUserName<TEntity>(), DataUtils.GetPrimaryKeyWhere<TEntity>(pkValues)));
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pkValues"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Delete<TEntity>(DbTransaction tran, params int[] pkValues)
            where TEntity : Entity
        {
            return DeleteByPrimaryKey<TEntity>(tran, pkValues);
        }
        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pkValues"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Delete<TEntity>(DbTransaction tran, params long[] pkValues)
            where TEntity : Entity
        {
            return DeleteByPrimaryKey<TEntity>(tran, pkValues);
        }
        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pkValues"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Delete<TEntity>(DbTransaction tran, params Guid[] pkValues)
            where TEntity : Entity
        {
            return DeleteByPrimaryKey<TEntity>(tran, pkValues);
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pkValues"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Delete<TEntity>(DbTransaction tran, params string[] pkValues)
            where TEntity : Entity
        {
            return DeleteByPrimaryKey<TEntity>(tran, pkValues);
        }


        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pkValues"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        internal int DeleteByPrimaryKey<TEntity>(DbTransaction tran, params Array[] pkValues)
            where TEntity : Entity
        {
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));
            return ExecuteNonQuery(_cmdCreator.CreateDeleteCommand(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), DataUtils.GetPrimaryKeyWhere<TEntity>(pkValues)), tran);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete<TEntity>(DbTransaction tran, string tableName, WhereExpression where)
            where TEntity : Entity
        {
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = EntityCache.GetTableName<TEntity>();
            }
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", tableName, ") is readonly!"));
            if (tran == null)
            {
                return ExecuteNonQuery(_cmdCreator.CreateDeleteCommand(tableName, EntityCache.GetUserName<TEntity>(), where));
            }
            return ExecuteNonQuery(_cmdCreator.CreateDeleteCommand(tableName, EntityCache.GetUserName<TEntity>(), where), tran);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete<TEntity>(DbTransaction tran, string tableName, Where where)
            where TEntity : Entity
        {
            return Delete<TEntity>(tran, tableName, where.ToWhereClip());
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete<TEntity>(DbTransaction tran, Where where)
            where TEntity : Entity
        {
            return Delete<TEntity>(tran, EntityCache.GetTableName<TEntity>(), where);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Delete<TEntity>(string tableName, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Delete<TEntity>(tableName, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync<TEntity>(string tableName, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return await DeleteAsync<TEntity>(tableName, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Delete<TEntity>(DbTransaction tran, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Delete(tran, EntityCache.GetTableName<TEntity>(), lambdaWhere);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Delete<TEntity>(DbTransaction tran, string tableName, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Delete<TEntity>(tran, tableName, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(string tableName, WhereExpression where)
            where TEntity : Entity
        {
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));

            return ExecuteNonQuery(_cmdCreator.CreateDeleteCommand(tableName ?? EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where));
        }

        /// <summary>
        /// 删除
        /// </summary>
        public int Delete<TEntity>(string tableName, Where where)
            where TEntity : Entity
        {
            return Delete<TEntity>(tableName, where.ToWhereClip());
        }

        /// <summary>
        /// 删除
        /// </summary>
        public async Task<int> DeleteAsync<TEntity>(string tableName, Where where)
            where TEntity : Entity
        {
            return await DeleteAsync<TEntity>(tableName, where.ToWhereClip());
        }

        /// <summary>
        /// 删除整表数据
        /// </summary>
        public int DeleteAll<TEntity>(string tableName)
            where TEntity : Entity
        {
            return Delete<TEntity>(tableName, d => true);
        }

        #endregion

        #region 添加操作
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public List<int> Insert<TEntity>(params TEntity[] entities)
            where TEntity : Entity
        {
            if (null == entities || entities.Length == 0)
                return null;
            var result = new List<int>();
            using (var trans = BeginTransaction<TEntity>())
            {
                result.Add(Insert(trans, entities));
            }
            return result;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public List<int> Insert<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return Insert(entities.ToArray());
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<List<int>> InsertAsync<TEntity>(IEnumerable<TEntity> entities)
        {
            return await InsertAsync(entities.ToArray());
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public List<int> Insert<TEntity>(string tableName, IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return Insert(tableName, entities.ToArray());
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public List<int> Insert<TEntity>(List<TEntity> entities)
            where TEntity : Entity
        {
            return Insert(entities.ToArray());
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            return InsertExecute<TEntity>(_cmdCreator.CreateInsertCommand(entity));
        }



        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Insert<TEntity>(DbTransaction tran, TEntity entity)
            where TEntity : Entity
        {
            return InsertExecute<TEntity>(_cmdCreator.CreateInsertCommand<TEntity>(entity), tran);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Insert<TEntity>(DbTransaction tran, params TEntity[] entities)
            where TEntity : Entity
        {
            if (null == entities || entities.Length == 0)
                return 0;
            return Insert(tran, entities.First().GetTableName(), entities);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Insert<TEntity>(DbTransaction tran, string tableName, params TEntity[] entities)
           where TEntity : Entity
        {
            if (null == entities || entities.Length == 0)
                return 0;
            int count = 0;
            foreach (TEntity entity in entities)
            {
                var tcount = InsertExecute<TEntity>(_cmdCreator.CreateInsertCommand(tableName, entity), tran);
                if (tcount > 1)
                    count = tcount;
                else
                    count += tcount;
            }
            return count;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Insert<TEntity>(DbTransaction tran, List<TEntity> entities)
            where TEntity : Entity
        {
            return Insert(tran, entities.ToArray());
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Insert<TEntity>(DbTransaction tran, IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return Insert(tran, entities.ToArray());
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Insert<TEntity>(DbTransaction tran, string tableName, IEnumerable<TEntity> entities)
           where TEntity : Entity
        {
            return Insert(tran, tableName, entities.ToArray());
        }

        /// <summary>
        /// insert
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public int Insert<TEntity>(string tableName, Field[] fields, object[] values)
            where TEntity : Entity
        {
            return InsertExecute<TEntity>(_cmdCreator.CreateInsertCommand<TEntity>(tableName, fields, values));
        }

        /// <summary>
        /// insert
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public int Insert<TEntity>(DbTransaction tran, string tableName, Field[] fields, object[] values)
            where TEntity : Entity
        {
            return InsertExecute<TEntity>(_cmdCreator.CreateInsertCommand<TEntity>(tableName, fields, values), tran);
        }

        /// <summary>
        /// InsertExecute
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private int InsertExecute<TEntity>(DbCommand cmd)
             where TEntity : Entity
        {
            int returnValue = 0;

            if (null == cmd)
                return returnValue;
            returnValue = InsertExecute<TEntity>(cmd, null);
            return returnValue;
        }

        /// <summary>
        /// InsertExecute
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private int InsertExecute<TEntity>(DbCommand cmd, DbTransaction tran)
             where TEntity : Entity
        {
            if (null == cmd)
                return 0;

            Field identity = EntityCache.GetIdentityField<TEntity>();
            if (Field.IsNullOrEmpty(identity))
            {
                return tran == null ? ExecuteNonQuery(cmd) : ExecuteNonQuery(cmd, tran);
            }
            else
            {
                object scalarValue = null;
                if (Db.DbProvider.GetType().Name == "MsAccessProvider")
                {
                    if (tran == null)
                    {
                        ExecuteNonQuery(cmd);
                        scalarValue =
                            ExecuteScalar(
                                _db.GetSqlStringCommand(string.Format("select max({0}) from {1}", identity.FieldName,
                                    identity.TableName))); //Max<TEntity, int>(identity, WhereClip.All) + 1;
                    }
                    else
                    {
                        ExecuteNonQuery(cmd, tran);
                        scalarValue = ExecuteScalar(_db.GetSqlStringCommand(string.Format("select max({0}) from {1}", identity.FieldName, identity.TableName)), tran); //Max<TEntity, int>(identity, WhereClip.All) + 1;
                    }

                }
                else if (Db.DbProvider.GetType().Name == "OracleProvider")
                {
                    if (tran == null)
                    {
                        ExecuteNonQuery(cmd);
                        scalarValue =
                            ExecuteScalar(
                                _db.GetSqlStringCommand(string.Format(_db.DbProvider.RowAutoID,
                                    EntityCache.GetSequence<TEntity>())));
                    }
                    else
                    {
                        ExecuteNonQuery(cmd, tran);
                        scalarValue = ExecuteScalar(_db.GetSqlStringCommand(string.Format(_db.DbProvider.RowAutoID, EntityCache.GetSequence<TEntity>())), tran);
                    }
                }
                else
                {
                    if (Db.DbProvider.SupportBatch)
                    {
                        if (tran == null)
                        {
                            cmd.CommandText = string.Concat(cmd.CommandText, ";", _db.DbProvider.RowAutoID);
                            scalarValue = ExecuteScalar(cmd);
                        }
                        else
                        {
                            cmd.CommandText = string.Concat(cmd.CommandText, ";", _db.DbProvider.RowAutoID);
                            scalarValue = ExecuteScalar(cmd, tran);
                        }
                    }
                    else
                    {
                        if (tran == null)
                        {
                            ExecuteNonQuery(cmd);
                            scalarValue = ExecuteScalar(_db.GetSqlStringCommand(Db.DbProvider.RowAutoID));
                        }
                        else
                        {
                            ExecuteNonQuery(cmd, tran);
                            scalarValue = ExecuteScalar(_db.GetSqlStringCommand(Db.DbProvider.RowAutoID), tran);
                        }
                    }
                }

                if (null == scalarValue || Convert.IsDBNull(scalarValue))
                    return 0;
                return Convert.ToInt32(scalarValue);
            }
        }
        #endregion

        #region 批量

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>

        public void BulkInsert<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        {
            if (entities == null || !entities.Any()) return;

            var batch = CreateBatch<TEntity>();

            batch.Insert(entities);

            batch.Execute();
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public int BulkInsert(string tableName, DataTable dataTable)
        {
            return _db.BulkInsert(tableName, dataTable);
        }
        #endregion

        #region Save操作
        /// <summary>
        /// 将实体批量提交至数据库，内置事务。每个实体需要手动标记EntityState状态。
        /// </summary>
        public int Save<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            int count = 0;
            using (DbTrans trans = this.BeginTransaction<TEntity>())
            {
                foreach (var entity in entities)
                {
                    var es = entity.GetEntityState();
                    switch (es)
                    {
                        case EntityState.Added:
                            count += Insert(trans, entity);
                            break;
                        case EntityState.Deleted:
                            count += Delete(trans, entity);
                            break;
                        case EntityState.Modified:
                            count += Update(trans, entity);
                            break;
                    }
                }
            }
            return count;
        }
        /// <summary>
        ///保存实体。需要手动标记EntityState状态。
        /// </summary>
        public int Save<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            int count = 0;
            EntityState es = entity.GetEntityState();
            switch (es)
            {
                case EntityState.Added:
                    count = Insert(entity);
                    break;
                case EntityState.Deleted:
                    count = Delete(entity);
                    break;
                case EntityState.Modified:
                    count = Update(entity);
                    break;
            }
            return count;
        }
        /// <summary>
        /// 将实体批量提交至数据库。每个实体需要手动标记EntityState状态。
        /// </summary>
        public int Save<TEntity>(DbTransaction tran, IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            int count = 0;
            foreach (var entity in entities)
            {
                var es = entity.GetEntityState();
                switch (es)
                {
                    case EntityState.Added:
                        count += Insert(tran, entity);
                        break;
                    case EntityState.Deleted:
                        count += Delete(tran, entity);
                        break;
                    case EntityState.Modified:
                        count += Update(tran, entity);
                        break;
                }
            }
            return count;
        }
        /// <summary>
        ///保存实体。需要手动标记EntityState状态。
        /// </summary>
        public int Save<TEntity>(DbTransaction tran, TEntity entity)
            where TEntity : Entity
        {
            int count = 0;
            EntityState es = entity.GetEntityState();
            switch (es)
            {
                case EntityState.Added:
                    count = Insert(tran, entity);
                    break;
                case EntityState.Deleted:
                    count = Delete(tran, entity);
                    break;
                case EntityState.Modified:
                    count = Update(tran, entity);
                    break;
            }
            return count;
        }
        #endregion

        #region 执行command


        /// <summary>
        /// 执行ExecuteNonQuery
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbCommand cmd)
        {
            return _db.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 执行ExecuteNonQuery
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbCommand cmd, DbTransaction tran)
        {
            return _db.ExecuteNonQuery(cmd, tran);
        }

        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public object ExecuteScalar(DbCommand cmd, DbTransaction tran)
        {
            return _db.ExecuteScalar(cmd, tran);
        }

        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public object ExecuteScalar(DbCommand cmd)
        {
            return _db.ExecuteScalar(cmd);
        }

        /// <summary>
        /// 执行ExecuteReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, Dictionary<string, object> dbParameters)
        {
            return _db.ExecuteReader(sql, dbParameters);
        }

        /// <summary>
        /// 执行ExecuteReader
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(DbCommand cmd)
        {
            return _db.ExecuteReader(cmd);
        }


        /// <summary>
        /// 执行ExecuteReader
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(DbCommand cmd, DbTransaction tran)
        {
            return _db.ExecuteReader(cmd, tran);
        }

        /// <summary>
        /// 执行ExecuteReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, DbTransaction transaction, Dictionary<string, object> dbParameters)
        {
            return _db.ExecuteReader(sql, transaction, dbParameters);
        }

        /// <summary>
        /// 执行ExecuteDataSet
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbCommand cmd)
        {
            return _db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 执行ExecuteDataSet
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public async Task<DataSet> ExecuteDataSetAsync(DbCommand cmd)
        {
            return await _db.ExecuteDataSetAsync(cmd);
        }

        /// <summary>
        /// 执行ExecuteDataSet
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbCommand cmd, DbTransaction tran)
        {
            return _db.ExecuteDataSet(cmd, tran);
        }

        #endregion

        #region 存储过程

        /// <summary>
        /// 存储过程查询
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public ProcSection FromProc(string procName)
        {
            return new ProcSection(this, procName);
        }

        /// <summary>
        /// 存储过程查询,带参数
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="inputParamas"></param>
        /// <returns></returns>
        public ProcSection FromProc(string procName, Dictionary<string, object> inputParamas)
        {
            return new ProcSection(this, procName).AddInParameter(inputParamas);
        }

        /// <summary>
        /// 存储过程查询,带参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="inputParamas"></param>
        /// <returns></returns>
        public ProcSection FromProc<T>(string procName, T inputParamas) where T : class, new()
        {
            return new ProcSection(this, procName).AddInParameterWithModel(inputParamas);
        }

        #endregion

        #region sql语句


        /// <summary>
        /// 执行ExecuteDataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, params DbParameter[] dbParameters)
        {
            var ds = _db.ExecuteDataSet(sql, dbParameters);
            if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }


        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql, params DbParameter[] dbParameters)
        {
            return _db.ExecuteDataSet(sql, dbParameters);
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params DbParameter[] dbParameters)
        {
            return _db.ExecuteNonQuery(sql, dbParameters);
        }
        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, DbTransaction transaction, params DbParameter[] dbParameters)
        {
            return _db.ExecuteNonQuery(sql, transaction, dbParameters);
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, DbTransaction transaction, Dictionary<string, object> dbParameters)
        {
            return _db.ExecuteNonQuery(sql, transaction, dbParameters);
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, Dictionary<string, object> dbParameters)
        {
            return _db.ExecuteNonQuery(sql, dbParameters);
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery<Model>(string sql, Model dbParameters) where Model : class, new()
        {
            var keyValuePairs = new Dictionary<string, object>();
            var type = typeof(Model);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var value = DynamicCalls.GetPropertyGetter(property).Invoke(dbParameters);
                if (keyValuePairs.ContainsKey($"{ParamPrefix}{property.Name}"))
                {
                    keyValuePairs[$"{ParamPrefix}{property.Name}"] = value;
                }
                else
                {
                    keyValuePairs.Add($"{ParamPrefix}{property.Name}", value);
                }
            }

            return _db.ExecuteNonQuery(sql, keyValuePairs);
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params DbParameter[] dbParameters)
        {
            return _db.ExecuteScalar(sql, dbParameters);
        }

        /// <summary>
        /// 执行ExecuteReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, params DbParameter[] dbParameters)
        {
            return _db.ExecuteReader(sql, dbParameters);
        }

        /// <summary>
        /// 从sql中获取实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public List<T> ToList<T>(string sql, params DbParameter[] dbParameters)
        {
            return ExecuteReader(sql, dbParameters).ReaderToList<T>();
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql, params Parameter[] parameters)
        {
            return new SqlSection(this, sql, parameters);
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public SqlSection FromSqlWithdAutomatic(string sql, params KeyValuePair<string, object>[] keyValuePairs)
        {
            return new SqlSection(this, sql, keyValuePairs);
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public SqlSection FromSqlWithdModel<T>(string sql, T parameter) where T : class, new()
        {
            List<KeyValuePair<string, object>> keyValuePairs = new List<KeyValuePair<string, object>>();
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var value = DynamicCalls.GetPropertyGetter(property).Invoke(parameter);
                keyValuePairs.Add(new KeyValuePair<string, object>($"{ParamPrefix}{property.Name}", value));
            }
            return FromSqlWithdAutomatic(sql).AddInParameterWithModel(parameter);
        }
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql, int pageIndex, int pageSize, string orderBy, bool asc = true)
        {
            return new SqlSection(this, sql, pageIndex, pageSize, orderBy, asc);
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBys"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql, int pageIndex, int pageSize, Dictionary<string, OrderByOperater> orderBys, params Parameter[] parameters)
        {
            return new SqlSection(this, sql, pageIndex, pageSize, orderBys, parameters);
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBys"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql, int pageIndex, int pageSize, Dictionary<string, OrderByOperater> orderBys, KeyValuePair<string, object>[] keyValuePairs)
        {
            return new SqlSection(this, sql, pageIndex, pageSize, orderBys, keyValuePairs);
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBys"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public SqlSection FromSql<T>(string sql, int pageIndex, int pageSize, Dictionary<string, OrderByOperater> orderBys, T parameter) where T : class, new()
        {
            return new SqlSection(this, sql, pageIndex, pageSize, orderBys).AddInParameterWithModel(parameter);
        }

        #endregion

        #region 参数

        /// <summary>
        /// CreateParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <param name="nullable"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            return _db.CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
        }

        /// <summary>
        /// CreateParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, DbType dbType, int size, object value)
        {
            return CreateParameter(name, dbType, size, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, value);
        }
        #endregion

        #region 单库分布式锁

        static object _locker = new object();
        /// <summary>
        /// 创建分布式锁
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="lockTimeout"></param>
        /// <returns></returns>
        public DistributedLock CreateLock(string keyName, int lockTimeout = 180)
        {
            lock (_locker)
            {
                var dl = new DistributedLock(this, keyName, lockTimeout);
                while (!dl.AcquireLock())
                {
                    Thread.Sleep(10);
                }
                return dl;
            }
        }

        #endregion

        #region 数据更新

        AdoBatcher _adoBatcher = null;

        /// <summary>
        /// 获取源数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="size"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public DataTable ReadDataSource(string tableName, int size = 100, int timeOut = 180)
        {
            _adoBatcher = new AdoBatcher(_db, tableName, timeOut);

            return _adoBatcher.Read(size);
        }

        /// <summary>
        /// 更新源数据
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public int WriteDataSource(DataTable dataTable)
        {
            if (_adoBatcher != null)
            {
                return _adoBatcher.Write(dataTable);
            }
            return -1;
        }

        #endregion

        #region 数据导入导出

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<int> Import<TEntity>(string filePath) where TEntity : Entity
        {
            var dataTable = DataTableHelper.ReadFromCSV(filePath);

            var list = dataTable.DataTableToEntityList<TEntity>();

            return Insert(list);
        }
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="withHeader"></param>
        public void Export<TEntity>(string filePath, bool withHeader = false) where TEntity : Entity
        {
            var list = Search<TEntity>().ToList();

            var dataTable = list.EntitiesToDataTable();

            dataTable.WriteToCSV(filePath, withHeader);
        }

        #endregion

        #region 复制表

        /// <summary>
        /// 按天复制表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="isTruncate"></param>
        /// <returns></returns>
        public bool CopyTableByDay<TEntity>(bool isTruncate = false) where TEntity : Entity
        {
            var talbleName = EntityCache.GetTableName<TEntity>();
            var newTalbeName = $"{talbleName}_{DateTime.Now.ToString("yyyyMMdd")}";
            return CopyTable(talbleName, newTalbeName, isTruncate);
        }

        /// <summary>
        /// 按天复制表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="isTruncate"></param>
        /// <returns></returns>
        public async Task<bool> CopyTableByDayAsync<TEntity>(bool isTruncate = false) where TEntity : Entity
        {
            var talbleName = EntityCache.GetTableName<TEntity>();
            var newTalbeName = $"{talbleName}_{DateTime.Now.ToString("yyyyMMdd")}";
            return await CopyTableAsync(talbleName, newTalbeName, isTruncate);
        }

        /// <summary>
        /// 复制表
        /// </summary>
        /// <param name="talbName"></param>
        /// <param name="newTableName"></param>
        /// <param name="isTruncate"></param>
        /// <returns></returns>
        public bool CopyTable(string talbName, string newTableName, bool isTruncate = false)
        {
            var exist = _db.DbProvider.IsTableExist(newTableName);
            if (!exist)
            {
                if (_db.DbProvider.CopyTable(talbName, newTableName))
                {
                    if (isTruncate)
                    {
                        _db.DbProvider.TruncateTable(talbName);
                        return true;

                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 复制表
        /// </summary>
        /// <param name="talbName"></param>
        /// <param name="newTableName"></param>
        /// <param name="isTruncate"></param>
        /// <returns></returns>
        public async Task<bool> CopyTableAsync(string talbName, string newTableName, bool isTruncate = false)
        {
            var exist = _db.DbProvider.IsTableExist(newTableName);
            if (!exist)
            {
                if (await _db.DbProvider.CopyTableAsync(talbName, newTableName))
                {
                    if (isTruncate)
                    {
                        await _db.DbProvider.TruncateTableAsync(talbName);
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            CallContext.FreeNamedDataSlot(CONTEXTKEY);
        }
    }
}