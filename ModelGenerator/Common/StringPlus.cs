/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2019
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WEF.ModelGenerator.Common
{
    /// <summary>
    /// 根据代码格式特点，定制一个字符串类
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


        static string[] sqlKeyWords = null;

        public static string[] SQLKeyWords
        {
            get
            {
                if (sqlKeyWords == null)
                {
                    sqlKeyWords = new string[]
                    {
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
                }
                return sqlKeyWords;
            }
        }

        public static string[] GetSQLKeyWords(string keyCode)
        {
            return SQLKeyWords.Where(b => b.Substring(0, 1) == keyCode).ToArray();
        }
    }
}

