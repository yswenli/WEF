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
 * 命名空间：WEF.NoSql.Extention
 * 类名称：MongoEntityExtention
 * 文件名：MongoEntityExtention
 * 创建年份：2015
 * 创建时间：2015-09-29 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using System.Collections.Generic;
using System.Linq;

namespace WEF.NoSql.Extention
{
    public static class MongoEntityExtention
    {

        /// <summary>
        /// 数字转换功能
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ParseInt(this string str)
        {
            int result = 0;

            if (!int.TryParse(str, out result))
            {
                result = 30;
            }
            return result;
        }


        /// <summary>
        /// 实体转换
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget Covert<TSource, TTarget>(this TSource source)
            where TSource : new()
            where TTarget : new()
        {
            var target = new TTarget();

            if (source != null)
            {
                var type1 = source.GetType();
                var type2 = target.GetType();

                var properties1 = type1.GetProperties();

                var properties2 = type2.GetProperties();

                foreach (var item in properties1)
                {
                    var v = item.GetValue(source, null);

                    if (v != null)
                    {
                        var p = properties2.Where(b => b.Name == item.Name).FirstOrDefault();
                        if (p != null)
                        {
                            p.SetValue(target, v, null);
                        }
                    }
                }
                return target;
            }
            else
            {
                return default(TTarget);
            }
        }


        /// <summary>
        /// 集合转换
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TTarget> ToList<TSource, TTarget>(this IEnumerable<TSource> source)
            where TSource : new()
            where TTarget : new()
        {
            var result = new List<TTarget>();

            foreach (var item in source)
            {
                var t = item.Covert<TSource, TTarget>();
                if (t != null)
                    result.Add(t);
            }
            return result;
        }
    }
}
