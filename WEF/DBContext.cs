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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WEF.Cache;
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
    public sealed class DBContext
    {
        /// <summary>
        /// 
        /// </summary>
        private Database db;

        /// <summary>
        /// 
        /// </summary>
        private CommandCreator cmdCreator;


        #region Cache

        /// <summary>
        /// 开启缓存
        /// </summary>
        public void TurnOnCache()
        {
            if (null != db.DbProvider.CacheConfig)
            {
                db.DbProvider.CacheConfig.Enable = true;
            }
        }


        /// <summary>
        /// 关闭缓存
        /// </summary>
        public void TurnOffCache()
        {
            if (null != db.DbProvider.CacheConfig)
            {
                db.DbProvider.CacheConfig.Enable = false;
            }
        }

        #endregion

        #region batch

        /// <summary>
        /// 开始批处理，默认10条sql组合
        /// </summary>
        public DbBatch BeginBatchConnection()
        {
            return BeginBatchConnection(10);
        }

        /// <summary>
        /// 开始批处理
        /// </summary>
        /// <param name="batchSize">sql组合条数</param>
        public DbBatch BeginBatchConnection(int batchSize)
        {
            return new DbBatch(cmdCreator, new BatchCommander(db, batchSize));
        }

        /// <summary>
        /// 开始批处理
        /// </summary>
        /// <param name="batchSize">sql组合条数</param>
        /// <param name="tran">事务</param>
        public DbBatch BeginBatchConnection(int batchSize, DbTransaction tran)
        {
            return new DbBatch(cmdCreator, new BatchCommander(db, batchSize, tran));
        }

        /// <summary>
        /// 开始批处理
        /// </summary>
        /// <param name="batchSize">sql组合条数</param>
        /// <param name="il">事务</param>
        public DbBatch BeginBatchConnection(int batchSize, IsolationLevel il)
        {
            return new DbBatch(cmdCreator, new BatchCommander(db, batchSize, il));
        }


        #endregion

        #region Default


        /// <summary>
        /// Get the default gateway, which mapping to the default Database.
        /// </summary>
        public static DBContext Default = new DBContext(Database.Default);

        /// <summary>
        /// Sets the default DBContext.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="connStr">The conn STR.</param>
        public static void SetDefault(DatabaseType dt, string connStr)
        {
            DbProvider provider = CreateDbProvider(dt, connStr);

            Default = new DBContext(new Database(provider));
        }

        /// <summary>
        /// Creates the db provider.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="connStr">The conn STR.</param>
        /// <returns>The db provider.</returns>
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
                case DatabaseType.MySql:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(MySqlProvider).FullName, connStr, dt);
                    break;
                case DatabaseType.Sqlite3:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(SqliteProvider).FullName, connStr, dt);
                    break;
                case DatabaseType.MsAccess:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(MsAccessProvider).FullName, connStr, dt);
                    break;
            }
            if (provider != null)
            {
                provider.DatabaseType = dt;
            }
            return provider;
        }

        /// <summary>
        /// Sets the default DBContext.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="connStr">The conn STR.</param>
        public static void SetDefault(string assemblyName, string className, string connStr)
        {
            DbProvider provider = ProviderFactory.CreateDbProvider(assemblyName, className, connStr, null);
            if (provider == null)
            {
                throw new NotSupportedException(string.Format("Cannot construct DbProvider by specified parameters: {0}, {1}, {2}",
                    assemblyName, className, connStr));
            }

            Default = new DBContext(new Database(provider));
        }

        /// <summary>
        /// Sets the default DBContext.
        /// </summary>
        /// <param name="connStrName">Name of the conn STR.</param>
        public static void SetDefault(string connStrName)
        {
            DbProvider provider = ProviderFactory.CreateDbProvider(connStrName);
            provider.ConnectionStringsName = connStrName;
            if (provider == null)
            {
                throw new NotSupportedException(string.Format("Cannot construct DbProvider by specified ConnectionStringName: {0}", connStrName));
            }

            Default = new DBContext(new Database(provider));
        }
        #endregion

        #region 构造函数



        private void initDbSesion()
        {
            cmdCreator = new CommandCreator(db);

            if (db.DbProvider.CacheConfig == null)
            {
                object cacheConfig = System.Configuration.ConfigurationManager.GetSection("WEFCacheConfig");

                if (cacheConfig != null)
                {
                    db.DbProvider.CacheConfig = (CacheConfiguration)cacheConfig;

                    ConcurrentDictionary<string, CacheInfo> entitiesCache = new ConcurrentDictionary<string, CacheInfo>();

                    //获取缓存配制

                    foreach (string key in db.DbProvider.CacheConfig.Entities.AllKeys)
                    {
                        if (key.IndexOf('.') > 0)
                        {
                            string[] splittedKey = key.Split('.');

                            if (splittedKey[0].Trim() == db.DbProvider.ConnectionStringsName)
                            {
                                int expireSeconds = 0;
                                CacheInfo cacheInfo = new CacheInfo();

                                if (int.TryParse(db.DbProvider.CacheConfig.Entities[key].Value, out expireSeconds))

                                {
                                    cacheInfo.TimeOut = expireSeconds;
                                }
                                else
                                {
                                    string tempFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, db.DbProvider.CacheConfig.Entities[key].Value);

                                    if (File.Exists(tempFilePath))
                                    {
                                        cacheInfo.FilePath = tempFilePath;
                                    }
                                }

                                if (!cacheInfo.IsNullOrEmpty())
                                {
                                    string entityName = string.Concat(db.DbProvider.ConnectionStringsName, splittedKey[1].Trim());                                   

                                    entitiesCache[entityName] = cacheInfo;
                                }
                            }
                        }
                    }

                    db.DbProvider.EntitiesCache = entitiesCache;
                }

            }
        }


        /// <summary>
        /// 构造函数    使用默认  DBContext.Default
        /// </summary>
        public DBContext()
        {
            db = Database.Default;

            initDbSesion();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connStrName">config文件中connectionStrings节点的name</param>
        public DBContext(string connStrName)
        {
            this.db = new Database(ProviderFactory.CreateDbProvider(connStrName));
            this.db.DbProvider.ConnectionStringsName = connStrName;
            initDbSesion();
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db">已知的Database</param>
        public DBContext(Database db)
        {
            this.db = db;

            initDbSesion();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dt">数据库类别</param>
        /// <param name="connStr">连接字符串</param>
        public DBContext(DatabaseType dt, string connStr)
        {
            DbProvider provider = CreateDbProvider(dt, connStr);

            this.db = new Database(provider);

            initDbSesion();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="assemblyName">程序集</param>
        /// <param name="className">类名</param>
        /// <param name="connStr">连接字符串</param>
        public DBContext(string assemblyName, string className, string connStr)
        {
            DbProvider provider = ProviderFactory.CreateDbProvider(assemblyName, className, connStr, null);
            if (provider == null)
            {
                throw new NotSupportedException(string.Format("Cannot construct DbProvider by specified parameters: {0}, {1}, {2}",
                    assemblyName, className, connStr));
            }

            this.db = new Database(provider);

            cmdCreator = new CommandCreator(db);
        }

        #endregion

        #region 查询


        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public ISearch<TEntity> Search<TEntity>(string asName = "")
            where TEntity : Entity
        {
            return new Search<TEntity>(db, null, asName);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ISearch Search(string tableName)
        {
            return new Search(db, tableName);
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
            return new Search<TEntity>(db, null, entity.GetTableName());
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
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> lambdaWhere)
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
        /// <param name="field"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Count<TEntity>(Field field, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Search<TEntity>().Select(field.Count()).Where(ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere)).ToScalar<int>();
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
            db.OnLog += handler;
        }

        /// <summary>
        /// Unregisters the SQL logger.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void UnregisterSqlLogger(LogHandler handler)
        {
            db.OnLog -= handler;
        }

        /// <summary>
        /// Gets the db.
        /// </summary>
        /// <value>The db.</value>
        public Database Db
        {
            get
            {
                return this.db;
            }
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns>The begined transaction.</returns>
        public DbTrans BeginTransaction()
        {
            return new DbTrans(db.BeginTransaction(), this);
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="il">The il.</param>
        /// <returns>The begined transaction.</returns>
        public DbTrans BeginTransaction(System.Data.IsolationLevel il)
        {
            return new DbTrans(db.BeginTransaction(il), this);
        }

        /// <summary>
        /// Closes the transaction.
        /// </summary>
        /// <param name="tran">The tran.</param>
        public void CloseTransaction(DbTransaction tran)
        {
            db.CloseConnection(tran);
        }

        /// <summary>
        /// Builds the name of the db param.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The name of the db param</returns>
        public string BuildDbParamName(string name)
        {
            Check.Require(name, "name", Check.NotNullOrEmpty);

            return db.DbProvider.BuildParameterName(name);
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

            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(entity.GetFields(), entity.GetValues(), where));
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

            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(entity.GetFields(), entity.GetValues(), where), tran);
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
        /// 
        /// </summary>
        public int Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(entity, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
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
            return Update<TEntity>(tran, entity, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
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
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(Field field, object value, WhereOperation where)
            where TEntity : Entity
        {
            if (Field.IsNullOrEmpty(field))
                return 0;

            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(new Field[] { field }, new object[] { value }, where));
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(Field field, object value, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(field, value, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(Field field, object value, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(field, value, where.ToWhereClip());
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, Field field, object value, WhereOperation where)
            where TEntity : Entity
        {
            if (Field.IsNullOrEmpty(field))
                return 0;

            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(new Field[] { field }, new object[] { value }, where), tran);
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(DbTransaction tran, Field field, object value, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, field, value, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(DbTransaction tran, Field field, object value, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, field, value, where.ToWhereClip());
        }

        /// <summary>
        /// 更新多个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fieldValue"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(Dictionary<Field, object> fieldValue, WhereOperation where)
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
            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(fields, values, where));
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(Dictionary<Field, object> fieldValue, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(fieldValue, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(Dictionary<Field, object> fieldValue, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(fieldValue, where.ToWhereClip());
        }
        /// <summary>
        /// 更新多个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fieldValue"></param>
        /// <param name="where"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, Dictionary<Field, object> fieldValue, WhereOperation where)
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

            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(fields, values, where), tran);
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(DbTransaction tran, Dictionary<Field, object> fieldValue, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, fieldValue, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(DbTransaction tran, Dictionary<Field, object> fieldValue, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, fieldValue, where.ToWhereClip());
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(Field[] fields, object[] values, WhereOperation where)
            where TEntity : Entity
        {

            if (null == fields || fields.Length == 0)
                return 0;
            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(fields, values, where));
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(Field[] fields, object[] values, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(fields, values, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(Field[] fields, object[] values, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(fields, values, where.ToWhereClip());
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Update<TEntity>(DbTransaction tran, Field[] fields, object[] values, WhereOperation where)
            where TEntity : Entity
        {
            if (null == fields || fields.Length == 0)
                return 0;

            return ExecuteNonQuery(cmdCreator.CreateUpdateCommand<TEntity>(fields, values, where), tran);
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(DbTransaction tran, Field[] fields, object[] values, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, fields, values, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 
        /// </summary>
        public int Update<TEntity>(DbTransaction tran, Field[] fields, object[] values, Where where)
            where TEntity : Entity
        {
            return Update<TEntity>(tran, fields, values, where.ToWhereClip());
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
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));

            WhereOperation where = DataUtils.GetPrimaryKeyWhere(entity);

            Check.Require(!WhereOperation.IsNullOrEmpty(where), "entity must have the primarykey!");

            return Delete<TEntity>(where);
        }

        ///// <summary>
        /////  删除
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <param name="entity"></param>
        ///// <param name="where"></param>
        ///// <returns></returns>
        //[Obsolete("请使用Delete<TEntity>(WhereClip where)方法!")]
        //public int Delete<TEntity>(TEntity entity, WhereClip where)
        //    where TEntity : Entity
        //{
        //    Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));

        //    return ExecuteNonQuery(createDeleteCommand(entity.GetTableName(), where));
        //}


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
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));

            return Delete<TEntity>(tran, DataUtils.GetPrimaryKeyWhere(entity));
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


        ///// <summary>
        /////  删除
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <param name="where"></param>
        ///// <param name="tran"></param>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //[Obsolete("请使用Delete<TEntity>(WhereClip where, DbTransaction tran)方法!")]
        //public int Delete<TEntity>(TEntity entity, WhereClip where, DbTransaction tran)
        //    where TEntity : Entity
        //{
        //    Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));

        //    return ExecuteNonQuery(createDeleteCommand(entity.GetTableName(), where), tran);
        //}

        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(params int[] pkValues)
            where TEntity : Entity
        {
            return DeleteByPrimaryKey<TEntity>(pkValues.ToArray());
        }
        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(params Guid[] pkValues)
            where TEntity : Entity
        {
            return DeleteByPrimaryKey<TEntity>(pkValues.ToArray());
        }
        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(params long[] pkValues)
            where TEntity : Entity
        {
            return DeleteByPrimaryKey<TEntity>(pkValues.ToArray());
        }
        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(params string[] pkValues)
            where TEntity : Entity
        {
            return DeleteByPrimaryKey<TEntity>(pkValues.ToArray());
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
                    return Delete<TEntity>(where);
            }
            //var count = 0;
            //using (DbTrans trans = BeginTransaction())
            //{
            //    foreach (var entity in entities)
            //    {
            //        count += Delete<TEntity>(entity, trans);
            //    }
            //    trans.Commit();
            //}
            //return count;
        }
        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        //public int Delete<TEntity>(params string[] pkValues)
        //    where TEntity : Entity
        //{
        //    return DeleteByPrimaryKey<TEntity>(pkValues);
        //}

        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        internal int DeleteByPrimaryKey<TEntity>(Array pkValues)//params object[] pkValues 2015-08-20
            where TEntity : Entity
        {
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));


            return ExecuteNonQuery(cmdCreator.CreateDeleteCommand(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), DataUtils.GetPrimaryKeyWhere<TEntity>(pkValues)));
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
        public int Delete<TEntity>(DbTransaction tran, WhereOperation where)
            where TEntity : Entity
        {
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));

            return ExecuteNonQuery(cmdCreator.CreateDeleteCommand(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where), tran);
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
        public int Delete<TEntity>(Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Delete<TEntity>(ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(DbTransaction tran, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return Delete<TEntity>(tran, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        ///  删除
        /// </summary>
        public int Delete<TEntity>(WhereOperation where)
            where TEntity : Entity
        {
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));

            return ExecuteNonQuery(cmdCreator.CreateDeleteCommand(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where));
        }
        /// <summary>
        /// 删除
        /// </summary>
        public int Delete<TEntity>(Where where)
            where TEntity : Entity
        {
            return Delete<TEntity>(where.ToWhereClip());
        }
        /// <summary>
        /// 删除整表数据
        /// </summary>
        public int DeleteAll<TEntity>()
            where TEntity : Entity
        {
            return Delete<TEntity>(d => true);
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <param name="where"></param>
        ///// <returns></returns>
        //private DbCommand createDeleteCommand(string tableName, WhereClip where)
        //{
        //    if (WhereClip.IsNullOrEmpty(where))
        //        where = WhereClip.All;

        //    StringBuilder sql = new StringBuilder();
        //    sql.Append("DELETE FROM ");
        //    sql.Append(db.DbProvider.LeftToken.ToString());
        //    sql.Append(tableName);
        //    sql.Append(db.DbProvider.RightToken.ToString());
        //    sql.Append(where.WhereString);
        //    DbCommand cmd = db.GetSqlStringCommand(sql.ToString());
        //    db.AddCommandParameter(cmd, where.Parameters.ToArray());

        //    return cmd;
        //}

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
                //2015-08-20 修改为tcount判断
                var tcount = insertExecute<TEntity>(cmdCreator.CreateInsertCommand<TEntity>(entity), tran);
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
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public int Insert<TEntity>(Field[] fields, object[] values)
            where TEntity : Entity
        {
            return insertExecute<TEntity>(cmdCreator.CreateInsertCommand<TEntity>(fields, values));
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Insert<TEntity>(DbTransaction tran, Field[] fields, object[] values)
            where TEntity : Entity
        {
            return insertExecute<TEntity>(cmdCreator.CreateInsertCommand<TEntity>(fields, values), tran);
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

            //using (DbTrans dbTrans = BeginTransaction())
            //{
            //    returnValue = insertExecute<TEntity>(cmd, dbTrans);
            //    dbTrans.Commit();
            //}
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
                                db.GetSqlStringCommand(string.Format("select max({0}) from {1}", identity.FieldName,
                                    identity.TableName))); //Max<TEntity, int>(identity, WhereClip.All) + 1;
                    }
                    else
                    {
                        ExecuteNonQuery(cmd, tran);
                        scalarValue = ExecuteScalar(db.GetSqlStringCommand(string.Format("select max({0}) from {1}", identity.FieldName, identity.TableName)), tran); //Max<TEntity, int>(identity, WhereClip.All) + 1;
                    }

                }
                else if (Db.DbProvider is OracleProvider)
                {
                    if (tran == null)
                    {
                        ExecuteNonQuery(cmd);
                        scalarValue =
                            ExecuteScalar(
                                db.GetSqlStringCommand(string.Format(db.DbProvider.RowAutoID,
                                    EntityCache.GetSequence<TEntity>())));
                    }
                    else
                    {
                        ExecuteNonQuery(cmd, tran);
                        scalarValue = ExecuteScalar(db.GetSqlStringCommand(string.Format(db.DbProvider.RowAutoID, EntityCache.GetSequence<TEntity>())), tran);
                    }
                }
                else
                {
                    if (Db.DbProvider.SupportBatch)
                    {
                        if (tran == null)
                        {
                            cmd.CommandText = string.Concat(cmd.CommandText, ";", db.DbProvider.RowAutoID);
                            scalarValue = ExecuteScalar(cmd);
                        }
                        else
                        {
                            cmd.CommandText = string.Concat(cmd.CommandText, ";", db.DbProvider.RowAutoID);
                            scalarValue = ExecuteScalar(cmd, tran);
                        }
                    }
                    else
                    {
                        if (tran == null)
                        {
                            ExecuteNonQuery(cmd);
                            scalarValue = ExecuteScalar(db.GetSqlStringCommand(Db.DbProvider.RowAutoID));
                        }
                        else
                        {
                            ExecuteNonQuery(cmd, tran);
                            scalarValue = ExecuteScalar(db.GetSqlStringCommand(Db.DbProvider.RowAutoID), tran);
                        }
                    }
                }

                if (null == scalarValue || Convert.IsDBNull(scalarValue))
                    return 0;
                return Convert.ToInt32(scalarValue);
            }
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

            return db.ExecuteNonQuery(cmd);
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
            return db.ExecuteNonQuery(cmd, tran);
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

            return db.ExecuteScalar(cmd, tran);
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

            return db.ExecuteScalar(cmd);
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
            return db.ExecuteReader(cmd);
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
            return db.ExecuteReader(cmd, tran);
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
            return db.ExecuteDataSet(cmd);
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
            return db.ExecuteDataSet(cmd, tran);
        }

        #endregion

        #region 属性


        /// <summary>
        /// 左边  
        /// <example>例如:sqlserver   的    [</example>
        /// </summary>
        public string LeftToken
        {
            get
            {
                return db.DbProvider.LeftToken.ToString();
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
                return db.DbProvider.RightToken.ToString();
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
                return db.DbProvider.ParamPrefix.ToString();
            }
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
        /// sql查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql)
        {
            return new SqlSection(this, sql);
        }

        /// <summary>
        /// 执行ExecuteDataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql)
        {
            var ds = db.ExecuteDataSet(sql);
            if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// 执行ExecuteDataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql)
        {
            return db.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 执行ExecuteNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            return db.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            return db.ExecuteScalar(sql);
        }

        /// <summary>
        /// 执行ExecuteReader
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql)
        {
            return db.ExecuteReader(sql);
        }

        #endregion

        #region mvc 

        public DataTable GetMap(string tableName)
        {
            return db.GetMap(tableName);
        }

        /// <summary>
        /// 获取表中字段信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<ColumnInfo> GetColumnInfos(string tableName)
        {
            var clts = new List<ColumnInfo>();

            var schemaTable = GetMap(tableName);

            if (schemaTable != null && schemaTable.Rows.Count > 0)
            {
                foreach (DataRow dr in schemaTable.Rows)
                {
                    ColumnInfo info = new ColumnInfo();
                    info.Name = dr["ColumnName"].ToString().Trim();
                    info.Ordinal = Convert.ToInt32(dr["ColumnOrdinal"].ToString());
                    info.AllowDBNull = (bool)dr["AllowDBNull"];
                    info.MaxLength = Convert.ToInt32(dr["ColumnSize"].ToString());
                    info.DataTypeName = dr["DataTypeName"].ToString().Trim();
                    info.AutoIncrement = (bool)dr["IsAutoIncrement"];
                    info.IsPrimaryKey = (bool)dr["IsKey"];
                    info.Unique = (bool)dr["IsUnique"];
                    info.IsReadOnly = (bool)dr["IsReadOnly"];
                    clts.Add(info);
                }
            }
            return clts;
        }

        /// <summary>
        /// 获取表中字段信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public ColumnInfo GetColumnInfo(string tableName, string columnName)
        {
            return GetColumnInfos(tableName).Where(b => b.Name == columnName).First();
        }


        /// <summary>
        /// 获取表中字段类型及长度
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public DataTable GetTypeAndLength(string tableName, string columnName)
        {
            DataTable dt = new DataTable();
            if (Db.DbProvider is SqlServer9Provider || Db.DbProvider is SqlServerProvider)
            {
                string sql = "";
                if (string.IsNullOrEmpty(tableName))
                {
                    throw new Exception("表名不能为空");
                }
                sql = "SELECT  COLUMN_NAME ,DATA_TYPE ,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='" + DataUtils.ReplaceSqlKey(tableName, 50) + "' ";
                if (!string.IsNullOrEmpty(columnName))
                {
                    sql += " AND COLUMN_NAME='" + DataUtils.ReplaceSqlKey(columnName, 50) + "'";
                }
                dt = ExecuteDataTable(sql);
            }
            else
            {
                var sci = GetColumnInfo(tableName, columnName);
                DataColumn column1 = new DataColumn("COLUMN_NAME", typeof(string));
                DataColumn column2 = new DataColumn("DATA_TYPE", typeof(string));
                DataColumn column3 = new DataColumn("CHARACTER_MAXIMUM_LENGTH", typeof(int));
                dt.Columns.Add(column1);
                dt.Columns.Add(column2);
                dt.Columns.Add(column3);
                dt.AcceptChanges();
                var row = dt.NewRow();
                row["COLUMN_NAME"] = sci.Name;
                row["DATA_TYPE"] = sci.DataTypeName;
                row["CHARACTER_MAXIMUM_LENGTH"] = sci.MaxLength;
                dt.Rows.Add(row);
                dt.AcceptChanges();
            }
            return dt;
        }


        #region MVC前端传值更新模型
        /// <summary>
        /// 列举操作方法
        /// </summary>
        public enum EnumOperation
        {
            Insert,
            Update
        }
        /// <summary>
        /// MVC前端传值更新模型
        /// </summary>
        /// <param name="id">更新时的主键值</param>
        /// <param name="tableName">表名</param>
        /// <param name="collection">form表单</param>
        /// <param name="operation">更新类型</param>
        /// <returns>更新的记录数</returns>
        public int UpdateModel(int? id, string tableName, NameValueCollection collection, EnumOperation operation)
        {
            int error = 0;
            DataTable Column = db.GetMap(tableName.ToString());
            string sqlStr = operation + " [" + tableName + "] ";
            if (operation == EnumOperation.Insert)
            {
                string conStrColumn = "(";
                string conStrValue = " values (";
                foreach (DataColumn item in Column.Columns)
                {
                    string columnName = item.ColumnName;
                    if (collection[columnName] != null && columnName != "ID")
                    {
                        string itemValue = collection[columnName].Replace("'", "").Trim();
                        conStrColumn = conStrColumn + "[" + columnName + "] ,";
                        if (item.DataType == typeof(String) || item.DataType == typeof(DateTime) || item.DataType == typeof(TimeSpan))
                        {
                            if (string.IsNullOrEmpty(itemValue))
                            {
                                conStrValue += " null" + ",";
                            }
                            else
                            {
                                conStrValue += " '" + itemValue + "'" + " ,";
                            }
                        }
                        else if (item.DataType == typeof(Boolean))
                        {
                            if (string.IsNullOrEmpty(itemValue))
                            {
                                conStrValue += "null,";
                            }
                            else
                            {
                                if (itemValue == "True")
                                {
                                    conStrValue += " 1 ,";
                                }
                                else
                                {
                                    conStrValue += " 0 ,";
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(itemValue))
                            {
                                conStrValue += "null,";
                            }
                            else
                            {
                                conStrValue += " " + itemValue + " ,";
                            }
                        }
                    }
                }
                conStrColumn = conStrColumn.Substring(0, conStrColumn.Length - 1);
                conStrValue = conStrValue.Substring(0, conStrValue.Length - 1);
                conStrColumn += ")";
                conStrValue += ")";
                sqlStr = sqlStr + conStrColumn + conStrValue;
            }
            else if (operation == EnumOperation.Update)
            {
                if (id != null)
                {
                    string conStrColumn = "";
                    string conStrValue = "";
                    string ConStr = " set ";
                    foreach (DataColumn item in Column.Columns)
                    {
                        string columnName = item.ColumnName;
                        if (collection[columnName] != null && columnName != "ID")
                        {
                            string itemValue = collection[columnName].Replace("'", "").Trim();
                            if (string.IsNullOrEmpty(itemValue)) itemValue = "null";
                            conStrColumn = " [" + columnName + "]";
                            if (item.DataType == typeof(String) || item.DataType == typeof(DateTime) || item.DataType == typeof(TimeSpan))
                            {
                                if (string.IsNullOrEmpty(itemValue))
                                {
                                    conStrValue = " null ";
                                }
                                else
                                {
                                    conStrValue = " '" + itemValue + "' ";
                                }
                            }
                            else if (item.DataType == typeof(Boolean))
                            {
                                if (string.IsNullOrEmpty(itemValue))
                                {
                                    conStrValue = " null ";
                                }
                                else
                                {
                                    if (itemValue == "True")
                                    {
                                        conStrValue = " 1 ";
                                    }
                                    else
                                    {
                                        conStrValue = " 0 ";
                                    }
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(itemValue))
                                {
                                    conStrValue = " null ";
                                }
                                else
                                {
                                    conStrValue = " " + itemValue + " ";
                                }
                            }
                            ConStr += conStrColumn + "=" + conStrValue + " ,";
                        }
                    }
                    ConStr = ConStr.Substring(0, ConStr.Length - 1);
                    string PKColumnName = "ID";
                    sqlStr += ConStr + " where [" + PKColumnName + "]=" + id;
                }
                else
                {
                    error = -1;
                }
            }
            if (error == -1)
                return error;
            else
                return ExecuteNonQuery(sqlStr);
        }
        #endregion
        #endregion

        #region 数据导入导出
        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable ReadFromCSV(string filePath)
        {
            Encoding encoding = Encoding.GetEncoding("utf-8");

            DataTable dt = new DataTable();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, encoding))
                {
                    //记录每次读取的一行记录
                    string strLine = "";

                    //记录每行记录中的各字段内容
                    string[] aryLine = null;

                    string[] tableHead = null;

                    //标示列数
                    int columnCount = 0;

                    //标示是否是读取的第一行
                    bool IsFirst = true;

                    //逐行读取CSV中的数据
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        if (IsFirst == true)
                        {
                            tableHead = strLine.Split(',');

                            IsFirst = false;

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
        /// <param name="titlee"></param>
        public static void WriteToCSV(DataTable table, string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            string path = fi.DirectoryName;
            string name = fi.Name;
            //\/:*?"<>|
            name = name.Replace(@"\", "＼");
            name = name.Replace(@"/", "／");
            name = name.Replace(@":", "：");
            name = name.Replace(@"*", "＊");
            name = name.Replace(@"?", "？");
            name = name.Replace(@"<", "＜");
            name = name.Replace(@">", "＞");
            name = name.Replace(@"|", "｜");
            string title = "";

            using (FileStream fs = new FileStream(path + "\\" + name, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.Default))
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        title += table.Columns[i].ColumnName + ",";
                    }
                    title = title.Substring(0, title.Length - 1) + "\n";
                    sw.Write(title);
                    foreach (DataRow row in table.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted) continue;
                        string line = "";
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            line += row[i].ToString().Replace(",", "") + ",";
                        }
                        line = line.Substring(0, line.Length - 1) + "\n";
                        sw.Write(line);
                    }
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
        public void Export<TEntity>(string filePath) where TEntity : Entity
        {
            var list = Search<TEntity>().ToList();

            var dataTable = list.EntityArrayToDataTable();

            WriteToCSV(dataTable, filePath);
        }

        #endregion

        #region 监控数据变化



        #endregion
    }
}
