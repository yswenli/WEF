//------------------------------------------------------------------------------
// <WEF-ModelGenerator>
//     此代码由WEF数据库工具, Version=5.2.1.7, Culture=neutral, PublicKeyToken=null生成;时间 2021-11-22 11:16:28.279
//     运行时版本:4.0.30319.42000
//     不建议手动更改此代码，如有需要请自行扩展partial类
// </WEF-ModelGenerator>
//------------------------------------------------------------------------------


using System;
using System.Runtime.Serialization;

using WEF;
using WEF.Db;

namespace WEF.Models
{

    /// <summary>
    /// 实体类DBOcWarehouseArea
    /// </summary>
    [Serializable, DataContract, Table("oc_warehouse_area")]
	public partial class DBOcWarehouseArea : Entity
	{
		private static string m_tableName;
		/// <summary>
		/// 实体类DBOcWarehouseArea
		/// </summary>
		public DBOcWarehouseArea() : base("oc_warehouse_area") { m_tableName = "oc_warehouse_area"; }
		/// <summary>
		/// 实体类DBOcWarehouseArea
		/// <param name="tableName">表名</param>
		/// </summary>
		public DBOcWarehouseArea(string tableName) : base(tableName) { m_tableName = tableName; }

		#region Model
		private string _ID;
		private string _WarehouseCode;
		private int _SortID;
		private string _Name;
		private string _Description;
		private bool _Receivable;
		private bool _Detectable;
		private bool _IsTemporary;
		private bool _Repairable;
		private bool _Storable;
		private bool _Pickable;
		private bool _Sendable;
		private bool _IsSecondHand;
		private bool _IsAbandoned;
		private string _CreateBy;
		private DateTime _CreateTime;
		private string _ModifyBy;
		private DateTime? _ModifyTime;
		/// <summary>
		/// ID 区域Code
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
		/// WarehouseCode 仓库编码
		/// </summary>
		[DataMember]
		public string WarehouseCode
		{
			get { return _WarehouseCode; }
			set
			{
				this.OnPropertyValueChange(_.WarehouseCode, _WarehouseCode, value);
				this._WarehouseCode = value;
			}
		}
		/// <summary>
		/// SortID 序号
		/// </summary>
		[DataMember]
		public int SortID
		{
			get { return _SortID; }
			set
			{
				this.OnPropertyValueChange(_.SortID, _SortID, value);
				this._SortID = value;
			}
		}
		/// <summary>
		/// Name 区域名称
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
		/// Description 区域描述
		/// </summary>
		[DataMember]
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
		/// Receivable 是否是收货区域
		/// </summary>
		[DataMember]
		public bool Receivable
		{
			get { return _Receivable; }
			set
			{
				this.OnPropertyValueChange(_.Receivable, _Receivable, value);
				this._Receivable = value;
			}
		}
		/// <summary>
		/// Detectable 是否是检验区域
		/// </summary>
		[DataMember]
		public bool Detectable
		{
			get { return _Detectable; }
			set
			{
				this.OnPropertyValueChange(_.Detectable, _Detectable, value);
				this._Detectable = value;
			}
		}
		/// <summary>
		/// IsTemporary 是否是临时区域
		/// </summary>
		[DataMember]
		public bool IsTemporary
		{
			get { return _IsTemporary; }
			set
			{
				this.OnPropertyValueChange(_.IsTemporary, _IsTemporary, value);
				this._IsTemporary = value;
			}
		}
		/// <summary>
		/// Repairable 是否是维修区域
		/// </summary>
		[DataMember]
		public bool Repairable
		{
			get { return _Repairable; }
			set
			{
				this.OnPropertyValueChange(_.Repairable, _Repairable, value);
				this._Repairable = value;
			}
		}
		/// <summary>
		/// Storable 是否是存储区域
		/// </summary>
		[DataMember]
		public bool Storable
		{
			get { return _Storable; }
			set
			{
				this.OnPropertyValueChange(_.Storable, _Storable, value);
				this._Storable = value;
			}
		}
		/// <summary>
		/// Pickable 是否是拣货区域
		/// </summary>
		[DataMember]
		public bool Pickable
		{
			get { return _Pickable; }
			set
			{
				this.OnPropertyValueChange(_.Pickable, _Pickable, value);
				this._Pickable = value;
			}
		}
		/// <summary>
		/// Sendable 是否是发货区域
		/// </summary>
		[DataMember]
		public bool Sendable
		{
			get { return _Sendable; }
			set
			{
				this.OnPropertyValueChange(_.Sendable, _Sendable, value);
				this._Sendable = value;
			}
		}
		/// <summary>
		/// IsSecondHand 是否是二手区域
		/// </summary>
		[DataMember]
		public bool IsSecondHand
		{
			get { return _IsSecondHand; }
			set
			{
				this.OnPropertyValueChange(_.IsSecondHand, _IsSecondHand, value);
				this._IsSecondHand = value;
			}
		}
		/// <summary>
		/// IsAbandoned 是否是废弃区域
		/// </summary>
		[DataMember]
		public bool IsAbandoned
		{
			get { return _IsAbandoned; }
			set
			{
				this.OnPropertyValueChange(_.IsAbandoned, _IsAbandoned, value);
				this._IsAbandoned = value;
			}
		}
		/// <summary>
		/// CreateBy 创建人
		/// </summary>
		[DataMember]
		public string CreateBy
		{
			get { return _CreateBy; }
			set
			{
				this.OnPropertyValueChange(_.CreateBy, _CreateBy, value);
				this._CreateBy = value;
			}
		}
		/// <summary>
		/// CreateTime 创建日期
		/// </summary>
		[DataMember]
		public DateTime CreateTime
		{
			get { return _CreateTime; }
			set
			{
				this.OnPropertyValueChange(_.CreateTime, _CreateTime, value);
				this._CreateTime = value;
			}
		}
		/// <summary>
		/// ModifyBy 修改人
		/// </summary>
		[DataMember]
		public string ModifyBy
		{
			get { return _ModifyBy; }
			set
			{
				this.OnPropertyValueChange(_.ModifyBy, _ModifyBy, value);
				this._ModifyBy = value;
			}
		}
		/// <summary>
		/// ModifyTime 修改日期
		/// </summary>
		[DataMember]
		public DateTime? ModifyTime
		{
			get { return _ModifyTime; }
			set
			{
				this.OnPropertyValueChange(_.ModifyTime, _ModifyTime, value);
				this._ModifyTime = value;
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
				_.WarehouseCode,
				_.SortID,
				_.Name,
				_.Description,
				_.Receivable,
				_.Detectable,
				_.IsTemporary,
				_.Repairable,
				_.Storable,
				_.Pickable,
				_.Sendable,
				_.IsSecondHand,
				_.IsAbandoned,
				_.CreateBy,
				_.CreateTime,
				_.ModifyBy,
				_.ModifyTime};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._ID,
				this._WarehouseCode,
				this._SortID,
				this._Name,
				this._Description,
				this._Receivable,
				this._Detectable,
				this._IsTemporary,
				this._Repairable,
				this._Storable,
				this._Pickable,
				this._Sendable,
				this._IsSecondHand,
				this._IsAbandoned,
				this._CreateBy,
				this._CreateTime,
				this._ModifyBy,
				this._ModifyTime};
		}
		#endregion

		#region _Field
		/// <summary>
		/// 字段信息
		/// </summary>
		public class _
		{
			/// <summary>
			/// oc_warehouse_area 
			/// </summary>
			public readonly static Field All = new Field("*", m_tableName);
			/// <summary>
			/// ID 区域Code
			/// </summary>
			public readonly static Field ID = new Field("ID", m_tableName, "区域Code");
			/// <summary>
			/// WarehouseCode 仓库编码
			/// </summary>
			public readonly static Field WarehouseCode = new Field("WarehouseCode", m_tableName, "仓库编码");
			/// <summary>
			/// SortID 序号
			/// </summary>
			public readonly static Field SortID = new Field("SortID", m_tableName, "序号");
			/// <summary>
			/// Name 区域名称
			/// </summary>
			public readonly static Field Name = new Field("Name", m_tableName, "区域名称");
			/// <summary>
			/// Description 区域描述
			/// </summary>
			public readonly static Field Description = new Field("Description", m_tableName, "区域描述");
			/// <summary>
			/// Receivable 是否是收货区域
			/// </summary>
			public readonly static Field Receivable = new Field("Receivable", m_tableName, System.Data.DbType.Boolean, 1, "是否是收货区域");
			/// <summary>
			/// Detectable 是否是检验区域
			/// </summary>
			public readonly static Field Detectable = new Field("Detectable", m_tableName, System.Data.DbType.Boolean, 1, "是否是检验区域");
			/// <summary>
			/// IsTemporary 是否是临时区域
			/// </summary>
			public readonly static Field IsTemporary = new Field("IsTemporary", m_tableName, System.Data.DbType.Boolean, 1, "是否是临时区域");
			/// <summary>
			/// Repairable 是否是维修区域
			/// </summary>
			public readonly static Field Repairable = new Field("Repairable", m_tableName, System.Data.DbType.Boolean, 1, "是否是维修区域");
			/// <summary>
			/// Storable 是否是存储区域
			/// </summary>
			public readonly static Field Storable = new Field("Storable", m_tableName, System.Data.DbType.Boolean, 1, "是否是存储区域");
			/// <summary>
			/// Pickable 是否是拣货区域
			/// </summary>
			public readonly static Field Pickable = new Field("Pickable", m_tableName, System.Data.DbType.Boolean, 1, "是否是拣货区域");
			/// <summary>
			/// Sendable 是否是发货区域
			/// </summary>
			public readonly static Field Sendable = new Field("Sendable", m_tableName, System.Data.DbType.Boolean, 1, "是否是发货区域");
			/// <summary>
			/// IsSecondHand 是否是二手区域
			/// </summary>
			public readonly static Field IsSecondHand = new Field("IsSecondHand", m_tableName, System.Data.DbType.Boolean, 1, "是否是二手区域");
			/// <summary>
			/// IsAbandoned 是否是废弃区域
			/// </summary>
			public readonly static Field IsAbandoned = new Field("IsAbandoned", m_tableName, System.Data.DbType.Boolean, 1, "是否是废弃区域");
			/// <summary>
			/// CreateBy 创建人
			/// </summary>
			public readonly static Field CreateBy = new Field("CreateBy", m_tableName, "创建人");
			/// <summary>
			/// CreateTime 创建日期
			/// </summary>
			public readonly static Field CreateTime = new Field("CreateTime", m_tableName, "创建日期");
			/// <summary>
			/// ModifyBy 修改人
			/// </summary>
			public readonly static Field ModifyBy = new Field("ModifyBy", m_tableName, "修改人");
			/// <summary>
			/// ModifyTime 修改日期
			/// </summary>
			public readonly static Field ModifyTime = new Field("ModifyTime", m_tableName, "修改日期");
		}
		#endregion


	}
	/// <summary>
	/// 实体类DBOcWarehouseArea操作类
	/// </summary>
	public partial class DBOcWarehouseAreaRepository : BaseRepository<DBOcWarehouseArea>
	{
		/// <summary>
		/// DBOcWarehouseArea构造方法
		/// </summary>
		public DBOcWarehouseAreaRepository() : base()
		{
			_dbContext = new DBContext();
		}
		/// <summary>
		/// DBOcWarehouseArea构造方法
		/// </summary>
		public DBOcWarehouseAreaRepository(DBContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
		/// <summary>
		/// DBOcWarehouseArea构造方法
		/// <param name="connStrName">连接字符串中的名称</param>
		/// </summary>
		public DBOcWarehouseAreaRepository(string connStrName) : base(connStrName)
		{
			_dbContext = new DBContext(connStrName);
		}
		/// <summary>
		/// DBOcWarehouseArea构造方法
		/// <param name="dbType">数据库类型</param>
		/// <param name="connStr">连接字符串</param>
		/// </summary>
		public DBOcWarehouseAreaRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
		{
			_dbContext = new DBContext(dbType, connStr);
		}
	}

}

