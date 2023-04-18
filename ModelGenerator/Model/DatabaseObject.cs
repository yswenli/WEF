/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.ModelGenerator.Model
*文件名： DatabaseObject
*版本号： V1.0.0.0
*唯一标识：838fd268-a417-4725-aa94-cb707dfbe478
*当前的用户域：OCEANIA
*创建人： Walle.Wen
*电子邮箱：Walle.Wen@oceania-inc.com
*创建时间：2021/7/19 13:32:01
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/7/19 13:32:01
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

namespace WEF.ModelGenerator.Model
{
    public class DatabaseObject
    {
        public string Owner { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}
