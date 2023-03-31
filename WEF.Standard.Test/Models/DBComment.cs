/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Test.Models
*文件名： DBComment
*版本号： V1.0.0.0
*唯一标识：51b87954-e864-42a5-a5e3-f224ac9228bd
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/9/27 18:27:24
*描述：
*
*=================================================
*修改标记
*修改时间：2022/9/27 18:27:24

*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;

using WEF.Db;

namespace WEF.Test.Models
{

    /// <summary>
    /// 实体类DBComment
    /// </summary>
    [Serializable, DataContract, Table("Comment")]
    public partial class DBComment : Entity
    {
        private static string m_tableName;
        /// <summary>
        /// 实体类DBComment
        /// </summary>
        public DBComment() : base("Comment") { m_tableName = "Comment"; }
        /// <summary>
        /// 实体类DBComment
        /// <param name="tableName">表名</param>
        /// </summary>
        public DBComment(string tableName) : base(tableName) { m_tableName = tableName; }

        #region Model
        private string _ID;
        private DateTime _Created;
        private DateTime? _Modified;
        private string _CreatedBy;
        private string _ModifiedBy;
        private bool _IsDeleted;
        private string _Type;
        private string _PageID;
        private string _CommentName;
        private string _Content;
        private int _PraiseCount;
        private string _CommentImg;
        private string _CommentVoice;
        private int? _CommentVoiceTime;
        private int? _AuditStatus;
        private DateTime? _AuditDate;
        private string _AuditRemark;
        private string _AuditUserID;
        private string _BeCommentedID;
        private string _BeCommentedName;
        private string _CommentID;
        private string _IP;
        private string _WXHeadImg;
        /// <summary>
        /// ID 
        /// </summary>
        [DataMember, Description("ID")]
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
        [DataMember, Description("Created")]
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
        [DataMember, Description("Modified")]
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
        /// CreatedBy 用户ID
        /// </summary>
        [DataMember, Description("用户ID")]
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
        [DataMember, Description("ModifiedBy")]
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
        [DataMember, Description("IsDeleted")]
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
        /// Type 页面类型(文章Article, 评论Comment，一问一答Consulting)
        /// </summary>
        [DataMember, Description("页面类型(文章Article, 评论Comment，一问一答Consulting)")]
        public string Type
        {
            get { return _Type; }
            set
            {
                this.OnPropertyValueChange(_.Type, _Type, value);
                this._Type = value;
            }
        }
        /// <summary>
        /// PageID 页面ID
        /// </summary>
        [DataMember, Description("页面ID")]
        public string PageID
        {
            get { return _PageID; }
            set
            {
                this.OnPropertyValueChange(_.PageID, _PageID, value);
                this._PageID = value;
            }
        }
        /// <summary>
        /// CommentName 评论人名称
        /// </summary>
        [DataMember, Description("评论人名称")]
        public string CommentName
        {
            get { return _CommentName; }
            set
            {
                this.OnPropertyValueChange(_.CommentName, _CommentName, value);
                this._CommentName = value;
            }
        }
        /// <summary>
        /// Content 回复内容
        /// </summary>
        [DataMember, Description("回复内容")]
        public string Content
        {
            get { return _Content; }
            set
            {
                this.OnPropertyValueChange(_.Content, _Content, value);
                this._Content = value;
            }
        }
        /// <summary>
        /// PraiseCount 点赞数
        /// </summary>
        [DataMember, Description("点赞数")]
        public int PraiseCount
        {
            get { return _PraiseCount; }
            set
            {
                this.OnPropertyValueChange(_.PraiseCount, _PraiseCount, value);
                this._PraiseCount = value;
            }
        }
        /// <summary>
        /// CommentImg 评论图片
        /// </summary>
        [DataMember, Description("评论图片")]
        public string CommentImg
        {
            get { return _CommentImg; }
            set
            {
                this.OnPropertyValueChange(_.CommentImg, _CommentImg, value);
                this._CommentImg = value;
            }
        }
        /// <summary>
        /// CommentVoice 音频
        /// </summary>
        [DataMember, Description("音频")]
        public string CommentVoice
        {
            get { return _CommentVoice; }
            set
            {
                this.OnPropertyValueChange(_.CommentVoice, _CommentVoice, value);
                this._CommentVoice = value;
            }
        }
        /// <summary>
        /// CommentVoiceTime 音频时长
        /// </summary>
        [DataMember, Description("音频时长")]
        public int? CommentVoiceTime
        {
            get { return _CommentVoiceTime; }
            set
            {
                this.OnPropertyValueChange(_.CommentVoiceTime, _CommentVoiceTime, value);
                this._CommentVoiceTime = value;
            }
        }
        /// <summary>
        /// AuditStatus 审核状态(1先发待审，2先审待发，3审核通过，4审核不通过)
        /// </summary>
        [DataMember, Description("审核状态(1先发待审，2先审待发，3审核通过，4审核不通过)")]
        public int? AuditStatus
        {
            get { return _AuditStatus; }
            set
            {
                this.OnPropertyValueChange(_.AuditStatus, _AuditStatus, value);
                this._AuditStatus = value;
            }
        }
        /// <summary>
        /// AuditDate 审核时间
        /// </summary>
        [DataMember, Description("审核时间")]
        public DateTime? AuditDate
        {
            get { return _AuditDate; }
            set
            {
                this.OnPropertyValueChange(_.AuditDate, _AuditDate, value);
                this._AuditDate = value;
            }
        }
        /// <summary>
        /// AuditRemark 审核备注
        /// </summary>
        [DataMember, Description("审核备注")]
        public string AuditRemark
        {
            get { return _AuditRemark; }
            set
            {
                this.OnPropertyValueChange(_.AuditRemark, _AuditRemark, value);
                this._AuditRemark = value;
            }
        }
        /// <summary>
        /// AuditUserID 审核人
        /// </summary>
        [DataMember, Description("审核人")]
        public string AuditUserID
        {
            get { return _AuditUserID; }
            set
            {
                this.OnPropertyValueChange(_.AuditUserID, _AuditUserID, value);
                this._AuditUserID = value;
            }
        }
        /// <summary>
        /// BeCommentedID 被评论人的ID
        /// </summary>
        [DataMember, Description("被评论人的ID")]
        public string BeCommentedID
        {
            get { return _BeCommentedID; }
            set
            {
                this.OnPropertyValueChange(_.BeCommentedID, _BeCommentedID, value);
                this._BeCommentedID = value;
            }
        }
        /// <summary>
        /// BeCommentedName 被评论人名称
        /// </summary>
        [DataMember, Description("被评论人名称")]
        public string BeCommentedName
        {
            get { return _BeCommentedName; }
            set
            {
                this.OnPropertyValueChange(_.BeCommentedName, _BeCommentedName, value);
                this._BeCommentedName = value;
            }
        }
        /// <summary>
        /// CommentID 被评论id
        /// </summary>
        [DataMember, Description("被评论id")]
        public string CommentID
        {
            get { return _CommentID; }
            set
            {
                this.OnPropertyValueChange(_.CommentID, _CommentID, value);
                this._CommentID = value;
            }
        }
        /// <summary>
        /// IP IP地址
        /// </summary>
        [DataMember, Description("IP地址")]
        public string IP
        {
            get { return _IP; }
            set
            {
                this.OnPropertyValueChange(_.IP, _IP, value);
                this._IP = value;
            }
        }
        /// <summary>
        /// WXHeadImg 微信头像
        /// </summary>
        [DataMember, Description("微信头像")]
        public string WXHeadImg
        {
            get { return _WXHeadImg; }
            set
            {
                this.OnPropertyValueChange(_.WXHeadImg, _WXHeadImg, value);
                this._WXHeadImg = value;
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
                _.Type,
                _.PageID,
                _.CommentName,
                _.Content,
                _.PraiseCount,
                _.CommentImg,
                _.CommentVoice,
                _.CommentVoiceTime,
                _.AuditStatus,
                _.AuditDate,
                _.AuditRemark,
                _.AuditUserID,
                _.BeCommentedID,
                _.BeCommentedName,
                _.CommentID,
                _.IP,
                _.WXHeadImg};
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
                this._Type,
                this._PageID,
                this._CommentName,
                this._Content,
                this._PraiseCount,
                this._CommentImg,
                this._CommentVoice,
                this._CommentVoiceTime,
                this._AuditStatus,
                this._AuditDate,
                this._AuditRemark,
                this._AuditUserID,
                this._BeCommentedID,
                this._BeCommentedName,
                this._CommentID,
                this._IP,
                this._WXHeadImg};
        }
        #endregion

        #region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// Comment 
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
            /// CreatedBy 用户ID
            /// </summary>
            public readonly static Field CreatedBy = new Field("CreatedBy", m_tableName, "用户ID");
            /// <summary>
            /// ModifiedBy 
            /// </summary>
            public readonly static Field ModifiedBy = new Field("ModifiedBy", m_tableName, "ModifiedBy");
            /// <summary>
            /// IsDeleted 
            /// </summary>
            public readonly static Field IsDeleted = new Field("IsDeleted", m_tableName, DbType.Boolean, 1, "IsDeleted");
            /// <summary>
            /// Type 页面类型(文章Article, 评论Comment，一问一答Consulting)
            /// </summary>
            public readonly static Field Type = new Field("Type", m_tableName, "页面类型(文章Article, 评论Comment，一问一答Consulting)");
            /// <summary>
            /// PageID 页面ID
            /// </summary>
            public readonly static Field PageID = new Field("PageID", m_tableName, "页面ID");
            /// <summary>
            /// CommentName 评论人名称
            /// </summary>
            public readonly static Field CommentName = new Field("CommentName", m_tableName, "评论人名称");
            /// <summary>
            /// Content 回复内容
            /// </summary>
            public readonly static Field Content = new Field("Content", m_tableName, "回复内容");
            /// <summary>
            /// PraiseCount 点赞数
            /// </summary>
            public readonly static Field PraiseCount = new Field("PraiseCount", m_tableName, DbType.Int32, 1, "点赞数");
            /// <summary>
            /// CommentImg 评论图片
            /// </summary>
            public readonly static Field CommentImg = new Field("CommentImg", m_tableName, "评论图片");
            /// <summary>
            /// CommentVoice 音频
            /// </summary>
            public readonly static Field CommentVoice = new Field("CommentVoice", m_tableName, "音频");
            /// <summary>
            /// CommentVoiceTime 音频时长
            /// </summary>
            public readonly static Field CommentVoiceTime = new Field("CommentVoiceTime", m_tableName, DbType.Int32, 1, "音频时长");
            /// <summary>
            /// AuditStatus 审核状态(1先发待审，2先审待发，3审核通过，4审核不通过)
            /// </summary>
            public readonly static Field AuditStatus = new Field("AuditStatus", m_tableName, DbType.Int32, 1, "审核状态(1先发待审，2先审待发，3审核通过，4审核不通过)");
            /// <summary>
            /// AuditDate 审核时间
            /// </summary>
            public readonly static Field AuditDate = new Field("AuditDate", m_tableName, DbType.DateTime, 1, "审核时间");
            /// <summary>
            /// AuditRemark 审核备注
            /// </summary>
            public readonly static Field AuditRemark = new Field("AuditRemark", m_tableName, "审核备注");
            /// <summary>
            /// AuditUserID 审核人
            /// </summary>
            public readonly static Field AuditUserID = new Field("AuditUserID", m_tableName, "审核人");
            /// <summary>
            /// BeCommentedID 被评论人的ID
            /// </summary>
            public readonly static Field BeCommentedID = new Field("BeCommentedID", m_tableName, "被评论人的ID");
            /// <summary>
            /// BeCommentedName 被评论人名称
            /// </summary>
            public readonly static Field BeCommentedName = new Field("BeCommentedName", m_tableName, "被评论人名称");
            /// <summary>
            /// CommentID 被评论id
            /// </summary>
            public readonly static Field CommentID = new Field("CommentID", m_tableName, "被评论id");
            /// <summary>
            /// IP IP地址
            /// </summary>
            public readonly static Field IP = new Field("IP", m_tableName, "IP地址");
            /// <summary>
            /// WXHeadImg 微信头像
            /// </summary>
            public readonly static Field WXHeadImg = new Field("WXHeadImg", m_tableName, "微信头像");
        }
        #endregion


    }
    /// <summary>
    /// 实体类DBComment操作类
    /// </summary>
    public partial class DBCommentRepository : BaseRepository<DBComment>
    {
        /// <summary>
        /// DBComment构造方法
        /// </summary>
        public DBCommentRepository() : base()
        {
            _dbContext = new DBContext();
        }
        /// <summary>
        /// DBComment构造方法
        /// </summary>
        public DBCommentRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// DBComment构造方法
        /// <param name="connStrName">连接字符串中的名称</param>
        /// </summary>
        public DBCommentRepository(string connStrName) : base(connStrName)
        {
            _dbContext = new DBContext(connStrName);
        }
        /// <summary>
        /// DBComment构造方法
        /// <param name="dbType">数据库类型</param>
        /// <param name="connStr">连接字符串</param>
        /// </summary>
        public DBCommentRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
        {
            _dbContext = new DBContext(dbType, connStr);
        }
    }



}
