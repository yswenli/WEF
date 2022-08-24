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
    public class StringPlus
    {
        private StringBuilder str = new StringBuilder();

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
    }
}
