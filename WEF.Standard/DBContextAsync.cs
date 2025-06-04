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

namespace WEF
{
    /// <summary>
    /// WEF核心类，数据操作上下文，
    /// DBContextAsync
    /// </summary>
    public sealed partial class DBContext
    {
        #region 异步查询操作        

        private async Task<int> InsertExecuteAsync<TEntity>(DbCommand cmd, DbTransaction tran)
           where TEntity : Entity
        {
            if (null == cmd)
                return 0;

            Field identity = EntityCache.GetIdentityField<TEntity>();
            if (Field.IsNullOrEmpty(identity))
            {
                return tran == null ? await ExecuteNonQueryAsync(cmd) : await ExecuteNonQueryAsync(cmd, tran);
            }
            else
            {
                object scalarValue = null;
                if (Db.DbProvider.GetType().Name == "MsAccessProvider")
                {
                    if (tran == null)
                    {
                        await ExecuteNonQueryAsync(cmd);
                        scalarValue =
                            ExecuteScalar(
                                _db.GetSqlStringCommand(string.Format("select max({0}) from {1}", identity.FieldName,
                                    identity.TableName))); //Max<TEntity, int>(identity, WhereClip.All) + 1;
                    }
                    else
                    {
                        await ExecuteNonQueryAsync(cmd, tran);
                        scalarValue = await ExecuteScalarAsync(_db.GetSqlStringCommand(string.Format("select max({0}) from {1}", identity.FieldName, identity.TableName)), tran); //Max<TEntity, int>(identity, WhereClip.All) + 1;
                    }

                }
                else if (Db.DbProvider.GetType().Name == "OracleProvider")
                {
                    if (tran == null)
                    {
                        ExecuteNonQuery(cmd);
                        scalarValue =
                           await ExecuteScalarAsync(
                                _db.GetSqlStringCommand(string.Format(_db.DbProvider.RowAutoID,
                                    EntityCache.GetSequence<TEntity>())));
                    }
                    else
                    {
                        await ExecuteNonQueryAsync(cmd, tran);
                        scalarValue = await ExecuteScalarAsync(_db.GetSqlStringCommand(string.Format(_db.DbProvider.RowAutoID, EntityCache.GetSequence<TEntity>())), tran);
                    }
                }
                else
                {
                    if (Db.DbProvider.SupportBatch)
                    {
                        if (tran == null)
                        {
                            cmd.CommandText = string.Concat(cmd.CommandText, ";", _db.DbProvider.RowAutoID);
                            scalarValue = await ExecuteScalarAsync(cmd);
                        }
                        else
                        {
                            cmd.CommandText = string.Concat(cmd.CommandText, ";", _db.DbProvider.RowAutoID);
                            scalarValue = await ExecuteScalarAsync(cmd, tran);
                        }
                    }
                    else
                    {
                        if (tran == null)
                        {
                            await ExecuteNonQueryAsync(cmd);
                            scalarValue = await ExecuteScalarAsync(_db.GetSqlStringCommand(Db.DbProvider.RowAutoID));
                        }
                        else
                        {
                            await ExecuteNonQueryAsync(cmd, tran);
                            scalarValue = await ExecuteScalarAsync(_db.GetSqlStringCommand(Db.DbProvider.RowAutoID), tran);
                        }
                    }
                }

                if (null == scalarValue || Convert.IsDBNull(scalarValue))
                    return 0;
                return Convert.ToInt32(scalarValue);
            }
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync<TEntity>(DbTransaction tran, TEntity entity)
            where TEntity : Entity
        {
            return await InsertExecuteAsync<TEntity>(_cmdCreator.CreateInsertCommand<TEntity>(entity), tran);
        }

        /// <summary>
        /// 异步判断是否存在记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync<TEntity>(WhereExpression where)
            where TEntity : Entity
        {
            if (where == null)
                throw new ArgumentNullException(nameof(where));

            try
            {
                var search = Search<TEntity>().Where(where).Top(1).Select(EntityCache.GetFirstField<TEntity>());
                var sql = search.SqlString; // 只查一条
                Dictionary<string, object> parameters = null;
                if (search.Parameters != null)
                {
                    parameters = search.Parameters.ToDictionary();
                }

                using (var reader = await ExecuteReaderAsync(sql, parameters))
                {
                    return reader != null && reader.Read();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"ExistsAsync<{typeof(TEntity).Name}> failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 异步判断是否存在记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync<TEntity>(string tableName, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return await ExistsAsync<TEntity>(ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }

        /// <summary>
        /// 异步判断是否存在记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync<TEntity>(Where where)
            where TEntity : Entity
        {
            return await ExistsAsync<TEntity>(where.ToWhereClip());
        }

        /// <summary>
        /// 异步获取单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<TEntity> GetEntityAsync<TEntity>(WhereExpression where)
            where TEntity : Entity, new()
        {
            if (where == null)
                throw new ArgumentNullException(nameof(where));

            var search = Search<TEntity>().Where(where).Top(1);
            var sql = search.SqlString;
            var parameters = search.Parameters?.ToDictionary();

            using (var reader = await ExecuteReaderAsync(sql, parameters))
            {
                if (reader != null && reader.Read())
                {
                    return reader.Reader<TEntity>();
                }
                return null;
            }
        }

        /// <summary>
        /// 异步获取实体列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListAsync<TEntity>(WhereExpression where)
            where TEntity : Entity, new()
        {
            if (where == null)
                throw new ArgumentNullException(nameof(where));

            var search = Search<TEntity>().Where(where);
            var sql = search.SqlString;
            var parameters = search.Parameters?.ToDictionary();

            using (var reader = await ExecuteReaderAsync(sql, parameters))
            {
                return reader?.ReaderToList<TEntity>() ?? new List<TEntity>();
            }
        }

        /// <summary>
        /// 异步获取分页数据
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序</param>
        /// <returns>分页列表</returns>
        public async Task<PagedList<TEntity>> GetPageAsync<TEntity>(int pageIndex, int pageSize, WhereExpression where, OrderByOperation orderBy = null)
            where TEntity : Entity, new()
        {
            if (where == null)
                throw new ArgumentNullException(nameof(where));

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;

            var search = Search<TEntity>().Where(where);
            if (orderBy != null)
            {
                search.OrderBy(orderBy);
            }

            var totalCount = await GetCountAsync<TEntity>(where);

            // 添加分页参数
            search.Page(pageIndex, pageSize);

            // 使用分页后的查询获取数据
            var items = await search.ToListAsync();

            return new PagedList<TEntity>
            {
                TotalItemCount = totalCount,
                Data = items,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// 异步获取记录数
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<int> GetCountAsync<TEntity>(WhereExpression where)
            where TEntity : Entity
        {
            return await Search<TEntity>().Select(Field.All.Count()).Where(where).ToScalarAsync<int>();
        }

        #endregion

        #region 异步更新操作

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public async Task<int> UpdateAsync<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            if (!entity.IsModify())
                return 0;

            WhereExpression where = DataUtils.GetPrimaryKeyWhere(entity);

            Check.Require(!WhereExpression.IsNullOrEmpty(where), "entity must have the primarykey!");

            return await UpdateAsync(entity, where);
        }

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tran"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<List<int>> UpdateAsync<TEntity>(DbTrans<TEntity> tran, params TEntity[] entities)
          where TEntity : Entity
        {
            var result = new List<int>();
            if (entities == null || entities.Length < 1)
                return result;
            using (var trans = BeginTransaction<TEntity>())
            {
                return await trans.UpdateAsync(entities);
            }
        }

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public async Task<List<int>> UpdateAsync<TEntity>(params TEntity[] entities)
            where TEntity : Entity
        {
            if (null == entities || entities.Length == 0)
                return null;
            using (var trans = BeginTransaction<TEntity>())
            {
                return await UpdateAsync(trans, entities);
            }
        }

        /// <summary>
        /// 异步更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<TEntity>(TEntity entity, WhereExpression where)
            where TEntity : Entity
        {
            return !entity.IsModify()
                ? 0
                : await ExecuteNonQueryAsync(_cmdCreator.CreateUpdateCommand(entity, @where));
        }

        #endregion

        #region 异步删除操作

        /// <summary>
        /// 异步删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            return await DeleteAsync(entity.GetTableName(), entity);
        }

        /// <summary>
        /// 异步删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync<TEntity>(string tableName, TEntity entity)
            where TEntity : Entity
        {
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", tableName, ") is readonly!"));

            WhereExpression where = DataUtils.GetPrimaryKeyWhere(entity);

            Check.Require(!WhereExpression.IsNullOrEmpty(where), "entity must have the primarykey!");

            return await DeleteAsync<TEntity>(tableName, where);
        }

        /// <summary>
        /// 异步删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync<TEntity>(string tableName, WhereExpression where)
           where TEntity : Entity
        {
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));

            return await ExecuteNonQueryAsync(_cmdCreator.CreateDeleteCommand(tableName ?? EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where));
        }

        #endregion

        #region 异步插入操作

        /// <summary>
        /// 异步添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<List<int>> InsertAsync<TEntity>(params TEntity[] entities)
            where TEntity : Entity
        {
            if (entities == null || entities.Length == 0)
                return new List<int>();

            var result = new List<int>();
            using (var trans = BeginTransaction<TEntity>())
            {
                try
                {
                    foreach (var entity in entities)
                    {
                        if (entity == null)
                            continue;

                        var insertResult = await InsertAsync(trans, entity);
                        result.Add(insertResult);
                    }

                    await trans.CommitAsync();
                    return result;
                }
                catch
                {
                    await trans.RollbackAsync();
                    throw;
                }
            }
        }

        /// <summary>
        /// 异步添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            var cmd = _cmdCreator.CreateInsertCommand(entity);
            int returnValue = 0;

            if (null == cmd)
                return returnValue;
            returnValue = await InsertExecuteAsync<TEntity>(cmd, null);
            return returnValue;
        }

        /// <summary>
        /// 异步从sql中获取实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public async Task<List<T>> ToListAsync<T>(string sql, params DbParameter[] dbParameters)
        {
            using (var reader = await ExecuteReaderAsync(sql, dbParameters))
            {
                return reader.ReaderToList<T>();
            }
        }

        #endregion

        #region 异步执行command

        /// <summary>
        /// 异步执行ExecuteNonQuery
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(DbCommand cmd)
        {
            return await _db.ExecuteNonQueryAsync(cmd);
        }

        /// <summary>
        /// 异步执行ExecuteNonQuery
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(DbCommand cmd, DbTransaction tran)
        {
            return await _db.ExecuteNonQueryAsync(cmd, tran);
        }

        /// <summary>
        /// 异步执行ExecuteNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, DbTransaction tran)
        {
            return await _db.ExecuteNonQueryAsync(sql, tran);
        }


        /// <summary>
        /// 异步执行ExecuteNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="inputParamas"></param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, Dictionary<string, object> inputParamas)
        {
            return await _db.ExecuteNonQueryAsync(sql, inputParamas);
        }

        /// <summary>
        /// 异步执行ExecuteScalar
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarAsync(DbCommand cmd, DbTransaction tran)
        {
            return await _db.ExecuteScalarAsync(cmd, tran);
        }

        /// <summary>
        /// 异步执行ExecuteScalar
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarAsync(DbCommand cmd)
        {
            return await _db.ExecuteScalarAsync(cmd);
        }

        /// <summary>
        /// 异步执行ExecuteReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public async Task<IDataReader> ExecuteReaderAsync(string sql, Dictionary<string, object> dbParameters)
        {
            return await _db.ExecuteReaderAsync(sql, dbParameters);
        }

        /// <summary>
        /// 异步执行ExecuteReader
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public async Task<IDataReader> ExecuteReaderAsync(DbCommand cmd)
        {
            return await _db.ExecuteReaderAsync(cmd);
        }

        /// <summary>
        /// 异步执行ExecuteReader
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public async Task<IDataReader> ExecuteReaderAsync(DbCommand cmd, DbTransaction tran)
        {
            return await _db.ExecuteReaderAsync(cmd, tran);
        }


        /// <summary>
        /// 异步执行ExecuteDataSet
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public async Task<DataSet> ExecuteDataSetAsync(DbCommand cmd, DbTransaction tran)
        {
            return await _db.ExecuteDataSetAsync(cmd, tran);
        }

        /// <summary>
        /// 异步执行ExecuteDataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public async Task<DataTable> ExecuteDataTableAsync(string sql, params DbParameter[] dbParameters)
        {
            var ds = await _db.ExecuteDataSetAsync(sql, dbParameters);
            return ds?.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        /// <summary>
        /// 异步执行ExecuteDataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public async Task<DataSet> ExecuteDataSetAsync(string sql, params DbParameter[] dbParameters)
        {
            return await _db.ExecuteDataSetAsync(sql, dbParameters);
        }

        /// <summary>
        /// 异步执行ExecuteNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, params DbParameter[] dbParameters)
        {
            return await _db.ExecuteNonQueryAsync(sql, dbParameters);
        }

        /// <summary>
        /// 异步执行ExecuteScalar
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarAsync(string sql, params DbParameter[] dbParameters)
        {
            return await _db.ExecuteScalarAsync(sql, dbParameters);
        }

        /// <summary>
        /// 异步执行ExecuteReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public async Task<IDataReader> ExecuteReaderAsync(string sql, params DbParameter[] dbParameters)
        {
            return await _db.ExecuteReaderAsync(sql, dbParameters);
        }

        #endregion
    }
}
