using System.Collections.Generic;

namespace WEF.MvcPager
{
    /// <summary>
    /// 页面对象 内部封装分页用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Page<T>
    {
        public Page()
        {
            PageList = new List<T>();
            PageSize = 1;
            PageIndex = 1;
            TotalItemCount = 0;
        }

        public Page(IList<T> t, int pageIndex, int pageSize, int totalItemCount)
        {
            PageList = t;
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalItemCount = totalItemCount;
        }

        public IList<T> PageList { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalItemCount { get; set; }

    }
}
