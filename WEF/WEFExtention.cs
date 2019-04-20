/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2019
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：c7fbd6e7-f64f-4258-b6e8-af04f278e907
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：WEF
 * 类名称：ObjectExtention
 * 创建时间：2017/7/26 17:20:19
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;

namespace WEF
{
    /// <summary>
    /// WEF lambda相关检测扩展
    /// </summary>
    public static class WEFExtention
    {
        /// <summary>
        /// 
        /// </summary>
        private const string Tips = "该方法({0})只能用于WEF lambda表达式！";


        /// <summary>
        /// like '%value%' 模糊查询，同Contains。
        /// </summary>
        public static bool Like(this object key, object values)
        {
            throw new Exception(string.Format(Tips, "Like"));
        }
        /// <summary>
        /// where field in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        public static bool In<T>(this object key, params T[] values)
        {
            throw new Exception(string.Format(Tips, "In"));
        }
        /// <summary>
        /// where field in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        public static bool In<T>(this object key, List<T> values)
        {
            throw new Exception(string.Format(Tips, "In"));
        }
        /// <summary>
        /// where field not in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        public static bool NotIn<T>(this object key, params T[] values)
        {
            throw new Exception(string.Format(Tips, "NotIn"));
        }
        /// <summary>
        /// where field not in (value,value,value)。传入Array或List&lt;T>。
        /// </summary>
        public static bool NotIn<T>(this object key, List<T> values)
        {
            throw new Exception(string.Format(Tips, "NotIn"));
        }
        /// <summary>
        /// IS NULL
        /// </summary>
        public static bool IsNull(this object key)
        {
            throw new Exception(string.Format(Tips, "IsNull"));
        }
        /// <summary>
        /// IS NOT NULL
        /// </summary>
        public static bool IsNotNull(this object key)
        {
            throw new Exception(string.Format(Tips, "IsNotNull"));
        }
        /// <summary>
        /// As
        /// </summary>
        public static bool As(this object key, string values)
        {
            throw new Exception(string.Format(Tips, "As"));
        }
        /// <summary>
        /// Sum
        /// </summary>
        public static decimal Sum(this object key)
        {
            throw new Exception(string.Format(Tips, "Sum"));
        }
        /// <summary>
        /// Count
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int Count(this object key)
        {
            throw new Exception(string.Format(Tips, "Count"));
        }
        /// <summary>
        /// Avg
        /// </summary>
        public static decimal Avg(this object key)
        {
            throw new Exception(string.Format(Tips, "Avg"));
        }
        /// <summary>
        /// Len
        /// </summary>
        public static int Len(this object key)
        {
            throw new Exception(string.Format(Tips, "Len"));
        }


        /// <summary>
        /// 深复制当前对象到另外的实体,
        /// 可触发source的属性修改
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Target ConvertTo<Source, Target>(this Source source)
            where Source : class, new()
            where Target : class, new()
        {
            var json = JsonSerialize(source);

            return JsonDeserialize<Target>(json);

            //return source.ConvertTo<Target>();
        }

        /// <summary>
        /// 深复制当前对象到另外的实体,
        /// 可触发source的属性修改
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="sources"></param>
        /// <returns></returns>
        public static List<Target> ConvertTo<Source, Target>(this List<Source> sources)
            where Source : class, new()
            where Target : class, new()
        {
            var json = JsonSerialize(sources);

            return JsonDeserialize<List<Target>>(json);

            //return sources.ConvertTo<List<Target>>();
        }

        /// <summary>
        ///     将C#数据实体转化为JSON数据
        /// </summary>
        /// <param name="obj">要转化的数据实体</param>
        /// <returns>JSON格式字符串</returns>
        public static string JsonSerialize(object obj)
        {
            System.Web.Script.Serialization.JavaScriptSerializer s = new System.Web.Script.Serialization.JavaScriptSerializer();
            return s.Serialize(obj);
        }

        /// <summary>
        ///     将JSON数据转化为C#数据实体
        /// </summary>
        /// <param name="json">符合JSON格式的字符串</param>
        /// <returns>T类型的对象</returns>
        public static T JsonDeserialize<T>(string json)
        {
            System.Web.Script.Serialization.JavaScriptSerializer d = new System.Web.Script.Serialization.JavaScriptSerializer();
            return d.Deserialize<T>(json);
        }

    }


}
