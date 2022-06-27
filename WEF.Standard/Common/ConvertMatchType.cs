/****************************************************************************
*项目名称：WEF.Common
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Common
*类 名 称：ConvertType
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2019/9/18 15:17:58
*描述：
*=====================================================================
*修改时间：2019/9/18 15:17:58
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/

namespace WEF.Common
{
    /// <summary>
    /// 转换匹配类型
    /// </summary>
    public enum ConvertMatchType : byte
    {
        /// <summary>
        /// 属性名称完全匹配
        /// </summary>
        ExactlyMatch = 0,

        /// <summary>
        /// 属性名称忽略大小写
        /// </summary>
        IgnoreCase = 1,

        /// <summary>
        /// 属性名称包含
        /// </summary>
        Contain = 2,

        /// <summary>
        /// 属性名称包含并忽略大小写
        /// </summary>
        ContainAndIgnoreCase = 3
    }
}
