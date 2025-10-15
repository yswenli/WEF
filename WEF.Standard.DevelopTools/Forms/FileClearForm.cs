
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using WEF.Standard.DevelopTools.Common;

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
                WEFMessageBox.Show(this, "指定清理目录不能为空！", "清理工具提示");
                return;
            }
            if (string.IsNullOrEmpty(cis))
            {
                WEFMessageBox.Show(this, "指定清理文件类型不能为空！", "清理工具提示");
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
                if (WEFMessageBox.Show(this, "确认开始清理吗？", "清理工具提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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
    
}
