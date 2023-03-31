/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/

using System.Collections.Concurrent;
using System.Collections.Generic;
using WEF.Db;

namespace WEF.Common
{
    /// <summary>
    /// 实体信息缓存
    /// </summary>
    public class EntityCache
    {
        /// <summary>
        /// 保存实体列表
        /// </summary>
        private static Dictionary<string, object> _entityList = new Dictionary<string, object>();

        /// <summary>
        /// lock object
        /// </summary>
        private static readonly object LockObj = new object();


        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public static void Reset()
        {
            _entityList.Clear();
        }

        /// <summary>
        /// 清理具体实体的缓存
        /// </summary>
        public static void Reset<TEntity>()
            where TEntity : Entity
        {
            var typestring = typeof(TEntity).ToString();
            if (_entityList.ContainsKey(typestring))
                _entityList.Remove(typestring);
        }

        /// <summary>
        /// 返回表名
        /// </summary>
        /// <returns></returns>
        public static string GetTableName<TEntity>()
            where TEntity : Entity
        {
            return getTEntity<TEntity>().GetTableName();
        }


        static ConcurrentDictionary<string,string> _tableNames= new ConcurrentDictionary<string,string>();

        /// <summary>
        /// 从缓存中加载表名
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static string GetTableNameFromCache<TEntity>()
            where TEntity : Entity
        {

            var type = typeof(TEntity);
            var typeStr = typeof(TEntity).ToString();

            return _tableNames.GetOrAdd(typeStr, (k) => {
                var tba = type.GetCustomAttribute<TableAttribute>(false) as TableAttribute;
                var tableName = tba != null ? tba.GetTableName() : type.Name;
                return tableName;
            });
        }

        /// <summary>
        /// 返回用户名
        /// </summary>
        /// <returns></returns>
        public static string GetUserName<TEntity>()
            where TEntity : Entity
        {
            return getTEntity<TEntity>().GetUserName();
        }
        /// <summary>
        /// 返回T
        /// </summary>
        /// <returns></returns>
        private static TEntity getTEntity<TEntity>()
            where TEntity : Entity
        {
            var typestring = typeof(TEntity).ToString();

            if (_entityList.ContainsKey(typestring))
                return (TEntity)_entityList[typestring];

            lock (LockObj)
            {
                if (_entityList.ContainsKey(typestring))
                    return (TEntity)_entityList[typestring];

                var t = DataUtils.Create<TEntity>();
                _entityList.Add(typestring, t);
                return t;
            }
        }


        /// <summary>
        /// 获取主键字段
        /// </summary>
        /// <returns></returns>
        public static Field[] GetPrimaryKeyFields<TEntity>()
            where TEntity : Entity
        {
            return getTEntity<TEntity>().GetPrimaryKeyFields();
        }

        /// <summary>
        /// 返回所有字段
        /// </summary>
        /// <returns></returns>
        public static Field[] GetFields<TEntity>()
            where TEntity : Entity
        {
            return getTEntity<TEntity>().GetFields();
        }

        /// <summary>
        /// GetValues
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static object[] GetValues<TEntity>()
            where TEntity : Entity
        {
            return getTEntity<TEntity>().GetValues();
        }

        /// <summary>
        /// GetModifyFields
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static List<ModifyField> GetModifyFields<TEntity>()
         where TEntity : Entity
        {
            return getTEntity<TEntity>().GetModifyFields();
        }

        /// <summary>
        /// 返回第一个字段
        /// </summary>
        /// <returns></returns>
        public static Field GetFirstField<TEntity>()
            where TEntity : Entity
        {
            var fields = GetFields<TEntity>();
            if (null != fields && fields.Length > 0)
                return fields[0];
            return null;
        }


        /// <summary>
        /// 返回标识字段
        /// </summary>
        /// <returns></returns>
        public static Field GetIdentityField<TEntity>()
            where TEntity : Entity
        {
            return getTEntity<TEntity>().GetIdentityField();
        }

        /// <summary>
        /// 是否只读
        /// </summary>
        /// <returns></returns>
        public static bool IsReadOnly<TEntity>()
            where TEntity : Entity
        {
            return getTEntity<TEntity>().IsReadOnly();
        }


        /// <summary>
        /// 标识列的名称（Oracle）
        /// </summary>
        /// <returns></returns>
        public static string GetSequence<TEntity>()
            where TEntity : Entity
        {
            return getTEntity<TEntity>().GetSequence();
        }
    }
}
