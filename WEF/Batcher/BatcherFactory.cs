/****************************************************************************
*项目名称：WEF.Batcher
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Batcher
*类 名 称：BatcherFactory
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/9/14 13:28:58
*描述：
*=====================================================================
*修改时间：2020/9/14 13:28:58
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using WEF.Provider;

namespace WEF.Batcher
{
    /// <summary>
    /// BatcherFactory
    /// </summary>
    public static class BatcherFactory
    {
        /// <summary>
        /// 创建一个批量对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbProvider"></param>
        /// <returns></returns>
        public static IBatcher<T> CreateBatcher<T>(DbProvider dbProvider) where T : Entity
        {
            switch (dbProvider.DatabaseType)
            {
                case DatabaseType.SqlServer:
                case DatabaseType.SqlServer9:
                    return new MsSqlBatcher<T>((SqlServer9Provider)dbProvider);
                case DatabaseType.MySql:
                case DatabaseType.MariaDB:
                    return new MySqlBatcher<T>((MySqlProvider)dbProvider);
                case DatabaseType.Oracle:
                    return new OracleBatcher<T>((OracleProvider)dbProvider);
                case DatabaseType.PostgreSQL:
                    return new PostgresSqlBatcher<T>((PostgreSqlProvider)dbProvider);
                default:
                    throw new Exception("不支持的数据库类型：" + dbProvider.DatabaseType.ToString());

            }
        }
    }
}
