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

        public static event OnDisconnectedHandler OnDisconnected;

        public static event OnErrorHandler OnError;

        static MongoDBFactory()
        {
            var ns = new DateTimeSerializer(DateTimeKind.Local, BsonType.DateTime);

            BsonSerializer.RegisterSerializer(typeof(DateTime), ns);

            concurrentDictionary = new ConcurrentDictionary<string, object>();

            MongoExtentions.OnDisconnected += OnDisconnectedAction;

            MongoExtentions.OnError += OnErrorAction;
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


        public static IOperator<T> CreateWithAppSetting<T>(string appKeyName = MongoExtentions<ObjectId>.DefaultName) where T : MongoEntity
        {
            var connectionString = ConfigurationManager.AppSettings[appKeyName];

            return Create<T>(connectionString);
        }



        public static IOperator<T> CreateWithName<T>(string name = MongoExtentions<ObjectId>.DefaultName) where T : MongoEntity
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name].ToString();

            return Create<T>(connectionString);
        }


        internal static void OnDisconnectedAction(string settingInfo)
        {
            OnDisconnected?.Invoke($"MongoDBFactory.OnDisconnected,settingInfo:{settingInfo}");
        }

        internal static void OnErrorAction(string settingInfo, Exception ex)
        {
            OnError?.Invoke($"MongoDBFactory.OnError,settingInfo:{settingInfo}", ex);
        }

    }
}
