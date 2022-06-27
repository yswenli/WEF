/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：9a4fe848-95cb-4ad2-ac1b-d757a6ea1cd0
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WEF.Expressions
 * 类名称：OrderByOperation
 * 文件名：OrderByOperation
 * 创建年份：2015
 * 创建时间：2015-09-29 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using WEF.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace WEF.Expressions
{
    /// <summary>
    /// 排序类型
    /// </summary>
    public enum OrderByOperater : byte
    {
        ASC, DESC
    }


    /// <summary>
    /// 排序
    /// </summary>
    [Serializable]
    public class OrderByOperation
    {


        private Dictionary<string, OrderByOperater> orderByOperation = new Dictionary<string, OrderByOperater>();

        /// <summary>
        /// null
        /// </summary>
        public readonly static OrderByOperation None = new OrderByOperation();

        private OrderByOperation()
        {
        }

        public OrderByOperation(string fieldName, OrderByOperater orderBy)
        {
            orderByOperation.Add(fieldName, orderBy);
        }

        public OrderByOperation(Field field)
            : this(field.TableFieldName, OrderByOperater.ASC)
        {

        }

        public OrderByOperation(Field field, OrderByOperater orderBy)
            : this(field.TableFieldName, orderBy)
        {

        }
        /// <summary>
        /// 判断 OrderByClip  是否为null
        /// </summary>
        /// <param name="orderByClip"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(OrderByOperation orderByClip)
        {
            if ((null == orderByClip) || string.IsNullOrEmpty(orderByClip.ToString()))
                return true;
            return false;
        }

        /// <summary>
        /// 两个OrderByClip相加
        /// </summary>
        /// <param name="leftOrderByOpt"></param>
        /// <param name="rightOrderByOpt"></param>
        /// <returns></returns>
        public static OrderByOperation operator &(OrderByOperation leftOrderByOpt, OrderByOperation rightOrderByOpt)
        {
            if (IsNullOrEmpty(leftOrderByOpt) && IsNullOrEmpty(rightOrderByOpt))
                return None;

            if (IsNullOrEmpty(leftOrderByOpt))
                return rightOrderByOpt;
            if (IsNullOrEmpty(rightOrderByOpt))
                return leftOrderByOpt;


            OrderByOperation orderby = new OrderByOperation();
            foreach (KeyValuePair<string, OrderByOperater> kv in leftOrderByOpt.orderByOperation)
            {
                orderby.orderByOperation.Add(kv.Key, kv.Value);
            }

            foreach (KeyValuePair<string, OrderByOperater> kv in rightOrderByOpt.orderByOperation)
            {
                if (!orderby.orderByOperation.ContainsKey(kv.Key))
                    orderby.orderByOperation.Add(kv.Key, kv.Value);
            }

            return orderby;
        }

        public static bool operator true(OrderByOperation right)
        {
            return false;
        }

        public static bool operator false(OrderByOperation right)
        {
            return false;
        }


        /// <summary>
        /// 去掉的表前缀
        /// </summary>
        /// <returns></returns>
        public OrderByOperation RemovePrefixTableName()
        {
            OrderByOperation tempOrderByClip = new OrderByOperation();

            foreach (KeyValuePair<string, OrderByOperater> kv in this.orderByOperation)
            {
                string keyName = kv.Key;
                if (kv.Key.IndexOf('.') > 0)
                    keyName = keyName.Substring(keyName.IndexOf('.') + 1);

                tempOrderByClip.orderByOperation.Add(keyName, kv.Value);
            }

            return tempOrderByClip;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder orderBy = new StringBuilder();
            foreach (KeyValuePair<string, OrderByOperater> kv in this.orderByOperation)
            {
                orderBy.Append(",");
                orderBy.Append(kv.Key);
                orderBy.Append(" ");
                orderBy.Append(kv.Value.ToString());
            }
            if (orderBy.Length > 1)
                return orderBy.ToString().Substring(1);
            return orderBy.ToString();
        }

        /// <summary>
        /// 倒叙
        /// </summary>
        public OrderByOperation ReverseOrderByOperation
        {
            get
            {
                OrderByOperation tempOrderByOpt = new OrderByOperation();

                foreach (KeyValuePair<string, OrderByOperater> kv in this.orderByOperation)
                {
                    tempOrderByOpt.orderByOperation.Add(kv.Key, kv.Value == OrderByOperater.ASC ? OrderByOperater.DESC : OrderByOperater.ASC);
                }

                return tempOrderByOpt;

            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="orderByClip"></param>
        /// <returns></returns>
        public bool Equals(OrderByOperation orderByClip)
        {
            if (null == orderByClip)
                return false;
            return this.ToString().Equals(orderByClip.ToString());
        }


        /// <summary>
        /// OrderByString
        /// <example>
        /// order by id desc
        /// </example>
        /// </summary>
        public string OrderByString
        {
            get
            {
                if (this.orderByOperation.Count == 0)
                    return string.Empty;

                return string.Concat(" ORDER BY ", this.ToString());
            }
        }

    }
}
