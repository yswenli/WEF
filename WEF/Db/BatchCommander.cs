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
using System.Linq;
using System.Text;
using WEF.Common;
using WEF.Provider;

namespace WEF.Db
{
    /// <summary>
    /// BatchCommander
    /// </summary>
    public sealed class BatchCommander
    {
        #region Private Members

        private Database _db;

        private int _batchSize;

        private DbTransaction _tran;

        private List<DbCommand> _batchCommands;

        private bool _isUsingOutsideTransaction = false;


        /// <summary>
        /// 合并命令
        /// </summary>
        /// <returns></returns>
        private DbCommand MergeCommands()
        {
            if (_batchCommands == null || !_batchCommands.Any()) return null;

            DbCommand cmd = _db.GetSqlStringCommand("init");

            StringBuilder sb = new StringBuilder();

            foreach (DbCommand item in _batchCommands)
            {
                if (item.CommandType == CommandType.Text)
                {
                    foreach (DbParameter dbPara in item.Parameters)
                    {
                        DbParameter p = (DbParameter)((ICloneable)dbPara).Clone();
                        cmd.Parameters.Add(p);
                    }
                    sb.Append(item.CommandText + ";");
                }
            }

            if (sb.Length > 0)
            {
                if (_db.DbProvider is OracleProvider)
                {
                    sb.Insert(0, "begin ");
                    sb.Append(" end;");
                }
            }

            cmd.CommandText = sb.ToString();

            return cmd;
        }

        #endregion

        #region Public Members


        /// <summary>
        /// 执行
        /// </summary>
        void ExecuteBatch()
        {
            DbCommand cmd = MergeCommands();

            if (cmd == null) return;

            if (cmd.CommandText.Trim().Length > 0)
            {
                if (_tran != null)
                {
                    cmd.Connection = _tran.Connection;
                    cmd.Transaction = _tran;

                }
                else
                {
                    cmd.Connection = _db.GetConnection();
                }

                _db.DbProvider.PrepareCommand(cmd);

                cmd.ExecuteNonQuery();
            }

            _batchCommands.Clear();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCommander"/> class.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="il">The il.</param>
        public BatchCommander(Database db, int batchSize, IsolationLevel il)
            : this(db, batchSize, db.BeginTransaction(il))
        {
            _isUsingOutsideTransaction = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCommander"/> class.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="tran">The tran.</param>
        public BatchCommander(Database db, int batchSize, DbTransaction tran)
        {
            Check.Require(db != null, "db could not be null.");
            Check.Require(batchSize > 0, "Arguments error - batchSize should > 0.");

            this._db = db;
            this._batchSize = batchSize;
            _batchCommands = new List<DbCommand>(batchSize);
            this._tran = tran;
            if (tran != null)
            {
                _isUsingOutsideTransaction = true;
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCommander"/> class.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="batchSize">Size of the batch.</param>
        public BatchCommander(Database db, int batchSize)
            : this(db, batchSize, db.BeginTransaction())
        {
            _isUsingOutsideTransaction = false;
        }

        /// <summary>
        /// Processes the specified CMD.
        /// </summary>
        /// <param name="cmd">The CMD.</param>
        public void Process(DbCommand cmd)
        {
            if (cmd == null)
            {
                return;
            }

            cmd.Transaction = null;

            cmd.Connection = null;

            _batchCommands.Add(cmd);

            if (!_db.DbProvider.SupportBatch || _batchCommands.Count >= _batchSize)
            {
                try
                {
                    ExecuteBatch();
                }
                catch
                {
                    if (_tran != null && (!_isUsingOutsideTransaction))
                    {
                        _tran.Rollback();
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            try
            {
                ExecuteBatch();

                if (_tran != null && (!_isUsingOutsideTransaction))
                {
                    _tran.Commit();
                }
            }
            catch
            {
                if (_tran != null && (!_isUsingOutsideTransaction))
                {
                    _tran.Rollback();
                }

                throw;
            }
            finally
            {
                if (_tran != null && (!_isUsingOutsideTransaction))
                {
                    _db.CloseConnection(_tran);
                }
            }
        }

        #endregion
    }
}
