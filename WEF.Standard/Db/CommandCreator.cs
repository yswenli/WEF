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
using System.Data.Common;

using WEF.Common;
using WEF.Expressions;

namespace WEF.Db
{
    /// <summary>
    /// Command Creator
    /// </summary>
    public class CommandCreator
    {

        /// <summary>
        /// 
        /// </summary>
        private Database _db;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        public CommandCreator(Database db)
        {
            _db = db;
        }


        #region 更新
        /// <summary>
        /// 创建更新DbCommand
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DbCommand CreateUpdateCommand<TEntity>(TEntity entity, WhereExpression where)
            where TEntity : Entity
        {
            var tableName = _db.DbProvider.BuildTableName(entity.GetTableName(), null);
            return CreateUpdateCommand(tableName, entity, where);
        }
 
        /// <summary>
        /// 创建更新DbCommand
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="joinOn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DbCommand CreateUpdateCommand<TEntity>(TEntity entity, JoinOn joinOn, WhereExpression where)
            where TEntity : Entity
        {
            var tableName = _db.DbProvider.BuildTableName(entity.GetTableName(), null);
            return CreateUpdateCommand(tableName, entity, joinOn, where);
        }
        /// <summary>
        /// 创建更新DbCommand
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="talbeName"></param>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DbCommand CreateUpdateCommand<TEntity>(string talbeName, TEntity entity, WhereExpression where)
            where TEntity : Entity
        {
            return CreateUpdateCommand<TEntity>(talbeName, entity, null, where);
        }

        /// <summary>
        /// 创建更新DbCommand
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="talbeName"></param>
        /// <param name="entity"></param>
        /// <param name="joinOn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DbCommand CreateUpdateCommand<TEntity>(string talbeName, TEntity entity, JoinOn joinOn, WhereExpression where)
            where TEntity : Entity
        {
            var mfields = entity.GetModifyFields();
            if (null == mfields || mfields.Count == 0)
                return null;
            var fields = new Field[mfields.Count];
            var values = new object[mfields.Count];
            var i = 0;
            foreach (ModifyField mf in mfields)
            {
                fields[i] = mf.Field;
                values[i] = mf.NewValue;
                i++;
            }
            return CreateUpdateCommand<TEntity>(talbeName, fields, values, joinOn, where);
        }

        /// <summary>
        /// 创建更新DbCommand
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="joinOn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DbCommand CreateUpdateCommand<TEntity>(Field[] fields, object[] values, JoinOn joinOn, WhereExpression where)
            where TEntity : Entity
        {
            var tableName = _db.DbProvider.BuildTableName(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>());
            return CreateUpdateCommand<TEntity>(tableName, fields, values, joinOn, where);
        }
        /// <summary>
        /// 创建更新DbCommand
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="joinOn"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DbCommand CreateUpdateCommand<TEntity>(string tableName,
            Field[] fields,
            object[] values,
            JoinOn joinOn,
            WhereExpression where)
            where TEntity : Entity
        {
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));

            if (null == fields || fields.Length == 0 || null == values || values.Length == 0)
                return null;

            Check.Require(fields.Length == values.Length, "fields.Length must be equal values.Length");

            var length = fields.Length;

            var sql = new StringPlus();
            sql.Append("UPDATE ");
            tableName = string.IsNullOrEmpty(tableName) ? _db.DbProvider.BuildTableName(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>()) : tableName;
            sql.Append(tableName);
            sql.Append(" SET ");

            var identityField = EntityCache.GetIdentityField<TEntity>();
            var identityExist = !Field.IsNullOrEmpty(identityField);
            var list = new List<Parameter>();
            var colums = new StringPlus();
            for (var i = 0; i < length; i++)
            {
                if (identityExist)
                {
                    //标识列  排除
                    if (fields[i].PropertyName.Equals(identityField.PropertyName))
                        continue;
                }

                colums.Append(",");
                colums.Append(fields[i].FieldName);
                colums.Append("=");

                if (values[i] is Expression)
                {
                    var expression = (Expression)values[i];
                    colums.Append(expression);
                    list.AddRange(expression.Parameters);
                }
                else if (values[i] is Field)
                {
                    var fieldValue = (Field)values[i];
                    colums.Append(fieldValue.TableFieldName);
                }
                else
                {
                    var pname = DataUtils.MakeUniqueKey(fields[i]);
                    colums.Append(pname);
                    var p = new Parameter(pname, values[i], fields[i].ParameterDbType, fields[i].ParameterSize);
                    list.Add(p);
                }
            }
            sql.Append(colums.ToString().Substring(1));

            //join
            if (joinOn != null && joinOn._joins != null && joinOn._joins.Count > 0)
            {
                sql.Append($" FROM {tableName}");
                sql.Append($" {joinOn.ToUpdateString(tableName, _db.DbProvider.GetType().Name)}");
                if (joinOn.Parameters != null && joinOn.Parameters.Count > 0)
                    list.AddRange(joinOn.Parameters);
            }

            //where
            if (WhereExpression.IsNullOrEmpty(where))
                where = WhereExpression.All;
            sql.Append(where.WhereString);
            if (where.Parameters != null && where.Parameters.Count > 0)
                list.AddRange(where.Parameters);

            var cmd = _db.GetSqlStringCommand(sql.ToString());
            _db.AddCommandParameter(cmd, list.ToArray());
            return cmd;
        }

        #endregion

        #region 删除

        /// <summary>
        /// 创建删除DbCommand
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DbCommand CreateDeleteCommand(string tableName, string userName, WhereExpression where)
        {
            if (WhereExpression.IsNullOrEmpty(where))
                throw new Exception("请传入删除条件，删除整表数据请使用.DeleteAll<T>()方法。");

            StringPlus sql = new StringPlus();
            sql.Append("DELETE FROM ");
            sql.Append(_db.DbProvider.BuildTableName(tableName, userName));
            sql.Append(where.WhereString);
            DbCommand cmd = _db.GetSqlStringCommand(sql.ToString());
            _db.AddCommandParameter(cmd, where.Parameters.ToArray());

            return cmd;
        }

        /// <summary>
        /// 创建删除DbCommand
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DbCommand CreateDeleteCommand<TEntity>(WhereExpression where)
             where TEntity : Entity
        {
            return CreateDeleteCommand(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>(), where);
        }

        #endregion

        #region 添加
        /// <summary>
        /// 创建添加DbCommand
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public DbCommand CreateInsertCommand<TEntity>(Field[] fields, object[] values)
            where TEntity : Entity
        {
            return CreateInsertCommand<TEntity>(_db.DbProvider.BuildTableName(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>()), fields, values);
        }

        /// <summary>
        /// 创建添加DbCommand
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public DbCommand CreateInsertCommand<TEntity>(string tableName, Field[] fields, object[] values)
            where TEntity : Entity
        {
            Check.Require(!EntityCache.IsReadOnly<TEntity>(), string.Concat("Entity(", EntityCache.GetTableName<TEntity>(), ") is readonly!"));

            if (null == fields || fields.Length == 0 || null == values || values.Length == 0)
                return null;

            var sql = new StringPlus();
            sql.Append("INSERT INTO ");
            tableName = string.IsNullOrEmpty(tableName) ? _db.DbProvider.BuildTableName(EntityCache.GetTableName<TEntity>(), EntityCache.GetUserName<TEntity>()) : tableName;
            sql.Append(tableName);
            sql.Append(" (");

            var identityField = EntityCache.GetIdentityField<TEntity>();
            var identityExist = !Field.IsNullOrEmpty(identityField);
            var isSequence = false;

            if (_db.DbProvider.GetType().Name == "OracleProvider")
            {
                if (!string.IsNullOrEmpty(EntityCache.GetSequence<TEntity>()))
                    isSequence = true;
            }

            var insertFields = new Dictionary<string, string>();
            var parameters = new List<Parameter>();

            var length = fields.Length;
            for (var i = 0; i < length; i++)
            {
                if (identityExist)
                {
                    if (fields[i].PropertyName.Equals(identityField.PropertyName))
                    {
                        if (isSequence)
                        {
                            insertFields.Add(fields[i].FieldName, string.Concat(EntityCache.GetSequence<TEntity>(), ".nextval"));
                        }
                        continue;
                    }
                }
                string panme = DataUtils.MakeUniqueKey(fields[i]);
                insertFields.Add(fields[i].FieldName, panme);
                var p = new Parameter(panme, values[i], fields[i].ParameterDbType, fields[i].ParameterSize);
                parameters.Add(p);
            }
            var fs = new StringPlus();
            var ps = new StringPlus();

            foreach (var kv in insertFields)
            {
                fs.Append(",");
                fs.Append(kv.Key);

                ps.Append(",");
                ps.Append(kv.Value);
            }

            sql.Append(fs.ToString().Substring(1));
            sql.Append(") VALUES (");
            sql.Append(ps.ToString().Substring(1));
            sql.Append(")");

            var cmd = _db.GetSqlStringCommand(sql.ToString());
            _db.AddCommandParameter(cmd, parameters.ToArray());
            return cmd;
        }

        /// <summary>
        /// 创建添加DbCommand
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbCommand CreateInsertCommand<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            var tableName = _db.DbProvider.BuildTableName(entity.GetTableName(), null);
            return CreateInsertCommand(tableName, entity);
        }

        /// <summary>
        /// 创建添加DbCommand
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbCommand CreateInsertCommand<TEntity>(string tableName, TEntity entity)
           where TEntity : Entity
        {
            if (null == entity)
                return null;
            var mfields = entity.GetModifyFields();

            if (null == mfields || mfields.Count == 0)
            {
                return CreateInsertCommand<TEntity>(tableName, entity.GetFields(), entity.GetValues());
            }
            else
            {
                List<Field> fields = new List<Field>();
                List<object> values = new List<object>();
                foreach (ModifyField m in mfields)
                {
                    fields.Add(m.Field);
                    values.Add(m.NewValue);
                }

                return CreateInsertCommand<TEntity>(tableName, fields.ToArray(), values.ToArray());
            }
        }
        #endregion
    }

}
