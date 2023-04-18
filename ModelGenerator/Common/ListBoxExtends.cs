/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Walle.Wen
*命名空间：WEF.ModelGenerator.Common
*文件名： ListBoxExtends
*版本号： V1.0.0.0
*唯一标识：6ab5a043-e378-4e55-9122-0a06559ad8b7
*当前的用户域：Walle.Wen
*创建人： Walle.Wen
*电子邮箱：Walle.Wen@oceania-inc.com
*创建时间：2022/1/12 11:58:40
*描述：
*
*=====================================================================
*修改标记
*修改时间：2022/1/12 11:58:40
*修改人： Walle.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WEF.ModelGenerator.Common
{
    /// <summary>
    /// listbox扩展
    /// </summary>
    public static class ListBoxExtends
    {
        /// <summary>
        /// 自动添加项和显示大小
        /// </summary>
        /// <param name="listBox"></param>
        /// <param name="strs"></param>
        public static void AutoDisplay(this ListBox listBox, IEnumerable<string> strs)
        {
            if (!strs.Any())
            {
                listBox.Hide();
                return;
            }

            //自适应最长项的宽度
            string maxLengthStr = string.Empty;
            listBox.Items.Clear();
            foreach (var str in strs)
            {
                if (str.Length > maxLengthStr.Length) maxLengthStr = str;
                listBox.Items.Add(str);
            }
            if (Win32Helper.GetCaretPos(out Point point))
            {
                point.Y += 20;
                listBox.Location = point;
            }
            listBox.SelectedIndex = 0;
            var size = listBox.CreateGraphics().MeasureString(maxLengthStr, listBox.Font);
            listBox.Width = Convert.ToInt32(size.Width) + 20;
            listBox.Show();
        }

        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="listBox"></param>
        /// <param name="invoke"></param>
        public static void DeleteSelectedItems(this ListBox listBox, Action<string> invoke = null)
        {
            if (listBox.SelectedItems == null || listBox.SelectedItems.Count < 1) return;
            var items = new List<string>();
            foreach (var item in listBox.SelectedItems)
            {
                items.Add(item.ToString());
            }
            foreach (var item in items)
            {
                listBox.Items.Remove(item);
                invoke?.Invoke(item);
            }
        }
    }
}
