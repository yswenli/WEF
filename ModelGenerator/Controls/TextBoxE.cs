/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：439f0b94-1157-43ca-b4c9-b979fdf70a17
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：HttpTool
 * 类名称：TextBoxE
 * 创建时间：2016/11/15 16:37:04
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/

using System.Windows.Forms;

using WEF.ModelGenerator.Common;

namespace WEF.ModelGenerator.Controls
{
    /// <summary>
    /// 扩展输入控件
    /// </summary>
    public class TextBoxE : TextBox
    {
        /// <summary>
        /// 是否是输入键
        /// </summary>
        /// <param name="KeyData"></param>
        /// <returns></returns>
        protected override bool IsInputKey(Keys KeyData)
        {
            if ((KeyData == Keys.Up) || (KeyData == Keys.Down))
                return true;
            return base.IsInputKey(KeyData);
        }

        /// <summary>
        /// 检查粘帖内容及长度
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            //0x007B:鼠标右键 message ID
            //0x0301:复制(包括ctrl + c) message ID （其实这个判断不要，因为设置textbox的PasswordChar属性 实际上已经屏蔽了复制功能）
            //0x0302：粘贴（包括ctrl + v) message ID            
            if (m.Msg == 0x0302)
            {
                var txt = Clipboard.GetText();
                if (string.IsNullOrEmpty(txt))
                {
                    return;
                }
                if (txt.Length > this.MaxLength)
                {
                    WEFMessageBox.Show(this, "粘贴的内容不能超过" + this.MaxLength, "Base64转小图片");
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}