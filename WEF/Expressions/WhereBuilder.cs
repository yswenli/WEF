/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2019
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
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
    /// Where条件拼接，同Where类
    /// </summary>
    public class WhereBuilder<T> : WhereBuilder
        where T : Entity
    {
        /// <summary>
        /// AND
        /// </summary>
        public void And(Expression<Func<T, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            And(tempWhere);
        }
        /// <summary>
        /// Or
        /// </summary>
        public void Or(Expression<Func<T, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            Or(tempWhere);
        }
    }

    /// <summary>
    /// WhereClipBuilder
    /// </summary>
    public class WhereBuilder
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
        public WhereBuilder()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="where"></param>
        public WhereBuilder(WhereOperation where)
        {
            expressionStringBuilder.Append(where.ToString());

            parameters.AddRange(where.Parameters);

        }
        /// <summary>
        /// AND
        /// </summary>
        /// <param name="where"></param>
        public void And(WhereOperation where)
        {
            if (WhereOperation.IsNullOrEmpty(where))
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
        public void Or(WhereOperation where)
        {
            if (WhereOperation.IsNullOrEmpty(where))
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
        public WhereOperation ToWhereClip()
        {
            return new WhereOperation(expressionStringBuilder.ToString(), parameters.ToArray());
        }
    }
}
