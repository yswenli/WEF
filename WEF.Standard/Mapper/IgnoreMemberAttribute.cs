/****************************************************************************
*Copyright (c) 2024 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：河之洲
*命名空间：WEF.Mapper
*文件名： IgnoreMemberAttribute
*版本号： V1.0.0.0
*唯一标识：f739cbc6-7341-411f-81b0-150ba9e072a8
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2024/4/25 16:14:58
*描述：映射忽略属性标签
*
*=================================================
*修改标记
*修改时间：2024/4/25 16:14:58
*修改人： yswenli
*版本号： V1.0.0.0
*描述：映射忽略属性标签
*
*****************************************************************************/
using System;

namespace WEF.Mapper
{
    /// <summary>
    /// EmitMapper 映射忽略属性标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnoreMemberAttribute : Attribute
    {

    }
}
