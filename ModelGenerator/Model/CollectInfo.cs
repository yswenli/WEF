/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.ModelGenerator.Model
*文件名： CollectInfo
*版本号： V1.0.0.0
*唯一标识：6e4af0f9-87eb-4611-b640-6a55bf2ec47a
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2021/8/19 17:01:29
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/8/19 17:01:29
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;

namespace WEF.ModelGenerator.Model
{
    public class CollectInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        #region static

        static string _filePath = Application.StartupPath + "/Config/collectData.json";

        static ConfigHelper _configHelper = new ConfigHelper(_filePath);

        public static List<CollectInfo> Read()
        {
            return _configHelper.Read<List<CollectInfo>>();
        }

        public static void Save(List<CollectInfo> list)
        {
            _configHelper.Write(list);
        }

        #endregion
    }
}
