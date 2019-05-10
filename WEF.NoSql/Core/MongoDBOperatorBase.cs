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
    /// <typeparam name="TKey">用于实体ID的类型。</typeparam>
    public class MongoDBOperatorBase<T, TKey> : IOperator<T, TKey>
        where T : IMongoEntity<TKey>
    {
        protected internal MongoCollection<T> collection;

        protected string _connStr = string.Empty;


        public string ConnectionString
        {
            get { return _connStr; }
        }


        public MongoDBOperatorBase()
            : this(Extentions<TKey>.GetDefaultConnectionString())
        {
        }

        /// <summary>
        /// 若设置过MongoServerAddress 、MongReplicaSetName则已cluster优先
        /// 否则默认为最后一个ConnectionString设置
        /// </summary>
        /// <param name="connectionString"></param>
        public MongoDBOperatorBase(string connectionString)
        {
            _connStr = connectionString;

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MongReplicaSetName"]) &&
                !string.IsNullOrEmpty(ConfigurationManager.AppSettings["MongoServerAddress"]))
            {
                this.collection = Extentions<TKey>.GetCollectionFromCluster<T>(connectionString);
            }
            else
                this.collection = Extentions<TKey>.GetCollectionFromConnectionString<T>(connectionString);
        }

        public MongoDBOperatorBase(string connectionString, string collectionName)
        {
            _connStr = connectionString;
            this.collection = Extentions<TKey>.GetCollectionFromConnectionString<T>(connectionString, collectionName);
        }

        public MongoDBOperatorBase(MongoUrl url)
        {
            this.collection = Extentions<TKey>.GetCollectionFromUrl<T>(url);
        }

        public MongoDBOperatorBase(MongoUrl url, string collectionName)
        {
            this.collection = Extentions<TKey>.GetCollectionFromUrl<T>(url, collectionName);
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

        public virtual T GetById(TKey id)
        {
            if (typeof(T).IsSubclassOf(typeof(MongoEntity)))
            {
                return this.GetById(new ObjectId(id as string));
            }

            return this.collection.FindOneByIdAs<T>(BsonValue.Create(id));
        }

        public virtual T GetById(ObjectId id)
        {
            return this.collection.FindOneByIdAs<T>(id);
        }

        public virtual T Add(T entity)
        {
            this.collection.Insert<T>(entity);

            return entity;
        }


        public virtual void Add(IEnumerable<T> entities)
        {
            this.collection.InsertBatch<T>(entities);
        }

        public virtual T Update(T entity)
        {
            this.collection.Save<T>(entity);

            return entity;
        }


        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                this.collection.Save<T>(entity);
            }
        }

        public virtual void Delete(TKey id)
        {
            if (typeof(T).IsSubclassOf(typeof(MongoEntity)))
            {
                this.collection.Remove(Query.EQ("_id", new ObjectId(id as string)));
            }
            else
            {
                this.collection.Remove(Query.EQ("_id", BsonValue.Create(id)));
            }
        }

        public virtual void Delete(ObjectId id)
        {
            this.collection.Remove(Query.EQ("_id", id));
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
            this.collection.RemoveAll();
        }

        public virtual long Count()
        {
            return this.collection.Count();
        }


        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return this.collection.AsQueryable<T>().Any(predicate);
        }



        #region IQueryable<T>        

        public virtual IEnumerator<T> GetEnumerator()
        {
            return this.collection.AsQueryable<T>().GetEnumerator();
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.collection.AsQueryable<T>().GetEnumerator();
        }


        public virtual Type ElementType
        {
            get
            {
                return this.collection.AsQueryable<T>().ElementType;
            }
        }

        public virtual Expression Expression
        {
            get
            {
                return this.collection.AsQueryable<T>().Expression;
            }
        }

        public virtual IQueryProvider Provider
        {
            get
            {
                return this.collection.AsQueryable<T>().Provider;
            }
        }
        #endregion
    }
}
