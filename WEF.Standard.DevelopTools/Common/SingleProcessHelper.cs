/****************************************************************************
*项目名称：WEF.Standard.DevelopTools.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Standard.DevelopTools.Common
*类 名 称：SingleProcessHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/7/6 14:04:17
*描述：单进程处理
*=====================================================================
*修改时间：2020/7/6 14:04:17
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：单进程处理
*****************************************************************************/
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace WEF.Standard.DevelopTools.Common
{
    /// <summary>
    /// 单进程处理
    /// </summary>
    public static class SingleProcessHelper
    {
        static string _filePath;

        /// <summary>
        /// 单进程处理
        /// </summary>
        static SingleProcessHelper()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            _filePath = Path.Combine(dir, "SingleProcess.Data");
        }

        /// <summary>
        /// 设置窗体显示
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="fAltTab">是否显示</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        /// <summary>
        /// 将当前主窗体序列化到文件
        /// </summary>
        /// <param name="hWnd"></param>
        public static void SetMainFormSerialize(IntPtr hWnd)
        {
            FileStream fs = new FileStream(_filePath, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, hWnd);
            fs.Close();
        }

        /// <summary>
        /// 获取前主窗体序列化文件对象
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetMainFormSerialize()
        {
            if (File.Exists(_filePath))
            {
                using (FileStream fs = new FileStream(_filePath, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    return (IntPtr)bf.Deserialize(fs);
                }
            }
            return new IntPtr();
        }

        /// <summary>
        /// 运行进程
        /// </summary>
        /// <param name="mainForm"></param>
        /// <param name="run"></param>
        public static void ProcessRun<T>(Action<T> run) where T : Form
        {
            new Mutex(false, "WEF.Standard.DevelopTools", out bool isCanStart);

            if (isCanStart)
            {
                var form = Activator.CreateInstance<T>();
                form.Load += Form_Load;
                run.Invoke(form);
            }
            else
            {
                try
                {
                    var intPtr = GetMainFormSerialize();
                    ShowWindow(intPtr, 9);
                    SwitchToThisWindow(intPtr, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ex:" + ex.Message, "启动失败");
                }
            }
        }

        private static void Form_Load(object sender, EventArgs e)
        {
            SetMainFormSerialize((sender as Form).Handle);
        }
    }
}
