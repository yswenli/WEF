using WEF.NoSql.Core;
using WEF.NoSql.Model;

namespace WEF.NoSql
{
    public static class MongoDBFactory
    {
        public static IOperator<T> Create<T>() where T : MongoEntity
        {
            return new MongoOperator<T>();
        }

        public static IOperator<T> Create<T>(string connectionString) where T : MongoEntity
        {
            return new MongoOperator<T>(connectionString);
        }
    }
}
