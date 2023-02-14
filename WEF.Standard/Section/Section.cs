/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
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
using WEF.Expressions;
using WEF.MvcPager;

namespace WEF.Section
{

    /// <summary>
    /// Section
    /// </summary>
    public abstract class Section
    {
        protected DBContext _dbContext;
        protected DbCommand _dbCommand;
        protected DbCommand _dbCountCommand;
        protected DbTransaction _dbTransaction = null;

        int _pageIndex = 1;
        int _pageSize = 100;

        /// <summary>
        /// Section
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public Section(DBContext dbContext, string sql, int pageIndex = 1, int pageSize = 100)
        {
            Check.Require(dbContext, "dbContext", Check.NotNullOrEmpty);
            _dbContext = dbContext;
            _dbCountCommand = dbContext.Db.GetSqlStringCommand($"SELECT COUNT(1) FROM ({sql}) AS _WEF_TEMP_");
            _pageIndex = pageIndex;
            _pageSize = pageSize;
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

        /// <summary>
        /// 数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            var obj = (_dbTransaction == null ? this._dbContext.ExecuteScalar(_dbCountCommand) : this._dbContext.ExecuteScalar(_dbCountCommand, _dbTransaction));
            return DataUtils.ConvertValue<int>(obj);
        }
        #endregion

        #region 返回模型

        /// <summary>
        /// 返回第一个实体，同ToFirst()。无数据返回Null。
        /// </summary>
        /// <returns></returns>
        public TModel First<TModel>()
        {
            return ToFirst<TModel>();
        }
        /// <summary>
        /// 返回单个实体
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public TModel ToFirst<TModel>()
        {
            using (IDataReader reader = ToDataReader())
            {
                var list = reader.ReaderToList<TModel>();
                if (list != null)
                {
                    return list.FirstOrDefault();
                }
                return default;
            }
        }

        /// <summary>
        /// 返回单个实体
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public TModel ToFirstDefault<TModel>()
            where TModel : Entity
        {
            TModel t = ToFirst<TModel>();

            if (t == null)
                t = DataUtils.Create<TModel>();
            return t;
        }
        /// <summary>
        /// 返回实体列表
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public List<TModel> ToList<TModel>()
        {
            using (IDataReader reader = ToDataReader())
            {
                var list = reader.ReaderToList<TModel>();
                if (list != null)
                    return list;
                else
                    return null;
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
                var list1 = reader.ReaderToList<T1>();
                reader.NextResult();
                var list2 = reader.ReaderToList<T2>();
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
                var list1 = reader.ReaderToList<T1>();
                reader.NextResult();
                var list2 = reader.ReaderToList<T2>();
                reader.NextResult();
                var list3 = reader.ReaderToList<T3>();
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
                var list1 = reader.ReaderToList<T1>();
                reader.NextResult();
                var list2 = reader.ReaderToList<T2>();
                reader.NextResult();
                var list3 = reader.ReaderToList<T3>();
                reader.NextResult();
                var list4 = reader.ReaderToList<T4>();
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
                var list1 = reader.ReaderToList<T1>();
                reader.NextResult();
                var list2 = reader.ReaderToList<T2>();
                reader.NextResult();
                var list3 = reader.ReaderToList<T3>();
                reader.NextResult();
                var list4 = reader.ReaderToList<T4>();
                reader.NextResult();
                var list5 = reader.ReaderToList<T5>();
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
                var list1 = reader.ReaderToList<T1>();
                reader.NextResult();
                var list2 = reader.ReaderToList<T2>();
                reader.NextResult();
                var list3 = reader.ReaderToList<T3>();
                reader.NextResult();
                var list4 = reader.ReaderToList<T4>();
                reader.NextResult();
                var list5 = reader.ReaderToList<T5>();
                reader.NextResult();
                var list6 = reader.ReaderToList<T6>();
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
                    keyValuePairs[type.FullName] = reader.ReaderToList(type);
                    reader.NextResult();
                }
            }
            return keyValuePairs;
        }

        #endregion



        #region 获取分页结果

        /// <summary>
        /// 获取分页结果
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public PagedList<TModel> ToPagedList<TModel>()
        {
            var total = Count();

            return new PagedList<TModel>(ToList<TModel>(), _pageIndex, _pageSize, total);
        }

        /// <summary>
        /// 获取分页结果
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedList<TModel> ToPagedList<TModel>(int pageIndex, int pageSize)
        {
            var total = Count();

            return new PagedList<TModel>(ToList<TModel>(), pageIndex, pageSize, total);
        }

        #endregion
    }
}

