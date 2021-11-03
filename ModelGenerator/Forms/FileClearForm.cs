using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FileCleaner
{
    public partial class FileClearForm : Form
    {
        public FileClearForm()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;

            textBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var selectDir = folderBrowserDialog1.SelectedPath;
                if (!string.IsNullOrEmpty(selectDir))
                {
                    textBox1.Text = selectDir;
                }
            }
        }

        Cleaner _cleaner;


        /// <summary>
        /// 清理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            var path = textBox1.Text;
            var cis = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            var before = dateTimePicker1.Value;

            if (string.IsNullOrEmpty(path))
            {
                MessageBoxEx.Show(this, "指定清理目录不能为空！", "清理工具提示");
                return;
            }
            if (string.IsNullOrEmpty(cis))
            {
                MessageBoxEx.Show(this, "指定清理文件类型不能为空！", "清理工具提示");
                return;
            }

            if (button2.Text == "停止清理")
            {
                _cleaner.Dispose();
                foreach (Control item in this.Controls)
                {
                    item.Enabled = true;
                }
                button2.Text = "开始清理";
            }
            else
            {
                if (MessageBoxEx.Show(this, "确认开始清理吗？", "清理工具提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
                foreach (Control item in this.Controls)
                {
                    item.Enabled = false;
                }
                button2.Enabled = true;
                button2.Text = "停止清理";

                _cleaner = new Cleaner(path, cis, before);
                _cleaner.OnCleaned += Cleaner_OnCleaned;
                _cleaner.OnStoped += Cleaner_OnStoped;
                _cleaner.Start();
            }
        }

        private void Cleaner_OnStoped()
        {
            this.Invoke(new Action(() =>
            {
                foreach (Control item in this.Controls)
                {
                    item.Enabled = true;
                }
                button2.Text = "开始清理";
            }));
        }

        private void Cleaner_OnCleaned(Cleaner cleaner, string msg)
        {
            if (string.IsNullOrEmpty(msg)) return;

            this.Invoke(new Action(() =>
            {
                if (textBox3.Text.Length >= 30 * 1000)
                {
                    textBox3.Text = textBox3.Text.Substring(1000);
                }
                textBox3.Text = textBox3.Text + msg;
                textBox3.SelectionStart = textBox3.Text.Length;
                textBox3.ScrollToCaret();
            }));
        }
    }

    /// <summary>
    /// 队列工具类
    /// </summary>
    public class BlockQueue<T> : IDisposable
    {
        ConcurrentQueue<T> _queue;

        AutoResetEvent _autoResetEvent;

        int _change = 0;

        /// <summary>
        /// 队列工具类
        /// </summary>
        public BlockQueue()
        {
            _queue = new ConcurrentQueue<T>();

            _autoResetEvent = new AutoResetEvent(true);
        }

        /// <summary>
        /// 长度
        /// </summary>
        public int Lenght
        {
            get
            {
                return _queue.Count;
            }
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="t"></param>
        public void Enqueue(T t)
        {
            _queue.Enqueue(t);

            if (Interlocked.Exchange(ref _change, 1) == 0)
            {
                _autoResetEvent.Set();
            }
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        public T Dequeue(int timeOut = 50)
        {
            if (_queue.TryDequeue(out T t))
            {
                return t;
            }
            else
            {
                if (!_autoResetEvent.WaitOne(timeOut))
                {
                    return default;
                }
                else
                {
                    Interlocked.Exchange(ref _change, 0);
                    return Dequeue(timeOut);
                }
            }
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            var count = _queue.Count;
            for (int i = 0; i < count; i++)
            {
                _queue.TryDequeue(out T _);
            }
            _autoResetEvent.Close();
        }
    }

    /// <summary>
    /// 清理程序
    /// </summary>
    public class Cleaner : IDisposable
    {
        string _path;
        string _pattern;
        DateTime _before;
        bool _disposed = false;

        BlockQueue<string> _blockQueue;


        /// <summary>
        /// 是否已开始
        /// </summary>
        public bool Started
        {
            get; set;
        } = false;

        /// <summary>
        /// 清理事件
        /// </summary>
        public event Action<Cleaner, string> OnCleaned;
        /// <summary>
        /// 停止事件
        /// </summary>
        public event Action OnStoped;

        /// <summary>
        /// 清理程序
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parttern"></param>
        /// <param name="before"></param>
        public Cleaner(string path, string parttern, DateTime before)
        {
            _path = path;
            _pattern = parttern;
            _before = before;
            _blockQueue = new BlockQueue<string>();

            FileHelper.OnFind += FileHelper_OnFind;

            Task.Factory.StartNew(() =>
            {
                StringBuilder sb = new StringBuilder();
                var begin = DateTime.Now;
                while (!_disposed)
                {
                    var str = _blockQueue.Dequeue();
                    if (!string.IsNullOrEmpty(str))
                        sb.AppendLine(str);

                    var span = DateTime.Now - begin;

                    if (span.TotalMilliseconds > 50)
                    {
                        OnCleaned?.Invoke(this, sb.ToString());
                        sb.Clear();
                        begin = DateTime.Now;
                    }
                }

            }, TaskCreationOptions.LongRunning);
        }



        int _success = 0;
        int _fail = 0;
        private void FileHelper_OnFind(string file)
        {
            if (FileHelper.Delete(file))
            {
                _blockQueue.Enqueue($"{DateTime.Now.ToLongTimeString()}  Success {file}");
                _success += 1;
            }
            else
            {
                _blockQueue.Enqueue($"{DateTime.Now.ToLongTimeString()}  Fail {file}");
                _fail += 1;
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parttern"></param>
        /// <param name="before"></param>
        public void Start()
        {
            if (!Started)
            {
                Started = true;

                _blockQueue.Enqueue("-----------------------------------------------------");
                _blockQueue.Enqueue($"{DateTime.Now.ToLongTimeString()}  清理工具已启动");

                Task.Run(() =>
                {
                    _success = 0;
                    _fail = 0;
                    _blockQueue.Enqueue($"{DateTime.Now.ToLongTimeString()}  正在准备清理目标文件");
                    FileHelper.Find(_path, _pattern, _before);
                    _blockQueue.Enqueue($"{DateTime.Now.ToLongTimeString()}  清理已完成，共成功清理:{_success},失败:{_fail}");
                    _blockQueue.Enqueue("-----------------------------------------------------");
                    Started = false;
                    OnStoped?.Invoke();
                });
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (Started)
            {
                _blockQueue.Enqueue($"{DateTime.Now.ToLongTimeString()}  正在停止清理操作");

                Started = false;
            }

        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Stop();

            _blockQueue.Dispose();

            _disposed = true;
        }
    }
    /// <summary>
    /// 文件工具类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 查找事件
        /// </summary>
        public static event Action<string> OnFind;

        /// <summary>
        /// 获取指定格式文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <param name="before"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetFiles(string path, string pattern, DateTime before)
        {
            var dir = new DirectoryInfo(path);

            if (!dir.Exists) yield break;

            var files = dir.EnumerateFiles(pattern).Where(q => q.LastWriteTime <= before).Select(q => q.FullName);

            foreach (var item in files)
            {
                OnFind?.Invoke(item);
                yield return item;
            }

            var childrens = dir.EnumerateDirectories();

            foreach (var dirItem in childrens)
            {
                foreach (var fileItem in GetFiles(dirItem.FullName, pattern, before))
                {
                    OnFind?.Invoke(fileItem);
                    yield return fileItem;
                }
            }
        }
        /// <summary>
        /// 查找，事件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <param name="before"></param>
        public static void Find(string path, string pattern, DateTime before)
        {
            try
            {
                GetFiles(path, pattern, before).ToList();
            }
            catch { }
        }

        /// <summary>
        /// 清理
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool Delete(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch { }
            return false;
        }
    }
    /// <summary>
    /// 居中显示的消息框
    /// </summary>
    public class MessageBoxEx
    {
        private static IWin32Window _owner;
        private static HookProc _hookProc;
        private static IntPtr _hHook;

        public static DialogResult Show(string text)
        {
            Initialize();
            return MessageBox.Show(text);
        }

        public static DialogResult Show(string text, string caption)
        {
            Initialize();
            return MessageBox.Show(text, caption);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton, options);
        }

        public static DialogResult Show(IWin32Window owner, string text)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon, defButton);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            _owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon,
                                   defButton, options);
        }

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate void TimerProc(IntPtr hWnd, uint uMsg, UIntPtr nIDEvent, uint dwTime);

        public const int WH_CALLWNDPROCRET = 12;

        public enum CbtHookAction : int
        {
            HCBT_MOVESIZE = 0,
            HCBT_MINMAX = 1,
            HCBT_QS = 2,
            HCBT_CREATEWND = 3,
            HCBT_DESTROYWND = 4,
            HCBT_ACTIVATE = 5,
            HCBT_CLICKSKIPPED = 6,
            HCBT_KEYSKIPPED = 7,
            HCBT_SYSCOMMAND = 8,
            HCBT_SETFOCUS = 9
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

        [DllImport("user32.dll")]
        private static extern int MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("User32.dll")]
        public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, TimerProc lpTimerFunc);

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll")]
        public static extern int UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

        [DllImport("user32.dll")]
        public static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

        [StructLayout(LayoutKind.Sequential)]
        public struct CWPRETSTRUCT
        {
            public IntPtr lResult;
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        };

        static MessageBoxEx()
        {
            _hookProc = new HookProc(MessageBoxHookProc);
            _hHook = IntPtr.Zero;
        }

        private static void Initialize()
        {
            if (_hHook != IntPtr.Zero)
            {
                throw new NotSupportedException("multiple calls are not supported");
            }

            if (_owner != null)
            {
                _hHook = SetWindowsHookEx(WH_CALLWNDPROCRET, _hookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
            }
        }

        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(_hHook, nCode, wParam, lParam);
            }

            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            IntPtr hook = _hHook;

            if (msg.message == (int)CbtHookAction.HCBT_ACTIVATE)
            {
                try
                {
                    CenterWindow(msg.hwnd);
                }
                finally
                {
                    UnhookWindowsHookEx(_hHook);
                    _hHook = IntPtr.Zero;
                }
            }

            return CallNextHookEx(hook, nCode, wParam, lParam);
        }

        private static void CenterWindow(IntPtr hChildWnd)
        {
            Rectangle recChild = new Rectangle(0, 0, 0, 0);
            bool success = GetWindowRect(hChildWnd, ref recChild);

            int width = recChild.Width - recChild.X;
            int height = recChild.Height - recChild.Y;

            Rectangle recParent = new Rectangle(0, 0, 0, 0);
            success = GetWindowRect(_owner.Handle, ref recParent);

            Point ptCenter = new Point(0, 0);
            ptCenter.X = recParent.X + ((recParent.Width - recParent.X) / 2);
            ptCenter.Y = recParent.Y + ((recParent.Height - recParent.Y) / 2);


            Point ptStart = new Point(0, 0);
            ptStart.X = (ptCenter.X - (width / 2));
            ptStart.Y = (ptCenter.Y - (height / 2));

            ptStart.X = (ptStart.X < 0) ? 0 : ptStart.X;
            ptStart.Y = (ptStart.Y < 0) ? 0 : ptStart.Y;

            int result = MoveWindow(hChildWnd, ptStart.X, ptStart.Y, width,
                                    height, false);
        }
    }
}
