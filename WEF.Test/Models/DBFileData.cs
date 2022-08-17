//------------------------------------------------------------------------------
// <WEF-ModelGenerator>
//     此代码由WEF数据库工具, Version=5.2.1.7, Culture=neutral, PublicKeyToken=null生成;时间 2022-08-17 11:06:00.988
//     运行时版本:4.0.30319.42000
//     不建议手动更改此代码，如有需要请自行扩展partial类
// </WEF-ModelGenerator>
//------------------------------------------------------------------------------


using System;
using System.Data;
using System.Runtime.Serialization;

using WEF.Common;

namespace WEF.Models
{

    /// <summary>
    /// 实体类DBFileData
    /// </summary>
    [Serializable, DataContract, Table("FileData")]
    public partial class DBFileData : Entity
    {
        private static string m_tableName;
        /// <summary>
        /// 实体类DBFileData
        /// </summary>
        public DBFileData() : base("FileData") { m_tableName = "FileData"; }
        /// <summary>
        /// 实体类DBFileData
        /// <param name="tableName">表名</param>
        /// </summary>
        public DBFileData(string tableName) : base(tableName) { m_tableName = tableName; }

        #region Model
        private string _ID;
        private DateTime _Created;
        private DateTime? _Modified;
        private string _CreatedBy;
        private string _ModifiedBy;
        private bool _IsDeleted;
        private string _MD5;
        private byte[] _Data;
        private int? _Size;
        /// <summary>
        /// ID 
        /// </summary>
        [DataMember]
        public string ID
        {
            get { return _ID; }
            set
            {
                this.OnPropertyValueChange(_.ID, _ID, value);
                this._ID = value;
            }
        }
        /// <summary>
        /// Created 
        /// </summary>
        [DataMember]
        public DateTime Created
        {
            get { return _Created; }
            set
            {
                this.OnPropertyValueChange(_.Created, _Created, value);
                this._Created = value;
            }
        }
        /// <summary>
        /// Modified 
        /// </summary>
        [DataMember]
        public DateTime? Modified
        {
            get { return _Modified; }
            set
            {
                this.OnPropertyValueChange(_.Modified, _Modified, value);
                this._Modified = value;
            }
        }
        /// <summary>
        /// CreatedBy 
        /// </summary>
        [DataMember]
        public string CreatedBy
        {
            get { return _CreatedBy; }
            set
            {
                this.OnPropertyValueChange(_.CreatedBy, _CreatedBy, value);
                this._CreatedBy = value;
            }
        }
        /// <summary>
        /// ModifiedBy 
        /// </summary>
        [DataMember]
        public string ModifiedBy
        {
            get { return _ModifiedBy; }
            set
            {
                this.OnPropertyValueChange(_.ModifiedBy, _ModifiedBy, value);
                this._ModifiedBy = value;
            }
        }
        /// <summary>
        /// IsDeleted 
        /// </summary>
        [DataMember]
        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set
            {
                this.OnPropertyValueChange(_.IsDeleted, _IsDeleted, value);
                this._IsDeleted = value;
            }
        }
        /// <summary>
        /// MD5 文件md5值
        /// </summary>
        [DataMember]
        public string MD5
        {
            get { return _MD5; }
            set
            {
                this.OnPropertyValueChange(_.MD5, _MD5, value);
                this._MD5 = value;
            }
        }
        /// <summary>
        /// Data blob数据
        /// </summary>
        [DataMember]
        public byte[] Data
        {
            get { return _Data; }
            set
            {
                this.OnPropertyValueChange(_.Data, _Data, value);
                this._Data = value;
            }
        }
        /// <summary>
        /// Size 文件大小
        /// </summary>
        [DataMember]
        public int? Size
        {
            get { return _Size; }
            set
            {
                this.OnPropertyValueChange(_.Size, _Size, value);
                this._Size = value;
            }
        }
        #endregion

        #region Method
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
                _.Created,
                _.Modified,
                _.CreatedBy,
                _.ModifiedBy,
                _.IsDeleted,
                _.MD5,
                _.Data,
                _.Size};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
                this._ID,
                this._Created,
                this._Modified,
                this._CreatedBy,
                this._ModifiedBy,
                this._IsDeleted,
                this._MD5,
                this._Data,
                this._Size};
        }
        #endregion

        #region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// FileData 
            /// </summary>
            public readonly static Field All = new Field("*", m_tableName);
            /// <summary>
            /// ID 
            /// </summary>
            public readonly static Field ID = new Field("ID", m_tableName, "ID");
            /// <summary>
            /// Created 
            /// </summary>
            public readonly static Field Created = new Field("Created", m_tableName, DbType.DateTime, 1, "Created");
            /// <summary>
            /// Modified 
            /// </summary>
            public readonly static Field Modified = new Field("Modified", m_tableName, DbType.DateTime, 1, "Modified");
            /// <summary>
            /// CreatedBy 
            /// </summary>
            public readonly static Field CreatedBy = new Field("CreatedBy", m_tableName, "CreatedBy");
            /// <summary>
            /// ModifiedBy 
            /// </summary>
            public readonly static Field ModifiedBy = new Field("ModifiedBy", m_tableName, "ModifiedBy");
            /// <summary>
            /// IsDeleted 
            /// </summary>
            public readonly static Field IsDeleted = new Field("IsDeleted", m_tableName, DbType.Boolean, 1, "IsDeleted");
            /// <summary>
            /// MD5 文件md5值
            /// </summary>
            public readonly static Field MD5 = new Field("MD5", m_tableName, "文件md5值");
            /// <summary>
            /// Data blob数据
            /// </summary>
            public readonly static Field Data = new Field("Data", m_tableName, DbType.Binary, -1, "blob数据");
            /// <summary>
            /// Size 文件大小
            /// </summary>
            public readonly static Field Size = new Field("Size", m_tableName, DbType.Int32, 1, "文件大小");
        }
        #endregion


    }
    /// <summary>
    /// 实体类DBFileData操作类
    /// </summary>
    public partial class DBFileDataRepository : BaseRepository<DBFileData>
    {
        /// <summary>
        /// DBFileData构造方法
        /// </summary>
        public DBFileDataRepository() : base()
        {
            _db = new DBContext();
        }
        /// <summary>
        /// DBFileData构造方法
        /// </summary>
        public DBFileDataRepository(DBContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }
        /// <summary>
        /// DBFileData构造方法
        /// <param name="connStrName">连接字符串中的名称</param>
        /// </summary>
        public DBFileDataRepository(string connStrName) : base(connStrName)
        {
            _db = new DBContext(connStrName);
        }
        /// <summary>
        /// DBFileData构造方法
        /// <param name="dbType">数据库类型</param>
        /// <param name="connStr">连接字符串</param>
        /// </summary>
        public DBFileDataRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
        {
            _db = new DBContext(dbType, connStr);
        }
    }

}

