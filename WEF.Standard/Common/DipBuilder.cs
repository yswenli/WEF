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
*修改人： yswenli
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Reflection;

using WEF.Batcher;
using WEF.Db;
using WEF.Provider;

namespace WEF.Common
{
    /// <summary>
    /// 依赖倒置工具类
    /// </summary>
    public static class DipBuilder
    {
        static ConcurrentDictionary<string, Assembly> _typeCache;

        static ConcurrentDictionary<string, dynamic> _instanceCahce;

        static readonly string _version;

        /// <summary>
        /// 依赖倒置工具类
        /// </summary>
        static DipBuilder()
        {
            _typeCache = new ConcurrentDictionary<string, Assembly>();

            _instanceCahce = new ConcurrentDictionary<string, dynamic>();

            _version= Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// 获取版本
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            return _version;
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
            var key = assemblyName + typeName + connStr;
            return (DbProvider)_instanceCahce.GetOrAdd(key, (a) => Create<DbProvider>(assemblyName, typeName, new object[] { connStr }));
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static dynamic CreateWithGeneric<T>(string assemblyName, string typeName, object[] args = null)
        {
            var fullName = $"{typeName}`1,{assemblyName}";
            var key = fullName + typeof(T).Name;
            return _instanceCahce.GetOrAdd(key, (k) =>
            {
                Type classType = Type.GetType(fullName);
                Type constructedType = classType.MakeGenericType(typeof(T));
                return Activator.CreateInstance(constructedType, args);
            });
        }

        /// <summary>
        /// 创建一个批量对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IBatcher<T> CreateBatcher<T>(Database database) where T : Entity
        {
            var version = DipBuilder.GetVersion();
            switch (database.DbProvider.DatabaseType)
            {
                case DatabaseType.SqlServer:
                case DatabaseType.SqlServer9:
                    return (IBatcher<T>)DipBuilder.CreateWithGeneric<T>($"WEF.Standard.MSSQL, Version={version}, Culture=neutral, PublicKeyToken=null", "WEF.Batcher.MsSqlBatcher", new object[] { database });
                case DatabaseType.MsAccess:
                    return (IBatcher<T>)DipBuilder.CreateWithGeneric<T>($"WEF.Standard.OLEDB, Version={version}, Culture=neutral, PublicKeyToken=null", "WEF.Batcher.MsAccessBatcher", new object[] { database });
                case DatabaseType.MySql:
                case DatabaseType.MariaDB:
                    return (IBatcher<T>)DipBuilder.CreateWithGeneric<T>($"WEF.Standard.MySQL, Version={version}, Culture=neutral, PublicKeyToken=null", "WEF.Batcher.MySqlBatcher", new object[] { database });
                case DatabaseType.Oracle:
                    return (IBatcher<T>)DipBuilder.CreateWithGeneric<T>($"WEF.Standard.Oracle, Version={version}, Culture=neutral, PublicKeyToken=null", "WEF.Batcher.OracleBatcher", new object[] { database });
                case DatabaseType.PostgreSQL:
                    return (IBatcher<T>)DipBuilder.CreateWithGeneric<T>($"WEF.Standard.Postgre, Version={version}, Culture=neutral, PublicKeyToken=null", "WEF.Batcher.PostgresSqlBatcher", new object[] { database });
                default:
                    throw new Exception("不支持的数据库类型：" + database.DbProvider.DatabaseType.ToString());

            }
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
