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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;

using WEF.Common;
using WEF.Expressions;
using WEF.Provider;

namespace WEF.Db
{

    /// <summary>
    /// 下拉框选择
    /// </summary>
    public sealed class Database : ILogable
    {

        private DbProvider _dbProvider;

        /// <summary>
        /// Default Database
        /// </summary>
        public static Database Default = new Database(ProviderFactory.Default);

        /// <summary>
        /// 设置操作超时间
        /// </summary>
        public int TimeOut
        {
            get; set;
        }

        /// <summary>
        /// 下拉框选择
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <param name="timeout"></param>
        public Database(DbProvider dbProvider, int timeout = 30)
        {
            this._dbProvider = dbProvider;
            TimeOut = timeout;
        }

        #region Properties

        /// <summary>
        /// Gets the connect string.
        /// </summary>
        /// <value>The connect string.</value>
        public string ConnectionString
        {
            get
            {
                return _dbProvider.ConnectionString;
            }
        }

        /// <summary>
        /// Gets the DbProviderFactory
        /// </summary>
        public DbProviderFactory DbProviderFactory
        {
            get
            {
                return _dbProvider.DbProviderFactory;
            }
        }

        /// <summary>
        /// Gets the db provider.
        /// </summary>
        /// <value>The db provider.</value>
        public DbProvider DbProvider
        {
            get
            {
                return _dbProvider;
            }
            set
            {

            }
        }

        /// <summary>
        /// 命令令超时时间 30s
        /// </summary>
        public int CommandTimeout
        {
            get
            {
                return TimeOut;
            }
            set
            {
                TimeOut = value;
            }
        }

        #endregion

        #region Log

        /// <summary>
        /// OnLog event.
        /// </summary>
        public event LogHandler OnLog;

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="command">The command.</param>
        public void WriteLog(DbCommand command)
        {
            if (OnLog != null)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(string.Format("{0}:\t{1}\t\r\n", command.CommandType, command.CommandText));
                if (command.Parameters != null && command.Parameters.Count > 0)
                {
                    sb.Append("Parameters:\r\n");
                    foreach (DbParameter p in command.Parameters)
                    {
                        sb.Append(string.Format("{0}[{2}] = {1}\r\n", p.ParameterName, p.Value, p.DbType));
                    }
                }
                sb.Append("\r\n");

                OnLog(sb.ToString());
            }
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="logMsg">The log MSG.</param>
        public void WriteLog(string logMsg)
        {
            if (OnLog != null)
            {
                OnLog(logMsg);
            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// CreateCommandByCommandType
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DbCommand CreateCommandByCommandType(CommandType commandType, string commandText)
        {
            DbCommand command = _dbProvider.DbProviderFactory.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;
            command.CommandTimeout = TimeOut;
            return command;
        }
        /// <summary>
        /// CreateCommandByCommandType
        /// </summary>
        /// <param name="timeOut"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DbCommand CreateCommandByCommandType(int timeOut, CommandType commandType, string commandText)
        {
            DbCommand command = _dbProvider.DbProviderFactory.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;
            command.CommandTimeout = timeOut;
            return command;
        }

        private DbCommand CreateCommandByCommandType(CommandType commandType, string commandText, int pageIndex, int pageSize, string orderBy, bool asc = true)
        {
            if (_dbProvider.DbProviderFactory.GetType().Name == "SqlClientFactory")
            {
                commandText = $"SELECT * FROM(SELECT ROW_NUMBER() over(order by {orderBy} {(asc ? "asc" : "desc")}) rowNO, * From({commandText}) queryData) pagerows WHERE pagerows.rowNO>={(pageIndex - 1) * pageSize + 1} and pagerows.rowNO<= {pageIndex * pageSize}";
            }
            else if (_dbProvider.DbProviderFactory.GetType().Name == "OracleClientFactory")
            {
                commandText = $"SELECT * FROM (SELECT queryData.*, ROWNUM rowNO FROM ({commandText} order by {orderBy} {(asc ? "asc" : "desc")}) queryData WHERE rowNO <= {pageIndex * pageSize}) WHERE rowNO >= {(pageIndex - 1) * pageSize + 1}";
            }
            else
            {
                commandText = $"{commandText} order by {orderBy} {(asc ? "asc" : "desc")} limit {pageSize} offset {(pageIndex - 1) * pageSize}";
            }
            DbCommand command = _dbProvider.DbProviderFactory.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;
            command.CommandTimeout = TimeOut;
            return command;
        }

        private DbCommand CreateCommandByCommandType(CommandType commandType, string commandText, int pageIndex, int pageSize, Dictionary<string, OrderByOperater> orderBys)
        {
            StringBuilder orderByBuilder = new StringBuilder();
            foreach (var item in orderBys)
            {
                orderByBuilder.Append($",{item.Key} {item.Value}");
            }
            var orderStr = orderByBuilder.ToString(1, orderByBuilder.Length - 1);

            if (_dbProvider.DbProviderFactory.GetType().Name == "SqlClientFactory")
            {
                commandText = $"SELECT * FROM(SELECT ROW_NUMBER() over(order by {orderStr}) rowNO, * From({commandText}) queryData) pagerows WHERE pagerows.rowNO>={(pageIndex - 1) * pageSize + 1} and pagerows.rowNO<= {pageIndex * pageSize}";
            }
            else if (_dbProvider.DbProviderFactory.GetType().Name == "OracleClientFactory")
            {
                commandText = $"SELECT * FROM (SELECT queryData.*, ROWNUM rowNO FROM ({commandText} order by {orderStr}) queryData WHERE rowNO <= {pageIndex * pageSize}) WHERE rowNO >= {(pageIndex - 1) * pageSize + 1}";
            }
            else
            {
                commandText = $"{commandText} order by {orderStr} limit {pageSize} offset {(pageIndex - 1) * pageSize}";
            }
            DbCommand command = _dbProvider.DbProviderFactory.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;
            command.CommandTimeout = TimeOut;
            return command;
        }

        private void DoLoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames)
        {
            Check.Require(tableNames != null && tableNames.Length > 0, "tableNames could not be null or empty.");

            Check.Require(dataSet != null, "dataSet could not be null.");

            using (DbDataAdapter adapter = GetDataAdapter())
            {
                WriteLog(command);

                ((IDbDataAdapter)adapter).SelectCommand = command;

                string systemCreatedTableNameRoot = "Table";
                for (int i = 0; i < tableNames.Length; i++)
                {
                    string systemCreatedTableName = (i == 0)
                         ? systemCreatedTableNameRoot
                         : systemCreatedTableNameRoot + i;

                    adapter.TableMappings.Add(systemCreatedTableName, tableNames[i]);
                }

                adapter.Fill(dataSet);
            }
        }

        private object DoExecuteScalar(DbCommand command)
        {
            try
            {
                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception($"DoExecuteScalar 异常，ConnectionString:{ConnectionString} CommandText:{command.CommandText}", ex);
            }
        }

        private int DoExecuteNonQuery(DbCommand command)
        {
            try
            {
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"DoExecuteScalar 异常，ConnectionString:{ConnectionString} CommandText:{command.CommandText}", ex);
            }
        }

        private IDataReader DoExecuteReader(DbCommand command, CommandBehavior cmdBehavior)
        {
            try
            {
                return command.ExecuteReader(cmdBehavior);
            }
            catch (Exception ex)
            {
                throw new Exception($"DoExecuteReader 异常， ConnectionString:{ConnectionString} CommandText:{command.CommandText}", ex);
            }
        }


        private DbTransaction BeginTransaction(DbConnection connection)
        {
            return connection.BeginTransaction();
        }


        private IDbTransaction BeginTransaction(DbConnection connection, IsolationLevel il)
        {
            return connection.BeginTransaction(il);
        }


        /// <summary>
        /// PrepareCommand
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        public void PrepareCommand(DbCommand command, DbConnection connection)
        {
            Check.Require(command != null, "command could not be null.");
            Check.Require(connection != null, "connection could not be null.");

            command.Connection = connection;
            _dbProvider.PrepareCommand(command);

        }

        private void PrepareCommand(DbCommand command, DbTransaction transaction)
        {
            Check.Require(command != null, "command could not be null.");
            Check.Require(transaction != null, "transaction could not be null.");

            PrepareCommand(command, transaction.Connection);
            command.Transaction = transaction;

        }

        /// <summary>
        /// ConfigureParameter
        /// </summary>
        /// <param name="param"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <param name="nullable"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        /// <param name="value"></param>
        public static void ConfigureParameter(DbParameter param, string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            param.DbType = dbType;
            param.Size = size;
            param.Value = value ?? DBNull.Value;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
        }

        /// <summary>
        /// CreateParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <param name="nullable"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            DbParameter param = CreateParameter(name);
            ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return param;
        }

        /// <summary>
        /// CreateParameter
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        DbParameter CreateParameter(string name)
        {
            DbParameter param = _dbProvider.DbProviderFactory.CreateParameter();

            param.ParameterName = _dbProvider.BuildParameterName(name);

            return param;
        }



        #endregion

        #region Close Connection

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="command">The command.</param>
        public void CloseConnection(DbCommand command)
        {
            if (command != null && command.Connection.State != ConnectionState.Closed)
            {
                if (command.Transaction == null)
                {
                    CloseConnection(command.Connection);
                    command.Dispose();
                }
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="conn">The conn.</param>
        public void CloseConnection(DbConnection conn)
        {
            if (conn != null && conn.State != ConnectionState.Closed)
                try
                {
                    conn.Close();
                    conn.Dispose();
                }
                catch
                {
                }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="tran">The tran.</param>
        public void CloseConnection(DbTransaction tran)
        {
            if (tran.Connection != null)
            {
                CloseConnection(tran.Connection);
                tran.Dispose();
            }
        }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection()
        {
            return CreateConnection();
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <param name="tryOpen">if set to <c>true</c> [try open].</param>
        /// <returns></returns>
        public DbConnection GetConnection(bool tryOpen = true)
        {
            return CreateConnection(tryOpen);
        }

        /// <summary>
        /// <para>When overridden in a derived class, gets the connection for this database.</para>
        /// <seealso cref="DbConnection"/>        
        /// </summary>
        /// <returns>
        /// <para>The <see cref="DbConnection"/> for this database.</para>
        /// </returns>
        public DbConnection CreateConnection()
        {
            DbConnection newConnection = _dbProvider.DbProviderFactory.CreateConnection();

            newConnection.ConnectionString = ConnectionString;

            return newConnection;
        }

        /// <summary>
        /// <para>When overridden in a derived class, gets the connection for this database.</para>
        /// <seealso cref="DbConnection"/>        
        /// </summary>
        /// <returns>
        /// <para>The <see cref="DbConnection"/> for this database.</para>
        /// </returns>
        public DbConnection CreateConnection(bool tryOpenning)
        {
            if (!tryOpenning)
            {
                return CreateConnection();
            }

            DbConnection connection = null;
            try
            {
                connection = CreateConnection();
                connection.Open();
            }
            catch (Exception ex)
            {
                try
                {
                    connection.Close();
                }
                catch
                {
                }

                throw new Exception($"CreateConnection 异常， ConnectionString:{ConnectionString}", ex);
            }

            return connection;
        }

        /// <summary>
        /// <para>When overridden in a derived class, creates a <see cref="DbCommand"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <returns><para>The <see cref="DbCommand"/> for the stored procedure.</para></returns>       
        public DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            Check.Require(!string.IsNullOrEmpty(storedProcedureName), "storedProcedureName could not be null.");

            return CreateCommandByCommandType(CommandType.StoredProcedure, storedProcedureName);
        }

        /// <summary>
        /// <para>When overridden in a derived class, creates an <see cref="DbCommand"/> for a SQL query.</para>
        /// </summary>
        /// <param name="query"><para>The text of the query.</para></param>        
        /// <returns><para>The <see cref="DbCommand"/> for the SQL query.</para></returns>        
        public DbCommand GetSqlStringCommand(string query)
        {
            Check.Require(!string.IsNullOrEmpty(query), "query could not be null.");

            return CreateCommandByCommandType(CommandType.Text, query);
        }

        /// <summary>
        /// 创建Command
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public DbCommand GetSqlStringCommand(string query, int pageIndex, int pageSize, string orderBy, bool asc = true)
        {
            Check.Require(!string.IsNullOrEmpty(query), "query could not be null.");

            return CreateCommandByCommandType(CommandType.Text, query, pageIndex, pageSize, orderBy, asc);
        }

        /// <summary>
        /// 创建Command
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBys"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DbCommand GetSqlStringCommand(string query, int pageIndex, int pageSize, Dictionary<string, OrderByOperater> orderBys)
        {
            Check.Require(!string.IsNullOrEmpty(query), "query could not be null.");

            return CreateCommandByCommandType(CommandType.Text, query, pageIndex, pageSize, orderBys);
        }

        /// <summary>
        /// Gets a DbDataAdapter with Standard update behavior.
        /// </summary>
        /// <returns>A <see cref="DbDataAdapter"/>.</returns>
        /// <seealso cref="DbDataAdapter"/>
        public DbDataAdapter GetDataAdapter()
        {
            return _dbProvider.DbProviderFactory.CreateDataAdapter();
        }


        #endregion

        #region Load & Execute Methods

        /// <summary>
        /// <para>Loads a <see cref="DataSet"/> from command text in a transaction.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command in.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        public void LoadDataSet(DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                LoadDataSet(command, dataSet, tableNames, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>        
        public IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteReader(command);
            }
        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, params DbParameter[] dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbConnection connection = GetConnection(true))
            {
                using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
                {
                    PrepareCommand(command, connection);

                    if (dbParameters != null && dbParameters.Any())
                    {
                        command.Parameters.AddRange(dbParameters);
                    }

                    return ExecuteReader(command);
                }
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> within the given 
        /// <paramref name="transaction" /> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>        
        public IDataReader ExecuteReader(DbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteReader(command, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and adds a new <see cref="DataTable"></see> to the existing <see cref="DataSet"></see>.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="DbCommand"/> to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to load.</para>
        /// </param>
        /// <param name="tableName">
        /// <para>The name for the new <see cref="DataTable"/> to add to the <see cref="DataSet"/>.</para>
        /// </param>        
        /// <exception cref="System.ArgumentNullException">Any input parameter was <see langword="null"/> (<b>Nothing</b> in Visual Basic)</exception>
        /// <exception cref="System.ArgumentException">tableName was an empty string</exception>
        public void LoadDataSet(DbCommand command, DataSet dataSet, string tableName)
        {
            LoadDataSet(command, dataSet, new string[] { tableName });
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> within the given <paramref name="transaction" /> and adds a new <see cref="DataTable"></see> to the existing <see cref="DataSet"></see>.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="DbCommand"/> to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to load.</para>
        /// </param>
        /// <param name="tableName">
        /// <para>The name for the new <see cref="DataTable"/> to add to the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>        
        /// <exception cref="System.ArgumentNullException">Any input parameter was <see langword="null"/> (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.ArgumentException">tableName was an empty string.</exception>
        public void LoadDataSet(DbCommand command, DataSet dataSet, string tableName, DbTransaction transaction)
        {
            LoadDataSet(command, dataSet, new string[] { tableName }, transaction);
        }

        /// <summary>
        /// <para>Loads a <see cref="DataSet"/> from a <see cref="DbCommand"/>.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command to execute to fill the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        public void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames)
        {
            using (DbConnection connection = GetConnection())
            {
                PrepareCommand(command, connection);
                DoLoadDataSet(command, dataSet, tableNames);
            }
        }

        /// <summary>
        /// <para>Loads a <see cref="DataSet"/> from a <see cref="DbCommand"/> in  a transaction.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command to execute to fill the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command in.</para>
        /// </param>
        public void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames, DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            DoLoadDataSet(command, dataSet, tableNames);
        }

        /// <summary>
        /// <para>Loads a <see cref="DataSet"/> from command text.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        public void LoadDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                LoadDataSet(command, dataSet, tableNames);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="command"><para>The <see cref="DbCommand"/> to execute.</para></param>
        /// <returns>A <see cref="DataSet"/> with the results of the <paramref name="command"/>.</returns>        
        public DataSet ExecuteDataSet(DbCommand command)
        {
            DataSet dataSet = new DataSet();
            dataSet.Locale = CultureInfo.InvariantCulture;
            LoadDataSet(command, dataSet, "Table");
            return dataSet;
        }

        /// <summary>
        /// 执行sql 获取DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql, params DbParameter[] dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);
            using (DbConnection connection = GetConnection(true))
            {
                using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
                {
                    PrepareCommand(command, connection);
                    if (dbParameters != null && dbParameters.Any())
                    {
                        command.Parameters.AddRange(dbParameters);
                    }
                    DataSet dataSet = new DataSet();
                    dataSet.Locale = CultureInfo.InvariantCulture;
                    LoadDataSet(command, dataSet, "Table");
                    return dataSet;
                }
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> as part of the <paramref name="transaction" /> and returns the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="command"><para>The <see cref="DbCommand"/> to execute.</para></param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <returns>A <see cref="DataSet"/> with the results of the <paramref name="command"/>.</returns>        
        public DataSet ExecuteDataSet(DbCommand command, DbTransaction transaction)
        {
            DataSet dataSet = new DataSet();
            dataSet.Locale = CultureInfo.InvariantCulture;
            LoadDataSet(command, dataSet, "Table", transaction);
            return dataSet;
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> and returns the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="DataSet"/> with the results of the <paramref name="commandText"/>.</para>
        /// </returns>
        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteDataSet(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> as part of the given <paramref name="transaction" /> and returns the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="DataSet"/> with the results of the <paramref name="commandText"/>.</para>
        /// </returns>
        public DataSet ExecuteDataSet(DbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteDataSet(command, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the result set.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public object ExecuteScalar(DbCommand command)
        {
            using (DbConnection connection = GetConnection(true))
            {
                command.CommandTimeout = TimeOut;
                PrepareCommand(command, connection);
                return DoExecuteScalar(command);
            }
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbConnection connection = GetConnection(true))
            {
                using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
                {
                    command.CommandTimeout = TimeOut;
                    PrepareCommand(command, connection);
                    return DoExecuteScalar(command);
                }
            }
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params DbParameter[] dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbConnection connection = GetConnection(true))
            {
                using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
                {
                    command.CommandTimeout = TimeOut;

                    if (dbParameters != null && dbParameters.Any())
                    {
                        command.Parameters.AddRange(dbParameters);
                    }

                    PrepareCommand(command, connection);
                    return DoExecuteScalar(command);
                }
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> within a <paramref name="transaction" />, and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the result set.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public object ExecuteScalar(DbCommand command, DbTransaction transaction)
        {
            command.CommandTimeout = TimeOut;
            PrepareCommand(command, transaction);
            return DoExecuteScalar(command);
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" />  and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the result set.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                command.CommandTimeout = TimeOut;
                return ExecuteScalar(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> 
        /// within the given <paramref name="transaction" /> and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the result set.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public object ExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                command.CommandTimeout = TimeOut;
                return ExecuteScalar(command, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>       
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public int ExecuteNonQuery(DbCommand command)
        {
            command.CommandTimeout = TimeOut;

            using (DbConnection connection = GetConnection(true))
            {
                PrepareCommand(command, connection);
                return DoExecuteNonQuery(command);
            }
        }


        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                command.CommandTimeout = TimeOut;

                using (DbConnection connection = GetConnection(true))
                {
                    PrepareCommand(command, connection);
                    return DoExecuteNonQuery(command);
                }
            }
        }
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params DbParameter[] dbParameters)
        {
            Check.Require(sql, "sql", Check.NotNullOrEmpty);

            using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
            {
                command.CommandTimeout = TimeOut;

                using (DbConnection connection = GetConnection(true))
                {
                    PrepareCommand(command, connection);

                    if (dbParameters != null && dbParameters.Any())
                    {
                        command.Parameters.AddRange(dbParameters);
                    }
                    return DoExecuteNonQuery(command);
                }
            }
        }
        /// <summary>
        /// <para>Executes the <paramref name="command"/> within the given <paramref name="transaction" />, and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public int ExecuteNonQuery(DbCommand command, DbTransaction transaction)
        {
            command.CommandTimeout = TimeOut;
            PrepareCommand(command, transaction);
            return DoExecuteNonQuery(command);
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The number of rows affected.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                command.CommandTimeout = TimeOut;
                return ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> as part of the given <paramref name="transaction" /> and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The number of rows affected</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                command.CommandTimeout = TimeOut;
                return ExecuteNonQuery(command, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>        
        public IDataReader ExecuteReader(DbCommand command)
        {
            command.CommandTimeout = TimeOut;

            DbConnection connection = GetConnection(true);

            PrepareCommand(command, connection);

            try
            {
                return DoExecuteReader(command, CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                try
                {
                    connection.Close();
                }
                catch
                {

                }

                throw new Exception($"ExecuteReader 异常，ConnectionString:{ConnectionString}，CommandText:{command.CommandText}", ex);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> within a transaction and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>        
        public IDataReader ExecuteReader(DbCommand command, DbTransaction transaction)
        {
            command.CommandTimeout = TimeOut;
            PrepareCommand(command, transaction);
            return DoExecuteReader(command, CommandBehavior.Default);
        }

        /// <summary>
        /// 为adapter生成其他操作命令
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="transaction"></param>
        public void PrepareAdapter(DbDataAdapter adapter, DbTransaction transaction)
        {
            var commandBuilder = _dbProvider.DbProviderFactory.CreateCommandBuilder();
            commandBuilder.DataAdapter = adapter;
            commandBuilder.ConflictOption = ConflictOption.OverwriteChanges;
            commandBuilder.SetAllValues = false;
            adapter.InsertCommand = commandBuilder.GetInsertCommand();
            adapter.UpdateCommand = commandBuilder.GetUpdateCommand();
            adapter.DeleteCommand = commandBuilder.GetDeleteCommand();
            if (transaction != null)
            {
                adapter.InsertCommand.Transaction = transaction;
                adapter.UpdateCommand.Transaction = transaction;
                adapter.DeleteCommand.Transaction = transaction;
            }
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sourceDataTable"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public int BulkInsert(string tableName, DataTable sourceDataTable, int timeOut = 180)
        {
            Check.Require(tableName, "tableName", Check.NotNullOrEmpty);

            if (sourceDataTable == null || sourceDataTable.Rows == null || sourceDataTable.Rows.Count < 1)
                return -1;

            try
            {
                sourceDataTable.Locale = CultureInfo.InvariantCulture;

                using (DbConnection connection = GetConnection(true))
                {
                    var selectCommand = CreateCommandByCommandType(timeOut, CommandType.Text, $"select * from {tableName} where 1=1");

                    PrepareCommand(selectCommand, connection);

                    using (var transction = connection.BeginTransaction())
                    {
                        selectCommand.Transaction = transction;

                        using (DbDataAdapter adapter = GetDataAdapter())
                        {
                            DataTable targetDataTable = new DataTable(tableName);

                            targetDataTable.Locale = CultureInfo.InvariantCulture;

                            try
                            {
                                adapter.SelectCommand = selectCommand;

                                adapter.Fill(targetDataTable);

                                PrepareAdapter(adapter, transction);

                                var columns1 = sourceDataTable.Columns;

                                var columns2 = targetDataTable.Columns;

                                for (int i = 0; i < sourceDataTable.Rows.Count; i++)
                                {
                                    var newRow = targetDataTable.NewRow();

                                    for (int j = 0; j < columns1.Count; j++)
                                    {
                                        if (sourceDataTable.Rows[i][j] == null || sourceDataTable.Rows[i][j] is DBNull)
                                        {
                                            continue;
                                        }
                                        if (columns2[j].DataType.Name == "MySqlDateTime")
                                        {
                                            var datetime = sourceDataTable.Rows[i][j];
                                            if (datetime != null)
                                            {
                                                var dtVal = (DateTime)datetime;
                                                newRow[j] = new MySql.Data.Types.MySqlDateTime(dtVal);
                                            }
                                        }
                                        else if (columns2[j].DataType.Name == "Image" || columns2[j].DataType.Name == "Byte[]" || columns2[j].DataType.Name == "Timespan")
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            newRow[j] = sourceDataTable.Rows[i][j];
                                        }
                                    }

                                    targetDataTable.Rows.Add(newRow);
                                }

                                var count = adapter.Update(targetDataTable);

                                transction.Commit();

                                return count;
                            }
                            catch (Exception)
                            {
                                transction.Rollback();
                                throw;
                            }
                            finally
                            {
                                targetDataTable.Clear();
                            }
                        }
                    }
                }
            }
            finally
            {
                sourceDataTable.Clear();
            }
        }
        #endregion

        #region Transactions

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginTransaction()
        {
            return GetConnection(true).BeginTransaction();
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="il">The il.</param>
        /// <returns></returns>
        public DbTransaction BeginTransaction(IsolationLevel il)
        {
            return GetConnection(true).BeginTransaction(il);
        }

        #endregion

        #region DbCommand Parameter Methods

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>Avalue indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>       
        public void AddParameter(DbCommand command, string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            command.CommandTimeout = TimeOut;
            DbParameter parameter = CreateParameter(name, dbType == DbType.Object ? DbType.String : dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>        
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>    
        public void AddParameter(DbCommand command, string name, DbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            command.CommandTimeout = TimeOut;
            AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
        }

        /// <summary>
        /// Adds a new Out <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the out parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>        
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>        
        public void AddOutParameter(DbCommand command, string name, DbType dbType, int size)
        {
            command.CommandTimeout = TimeOut;
            AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the in parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <remarks>
        /// <para>This version of the method is used when you can have the same parameter object multiple times with different values.</para>
        /// </remarks>        
        public void AddInParameter(DbCommand command, string name, DbType dbType)
        {
            command.CommandTimeout = TimeOut;
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The commmand to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public void AddInParameter(DbCommand command, string name, DbType dbType, object value)
        {
            command.CommandTimeout = TimeOut;
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The commmand to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>       
        /// <param name="size">size</param>
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public void AddInParameter(DbCommand command, string name, DbType dbType, int size, object value)
        {
            command.CommandTimeout = TimeOut;
            AddParameter(command, name, dbType, size, ParameterDirection.Input, true, 0, 0, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The commmand to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public void AddInParameter(DbCommand command, string name, object value)
        {
            command.CommandTimeout = TimeOut;
            AddParameter(command, name, DbType.Object, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the value.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        public void AddInParameter(DbCommand command, string name, DbType dbType, string sourceColumn, DataRowVersion sourceVersion)
        {
            command.CommandTimeout = TimeOut;
            AddParameter(command, name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, null);
        }


        /// <summary>
        /// Adds a new In and Out
        /// </summary>
        public void AddInputOutputParameter(DbCommand command, string name, DbType dbType, int size, object value)
        {
            command.CommandTimeout = TimeOut;
            AddParameter(command, name, dbType, size, ParameterDirection.InputOutput, true, 0, 0, String.Empty, DataRowVersion.Default, value);
        }


        /// <summary>
        /// Adds a new return
        /// </summary>
        public void AddReturnValueParameter(DbCommand command, string name, DbType dbType, int size)
        {
            command.CommandTimeout = TimeOut;
            AddParameter(command, name, dbType, size, ParameterDirection.ReturnValue, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }


        /// <summary>
        /// Adds parameters
        /// </summary>
        public void AddParameter(DbCommand command, params DbParameter[] parameters)
        {
            command.CommandTimeout = TimeOut;

            if (null == parameters || parameters.Length == 0)
                return;
            foreach (DbParameter p in parameters)
            {
                p.ParameterName = _dbProvider.BuildParameterName(p.ParameterName);
                command.Parameters.Add(p);
            }

        }


        /// <summary>
        /// 给命令添加参数  where paramters
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal DbCommand AddCommandParameter(DbCommand command, params Parameter[] parameters)
        {
            command.CommandTimeout = TimeOut;

            if (null == parameters || parameters.Length == 0)
                return command;
            //var i = 0;
            foreach (Parameter p in parameters)
            {
                DbParameter dbParameter = CreateParameter(p.ParameterName);// + i
                dbParameter.Value = p.ParameterValue;
                //if (p.ParameterDbType.HasValue)
                dbParameter.DbType = p.ParameterDbType;
                //if (p.ParameterSize.HasValue)
                dbParameter.Size = p.ParameterSize;
                command.Parameters.Add(dbParameter);
                //i++;
            }

            return command;
        }

        #endregion


        #region Extiond

        DataTable LoadMap(DbCommand command, string tableName)
        {
            Check.Require(tableName != null && tableName.Length > 0, "tableNames could not be null or empty.");

            DataTable data = new DataTable();

            using (DbDataAdapter adapter = GetDataAdapter())
            {
                command.CommandTimeout = TimeOut;
                adapter.SelectCommand = command;
                adapter.FillSchema(data, SchemaType.Mapped);
                data.AcceptChanges();
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return data;
        }
        /// <summary>
        /// 获取表结构
        /// </summary>
        /// <param name="tableName"></param>k
        /// <returns></returns>
        public DataTable GetMap(string tableName)
        {
            Check.Require(tableName, "tableName", Check.NotNullOrEmpty);

            if (tableName.IndexOf("`") > -1)
            {
                tableName = tableName.Replace("`", "");
            }

            if (tableName.IndexOf("[") > -1)
            {
                tableName = tableName.Replace("[", "").Replace("]", "");
            }

            var sql = "select * from " + tableName + " where 1=2";

            using (DbConnection connection = GetConnection(true))
            {
                using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
                {
                    command.CommandTimeout = TimeOut;
                    PrepareCommand(command, connection);
                    return LoadMap(command, tableName);
                }
            }
        }

        /// <summary>
        /// 获取最大自增长主键值
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="idName"></param>
        /// <returns></returns>
        public int GetMaxId(string tableName, string idName)
        {
            Check.Require(tableName, "tableName", Check.NotNullOrEmpty);

            if (tableName.IndexOf("`") > -1)
            {
                tableName = tableName.Replace("`", "");
            }

            if (tableName.IndexOf("[") > -1)
            {
                tableName = tableName.Replace("[", "").Replace("]", "");
            }

            var sql = "select max(" + idName + ") from " + tableName;

            using (DbConnection connection = GetConnection(true))
            {
                using (DbCommand command = CreateCommandByCommandType(CommandType.Text, sql))
                {
                    command.CommandTimeout = TimeOut;
                    PrepareCommand(command, connection);
                    var result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int maxId))
                    {
                        return maxId;
                    }
                    return 0;
                }
            }
        }
        #endregion
    }
}
