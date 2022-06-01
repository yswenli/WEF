/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.ModelGenerator.Common
*文件名： MessageQueue
*版本号： V1.0.0.0
*唯一标识：33986572-cc52-4214-a294-fe55478786fa
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2022/2/24 18:05:28
*描述：
*
*=====================================================================
*修改标记
*修改时间：2022/2/24 18:05:28
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace WEF.ModelGenerator.Common
{
    /// <summary>
    /// 消息队列
    /// </summary>
    public class MessageQueue
    {
        ConcurrentQueue<string> _queue;

        public event Action<string> OnMessage;

        public event Action OnComplete;

        DateTime _last;

        /// <summary>
        /// 消息队列
        /// </summary>
        MessageQueue()
        {
            _last = DateTime.Now;
            _queue = new ConcurrentQueue<string>();
            Task.Factory.StartNew(() =>
            {
                do
                {
                    if (_queue.TryDequeue(out string result))
                    {
                        OnMessage?.Invoke(result);
                        _last = DateTime.Now;
                    }
                    Thread.Sleep(10);
                }
                while (true);
            }, TaskCreationOptions.LongRunning);

            Task.Factory.StartNew(() =>
            {
                do
                {
                    if (_last.AddSeconds(5) < DateTime.Now)
                    {
                        OnComplete?.Invoke();
                    }
                    Thread.Sleep(1);
                }
                while (true);

            }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// SubMsg
        /// </summary>
        /// <param name="msg"></param>
        public void SubMsg(string msg)
        {
            _queue.Enqueue(msg);
        }


        static Lazy<MessageQueue> _lazy = new Lazy<MessageQueue>(() => new MessageQueue());

        /// <summary>
        /// MessageQueue
        /// </summary>
        public static MessageQueue Instance
        {
            get
            {
                return _lazy.Value;
            }
        }
    }
}
