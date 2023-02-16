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
using System;
using System.Data;
using System.ComponentModel;
using System.Runtime.Serialization;
using WEF;
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
        private string _PageType;
        private string _PageID;
        private string _RootID;
        private string _Content;
        private int _PraiseCount;
        private string _CommentImg;
        private string _CommentVoice;
        private int? _CommentVoiceTime;
        private string _UserName;
        private int? _Status;
        private DateTime? _StatusDate;
        private string _StatusRemark;
        private string _StatusUserID;
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
        /// PageType 页面类型
        /// </summary>
        [DataMember, Description("页面类型")]
        public string PageType
        {
            get { return _PageType; }
            set
            {
                this.OnPropertyValueChange(_.PageType, _PageType, value);
                this._PageType = value;
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
        /// RootID 回复别人的回复，就不是null
        /// </summary>
        [DataMember, Description("回复别人的回复，就不是null")]
        public string RootID
        {
            get { return _RootID; }
            set
            {
                this.OnPropertyValueChange(_.RootID, _RootID, value);
                this._RootID = value;
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
        /// UserName 用户名字
        /// </summary>
        [DataMember, Description("用户名字")]
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
        /// Status 状态1未审核 2审核通过 3审核不通过
        /// </summary>
        [DataMember, Description("状态1未审核 2审核通过 3审核不通过")]
        public int? Status
        {
            get { return _Status; }
            set
            {
                this.OnPropertyValueChange(_.Status, _Status, value);
                this._Status = value;
            }
        }
        /// <summary>
        /// StatusDate 状态
        /// </summary>
        [DataMember, Description("状态")]
        public DateTime? StatusDate
        {
            get { return _StatusDate; }
            set
            {
                this.OnPropertyValueChange(_.StatusDate, _StatusDate, value);
                this._StatusDate = value;
            }
        }
        /// <summary>
        /// StatusRemark 审核备注
        /// </summary>
        [DataMember, Description("审核备注")]
        public string StatusRemark
        {
            get { return _StatusRemark; }
            set
            {
                this.OnPropertyValueChange(_.StatusRemark, _StatusRemark, value);
                this._StatusRemark = value;
            }
        }
        /// <summary>
        /// StatusUserID 审核人
        /// </summary>
        [DataMember, Description("审核人")]
        public string StatusUserID
        {
            get { return _StatusUserID; }
            set
            {
                this.OnPropertyValueChange(_.StatusUserID, _StatusUserID, value);
                this._StatusUserID = value;
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
                _.PageType,
                _.PageID,
                _.RootID,
                _.Content,
                _.PraiseCount,
                _.CommentImg,
                _.CommentVoice,
                _.CommentVoiceTime,
                _.UserName,
                _.Status,
                _.StatusDate,
                _.StatusRemark,
                _.StatusUserID};
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
                this._PageType,
                this._PageID,
                this._RootID,
                this._Content,
                this._PraiseCount,
                this._CommentImg,
                this._CommentVoice,
                this._CommentVoiceTime,
                this._UserName,
                this._Status,
                this._StatusDate,
                this._StatusRemark,
                this._StatusUserID};
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
            /// PageType 页面类型
            /// </summary>
            public readonly static Field PageType = new Field("PageType", m_tableName, "页面类型");
            /// <summary>
            /// PageID 页面ID
            /// </summary>
            public readonly static Field PageID = new Field("PageID", m_tableName, "页面ID");
            /// <summary>
            /// RootID 回复别人的回复，就不是null
            /// </summary>
            public readonly static Field RootID = new Field("RootID", m_tableName, "回复别人的回复，就不是null");
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
            /// UserName 用户名字
            /// </summary>
            public readonly static Field UserName = new Field("UserName", m_tableName, "用户名字");
            /// <summary>
            /// Status 状态1未审核 2审核通过 3审核不通过
            /// </summary>
            public readonly static Field Status = new Field("Status", m_tableName, DbType.Int32, 1, "状态1未审核 2审核通过 3审核不通过");
            /// <summary>
            /// StatusDate 状态
            /// </summary>
            public readonly static Field StatusDate = new Field("StatusDate", m_tableName, DbType.DateTime, 1, "状态");
            /// <summary>
            /// StatusRemark 审核备注
            /// </summary>
            public readonly static Field StatusRemark = new Field("StatusRemark", m_tableName, "审核备注");
            /// <summary>
            /// StatusUserID 审核人
            /// </summary>
            public readonly static Field StatusUserID = new Field("StatusUserID", m_tableName, "审核人");
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
