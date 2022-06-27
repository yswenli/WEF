/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：80082f38-5128-45aa-9217-d181a0bb7f92
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：WEF
 * 命名空间：WEF.MvcPager
 * 类名称：PagerItem
 * 创建时间：2017/7/26 14:34:40
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
namespace WEF.MvcPager
{
    /// <summary>
    /// 页元素
    /// </summary>
    public class PagerItem
    {
        /// <summary>
        /// 页元素
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pageIndex"></param>
        /// <param name="disabled"></param>
        /// <param name="type"></param>
        public PagerItem(string text, int pageIndex, bool disabled, PagerItemType type)
        {
            Text = text;
            PageIndex = pageIndex;
            Disabled = disabled;
            Type = type;
        }

        public string Text { get; set; }
        public int PageIndex { get; set; }
        public bool Disabled { get; set; }
        public PagerItemType Type { get; set; }
    }

    public enum PagerItemType : byte
    {
        FirstPage,
        NextPage,
        PrevPage,
        LastPage,
        MorePage,
        NumericPage
    }
}
