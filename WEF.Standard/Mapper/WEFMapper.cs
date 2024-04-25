/****************************************************************************
*Copyright (c) 2024 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：河之洲
*命名空间：WEF.Mapper
*文件名： WEFMapper
*版本号： V1.0.0.0
*唯一标识：79c43081-c0fa-472b-a95a-60776351cfb5
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2024/4/25 16:13:18
*描述：模型映射工具类
*
*=================================================
*修改标记
*修改时间：2024/4/25 16:13:18
*修改人： yswenli
*版本号： V1.0.0.0
*描述：模型映射工具类
*
*****************************************************************************/
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using WEF.Common;
using WEF.Mapper;

namespace WEF
{
    /// <summary>
    /// 模型映射工具类
    /// </summary>
    public static class WEFMapper
    {
        static ConcurrentDictionary<Type, List<string>> _ignoreCache = new ConcurrentDictionary<Type, List<string>>();

        /// <summary>
        /// 是否有忽略标签
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertiyName"></param>
        /// <returns></returns>
        static bool HasIgnoreAttribute(Type type, string propertiyName)
        {
            var ignoreList = _ignoreCache.GetOrAdd(type, (k) =>
            {
                var members = new List<string>();
                PropertyInfo[] properties = k.GetProperties();
                foreach (PropertyInfo p in properties)
                {
                    var isIgnore = false;
                    Attribute[] attibutes = Attribute.GetCustomAttributes(p);
                    foreach (Attribute attribute in attibutes)
                    {
                        //检查是否设置了SqlIgnore属性
                        if (attribute.GetType() == typeof(IgnoreMemberAttribute))
                        {
                            isIgnore = true;
                            break;
                        }
                    }
                    if (isIgnore)
                    {
                        members.Add(p.Name);
                    }
                }
                return members;
            });

            if (ignoreList.Count > 0 && ignoreList.Contains(propertiyName))
            {
                return true;
            }
            return false;
        }

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
            if (source != null)
            {
                var sourceType = source.GetType();

                if (!sourceType.IsClass) return null;

                var sourceProperties = sourceType.GetProperties();

                var type = targetType;

                var target = Activator.CreateInstance(targetType);

                var targetProperties = type.GetProperties();

                foreach (var targetProperty in targetProperties)
                {
                    if (HasIgnoreAttribute(type, targetProperty.Name))
                    {
                        continue;
                    }

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
                            if (HasIgnoreAttribute(sourceType, sourceProperty.Name))
                            {
                                continue;
                            }

                            var val = sourceProperty.GetValue(source, null);

                            if (sourceProperty.PropertyType == targetProperty.PropertyType)
                            {
                                if (val != null)
                                {
                                    DataUtils.SetPropertyValue(target, targetProperty, val);
                                }
                            }
                            else if (sourceProperty.PropertyType.GetInterface("IList", true) != null && targetProperty.PropertyType.GetInterface("IList", true) != null)
                            {
                                val = DataUtils.GetPropertyValue(source, sourceProperty);
                                if (val != null)
                                {
                                    var sType = targetProperty.PropertyType.GenericTypeArguments[0];
                                    var list = val.ConvertToList(sType, convertMatchType);
                                    targetProperty.SetValue(target, list);
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
                if (source.GetType().GetInterface("IList", true) != null)
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

        /// <summary>
        /// 转换成另外一个实体列表
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetType"></param>
        /// <param name="convertMatchType"></param>
        /// <returns></returns>
        public static IList ConvertToList(this object source, Type targetType, ConvertMatchType convertMatchType = ConvertMatchType.IgnoreCase)
        {
            if (source != null)
            {
                if (source.GetType().GetInterface("IList", true) != null)
                {
                    var list = (System.Collections.IEnumerable)source;

                    var result = targetType.CreateList();

                    foreach (var item in list)
                    {
                        result.Add(item.ConvertTo(targetType, convertMatchType));
                    }

                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList CreateList(this Type type)
        {
            return (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { type }));
        }
        #endregion
    }
}
