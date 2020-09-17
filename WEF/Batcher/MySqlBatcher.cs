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
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using WEF.Provider;

namespace WEF.Batcher
{
    /// <summary>
    /// MySqlBatcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MySqlBatcher<T> : IBatcher<T> where T : Entity
    {

        List<T> _list;

        DbProvider _mysqlProvider;

        DataTable _dataTable;

        /// <summary>
        /// MySqlBatcher
        /// </summary>
        /// <param name="mysqlProvider"></param>
        public MySqlBatcher(DbProvider mysqlProvider)
        {
            _list = new List<T>();

            _mysqlProvider = mysqlProvider;
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="t"></param>
        public void Insert(T t)
        {
            _list.Add(t);
        }

        /// <summary>
        /// 插入实体集合
        /// </summary>
        /// <param name="data"></param>
        public void Insert(IEnumerable<T> data)
        {
            _list.AddRange(data);
        }


        /// <summary>
        /// 批量执行
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="timeout"></param>
        public void Execute(int batchSize = 10000, int timeout = 10 * 1000)
        {
            MySqlConnection newConnection = (MySqlConnection)_mysqlProvider.DbProviderFactory.CreateConnection();

            newConnection.ConnectionString = _mysqlProvider.ConnectionString;

            try
            {
                _dataTable = _list.EntitiesToDataTable();

                if (_dataTable == null || _dataTable.Rows.Count == 0) return;

                string tmpPath = Path.Combine(Directory.GetCurrentDirectory(), _dataTable.TableName + ".csv");

                DBContext.WriteToCSV(_dataTable, tmpPath);

                MySqlBulkLoader bulk = new MySqlBulkLoader(newConnection)
                {
                    FieldTerminator = ",",
                    FieldQuotationCharacter = '"',
                    EscapeCharacter = '"',
                    LineTerminator = "\r\n",
                    FileName = tmpPath,
                    NumberOfLinesToSkip = 0,
                    TableName = _dataTable.TableName,
                };

                var columns = _dataTable.Columns.Cast<DataColumn>().Select(_columns => _columns.ColumnName).ToList();

                if (newConnection.State != ConnectionState.Open)
                {
                    newConnection.Open();
                }

                bulk.Columns.AddRange(columns);
                bulk.Load();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (newConnection.State == ConnectionState.Open)
                    newConnection.Close();
                _list.Clear();
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Execute();
        }
    }
}
