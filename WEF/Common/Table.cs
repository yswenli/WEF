/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2019
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/

using System;

namespace WEF.Common
{
    /// <summary>
    /// 标记实体类表名
    /// </summary>
    public class Table : Attribute
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
        public Table(string tableName)
        {
            this._tableName = tableName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="username"></param>
        public Table(string tableName, string username)
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
    }
}
