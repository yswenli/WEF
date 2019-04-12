/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2016
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：Administrator
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/

namespace WEF.Cache
{
    public class CacheInfo
    {
        /// <summary>
        /// 过期时间 单位：秒
        /// </summary>
        private int? timeout;

        /// <summary>
        /// 过期时间 单位：秒
        /// </summary>
        public int? TimeOut
        {
            get { return timeout; }
            set { timeout = value; }
        }


        /// <summary>
        /// 文件路径
        /// </summary>
        private string filePath;

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }


        /// <summary>
        /// 判断该cache是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsNullOrEmpty()
        {
            if (timeout.HasValue || !string.IsNullOrEmpty(filePath))
                return false;

            return true;
        }
    }
}
