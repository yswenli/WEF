using Npgsql;
using System;
using System.Data;


namespace WEF.DbDAL.PostgreSQL
{
    public class DbObject : IDbObject
    {
        private string _dbconnectStr;

        private NpgsqlConnection _connect;

        private DBContext _dbContext;


        public DbObject(string connectStr)
        {
            this._dbconnectStr = connectStr;
            _dbContext = new DBContext(DatabaseType.PostgreSQL, connectStr);

        }

        public DbObject(string server, string port, string userName, string pwd, string dataBaseName)
        {
            var connectStr = $"PORT={port};DATABASE={dataBaseName};HOST={server};PASSWORD={pwd};USER ID={userName};";

            this._dbconnectStr = connectStr;

            _dbContext = new DBContext(DatabaseType.PostgreSQL, connectStr);
        }


        public DbObject(string server, string port, string userName, string pwd)
        {
            var connectStr = $"PORT={port};HOST={server};PASSWORD={pwd};USER ID={userName};";

            this._dbconnectStr = connectStr;

            _dbContext = new DBContext(DatabaseType.PostgreSQL, connectStr);
        }


        public void OpenDB()
        {
            using (var conn = new NpgsqlConnection(_dbconnectStr))
            {
                conn.Open();
            }

        }

        public DataSet Query(string dbName, string sqlStr)
        {
            DataSet ds = new DataSet();
            using (NpgsqlConnection conn = new NpgsqlConnection(_dbconnectStr))
            {
                conn.Open();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sqlStr, conn);
                adapter.Fill(ds, "ds");
            }
            return ds;
        }

        public int ExecuteSql(string dbName, string sqlStr)
        {
            using (var conn = new NpgsqlConnection(_dbconnectStr))
            {
                conn.Open();
                var dbCommand = new NpgsqlCommand(sqlStr, conn);
                dbCommand.CommandText = sqlStr;
                int rows = dbCommand.ExecuteNonQuery();
                return rows;
            }
        }

        public IDataReader GetDataReader(string dbName, string sqlStr)
        {
            var conn = new NpgsqlConnection(_dbconnectStr);
            conn.Open();
            var dbCommand = new NpgsqlCommand(sqlStr, conn);
            dbCommand.CommandText = sqlStr;
            return dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public DataTable GetColumnInfoList(string DbName, string TableName)
        {
            var sql = "SELECT ordinal_position as colorder,column_name as ColumnName,data_type as TypeName,character_maximum_length as Length,numeric_precision as Preci,numeric_scale as Scale,is_identity as IsIdentity,'' as isPK,is_nullable as cisNull,column_default as defaultVal,'' as deText FROM information_schema.columns WHERE table_schema = 'public' AND table_name = '" + TableName + "'";
            DataTable alldt = this.Query("", sql).Tables[0];

            var sql2 = "select pg_attribute.attname as column_name,pg_type.typname as typename,pg_constraint.conname as pk_name from pg_constraint inner join pg_class on pg_constraint.conrelid = pg_class.oid inner join pg_attribute on pg_attribute.attrelid = pg_class.oid and pg_attribute.attnum = pg_constraint.conkey[1] inner join pg_type on pg_type.oid = pg_attribute.atttypid where pg_class.relname = '" + TableName + "' and pg_constraint.contype = 'p'";

            DataTable keydt = Query("", sql2).Tables[0];

            foreach (DataRow drkey in keydt.Rows)
            {
                DataRow[] drs = alldt.Select("ColumnName='" + drkey["column_name"].ToString() + "'");
                if (null != drs && drs.Length > 0)
                    drs[0]["isPK"] = "√";
            }
            alldt.AcceptChanges();
            return alldt;
        }

        public DataTable GetColumnList(string DbName, string TableName)
        {
            var sql = "SELECT a.attname as name,format_type(a.atttypid,a.atttypmod) as type,col_description(a.attrelid,a.attnum) as comment, a.attnotnull as notnull FROM pg_class as c,pg_attribute as a where c.relname = '" + TableName + "' and a.attrelid = c.oid and a.attnum>0;";
            return this.Query("", sql).Tables[0];
        }

        public DataTable GetDBList()
        {
            return this.Query("", "select datname from pg_database order by datname").Tables[0];
        }

        public DataTable GetKeyName(string DbName, string TableName)
        {
            string sql = "select pg_attribute.attname as ColumnName,pg_type.typname as typename,pg_constraint.conname as pk_name from pg_constraint inner join pg_class on pg_constraint.conrelid = pg_class.oid inner join pg_attribute on pg_attribute.attrelid = pg_class.oid and pg_attribute.attnum = pg_constraint.conkey[1] inner join pg_type on pg_type.oid = pg_attribute.atttypid where pg_class.relname = '" + TableName + "' and pg_constraint.contype = 'p'";
            return this.Query("", sql).Tables[0];
        }

        public string GetObjectInfo(string DbName, string objName)
        {
            return null;
        }

        public DataTable GetProcInfo(string DbName)
        {
            return null;
        }

        public DataTable GetProcs(string DbName)
        {
            string sQLString = "SELECT * FROM ALL_SOURCE  where TYPE='PROCEDURE'  ";
            return this.Query(DbName, sQLString).Tables[0];
        }

        public object GetSingle(string DbName, string SQLString)
        {
            return _dbContext.FromSql(SQLString).ToScalar();
        }

        public DataTable GetTabData(string DbName, string TableName, int TopNum)
        {
            return _dbContext.Search(TableName).Top(TopNum).ToDataTable();
        }

        public DataTable GetTables(string DbName)
        {
            string sQLString = "SELECT tablename as name,obj_description(relfilenode,'pg_class') FROM pg_tables a, pg_class b WHERE a.tablename = b.relname and a.tablename NOT LIKE 'pg%' AND a.tablename NOT LIKE 'sql_%' ORDER BY a.tablename;";
            return this.Query("", sQLString).Tables[0];
        }

        public DataTable GetTablesInfo(string DbName)
        {
            string sQLString = "SELECT tablename as name,obj_description(relfilenode,'pg_class') FROM pg_tables a, pg_class b WHERE a.tablename = b.relname and a.tablename NOT LIKE 'pg%' AND a.tablename NOT LIKE 'sql_%' ORDER BY a.tablename;";
            return this.Query("", sQLString).Tables[0];
        }

        public DataTable GetTabViews(string DbName)
        {
            string sQLString = "SELECT viewname as name FROM pg_views a WHERE schemaname != 'pg_catalog' and schemaname != 'information_schema' ORDER BY viewname;";
            return this.Query("", sQLString).Tables[0];
        }

        public DataTable GetTabViewsInfo(string DbName)
        {
            string sQLString = "SELECT viewname as name FROM pg_views a WHERE schemaname != 'pg_catalog' and schemaname != 'information_schema' ORDER BY viewname;";
            return this.Query("", sQLString).Tables[0];
        }

        public string GetVersion()
        {
            return "";
        }

        public DataTable GetVIEWs(string DbName)
        {
            string sQLString = "SELECT viewname as name FROM pg_views a WHERE schemaname != 'pg_catalog' and schemaname != 'information_schema' ORDER BY viewname;";
            return this.Query("", sQLString).Tables[0];
        }

        public DataTable GetVIEWsInfo(string DbName)
        {
            string sQLString = "SELECT viewname as name FROM pg_views a WHERE schemaname != 'pg_catalog' and schemaname != 'information_schema' ORDER BY viewname;";
            return this.Query("", sQLString).Tables[0];
        }


        public bool DeleteTable(string DbName, string TableName)
        {
            try
            {
                ExecuteSql(DbName, "DROP TABLE " + TableName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除视图
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public bool DeleteView(string DbName, string TableName)
        {
            try
            {
                string text1 = "DROP VIEW " + TableName + "";
                this.ExecuteSql(DbName, TableName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RenameTable(string DbName, string OldName, string NewName)
        {
            return false;
        }

        public string DbConnectStr
        {
            get
            {
                return this._dbconnectStr;
            }
            set
            {
                this._dbconnectStr = value;
            }
        }

        public string DbType
        {
            get
            {
                return "PostgreSQL";
            }
        }

        public DataTable CreateColumnTable()
        {
            DataTable table = new DataTable();
            DataColumn col;

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "colorder";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "ColumnName";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "deText";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "TypeName";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "Length";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "Preci";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "Scale";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "IsIdentity";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "isPK";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "cisNull";
            table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = Type.GetType("System.String");
            col.ColumnName = "defaultVal";
            table.Columns.Add(col);
            return table;
        }
    }
}

