﻿/****************************************************************************
*项目名称：WEF.Standard.DevelopTools.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Standard.DevelopTools.Common
*类 名 称：CsvHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/6/30 14:21:31
*描述：
*=====================================================================
*修改时间：2020/6/30 14:21:31
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WEF.Standard.DevelopTools.Model;

namespace WEF.Standard.DevelopTools.Common
{
    public class CsvHelper
    {
        public event Action OnStart;
        public event Action<long, long> OnRunning;
        public event Action OnStop;

        public async Task CSV(DataTable dt, string filename)
        {
            await Task.Yield();
            OnStart?.Invoke();
            using (var fs = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var stream = ExportFromDataTable(dt);
                stream.CopyTo(fs);
                await fs.FlushAsync();
            }
            OnStop?.Invoke();
        }

        /// <summary>
        /// 空格，双引号等照原输出，逗号换成中文
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static string ConvertToSaveCell(object cell)
        {
            var cellStr = "";
            if (cell != null) cellStr = cell.ToString();
            cellStr = cellStr.Replace(",", "，").Trim();
            cellStr = cellStr.Replace("\"", "\"\"");
            return "\"" + cellStr + "\"" + ",";
        }

        /// <summary>
        /// 大数据转换
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="tableName"></param>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CSV(ConnectionModel cnn, string tableName, string fileName, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnStart?.Invoke();

            await Task.Run(async () =>
            {
                var dbObj = DBObjectHelper.GetDBObject(cnn);

                var reader = dbObj.GetDataReader(cnn.Database, $"select * from {tableName}");

                var header = new List<string>();

                var filedCount = reader.FieldCount;

                if (filedCount > 0)
                {
                    for (int i = 0; i < filedCount; i++)
                    {
                        header.Add(reader.GetName(i));
                    }
                }

                using (StringWriter sw = new StringWriter())
                {
                    StringBuilder sb = new StringBuilder();

                    char c = ',';

                    for (int i = 0; i < filedCount; i++)
                    {
                        if (i == filedCount - 1)
                            sb.Append(header[i]);
                        else
                            sb.Append(header[i] + c);
                    }
                    sb.Append("\r\n");

                    var rowCount = 1;

                    while (reader.Read())
                    {
                        rowCount++;

                        for (int j = 0; j < filedCount; j++)
                        {
                            try
                            {
                                sb.Append(reader[j]?.ToString() ?? "" + c);

                                try
                                {
                                    OnRunning?.Invoke(filedCount * j + rowCount, filedCount * rowCount);
                                }
                                catch { }
                            }
                            catch
                            {
                                sb.Append(c);
                            }
                        }
                        sb.Append("\r\n");
                    }

                    sw.Write(Encoding.UTF8.GetString(new byte[] { (byte)0xEF, (byte)0xBB, (byte)0xBF }));

                    sw.Write(sb);

                    using (var fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (StreamWriter ssw = new StreamWriter(fs))
                        {
                            await ssw.WriteAsync(sb.ToString());
                        }
                    }
                }
                OnStop?.Invoke();
            });
        }


        /// <summary>
        /// 从文件中读取CSV文件到DataTable
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="splitStr">分隔符,因为逗号经常在字段中使用，建议使用\t</param>
        /// <param name="columnNameList">自定义列名</param>
        /// <returns></returns>
        public static DataTable ImportToDataTable(string filePath, string splitStr = ",", IEnumerable<string> columnNameList = null)
        {
            if (!File.Exists(filePath)) return null;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    return ImportToDataTableFromString(sr.ReadToEnd(), splitStr, columnNameList);
                }
            }
        }

        /// <summary>
        /// 从string中读取CSV文件到DataTable
        /// </summary>
        /// <param name="csvString"></param>
        /// <param name="splitStr">分隔符,因为逗号经常在字段中使用，建议使用\t</param>
        /// <param name="columnNameList">自定义列名</param>
        /// <returns></returns>
        public static DataTable ImportToDataTableFromString(string csvString, string splitStr = ",", IEnumerable<string> columnNameList = null)
        {
            if (string.IsNullOrEmpty(csvString)) return null;

            var lines = csvString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            if (lines != null && lines.Length > 2)
            {
                DataTable dt = new DataTable();

                //记录每行记录中的
                //判断，若是第一次，建立表头
                bool isFirst = true;
                foreach (var strLine in lines)
                {
                    string[] arrayLine = strLine.Trim().Split(splitStr, StringSplitOptions.RemoveEmptyEntries);//分隔字符串，返回数组

                    int dtColumns = arrayLine.Length;//列的个数

                    if (isFirst)  //建立表头
                    {
                        if (columnNameList != null)
                        {
                            if (columnNameList.Count() != dtColumns) throw new ArgumentOutOfRangeException("自定义列数与数据源不一致");

                            foreach (var columnName in columnNameList)
                            {
                                dt.Columns.Add(columnName);//每一列名称
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtColumns; i++)
                            {
                                dt.Columns.Add(arrayLine[i]);//每一列名称
                            }
                        }

                    }
                    else //表内容
                    {
                        DataRow dataRow = dt.NewRow();//新建一行
                        for (int j = 0; j < dtColumns; j++)
                        {
                            dataRow[j] = arrayLine[j];
                        }
                        dt.Rows.Add(dataRow);//添加一行
                    }
                }

                return dt;
            }
            return null;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="splitStr"></param>
        /// <param name="columnNameList"></param>
        /// <returns></returns>
        public static Stream ExportFromDataTable(DataTable dt, string splitStr = ",", IEnumerable<string> columnNameList = null)
        {
            if (dt == null || dt.Rows == null || dt.Rows.Count == 0) return null;

            MemoryStream memoryStream = new MemoryStream();

            if (columnNameList != null)
            {
                if (columnNameList.Count() != dt.Columns.Count) throw new ArgumentOutOfRangeException("自定义列数与数据源不一致");

                foreach (var columnName in columnNameList)
                {
                    if (columnNameList.First() == columnName)
                    {
                        memoryStream.Write(Encoding.UTF8.GetBytes(columnName));
                    }
                    else
                    {
                        memoryStream.Write(Encoding.UTF8.GetBytes(splitStr));
                        memoryStream.Write(Encoding.UTF8.GetBytes(columnName));
                    }
                }
            }
            else
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    memoryStream.Write(Encoding.UTF8.GetBytes(dt.Columns[i].ColumnName.ToString()));
                    if (i < dt.Columns.Count - 1)
                    {
                        memoryStream.Write(Encoding.UTF8.GetBytes(splitStr));
                    }
                }
            }

            memoryStream.Write(Encoding.UTF8.GetBytes(Environment.NewLine));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    memoryStream.Write(Encoding.UTF8.GetBytes(dt.Rows[i][j].ToString()));
                    if (j < dt.Columns.Count - 1)
                    {
                        memoryStream.Write(Encoding.UTF8.GetBytes(splitStr));
                    }
                }
                memoryStream.Write(Encoding.UTF8.GetBytes(Environment.NewLine));
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
    }

    public static class MemoryStreamExtend
    {
        public static void Write(this MemoryStream memoryStream, byte[] data)
        {
            memoryStream.Write(data, 0, data.Length);
        }
    }

}
