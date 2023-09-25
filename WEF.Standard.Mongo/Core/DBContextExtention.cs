/****************************************************************************
*Copyright (c) 2023 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：河之洲
*命名空间：WEF.Standard.Mongo.Extention
*文件名： DBContextExtention
*版本号： V1.0.0.0
*唯一标识：ef54a598-b5c1-4dab-883c-59b910242b1e
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2023/9/25 15:51:48
*描述：数据上下文扩展类
*
*=================================================
*修改标记
*修改时间：2023/9/25 15:51:48
*修改人： yswenli
*版本号： V1.0.0.0
*描述：数据上下文扩展类
*
*****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;

using WEF.Standard.Mongo.Model;

namespace WEF.Standard.Mongo.Core
{
    /// <summary>
    /// 数据上下文扩展类
    /// </summary>
    internal static class DBContextExtention
    {
        static Dictionary<string, MongoClient> _clientDic;
        static Dictionary<string, MongoServer> _serverDic;
        static Dictionary<string, MongoDatabase> _dataBaseDic;

        static object _locker;

        internal const string DefaultName = "MongoDBConnectStr";

        static void MaintenanceTask()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var servers = _serverDic.Values;

                    foreach (var mongoServer in servers)
                    {
                        try
                        {
                            if (mongoServer != null && mongoServer.State != MongoServerState.Connected)
                            {
                                mongoServer.Reconnect();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    Thread.Sleep(5000);
                }
            }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 数据上下文扩展类
        /// </summary>
        static DBContextExtention()
        {
            var ns = new DateTimeSerializer(DateTimeKind.Local, BsonType.DateTime);
            BsonSerializer.RegisterSerializer(typeof(DateTime), ns);

            _clientDic = new Dictionary<string, MongoClient>();
            _serverDic = new Dictionary<string, MongoServer>();
            _dataBaseDic = new Dictionary<string, MongoDatabase>();
            _locker = new object();
            MaintenanceTask();
        }



        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static MongoClient GetClient(MongoUrl url)
        {
            lock (_locker)
            {
                var key = url.ToString();
                if (_clientDic.ContainsKey(key))
                {
                    return _clientDic[key];
                }
                else
                {
                    var mc = new MongoClient(url);
                    _clientDic.Add(key, mc);
                    return mc;
                }
            }
        }

        public static MongoServer GetServer(MongoUrl url, MongoClient mc)
        {
            lock (_locker)
            {
                var key = url.ToString();

                if (_serverDic.ContainsKey(key))
                {
                    return _serverDic[key];
                }
                else
                {
                    var ms = mc.GetServer();
                    ms.WithReadPreference(new ReadPreference(ReadPreferenceMode.SecondaryPreferred));
                    if (!ms.Settings.IsFrozen)
                    {
                        ms.Settings.ConnectionMode = ConnectionMode.Automatic;

                        ms.Settings.ConnectTimeout = TimeSpan.FromSeconds(10);
                    }
                    _serverDic[key] = ms;
                    return ms;
                }
            }
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ms"></param>
        /// <param name="dataBaseName"></param>
        /// <returns></returns>
        public static MongoDatabase GetDatabase(MongoUrl url, MongoServer ms, string dataBaseName = null)
        {
            lock (_locker)
            {
                if (string.IsNullOrEmpty(dataBaseName))
                {
                    dataBaseName = url.DatabaseName;
                }
                var key = $"{url}_{dataBaseName}";
                if (_dataBaseDic.ContainsKey(key))
                {
                    return _dataBaseDic[key];
                }
                else
                {
                    var db = ms.GetDatabase(dataBaseName);
                    _dataBaseDic[key] = db;
                    return db;
                }
            }
        }

        /// <summary>
        /// 获取集合名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Key"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetCollectionName<T, Key>() where T : IMongoEntity<Key>
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

        /// <summary>
        /// 从指定CollectionName确定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

        public static string GetDefaultConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[DefaultName].ConnectionString;
        }

    }
}
