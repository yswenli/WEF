/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Test
*文件名： MultyTableTest
*版本号： V1.0.0.0
*唯一标识：645f0536-df8d-4cd1-ad3c-fcc8e33f6480
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2021/10/15 16:45:39
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/10/15 16:45:39
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Runtime.Serialization;

using WEF.Common;

namespace WEF.Test
{
    public static class MultyTableTest
    {
        public static void Test()
        {
            var dbContext = new DBContext(DatabaseType.SqlServer, @"Data Source=LP20210416002\MSSQLSERVER2014;Initial Catalog=Oceania_Test;Integrated Security=True");

			var list = dbContext.Search<DBA>().LeftJoin<DBB>((a, b) => a.ID == b.AID).ToList<DBA>();
        }
	}

	/// <summary>
	/// 实体类DBA
	/// </summary>
	[Serializable, DataContract, Table("A")]
	public partial class DBA : Entity
	{
		private static string m_tableName;
		/// <summary>
		/// 实体类DBA
		/// </summary>
		public DBA() : base("A") { m_tableName = "A"; }
		/// <summary>
		/// 实体类DBA
		/// <param name="tableName">表名</param>
		/// </summary>
		public DBA(string tableName) : base(tableName) { m_tableName = tableName; }

		#region Model
		private int _ID;
		private int _CID;
		/// <summary>
		/// ID 
		/// </summary>
		[DataMember]
		public int ID
		{
			get { return _ID; }
			set
			{
				this.OnPropertyValueChange(_.ID, _ID, value);
				this._ID = value;
			}
		}
		/// <summary>
		/// CID 
		/// </summary>
		[DataMember]
		public int CID
		{
			get { return _CID; }
			set
			{
				this.OnPropertyValueChange(_.CID, _CID, value);
				this._CID = value;
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
				_.CID};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._ID,
				this._CID};
		}
		#endregion

		#region _Field
		/// <summary>
		/// 字段信息
		/// </summary>
		public class _
		{
			/// <summary>
			/// A 
			/// </summary>
			public readonly static Field All = new Field("*", m_tableName);
			/// <summary>
			/// ID 
			/// </summary>
			public readonly static Field ID = new Field("ID", m_tableName, "ID");
			/// <summary>
			/// CID 
			/// </summary>
			public readonly static Field CID = new Field("CID", m_tableName, "CID");
		}
		#endregion


	}
	/// <summary>
	/// 实体类DBA操作类
	/// </summary>
	public partial class DBARepository : BaseRepository<DBA>
	{
		/// <summary>
		/// DBA构造方法
		/// </summary>
		public DBARepository() : base()
		{
			_dbContext = new DBContext();
		}
		/// <summary>
		/// DBA构造方法
		/// </summary>
		public DBARepository(DBContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
		/// <summary>
		/// DBA构造方法
		/// <param name="connStrName">连接字符串中的名称</param>
		/// </summary>
		public DBARepository(string connStrName) : base(connStrName)
		{
			_dbContext = new DBContext(connStrName);
		}
		/// <summary>
		/// DBA构造方法
		/// <param name="dbType">数据库类型</param>
		/// <param name="connStr">连接字符串</param>
		/// </summary>
		public DBARepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
		{
			_dbContext = new DBContext(dbType, connStr);
		}
	}

	/// <summary>
	/// 实体类DBB
	/// </summary>
	[Serializable, DataContract, Table("B")]
	public partial class DBB : Entity
	{
		private static string m_tableName;
		/// <summary>
		/// 实体类DBB
		/// </summary>
		public DBB() : base("B") { m_tableName = "B"; }
		/// <summary>
		/// 实体类DBB
		/// <param name="tableName">表名</param>
		/// </summary>
		public DBB(string tableName) : base(tableName) { m_tableName = tableName; }

		#region Model
		private int _ID;
		private int _AID;
		/// <summary>
		/// ID 
		/// </summary>
		[DataMember]
		public int ID
		{
			get { return _ID; }
			set
			{
				this.OnPropertyValueChange(_.ID, _ID, value);
				this._ID = value;
			}
		}
		/// <summary>
		/// AID 
		/// </summary>
		[DataMember]
		public int AID
		{
			get { return _AID; }
			set
			{
				this.OnPropertyValueChange(_.AID, _AID, value);
				this._AID = value;
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
				_.AID};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._ID,
				this._AID};
		}
		#endregion

		#region _Field
		/// <summary>
		/// 字段信息
		/// </summary>
		public class _
		{
			/// <summary>
			/// B 
			/// </summary>
			public readonly static Field All = new Field("*", m_tableName);
			/// <summary>
			/// ID 
			/// </summary>
			public readonly static Field ID = new Field("ID", m_tableName, "ID");
			/// <summary>
			/// AID 
			/// </summary>
			public readonly static Field AID = new Field("AID", m_tableName, "AID");
		}
		#endregion


	}
	/// <summary>
	/// 实体类DBB操作类
	/// </summary>
	public partial class DBBRepository : BaseRepository<DBB>
	{
		/// <summary>
		/// DBB构造方法
		/// </summary>
		public DBBRepository() : base()
		{
			_dbContext = new DBContext();
		}
		/// <summary>
		/// DBB构造方法
		/// </summary>
		public DBBRepository(DBContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
		/// <summary>
		/// DBB构造方法
		/// <param name="connStrName">连接字符串中的名称</param>
		/// </summary>
		public DBBRepository(string connStrName) : base(connStrName)
		{
			_dbContext = new DBContext(connStrName);
		}
		/// <summary>
		/// DBB构造方法
		/// <param name="dbType">数据库类型</param>
		/// <param name="connStr">连接字符串</param>
		/// </summary>
		public DBBRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
		{
			_dbContext = new DBContext(dbType, connStr);
		}
	}


}
