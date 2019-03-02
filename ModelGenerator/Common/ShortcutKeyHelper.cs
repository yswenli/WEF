/****************************************************************************
*项目名称：WEF.ModelGenerator.Common
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：WEF.ModelGenerator.Common
*类 名 称：ShortcutKeyHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/2/19 11:13:26
*描述：
*=====================================================================
*修改时间：2019/2/19 11:13:26
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Windows.Forms;

namespace WEF.ModelGenerator.Common
{
    public class ShortcutKeyHelper
    {

        public static void AllSelect(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var text = (TextBox)sender;

            if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control)
            {
                text.SelectAll();
            }
        }

        public static void Run(object sender, System.Windows.Forms.KeyEventArgs e, Action action)
        {
            if (e.KeyCode == Keys.F5)
            {
                action?.Invoke();
            }
        }

        public static void Select(object sender, System.Windows.Forms.KeyEventArgs e, Action action1, Action action2)
        {
            if (e.KeyCode == Keys.Enter)
            {
                action1?.Invoke();
            }
            else
            {
                action2?.Invoke();
            }
        }

        public static void Choose(object sender, System.Windows.Forms.KeyEventArgs e, Action<int> action1, Action action2)
        {
            if (e.KeyCode == Keys.Up)
            {
                action1?.Invoke(-1);
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.Down)
            {
                action1?.Invoke(1);
                e.Handled = true;
                return;
            }
            action2?.Invoke();
        }
    }
}
