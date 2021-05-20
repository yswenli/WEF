/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2019
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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

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

        /// <summary>
        /// 
        /// </summary>
        private Database _db;

        /// <summary>
        /// 
        /// </summary>
        private CommandCreator cmdCreator;

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
        /// <param name="batchSize">1000</param>
        /// <returns></returns>
        public IBatcher<T> CreateBatch<T>() where T : Entity
        {
            return BatcherFactory.CreateBatcher<T>(_db);
        }
        #endregion

        private static DbProvider CreateDbProvider(DatabaseType dt, string connStr)
        {
            DbProvider provider = null;
            switch (dt)
            {
                case DatabaseType.SqlServer9:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(SqlServer9Provider).FullName, connStr, dt);
                    break;
                case DatabaseType.SqlServer:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(SqlServerProvider).FullName, connStr, dt);
                    break;
                case DatabaseType.Oracle:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(OracleProvider).FullName, connStr, dt);
                    break;
                case DatabaseType.MariaDB:
                case DatabaseType.MySql:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(MySqlProvider).FullName, connStr, dt);
                    break;
                case DatabaseType.Sqlite3:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(SqliteProvider).FullName, connStr, dt);
                    break;
                case DatabaseType.MsAccess:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(MsAccessProvider).FullName, connStr, dt);
                    break;
                case DatabaseType.PostgreSQL:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(PostgreSqlProvider).FullName, connStr, dt);
                    break;
            }
            if (provider != null)
            {
                provider.DatabaseType = dt;
            }
            return provider;
        }

        #region 构造函数



        private void initDbSesion()
        {
            cmdCreator = new CommandCreator(_db);
        }

        /// <summary>
        /// 构造函数    使用默认  DBContext.Default
        /// </summary>
        /// <param name="timeOut"></param>
        public DBContext(int timeOut = 30)
        {
            _db = Database.Default;

            initDbSesion();

            CallContext.SetData(CONTEXTKEY, this);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connStrName">config文件中connectionStrings节点的name</param>
        /// <param name="timeout"></param>
        public DBContext(string connStrName, int timeout = 30)
        {
            this._db = new Database(ProviderFactory.CreateDbProvider(connStrName), timeout);
            this._db.DbProvider.ConnectionStringsName = connStrName;
            initDbSesion();
            CallContext.SetData(CONTEXTKEY, this);
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">已知的Database</param>
        /// <param name="timeout"></param>
        public DBContext(Database db, int timeout = 30)
        {
            this._db = db;

            initDbSesion();

            CallContext.SetData(CONTEXTKEY, this);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dt">数据库类别</param>
        /// <param name="connStr">连接字符串</param>
        /// <param name="timeout"></param>
        public DBContext(DatabaseType dt, string connStr, int timeout = 30)
        {
            DbProvider provider = CreateDbProvider(dt, connStr);

            this._db = new Database(provider, timeout);

            initDbSesion();

            CallContext.SetData(CONTEXTKEY, this);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="assemblyName">程序集</param>
        /// <param name="className">类名</param>
        /// <param name="connStr">连接字符串</param>
        /// <param name="timeout"></param>
        public DBContext(string assemblyName, string className, string connStr, int timeout = 30)
        {
            DbProvider provider = ProviderFactory.CreateDbProvider(assemblyName, className, connStr, null);
            if (provider == null)
            {
                throw new NotSupportedException(string.Format("Cannot construct DbProvider by specified parameters: {0}, {1}, {2}",
                    assemblyName, className, connStr));
            }

            this._db = new Database(provider, timeout);

            cmdCreator = new CommandCreator(_db);

            CallContext.SetData(CONTEXTKEY, this);
        }

        #endregion

        #region 查询


        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public ISearch<TEntity> Search<TEntity>(string tableName = "")
            where TEntity : Entity
        {
            return new Search<TEntity>(_db, null, tableName);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ISearch Search(string tableName)
        {
            return new Search(_db, tableName);
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ISearch<TEntity> Search<TEntity>(TEntity entity)
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
        public object Sum<TEntity>(Field field, WhereOperation where)
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
        public object Max<TEntity>(Field field, WhereOperation where)
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
        public object Min<TEntity>(Field field, WhereOperation where)
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
        public object Avg<TEntity>(Field field, WhereOperation where)
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
        public TResult Sum<TEntity, TResult>(Field field, WhereOperation where)
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
        public TResult Max<TEntity, TResult>(Field field, WhereOperation where)
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
        public TResult Min<TEntity, TResult>(Field field, WhereOperation where)
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
        public TResult Avg<TEntity, TResult>(Field field, WhereOperation where)
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
        public bool Exists<TEntity>(WhereOperation where)
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
            return Exists<TEntity>(ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere));
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
        public int Count<TEntity>(Field field, WhereOperation where)
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
            return Search<TEntity>().Select(field.Count()).Where(ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere)).ToScalar<int>();
        }
        /// <summary>
        /// Count
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count<TEntity>(WhereOperation where)
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
        /// <param name="tableName"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Count<TEntity>(string tableName, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(Field.All.Count()).Where(ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere)).ToScalar<int>();
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
                return this._db;
            }
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns>The begined transaction.</returns>
        public DbTrans BeginTransaction()
        {
            return new DbTrans(_db.BeginTransaction(), this);
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="il">The il.</param>
        /// <returns>The begined transaction.</returns>
        public DbTrans BeginTransaction(System.Data.IsolationLevel il)
        {
            return new DbTrans(_db.BeginTransaction(il), this);
        }

        /// <summary>
        /// Closes the transaction.
        /// </summary>
        /// <param name="tran">The tran.</param>
        public void CloseTransaction(DbTransaction tran)
        {
            _db.CloseConnection(tran);
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

        #region 更新操作

        /// <summary>
        /// 更新全部字段  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public int UpdateAll<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            WhereOperation where = DataUtils.GetPrimaryKeyWhere(entity);

            Check.Require(!WhereOperation.IsNullOrEmpty(where), "entity must have the primarykey!");

            return UpdateAll<TEntity>(entity, where);
        }

        /// <summary>
        /// 更新全部字段  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public void UpdateAll<TEntity>(params TEntity[] entities)
            where TEntity : Entity
        {
            if (null == entities || entities.Length == 0)
                return;

            using (DbTrans trans = BeginTransaction())
            {
                UpdateAll<TEntity>(trans, entities);

                trans.Commit();
            }
        }

        /// <summary>
        /// 更新全部字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entities"></param>
        public int UpdateAll<TEntity>(DbTransaction tran, params TEntity[] entities)
            where TEntity : Entity
        {

            if (null == entities || entities.Length == 0)
                return 0;
            var count = 0;
            foreach (TEntity entity in entities)
            {
                if (entity == null)
                    break;//2015-08-20  break修改为continue

                count += UpdateAll<TEntity>(tran, entity);
            }
            return count;
        }


        /// <summary>
        /// 更新全部字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int UpdateAll<TEntity>(TEntity entity, WhereOperation where)
            where TEntity : Entity
        {
            if (entity == null)
                return 0;

            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(entity.GetTableName(), entity.GetFields(), entity.GetValues(), where));
        }

        /// <summary>
        /// 更新全部字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entity"></param>
        public int UpdateAll<TEntity>(DbTransaction tran, TEntity entity)
            where TEntity : Entity
        {
            if (entity == null)
                return 0;

            WhereOperation where = DataUtils.GetPrimaryKeyWhere(entity);

            Check.Require(!WhereOperation.IsNullOrEmpty(where), "entity must have the primarykey!");

            return UpdateAll<TEntity>(tran, entity, where);
        }

        /// <summary>
        /// 更新全部字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="where"></param>
        /// <param name="entity"></param>
        public int UpdateAll<TEntity>(DbTransaction tran, TEntity entity, WhereOperation where)
            where TEntity : Entity
        {
            if (entity == null)
                return 0;

            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(entity.GetTableName(), entity.GetFields(), entity.GetValues(), where), tran);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int UpdateAll<TEntity>(DbTransaction tran, TEntity entity, Where where)
            where TEntity : Entity
        {
            return UpdateAll(tran, entity, where.ToWhereClip());
        }

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

            WhereOperation where = DataUtils.GetPrimaryKeyWhere(entity);

            Check.Require(!WhereOperation.IsNullOrEmpty(where), "entity must have the primarykey!");

            return Update<TEntity>(entity, where);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Update<TEntity>(params TEntity[] entities)
            where TEntity : Entity
        {
            if (null == entities || entities.Length == 0)
                return 0;
            int count = 0;
            using (DbTrans trans = BeginTransaction())
            {
                count = Update<TEntity>(trans, entities);
                trans.Commit();
            }
            return count;
        }
        /// <summary>
        /// 更新  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Update<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return Update(entities.ToArray());
        }
        /// <summary>
        /// 更新  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Update<TEntity>(List<TEntity> entities)
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
        public int Update<TEntity>(TEntity entity, WhereOperation where)
            where TEntity : Entity
        {
            return !entity.IsModify()
                ? 0
                : ExecuteNonQuery(cmdCreator.CreateUpdateCommand(entity, @where));
        }

        /// <summary>
        /// Update<TEntity>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(entity, ExpressionToOperation<TEntity>.ToWhereOperation(entity.GetTableName(), lambdaWhere));
        }

        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(TEntity entity, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(entity, where.ToWhereClip());
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

            WhereOperation where = DataUtils.GetPrimaryKeyWhere(entity);

            Check.Require(!WhereOperation.IsNullOrEmpty(where), "entity must have the primarykey!");

            return Update<TEntity>(tran, entity, where);
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
            int count = 0;
            foreach (TEntity entity in entities)
            {
                if (!entity.IsModify())
                    continue;//2015-08-20 break修改为continue
                count += Update<TEntity>(tran, entity, DataUtils.GetPrimaryKeyWhere(entity));
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
            return Update(tran, entities.ToArray());

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
        public int Update<TEntity>(DbTransaction tran, TEntity entity, WhereOperation where)
            where TEntity : Entity
        {
            if (!entity.IsModify())
                return 0;
            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(entity, where), tran);
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(DbTransaction tran, TEntity entity, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, entity, ExpressionToOperation<TEntity>.ToWhereOperation(entity.GetTableName(), lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(DbTransaction tran, TEntity entity, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, entity, where.ToWhereClip());
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
        public int Update<TEntity>(string tableName, Field field, object value, WhereOperation where)
            where TEntity : Entity
        {
            if (Field.IsNullOrEmpty(field))
                return 0;

            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(tableName, new Field[] { field }, new object[] { value }, where));
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
            return Update<TEntity>(tableName, field, value, ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere));
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
        public int Update<TEntity>(DbTransaction tran, string tableName, Field field, object value, WhereOperation where)
            where TEntity : Entity
        {
            if (Field.IsNullOrEmpty(field))
                return 0;

            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(tableName, new Field[] { field }, new object[] { value }, where), tran);
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
            return Update<TEntity>(tran, tableName, field, value, ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere));
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
        /// 更新多个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(string tableName, Dictionary<Field, object> fieldValue, WhereOperation where)
              where TEntity : Entity
        {
            if (null == fieldValue || fieldValue.Count == 0)
                return 0;
            Field[] fields = new Field[fieldValue.Count];
            object[] values = new object[fieldValue.Count];
            int i = 0;
            foreach (KeyValuePair<Field, object> kv in fieldValue)
            {
                fields[i] = kv.Key;
                values[i] = kv.Value;

                i++;
            }
            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(tableName, fields, values, where));
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(string tableName, Dictionary<Field, object> fieldValue, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(tableName, fieldValue, ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(string tableName, Dictionary<Field, object> fieldValue, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(tableName, fieldValue, where.ToWhereClip());
        }
        /// <summary>
        /// 更新多个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, string tableName, Dictionary<Field, object> fieldValue, WhereOperation where)
              where TEntity : Entity
        {
            if (null == fieldValue || fieldValue.Count == 0)
                return 0;

            Field[] fields = new Field[fieldValue.Count];
            object[] values = new object[fieldValue.Count];

            int i = 0;

            foreach (KeyValuePair<Field, object> kv in fieldValue)
            {
                fields[i] = kv.Key;
                values[i] = kv.Value;

                i++;
            }

            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(tableName, fields, values, where), tran);
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(DbTransaction tran, string tableName, Dictionary<Field, object> fieldValue, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, tableName, fieldValue, ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(DbTransaction tran, string tableName, Dictionary<Field, object> fieldValue, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, tableName, fieldValue, where.ToWhereClip());
        }

        public int Update<TEntity>(string tableName, Field[] fields, object[] values, WhereOperation where)
            where TEntity : Entity
        {

            if (null == fields || fields.Length == 0)
                return 0;
            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(tableName, fields, values, where));
        }


        public int Update<TEntity>(string tableName, Field[] fields, object[] values, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(tableName, fields, values, ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere));
        }


        public int Update<TEntity>(string tableName, Field[] fields, object[] values, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(tableName, fields, values, where.ToWhereClip());
        }
        public int Update<TEntity>(DbTransaction tran, string tableName, Field[] fields, object[] values, WhereOperation where)
            where TEntity : Entity
        {
            if (null == fields || fields.Length == 0)
                return 0;

            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(tableName, fields, values, where), tran);
        }


        public int Update<TEntity>(DbTransaction tran, string tableName, Field[] fields, object[] values, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, tableName, fields, values, ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere));
        }


        public int Update<TEntity>(DbTransaction tran, string tableName, Field[] fields, object[] values, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, tableName, fields, values, where.ToWhereClip());
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
            var tableName = entity.GetTableName();

            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", tableName, ") is readonly!"));

            WhereOperation where = DataUtils.GetPrimaryKeyWhere(entity);

            Check.Require(!WhereOperation.IsNullOrEmpty(where), "entity must have the primarykey!");

            return Delete<TEntity>(tableName, where);
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

            return Delete<TEntity>(tran, entity.GetTableName(), DataUtils.GetPrimaryKeyWhere(entity));
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entities"></param>
        public int Delete<TEntity>(DbTransaction tran, params TEntity[] entities)
            where TEntity : Entity
        {
            var eCount = entities.Length;
            switch (eCount)
            {
                case 0:
                    return 0;
                case 1:
                    return Delete(tran, entities.First());
                default:
                    //TODO 修改成In条件，性能更高。 
                    var listKey = new List<object>();
                    var where = new Where();
                    var f = entities.First().GetPrimaryKeyFields().First();
                    foreach (var entity in entities)
                    {
                        listKey.Add(DataUtils.GetPropertyValue(entity, f.Name));
                    }
                    where.And(f.In(listKey));
                    return Delete<TEntity>(tran, where);
            }
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
        /// <param name="entities"></param>
        public int Delete<TEntity>(DbTransaction tran, IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return Delete(tran, entities.ToArray());
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
        ///  删除
        /// </summary>
        public int Delete<TEntity>(string tableName, params string[] pkValues)
            where TEntity : Entity
        {
            return DeleteByPrimaryKey<TEntity>(tableName, pkValues.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        public int Delete<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            var arr = entities as TEntity[] ?? entities.ToArray();
            var eCount = arr.Length;
            switch (eCount)
            {
                case 0:
                    return 0;
                case 1:
                    return Delete(arr.First());
                default:
                    //TODO 修改成In条件，性能更高。 
                    var listKey = new List<object>();
                    var where = new Where();
                    var f = arr.First().GetPrimaryKeyFields().First();
                    foreach (var entity in arr)
                    {
                        listKey.Add(DataUtils.GetPropertyValue(entity, f.Name));
                    }
                    where.And(f.In(listKey));
                    return Delete<TEntity>(f.tableName, where);
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


            return ExecuteNonQuery(cmdCreator.CreateDeleteCommand(tableName, EntityCache.GetUserName<TEntity>(), DataUtils.GetPrimaryKeyWhere<TEntity>(pkValues)));
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


            return ExecuteNonQuery(cmdCreator.CreateDeleteCommand(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), DataUtils.GetPrimaryKeyWhere<TEntity>(pkValues)), tran);
        }





        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(DbTransaction tran, string tableName, WhereOperation where)
            where TEntity : Entity
        {
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = EntityCache.GetTableName<TEntity>();
            }

            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", tableName, ") is readonly!"));


            return ExecuteNonQuery(cmdCreator.CreateDeleteCommand(tableName, EntityCache.GetUserName<TEntity>(), where), tran);
        }
        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(DbTransaction tran, Where where)
            where TEntity : Entity
        {
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));
            return ExecuteNonQuery(cmdCreator.CreateDeleteCommand(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where.ToWhereClip()), tran);
        }
        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(string tableName, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Delete<TEntity>(tableName, ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere));
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
            return Delete<TEntity>(tran, tableName, ExpressionToOperation<TEntity>.ToWhereOperation(tableName, lambdaWhere));
        }
        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(string tableName, WhereOperation where)
            where TEntity : Entity
        {
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));

            return ExecuteNonQuery(cmdCreator.CreateDeleteCommand(tableName ?? EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where));
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
        public int Insert<TEntity>(params TEntity[] entities)
            where TEntity : Entity
        {
            if (null == entities || entities.Length == 0)
                return 0;
            int count;
            using (var trans = this.BeginTransaction())
            {
                count = Insert(trans, entities);
                trans.Commit();
            }
            return count;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Insert<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return Insert(entities.ToArray());
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Insert<TEntity>(List<TEntity> entities)
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
            return insertExecute<TEntity>(cmdCreator.CreateInsertCommand<TEntity>(entity));
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
            return insertExecute<TEntity>(cmdCreator.CreateInsertCommand<TEntity>(entity), tran);
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
            int count = 0;
            foreach (TEntity entity in entities)
            {
                var tcount = insertExecute<TEntity>(cmdCreator.CreateInsertCommand(entity), tran);
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
            return insertExecute<TEntity>(cmdCreator.CreateInsertCommand<TEntity>(tableName, fields, values));
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
            return insertExecute<TEntity>(cmdCreator.CreateInsertCommand<TEntity>(tableName, fields, values), tran);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private int insertExecute<TEntity>(DbCommand cmd)
             where TEntity : Entity
        {
            int returnValue = 0;

            if (null == cmd)
                return returnValue;
            returnValue = insertExecute<TEntity>(cmd, null);
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private int insertExecute<TEntity>(DbCommand cmd, DbTransaction tran)
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
                if (Db.DbProvider is MsAccessProvider)
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
                else if (Db.DbProvider is OracleProvider)
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
        #endregion

        #region Save操作
        /// <summary>
        /// 将实体批量提交至数据库，内置事务。每个实体需要手动标记EntityState状态。
        /// </summary>
        public int Save<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            int count = 0;
            using (DbTrans trans = this.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    var es = entity.GetEntityState();
                    switch (es)
                    {
                        case EntityState.Added:
                            count += Insert<TEntity>(trans, entity);
                            break;
                        case EntityState.Deleted:
                            count += Delete<TEntity>(trans, entity);
                            break;
                        case EntityState.Modified:
                            count += Update<TEntity>(trans, entity);
                            break;
                    }
                }
                trans.Commit();
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
                    count = Insert<TEntity>(entity);
                    break;
                case EntityState.Deleted:
                    count = Delete<TEntity>(entity);
                    break;
                case EntityState.Modified:
                    count = Update<TEntity>(entity);
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
                        count += Insert<TEntity>(tran, entity);
                        break;
                    case EntityState.Deleted:
                        count += Delete<TEntity>(tran, entity);
                        break;
                    case EntityState.Modified:
                        count += Update<TEntity>(tran, entity);
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
                    count = Insert<TEntity>(tran, entity);
                    break;
                case EntityState.Deleted:
                    count = Delete<TEntity>(tran, entity);
                    break;
                case EntityState.Modified:
                    count = Update<TEntity>(tran, entity);
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
            if (null == cmd)
                return 0;

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
            if (null == cmd)
                return 0;
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
            if (null == cmd)
                return null;

            return _db.ExecuteScalar(cmd, tran);
        }

        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public object ExecuteScalar(DbCommand cmd)
        {
            if (null == cmd)
                return null;

            return _db.ExecuteScalar(cmd);
        }

        /// <summary>
        /// 执行ExecuteReader
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(DbCommand cmd)
        {
            if (null == cmd)
                return null;
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
            if (null == cmd)
                return null;
            return _db.ExecuteReader(cmd, tran);
        }

        /// <summary>
        /// 执行ExecuteDataSet
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbCommand cmd)
        {
            if (null == cmd)
                return null;
            return _db.ExecuteDataSet(cmd);
        }

        /// <summary>
        /// 执行ExecuteDataSet
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbCommand cmd, DbTransaction tran)
        {
            if (null == cmd)
                return null;
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
            return ExecuteReader(sql, dbParameters).ToList<T>();
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql)
        {
            return new SqlSection(this, sql);
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

        #region 数据导入导出

        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="withHeader"></param>
        /// <returns></returns>
        public static DataTable ReadFromCSV(string filePath)
        {
            DataTable dt = new DataTable();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, new UTF8Encoding(false)))
                {
                    //记录每次读取的一行记录
                    string strLine = "";

                    //记录每行记录中的各字段内容
                    string[] aryLine = null;

                    string[] tableHead = null;

                    //标示列数
                    int columnCount = 0;

                    //标示是否是读取的第一行
                    bool isFirst = true;

                    //逐行读取CSV中的数据
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        if (isFirst == true)
                        {
                            tableHead = strLine.Split(',');

                            isFirst = false;

                            columnCount = tableHead.Length;

                            //创建列
                            for (int i = 0; i < columnCount; i++)
                            {
                                DataColumn dc = new DataColumn(tableHead[i]);
                                dt.Columns.Add(dc);
                            }
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(strLine))
                            {
                                aryLine = strLine.Split(',');

                                DataRow dr = dt.NewRow();

                                for (int j = 0; j < columnCount; j++)
                                {
                                    dr[j] = aryLine[j];
                                }

                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    if (aryLine != null && aryLine.Length > 0)
                    {
                        dt.DefaultView.Sort = tableHead[0] + " " + "asc";
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 写入CSV文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filePath"></param>
        /// <param name="withHeader"></param>
        public static void WriteToCSV(DataTable table, string filePath, bool withHeader = true)
        {
            FileInfo fi = new FileInfo(filePath);
            string path = fi.DirectoryName;
            string name = fi.Name;

            StringBuilder sb = new StringBuilder();

            DataColumn colum;

            if (withHeader)
            {
                foreach (DataColumn column in table.Columns)
                {
                    if (column == table.Columns[0])
                        sb.Append(column.ColumnName);
                    else
                        sb.Append("," + column.ColumnName);
                }
                sb.AppendLine();
            }

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }

            var csvStr = sb.ToString();

            using (FileStream fs = new FileStream(path + "\\" + name, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, new UTF8Encoding(false)))
                {
                    sw.Write(csvStr);
                }
            }
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public int Import<TEntity>(string filePath) where TEntity : Entity
        {
            var dataTable = ReadFromCSV(filePath);

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

            WriteToCSV(dataTable, filePath, withHeader);
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
