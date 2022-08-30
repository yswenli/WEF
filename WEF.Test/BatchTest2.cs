/****************************************************************************
*项目名称：WEF.Test
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Test
*类 名 称：BatchTest2
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2021/3/12 16:30:38
*描述：
*=====================================================================
*修改时间：2021/3/12 16:30:38
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using WEF.Common;
using WEF.MvcPager;
using WEF.Section;

namespace WEF.Test
{
    public static class BatchTest2
    {

        public static void Test()
        {
            var cnnStr = "";

            var rep = new DBGiftbatchaddRepository(DatabaseType.MySql, cnnStr);

            var list = new List<DBGiftbatchadd>();

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < 1000; i++)
            {
                list.Add(new DBGiftbatchadd()
                {
                    Uid = i.ToString(),
                    Parentid = Guid.NewGuid().ToString("N"),
                    Createtime = DateTime.Now,
                    Remark = "中文行不行"
                });
            }

            rep.BulkInsert(list);

            stopwatch.Stop();

            Console.WriteLine($"test!,ms:{stopwatch.ElapsedMilliseconds}");
            Console.ReadLine();
        }

    }


    /// <summary>
    /// 实体类DBGiftbatchadd
    /// </summary>
    [Serializable, DataContract, Table("tb_giftbatchadd")]
    public partial class DBGiftbatchadd : Entity
    {
        private static string m_tableName;
        public DBGiftbatchadd() : base("tb_giftbatchadd") { m_tableName = "tb_giftbatchadd"; }
        public DBGiftbatchadd(string tableName) : base(tableName) { m_tableName = tableName; }

        #region Model
        private string _Parentid;
        private int? _Sendstatus;
        private string _IsOperSuc;
        private string _ErrorMsg;
        private string _Operuser;
        private DateTime? _Createtime;
        private string _Remark;
        private decimal? _Addgiftvalue;
        private string _Giftid;
        private string _Taskid;
        private string _Uid;
        private int _Id;
        /// <summary>
        /// Parentid 
        /// </summary>
        [DataMember]
        public string Parentid
        {
            get { return _Parentid; }
            set
            {
                this.OnPropertyValueChange(_.Parentid, _Parentid, value);
                this._Parentid = value;
            }
        }
        /// <summary>
        /// Sendstatus 
        /// </summary>
        [DataMember]
        public int? Sendstatus
        {
            get { return _Sendstatus; }
            set
            {
                this.OnPropertyValueChange(_.Sendstatus, _Sendstatus, value);
                this._Sendstatus = value;
            }
        }
        /// <summary>
        /// IsOperSuc 
        /// </summary>
        [DataMember]
        public string IsOperSuc
        {
            get { return _IsOperSuc; }
            set
            {
                this.OnPropertyValueChange(_.IsOperSuc, _IsOperSuc, value);
                this._IsOperSuc = value;
            }
        }
        /// <summary>
        /// ErrorMsg 
        /// </summary>
        [DataMember]
        public string ErrorMsg
        {
            get { return _ErrorMsg; }
            set
            {
                this.OnPropertyValueChange(_.ErrorMsg, _ErrorMsg, value);
                this._ErrorMsg = value;
            }
        }
        /// <summary>
        /// Operuser 
        /// </summary>
        [DataMember]
        public string Operuser
        {
            get { return _Operuser; }
            set
            {
                this.OnPropertyValueChange(_.Operuser, _Operuser, value);
                this._Operuser = value;
            }
        }
        /// <summary>
        /// Createtime 
        /// </summary>
        [DataMember]
        public DateTime? Createtime
        {
            get { return _Createtime; }
            set
            {
                this.OnPropertyValueChange(_.Createtime, _Createtime, value);
                this._Createtime = value;
            }
        }
        /// <summary>
        /// Remark 
        /// </summary>
        [DataMember]
        public string Remark
        {
            get { return _Remark; }
            set
            {
                this.OnPropertyValueChange(_.Remark, _Remark, value);
                this._Remark = value;
            }
        }
        /// <summary>
        /// Addgiftvalue 
        /// </summary>
        [DataMember]
        public decimal? Addgiftvalue
        {
            get { return _Addgiftvalue; }
            set
            {
                this.OnPropertyValueChange(_.Addgiftvalue, _Addgiftvalue, value);
                this._Addgiftvalue = value;
            }
        }
        /// <summary>
        /// Giftid 
        /// </summary>
        [DataMember]
        public string Giftid
        {
            get { return _Giftid; }
            set
            {
                this.OnPropertyValueChange(_.Giftid, _Giftid, value);
                this._Giftid = value;
            }
        }
        /// <summary>
        /// Taskid 
        /// </summary>
        [DataMember]
        public string Taskid
        {
            get { return _Taskid; }
            set
            {
                this.OnPropertyValueChange(_.Taskid, _Taskid, value);
                this._Taskid = value;
            }
        }
        /// <summary>
        /// Uid 
        /// </summary>
        [DataMember]
        public string Uid
        {
            get { return _Uid; }
            set
            {
                this.OnPropertyValueChange(_.Uid, _Uid, value);
                this._Uid = value;
            }
        }
        /// <summary>
        /// Id auto_increment
        /// </summary>
        [DataMember]
        public int Id
        {
            get { return _Id; }
            set
            {
                this.OnPropertyValueChange(_.Id, _Id, value);
                this._Id = value;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 获取实体中的标识列
        /// </summary>
        public override Field GetIdentityField()
        {
            return _.Id;
        }
        /// <summary>
        /// 获取实体中的主键列
        /// </summary>
        public override Field[] GetPrimaryKeyFields()
        {
            return new Field[] {
                _.Id};
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
                _.Parentid,
                _.Sendstatus,
                _.IsOperSuc,
                _.ErrorMsg,
                _.Operuser,
                _.Createtime,
                _.Remark,
                _.Addgiftvalue,
                _.Giftid,
                _.Taskid,
                _.Uid,
                _.Id};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
                this._Parentid,
                this._Sendstatus,
                this._IsOperSuc,
                this._ErrorMsg,
                this._Operuser,
                this._Createtime,
                this._Remark,
                this._Addgiftvalue,
                this._Giftid,
                this._Taskid,
                this._Uid,
                this._Id};
        }
        #endregion

        #region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// tb_giftbatchadd 
            /// </summary>
            public readonly static Field All = new Field("*", m_tableName);
            /// <summary>
            /// Parentid 
            /// </summary>
            public readonly static Field Parentid = new Field("parentid", m_tableName, "parentid");
            /// <summary>
            /// Sendstatus 
            /// </summary>
            public readonly static Field Sendstatus = new Field("sendstatus", m_tableName, "sendstatus");
            /// <summary>
            /// IsOperSuc 
            /// </summary>
            public readonly static Field IsOperSuc = new Field("IsOperSuc", m_tableName, "IsOperSuc");
            /// <summary>
            /// ErrorMsg 
            /// </summary>
            public readonly static Field ErrorMsg = new Field("ErrorMsg", m_tableName, "ErrorMsg");
            /// <summary>
            /// Operuser 
            /// </summary>
            public readonly static Field Operuser = new Field("Operuser", m_tableName, "Operuser");
            /// <summary>
            /// Createtime 
            /// </summary>
            public readonly static Field Createtime = new Field("createtime", m_tableName, "createtime");
            /// <summary>
            /// Remark 
            /// </summary>
            public readonly static Field Remark = new Field("remark", m_tableName, "remark");
            /// <summary>
            /// Addgiftvalue 
            /// </summary>
            public readonly static Field Addgiftvalue = new Field("addgiftvalue", m_tableName, "addgiftvalue");
            /// <summary>
            /// Giftid 
            /// </summary>
            public readonly static Field Giftid = new Field("giftid", m_tableName, "giftid");
            /// <summary>
            /// Taskid 
            /// </summary>
            public readonly static Field Taskid = new Field("taskid", m_tableName, "taskid");
            /// <summary>
            /// Uid 
            /// </summary>
            public readonly static Field Uid = new Field("uid", m_tableName, "uid");
            /// <summary>
            /// Id auto_increment
            /// </summary>
            public readonly static Field Id = new Field("id", m_tableName, "auto_increment");
        }
        #endregion


    }
    /// <summary>
    /// 实体类DBGiftbatchadd操作类
    /// </summary>
    public partial class DBGiftbatchaddRepository : BaseRepository<DBGiftbatchadd>
    {
        DBContext db;
        /// <summary>
        /// 构造方法
        /// </summary>
        public DBGiftbatchaddRepository()
        {
            db = new DBContext();
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public DBGiftbatchaddRepository(DBContext dbContext)
        {
            db = dbContext;
        }
        /// <summary>
        /// 构造方法
        /// <param name="connStrName">连接字符串中的名称</param>
        /// </summary>
        public DBGiftbatchaddRepository(string connStrName)
        {
            db = new DBContext(connStrName);
        }
        /// <summary>
        /// 构造方法
        /// <param name="dbType">数据库类型</param>
        /// <param name="connStr">连接字符串</param>
        /// </summary>
        public DBGiftbatchaddRepository(DatabaseType dbType, string connStr)
        {
            db = new DBContext(dbType, connStr);
        }
        /// <summary>
        /// 当前db操作上下文
        /// </summary>
        public DBContext DBContext
        {
            get
            {
                return db;
            }
        }
        /// <summary>
        /// 总数
        /// </summary>
        /// <returns></returns>
        public int Total
        {
            get
            {
                return Search().Count();
            }
        }
        /// <summary>
        /// 当前实体查询上下文
        /// </summary>
        public Search<DBGiftbatchadd> Search(string tableName = "")
        {
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = "tb_giftbatchadd";
            }
            return db.Search<DBGiftbatchadd>(tableName);
        }
        /// <summary>
        /// 当前实体查询上下文
        /// </summary>
        public Search<DBGiftbatchadd> Search(DBGiftbatchadd entity)
        {
            return db.Search<DBGiftbatchadd>(entity);
        }
        /// <summary>
        /// 获取实体
        /// <param name="Id">Id</param>
        /// <param name="tableName">表名</param>
        /// </summary>
        /// <returns></returns>
        public DBGiftbatchadd GetDBGiftbatchadd(int Id, string tableName = "")
        {
            return Search(tableName).Where(b => b.Id == Id).First();
        }
        /// <summary>
        /// 获取列表
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// </summary>
        /// <returns></returns>
        public List<DBGiftbatchadd> GetList(int pageIndex, int pageSize)
        {
            return this.Search().Page(pageIndex, pageSize).ToList();
        }
        /// <summary>
        /// 获取列表
        /// <param name="tableName">表名</param>
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// </summary>
        /// <returns></returns>
        public List<DBGiftbatchadd> GetList(string tableName, int pageIndex = 1, int pageSize = 12)
        {
            return this.Search(tableName).Page(pageIndex, pageSize).ToList();
        }
        /// <summary>
        /// 分页查询
        /// <param name="lambdaWhere">查询表达式</param>
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// <param name="orderBy">排序</param>
        /// <param name="asc">升降</param>
        /// </summary>
        /// <returns></returns>
        public PagedList<DBGiftbatchadd> GetPagedList(Expression<Func<DBGiftbatchadd, bool>> lambdaWhere, string tableName = "", int pageIndex = 1, int pageSize = 12, string orderBy = "Id", bool asc = true)
        {
            return this.Search(tableName).ToPagedList(lambdaWhere, pageIndex, pageSize, orderBy, asc);
        }
        /// <summary>
        /// 添加实体
        /// <param name="entity">传进的实体</param>
        /// </summary>
        public int Insert(DBGiftbatchadd entity)
        {
            return db.Insert(entity);
        }
        /// <summary>
        /// 批量添加实体
        /// <param name="entities">传进的实体列表</param>
        /// </summary>
        public void BulkInsert(IEnumerable<DBGiftbatchadd> entities)
        {
            db.BulkInsert(entities);
        }
        /// <summary>
        /// 更新实体
        /// <param name="entity">传进的实体</param>
        /// </summary>
        public int Update(DBGiftbatchadd entity)
        {
            return db.Update(entity);
        }
        /// <summary>
        /// 更新实体
        /// <param name="entities">传进的实体</param>
        /// </summary>
        public int Update(IEnumerable<DBGiftbatchadd> entities)
        {
            return db.Update(entities);
        }
        /// <summary>
        /// 删除实体
        /// <param name="entity">传进的实体</param>
        /// </summary>
        public int Delete(DBGiftbatchadd entity)
        {
            return db.Delete(entity);
        }
        /// <summary>
        /// 批量删除实体
        /// <param name="obj">传进的实体列表</param>
        /// </summary>
        public int Deletes(List<DBGiftbatchadd> entities)
        {
            return db.Delete<DBGiftbatchadd>(entities);
        }
        /// <summary>
        /// 持久化实体
        /// <param name="entity">传进的实体</param>
        /// </summary>
        public int Save(DBGiftbatchadd entity)
        {
            return db.Save<DBGiftbatchadd>(entity);
        }
        /// <summary>
        /// 批量持久化实体
        /// <param name="entities">传进的实体列表</param>
        /// </summary>
        public int Save(List<DBGiftbatchadd> entities)
        {
            return db.Save<DBGiftbatchadd>(entities);
        }
        /// <summary>
        /// 持久化实体
        /// <param name="tran">事务</param>
        /// <param name="entity">传进的实体</param>
        /// </summary>
        public int Save(DbTransaction tran, DBGiftbatchadd entity)
        {
            return db.Save<DBGiftbatchadd>(tran, entity);
        }
        /// <summary>
        /// 批量持久化实体
        /// <param name="tran">事务</param>
        /// <param name="entity">传进的实体列表</param>
        /// </summary>
        public int Save(DbTransaction tran, List<DBGiftbatchadd> entities)
        {
            return db.Save<DBGiftbatchadd>(tran, entities);
        }
        /// <summary>
        /// 执行sql语句
        /// <param name="sql"></param>
        /// </summary>
        public SqlSection FromSql(string sql)
        {
            return db.FromSql(sql);
        }
        /// <summary>
        /// 执行存储过程
        /// <param name="sql"></param>
        /// </summary>
        public ProcSection FromProc(string procName)
        {
            return db.FromProc(procName);
        }

        public int Deletes(IEnumerable<DBGiftbatchadd> objs)
        {
            throw new NotImplementedException();
        }
    }





}






