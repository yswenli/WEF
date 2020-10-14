/****************************************************************************
*项目名称：WEF.Persists
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Persists
*类 名 称：DataPersistence
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/10/13 9:52:41
*描述：
*=====================================================================
*修改时间：2020/10/13 9:52:41
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Concurrent;

namespace WEF.Persists
{
    /// <summary>
    /// 数据持久化
    /// </summary>
    public static class DataPersistence
    {
        static ConcurrentDictionary<string, IClassifiedDataPersistence> _dic;

        /// <summary>
        /// 数据持久化
        /// </summary>
        static DataPersistence()
        {
            _dic = new ConcurrentDictionary<string, IClassifiedDataPersistence>();
        }

        /// <summary>
        /// OnError
        /// </summary>
        public static event EventHandler<Exception> OnError;


        /// <summary>
        /// 创建持久化类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="databaseType"></param>
        /// <param name="connectStr"></param>
        /// <param name="timeout"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static ClassifiedDataPersistence<T> Create<T>(DatabaseType databaseType = DatabaseType.Undefined, string connectStr = "", int timeout = 10 * 1000, int maxCount = 10000) where T : Entity
        {
            return (ClassifiedDataPersistence<T>)_dic.GetOrAdd(typeof(T).Name, (k) =>
            {
                var ct = new ClassifiedDataPersistence<T>(databaseType, connectStr, timeout, maxCount);
                ct.OnError += Ct_OnError;
                return (IClassifiedDataPersistence)ct;
            });
        }


        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void Insert<T>(T t) where T : Entity
        {
            var cdp = Create<T>();

            cdp.Insert(t);
        }



        private static void Ct_OnError(object sender, Exception e)
        {
            OnError?.Invoke(sender, e);
        }
    }
}
