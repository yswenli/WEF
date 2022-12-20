/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.ModelGenerator.Model
*文件名： ConnectionInfo
*版本号： V1.0.0.0
*唯一标识：ca80519f-e1d5-4070-b3ba-1fa7b0fba364
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2021/7/23 11:28:55
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/7/23 11:28:55
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;

namespace WEF.ModelGenerator.Model
{
    public class ConnectionInfo
    {
        public DatabaseType DatabaseType { get; set; }

        public string Server { get; set; }

        public int Port { get; set; }

        public string DataBase { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        #region static

        public static ConnectionInfo GetConnectionInfo(ConnectionModel cm)
        {
            return GetConnectionInfo(cm.ConnectionString);
        }

        public static ConnectionInfo GetConnectionInfo(string connectionString)
        {
            ConnectionInfo connectionInfo = null;
            //sqlserver
            if (connectionString.IndexOf("Initial Catalog=") > -1)
            {
                connectionInfo = new ConnectionInfo();
                connectionInfo.DatabaseType = DatabaseType.SqlServer9;
                var arr = connectionString.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (arr != null && arr.Length > 0)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var item in arr)
                    {
                        var sarr = item.Split(new string[] { "=" }, StringSplitOptions.None);
                        if (sarr.Length == 2)
                        {
                            dic[sarr[0].Trim()] = sarr[1].Trim();
                        }
                        else
                        {
                            dic[item.Trim()] = "";
                        }
                    }
                    if (dic.ContainsKey("Data Source"))
                    {
                        connectionInfo.Server = dic["Data Source"];
                    }
                    if (dic.ContainsKey("Initial Catalog"))
                    {
                        connectionInfo.DataBase = dic["Initial Catalog"];
                    }
                    if (dic.ContainsKey("User Id"))
                    {
                        connectionInfo.UserName = dic["User Id"];
                    }
                    if (dic.ContainsKey("Password"))
                    {
                        connectionInfo.Password = dic["Password"];
                    }
                }
            }
            //mysql
            if (connectionString.IndexOf("server=") > -1)
            {
                connectionInfo = new ConnectionInfo();
                connectionInfo.DatabaseType = DatabaseType.SqlServer9;
                var arr = connectionString.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (arr != null && arr.Length > 0)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var item in arr)
                    {
                        var sarr = item.Split(new string[] { "=" }, StringSplitOptions.None);
                        if (sarr.Length == 2)
                        {
                            dic[sarr[0].Trim()] = sarr[1].Trim();
                        }
                        else
                        {
                            dic[item.Trim()] = "";
                        }
                    }
                    if (dic.ContainsKey("server"))
                    {
                        connectionInfo.Server = dic["server"];
                    }
                    if (dic.ContainsKey("Port"))
                    {
                        connectionInfo.Port =int.Parse(dic["Port"]);
                    }
                    if (dic.ContainsKey("database"))
                    {
                        connectionInfo.DataBase = dic["database"];
                    }
                    if (dic.ContainsKey("User Id"))
                    {
                        connectionInfo.UserName = dic["User Id"];
                    }
                    if (dic.ContainsKey("Password"))
                    {
                        connectionInfo.Password = dic["Password"];
                    }
                }
            }
            return connectionInfo;
        }

        #endregion
    }
}
