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
        /// ToDataTable
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public new DataTable ToDataTable<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            if (entities == null || !entities.Any()) return null;

            var first = entities.First();

            var fields = first.GetFields();

            var tableName = first.GetTableName();

            var dt = _database.GetMap(tableName);

            var maxId = 0;

            var autoIncrementName = "";

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].AutoIncrement)
                {
                    autoIncrementName = dt.Columns[i].ColumnName;
                }
            }

            if (!string.IsNullOrEmpty(autoIncrementName))
            {
                maxId = _database.GetMaxId(tableName, autoIncrementName);
            }

            foreach (TEntity entity in entities)
            {
                DataRow dtRow = dt.NewRow();

                object[] values = entity.GetValues();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].AutoIncrement)
                    {
                        maxId++;
                        dtRow[fields[i].Name] = maxId;
                    }
                    else
                    {
                        if (dt.Columns[i].AllowDBNull)
                        {
                            if (values[i] == null)
                                continue;
                        }
                        if (values[i] != null && values[i].GetType().Name == "DateTime")
                        {
                            dtRow[fields[i].Name] = new MySqlDateTime(values[i].ToString());
                        }
                        else
                            dtRow[fields[i].Name] = values[i];

                    }

                }
                dt.Rows.Add(dtRow);
            }
            return dt;
        }



        /// <summary>
        /// 批量执行
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="timeout"></param>
        public override void Execute(int batchSize = 10000, int timeout = 10 * 1000)
        {
            MySqlConnection newConnection = (MySqlConnection)_database.CreateConnection();

            try
            {
                _dataTable = ToDataTable(_list);

                if (_dataTable == null || _dataTable.Rows.Count == 0) return;

                string tmpPath = Path.Combine(Directory.GetCurrentDirectory(), _dataTable.TableName + ".csv");

                DBContext.WriteToCSV(_dataTable, tmpPath, false);

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
                        TableName = _dataTable.TableName,
                        CharacterSet = "utf8"
                    };
                    try
                    {
                        bulk.Columns.AddRange(_dataTable.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList());
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
