/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Standard.DevelopTools.Common
*文件名： ConfigHelper
*版本号： V1.0.0.0
*唯一标识：9b642397-4ee6-4277-8a1a-52b6010fa188
*当前的用户域：OCEANIA
*创建人： Walle.Wen
*电子邮箱：Walle.Wen@oceania-inc.com
*创建时间：2021/7/19 14:20:22
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/7/19 14:20:22
*修改人： Walle.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/

namespace WEF.Standard.DevelopTools.Common
{
    public class ConfigHelper
    {
        string _filePath = "";

        public ConfigHelper(string filePath)
        {
            _filePath = filePath;
        }

        public T Read<T>()
        {
            try
            {
                return UtilsHelper.Read<T>(_filePath);
            }
            catch
            {
                return default(T);
            }
        }


        public void Write<T>(T t)
        {
            try
            {
                UtilsHelper.Write(_filePath, t);
            }
            catch { }
        }
    }
}
