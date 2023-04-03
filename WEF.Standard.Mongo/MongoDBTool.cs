/****************************************************************************
*项目名称：WEF.Standard.Mongo
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：WEF.Standard.Mongo
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
using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WEF.Standard.Mongo.Model;

namespace WEF.Standard.Mongo
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
            using (var cursor = _mongoClient.ListDatabases())
            {
                foreach (var document in cursor.ToEnumerable())
                {
                    yield return document["name"].ToString();
                }
            }
        }

        public IEnumerable<string> GetCollections(string dataBaseName)
        {
            using (var cursor = _mongoClient.GetDatabase(dataBaseName).ListCollections())
            {
                foreach (var document in cursor.ToEnumerable())
                {
                    yield return document["name"].ToString();
                }
            }

            //var db = _mongoClient.GetDatabase(dataBaseName, null).RunCommand<string>("");
        }

        public List<MongoResult> GetList(string dataBaseName, string collectionName, string cmd= "{\"find\":\"test\", limit:20, sort:{AddTime:-1}}")
        {

            var db = _mongoClient.GetDatabase(dataBaseName, null);

            var result= db.RunCommand<string>(cmd);

            return new List<MongoResult>();
        }
    }
}
