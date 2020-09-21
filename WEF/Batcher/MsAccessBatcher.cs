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

namespace WEF.Batcher
{
    /// <summary>
    /// MsAccessBatcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MsAccessBatcher<T> : BatcherBase<T>, IBatcher<T> where T : Entity
    {
        /// <summary>
        /// MsAccessBatcher
        /// </summary>
        /// <param name="database"></param>
        public MsAccessBatcher(WEF.Db.Database database) : base(database)
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
            DBEngine dbEngine = new DBEngine();

            Database db = null;

            try
            {
                db = dbEngine.OpenDatabase(_database.DbProvider.ConnectionString);

                _dataTable = ToDataTable(_list);

                if (_dataTable == null || _dataTable.Rows.Count == 0) return;

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
                db?.Close();
                _list.Clear();
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            this.Execute();
        }
    }
}
