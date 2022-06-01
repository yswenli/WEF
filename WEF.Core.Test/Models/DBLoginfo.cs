//------------------------------------------------------------------------------
// <WEF-ModelGenerator>
//     此代码由WEF数据库工具, Version=5.2.1.7, Culture=neutral, PublicKeyToken=null生成;时间 2022-03-07 09:53:08.130
//     运行时版本:4.0.30319.42000
//     不建议手动更改此代码，如有需要请自行扩展partial类
// </WEF-ModelGenerator>
//------------------------------------------------------------------------------


using System;
using System.Data;
using System.Runtime.Serialization;

using WEF;
using WEF.Common;

namespace WEF.Core.Test.Models
{

	/// <summary>
	/// 实体类DBLoginfo
	/// </summary>
	[Serializable, DataContract, Table("dbloginfo")]
	public partial class DBLoginfo : Entity
	{
		private static string m_tableName;
		/// <summary>
		/// 实体类DBLoginfo
		/// </summary>
		public DBLoginfo() : base("dbloginfo") { m_tableName = "dbloginfo"; }
		/// <summary>
		/// 实体类DBLoginfo
		/// <param name="tableName">表名</param>
		/// </summary>
		public DBLoginfo(string tableName) : base(tableName) { m_tableName = tableName; }

		#region Model
		private string _ID;
		private string _ServiceName;
		private string _Description;
		private string _IP;
		private string _Error;
		private int? _Level;
		private DateTime? _Created;
		private bool _Resolved;
		private string _Operator;
		private string _Reason;
		private DateTime? _Updated;
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
		/// ServiceName 
		/// </summary>
		[DataMember]
		public string ServiceName
		{
			get { return _ServiceName; }
			set
			{
				this.OnPropertyValueChange(_.ServiceName, _ServiceName, value);
				this._ServiceName = value;
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
		/// IP 
		/// </summary>
		[DataMember]
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
		/// Error 
		/// </summary>
		[DataMember]
		public string Error
		{
			get { return _Error; }
			set
			{
				this.OnPropertyValueChange(_.Error, _Error, value);
				this._Error = value;
			}
		}
		/// <summary>
		/// Level 
		/// </summary>
		[DataMember]
		public int? Level
		{
			get { return _Level; }
			set
			{
				this.OnPropertyValueChange(_.Level, _Level, value);
				this._Level = value;
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
		/// <summary>
		/// Resolved 
		/// </summary>
		[DataMember]
		public bool Resolved
		{
			get { return _Resolved; }
			set
			{
				this.OnPropertyValueChange(_.Resolved, _Resolved, value);
				this._Resolved = value;
			}
		}
		/// <summary>
		/// Operator 
		/// </summary>
		[DataMember]
		public string Operator
		{
			get { return _Operator; }
			set
			{
				this.OnPropertyValueChange(_.Operator, _Operator, value);
				this._Operator = value;
			}
		}
		/// <summary>
		/// Reason 
		/// </summary>
		[DataMember]
		public string Reason
		{
			get { return _Reason; }
			set
			{
				this.OnPropertyValueChange(_.Reason, _Reason, value);
				this._Reason = value;
			}
		}
		/// <summary>
		/// Updated 
		/// </summary>
		[DataMember]
		public DateTime? Updated
		{
			get { return _Updated; }
			set
			{
				this.OnPropertyValueChange(_.Updated, _Updated, value);
				this._Updated = value;
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
				_.ServiceName,
				_.Description,
				_.IP,
				_.Error,
				_.Level,
				_.Created,
				_.Resolved,
				_.Operator,
				_.Reason,
				_.Updated};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._ID,
				this._ServiceName,
				this._Description,
				this._IP,
				this._Error,
				this._Level,
				this._Created,
				this._Resolved,
				this._Operator,
				this._Reason,
				this._Updated};
		}
		#endregion

		#region _Field
		/// <summary>
		/// 字段信息
		/// </summary>
		public class _
		{
			/// <summary>
			/// dbloginfo 
			/// </summary>
			public readonly static Field All = new Field("*", m_tableName);
			/// <summary>
			/// ID 
			/// </summary>
			public readonly static Field ID = new Field("ID", m_tableName, "ID");
			/// <summary>
			/// ServiceName 
			/// </summary>
			public readonly static Field ServiceName = new Field("ServiceName", m_tableName, "ServiceName");
			/// <summary>
			/// Description 
			/// </summary>
			public readonly static Field Description = new Field("Description", m_tableName, "Description");
			/// <summary>
			/// IP 
			/// </summary>
			public readonly static Field IP = new Field("IP", m_tableName, "IP");
			/// <summary>
			/// Error 
			/// </summary>
			public readonly static Field Error = new Field("Error", m_tableName, "Error");
			/// <summary>
			/// Level 
			/// </summary>
			public readonly static Field Level = new Field("Level", m_tableName, DbType.Int32, 1, "Level");
			/// <summary>
			/// Created 
			/// </summary>
			public readonly static Field Created = new Field("Created", m_tableName, DbType.DateTime, 1, "Created");
			/// <summary>
			/// Resolved 
			/// </summary>
			public readonly static Field Resolved = new Field("Resolved", m_tableName, DbType.Int32, 1, "Resolved");
			/// <summary>
			/// Operator 
			/// </summary>
			public readonly static Field Operator = new Field("Operator", m_tableName, "Operator");
			/// <summary>
			/// Reason 
			/// </summary>
			public readonly static Field Reason = new Field("Reason", m_tableName, "Reason");
			/// <summary>
			/// Updated 
			/// </summary>
			public readonly static Field Updated = new Field("Updated", m_tableName, DbType.DateTime, 1, "Updated");
		}
		#endregion


	}
	/// <summary>
	/// 实体类DBLoginfo操作类
	/// </summary>
	public partial class DBLoginfoRepository : BaseRepository<DBLoginfo>
	{
		/// <summary>
		/// DBLoginfo构造方法
		/// </summary>
		public DBLoginfoRepository() : base()
		{
			_db = new DBContext();
		}
		/// <summary>
		/// DBLoginfo构造方法
		/// </summary>
		public DBLoginfoRepository(DBContext dbContext) : base(dbContext)
		{
			_db = dbContext;
		}
		/// <summary>
		/// DBLoginfo构造方法
		/// <param name="connStrName">连接字符串中的名称</param>
		/// </summary>
		public DBLoginfoRepository(string connStrName) : base(connStrName)
		{
			_db = new DBContext(connStrName);
		}
		/// <summary>
		/// DBLoginfo构造方法
		/// <param name="dbType">数据库类型</param>
		/// <param name="connStr">连接字符串</param>
		/// </summary>
		public DBLoginfoRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
		{
			_db = new DBContext(dbType, connStr);
		}
	}

}

