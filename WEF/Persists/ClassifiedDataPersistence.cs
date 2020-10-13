/****************************************************************************
*项目名称：WEF.Persists
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Persists
*类 名 称：ClassifiedDataPersistence
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/10/13 9:53:12
*描述：
*=====================================================================
*修改时间：2020/10/13 9:53:12
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WEF.Persists
{
    /// <summary>
    /// 分类数据持久化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ClassifiedDataPersistence<T> : IClassifiedDataPersistence where T : Entity
    {
        ConcurrentBag<T> _bag;

        Stopwatch _stopwatch;

        int _timeout = 60 * 1000, _maxcount = 100000;

        string _type = "";

        DatabaseType _databaseType;

        string _connectStr;

        /// <summary>
        /// OnError
        /// </summary>
        public event EventHandler<Exception> OnError;

        /// <summary>
        /// 分类数据持久化
        /// </summary>
        public ClassifiedDataPersistence() : this(60 * 1000, 100000)
        {

        }
        /// <summary>
        /// 分类数据持久化
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="maxcount"></param>
        public ClassifiedDataPersistence(int timeout, int maxcount) : this(DatabaseType.Undefined, "", timeout, maxcount)
        {

        }

        /// <summary>
        /// 分类数据持久化
        /// </summary>
        /// <param name="databaseType"></param>
        /// <param name="connectStr"></param>
        /// <param name="timeout"></param>
        /// <param name="maxcount"></param>
        public ClassifiedDataPersistence(DatabaseType databaseType, string connectStr, int timeout, int maxcount)
        {
            _databaseType = databaseType;

            _connectStr = connectStr;

            _timeout = timeout;

            _maxcount = maxcount;

            _bag = new ConcurrentBag<T>();

            _stopwatch = Stopwatch.StartNew();

            _type = typeof(T).Name;

            Task.Factory.StartNew(Process, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="t"></param>
        public void Insert(T t)
        {
            while (_bag.Count > 10 * _maxcount)
            {
                Thread.Sleep(50);
            }
            _bag.Add(t);
        }

        DBContext GetDBContext()
        {
            if (_databaseType == DatabaseType.Undefined || string.IsNullOrEmpty(_connectStr))
            {
                return new DBContext();
            }
            else
            {
                return new DBContext(_databaseType, _connectStr);
            }
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        void Process()
        {
            while (true)
            {
                try
                {
                    if (_bag.Count >= _maxcount || (_bag.Count > 0 && _stopwatch.Elapsed.TotalMilliseconds >= _timeout))
                    {
                        var rlist = new List<T>();

                        for (int i = 0; i < _bag.Count; i++)
                        {
                            if (_bag.TryTake(out T t))
                            {
                                rlist.Add(t);
                            }
                        }

                        GetDBContext().BulkInsert(rlist);

                        rlist.Clear();

                        _stopwatch.Restart();
                    }
                    else
                        Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(this, ex);
                }

            }
        }
    }
}
