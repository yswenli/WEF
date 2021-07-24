/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.ModelGenerator.Model
*文件名： TemplateToCodeConfig
*版本号： V1.0.0.0
*唯一标识：d5ed1206-2a5c-4b32-99c6-74265d9b53a9
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2021/7/13 13:21:18
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/7/13 13:21:18
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEF.ModelGenerator.Model
{
    public class TemplateToCodeConfig
    {
        public string TemplateName { get; set; }

        public string NameSpace { get; set; }

        public string ReplaceString { get; set; }

        public string Suffix { get; set; }

        public string ProjectFilePath { get; set; }

    }
}
