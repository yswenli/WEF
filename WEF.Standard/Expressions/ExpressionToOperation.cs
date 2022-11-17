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
 * 类名称：ExpressionToOperation<T>
 * 文件名：ExpressionToOperation<T>
 * 创建年份：2015
 * 创建时间：2019-04-17 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using WEF.Common;

namespace WEF.Expressions
{
    /// <summary>
    /// 将表达式转换成操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ExpressionToOperation<T>
    {
        private static Evaluator evaluator = new Evaluator();
        private static CacheEvaluator cacheEvaluator = new CacheEvaluator();
        private static FastEvaluator fastEvaluator = new FastEvaluator();


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static WhereOperation ToJoinWhere<TEntity>(Expression<Func<T, TEntity, bool>> e)
        {
            return ToWhereOperationChild(e.Body, WhereType.JoinWhere);
        }

        public static WhereOperation ToWhereOperation(Expression<Func<T, bool>> e)
        {
            return ToWhereOperationChild(e.Body);
        }

        public static WhereOperation ToWhereOperation<T2>(Expression<Func<T, T2, bool>> e)
        {
            return ToWhereOperationChild(e.Body);
        }

        public static WhereOperation ToWhereOperation<T2, T3>(Expression<Func<T, T2, T3, bool>> e)
        {
            return ToWhereOperationChild(e.Body);
        }

        public static WhereOperation ToWhereOperation<T2, T3, T4>(Expression<Func<T, T2, T3, T4, bool>> e)
        {
            return ToWhereOperationChild(e.Body);
        }

        public static WhereOperation ToWhereOperation<T2, T3, T4, T5>(Expression<Func<T, T2, T3, T4, T5, bool>> e)
        {
            return ToWhereOperationChild(e.Body);
        }

        public static WhereOperation ToWhereOperation<T2, T3, T4, T5, T6>(Expression<Func<T, T2, T3, T4, T5, T6, bool>> e)
        {
            return ToWhereOperationChild(e.Body);
        }


        private static WhereOperation ToWhereOperationChild(System.Linq.Expressions.Expression e, WhereType wt = WhereType.Where)
        {
            if (e is BinaryExpression)
            {
                return ConvertBinary((BinaryExpression)e, wt);
            }
            if (e is MethodCallExpression)
            {
                return ConvertMethodCall((MethodCallExpression)e);
            }
            if (e is UnaryExpression)
            {
                return ConvertUnary((UnaryExpression)e);
            }
            if (IsBoolFieldOrProperty(e))
            {
                var d = (MemberExpression)e;

                return new WhereOperation(new Field(d.Member.Name), true, QueryOperator.Equal);
            }
            if (e is ConstantExpression)
            {
                var key = ((ConstantExpression)e).Value;
                if (DataUtils.ConvertValue<bool>(key))
                {
                    return new WhereOperation(" 1=1 ");
                }
                return new WhereOperation(" 1=2 ");
            }
            throw new Exception("暂时不支持的Where条件Lambda表达式写法！请使用经典写法！");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static bool IsBoolFieldOrProperty(System.Linq.Expressions.Expression e)
        {
            if (!(e is MemberExpression)) return false;
            var member = ((MemberExpression)e);
            if (member.Member.MemberType != MemberTypes.Field && member.Member.MemberType != MemberTypes.Property)
                return false;
            return member.Type == typeof(bool);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ue"></param>
        /// <param name="wtype"></param>
        /// <returns></returns>
        private static WhereOperation ConvertUnary(UnaryExpression ue, WhereType wtype = WhereType.Where)
        {
            switch (ue.NodeType)
            {
                case ExpressionType.Not:
                    return !ToWhereOperationChild(ue.Operand, wtype);
            }
            throw new Exception("暂时不支持的NodeType(" + ue.NodeType + ") lambda写法！请使用经典写法！");
        }

        private static WhereOperation ConvertBinary(BinaryExpression be, WhereType wt = WhereType.Where)
        {
            switch (be.NodeType)
            {
                case ExpressionType.Equal:
                    return LeftAndRight(be, QueryOperator.Equal, wt);
                case ExpressionType.GreaterThan:
                    return LeftAndRight(be, QueryOperator.Greater, wt);
                case ExpressionType.GreaterThanOrEqual:
                    return LeftAndRight(be, QueryOperator.GreaterOrEqual, wt);
                case ExpressionType.LessThan:
                    return LeftAndRight(be, QueryOperator.Less, wt);
                case ExpressionType.LessThanOrEqual:
                    return LeftAndRight(be, QueryOperator.LessOrEqual, wt);
                case ExpressionType.NotEqual:
                    return LeftAndRight(be, QueryOperator.NotEqual, wt);
                case ExpressionType.AndAlso:
                    return ToWhereOperationChild(be.Left, wt) && ToWhereOperationChild(be.Right, wt);
                case ExpressionType.OrElse:
                    return ToWhereOperationChild(be.Left, wt) || ToWhereOperationChild(be.Right, wt);
                default:
                    throw new Exception("暂时不支持的Where条件(" + be.NodeType + ")Lambda表达式写法！请使用经典写法！");
            }
        }

        private static WhereOperation ConvertMethodCall(MethodCallExpression mce)
        {
            var tableName = GetTableNameByExpression(mce);

            switch (mce.Method.Name)
            {
                case "StartsWith":
                    return ConvertLikeCall(tableName, mce, "", "%");
                case "EndsWith":
                    return ConvertLikeCall(tableName, mce, "%", "");
                case "Contains":
                    return ConvertLikeCall(tableName, mce, "%", "%");
                case "Like":
                    return ConvertLikeCall(tableName, mce, "%", "%", true);
                case "Equals":
                    return ConvertEqualsCall(tableName, mce);
                case "In":
                    return ConvertInCall(tableName, mce);
                case "NotIn":
                    return ConvertInCall(tableName, mce, true);
                case "IsNull":
                    return ConvertNull(tableName, mce, true);
                case "IsNotNull":
                    return ConvertNull(tableName, mce);
            }
            throw new Exception("暂时不支持的Lambda表达式方法: " + mce.Method.Name + "！请使用经典写法！");
        }


        private static WhereOperation ConvertNull(string tableName, MethodCallExpression mce, bool isNull = false)
        {
            ColumnFunction function;
            MemberExpression member;
            var key = GetMemberName(mce.Arguments[0], out function, out member);
            return isNull ? CreateField(tableName, key, member.Expression.Type).IsNull()
                : CreateField(tableName, key, member.Expression.Type).IsNotNull();
        }


        private static WhereOperation ConvertEqualsCall(string tableName, MethodCallExpression mce, bool isLike = false)
        {
            ColumnFunction function;
            MemberExpression member;
            var key = GetMemberName(mce.Object, out function, out member);
            var value = GetValue(mce.Arguments[0]);
            if (value != null)
            {
                return new WhereOperation(CreateField(tableName, key, member.Expression.Type),
                    string.Concat(value), QueryOperator.Equal);
            }
            throw new Exception("'Like'仅支持一个参数，参数应为字符串且不允许为空");
        }


        private static WhereOperation ConvertInCall(string tableName, MethodCallExpression mce, bool notIn = false)
        {
            ColumnFunction function;
            MemberExpression member;
            var key = GetMemberName(mce.Arguments[0], out function, out member);
            var list = new List<object>();
            var ie = GetValue(mce.Arguments[1]);
            if (ie is IEnumerable)
            {
                list.AddRange(((IEnumerable)GetValue(mce.Arguments[1])).Cast<object>());
            }
            else
            {
                list.Add(ie);
            }
            return notIn ? CreateField(tableName, key, member.Expression.Type).SelectNotIn(list.ToArray())
                : CreateField(tableName, key, member.Expression.Type).SelectIn(list.ToArray());
        }


        private static WhereOperation ConvertLikeCall(string tableName, MethodCallExpression mce, string left, string right, bool isLike = false)
        {
            ColumnFunction function;
            MemberExpression member;
            var key = GetMemberName(isLike ? mce.Arguments[0] : mce.Object, out function, out member);
            if (isLike ? mce.Arguments.Count == 2 : mce.Arguments.Count == 1)
            {
                var value = GetValue(isLike ? mce.Arguments[1] : mce.Arguments[0]);
                if (value != null && value is string)
                {
                    return new WhereOperation(CreateField(tableName, key, member.Expression.Type),
                        string.Concat(left, value, right), QueryOperator.Like);
                }
            }
            throw new Exception("'Like'仅支持一个参数，参数应为字符串且不允许为空");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="function"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string[] GetMemberName(System.Linq.Expressions.Expression expr, out ColumnFunction function, out MemberExpression obj)
        {
            if (expr.NodeType == ExpressionType.Convert)
            {
                expr = ((UnaryExpression)expr).Operand;
            }
            if (expr is MemberExpression)
            {
                function = ColumnFunction.None;
                obj = (MemberExpression)expr;
                return GetFieldName(obj.Member);
            }
            if (expr is MethodCallExpression)
            {
                var e = (MethodCallExpression)expr;
                if (e.Method.Name == "ToLower" && e.Object is MemberExpression)
                {
                    function = ColumnFunction.ToLower;
                    obj = (MemberExpression)e.Object;
                    return GetFieldName(obj.Member);
                }
                if (e.Method.Name == "ToUpper" && e.Object is MemberExpression)
                {
                    function = ColumnFunction.ToUpper;
                    obj = (MemberExpression)e.Object;
                    return GetFieldName(obj.Member);
                }
                throw new Exception("暂时不支持的Lambda表达式写法！请使用经典写法！");
            }
            throw new Exception("暂时不支持的Lambda表达式写法！请使用经典写法！");
        }

        /// <summary>
        /// 根据表达式获取表名
        /// </summary>
        /// <param name="expLeft"></param>
        /// <returns></returns>
        static string GetTableNameByExpression(System.Linq.Expressions.Expression expLeft)
        {
            var tableName = "";

            if (expLeft is MemberExpression)
            {
                tableName = GetTableNameByType("", ((MemberExpression)expLeft).Expression.Type);
            }
            else if (expLeft is MethodCallExpression)
            {
                var methodObj = ((MethodCallExpression)expLeft).Object;
                if (methodObj == null)
                {
                    var args = ((MethodCallExpression)expLeft).Arguments[0];
                    tableName = GetTableNameByType("", ((MemberExpression)args).Expression.Type);
                }
                else
                {
                    tableName = GetTableNameByType("", ((MemberExpression)methodObj).Expression.Type);
                }
            }
            else
            {
                tableName = GetTableNameByType("", ((expLeft as UnaryExpression).Operand as MemberExpression).Expression.Type);
            }
            return tableName;
        }

        private static WhereOperation LeftAndRight(BinaryExpression be, QueryOperator co, WhereType wtype = WhereType.Where)
        {
            ColumnFunction leftFunction;
            ColumnFunction rightFunction;
            MemberExpression leftMe = null;
            MemberExpression rightMe;
            System.Linq.Expressions.Expression expLeft = be.Left;
            System.Linq.Expressions.Expression expRight = be.Right;

            var tableName = GetTableNameByExpression(expLeft);

            if (be.Left.NodeType == ExpressionType.Convert)
            {
                expLeft = ((UnaryExpression)be.Left).Operand;
            }
            if (be.Right.NodeType == ExpressionType.Convert)
            {
                expRight = ((UnaryExpression)be.Right).Operand;
            }
            var isAgain = false;
        Again:
            if (expLeft.NodeType == ExpressionType.Constant
                || (expLeft.NodeType == ExpressionType.MemberAccess && ((MemberExpression)expLeft).Expression == null) || isAgain)
            {
                if (expRight.NodeType == ExpressionType.Constant ||
                    (expRight.NodeType == ExpressionType.MemberAccess && ((MemberExpression)expRight).Expression == null))
                {
                    return DataUtils.ConvertValue<bool>(fastEvaluator.Eval(be))
                        ? new WhereOperation(" 1=2 ")
                        : new WhereOperation(" 1=1 ");
                }
                else
                {
                    var keyRightName = GetMemberName(expRight, out rightFunction, out rightMe);

                    if (expLeft.NodeType == ExpressionType.MemberAccess)
                    {
                        var left = (MemberExpression)expLeft;
                        if (left.Expression != null && (wtype == WhereType.JoinWhere || left.Expression.ToString() == rightMe.Expression.ToString()))
                        {
                            ColumnFunction functionLeft;
                            var keyLeft = GetMemberName(expLeft, out functionLeft, out left);
                            if (keyRightName[0].Contains("$"))
                            {
                                return new WhereOperation(CreateField(tableName, keyLeft, left.Expression.Type), GetValue(expRight), co);
                            }
                            else
                            {
                                return new WhereOperation(CreateField(GetTableNameByType("", ((MemberExpression)be.Right).Expression.Type), keyRightName, rightMe.Expression.Type), CreateField(tableName, keyLeft, left.Expression.Type), co);
                            }
                        }
                    }
                    object value = GetValue(expLeft);
                    if (keyRightName[0].Contains("$"))
                    {
                        if (DataUtils.ConvertValue<bool>(fastEvaluator.Eval(be)))
                        {
                            return new WhereOperation(" 1=2 ");
                        }
                        return new WhereOperation(" 1=1 ");
                    }

                    var rigthTableName = GetTableNameByType("", ((MemberExpression)be.Right).Expression.Type);

                    if (value != null)
                    {
                        return new WhereOperation(CreateField(rigthTableName, keyRightName, rightMe.Expression.Type), value, co);
                    }

                    switch (co)
                    {
                        case QueryOperator.Equal:
                            return CreateField(rigthTableName, keyRightName, rightMe.Expression.Type).IsNull();
                        case QueryOperator.NotEqual:
                            return CreateField(rigthTableName, keyRightName, rightMe.Expression.Type).IsNotNull();
                    }
                    throw new Exception("null值只支持等于或不等于！出错比较符：" + co.ToString());
                }
            }
            else
            {
                string[] key;
                try
                {
                    key = GetMemberName(expLeft, out leftFunction, out leftMe);
                    if (key[0].Contains("$"))
                    {
                        isAgain = true;
                        goto Again;
                    }
                }
                catch (Exception)
                {
                    isAgain = true;
                    goto Again;
                }
                if (expRight.NodeType == ExpressionType.MemberAccess)
                {
                    if (!expRight.ToString().StartsWith("value"))
                    {
                        var right = (MemberExpression)expRight;

                        if (right.Expression != null && (wtype == WhereType.JoinWhere || right.Expression == leftMe.Expression))
                        {
                            var rigthTableName = GetTableNameByType("", ((MemberExpression)be.Right).Expression.Type);

                            var keyRight = GetMemberName(expRight, out ColumnFunction functionRight, out right);

                            return new WhereOperation(
                                CreateField(tableName, key, leftMe.Expression.Type),
                                CreateField(rigthTableName, keyRight, right.Expression.Type)
                                , co);
                        }
                    }
                }
                object value = GetValue(expRight);
                if (value == null)
                {
                    if (co == QueryOperator.Equal)
                    {
                        return CreateField(tableName, key, leftMe.Expression.Type).IsNull();
                    }
                    if (co == QueryOperator.NotEqual)
                    {
                        return CreateField(tableName, key, leftMe.Expression.Type).IsNotNull();
                    }
                    throw new Exception("null值只支持等于或不等于！");
                }
                return new WhereOperation(CreateField(tableName, key, leftMe.Expression.Type), value, co);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        private static object GetValue(System.Linq.Expressions.Expression right)
        {
            return fastEvaluator.Eval(right);
        }
        /// <summary>
        /// ToGroupByClip
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static GroupByOperation ToGroupByClip(Expression<Func<T, object>> expr)
        {
            return ToGroupByClipChild(expr.Body);
        }
        /// <summary>
        /// ToGroupByClipChild
        /// </summary>
        /// <param name="exprBody"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static GroupByOperation ToGroupByClipChild(System.Linq.Expressions.Expression exprBody)
        {
            if (exprBody is MemberExpression)
            {
                var e = (MemberExpression)exprBody;
                var filedProp = GetFieldName(e.Member);
                return new GroupByOperation(CreateField(string.Empty, filedProp, e.Expression.Type));
            }
            if (exprBody is NewExpression)
            {
                var exNew = (NewExpression)exprBody;
                var type = exNew.Constructor.DeclaringType;
                var list = new List<string>(exNew.Arguments.Count);
                return exNew.Arguments.Cast<MemberExpression>().Aggregate(GroupByOperation.None, (current, member)
                    => current && CreateField(string.Empty, GetFieldName(member.Member), member.Expression.Type).GroupBy);
            }
            if (exprBody is UnaryExpression)
            {
                var exNew = (UnaryExpression)exprBody;
                return ToGroupByClipChild(exNew.Operand);
            }

            throw new Exception("暂时不支持的Group by lambda写法！请使用经典写法！");
        }

        public static OrderByOperation ToOrderByClip(Expression<Func<T, object>> expr)
        {
            return ToOrderByClipChild(expr.Body, OrderByOperater.ASC);
        }

        public static OrderByOperation ToOrderByDescendingClip(string tableName, Expression<Func<T, object>> expr)
        {
            return ToOrderByClipChild(expr.Body, OrderByOperater.DESC);
        }

        private static OrderByOperation ToOrderByClipChild(System.Linq.Expressions.Expression exprBody, OrderByOperater orderBy)
        {
            if (exprBody is MemberExpression)
            {
                var e = (MemberExpression)exprBody;
                OrderByOperation gb = OrderByOperation.None;
                var filedProp = GetFieldName(e.Member);
                if (orderBy == OrderByOperater.DESC)
                {
                    gb = gb && CreateField(string.Empty, filedProp, e.Expression.Type).Desc;
                }
                else
                {
                    gb = gb && CreateField(string.Empty, filedProp, e.Expression.Type).Asc;
                }
                return gb;
            }
            if (exprBody is NewExpression)
            {
                var exNew = (NewExpression)exprBody;
                var type = exNew.Constructor.DeclaringType;
                var list = new List<string>(exNew.Arguments.Count);
                OrderByOperation gb = OrderByOperation.None;
                foreach (MemberExpression member in exNew.Arguments)
                {
                    var filedProp = GetFieldName(member.Member);
                    if (orderBy == OrderByOperater.DESC)
                    {
                        gb = gb && CreateField(string.Empty, filedProp, member.Expression.Type).Desc;
                    }
                    else
                    {
                        gb = gb && CreateField(string.Empty, filedProp, member.Expression.Type).Asc;
                    }
                }
                return gb;
            }
            if (exprBody is UnaryExpression)
            {
                var ueEx = (UnaryExpression)exprBody;
                return ToOrderByClipChild(ueEx.Operand, orderBy);
            }
            throw new Exception("暂时不支持的Order by lambda写法！请使用经典写法！");
        }


        /// <summary>
        /// ToSelect
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="exprs"></param>
        /// <returns></returns>
        public static Field[] ToSelect(string tableName, params Expression<Func<T, object>>[] exprs)
        {
            var list = new List<Field>();
            foreach (var expr in exprs)
            {
                var fields = ToSelectChild(tableName, expr.Body);
                if (fields == null || fields.Length < 1) continue;
                list.AddRange(fields);
            }
            return list.ToArray();
        }

        /// <summary>
        /// ToSelect
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Field[] ToSelect<T2>(string tableName, Expression<Func<T, T2, object>> expr)
        {
            return ToSelectChild(tableName, expr.Body);
        }

        public static Field[] ToSelect<T2, T3>(string tableName, Expression<Func<T, T2, T3, object>> expr)
        {
            return ToSelectChild(tableName, expr.Body);
        }

        public static Field[] ToSelect<T2, T3, T4>(string tableName, Expression<Func<T, T2, T3, T4, object>> expr)
        {
            return ToSelectChild(tableName, expr.Body);
        }
        /// <summary>
        /// ToSelect
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Field[] ToSelect<T2, T3, T4, T5>(string tableName, Expression<Func<T, T2, T3, T4, T5, object>> expr)
        {
            return ToSelectChild(tableName, expr.Body);
        }
        /// <summary>
        /// ToSelect
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Field[] ToSelect<T2, T3, T4, T5, T6>(string tableName, Expression<Func<T, T2, T3, T4, T5, T6, object>> expr)
        {
            return ToSelectChild(tableName, expr.Body);
        }
        /// <summary>
        /// ToSelect
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Field[] ToSelect(string tableName, Expression<Func<T, bool>> expr)
        {
            return ToSelectChild(tableName, expr.Body);
        }


        private static Field[] ToSelectChild(string tableName, System.Linq.Expressions.Expression exprBody)
        {
            if (exprBody is MemberExpression)
            {
                var e = (MemberExpression)exprBody;
                var filedProp = GetFieldName(e.Member);
                return new[] { CreateField(tableName, filedProp, e.Expression.Type) };
            }
            if (exprBody is MethodCallExpression)
            {
                var e = (MethodCallExpression)exprBody;
                switch (e.Method.Name)
                {
                    case "As":
                        return ConvertAs(tableName, e);
                    default:
                        return ConvertFun(tableName, e);
                }
                throw new Exception("暂时不支持的Select lambda写法！请使用经典写法！");
            }
            if (exprBody is NewExpression)
            {
                var exNew = (NewExpression)exprBody;
                var type = exNew.Constructor.DeclaringType;
                var list = new List<string>(exNew.Arguments.Count);
                var f = new Field[exNew.Arguments.Count];
                var i = 0;
                foreach (var item in exNew.Arguments)
                {
                    if (item is MemberExpression)
                    {
                        var propertyType = ((MemberExpression)item).Expression.Type;
                        tableName = TableAttribute.GetTableName(propertyType);
                        var aliasName = exNew.Members[i].Name;

                        var member = (MemberExpression)item;
                        var filedProp = GetFieldName(member.Member);
                        if (aliasName == "All")
                        {
                            f[i] = new Field("*", GetTableNameByType(tableName, member.Expression.Type));
                        }
                        else if ((filedProp[0] == filedProp[1] && filedProp[0] != aliasName) || (filedProp[0] != aliasName && filedProp[1] != aliasName))
                        {
                            f[i] = CreateField(tableName, filedProp, member.Expression.Type, aliasName);
                        }
                        else
                        {
                            f[i] = CreateField(tableName, filedProp, member.Expression.Type);
                        }
                    }
                    else if (item is MethodCallExpression)
                    {
                        var method = ((MethodCallExpression)item);
                        var propertyType = ((MemberExpression)method.Arguments[0]).Expression.Type;
                        tableName = TableAttribute.GetTableName(propertyType);
                        var aliasName = exNew.Members[i].Name;
                        f[i] = ConvertFun(tableName, method, aliasName)[0];
                    }
                    i++;
                }
                return f;
            }
            if (exprBody is UnaryExpression)
            {
                var expr = (UnaryExpression)exprBody;
                switch (expr.NodeType)
                {
                    case ExpressionType.Convert:
                        return ToSelectChild(tableName, expr.Operand);
                }
            }
            throw new Exception("暂时不支持的Select lambda写法！请使用经典写法！");
        }

        private static Field[] ConvertFun(string tableName, MethodCallExpression e, string aliasName = null)
        {
            ColumnFunction function;
            MemberExpression member;
            var key = GetMemberName(e.Arguments[0], out function, out member);
            Field f;
            f = string.IsNullOrWhiteSpace(aliasName)
                ? CreateField(tableName, key, member.Expression.Type)
                : CreateField(tableName, key, member.Expression.Type, aliasName);
            switch (e.Method.Name)
            {
                case "Sum":
                    return new[] { f.Sum() };
                case "Avg":
                    return new[] { f.Avg() };
                case "Len":
                    return new[] { f.Len() };
                case "Count":
                    return new[] { f.Count() };
                case "Max":
                    return new[] { f.Max() };
                case "Min":
                    return new[] { f.Min() };
                default:
                    throw new Exception("暂时不支持的Lambda表达式写法(" + e.Method.Name + ")！请使用经典写法！");
            }
        }


        private static Field[] ConvertAs(string tableName, MethodCallExpression e)
        {
            ColumnFunction function;
            MemberExpression member;
            string[] key = GetMemberName(e.Arguments[0], out function, out member);
            if (e.Arguments.Count == 2)
            {
                object value = GetValue(e.Arguments[1]);
                if (value != null && value is string)
                {
                    return new[] { CreateField(tableName, key, member.Expression.Type, value.ToString()) };
                }
            }
            throw new Exception("'As'仅支持一个参数，参数应为字符串且不允许为空");
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTableNameByType(string tableName, Type type)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(tableName))
            {
                result = tableName;
            }
            else
            {
                var tbl = CommonExpand.GetCustomAttribute<TableAttribute>(type, false);

                result = tbl != null ? tbl.GetTableName() : type.Name;
            }

            if (result.IndexOf("`") > -1)
            {
                result = result.Replace("`", "");
            }
            if (result.IndexOf("'") > -1)
            {
                result = result.Replace("'", "");
            }
            if (result.IndexOf("[") > -1)
            {
                result = result.Replace("[", "");
            }
            if (result.IndexOf("]") > -1)
            {
                result = result.Replace("]", "");
            }
            return result;
        }

        private static string[] GetFieldName(MemberInfo type)
        {
            var tbl = CustomAttributeExtensions.GetCustomAttribute<FieldAttribute>(type, false);
            return new string[] { tbl != null ? tbl.Field : type.Name, type.Name };
        }
        

        /// <summary>
        /// CreateField
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="mi"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static Field CreateField(string tableName, MemberInfo mi, Type t)
        {
            var filedProp = GetFieldName(mi);
            return new Field(filedProp[0], GetTableNameByType(tableName, t), null, null, null, filedProp[1] == filedProp[0] ? null : filedProp[1]);
        }

        /// <summary>
        /// CreateField
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filedProp"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static Field CreateField(string tableName, string[] filedProp, Type t)
        {
            if (filedProp[0] == "All")
            {
                filedProp[0] = "*";
            }

            return new Field(filedProp[0], GetTableNameByType(tableName, t));
        }

        private static Field CreateField(string tableName, string[] filedProp, Type t, string aliasName)
        {
            if (filedProp[0] == "All")
            {
                filedProp[0] = "*";
            }
            if (filedProp[0] == "All")
            {
                filedProp[0] = "*";
            }

            return new Field(filedProp[0], GetTableNameByType(tableName, t), null, null, null, aliasName);
        }
    }
}