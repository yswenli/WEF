using System;

namespace WEF.Standard.DevelopTools.Model
{
    public class ConnectionModel
    {
        private Guid id = Guid.Empty;
        public Guid ID { get { return id; } set { id = value; } }

        private string name;
        public string Name { get { return name; } set { name = value; } }

        private string database;
        public string Database { get { return database; } set { database = value; } }

        private string dbType;
        public string DbType { get { return dbType; } set { dbType = value; } }

        private string connectionString;
        public string ConnectionString { get { return connectionString; } set { connectionString = value; } }

        public string TableName { get; set; }

        public bool IsView { get; set; }

        public string Sql { get; set; }
    }
}
