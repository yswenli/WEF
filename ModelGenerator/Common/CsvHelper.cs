/****************************************************************************
*项目名称：WEF.ModelGenerator.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.ModelGenerator.Common
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Common
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
            await Task.Yield();
            await Task.Run(async () =>
            {
                using (StringWriter sw = new StringWriter())
                {
                    StringBuilder sb = new StringBuilder();
                    char c = ',';
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sb.Append(dt.Columns[i].Caption + c);
                    }
                    sb.Append("\r\n");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            try
                            {
                                sb.Append(ConvertToSaveCell(dt.Rows[i][j]));

                                OnRunning.Invoke(i * dt.Columns.Count + (j + 1), dt.Rows.Count * dt.Columns.Count);
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
                    using (var fs = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
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
            await Task.Yield();

            OnStart?.Invoke();

            await Task.Yield();

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
                                sb.Append(ConvertToSaveCell(reader[j]));

                                OnRunning?.Invoke(filedCount * j + rowCount, filedCount * rowCount);
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

    }
}
