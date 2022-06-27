/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Db
*文件名： DbProviderCreator
*版本号： V1.0.0.0
*唯一标识：f48ff98a-a27e-44be-a636-8fa2bd7f9d56
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2022/2/28 14:19:57
*描述：创建Provider
*
*=====================================================================
*修改标记
*修改时间：2022/2/28 14:19:57
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：创建Provider
*
*****************************************************************************/

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
        /// <param name="dt"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static DbProvider Create(DatabaseType dt, string connStr)
        {

            DbProvider provider = null;
            switch (dt)
            {
                case DatabaseType.SqlServer9:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(SqlServer9Provider).FullName, connStr, dt);
                    break;
                case DatabaseType.SqlServer:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(SqlServerProvider).FullName, connStr, dt);
                    break;
                case DatabaseType.Oracle:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(OracleProvider).FullName, connStr, dt);
                    break;
                case DatabaseType.MariaDB:
                case DatabaseType.MySql:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(MySqlProvider).FullName, connStr, dt);
                    break;
                case DatabaseType.Sqlite3:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(SqliteProvider).FullName, connStr, dt);
                    break;
                case DatabaseType.MsAccess:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(MsAccessProvider).FullName, connStr, dt);
                    break;
                case DatabaseType.PostgreSQL:
                    provider = ProviderFactory.CreateDbProvider(null, typeof(PostgreSqlProvider).FullName, connStr, dt);
                    break;
            }
            if (provider != null)
            {
                provider.DatabaseType = dt;
            }
            return provider;
        }
    }
}
