﻿/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Db
*文件名： DbProviderCreator
*版本号： V1.0.0.0
*唯一标识：f48ff98a-a27e-44be-a636-8fa2bd7f9d56
*当前的用户域：OCEANIA
*创建人： Walle.Wen
*电子邮箱：Walle.Wen@oceania-inc.com
*创建时间：2022/2/28 14:19:57
*描述：创建Provider
*
*=====================================================================
*修改标记
*修改时间：2022/2/28 14:19:57
*修改人： Walle.Wen
*版本号： V1.0.0.0
*描述：创建Provider
*
*****************************************************************************/

using WEF.Common;

namespace WEF.Provider
{
    /// <summary>
    /// 创建Provider
    /// </summary>
    public static class DbProviderCreator
    {
        /// <summary>
        /// 创建Provider
        /// </summary>
        /// <param name="databaseType"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static DbProvider Create(DatabaseType databaseType, string connStr)
        {
            var version = DipBuilder.GetVersion();
            DbProvider provider = null;
            switch (databaseType)
            {
                case DatabaseType.SqlServer9:
                    provider = ProviderFactory.CreateDbProvider($"WEF.Standard.MSSQL, Version={version}, Culture=neutral, PublicKeyToken=null",
                        "WEF.Provider.SqlServer9Provider", connStr);
                    break;
                case DatabaseType.SqlServer:
                    provider = ProviderFactory.CreateDbProvider($"WEF.Standard.MSSQL, Version={version}, Culture=neutral, PublicKeyToken=null",
                        "WEF.Provider.SqlServerProvider", connStr);
                    break;
                case DatabaseType.Oracle:
                    provider = ProviderFactory.CreateDbProvider($"WEF.Standard.Oracle, Version={version}, Culture=neutral, PublicKeyToken=null",
                        "WEF.ProviderOracleProvider", connStr);
                    break;
                case DatabaseType.MariaDB:
                case DatabaseType.MySql:
                    provider = ProviderFactory.CreateDbProvider($"WEF.Standard.MySQL, Version={version}, Culture=neutral, PublicKeyToken=null",
                        "WEF.Provider.MySqlProvider", connStr);
                    break;
                case DatabaseType.Sqlite3:
                    provider = ProviderFactory.CreateDbProvider($"WEF.Standard.SQLite, Version={version}, Culture=neutral, PublicKeyToken=null",
                        "WEF.Provider.SqliteProvider", connStr);
                    break;
                case DatabaseType.MsAccess:
                    provider = ProviderFactory.CreateDbProvider($"WEF.Standard.OLEDB, Version={version}, Culture=neutral, PublicKeyToken=null",
                        "WEF.Provider.MsAccessProvider", connStr);
                    break;
                case DatabaseType.PostgreSQL:
                    provider = ProviderFactory.CreateDbProvider($"WEF.Standard.Postgre, Version={version}, Culture=neutral, PublicKeyToken=null",
                        "WEF.Provider.PostgreSqlProvider", connStr);
                    break;
            }
            if (provider != null)
            {
                provider.DatabaseType = databaseType;
            }
            return provider;
        }
    }
}
