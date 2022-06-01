/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;

using WEF.Expressions;

namespace WEF.Common
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public static class DataUtils
    {
        /// <summary>
        /// 格式化sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="leftToken"></param>
        /// <param name="rightToken"></param>
        /// <returns></returns>
        internal static string FormatSQL(string sql, char leftToken, char rightToken)
        {
            if (sql == null)
            {
                return string.Empty;
            }

            sql = sql.Replace("{0}", leftToken.ToString()).Replace("{1}", rightToken.ToString());

            return sql;
        }

        /// <summary>
        /// 格式化数据为数据库通用格式
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string FormatValue(object val)
        {
            if (val == null || val == DBNull.Value)
            {
                return "null";
            }

            Type type = val.GetType();

            if (type == typeof(DateTime) || type == typeof(Guid))
            {
                return string.Format("'{0}'", val);
            }
            else if (type == typeof(TimeSpan))
            {
                DateTime baseTime = new DateTime(1900, 01, 01);
                return string.Format("(cast('{0}' as datetime) - cast('{1}' as datetime))", baseTime + ((TimeSpan)val), baseTime);
            }
            else if (type == typeof(bool))
            {
                return ((bool)val) ? "1" : "0";
            }
            else if (val is Field)
            {
                return ((Field)val).TableFieldName;
            }
            else if (type.IsEnum)
            {
                return Convert.ToInt32(val).ToString();
            }
            else if (type.IsValueType)
            {
                return val.ToString();
            }
            else
            {
                return string.Concat("N'", val.ToString().Replace("'", "''"), "'");
            }
        }

        /// <summary>
        /// CheckStuct
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool CheckStruct(Type type)
        {
            return ((type.IsValueType && !type.IsEnum) && (!type.IsPrimitive && !type.IsSerializable));
        }

        /// <summary>
        /// DeepClone
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object Clone(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0L;
                return formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ConvertValue(Type type, object value)
        {
            if (Convert.IsDBNull(value) || (value == null))
            {
                return null;
            }
            if (CheckStruct(type))
            {
                string data = value.ToString();
                return SerializationManager.Deserialize(type, data);
            }
            Type type2 = value.GetType();
            if (type == type2)
            {
                return value;
            }
            if (((type == typeof(Guid)) || (type == typeof(Guid?))) && (type2 == typeof(string)))
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return Guid.Empty;
                }
                return new Guid(value.ToString());
            }
            if (((type == typeof(DateTime)) || (type == typeof(DateTime?))) && (type2 == typeof(string)))
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return null;
                }
                return Convert.ToDateTime(value);
            }
            if (type.IsEnum)
            {
                try
                {
                    return Enum.Parse(type, value.ToString(), true);
                }
                catch
                {
                    return Enum.ToObject(type, value);
                }
            }
            if (((type == typeof(bool)) || (type == typeof(bool?))))
            {
                bool tempbool = false;
                if (bool.TryParse(value.ToString(), out tempbool))
                {
                    return tempbool;
                }
                else
                {
                    //处理  Request.Form  的 checkbox  如果没有返回值就是没有选中false  
                    if (string.IsNullOrEmpty(value.ToString()))
                        return false;
                    else
                    {
                        if (value.ToString() == "0")
                        {
                            return false;
                        }
                        return true;
                    }
                }

            }

            if (type.IsGenericType)
            {
                type = type.GetGenericArguments()[0];
            }

            return Convert.ChangeType(value, type);
        }

        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TResult ConvertValue<TResult>(object value)
        {
            if (Convert.IsDBNull(value) || value == null)
                return default(TResult);

            object obj = ConvertValue(typeof(TResult), value);
            if (obj == null)
            {
                return default(TResult);
            }
            return (TResult)obj;
        }


        /// <summary>
        /// 快速执行Method
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object FastMethodInvoke(object obj, MethodInfo method, params object[] parameters)
        {
            return DynamicCalls.GetMethodInvoker(method)(obj, parameters);
        }

        /// <summary>
        /// 快速实例化一个T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>()
        {
            return (T)Create(typeof(T))();
        }

        /// <summary>
        /// 快速实例化一个FastCreateInstanceHandler
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FastCreateInstanceHandler Create(Type type)
        {
            return DynamicCalls.GetInstanceCreator(type);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object obj, PropertyInfo property, object value)
        {
            if (property.CanWrite)
            {
                FastPropertySetHandler propertySetter = DynamicCalls.GetPropertySetter(property);
                value = ConvertValue(property.PropertyType, value);
                propertySetter(obj, value);
            }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object obj, PropertyInfo property)
        {
            if (property.CanRead)
            {
                FastPropertyGetHandler propertyGetter = DynamicCalls.GetPropertyGetter(property);

                return propertyGetter(obj);
            }
            return null;
        }
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            SetPropertyValue(obj.GetType(), obj, propertyName, value);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(Type type, object obj, string propertyName, object value)
        {
            var properties = type.GetProperties();

            var property = properties.Where(b => b.Name.ToLower() == propertyName.ToLower()).FirstOrDefault();

            if (property != null)
            {
                SetPropertyValue(obj, property, value);
            }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue<TEntity>(TEntity entity, string propertyName)
        {
            var properties = entity.GetType().GetProperties();

            var property = properties.Where(b => b.Name.ToLower() == propertyName.ToLower()).FirstOrDefault();

            if (property != null)
            {
                return GetPropertyValue(entity, property);
            }

            return null;


        }

        private static System.Text.RegularExpressions.Regex keyReg = new System.Text.RegularExpressions.Regex("[^a-zA-Z]", System.Text.RegularExpressions.RegexOptions.Compiled);
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private static byte[] bt = new byte[5];
        private static Random rd = new Random();
        private static object obj = new object();
        private static readonly ThreadLocal<Random> appRandom = new ThreadLocal<Random>(() => new Random());

        public static int GetRandomNumber()
        {
            return appRandom.Value.Next();
        }

        public static long paramCount = 0;

        /// <summary>
        /// 生成下一个参数
        /// </summary>
        /// <returns></returns>
        public static long GetNewParamCount()
        {
            if (paramCount >= long.MaxValue)
            {
                Interlocked.Exchange(ref paramCount, 0);
            }

            Interlocked.Increment(ref paramCount);

            return paramCount;
        }

        /// <summary>
        /// 生成唯一字符串
        /// </summary>
        /// <returns></returns>
        public static string MakeUniqueKey(Field field)//string prefix,
        {
            return string.Concat("@", field.Name, GetNewParamCount());
        }


        /// <summary>
        /// 生成主键条件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static WhereOperation GetPrimaryKeyWhere(Entity entity)
        {
            WhereOperation where = new WhereOperation();
            var keyfields = entity.GetPrimaryKeyFields();
            var allfields = entity.GetFields();
            var allValues = entity.GetValues();
            var fieldlength = allfields.Length;
            if (keyfields == null) return where;
            foreach (var pkField in keyfields)
            {
                for (int i = 0; i < fieldlength; i++)
                {
                    if (string.Compare(allfields[i].PropertyName, pkField.PropertyName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        where = where.And(new WhereOperation(pkField, allValues[i], QueryOperator.Equal));
                        break;
                    }
                }

            }
            return where;
        }

        /// <summary>
        /// 生成主键条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        internal static WhereOperation GetPrimaryKeyWhere<TEntity>() where TEntity: Entity
        {
            WhereOperation where = new WhereOperation();
            var keyfields = EntityCache.GetPrimaryKeyFields<TEntity>();
            var allfields = EntityCache.GetFields<TEntity>();
            var allValues = EntityCache.GetValues<TEntity>();
            var fieldlength = allfields.Length;
            if (keyfields == null) return where;
            foreach (var pkField in keyfields)
            {
                for (int i = 0; i < fieldlength; i++)
                {
                    if (string.Compare(allfields[i].PropertyName, pkField.PropertyName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        where = where.And(new WhereOperation(pkField, allValues[i], QueryOperator.Equal));
                        break;
                    }
                }
            }
            return where;
        }

        /// <summary>
        /// 生成主键条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        internal static WhereOperation GetPrimaryKeyWhere<TEntity>(Array pkValues)//params object[] pkValues  
            where TEntity : Entity
        {
            WhereOperation where = new WhereOperation();
            Field[] keyfields = EntityCache.GetPrimaryKeyFields<TEntity>();

            if (keyfields == null)
                return where;

            Check.Require(keyfields.Length == pkValues.Length, "主键列与主键值无法对应!");

            int index = keyfields.Length;
            for (int i = 0; i < index; i++)
            {
                where = where.And(new WhereOperation(keyfields[i], pkValues.GetValue(i), QueryOperator.Equal));
            }
            return where;
        }

        /// <summary>
        /// 数组转成字典
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Dictionary<Field, object> FieldValueToDictionary(Field[] fields, object[] values)
        {
            Dictionary<Field, object> dic = new Dictionary<Field, object>();
            if (null == fields || fields.Length == 0)
                return dic;
            int length = fields.Length;

            for (int i = 0; i < length; i++)
            {
                dic.Add(fields[i], values[i]);
            }

            return dic;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string ToString(QueryOperator op)
        {
            switch (op)
            {
                case QueryOperator.Add:
                    return " + ";
                case QueryOperator.BitwiseAND:
                    return " & ";
                case QueryOperator.BitwiseNOT:
                    return " ~ ";
                case QueryOperator.BitwiseOR:
                    return " | ";
                case QueryOperator.BitwiseXOR:
                    return " ^ ";
                case QueryOperator.Divide:
                    return " / ";
                case QueryOperator.Equal:
                    return " = ";
                case QueryOperator.Greater:
                    return " > ";
                case QueryOperator.GreaterOrEqual:
                    return " >= ";
                case QueryOperator.IsNULL:
                    return " IS NULL ";
                case QueryOperator.IsNotNULL:
                    return " IS NOT NULL ";
                case QueryOperator.Less:
                    return " < ";
                case QueryOperator.LessOrEqual:
                    return " <= ";
                case QueryOperator.Like:
                    return " LIKE ";
                case QueryOperator.Modulo:
                    return " % ";
                case QueryOperator.Multiply:
                    return " * ";
                case QueryOperator.NotEqual:
                    return " <> ";
                case QueryOperator.Subtract:
                    return " - ";
            }

            throw new NotSupportedException("Unknown QueryOperator: " + op.ToString() + "!");
        }


        public static int GetEndIndexOfMethod(string cmdText, int startIndexOfCharIndex)
        {
            int foundEnd = -1;
            int endIndexOfCharIndex = 0;
            for (int i = startIndexOfCharIndex; i < cmdText.Length; ++i)
            {
                if (cmdText[i] == '(')
                {
                    --foundEnd;
                }
                else if (cmdText[i] == ')')
                {
                    ++foundEnd;
                }

                if (foundEnd == 0)
                {
                    endIndexOfCharIndex = i;
                    break;
                }
            }
            return endIndexOfCharIndex;
        }

        public static string[] SplitTwoParamsOfMethodBody(string bodyText)
        {
            int colonIndex = 0;
            int foundEnd = 0;
            for (int i = 1; i < bodyText.Length - 1; i++)
            {
                if (bodyText[i] == '(')
                {
                    --foundEnd;
                }
                else if (bodyText[i] == ')')
                {
                    ++foundEnd;
                }

                if (bodyText[i] == ',' && foundEnd == 0)
                {
                    colonIndex = i;
                    break;
                }
            }

            return new string[] { bodyText.Substring(0, colonIndex), bodyText.Substring(colonIndex + 1) };
        }


        private static T ConvertObj<T>(dynamic obj)
        {
            return (T)obj;
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public static char ReadChar(object value)
        {
            if (value == null || value is DBNull) throw new ArgumentNullException("value");
            string s = value as string;
            if (s == null || s.Length != 1) throw new ArgumentException("A single-character was expected", "value");
            return s[0];
        }
        /// <summary>
        /// Internal use only
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public static char? ReadNullableChar(object value)
        {
            if (value == null || value is DBNull) return null;
            string s = value as string;
            if (s == null || s.Length != 1) throw new ArgumentException("A single-character was expected", "value");
            return s[0];
        }
        /// <summary>
        /// Throws a data exception, only used internally
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="index"></param>
        /// <param name="reader"></param>
        public static void ThrowDataException(Exception ex, int index, IDataReader reader)
        {
            string name = "(n/a)", value = "(n/a)";
            if (reader != null && index >= 0 && index < reader.FieldCount)
            {
                name = reader.GetName(index);
                object val = reader.GetValue(index);
                if (val == null || val is DBNull)
                {
                    value = "<null>";
                }
                else
                {
                    value = Convert.ToString(val) + " - " + Type.GetTypeCode(val.GetType());
                }
            }
            if (!(index >= reader.FieldCount))
            {
                throw new DataException(string.Format("Error parsing column {0} ({1}={2})", index, name, value), ex);
            }
        }

        public static string ReplaceSqlKey(string text, int maxlength)
        {
            text = text.ToLower().Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxlength)
                text = text.Substring(0, maxlength);

            text = Regex.Replace(text, "'", "");
            text = Regex.Replace(text, "\r\n", "");
            text = Regex.Replace(text, ";", "");
            return text;
        }
    }
}
