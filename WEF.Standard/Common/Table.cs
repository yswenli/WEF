/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Linq;

namespace WEF.Common
{
    /// <summary>
    /// 标记实体类表名
    /// </summary>
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        private string _tableName;
        private string _userName;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        public TableAttribute(string tableName)
        {
            this._tableName = tableName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="username"></param>
        public TableAttribute(string tableName, string username)
        {
            this._tableName = tableName;
            this._userName = username;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetTableName()
        {
            return _tableName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return _userName;
        }


        static ConcurrentDictionary<Type, string> _dic = new ConcurrentDictionary<Type, string>();

        /// <summary>
        /// 根据类型获取表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTableName<T>()
        {
            var type = typeof(T);

            return GetTableName(type);           
        }

        /// <summary>
        /// 根据类型获取表名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTableName(Type type)
        {
            return _dic.GetOrAdd(type, (k) => ((TableAttribute)type.GetCustomAttributes(true).Where(b => b.GetType().Name == "TableAttribute").First()).GetTableName());
        }
    }
}
