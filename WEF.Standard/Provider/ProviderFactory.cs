/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2024
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;

using WEF.Common;

namespace WEF.Provider
{
    /// <summary>
    /// The db provider factory.
    /// </summary>
    public sealed class ProviderFactory
    {
        #region Private Members

        private static Dictionary<string, DbProvider> _providerCache = new Dictionary<string, DbProvider>();
        private static object _lock = new object();

        private ProviderFactory()
        {
        }

        #endregion

        #region Public Members

        /// <summary>
        /// 创建数据库事件提供程序
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="connectionString">The conn STR.</param>
        /// <returns>The db provider.</returns>
        public static DbProvider CreateDbProvider(string assemblyName, string className, string connectionString)
        {
            Check.Require(connectionString, "connectionString", Check.NotNullOrEmpty);

            lock (_lock)
            {
                string cacheKey = string.Concat(assemblyName, className, connectionString);

                if (_providerCache.ContainsKey(cacheKey))
                {
                    return _providerCache[cacheKey];
                }
                else
                {
                    if (connectionString.IndexOf("microsoft.jet.oledb", StringComparison.OrdinalIgnoreCase) > -1 || connectionString.IndexOf(".db3", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        Check.Require(connectionString.IndexOf("data source", StringComparison.OrdinalIgnoreCase) > -1, "ConnectionString的格式有错误，请查证！");

                        string mdbPath = connectionString.Substring(connectionString.IndexOf("data source", StringComparison.OrdinalIgnoreCase) + "data source".Length + 1).TrimStart(' ', '=');
                        if (mdbPath.ToLower().StartsWith("|datadirectory|"))
                        {
                            mdbPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\App_Data" + mdbPath.Substring("|datadirectory|".Length);
                        }
                        else if (connectionString.StartsWith("./") || connectionString.EndsWith(".\\"))
                        {
                            connectionString = connectionString.Replace("/", "\\").Replace(".\\", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\");
                        }
                        connectionString = connectionString.Substring(0, connectionString.ToLower().IndexOf("data source")) + "Data Source=" + mdbPath;
                    }

                    //如果是~则表示当前目录
                    if (connectionString.Contains("~/") || connectionString.Contains("~\\"))
                    {
                        connectionString = connectionString.Replace("/", "\\").Replace("~\\", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\");
                    }

                    //by default, using sqlserver db provider
                    if (string.IsNullOrEmpty(className))
                    {
                        className = "WEF.Provider.SqlServerProvider";
                    }
                    else if (String.Compare(className, "System.Data.SqlClient", StringComparison.OrdinalIgnoreCase) == 0 || 
                        String.Compare(className, "WEF.Standard.MSSQL", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        className = "WEF.Provider.SqlServerProvider";
                    }
                    else if (String.Compare(className, "WEF.SqlServer9", StringComparison.OrdinalIgnoreCase) == 0
                        || String.Compare(className, "WEF.Provider.SqlServerProvider", StringComparison.OrdinalIgnoreCase) == 0
                        || className.IndexOf("SqlServer9", StringComparison.OrdinalIgnoreCase) >= 0
                        || className.IndexOf("sqlserver2005", StringComparison.OrdinalIgnoreCase) >= 0
                        || className.IndexOf("sql2005", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        className = "WEF.Provider.SqlServer9Provider";
                    }
                    else if (className.IndexOf("oracle", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        className = "WEF.Provider.OracleProvider";
                    }
                    else if (className.IndexOf("access", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        className = "WEF.Provider.MsAccessProvider";
                    }
                    else if (className.IndexOf("mysql", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        className = "WEF.Provider.MySqlProvider";
                    }
                    else if (className.IndexOf("sqlite", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        className = "WEF.Sqlite.SqliteProvider";
                    }

                    var provider = DipBuilder.CreateDbProvider(assemblyName, className, connectionString);

                    _providerCache[cacheKey] = provider;

                    return provider;
                }
            }
        }

        /// <summary>
        /// Gets the default db provider.
        /// </summary>
        /// <value>The default.</value>
        public static DbProvider Default
        {
            get
            {
                try
                {
                    if (ConfigurationManager.ConnectionStrings.Count > 0)
                    {
                        DbProvider dbProvider;
                        ConnectionStringSettings connStrSetting = ConfigurationManager.ConnectionStrings[ConfigurationManager.ConnectionStrings.Count - 1];
                        string[] assAndClass = connStrSetting.ProviderName.Split(',');
                        if (assAndClass.Length > 1)
                        {
                            dbProvider = CreateDbProvider(assAndClass[1].Trim(), assAndClass[0].Trim(), connStrSetting.ConnectionString);
                        }
                        else
                        {
                            dbProvider = CreateDbProvider(null, assAndClass[0].Trim(), connStrSetting.ConnectionString);
                        }

                        dbProvider.ConnectionStringsName = connStrSetting.Name;

                        return dbProvider;
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }
        }

        static object locker = new object();

        /// <summary>
        /// Creates the db provider.
        /// </summary>
        /// <param name="connStrName">Name of the conn STR.</param>
        /// <returns>The db provider.</returns>
        public static DbProvider CreateDbProvider(string connStrName)
        {

            lock (locker)
            {
                Check.Require(connStrName, "connStrName", Check.NotNullOrEmpty);

                var connStrSetting = ConfigurationManager.ConnectionStrings[connStrName];
                Check.Invariant(connStrSetting != null, null, new ConfigurationErrorsException(string.Concat("Cannot find specified connection string setting named as ", connStrName, " in application config file's ConnectionString section.")));
                if (connStrSetting == null || string.IsNullOrWhiteSpace(connStrSetting.ConnectionString))
                {
                    throw new Exception("连接字符串名称为connStrName：【" + connStrName + "】的没有配置具体的值！");
                }
                var assAndClass = connStrSetting.ProviderName.Split(',');
                var dbProvider = assAndClass.Length > 1
                    ? CreateDbProvider(assAndClass[0].Trim(), assAndClass[1].Trim(), connStrSetting.ConnectionString)
                    : CreateDbProvider(null, assAndClass[0].Trim(), connStrSetting.ConnectionString);
                dbProvider.ConnectionStringsName = connStrName;
                return dbProvider;
            }


        }
        #endregion
    }
}
