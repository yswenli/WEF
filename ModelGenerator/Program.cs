using System;
using System.Threading;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;

namespace WEF.ModelGenerator
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //全局异常
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //线程异常
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.EnableVisualStyles();
            //文本渲染
            Application.SetCompatibleTextRenderingDefault(false);
            //单例
            SingleProcessHelper.ProcessRun<MainForm>((f) =>
            {
                try
                {
                    Application.Run(f);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            });
        }

        /// <summary>
        /// 全局异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var error = e.ExceptionObject as Exception;

            if (error.Message.IndexOf("矩形") > -1) return;

            if (error.Message.IndexOf("GDI+ 中发生一般性错误") > -1) return;

            MessageBox.Show(error.Message + "\r\n 详情请查看日志!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Logger.Error(error);
        }


        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (e.Exception.Message.IndexOf("矩形") > -1) return;

            if (e.Exception.Message.IndexOf("GDI+ 中发生一般性错误") > -1) return;

            MessageBox.Show(e.Exception.Message + "\r\n 详情请查看日志!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Logger.Error(e.Exception);
        }

    }
}
