
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using WEF.Standard.DevelopTools.Common;

namespace WEF.Standard.DevelopTools.Controls
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

        /// <summary>
        /// 按钮事件
        /// </summary>
        public new event KeyEventHandler KeyDown;


        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var positionInfo = TextBoxPositionInfo.GetPositionInfo(textBox1);
            var selectStr = listBox1.SelectedItem.ToString();
            textBox1.Text = textBox1.Text.Substring(0, positionInfo.Start) + selectStr + textBox1.Text.Substring(positionInfo.Start + positionInfo.Length);            
            textBox1.SelectionStart = positionInfo.Start + selectStr.Length;
            textBox1.ScrollToCaret();
            listBox1.Hide();
            textBox1.Focus();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDown?.Invoke(sender, e);

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                return;
            }

            ShortcutKeyHelper.Select(sender, e, () =>
            {
                if (listBox1.Visible == true)
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                    listBox1_MouseDoubleClick(null, null);
                }
            }, () =>
            {
                ShortcutKeyHelper.Choose(sender, e, (inc) =>
                {
                    if (listBox1.Visible)
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = true;

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
                    //代码提示

                    //排除组合键和空格的提示
                    if (e.Modifiers == Keys.Control || e.Modifiers == Keys.Shift || e.Modifiers == Keys.Alt || e.KeyCode == Keys.Space || e.KeyCode == Keys.F5)
                    {
                        listBox1.Hide();
                        return;
                    }

                    //将前面输的内容作为一个索引项
                    var positionInfo = TextBoxPositionInfo.GetPositionInfo(textBox1);
                    if (string.IsNullOrEmpty(positionInfo.InputStr)) return;

                    var strs = StringPlus.GetSQLKeyWords(positionInfo.InputStr);

                    listBox1.AutoDisplay(strs);
                });
            });
        }
    }
}
