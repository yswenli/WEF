/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Standard.DevelopTools.Common
*文件名： BlockingQueue
*版本号： V1.0.0.0
*唯一标识：6c996c42-159a-4015-98b5-0d393777f494
*当前的用户域：WALLE
*创建人： wenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/8/31 16:50:02
*描述：
*
*=================================================
*修改标记
*修改时间：2022/8/31 16:50:02
*修改人： yswenli
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Concurrent;

namespace WEF.Standard.DevelopTools.Common
{
    /// <summary>
    /// 线程安全的阻塞式队列
    /// </summary>
    public class BlockingQueue<T> : IDisposable
    {
        BlockingCollection<T> _queue;

        /// <summary>
        /// 阻塞式队列
        /// </summary>
        /// <param name="boundedCapacity"></param>
        public BlockingQueue(int boundedCapacity = 1000)
        {
            _queue = new BlockingCollection<T>(boundedCapacity);
        }

        /// <summary>
        /// 长度
        /// </summary>
        public int Count
        {
            get
            {
                return _queue.Count;
            }
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="t"></param>
        public void Enqueue(T t)
        {
            _queue.Add(t);
        }
        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="t"></param>
        /// <param name="timeOut"></param>
        public bool Enqueue(T t, int timeOut)
        {
            if (timeOut > 0)
            {
                return _queue.TryAdd(t, timeOut);
            }
            else
            {
                Enqueue(t);
            }
            return true;
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            return _queue.Take();
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public T Dequeue(int timeOut)
        {
            if (timeOut > 0)
            {
                if (_queue.TryTake(out T t, timeOut))
                {
                    return t;
                }
                return default;
            }
            else
            {
                return Dequeue();
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _queue.CompleteAdding();
            _queue.Dispose();
        }
    }
}
