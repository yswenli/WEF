/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2019
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：91cda8c0-1bb9-4784-b3bb-755cd451d53d
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：WEF.Common
 * 类名称：ModifyField
 * 创建时间：2017/7/27 9:57:57
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System;

namespace WEF.Common
{
    /// <summary>
    /// 实体属性修改记录 （字段修改记录）
    /// </summary>
    [Serializable]
    public class ModifyField
    {
        private Field field;
        private object oldValue;
        private object newValue;

        /// <summary>
        /// 字段
        /// </summary>
        public Field Field { get { return field; } }

        /// <summary>
        /// 原始值
        /// </summary>
        public object OldValue { get { return oldValue; } set { oldValue = value; } }

        /// <summary>
        /// 新值
        /// </summary>
        public object NewValue { get { return newValue; } set { newValue = value; } }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        public ModifyField(Field field, object oldValue, object newValue)
        {
            this.field = field;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }
    }
}
