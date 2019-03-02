using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WEF.Common;
using WEF.Section;

namespace WEF.Test.Models
{
    /// <summary>
	/// 实体类ArticleKind
	/// </summary>
	[Serializable, DataContract]
    public partial class ArticleKind : Entity
    {
        private static string m_tableName;
        public ArticleKind() : base("ArticleKind") { m_tableName = "ArticleKind"; }
        public ArticleKind(string tableName) : base(tableName) { m_tableName = tableName; }

        #region Model
        private int _ID;
        private int? _SortID;
        private int? _PID;
        private string _Name;
        private string _Discription;
        private DateTime? _CreateDate;
        private int? _CreateUserID;
        private DateTime? _LastUpdateDate;
        private int? _LastUpdateUserID;
        private bool? _IsDeleted;
        /// <summary>
        /// ID 
        /// </summary>
        [DataMember]
        public int ID
        {
            get { return _ID; }
            set
            {
                this.OnPropertyValueChange(_.ID, _ID, value);
                this._ID = value;
            }
        }
        /// <summary>
        /// SortID 
        /// </summary>
        [DataMember]
        public int? SortID
        {
            get { return _SortID; }
            set
            {
                this.OnPropertyValueChange(_.SortID, _SortID, value);
                this._SortID = value;
            }
        }
        /// <summary>
        /// PID 
        /// </summary>
        [DataMember]
        public int? PID
        {
            get { return _PID; }
            set
            {
                this.OnPropertyValueChange(_.PID, _PID, value);
                this._PID = value;
            }
        }
        /// <summary>
        /// Name 
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set
            {
                this.OnPropertyValueChange(_.Name, _Name, value);
                this._Name = value;
            }
        }
        /// <summary>
        /// Discription 
        /// </summary>
        [DataMember]
        public string Discription
        {
            get { return _Discription; }
            set
            {
                this.OnPropertyValueChange(_.Discription, _Discription, value);
                this._Discription = value;
            }
        }
        /// <summary>
        /// CreateDate 
        /// </summary>
        [DataMember]
        public DateTime? CreateDate
        {
            get { return _CreateDate; }
            set
            {
                this.OnPropertyValueChange(_.CreateDate, _CreateDate, value);
                this._CreateDate = value;
            }
        }
        /// <summary>
        /// CreateUserID 
        /// </summary>
        [DataMember]
        public int? CreateUserID
        {
            get { return _CreateUserID; }
            set
            {
                this.OnPropertyValueChange(_.CreateUserID, _CreateUserID, value);
                this._CreateUserID = value;
            }
        }
        /// <summary>
        /// LastUpdateDate 
        /// </summary>
        [DataMember]
        public DateTime? LastUpdateDate
        {
            get { return _LastUpdateDate; }
            set
            {
                this.OnPropertyValueChange(_.LastUpdateDate, _LastUpdateDate, value);
                this._LastUpdateDate = value;
            }
        }
        /// <summary>
        /// LastUpdateUserID 
        /// </summary>
        [DataMember]
        public int? LastUpdateUserID
        {
            get { return _LastUpdateUserID; }
            set
            {
                this.OnPropertyValueChange(_.LastUpdateUserID, _LastUpdateUserID, value);
                this._LastUpdateUserID = value;
            }
        }
        /// <summary>
        /// IsDeleted 
        /// </summary>
        [DataMember]
        public bool? IsDeleted
        {
            get { return _IsDeleted; }
            set
            {
                this.OnPropertyValueChange(_.IsDeleted, _IsDeleted, value);
                this._IsDeleted = value;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 获取实体中的标识列
        /// </summary>
        public override Field GetIdentityField()
        {
            return _.ID;
        }
        /// <summary>
        /// 获取实体中的主键列
        /// </summary>
        public override Field[] GetPrimaryKeyFields()
        {
            return new Field[] {
                _.ID};
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
                _.ID,
                _.SortID,
                _.PID,
                _.Name,
                _.Discription,
                _.CreateDate,
                _.CreateUserID,
                _.LastUpdateDate,
                _.LastUpdateUserID,
                _.IsDeleted};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
                this._ID,
                this._SortID,
                this._PID,
                this._Name,
                this._Discription,
                this._CreateDate,
                this._CreateUserID,
                this._LastUpdateDate,
                this._LastUpdateUserID,
                this._IsDeleted};
        }
        #endregion

        #region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// ArticleKind 
            /// </summary>
            public readonly static Field All = new Field("*", m_tableName);
            /// <summary>
            /// ID 
            /// </summary>
            public readonly static Field ID = new Field("ID", m_tableName, "ID");
            /// <summary>
            /// SortID 
            /// </summary>
            public readonly static Field SortID = new Field("SortID", m_tableName, "SortID");
            /// <summary>
            /// PID 
            /// </summary>
            public readonly static Field PID = new Field("PID", m_tableName, "PID");
            /// <summary>
            /// Name 
            /// </summary>
            public readonly static Field Name = new Field("Name", m_tableName, "Name");
            /// <summary>
            /// Discription 
            /// </summary>
            public readonly static Field Discription = new Field("Discription", m_tableName, "Discription");
            /// <summary>
            /// CreateDate 
            /// </summary>
            public readonly static Field CreateDate = new Field("CreateDate", m_tableName, "CreateDate");
            /// <summary>
            /// CreateUserID 
            /// </summary>
            public readonly static Field CreateUserID = new Field("CreateUserID", m_tableName, "CreateUserID");
            /// <summary>
            /// LastUpdateDate 
            /// </summary>
            public readonly static Field LastUpdateDate = new Field("LastUpdateDate", m_tableName, "LastUpdateDate");
            /// <summary>
            /// LastUpdateUserID 
            /// </summary>
            public readonly static Field LastUpdateUserID = new Field("LastUpdateUserID", m_tableName, "LastUpdateUserID");
            /// <summary>
            /// IsDeleted 
            /// </summary>
            public readonly static Field IsDeleted = new Field("IsDeleted", m_tableName, "IsDeleted");
        }
        #endregion


    }
    /// <summary>
    /// 实体类ArticleKind操作类
    /// </summary>
    public partial class ArticleKindRepository
    {
        DBContext db;
        /// <summary>
        /// 当前实体查询上下文
        /// </summary>
        public ISearch<ArticleKind> Search(string tableName = "")
        {
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = "ArticleKind";
            }
            return db.Search<ArticleKind>(tableName);
        }
        /// <summary>
        /// 当前实体查询上下文
        /// </summary>
        public ISearch<ArticleKind> Search(ArticleKind entity)
        {
            return db.Search<ArticleKind>(entity);
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public ArticleKindRepository()
        {
            db = new DBContext();
        }
        /// <summary>
        /// 构造方法
        /// <param name="connStrName">连接字符串中的名称</param>
        /// </summary>
        public ArticleKindRepository(string connStrName)
        {
            db = new DBContext(connStrName);
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
        /// 获取实体
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// </summary>
        public ArticleKind GetArticleKind(int id)
        {
            return Search().Where(b => b.ID == id).First();
        }
        /// <summary>
        /// 获取列表
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// </summary>
        public List<ArticleKind> GetList(int pageIndex, int pageSize)
        {
            return this.Search().Page(pageIndex, pageSize).ToList();
        }
        /// <summary>
        /// 获取列表
        /// <param name="tableName">表名</param>
        /// <param name="pageIndex">分页第几页</param>
        /// <param name="pageSize">分页一页取值</param>
        /// </summary>
        public List<ArticleKind> GetList(string tableName, int pageIndex = 1, int pageSize = 12)
        {
            return this.Search(tableName).Page(pageIndex, pageSize).ToList();
        }
        /// <summary>
        /// 添加实体
        /// <param name="obj">传进的实体</param>
        /// </summary>
        public int Insert(ArticleKind obj)
        {
            return db.Insert(obj);
        }
        /// <summary>
        /// 更新实体
        /// <param name="obj">传进的实体</param>
        /// </summary>
        public int Update(ArticleKind obj)
        {
            return db.Update(obj);
        }
        /// <summary>
        /// 删除实体
        /// <param name="obj">传进的实体</param>
        /// </summary>
        public int Delete(ArticleKind obj)
        {
            return db.Delete(obj);
        }
        /// <summary>
        /// 删除实体
        /// <param name="id">id</param>
        /// </summary>
        public int Delete(int id)
        {
            var obj = Search().Where(b => b.ID == id).First();
            return db.Delete(obj);
        }
        /// <summary>
        /// 批量删除实体
        /// <param name="obj">传进的实体列表</param>
        /// </summary>
        public int Deletes(List<ArticleKind> objs)
        {
            return db.Delete<ArticleKind>(objs);
        }
        /// <summary>
        /// 执行sql语句
        /// <param name="sql"></param>
        /// </summary>
        public SqlSection ExecuteSQL(string sql)
        {
            return db.FromSql(sql);
        }
        /// <summary>
        /// 执行存储过程
        /// <param name="sql"></param>
        /// </summary>
        public ProcSection ExcuteProc(string procName)
        {
            return db.FromProc(procName);
        }
    }
}
