/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2019
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
using System.Collections.Generic;
using System.Linq;

namespace WEF
{
    /// <summary>
    /// WEF lambda相关检测扩展
    /// </summary>
    public static class WEFExtention
    {
        /// <summary>
        /// 
        /// </summary>
        private const string Tips = "该方法({0})只能用于WEF lambda表达式！";


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
        ///     将C#数据实体转化为JSON数据
        /// </summary>
        /// <param name="obj">要转化的数据实体</param>
        /// <returns>JSON格式字符串</returns>
        public static string JsonSerialize(object obj)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            return s.Serialize(obj);
        }

        /// <summary>
        ///     将JSON数据转化为C#数据实体
        /// </summary>
        /// <param name="json">符合JSON格式的字符串</param>
        /// <returns>T类型的对象</returns>
        public static T JsonDeserialize<T>(string json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer d = new System.Web.Script.Serialization.JavaScriptSerializer();
            return d.Deserialize<T>(json);
        }

        /// <summary>
        /// 转换成另外一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object source) where T : class
        {
            if (source != null && source.GetType().IsClass)
            {
                var sourceProperties = source.GetType().GetProperties();

                var type = typeof(T);

                var target = (T)Activator.CreateInstance(type);

                var targetProperties = type.GetProperties();

                foreach (var targetProperty in targetProperties)
                {

                    var sourceProperty = sourceProperties.Where(b => b.Name.ToLower() == targetProperty.Name.ToLower()).FirstOrDefault();

                    if (sourceProperty != null)
                    {
                        try
                        {
                            var val = sourceProperty.GetValue(source, null);

                            if (sourceProperty.PropertyType == targetProperty.PropertyType)
                            {
                                if (val != null)
                                {
                                    targetProperty.SetValue(target, val, null);
                                }
                            }
                            else
                            {
                                //不同类型转换

                                #region 日期

                                if (sourceProperty.PropertyType == typeof(DateTime))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        if (val != null)
                                        {
                                            var dt = (DateTime)val;
                                            targetProperty.SetValue(target, dt.ToString("yyyy-MM-dd HH:mm:ss"), null);
                                        }
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<DateTime>))
                                    {
                                        if (val != null)
                                        {
                                            targetProperty.SetValue(target, val, null);
                                        }
                                    }
                                }
                                else if (sourceProperty.PropertyType == typeof(Nullable<DateTime>))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        if (val != null)
                                        {
                                            var dt = (Nullable<DateTime>)val;
                                            if (dt.HasValue)
                                            {
                                                targetProperty.SetValue(target, dt.Value.ToString("yyyy-MM-dd HH:mm:ss"), null);
                                            }
                                        }
                                    }
                                    else if (targetProperty.PropertyType == typeof(DateTime))
                                    {
                                        if (val != null)
                                        {
                                            var dt = (Nullable<DateTime>)val;
                                            if (dt.HasValue)
                                            {
                                                targetProperty.SetValue(target, dt.Value, null);
                                            }
                                        }
                                    }
                                }

                                #endregion

                                #region 字符串

                                if (sourceProperty.PropertyType == typeof(string))
                                {
                                    var str = (string)val;

                                    if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        var num = 0;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            int.TryParse(str, out num);
                                        }

                                        targetProperty.SetValue(target, num, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                    {
                                        long num = 0;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            long.TryParse(str, out num);
                                        }

                                        targetProperty.SetValue(target, num, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        short num = 0;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            short.TryParse(str, out num);
                                        }

                                        targetProperty.SetValue(target, num, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        byte num = 0;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            byte.TryParse(str, out num);
                                        }

                                        targetProperty.SetValue(target, num, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        float num = 0;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            float.TryParse(str, out num);
                                        }

                                        targetProperty.SetValue(target, num, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        double num = 0;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            double.TryParse(str, out num);
                                        }

                                        targetProperty.SetValue(target, num, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        decimal num = 0;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            decimal.TryParse(str, out num);
                                        }

                                        targetProperty.SetValue(target, num, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(bool) || targetProperty.PropertyType == typeof(Nullable<bool>))
                                    {
                                        var bVal = false;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            bool.TryParse(str, out bVal);
                                        }

                                        if (str != "0" && str != "false")
                                        {
                                            bVal = true;
                                        }

                                        targetProperty.SetValue(target, bVal, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(DateTime) || targetProperty.PropertyType == typeof(Nullable<DateTime>))
                                    {
                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            if(DateTime.TryParse(str, out DateTime dt))
                                            {
                                                targetProperty.SetValue(target, dt, null);
                                            }
                                        }
                                    }
                                }

                                #endregion

                                #region 数字

                                if (sourceProperty.PropertyType == typeof(byte))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        targetProperty.SetValue(target, val.ToString(), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        targetProperty.SetValue(target, val, null);
                                    }
                                    else if (targetProperty.PropertyType.IsEnum)
                                    {
                                        var eVal = Enum.Parse(targetProperty.PropertyType, val.ToString());

                                        targetProperty.SetValue(target, eVal, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(bool) || targetProperty.PropertyType == typeof(Nullable<bool>))
                                    {
                                        targetProperty.SetValue(target, (Convert.ToByte(val)) != 0, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToInt16(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToInt32(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToDecimal(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToSingle(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToDouble(val), null);
                                    }
                                }
                                else if (sourceProperty.PropertyType == typeof(Nullable<byte>))
                                {
                                    if (val != null)
                                    {
                                        var nVal = (Nullable<byte>)val;

                                        if (nVal.HasValue)
                                        {
                                            if (targetProperty.PropertyType == typeof(string))
                                            {
                                                targetProperty.SetValue(target, nVal.Value.ToString(), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(byte))
                                            {
                                                targetProperty.SetValue(target, nVal, null);
                                            }
                                            else if (targetProperty.PropertyType.IsEnum)
                                            {
                                                var eVal = Enum.Parse(targetProperty.PropertyType, nVal.Value.ToString());

                                                targetProperty.SetValue(target, eVal, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(bool) || targetProperty.PropertyType == typeof(Nullable<bool>))
                                            {
                                                targetProperty.SetValue(target, (Convert.ToByte(nVal.Value)) != 0, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToInt16(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToInt32(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToDecimal(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                            {
                                                targetProperty.SetValue(target, (long)nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                            {
                                                targetProperty.SetValue(target, (float)nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToDouble(nVal.Value), null);
                                            }
                                        }
                                    }
                                }

                                if (sourceProperty.PropertyType == typeof(short))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        targetProperty.SetValue(target, val.ToString(), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        targetProperty.SetValue(target, val, null);
                                    }
                                    else if (targetProperty.PropertyType.IsEnum)
                                    {
                                        var eVal = Enum.Parse(targetProperty.PropertyType, val.ToString());

                                        targetProperty.SetValue(target, eVal, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(bool) || targetProperty.PropertyType == typeof(Nullable<bool>))
                                    {
                                        targetProperty.SetValue(target, (Convert.ToInt16(val)) != 0, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToByte(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToInt32(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToDecimal(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToSingle(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToDouble(val), null);
                                    }
                                }
                                else if (sourceProperty.PropertyType == typeof(Nullable<short>))
                                {
                                    if (val != null)
                                    {
                                        var nVal = (Nullable<short>)val;

                                        if (nVal.HasValue)
                                        {
                                            if (targetProperty.PropertyType == typeof(string))
                                            {
                                                targetProperty.SetValue(target, nVal.Value.ToString(), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(short))
                                            {
                                                targetProperty.SetValue(target, nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType.IsEnum)
                                            {
                                                var eVal = Enum.Parse(targetProperty.PropertyType, nVal.Value.ToString());

                                                targetProperty.SetValue(target, eVal, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(bool) || targetProperty.PropertyType == typeof(Nullable<bool>))
                                            {
                                                targetProperty.SetValue(target, (Convert.ToInt16(nVal.Value)) != 0, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToByte(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToInt32(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToDecimal(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                            {
                                                targetProperty.SetValue(target, (long)nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                            {
                                                targetProperty.SetValue(target, (float)nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToDouble(nVal.Value), null);
                                            }
                                        }
                                    }
                                }

                                if (sourceProperty.PropertyType == typeof(int))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        targetProperty.SetValue(target, val.ToString(), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        targetProperty.SetValue(target, val, null);
                                    }
                                    else if (targetProperty.PropertyType.IsEnum)
                                    {
                                        var eVal = Enum.Parse(targetProperty.PropertyType, val.ToString());

                                        targetProperty.SetValue(target, eVal, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(bool) || targetProperty.PropertyType == typeof(Nullable<bool>))
                                    {
                                        targetProperty.SetValue(target, (Convert.ToInt32(val)) != 0, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToByte(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToInt16(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToDecimal(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToSingle(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToDouble(val), null);
                                    }
                                }
                                else if (sourceProperty.PropertyType == typeof(Nullable<int>))
                                {
                                    if (val != null)
                                    {
                                        var nVal = (Nullable<int>)val;

                                        if (nVal.HasValue)
                                        {
                                            if (targetProperty.PropertyType == typeof(string))
                                            {
                                                targetProperty.SetValue(target, nVal.Value.ToString(), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(int))
                                            {
                                                targetProperty.SetValue(target, nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType.IsEnum)
                                            {
                                                var eVal = Enum.Parse(targetProperty.PropertyType, nVal.Value.ToString());

                                                targetProperty.SetValue(target, eVal, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(bool) || targetProperty.PropertyType == typeof(Nullable<bool>))
                                            {
                                                targetProperty.SetValue(target, (Convert.ToInt32(nVal.Value)) != 0, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToByte(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToInt16(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToDecimal(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                            {
                                                targetProperty.SetValue(target, (long)nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                            {
                                                targetProperty.SetValue(target, (float)nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToDouble(nVal.Value), null);
                                            }
                                        }
                                    }
                                }

                                if (sourceProperty.PropertyType == typeof(long))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        targetProperty.SetValue(target, val.ToString(), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<long>))
                                    {
                                        targetProperty.SetValue(target, val, null);
                                    }
                                    else if (targetProperty.PropertyType.IsEnum)
                                    {
                                        var eVal = Enum.Parse(targetProperty.PropertyType, val.ToString());

                                        targetProperty.SetValue(target, eVal, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToByte(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToInt16(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToInt32(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToDecimal(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToSingle(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToDouble(val), null);
                                    }
                                }
                                else if (sourceProperty.PropertyType == typeof(Nullable<long>))
                                {
                                    if (val != null)
                                    {
                                        var nVal = (Nullable<long>)val;

                                        if (nVal.HasValue)
                                        {
                                            if (targetProperty.PropertyType == typeof(string))
                                            {
                                                targetProperty.SetValue(target, nVal.Value.ToString(), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(long))
                                            {
                                                targetProperty.SetValue(target, nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType.IsEnum)
                                            {
                                                var eVal = Enum.Parse(targetProperty.PropertyType, nVal.Value.ToString());

                                                targetProperty.SetValue(target, eVal, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToByte(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToInt16(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToInt32(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToDouble(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToDecimal(nVal.Value), null);
                                            }
                                        }
                                    }
                                }


                                if (sourceProperty.PropertyType == typeof(float))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        targetProperty.SetValue(target, val.ToString(), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToByte(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToInt16(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToInt32(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                    {
                                        targetProperty.SetValue(target, (long)val, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToSingle(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToDecimal(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        targetProperty.SetValue(target, val, null);
                                    }
                                }
                                else if (sourceProperty.PropertyType == typeof(Nullable<float>))
                                {
                                    if (val != null)
                                    {
                                        var nVal = (Nullable<float>)val;

                                        if (nVal.HasValue)
                                        {
                                            if (targetProperty.PropertyType == typeof(string))
                                            {
                                                targetProperty.SetValue(target, nVal.Value.ToString(), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(float))
                                            {
                                                targetProperty.SetValue(target, nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToByte(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToInt16(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToInt32(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                            {
                                                targetProperty.SetValue(target, (long)nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToDecimal(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToDouble(nVal.Value), null);
                                            }
                                        }
                                    }
                                }

                                if (sourceProperty.PropertyType == typeof(double))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        targetProperty.SetValue(target, val.ToString(), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToByte(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToInt16(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToInt32(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                    {
                                        targetProperty.SetValue(target, (long)val, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToSingle(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToDecimal(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        targetProperty.SetValue(target, val, null);
                                    }
                                }
                                else if (sourceProperty.PropertyType == typeof(Nullable<double>))
                                {
                                    if (val != null)
                                    {
                                        var nVal = (Nullable<double>)val;

                                        if (nVal.HasValue)
                                        {
                                            if (targetProperty.PropertyType == typeof(string))
                                            {
                                                targetProperty.SetValue(target, nVal.Value.ToString(), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                            {
                                                targetProperty.SetValue(target, nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToByte(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToInt16(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToInt32(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                            {
                                                targetProperty.SetValue(target, (long)nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                            {
                                                targetProperty.SetValue(target, (float)nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(double))
                                            {
                                                targetProperty.SetValue(target, Convert.ToDouble(nVal.Value), null);
                                            }
                                        }
                                    }
                                }

                                if (sourceProperty.PropertyType == typeof(decimal))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        targetProperty.SetValue(target, val.ToString(), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToByte(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToInt16(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToInt32(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                    {
                                        targetProperty.SetValue(target, (long)val, null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToSingle(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        targetProperty.SetValue(target, Convert.ToDouble(val), null);
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        targetProperty.SetValue(target, val, null);
                                    }
                                }
                                else if (sourceProperty.PropertyType == typeof(Nullable<decimal>))
                                {
                                    if (val != null)
                                    {
                                        var nVal = (Nullable<decimal>)val;

                                        if (nVal.HasValue)
                                        {
                                            if (targetProperty.PropertyType == typeof(string))
                                            {
                                                targetProperty.SetValue(target, nVal.Value.ToString(), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal))
                                            {
                                                targetProperty.SetValue(target, nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToByte(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToInt16(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToInt32(nVal.Value), null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                            {
                                                targetProperty.SetValue(target, (long)nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                            {
                                                targetProperty.SetValue(target, (float)nVal.Value, null);
                                            }
                                            else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                            {
                                                targetProperty.SetValue(target, Convert.ToDouble(nVal.Value), null);
                                            }
                                        }
                                    }
                                }

                                #endregion

                                #region 枚举

                                if (sourceProperty.PropertyType.IsEnum)
                                {
                                    if (sourceProperty.PropertyType == typeof(string))
                                    {
                                        var str = Enum.GetName(val.GetType(), val);
                                        targetProperty.SetValue(target, str, null);
                                    }
                                    else if (sourceProperty.PropertyType == typeof(byte))
                                    {
                                        var nVal = Convert.ToByte(val);
                                        targetProperty.SetValue(target, nVal, null);
                                    }
                                    else if (sourceProperty.PropertyType == typeof(short))
                                    {
                                        var nVal = Convert.ToInt16(val);
                                        targetProperty.SetValue(target, nVal, null);
                                    }
                                    else if (sourceProperty.PropertyType == typeof(int))
                                    {
                                        var nVal = Convert.ToInt32(val);
                                        targetProperty.SetValue(target, nVal, null);
                                    }
                                }
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"WEF ConvertTo 中指定的转换无效，sourceProperty:{sourceProperty.Name} {sourceProperty.PropertyType}, targetProperty:{targetProperty.Name} {targetProperty.PropertyType} ,err:{ex.Message}");
                        }
                    }


                }
                return target;
            }
            return default(T);
        }

        /// <summary>
        /// 转换成另外一个实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(this object source) where T : class
        {
            if (source != null)
            {
                if (source.GetType().GetInterface("IEnumerable", true) != null)
                {
                    var list = (System.Collections.IEnumerable)source;

                    var result = new List<T>();

                    foreach (var item in list)
                    {
                        result.Add(item.ConvertTo<T>());
                    }

                    return result;
                }
            }
            return null;
        }
    }
}
