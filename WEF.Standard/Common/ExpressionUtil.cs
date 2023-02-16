/****************************************************************************
*Copyright (c) 2023 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Common
*文件名： ExpressionUtil
*版本号： V1.0.0.0
*唯一标识：0ca2a4ea-473d-4c63-afa9-5c6582202c55
*当前的用户域：WALLE
*创建人： WALLE
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2023/2/16 14:17:25
*描述：表达式工具类
*
*=================================================
*修改标记
*修改时间：2023/2/16 14:17:25
*修改人： yswen
*版本号： V1.0.0.0
*描述：表达式工具类
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using WEF.Db;
using WEF.Expressions;

namespace WEF.Common
{
    /// <summary>
    /// 表达式工具类
    /// </summary>
    public static class ExpressionUtil
    {

        private static FastEvaluator _fastEvaluator = new FastEvaluator();

        /// <summary>
        /// 获取字段和值的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static Dictionary<Field, object> GetFieldVals<T>(this Expression<Func<T, object>> expr, string tableName = "") where T : Entity
        {
            return expr.Body.GetFieldVals<T>(tableName);
        }
        /// <summary>
        /// 获取字段和值的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exprBody"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Dictionary<Field, object> GetFieldVals<T>(this System.Linq.Expressions.Expression exprBody, string tableName = "") where T : Entity
        {
            Dictionary<Field, object> map = new Dictionary<Field, object>();

            if (string.IsNullOrEmpty(tableName)) tableName = EntityCache.GetTableName<T>();

            if (exprBody is NewExpression)
            {
                var exNew = (NewExpression)exprBody;
                var i = 0;
                foreach (var item in exNew.Arguments)
                {
                    Field f;
                    object v;

                    if (item is MemberExpression)
                    {
                        var propertyType = ((MemberExpression)item).Expression.Type;
                        var aliasName = exNew.Members[i].Name;

                        var member = (MemberExpression)item;
                        var filedProp = GetFieldName(member.Member);
                        if (aliasName == "All")
                        {
                            f = new Field("*", GetTableNameByType(tableName, member.Expression.Type));
                        }
                        else if ((filedProp[0] == filedProp[1] && filedProp[0] != aliasName) || (filedProp[0] != aliasName && filedProp[1] != aliasName))
                        {
                            f = CreateField(tableName, filedProp, member.Expression.Type, aliasName);
                        }
                        else
                        {
                            f = CreateField(tableName, filedProp, member.Expression.Type);
                        }
                        v = GetValue(member.Expression);
                    }
                    else if (item is MethodCallExpression)
                    {
                        var method = ((MethodCallExpression)item);
                        var exp = ((MemberExpression)method.Arguments[0]).Expression;
                        var propertyType = exp.Type;
                        var aliasName = exNew.Members[i].Name;
                        f = ConvertFun(tableName, method, aliasName)[0];
                        v = GetValue(exp);
                    }
                    else
                    {
                        i++;
                        continue;
                    }
                    i++;
                    map.Add(f, v);
                }
            }
            else
            {
                throw new Exception("未支持类型的lambada表达式");
            }
            return map;
        }


        #region private

        static Field[] ConvertAs(string tableName, MethodCallExpression e)
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

        static string[] GetMemberName(System.Linq.Expressions.Expression expr, out ColumnFunction function, out MemberExpression obj)
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

        static string[] GetFieldName(MemberInfo type)
        {
            var tbl = CustomAttributeExtensions.GetCustomAttribute<FieldAttribute>(type, false);
            return new string[] { tbl != null ? tbl.Field : type.Name, type.Name };
        }

        static Field CreateField(string tableName, MemberInfo mi, Type t)
        {
            var filedProp = GetFieldName(mi);
            return new Field(filedProp[0], GetTableNameByType(tableName, t), null, null, null, filedProp[1] == filedProp[0] ? null : filedProp[1]);
        }

        static Field CreateField(string tableName, string[] filedProp, Type t)
        {
            if (filedProp[0] == "All")
            {
                filedProp[0] = "*";
            }
            return new Field(filedProp[0], GetTableNameByType(tableName, t));
        }

        static Field CreateField(string tableName, string[] filedProp, Type t, string aliasName)
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

        static string GetTableNameByType(string tableName, Type type)
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

        static object GetValue(System.Linq.Expressions.Expression right)
        {
            return _fastEvaluator.Eval(right);
        }

        static Field[] ConvertFun(string tableName, MethodCallExpression e, string aliasName = null)
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

        #endregion
    }
}
