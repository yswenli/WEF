using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using WEF.Standard.DevelopTools.Capture;
using WEF.Standard.DevelopTools.Common.Win32;

namespace WEF.Standard.DevelopTools.ScreenCapture
{
    public partial class MainForm : Form
    {
        bool isHidden = false;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(string[] args) : this()
        {
            if (args != null && args.Any(q => q == "-h"))
            {
                isHidden = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Filter = "图片文件|*.jpg;*.png;*.bmp;*.jpeg;";
            openFileDialog1.Title = "选择图片文件";
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.DefaultExt = "jpg";
            MouseAndKeyHelper.RegistHotKeys(this, true, false, true, "C", 1);
            if (isHidden)
            {
                StartCapture();
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartCapture();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MouseAndKeyHelper.UnregisterHotKey(this, 1);
        }

        /// <summary>
        /// 重写 WndProc：监听系统发送的 WM_HOTKEY 消息
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == MouseAndKeyHelper.WM_HOTKEY)
            {
                // 解析热键ID：m.WParam 存储的是注册时的 id 参数
                int hotKeyId = m.WParam.ToInt32();

                // 根据不同热键ID执行逻辑
                switch (hotKeyId)
                {
                    case 1:
                        StartCapture();
                        break;
                    default:
                        break;
                }
            }
        }



        private static CaptureForm frmCapture;

        private static void frmCapture_OnCaptured(Image imgDatas)
        {
            Clipboard.SetImage(imgDatas);
            MessageBox.Show("截图成功，已复制到剪切板");
        }

        private void StartCapture()
        {
            this.Hide();
            if ((frmCapture == null) || frmCapture.IsDisposed)
            {
                frmCapture = new CaptureForm();
                frmCapture.OnCaptured += frmCapture_OnCaptured;
            }
            frmCapture.IsCaptureCursor = true;
            frmCapture.IsFromClipBoard = false;
            frmCapture.Show();
            if (!isHidden)
                this.Show();
        }

    }
}
