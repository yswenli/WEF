using System;
using System.Collections.Concurrent;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;

namespace WEF.ModelGenerator.Forms
{
    public partial class FindForm : Form
    {
        TextBox _target = null;

        /// <summary>
        /// 文本内容
        /// </summary>
        public new string Text
        {
            get; private set;
        }

        /// <summary>
        /// 查找起始位置
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// FindForm
        /// </summary>
        FindForm()
        {
            InitializeComponent();
            skinWaterTextBox1.KeyUp += SkinWaterTextBox1_KeyUp;
        }

        private void SkinWaterTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }

        /// <summary>
        /// FindForm
        /// </summary>
        /// <param name="target"></param>
        public FindForm(TextBox target) : this()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            _target = target;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Text = skinWaterTextBox1.Text;

            if (string.IsNullOrEmpty(Text))
            {
                return;
            }
            try
            {
                var source = _target.Text;

                if (Offset == -1)
                {
                    return;
                }

                Offset = source.IndexOf(Text, Offset, StringComparison.OrdinalIgnoreCase);

                if (Offset == -1)
                {
                    return;
                }

                _target.SelectionStart = Offset;
                _target.SelectionLength = this.Text.Length;
                _target.ScrollToCaret();
                _target.Focus();
                Offset++;
            }
            finally
            {

            }
        }
    }


    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class FindFormExtends
    {
        /// <summary>
        /// 注册关联查找窗口
        /// </summary>
        /// <param name="target"></param>
        public static void RegistFindForm(this TextBox target)
        {
            if (target == null) return;

            var findForm = new FindForm(target);
            target.Tag = findForm;
            target.KeyUp += Target_KeyUp;
            target.MouseClick += Target_MouseClick;
        }

        private static FindForm GetFindForm(TextBox target)
        {
            FindForm findForm = null;

            if (target.Tag != null)
            {
                findForm = (FindForm)target.Tag;
            }

            if (findForm == null || findForm.IsDisposed)
            {
                findForm = new FindForm(target);
                target.Tag = findForm;
            }
            return findForm;
        }

        private static void Target_MouseClick(object sender, MouseEventArgs e)
        {
            var target = (TextBox)sender;

            var findForm = GetFindForm(target);

            findForm.Offset = target.SelectionStart;
        }

        private static void Target_KeyUp(object sender, KeyEventArgs e)
        {
            ShortcutKeyHelper.Find(e, () =>
            {
                try
                {
                    var target = (TextBox)sender;

                    var findForm = GetFindForm(target);

                    findForm.Show(target);
                }
                finally { }
            });
        }
    }
}
