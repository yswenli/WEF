using MongoDB.Driver;
using WEF.NoSql.Model;

namespace WEF.NoSql.Core
{
    public class MongoOperator<T> : MongoDBOperatorBase<T, string>, IOperator<T>
        where T : IMongoEntity<string>
    {

        public MongoOperator()
            : base() { }


        public MongoOperator(MongoUrl url)
            : base(url) { }


        public MongoOperator(MongoUrl url, string collectionName)
            : base(url, collectionName) { }


        public MongoOperator(string connectionString)
            : base(connectionString) { }


        public MongoOperator(string connectionString, string collectionName)
            : base(connectionString, collectionName) { }
    }
}
