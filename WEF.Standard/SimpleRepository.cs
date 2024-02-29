namespace WEF
{
    /// <summary>
    /// 静态仓库工具类
    /// </summary>
    public static class SimpleRepository
    {
        static string _cnnStr;
        static DatabaseType _databaseType;

        /// <summary>
        /// 初始化仓库工具类
        /// </summary>
        /// <param name="cnnStr"></param>
        /// <param name="databaseType"></param>
        public static void Init(string cnnStr, DatabaseType databaseType)
        {
            _cnnStr = cnnStr;
            _databaseType = databaseType;
        }

        /// <summary>
        /// 快捷获取仓储处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static BaseRepository<T> Create<T>() where T : Entity, new()
        {
            return new BaseRepository<T>(_databaseType, _cnnStr);
        }

        /// <summary>
        /// 当前实体查询上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Search<T> Search<T>() where T : Entity, new()
        {
            return Create<T>().Search();
        }

        /// <summary>
        /// 当前实体查询上下文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Search<T> Query<T>() where T : Entity, new()
        {
            return Create<T>().Search();
        }
    }
}
