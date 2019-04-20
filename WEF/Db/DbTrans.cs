/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2019
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/
using WEF.Common;
using WEF.Expressions;
using WEF.Section;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace WEF.Db
{
    /// <summary>
    /// 事务
    /// </summary>
    public class DbTrans : IDisposable
    {

        /// <summary>
        /// 事务
        /// </summary>
        private DbTransaction trans;

        /// <summary>
        /// 连接
        /// </summary>
        private DbConnection conn;


        /// <summary>
        /// 
        /// </summary>
        private DBContext DBContext;

        /// <summary>
        /// 判断是否有提交或回滚
        /// </summary>
        private bool isCommitOrRollback = false;

        /// <summary>
        /// 是否关闭
        /// </summary>
        private bool isClose = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="DBContext"></param>
        public DbTrans(DbTransaction trans, DBContext DBContext)
        {
            Check.Require(trans, "trans", Check.NotNull);

            this.trans = trans;
            this.conn = trans.Connection;
            this.DBContext = DBContext;

            if (this.conn.State != ConnectionState.Open)
                this.conn.Open();

        }



        /// <summary>
        /// 连接
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                return conn;
            }
        }

        /// <summary>
        /// 事务级别
        /// </summary>
        public IsolationLevel IsolationLevel
        {
            get { return trans.IsolationLevel; }
        }

        /// <summary>
        /// 提交
        /// </summary>
        public void Commit()
        {
            trans.Commit();

            isCommitOrRollback = true;

            Close();
        }


        /// <summary>
        /// 回滚
        /// </summary>
        public void Rollback()
        {
            trans.Rollback();

            isCommitOrRollback = true;

            Close();
        }


        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="dbTrans"></param>
        /// <returns></returns>
        public static implicit operator DbTransaction(DbTrans dbTrans)
        {
            return dbTrans.trans;
        }


        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (isClose)
                return;

            if (!isCommitOrRollback)
            {
                isCommitOrRollback = true;

                trans.Rollback();
            }

            if (conn.State != ConnectionState.Closed)
            {
                conn.Close();
            }

            trans.Dispose();

            isClose = true;
        }


        #region IDisposable 成员
        /// <summary>
        /// 关闭连接并释放资源
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion


        /// <summary>
        /// FromSql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlSection FromSql(string sql)
        {
            return DBContext.FromSql(sql).SetDbTransaction(trans);
        }


        /// <summary>
        /// FromPro
        /// </summary>
        /// <param name="proName"></param>
        /// <returns></returns>
        public ProcSection FromPro(string proName)
        {
            return DBContext.FromProc(proName).SetDbTransaction(trans);
        }


        #region 查询


        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public ISearch<TEntity> From<TEntity>()
            where TEntity : Entity
        {
            return new Search<TEntity>(DBContext.Db, trans);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public ISearch<TEntity> From<TEntity>(string asName)
            where TEntity : Entity
        {
            return new Search<TEntity>(DBContext.Db, trans, asName);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public ISearch From(string tableName)
        {
            return new Search(DBContext.Db, tableName, "", trans);
        }
        #endregion


        #region 更新

        /// <summary>
        /// 更新全部字段  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int UpdateAll<TEntity>(params TEntity[] entities)
            where TEntity : Entity
        {
            return DBContext.UpdateAll<TEntity>(trans, entities);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int UpdateAll<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.UpdateAll<TEntity>(trans, entities.ToArray());
        }
        /// <summary>
        /// 更新全部字段  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public int UpdateAll<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            return DBContext.UpdateAll<TEntity>(trans, entity);
        }


        /// <summary>
        /// 更新全部字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int UpdateAll<TEntity>(TEntity entity, WhereOperation where)
            where TEntity : Entity
        {
            return DBContext.UpdateAll<TEntity>(trans, entity, where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int UpdateAll<TEntity>(TEntity entity, Where where)
            where TEntity : Entity
        {
            return DBContext.UpdateAll<TEntity>(trans, entity, where);
        }


        /// <summary>
        /// 更新  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Update<TEntity>(params TEntity[] entities)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, entities);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Update<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, entities.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Update<TEntity>(List<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, entities.ToArray());
        }
        /// <summary>
        /// 更新  
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public int Update<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, entity);
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity entity, WhereOperation where)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, entity, where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity entity, Where where)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, entity, where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, entity, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 更新单个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(Field field, object value, WhereOperation where)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, field, value, where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(Field field, object value, Where where)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, field, value, where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(Field field, object value, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, field, value, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 更新多个值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fieldValue"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(Dictionary<Field, object> fieldValue, WhereOperation where)
              where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, fieldValue, where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fieldValue"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(Dictionary<Field, object> fieldValue, Where where)
              where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, fieldValue, where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fieldValue"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(Dictionary<Field, object> fieldValue, Expression<Func<TEntity, bool>> lambdaWhere)
              where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, fieldValue, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(Field[] fields, object[] values, WhereOperation where)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, fields, values, where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Update<TEntity>(Field[] fields, object[] values, Where where)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, fields, values, where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Update<TEntity>(Field[] fields, object[] values, Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return DBContext.Update<TEntity>(trans, fields, values, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }

        #endregion


        #region 删除

        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            return DBContext.Delete<TEntity>(trans, entity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Delete<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.Delete<TEntity>(trans, entities);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Delete<TEntity>(List<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.Delete<TEntity>(trans, entities);
        }
        ///// <summary>
        /////  删除
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <param name="pkValues"></param>
        ///// <returns></returns>
        //public int Delete<TEntity>(params object[] pkValues)
        //    where TEntity : Entity
        //{
        //    return DBContext.DeleteByPrimaryKey<TEntity>(trans, pkValues);
        //}
        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public int Delete<TEntity>(params string[] pkValues)
            where TEntity : Entity
        {
            return DBContext.DeleteByPrimaryKey<TEntity>(trans, pkValues);
        }
        public int Delete<TEntity>(params Guid[] pkValues)
            where TEntity : Entity
        {
            return DBContext.DeleteByPrimaryKey<TEntity>(trans, pkValues);
        }
        public int Delete<TEntity>(params long[] pkValues)
            where TEntity : Entity
        {
            return DBContext.DeleteByPrimaryKey<TEntity>(trans, pkValues);
        }
        public int Delete<TEntity>(params int[] pkValues)
            where TEntity : Entity
        {
            return DBContext.DeleteByPrimaryKey<TEntity>(trans, pkValues);
        }
        /// <summary>
        ///  删除
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete<TEntity>(WhereOperation where)
            where TEntity : Entity
        {
            return DBContext.Delete<TEntity>(trans, where);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete<TEntity>(Where where)
            where TEntity : Entity
        {
            return DBContext.Delete<TEntity>(trans, where.ToWhereClip());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="lambdaWhere"></param>
        /// <returns></returns>
        public int Delete<TEntity>(Expression<Func<TEntity, bool>> lambdaWhere)
            where TEntity : Entity
        {
            return DBContext.Delete<TEntity>(trans, ExpressionToOperation<TEntity>.ToWhereOperation(lambdaWhere));
        }
        #endregion


        #region 添加

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Insert<TEntity>(params TEntity[] entities)
            where TEntity : Entity
        {
            return DBContext.Insert<TEntity>(trans, entities);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Insert<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.Insert<TEntity>(trans, entities);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public int Insert<TEntity>(List<TEntity> entities)
            where TEntity : Entity
        {
            return DBContext.Insert<TEntity>(trans, entities);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            return DBContext.Insert<TEntity>(trans, entity);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public int Insert<TEntity>(Field[] fields, object[] values)
            where TEntity : Entity
        {
            return DBContext.Insert<TEntity>(trans, fields, values);
        }
        #endregion
    }
}
