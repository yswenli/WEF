/****************************************************************************
*Copyright (c) 2023 yswenli All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：yswenli
*公司名称：yswenli
*命名空间：WEF.Db
*文件名： PivotItem
*版本号： V1.0.0.0
*唯一标识：1c38aa0d-819b-4aab-9651-2cdb53a639b7
*当前的用户域：yswenli
*创建人： yswenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2023/2/27 11:29:26
*描述：转换项
*
*=================================================
*修改标记
*修改时间：2023/2/27 11:29:26
*修改人： yswenli
*版本号： V1.0.0.0
*描述：转换项
*
*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WEF.Db
{
    /// <summary>
    /// 转换项
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PivotInfo<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// 所需原表字段，用于分类汇总
        /// </summary>
        public Expression<Func<TEntity, object>> OriginalColumns { get; set; }
        /// <summary>
        /// 目标列名，用于要获取的字段数据，可以不存在
        /// </summary>
        public List<string> ColumnNames { get; set; }
        /// <summary>
        /// 被分类别的列名，作为列名的字段名
        /// </summary>
        public string TypeFieldName { get; set; }
        /// <summary>
        /// 被取值的列名，作为值的字段名
        /// </summary>
        public string ValueFieldName { get; set; }
        /// <summary>
        /// 原表字段的筛选条件
        /// </summary>
        public Expression<Func<TEntity, bool>> WhereLambada { get; set; }
    }
}
