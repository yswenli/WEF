using Npgsql;
using System;
using System.Data;
using System.Text;


namespace WEF.DbDAL.PostgreSQL
{
    //public class DbObject : IDbObject
    //{
    //    private string _dbconnectStr;

    //    private NpgsqlConnection _connect;

    //    private DBContext _dbContext;


    //    public DbObject(string connectStr)
    //    {
    //        this._dbconnectStr = connectStr;
    //        _dbContext = new DBContext(DatabaseType.PostgreSQL, connectStr);

    //    }

    //    public DbObject(bool SSPI, string server, string serviceName, string User, string Pass)
    //    {
    //        var ipPort = server.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

    //        var connectStr = $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={ipPort[0]})(PORT={ipPort[1]})))(CONNECT_DATA =(SERVICE_NAME = {serviceName})));User Id={User};Password={Pass};";

    //        var connectStr = $"PORT={ipPort[1]};DATABASE=Demo;HOST={ipPort[0]};PASSWORD={Pass};USER ID={User};";

    //        this._dbconnectStr = connectStr;

    //        _dbContext = new DBContext(DatabaseType.Oracle, connectStr);
    //    }

    //    public bool DeleteTable(string DbName, string TableName)
    //    {
    //        try
    //        {
    //            ExecuteSql(DbName, "DROP TABLE " + TableName);
    //            return true;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }

    //    public int ExecuteSql(string DbName, string SQLString)
    //    {
    //        using (var oracleCon = new OracleConnection(_dbconnectStr))
    //        {
    //            oracleCon.Open();
    //            var dbCommand = new OracleCommand(SQLString, oracleCon);
    //            dbCommand.CommandText = SQLString;
    //            int rows = dbCommand.ExecuteNonQuery();
    //            return rows;
    //        }
    //    }

    //    public DataTable GetColumnInfoList(string DbName, string TableName)
    //    {
    //        StringBuilder builder = new StringBuilder();
    //        builder.Append("select ");
    //        builder.Append("COLUMN_ID as colorder,");
    //        builder.Append("COLUMN_NAME as ColumnName,");
    //        builder.Append("DATA_TYPE as TypeName,");
    //        builder.Append("DATA_LENGTH as Length,");
    //        builder.Append("DATA_PRECISION as Preci,");
    //        builder.Append("DATA_SCALE as Scale,");
    //        builder.Append("'' as IsIdentity,");
    //        builder.Append("'' as isPK,");
    //        builder.Append("NULLABLE as cisNull ,");
    //        builder.Append("DATA_DEFAULT as defaultVal, ");
    //        builder.Append("'' as deText ");
    //        builder.Append(" from USER_TAB_COLUMNS ");
    //        builder.Append(" where TABLE_NAME='" + TableName + "'");
    //        builder.Append(" order by COLUMN_ID");
    //        DataTable alldt = this.Query("", builder.ToString()).Tables[0];
    //        DataTable keydt = Query("", "select column_name from user_constraints c,user_cons_columns col where c.constraint_name=col.constraint_name and c.constraint_type='P' and c.table_name='" + TableName + "'").Tables[0];

    //        foreach (DataRow drkey in keydt.Rows)
    //        {
    //            DataRow[] drs = alldt.Select("ColumnName='" + drkey["column_name"].ToString() + "'");
    //            if (null != drs && drs.Length > 0)
    //                drs[0]["isPK"] = "√";
    //        }
    //        alldt.AcceptChanges();
    //        return alldt;
    //    }

    //    public DataTable GetColumnList(string DbName, string TableName)
    //    {
    //        StringBuilder builder = new StringBuilder();
    //        builder.Append("select ");
    //        builder.Append("COLUMN_ID as colorder,");
    //        builder.Append("COLUMN_NAME as ColumnName,");
    //        builder.Append("DATA_TYPE as TypeName ");
    //        builder.Append(" from USER_TAB_COLUMNS ");
    //        builder.Append(" where TABLE_NAME='" + TableName + "'");
    //        builder.Append(" order by COLUMN_ID");
    //        return this.Query("", builder.ToString()).Tables[0];


    //    }

    //    public DataTable GetDBList()
    //    {
    //        return null;
    //    }

    //    public DataTable GetKeyName(string DbName, string TableName)
    //    {
    //        StringBuilder builder = new StringBuilder();
    //        builder.Append("select * from ");
    //        builder.Append("( ");
    //        builder.Append("select ");
    //        builder.Append("COLUMN_ID as colorder,");
    //        builder.Append("COLUMN_NAME as ColumnName,");
    //        builder.Append("DATA_TYPE as TypeName,");
    //        builder.Append("DATA_LENGTH as Length,");
    //        builder.Append("DATA_PRECISION as Preci,");
    //        builder.Append("DATA_SCALE as Scale,");
    //        builder.Append("'' as IsIdentity,");
    //        builder.Append("'' as isPK,");
    //        builder.Append("NULLABLE as cisNull ,");
    //        builder.Append("DATA_DEFAULT as defaultVal, ");
    //        builder.Append("'' as deText ");
    //        builder.Append(" from USER_TAB_COLUMNS ");
    //        builder.Append(" where TABLE_NAME='" + TableName + "'");
    //        builder.Append(") Keyname ");
    //        builder.Append(" where ColumnName in (");
    //        builder.Append("select column_name from user_constraints c,user_cons_columns col where c.constraint_name=col.constraint_name and c.constraint_type='P' and c.table_name='" + TableName + "'");
    //        builder.Append(")");
    //        return this.Query("", builder.ToString()).Tables[0];
    //    }

    //    public string GetObjectInfo(string DbName, string objName)
    //    {
    //        return null;
    //    }

    //    public DataTable GetProcInfo(string DbName)
    //    {
    //        return null;
    //    }

    //    public DataTable GetProcs(string DbName)
    //    {
    //        string sQLString = "SELECT * FROM ALL_SOURCE  where TYPE='PROCEDURE'  ";
    //        return this.Query(DbName, sQLString).Tables[0];
    //    }

    //    public object GetSingle(string DbName, string SQLString)
    //    {
    //        return _dbContext.FromSql(SQLString).ToScalar();
    //    }

    //    public DataTable GetTabData(string DbName, string TableName, int TopNum)
    //    {
    //        return _dbContext.Search(TableName).Top(TopNum).ToDataTable();
    //    }

    //    public DataTable GetTables(string DbName)
    //    {
    //        string sQLString = "select TNAME name from tab where TABTYPE='TABLE' order by TNAME";
    //        return this.Query("", sQLString).Tables[0];
    //    }

    //    public DataTable GetTablesInfo(string DbName)
    //    {
    //        string sQLString = "select TNAME name,'dbo' cuser,TABTYPE type,'' dates from tab where TABTYPE='TABLE' order by TNAME";
    //        return this.Query("", sQLString).Tables[0];
    //    }

    //    public DataTable GetTabViews(string DbName)
    //    {
    //        string sQLString = "select TNAME name,TABTYPE type from tab  order by TNAME";
    //        return this.Query("", sQLString).Tables[0];
    //    }

    //    public DataTable GetTabViewsInfo(string DbName)
    //    {
    //        string sQLString = "select TNAME name,'dbo' cuser,TABTYPE type,'' dates from tab  order by TNAME";
    //        return this.Query("", sQLString).Tables[0];
    //    }

    //    public string GetVersion()
    //    {
    //        return "";
    //    }

    //    public DataTable GetVIEWs(string DbName)
    //    {
    //        string sQLString = "select TNAME name from tab where TABTYPE='VIEW' order by TNAME";
    //        return this.Query("", sQLString).Tables[0];
    //    }

    //    public DataTable GetVIEWsInfo(string DbName)
    //    {
    //        string sQLString = "select TNAME name,'dbo' cuser,TABTYPE type,'' dates from tab where TABTYPE='VIEW' order by TNAME";
    //        return this.Query("", sQLString).Tables[0];
    //    }


    //    public DataSet Query(string DbName, string SQLString)
    //    {

    //        DataSet ds = new DataSet();

    //        using (OracleConnection oracleCon = new OracleConnection(_dbconnectStr))
    //        {
    //            oracleCon.Open();
    //            OracleDataAdapter command = new OracleDataAdapter(SQLString, oracleCon);
    //            command.Fill(ds, "ds");
    //        }

    //        return ds;
    //    }

    //    public bool RenameTable(string DbName, string OldName, string NewName)
    //    {
    //        return false;
    //    }

    //    public string DbConnectStr
    //    {
    //        get
    //        {
    //            return this._dbconnectStr;
    //        }
    //        set
    //        {
    //            this._dbconnectStr = value;
    //        }
    //    }

    //    public string DbType
    //    {
    //        get
    //        {
    //            return "Oracle";
    //        }
    //    }
    //}
}

