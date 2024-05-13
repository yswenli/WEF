/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2024
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：9a4fe848-95cb-4ad2-ac1b-d757a6ea1cd0
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WEF.Standard.Mongo
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
using WEF.Standard.Mongo.Core;
using WEF.Standard.Mongo.Model;

namespace WEF.Standard.Mongo
{
    /// <summary>
    /// mongodb工厂类
    /// </summary>
    public static class MongoDBFactory
    {
        static ConcurrentDictionary<string, object> _concurrentDictionary = null;

        /// <summary>
        /// mongodb工厂类
        /// </summary>
        static MongoDBFactory()
        {
            _concurrentDictionary = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// 创建Mongo操作实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IRepository<T> Create<T>() where T : MongoEntity
        {
            return CreateWithName<T>();
        }

        /// <summary>
        /// 创建Mongo操作实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IRepository<T> Create<T>(string connectionString) where T : MongoEntity
        {
            var mo = _concurrentDictionary.GetOrAdd(DBContextExtention.DefaultName, (k) =>
            {
                return new BaseRepository<T>(connectionString);
            });

            return (IRepository<T>)mo;
        }

        /// <summary>
        /// 创建Mongo操作实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appKeyName"></param>
        /// <returns></returns>
        public static IRepository<T> CreateWithAppSetting<T>(string appKeyName = DBContextExtention.DefaultName) where T : MongoEntity
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
        public static IRepository<T> CreateWithName<T>(string name = DBContextExtention.DefaultName) where T : MongoEntity
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name].ToString();

            return Create<T>(connectionString);
        }

    }
}
