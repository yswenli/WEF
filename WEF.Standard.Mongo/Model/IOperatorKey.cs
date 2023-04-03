using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WEF.Standard.Mongo.Model
{
    public interface IOperator<T, TKey> : IQueryable<T>
        where T : IMongoEntity<TKey>
    {
        string ConnectionString
        {
            get;
        }

        MongoCollection<T> Collection
        {
            get;
        }

        T GetById(TKey id);

        T Add(T entity);

        void Add(IEnumerable<T> entities);

        T Update(T entity);

        void Update(IEnumerable<T> entities);

        void Delete(TKey id);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> predicate);

        void DeleteAll();

        long Count();

        bool Exists(Expression<Func<T, bool>> predicate);


        string Run(string json = "{\"dbStats\":1}");



        IEnumerable<string> CollectionNames
        {
            get;
        }

        string DataBaseName
        {
            get;
        }

        IEnumerable<string> DataBaseNames
        {
            get;
        }

        string ServerInfo
        {
            get;
        }
    }
}
