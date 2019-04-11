/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：9a4fe848-95cb-4ad2-ac1b-d757a6ea1cd0
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WEF.Expressions
 * 类名称：Where
 * 文件名：Where
 * 创建年份：2015
 * 创建时间：2015-09-29 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace WEF.Expressions
{
    /// <summary>
    /// Where条件拼接
    /// </summary>
    public class Where : WhereClipBuilder
    {

    }

    /// <summary>
    /// Where条件拼接
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Where<T> : WhereClipBuilder
        where T : Entity
    {
        /// <summary>
        /// AND
        /// </summary>
        public void And(Expression<Func<T, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            And(tempWhere);
        }
        /// <summary>
        /// AND
        /// </summary>
        public void And<T2>(Expression<Func<T, T2, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            And(tempWhere);
        }
        public void And<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            And(tempWhere);
        }
        public void And<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            And(tempWhere);
        }
        public void And<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            And(tempWhere);
        }
        public void And<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            And(tempWhere);
        }
        /// <summary>
        /// Or
        /// </summary>
        public void Or(Expression<Func<T, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            Or(tempWhere);
        }
        public void Or<T2>(Expression<Func<T, T2, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            Or(tempWhere);
        }
        public void Or<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            Or(tempWhere);
        }
        public void Or<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            Or(tempWhere);
        }
        public void Or<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            Or(tempWhere);
        }
        public void Or<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            Or(tempWhere);
        }
    }
    /// <summary>
    /// Where条件拼接，同Where类
    /// </summary>
    public class WhereClipBuilder<T> : WhereClipBuilder
        where T : Entity
    {
        /// <summary>
        /// AND
        /// </summary>
        public void And(Expression<Func<T, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            And(tempWhere);
        }
        /// <summary>
        /// Or
        /// </summary>
        public void Or(Expression<Func<T, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToClip<T>.ToWhereClip(lambdaWhere);
            Or(tempWhere);
        }
    }

    /// <summary>
    /// WhereClipBuilder
    /// </summary>
    public class WhereClipBuilder
    {
        /// <summary>
        /// 条件字符串
        /// </summary>
        private StringBuilder expressionStringBuilder = new StringBuilder();

        /// <summary>
        /// 条件参数
        /// </summary>
        private List<Parameter> parameters = new List<Parameter>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public WhereClipBuilder()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="where"></param>
        public WhereClipBuilder(WhereClip where)
        {
            expressionStringBuilder.Append(where.ToString());

            parameters.AddRange(where.Parameters);

        }
        /// <summary>
        /// AND
        /// </summary>
        /// <param name="where"></param>
        public void And(WhereClip where)
        {
            if (WhereClip.IsNullOrEmpty(where))
                return;


            if (expressionStringBuilder.Length > 0)
            {
                expressionStringBuilder.Append(" AND ");
                expressionStringBuilder.Append(where.ToString());
                expressionStringBuilder.Append(")");
                expressionStringBuilder.Insert(0, "(");
            }
            else
            {
                expressionStringBuilder.Append(where.ToString());
            }

            parameters.AddRange(where.Parameters);
        }

        /// <summary>
        /// Or
        /// </summary>
        /// <param name="where"></param>
        public void Or(WhereClip where)
        {
            if (WhereClip.IsNullOrEmpty(where))
                return;


            if (expressionStringBuilder.Length > 0)
            {
                expressionStringBuilder.Append(" OR ");
                expressionStringBuilder.Append(where.ToString());
                expressionStringBuilder.Append(")");
                expressionStringBuilder.Insert(0, "(");
            }
            else
            {
                expressionStringBuilder.Append(where.ToString());
            }


            parameters.AddRange(where.Parameters);
        }


        /// <summary>
        /// 转换成WhereClip
        /// </summary>
        /// <returns></returns>
        public WhereClip ToWhereClip()
        {
            return new WhereClip(expressionStringBuilder.ToString(), parameters.ToArray());
        }
    }
}
