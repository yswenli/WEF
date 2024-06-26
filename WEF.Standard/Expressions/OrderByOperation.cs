﻿/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2024
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
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
using System;
using System.Collections.Generic;
using System.Data.Common;

using WEF.Common;
using WEF.Db;

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
        private Dictionary<string, OrderByOperater> _orderByOperation = new Dictionary<string, OrderByOperater>();

        /// <summary>
        /// null
        /// </summary>
        public readonly static OrderByOperation None = new OrderByOperation();

        private OrderByOperation()
        {
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="orderBy"></param>
        public OrderByOperation(string fieldName, OrderByOperater orderBy)
        {
            _orderByOperation.Add(fieldName, orderBy);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="field"></param>
        public OrderByOperation(Field field)
            : this(field.TableFieldName, OrderByOperater.ASC)
        {

        }

        public OrderByOperation(Field field, OrderByOperater orderBy)
            : this(field.TableFieldName, orderBy)
        {

        }
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="orderBy"></param>
        public void Add(string fieldName, OrderByOperater orderBy)
        {
            _orderByOperation.Add(fieldName, orderBy);
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
            foreach (KeyValuePair<string, OrderByOperater> kv in leftOrderByOpt._orderByOperation)
            {
                orderby._orderByOperation.Add(kv.Key, kv.Value);
            }

            foreach (KeyValuePair<string, OrderByOperater> kv in rightOrderByOpt._orderByOperation)
            {
                if (!orderby._orderByOperation.ContainsKey(kv.Key))
                    orderby._orderByOperation.Add(kv.Key, kv.Value);
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

            foreach (KeyValuePair<string, OrderByOperater> kv in this._orderByOperation)
            {
                string keyName = kv.Key;
                if (kv.Key.IndexOf('.') > 0)
                    keyName = keyName.Substring(keyName.IndexOf('.') + 1);

                tempOrderByClip._orderByOperation.Add(keyName, kv.Value);
            }

            return tempOrderByClip;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringPlus orderBy = new StringPlus();
            foreach (KeyValuePair<string, OrderByOperater> kv in this._orderByOperation)
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

                foreach (KeyValuePair<string, OrderByOperater> kv in this._orderByOperation)
                {
                    tempOrderByOpt._orderByOperation.Add(kv.Key, kv.Value == OrderByOperater.ASC ? OrderByOperater.DESC : OrderByOperater.ASC);
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
                if (this._orderByOperation.Count == 0)

                    return string.Empty;

                return string.Concat(" ORDER BY ", this.ToString());
            }
        }

        /// <summary>
        /// 将排序对象转换成字典
        /// </summary>
        /// <param name="orderByOperation"></param>
        public static implicit operator Dictionary<string, OrderByOperater>(OrderByOperation orderByOperation)
        {
            return orderByOperation._orderByOperation;
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="pairs"></param>
        public static implicit operator OrderByOperation(Dictionary<string, OrderByOperater> pairs)
        {
            var obo = new OrderByOperation();
            foreach (var item in pairs)
            {
                obo.Add(item.Key, item.Value);
            }
            return obo;
        }

    }
}
