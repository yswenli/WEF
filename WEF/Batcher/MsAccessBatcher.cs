/****************************************************************************
*项目名称：WEF.Batcher
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Batcher
*类 名 称：MsAccessBatcher
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/9/14 14:31:05
*描述：
*=====================================================================
*修改时间：2020/9/14 14:31:05
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using Microsoft.Office.Interop.Access.Dao;
using System;
using System.Collections.Generic;
using System.Data;
using WEF.Provider;

namespace WEF.Batcher
{
    /// <summary>
    /// MsAccessBatcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MsAccessBatcher<T> : IBatcher<T> where T : Entity
    {

        List<T> _list;

        MsAccessProvider _msAccessProvider;

        DataTable _dataTable;

        /// <summary>
        /// MsAccessBatcher
        /// </summary>
        public MsAccessBatcher(MsAccessProvider msAccessProvider)
        {
            _list = new List<T>();

            _msAccessProvider = msAccessProvider;
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
            DBEngine dbEngine = new DBEngine();

            Database db = dbEngine.OpenDatabase(_msAccessProvider.ConnectionString);

            try
            {
                _dataTable = _list.EntitiesToDataTable();

                if (_dataTable == null) return;

                Recordset rs = db.OpenRecordset(_dataTable.TableName);

                var columns = _dataTable.Columns;

                Field[] myFields = new Field[columns.Count];

                for (int i = 0; i < columns.Count; i++)
                {
                    myFields[i] = rs.Fields[columns[i].ColumnName];
                }

                for (int i = 0; i < _dataTable.Rows.Count; i++)
                {
                    rs.AddNew();

                    for (int j = 0; j < columns.Count; j++)
                    {
                        myFields[0].Value = _dataTable.Rows[i][j];
                    }
                    rs.Update();
                }
                rs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.Close();
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
