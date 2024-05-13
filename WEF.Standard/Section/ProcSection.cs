/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2024
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

using WEF.Common;

namespace WEF.Section
{
    /// <summary>
    /// 执行存储过程
    /// </summary>
    /// <summary>
    /// 执行存储过程
    /// </summary>
    public class ProcSection : Section
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbSession"></param>
        /// <param name="procName"></param>
        public ProcSection(DBContext dbSession, string procName)
            : base(dbSession, procName)
        {
            Check.Require(procName, "procName", Check.NotNullOrEmpty);
            this._dbCommand = dbSession.Db.GetStoredProcCommand(procName);
        }

        /// <summary>
        /// 返回的参数
        /// </summary>
        private List<string> outParameters = new List<string>();

        /// <summary>
        /// 设置事务
        /// </summary>
        /// <param name="tran"></param>
        /// <returns></returns>
        public ProcSection SetDbTransaction(DbTransaction tran)
        {
            this._dbTransaction = tran;
            return this;
        }

        /// <summary>
        /// 存储过程参数不要加前缀
        /// </summary>
        protected bool isParameterSpecial
        {
            get
            {
                var name = _dbContext.Db.DbProvider.GetType().Name;
                return !(name == "SqlServerProvider"
                           || name == "SqlServer9Provider"
                           || name == "MsAccessProvider");
            }
        }

        /// <summary>
        /// 获取参数名字
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        protected string getParameterName(string parameterName)
        {
            Check.Require(parameterName, "parameterName", Check.NotNullOrEmpty);

            if (!isParameterSpecial)
            {
                return _dbContext.Db.DbProvider.BuildParameterName(parameterName);
            }
            else
            {
                return parameterName.TrimStart(_dbContext.Db.DbProvider.ParamPrefix);
            }


        }
        /// <summary>
        /// 返回存储过程返回值
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetReturnValues()
        {
            Dictionary<string, object> returnValues = new Dictionary<string, object>();
            foreach (string outParameter in outParameters)
            {
                returnValues.Add(outParameter, _dbCommand.Parameters[getParameterName(outParameter)].Value);
            }
            return returnValues;
        }

        #region 添加参数

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public ProcSection AddInParameter(string parameterName, DbType dbType, object value)
        {
            return AddInParameter(parameterName, dbType, 0, value);
        }

        /// <summary>
        /// 添加输入参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ProcSection AddInParameter(string parameterName, DbType dbType, int size, object value)
        {
            Check.Require(parameterName, "parameterName", Check.NotNullOrEmpty);
            Check.Require(dbType, "dbType", Check.NotNullOrEmpty);

            _dbContext.Db.AddInParameter(this._dbCommand, parameterName, dbType, size, value);
            _dbContext.Db.AddInParameter(this._dbCountCommand, parameterName, dbType, size, value);
            return this;
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ProcSection AddInParameter(string parameterName, object value)
        {
            Check.Require(parameterName, "parameterName", Check.NotNullOrEmpty);
            Check.Require(value, "value", Check.NotNullOrEmpty);
            return AddInParameter(parameterName, value.GetDbType(), 0, value);
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public ProcSection AddInParameter(IEnumerable<KeyValuePair<string, object>> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                this.AddInParameter(keyValuePair.Key, keyValuePair.Value.GetDbType(), 0, keyValuePair.Value);
            }
            return this;
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ProcSection AddInParameterWithModel<T>(T parameter) where T : class
        {
            Check.Require(parameter, "parameter", Check.NotNullOrEmpty);
            List<KeyValuePair<string, object>> keyValuePairs = new List<KeyValuePair<string, object>>();
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var value = DynamicCalls.GetPropertyGetter(property).Invoke(parameter);
                keyValuePairs.Add(new KeyValuePair<string, object>($"{property.Name}", value));
            }
            this.AddInParameter(keyValuePairs);
            return this;
        }


        /// <summary>
        /// 添加输出参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public ProcSection AddOutParameter(string parameterName, DbType dbType)
        {
            return AddOutParameter(parameterName, dbType, 0);
        }

        /// <summary>
        /// 添加输出参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ProcSection AddOutParameter(string parameterName, DbType dbType, int size)
        {
            Check.Require(parameterName, "parameterName", Check.NotNullOrEmpty);
            Check.Require(dbType, "dbType", Check.NotNullOrEmpty);

            _dbContext.Db.AddOutParameter(this._dbCommand, parameterName, dbType, size);
            _dbContext.Db.AddOutParameter(this._dbCountCommand, parameterName, dbType, size);
            outParameters.Add(parameterName);
            return this;
        }


        /// <summary>
        /// 添加输入输出参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ProcSection AddInputOutputParameter(string parameterName, DbType dbType, object value)
        {
            return AddInputOutputParameter(parameterName, dbType, 0, value);
        }


        /// <summary>
        /// 添加输入输出参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ProcSection AddInputOutputParameter(string parameterName, DbType dbType, int size, object value)
        {
            Check.Require(parameterName, "parameterName", Check.NotNullOrEmpty);
            Check.Require(dbType, "dbType", Check.NotNullOrEmpty);

            _dbContext.Db.AddInputOutputParameter(this._dbCommand, parameterName, dbType, size, value);
            _dbContext.Db.AddInputOutputParameter(this._dbCountCommand, parameterName, dbType, size, value);

            outParameters.Add(parameterName);

            return this;
        }

        /// <summary>
        /// 添加返回参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public ProcSection AddReturnValueParameter(string parameterName, DbType dbType)
        {
            return AddReturnValueParameter(parameterName, dbType, 0);
        }

        /// <summary>
        /// 添加返回参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ProcSection AddReturnValueParameter(string parameterName, DbType dbType, int size)
        {
            Check.Require(parameterName, "parameterName", Check.NotNullOrEmpty);
            Check.Require(dbType, "dbType", Check.NotNullOrEmpty);

            _dbContext.Db.AddReturnValueParameter(this._dbCommand, parameterName, dbType, size);
            _dbContext.Db.AddReturnValueParameter(this._dbCountCommand, parameterName, dbType, size);
            outParameters.Add(parameterName);
            return this;
        }

        #endregion

        #region 执行

        /// <summary>
        /// 操作参数名称
        /// </summary>
        protected void ExecuteBefore()
        {
            if (isParameterSpecial)
            {
                if (_dbCommand.Parameters != null && _dbCommand.Parameters.Count > 0)
                {
                    foreach (DbParameter dbpara in _dbCommand.Parameters)
                    {
                        if (!string.IsNullOrEmpty(dbpara.ParameterName))
                        {
                            dbpara.ParameterName = dbpara.ParameterName.TrimStart(_dbContext.Db.DbProvider.ParamPrefix);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 返回单个值
        /// </summary>
        /// <returns></returns>
        public override object ToScalar()
        {
            ExecuteBefore();

            return base.ToScalar();
        }

        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <returns></returns>
        public override IDataReader ToDataReader()
        {
            ExecuteBefore();

            return base.ToDataReader();
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <returns></returns>
        public override DataSet ToDataSet()
        {
            ExecuteBefore();

            return base.ToDataSet();
        }


        /// <summary>
        /// 执行ExecuteNonQuery
        /// </summary>
        /// <returns></returns>
        public override int ExecuteNonQuery()
        {
            ExecuteBefore();

            return base.ExecuteNonQuery();
        }

        #endregion

    }
}

