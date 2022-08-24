/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF
*文件名： BaseRepository
*版本号： V1.0.0.0
*唯一标识：565b31a1-4753-4ffb-bbae-f0dc1eae1b38
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2021/5/20 15:11:37
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/5/20 15:11:37
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;

using WEF.Db;
using WEF.MvcPager;
using WEF.Section;

namespace WEF
{
    public interface IRepository<T> where T : Entity, new()
    {
        DBContext DBContext { get; }
        int Total { get; }

        void BulkInsert(IEnumerable<T> entities);
        DbTrans CreateTransaction(int timeout = 30);
        int Delete(Expression<Func<T, bool>> lambdaWhere);
        int Delete(IEnumerable<int> ids);
        int Delete(IEnumerable<string> ids);
        int Delete(int id);
        int Delete(string id);
        int Delete(T entity);
        int Deletes(IEnumerable<T> entities);
        ProcSection FromProc(string procName);
        ProcSection FromProc(string procName, Dictionary<string, object> inputParamas);
        ProcSection FromProc<Model>(string procName, Model inputParamas) where Model : class, new();
        SqlSection FromSql(string sql);
        SqlSection FromSql(string sql, Dictionary<string, object> inputParamas);
        SqlSection FromSql<Model>(string sql, Model inputParamas) where Model : class, new();
        T Get(int id);
        T Get(string id);
        List<T> GetList(IEnumerable<int> ids);
        List<T> GetList(IEnumerable<string> ids);
        List<T> GetList(int pageIndex, int pageSize);
        List<T> GetList(string tableName, int pageIndex = 1, int pageSize = 12);
        PagedList<T> GetPagedList(Expression<Func<T, bool>> lambdaWhere, int pageIndex = 1, int pageSize = 12, string orderBy = "ID", bool asc = true);
        PagedList<T> GetPagedList(Expression<Func<T, bool>> lambdaWhere, string tableName = "", int pageIndex = 1, int pageSize = 12, string orderBy = "ID", bool asc = true);
        PagedList<T> GetPagedList(int pageIndex, int pageSize, string orderBy = "ID", bool asc = true);
        int Insert(T entity);
        int Save(DbTransaction tran, List<T> entities);
        int Save(DbTransaction tran, T entity);
        int Save(List<T> entities);
        int Save(T entity);
        ISearch<T> Search(string tableName = "");
        ISearch<T> Search(T entity);
        int Update(IEnumerable<T> entities);
        int Update(T entity);
        int Update(T entity, Expression<Func<T, bool>> lambdaWhere);
    }
}