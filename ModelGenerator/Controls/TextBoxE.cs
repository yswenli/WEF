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

namespace WEF.ModelGenerator.Controls
{
    public class TextBoxE : TextBox
    {
        protected override bool IsInputKey(Keys KeyData)
        {
            if ((KeyData == Keys.Up) || (KeyData == Keys.Down))
                return true;
            return base.IsInputKey(KeyData);
        }
    }
}