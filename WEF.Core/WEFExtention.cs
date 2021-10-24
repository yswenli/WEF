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
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using WEF.Common;

namespace WEF
{
    /// <summary>
    /// WEF lambda相关检测扩展
    /// </summary>
    public static class WEFExtention
    {
        #region 限制对其他非WEF lambda表达式的使用方法

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

        #endregion

        #region Convert
        /// <summary>
        /// 转换成另外一个实体
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetType"></param>
        /// <param name="convertMatchType"></param>
        /// <returns></returns>
        public static object ConvertTo(this object source, Type targetType, ConvertMatchType convertMatchType = ConvertMatchType.IgnoreCase)
        {
            if (source != null && source.GetType().IsClass)
            {
                var sourceProperties = source.GetType().GetProperties();

                var type = targetType;

                var target = Activator.CreateInstance(targetType);

                var targetProperties = type.GetProperties();

                foreach (var targetProperty in targetProperties)
                {
                    PropertyInfo sourceProperty = null;

                    switch (convertMatchType)
                    {
                        case ConvertMatchType.ExactlyMatch:
                            sourceProperty = sourceProperties.Where(b => b.Name == targetProperty.Name).FirstOrDefault();
                            break;
                        case ConvertMatchType.IgnoreCase:
                            sourceProperty = sourceProperties.Where(b => b.Name.ToLower() == targetProperty.Name.ToLower()).FirstOrDefault();
                            break;
                        case ConvertMatchType.Contain:
                            sourceProperty = sourceProperties.Where(b => b.Name.IndexOf(targetProperty.Name) > -1 || targetProperty.Name.IndexOf(b.Name) > -1).FirstOrDefault();
                            break;
                        case ConvertMatchType.ContainAndIgnoreCase:
                            sourceProperty = sourceProperties.Where(b => b.Name.IndexOf(targetProperty.Name, StringComparison.CurrentCultureIgnoreCase) > -1 || targetProperty.Name.IndexOf(b.Name, StringComparison.CurrentCultureIgnoreCase) > -1).FirstOrDefault();
                            break;
                    }

                    if (sourceProperty != null)
                    {
                        try
                        {
                            var val = sourceProperty.GetValue(source, null);

                            if (sourceProperty.PropertyType == targetProperty.PropertyType)
                            {
                                if (val != null)
                                {
                                    DataUtils.SetPropertyValue(target, targetProperty, val);
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
                                            DataUtils.SetPropertyValue(target, targetProperty, dt.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                        }
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<DateTime>))
                                    {
                                        if (val != null)
                                        {
                                            DataUtils.SetPropertyValue(target, targetProperty, val);
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
                                                DataUtils.SetPropertyValue(target, targetProperty, dt.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
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
                                                DataUtils.SetPropertyValue(target, targetProperty, dt.Value);
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

                                        DataUtils.SetPropertyValue(target, targetProperty, num);

                                    }
                                    else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                    {
                                        long num = 0;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            long.TryParse(str, out num);
                                        }

                                        DataUtils.SetPropertyValue(target, targetProperty, num);
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        short num = 0;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            short.TryParse(str, out num);
                                        }

                                        DataUtils.SetPropertyValue(target, targetProperty, num);
                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        byte num = 0;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            byte.TryParse(str, out num);
                                        }

                                        DataUtils.SetPropertyValue(target, targetProperty, num);
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        float num = 0;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            float.TryParse(str, out num);
                                        }

                                        DataUtils.SetPropertyValue(target, targetProperty, num);
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        double num = 0;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            double.TryParse(str, out num);
                                        }

                                        DataUtils.SetPropertyValue(target, targetProperty, num);
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        decimal num = 0;

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            decimal.TryParse(str, out num);
                                        }

                                        DataUtils.SetPropertyValue(target, targetProperty, num);
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

                                        DataUtils.SetPropertyValue(target, targetProperty, bVal);
                                    }
                                    else if (targetProperty.PropertyType == typeof(DateTime) || targetProperty.PropertyType == typeof(Nullable<DateTime>))
                                    {
                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            if (DateTime.TryParse(str, out DateTime dt))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, dt);
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
                                        DataUtils.SetPropertyValue(target, targetProperty, val.ToString());
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, val);
                                    }
                                    else if (targetProperty.PropertyType.IsEnum)
                                    {
                                        var eVal = Enum.Parse(targetProperty.PropertyType, val.ToString());

                                        DataUtils.SetPropertyValue(target, targetProperty, eVal);
                                    }
                                    else if (targetProperty.PropertyType == typeof(bool) || targetProperty.PropertyType == typeof(Nullable<bool>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, (Convert.ToByte(val)) != 0);
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt16(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt32(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDecimal(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToSingle(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDouble(val));
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
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal.Value.ToString());
                                            }
                                            else if (targetProperty.PropertyType == typeof(byte))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal);
                                            }
                                            else if (targetProperty.PropertyType.IsEnum)
                                            {
                                                var eVal = Enum.Parse(targetProperty.PropertyType, nVal.Value.ToString());

                                                DataUtils.SetPropertyValue(target, targetProperty, eVal);
                                            }
                                            else if (targetProperty.PropertyType == typeof(bool) || targetProperty.PropertyType == typeof(Nullable<bool>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (Convert.ToByte(nVal.Value)) != 0);

                                            }
                                            else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt16(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt32(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDecimal(nVal.Value));

                                            }
                                            else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (long)nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (float)nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDouble(nVal.Value));

                                            }
                                        }
                                    }
                                }

                                if (sourceProperty.PropertyType == typeof(short))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, val.ToString());
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, val);
                                    }
                                    else if (targetProperty.PropertyType.IsEnum)
                                    {
                                        var eVal = Enum.Parse(targetProperty.PropertyType, val.ToString());

                                        DataUtils.SetPropertyValue(target, targetProperty, eVal);
                                    }
                                    else if (targetProperty.PropertyType == typeof(bool) || targetProperty.PropertyType == typeof(Nullable<bool>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, (Convert.ToInt16(val)) != 0);
                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToByte(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt32(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDecimal(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToSingle(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDouble(val));
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
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal.Value.ToString());
                                            }
                                            else if (targetProperty.PropertyType == typeof(short))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType.IsEnum)
                                            {
                                                var eVal = Enum.Parse(targetProperty.PropertyType, nVal.Value.ToString());

                                                DataUtils.SetPropertyValue(target, targetProperty, eVal);
                                            }
                                            else if (targetProperty.PropertyType == typeof(bool) || targetProperty.PropertyType == typeof(Nullable<bool>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (Convert.ToInt16(nVal.Value)) != 0);

                                            }
                                            else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToByte(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt32(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDecimal(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (long)nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (float)nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDouble(nVal.Value));
                                            }
                                        }
                                    }
                                }

                                if (sourceProperty.PropertyType == typeof(int))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, val.ToString());
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, val);
                                    }
                                    else if (targetProperty.PropertyType.IsEnum)
                                    {
                                        var eVal = Enum.Parse(targetProperty.PropertyType, val.ToString());

                                        DataUtils.SetPropertyValue(target, targetProperty, eVal);
                                    }
                                    else if (targetProperty.PropertyType == typeof(bool) || targetProperty.PropertyType == typeof(Nullable<bool>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, (Convert.ToInt32(val)) != 0);

                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToByte(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt16(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDecimal(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToSingle(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDouble(val));
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
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal.Value.ToString());
                                            }
                                            else if (targetProperty.PropertyType == typeof(int))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType.IsEnum)
                                            {
                                                var eVal = Enum.Parse(targetProperty.PropertyType, nVal.Value.ToString());

                                                DataUtils.SetPropertyValue(target, targetProperty, eVal);
                                            }
                                            else if (targetProperty.PropertyType == typeof(bool) || targetProperty.PropertyType == typeof(Nullable<bool>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (Convert.ToInt32(nVal.Value)) != 0);

                                            }
                                            else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToByte(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt16(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDecimal(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (long)nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (float)nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDouble(nVal.Value));
                                            }
                                        }
                                    }
                                }

                                if (sourceProperty.PropertyType == typeof(long))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, val.ToString());
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<long>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, val);
                                    }
                                    else if (targetProperty.PropertyType.IsEnum)
                                    {
                                        var eVal = Enum.Parse(targetProperty.PropertyType, val.ToString());

                                        DataUtils.SetPropertyValue(target, targetProperty, eVal);
                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToByte(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt16(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt32(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDecimal(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToSingle(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDouble(val));
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
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal.Value.ToString());
                                            }
                                            else if (targetProperty.PropertyType == typeof(long))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType.IsEnum)
                                            {
                                                var eVal = Enum.Parse(targetProperty.PropertyType, nVal.Value.ToString());

                                                DataUtils.SetPropertyValue(target, targetProperty, eVal);
                                            }
                                            else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToByte(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt16(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt32(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDouble(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDecimal(nVal.Value));
                                            }
                                        }
                                    }
                                }


                                if (sourceProperty.PropertyType == typeof(float))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, val.ToString());
                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToByte(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt16(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt32(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, (long)val);
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToSingle(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDecimal(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, val);
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
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal.Value.ToString());
                                            }
                                            else if (targetProperty.PropertyType == typeof(float))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToByte(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt16(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt32(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (long)nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDecimal(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDouble(nVal.Value));
                                            }
                                        }
                                    }
                                }

                                if (sourceProperty.PropertyType == typeof(double))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, val.ToString());
                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToByte(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt16(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt32(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, (long)val);
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToSingle(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDecimal(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, val);
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
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal.Value.ToString());
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal) || targetProperty.PropertyType == typeof(Nullable<decimal>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToByte(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt16(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt32(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (long)nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (float)nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(double))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDouble(nVal.Value));
                                            }
                                        }
                                    }
                                }

                                if (sourceProperty.PropertyType == typeof(decimal))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, val.ToString());
                                    }
                                    else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToByte(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt16(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt32(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, (long)val);
                                    }
                                    else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToSingle(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDouble(val));
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<decimal>))
                                    {
                                        DataUtils.SetPropertyValue(target, targetProperty, val);
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
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal.Value.ToString());
                                            }
                                            else if (targetProperty.PropertyType == typeof(decimal))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(byte) || targetProperty.PropertyType == typeof(Nullable<byte>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToByte(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(short) || targetProperty.PropertyType == typeof(Nullable<short>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt16(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(int) || targetProperty.PropertyType == typeof(Nullable<int>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToInt32(nVal.Value));
                                            }
                                            else if (targetProperty.PropertyType == typeof(long) || targetProperty.PropertyType == typeof(Nullable<long>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (long)nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(float) || targetProperty.PropertyType == typeof(Nullable<float>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, (float)nVal.Value);
                                            }
                                            else if (targetProperty.PropertyType == typeof(double) || targetProperty.PropertyType == typeof(Nullable<double>))
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, Convert.ToDouble(nVal.Value));
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
                                        DataUtils.SetPropertyValue(target, targetProperty, str);
                                    }
                                    else if (sourceProperty.PropertyType == typeof(byte))
                                    {
                                        var nVal = Convert.ToByte(val);
                                        DataUtils.SetPropertyValue(target, targetProperty, nVal);
                                    }
                                    else if (sourceProperty.PropertyType == typeof(short))
                                    {
                                        var nVal = Convert.ToInt16(val);
                                        DataUtils.SetPropertyValue(target, targetProperty, nVal);
                                    }
                                    else if (sourceProperty.PropertyType == typeof(int))
                                    {
                                        var nVal = Convert.ToInt32(val);
                                        DataUtils.SetPropertyValue(target, targetProperty, nVal);
                                    }
                                }
                                #endregion

                                #region bool

                                if (sourceProperty.PropertyType == typeof(bool))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        if (val != null)
                                        {
                                            DataUtils.SetPropertyValue(target, targetProperty, val.ToString());
                                        }
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<bool>))
                                    {
                                        if (val != null)
                                        {
                                            DataUtils.SetPropertyValue(target, targetProperty, val);
                                        }
                                    }
                                }
                                else if (sourceProperty.PropertyType == typeof(Nullable<bool>))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        if (val != null)
                                        {
                                            var tval = (Nullable<bool>)val;
                                            if (tval.HasValue)
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, tval.ToString());
                                            }
                                        }
                                    }
                                    else if (targetProperty.PropertyType == typeof(bool))
                                    {
                                        if (val != null)
                                        {
                                            var tavl = (Nullable<bool>)val;
                                            if (tavl.HasValue)
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, tavl.Value);
                                            }
                                        }
                                    }
                                }

                                #endregion

                                #region guid

                                if (sourceProperty.PropertyType == typeof(Guid))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        if (val != null)
                                        {
                                            var guid = (Guid)val;
                                            DataUtils.SetPropertyValue(target, targetProperty, guid.ToString("N"));
                                        }
                                        else
                                        {
                                            DataUtils.SetPropertyValue(target, targetProperty, Guid.Empty.ToString("N"));
                                        }
                                    }
                                    else if (targetProperty.PropertyType == typeof(Nullable<Guid>))
                                    {
                                        if (val != null)
                                        {
                                            DataUtils.SetPropertyValue(target, targetProperty, val);
                                        }
                                    }
                                }
                                else if (sourceProperty.PropertyType == typeof(Nullable<Guid>))
                                {
                                    if (targetProperty.PropertyType == typeof(string))
                                    {
                                        if (val != null)
                                        {
                                            var guid = (Nullable<Guid>)val;
                                            if (guid.HasValue)
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, guid.Value.ToString("N"));
                                            }
                                        }
                                    }
                                    else if (targetProperty.PropertyType == typeof(Guid))
                                    {
                                        if (val != null)
                                        {
                                            var guid = (Nullable<Guid>)val;
                                            if (guid.HasValue)
                                            {
                                                DataUtils.SetPropertyValue(target, targetProperty, guid.Value);
                                            }
                                        }
                                    }
                                }
                                else if (sourceProperty.PropertyType == typeof(string))
                                {
                                    var str = (string)val;

                                    if (targetProperty.PropertyType == typeof(Nullable<Guid>) || targetProperty.PropertyType == typeof(Guid))
                                    {
                                        if (string.IsNullOrEmpty(str))
                                        {
                                            DataUtils.SetPropertyValue(target, targetProperty, Guid.Empty);
                                        }
                                        else
                                        {
                                            if (Guid.TryParse(str, out Guid sval))
                                                DataUtils.SetPropertyValue(target, targetProperty, sval);
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"WEF ConvertTo 中发生异常，指定的类不是公开的或者不支持的属性类型，sourceProperty:{sourceProperty.Name} {sourceProperty.PropertyType}, targetProperty:{targetProperty.Name} {targetProperty.PropertyType} ,err:{ex.Message}");
                        }
                    }


                }
                return target;
            }
            return null;
        }

        /// <summary>
        /// 转换成另外一个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="convertMatchType"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object source, ConvertMatchType convertMatchType = ConvertMatchType.IgnoreCase) where T : class
        {
            return (T)source.ConvertTo(typeof(T), convertMatchType);
        }

        /// <summary>
        /// 转换成另外一个实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="convertMatchType"></param>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(this object source, ConvertMatchType convertMatchType = ConvertMatchType.IgnoreCase) where T : class
        {
            if (source != null)
            {
                if (source.GetType().GetInterface("IEnumerable", true) != null)
                {
                    var list = (System.Collections.IEnumerable)source;

                    var result = new List<T>();

                    foreach (var item in list)
                    {
                        result.Add(item.ConvertTo<T>(convertMatchType));
                    }

                    return result;
                }
            }
            return null;
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
        /// 填充模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="t2"></param>
        /// <param name="allowNull"></param>
        /// <returns></returns>
        public static bool FillModel<T>(this T source, ref T t2, bool allowNull = false)
        {
            try
            {
                if (source != null)
                {
                    var type = source.GetType();

                    var properties = type.GetProperties();

                    if (!allowNull)
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
                return true;
            }
            catch
            {

            }
            return false;
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
            var result = FillModel(source, ref t2, allowNull);

            t2.ClearModifyFields();

            return result;
        }

        /// <summary>
        /// 从某个模型中填充当前实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="target"></param>
        /// <param name="convertMatchType"></param>
        /// <param name="allowNull">是否填空值</param>
        public static bool FillFrom<T>(this IEntity entity, T target, ConvertMatchType convertMatchType = ConvertMatchType.IgnoreCase, bool allowNull = false) where T : class
        {
            var s = (IEntity)target.ConvertTo(entity.GetType(), convertMatchType);

            return FillModel(s, ref entity, allowNull);
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
                default:
                    throw new NotSupportedException("不支持的类型");
            }
        }

        #endregion

    }
}
