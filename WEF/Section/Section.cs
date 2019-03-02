/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/
using WEF.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using WEF.Db;

namespace WEF.Section
{

    /// <summary>
    /// Section
    /// </summary>
    public abstract class Section
    {
        protected DBContext dbSession;
        protected DbCommand cmd;
        protected DbTransaction tran = null;

        public Section(DBContext dbSession)
        {
            Check.Require(dbSession, "dbSession", Check.NotNullOrEmpty);
            this.dbSession = dbSession;
        }

        #region 执行

        /// <summary>
        /// 返回单个值
        /// </summary>
        /// <returns></returns>
        public virtual object ToScalar()
        {
            return (tran == null ? this.dbSession.ExecuteScalar(cmd) : this.dbSession.ExecuteScalar(cmd, tran));
        }


        /// <summary>
        /// 返回单个值
        /// </summary>
        /// <returns></returns>
        public TResult ToScalar<TResult>()
        {
            return DataUtils.ConvertValue<TResult>(ToScalar());
        }

        /// <summary>
        /// 返回第一个实体，同ToFirst()。无数据返回Null。
        /// </summary>
        /// <returns></returns>
        public TEntity First<TEntity>()
        {
            return ToFirst<TEntity>();
        }
        /// <summary>
        /// 返回单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TEntity ToFirst<TEntity>()
        {
            TEntity t = default(TEntity);
            using (IDataReader reader = ToDataReader())
            {
                //var tempt = EntityUtils.Mapper.Map<TEntity>(reader);
                //if (tempt.Any())
                //{
                //    t = tempt.First();
                //}
                var result = EntityUtils.ReaderToEnumerable<TEntity>(reader).ToArray();
                if (result.Any())
                {
                    t = result.First();
                }
                #region 2015-08-10注释
                //if (reader.Read())
                //{
                //    t = DataUtils.Create<TEntity>();
                //    t.SetPropertyValues(reader);
                //}
                #endregion
            }
            return t;
        }

        /// <summary>
        /// 返回单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TEntity ToFirstDefault<TEntity>()
            where TEntity : Entity
        {
            TEntity t = ToFirst<TEntity>();

            if (t == null)
                t = DataUtils.Create<TEntity>();

            return t;
        }
        /// <summary>
        /// 返回实体列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public List<TEntity> ToList<TEntity>()
        {
            //List<TEntity> listT = new List<TEntity>();
            using (IDataReader reader = ToDataReader())
            {
                //listT = EntityUtils.Mapper.Map<TEntity>(reader);
                //reader.Close();
                return EntityUtils.ReaderToEnumerable<TEntity>(reader).ToList();
            }
            //return listT;
        }
        /// <summary>
        /// 返回懒加载数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> ToEnumerable<TEntity>()
        {
            IEnumerable<TEntity> result;
            using (IDataReader reader = ToDataReader())
            {
                var info = new EntityUtils.CacheInfo()
                {
                    Deserializer = EntityUtils.GetDeserializer(typeof(TEntity), reader, 0, -1, false)
                };
                while (reader.Read())
                {
                    dynamic next = info.Deserializer(reader);
                    yield return (TEntity)next;
                }
            }
        }

        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <returns></returns>
        public virtual IDataReader ToDataReader()
        {
            return (tran == null ? this.dbSession.ExecuteReader(cmd) : this.dbSession.ExecuteReader(cmd, tran));
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <returns></returns>
        public virtual DataSet ToDataSet()
        {
            return (tran == null ? this.dbSession.ExecuteDataSet(cmd) : this.dbSession.ExecuteDataSet(cmd, tran));
        }


        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            return this.ToDataSet().Tables[0];
        }

        /// <summary>
        /// 执行ExecuteNonQuery
        /// </summary>
        /// <returns></returns>
        public virtual int ExecuteNonQuery()
        {
            return (tran == null ? this.dbSession.ExecuteNonQuery(cmd) : this.dbSession.ExecuteNonQuery(cmd, tran));
        }


        #endregion

    }
}

