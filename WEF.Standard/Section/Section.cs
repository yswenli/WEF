/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using WEF.Common;

namespace WEF.Section
{

    /// <summary>
    /// Section
    /// </summary>
    public abstract class Section
    {
        protected DBContext _dbContext;
        protected DbCommand _dbCommand;
        protected DbTransaction _dbTransaction = null;

        /// <summary>
        /// Section
        /// </summary>
        /// <param name="dbContext"></param>
        public Section(DBContext dbContext)
        {
            Check.Require(dbContext, "dbContext", Check.NotNullOrEmpty);
            this._dbContext = dbContext;
        }

        #region 执行

        /// <summary>
        /// 返回单个值
        /// </summary>
        /// <returns></returns>
        public virtual object ToScalar()
        {
            return (_dbTransaction == null ? this._dbContext.ExecuteScalar(_dbCommand) : this._dbContext.ExecuteScalar(_dbCommand, _dbTransaction));
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
        /// 返回DataReader
        /// </summary>
        /// <returns></returns>
        public virtual IDataReader ToDataReader()
        {
            return (_dbTransaction == null ? this._dbContext.ExecuteReader(_dbCommand) : this._dbContext.ExecuteReader(_dbCommand, _dbTransaction));
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <returns></returns>
        public virtual DataSet ToDataSet()
        {
            return (_dbTransaction == null ? this._dbContext.ExecuteDataSet(_dbCommand) : this._dbContext.ExecuteDataSet(_dbCommand, _dbTransaction));
        }


        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable()
        {
            return ToDataSet().Tables[0];
        }

        /// <summary>
        /// 执行ExecuteNonQuery
        /// </summary>
        /// <returns></returns>
        public virtual int ExecuteNonQuery()
        {
            return (_dbTransaction == null ? this._dbContext.ExecuteNonQuery(_dbCommand) : this._dbContext.ExecuteNonQuery(_dbCommand, _dbTransaction));
        }
        #endregion

        #region 返回模型

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
            using (IDataReader reader = ToDataReader())
            {
                return reader.ReaderToEnumerable<TEntity>().FirstOrDefault();
            }
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
            using (IDataReader reader = ToDataReader())
            {
                return reader.ReaderToEnumerable<TEntity>().ToList();
            }
        }
        /// <summary>
        /// 返回懒加载数据
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> ToEnumerable<TEntity>()
        {
            using (IDataReader reader = ToDataReader())
            {
                return reader.ReaderToEnumerable<TEntity>();
            }
        }

        /// <summary>
        /// 返回多集合列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public Tuple<List<T1>, List<T2>> ToMultipleList<T1, T2>()
        {
            using (IDataReader reader = ToDataReader())
            {
                var list1 = reader.ReaderToEnumerable<T1>().ToList();
                reader.NextResult();
                var list2 = reader.ReaderToEnumerable<T2>().ToList();
                return new Tuple<List<T1>, List<T2>>(list1, list2);
            }
        }
        /// <summary>
        /// 返回多集合列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <returns></returns>
        public Tuple<List<T1>, List<T2>, List<T3>> ToMultipleList<T1, T2, T3>()
        {
            using (IDataReader reader = ToDataReader())
            {
                var list1 = reader.ReaderToEnumerable<T1>().ToList();
                reader.NextResult();
                var list2 = reader.ReaderToEnumerable<T2>().ToList();
                reader.NextResult();
                var list3 = reader.ReaderToEnumerable<T3>().ToList();
                return new Tuple<List<T1>, List<T2>, List<T3>>(list1, list2, list3);
            }
        }
        /// <summary>
        /// 返回多集合列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <returns></returns>
        public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> ToMultipleList<T1, T2, T3, T4>()
        {
            using (IDataReader reader = ToDataReader())
            {
                var list1 = reader.ReaderToEnumerable<T1>().ToList();
                reader.NextResult();
                var list2 = reader.ReaderToEnumerable<T2>().ToList();
                reader.NextResult();
                var list3 = reader.ReaderToEnumerable<T3>().ToList();
                reader.NextResult();
                var list4 = reader.ReaderToEnumerable<T4>().ToList();
                return new Tuple<List<T1>, List<T2>, List<T3>, List<T4>>(list1, list2, list3, list4);
            }
        }
        /// <summary>
        /// 返回多集合列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <returns></returns>
        public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> ToMultipleList<T1, T2, T3, T4, T5>()
        {
            using (IDataReader reader = ToDataReader())
            {
                var list1 = reader.ReaderToEnumerable<T1>().ToList();
                reader.NextResult();
                var list2 = reader.ReaderToEnumerable<T2>().ToList();
                reader.NextResult();
                var list3 = reader.ReaderToEnumerable<T3>().ToList();
                reader.NextResult();
                var list4 = reader.ReaderToEnumerable<T4>().ToList();
                reader.NextResult();
                var list5 = reader.ReaderToEnumerable<T5>().ToList();
                return new Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>>(list1, list2, list3, list4, list5);
            }
        }
        /// <summary>
        /// 返回多集合列表
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <returns></returns>
        public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> ToMultipleList<T1, T2, T3, T4, T5, T6>()
        {
            using (IDataReader reader = ToDataReader())
            {
                var list1 = reader.ReaderToEnumerable<T1>().ToList();
                reader.NextResult();
                var list2 = reader.ReaderToEnumerable<T2>().ToList();
                reader.NextResult();
                var list3 = reader.ReaderToEnumerable<T3>().ToList();
                reader.NextResult();
                var list4 = reader.ReaderToEnumerable<T4>().ToList();
                reader.NextResult();
                var list5 = reader.ReaderToEnumerable<T5>().ToList();
                reader.NextResult();
                var list6 = reader.ReaderToEnumerable<T6>().ToList();
                return new Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>>(list1, list2, list3, list4, list5, list6);
            }
        }

        /// <summary>
        /// 返回多集合列表
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public Dictionary<string, List<dynamic>> ToMultipleList(params Type[] types)
        {
            if (types == null || types.Length < 1) return null;
            Dictionary<string, List<dynamic>> keyValuePairs = new Dictionary<string, List<dynamic>>();
            using (IDataReader reader = ToDataReader())
            {
                foreach (var type in types)
                {
                    var list = reader.ReaderToEnumerable(type).ToList();
                    keyValuePairs[type.FullName] = list;
                    reader.NextResult();
                }
            }
            return keyValuePairs;
        }

        #endregion

    }
}

