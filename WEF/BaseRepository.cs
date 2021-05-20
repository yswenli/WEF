/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF
*文件名： BaseRepository
*版本号： V1.0.0.0
*唯一标识：65742572-f2c9-4ae0-945f-ac4a46742f4a
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2021/5/20 15:17:53
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/5/20 15:17:53
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

using WEF.Common;
using WEF.MvcPager;
using WEF.Section;

namespace WEF
{
    /// <summary>
    /// Repository基础类，具体业务可以继承此类，或直接使用此类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : IRepository<T> where T : Entity
    {
        protected DBContext _db;

        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseRepository()
        {
            _db = new DBContext();
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseRepository(DBContext dbContext)
        {
            _db = dbContext;
        }
        /// <summary>
        /// 构造方法
        /// <param name="connStrName">连接字符串中的名称</param>
        /// </summary>
        public BaseRepository(string connStrName)
        {
            _db = new DBContext(connStrName);
        }
        /// <summary>
        /// 构造方法
        /// <param name="dbType">数据库类型</param>
        /// <param name="connStr">连接字符串</param>
        /// </summary>
        public BaseRepository(DatabaseType dbType, string connStr)
        {
            _db = new DBContext(dbType, connStr);
        }
        /// <summary>
        /// 当前db操作上下文
        /// </summary>
        public DBContext DBContext
        {
            get
            {
                return _db;
            }
        }
        /// <summary>
        /// 总数
        /// </summary>
        /// <returns></returns>
        public int Total
        {
            get
            {
                return Search().Count();
            }
        }
        /// <summary>
        /// 当前实体查询上下文
        /// </summary>
        public ISearch<T> Search(string tableName = "")
        {
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = TableAttribute.GetTableName<T>();
            }
            return _db.Search<T>(tableName);
        }
        /// <summary>
        /// 当前实体查询上下文
        /// </summary>
        public ISearch<T> Search(T entity)
        {
            return _db.Search<T>(entity);
        }

        /// <summary>
        /// 获取列表
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// </summary>
        /// <returns></returns>
        public List<T> GetList(int pageIndex, int pageSize)
        {
            return this.Search().Page(pageIndex, pageSize).ToList();
        }
        /// <summary>
        /// 获取列表
        /// <param name="tableName">表名</param>
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// </summary>
        /// <returns></returns>
        public List<T> GetList(string tableName, int pageIndex = 1, int pageSize = 12)
        {
            return this.Search(tableName).Page(pageIndex, pageSize).ToList();
        }
        /// <summary>
        /// 分页查询
        /// <param name="lambdaWhere">查询表达式</param>
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// <param name="orderBy">排序</param>
        /// <param name="asc">升降</param>
        /// </summary>
        /// <returns></returns>
        public PagedList<T> GetPagedList(Expression<Func<T, bool>> lambdaWhere, string tableName = "", int pageIndex = 1, int pageSize = 12, string orderBy = "ID", bool asc = true)
        {
            return this.Search(tableName).GetPagedList(lambdaWhere, pageIndex, pageSize, orderBy, asc);
        }
        /// <summary>
        /// 添加实体
        /// <param name="entity">传进的实体</param>
        /// </summary>
        public int Insert(T entity)
        {
            return _db.Insert(entity);
        }
        /// <summary>
        /// 批量添加实体
        /// <param name="entities">传进的实体列表</param>
        /// </summary>
        public void BulkInsert(IEnumerable<T> entities)
        {
            _db.BulkInsert(entities);
        }
        /// <summary>
        /// 更新实体
        /// <param name="entity">传进的实体</param>
        /// </summary>
        public int Update(T entity)
        {
            return _db.Update(entity);
        }
        /// <summary>
        /// 更新实体
        /// <param name="entities">传进的实体</param>
        /// </summary>
        public int Update(IEnumerable<T> entities)
        {
            return _db.Update(entities);
        }
        /// <summary>
        /// 删除实体
        /// <param name="entity">传进的实体</param>
        /// </summary>
        public int Delete(T entity)
        {
            return _db.Delete(entity);
        }
        /// <summary>
        /// 批量删除实体
        /// <param name="obj">传进的实体列表</param>
        /// </summary>
        public int Deletes(List<T> entities)
        {
            return _db.Delete(entities);
        }
        /// <summary>
        /// 持久化实体
        /// <param name="entity">传进的实体</param>
        /// </summary>
        public int Save(T entity)
        {
            return _db.Save(entity);
        }
        /// <summary>
        /// 批量持久化实体
        /// <param name="entities">传进的实体列表</param>
        /// </summary>
        public int Save(List<T> entities)
        {
            return _db.Save(entities);
        }
        /// <summary>
        /// 持久化实体
        /// <param name="tran">事务</param>
        /// <param name="entity">传进的实体</param>
        /// </summary>
        public int Save(DbTransaction tran, T entity)
        {
            return _db.Save(tran, entity);
        }
        /// <summary>
        /// 批量持久化实体
        /// <param name="tran">事务</param>
        /// <param name="entity">传进的实体列表</param>
        /// </summary>
        public int Save(DbTransaction tran, List<T> entities)
        {
            return _db.Save(tran, entities);
        }
        /// <summary>
        /// 执行sql语句
        /// <param name="sql"></param>
        /// </summary>
        public SqlSection FromSql(string sql)
        {
            return _db.FromSql(sql);
        }
        /// <summary>
        /// 执行存储过程
        /// <param name="procName"></param>
        /// </summary>
        public ProcSection FromProc(string procName)
        {
            return _db.FromProc(procName);
        }
    }
}
