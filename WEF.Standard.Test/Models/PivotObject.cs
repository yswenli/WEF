/****************************************************************************
*Copyright (c) 2023 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Standard.Test.Models
*文件名： PivotObject
*版本号： V1.0.0.0
*唯一标识：cdd39a20-60c3-4b5f-b9e1-6e3fa28778e2
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2023/3/13 15:24:31
*描述：
*
*=================================================
*修改标记
*修改时间：2023/3/13 15:24:31
*修改人： yswenli
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEF.Standard.Test.Models
{
    public class PivotObject
    {
        public string BatchNo { get; set; }

        public string WorkNum { get; set; }

        public string TaskType { get; set; }

        public string FailReason { get; set; }
    }
}
