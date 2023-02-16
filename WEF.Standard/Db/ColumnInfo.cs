namespace WEF.Db
{
    /// <summary>
    /// 列描述信息
    /// </summary>
    public sealed class ColumnInfo
    {
        public string Colorder { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Ordinal { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }

        public string ColumnNameRealName { get; set; }

        /// <summary>
        /// 是否可空
        /// </summary>
        public bool AllowDBNull { get; set; }
        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaxLength { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataTypeName { get; set; }
        /// <summary>
        /// 是否自增长
        /// </summary>
        public bool AutoIncrement { get; set; }

        public bool IsIdentity { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        /// <summary>
        /// 是否唯一
        /// </summary>
        public bool Unique { get; set; }
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly { get; set; }


        public string Preci { get; set; }

        public string Scale { get; set; }


        public string DeText { get; set; }

        public string DefaultVal { get; set; }
    }
}
