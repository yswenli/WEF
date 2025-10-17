using System;
using System.Windows.Forms;

using WEF.Standard.DevelopTools.Common.Win32;

namespace WEF.Standard.DevelopTools.ScreenCapture
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ScreenHelper.SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);           
            Application.Run(new MainForm(args));
        }
    }
}
