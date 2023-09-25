/****************************************************************************
*Copyright (c) 2023 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：河之洲
*命名空间：WEF.Standard.Mongo.Core
*文件名： DBContext
*版本号： V1.0.0.0
*唯一标识：02d5db8c-7591-44ec-800b-389a9510faee
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2023/9/25 15:46:56
*描述：
*
*=================================================
*修改标记
*修改时间：2023/9/25 15:46:56
*修改人： yswenli
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using MongoDB.Driver;
using WEF.Standard.Mongo.Model;

namespace WEF.Standard.Mongo.Core
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class DBContext
    {
        MongoClient _mongoClient;
        MongoServer _mongoServer;
        MongoDatabase _mongoDatabase;

        /// <summary>
        /// 数据库上下文
        /// </summary>
        /// <param name="url"></param>
        /// <param name="databaseName"></param>
        public DBContext(MongoUrl url, string databaseName = "")
        {
            _mongoClient = DBContextExtention.GetClient(url);
            _mongoServer = DBContextExtention.GetServer(url, _mongoClient);
            _mongoDatabase = DBContextExtention.GetDatabase(url, _mongoServer, databaseName);
        }
        /// <summary>
        /// 数据库上下文
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        public DBContext(string connectionString, string databaseName = "")
            : this(new MongoUrl(connectionString), databaseName)
        {
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal MongoCollection<T> GetCollection<T, Key>(string collectionName = "") where T : IMongoEntity<Key>
        {
            if (string.IsNullOrEmpty(collectionName))
            {
                collectionName = DBContextExtention.GetCollectionName<T, Key>();
            }
            return _mongoDatabase.GetCollection<T>(collectionName);
        }
    }
}
