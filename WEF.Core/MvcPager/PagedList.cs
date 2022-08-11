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
 * 类名称：PagedList<T>
 * 创建时间：2017/7/26 14:34:40
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WEF.MvcPager
{
    /// <summary>
    /// 分页列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable, DataContract]
    public class PagedList<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        [DataMember(Name = "pageIndex")]
        public int PageIndex { get; set; }
        /// <summary>
        /// 页容量
        /// </summary>
        [DataMember(Name = "pageSize")]
        public int PageSize { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        [DataMember(Name = "totalItemCount")]
        public int TotalItemCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        [DataMember(Name = "totalPageCount")]
        public int TotalPageCount { get; private set; }
        /// <summary>
        /// 开始页数
        /// </summary>
        [DataMember(Name = "startRecordIndex")]
        public int StartRecordIndex { get; private set; }
        /// <summary>
        /// 结束页数
        /// </summary>
        [DataMember(Name = "endRecordIndex")]
        public int EndRecordIndex { get; private set; }
        /// <summary>
        /// 数据
        /// </summary>
        [DataMember(Name = "data")]
        public List<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// 分页列表
        /// </summary>
        public PagedList()
        {

        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagedList(IList<T> items, int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            TotalItemCount = items.Count;
            TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            PageIndex = pageIndex;
            StartRecordIndex = (PageIndex - 1) * PageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : TotalItemCount;
            for (int i = StartRecordIndex - 1; i < EndRecordIndex; i++)
            {
                Data.Add(items[i]);
            }
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalItemCount"></param>
        public PagedList(IList<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            Data.AddRange(items);
            TotalItemCount = totalItemCount;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            PageIndex = pageIndex;
            PageSize = pageSize;
            StartRecordIndex = (pageIndex - 1) * pageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : totalItemCount;
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalItemCount"></param>
        public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            Data.AddRange(items);
            TotalItemCount = totalItemCount;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            PageIndex = pageIndex;
            PageSize = pageSize;
            StartRecordIndex = (pageIndex - 1) * pageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : totalItemCount;
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalItemCount"></param>
        public PagedList(int pageIndex, int pageSize, int totalItemCount)
        {
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
            TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            PageIndex = pageIndex;
            StartRecordIndex = (PageIndex - 1) * PageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : TotalItemCount;
        }
       
    }
}
