/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Standard.DevelopTools.Common
*文件名： TemplateHelper
*版本号： V1.0.0.0
*唯一标识：a691abfd-3fc6-4ada-a73b-02efc38486e5
*当前的用户域：OCEANIA
*创建人： Walle.Wen
*电子邮箱：Walle.Wen@oceania-inc.com
*创建时间：2021/7/13 10:26:21
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/7/13 10:26:21
*修改人： Walle.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System.Collections.Generic;
using System.Windows.Forms;

using WEF.Standard.DevelopTools.Model;

namespace WEF.Standard.DevelopTools.Common
{
    /// <summary>
    /// 模板工具类
    /// </summary>
    public static class TemplateHelper
    {
        static string _filePath = Application.StartupPath + "/Config/templateData.json";

        static ConfigHelper _configHelper;        

        /// <summary>
        /// 模板数据
        /// </summary>
        public static List<TemplateData> Data
        {
            get; set;
        }

        /// <summary>
        /// 模板工具类
        /// </summary>
        static TemplateHelper()
        {
            _configHelper = new ConfigHelper(_filePath);
            var data = _configHelper.Read<List<TemplateData>>();
            if (data == null) Data = new List<TemplateData>();
            else Data = data;
        }


        /// <summary>
        /// 保存配置
        /// </summary>
        public static void Save()
        {
            _configHelper.Write(Data);
        }
    }
}
