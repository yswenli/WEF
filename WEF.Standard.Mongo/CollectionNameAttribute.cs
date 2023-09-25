using System;

namespace WEF.Standard.Mongo
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CollectionNameAttribute : Attribute
    {
        /// <summary>
        /// initializes新实例of the收藏类属性with the desired name。
        /// </summary>
        /// <param name="value"></param>
        public CollectionNameAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("不允许空collection name", "value");
            Name = value;
        }

        /// <summary>
        /// 获取集合的名称
        /// </summary>
        /// <value>.</value>
        public virtual string Name
        {
            get; private set;
        }
    }
}
