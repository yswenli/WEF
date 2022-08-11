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

using WEF.Common;
using WEF.Db;

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
        /// <param name="database"></param>
        /// <returns></returns>
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
    }
}
