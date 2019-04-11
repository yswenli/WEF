/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2017
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：fc1c123f-4e25-4cad-b5f8-10298585554f
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：WEF
 * 类名称：Entity
 * 创建时间：2017/7/27 9:56:24
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using WEF.Common;

namespace WEF
{
    /// <summary>
    /// 实体信息
    /// </summary>
    [Serializable]
    public class Entity
    {
        /// <summary>
        /// 表名
        /// </summary>
        private string _tableName;
        /// <summary>
        /// 
        /// </summary>
        private string _userName;
        /// <summary>
        /// 别名
        /// </summary>
        private string _tableAsName;
        /// <summary>
        /// 是否
        /// </summary>
        private bool _isAttached;

        private bool _isFilterModifyFields = false;//2016-02-03新增
        /// <summary>
        /// 实体状态
        /// </summary>
        private EntityState _entityState = EntityState.Unchanged;
        /// <summary>
        /// select *。用于Lambda写法实现 select * 。注：表中不得含有字段名为All。
        /// </summary>
        [XmlIgnore]
        [NonSerialized]
        [ScriptIgnore]
        public object All;
        ///// <summary>
        ///// 参数计数器  2015-07-30
        ///// </summary>
        //public int paramCount = 0;
        /// <summary>
        /// 修改的字段集合
        /// </summary>
        private List<ModifyField> _modifyFields = new List<ModifyField>();
        /// <summary>
        /// 修改的字段集合 v1.10.5.6及以上版本可使用。
        /// </summary>
        private List<string> _modifyFieldsStr = new List<string>();

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Entity()
        {
            var tbl = GetType().GetCustomAttribute<Table>(false) as Table;
            _tableName = tbl != null ? tbl.GetTableName() : GetType().Name;
            _userName = tbl != null ? tbl.GetUserName() : "";
            _isAttached = true;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名</param>
        public Entity(string tableName)
        {
            this._tableName = tableName;
            _isAttached = true;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="userName"></param>
        public Entity(string tableName, string userName)
        {
            this._tableName = tableName;
            this._userName = userName;
            _isAttached = true;
        }
        #endregion
        /// <summary>
        /// 将实体置为修改状态
        /// </summary>
        public void Attach()
        {
            _isAttached = true;
        }
        /// <summary>
        /// 将实体所有字段置为修改状态
        /// </summary>
        public void AttachAll()
        {
            AttachAll(false);
        }
        /// <summary>
        /// 将实体所有字段置为修改状态
        /// </summary>
        /// <param name="ignoreNullOrEmpty">忽略null值字段与空字符串字段</param>
        public void AttachAll(bool ignoreNullOrEmpty)
        {
            var fs = GetFields();
            var values = GetValues();
            for (int i = 0; i < fs.Length; i++)
            {
                if (ignoreNullOrEmpty && (values[i] == null || string.IsNullOrEmpty(values[i].ToString())))
                {
                    continue;
                }
                _modifyFields.Add(new ModifyField(fs[i], values[i], values[i]));
            }
        }

        /// <summary>
        /// 将实体置为指定状态（仅对.Save()有效果）
        /// </summary>
        public void Attach(EntityState entityState)
        {
            this._entityState = entityState;
        }
        /// <summary>
        /// 获取实体状态
        /// </summary>
        public EntityState GetEntityState()
        {
            return _entityState;
        }
        /// <summary>
        /// 1、恢复实体为默认状态。
        /// 2、标记实体为不做任何数据库操作（仅对.Save()有效果）
        /// </summary>
        public void DeAttach()
        {
            _isAttached = false;
            _entityState = EntityState.Unchanged;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        public void OnPropertyValueChange(string f)
        {
            if (_isAttached)
            {
                _modifyFieldsStr.Add(f);
            }
        }

        /// <summary>
        /// 记录字段修改
        /// </summary>
        /// <param name="field"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public void OnPropertyValueChange(Field field, object oldValue, object newValue)
        {
            if (_isAttached)
            {
                lock (_modifyFields)
                {
                    _isFilterModifyFields = true;
                    //自增主键不参与更新
                    if (GetIdentityField() != null)
                    {
                        if (GetIdentityField().FieldName != field.FieldName)
                        {
                            _modifyFields.Add(new ModifyField(field, oldValue, newValue));
                        }
                    }
                    else
                    {
                        _modifyFields.Add(new ModifyField(field, oldValue, newValue));
                    }
                }
            }
        }
        /// <summary>
        /// 清除修改记录
        /// </summary>
        public void ClearModifyFields()
        {
            _modifyFields.Clear();
            _modifyFieldsStr.Clear();
        }
        /// <summary>
        /// GetFields
        /// </summary>
        /// <returns></returns>
        public virtual Field[] GetFields()
        {
            return null;
        }
        /// <summary>
        /// GetValues
        /// </summary>
        /// <returns></returns>
        public virtual object[] GetValues()
        {
            return null;
        }
        /// <summary>
        /// GetPrimaryKeyFields
        /// </summary>
        /// <returns></returns>
        public virtual Field[] GetPrimaryKeyFields()
        {
            return null;
        }
        /// <summary>
        /// 标识列
        /// </summary>
        public virtual Field GetIdentityField()
        {
            return null;
        }
        /// <summary>
        /// 标识列的名称（例如如Oracle中Sequence名称）
        /// </summary>
        /// <returns></returns>
        public virtual string GetSequence()
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsModify()
        {
            return _modifyFieldsStr.Count > 0;
        }

        /// <summary>
        /// 返回修改记录。v.10.5.5以上版本请不要使用此方法。
        /// </summary>
        public List<ModifyField> GetModifyFields()
        {
            if (_isFilterModifyFields)
            {
                var newFileds = new List<ModifyField>();
                for (int i = _modifyFields.Count - 1; i >= 0; i--)
                {
                    newFileds.Add(_modifyFields[i]);
                }
                newFileds = newFileds.Distinct(new ModelComparer()).ToList();
                _isFilterModifyFields = false;
                _modifyFields = newFileds;
                return newFileds;
            }
            return _modifyFields;
        }
        /// <summary>
        /// 返回修改记录。v.10.5.5及以下版本请不要使用此方法。
        /// </summary>
        public List<string> GetModifyFieldsStr()
        {
            return _modifyFieldsStr.Distinct().ToList();
        }
        private class ModelComparer : IEqualityComparer<ModifyField>
        {
            public bool Equals(ModifyField x, ModifyField y)
            {
                return string.Equals(x.Field.PropertyName, y.Field.PropertyName, StringComparison.CurrentCultureIgnoreCase);
            }
            public int GetHashCode(ModifyField obj)
            {
                return obj.Field.PropertyName.ToUpper().GetHashCode();
            }
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        public virtual bool IsReadOnly()
        {
            return false;
        }
        /// <summary>
        /// 获取表名
        /// </summary>
        public string GetTableName()
        {
            return _tableName;
        }
        /// <summary>
        /// 获取表名
        /// </summary>
        public string GetUserName()
        {
            return _userName;
        }
        /// <summary>
        /// 获取表名别名
        /// </summary>
        public string GetTableAsName()
        {
            return _tableAsName;
        }
    }
}
