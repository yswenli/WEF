using MongoDB.Bson.Serialization.Attributes;

namespace WEF.NoSql.Model
{
    public interface IMongoEntity<MongoEntityId>
    {
        [BsonId]
        MongoEntityId Id
        {
            get; set;
        }
    }
}
