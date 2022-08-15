/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/
using System;
using System.Data;
using System.Data.Common;

using Npgsql;

using NpgsqlTypes;

using WEF.Common;

namespace WEF.Provider
{

    public class PostgreSqlProvider : DbProvider
    {

        public PostgreSqlProvider(string connectionString)
            : base(connectionString, global::Npgsql.NpgsqlFactory.Instance, '\"', '\"', '@')
        {
            this.DatabaseType = DatabaseType.PostgreSQL;
        }

        public override string RowAutoID
        {
            get
            {
                return "";
            }
        }

        public override bool SupportBatch
        {
            get
            {
                return true;
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
                NpgsqlParameter sqlParam = (NpgsqlParameter)p;

                if (type == typeof(Guid))
                {
                    sqlParam.NpgsqlDbType = NpgsqlDbType.Varchar;
                    sqlParam.Size = 36;
                    continue;
                }

                if ((p.DbType == DbType.Time || p.DbType == DbType.DateTime) && type == typeof(TimeSpan))
                {
                    sqlParam.NpgsqlDbType = NpgsqlDbType.Double;
                    sqlParam.Value = ((TimeSpan)value).TotalDays;
                    continue;
                }

                switch (p.DbType)
                {
                    case DbType.Binary:
                        if (((byte[])value).Length > 2000)
                        {
                            sqlParam.NpgsqlDbType = NpgsqlDbType.Text;
                        }
                        break;
                    case DbType.Time:
                        sqlParam.NpgsqlDbType = NpgsqlDbType.Time;
                        break;
                    case DbType.DateTime:
                        sqlParam.NpgsqlDbType = NpgsqlDbType.Date;
                        break;
                    case DbType.AnsiString:
                        if (value.ToString().Length > 65535)
                        {
                            sqlParam.NpgsqlDbType = NpgsqlDbType.Text;
                        }
                        break;
                    case DbType.String:
                        if (value.ToString().Length > 65535)
                        {
                            sqlParam.NpgsqlDbType = NpgsqlDbType.Text;
                        }
                        break;
                    case DbType.Object:
                        sqlParam.NpgsqlDbType = NpgsqlDbType.Text;
                        p.Value = SerializationManager.Serialize(value);
                        break;
                }
            }




            cmd.CommandText = cmd.CommandText
                .Replace("len(", "length(")
                .Replace("getdate()", "now()")
                .Replace("datepart(year,", "year(")
                .Replace("datepart(month,", "month(")
                .Replace("datepart(day,", "day(");

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
                    cmd.CommandText.Substring(endIndexOfCharIndex + 1) : string.Empty);

                startIndexOfCharIndex = cmd.CommandText.IndexOf("charindex(");
            }
        }


        /// <summary>
        /// 创建分页查询
        /// </summary>
        /// <param name="fromSection"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public override Search CreatePageFromSection(Search fromSection, int startIndex, int endIndex)
        {
            Check.Require(startIndex, "startIndex", Check.GreaterThanOrEqual<int>(1));
            Check.Require(endIndex, "endIndex", Check.GreaterThanOrEqual<int>(1));
            Check.Require(startIndex <= endIndex, "startIndex must be less than endIndex!");
            Check.Require(fromSection, "fromSection", Check.NotNullOrEmpty);

            fromSection.LimitString = string.Concat(" limit ", (endIndex - startIndex + 1).ToString(), " offset ", (startIndex - 1).ToString());


            return fromSection;
        }
    }
}
