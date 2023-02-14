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
 * 命名空间：WEF.NoSql.Core
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
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using WEF.NoSql.Extention;
using WEF.NoSql.Model;

namespace WEF.NoSql.Core
{
    /// <summary>
    /// MongoDB实体操作类
    /// </summary>
    /// <typeparam name="T">存储库中包含的类型.</typeparam>
    /// <typeparam name="ObjectId">用于实体ID的类型。</typeparam>
    public class MongoDBOperatorBase<T, ObjectId> : IOperator<T, ObjectId>
        where T : IMongoEntity<ObjectId>
    {
        protected internal MongoCollection<T> collection;

        protected string _connStr = string.Empty;
        public string ConnectionString
        {
            get { return _connStr; }
        }


        public MongoDBOperatorBase()
            : this(MongoCoreExtentions<ObjectId>.GetDefaultConnectionString())
        {
        }

        /// <summary>
        /// 若设置过MongoServerAddress 、MongReplicaSetName则已Default优先
        /// 否则默认为最后一个ConnectionString设置
        /// </summary>
        /// <param name="connectionString"></param>
        public MongoDBOperatorBase(string connectionString)
        {
            _connStr = connectionString;

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MongReplicaSetName"]) &&
                !string.IsNullOrEmpty(ConfigurationManager.AppSettings["MongoServerAddress"]))
            {
                this.collection = MongoCoreExtentions<ObjectId>.GetCollectionFromDefault<T>(connectionString);
            }
            else
                this.collection = MongoCoreExtentions<ObjectId>.GetCollectionFromConnectionString<T>(connectionString);
        }

        /// <summary>
        /// MongoDB实体操作类
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="collectionName"></param>
        public MongoDBOperatorBase(string connectionString, string collectionName)
        {
            _connStr = connectionString;
            this.collection = MongoCoreExtentions<ObjectId>.GetCollectionFromConnectionString<T>(connectionString, collectionName);
        }

        /// <summary>
        /// MongoDB实体操作类
        /// </summary>
        /// <param name="url"></param>
        public MongoDBOperatorBase(MongoUrl url)
        {
            this.collection = MongoCoreExtentions<ObjectId>.GetCollectionFromUrl<T>(url);
        }

        /// <summary>
        /// MongoDB实体操作类
        /// </summary>
        /// <param name="url"></param>
        /// <param name="collectionName"></param>
        public MongoDBOperatorBase(MongoUrl url, string collectionName)
        {
            this.collection = MongoCoreExtentions<ObjectId>.GetCollectionFromUrl<T>(url, collectionName);
        }

        public MongoCollection<T> Collection
        {
            get
            {
                return this.collection;
            }
        }


        public string CollectionName
        {
            get
            {
                return this.collection.Name;
            }
        }



        public string ServerInfo
        {
            get
            {
                var cs = this.collection;

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
                var cs = this.collection;

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
            try
            {
                if (typeof(T).IsSubclassOf(typeof(MongoEntity)))
                {
                    return this.GetById(new MongoDB.Bson.ObjectId(id.ToString()));
                }
                return this.collection.FindOneByIdAs<T>(BsonValue.Create(id));
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
            }
            return default(T);
        }

        public virtual T GetById(MongoDB.Bson.ObjectId id)
        {
            try
            {
                return this.collection.FindOneByIdAs<T>(id);
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
            }
            return default(T);
        }

        public virtual T Add(T entity)
        {
            try
            {
                var result = this.collection.Insert<T>(entity);

                if (result != null && result?.Response?.AsBsonValue?.AsBsonDocument?.Elements?.FirstOrDefault().Value?.ToBoolean() == true)
                {
                    return entity;
                }
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
            }
            return default(T);
        }


        public virtual void Add(IEnumerable<T> entities)
        {
            try
            {
                this.collection.InsertBatch<T>(entities);
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
            }
        }

        public virtual T Update(T entity)
        {
            try
            {
                var result = this.collection.Save<T>(entity);

                if (result != null && result?.Response?.AsBsonValue?.AsBsonDocument?.Elements?.FirstOrDefault().Value?.ToBoolean() == true)
                {
                    return entity;
                }
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
            }
            return default(T);
        }


        public virtual void Update(IEnumerable<T> entities)
        {
            try
            {
                foreach (T entity in entities)
                {
                    this.collection.Save<T>(entity);
                }
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
            }
        }

        public virtual void Delete(ObjectId id)
        {
            try
            {
                if (typeof(T).IsSubclassOf(typeof(MongoEntity)))
                {
                    this.collection.Remove(Query.EQ("_id", new MongoDB.Bson.ObjectId(id as string)));
                }
                else
                {
                    this.collection.Remove(Query.EQ("_id", BsonValue.Create(id)));
                }
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
            }
        }

        public virtual void Delete(MongoDB.Bson.ObjectId id)
        {
            try
            {
                this.collection.Remove(Query.EQ("_id", id));
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
            }
        }

        public virtual void Delete(T entity)
        {
            this.Delete(entity.Id);
        }

        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            foreach (T entity in this.collection.AsQueryable<T>().Where(predicate))
            {
                this.Delete(entity.Id);
            }
        }

        public virtual void DeleteAll()
        {
            try
            {
                this.collection.RemoveAll();
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
            }
        }

        public virtual long Count()
        {
            try
            {
                return this.collection.Count();
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
            }
            return -1;
        }


        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return this.collection.AsQueryable<T>().Any(predicate);
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
            }
            return false;
        }


        #region Test

        public virtual string Run(string json = "{\"dbStats\":1}")
        {
            return Run(BsonDocument.Parse(json));
        }


        public virtual string Run(BsonDocument bsons)
        {
            var command = new CommandDocument(bsons);

            var result = this.collection.Database.RunCommand(command);
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

            var data = this.collection.Aggregate(new AggregateArgs { Pipeline = pipeline });

            return data;
        }

        #endregion

        #region IQueryable<T>        

        public virtual IEnumerator<T> GetEnumerator()
        {
            try
            {
                return this.collection.AsQueryable<T>().GetEnumerator();
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
            }
            return null;
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            try
            {
                return this.collection.AsQueryable<T>().GetEnumerator();
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
            }
            return null;
        }

        public virtual Type ElementType
        {
            get
            {
                try
                {
                    return this.collection.AsQueryable<T>().ElementType;
                }
                catch (Exception ex)
                {
                    MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
                }
                return null;
            }
        }

        public virtual Expression Expression
        {
            get
            {
                try
                {
                    return this.collection.AsQueryable<T>().Expression;
                }
                catch (Exception ex)
                {
                    MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
                }
                return null;
            }
        }

        public virtual IQueryProvider Provider
        {
            get
            {
                try
                {
                    return this.collection.AsQueryable<T>().Provider;
                }
                catch (Exception ex)
                {
                    MongoCoreExtentions.OnError?.BeginInvoke(this.collection?.Database?.Server?.Settings?.ToString(), ex, null, null);
                }
                return null;
            }
        }
        #endregion
    }
}
