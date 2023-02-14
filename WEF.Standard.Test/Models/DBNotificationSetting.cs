/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Standard.Test.Models
*文件名： DBNotificationSetting
*版本号： V1.0.0.0
*唯一标识：932e56fd-62aa-4231-a9f1-99b6e9a6e5b9
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/11/29 14:28:14
*描述：
*
*=================================================
*修改标记
*修改时间：2022/11/29 14:28:14
*修改人： yswenli
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
    /// 实体类DBNotificationSetting
    /// </summary>
    [Serializable, DataContract, Table("NotificationSetting")]
    public partial class DBNotificationSetting : Entity
    {
        private static string m_tableName;
        /// <summary>
        /// 实体类DBNotificationSetting
        /// </summary>
        public DBNotificationSetting() : base("NotificationSetting") { m_tableName = "NotificationSetting"; }
        /// <summary>
        /// 实体类DBNotificationSetting
        /// <param name="tableName">表名</param>
        /// </summary>
        public DBNotificationSetting(string tableName) : base(tableName) { m_tableName = tableName; }

        #region Model
        private string _ID;
        private DateTime _Created;
        private DateTime? _Modified;
        private string _CreatedBy;
        private string _ModifiedBy;
        private bool _IsDeleted;
        private string _Key;
        private int _BusinessType;
        private string _Type;
        private string _Name;
        private bool? _SendNotification;
        private bool? _SendWXTemplateMsg;
        private bool? _SendSMS;
        private bool? _SendHumanService;
        private string _Icon;
        private string _BtnText;
        private string _BtnUrl;
        private string _SMSTemplateID;
        private string _AppSettingName;
        private string _WXTemplateID;
        private string _Remark;
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
        /// Key 唯一值
        /// </summary>
        [DataMember, Description("唯一值")]
        public string Key
        {
            get { return _Key; }
            set
            {
                this.OnPropertyValueChange(_.Key, _Key, value);
                this._Key = value;
            }
        }
        /// <summary>
        /// BusinessType 业务大类 1一般消息，2 患者绑定，3咨询消息，4评论点赞，5钱包消息
        /// </summary>
        [DataMember, Description("业务大类 1一般消息，2 患者绑定，3咨询消息，4评论点赞，5钱包消息")]
        public int BusinessType
        {
            get { return _BusinessType; }
            set
            {
                this.OnPropertyValueChange(_.BusinessType, _BusinessType, value);
                this._BusinessType = value;
            }
        }
        /// <summary>
        /// Type 通知类型
        /// </summary>
        [DataMember, Description("通知类型")]
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
        /// Name 通知名称
        /// </summary>
        [DataMember, Description("通知名称")]
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
        /// SendNotification 发送通知
        /// </summary>
        [DataMember, Description("发送通知")]
        public bool? SendNotification
        {
            get { return _SendNotification; }
            set
            {
                this.OnPropertyValueChange(_.SendNotification, _SendNotification, value);
                this._SendNotification = value;
            }
        }
        /// <summary>
        /// SendWXTemplateMsg 发送微信模板消息
        /// </summary>
        [DataMember, Description("发送微信模板消息")]
        public bool? SendWXTemplateMsg
        {
            get { return _SendWXTemplateMsg; }
            set
            {
                this.OnPropertyValueChange(_.SendWXTemplateMsg, _SendWXTemplateMsg, value);
                this._SendWXTemplateMsg = value;
            }
        }
        /// <summary>
        /// SendSMS 发送短信
        /// </summary>
        [DataMember, Description("发送短信")]
        public bool? SendSMS
        {
            get { return _SendSMS; }
            set
            {
                this.OnPropertyValueChange(_.SendSMS, _SendSMS, value);
                this._SendSMS = value;
            }
        }
        /// <summary>
        /// SendHumanService 发送人工服务
        /// </summary>
        [DataMember, Description("发送人工服务")]
        public bool? SendHumanService
        {
            get { return _SendHumanService; }
            set
            {
                this.OnPropertyValueChange(_.SendHumanService, _SendHumanService, value);
                this._SendHumanService = value;
            }
        }
        /// <summary>
        /// Icon 通知图标
        /// </summary>
        [DataMember, Description("通知图标")]
        public string Icon
        {
            get { return _Icon; }
            set
            {
                this.OnPropertyValueChange(_.Icon, _Icon, value);
                this._Icon = value;
            }
        }
        /// <summary>
        /// BtnText 按钮标题
        /// </summary>
        [DataMember, Description("按钮标题")]
        public string BtnText
        {
            get { return _BtnText; }
            set
            {
                this.OnPropertyValueChange(_.BtnText, _BtnText, value);
                this._BtnText = value;
            }
        }
        /// <summary>
        /// BtnUrl 按钮地址
        /// </summary>
        [DataMember, Description("按钮地址")]
        public string BtnUrl
        {
            get { return _BtnUrl; }
            set
            {
                this.OnPropertyValueChange(_.BtnUrl, _BtnUrl, value);
                this._BtnUrl = value;
            }
        }
        /// <summary>
        /// SMSTemplateID sms模板id
        /// </summary>
        [DataMember, Description("sms模板id")]
        public string SMSTemplateID
        {
            get { return _SMSTemplateID; }
            set
            {
                this.OnPropertyValueChange(_.SMSTemplateID, _SMSTemplateID, value);
                this._SMSTemplateID = value;
            }
        }
        /// <summary>
        /// AppSettingName 微信配置名称
        /// </summary>
        [DataMember, Description("微信配置名称")]
        public string AppSettingName
        {
            get { return _AppSettingName; }
            set
            {
                this.OnPropertyValueChange(_.AppSettingName, _AppSettingName, value);
                this._AppSettingName = value;
            }
        }
        /// <summary>
        /// WXTemplateID 微信模板id
        /// </summary>
        [DataMember, Description("微信模板id")]
        public string WXTemplateID
        {
            get { return _WXTemplateID; }
            set
            {
                this.OnPropertyValueChange(_.WXTemplateID, _WXTemplateID, value);
                this._WXTemplateID = value;
            }
        }
        /// <summary>
        /// Remark 备注
        /// </summary>
        [DataMember, Description("备注")]
        public string Remark
        {
            get { return _Remark; }
            set
            {
                this.OnPropertyValueChange(_.Remark, _Remark, value);
                this._Remark = value;
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
                _.Key,
                _.BusinessType,
                _.Type,
                _.Name,
                _.SendNotification,
                _.SendWXTemplateMsg,
                _.SendSMS,
                _.SendHumanService,
                _.Icon,
                _.BtnText,
                _.BtnUrl,
                _.SMSTemplateID,
                _.AppSettingName,
                _.WXTemplateID,
                _.Remark};
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
                this._Key,
                this._BusinessType,
                this._Type,
                this._Name,
                this._SendNotification,
                this._SendWXTemplateMsg,
                this._SendSMS,
                this._SendHumanService,
                this._Icon,
                this._BtnText,
                this._BtnUrl,
                this._SMSTemplateID,
                this._AppSettingName,
                this._WXTemplateID,
                this._Remark};
        }
        #endregion

        #region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// NotificationSetting 
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
            /// Key 唯一值
            /// </summary>
            public readonly static Field Key = new Field("Key", m_tableName, "唯一值");
            /// <summary>
            /// BusinessType 业务大类 1一般消息，2 患者绑定，3咨询消息，4评论点赞，5钱包消息
            /// </summary>
            public readonly static Field BusinessType = new Field("BusinessType", m_tableName, DbType.Int32, 1, "业务大类 1一般消息，2 患者绑定，3咨询消息，4评论点赞，5钱包消息");
            /// <summary>
            /// Type 通知类型
            /// </summary>
            public readonly static Field Type = new Field("Type", m_tableName, "通知类型");
            /// <summary>
            /// Name 通知名称
            /// </summary>
            public readonly static Field Name = new Field("Name", m_tableName, "通知名称");
            /// <summary>
            /// SendNotification 发送通知
            /// </summary>
            public readonly static Field SendNotification = new Field("SendNotification", m_tableName, DbType.Boolean, 1, "发送通知");
            /// <summary>
            /// SendWXTemplateMsg 发送微信模板消息
            /// </summary>
            public readonly static Field SendWXTemplateMsg = new Field("SendWXTemplateMsg", m_tableName, DbType.Boolean, 1, "发送微信模板消息");
            /// <summary>
            /// SendSMS 发送短信
            /// </summary>
            public readonly static Field SendSMS = new Field("SendSMS", m_tableName, DbType.Boolean, 1, "发送短信");
            /// <summary>
            /// SendHumanService 发送人工服务
            /// </summary>
            public readonly static Field SendHumanService = new Field("SendHumanService", m_tableName, DbType.Boolean, 1, "发送人工服务");
            /// <summary>
            /// Icon 通知图标
            /// </summary>
            public readonly static Field Icon = new Field("Icon", m_tableName, "通知图标");
            /// <summary>
            /// BtnText 按钮标题
            /// </summary>
            public readonly static Field BtnText = new Field("BtnText", m_tableName, "按钮标题");
            /// <summary>
            /// BtnUrl 按钮地址
            /// </summary>
            public readonly static Field BtnUrl = new Field("BtnUrl", m_tableName, "按钮地址");
            /// <summary>
            /// SMSTemplateID sms模板id
            /// </summary>
            public readonly static Field SMSTemplateID = new Field("SMSTemplateID", m_tableName, "sms模板id");
            /// <summary>
            /// AppSettingName 微信配置名称
            /// </summary>
            public readonly static Field AppSettingName = new Field("AppSettingName", m_tableName, "微信配置名称");
            /// <summary>
            /// WXTemplateID 微信模板id
            /// </summary>
            public readonly static Field WXTemplateID = new Field("WXTemplateID", m_tableName, "微信模板id");
            /// <summary>
            /// Remark 备注
            /// </summary>
            public readonly static Field Remark = new Field("Remark", m_tableName, "备注");
        }
        #endregion


    }
    /// <summary>
    /// 实体类DBNotificationSetting操作类
    /// </summary>
    public partial class DBNotificationSettingRepository : BaseRepository<DBNotificationSetting>
    {
        /// <summary>
        /// DBNotificationSetting构造方法
        /// </summary>
        public DBNotificationSettingRepository() : base()
        {
            _dbContext = new DBContext();
        }
        /// <summary>
        /// DBNotificationSetting构造方法
        /// </summary>
        public DBNotificationSettingRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// DBNotificationSetting构造方法
        /// <param name="connStrName">连接字符串中的名称</param>
        /// </summary>
        public DBNotificationSettingRepository(string connStrName) : base(connStrName)
        {
            _dbContext = new DBContext(connStrName);
        }
        /// <summary>
        /// DBNotificationSetting构造方法
        /// <param name="dbType">数据库类型</param>
        /// <param name="connStr">连接字符串</param>
        /// </summary>
        public DBNotificationSettingRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
        {
            _dbContext = new DBContext(dbType, connStr);
        }
    }
}
