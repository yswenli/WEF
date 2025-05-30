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
 * 类名称：PageLinqExtensions
 * 创建时间：2017/7/26 14:34:40
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
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
        /// <summary>
        /// 将IQueryable转换为分页列表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="allItems">数据源</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页列表</returns>
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> allItems, int pageIndex, int pageSize)
        {
            if (allItems == null)
                throw new ArgumentNullException(nameof(allItems));

            var totalItemCount = allItems.Count();

            if (pageIndex < 1 || (totalItemCount / pageSize) < (pageIndex - 1))
                pageIndex = 1;

            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);

            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }

        /// <summary>
        /// 将IList转换为分页列表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="allItems">数据源</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页列表</returns>
        public static PagedList<T> ToPagedList<T>(this IList<T> allItems, int pageIndex, int pageSize)
        {
            if (allItems == null)
                throw new ArgumentNullException(nameof(allItems));

            var totalItemCount = allItems.Count;

            if (pageIndex < 1 || (totalItemCount / pageSize) < (pageIndex - 1))
                pageIndex = 1;

            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);

            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }

        /// <summary>
        /// 将IEnumerable转换为分页列表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="allItems">数据源</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页列表</returns>
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> allItems, int pageIndex, int pageSize)
        {
            if (allItems == null)
                throw new ArgumentNullException(nameof(allItems));

            var totalItemCount = allItems.Count();

            if (pageIndex < 1 || (totalItemCount / pageSize) < (pageIndex - 1))
                pageIndex = 1;

            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);

            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }

        /// <summary>
        /// 将DataTable转换为分页列表
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页列表</returns>
        public static PagedList<DataRow> ToPagedList(this DataTable source, int pageIndex, int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var allItems = source.AsEnumerable();
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            int count = (pageIndex - 1) * pageSize;
            var items = allItems.Skip(count).Take(pageSize);
            return new PagedList<DataRow>(items.ToList(), pageIndex, pageSize, allItems.Count());
        }

        /// <summary>
        /// 将DataTable转换为IEnumerable
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>数据行枚举</returns>
        public static IEnumerable<DataRow> AsEnumerable(this DataTable dt)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt));

            foreach (DataRow item in dt.Rows)
            {
                yield return item;
            }
        }

        /// <summary>
        /// 将List转换为分页列表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页列表</returns>
        public static PagedList<T> ToPagedList<T>(this List<T> source, int pageIndex, int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var allItems = source.AsEnumerable();
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            int count = (pageIndex - 1) * pageSize;
            var items = allItems.Skip(count).Take(pageSize);
            return new PagedList<T>(items.ToList(), pageIndex, pageSize, allItems.Count());
        }

        /// <summary>
        /// 将IOrderedEnumerable转换为分页列表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>分页列表</returns>
        public static PagedList<T> ToPagedList<T>(this IOrderedEnumerable<T> source, int pageIndex, int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var allItems = source.AsEnumerable();
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            int count = (pageIndex - 1) * pageSize;
            var items = allItems.Skip(count).Take(pageSize);
            return new PagedList<T>(items.ToList(), pageIndex, pageSize, allItems.Count());
        }

        /// <summary>
        /// 将Page转换为分页列表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="source">数据源</param>
        /// <returns>分页列表</returns>
        public static PagedList<T> ToPagedList<T>(this Page<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var allItem = source.PageList.ToList();
            if (allItem.Count <= source.PageSize)
            {
                return new PagedList<T>(allItem, source.PageIndex, source.PageSize, source.TotalItemCount);
            }

            int count = (source.PageIndex - 1) * source.PageSize;
            var items = allItem.Skip(count).Take(source.PageSize);
            return new PagedList<T>(items, source.PageIndex, source.PageSize, source.TotalItemCount);
        }
    }
}
