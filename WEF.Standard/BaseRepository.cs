﻿/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF
*文件名： BaseRepository
*版本号： V1.0.0.0
*唯一标识：565b31a1-4753-4ffb-bbae-f0dc1eae1b38
*当前的用户域：OCEANIA
*创建人： Walle.Wen
*电子邮箱：Walle.Wen@oceania-inc.com
*创建时间：2019/5/20 15:11:37
*描述：
*
*=====================================================================
*修改标记
*修改时间：2022/9/14 15:11:37
*修改人： Walle.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using WEF.Common;
using WEF.Db;
using WEF.Expressions;
using WEF.MvcPager;
using WEF.Section;

namespace WEF
{
    /// <summary>
    /// Repository基础类，具体业务可以继承此类，或直接使用此类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : IDisposable
        where T : Entity, new()
    {
        /// <summary>
        /// DBContext
        /// </summary>
        protected DBContext _dbContext;

        private T _entity;

        private Field _primaryKey;

        private string _tableName;

        /// <summary>
        /// Repository基础类，具体业务可以继承此类，或直接使用此类
        /// </summary>
        public BaseRepository(DBContext dbContext) : base()
        {
            _dbContext = dbContext;

            _entity = new T();

            _primaryKey = _entity.GetPrimaryKeyFields().First();

            if (_primaryKey == null) throw new Exception($"表{_entity.GetTableName()}中缺失主键！");

            _tableName = TableAttribute.GetTableName<T>();
        }

        /// <summary>
        /// Repository基础类，具体业务可以继承此类，或直接使用此类
        /// </summary>
        public BaseRepository() : this(new DBContext())
        {

        }

        /// <summary>
        /// Repository基础类，具体业务可以继承此类，或直接使用此类
        /// <param name="connStrName">连接字符串中的名称</param>
        /// </summary>
        public BaseRepository(string connStrName) : this(new DBContext(connStrName))
        {

        }

        /// <summary>
        /// Repository基础类，具体业务可以继承此类，或直接使用此类
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
                return _dbContext;
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
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Search<T> Search(string tableName = "")
        {
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = _tableName;
            }
            return _dbContext.Search<T>(tableName);
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(string id)
        {
            return _dbContext.FromSql($"select * from {_tableName} where {_primaryKey.Name}=@{_primaryKey.Name}")
                .AddInParameter($"@{_primaryKey.Name}", id)
                .First<T>();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string id)
        {
            return await _dbContext.FromSql($"select * from {_tableName} where {_primaryKey.Name}=@{_primaryKey.Name}")
                .AddInParameter($"@{_primaryKey.Name}", id)
                .FirstAsync<T>();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(int id)
        {
            return _dbContext.FromSql($"select * from {_tableName} where {_primaryKey.Name}=@{_primaryKey.Name}")
                .AddInParameter($"@{_primaryKey.Name}", id)
                .First<T>();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(int id)
        {
            return await _dbContext.FromSql($"select * from {_tableName} where {_primaryKey.Name}=@{_primaryKey.Name}")
                .AddInParameter($"@{_primaryKey.Name}", id)
                .FirstAsync<T>();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> lambdaWhere)
        {
            return Search().First(lambdaWhere);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(Expression<Func<T, bool>> lambdaWhere)
        {
            return await Search().FirstAsync(lambdaWhere);
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public T Single(Expression<Func<T, bool>> lambdaWhere)
        {
            return Search().Single(lambdaWhere);
        }

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<T> SingleAsync(Expression<Func<T, bool>> lambdaWhere)
        {
            return await Search().SingleAsync(lambdaWhere);
        }

        /// <summary>
        /// 返回全部
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            return Search().ToList();
        }

        /// <summary>
        /// 返回全部
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAllAsync()
        {
            return await Search().ToListAsync();
        }

        /// <summary>
        /// 返回总数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Search().Count();
        }

        /// <summary>
        /// 返回总数
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> lambdaWhere)
        {
            return Search().Where(lambdaWhere).Count();
        }
        /// <summary>
        /// 返回总数
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync()
        {
            return await Search().CountAsync();
        }
        /// <summary>
        /// 返回总数
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> lambdaWhere)
        {
            return await Search().Where(lambdaWhere).CountAsync();
        }

        /// <summary>
        /// 返回总数
        /// </summary>
        /// <returns></returns>
        public long LongCount()
        {
            return Search().LongCount();
        }
        /// <summary>
        /// 返回总数
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public long LongCount(Expression<Func<T, bool>> lambdaWhere)
        {
            return Search().Where(lambdaWhere).LongCount();
        }
        /// <summary>
        /// 返回总数
        /// </summary>
        /// <returns></returns>
        public async Task<long> LongCountAsync()
        {
            return await Search().LongCountAsync();
        }
        /// <summary>
        /// 返回总数
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<long> LongCountAsync(Expression<Func<T, bool>> lambdaWhere)
        {
            return await Search().Where(lambdaWhere).LongCountAsync();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<T> GetList(IEnumerable<string> ids)
        {
            var idsStr = $"'{string.Join("','", ids.ToArray())}'";
            return _dbContext.FromSql($"select * from {_tableName} where {_primaryKey.Name} in({idsStr})").ToList<T>();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(IEnumerable<string> ids)
        {
            var idsStr = $"'{string.Join("','", ids.ToArray())}'";
            return await _dbContext.FromSql($"select * from {_tableName} where {_primaryKey.Name} in({idsStr})").ToListAsync<T>();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<T> GetList(IEnumerable<int> ids)
        {
            var idsStr = $"'{string.Join("','", ids.ToArray())}'";
            return _dbContext.FromSql($"select * from {_tableName} where {_primaryKey.Name} in({idsStr})").ToList<T>();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(IEnumerable<int> ids)
        {
            var idsStr = $"'{string.Join("','", ids.ToArray())}'";
            return await _dbContext.FromSql($"select * from {_tableName} where {_primaryKey.Name} in({idsStr})").ToListAsync<T>();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> lambdaWhere)
        {
            return Search().ToList(lambdaWhere);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> lambdaWhere)
        {
            return await Search().ToListAsync(lambdaWhere);
        }

        /// <summary>
        /// 获取列表
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// </summary>
        /// <returns></returns>
        public List<T> GetList(int pageIndex, int pageSize)
        {
            return Search().Page(pageIndex, pageSize).ToList();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(int pageIndex, int pageSize)
        {
            return await Search().Page(pageIndex, pageSize).ToListAsync();
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
            return Search(tableName).Page(pageIndex, pageSize).ToList();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(string tableName, int pageIndex = 1, int pageSize = 12)
        {
            return await Search(tableName).Page(pageIndex, pageSize).ToListAsync();
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
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public async Task<PagedList<T>> GetPagedListAsync(int pageIndex, int pageSize, string orderBy = "ID", bool asc = true)
        {
            return await Search().ToPagedListAsync(pageIndex, pageSize, orderBy, asc);
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
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public async Task<PagedList<T>> GetPagedListAsync(Expression<Func<T, bool>> lambdaWhere, int pageIndex = 1, int pageSize = 12, string orderBy = "ID", bool asc = true)
        {
            return await GetPagedListAsync(lambdaWhere, pageIndex, pageSize, orderBy, asc);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <param name="tableName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<T> GetPagedList(Expression<Func<T, bool>> lambdaWhere, string tableName = "", int pageIndex = 1, int pageSize = 12, string orderBy = "ID", bool asc = true)
        {
            return Search(tableName).ToPagedList(lambdaWhere, pageIndex, pageSize, orderBy, asc);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <param name="tableName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public async Task<PagedList<T>> GetPagedListAsync(Expression<Func<T, bool>> lambdaWhere, string tableName = "", int pageIndex = 1, int pageSize = 12, string orderBy = "ID", bool asc = true)
        {
            return await GetPagedListAsync(lambdaWhere, tableName, pageIndex, pageSize, orderBy, asc);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="Model">自定义模型</typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<Model> GetPagedList<Model>(int pageIndex, int pageSize, string orderBy = "ID", bool asc = true)
        {
            return Search().ToPagedList<Model>(pageIndex, pageSize, orderBy, asc);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public async Task<PagedList<Model>> GetPagedListAsync<Model>(int pageIndex, int pageSize, string orderBy = "ID", bool asc = true)
        {
            return await GetPagedListAsync<Model>(pageIndex, pageSize, orderBy, asc);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="Model">自定义模型</typeparam>
        /// <param name="lambdaWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<Model> GetPagedList<Model>(Expression<Func<T, bool>> lambdaWhere, int pageIndex = 1, int pageSize = 12, string orderBy = "ID", bool asc = true)
        {
            return Search().ToPagedList<Model>(lambdaWhere, pageIndex, pageSize, orderBy, asc);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public async Task<PagedList<Model>> GetPagedListAsync<Model>(Expression<Func<T, bool>> lambdaWhere, int pageIndex = 1, int pageSize = 12, string orderBy = "ID", bool asc = true)
        {
            return await GetPagedListAsync<Model>(lambdaWhere, pageIndex, pageSize, orderBy, asc);
        }

        /// <summary>
        /// 分页查询
        /// <typeparam name="Model">自定义模型</typeparam>
        /// <param name="lambdaWhere">查询表达式</param>
        /// <param name="tableName">表名</param>
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// <param name="orderBy">排序</param>
        /// <param name="asc">升降</param>
        /// </summary>
        /// <returns></returns>
        public PagedList<Model> GetPagedList<Model>(Expression<Func<T, bool>> lambdaWhere, string tableName = "", int pageIndex = 1, int pageSize = 12, string orderBy = "ID", bool asc = true)
        {
            return Search(tableName).ToPagedList<Model>(lambdaWhere, pageIndex, pageSize, orderBy, asc);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <param name="tableName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public async Task<PagedList<Model>> GetPagedListAsync<Model>(Expression<Func<T, bool>> lambdaWhere, string tableName = "", int pageIndex = 1, int pageSize = 12, string orderBy = "ID", bool asc = true)
        {
            return await GetPagedListAsync<Model>(lambdaWhere, tableName, pageIndex, pageSize, orderBy, asc);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public bool Exists(Expression<Func<T, bool>> lambdaWhere)
        {
            return Search().Top(1).Where(lambdaWhere).First() != null;
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> lambdaWhere)
        {
            return await ExistsAsync(lambdaWhere);
        }

        /// <summary>
        /// 添加实体
        /// <param name="entity">传进的实体</param>
        /// </summary>
        public int Insert(T entity)
        {
            return _dbContext.Insert(entity);
        }
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(T entity)
        {
            return await InsertAsync(entity);
        }


        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public List<int> Insert(IEnumerable<T> entities)
        {
            return _dbContext.Insert(entities);
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<List<int>> InsertAsync(IEnumerable<T> entities)
        {
            return await InsertAsync(entities);
        }

        /// <summary>
        /// 批量添加实体
        /// <param name="entities">传进的实体列表</param>
        /// </summary>
        public void BulkInsert(IEnumerable<T> entities)
        {
            _dbContext.BulkInsert(entities);
        }
        /// <summary>
        /// 更新实体
        /// <param name="entity">传进的实体</param>
        /// </summary>
        public int Update(T entity)
        {
            return _dbContext.Update(entity);
        }

        /// <summary>
        /// 更新实体
        /// <param name="entities">传进的实体</param>
        /// </summary>
        public List<int> Update(IEnumerable<T> entities)
        {
            return _dbContext.Update(entities);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<List<int>> UpdateAsync(IEnumerable<T> entities)
        {
            return await _dbContext.UpdateAsync(entities);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update(T entity, Expression<Func<T, bool>> lambdaWhere)
        {
            return _dbContext.Update(entity, lambdaWhere);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="lambadaSelect"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update(Expression<Func<T, object>> lambadaSelect, Expression<Func<T, bool>> lambdaWhere)
        {
            return _dbContext.Update(lambadaSelect, lambdaWhere);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(T entity, Expression<Func<T, bool>> lambdaWhere)
        {
            return await _dbContext.UpdateAsync(entity, lambdaWhere);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        public int Update(T entity, string whereSql)
        {
            return _dbContext.Update<T>(entity, whereSql);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(T entity, string whereSql)
        {
            return await _dbContext.UpdateAsync(entity, whereSql);
        }

        /// <summary>
        /// 多表关联更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="joinOn"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(T entity,
            JoinOn<T, TEntity> joinOn,
            Expression<Func<T, TEntity, bool>> lambdaWhere)
        {
            var where = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            return _dbContext.Update(entity, joinOn, where);
        }
        /// <summary>
        /// 多表关联更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="joinOn"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(T entity,
            JoinOn<T, TEntity> joinOn,
            Expression<Func<T, TEntity, bool>> lambdaWhere)
        {
            var where = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            return await _dbContext.UpdateAsync(entity, joinOn, where);
        }

        /// <summary>
        /// 多表关联更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="joinOn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(T entity,
           JoinOn<T, TEntity> joinOn,
           Where where)
        {
            return _dbContext.Update(entity, joinOn, where.ToWhereClip());
        }
        /// <summary>
        /// 多表关联更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="joinOn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(T entity,
          JoinOn<T, TEntity> joinOn,
          Where where)
        {
            return await _dbContext.UpdateAsync(entity, joinOn, where.ToWhereClip());
        }

        /// <summary>
        /// 多表关联更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="joinOn"></param>
        /// <param name="joinType"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(T entity,
            Expression<Func<T, TEntity, bool>> joinOn,
            JoinType joinType,
            Expression<Func<T, TEntity, bool>> lambdaWhere)
        {
            var jo = new JoinOn<T, TEntity>(joinOn, joinType);
            var where = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            return _dbContext.Update(entity, jo, where);
        }
        /// <summary>
        /// 多表关联更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="joinOn"></param>
        /// <param name="joinType"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(T entity,
            Expression<Func<T, TEntity, bool>> joinOn,
            JoinType joinType,
            Expression<Func<T, TEntity, bool>> lambdaWhere)
        {
            var jo = new JoinOn<T, TEntity>(joinOn, joinType);
            var where = ExpressionToOperation<T>.ToWhereOperation(lambdaWhere);
            return await _dbContext.UpdateAsync(entity, jo, where);
        }

        /// <summary>
        /// 多表关联更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="joinOn"></param>
        /// <param name="joinType"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(T entity,
           Expression<Func<T, TEntity, bool>> joinOn,
           JoinType joinType,
           Where where)
        {
            var jo = new JoinOn<T, TEntity>(joinOn, joinType);
            return _dbContext.Update(entity, jo, where.ToWhereClip());
        }
        /// <summary>
        /// 多表关联更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="joinOn"></param>
        /// <param name="joinType"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(T entity,
           Expression<Func<T, TEntity, bool>> joinOn,
           JoinType joinType,
           Where where)
        {
            var jo = new JoinOn<T, TEntity>(joinOn, joinType);
            return await _dbContext.UpdateAsync(entity, jo, where.ToWhereClip());
        }

        /// <summary>
        /// 删除实体
        /// <param name="entity">传进的实体</param>
        /// </summary>
        public int Delete(T entity)
        {
            return _dbContext.Delete(entity);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(T entity)
        {
            return await _dbContext.DeleteAsync(entity);
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
        public async Task<int> DeleteAsync(string id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                return await DeleteAsync(entity);
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
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                return await DeleteAsync(entity);
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
            var list = Enumerable.ToList(entities);
            return _dbContext.Delete(list);
        }
        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(IEnumerable<T> entities)
        {
            var list = Enumerable.ToList(entities);
            return await _dbContext.DeleteAsync(list);
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
        public async Task<int> DeleteAsync(IEnumerable<string> ids)
        {
            var list = await GetListAsync(ids);
            return await DeletesAsync(list);
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
        /// 批量删除实体
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(IEnumerable<int> ids)
        {
            var list = await GetListAsync(ids);
            return await DeletesAsync(list);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Delete(Expression<Func<T, bool>> lambdaWhere)
        {
            return _dbContext.Delete<T>(lambdaWhere);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(Expression<Func<T, bool>> lambdaWhere)
        {
            return await _dbContext.DeleteAsync<T>(lambdaWhere);
        }


        #region sql
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public int Execute(string sql, params DbParameter[] dbParameters)
        {
            return _dbContext.ExecuteNonQuery(sql, dbParameters);
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="inputParamas"></param>
        /// <returns></returns>
        public int Execute(string sql, Dictionary<string, object> inputParamas)
        {
            return _dbContext.ExecuteNonQuery(sql, inputParamas);
        }
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <typeparam name="Model"></typeparam>
        /// <param name="sql"></param>
        /// <param name="inputParamas"></param>
        /// <returns></returns>
        public int Execute<Model>(string sql, Model inputParamas) where Model : class, new()
        {
            return _dbContext.ExecuteNonQuery(sql, inputParamas);
        }

        /// <summary>
        /// 执行sql语句
        /// <param name="sql"></param>
        /// </summary>
        public SqlSection FromSql(string sql)
        {
            return _dbContext.FromSql(sql);
        }
        /// <summary>
        /// 执行sql语句,带参数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="inputParamas"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql, Dictionary<string, object> inputParamas)
        {
            return _dbContext.FromSqlWithdAutomatic(sql, inputParamas.ToArray());
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
            return _dbContext.FromSqlWithdModel(sql, inputParamas);
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql, int pageIndex, int pageSize, string orderBy, bool asc = true, Dictionary<string, dynamic> keyValuePairs = null)
        {
            var sqlSection = _dbContext.FromSql(sql, pageIndex, pageSize, orderBy, asc);
            if (keyValuePairs != null && keyValuePairs.Count > 0)
            {
                foreach (var item in keyValuePairs)
                {
                    sqlSection = sqlSection.AddInParameter(item.Key, item.Value);
                }
            }
            return sqlSection;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="inputParamas"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string tableName, Dictionary<string, dynamic> inputParamas)
        {
            var sp = new StringPlus();
            sp.Append($"SELECT * FROM [{tableName}] WHERE 1=1");
            foreach (var item in inputParamas)
            {
                sp.Append($" AND [{item.Key}]=@{item.Key}");
            }
            var sqlSection = FromSql(sp.ToString());
            foreach (var item in inputParamas)
            {
                sqlSection = sqlSection.AddInParameter($"@{item.Key}", item.Value);
            }
            return sqlSection.ToDataTable();
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="inputParamas"></param>
        /// <returns></returns>
        public bool Update(string tableName, Dictionary<string, dynamic> inputParamas)
        {
            var sp = new StringPlus();
            sp.Append($"UPDATE [{tableName}] SET");
            foreach (var item in inputParamas)
            {
                if (!item.Key.Equals("ID"))
                {
                    sp.Append($" [{item.Key}]=@{item.Key},");
                }
            }
            sp.Remove(sp.Length - 1, 1);
            sp.Append(" WHERE [ID]=@ID");

            var sqlSection = FromSql(sp.ToString());

            foreach (var item in inputParamas)
            {
                if (item.Key.Equals("ID"))
                {
                    sqlSection = sqlSection.AddInParameter($"@{item.Key}", DbType.String, item.Value.ToString());
                }
                else
                {
                    sqlSection = sqlSection.AddInParameter($"@{item.Key}", item.Value);
                }
            }
            return sqlSection.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// 执行存储过程
        /// <param name="procName"></param>
        /// </summary>
        public ProcSection FromProc(string procName)
        {
            return _dbContext.FromProc(procName);
        }

        /// <summary>
        /// 执行存储过程，带参数
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="inputParamas"></param>
        /// <returns></returns>
        public ProcSection FromProc(string procName, Dictionary<string, object> inputParamas)
        {
            return _dbContext.FromProc(procName, inputParamas);
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
            return _dbContext.FromProc(procName, inputParamas);
        }

        #endregion

        /// <summary>
        /// 创建事务
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public DbTrans<T> BeginTransaction(int timeout = 30)
        {
            return _dbContext.BeginTransaction<T>(timeout);
        }

        /// <summary>
        /// 关闭事务
        /// </summary>
        /// <param name="transaction"></param>
        public void CloseTransaction(DbTransaction transaction)
        {
            _dbContext.CloseTransaction(transaction);
        }

        /// <summary>
        /// 创建事务，使用事务curd时推荐方式 using(var tran=CreateTransaction()) 方式
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public DbTrans<T> CreateTransaction(int timeout = 30)
        {
            return BeginTransaction(timeout);
        }
        /// <summary>
        /// 创建事务，使用事务curd时推荐方式 using(var tran=CreateTransaction()) 方式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public DbTrans<T> CreateTransaction(DbTransType type, int timeout = 30)
        {
            return _dbContext.BeginTransaction<T>(type, timeout);
        }

        /// <summary>
        /// 尝试事务提交
        /// </summary>
        /// <param name="action"></param>
        /// <param name="tryCount"></param>
        /// <param name="sleep"></param>
        /// <returns></returns>
        public Exception TryCommit(Action<DbTrans<T>> action, int tryCount = 3, int sleep = 3 * 1000)
        {
            return _dbContext.BeginTransaction<T>().TryCommit(action, tryCount, sleep);
        }


        #region 行转列

        /// <summary>
        /// 自定义行转列
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="Model"></typeparam>
        /// <param name="pivotInfo"></param>
        /// <param name="whereLambada"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<Model> ToPivotList<TEntity, Model>(PivotInfo<TEntity> pivotInfo,
            Expression<Func<Model, bool>> whereLambada,
            int pageIndex = 1,
            int pageSize = 1000,
            string orderBy = null,
            bool asc = true)
            where TEntity : Entity
            where Model : class, new()
        {
            using (var fdTran = CreateTransaction())
            {
                return fdTran.ToPivotList(pivotInfo, whereLambada, pageIndex, pageSize, orderBy, asc);
            }
        }
        /// <summary>
        /// 自定义行转列
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pivotInfo"></param>
        /// <param name="whereLambada"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<dynamic> ToPivotList<TEntity>(PivotInfo<TEntity> pivotInfo,
            Expression<Func<dynamic, bool>> whereLambada,
            int pageIndex = 1,
            int pageSize = 1000,
            string orderBy = null,
            bool asc = true)
            where TEntity : Entity
        {
            using (var fdTran = CreateTransaction())
            {
                return fdTran.ToPivotList(pivotInfo, whereLambada, pageIndex, pageSize, orderBy, asc);
            }
        }
        /// <summary>
        /// 自定义行转列
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pivotInfo"></param>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<dynamic> ToPivotList<TEntity>(PivotInfo<TEntity> pivotInfo,
            WhereExpression where,
            int pageIndex = 1,
            int pageSize = 1000,
            string orderBy = null,
            bool asc = true)
            where TEntity : Entity
        {
            using (var fdTran = this.CreateTransaction())
            {
                return fdTran.ToPivotList<dynamic, TEntity>(pivotInfo, where, pageIndex, pageSize, orderBy, asc);
            }
        }

        /// <summary>
        /// 自定义行转列
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pivotInfo"></param>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<dynamic> ToPivotList<TEntity>(PivotInfo<TEntity> pivotInfo,
            string where = null,
            int pageIndex = 1,
            int pageSize = 1000,
            string orderBy = null,
            bool asc = true)
            where TEntity : Entity
        {
            if (string.IsNullOrEmpty(where))
            {
                return ToPivotList(pivotInfo, WhereExpression.All, pageIndex, pageSize, orderBy, asc);
            }
            return ToPivotList(pivotInfo, new WhereExpression(where), pageIndex, pageSize, orderBy, asc);
        }

        /// <summary>
        /// 自定义行转列
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="Model"></typeparam>
        /// <param name="pivotInfo"></param>
        /// <param name="whereLambada"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public PagedList<Model> ToPivotList<TEntity, Model>(PivotInfo<TEntity> pivotInfo,
            Expression<Func<Model, bool>> whereLambada,
            int pageIndex = 1,
            int pageSize = 1000,
            Expression<Func<Model, object>> orderBy = null,
            bool asc = true)
            where TEntity : Entity
            where Model : class, new()
        {
            var order = "";
            if (orderBy != null)
                order = ExpressionToOperation<Model>.ToSelect("", orderBy).First()?.FieldName ?? "";
            return ToPivotList(pivotInfo, whereLambada, pageIndex, pageSize, order, asc);
        }

        #endregion

        /// <summary>
        /// 按天复制表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="isTruncate"></param>
        /// <returns></returns>
        public bool CopyTableByDay<TEntity>(bool isTruncate = false) where TEntity : Entity
        {
            return _dbContext.CopyTableByDay<TEntity>(isTruncate);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="dbParameters">参数数组</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteAsync(string sql, params DbParameter[] dbParameters)
        {
            return await _dbContext.ExecuteNonQueryAsync(sql, dbParameters);
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="inputParamas">参数字典</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteAsync(string sql, Dictionary<string, object> inputParamas)
        {
            return await _dbContext.ExecuteNonQueryAsync(sql, inputParamas);
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <typeparam name="Model">参数类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="inputParamas">参数对象</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteAsync<Model>(string sql, Model inputParamas) where Model : class, new()
        {
            return await _dbContext.ExecuteNonQueryAsync(sql, inputParamas.ToDictionary());
        }

        /// <summary>
        /// 查询数据表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="inputParamas">参数字典</param>
        /// <returns>数据表</returns>
        public async Task<DataTable> GetDataTableAsync(string tableName, Dictionary<string, dynamic> inputParamas)
        {
            var sp = new StringPlus();
            sp.Append($"SELECT * FROM [{tableName}] WHERE 1=1");
            foreach (var item in inputParamas)
            {
                sp.Append($" AND [{item.Key}]=@{item.Key}");
            }
            var sqlSection = FromSql(sp.ToString());
            foreach (var item in inputParamas)
            {
                sqlSection = sqlSection.AddInParameter($"@{item.Key}", item.Value);
            }
            return await sqlSection.ToDataTableAsync();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="inputParamas">参数字典</param>
        /// <returns>是否成功</returns>
        public async Task<bool> UpdateAsync(string tableName, Dictionary<string, dynamic> inputParamas)
        {
            var sp = new StringPlus();
            sp.Append($"UPDATE [{tableName}] SET");
            foreach (var item in inputParamas)
            {
                if (!item.Key.Equals("ID"))
                {
                    sp.Append($" [{item.Key}]=@{item.Key},");
                }
            }
            sp.Remove(sp.Length - 1, 1);
            sp.Append(" WHERE [ID]=@ID");

            var sqlSection = FromSql(sp.ToString());

            foreach (var item in inputParamas)
            {
                if (item.Key.Equals("ID"))
                {
                    sqlSection = sqlSection.AddInParameter($"@{item.Key}", DbType.String, item.Value.ToString());
                }
                else
                {
                    sqlSection = sqlSection.AddInParameter($"@{item.Key}", item.Value);
                }
            }
            return await sqlSection.ExecuteNonQueryAsync() > 0;
        }

        /// <summary>
        /// 按天复制表
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="isTruncate">是否清空目标表</param>
        /// <returns>是否成功</returns>
        public async Task<bool> CopyTableByDayAsync<TEntity>(bool isTruncate = false) where TEntity : Entity
        {
            return await _dbContext.CopyTableByDayAsync<TEntity>(isTruncate);
        }



    }
}
