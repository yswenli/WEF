using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using WEF.NoSql.Core;
using WEF.NoSql.Extention;
using WEF.NoSql.Model;

namespace WEF.NoSql
{
    /// <summary>
    /// mongodb工厂类
    /// </summary>
    public static class MongoDBFactory
    {
        static ConcurrentDictionary<string, object> concurrentDictionary = null;

        static MongoDBFactory()
        {
            var ns = new DateTimeSerializer(DateTimeKind.Local, BsonType.DateTime);

            BsonSerializer.RegisterSerializer(typeof(DateTime), ns);

            concurrentDictionary = new ConcurrentDictionary<string, object>();
        }

        public static IOperator<T> Create<T>() where T : MongoEntity
        {
            return CreateWithName<T>();
        }


        public static IOperator<T> Create<T>(string connectionString) where T : MongoEntity
        {
            var key = connectionString + typeof(T).Name;

            var mo = concurrentDictionary.GetOrAdd(key, (k) =>
            {
                return new MongoOperator<T>(connectionString);
            });
            return (IOperator<T>)mo;
        }


        public static IOperator<T> CreateWithAppSetting<T>(string appKeyName = Extentions<T>.DefaultName) where T : MongoEntity
        {
            var connectionString = ConfigurationManager.AppSettings[appKeyName];

            return Create<T>(connectionString);
        }



        public static IOperator<T> CreateWithName<T>(string name = Extentions<T>.DefaultName) where T : MongoEntity
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name].ToString();

            return Create<T>(connectionString);
        }
    }
}
