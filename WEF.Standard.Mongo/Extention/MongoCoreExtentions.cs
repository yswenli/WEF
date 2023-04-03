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
 * 命名空间：WEF.Standard.Mongo.Extention
 * 类名称：MongoCoreExtentions
 * 文件名：MongoCoreExtentions
 * 创建年份：2015
 * 创建时间：2015-09-29 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WEF.Standard.Mongo.Core;
using WEF.Standard.Mongo.Model;

namespace WEF.Standard.Mongo.Extention
{
    /// <summary>
    /// 封装核心扩展类
    /// </summary>
    /// <typeparam name="ObjectId"></typeparam>
    internal static class MongoCoreExtentions<ObjectId>
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
            MongoServer ms = null;
            try
            {
                var mc = new MongoClient(url);

                ms = mc.GetServer();

                ms.WithReadPreference(new ReadPreference(ReadPreferenceMode.SecondaryPreferred));

                if (!ms.Settings.IsFrozen)
                {
                    ms.Settings.ConnectionMode = ConnectionMode.Automatic;

                    ms.Settings.ConnectTimeout = TimeSpan.FromSeconds(10);
                }

                MaintenanceTask(ms);

                return ms.GetDatabase(url.DatabaseName);
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(ms?.Settings?.ToString(), ex, null, null);
            }
            return null;
        }

        /// <summary>  
        /// 取得数据库集群连接  ,
        /// MongoServerAddress
        /// MongReplicaSetName
        /// </summary>  
        /// <returns>数据库连接字符串</returns>  
        private static MongoDatabase GetDatabaseFromAppSettings(string databaseName)
        {
            MongoServer ms = null;
            try
            {
                List<MongoServerAddress> servers = new List<MongoServerAddress>();

                string reg = @"^(?'server'\d{1,}.\d{1,}.\d{1,}.\d{1,}):(?'port'\d{1,})$";

                string[] ServerList = ConfigurationManager.AppSettings[AppKeyNameMongoServerAddress].Trim().Split('|');

                foreach (string server in ServerList)
                {
                    MatchCollection collections = Regex.Matches(server, reg);
                    if (collections != null && collections.Count > 0)
                        servers.Add(new MongoServerAddress(collections[0].Groups["server"].ToString(), Convert.ToInt32(collections[0].Groups["port"].ToString())));
                }

                if (servers == null || servers.Count < 1)
                    return null;

                MongoClientSettings set = new MongoClientSettings();

                set.Servers = servers;

                set.ConnectionMode = ConnectionMode.Automatic;

                set.ReplicaSetName = ConfigurationManager.AppSettings[AppKeyNameMongReplicaSetName].Trim();//设置副本集名称  

                int TimeOut = ConfigurationManager.AppSettings[AppKeyNameTimeOut].ParseInt();//设置副本集名称  

                set.ConnectTimeout = new TimeSpan(0, 0, 0, TimeOut, 0);//设置超时时间为5秒  

                set.ReadPreference = new ReadPreference(ReadPreferenceMode.SecondaryPreferred);

                MongoClient mc = new MongoClient(set);

                ms = mc.GetServer();

                MaintenanceTask(ms);

                return ms.GetDatabase(databaseName);
            }
            catch (Exception ex)
            {
                MongoCoreExtentions.OnError?.BeginInvoke(ms?.Settings?.ToString(), ex, null, null);
            }
            return null;
        }


        public static MongoCollection<T> GetCollectionFromDefault<T>(string databaseName)
            where T : IMongoEntity<ObjectId>
        {
            return GetDatabaseFromAppSettings(databaseName)?.GetCollection<T>(GetCollectionName<T>());
        }

        public static MongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString)
            where T : IMongoEntity<ObjectId>
        {
            return MongoCoreExtentions<ObjectId>.GetCollectionFromConnectionString<T>(connectionString, GetCollectionName<T>());
        }

        public static MongoCollection<T> GetCollectionFromConnectionString<T>(string connectionString, string collectionName)
            where T : IMongoEntity<ObjectId>
        {
            return MongoCoreExtentions<ObjectId>.GetDatabaseFromUrl(new MongoUrl(connectionString))?.GetCollection<T>(collectionName);
        }

        public static MongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url)
            where T : IMongoEntity<ObjectId>
        {
            return MongoCoreExtentions<ObjectId>.GetCollectionFromUrl<T>(url, GetCollectionName<T>());
        }

        public static MongoCollection<T> GetCollectionFromUrl<T>(MongoUrl url, string collectionName)
            where T : IMongoEntity<ObjectId>
        {
            return MongoCoreExtentions<ObjectId>.GetDatabaseFromUrl(url)?.GetCollection<T>(collectionName);
        }

        private static string GetCollectionName<T>() where T : IMongoEntity<ObjectId>
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

        /// <summary>
        /// 记录mongoserver是否连接成功过
        /// </summary>
        static ConcurrentDictionary<MongoServer, bool> _recordConnectedCollection = new ConcurrentDictionary<MongoServer, bool>();

        /// <summary>
        /// 维护任务
        /// </summary>
        /// <param name="mongoServer"></param>
        private static void MaintenanceTask(MongoServer mongoServer)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        if (mongoServer.State == MongoServerState.Disconnected)
                        {
                            if (_recordConnectedCollection.TryGetValue(mongoServer, out bool connected))
                            {
                                if (connected)
                                    MongoCoreExtentions.OnDisconnected?.BeginInvoke(mongoServer.Settings.ToString(), null, null);
                            }
                            mongoServer.Reconnect();
                        }
                        else if (mongoServer.State == MongoServerState.Connected)
                        {
                            _recordConnectedCollection.TryAdd(mongoServer, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        MongoCoreExtentions.OnError?.BeginInvoke(mongoServer.Settings.ToString(), ex, null, null);
                    }
                    finally
                    {
                        Task.Delay(3 * 1000).GetAwaiter().GetResult();
                    }
                }
            });
        }

    }

    /// <summary>
    /// 封装核心扩展类
    /// </summary>
    internal static class MongoCoreExtentions
    {
        internal static OnDisconnectedHandler OnDisconnected;

        internal static OnErrorHandler OnError;
    }
}
