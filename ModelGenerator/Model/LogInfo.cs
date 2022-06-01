/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.ModelGenerator.Model
*文件名： LogInfo
*版本号： V1.0.0.0
*唯一标识：f7f7aabb-0401-486d-9457-a29c20ad3c20
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2022/4/14 21:42:57
*描述：
*
*=====================================================================
*修改标记
*修改时间：2022/4/14 21:42:57
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
    /// <summary>
    /// 日志信息
    /// </summary>
    public class LogInfo
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public object[] Params { get; set; }

        /// <summary>
        /// 等级，
        /// 1表示代码异常，其他为逻辑异常
        /// </summary>
        public int Level { get; set; } = 0;
    }


    /// <summary>
    /// api日志记录实体
    /// </summary>
    public class ApiLogInfo : LogInfo
    {
        /// <summary>
        /// 跟踪id
        /// </summary>
        public string TraceId { get; set; }
        /// <summary>
        /// 调用方ip
        /// </summary>
        public string CallIp { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求方式
        /// </summary>
        public string RequestMethod { get; set; }
        /// <summary>
        /// 请求头
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// 输入值
        /// </summary>
        public string Input { get; set; }
        /// <summary>
        /// 用时
        /// </summary>
        public long Cost { get; set; }
        /// <summary>
        /// 响应码
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// 输出值
        /// </summary>
        public string Output { get; set; }

    }
}
