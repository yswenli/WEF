/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2019
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：9a4fe848-95cb-4ad2-ac1b-d757a6ea1cd0
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WEF.NoSql
 * 类名称：MongoDBFactory
 * 文件名：MongoDBFactory
 * 创建年份：2015
 * 创建时间：2015-09-29 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
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

        /// <summary>
        /// 断开连接事件
        /// </summary>
        public static event OnDisconnectedHandler OnDisconnected;
        /// <summary>
        /// 异常事件
        /// </summary>
        public static event OnErrorHandler OnError;

        /// <summary>
        /// mongodb工厂类
        /// </summary>
        static MongoDBFactory()
        {
            var ns = new DateTimeSerializer(DateTimeKind.Local, BsonType.DateTime);

            BsonSerializer.RegisterSerializer(typeof(DateTime), ns);

            concurrentDictionary = new ConcurrentDictionary<string, object>();

            MongoCoreExtentions.OnDisconnected += OnDisconnectedAction;

            MongoCoreExtentions.OnError += OnErrorAction;
        }

        /// <summary>
        /// 创建Mongo操作实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IOperator<T> Create<T>() where T : MongoEntity
        {
            return CreateWithName<T>();
        }

        /// <summary>
        /// 创建Mongo操作实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IOperator<T> Create<T>(string connectionString) where T : MongoEntity
        {
            var key = connectionString + typeof(T).Name;

            var mo = concurrentDictionary.GetOrAdd(key, (k) =>
            {
                return new MongoOperator<T>(connectionString);
            });

            return (IOperator<T>)mo;
        }

        /// <summary>
        /// 创建Mongo操作实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appKeyName"></param>
        /// <returns></returns>
        public static IOperator<T> CreateWithAppSetting<T>(string appKeyName = MongoCoreExtentions<ObjectId>.DefaultName) where T : MongoEntity
        {
            var connectionString = ConfigurationManager.AppSettings[appKeyName];

            return Create<T>(connectionString);
        }


        /// <summary>
        /// 创建Mongo操作实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IOperator<T> CreateWithName<T>(string name = MongoCoreExtentions<ObjectId>.DefaultName) where T : MongoEntity
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
