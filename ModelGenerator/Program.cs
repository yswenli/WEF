using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;
using WEF.ModelGenerator.Forms;

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
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new DesignForm());
            //return;

            //MainForm
            SingleProcessHelper.ProcessRun<MainForm>((f) =>
            {
                try
                {
                    Application.Run(f);
                }
                catch(Exception ex)
                {
                    Logger.Error(ex);
                }                
            });
        }


        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (e.Exception.Message.IndexOf("矩形") > -1) return;

            if (e.Exception.Message.IndexOf("GDI+ 中发生一般性错误") > -1) return;

            MessageBox.Show(e.Exception.Message + "\r\n 详情请查看日志!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Logger.Error(e.Exception);
        }

    }
}
