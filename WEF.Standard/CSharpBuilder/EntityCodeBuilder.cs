/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.CSharpBuilder
*文件名： EntityCodeBuilder
*版本号： V1.0.0.0
*唯一标识：90cf894b-0d82-4d0d-9a84-c716220b6836
*当前的用户域：WALLE
*创建人： wenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/8/24 10:08:51
*描述：
*
*=================================================
*修改标记
*修改时间：2022/8/24 10:08:51
*修改人： yswen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;

using WEF.Common;

namespace WEF.CSharpBuilder
{
    /// <summary>
    /// 实体代码生成
    /// </summary>
    public class EntityCodeBuilder
    {
        private List<ColumnInfo> _columns = new List<ColumnInfo>();

        private string _tableName;

        private string _dbType;

        private string _nameSpace = "WEF.Models";

        private string _className;

        private bool _isView = false;

        private bool _isSZMDX = false;
        /// <summary>
        /// 实体代码生成
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="nameSpace"></param>
        /// <param name="className"></param>
        /// <param name="columns"></param>
        /// <param name="isView"></param>
        public EntityCodeBuilder(string tableName, string nameSpace, string className, List<ColumnInfo> columns, bool isView)
            : this(tableName, nameSpace, className, columns, isView, false)
        {

        }

        /// <summary>
        /// 实体代码生成
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="nameSpace"></param>
        /// <param name="className"></param>
        /// <param name="columns"></param>
        /// <param name="isView"></param>
        /// <param name="isSZMDX"></param>
        /// <param name="dbType"></param>
        public EntityCodeBuilder(string tableName, string nameSpace, string className, List<ColumnInfo> columns, bool isView, bool isSZMDX, string dbType = null)
        {
            _isSZMDX = isSZMDX;
            _className = StringPlus.ReplaceSpace(className);
            _nameSpace = StringPlus.ReplaceSpace(nameSpace);
            _tableName = tableName;
            _dbType = dbType;
            if (_isSZMDX)
            {
                _className = StringPlus.ToUpperFirstword(_className);
            }
            _isView = isView;



            foreach (ColumnInfo col in columns)
            {
                col.Name = StringPlus.ReplaceSpace(col.Name);
                if (_isSZMDX)
                {
                    col.Name = StringPlus.ToUpperFirstword(col.Name);
                }

                col.DeText = StringPlus.ReplaceSpace(col.DeText);
                _columns.Add(col);
            }

        }

        public List<ColumnInfo> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
        public string NameSpace
        {
            get { return _nameSpace; }
            set { _nameSpace = value; }
        }
        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }
        public string DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }
        public bool IsView
        {
            get { return _isView; }
            set { _isView = value; }
        }
        /// <summary>
        /// Build
        /// </summary>
        /// <param name="simple"></param>
        /// <returns></returns>
        public string Build(bool simple = false)
        {
            if (!simple)
            {
                return WholeBuilder();
            }
            else
            {
                return SimpleBuilder();
            }

        }

        /// <summary>
        /// 生成完整支持wef的实体类代码
        /// </summary>
        /// <returns></returns>
        string WholeBuilder()
        {
            Columns = DBToCSharp.DbtoCSColumns(Columns, DbType);

            StringPlus plus = new StringPlus();
            plus.AppendLine("//------------------------------------------------------------------------------");
            plus.AppendLine("// <WEF-ModelGenerator>");
            plus.AppendLine("//     此代码由" + System.Reflection.Assembly.GetExecutingAssembly().FullName + "生成;时间 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture));
            plus.AppendLine("//     运行时版本:" + Environment.Version.ToString());
            plus.AppendLine("//     不建议手动更改此代码，如有需要请自行扩展partial类");
            plus.AppendLine("// </WEF-ModelGenerator>");
            plus.AppendLine("//------------------------------------------------------------------------------");

            plus.AppendLine();
            plus.AppendLine();

            plus.AppendLine("using System.Data;");
            plus.AppendLine("using System.Runtime.Serialization;");
            plus.AppendLine("using WEF;");
            plus.AppendLine("using WEF.Common;");

            plus.AppendLine();
            plus.AppendLine("namespace " + NameSpace);
            plus.AppendLine("{");
            plus.AppendLine();
            plus.AppendSpaceLine(1, "/// <summary>");
            plus.AppendSpaceLine(1, "/// 实体类" + ClassName + "");
            plus.AppendSpaceLine(1, "/// </summary>");
            plus.AppendSpaceLine(1, $"[Serializable, DataContract, Table(\"{TableName}\")]");
            plus.AppendSpaceLine(1, "public partial class " + ClassName + " : Entity ");
            plus.AppendSpaceLine(1, "{");
            plus.AppendSpaceLine(2, "private static string m_tableName;");
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 实体类" + ClassName + "");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public " + ClassName + "():base(\"" + TableName + "\") {m_tableName=\"" + TableName + "\";}");
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 实体类" + ClassName + "");
            plus.AppendSpaceLine(2, "/// <param name=\"tableName\">表名</param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public " + ClassName + "(string tableName) : base(tableName) { m_tableName=tableName;}");
            plus.AppendLine();
            plus.AppendLine(BuilderModel());
            plus.AppendLine(BuilderMethod());
            plus.AppendSpaceLine(1, "}");

            var repositoryCs = BuilderRepository();

            if (string.IsNullOrEmpty(repositoryCs))
                return string.Empty;

            plus.AppendLine(repositoryCs);

            plus.AppendLine("}");
            plus.AppendLine("");
            return plus.ToString();
        }

        /// <summary>
        /// 简单生成贫血模式的实体类
        /// </summary>
        /// <returns></returns>
        string SimpleBuilder()
        {
            Columns = DBToCSharp.DbtoCSColumns(Columns, DbType);

            StringPlus plus = new StringPlus();
            plus.AppendLine("//------------------------------------------------------------------------------");
            plus.AppendLine("// <WEF-ModelGenerator>");
            plus.AppendLine("//     此代码由" + System.Reflection.Assembly.GetExecutingAssembly().FullName + "生成;时间 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture));
            plus.AppendLine("//     运行时版本:" + Environment.Version.ToString());
            plus.AppendLine("//     不建议手动更改此代码，如有需要请自行扩展partial类");
            plus.AppendLine("// </WEF-ModelGenerator>");
            plus.AppendLine("//------------------------------------------------------------------------------");

            plus.AppendLine();
            plus.AppendLine();


            plus.AppendLine("using System;");
            plus.AppendLine("using System.ComponentModel;");
            plus.AppendLine("using System.Runtime.Serialization;");

            plus.AppendLine();
            plus.AppendLine("namespace " + NameSpace);
            plus.AppendLine("{");
            plus.AppendLine();
            plus.AppendSpaceLine(1, "/// <summary>");
            plus.AppendSpaceLine(1, "/// 实体类" + ClassName + "");
            plus.AppendSpaceLine(1, "/// " + TableName + "");
            plus.AppendSpaceLine(1, "/// </summary>");
            plus.AppendSpaceLine(1, $"[Serializable, DataContract]");
            plus.AppendSpaceLine(1, "public partial class " + ClassName);
            plus.AppendSpaceLine(1, "{");
            plus.AppendLine();
            StringPlus plus1 = new StringPlus();
            StringPlus plus2 = new StringPlus();
            foreach (ColumnInfo column in Columns)
            {
                plus2.AppendSpaceLine(2, "/// <summary>");
                plus2.AppendSpaceLine(2, "/// " + column.Name + " " + column.DeText);
                plus2.AppendSpaceLine(2, "/// </summary>");
                plus2.AppendSpaceLine(2, "[DataMember]");
                plus2.AppendSpaceLine(2, "public " + column.DataTypeName + " " + column.Name);
                plus2.AppendSpaceLine(2, "{");
                plus2.AppendSpaceLine(3, "get;");
                plus2.AppendSpaceLine(3, "set;");
                plus2.AppendSpaceLine(2, "}");
            }
            plus1.Append(plus2.Value);
            plus.AppendLine(plus1.ToString());
            plus.AppendSpaceLine(1, "}");
            plus.AppendLine("}");
            plus.AppendLine("");
            return plus.ToString();
        }

        private string BuilderModel()
        {
            StringPlus plus = new StringPlus();
            StringPlus plus2 = new StringPlus();
            StringPlus plus3 = new StringPlus();
            plus.AppendSpaceLine(2, "#region Model");
            foreach (ColumnInfo column in Columns)
            {
                plus2.AppendSpaceLine(2, "private " + column.DataTypeName + " _" + column.Name + ";");
                plus3.AppendSpaceLine(2, "/// <summary>");
                plus3.AppendSpaceLine(2, "/// " + column.Name + " " + column.DeText);
                plus3.AppendSpaceLine(2, "/// </summary>");
                plus3.AppendSpaceLine(2, $"[DataMember, Description(\"{column.DeText}\")]");
                plus3.AppendSpaceLine(2, "public " + column.DataTypeName + " " + column.Name);
                plus3.AppendSpaceLine(2, "{");
                plus3.AppendSpaceLine(3, "get{ return _" + column.Name + "; }");
                plus3.AppendSpaceLine(3, "set");
                plus3.AppendSpaceLine(3, "{");
                plus3.AppendSpaceLine(4, "this.OnPropertyValueChange(_." + column.Name + ",_" + column.Name + ",value);");
                plus3.AppendSpaceLine(4, "this._" + column.Name + "=value;");
                plus3.AppendSpaceLine(3, "}");
                plus3.AppendSpaceLine(2, "}");
            }
            plus.Append(plus2.Value);
            plus.Append(plus3.Value);
            plus.AppendSpaceLine(2, "#endregion");

            return plus.ToString();
        }

        private string BuilderMethod()
        {
            if (Columns == null || Columns.Count < 1) return String.Empty;

            StringPlus plus = new StringPlus();

            plus.AppendSpaceLine(2, "#region Method");

            //只读
            if (IsView)
            {
                plus.AppendSpaceLine(2, "/// <summary>");
                plus.AppendSpaceLine(2, "/// 是否只读");
                plus.AppendSpaceLine(2, "/// </summary>");
                plus.AppendSpaceLine(2, "public override bool IsReadOnly()");
                plus.AppendSpaceLine(2, "{");
                plus.AppendSpaceLine(3, "return true;");
                plus.AppendSpaceLine(2, "}");
            }

            ColumnInfo identityColumn = Columns.Find(delegate (ColumnInfo col) { return col.IsIdentity; });
            if (null != identityColumn)
            {
                plus.AppendSpaceLine(2, "/// <summary>");
                plus.AppendSpaceLine(2, "/// 获取实体中的标识列");
                plus.AppendSpaceLine(2, "/// </summary>");
                plus.AppendSpaceLine(2, "public override Field GetIdentityField()");
                plus.AppendSpaceLine(2, "{");
                plus.AppendSpaceLine(3, "return _." + identityColumn.Name + ";");
                plus.AppendSpaceLine(2, "}");
            }

            List<ColumnInfo> primarykeyColumns = Columns.FindAll(delegate (ColumnInfo col) { return col.IsPrimaryKey; });
            if (null != primarykeyColumns && primarykeyColumns.Count > 0)
            {
                plus.AppendSpaceLine(2, "/// <summary>");
                plus.AppendSpaceLine(2, "/// 获取实体中的主键列");
                plus.AppendSpaceLine(2, "/// </summary>");
                plus.AppendSpaceLine(2, "public override Field[] GetPrimaryKeyFields()");
                plus.AppendSpaceLine(2, "{");
                plus.AppendSpaceLine(3, "return new Field[] {");
                StringPlus plus2 = new StringPlus();
                foreach (ColumnInfo col in primarykeyColumns)
                {
                    plus2.AppendSpaceLine(4, "_." + col.Name + ",");
                }
                plus.Append(plus2.ToString().TrimEnd().Substring(0, plus2.ToString().TrimEnd().Length - 1));
                plus.AppendLine("};");
                plus.AppendSpaceLine(2, "}");
            }

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获取列信息");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public override Field[] GetFields()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return new Field[] {");
            StringPlus plus3 = new StringPlus();
            foreach (ColumnInfo col in Columns)
            {
                plus3.AppendSpaceLine(4, "_." + col.Name + ",");
            }
            plus.Append(plus3.ToString().TrimEnd().Substring(0, plus3.ToString().TrimEnd().Length - 1));
            plus.AppendLine("};");
            plus.AppendSpaceLine(2, "}");


            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获取值信息");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public override object[] GetValues()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return new object[] {");
            StringPlus plus4 = new StringPlus();
            foreach (ColumnInfo col in Columns)
            {
                plus4.AppendSpaceLine(4, "this._" + col.Name + ",");
            }
            plus.Append(plus4.ToString().TrimEnd().Substring(0, plus4.ToString().TrimEnd().Length - 1));
            plus.AppendLine("};");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "#endregion");
            plus.AppendLine();

            plus.AppendSpaceLine(2, "#region _Field");
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 字段信息");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public class _");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "/// <summary>");
            plus.AppendSpaceLine(3, "/// " + TableName + " ");
            plus.AppendSpaceLine(3, "/// </summary>");
            plus.AppendSpaceLine(3, "public readonly static Field All = new Field(\"*\",m_tableName);");
            foreach (ColumnInfo col in Columns)
            {
                plus.AppendSpaceLine(3, "/// <summary>");
                plus.AppendSpaceLine(3, "/// " + col.Name + " " + col.DeText);
                plus.AppendSpaceLine(3, "/// </summary>");

                switch (col.DataTypeName)
                {
                    case "bool":
                    case "bool?":
                        plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName," + "DbType.Boolean,1" + ",\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
                        break;
                    case "byte":
                    case "byte?":
                        plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName," + "DbType.Byte,1" + ",\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
                        break;
                    case "short":
                    case "short?":
                        plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName," + "DbType.Int16,1" + ",\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
                        break;
                    case "int":
                    case "int?":
                        plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName," + "DbType.Int32,1" + ",\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
                        break;
                    case "long":
                    case "long?":
                        plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName," + "DbType.Int64,1" + ",\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
                        break;
                    case "float":
                    case "float?":
                        plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName," + "DbType.Single,1" + ",\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
                        break;
                    case "double":
                    case "double?":
                        plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName," + "DbType.Double,1" + ",\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
                        break;
                    case "decimal":
                    case "decimal?":
                        plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName," + "DbType.Decimal,1" + ",\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
                        break;
                    case "Enum":
                    case "Enum?":
                        plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName," + "DbType.Decimal,1" + ",\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
                        break;
                    case "DateTime":
                    case "DateTime?":
                        plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName," + "DbType.DateTime,1" + ",\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
                        break;
                    case "Guid":
                    case "Guid?":
                        plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName," + "DbType.Guid,1" + ",\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
                        break;
                    case "byte[]":
                    case "byte[]?":
                        plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName," + "DbType.Binary, " + col.MaxLength + ",\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
                        break;
                    case "object":
                    case "string":
                    default:
                        plus.AppendSpaceLine(3, "public readonly static Field " + col.Name + " = new Field(\"" + col.ColumnNameRealName + "\",m_tableName,\"" + (string.IsNullOrEmpty(col.DeText) ? col.ColumnNameRealName : col.DeText) + "\");");
                        break;
                }
            }
            plus.AppendSpaceLine(2, "}");
            plus.AppendSpaceLine(2, "#endregion");
            plus.AppendLine();

            return plus.ToString();


        }

        private string BuilderRepository()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(1, "/// <summary>");
            plus.AppendSpaceLine(1, "/// 实体类" + ClassName + "操作类");
            plus.AppendSpaceLine(1, "/// </summary>");
            //plus.AppendSpaceLine(1, "public partial class " + ClassName + "Repository: IRepository<" + ClassName + ">");
            plus.AppendSpaceLine(1, "public partial class " + ClassName + "Repository: BaseRepository<" + ClassName + ">");
            plus.AppendSpaceLine(1, "{");
            //plus.AppendSpaceLine(2, "DBContext db;");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, $"/// {ClassName}构造方法");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public " + ClassName + "Repository() : base()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "_db = new DBContext();");
            plus.AppendSpaceLine(2, "}");


            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, $"/// {ClassName}构造方法");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public " + ClassName + "Repository(DBContext dbContext) : base(dbContext)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "_db = dbContext;");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, $"/// {ClassName}构造方法");
            plus.AppendSpaceLine(2, "/// <param name=\"connStrName\">连接字符串中的名称</param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public " + ClassName + "Repository(string connStrName) : base(connStrName)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "_db = new DBContext(connStrName);");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, $"/// {ClassName}构造方法");
            plus.AppendSpaceLine(2, "/// <param name=\"dbType\">数据库类型</param>");
            plus.AppendSpaceLine(2, "/// <param name=\"connStr\">连接字符串</param>");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public " + ClassName + "Repository(DatabaseType dbType, string connStr) : base(dbType, connStr)");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "_db = new DBContext(dbType, connStr);");
            plus.AppendSpaceLine(2, "}");

            plus.AppendSpaceLine(1, "}");
            return plus.ToString();
        }

    }
}
