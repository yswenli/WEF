using MongoDB.Bson.Serialization.Attributes;

namespace WEF.Standard.Mongo.Model
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
