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
 * 类名称：DBToCSharp
 * 文件名：DBToCSharp
 * 创建年份：2015
 * 创建时间：2015-09-23 14:54:06
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/

using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using WEF.Common;

namespace WEF.ModelGenerator.Common
{
    /// <summary>
    /// 从配置中读取数据类型映射到C#类型
    /// </summary>
    public class DBToCSharp
    {

        /// <summary>
        /// 类型配置文件
        /// </summary>
        public static readonly string DbTypePath = Application.StartupPath + "/Config/dbtype.xml";


        private const string cachekeystring = "_dbtype_cache_";


        static Dictionary<string, string> loadType()
        {

            Dictionary<string, string> types = WEF.Cache.Cache.Default.GetCache(cachekeystring) as Dictionary<string, string>;

            if (null == types)
            {

                types = new Dictionary<string, string>();

                XmlDocument doc = new XmlDocument();

                doc.Load(DbTypePath);

                XmlNodeList nodes = doc.SelectNodes("//type");

                if (null != nodes && nodes.Count > 0)
                {
                    foreach (XmlNode node in nodes)
                    {
                        XmlAttribute att = node.Attributes["dbtype"];
                        if (null != att)
                        {
                            string dbtypeStr = att.Value.Trim().ToLower();
                            if (!types.ContainsKey(dbtypeStr))
                            {
                                XmlAttribute attcstype = node.Attributes["cstype"];
                                if (null != attcstype)
                                {
                                    types.Add(dbtypeStr, attcstype.Value);
                                }
                            }
                        }
                    }
                }

                Cache.Cache.Default.AddCacheFilesDependency(cachekeystring, types, DbTypePath);
            }

            return types;
        }


        /// <summary>
        /// 修改TypeName
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static List<ColumnInfo> DbtoCSColumns(List<ColumnInfo> columns, string dbType)
        {
            Dictionary<string, string> types = loadType();

            foreach (ColumnInfo column in columns)
            {
                try
                {
                    if (column.DataTypeName.Trim().ToLower() == "char" && column.MaxLength == 36 && dbType == "MySql")
                    {
                        column.DataTypeName = types["uniqueidentifier"];
                    }
                    else if (column.DataTypeName.Trim().ToLower() == "tinyint" && column.MaxLength == 1 && dbType == "MySql")
                    {
                        column.DataTypeName = types["bit"];
                    }
                    else if (column.DataTypeName.Trim().ToLower() == "number" && dbType == "Oracle")
                    {
                        if (column.Preci == "1")
                        {
                            column.DataTypeName = types["bit"];
                        }
                        else if (column.Preci == "18" || column.Scale != "0")
                        {
                            column.DataTypeName = types["decimal"];
                        }
                        else
                        {
                            column.DataTypeName = types["int"];
                        }

                    }
                    else
                    {
                        column.DataTypeName = types[column.DataTypeName.Trim().ToLower()];
                    }
                }
                catch
                {
                    column.DataTypeName = "object";
                }
                if (!column.IsIdentity && !column.IsPrimaryKey && column.AllowDBNull)
                {
                    if (!column.DataTypeName.Equals("string") && !column.DataTypeName.Equals("object") && !column.DataTypeName.Equals("byte[]"))
                        column.DataTypeName += "?";
                }
            }

            return columns;
        }

    }
}
