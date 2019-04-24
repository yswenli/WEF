using MongoDB.Bson.Serialization.Attributes;

namespace WEF.NoSql.Model
{
    public interface IMongoEntity<TKey>
    {
        [BsonId]
        TKey Id
        {
            get; set;
        }
    }
}
