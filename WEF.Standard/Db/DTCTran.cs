/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Common
*文件名： DTCTran
*版本号： V1.0.0.0
*唯一标识：5f2f048d-3c8f-411d-8cff-ba8334a46c72
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2022/1/24 10:40:13
*描述：
*
*=====================================================================
*修改标记
*修改时间：2022/1/24 10:40:13
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Transactions;

namespace WEF.Db
{
    /// <summary>
    /// 分布式事务
    /// </summary>
    public class DTCTran : IDisposable
    {
        TransactionScope _tran;

        TransactionOptions _transactionOption = new TransactionOptions();

        /// <summary>
        /// 分布式事务
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <param name="scopeTimeout"></param>
        public DTCTran(IsolationLevel isolationLevel, TimeSpan scopeTimeout)
        {
            //设置事务隔离级别
            _transactionOption.IsolationLevel = isolationLevel;
            // 设置事务超时时间为60秒
            _transactionOption.Timeout = scopeTimeout;
            _tran = new TransactionScope(TransactionScopeOption.Required, _transactionOption);
        }

        /// <summary>
        /// 分布式事务
        /// </summary>
        public DTCTran() : this(IsolationLevel.ReadCommitted, TimeSpan.FromSeconds(60))
        {

        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            try
            {
                _tran.Complete();
            }
            finally
            {
                _tran.Dispose();
            }
        }
    }
}
