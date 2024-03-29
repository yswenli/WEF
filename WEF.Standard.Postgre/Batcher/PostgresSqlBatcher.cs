﻿/****************************************************************************
*项目名称：WEF.Batcher
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Batcher
*类 名 称：PostgresSqlBatcher
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/9/14 14:59:58
*描述：
*=====================================================================
*修改时间：2020/9/14 14:59:58
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using Npgsql;

using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace WEF.Batcher
{
    /// <summary>
    /// PostgresSqlBatcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PostgresSqlBatcher<T> : BatcherBase<T>, IBatcher<T> where T : Entity
    {
        /// <summary>
        /// PostgresSqlBatcher
        /// </summary>
        /// <param name="database"></param>
        public PostgresSqlBatcher(WEF.Db.Database database) : base(database)
        {

        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="t"></param>
        public override void Insert(T t)
        {
            _list.Add(t);
        }

        /// <summary>
        /// 插入实体集合
        /// </summary>
        /// <param name="data"></param>
        public override void Insert(IEnumerable<T> data)
        {
            _list.AddRange(data);
        }


        /// <summary>
        /// 批量执行
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="timeout"></param>
        public override void Execute(int batchSize = 10000, int timeout = 10 * 1000)
        {
            lock (_locker)
            {
                _dataTable = ToDataTable(_list);
                Execute(_dataTable);
            }
        }

        /// <summary>
        /// 批量执行
        /// </summary>
        /// <param name="dataTable"></param>
        public override void Execute(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0) return;

            var newConnection = (NpgsqlConnection)_database.CreateConnection();

            try
            {
                var commandFormat = string.Format(CultureInfo.InvariantCulture, "COPY {0} FROM STDIN BINARY", dataTable.TableName);

                using (var writer = newConnection.BeginBinaryImport(commandFormat))
                {
                    foreach (DataRow item in dataTable.Rows)
                        writer.WriteRow(item.ItemArray);
                }
            }
            finally
            {
                if (newConnection.State == ConnectionState.Open)
                    newConnection.Close();
                dataTable?.Clear();
                _list.Clear();
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            Execute();
        }
    }
}
