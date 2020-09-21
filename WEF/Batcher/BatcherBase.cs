/****************************************************************************
*项目名称：WEF.Batcher
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Batcher
*类 名 称：BatcherBase
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/9/21 21:16:33
*描述：
*=====================================================================
*修改时间：2020/9/21 21:16:33
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WEF.Db;

namespace WEF.Batcher
{
    /// <summary>
    /// BatcherBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BatcherBase<T> : IBatcher<T> where T : Entity
    {
        protected List<T> _list;

        protected Database _database;

        protected DataTable _dataTable;

        /// <summary>
        /// BatcherBase
        /// </summary>
        /// <param name="database"></param>
        public BatcherBase(Database database)
        {
            _database = database;
            _list = new List<T>();
        }

        /// <summary>
        /// ToDataTable
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public DataTable ToDataTable<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            if (entities == null || !entities.Any()) return null;

            var first = entities.First();

            var fields = first.GetFields();

            DataTable dt = _database.GetMap(first.GetTableName());

            foreach (TEntity entity in entities)
            {
                DataRow dtRow = dt.NewRow();

                object[] values = entity.GetValues();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!dt.Columns[i].AutoIncrement)
                    {
                        if (dt.Columns[i].AllowDBNull)
                        {
                            if (values[i] == null)
                                continue;
                        }
                        dtRow[fields[i].Name] = values[i];
                    }
                }
                dt.Rows.Add(dtRow);
            }
            return dt;
        }

        public abstract void Dispose();

        public abstract void Execute(int batchSize = 10000, int timeout = 10000);

        public abstract void Insert(T t);

        public abstract void Insert(IEnumerable<T> data);
    }
}
