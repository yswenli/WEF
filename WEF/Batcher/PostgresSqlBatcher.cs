/****************************************************************************
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using WEF.Provider;

namespace WEF.Batcher
{
    /// <summary>
    /// PostgresSqlBatcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PostgresSqlBatcher<T> : IBatcher<T> where T : Entity
    {

        List<T> _list;

        PostgreSqlProvider _psProvider;

        DataTable _dataTable;

        /// <summary>
        /// PostgresSqlBatcher
        /// </summary>
        /// <param name="psProvider"></param>
        public PostgresSqlBatcher(PostgreSqlProvider psProvider)
        {
            _list = new List<T>();

            _psProvider = psProvider;
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
            NpgsqlConnection newConnection = (NpgsqlConnection)_psProvider.DbProviderFactory.CreateConnection();

            try
            {
                _dataTable = _list.EntitiesToDataTable();

                if (_dataTable == null) return;

                var commandFormat = string.Format(CultureInfo.InvariantCulture, "COPY {0} FROM STDIN BINARY", _dataTable.TableName);

                newConnection.Open();                

                using (var writer = newConnection.BeginBinaryImport(commandFormat))
                {
                    foreach (DataRow item in _dataTable.Rows)
                        writer.WriteRow(item.ItemArray);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (newConnection.State == ConnectionState.Open)
                    newConnection.Close();
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _list.Clear();
        }
    }
}
