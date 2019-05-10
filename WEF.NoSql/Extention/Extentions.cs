using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using WEF.NoSql.Model;

namespace WEF.NoSql.Extention
{
    internal static class Extentions<U>
    {
        internal const string DefaultName = "MongoDBConnectStr";

        internal const string AppKeyNameMongoServerAddress = "MongoServerAddress";

        internal const string AppKeyNameMongReplicaSetName = "MongReplicaSetName";

        internal const string AppKeyNameTimeOut = "TimeOut";

        public static string GetConnectionString(string settingName)
        {
            return ConfigurationManager.ConnectionStrings[settingName].ConnectionString;
        }

        public static string GetDefaultConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[DefaultName].ConnectionString;
        }

        private static MongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
            return new MongoClient(url).GetServer().GetDatabase(url.DatabaseName);
        }

        /// <summary>  
        /// 取得数据库集群连接  ,
        /// MongoServerAddress
        /// MongReplicaSetName
        /// </summary>  
        /// <returns>数据库连接字符串</returns>  
        private static MongoDatabase GetDatabaseFromAppSettings(string databaseName)
        {
            List<MongoServerAddress> servers = new List<MongoServerAddress>();

            string reg = @"^(?'server'\d{1,}.\d{1,}.\d{1,}.\d{1,}):(?'port'\d{1,})$";

            string[] ServerList = ConfigurationManager.AppSettings[AppKeyNameMongoServerAddress].Trim().Split('|');

            foreach (string server in ServerList)
            {
                MatchCollection mc = Regex.Matches(server, reg);
                if (mc != null && mc.Count > 0)
                    servers.Add(new MongoServerAddress(mc[0].Groups["server"].ToString(), Convert.ToInt32(mc[0].Groups["port"].ToString())));
            }

            if (servers == null || servers.Count < 1)
                return null;

            MongoClientSettings set = new MongoClientSettings();

            set.Servers = servers;

            set.ReplicaSetName = ConfigurationManager.AppSettings[AppKeyNameMongReplicaSetName].Trim();//设置副本集名称  

            int TimeOut = ConfigurationManager.AppSettings[AppKeyNameTimeOut].ParseInt();//设置副本集名称  

            set.ConnectTimeout = new TimeSpan(0, 0, 0, TimeOut, 0);//设置超时时间为5秒  

            set.ReadPreference = new ReadPreference(ReadPreferenceMode.SecondaryPreferred);

            MongoClient client = new MongoClient(set);

            return client.GetServer().GetDatabase(databaseName);
        }


        public static MongoCollection<T> GetCollectionFromCluster<T>(string databaseName)
            where T : IMongoEntity<U>
        {
            return GetDatabaseFromAppSettings(databaseName).GetCollection<T>(GetCollectionName<T>());
        }

        public static MongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString)
            where T : IMongoEntity<U>
        {
            return Extentions<U>.GetCollectionFromConnectionString<T>(connectionString, GetCollectionName<T>());
        }

        public static MongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString, string collectionName)
            where T : IMongoEntity<U>
        {
            return Extentions<U>.GetDatabaseFromUrl(new MongoUrl(connectionString))
                .GetCollection<T>(collectionName);
        }

        public static MongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url)
            where T : IMongoEntity<U>
        {
            return Extentions<U>.GetCollectionFromUrl<T>(url, GetCollectionName<T>());
        }

        public static MongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url, string collectionName)
            where T : IMongoEntity<U>
        {
            return Extentions<U>.GetDatabaseFromUrl(url)
                .GetCollection<T>(collectionName);
        }

        private static string GetCollectionName<T>() where T : IMongoEntity<U>
        {
            string collectionName;
            if (typeof(T).BaseType.Equals(typeof(object)))
            {
                collectionName = GetCollectioNameFromInterface<T>();
            }
            else
            {
                collectionName = GetCollectionNameFromType(typeof(T));
            }

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty for this entity");
            }
            return collectionName;
        }

        private static string GetCollectioNameFromInterface<T>()
        {
            string collectionname;

            // 查看对象（实体继承）有一个collectionname属性
            var att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionBase));
            if (att != null)
            {
                // 返回由collectionname属性指定的值
                collectionname = ((CollectionNameAttribute)att).Name;
            }
            else
            {
                collectionname = typeof(T).Name;
            }

            return collectionname;
        }

        /// <summary>
        /// 从指定CollectionName确定类型。
        /// </summary>
        /// <param name="entitytype"></param>
        /// <returns>返回指定类型的collectionname.</returns>
        private static string GetCollectionNameFromType(Type entitytype)
        {
            string collectionname;

            var att = Attribute.GetCustomAttribute(entitytype, typeof(CollectionNameAttribute));
            if (att != null)
            {
                collectionname = ((CollectionNameAttribute)att).Name;
            }
            else
            {
                if (typeof(MongoEntity).IsAssignableFrom(entitytype))
                {
                    while (!entitytype.BaseType.Equals(typeof(MongoEntity)))
                    {
                        entitytype = entitytype.BaseType;
                    }
                }
                collectionname = entitytype.Name;
            }

            return collectionname;
        }


    }
}
