/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：1e7ab7e0-8733-46b2-a556-1fbb0ad96298
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WEF.ModelGenerator.Common
 * 类名称：StringPlus
 * 文件名：StringPlus
 * 创建年份：2015
 * 创建时间：2015-09-23 14:54:06
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace WEF.ModelGenerator.Common
{
    /// <summary>
    /// 根据代码格式特点，定制一个字符串类
    /// </summary>
    public class StringPlus
    {
        /// <summary>
        /// 字符串类
        /// </summary>
        public StringPlus()
        {

        }
        /// <summary>
        /// 字符串类
        /// </summary>
        /// <param name="text"></param>
        public StringPlus(string text)
        {
            this.Append(text);
        }

        private StringBuilder str = new StringBuilder();

        /// <summary>
        /// Length
        /// </summary>
        public int Length
        {
            get
            {
                return str.Length;
            }
        }

        public string Append(string Text)
        {
            this.str.Append(Text);
            return this.str.ToString();
        }
        /// <summary>
        /// 添加空行
        /// </summary>
        /// <returns></returns>
        public string AppendLine()
        {
            this.str.Append("\r\n");
            return this.str.ToString();
        }
        /// <summary>
        /// 添加一行字符串
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public string AppendLine(string Text)
        {
            this.str.Append(Text + "\r\n");
            return this.str.ToString();
        }
        /// <summary>
        /// 添加若干个空格符后的文本内容
        /// </summary>
        /// <param name="SpaceNum"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public string AppendSpace(int SpaceNum, string Text)
        {
            this.str.Append(this.Space(SpaceNum));
            this.str.Append(Text);
            return this.str.ToString();
        }
        /// <summary>
        /// 添加若干个空格符后的文本，并换行
        /// </summary>
        /// <param name="SpaceNum">空格数</param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public string AppendSpaceLine(int SpaceNum, string Text)
        {
            this.str.Append(this.Space(SpaceNum));
            this.str.Append(Text);
            this.str.Append("\r\n");
            return this.str.ToString();
        }
        /// <summary>
        /// 删除末尾指定字符串
        /// </summary>
        /// <param name="strchar"></param>
        public void DelLastChar(string strchar)
        {
            string str = this.str.ToString();
            int length = str.LastIndexOf(strchar);
            if (length > 0)
            {
                this.str = new StringBuilder();
                this.str.Append(str.Substring(0, length));
            }
        }
        /// <summary>
        /// 删除最后一个逗号
        /// </summary>
        public void DelLastComma()
        {
            string str = this.str.ToString();
            int length = str.LastIndexOf(",");
            if (length > 0)
            {
                this.str = new StringBuilder();
                this.str.Append(str.Substring(0, length));
            }
        }
        /// <summary>
        /// 移除尾部
        /// </summary>
        public void RemoveLast()
        {
            if (str.Length > 0)
                str.Remove(str.Length - 1, 1);
        }
        /// <summary>
        /// 移除头部
        /// </summary>
        public void RemoveFirst()
        {
            if (str.Length > 0)
                str.Remove(0, 1);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="Num"></param>
        public void Remove(int Start, int Num)
        {
            this.str.Remove(Start, Num);
        }
        /// <summary>
        /// 空格串
        /// </summary>
        /// <param name="SpaceNum"></param>
        /// <returns></returns>
        public string Space(int SpaceNum)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < SpaceNum; i++)
            {
                builder.Append("\t");  //制表符
            }
            return builder.ToString();
        }

        public override string ToString()
        {
            return this.str.ToString();
        }

        public string Value
        {
            get
            {
                return this.str.ToString();
            }
        }


        static readonly List<string> _sqlKeyWords = new List<string>() {

                        "SELECT * FROM TABLE",
                        "INSERT INTO TALBE(FILED) VALUES(VALUE)",
                        "DELETE FROM",
                        "UPDATE [TABLE] SET [FILED]=VALUE WHERE",
                        "DROP",
                        "ALTER",
                        "TOP",
                        "DISTINCT",
                        "ALL",
                        "AND",
                        "NOT",
                        "OR",
                        "WHERE",
                        "FROM",
                        "SET",
                        "VALUES",
                        "JOIN",
                        "LEFT",
                        "FULL",
                        "RIGHT",
                        "INNER",
                        "GROUP BY",
                        "BY",
                        "ORDER BY",
                        "ASC",
                        "DESC",
                        "UNION",
                        "BETWEEN",
                        "IS",
                        "WITH",
                        "AS",
                        "AVG",
                        "MIN",
                        "MAX",
                        "SUM",
                        "COUNT",
                        "HAVING",
                        "TRUNCATE TABLE",
                        "TABLE",
                        "VIEW",
                        "PROCEDURE",
                        "BEGIN",
                        "END",
                        "CREATE",
                        "ADD"
        };

        /// <summary>
        /// 提示字符集
        /// </summary>
        public static List<string> SQLKeyWords
        {
            get
            {
                return _sqlKeyWords;
            }
        }

        public static string[] GetSQLKeyWords(string keyCode)
        {
            return SQLKeyWords.Where(b => b.IndexOf(keyCode, StringComparison.OrdinalIgnoreCase) == 0).ToArray();
        }

        /// <summary>
        /// 获取sql中表名
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string GetTableName(string sql)
        {
            if (string.IsNullOrEmpty(sql)) return null;
            var sqlArr = sql.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (sqlArr == null || sqlArr.Length < 1) return null;
            for (int i = 0; i < sqlArr.Length; i++)
            {
                if ("From".Equals(sqlArr[i], StringComparison.InvariantCultureIgnoreCase))
                {
                    return sqlArr[i + 1];
                }
            }
            return null;
        }

    }
}

