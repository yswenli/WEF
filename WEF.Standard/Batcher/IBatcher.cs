/****************************************************************************
*项目名称：WEF.Batcher
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Batcher
*类 名 称：IBatcher
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/9/14 13:30:53
*描述：
*=====================================================================
*修改时间：2020/9/14 13:30:53
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;

namespace WEF.Batcher
{
    /// <summary>
    /// IBatcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBatcher<T> : IDisposable where T : Entity
    {
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="t"></param>
        void Insert(T t);

        /// <summary>
        /// 插入实体列表
        /// </summary>
        /// <param name="data"></param>
        void Insert(IEnumerable<T> data);

        /// <summary>
        /// 批量执行
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="timeout"></param>
        void Execute(int batchSize = 10000, int timeout = 10 * 1000);
        /// <summary>
        /// 批量执行
        /// </summary>
        /// <param name="dataTable"></param>
        void Execute(DataTable dataTable);
    }
}
