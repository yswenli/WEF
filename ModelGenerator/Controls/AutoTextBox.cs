using CCWin.SkinControl;

using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using WEF.ModelGenerator.Common;

namespace WEF.ModelGenerator.Controls
{
    public partial class AutoTextBox : UserControl
    {
        public AutoTextBox()
        {
            InitializeComponent();

            listBox1.Hide();
        }

        public TextBox TextBox
        {
            get
            {
                return textBox1;
            }
        }

        public event KeyEventHandler KeyUp;

        int textStart = 0;

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            KeyUp?.Invoke(sender, e);

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                return;
            }

            ShortcutKeyHelper.Select(sender, e, () =>
            {
                if (listBox1.Visible == true)
                {
                    listBox1_MouseDoubleClick(null, null);
                    e.Handled = true;
                    textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
                    textBox1.SelectionStart = textBox1.Text.Length;
                }
            }, () =>
            {
                ShortcutKeyHelper.Choose(sender, e, (inc) =>
                {
                    if (listBox1.Visible)
                    {
                        if (inc < 0 && listBox1.SelectedIndex == 0)
                        {
                            listBox1.SelectedIndex = 0;
                        }
                        else if (inc > 0 && listBox1.SelectedIndex == listBox1.Items.Count - 1)
                        {
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        }
                        else
                        {
                            listBox1.SelectedIndex += inc;
                        }
                    }

                }, () =>
                {
                    if (GetCaretPos(out Point point))
                    {
                        var strs = StringPlus.GetSQLKeyWords(e.KeyCode.ToString());

                        listBox1.Items.Clear();

                        if (strs != null && strs.Any())
                        {
                            textStart = textBox1.SelectionStart;
                            foreach (var str in strs)
                            {
                                listBox1.Items.Add(str);
                            }
                            point.Y += 20;
                            listBox1.Location = point;
                            listBox1.SelectedIndex = 0;
                            listBox1.Show();
                        }
                        else
                        {
                            listBox1.Hide();
                        }
                    }
                });
            });
        }

        [DllImport("user32.dll")]
        static extern bool GetCaretPos(out Point lpPoint);

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = textBox1.Text.Substring(0, textStart - 1) + listBox1.SelectedItem.ToString() + textBox1.Text.Substring(textStart);
            textBox1.SelectionStart = textBox1.Text.Length;
            listBox1.Hide();
            textBox1.Focus();
        }
    }
}
