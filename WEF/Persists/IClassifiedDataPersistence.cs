/****************************************************************************
*项目名称：WEF.Persists
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Persists
*类 名 称：IClassifiedDataPersistence
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/10/13 9:52:41
*描述：
*=====================================================================
*修改时间：2020/10/13 9:52:41
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
namespace WEF.Persists
{
    /// <summary>
    /// IClassifiedDataPersistence
    /// </summary>
    public interface IClassifiedDataPersistence
    {
        /// <summary>
        /// Insert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        void Insert<T>(T t) where T : Entity;
    }
}
