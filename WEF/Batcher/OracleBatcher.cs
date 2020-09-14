/****************************************************************************
*项目名称：WEF.Batcher
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Batcher
*类 名 称：OracleBatcher
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/9/14 13:59:32
*描述：
*=====================================================================
*修改时间：2020/9/14 13:59:32
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using WEF.Provider;

namespace WEF.Batcher
{
    /// <summary>
    /// OracleBatcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OracleBatcher<T> : IBatcher<T> where T : Entity
    {

        List<T> _list;

        OracleProvider _oracleProvider;

        DataTable _dataTable;

        /// <summary>
        /// MsSqlBatcher
        /// </summary>
        public OracleBatcher(OracleProvider oracleProvider)
        {
            _list = new List<T>();

            _oracleProvider = oracleProvider;
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
            OracleConnection newConnection = (OracleConnection)_oracleProvider.DbProviderFactory.CreateConnection();

            try
            {
                _dataTable = _list.EntitiesToDataTable();

                if (_dataTable == null) return;

                var sbc = new OracleBulkCopy(newConnection);

                using (sbc)
                {
                    sbc.BatchSize = batchSize;

                    sbc.DestinationTableName = _dataTable.TableName;

                    sbc.BulkCopyTimeout = timeout;

                    if (newConnection.State != ConnectionState.Open)
                    {
                        newConnection.Open();
                    }

                    sbc.WriteToServer(_dataTable);
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
