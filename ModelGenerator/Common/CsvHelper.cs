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
using System.Data;
using System.IO;
using System.Text;
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
                using(var fs = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using(StreamWriter ssw=new StreamWriter(fs))
                    {
                        ssw.Write(sb.ToString());
                    }
                }
            }
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


        public static void CSV(Connection cnn, string tableName,string fileName)
        {
            //todo
        }
    }
}
