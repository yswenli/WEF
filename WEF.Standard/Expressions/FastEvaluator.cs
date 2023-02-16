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
 * 类名称：FastEvaluator
 * 文件名：FastEvaluator
 * 创建年份：2015
 * 创建时间：2015-09-29 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WEF.Expressions
{
    /// <summary>
    /// FastEvaluator
    /// </summary>
    public class FastEvaluator : IEvaluator
    {
        private static IExpressionCache<Func<List<object>, object>> s_cache =
            new HashedListCache<Func<List<object>, object>>();

        private IExpressionCache<Func<List<object>, object>> m_cache;
        private Func<System.Linq.Expressions.Expression, Func<List<object>, object>> m_creatorDelegate;

        /// <summary>
        /// FastEvaluator
        /// </summary>
        public FastEvaluator() : this(s_cache)
        {
        }
        /// <summary>
        /// FastEvaluator
        /// </summary>
        /// <param name="cache"></param>
        public FastEvaluator(IExpressionCache<Func<List<object>, object>> cache)
        {
            this.m_cache = cache;
            this.m_creatorDelegate = (key) => new DelegateGenerator().Generate(key);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public object Eval(System.Linq.Expressions.Expression exp)
        {
            if (exp.NodeType == ExpressionType.Constant)
            {
                return ((ConstantExpression)exp).Value;
            }
            var parameters = new ConstantExtractor().Extract(exp);
            var func = this.m_cache.Get(exp, this.m_creatorDelegate);
            return func(parameters);
        }
    }
}
