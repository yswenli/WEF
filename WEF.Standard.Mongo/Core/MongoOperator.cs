/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：9a4fe848-95cb-4ad2-ac1b-d757a6ea1cd0
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WEF.Standard.Mongo.Core
 * 类名称：MongoOperator<T>
 * 文件名：MongoOperator<T>
 * 创建年份：2015
 * 创建时间：2015-09-29 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using MongoDB.Driver;
using WEF.Standard.Mongo.Model;

namespace WEF.Standard.Mongo.Core
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
