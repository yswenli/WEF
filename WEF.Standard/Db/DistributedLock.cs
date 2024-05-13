/****************************************************************************
*Copyright (c) 2024 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：河之洲
*命名空间：WEF.Db
*文件名： DistributedLock
*版本号： V1.0.0.0
*唯一标识：beeb1cd5-be73-4182-a5c8-53c0bfaccb16
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2024/5/13 11:36:13
*描述：
*
*=================================================
*修改标记
*修改时间：2024/5/13 11:36:13
*修改人： yswenli
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace WEF.Db
{
    /// <summary>
    /// 基于DistributedLock表的单库分布式锁
    /// </summary>
    public class DistributedLock : IDisposable
    {
        private DBContext _context;
        private readonly string _lockKey;
        private int _lockTimeout;
        private DbTransaction _transaction;

        /// <summary>
        /// 数据库分布式锁
        /// </summary>
        /// <param name="context"></param>
        /// <param name="lockKeyName"></param>
        /// <param name="lockTimeout"></param>
        public DistributedLock(DBContext context, string lockKeyName, int lockTimeout = 180)
        {
            _context = context;
            _lockKey = lockKeyName;
            _lockTimeout = lockTimeout;
        }

        /// <summary>
        /// 请求锁
        /// </summary>
        /// <returns></returns>
        public bool AcquireLock()
        {
            try
            {

                var paramDic = new Dictionary<string, object>
                {
                    { "@LockKey", _lockKey }
                };

                using (var reader = _context.ExecuteReader("SELECT * FROM DistributedLock WHERE LockKey = @LockKey",  paramDic))
                {
                    if (!reader.Read())
                    {
                        reader.Close();
                        // 锁未被占用，尝试插入新记录
                        _transaction = _context.Db.BeginTransaction();
                        var result = _context.ExecuteNonQuery("INSERT INTO DistributedLock (LockKey, AcquiredDate) VALUES (@LockKey, GETDATE())", _transaction, paramDic);
                        if (result > 0)
                        {
                            // 锁获取成功，提交事务
                            _transaction.Commit();
                            return true;
                        }
                    }
                    else
                    {
                        if (reader.Read())
                        {
                            //超过180秒的自动解锁
                            if (DateTime.TryParse(reader[1]?.ToString() ?? "", out DateTime dt) && DateTime.Now > dt.AddSeconds(_lockTimeout))
                            {
                                reader.Close();
                                ReleaseLock();
                                return false;
                            }
                        }
                    }
                }
            }
            catch
            {
                _transaction.Rollback();
            }

            return false;
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        public void ReleaseLock()
        {
            try
            {
                _context.ExecuteNonQuery("DELETE DistributedLock WHERE LockKey = @LockKey", _transaction, new Dictionary<string, object> { { "@LockKey", _lockKey } });
                _transaction.Commit();
            }
            catch { }
        }


        /// <summary>
        /// 释放锁
        /// </summary>
        public void Dispose()
        {
            ReleaseLock();
        }
    }
}
