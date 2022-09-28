//------------------------------------------------------------------------------
// <WEF-ModelGenerator>
//     此代码由WEF数据库工具, Version=5.2.1.7, Culture=neutral, PublicKeyToken=null生成;时间 2021-12-06 19:34:32.927
//     运行时版本:4.0.30319.42000
//     不建议手动更改此代码，如有需要请自行扩展partial类
// </WEF-ModelGenerator>
//------------------------------------------------------------------------------


using System;
using System.Data;
using System.Runtime.Serialization;

using WEF;
using WEF.Common;

namespace WEF.Test.Models
{

	/// <summary>
	/// 实体类DBWarehouseInboundData
	/// </summary>
	[Serializable, DataContract, Table("oc_warehouse_inbound_data")]
	public partial class DBWarehouseInboundData : Entity
	{
		private static string m_tableName;
		/// <summary>
		/// 实体类DBWarehouseInboundData
		/// </summary>
		public DBWarehouseInboundData() : base("oc_warehouse_inbound_data") { m_tableName = "oc_warehouse_inbound_data"; }
		/// <summary>
		/// 实体类DBWarehouseInboundData
		/// <param name="tableName">表名</param>
		/// </summary>
		public DBWarehouseInboundData(string tableName) : base(tableName) { m_tableName = tableName; }

		#region Model
		private string _ID;
		private string _IID;
		private string _SKU;
		private string _Company;
		private string _BatchNumber;
		private decimal _Amount;
		private int _ApplyQty;
		private int _ApplyBoxQty;
		private int _PerBoxQty;
		private float? _Length;
		private float? _Width;
		private float? _Height;
		private float? _Weight;
		private bool? _Regular;
		private bool? _Dangerous;
		private bool? _Fragile;
		private bool? _NoneBox;
		private bool? _SideUp;
		private DateTime _ExpireTime;
		private int _Status;
		/// <summary>
		/// ID id
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
		/// IID 入库申请单号
		/// </summary>
		[DataMember]
		public string IID
		{
			get { return _IID; }
			set
			{
				this.OnPropertyValueChange(_.IID, _IID, value);
				this._IID = value;
			}
		}
		/// <summary>
		/// SKU sku
		/// </summary>
		[DataMember]
		public string SKU
		{
			get { return _SKU; }
			set
			{
				this.OnPropertyValueChange(_.SKU, _SKU, value);
				this._SKU = value;
			}
		}
		/// <summary>
		/// Company 公司
		/// </summary>
		[DataMember]
		public string Company
		{
			get { return _Company; }
			set
			{
				this.OnPropertyValueChange(_.Company, _Company, value);
				this._Company = value;
			}
		}
		/// <summary>
		/// BatchNumber 入库批次号
		/// </summary>
		[DataMember]
		public string BatchNumber
		{
			get { return _BatchNumber; }
			set
			{
				this.OnPropertyValueChange(_.BatchNumber, _BatchNumber, value);
				this._BatchNumber = value;
			}
		}
		/// <summary>
		/// Amount 金额
		/// </summary>
		[DataMember]
		public decimal Amount
		{
			get { return _Amount; }
			set
			{
				this.OnPropertyValueChange(_.Amount, _Amount, value);
				this._Amount = value;
			}
		}
		/// <summary>
		/// ApplyQty 申请数量
		/// </summary>
		[DataMember]
		public int ApplyQty
		{
			get { return _ApplyQty; }
			set
			{
				this.OnPropertyValueChange(_.ApplyQty, _ApplyQty, value);
				this._ApplyQty = value;
			}
		}
		/// <summary>
		/// ApplyBoxQty 申请箱数(调拨有效)
		/// </summary>
		[DataMember]
		public int ApplyBoxQty
		{
			get { return _ApplyBoxQty; }
			set
			{
				this.OnPropertyValueChange(_.ApplyBoxQty, _ApplyBoxQty, value);
				this._ApplyBoxQty = value;
			}
		}
		/// <summary>
		/// PerBoxQty 单箱数量(调拨有效)
		/// </summary>
		[DataMember]
		public int PerBoxQty
		{
			get { return _PerBoxQty; }
			set
			{
				this.OnPropertyValueChange(_.PerBoxQty, _PerBoxQty, value);
				this._PerBoxQty = value;
			}
		}
		/// <summary>
		/// Length 长
		/// </summary>
		[DataMember]
		public float? Length
		{
			get { return _Length; }
			set
			{
				this.OnPropertyValueChange(_.Length, _Length, value);
				this._Length = value;
			}
		}
		/// <summary>
		/// Width 宽
		/// </summary>
		[DataMember]
		public float? Width
		{
			get { return _Width; }
			set
			{
				this.OnPropertyValueChange(_.Width, _Width, value);
				this._Width = value;
			}
		}
		/// <summary>
		/// Height 高
		/// </summary>
		[DataMember]
		public float? Height
		{
			get { return _Height; }
			set
			{
				this.OnPropertyValueChange(_.Height, _Height, value);
				this._Height = value;
			}
		}
		/// <summary>
		/// Weight 重量
		/// </summary>
		[DataMember]
		public float? Weight
		{
			get { return _Weight; }
			set
			{
				this.OnPropertyValueChange(_.Weight, _Weight, value);
				this._Weight = value;
			}
		}
		/// <summary>
		/// Regular 规则的
		/// </summary>
		[DataMember]
		public bool? Regular
		{
			get { return _Regular; }
			set
			{
				this.OnPropertyValueChange(_.Regular, _Regular, value);
				this._Regular = value;
			}
		}
		/// <summary>
		/// Dangerous 危险的
		/// </summary>
		[DataMember]
		public bool? Dangerous
		{
			get { return _Dangerous; }
			set
			{
				this.OnPropertyValueChange(_.Dangerous, _Dangerous, value);
				this._Dangerous = value;
			}
		}
		/// <summary>
		/// Fragile 易碎的
		/// </summary>
		[DataMember]
		public bool? Fragile
		{
			get { return _Fragile; }
			set
			{
				this.OnPropertyValueChange(_.Fragile, _Fragile, value);
				this._Fragile = value;
			}
		}
		/// <summary>
		/// NoneBox 无包装的
		/// </summary>
		[DataMember]
		public bool? NoneBox
		{
			get { return _NoneBox; }
			set
			{
				this.OnPropertyValueChange(_.NoneBox, _NoneBox, value);
				this._NoneBox = value;
			}
		}
		/// <summary>
		/// SideUp 正放的
		/// </summary>
		[DataMember]
		public bool? SideUp
		{
			get { return _SideUp; }
			set
			{
				this.OnPropertyValueChange(_.SideUp, _SideUp, value);
				this._SideUp = value;
			}
		}
		/// <summary>
		/// ExpireTime 过期时间
		/// </summary>
		[DataMember]
		public DateTime ExpireTime
		{
			get { return _ExpireTime; }
			set
			{
				this.OnPropertyValueChange(_.ExpireTime, _ExpireTime, value);
				this._ExpireTime = value;
			}
		}
		/// <summary>
		/// Status 状态(1正常，2无效)
		/// </summary>
		[DataMember]
		public int Status
		{
			get { return _Status; }
			set
			{
				this.OnPropertyValueChange(_.Status, _Status, value);
				this._Status = value;
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
				_.IID,
				_.SKU,
				_.Company,
				_.BatchNumber,
				_.Amount,
				_.ApplyQty,
				_.ApplyBoxQty,
				_.PerBoxQty,
				_.Length,
				_.Width,
				_.Height,
				_.Weight,
				_.Regular,
				_.Dangerous,
				_.Fragile,
				_.NoneBox,
				_.SideUp,
				_.ExpireTime,
				_.Status};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._ID,
				this._IID,
				this._SKU,
				this._Company,
				this._BatchNumber,
				this._Amount,
				this._ApplyQty,
				this._ApplyBoxQty,
				this._PerBoxQty,
				this._Length,
				this._Width,
				this._Height,
				this._Weight,
				this._Regular,
				this._Dangerous,
				this._Fragile,
				this._NoneBox,
				this._SideUp,
				this._ExpireTime,
				this._Status};
		}
		#endregion

		#region _Field
		/// <summary>
		/// 字段信息
		/// </summary>
		public class _
		{
			/// <summary>
			/// oc_warehouse_inbound_data 
			/// </summary>
			public readonly static Field All = new Field("*", m_tableName);
			/// <summary>
			/// ID id
			/// </summary>
			public readonly static Field ID = new Field("ID", m_tableName, "id");
			/// <summary>
			/// IID 入库申请单号
			/// </summary>
			public readonly static Field IID = new Field("IID", m_tableName, "入库申请单号");
			/// <summary>
			/// SKU sku
			/// </summary>
			public readonly static Field SKU = new Field("SKU", m_tableName, "sku");
			/// <summary>
			/// Company 公司
			/// </summary>
			public readonly static Field Company = new Field("Company", m_tableName, "公司");
			/// <summary>
			/// BatchNumber 入库批次号
			/// </summary>
			public readonly static Field BatchNumber = new Field("BatchNumber", m_tableName, "入库批次号");
			/// <summary>
			/// Amount 金额
			/// </summary>
			public readonly static Field Amount = new Field("Amount", m_tableName, DbType.Decimal, 1, "金额");
			/// <summary>
			/// ApplyQty 申请数量
			/// </summary>
			public readonly static Field ApplyQty = new Field("ApplyQty", m_tableName, DbType.Int32, 1, "申请数量");
			/// <summary>
			/// ApplyBoxQty 申请箱数(调拨有效)
			/// </summary>
			public readonly static Field ApplyBoxQty = new Field("ApplyBoxQty", m_tableName, DbType.Int32, 1, "申请箱数(调拨有效)");
			/// <summary>
			/// PerBoxQty 单箱数量(调拨有效)
			/// </summary>
			public readonly static Field PerBoxQty = new Field("PerBoxQty", m_tableName, DbType.Int32, 1, "单箱数量(调拨有效)");
			/// <summary>
			/// Length 长
			/// </summary>
			public readonly static Field Length = new Field("Length", m_tableName, DbType.Single, 1, "长");
			/// <summary>
			/// Width 宽
			/// </summary>
			public readonly static Field Width = new Field("Width", m_tableName, DbType.Single, 1, "宽");
			/// <summary>
			/// Height 高
			/// </summary>
			public readonly static Field Height = new Field("Height", m_tableName, DbType.Single, 1, "高");
			/// <summary>
			/// Weight 重量
			/// </summary>
			public readonly static Field Weight = new Field("Weight", m_tableName, DbType.Single, 1, "重量");
			/// <summary>
			/// Regular 规则的
			/// </summary>
			public readonly static Field Regular = new Field("Regular", m_tableName, DbType.Boolean, 1, "规则的");
			/// <summary>
			/// Dangerous 危险的
			/// </summary>
			public readonly static Field Dangerous = new Field("Dangerous", m_tableName, DbType.Boolean, 1, "危险的");
			/// <summary>
			/// Fragile 易碎的
			/// </summary>
			public readonly static Field Fragile = new Field("Fragile", m_tableName, DbType.Boolean, 1, "易碎的");
			/// <summary>
			/// NoneBox 无包装的
			/// </summary>
			public readonly static Field NoneBox = new Field("NoneBox", m_tableName, DbType.Boolean, 1, "无包装的");
			/// <summary>
			/// SideUp 正放的
			/// </summary>
			public readonly static Field SideUp = new Field("SideUp", m_tableName, DbType.Boolean, 1, "正放的");
			/// <summary>
			/// ExpireTime 过期时间
			/// </summary>
			public readonly static Field ExpireTime = new Field("ExpireTime", m_tableName, DbType.DateTime, 1, "过期时间");
			/// <summary>
			/// Status 状态(1正常，2无效)
			/// </summary>
			public readonly static Field Status = new Field("Status", m_tableName, DbType.Int32, 1, "状态(1正常，2无效)");
		}
		#endregion


	}
	/// <summary>
	/// 实体类DBWarehouseInboundData操作类
	/// </summary>
	public partial class DBWarehouseInboundDataRepository : BaseRepository<DBWarehouseInboundData>
	{
		/// <summary>
		/// DBWarehouseInboundData构造方法
		/// </summary>
		public DBWarehouseInboundDataRepository() : base()
		{
			_dbContext = new DBContext();
		}
		/// <summary>
		/// DBWarehouseInboundData构造方法
		/// </summary>
		public DBWarehouseInboundDataRepository(DBContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
		/// <summary>
		/// DBWarehouseInboundData构造方法
		/// <param name="connStrName">连接字符串中的名称</param>
		/// </summary>
		public DBWarehouseInboundDataRepository(string connStrName) : base(connStrName)
		{
			_dbContext = new DBContext(connStrName);
		}
		/// <summary>
		/// DBWarehouseInboundData构造方法
		/// <param name="dbType">数据库类型</param>
		/// <param name="connStr">连接字符串</param>
		/// </summary>
		public DBWarehouseInboundDataRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
		{
			_dbContext = new DBContext(dbType, connStr);
		}
	}

}

