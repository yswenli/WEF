/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Test.Models
*文件名： DBArticle
*版本号： V1.0.0.0
*唯一标识：c31723bd-d2b1-4585-84e0-9b161217999e
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/9/27 18:25:45
*描述：
*
*=================================================
*修改标记
*修改时间：2022/9/27 18:25:45

*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System.Data;
using System.ComponentModel;
using System.Runtime.Serialization;
using WEF;
using WEF.Common;
using System;

namespace WEF.Test.Models
{
    /// <summary>
    /// 实体类DBArticle
    /// </summary>
    [Serializable, DataContract, Table("Article")]
    public partial class DBArticle : Entity
    {
        private static string m_tableName;
        /// <summary>
        /// 实体类DBArticle
        /// </summary>
        public DBArticle() : base("Article") { m_tableName = "Article"; }
        /// <summary>
        /// 实体类DBArticle
        /// <param name="tableName">表名</param>
        /// </summary>
        public DBArticle(string tableName) : base(tableName) { m_tableName = tableName; }

        #region Model
        private string _ID;
        private DateTime _Created;
        private DateTime? _Modified;
        private string _CreatedBy;
        private string _ModifiedBy;
        private bool _IsDeleted;
        private DateTime? _PublishDate;
        private bool _IsOnline;
        private string _PageType;
        private string _TagName;
        private string _Title;
        private string _TitleImg;
        private string _DoctorID;
        private string _DoctorWXHeadImg;
        private string _DoctorName;
        private string _DoctorMobile;
        private string _DoctorPosition;
        private string _DoctorHospital;
        private string _Content;
        private int _ReadCount;
        private int _PraiseCount;
        private int _CollectionCount;
        private int _CommentCount;
        private DateTime _LastUpdateDate;
        private int _Status;
        private DateTime? _StatusDate;
        private string _StatusRemark;
        private bool? _IsOriginal;
        private int? _Price;
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
        /// CreatedBy 
        /// </summary>
        [DataMember, Description("CreatedBy")]
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
        /// PublishDate 发布日期
        /// </summary>
        [DataMember, Description("发布日期")]
        public DateTime? PublishDate
        {
            get { return _PublishDate; }
            set
            {
                this.OnPropertyValueChange(_.PublishDate, _PublishDate, value);
                this._PublishDate = value;
            }
        }
        /// <summary>
        /// IsOnline 是否上广场
        /// </summary>
        [DataMember, Description("是否上广场")]
        public bool IsOnline
        {
            get { return _IsOnline; }
            set
            {
                this.OnPropertyValueChange(_.IsOnline, _IsOnline, value);
                this._IsOnline = value;
            }
        }
        /// <summary>
        /// PageType 文章类型 text，video
        /// </summary>
        [DataMember, Description("文章类型 text，video")]
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
        /// TagName 标签
        /// </summary>
        [DataMember, Description("标签")]
        public string TagName
        {
            get { return _TagName; }
            set
            {
                this.OnPropertyValueChange(_.TagName, _TagName, value);
                this._TagName = value;
            }
        }
        /// <summary>
        /// Title 标题
        /// </summary>
        [DataMember, Description("标题")]
        public string Title
        {
            get { return _Title; }
            set
            {
                this.OnPropertyValueChange(_.Title, _Title, value);
                this._Title = value;
            }
        }
        /// <summary>
        /// TitleImg 封面
        /// </summary>
        [DataMember, Description("封面")]
        public string TitleImg
        {
            get { return _TitleImg; }
            set
            {
                this.OnPropertyValueChange(_.TitleImg, _TitleImg, value);
                this._TitleImg = value;
            }
        }
        /// <summary>
        /// DoctorID 医生ID
        /// </summary>
        [DataMember, Description("医生ID")]
        public string DoctorID
        {
            get { return _DoctorID; }
            set
            {
                this.OnPropertyValueChange(_.DoctorID, _DoctorID, value);
                this._DoctorID = value;
            }
        }
        /// <summary>
        /// DoctorWXHeadImg 医生微信头像
        /// </summary>
        [DataMember, Description("医生微信头像")]
        public string DoctorWXHeadImg
        {
            get { return _DoctorWXHeadImg; }
            set
            {
                this.OnPropertyValueChange(_.DoctorWXHeadImg, _DoctorWXHeadImg, value);
                this._DoctorWXHeadImg = value;
            }
        }
        /// <summary>
        /// DoctorName 医生名字
        /// </summary>
        [DataMember, Description("医生名字")]
        public string DoctorName
        {
            get { return _DoctorName; }
            set
            {
                this.OnPropertyValueChange(_.DoctorName, _DoctorName, value);
                this._DoctorName = value;
            }
        }
        /// <summary>
        /// DoctorMobile 医生手机号
        /// </summary>
        [DataMember, Description("医生手机号")]
        public string DoctorMobile
        {
            get { return _DoctorMobile; }
            set
            {
                this.OnPropertyValueChange(_.DoctorMobile, _DoctorMobile, value);
                this._DoctorMobile = value;
            }
        }
        /// <summary>
        /// DoctorPosition 职位
        /// </summary>
        [DataMember, Description("职位")]
        public string DoctorPosition
        {
            get { return _DoctorPosition; }
            set
            {
                this.OnPropertyValueChange(_.DoctorPosition, _DoctorPosition, value);
                this._DoctorPosition = value;
            }
        }
        /// <summary>
        /// DoctorHospital 医生医院
        /// </summary>
        [DataMember, Description("医生医院")]
        public string DoctorHospital
        {
            get { return _DoctorHospital; }
            set
            {
                this.OnPropertyValueChange(_.DoctorHospital, _DoctorHospital, value);
                this._DoctorHospital = value;
            }
        }
        /// <summary>
        /// Content 富文本
        /// </summary>
        [DataMember, Description("富文本")]
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
        /// ReadCount 阅读数
        /// </summary>
        [DataMember, Description("阅读数")]
        public int ReadCount
        {
            get { return _ReadCount; }
            set
            {
                this.OnPropertyValueChange(_.ReadCount, _ReadCount, value);
                this._ReadCount = value;
            }
        }
        /// <summary>
        /// PraiseCount 点赞数量
        /// </summary>
        [DataMember, Description("点赞数量")]
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
        /// CollectionCount 收藏数量
        /// </summary>
        [DataMember, Description("收藏数量")]
        public int CollectionCount
        {
            get { return _CollectionCount; }
            set
            {
                this.OnPropertyValueChange(_.CollectionCount, _CollectionCount, value);
                this._CollectionCount = value;
            }
        }
        /// <summary>
        /// CommentCount 评论数量
        /// </summary>
        [DataMember, Description("评论数量")]
        public int CommentCount
        {
            get { return _CommentCount; }
            set
            {
                this.OnPropertyValueChange(_.CommentCount, _CommentCount, value);
                this._CommentCount = value;
            }
        }
        /// <summary>
        /// LastUpdateDate 最后更新日期
        /// </summary>
        [DataMember, Description("最后更新日期")]
        public DateTime LastUpdateDate
        {
            get { return _LastUpdateDate; }
            set
            {
                this.OnPropertyValueChange(_.LastUpdateDate, _LastUpdateDate, value);
                this._LastUpdateDate = value;
            }
        }
        /// <summary>
        /// Status 状态 草稿1，发布2
        /// </summary>
        [DataMember, Description("状态 草稿1，发布2")]
        public int Status
        {
            get { return _Status; }
            set
            {
                this.OnPropertyValueChange(_.Status, _Status, value);
                this._Status = value;
            }
        }
        /// <summary>
        /// StatusDate 状态变更日期
        /// </summary>
        [DataMember, Description("状态变更日期")]
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
        /// StatusRemark 状态备注
        /// </summary>
        [DataMember, Description("状态备注")]
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
        /// IsOriginal 是否原创
        /// </summary>
        [DataMember, Description("是否原创")]
        public bool? IsOriginal
        {
            get { return _IsOriginal; }
            set
            {
                this.OnPropertyValueChange(_.IsOriginal, _IsOriginal, value);
                this._IsOriginal = value;
            }
        }
        /// <summary>
        /// Price 定价 元
        /// </summary>
        [DataMember, Description("定价 元")]
        public int? Price
        {
            get { return _Price; }
            set
            {
                this.OnPropertyValueChange(_.Price, _Price, value);
                this._Price = value;
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
                _.PublishDate,
                _.IsOnline,
                _.PageType,
                _.TagName,
                _.Title,
                _.TitleImg,
                _.DoctorID,
                _.DoctorWXHeadImg,
                _.DoctorName,
                _.DoctorMobile,
                _.DoctorPosition,
                _.DoctorHospital,
                _.Content,
                _.ReadCount,
                _.PraiseCount,
                _.CollectionCount,
                _.CommentCount,
                _.LastUpdateDate,
                _.Status,
                _.StatusDate,
                _.StatusRemark,
                _.IsOriginal,
                _.Price};
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
                this._PublishDate,
                this._IsOnline,
                this._PageType,
                this._TagName,
                this._Title,
                this._TitleImg,
                this._DoctorID,
                this._DoctorWXHeadImg,
                this._DoctorName,
                this._DoctorMobile,
                this._DoctorPosition,
                this._DoctorHospital,
                this._Content,
                this._ReadCount,
                this._PraiseCount,
                this._CollectionCount,
                this._CommentCount,
                this._LastUpdateDate,
                this._Status,
                this._StatusDate,
                this._StatusRemark,
                this._IsOriginal,
                this._Price};
        }
        #endregion

        #region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// Article 
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
            /// PublishDate 发布日期
            /// </summary>
            public readonly static Field PublishDate = new Field("PublishDate", m_tableName, DbType.DateTime, 1, "发布日期");
            /// <summary>
            /// IsOnline 是否上广场
            /// </summary>
            public readonly static Field IsOnline = new Field("IsOnline", m_tableName, DbType.Boolean, 1, "是否上广场");
            /// <summary>
            /// PageType 文章类型 text，video
            /// </summary>
            public readonly static Field PageType = new Field("PageType", m_tableName, "文章类型 text，video");
            /// <summary>
            /// TagName 标签
            /// </summary>
            public readonly static Field TagName = new Field("TagName", m_tableName, "标签");
            /// <summary>
            /// Title 标题
            /// </summary>
            public readonly static Field Title = new Field("Title", m_tableName, "标题");
            /// <summary>
            /// TitleImg 封面
            /// </summary>
            public readonly static Field TitleImg = new Field("TitleImg", m_tableName, "封面");
            /// <summary>
            /// DoctorID 医生ID
            /// </summary>
            public readonly static Field DoctorID = new Field("DoctorID", m_tableName, "医生ID");
            /// <summary>
            /// DoctorWXHeadImg 医生微信头像
            /// </summary>
            public readonly static Field DoctorWXHeadImg = new Field("DoctorWXHeadImg", m_tableName, "医生微信头像");
            /// <summary>
            /// DoctorName 医生名字
            /// </summary>
            public readonly static Field DoctorName = new Field("DoctorName", m_tableName, "医生名字");
            /// <summary>
            /// DoctorMobile 医生手机号
            /// </summary>
            public readonly static Field DoctorMobile = new Field("DoctorMobile", m_tableName, "医生手机号");
            /// <summary>
            /// DoctorPosition 职位
            /// </summary>
            public readonly static Field DoctorPosition = new Field("DoctorPosition", m_tableName, "职位");
            /// <summary>
            /// DoctorHospital 医生医院
            /// </summary>
            public readonly static Field DoctorHospital = new Field("DoctorHospital", m_tableName, "医生医院");
            /// <summary>
            /// Content 富文本
            /// </summary>
            public readonly static Field Content = new Field("Content", m_tableName, "富文本");
            /// <summary>
            /// ReadCount 阅读数
            /// </summary>
            public readonly static Field ReadCount = new Field("ReadCount", m_tableName, DbType.Int32, 1, "阅读数");
            /// <summary>
            /// PraiseCount 点赞数量
            /// </summary>
            public readonly static Field PraiseCount = new Field("PraiseCount", m_tableName, DbType.Int32, 1, "点赞数量");
            /// <summary>
            /// CollectionCount 收藏数量
            /// </summary>
            public readonly static Field CollectionCount = new Field("CollectionCount", m_tableName, DbType.Int32, 1, "收藏数量");
            /// <summary>
            /// CommentCount 评论数量
            /// </summary>
            public readonly static Field CommentCount = new Field("CommentCount", m_tableName, DbType.Int32, 1, "评论数量");
            /// <summary>
            /// LastUpdateDate 最后更新日期
            /// </summary>
            public readonly static Field LastUpdateDate = new Field("LastUpdateDate", m_tableName, DbType.DateTime, 1, "最后更新日期");
            /// <summary>
            /// Status 状态 草稿1，发布2
            /// </summary>
            public readonly static Field Status = new Field("Status", m_tableName, DbType.Int32, 1, "状态 草稿1，发布2");
            /// <summary>
            /// StatusDate 状态变更日期
            /// </summary>
            public readonly static Field StatusDate = new Field("StatusDate", m_tableName, DbType.DateTime, 1, "状态变更日期");
            /// <summary>
            /// StatusRemark 状态备注
            /// </summary>
            public readonly static Field StatusRemark = new Field("StatusRemark", m_tableName, "状态备注");
            /// <summary>
            /// IsOriginal 是否原创
            /// </summary>
            public readonly static Field IsOriginal = new Field("IsOriginal", m_tableName, DbType.Boolean, 1, "是否原创");
            /// <summary>
            /// Price 定价 元
            /// </summary>
            public readonly static Field Price = new Field("Price", m_tableName, DbType.Int32, 1, "定价 元");
        }
        #endregion


    }
    /// <summary>
    /// 实体类DBArticle操作类
    /// </summary>
    public partial class DBArticleRepository : BaseRepository<DBArticle>
    {
        /// <summary>
        /// DBArticle构造方法
        /// </summary>
        public DBArticleRepository() : base()
        {
            _dbContext = new DBContext();
        }
        /// <summary>
        /// DBArticle构造方法
        /// </summary>
        public DBArticleRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// DBArticle构造方法
        /// <param name="connStrName">连接字符串中的名称</param>
        /// </summary>
        public DBArticleRepository(string connStrName) : base(connStrName)
        {
            _dbContext = new DBContext(connStrName);
        }
        /// <summary>
        /// DBArticle构造方法
        /// <param name="dbType">数据库类型</param>
        /// <param name="connStr">连接字符串</param>
        /// </summary>
        public DBArticleRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
        {
            _dbContext = new DBContext(dbType, connStr);
        }
    }


}
