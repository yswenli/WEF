using MongoDB.Driver;
using System.Collections.Generic;

namespace WEF.NoSql.Model
{
    /// <summary>
    /// 基于mongocsharpdriver 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IOperatorManager<T, TKey>
        where T : IMongoEntity<TKey>
    {

        bool Exists
        {
            get;
        }


        string Name
        {
            get;
        }


        void Drop();


        bool IsCapped();


        void DropIndex(string keyname);


        void DropIndexes(IEnumerable<string> keynames);


        void DropAllIndexes();


        void EnsureIndex(string keyname);


        void EnsureIndex(string keyname, bool descending, bool unique, bool sparse);


        void EnsureIndexes(IEnumerable<string> keynames);

        void EnsureIndexes(IEnumerable<string> keynames, bool descending, bool unique, bool sparse);

        void EnsureIndexes(IMongoIndexKeys keys, IMongoIndexOptions options);


        bool IndexExists(string keyname);


        bool IndexesExists(IEnumerable<string> keynames);

        void ReIndex();

        ValidateCollectionResult Validate();

        CollectionStatsResult GetStats();

        GetIndexesResult GetIndexes();
    }
}
