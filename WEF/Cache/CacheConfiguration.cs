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

using System.Configuration;

namespace WEF.Cache
{


    /// <summary>
    /// 缓存设置
    /// </summary>
    public class CacheConfiguration : ConfigurationSection
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private bool? enableCache = null;


        /// <summary>
        /// 是否开启缓存
        /// </summary>
        [ConfigurationProperty("enable")]
        public bool Enable
        {
            get
            {
                if (enableCache.HasValue)
                    return enableCache.Value;

                if (this["enable"] == null)
                    return false;

                return (bool)this["enable"];

            }
            set
            {
                enableCache = value;
            }
        }

        /// <summary>
        /// 设置表缓存
        /// </summary>
        [ConfigurationProperty("entities", IsRequired = true, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(KeyValueConfigurationCollection))]
        public KeyValueConfigurationCollection Entities
        {
            get
            {
                return (KeyValueConfigurationCollection)this["entities"];
            }
        }
    }
}
