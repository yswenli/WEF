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
    class CsvHelper
    {
        public static event Action OnStart;
        public static event Action<long, long> OnRunning;
        public static event Action OnStop;

        public static void CSV(DataTable dt, string filename)
        {
            OnStart?.Invoke();
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
                        ssw.Write(sb.ToString());
                    }
                }
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
        public static Task CSV(Connection cnn, string tableName, string fileName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() =>
            {
                OnStart?.Invoke();

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
                            ssw.Write(sb.ToString());
                        }
                    }
                }
                OnStop?.Invoke();
            }, cancellationToken);
        }
    }
}
