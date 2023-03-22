/****************************************************************************
*Copyright (c) 2023 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Common
*文件名： FastCopy
*版本号： V1.0.0.0
*唯一标识：f59839ee-708c-4dcb-a57a-c0c7ba3e368a
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2023/3/15 15:47:28
*描述：快速复制
*
*=================================================
*修改标记
*修改时间：2023/3/15 15:47:28
*修改人： yswenli
*版本号： V1.0.0.0
*描述：快速复制
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace WEF.Common
{
    /// <summary>
    /// 快速复制
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public static class FastCopy<TIn, TOut>
    {
        private static readonly Func<TIn, TOut> cache = GetFunc();
        private static Func<TIn, TOut> GetFunc()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();

            foreach (var item in typeof(TOut).GetProperties())
            {
                if (!item.CanWrite)
                    continue;

                MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }

            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

            return lambda.Compile();
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="tIn"></param>
        /// <returns></returns>
        public static TOut Copy(TIn tIn)
        {
            return cache(tIn);
        }
    }

    /// <summary>
    /// 快速复制
    /// </summary>
    public static class DeepCopy
    {
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static dynamic Clone(this object obj)
        {
            if (obj == null || (obj is string) || (obj.GetType().IsValueType)) return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                try { field.SetValue(retval, Clone(field.GetValue(obj))); }
                catch { }
            }
            return retval;
        }
    }
}
