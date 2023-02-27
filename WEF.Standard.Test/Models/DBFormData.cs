/****************************************************************************
*Copyright (c) 2023 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Standard.Test.Models
*文件名： DBFormData
*版本号： V1.0.0.0
*唯一标识：73988f6c-2446-4e9e-8ae7-f83d141aedee
*当前的用户域：WALLE
*创建人： WALLE
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2023/2/27 15:26:55
*描述：
*
*=================================================
*修改标记
*修改时间：2023/2/27 15:26:55
*修改人： yswen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;

using WEF.Db;

namespace WEF.Standard.Test.Models
{
    /// <summary>
    /// 实体类DBFormdata
    /// </summary>
    [Serializable, DataContract, Table("FormData")]
    public partial class DBFormdata : Entity
    {
        private static string m_tableName;
        /// <summary>
        /// 实体类DBFormdata
        /// </summary>
        public DBFormdata() : base("FormData") { m_tableName = "FormData"; }
        /// <summary>
        /// 实体类DBFormdata
        /// <param name="tableName">表名</param>
        /// </summary>
        public DBFormdata(string tableName) : base(tableName) { m_tableName = tableName; }

        #region Model
        private string _ID;
        private DateTime? _Created;
        private DateTime? _Modified;
        private string _CreatedBy;
        private string _ModifiedBy;
        private bool _IsDeleted;
        private string _BatchNo;
        private string _TemplateID;
        private string _FieldTypeID;
        private string _FieldName;
        private string _Value;
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
        public DateTime? Created
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
        /// BatchNo 批次号
        /// </summary>
        [DataMember, Description("批次号")]
        public string BatchNo
        {
            get { return _BatchNo; }
            set
            {
                this.OnPropertyValueChange(_.BatchNo, _BatchNo, value);
                this._BatchNo = value;
            }
        }
        /// <summary>
        /// TemplateID 页面模板id
        /// </summary>
        [DataMember, Description("页面模板id")]
        public string TemplateID
        {
            get { return _TemplateID; }
            set
            {
                this.OnPropertyValueChange(_.TemplateID, _TemplateID, value);
                this._TemplateID = value;
            }
        }
        /// <summary>
        /// FieldTypeID 字段类型id
        /// </summary>
        [DataMember, Description("字段类型id")]
        public string FieldTypeID
        {
            get { return _FieldTypeID; }
            set
            {
                this.OnPropertyValueChange(_.FieldTypeID, _FieldTypeID, value);
                this._FieldTypeID = value;
            }
        }
        /// <summary>
        /// FieldName 字段id
        /// </summary>
        [DataMember, Description("字段id")]
        public string FieldName
        {
            get { return _FieldName; }
            set
            {
                this.OnPropertyValueChange(_.FieldName, _FieldName, value);
                this._FieldName = value;
            }
        }
        /// <summary>
        /// Value 值
        /// </summary>
        [DataMember, Description("值")]
        public string Value
        {
            get { return _Value; }
            set
            {
                this.OnPropertyValueChange(_.Value, _Value, value);
                this._Value = value;
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
                _.BatchNo,
                _.TemplateID,
                _.FieldTypeID,
                _.FieldName,
                _.Value};
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
                this._BatchNo,
                this._TemplateID,
                this._FieldTypeID,
                this._FieldName,
                this._Value};
        }
        #endregion

        #region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// FormData 
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
            /// BatchNo 批次号
            /// </summary>
            public readonly static Field BatchNo = new Field("BatchNo", m_tableName, "批次号");
            /// <summary>
            /// TemplateID 页面模板id
            /// </summary>
            public readonly static Field TemplateID = new Field("TemplateID", m_tableName, "页面模板id");
            /// <summary>
            /// FieldTypeID 字段类型id
            /// </summary>
            public readonly static Field FieldTypeID = new Field("FieldTypeID", m_tableName, "字段类型id");
            /// <summary>
            /// FieldName 字段id
            /// </summary>
            public readonly static Field FieldName = new Field("FieldName", m_tableName, "字段id");
            /// <summary>
            /// Value 值
            /// </summary>
            public readonly static Field Value = new Field("Value", m_tableName, "值");
        }
        #endregion


    }
    /// <summary>
    /// 实体类DBFormdata操作类
    /// </summary>
    public partial class DBFormdataRepository : BaseRepository<DBFormdata>
    {
        /// <summary>
        /// DBFormdata构造方法
        /// </summary>
        public DBFormdataRepository() : base()
        {
            _dbContext = new DBContext();
        }
        /// <summary>
        /// DBFormdata构造方法
        /// </summary>
        public DBFormdataRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// DBFormdata构造方法
        /// <param name="connStrName">连接字符串中的名称</param>
        /// </summary>
        public DBFormdataRepository(string connStrName) : base(connStrName)
        {
            _dbContext = new DBContext(connStrName);
        }
        /// <summary>
        /// DBFormdata构造方法
        /// <param name="dbType">数据库类型</param>
        /// <param name="connStr">连接字符串</param>
        /// </summary>
        public DBFormdataRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
        {
            _dbContext = new DBContext(dbType, connStr);
        }
    }
}
