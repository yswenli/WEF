/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2024
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：c7fbd6e7-f64f-4258-b6e8-af04f278e907
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：WEF
 * 类名称：ObjectExtention
 * 创建时间：2017/7/26 17:20:19
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using WEF.Common;
using WEF.Db;
using WEF.Expressions;

namespace WEF
{
    /// <summary>
    /// WEF lambda相关检测扩展
    /// </summary>
    public static class WEFExtention
    {
        #region 限制对其他非WEF lambda表达式的使用方法

        private const string Tips = "该方法({0})只能用于WEF where lambda表达式！";


        /// <summary>
        /// like '%value%' 模糊查询，同Contains。
        /// </summary>
        public static bool Like(this object key, object values)
        {
            throw new Exception(string.Format(Tips, "Like"));
        }
        /// <summary>
        /// where field in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        public static bool In<T>(this object key, params T[] values)
        {
            throw new Exception(string.Format(Tips, "In"));
        }
        /// <summary>
        ///  where subquery in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool SubQueryIn<T>(this object key, params T[] values)
        {
            throw new Exception(string.Format(Tips, "In"));
        }
        /// <summary>
        /// where field in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        public static bool In<T>(this object key, List<T> values)
        {
            throw new Exception(string.Format(Tips, "In"));
        }
        /// <summary>
        /// where field not in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        public static bool NotIn<T>(this object key, params T[] values)
        {
            throw new Exception(string.Format(Tips, "NotIn"));
        }
        /// <summary>
        /// where field not in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        public static bool NotIn<T>(this object key, List<T> values)
        {
            throw new Exception(string.Format(Tips, "NotIn"));
        }
        /// <summary>
        /// IS NULL
        /// </summary>
        public static bool IsNull(this object key)
        {
            throw new Exception(string.Format(Tips, "IsNull"));
        }
        /// <summary>
        /// IS NOT NULL
        /// </summary>
        public static bool IsNotNull(this object key)
        {
            throw new Exception(string.Format(Tips, "IsNotNull"));
        }
        /// <summary>
        /// As
        /// </summary>
        public static bool As(this object key, string values)
        {
            throw new Exception(string.Format(Tips, "As"));
        }
        /// <summary>
        /// Sum
        /// </summary>
        public static decimal Sum(this object key)
        {
            throw new Exception(string.Format(Tips, "Sum"));
        }
        /// <summary>
        /// Count
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int Count(this object key)
        {
            throw new Exception(string.Format(Tips, "Count"));
        }
        /// <summary>
        /// Max
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Max<T>(this T key)
        {
            throw new Exception(string.Format(Tips, "Max"));
        }
        /// <summary>
        /// Min
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Min<T>(this T key)
        {
            throw new Exception(string.Format(Tips, "Min"));
        }

        /// <summary>
        /// Avg
        /// </summary>
        public static decimal Avg(this object key)
        {
            throw new Exception(string.Format(Tips, "Avg"));
        }
        /// <summary>
        /// Len
        /// </summary>
        public static int Len(this object key)
        {
            throw new Exception(string.Format(Tips, "Len"));
        }
        /// <summary>
        /// 子查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="search"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static WhereExpression SubQuery<T>(this object key, Search<T> search, QueryOperator queryOperator)
            where T : Entity
        {
            throw new Exception(string.Format(Tips, "SubQuery"));
        }
        /// <summary>
        /// 子查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static WhereExpression SubQueryIn<T>(this object key, Search<T> search)
           where T : Entity
        {
            throw new Exception(string.Format(Tips, "SubQuery"));
        }
        /// <summary>
        /// 子查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static WhereExpression SubQueryNotIn<T>(this object key, Search<T> search)
           where T : Entity
        {
            throw new Exception(string.Format(Tips, "SubQuery"));
        }
        /// <summary>
        /// Exists语句
        /// </summary>
        /// <param name="key"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool Exists<T>(this object key, Search<T> search)
            where T : Entity
        {
            throw new Exception(string.Format(Tips, "Exists"));
        }
        #endregion

        

        #region DataTable 与 Entity互相转换

        /// <summary>
        /// 从Entity数组转换成DataTable
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static DataTable EntitiesToDataTable<TEntity>(this IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            if (entities == null || !entities.Any()) return null;

            var first = entities.First();

            DataTable dt = new DataTable(first.GetTableName());

            Field[] fields = first.GetFields();

            int fieldLength = fields.Length;

            var vals = first.GetValues();

            for (int i = 0; i < fieldLength; i++)
            {
                if (vals[i] == null)
                    dt.Columns.Add(fields[i].Name);
                else
                    dt.Columns.Add(fields[i].Name, vals[i].GetType());
            }

            foreach (TEntity entity in entities)
            {
                DataRow dtRow = dt.NewRow();

                object[] values = entity.GetValues();

                for (int i = 0; i < fieldLength; i++)
                {
                    dtRow[fields[i].Name] = values[i];
                }
                dt.Rows.Add(dtRow);
            }
            return dt;
        }


        /// <summary>
        /// DataRow转化为T
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static TEntity DataRowToEntity<TEntity>(this DataRow row) where TEntity : Entity
        {
            TEntity local2;
            try
            {
                TEntity local = DataUtils.Create<TEntity>();
                Field[] fields = local.GetFields();
                Type type = typeof(TEntity);
                foreach (Field field in fields)
                {
                    if ((row.Table.Columns.Contains(field.Name)) && (null != row[field.Name]) && (!Convert.IsDBNull(row[field.Name])))
                    {
                        DataUtils.SetPropertyValue(type, local, field.PropertyName, row[field.Name]);
                    }
                }
                local2 = local;
            }
            catch
            {
                throw;
            }
            return local2;
        }

        /// <summary>
        /// DataTable转化为 List
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> DataTableToEntityList<TEntity>(this DataTable dt) where TEntity : Entity
        {
            List<TEntity> list = new List<TEntity>();

            if ((dt == null) || (dt.Rows.Count == 0))
                return list;

            foreach (DataRow row in dt.Rows)
            {
                list.Add(DataRowToEntity<TEntity>(row));
            }

            return list;
        }

        /// <summary>
        /// 将数据表按指定字段排序
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnNames"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static DataTable OrderBy(this DataTable dt, string columnNames, bool sort = true)
        {
            if (dt == null || dt.Rows.Count < 1) return dt;
            dt.DefaultView.Sort = $"{columnNames} {(sort ? "ASC" : "DESC")}";
            return dt.DefaultView.ToTable();
        }

        /// <summary>
        /// 填充模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="t2"></param>
        /// <param name="allowNull"></param>
        /// <param name="fileds"></param>
        /// <returns></returns>
        public static bool FillModel<T>(this T source, ref T t2, bool allowNull = false, List<string> fileds = null)
        {
            if (source == null || t2 == null) return false;

            var type = source.GetType();

            var properties = type.GetProperties();

            if (!allowNull)
            {
                if (fileds == null || fileds.Count < 1)
                {
                    foreach (var item in properties)
                    {
                        var v1 = item.GetValue(source, null);

                        if (v1 != null)
                        {
                            var p2 = properties.Where(b => b.Name == item.Name).FirstOrDefault();

                            if (p2 != null)
                            {
                                if (item.PropertyType == p2.PropertyType)
                                {
                                    p2.SetValue(t2, v1, null);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in properties)
                    {
                        var v1 = item.GetValue(source, null);

                        if (v1 != null)
                        {
                            var p2 = properties.Where(b => b.Name == item.Name && fileds.Contains(b.Name)).FirstOrDefault();

                            if (p2 != null)
                            {
                                if (item.PropertyType == p2.PropertyType)
                                {
                                    p2.SetValue(t2, v1, null);
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                if (fileds == null || fileds.Count < 1)
                {
                    foreach (var item in properties)
                    {
                        var v1 = item.GetValue(source, null);

                        var p2 = properties.Where(b => b.Name == item.Name).FirstOrDefault();

                        if (p2 != null)
                        {
                            if (item.PropertyType == p2.PropertyType)
                            {
                                p2.SetValue(t2, v1, null);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in properties)
                    {
                        var v1 = item.GetValue(source, null);

                        var p2 = properties.Where(b => b.Name == item.Name && fileds.Contains(b.Name)).FirstOrDefault();

                        if (p2 != null)
                        {
                            if (item.PropertyType == p2.PropertyType)
                            {
                                p2.SetValue(t2, v1, null);
                            }
                        }
                    }

                }

            }

            return true;
        }

        /// <summary>
        /// 填充模型
        /// </summary>
        /// <param name="source"></param>
        /// <param name="t2"></param>
        /// <param name="allowNull"></param>
        /// <returns></returns>
        public static bool FillEntity<T>(this T source, ref T t2, bool allowNull = false) where T : Entity
        {
            var result = source.FillModel(ref t2, allowNull);

            t2.ClearModifyFields();

            return result;
        }

        /// <summary>
        /// 从某个模型中取值填充当前实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        /// <param name="convertMatchType"></param>
        /// <param name="allowNull">是否填空值</param>
        public static bool FillFrom<T>(this IEntity entity, T target, ConvertMatchType convertMatchType = ConvertMatchType.IgnoreCase, bool allowNull = false) where T : class
        {
            if (entity == null || target == null) return false;
            var s = (IEntity)target.ConvertTo(entity.GetType(), convertMatchType);
            return s.FillModel(ref entity, allowNull, s.GetModifyFieldsStr());
        }

        /// <summary>
        /// 从某个模型中取值填充当前实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        /// <param name="allowNull"></param>
        /// <returns></returns>
        public static bool CopyFrom<T>(this T entity, object target, bool allowNull = false) where T : Entity
        {
            if (entity == null || target == null) return false;
            var s = SerializeHelper.Deserialize<T>(SerializeHelper.Serialize(target));
            return s.FillModel(ref entity, allowNull, s.GetModifyFieldsStr());
        }


        #endregion

        #region 获取列描述
        /// <summary>
        /// 获取字段描述
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetConlumDescriptions(Type type)
        {
            var _conlumDescriptions = new Dictionary<string, string>();
            var propertities = type.GetProperties();
            foreach (var item in propertities)
            {
                var des = item.Name;
                var attrs = item.GetCustomAttributes(true);
                if (attrs != null && attrs.Length > 0)
                {
                    foreach (var attr in attrs)
                    {
                        var da = attr as DescriptionAttribute;
                        if (da != null)
                        {
                            des = da.Description;
                            if (string.IsNullOrEmpty(des))
                            {
                                des = item.Name;
                            }
                        }
                    }
                }
                _conlumDescriptions.Add(item.Name, des);
            }
            return _conlumDescriptions;
        }

        /// <summary>
        /// 获取字段描述
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetConlumDescriptions(this IEntity entity)
        {
            return GetConlumDescriptions(entity.GetType());
        }

        /// <summary>
        /// 获取字段描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, string> GetConlumDescriptions<T>() where T : Entity
        {
            return GetConlumDescriptions(typeof(T));
        }

        #endregion


        #region 连接字符串处理

        /// <summary>
        /// 将sql连接符串转换成dictionary
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToConnectParmaDic(this string connStr)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(connStr))
            {
                var arr = connStr.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in arr)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var kv = item.Trim().Split(new string[] { "=" }, StringSplitOptions.None);

                        if (kv.Length == 2)
                            keyValuePairs.Add(kv[0].ToLower(), kv[1]);
                    }
                }
            }

            return keyValuePairs;
        }

        #endregion

        #region 特殊字符串过滤

        /// <summary>
        /// 特殊字符串过滤
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string ReplaceSQLString(this string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return String.Empty;
            sql = sql.Replace("'", "");
            sql = sql.Replace(";", "");
            sql = sql.Replace(",", "");
            sql = sql.Replace("?", "");
            sql = sql.Replace("<", "");
            sql = sql.Replace(">", "");
            sql = sql.Replace("(", "");
            sql = sql.Replace(")", "");
            sql = sql.Replace("@", "");
            sql = sql.Replace("=", "");
            sql = sql.Replace("+", "");
            sql = sql.Replace("*", "");
            sql = sql.Replace("&", "");
            sql = sql.Replace("#", "");
            sql = sql.Replace("%", "");
            sql = sql.Replace("$", "");
            //删除与数据库相关的词
            sql = Regex.Replace(sql, "select", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "insert", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "delete from", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "count", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "drop table", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "truncate", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "asc", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "mid", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "char", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "xp_cmdshell", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "exec master", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "net localgroup administrators", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "and", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "net user", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "or", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "net", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "-", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "delete", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "drop", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "script", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "update", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "and", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "chr", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "master", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "truncate", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "declare", "", RegexOptions.IgnoreCase);
            sql = Regex.Replace(sql, "mid", "", RegexOptions.IgnoreCase);
            return sql;
        }

        #endregion

        #region 常规类型转换

        /// <summary>
        /// 常规类型转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DbType GetDbType(this object value)
        {
            var typeName = value.GetType().Name;
            switch (typeName)
            {
                case "String":
                case "StringValue":
                    return DbType.String;
                case "Byte":
                    return DbType.Byte;
                case "Int16":
                    return DbType.Int16;
                case "Int32":
                    return DbType.Int32;
                case "Int64":
                    return DbType.Int64;
                case "Single":
                    return DbType.Single;
                case "Double":
                    return DbType.Double;
                case "Decimal":
                    return DbType.Decimal;
                case "DateTime":
                    return DbType.DateTime;
                case "Guid":
                    return DbType.Guid;
                case "Byte[]":
                    return DbType.Binary;
                case "Boolean":
                    return DbType.Boolean;
                default:
                    throw new NotSupportedException("不支持的类型:" + typeName);
            }
        }

        #endregion


        #region 变量参数化

        /// <summary>
        /// 将字典转换成参数列表
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public static List<DbParameter> ToParameters(this Dictionary<string, object> parameters, Database database)
        {
            if (parameters == null) return null;
            var list = new List<DbParameter>();
            foreach (var keyValuePair in parameters)
            {
                list.Add(database.CreateParameter(keyValuePair.Key, keyValuePair.Value.GetDbType(), 0, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, keyValuePair.Value));
            }
            return list;
        }
        /// <summary>
        /// 将实体转换成参数列表
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public static List<DbParameter> ToParameters<Model>(this Model parameters, Database database)
        {
            if (parameters == null) return null;
            var keyValuePairs = new Dictionary<string, object>();
            var type = typeof(Model);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var list = new List<DbParameter>();
            foreach (var property in properties)
            {
                var value = DynamicCalls.GetPropertyGetter(property).Invoke(parameters);
                list.Add(database.CreateParameter(property.Name, value.GetDbType(), 0, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, value));
            }
            return list;
        }

        #endregion
    }
}
