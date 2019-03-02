using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace WEF.MvcPager
{
    public static class PageLinqExtensions
    {
        public static PagedList<T> ToPagedList<T>
            (
                this IQueryable<T> allItems,
                int pageIndex,
                int pageSize
            )
        {
            var totalItemCount = allItems.Count();

            if (pageIndex < 1 || (totalItemCount / pageSize) < (pageIndex - 1))
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);

            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }
        public static PagedList<T> ToPagedList<T>
            (
                this IList<T> allItems,
                int pageIndex,
                int pageSize
            )
        {
            var totalItemCount = allItems.Count();

            if (pageIndex < 1 || (totalItemCount / pageSize) < (pageIndex - 1))
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);

            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }

        public static PagedList<T> ToPagedList<T>
           (
               this IEnumerable<T> allItems,
               int pageIndex,
               int pageSize
           )
        {
            var totalItemCount = allItems.Count();

            if (pageIndex < 1 || (totalItemCount / pageSize) < (pageIndex - 1))
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);

            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }

        /// <summary>
        /// 自定义DataTable分页控件
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static PagedList<DataRow> ToPagedList(this DataTable source, int pageIndex, int pageSize)
        {
            var allItems = source.AsEnumerable();
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int count = (pageIndex - 1) * pageSize;
            IEnumerable<DataRow> items = allItems.Skip<DataRow>(count).Take<DataRow>(pageSize);
            return new PagedList<DataRow>(items.ToList(), pageIndex, pageSize, System.Linq.Enumerable.Count(allItems));
        }
        /// <summary>
        /// 自定义List分页控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this List<T> source, int pageIndex, int pageSize)
        {
            var allItems = source.AsEnumerable();
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int count = (pageIndex - 1) * pageSize;
            IEnumerable<T> items = allItems.Skip<T>(count).Take<T>(pageSize);
            return new PagedList<T>(items.ToList(), pageIndex, pageSize, System.Linq.Enumerable.Count(allItems));
        }
        /// <summary>
        /// 自定义IOrderedEnumerable分页控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IOrderedEnumerable<T> source, int pageIndex, int pageSize)
        {
            var allItems = source.AsEnumerable();
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int count = (pageIndex - 1) * pageSize;
            IEnumerable<T> items = allItems.Skip<T>(count).Take<T>(pageSize);
            return new PagedList<T>(items.ToList(), pageIndex, pageSize, System.Linq.Enumerable.Count(allItems));
        }

        /// <summary>
        /// 自定义Page分页控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this Page<T> source)
        {
            var allItem = source.PageList.ToList();
            if (allItem.Count <= source.PageSize)
            {
                return new PagedList<T>(allItem, source.PageIndex, source.PageSize, source.TotalItemCount);
            }
            int count = (source.PageIndex - 1) * source.PageSize;
            IEnumerable<T> items = allItem.Skip<T>(count).Take<T>(source.PageSize);
            return new PagedList<T>(items, source.PageIndex, source.PageSize, source.TotalItemCount);
        }
    }
}
