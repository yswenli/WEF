/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：9a4fe848-95cb-4ad2-ac1b-d757a6ea1cd0
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WEF.Standard.Mongo.Core
 * 类名称：MongoDBOperatorBase<T>
 * 文件名：MongoDBOperatorBase<T>
 * 创建年份：2015
 * 创建时间：2015-09-29 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using WEF.Standard.Mongo.Core;
using WEF.Standard.Mongo.Model;

namespace WEF.Standard.Mongo
{
    /// <summary>
    /// MongoDB实体操作基类
    /// </summary>
    /// <typeparam name="T">存储库中包含的类型.</typeparam>
    /// <typeparam name="ObjectId">用于实体ID的类型。</typeparam>
    public class BaseRepository<T, ObjectId> : IRepository<T, ObjectId>
        where T : IMongoEntity<ObjectId>
    {
        protected DBContext _dbContext;

        protected internal MongoCollection<T> collection;

        protected string _connStr = string.Empty;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return _connStr; }
        }

        /// <summary>
        /// MongoDB实体操作类
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="collectionName"></param>
        public BaseRepository(string connectionString, string collectionName)
        {
            _connStr = connectionString;
            _dbContext = new DBContext(_connStr);
            collection = _dbContext.GetCollection<T, ObjectId>(collectionName);
        }

        /// <summary>
        /// MongoDB实体操作类
        /// </summary>
        public BaseRepository()
            : this(DBContextExtention.GetDefaultConnectionString())
        {
        }

        /// <summary>
        /// MongoDB实体操作类,
        /// 若设置过MongoServerAddress 、MongReplicaSetName则已Default优先
        /// 否则默认为最后一个ConnectionString设置
        /// </summary>
        /// <param name="connectionString"></param>
        public BaseRepository(string connectionString) : this(connectionString, "")
        {
        }

       
        public MongoCollection<T> Collection
        {
            get
            {
                return collection;
            }
        }


        public string CollectionName
        {
            get
            {
                return collection.Name;
            }
        }



        public string ServerInfo
        {
            get
            {
                var cs = collection;

                if (cs != null)
                {
                    var sb = new StringBuilder();

                    var instances = cs.Database.Server.Instances;

                    foreach (var instance in instances)
                    {
                        sb.AppendLine($"{instance.Settings.Server.Host}:{instance.Settings.Server.Port}");
                    }

                    return sb.ToString();
                }
                return null;
            }
        }

        public string DataBaseName
        {
            get
            {
                var cs = collection;

                if (cs != null)
                {
                    return cs.Database.Name;
                }
                return null;
            }
        }

        public IEnumerable<string> DataBaseNames
        {
            get
            {
                if (!string.IsNullOrEmpty(_connStr))
                    return new MongoClient(new MongoUrl(_connStr)).GetServer().GetDatabaseNames();

                return null;
            }
        }


        public IEnumerable<string> CollectionNames
        {
            get
            {
                if (!string.IsNullOrEmpty(_connStr))
                    return new MongoClient(new MongoUrl(_connStr)).GetServer().GetDatabase(DataBaseName).GetCollectionNames();

                return null;
            }
        }



        public virtual T GetById(ObjectId id)
        {
            if (typeof(T).IsSubclassOf(typeof(MongoEntity)))
            {
                return GetById(new MongoDB.Bson.ObjectId(id.ToString()));
            }
            return collection.FindOneByIdAs<T>(BsonValue.Create(id));
        }

        public virtual T GetById(MongoDB.Bson.ObjectId id)
        {
            return collection.FindOneByIdAs<T>(id);
        }

        public virtual T Add(T entity)
        {
            var result = collection.Insert<T>(entity);

            if (result != null && result?.Response?.AsBsonValue?.AsBsonDocument?.Elements?.FirstOrDefault().Value?.ToBoolean() == true)
            {
                return entity;
            }
            return default;
        }


        public virtual void Add(IEnumerable<T> entities)
        {
            collection.InsertBatch<T>(entities);
        }

        public virtual T Update(T entity)
        {
            var result = collection.Save<T>(entity);

            if (result != null && result?.Response?.AsBsonValue?.AsBsonDocument?.Elements?.FirstOrDefault().Value?.ToBoolean() == true)
            {
                return entity;
            }
            return default;
        }


        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                collection.Save<T>(entity);
            }
        }

        public virtual void Delete(ObjectId id)
        {
            if (typeof(T).IsSubclassOf(typeof(MongoEntity)))
            {
                collection.Remove(Query.EQ("_id", new MongoDB.Bson.ObjectId(id as string)));
            }
            else
            {
                collection.Remove(Query.EQ("_id", BsonValue.Create(id)));
            }
        }





        public virtual void Delete(T entity)
        {
            Delete(entity.Id);
        }



        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            foreach (T entity in collection.AsQueryable().Where(predicate))
            {
                Delete(entity.Id);
            }
        }

        public virtual void DeleteAll()
        {
            collection.RemoveAll();
        }

        public virtual long Count()
        {
            return collection.Count();
        }


        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return collection.AsQueryable().Any(predicate);
        }


        #region Test

        public virtual string Run(string json = "{\"dbStats\":1}")
        {
            return Run(BsonDocument.Parse(json));
        }


        public virtual string Run(BsonDocument bsons)
        {
            var command = new CommandDocument(bsons);

            var result = collection.Database.RunCommand(command);
            if (result != null && result.Ok)
            {
                return result.Response.ToJson();
            }
            return string.Empty;
        }


        public virtual IEnumerable<BsonDocument> Group(BsonDocument[] bsons)
        {
            var pipeline = new[]
            {
                new BsonDocument("$group", new BsonDocument { { "_id", "$x" }, { "count", new BsonDocument("$sum", 1) } })
            };

            var data = collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return data;
        }

        #endregion

        #region IQueryable<T>        

        public virtual IEnumerator<T> GetEnumerator()
        {
            return collection.AsQueryable().GetEnumerator();
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return collection.AsQueryable().GetEnumerator();
        }

        public virtual Type ElementType
        {
            get
            {
                return collection.AsQueryable().ElementType;
            }
        }

        public virtual Expression Expression
        {
            get
            {
                return collection.AsQueryable().Expression;
            }
        }

        public virtual IQueryProvider Provider
        {
            get
            {
                return collection.AsQueryable().Provider;
            }
        }
        #endregion
    }

    /// <summary>
    /// MongoDB实体操作基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : BaseRepository<T, string>, IRepository<T, string>
        where T : IMongoEntity<string>
    {

        public BaseRepository()
            : this(DBContextExtention.GetDefaultConnectionString())
        {
        }

        /// <summary>
        /// 若设置过MongoServerAddress 、MongReplicaSetName则已Default优先
        /// 否则默认为最后一个ConnectionString设置
        /// </summary>
        /// <param name="connectionString"></param>
        public BaseRepository(string connectionString) : base(connectionString)
        {

        }

        /// <summary>
        /// MongoDB实体操作类
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="collectionName"></param>
        public BaseRepository(string connectionString, string collectionName) : base(connectionString, collectionName)
        {
        }        
    }
}
