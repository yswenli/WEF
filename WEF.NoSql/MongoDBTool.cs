/****************************************************************************
*项目名称：WEF.NoSql
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：WEF.NoSql
*类 名 称：MongoDBTool
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/5/9 13:35:43
*描述：
*=====================================================================
*修改时间：2019/5/9 13:35:43
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WEF.NoSql.Model;

namespace WEF.NoSql
{
    public class MongoDBTool
    {
        static ConcurrentDictionary<string, MongoClient> _cache = new ConcurrentDictionary<string, MongoClient>();

        MongoClient _mongoClient = null;

        internal MongoDBTool(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;

            _mongoClient.GetServer().Connect();
        }

        public static MongoDBTool Connect(string connectStr)
        {
            return new MongoDBTool(_cache.GetOrAdd(connectStr, new MongoClient(new MongoUrl(connectStr))));
        }

        public string ServerInfo
        {
            get
            {
                return _mongoClient.GetServer().Instance.GetIPEndPoint().ToString();
            }
        }

        public IEnumerable<string> GetDataBases()
        {
            return _mongoClient.GetServer().GetDatabaseNames();
        }

        public IEnumerable<string> GetCollections(string dataBaseName)
        {
            return _mongoClient.GetServer().GetDatabase(dataBaseName).GetCollectionNames();
        }

        public List<MongoResult> GetList(string dataBaseName, string collectionName, string json)
        {
            var bson = BsonDocument.Parse(json);

            var rc= new CommandDocument(bson);

            var result= _mongoClient.GetServer().GetDatabase(dataBaseName).RunCommand(rc);

            return new List<MongoResult>();
        }
    }
}
