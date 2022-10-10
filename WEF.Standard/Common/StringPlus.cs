/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Common
*文件名： StringPlus
*版本号： V1.0.0.0
*唯一标识：984fa985-2f8a-41b3-a16a-94f163ba9e4f
*当前的用户域：WALLE
*创建人： wenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/8/24 10:10:05
*描述：
*
*=================================================
*修改标记
*修改时间：2022/8/24 10:10:05
*修改人： yswen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WEF.Common
{
    /// <summary>
    /// 字符串处理类
    /// </summary>
    public class StringPlus : IDisposable
    {
        private readonly StringBuilder _sb;

        /// <summary>
        /// Length
        /// </summary>
        public int Length
        {
            get
            {
                return _sb.Length;
            }
        }

        /// <summary>
        /// 字符串处理类
        /// </summary>
        public StringPlus()
        {
            _sb = new StringBuilder();
        }

        /// <summary>
        /// 字符串处理类
        /// </summary>
        /// <param name="str"></param>
        public StringPlus(string str) : this()
        {
            _sb.Append(str);
        }

        /// <summary>
        /// Append
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public void Append(dynamic text)
        {
            if (text == null) return;
            _sb.Append(text);
        }

        /// <summary>
        /// Append
        /// </summary>
        /// <param name="text"></param>
        /// <param name="repeatCount"></param>
        public void Append(dynamic text, int repeatCount)
        {
            if (text == null) return;
            _sb.Append(text, repeatCount);
        }

        /// <summary>
        /// 添加空行
        /// </summary>
        /// <returns></returns>
        public void AppendLine()
        {
            _sb.Append("\r\n");
        }

        /// <summary>
        /// 添加一行字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public void AppendLine(dynamic text)
        {
            if (text == null) return;
            _sb.Append(text + "\r\n");
        }

        /// <summary>
        /// 添加若干个空格符后的文本内容
        /// </summary>
        /// <param name="spaceNum"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public void AppendSpace(int spaceNum, dynamic text)
        {
            if (text == null) return;
            for (int i = 0; i < spaceNum; i++)
            {
                _sb.Append("\t");  //制表符
            }
            _sb.Append(text);
        }

        /// <summary>
        /// 添加若干个空格符后的文本，并换行
        /// </summary>
        /// <param name="spaceNum">空格数</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public void AppendSpaceLine(int spaceNum, dynamic text)
        {
            if (text == null) return;
            for (int i = 0; i < spaceNum; i++)
            {
                _sb.Append("\t");  //制表符
            }
            _sb.Append(text);
            _sb.Append("\r\n");
        }
        /// <summary>
        /// 删除末尾指定字符串
        /// </summary>
        /// <param name="strchar"></param>
        public void DelLastChar(string strchar)
        {
            string str = _sb.ToString();
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(strchar)) return;
            int index = str.LastIndexOf(strchar);
            if (index > -1)
            {
                _sb.Remove(index, strchar.Length);
            }
        }

        /// <summary>
        /// 删除最后一个逗号
        /// </summary>
        public void DelLastComma()
        {
            DelLastChar(",");
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="start"></param>
        /// <param name="num"></param>
        public void Remove(int start, int num)
        {
            _sb.Remove(start, num);
        }


        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public void Insert(int index, dynamic text)
        {
            if (text == null) return;
            _sb.Insert(index, text);
        }
        /// <summary>
        /// Replace
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public void Replace(string oldValue, string newValue)
        {
            _sb.Replace(oldValue, newValue);
        }

        /// <summary>
        /// Replace
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public void Replace(string oldValue, string newValue, int startIndex, int count)
        {
            _sb.Replace(oldValue, newValue, startIndex, count);
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _sb.ToString();
        }
        /// <summary>
        /// ToString
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string ToString(int start, int count)
        {
            return _sb.ToString(start, count);
        }
        /// <summary>
        /// Value
        /// </summary>
        public string Value
        {
            get
            {
                return _sb.ToString();
            }
        }

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="sp"></param>
        public static implicit operator StringBuilder(StringPlus sp)
        {
            return new StringBuilder(sp.ToString());
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
        /// <summary>
        /// GetSQLKeyWords
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public static string[] GetSQLKeyWords(string keyCode)
        {
            return SQLKeyWords.Where(b => b.IndexOf(keyCode, StringComparison.OrdinalIgnoreCase) == 0).ToArray();
        }


        private static Regex regSpace = new Regex(@"\s");
        /// <summary>
        /// 去掉空格
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceSpace(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            char firstChar = value[0];
            if (firstChar >= 48 && firstChar <= 57)
            {
                //value = "F" + value;
                value = "_" + value;
            }
            return regSpace.Replace(value.Trim(), " ");
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUpperFirstword(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            return value.Substring(0, 1).ToUpper() + value.Substring(1);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _sb.Clear();
        }
    }
}
