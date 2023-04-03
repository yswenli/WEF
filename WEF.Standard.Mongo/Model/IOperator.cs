using System.Linq;

namespace WEF.Standard.Mongo.Model
{
    public interface IOperator<T> : IQueryable<T>, IOperator<T, string>
        where T : IMongoEntity<string>
    {
    }
}
