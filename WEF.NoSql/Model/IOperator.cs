using System.Linq;

namespace WEF.NoSql.Model
{
    public interface IOperator<T> : IQueryable<T>, IOperator<T, string>
        where T : IMongoEntity<string>
    {
    }
}
