/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：9a4fe848-95cb-4ad2-ac1b-d757a6ea1cd0
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WEF.Expressions
 * 类名称：DelegateGenerator
 * 文件名：DelegateGenerator
 * 创建年份：2015
 * 创建时间：2015-09-29 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace WEF.Expressions
{
    public class DelegateGenerator : ExpressionVisitor
    {
        private static readonly MethodInfo s_indexerInfo = typeof(List<object>).GetMethod("get_Item");

        private int m_parameterCount;
        private ParameterExpression m_parametersExpression;

        public Func<List<object>, object> Generate(System.Linq.Expressions.Expression exp)
        {
            this.m_parameterCount = 0;
            this.m_parametersExpression =
                System.Linq.Expressions.Expression.Parameter(typeof(List<object>), "parameters");

            var body = this.Visit(exp); // normalize
            if (body.Type != typeof(object))
            {
                body = System.Linq.Expressions.Expression.Convert(body, typeof(object));
            }

            var lambda = System.Linq.Expressions.Expression.Lambda<Func<List<object>, object>>(body, this.m_parametersExpression);
            return lambda.Compile();
        }

        protected override System.Linq.Expressions.Expression VisitConstant(ConstantExpression c)
        {
            System.Linq.Expressions.Expression exp = System.Linq.Expressions.Expression.Call(
                this.m_parametersExpression,
                s_indexerInfo,
                System.Linq.Expressions.Expression.Constant(this.m_parameterCount++));
            return c.Type == typeof(object) ? exp : System.Linq.Expressions.Expression.Convert(exp, c.Type);
        }
    }
}
