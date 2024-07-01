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
using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

using WEF.Common;

namespace WEF.Provider
{
    public class MsAccessProvider : DbProvider
    {
        public MsAccessProvider(string connectionString)
            : base(connectionString, OleDbFactory.Instance, '[', ']', '@', '*')
        {
            this.DatabaseType = DatabaseType.MsAccess;
        }

        public override string RowAutoID
        {
            get
            {
                return string.Empty;
            }
        }

        public override bool SupportBatch
        {
            get
            {
                return false;
            }
        }

        public override void PrepareCommand(DbCommand cmd)
        {
            base.PrepareCommand(cmd);

            foreach (DbParameter p in cmd.Parameters)
            {
                if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue)
                {
                    continue;
                }

                object value = p.Value;
                if (value == DBNull.Value)
                {
                    continue;
                }
                Type type = value.GetType();
                OleDbParameter oleDbParam = (OleDbParameter)p;

                if (oleDbParam.DbType != DbType.Guid && type == typeof(Guid))
                {
                    oleDbParam.OleDbType = OleDbType.Char;
                    oleDbParam.Size = 36;
                    continue;
                }

                if ((p.DbType == DbType.Time || p.DbType == DbType.DateTime) && type == typeof(TimeSpan))
                {
                    oleDbParam.OleDbType = OleDbType.Double;
                    oleDbParam.Value = ((TimeSpan)value).TotalDays;
                    continue;
                }

                if (type == typeof(Boolean))
                {
                    p.Value = ((bool)value).ToString();
                    continue;
                }

                switch (p.DbType)
                {
                    case DbType.Binary:
                        if (((byte[])value).Length > 2000)
                        {
                            oleDbParam.OleDbType = OleDbType.LongVarBinary;
                        }
                        break;
                    case DbType.Time:
                        oleDbParam.OleDbType = OleDbType.LongVarWChar;
                        p.Value = value.ToString();
                        break;
                    case DbType.DateTime:
                        oleDbParam.OleDbType = OleDbType.LongVarWChar;
                        p.Value = value.ToString();
                        break;
                    case DbType.AnsiString:
                        if (value.ToString().Length > 4000)
                        {
                            oleDbParam.OleDbType = OleDbType.LongVarChar;
                        }
                        break;
                    case DbType.String:
                        if (value.ToString().Length > 2000)
                        {
                            oleDbParam.OleDbType = OleDbType.LongVarWChar;
                        }
                        break;
                    case DbType.Object:
                        oleDbParam.OleDbType = OleDbType.LongVarWChar;
                        p.Value = SerializationManager.Serialize(value);
                        break;
                }
            }

            //replace "N'" to "'"
            cmd.CommandText = cmd.CommandText.Replace("N'", "'");

            //replace msaccess specific function names in cmdText
            cmd.CommandText = cmd.CommandText.Replace("upper(", "ucase(")
                            .Replace("lower(", "lcase(")
                            .Replace("substring(", "mid(")
                            .Replace("getdate()", "date() + time()")
                            .Replace("datepart(year", "datepart('yyyy'")
                            .Replace("datepart(month", "datepart('m'")
                            .Replace("datepart(day", "datepart('d'");

            //replace CHARINDEX with INSTR and reverse seqeunce of param items in CHARINDEX()
            int startIndexOfCharIndex = cmd.CommandText.IndexOf("charindex(");
            while (startIndexOfCharIndex > 0)
            {
                int endIndexOfCharIndex = DataUtils.GetEndIndexOfMethod(cmd.CommandText, startIndexOfCharIndex + "charindex(".Length);
                string[] itemsInCharIndex = DataUtils.SplitTwoParamsOfMethodBody(
                    cmd.CommandText.Substring(startIndexOfCharIndex + "charindex(".Length,
                    endIndexOfCharIndex - startIndexOfCharIndex - "charindex(".Length));
                cmd.CommandText = cmd.CommandText.Substring(0, startIndexOfCharIndex)
                    + "instr(" + itemsInCharIndex[1] + "," + itemsInCharIndex[0] + ")"
                    + (cmd.CommandText.Length - 1 > endIndexOfCharIndex ?
                    cmd.CommandText.Substring(endIndexOfCharIndex + 1) : "");

                startIndexOfCharIndex = cmd.CommandText.IndexOf("charindex(");
            }

        }


        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override bool IsTableExist(string tableName)
        {
            using (var cnn = new OleDbConnection(ConnectionString))
            {
                cnn.Open();
                var exists = cnn.GetSchema("Tables", new string[4] { null, null, tableName, "TABLE" }).Rows.Count > 0;
                cnn.Close();
                return exists;
            }
        }

    }
}
