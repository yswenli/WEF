/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Standard.Test.Models
*文件名： DBNotification
*版本号： V1.0.0.0
*唯一标识：a1ce7337-602b-4270-853c-ab049d43b680
*当前的用户域：WALLE
*创建人： WALLE
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/11/29 14:28:38
*描述：
*
*=================================================
*修改标记
*修改时间：2022/11/29 14:28:38
*修改人： yswen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using WEF.Common;

namespace WEF.Standard.Test.Models
{

    /// <summary>
    /// 实体类DBNotification
    /// </summary>
    [Serializable, DataContract, Table("Notification")]
    public partial class DBNotification : Entity
    {
        private static string m_tableName;
        /// <summary>
        /// 实体类DBNotification
        /// </summary>
        public DBNotification() : base("Notification") { m_tableName = "Notification"; }
        /// <summary>
        /// 实体类DBNotification
        /// <param name="tableName">表名</param>
        /// </summary>
        public DBNotification(string tableName) : base(tableName) { m_tableName = tableName; }

        #region Model
        private string _ID;
        private DateTime _Created;
        private DateTime? _Modified;
        private string _CreatedBy;
        private string _ModifiedBy;
        private bool _IsDeleted;
        private string _Sender;
        private string _SenderName;
        private bool? _SenderGender;
        private string _SettingID;
        private string _Content;
        private string _ReceiverID;
        private bool _UnRead;
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
        /// Sender 发送者id
        /// </summary>
        [DataMember, Description("发送者id")]
        public string Sender
        {
            get { return _Sender; }
            set
            {
                this.OnPropertyValueChange(_.Sender, _Sender, value);
                this._Sender = value;
            }
        }
        /// <summary>
        /// SenderName 发送者名称
        /// </summary>
        [DataMember, Description("发送者名称")]
        public string SenderName
        {
            get { return _SenderName; }
            set
            {
                this.OnPropertyValueChange(_.SenderName, _SenderName, value);
                this._SenderName = value;
            }
        }
        /// <summary>
        /// SenderGender 发送者性别
        /// </summary>
        [DataMember, Description("发送者性别")]
        public bool? SenderGender
        {
            get { return _SenderGender; }
            set
            {
                this.OnPropertyValueChange(_.SenderGender, _SenderGender, value);
                this._SenderGender = value;
            }
        }
        /// <summary>
        /// SettingID 配置id
        /// </summary>
        [DataMember, Description("配置id")]
        public string SettingID
        {
            get { return _SettingID; }
            set
            {
                this.OnPropertyValueChange(_.SettingID, _SettingID, value);
                this._SettingID = value;
            }
        }
        /// <summary>
        /// Content 内容
        /// </summary>
        [DataMember, Description("内容")]
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
        /// ReceiverID 收信息的用户
        /// </summary>
        [DataMember, Description("收信息的用户")]
        public string ReceiverID
        {
            get { return _ReceiverID; }
            set
            {
                this.OnPropertyValueChange(_.ReceiverID, _ReceiverID, value);
                this._ReceiverID = value;
            }
        }
        /// <summary>
        /// UnRead 未读
        /// </summary>
        [DataMember, Description("未读")]
        public bool UnRead
        {
            get { return _UnRead; }
            set
            {
                this.OnPropertyValueChange(_.UnRead, _UnRead, value);
                this._UnRead = value;
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
                _.Sender,
                _.SenderName,
                _.SenderGender,
                _.SettingID,
                _.Content,
                _.ReceiverID,
                _.UnRead};
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
                this._Sender,
                this._SenderName,
                this._SenderGender,
                this._SettingID,
                this._Content,
                this._ReceiverID,
                this._UnRead};
        }
        #endregion

        #region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// Notification 
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
            /// Sender 发送者id
            /// </summary>
            public readonly static Field Sender = new Field("Sender", m_tableName, "发送者id");
            /// <summary>
            /// SenderName 发送者名称
            /// </summary>
            public readonly static Field SenderName = new Field("SenderName", m_tableName, "发送者名称");
            /// <summary>
            /// SenderGender 发送者性别
            /// </summary>
            public readonly static Field SenderGender = new Field("SenderGender", m_tableName, DbType.Boolean, 1, "发送者性别");
            /// <summary>
            /// SettingID 配置id
            /// </summary>
            public readonly static Field SettingID = new Field("SettingID", m_tableName, "配置id");
            /// <summary>
            /// Content 内容
            /// </summary>
            public readonly static Field Content = new Field("Content", m_tableName, "内容");
            /// <summary>
            /// ReceiverID 收信息的用户
            /// </summary>
            public readonly static Field ReceiverID = new Field("ReceiverID", m_tableName, "收信息的用户");
            /// <summary>
            /// UnRead 未读
            /// </summary>
            public readonly static Field UnRead = new Field("UnRead", m_tableName, DbType.Boolean, 1, "未读");
        }
        #endregion


    }
    /// <summary>
    /// 实体类DBNotification操作类
    /// </summary>
    public partial class DBNotificationRepository : BaseRepository<DBNotification>
    {
        /// <summary>
        /// DBNotification构造方法
        /// </summary>
        public DBNotificationRepository() : base()
        {
            _dbContext = new DBContext();
        }
        /// <summary>
        /// DBNotification构造方法
        /// </summary>
        public DBNotificationRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// DBNotification构造方法
        /// <param name="connStrName">连接字符串中的名称</param>
        /// </summary>
        public DBNotificationRepository(string connStrName) : base(connStrName)
        {
            _dbContext = new DBContext(connStrName);
        }
        /// <summary>
        /// DBNotification构造方法
        /// <param name="dbType">数据库类型</param>
        /// <param name="connStr">连接字符串</param>
        /// </summary>
        public DBNotificationRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
        {
            _dbContext = new DBContext(dbType, connStr);
        }
    }

}
