/****************************************************************************
*Copyright (c) 2023 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Standard.DevelopTools.Common
*文件名： Logger
*版本号： V1.0.0.0
*唯一标识：31b9a549-f5f7-4737-87a4-9f0788bd7f34
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2023/2/14 10:56:16
*描述：
*
*=================================================
*修改标记
*修改时间：2023/2/14 10:56:16
*修改人： yswenli
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WEF.Standard.DevelopTools.Common
{

    /// <summary>
    /// 日志
    /// </summary>
    internal static class Logger
    {
        internal static readonly string ErrorPath = Path.Combine(Application.StartupPath, "log");

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="ex"></param>
        public static void Error(Exception ex)
        {
            if (ex == null) return;

            if (!Directory.Exists(ErrorPath))
            {
                Directory.CreateDirectory(ErrorPath);
            }
            string errorDayPath = Path.Combine(ErrorPath, DateTime.Now.ToString("yyyyMM") + ".txt");
            var error = new StringBuilder();
            error.Append("DateTime:");
            error.Append(DateTime.Now.ToString());
            error.Append("\r\n");
            error.Append("Message:");
            error.Append(ex.Message);
            error.Append("\r\n");
            error.Append("Source:");
            error.Append(ex.Source);
            error.Append("\r\n");
            error.Append("StackTrace:");
            error.Append(ex.StackTrace);
            error.Append("\r\n--------------------------------------------------------------\r\n");
            File.AppendAllText(errorDayPath, error.ToString());
        }
    }
}
