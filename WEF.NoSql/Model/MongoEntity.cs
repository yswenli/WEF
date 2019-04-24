using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Runtime.Serialization;

namespace WEF.NoSql.Model
{
    /// <summary>
    /// mongo实体
    /// </summary>
    [DataContract]
    [Serializable]
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class MongoEntity : IMongoEntity<string>
    {
        /// <summary>
        /// 获取或设置此对象的ID（实体的主要记录）。
        /// </summary>
        /// <value>此对象的ID（实体的主要记录）。</value>
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id
        {
            get; set;
        }
    }
}
