/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Mason.Wen
*命名空间：WEF.Common
*文件名： DataTableHelper
*版本号： V1.0.0.0
*唯一标识：7d76ce7e-c970-4d5f-8ad8-9b8906b7e591
*当前的用户域：Mason.Wen
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@outlook.com
*创建时间：2022/3/7 10:28:58
*描述：
*
*=====================================================================
*修改标记
*修改时间：2022/3/7 10:28:58
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Data;
using System.IO;
using System.Text;

namespace WEF.Common
{
    /// <summary>
    /// datatable工具类
    /// </summary>
    public static class DataTableHelper
    {
        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable ReadFromCSV(string filePath)
        {
            DataTable dt = new DataTable();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, new UTF8Encoding(false)))
                {
                    //记录每次读取的一行记录
                    string strLine = "";

                    //记录每行记录中的各字段内容
                    string[] aryLine = null;

                    string[] tableHead = null;

                    //标示列数
                    int columnCount = 0;

                    //标示是否是读取的第一行
                    bool isFirst = true;

                    //逐行读取CSV中的数据
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        if (isFirst == true)
                        {
                            tableHead = strLine.Split(',');

                            isFirst = false;

                            columnCount = tableHead.Length;

                            //创建列
                            for (int i = 0; i < columnCount; i++)
                            {
                                DataColumn dc = new DataColumn(tableHead[i]);
                                dt.Columns.Add(dc);
                            }
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(strLine))
                            {
                                aryLine = strLine.Split(',');

                                DataRow dr = dt.NewRow();

                                for (int j = 0; j < columnCount; j++)
                                {
                                    dr[j] = aryLine[j];
                                }

                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    if (aryLine != null && aryLine.Length > 0)
                    {
                        dt.DefaultView.Sort = tableHead[0] + " " + "asc";
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 写入CSV文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filePath"></param>
        /// <param name="withHeader"></param>
        public static void WriteToCSV(this DataTable table, string filePath, bool withHeader = true)
        {
            StringBuilder sb = new StringBuilder();

            DataColumn colum;

            if (withHeader)
            {
                foreach (DataColumn column in table.Columns)
                {
                    if (column == table.Columns[0])
                        sb.Append(column.ColumnName);
                    else
                        sb.Append("," + column.ColumnName);
                }
                sb.AppendLine();
            }

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];

                    var val = row[colum];

                    if (i != 0) sb.Append(",");

                    if (val == null)
                    {
                        sb.Append("");
                    }
                    else
                    {
                        if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                        {
                            sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                        }
                        else
                        {
                            if (colum.DataType.Name == "MySqlDateTime")
                            {
                                if (val is DBNull)
                                {
                                    sb.Append("");
                                }
                                else
                                {
                                    sb.Append(((MySql.Data.Types.MySqlDateTime)val).GetDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                }                                
                            }
                            else
                            {
                                sb.Append(row[colum].ToString());
                            }
                        };
                    }
                }
                sb.AppendLine();
            }

            var csvStr = sb.ToString();

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, new UTF8Encoding(false)))
                {
                    sw.Write(csvStr);
                }
            }
        }
    }
}
