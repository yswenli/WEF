namespace WEF.NoSql.Model
{
    public interface IOperatorManager<T> : IOperatorManager<T, string>
        where T : IMongoEntity<string>
    {
    }
}
