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
 * 类名称：PageLinqExtensions
 * 创建时间：2017/7/26 14:34:40
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace WEF.MvcPager
{
    /// <summary>
    /// 分页Linq扩展
    /// </summary>
    public static class PageLinqExtensions
    {
        public static PagedList<T> ToPagedList<T>
            (
                this IQueryable<T> allItems,
                int pageIndex,
                int pageSize
            )
        {
            var totalItemCount = Enumerable.Count(allItems);

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
            var totalItemCount = Enumerable.Count(allItems);

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
            var totalItemCount = Enumerable.Count(allItems);

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
        /// standarnd中不支持此方法AsEnumerable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IEnumerable<DataRow> AsEnumerable(this DataTable dt)
        {
            foreach (DataRow item in dt.Rows)
            {
                yield return item;
            }
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
            return new PagedList<T>(items.ToList(), pageIndex, pageSize, Enumerable.Count(allItems));
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
