/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2017
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：70e6d026-6f02-4e99-bda8-6bef5bd43cbd
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：WEF.Common
 * 类名称：DosORMCommonExpand
 * 创建时间：2017/7/26 14:25:47
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WEF.Common
{
    public static class CommonExpand
    {
        private static Dictionary<MemberInfo, Object> _micache1 = new Dictionary<MemberInfo, Object>();
        private static Dictionary<MemberInfo, Object> _micache2 = new Dictionary<MemberInfo, Object>();
        /// <summary>
        /// 获取自定义特性，带有缓存功能，避免因.Net内部GetCustomAttributes没有缓存而带来的损耗
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="member"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static TAttribute[] GetCustomAttributes<TAttribute>(this MemberInfo member, Boolean inherit)
        {
            if (member == null)
                return new TAttribute[0];

            // 根据是否可继承，分属两个缓存集合
            var cache = inherit ? _micache1 : _micache2;

            Object obj = null;
            if (cache.TryGetValue(member, out obj))
                return (TAttribute[])obj;
            lock (cache)
            {
                if (cache.TryGetValue(member, out obj))
                    return (TAttribute[])obj;

                var atts = member.GetCustomAttributes(typeof(TAttribute), inherit) as TAttribute[];
                var att = atts == null ? new TAttribute[0] : atts;
                cache[member] = att;
                return att;
            }
        }
        /// <summary>获取自定义属性</summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="member"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this MemberInfo member, Boolean inherit)
        {
            var atts = member.GetCustomAttributes<TAttribute>(inherit);
            if (atts == null || atts.Length < 1)
                return default(TAttribute);
            return atts[0];
        }
    }
}
