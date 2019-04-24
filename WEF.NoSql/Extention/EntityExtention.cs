using System.Collections.Generic;
using System.Linq;

namespace WEF.NoSql.Extention
{
    public static class EntityExtention
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
                result = 0;
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
