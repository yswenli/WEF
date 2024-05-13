/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2024
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：fc1c123f-4e25-4cad-b5f8-10298585554f
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：WEF
 * 类名称：Entity
 * 创建时间：2020/6/2 18:56:24
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System.Collections.Generic;
using WEF.Common;
using WEF.Db;

namespace WEF
{
    /// <summary>
    /// WEF 实体信息接口
    /// </summary>
    public interface IEntity
    {
        void Attach();
        void Attach(EntityState entityState);
        void AttachAll();
        void AttachAll(bool ignoreNullOrEmpty);
        void ClearModifyFields();
        void DeAttach();
        EntityState GetEntityState();
        Field[] GetFields();
        Field GetIdentityField();
        List<ModifyField> GetModifyFields();
        List<string> GetModifyFieldsStr();
        Field[] GetPrimaryKeyFields();
        string GetSequence();
        string GetTableAsName();
        string GetTableName();
        void SetTableName(string tableName);
        string GetUserName();
        object[] GetValues();
        bool IsModify();
        bool IsReadOnly();
        void OnPropertyValueChange(Field field, object oldValue, object newValue);
    }
}