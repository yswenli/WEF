﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由WEF数据库工具, Version=5.0.0.3, Culture=neutral, PublicKeyToken=null生成;时间 2021-06-15 14:49:46.999
//     运行时版本:4.0.30319.42000
//     不建议手动更改此代码，如有需要请自行扩展partial类
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Runtime.Serialization;

using WEF;
using WEF.Common;

namespace WEF.Models
{

	/// <summary>
	/// 实体类SkuInfo
	/// </summary>
	[Serializable, DataContract, Table("SkuInfo")]
	public partial class SkuInfo : Entity
	{
		private static string m_tableName;
		/// <summary>
		/// 实体类SkuInfo
		/// </summary>
		public SkuInfo() : base("SkuInfo") { m_tableName = "SkuInfo"; }
		/// <summary>
		/// 实体类SkuInfo
		/// <param name="dbType">表名</param>
		/// </summary>
		public SkuInfo(string tableName) : base(tableName) { m_tableName = tableName; }

		#region Model
		private string _ID;
		private string _SKU;
		private decimal? _Price;
		private string _Description;
		private DateTime? _Created;
		/// <summary>
		/// ID 
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
		/// SKU 
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
		/// Price 
		/// </summary>
		[DataMember]
		public decimal? Price
		{
			get { return _Price; }
			set
			{
				this.OnPropertyValueChange(_.Price, _Price, value);
				this._Price = value;
			}
		}
		/// <summary>
		/// Description 
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
		/// Created 
		/// </summary>
		[DataMember]
		public DateTime? Created
		{
			get { return _Created; }
			set
			{
				this.OnPropertyValueChange(_.Created, _Created, value);
				this._Created = value;
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
				_.SKU,
				_.Price,
				_.Description,
				_.Created};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._ID,
				this._SKU,
				this._Price,
				this._Description,
				this._Created};
		}
		#endregion

		#region _Field
		/// <summary>
		/// 字段信息
		/// </summary>
		public class _
		{
			/// <summary>
			/// SkuInfo 
			/// </summary>
			public readonly static Field All = new Field("*", m_tableName);
			/// <summary>
			/// ID 
			/// </summary>
			public readonly static Field ID = new Field("ID", m_tableName, "ID");
			/// <summary>
			/// SKU 
			/// </summary>
			public readonly static Field SKU = new Field("SKU", m_tableName, "SKU");
			/// <summary>
			/// Price 
			/// </summary>
			public readonly static Field Price = new Field("Price", m_tableName, "Price");
			/// <summary>
			/// Description 
			/// </summary>
			public readonly static Field Description = new Field("Description", m_tableName, "Description");
			/// <summary>
			/// Created 
			/// </summary>
			public readonly static Field Created = new Field("Created", m_tableName, "Created");
		}
		#endregion


	}
	/// <summary>
	/// 实体类SkuInfo操作类
	/// </summary>
	public partial class SkuInfoRepository : BaseRepository<SkuInfo>
	{
		/// <summary>
		/// SkuInfo构造方法
		/// </summary>
		public SkuInfoRepository() : base()
		{
			_db = new DBContext();
		}
		/// <summary>
		/// SkuInfo构造方法
		/// </summary>
		public SkuInfoRepository(DBContext dbContext) : base(dbContext)
		{
			_db = dbContext;
		}
		/// <summary>
		/// SkuInfo构造方法
		/// <param name="connStrName">连接字符串中的名称</param>
		/// </summary>
		public SkuInfoRepository(string connStrName) : base(connStrName)
		{
			_db = new DBContext(connStrName);
		}
		/// <summary>
		/// SkuInfo构造方法
		/// <param name="dbType">数据库类型</param>
		/// <param name="connStr">连接字符串</param>
		/// </summary>
		public SkuInfoRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
		{
			_db = new DBContext(dbType, connStr);
		}
	}

}
