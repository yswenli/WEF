/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF
*文件名： BaseRepository
*版本号： V1.0.0.0
*唯一标识：565b31a1-4753-4ffb-bbae-f0dc1eae1b38
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2021/5/20 15:11:37
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/5/20 15:11:37
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
using WEF.Db;
using WEF.MvcPager;
using WEF.Section;

namespace WEF
{
    /// <summary>
    /// Repository基础类，具体业务可以继承此类，或直接使用此类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : IRepository<T> where T : Entity, new()
    {
        protected DBContext _db;

        private T _entity;

        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseRepository(DBContext dbContext)
        {
            _db = dbContext;

            _entity = new T();
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseRepository() : this(new DBContext())
        {

        }

        /// <summary>
        /// 构造方法
        /// <param name="connStrName">连接字符串中的名称</param>
        /// </summary>
        public BaseRepository(string connStrName) : this(new DBContext(connStrName))
        {

        }
        /// <summary>
        /// 构造方法
        /// <param name="dbType">数据库类型</param>
        /// <param name="connStr">连接字符串</param>
        /// </summary>
        public BaseRepository(DatabaseType dbType, string connStr) : this(new DBContext(dbType, connStr))
        {

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
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(string id)
        {
            return _db.FromSql($"select * from {_entity.GetTableName()} where {_entity.GetIdentityField().Name}='{id}'").ToFirst<T>();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(int id)
        {
            return _db.FromSql($"select * from {_entity.GetTableName()} where {_entity.GetIdentityField().Name}={id}").ToFirst<T>();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<T> GetList(IEnumerable<string> ids)
        {
            var idsStr = $"'{string.Join("','", System.Linq.Enumerable.ToArray(ids))}'";
            return _db.FromSql($"select * from {_entity.GetTableName()} where {_entity.GetIdentityField().Name} in({idsStr})").ToList<T>();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<T> GetList(IEnumerable<int> ids)
        {
            var idsStr = $"'{string.Join("','", System.Linq.Enumerable.ToArray(ids))}'";
            return _db.FromSql($"select * from {_entity.GetTableName()} where {_entity.GetIdentityField().Name} in({idsStr})").ToList<T>();
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
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<T> GetPagedList(int pageIndex, int pageSize, string orderBy = "ID", bool asc = true)
        {
            return Search().ToPagedList(pageIndex, pageSize, orderBy, asc);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<T> GetPagedList(Expression<Func<T, bool>> lambdaWhere, int pageIndex = 1, int pageSize = 12, string orderBy = "ID", bool asc = true)
        {
            return Search().ToPagedList(lambdaWhere, pageIndex, pageSize, orderBy, asc);
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
        public PagedList<T> GetPagedList(Expression<Func<T, bool>> lambdaWhere, string tableName, int pageIndex = 1, int pageSize = 12, string orderBy = "ID", bool asc = true)
        {
            return Search(tableName).ToPagedList(lambdaWhere, pageIndex, pageSize, orderBy, asc);
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
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update(T entity, Expression<Func<T, bool>> lambdaWhere)
        {
            return _db.Update<T>(entity, lambdaWhere);
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
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(string id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                return Delete(entity);
            }
            return -1;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                return Delete(entity);
            }
            return -1;
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Deletes(IEnumerable<T> entities)
        {
            var list = System.Linq.Enumerable.ToList(entities);
            return _db.Delete(list);
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int Delete(IEnumerable<string> ids)
        {
            var list = GetList(ids);
            return Deletes(list);
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int Delete(IEnumerable<int> ids)
        {
            var list = GetList(ids);
            return Deletes(list);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Delete(Expression<Func<T, bool>> lambdaWhere)
        {
            return _db.Delete<T>(lambdaWhere);
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
        /// <param name="entities">传进的实体列表</param>
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
        /// 执行sql语句,带参数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="inputParamas"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql, Dictionary<string, object> inputParamas)
        {
            return _db.FromSqlWithdAutomatic(sql, inputParamas.ToArray());
        }
        /// <summary>
        /// 执行sql语句,带参数
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="sql"></param>
        /// <param name="inputParamas"></param>
        /// <returns></returns>
        public SqlSection FromSql<Model>(string sql, Model inputParamas) where Model : class, new()
        {
            return _db.FromSqlWithdModel(sql, inputParamas);
        }

        /// <summary>
        /// 执行存储过程
        /// <param name="procName"></param>
        /// </summary>
        public ProcSection FromProc(string procName)
        {
            return _db.FromProc(procName);
        }

        /// <summary>
        /// 执行存储过程，带参数
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="inputParamas"></param>
        /// <returns></returns>
        public ProcSection FromProc(string procName, Dictionary<string, object> inputParamas)
        {
            return _db.FromProc(procName, inputParamas);
        }
        /// <summary>
        /// 执行存储过程，带参数
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="procName"></param>
        /// <param name="inputParamas"></param>
        /// <returns></returns>
        public ProcSection FromProc<Model>(string procName, Model inputParamas) where Model : class, new()
        {
            return _db.FromProc(procName, inputParamas);
        }

        /// <summary>
        /// 创建事务，使用事务curd时推荐方式 using(var tran=CreateTransaction()) tran.Commit() 方式
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public DbTrans CreateTransaction(int timeout = 30)
        {
            return _db.BeginTransaction(timeout);
        }
    }
}
