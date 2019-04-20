using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace WEF.Expressions
{
    /// <summary>
    /// Where条件拼接
    /// </summary>
    public class Where : WhereBuilder
    {

    }

    /// <summary>
    /// Where条件拼接
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Where<T> : WhereBuilder
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
        /// AND
        /// </summary>
        public void And<T2>(Expression<Func<T, T2, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            And(tempWhere);
        }
        public void And<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            And(tempWhere);
        }
        public void And<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            And(tempWhere);
        }
        public void And<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            And(tempWhere);
        }
        public void And<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere)
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
        public void Or<T2>(Expression<Func<T, T2, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            Or(tempWhere);
        }
        public void Or<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            Or(tempWhere);
        }
        public void Or<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            Or(tempWhere);
        }
        public void Or<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            Or(tempWhere);
        }
        public void Or<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere)
        {
            var tempWhere = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            Or(tempWhere);
        }
    }
}
