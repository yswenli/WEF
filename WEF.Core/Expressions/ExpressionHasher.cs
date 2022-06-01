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
 * 类名称：ExpressionHasher
 * 文件名：ExpressionHasher
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

namespace WEF.Expressions
{
    public class ExpressionHasher : ExpressionVisitor
    {
        public int Hash(System.Linq.Expressions.Expression exp)
        {
            this.HashCode = 0;
            this.Visit(exp);
            return this.HashCode;
        }

        public int HashCode
        {
            get; protected set;
        }

        protected virtual ExpressionHasher Hash(int value)
        {
            unchecked
            {
                this.HashCode += value;
            }
            return this;
        }

        protected virtual ExpressionHasher Hash(bool value)
        {
            unchecked
            {
                this.HashCode += value ? 1 : 0;
            }
            return this;
        }

        private static readonly object s_nullValue = new object();

        protected virtual ExpressionHasher Hash(object value)
        {
            value = value ?? s_nullValue;
            unchecked
            {
                this.HashCode += value.GetHashCode();
            }
            return this;
        }

        protected override System.Linq.Expressions.Expression Visit(System.Linq.Expressions.Expression exp)
        {
            if (exp == null)
                return exp;

            this.Hash((int)exp.NodeType).Hash(exp.Type);
            return base.Visit(exp);
        }

        protected override System.Linq.Expressions.Expression VisitBinary(BinaryExpression b)
        {
            this.Hash(b.IsLifted).Hash(b.IsLiftedToNull).Hash(b.Method);
            return base.VisitBinary(b);
        }

        protected override MemberBinding VisitBinding(MemberBinding binding)
        {
            this.Hash(binding.BindingType).Hash(binding.Member);
            return base.VisitBinding(binding);
        }

        protected override System.Linq.Expressions.Expression VisitConstant(ConstantExpression c)
        {
            this.Hash(c.Value);
            return base.VisitConstant(c);
        }

        protected override ElementInit VisitElementInitializer(ElementInit initializer)
        {
            this.Hash(initializer.AddMethod);
            return base.VisitElementInitializer(initializer);
        }

        protected override System.Linq.Expressions.Expression VisitLambda(LambdaExpression lambda)
        {
            foreach (var p in lambda.Parameters)
            {
                this.VisitParameter(p);
            }

            return base.VisitLambda(lambda);
        }

        protected override System.Linq.Expressions.Expression VisitMemberAccess(MemberExpression m)
        {
            this.Hash(m.Member);
            return base.VisitMemberAccess(m);
        }

        protected override System.Linq.Expressions.Expression VisitMethodCall(MethodCallExpression m)
        {
            this.Hash(m.Method);
            return base.VisitMethodCall(m);
        }

        protected override NewExpression VisitNew(NewExpression nex)
        {
            this.Hash(nex.Constructor);
            if (nex.Members != null)
            {
                foreach (var m in nex.Members)
                    this.Hash(m);
            }

            return base.VisitNew(nex);
        }

        protected override System.Linq.Expressions.Expression VisitParameter(ParameterExpression p)
        {
            this.Hash(p.Name);
            return base.VisitParameter(p);
        }

        protected override System.Linq.Expressions.Expression VisitTypeIs(TypeBinaryExpression b)
        {
            this.Hash(b.TypeOperand);
            return base.VisitTypeIs(b);
        }

        protected override System.Linq.Expressions.Expression VisitUnary(UnaryExpression u)
        {
            this.Hash(u.IsLifted).Hash(u.IsLiftedToNull).Hash(u.Method);
            return base.VisitUnary(u);
        }
    }
}
