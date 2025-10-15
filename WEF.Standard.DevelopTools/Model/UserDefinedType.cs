/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Standard.DevelopTools.Model
*文件名： UserDefinedType
*版本号： V1.0.0.0
*唯一标识：540ef405-6f17-40fe-abb4-166ba8eb7ebb
*当前的用户域：OCEANIA
*创建人： Walle.Wen
*电子邮箱：Walle.Wen@oceania-inc.com
*创建时间：2021/7/19 13:32:16
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/7/19 13:32:16
*修改人： Walle.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEF.Standard.DevelopTools.Model
{
    public class UserDefinedType : DatabaseObject
    {
        public string AttrName { get; set; }
        public string Type { get; set; }
        public int MaxLength { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }

        public bool IsRequired => !IsNullable;

        public bool IsNullable { get; set; }
    }
}
