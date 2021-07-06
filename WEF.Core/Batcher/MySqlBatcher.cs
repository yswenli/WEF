/****************************************************************************
*项目名称：WEF.Batcher
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Batcher
*类 名 称：MySqlBatcher
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/9/14 14:08:01
*描述：
*=====================================================================
*修改时间：2020/9/14 14:08:01
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace WEF.Batcher
{
    /// <summary>
    /// MySqlBatcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MySqlBatcher<T> : BatcherBase<T>, IBatcher<T> where T : Entity
    {

        /// <summary>
        /// MySqlBatcher
        /// </summary>
        /// <param name="database"></param>
        public MySqlBatcher(WEF.Db.Database database) : base(database)
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
            _dataTable = ToDataTable(_list);
            Execute(_dataTable);
        }

        /// <summary>
        /// 批量执行
        /// </summary>
        /// <param name="dataTable"></param>
        public override void Execute(DataTable dataTable)
        {
            MySqlConnection newConnection = (MySqlConnection)_database.CreateConnection();

            try
            {
                if (dataTable == null || dataTable.Rows.Count == 0) return;

                string tmpPath = Path.Combine(Path.GetTempPath(), dataTable.TableName + ".csv");

                DBContext.WriteToCSV(dataTable, tmpPath, false);

                newConnection.Open();

                using (MySqlTransaction tran = newConnection.BeginTransaction())
                {
                    MySqlBulkLoader bulk = new MySqlBulkLoader(newConnection)
                    {
                        FieldTerminator = ",",
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        LineTerminator = "\r\n",
                        FileName = tmpPath,
                        Local = true,
                        NumberOfLinesToSkip = 0,
                        TableName = dataTable.TableName,
                        CharacterSet = "utf8"
                    };
                    try
                    {
                        bulk.Columns.AddRange(dataTable.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList());
                        var size = bulk.Load();
                        tran.Commit();
                    }
                    catch (MySqlException ex)
                    {
                        if (tran != null)
                            tran.Rollback();

                        throw ex;
                    }
                    finally
                    {
                        File.Delete(tmpPath);
                    }
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
