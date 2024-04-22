//------------------------------------------------------------------------------
// <WEF-ModelGenerator>
//     此代码由WEF数据库工具, Version=5.2.1.7, Culture=neutral, PublicKeyToken=null生成;时间 2021-12-07 09:04:30.149
//     运行时版本:4.0.30319.42000
//     不建议手动更改此代码，如有需要请自行扩展partial类
// </WEF-ModelGenerator>
//------------------------------------------------------------------------------


using System;
using System.Data;
using System.Runtime.Serialization;

using WEF;
using WEF.Db;

namespace WEF.Test.Models
{

    /// <summary>
    /// 实体类DBOcJoybuycarriercode
    /// </summary>
    [Serializable, DataContract, Table("OC_JoybuyCarrierCode")]
	public partial class DBOcJoybuycarriercode : Entity
	{
		private static string m_tableName;
		/// <summary>
		/// 实体类DBOcJoybuycarriercode
		/// </summary>
		public DBOcJoybuycarriercode() : base("OC_JoybuyCarrierCode") { m_tableName = "OC_JoybuyCarrierCode"; }
		/// <summary>
		/// 实体类DBOcJoybuycarriercode
		/// <param name="tableName">表名</param>
		/// </summary>
		public DBOcJoybuycarriercode(string tableName) : base(tableName) { m_tableName = tableName; }

		#region Model
		private Guid _ID;
		private string _CarrierName;
		private long? _CarrierCode;
		private string _ShipmentsCarrierCode;
		/// <summary>
		/// ID 
		/// </summary>
		[DataMember]
		public Guid ID
		{
			get { return _ID; }
			set
			{
				this.OnPropertyValueChange(_.ID, _ID, value);
				this._ID = value;
			}
		}
		/// <summary>
		/// CarrierName Joybuy CarrierName
		/// </summary>
		[DataMember]
		public string CarrierName
		{
			get { return _CarrierName; }
			set
			{
				this.OnPropertyValueChange(_.CarrierName, _CarrierName, value);
				this._CarrierName = value;
			}
		}
		/// <summary>
		/// CarrierCode Joybuy CarrierCode
		/// </summary>
		[DataMember]
		public long? CarrierCode
		{
			get { return _CarrierCode; }
			set
			{
				this.OnPropertyValueChange(_.CarrierCode, _CarrierCode, value);
				this._CarrierCode = value;
			}
		}
		/// <summary>
		/// ShipmentsCarrierCode Barn..OC_Shipments中的CarrierCode
		/// </summary>
		[DataMember]
		public string ShipmentsCarrierCode
		{
			get { return _ShipmentsCarrierCode; }
			set
			{
				this.OnPropertyValueChange(_.ShipmentsCarrierCode, _ShipmentsCarrierCode, value);
				this._ShipmentsCarrierCode = value;
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
				_.CarrierName,
				_.CarrierCode,
				_.ShipmentsCarrierCode};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._ID,
				this._CarrierName,
				this._CarrierCode,
				this._ShipmentsCarrierCode};
		}
		#endregion

		#region _Field
		/// <summary>
		/// 字段信息
		/// </summary>
		public class _
		{
			/// <summary>
			/// OC_JoybuyCarrierCode 
			/// </summary>
			public readonly static Field All = new Field("*", m_tableName);
			/// <summary>
			/// ID 
			/// </summary>
			public readonly static Field ID = new Field("ID", m_tableName, DbType.Guid, 1, "ID");
			/// <summary>
			/// CarrierName Joybuy CarrierName
			/// </summary>
			public readonly static Field CarrierName = new Field("carrierName", m_tableName, "Joybuy CarrierName");
			/// <summary>
			/// CarrierCode Joybuy CarrierCode
			/// </summary>
			public readonly static Field CarrierCode = new Field("carrierCode", m_tableName, DbType.Int64, 1, "Joybuy CarrierCode");
			/// <summary>
			/// ShipmentsCarrierCode Barn..OC_Shipments中的CarrierCode
			/// </summary>
			public readonly static Field ShipmentsCarrierCode = new Field("ShipmentsCarrierCode", m_tableName, "Barn..OC_Shipments中的CarrierCode");
		}
		#endregion


	}
	/// <summary>
	/// 实体类DBOcJoybuycarriercode操作类
	/// </summary>
	public partial class DBOcJoybuycarriercodeRepository : BaseRepository<DBOcJoybuycarriercode>
	{
		/// <summary>
		/// DBOcJoybuycarriercode构造方法
		/// </summary>
		public DBOcJoybuycarriercodeRepository() : base()
		{
			_dbContext = new DBContext();
		}
		/// <summary>
		/// DBOcJoybuycarriercode构造方法
		/// </summary>
		public DBOcJoybuycarriercodeRepository(DBContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
		/// <summary>
		/// DBOcJoybuycarriercode构造方法
		/// <param name="connStrName">连接字符串中的名称</param>
		/// </summary>
		public DBOcJoybuycarriercodeRepository(string connStrName) : base(connStrName)
		{
			_dbContext = new DBContext(connStrName);
		}
		/// <summary>
		/// DBOcJoybuycarriercode构造方法
		/// <param name="dbType">数据库类型</param>
		/// <param name="connStr">连接字符串</param>
		/// </summary>
		public DBOcJoybuycarriercodeRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
		{
			_dbContext = new DBContext(dbType, connStr);
		}
	}

}

