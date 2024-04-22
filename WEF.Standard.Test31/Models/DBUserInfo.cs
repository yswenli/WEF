//------------------------------------------------------------------------------
// <WEF-ModelGenerator>
//     此代码由WEF, Version=6.3.4.8, Culture=neutral, PublicKeyToken=null生成;时间 2023-10-24 17:38:16.861
//     运行时版本:6.0.23
//     不建议手动更改此代码，如有需要请自行扩展partial类
// </WEF-ModelGenerator>
//------------------------------------------------------------------------------


using System.Data;
using System.ComponentModel;
using System.Runtime.Serialization;
using WEF;
using WEF.Common;
using WEF.Db;
using System;

namespace RiverLand.Common.Models.DataBase.TeJingCai
{

    /// <summary>
    /// 实体类DBUserInfo
    /// </summary>
    [Serializable, DataContract, Table("UserInfo")]
    public partial class DBUserInfo : Entity
    {
        private static string m_tableName;
        /// <summary>
        /// 实体类DBUserInfo
        /// </summary>
        public DBUserInfo() : base("UserInfo") { m_tableName = "UserInfo"; }
        /// <summary>
        /// 实体类DBUserInfo
        /// <param name="tableName">表名</param>
        /// </summary>
        public DBUserInfo(string tableName) : base(tableName) { m_tableName = tableName; }

        #region Model
        private string _ID;
        private DateTime _Created;
        private DateTime? _Modified;
        private string _CreatedBy;
        private string _ModifiedBy;
        private bool _IsDeleted;
        private string _Name;
        private string _UserName;
        private string _Description;
        private string _Password;
        private string _Mobile;
        private int _FailedCount;
        private DateTime? _FrozenDate;
        private string _EventKey;
        /// <summary>
        /// ID 用户ID
        /// </summary>
        [DataMember, Description("用户ID")]
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
        /// Created 创建时间
        /// </summary>
        [DataMember, Description("创建时间")]
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
        /// Modified 修改时间
        /// </summary>
        [DataMember, Description("修改时间")]
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
        /// CreatedBy 创建者ID
        /// </summary>
        [DataMember, Description("创建者ID")]
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
        /// ModifiedBy 修改者ID
        /// </summary>
        [DataMember, Description("修改者ID")]
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
        /// IsDeleted 是否已删
        /// </summary>
        [DataMember, Description("是否已删")]
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
        /// Name 姓名
        /// </summary>
        [DataMember, Description("姓名")]
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
        /// UserName 用户名
        /// </summary>
        [DataMember, Description("用户名")]
        public string UserName
        {
            get { return _UserName; }
            set
            {
                this.OnPropertyValueChange(_.UserName, _UserName, value);
                this._UserName = value;
            }
        }
        /// <summary>
        /// Description 描述
        /// </summary>
        [DataMember, Description("描述")]
        public string Description
        {
            get { return _Description; }
            set
            {
                this.OnPropertyValueChange(_.Description, _Description, value);
                this._Description = value;
            }
        }
        /// <summary>
        /// Password 密码
        /// </summary>
        [DataMember, Description("密码")]
        public string Password
        {
            get { return _Password; }
            set
            {
                this.OnPropertyValueChange(_.Password, _Password, value);
                this._Password = value;
            }
        }
        /// <summary>
        /// Mobile 手机
        /// </summary>
        [DataMember, Description("手机")]
        public string Mobile
        {
            get { return _Mobile; }
            set
            {
                this.OnPropertyValueChange(_.Mobile, _Mobile, value);
                this._Mobile = value;
            }
        }
        /// <summary>
        /// FailedCount 失败次数
        /// </summary>
        [DataMember, Description("失败次数")]
        public int FailedCount
        {
            get { return _FailedCount; }
            set
            {
                this.OnPropertyValueChange(_.FailedCount, _FailedCount, value);
                this._FailedCount = value;
            }
        }
        /// <summary>
        /// FrozenDate 冻结日期
        /// </summary>
        [DataMember, Description("冻结日期")]
        public DateTime? FrozenDate
        {
            get { return _FrozenDate; }
            set
            {
                this.OnPropertyValueChange(_.FrozenDate, _FrozenDate, value);
                this._FrozenDate = value;
            }
        }
        /// <summary>
        /// EventKey 来源
        /// </summary>
        [DataMember, Description("来源")]
        public string EventKey
        {
            get { return _EventKey; }
            set
            {
                this.OnPropertyValueChange(_.EventKey, _EventKey, value);
                this._EventKey = value;
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
                _.Name,
                _.UserName,
                _.Description,
                _.Password,
                _.Mobile,
                _.FailedCount,
                _.FrozenDate,
                _.EventKey};
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
                this._Name,
                this._UserName,
                this._Description,
                this._Password,
                this._Mobile,
                this._FailedCount,
                this._FrozenDate,
                this._EventKey};
        }
        #endregion

        #region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// UserInfo 
            /// </summary>
            public readonly static Field All = new Field("*", m_tableName);
            /// <summary>
            /// ID 用户ID
            /// </summary>
            public readonly static Field ID = new Field("ID", m_tableName, "用户ID");
            /// <summary>
            /// Created 创建时间
            /// </summary>
            public readonly static Field Created = new Field("Created", m_tableName, DbType.DateTime, 1, "创建时间");
            /// <summary>
            /// Modified 修改时间
            /// </summary>
            public readonly static Field Modified = new Field("Modified", m_tableName, DbType.DateTime, 1, "修改时间");
            /// <summary>
            /// CreatedBy 创建者ID
            /// </summary>
            public readonly static Field CreatedBy = new Field("CreatedBy", m_tableName, "创建者ID");
            /// <summary>
            /// ModifiedBy 修改者ID
            /// </summary>
            public readonly static Field ModifiedBy = new Field("ModifiedBy", m_tableName, "修改者ID");
            /// <summary>
            /// IsDeleted 是否已删
            /// </summary>
            public readonly static Field IsDeleted = new Field("IsDeleted", m_tableName, DbType.Boolean, 1, "是否已删");
            /// <summary>
            /// Name 姓名
            /// </summary>
            public readonly static Field Name = new Field("Name", m_tableName, "姓名");
            /// <summary>
            /// UserName 用户名
            /// </summary>
            public readonly static Field UserName = new Field("UserName", m_tableName, "用户名");
            /// <summary>
            /// Description 描述
            /// </summary>
            public readonly static Field Description = new Field("Description", m_tableName, "描述");
            /// <summary>
            /// Password 密码
            /// </summary>
            public readonly static Field Password = new Field("Password", m_tableName, "密码");
            /// <summary>
            /// Mobile 手机
            /// </summary>
            public readonly static Field Mobile = new Field("Mobile", m_tableName, "手机");
            /// <summary>
            /// FailedCount 失败次数
            /// </summary>
            public readonly static Field FailedCount = new Field("FailedCount", m_tableName, DbType.Int32, 1, "失败次数");
            /// <summary>
            /// FrozenDate 冻结日期
            /// </summary>
            public readonly static Field FrozenDate = new Field("FrozenDate", m_tableName, DbType.DateTime, 1, "冻结日期");
            /// <summary>
            /// EventKey 来源
            /// </summary>
            public readonly static Field EventKey = new Field("EventKey", m_tableName, "来源");
        }
        #endregion


    }
    /// <summary>
    /// 实体类DBUserInfo操作类
    /// </summary>
    public partial class DBUserInfoRepository : BaseRepository<DBUserInfo>
    {
        /// <summary>
        /// DBUserInfo构造方法
        /// </summary>
        public DBUserInfoRepository() : base()
        {
            _dbContext = new DBContext();
        }
        /// <summary>
        /// DBUserInfo构造方法
        /// </summary>
        public DBUserInfoRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// DBUserInfo构造方法
        /// <param name="connStrName">连接字符串中的名称</param>
        /// </summary>
        public DBUserInfoRepository(string connStrName) : base(connStrName)
        {
            _dbContext = new DBContext(connStrName);
        }
        /// <summary>
        /// DBUserInfo构造方法
        /// <param name="dbType">数据库类型</param>
        /// <param name="connStr">连接字符串</param>
        /// </summary>
        public DBUserInfoRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
        {
            _dbContext = new DBContext(dbType, connStr);
        }
    }

}

