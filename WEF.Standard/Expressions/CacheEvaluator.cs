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
 * 类名称：CacheEvaluator
 * 文件名：CacheEvaluator
 * 创建年份：2015
 * 创建时间：2015-09-29 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Linq.Expressions;

namespace WEF.Expressions
{
    public class CacheEvaluator : IEvaluator
    {
        private static IExpressionCache<Delegate> s_cache = new HashedListCache<Delegate>();

        private WeakTypeDelegateGenerator m_delegateGenerator = new WeakTypeDelegateGenerator();
        private ConstantExtractor m_constantExtrator = new ConstantExtractor();

        private IExpressionCache<Delegate> m_cache;
        private Func<System.Linq.Expressions.Expression, Delegate> m_creatorDelegate;

        public CacheEvaluator()
            : this(s_cache)
        { }

        public CacheEvaluator(IExpressionCache<Delegate> cache)
        {
            this.m_cache = cache;
            this.m_creatorDelegate = (key) => this.m_delegateGenerator.Generate(key);
        }

        public object Eval(System.Linq.Expressions.Expression exp)
        {
            if (exp.NodeType == ExpressionType.Constant)
            {
                return ((ConstantExpression)exp).Value;
            }

            var parameters = this.m_constantExtrator.Extract(exp);
            var func = this.m_cache.Get(exp, this.m_creatorDelegate);
            return func.DynamicInvoke(parameters.ToArray());
        }
    }
}
