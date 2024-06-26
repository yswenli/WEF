﻿/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2024
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：a566894a-54fa-4cd3-bc73-ddbcafe380c0
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：WEF.Expressions
 * 类名称：FastReflection
 * 创建时间：2017/7/26 14:41:36
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WEF.Expressions
{
    public abstract class FastReflectionCache<TKey, TValue> : IFastReflectionCache<TKey, TValue>
    {
        private Dictionary<TKey, TValue> m_cache = new Dictionary<TKey, TValue>();

        public TValue Get(TKey key)
        {
            TValue value = default(TValue);
            if (this.m_cache.TryGetValue(key, out value))
            {
                return value;
            }

            lock (key)
            {
                if (!this.m_cache.TryGetValue(key, out value))
                {
                    value = this.Create(key);
                    this.m_cache[key] = value;
                }
            }

            return value;
        }

        protected abstract TValue Create(TKey key);
    }
    public static class FastReflectionCaches
    {
        static FastReflectionCaches()
        {
            MethodInvokerCache = new MethodInvokerCache();
            PropertyAccessorCache = new PropertyAccessorCache();
            FieldAccessorCache = new FieldAccessorCache();
            ConstructorInvokerCache = new ConstructorInvokerCache();
        }

        public static IFastReflectionCache<MethodInfo, IMethodInvoker> MethodInvokerCache
        {
            get; set;
        }

        public static IFastReflectionCache<PropertyInfo, IPropertyAccessor> PropertyAccessorCache
        {
            get; set;
        }

        public static IFastReflectionCache<FieldInfo, IFieldAccessor> FieldAccessorCache
        {
            get; set;
        }

        public static IFastReflectionCache<ConstructorInfo, IConstructorInvoker> ConstructorInvokerCache
        {
            get; set;
        }
    }
    public static class FastReflectionExtensions
    {
        public static object FastInvoke(this MethodInfo methodInfo, object instance, params object[] parameters)
        {
            return FastReflectionCaches.MethodInvokerCache.Get(methodInfo).Invoke(instance, parameters);
        }

        public static void FastSetValue(this PropertyInfo propertyInfo, object instance, object value)
        {
            FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo).SetValue(instance, value);
        }

        public static object FastGetValue(this PropertyInfo propertyInfo, object instance)
        {
            return FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo).GetValue(instance);
        }

        public static object FastGetValue(this FieldInfo fieldInfo, object instance)
        {
            return FastReflectionCaches.FieldAccessorCache.Get(fieldInfo).GetValue(instance);
        }

        public static object FastInvoke(this ConstructorInfo constructorInfo, params object[] parameters)
        {
            return FastReflectionCaches.ConstructorInvokerCache.Get(constructorInfo).Invoke(parameters);
        }
    }
    public static class FastReflectionFactories
    {
        static FastReflectionFactories()
        {
            MethodInvokerFactory = new MethodInvokerFactory();
            PropertyAccessorFactory = new PropertyAccessorFactory();
            FieldAccessorFactory = new FieldAccessorFactory();
            ConstructorInvokerFactory = new ConstructorInvokerFactory();
        }

        public static IFastReflectionFactory<MethodInfo, IMethodInvoker> MethodInvokerFactory
        {
            get; set;
        }

        public static IFastReflectionFactory<PropertyInfo, IPropertyAccessor> PropertyAccessorFactory
        {
            get; set;
        }

        public static IFastReflectionFactory<FieldInfo, IFieldAccessor> FieldAccessorFactory
        {
            get; set;
        }

        public static IFastReflectionFactory<ConstructorInfo, IConstructorInvoker> ConstructorInvokerFactory
        {
            get; set;
        }
    }
    public interface IFastReflectionCache<TKey, TValue>
    {
        TValue Get(TKey key);
    }
    public interface IFastReflectionFactory<TKey, TValue>
    {
        TValue Create(TKey key);
    }
}
