/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2024
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：80082f38-5128-45aa-9217-d181a0bb7f92
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：WEF
 * 命名空间：WEF.MvcPager
 * 类名称：Page<T>
 * 创建时间：2017/7/26 14:34:40
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System.Collections.Generic;

namespace WEF.MvcPager
{
    /// <summary>
    /// 分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Page<T>
    {
        /// <summary>
        /// 分页
        /// </summary>
        public Page()
        {
            PageList = new List<T>();
            PageSize = 1;
            PageIndex = 1;
            TotalItemCount = 0;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalItemCount"></param>
        public Page(IList<T> t, int pageIndex, int pageSize, int totalItemCount)
        {
            PageList = t;
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalItemCount = totalItemCount;
        }

        /// <summary>
        /// 分页
        /// </summary>
        public IList<T> PageList { get; set; }

        /// <summary>
        /// 页容量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalItemCount { get; set; }

    }
}
