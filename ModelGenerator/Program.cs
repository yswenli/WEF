using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

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
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (process.MainModule.FileName == current.MainModule.FileName)
                    {
                        MessageBox.Show("WEF数据库工具已经在运行！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }


            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }


        /// <summary>
        /// 记录错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (e.Exception.Message == "矩形“{X=0,Y=0,Width=0,Height=-1}”的宽度或高度不能等于零。") return;
            MessageBox.Show(e.Exception.Message + "\r\n 详情请查看日志!", "出错啦!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (!Directory.Exists(errorpath))
            {
                Directory.CreateDirectory(errorpath);
            }

            string errorDayPath = Path.Combine(errorpath, DateTime.Now.ToString("yyyyMM") + ".txt");

            StringBuilder error = new StringBuilder();

            error.Append("DateTime:");
            error.Append(DateTime.Now.ToString());
            error.Append("\r\n");
            error.Append("Message:");
            error.Append(e.Exception.Message);
            error.Append("\r\n");
            error.Append("Source:");
            error.Append(e.Exception.Source);
            error.Append("\r\n");
            error.Append("StackTrace:");
            error.Append(e.Exception.StackTrace);
            error.Append("\r\n--------------------------------------------------------------\r\n");
            File.AppendAllText(errorDayPath, error.ToString());
        }

        public static string errorpath = Path.Combine(Application.StartupPath, "log");
    }
}
