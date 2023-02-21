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
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using WEF.Common;
using WEF.Db;
using WEF.ModelGenerator.Model;

namespace WEF.ModelGenerator.Common
{
    /// <summary>
    /// 生成器综合工具类
    /// </summary>
    public static class UtilsHelper
    {
        /// <summary>
        /// 数据库连接配置文件
        /// </summary>
        public static readonly string DatabaseconfigPath = Application.StartupPath + "/Config/databaseconfig.xml";

        /// <summary>
        /// 当前目录
        /// </summary>
        public static string CurrentPath
        {
            get
            {
                return Application.StartupPath;
            }
        }



        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        public static List<ConnectionModel> GetConnectionList()
        {
            List<ConnectionModel> list = new List<ConnectionModel>();
            XmlDocument doc = getXmlDocument();
            XmlNodeList xmlNodeList = doc.SelectNodes("servers/server");
            if (null != xmlNodeList && xmlNodeList.Count > 0)
            {
                foreach (XmlNode node in xmlNodeList)
                {
                    if (!node.HasChildNodes)
                        continue;
                    ConnectionModel connection = new ConnectionModel();
                    connection.ID = new Guid(node.Attributes["id"].Value);
                    connection.Name = node.Attributes["name"].Value;
                    connection.Database = node.Attributes["database"].Value;
                    connection.ConnectionString = node.FirstChild.InnerText;
                    connection.DbType = node.Attributes["dbtype"].Value;
                    list.Add(connection);
                }
            }
            return list;
        }


        static XmlDocument getXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(DatabaseconfigPath))
            {
                File.WriteAllText(DatabaseconfigPath, @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<servers>
</servers>
", Encoding.UTF8);
                //System.Threading.Thread.Sleep(2000);
            }


            doc.Load(DatabaseconfigPath);


            return doc;

        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteConnection(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            XmlDocument doc = getXmlDocument();

            XmlNodeList xmlNodeList = doc.SelectNodes("servers/server");
            if (null != xmlNodeList && xmlNodeList.Count > 0)
            {
                foreach (XmlNode node in xmlNodeList)
                {
                    if (node.Attributes["id"].Value.Trim().ToLower().Equals(id.Trim().ToLower()))
                    {
                        node.ParentNode.RemoveChild(node);
                        break;
                    }
                }
            }

            doc.Save(DatabaseconfigPath);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="conection"></param>
        public static void AddConnection(ConnectionModel conection)
        {
            XmlDocument doc = getXmlDocument();

            XmlNode root = doc.SelectSingleNode("servers");

            XmlElement xe = doc.CreateElement("server");

            xe.SetAttribute("id", conection.ID.ToString());
            xe.SetAttribute("name", conection.Name);
            xe.SetAttribute("database", conection.Database);
            xe.SetAttribute("dbtype", conection.DbType);

            XmlElement xe1 = doc.CreateElement("connectionstring");
            XmlCDataSection cdata = doc.CreateCDataSection(conection.ConnectionString);
            xe1.AppendChild(cdata);

            xe.AppendChild(xe1);

            root.AppendChild(xe);

            doc.Save(DatabaseconfigPath);
        }

        /// <summary>
        /// 获取一个连接配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ConnectionModel GetConnectionModel(string id)
        {
            ConnectionModel connModel = null;
            if (string.IsNullOrEmpty(id))
                return connModel;

            XmlDocument doc = new XmlDocument();
            doc.Load(DatabaseconfigPath);

            XmlNode xmlNode = doc.SelectSingleNode("servers/server[@id='" + id.ToString() + "']");
            if (null != xmlNode)
            {
                connModel = new ConnectionModel();
                connModel.ID = new Guid(xmlNode.Attributes["id"].Value);
                connModel.Name = xmlNode.Attributes["name"].Value;
                connModel.Database = xmlNode.Attributes["database"].Value;
                connModel.ConnectionString = xmlNode.FirstChild.InnerText;
                connModel.DbType = xmlNode.Attributes["dbtype"].Value;
            }

            return connModel;
        }

        /// <summary>
        /// 系统配置路径
        /// </summary>
        public static string SysconfigPath = Application.StartupPath + "/Config/sysconfig.xml";


        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <returns></returns>
        public static Sysconfig GetSysconfigModel()
        {
            Sysconfig sysconfigModel = new Sysconfig();

            XmlDocument doc = new XmlDocument();
            doc.Load(SysconfigPath);
            XmlNode node = doc.SelectSingleNode("configs/config[@key='namespace']");
            if (null != node)
            {
                sysconfigModel.Namespace = node.FirstChild.InnerText;
            }
            node = doc.SelectSingleNode("configs/config[@key='batchdirectorypath']");
            if (null != node)
            {
                sysconfigModel.BatchDirectoryPath = node.FirstChild.InnerText;
            }

            return sysconfigModel;
        }


        /// <summary>
        /// 设置系统配置
        /// </summary>
        /// <returns></returns>
        public static void GetSysconfigModel(Sysconfig sysconfigModel)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(SysconfigPath);
            XmlNode node = doc.SelectSingleNode("configs/config[@key='namespace']");
            if (null != node)
            {
                node.FirstChild.Value = sysconfigModel.Namespace;
            }
            node = doc.SelectSingleNode("configs/config[@key='batchdirectorypath']");
            if (null != node)
            {
                node.FirstChild.Value = sysconfigModel.BatchDirectoryPath;
            }

            doc.Save(SysconfigPath);
        }

        /// <summary>
        /// 写命名空间
        /// </summary>
        /// <param name="names"></param>
        public static void WriteNamespace(string names)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(SysconfigPath);
            XmlNode node = doc.SelectSingleNode("configs/config[@key='namespace']");
            node.FirstChild.Value = names;
            doc.Save(SysconfigPath);
        }

        /// <summary>
        /// 读命名空间
        /// </summary>
        /// <returns></returns>
        public static string ReadNamespace()
        {
            return GetSysconfigModel().Namespace;
        }


        /// <summary>
        /// 读保存的批量导出路径
        /// </summary>
        /// <returns></returns>
        public static string ReadBatchDirectoryPath()
        {
            return GetSysconfigModel().BatchDirectoryPath;
        }


        /// <summary>
        /// 列信息装换
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<ColumnInfo> GetColumnInfos(DataTable dt)
        {
            List<ColumnInfo> list = new List<ColumnInfo>();
            if (dt == null)
            {
                return null;
            }
            foreach (DataRow row in dt.Rows)
            {
                string str = row["Colorder"].ToString();  //序号
                string str2 = row["ColumnName"].ToString();  //列名
                string str3 = row["TypeName"].ToString();  //类型
                if (str3 == "longblob" || str3== "timestamp") continue;
                string str4 = row["IsIdentity"].ToString();  //标识
                string str5 = row["IsPK"].ToString();  //主键
                string str6 = row["Length"].ToString();  //长度
                string str7 = row["Preci"].ToString();  //精度
                string str8 = row["Scale"].ToString();  //小数位
                string str9 = row["cisNull"].ToString(); //为空
                string str10 = row["DefaultVal"].ToString();  //默认值
                string str11 = row["DeText"].ToString();  //描述
                ColumnInfo item = new ColumnInfo();
                item.Colorder = str;
                item.Name = str2;
                item.DataTypeName = str3;
                item.IsIdentity = str4 == "√";
                item.IsPrimaryKey = str5 == "√";

                var maxLen = 0;

                if (int.TryParse(str6, out maxLen))
                {
                    item.MaxLength = maxLen;
                }

                item.Preci = str7;
                item.Scale = str8;
                item.AllowDBNull = (str9 == "√") || (string.Compare(str9, "Y", true) == 0);
                item.DefaultVal = str10;
                item.DeText = str11;
                item.ColumnNameRealName = item.Name;
                list.Add(item);
            }
            return list;
        }

        public static DataTable GetColumnInfoDataTable(List<ColumnInfo> collist)
        {
            DataTable table = new DataTable();
            table.Columns.Add("colorder");
            table.Columns.Add("ColumnName");
            table.Columns.Add("TypeName");
            table.Columns.Add("Length");
            table.Columns.Add("Preci");
            table.Columns.Add("Scale");
            table.Columns.Add("IsIdentity");
            table.Columns.Add("isPK");
            table.Columns.Add("cisNull");
            table.Columns.Add("defaultVal");
            table.Columns.Add("deText");
            foreach (ColumnInfo info in collist)
            {
                DataRow row = table.NewRow();
                row["colorder"] = info.Colorder;
                row["ColumnName"] = info.Name;
                row["TypeName"] = info.DataTypeName;
                row["Length"] = info.MaxLength;
                row["Preci"] = info.Preci;
                row["Scale"] = info.Scale;
                row["IsIdentity"] = info.IsIdentity ? "√" : "";
                row["isPK"] = info.IsPrimaryKey ? "√" : "";
                row["cisNull"] = info.AllowDBNull ? "√" : "";
                row["defaultVal"] = info.DefaultVal;
                row["deText"] = info.DeText;
                table.Rows.Add(row);
            }
            return table;
        }

        private static System.Text.RegularExpressions.Regex regSpace = new System.Text.RegularExpressions.Regex(@"\s");

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
        /// 读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string Read(string filePath)
        {
            if (File.Exists(filePath))
                return File.ReadAllText(filePath, Encoding.UTF8);
            return string.Empty;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="txt"></param>
        public static void Write(string filePath, string txt)
        {
            using (var fs = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(txt);
                }
            }
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T Read<T>(string filePath)
        {
            var json = Read(filePath);
            if (!string.IsNullOrEmpty(json))
            {
                return SerializeHelper.Deserialize<T>(json);
            }
            return default(T);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="t"></param>
        public static void Write<T>(string filePath, T t)
        {
            if (t != null)
            {
                var json = SerializeHelper.Serialize(t);
                Write(filePath, json);
            }
        }

        /// <summary>
        /// split
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string[] Split(this string str, string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return str.Split(new string[] { separator }, options);
        }
    }
}
