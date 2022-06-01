/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Batcher
*文件名： AdoBatcher
*版本号： V1.0.0.0
*唯一标识：f0c04a8c-417f-49ee-9a11-b320874f4941
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2021/7/30 17:35:27
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/7/30 17:35:27
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System.Data;
using System.Data.Common;
using System.Globalization;

using WEF.Common;
using WEF.Db;

namespace WEF.Batcher
{
    /// <summary>
    /// ado批量操作类
    /// </summary>
    public class AdoBatcher
    {
        Database _database;

        string _tableName;

        int _timeout = 180;

        object _locker = new object();

        /// <summary>
        /// ado批量操作类
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        /// <param timeout="size"></param>
        public AdoBatcher(Database database, string tableName, int timeout = 180)
        {
            _database = database;
            _tableName = tableName;
            _timeout = timeout;

            Check.Require(tableName, "tableName", Check.NotNullOrEmpty);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public DataTable Fill(int size = 100)
        {
            using (DbConnection connection = _database.CreateConnection())
            {
                var selectCommand = _database.CreateCommandByCommandType(_timeout, CommandType.Text, $"select top {size} * from {_tableName}");

                if (_database.DbProvider.DatabaseType != DatabaseType.SqlServer && _database.DbProvider.DatabaseType != DatabaseType.SqlServer9)
                {
                    selectCommand = _database.CreateCommandByCommandType(_timeout, CommandType.Text, $"select * from {_tableName} limit 0,{size}");
                }

                _database.PrepareCommand(selectCommand, connection);

                using (DbDataAdapter adapter = _database.GetDataAdapter())
                {
                    DataTable targetDataTable = new DataTable(_tableName);

                    targetDataTable.Locale = CultureInfo.InvariantCulture;

                    adapter.SelectCommand = selectCommand;

                    adapter.Fill(targetDataTable);

                    targetDataTable.AcceptChanges();

                    return targetDataTable;
                }
            }
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public int Update(DataTable updateData)
        {
            lock (_locker)
            {

            }
            if (updateData == null || updateData.Rows == null || updateData.Rows.Count < 1)
                return -1;

            using (DbConnection connection = _database.CreateConnection())
            {
                using (var tran = connection.BeginTransaction())
                {
                    var selectCommand = _database.CreateCommandByCommandType(_timeout, CommandType.Text, $"select * from {_tableName} where 1=2");

                    if (_database.DbProvider.DatabaseType != DatabaseType.SqlServer && _database.DbProvider.DatabaseType != DatabaseType.SqlServer9)
                    {
                        selectCommand = _database.CreateCommandByCommandType(_timeout, CommandType.Text, $"select * from {_tableName} where 1=2");
                    }

                    _database.PrepareCommand(selectCommand, connection);

                    selectCommand.Transaction = tran;

                    using (DbDataAdapter adapter = _database.GetDataAdapter())
                    {
                        adapter.SelectCommand = selectCommand;

                        _database.PrepareAdapter(adapter, tran);

                        var count = adapter.Update(updateData);

                        tran.Commit();

                        return count;
                    }
                }
            }
        }

        //
    }
}
