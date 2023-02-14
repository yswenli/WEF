/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：9a4fe848-95cb-4ad2-ac1b-d757a6ea1cd0
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WEF.Expressions
 * 类名称：HashedListCache
 * 文件名：HashedListCache
 * 创建年份：2015
 * 创建时间：2015-09-29 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Threading;

namespace WEF.Expressions
{
    public class HashedListCache<T> : IExpressionCache<T> where T : class
    {
        private Dictionary<int, SortedList<System.Linq.Expressions.Expression, T>> m_storage =
            new Dictionary<int, SortedList<System.Linq.Expressions.Expression, T>>();
        private ReaderWriterLockSlim m_rwLock = new ReaderWriterLockSlim();

        public T Get(System.Linq.Expressions.Expression key, Func<System.Linq.Expressions.Expression, T> creator)
        {
            SortedList<System.Linq.Expressions.Expression, T> sortedList;
            T value;

            int hash = new Hasher().Hash(key);
            this.m_rwLock.EnterReadLock();
            try
            {
                if (this.m_storage.TryGetValue(hash, out sortedList) &&
                    sortedList.TryGetValue(key, out value))
                {
                    return value;
                }
            }
            finally
            {
                this.m_rwLock.ExitReadLock();
            }

            this.m_rwLock.EnterWriteLock();
            try
            {
                if (!this.m_storage.TryGetValue(hash, out sortedList))
                {
                    sortedList = new SortedList<System.Linq.Expressions.Expression, T>(new Comparer());
                    this.m_storage.Add(hash, sortedList);
                }

                if (!sortedList.TryGetValue(key, out value))
                {
                    value = creator(key);
                    sortedList.Add(key, value);
                }

                return value;
            }
            finally
            {
                this.m_rwLock.ExitWriteLock();
            }
        }

        private class Hasher : ExpressionHasher
        {
            protected override System.Linq.Expressions.Expression VisitConstant(ConstantExpression c)
            {
                return c;
            }
        }

        internal class Comparer : ExpressionComparer
        {
            protected override int CompareConstant(ConstantExpression x, ConstantExpression y)
            {
                return 0;
            }
        }
    }
}
