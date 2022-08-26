/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Common
*文件名： DBToCSharp
*版本号： V1.0.0.0
*唯一标识：88ca6e0c-836e-45a4-bf71-02fac8715600
*当前的用户域：WALLE
*创建人： wenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/8/24 10:54:01
*描述：数据库类型转换C#数据类型
*
*=================================================
*修改标记
*修改时间：2022/8/24 10:54:01
*修改人： yswen
*版本号： V1.0.0.0
*描述：数据库类型转换C#数据类型
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Xml;

namespace WEF.Common
{
    /// <summary>
    /// 数据库类型转换C#数据类型
    /// </summary>
    public static class DBToCSharp
    {
        static Lazy<Dictionary<string, string>> _cache;

        /// <summary>
        /// 数据库类型转换C#数据类型
        /// </summary>
        static DBToCSharp()
        {
            _cache = new Lazy<Dictionary<string, string>>(() =>
            {
                var types = new Dictionary<string, string>();

                XmlDocument doc = new XmlDocument();

                doc.LoadXml(Resource1.DbTypeToCsType);

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

                return types;
            });
        }


        /// <summary>
        /// 修改TypeName
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static List<ColumnInfo> DbtoCSColumns(List<ColumnInfo> columns, string dbType)
        {
            var types = _cache.Value;

            foreach (ColumnInfo column in columns)
            {
                try
                {
                    if (dbType == "MySql")
                    {
                        if (column.DataTypeName.Trim().ToLower() == "char" && column.MaxLength == 36)
                            column.DataTypeName = types["uniqueidentifier"];
                        else if (column.DataTypeName.Trim().ToLower() == "tinyint" && column.MaxLength == 1)
                            column.DataTypeName = types["bit"];
                        else
                            column.DataTypeName = types[column.DataTypeName.Trim().ToLower()];
                    }
                    else if (dbType == "Oracle")
                    {
                        if (column.DataTypeName.Trim().ToLower() == "number")
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
                            column.DataTypeName = types[column.DataTypeName.Trim().ToLower()];

                    }
                    else if (dbType == "PostgreSQL")
                    {
                        if (column.DataTypeName == "character")
                        {
                            column.DataTypeName = types["string"];
                        }
                        else if (column.DataTypeName == "date")
                        {
                            column.DataTypeName = types["datetime"];
                        }
                        else
                            column.DataTypeName = types[column.DataTypeName.Trim().ToLower()];
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
