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

using WEF.Common;

namespace WEF.Expressions
{

    /// <summary>
    /// Where条件拼接，同Where类
    /// </summary>
    public class WhereBuilder<T> : WhereBuilder
        where T : Entity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WhereBuilder() : base()
        {

        }

        /// <summary>
        /// WhereBuilder
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        public WhereBuilder(string tableName, WhereExpression where) : base(tableName, where)
        {

        }

        /// <summary>
        /// AND
        /// </summary>
        public void And(Expression<Func<T, bool>> lambdaWhere)
        {
            And(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// Or
        /// </summary>
        public void Or(Expression<Func<T, bool>> lambdaWhere)
        {
            Or(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }

        /// <summary>
        /// Where条件拼接，同Where类
        /// </summary>
        /// <param name="lambdaWhere"></param>
        public static implicit operator WhereBuilder<T>(Expression<Func<T, bool>> lambdaWhere)
        {
            return ExpressionToOperation<T>.ToWhereOperation(lambdaWhere) as WhereBuilder<T>;
        }
    }

    /// <summary>
    /// WhereBuilder
    /// </summary>
    public class WhereBuilder
    {
        /// <summary>
        /// 条件字符串
        /// </summary>
        protected StringPlus _expressionStringPlus;

        /// <summary>
        /// 条件参数
        /// </summary>
        protected List<Parameter> _parameters;

        /// <summary>
        /// 表名
        /// </summary>
        protected string _tablename;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WhereBuilder()
        {
            _expressionStringPlus = new StringPlus();

            _parameters = new List<Parameter>();
        }

        /// <summary>
        /// WhereBuilder
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        public WhereBuilder(string tableName, WhereExpression where) : this()
        {
            _tablename = tableName;

            if (where != null)
            {
                _expressionStringPlus.Append(where.ToString());

                _parameters.AddRange(where.Parameters);
            }
        }
        /// <summary>
        /// AND
        /// </summary>
        /// <param name="where"></param>
        public void And(WhereExpression where)
        {
            if (WhereExpression.IsNullOrEmpty(where))
                return;

            if (_expressionStringPlus.Length > 0)
            {
                _expressionStringPlus.Append(" AND ");
                _expressionStringPlus.Append(where.ToString());
                _expressionStringPlus.Append(")");
                _expressionStringPlus.Insert(0, "(");
            }
            else
            {
                _expressionStringPlus.Append(where.ToString());
            }

            _parameters.AddRange(where.Parameters);
        }

        /// <summary>
        /// Or
        /// </summary>
        /// <param name="where"></param>
        public void Or(WhereExpression where)
        {
            if (WhereExpression.IsNullOrEmpty(where))
                return;

            if (_expressionStringPlus.Length > 0)
            {
                _expressionStringPlus.Append(" OR ");
                _expressionStringPlus.Append(where.ToString());
                _expressionStringPlus.Append(")");
                _expressionStringPlus.Insert(0, "(");
            }
            else
            {
                _expressionStringPlus.Append(where.ToString());
            }

            _parameters.AddRange(where.Parameters);
        }

        /// <summary>
        /// and
        /// </summary>
        /// <param name="builder"></param>
        public void And(WhereBuilder builder)
        {
            And(builder.ToWhereClip());
        }

        /// <summary>
        /// or
        /// </summary>
        /// <param name="builder"></param>
        public void Or(WhereBuilder builder)
        {
            Or(builder.ToWhereClip());
        }

        /// <summary>
        /// 转换成WhereClip
        /// </summary>
        /// <returns></returns>
        public WhereExpression ToWhereClip()
        {
            return new WhereExpression(_expressionStringPlus.ToString(), _parameters.ToArray());
        }
    }
}
