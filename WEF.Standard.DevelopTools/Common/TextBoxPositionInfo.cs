/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Walle.Wen
*命名空间：WEF.Standard.DevelopTools.Common
*文件名： TextBoxPositionInfo
*版本号： V1.0.0.0
*唯一标识：9713a116-5bf9-4de3-ae1c-a5a8cc9aae84
*当前的用户域：Walle.Wen
*创建人： Walle.Wen
*电子邮箱：Walle.Wen@oceania-inc.com
*创建时间：2022/1/12 11:57:32
*描述：
*
*=====================================================================
*修改标记
*修改时间：2022/1/12 11:57:32
*修改人： Walle.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WEF.Standard.DevelopTools.Common
{
    /// <summary>
    /// 当前操作位置相关信息
    /// </summary>
    public class TextBoxPositionInfo
    {
        /// <summary>
        /// 输入值
        /// </summary>
        public string InputStr { get; set; } = string.Empty;
        /// <summary>
        /// 起始位置
        /// </summary>
        public int Start { get; set; } = 0;
        /// <summary>
        /// 结束位置
        /// </summary>
        public int Length { get; set; } = 0;

        /// <summary>
        /// 当前操作位置相关信息
        /// </summary>
        /// <param name="textBox"></param>
        public TextBoxPositionInfo(TextBox textBox)
        {
            if (string.IsNullOrEmpty(textBox.Text)) return;

            if (textBox.SelectionStart == 0) return;

            List<int> list = new List<int>();

            var start1 = textBox.Text.LastIndexOf(" ", textBox.SelectionStart);
            if (start1 > -1)
            {
                list.Add(start1 + 1);
            }

            var start2 = textBox.Text.LastIndexOf("\t", textBox.SelectionStart);
            if (start2 > -1)
            {
                list.Add(start2 + 4);
            }

            var start3 = textBox.Text.LastIndexOf(Environment.NewLine, textBox.SelectionStart);
            if (start3 > -1)
            {
                list.Add(start3 + 2);
            }

            if (list.Count > 0) Start = Enumerable.Max(list);

            Length = textBox.SelectionStart - Start;

            if (Length <= 0) return;

            if ((Start + Length) > textBox.Text.Length) return;

            InputStr = textBox.Text.Substring(Start, Length).Trim();
        }

        /// <summary>
        /// 当前操作位置相关信息
        /// </summary>
        /// <param name="textBox"></param>
        /// <returns></returns>
        public static TextBoxPositionInfo GetPositionInfo(TextBox textBox)
        {
            return new TextBoxPositionInfo(textBox);
        }
    }
}
