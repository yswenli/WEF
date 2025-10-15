/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Standard.DevelopTools.Model
*文件名： DataSyncConfig
*版本号： V1.0.0.0
*唯一标识：81377ac9-4323-4407-8610-ba4a23d21581
*当前的用户域：OCEANIA
*创建人： Walle.Wen
*电子邮箱：Walle.Wen@oceania-inc.com
*创建时间：2021/7/19 14:12:02
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/7/19 14:12:02
*修改人： Walle.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using WEF.Standard.DevelopTools.Common;

namespace WEF.Standard.DevelopTools.Model
{
    public class DataSyncConfig
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public ConnectionModel Source { get; set; }

        public ConnectionModel Target { get; set; }

        public DateTime Created { get; set; }

        public bool IsEnabled { get; set; }

        #region static

        static string _filePath = Application.StartupPath + "/Config/dataSyncConfig.json";

        static ConfigHelper _configHelper = new ConfigHelper(_filePath);


        public static List<DataSyncConfig> Read()
        {
            return _configHelper.Read<List<DataSyncConfig>>();
        }

        public static void Save(List<DataSyncConfig> list)
        {
            _configHelper.Write(list);
        }

        #endregion


    }
}
