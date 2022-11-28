using System;
using System.Linq.Expressions;

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
    public class Where<T> : WhereBuilder<T> where T : Entity
    {
        string _tableName;

        /// <summary>
        /// Where条件拼接
        /// </summary>
        public Where()
        {
            _tableName = string.Empty;
        }

        /// <summary>
        /// Where条件拼接
        /// </summary>
        /// <param name="tableName"></param>
        public Where(string tableName) : base(tableName, null)
        {
            _tableName = tableName;
        }

        /// <summary>
        /// Where条件拼接
        /// </summary>
        /// <param name="lambdaWhere"></param>
        public Where(Expression<Func<T, bool>> lambdaWhere) : base(null, ExpressionToOperation<T>.ToWhereOperation(lambdaWhere))
        {

        }

        /// <summary>
        /// Where条件拼接
        /// </summary>
        /// <param name="where"></param>
        public Where(WhereOperation where) : base(null, where)
        {

        }

        /// <summary>
        /// AND
        /// </summary>
        public void And(Expression<Func<T, bool>> lambdaWhere)
        {
            base.And(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// AND
        /// </summary>
        public void And<T2>(Expression<Func<T, T2, bool>> lambdaWhere)
        {
            base.And(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        public void And<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere)
        {
            base.And(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        public void And<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere)
        {
            base.And(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        public void And<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere)
        {
            base.And(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        public void And<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere)
        {
            base.And(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// Or
        /// </summary>
        public void Or(Expression<Func<T, bool>> lambdaWhere)
        {
            base.Or(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        public void Or<T2>(Expression<Func<T, T2, bool>> lambdaWhere)
        {
            base.Or(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        public void Or<T2, T3>(Expression<Func<T, T2, T3, bool>> lambdaWhere)
        {
            base.Or(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        public void Or<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> lambdaWhere)
        {
            base.Or(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        public void Or<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> lambdaWhere)
        {
            base.Or(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
        public void Or<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> lambdaWhere)
        {
            base.Or(ExpressionToOperation<T>.ToWhereOperation(lambdaWhere));
        }
    }
}
