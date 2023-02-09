/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：yswenli
*命名空间：WEF.Expressions
*文件名： JoinOn
*版本号： V1.0.0.0
*唯一标识：d80f35d1-1274-4260-a3de-e40d9448ee42
*当前的用户域：WALLE
*创建人： WALLE
*电子邮箱：yswenli.wen@outlook.com
*创建时间：2022/11/28 15:22:19
*描述：JoinOn
*
*=================================================
*修改标记
*修改时间：2022/11/28 15:22:19
*修改人： yswenli
*版本号： V1.0.0.0
*描述：JoinOn
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using WEF.Common;

namespace WEF.Expressions
{
    /// <summary>
    /// join
    /// </summary>
    public class JoinOn : IDisposable
    {
        /// <summary>
        /// _joins
        /// </summary>
        internal Dictionary<string, KeyValuePair<string, WhereExpression>> _joins;

        /// <summary>
        /// Parameters
        /// </summary>
        internal List<Parameter> Parameters { get; private set; }

        internal string _tableName;

        internal WhereExpression _where;

        internal JoinType _joinType;

        /// <summary>
        /// Join
        /// </summary>
        internal JoinOn()
        {
            _joins = new Dictionary<string, KeyValuePair<string, WhereExpression>>();
            Parameters = new List<Parameter>();
        }

        /// <summary>
        /// join
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <param name="joinType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public JoinOn(string tableName, WhereExpression where, JoinType joinType) : this()
        {
            Add(tableName, where, joinType);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <param name="joinType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public JoinOn Add(string tableName, WhereExpression where, JoinType joinType)
        {
            _tableName = tableName;
            _where = where;
            _joinType = joinType;

            if (string.IsNullOrEmpty(_tableName) || WhereExpression.IsNullOrEmpty(_where))
            {
                throw new ArgumentNullException(nameof(_where));
            }

            if (_joins.ContainsKey(_tableName))
            {
                var index = (_joins.Keys.Count(d => d.StartsWith(_tableName)) + 1).ToString();
                var realTableName = _tableName.Substring(1, _tableName.Length - 2);
                _tableName += " as " + _tableName.Insert(_tableName.Length - 1, index);
                where._expressionString = where._expressionString.Replace(realTableName, realTableName + index);
            }

            string joinString = string.Empty;
            switch (_joinType)
            {
                case JoinType.InnerJoin:
                    joinString = "INNER JOIN";
                    break;
                case JoinType.LeftJoin:
                    joinString = "LEFT OUTER JOIN";
                    break;
                case JoinType.RightJoin:
                    joinString = "RIGHT OUTER JOIN";
                    break;
                case JoinType.CrossJoin:
                    joinString = "CROSS JOIN";
                    break;
                case JoinType.FullJoin:
                    joinString = "FULL OUTER JOIN";
                    break;
                default:
                    joinString = "INNER JOIN";
                    break;
            }

            _joins.Add(_tableName, new KeyValuePair<string, WhereExpression>(joinString, _where));

            if (_where.Parameters.Count > 0)
                Parameters.AddRange(_where.Parameters);
            return this;
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="joinOn"></param>
        /// <returns></returns>
        public JoinOn Add(JoinOn joinOn)
        {
            return Add(joinOn._tableName, joinOn._where, joinOn._joinType);
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public string ToString(string tableName, string dbName)
        {
            if (_joins == null || _joins.Count < 1) return tableName;

            StringPlus fromstring = new StringPlus();

            //处理ACCESS 的多表联合查询
            if (!string.IsNullOrEmpty(dbName) && dbName == "MsAccessProvider")
            {
                fromstring.Append('(', _joins.Count);
                fromstring.Append(tableName);
                foreach (KeyValuePair<string, KeyValuePair<string, WhereExpression>> kv in _joins)
                {
                    fromstring.Append(" ");
                    fromstring.Append(kv.Value.Key);
                    fromstring.Append(" ");
                    fromstring.Append(kv.Key);
                    fromstring.Append(" ON ");
                    fromstring.Append(kv.Value.Value.ToString());
                    fromstring.Append(")");
                }
            }
            else
            {
                fromstring.Append(tableName);
                foreach (KeyValuePair<string, KeyValuePair<string, WhereExpression>> kv in _joins)
                {
                    fromstring.Append(" ");
                    fromstring.Append(kv.Value.Key);
                    fromstring.Append(" ");
                    fromstring.Append(kv.Key);
                    fromstring.Append(" ON ");
                    fromstring.Append(kv.Value.Value.ToString());
                }
            }

            return fromstring.ToString();

        }

        /// <summary>
        /// ToUpdateString
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public string ToUpdateString(string tableName, string dbName)
        {
            StringPlus fromstring = new StringPlus();

            //处理ACCESS 的多表联合查询
            if (!string.IsNullOrEmpty(dbName) && dbName == "MsAccessProvider")
            {
                fromstring.Append('(', _joins.Count);
                fromstring.Append(tableName);
                foreach (KeyValuePair<string, KeyValuePair<string, WhereExpression>> kv in _joins)
                {
                    fromstring.Append(" ");
                    fromstring.Append(kv.Value.Key);
                    fromstring.Append(" ");
                    fromstring.Append(kv.Key);
                    fromstring.Append(" ON ");
                    fromstring.Append(kv.Value.Value.ToString());
                    fromstring.Append(")");
                }
            }
            else
            {
                foreach (KeyValuePair<string, KeyValuePair<string, WhereExpression>> kv in _joins)
                {
                    fromstring.Append(" ");
                    fromstring.Append(kv.Value.Key);
                    fromstring.Append(" ");
                    fromstring.Append(kv.Key);
                    fromstring.Append(" ON ");
                    fromstring.Append(kv.Value.Value.ToString());
                }
            }

            return fromstring.ToString();

        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _joins.Clear();
            Parameters.Clear();
        }

        /// <summary>
        /// 快捷方法
        /// </summary>
        /// <typeparam name="TEntity1"></typeparam>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="joinOn"></param>
        /// <param name="joinType"></param>
        /// <returns></returns>
        public static JoinOn<TEntity1, TEntity2> Add<TEntity1, TEntity2>(Expression<Func<TEntity1, TEntity2, bool>> joinOn, JoinType joinType)
        {
            return new JoinOn<TEntity1, TEntity2>(joinOn, joinType);
        }
        /// <summary>
        /// Empty
        /// </summary>
        /// <returns></returns>
        public static JoinOn None
        {
            get
            {
                return new JoinOn();
            }
        }
    }

    /// <summary>
    /// LeftJoin
    /// </summary>
    public class LeftJoinOn : JoinOn
    {
        /// <summary>
        /// LeftJoin
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        public LeftJoinOn(string tableName, WhereExpression where) : base(tableName, where, JoinType.LeftJoin)
        {

        }

        /// <summary>
        /// 快捷方法
        /// </summary>
        /// <typeparam name="TEntity1"></typeparam>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="joinOn"></param>
        /// <returns></returns>
        public static JoinOn<TEntity1, TEntity2> Add<TEntity1, TEntity2>(Expression<Func<TEntity1, TEntity2, bool>> joinOn)
        {
            return Add(joinOn, JoinType.LeftJoin);
        }
    }
    /// <summary>
    /// RightJoin
    /// </summary>
    public class RightJoinOn : JoinOn
    {
        /// <summary>
        /// RightJoin
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        public RightJoinOn(string tableName, WhereExpression where) : base(tableName, where, JoinType.RightJoin)
        {

        }
        /// <summary>
        /// 快捷方法
        /// </summary>
        /// <typeparam name="TEntity1"></typeparam>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="joinOn"></param>
        /// <returns></returns>
        public static JoinOn<TEntity1, TEntity2> Add<TEntity1, TEntity2>(Expression<Func<TEntity1, TEntity2, bool>> joinOn)
        {
            return Add(joinOn, JoinType.RightJoin);
        }
    }
    /// <summary>
    /// InnerJoin
    /// </summary>
    public class InnerJoinOn : JoinOn
    {
        /// <summary>
        /// InnerJoin
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        public InnerJoinOn(string tableName, WhereExpression where) : base(tableName, where, JoinType.InnerJoin)
        {

        }

        /// <summary>
        /// 快捷方法
        /// </summary>
        /// <typeparam name="TEntity1"></typeparam>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="joinOn"></param>
        /// <returns></returns>
        public static JoinOn<TEntity1, TEntity2> Add<TEntity1, TEntity2>(Expression<Func<TEntity1, TEntity2, bool>> joinOn)
        {
            return Add(joinOn, JoinType.InnerJoin);
        }
    }
    /// <summary>
    /// FullJoin
    /// </summary>
    public class FullJoinOn : JoinOn
    {
        /// <summary>
        /// InnerJoin
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        public FullJoinOn(string tableName, WhereExpression where) : base(tableName, where, JoinType.FullJoin)
        {

        }

        /// <summary>
        /// 快捷方法
        /// </summary>
        /// <typeparam name="TEntity1"></typeparam>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="joinOn"></param>
        /// <returns></returns>
        public static JoinOn<TEntity1, TEntity2> Add<TEntity1, TEntity2>(Expression<Func<TEntity1, TEntity2, bool>> joinOn)
        {
            return Add(joinOn, JoinType.FullJoin);
        }
    }

    /// <summary>
    /// JoinOn
    /// </summary>
    /// <typeparam name="TEntity1"></typeparam>
    /// <typeparam name="TEntity2"></typeparam>
    public class JoinOn<TEntity1, TEntity2> : JoinOn
    {
        /// <summary>
        /// JoinOn
        /// </summary>
        /// <param name="where"></param>
        /// <param name="joinType"></param>
        public JoinOn(WhereExpression where, JoinType joinType)
        {
            _tableName = TableAttribute.GetTableName<TEntity2>();
            Add(_tableName, where, joinType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="joinOn"></param>
        /// <param name="joinType"></param>
        public JoinOn(Expression<Func<TEntity1, TEntity2, bool>> joinOn, JoinType joinType)
        {
            var where = ExpressionToOperation<TEntity1>.ToJoinWhere(joinOn);
            _tableName = TableAttribute.GetTableName<TEntity2>();
            Add(_tableName, where, joinType);
        }
    }
}
