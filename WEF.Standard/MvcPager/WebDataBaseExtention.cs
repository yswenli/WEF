/****************************************************************************
*项目名称：WEF.MvcPager
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.MvcPager
*类 名 称：WebDataBaseExtention
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/9/8 13:28:57
*描述：
*=====================================================================
*修改时间：2020/9/8 13:28:57
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using WEF.Common;
using WEF.Provider;

namespace WEF
{
    /// <summary>
    /// WebDataBaseExtention
    /// </summary>
    public sealed partial class DBContext
    {
        #region mvc 

        public DataTable GetMap(string tableName)
        {
            return _db.GetMap(tableName);
        }

        /// <summary>
        /// 获取表中字段信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<ColumnInfo> GetColumnInfos(string tableName)
        {
            var clts = new List<ColumnInfo>();

            var schemaTable = GetMap(tableName);

            if (schemaTable != null && schemaTable.Rows.Count > 0)
            {
                foreach (DataRow dr in schemaTable.Rows)
                {
                    ColumnInfo info = new ColumnInfo();
                    info.Name = dr["ColumnName"].ToString().Trim();
                    info.Ordinal = Convert.ToInt32(dr["ColumnOrdinal"].ToString());
                    info.AllowDBNull = (bool)dr["AllowDBNull"];
                    info.MaxLength = Convert.ToInt32(dr["ColumnSize"].ToString());
                    info.DataTypeName = dr["DataTypeName"].ToString().Trim();
                    info.AutoIncrement = (bool)dr["IsAutoIncrement"];
                    info.IsPrimaryKey = (bool)dr["IsKey"];
                    info.Unique = (bool)dr["IsUnique"];
                    info.IsReadOnly = (bool)dr["IsReadOnly"];
                    clts.Add(info);
                }
            }
            return clts;
        }

        /// <summary>
        /// 获取表中字段信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public ColumnInfo GetColumnInfo(string tableName, string columnName)
        {
            return GetColumnInfos(tableName).Where(b => b.Name == columnName).First();
        }


        /// <summary>
        /// 获取表中字段类型及长度
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public DataTable GetTypeAndLength(string tableName, string columnName)
        {
            DataTable dt = new DataTable();
            if (Db.DbProvider.GetType().Name== "SqlServer9Provider" || Db.DbProvider.GetType().Name == "SqlServerProvider")
            {
                string sql = "";
                if (string.IsNullOrEmpty(tableName))
                {
                    throw new Exception("表名不能为空");
                }
                sql = "SELECT  COLUMN_NAME ,DATA_TYPE ,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='" + DataUtils.ReplaceSqlKey(tableName, 50) + "' ";
                if (!string.IsNullOrEmpty(columnName))
                {
                    sql += " AND COLUMN_NAME='" + DataUtils.ReplaceSqlKey(columnName, 50) + "'";
                }
                dt = ExecuteDataTable(sql);
            }
            else
            {
                var sci = GetColumnInfo(tableName, columnName);
                DataColumn column1 = new DataColumn("COLUMN_NAME", typeof(string));
                DataColumn column2 = new DataColumn("DATA_TYPE", typeof(string));
                DataColumn column3 = new DataColumn("CHARACTER_MAXIMUM_LENGTH", typeof(int));
                dt.Columns.Add(column1);
                dt.Columns.Add(column2);
                dt.Columns.Add(column3);
                dt.AcceptChanges();
                var row = dt.NewRow();
                row["COLUMN_NAME"] = sci.Name;
                row["DATA_TYPE"] = sci.DataTypeName;
                row["CHARACTER_MAXIMUM_LENGTH"] = sci.MaxLength;
                dt.Rows.Add(row);
                dt.AcceptChanges();
            }
            return dt;
        }


        #region MVC前端传值更新模型
        /// <summary>
        /// 列举操作方法
        /// </summary>
        public enum EnumOperation
        {
            Insert,
            Update
        }
        /// <summary>
        /// MVC前端传值更新模型
        /// </summary>
        /// <param name="id">更新时的主键值</param>
        /// <param name="tableName">表名</param>
        /// <param name="collection">form表单</param>
        /// <param name="operation">更新类型</param>
        /// <returns>更新的记录数</returns>
        public int UpdateModel(int? id, string tableName, NameValueCollection collection, EnumOperation operation)
        {
            int error = 0;
            DataTable Column = _db.GetMap(tableName.ToString());
            string sqlStr = operation + " [" + tableName + "] ";
            if (operation == EnumOperation.Insert)
            {
                string conStrColumn = "(";
                string conStrValue = " values (";
                foreach (DataColumn item in Column.Columns)
                {
                    string columnName = item.ColumnName;
                    if (collection[columnName] != null && columnName != "ID")
                    {
                        string itemValue = collection[columnName].Replace("'", "").Trim();
                        conStrColumn = conStrColumn + "[" + columnName + "] ,";
                        if (item.DataType == typeof(String) || item.DataType == typeof(DateTime) || item.DataType == typeof(TimeSpan))
                        {
                            if (string.IsNullOrEmpty(itemValue))
                            {
                                conStrValue += " null" + ",";
                            }
                            else
                            {
                                conStrValue += " '" + itemValue + "'" + " ,";
                            }
                        }
                        else if (item.DataType == typeof(Boolean))
                        {
                            if (string.IsNullOrEmpty(itemValue))
                            {
                                conStrValue += "null,";
                            }
                            else
                            {
                                if (itemValue == "True")
                                {
                                    conStrValue += " 1 ,";
                                }
                                else
                                {
                                    conStrValue += " 0 ,";
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(itemValue))
                            {
                                conStrValue += "null,";
                            }
                            else
                            {
                                conStrValue += " " + itemValue + " ,";
                            }
                        }
                    }
                }
                conStrColumn = conStrColumn.Substring(0, conStrColumn.Length - 1);
                conStrValue = conStrValue.Substring(0, conStrValue.Length - 1);
                conStrColumn += ")";
                conStrValue += ")";
                sqlStr = sqlStr + conStrColumn + conStrValue;
            }
            else if (operation == EnumOperation.Update)
            {
                if (id != null)
                {
                    string conStrColumn = "";
                    string conStrValue = "";
                    string ConStr = " set ";
                    foreach (DataColumn item in Column.Columns)
                    {
                        string columnName = item.ColumnName;
                        if (collection[columnName] != null && columnName != "ID")
                        {
                            string itemValue = collection[columnName].Replace("'", "").Trim();
                            if (string.IsNullOrEmpty(itemValue)) itemValue = "null";
                            conStrColumn = " [" + columnName + "]";
                            if (item.DataType == typeof(String) || item.DataType == typeof(DateTime) || item.DataType == typeof(TimeSpan))
                            {
                                if (string.IsNullOrEmpty(itemValue))
                                {
                                    conStrValue = " null ";
                                }
                                else
                                {
                                    conStrValue = " '" + itemValue + "' ";
                                }
                            }
                            else if (item.DataType == typeof(Boolean))
                            {
                                if (string.IsNullOrEmpty(itemValue))
                                {
                                    conStrValue = " null ";
                                }
                                else
                                {
                                    if (itemValue == "True")
                                    {
                                        conStrValue = " 1 ";
                                    }
                                    else
                                    {
                                        conStrValue = " 0 ";
                                    }
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(itemValue))
                                {
                                    conStrValue = " null ";
                                }
                                else
                                {
                                    conStrValue = " " + itemValue + " ";
                                }
                            }
                            ConStr += conStrColumn + "=" + conStrValue + " ,";
                        }
                    }
                    ConStr = ConStr.Substring(0, ConStr.Length - 1);
                    string PKColumnName = "ID";
                    sqlStr += ConStr + " where [" + PKColumnName + "]=" + id;
                }
                else
                {
                    error = -1;
                }
            }
            if (error == -1)
                return error;
            else
                return ExecuteNonQuery(sqlStr);
        }
        #endregion
        #endregion

    }
}
