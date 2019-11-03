/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2019
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：9e0900e8-6a3c-41a2-8b9b-a31412c94244
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：WEF
 * 类名称：DatabaseType
 * 创建时间：2017/7/26 15:53:02
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/

namespace WEF
{
    /// <summary>
    /// Type of a database.
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// SQL Server 2000
        /// </summary>
        SqlServer = 0,
        /// <summary>
        /// MsAccess
        /// </summary>
        MsAccess = 1,
        /// <summary>
        /// SQL Server 2005
        /// </summary>
        SqlServer9 = 2,
        /// <summary>
        /// Oracle
        /// </summary>
        Oracle = 3,
        /// <summary>
        /// Sqlite
        /// </summary>
        Sqlite3 = 4,
        /// <summary>
        /// MySql
        /// </summary>
        MySql = 5,
        /// <summary>
        /// MongoDB
        /// </summary>
        MongoDB=6,
        /// <summary>
        /// PostgreSQL
        /// </summary>
        PostgreSQL = 7
    }
}
