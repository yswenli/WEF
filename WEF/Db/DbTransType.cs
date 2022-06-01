/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Db
*文件名： DbTransType
*版本号： V1.0.0.0
*唯一标识：d4e82219-272a-4d57-b315-c9a3814a8422
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2021/12/8 9:29:46
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/12/8 9:29:46
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System.Data;

namespace WEF.Db
{
    /// <summary>
    /// 事务类型
    /// </summary>
    public enum DbTransType
    {
        /// <summary>
        /// 未规定，一般按db server的设置
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// 当事务A更新某条数据的时候，不容许其他事务来更新该数据，但可以进行读取操作，可以进行脏读，意思是说，不发布共享锁，也不接受独占锁。
        /// NOLOCK
        /// </summary>
        WriteLock = 1,
        /// <summary>
        /// 当事务A更新数据时，不容许其他事务进行任何的操作包括读取，但事务A读取时，其他事务可以进行读取、更新.
        /// READPAST
        /// </summary>
        ReadLock = 2,
        /// <summary>
        /// 当事务A更新数据时，不容许其他事务进行任何的操作，但是当事务A进行读取的时候，其他事务只能读取，不能更新.
        /// UPDLOCK
        /// </summary>
        RepeatableRead = 3,
        /// <summary>
        /// 串行执行锁，在事务中的操作都将排队执行.
        /// HOLDLOCK
        /// </summary>
        QueueLock = 4
    }

    /// <summary>
    /// 事务类型转换
    /// </summary>
    public static class DBTransTypeConverter
    {
        /// <summary>
        /// 将类型转换成内置的级别
        /// </summary>
        /// <param name="dbTransType"></param>
        /// <returns></returns>
        public static IsolationLevel To(DbTransType dbTransType)
        {
            switch (dbTransType)
            {
                case DbTransType.Unspecified:
                    return IsolationLevel.Unspecified;
                case DbTransType.WriteLock:
                    return IsolationLevel.ReadUncommitted;
                case DbTransType.ReadLock:
                    return IsolationLevel.ReadCommitted;
                case DbTransType.RepeatableRead:
                    return IsolationLevel.RepeatableRead;
                case DbTransType.QueueLock:
                    return IsolationLevel.Serializable;
                default:
                    return IsolationLevel.ReadCommitted;
            }
        }
        /// <summary>
        /// 将内置级别转换成类型
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public static DbTransType From(IsolationLevel isolationLevel)
        {
            switch (isolationLevel)
            {
                case IsolationLevel.Unspecified:
                    return DbTransType.Unspecified;
                case IsolationLevel.ReadUncommitted:
                    return DbTransType.WriteLock;
                case IsolationLevel.ReadCommitted:
                    return DbTransType.ReadLock;
                case IsolationLevel.RepeatableRead:
                    return DbTransType.RepeatableRead;
                case IsolationLevel.Serializable:
                    return DbTransType.QueueLock;
                default:
                    return DbTransType.ReadLock;
            }
        }
    }
}
