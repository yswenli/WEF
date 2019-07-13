/****************************************************************************
*项目名称：WEF
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：WEF
*类 名 称：IRepository
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/7/11 14:13:38
*描述：
*=====================================================================
*修改时间：2019/7/11 14:13:38
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System.Collections.Generic;
using WEF.Section;

namespace WEF
{
    public interface IRepository<T> where T : Entity
    {
        DBContext DBContext { get; }

        int Delete(T obj);
        int Deletes(List<T> objs);
        ProcSection ExcuteProc(string procName);
        SqlSection ExecuteSQL(string sql);
        List<T> GetList(int pageIndex, int pageSize);
        List<T> GetList(string tableName, int pageIndex = 1, int pageSize = 12);
        int Insert(T obj);
        ISearch<T> Search(string tableName = "");
        ISearch<T> Search(T entity);
        int Update(T obj);
    }
}
