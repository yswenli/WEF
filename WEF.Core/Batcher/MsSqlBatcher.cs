/****************************************************************************
*项目名称：WEF.Batcher
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Batcher
*类 名 称：MsSqlBatcher
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/9/14 13:32:47
*描述：
*=====================================================================
*修改时间：2020/9/14 13:32:47
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace WEF.Batcher
{
    /// <summary>
    /// MsSqlBatcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MsSqlBatcher<T> : BatcherBase<T>, IBatcher<T> where T : Entity
    {
        /// <summary>
        /// MsSqlBatcher
        /// </summary>
        /// <param name="database"></param>
        public MsSqlBatcher(WEF.Db.Database database) : base(database)
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
            if (_list == null || !_list.Any()) return;

            _dataTable = ToDataTable(_list);

            Execute(_dataTable);
        }


        /// <summary>
        /// 批量执行
        /// </summary>
        /// <param name="dataTable"></param>
        public override void Execute(DataTable dataTable)
        {
            SqlConnection newConnection = (SqlConnection)_database.CreateConnection();

            try
            {
                if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count == 0) return;

                var sbc = new SqlBulkCopy(newConnection);

                using (sbc)
                {
                    sbc.BatchSize = dataTable.Rows.Count;

                    sbc.DestinationTableName = dataTable.TableName;

                    sbc.BulkCopyTimeout = 30*1000;

                    if (newConnection.State != ConnectionState.Open)
                    {
                        newConnection.Open();
                    }

                    sbc.WriteToServer(dataTable);
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
            _list = null;
        }
    }
}
