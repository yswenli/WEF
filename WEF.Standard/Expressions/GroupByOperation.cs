/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：9a4fe848-95cb-4ad2-ac1b-d757a6ea1cd0
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WEF.Expressions
 * 类名称：GroupByOperation
 * 文件名：GroupByOperation
 * 创建年份：2015
 * 创建时间：2015-09-29 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using System;

using WEF.Common;

namespace WEF.Expressions
{
    /// <summary>
    /// 分组
    /// </summary>
    [Serializable]
    public class GroupByOperation
    {
        private string groupByClip;

        public readonly static GroupByOperation None = new GroupByOperation((string)null);

        public GroupByOperation(string groupByClip)
        {
            this.groupByClip = groupByClip;
        }

        public GroupByOperation(Field field)
        {
            this.groupByClip = field.TableFieldName;
        }

        /// <summary>
        /// 判断 GroupByClip  是否为null
        /// </summary>
        /// <param name="groupByClip"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(GroupByOperation groupByClip)
        {
            if ((null == groupByClip) || string.IsNullOrEmpty(groupByClip.ToString()))
                return true;
            return false;
        }


        /// <summary>
        /// 两个GroupByClip相加
        /// </summary>
        /// <param name="leftGroupByClip"></param>
        /// <param name="rightGroupByClip"></param>
        /// <returns></returns>
        public static GroupByOperation operator &(GroupByOperation leftGroupByClip, GroupByOperation rightGroupByClip)
        {
            if (IsNullOrEmpty(leftGroupByClip) && IsNullOrEmpty(rightGroupByClip))
                return None;

            if (IsNullOrEmpty(leftGroupByClip))
                return rightGroupByClip;
            if (IsNullOrEmpty(rightGroupByClip))
                return leftGroupByClip;

            return new GroupByOperation(string.Concat(leftGroupByClip.ToString(), ",", rightGroupByClip.ToString()));

        }


        /// <summary>
        /// 去掉的表前缀
        /// </summary>
        /// <returns></returns>
        public GroupByOperation RemovePrefixTableName()
        {
            GroupByOperation groupc = new GroupByOperation(this.groupByClip);

            if (string.IsNullOrEmpty(groupc.groupByClip))
                return groupc;

            StringPlus gstring = new StringPlus();
            string[] fs = groupc.groupByClip.Split(',');
            foreach (string s in fs)
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                gstring.Append(",");
                if (s.IndexOf('.') > 0)
                    gstring.Append(s.Substring(s.IndexOf('.') + 1));
                else
                    gstring.Append(s);
            }

            if (gstring.Length > 1)
                groupc.groupByClip = gstring.ToString().Substring(1);
            else
                groupc.groupByClip = gstring.ToString();

            return groupc;

        }

        /// <summary>
        /// GroupByString
        /// <example>
        /// group by id
        /// </example>
        /// </summary>
        public string GroupByString
        {
            get
            {
                if (string.IsNullOrEmpty(this.groupByClip))
                    return string.Empty;

                return string.Concat(" GROUP BY ", this.groupByClip);
            }
        }


        public override string ToString()
        {
            return this.groupByClip;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator true(GroupByOperation right)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator false(GroupByOperation right)
        {
            return false;
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="groupByClip"></param>
        /// <returns></returns>
        public bool Equals(GroupByOperation groupByClip)
        {
            if (null == groupByClip)
                return false;

            return this.ToString().Equals(groupByClip.ToString());
        }
    }
}
