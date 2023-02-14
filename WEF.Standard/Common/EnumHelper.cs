/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/

namespace WEF.Common
{

    public enum FieldType
    {
        Normal,
        HasOne,
        HasMany,
        BelongsTo,
        HasAndBelongsToMany,
        LazyLoad
    }

    public enum ColumnFunction
    {
        None,
        ToLower,
        ToUpper
    }
    /// <summary>
    /// 连接类型
    /// </summary>
    public enum JoinType : byte
    {
        /// <summary>
        /// InnerJoin
        /// </summary>
        InnerJoin,
        /// <summary>
        /// LeftJoin
        /// </summary>
        LeftJoin,
        /// <summary>
        /// RightJoin
        /// </summary>
        RightJoin,
        /// <summary>
        /// CrossJoin
        /// </summary>
        CrossJoin,
        /// <summary>
        /// FullJoin
        /// </summary>
        FullJoin
    }
    /// <summary>
    /// 标记实体状态
    /// </summary>
    public enum EntityState
    {
        //Detached = 1,
        /// <summary>
        /// 标记为不做任何数据库操作。
        /// </summary>
        Default = 2,
        /// <summary>
        /// 标记为插入状态。.Save()触发。
        /// </summary>
        Added = 4,
        /// <summary>
        /// 标记为删除状态。.Save()触发。
        /// </summary>
        Deleted = 8,
        /// <summary>
        /// 标记为修改状态。.Save()触发。
        /// </summary>
        Modified = 16,
    }
}
