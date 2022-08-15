/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Common
*文件名： DipHelper
*版本号： V1.0.0.0
*唯一标识：925b61d7-aeb0-4f8a-9744-95dc522dcbcc
*当前的用户域：WALLE
*创建人： wenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/8/11 11:39:23
*描述：
*
*=================================================
*修改标记
*修改时间：2022/8/11 11:39:23
*修改人： yswen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Reflection;

using WEF.Provider;

namespace WEF.Common
{
    /// <summary>
    /// 依赖倒置工具类
    /// </summary>
    public static class DipBuilder
    {
        static ConcurrentDictionary<string, Assembly> _typeCache;

        static ConcurrentDictionary<string, object> _instanceCahce;

        /// <summary>
        /// 依赖倒置工具类
        /// </summary>
        static DipBuilder()
        {
            _typeCache = new ConcurrentDictionary<string, Assembly>();

            _instanceCahce = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// 获取版本
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object Create(string assemblyName, string typeName, object[] args = null)
        {
            var ass = _typeCache.GetOrAdd(assemblyName, (a) => Assembly.Load(assemblyName));
            return ass.CreateInstance(typeName,
                         true,
                         BindingFlags.Default,
                         null,
                         args,
                         System.Globalization.CultureInfo.InvariantCulture,
                         null);
        }
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T Create<T>(string assemblyName, string typeName, object[] args = null)
        {
            return (T)Create(assemblyName, typeName, args);
        }

        /// <summary>
        /// 创建DbProvider实例
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static DbProvider CreateDbProvider(string assemblyName, string typeName, string connStr)
        {
            return (DbProvider)_instanceCahce.GetOrAdd(assemblyName + typeName + connStr, (a) => Create<DbProvider>(assemblyName, typeName, new object[] { connStr }));
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateWithGeneric<T>(string assemblyName, string typeName, object[] args = null)
        {
            var key = $"{typeName}`1, {assemblyName}";
            return _instanceCahce.GetOrAdd(key, (k) =>
            {
                Type classType = Type.GetType(key);
                Type constructedType = classType.MakeGenericType(typeof(T));
                return Activator.CreateInstance(constructedType, args);
            });
        }

        /// <summary>
        /// 将mysql 的日期转换C# 日期
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static DateTime FromMysqlDateTime(this object val)
        {
            var type = val.GetType();
            return (DateTime)type.GetMethod("GetDateTime").Invoke(val, null);
        }

        /// <summary>
        /// 将C#日期转换成mysql日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static dynamic ToMysqlDateTime(this DateTime dt)
        {
            return Create("MySql.Data, Version=8.0.30.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d",
                "", new object[] { dt });
        }

    }
}
