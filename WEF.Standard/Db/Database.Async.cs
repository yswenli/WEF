/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2024
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.120319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using WEF.Common;

namespace WEF.Db
{
    /// <summary>
    /// 数据库对象异步方法
    /// </summary>
    public sealed partial class Database
    {
        #region Async Methods

        /// <summary>
        /// 异步执行标量
        /// </summary>
        /// <param name="command">数据库命令</param>
        /// <returns>执行结果</returns>
        public async Task<object> ExecuteScalarAsync(DbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command), "数据库命令不能为空");

            if (string.IsNullOrEmpty(command.CommandText))
                throw new InvalidOperationException("命令文本不能为空");

            using (DbConnection connection = CreateConnection())
            {
                command.CommandTimeout = TimeOut;
                PrepareCommand(command, connection);
                var result = await command.ExecuteScalarAsync();
                return result == DBNull.Value ? null : result;
            }
        }

        /// <summary>
        /// 异步执行标量
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>执行结果</returns>
        public async Task<object> ExecuteScalarAsync(string sql)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                return await ExecuteScalarAsync(command);
            }
        }

        /// <summary>
        /// 异步执行标量
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="dbParameters">参数数组</param>
        /// <returns>执行结果</returns>
        public async Task<object> ExecuteScalarAsync(string sql, params DbParameter[] dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                if (dbParameters != null && dbParameters.Any())
                {
                    command.Parameters.AddRange(dbParameters);
                }
                return await ExecuteScalarAsync(command);
            }
        }

        /// <summary>
        /// 异步执行标量
        /// </summary>
        /// <param name="command">数据库命令</param>
        /// <param name="transaction">事务</param>
        /// <returns>执行结果</returns>
        public async Task<object> ExecuteScalarAsync(DbCommand command, DbTransaction transaction)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command), "数据库命令不能为空");

            if (string.IsNullOrEmpty(command.CommandText))
                throw new InvalidOperationException("命令文本不能为空");

            command.CommandTimeout = TimeOut;
            PrepareCommand(command, transaction);
            var result = await command.ExecuteScalarAsync();
            return result == DBNull.Value ? null : result;
        }

        /// <summary>
        /// 异步执行标量
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令文本</param>
        /// <returns>执行结果</returns>
        public async Task<object> ExecuteScalarAsync(CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return await ExecuteScalarAsync(command);
            }
        }

        /// <summary>
        /// 异步执行标量
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令文本</param>
        /// <returns>执行结果</returns>
        public async Task<object> ExecuteScalarAsync(DbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return await ExecuteScalarAsync(command, transaction);
            }
        }

        /// <summary>
        /// 异步执行非查询
        /// </summary>
        /// <param name="command">数据库命令</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteNonQueryAsync(DbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command), "数据库命令不能为空");

            if (string.IsNullOrEmpty(command.CommandText))
                throw new InvalidOperationException("命令文本不能为空");

            using (DbConnection connection = CreateConnection())
            {
                command.CommandTimeout = TimeOut;
                PrepareCommand(command, connection);
                return await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// 异步执行非查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                return await ExecuteNonQueryAsync(command);
            }
        }

        /// <summary>
        /// 异步执行非查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="dbParameters">参数数组</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, params DbParameter[] dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                if (dbParameters != null && dbParameters.Any())
                {
                    command.Parameters.AddRange(dbParameters);
                }
                return await ExecuteNonQueryAsync(command);
            }
        }

        /// <summary>
        /// 异步执行非查询
        /// </summary>
        /// <param name="command">数据库命令</param>
        /// <param name="transaction">事务</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteNonQueryAsync(DbCommand command, DbTransaction transaction)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command), "数据库命令不能为空");

            if (string.IsNullOrEmpty(command.CommandText))
                throw new InvalidOperationException("命令文本不能为空");

            command.CommandTimeout = TimeOut;
            PrepareCommand(command, transaction);
            return await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// 异步执行非查询
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令文本</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteNonQueryAsync(CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return await ExecuteNonQueryAsync(command);
            }
        }

        /// <summary>
        /// 异步执行非查询
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令文本</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteNonQueryAsync(DbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return await ExecuteNonQueryAsync(command, transaction);
            }
        }

        /// <summary>
        /// 异步执行非查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="transaction">事务</param>
        /// <param name="dbParameters">参数数组</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, DbTransaction transaction, params DbParameter[] dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                command.CommandTimeout = TimeOut;
                PrepareCommand(command, transaction);

                if (dbParameters != null && dbParameters.Any())
                {
                    command.Parameters.AddRange(dbParameters);
                }
                return await ExecuteNonQueryAsync(command, transaction);
            }
        }

        /// <summary>
        /// 异步执行非查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="transaction">事务</param>
        /// <param name="dbParameters">参数字典</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, DbTransaction transaction, Dictionary<string, object> dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                command.CommandTimeout = TimeOut;
                PrepareCommand(command, transaction);

                if (dbParameters != null)
                {
                    foreach (var keyValuePair in dbParameters)
                    {
                        AddParameter(command, keyValuePair.Key, keyValuePair.Value.GetDbType(), 0, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, keyValuePair.Value);
                    }
                }
                return await ExecuteNonQueryAsync(command, transaction);
            }
        }

        /// <summary>
        /// 异步执行非查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="dbParameters">参数字典</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, Dictionary<string, object> dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                command.CommandTimeout = TimeOut;

                if (dbParameters != null)
                {
                    foreach (var keyValuePair in dbParameters)
                    {
                        AddParameter(command, keyValuePair.Key, keyValuePair.Value.GetDbType(), 0, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, keyValuePair.Value);
                    }
                }
                return await ExecuteNonQueryAsync(command);
            }
        }

        /// <summary>
        /// 异步执行读取器
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<IDataReader> ExecuteReaderAsync(DbCommand command)
        {
            command.CommandTimeout = TimeOut;

            DbConnection connection = CreateConnection();

            PrepareCommand(command, connection);

            try
            {
                return await DoExecuteReaderAsync(command, CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                try
                {
                    connection.Close();
                }
                finally
                {
                    throw new Exception($"ExecuteReaderAsync 异常，ConnectionString:{ConnectionString}，CommandText:{command.CommandText}", ex);
                }
            }
        }

        /// <summary>
        /// 异步执行读取器
        /// </summary>
        /// <param name="command"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task<DbDataReader> ExecuteReaderAsync(DbCommand command, DbTransaction transaction)
        {
            command.CommandTimeout = TimeOut;
            PrepareCommand(command, transaction);
            return await DoExecuteReaderAsync(command, CommandBehavior.Default);
        }

        /// <summary>
        /// 异步执行读取器
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public async Task<IDataReader> ExecuteReaderAsync(CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return await ExecuteReaderAsync(command);
            }
        }

        /// <summary>
        /// 异步执行读取器
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public async Task<IDataReader> ExecuteReaderAsync(string sql, params DbParameter[] dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                if (dbParameters != null && dbParameters.Any())
                {
                    command.Parameters.AddRange(dbParameters);
                }

                return await ExecuteReaderAsync(command);
            }
        }

        /// <summary>
        /// 异步执行读取器
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public async Task<DbDataReader> ExecuteReaderAsync(DbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return await ExecuteReaderAsync(command, transaction);
            }
        }

        /// <summary>
        /// 异步执行读取器
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public async Task<DbDataReader> ExecuteReaderAsync(string sql, DbTransaction transaction, Dictionary<string, object> dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                command.CommandTimeout = TimeOut;

                using (DbConnection connection = CreateConnection())
                {
                    PrepareCommand(command, transaction);

                    if (dbParameters != null)
                    {
                        foreach (var keyValuePair in dbParameters)
                        {
                            AddParameter(command, keyValuePair.Key, keyValuePair.Value.GetDbType(), 0, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, keyValuePair.Value);
                        }
                    }
                    return await DoExecuteReaderAsync(command, CommandBehavior.Default);
                }
            }
        }

        /// <summary>
        /// 异步执行读取器
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public async Task<DbDataReader> ExecuteReaderAsync(string sql, Dictionary<string, object> dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                command.CommandTimeout = TimeOut;

                using (DbConnection connection = CreateConnection())
                {
                    PrepareCommand(command, connection);

                    if (dbParameters != null)
                    {
                        foreach (var keyValuePair in dbParameters)
                        {
                            AddParameter(command, keyValuePair.Key, keyValuePair.Value.GetDbType(), 0, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, keyValuePair.Value);
                        }
                    }
                    return await DoExecuteReaderAsync(command, CommandBehavior.Default);
                }
            }
        }

        /// <summary>
        /// 异步执行数据表
        /// </summary>
        /// <param name="command">数据库命令</param>
        /// <returns>数据表</returns>
        public async Task<DataTable> ExecuteDataTableAsync(DbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command), "数据库命令不能为空");

            if (string.IsNullOrEmpty(command.CommandText))
                throw new InvalidOperationException("命令文本不能为空");

            using (DbConnection connection = CreateConnection())
            {
                command.CommandTimeout = TimeOut;
                PrepareCommand(command, connection);
                var dataSet = await ExecuteDataSetAsync(command);
                return dataSet?.Tables.Count > 0 ? dataSet.Tables[0] : null;
            }
        }

        /// <summary>
        /// 异步执行数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="dbParameters">参数数组</param>
        /// <returns>数据表</returns>
        public async Task<DataTable> ExecuteDataTableAsync(string sql, params DbParameter[] dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                if (dbParameters != null && dbParameters.Any())
                {
                    command.Parameters.AddRange(dbParameters);
                }
                return await ExecuteDataTableAsync(command);
            }
        }

        /// <summary>
        /// 异步执行数据表
        /// </summary>
        /// <param name="command">数据库命令</param>
        /// <param name="transaction">事务</param>
        /// <returns>数据表</returns>
        public async Task<DataTable> ExecuteDataTableAsync(DbCommand command, DbTransaction transaction)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command), "数据库命令不能为空");

            if (string.IsNullOrEmpty(command.CommandText))
                throw new InvalidOperationException("命令文本不能为空");

            command.CommandTimeout = TimeOut;
            PrepareCommand(command, transaction);
            var dataSet = await ExecuteDataSetAsync(command, transaction);
            return dataSet?.Tables.Count > 0 ? dataSet.Tables[0] : null;
        }

        /// <summary>
        /// 异步执行数据表
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令文本</param>
        /// <returns>数据表</returns>
        public async Task<DataTable> ExecuteDataTableAsync(CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return await ExecuteDataTableAsync(command);
            }
        }

        /// <summary>
        /// 异步执行数据表
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令文本</param>
        /// <returns>数据表</returns>
        public async Task<DataTable> ExecuteDataTableAsync(DbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return await ExecuteDataTableAsync(command, transaction);
            }
        }

        /// <summary>
        /// 异步执行数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="dbParameters">参数字典</param>
        /// <returns>数据表</returns>
        public async Task<DataTable> ExecuteDataTableAsync(string sql, Dictionary<string, object> dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                command.CommandTimeout = TimeOut;

                if (dbParameters != null)
                {
                    foreach (var keyValuePair in dbParameters)
                    {
                        AddParameter(command, keyValuePair.Key, keyValuePair.Value.GetDbType(), 0, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, keyValuePair.Value);
                    }
                }
                return await ExecuteDataTableAsync(command);
            }
        }

        /// <summary>
        /// 异步执行数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="transaction">事务</param>
        /// <param name="dbParameters">参数字典</param>
        /// <returns>数据表</returns>
        public async Task<DataTable> ExecuteDataTableAsync(string sql, DbTransaction transaction, Dictionary<string, object> dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                command.CommandTimeout = TimeOut;
                PrepareCommand(command, transaction);

                if (dbParameters != null)
                {
                    foreach (var keyValuePair in dbParameters)
                    {
                        AddParameter(command, keyValuePair.Key, keyValuePair.Value.GetDbType(), 0, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, keyValuePair.Value);
                    }
                }
                return await ExecuteDataTableAsync(command, transaction);
            }
        }

        /// <summary>
        /// 异步执行数据表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="transaction">事务</param>
        /// <param name="dbParameters">参数数组</param>
        /// <returns>数据表</returns>
        public async Task<DataTable> ExecuteDataTableAsync(string sql, DbTransaction transaction, params DbParameter[] dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                command.CommandTimeout = TimeOut;
                PrepareCommand(command, transaction);

                if (dbParameters != null && dbParameters.Any())
                {
                    command.Parameters.AddRange(dbParameters);
                }
                return await ExecuteDataTableAsync(command, transaction);
            }
        }

        /// <summary>
        /// 异步执行数据表
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令文本</param>
        /// <param name="dbParameters">参数数组</param>
        /// <returns>数据表</returns>
        public async Task<DataTable> ExecuteDataTableAsync(CommandType commandType, string commandText, params DbParameter[] dbParameters)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                command.CommandTimeout = TimeOut;

                if (dbParameters != null && dbParameters.Any())
                {
                    command.Parameters.AddRange(dbParameters);
                }
                return await ExecuteDataTableAsync(command);
            }
        }

        /// <summary>
        /// 异步执行数据表
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令文本</param>
        /// <param name="dbParameters">参数数组</param>
        /// <returns>数据表</returns>
        public async Task<DataTable> ExecuteDataTableAsync(DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] dbParameters)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                command.CommandTimeout = TimeOut;
                PrepareCommand(command, transaction);

                if (dbParameters != null && dbParameters.Any())
                {
                    command.Parameters.AddRange(dbParameters);
                }
                return await ExecuteDataTableAsync(command, transaction);
            }
        }

        #endregion

        #region Private Async Methods

        private async Task<int> DoExecuteNonQueryAsync(DbCommand command)
        {
            try
            {
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"DoExecuteNonQueryAsync 异常，ConnectionString:{ConnectionString} CommandText:{command.CommandText}", ex);
            }
        }       

        #endregion
    }
} 