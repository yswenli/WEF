/****************************************************************************
*项目名称：WEF.NoSql
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：WEF.NoSql
*类 名 称：FindCommandResult
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/5/15 13:45:05
*描述：
*=====================================================================
*修改时间：2019/5/15 13:45:05
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace WEF.NoSql.Model
{
    [BsonIgnoreExtraElements]
    public class FindCommandResult<T> : IMongoCommand
    {
        [BsonElement("cursor")]
        public ResultCursor<T> Cursor { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ResultCursor<T>
    {
        [BsonElement("firstBatch")]
        public T[] Batch { get; set; }
    }


}
