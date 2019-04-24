using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WEF.NoSql.Model
{
    public interface IOperator<T, TKey> : IQueryable<T>
        where T : IMongoEntity<TKey>
    {
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
    }
}
