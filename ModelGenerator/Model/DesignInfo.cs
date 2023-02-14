/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.ModelGenerator.Model
*文件名： DesignInfo
*版本号： V1.0.0.0
*唯一标识：b113da9a-66a9-49f0-afb4-0a711a0d051c
*当前的用户域：WALLE
*创建人： yswen
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/6/30 14:56:45
*描述：
*
*=================================================
*修改标记
*修改时间：2022/6/30 14:56:45
*修改人： yswenli
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEF.ModelGenerator.Model
{
    public class DesignInfo
    {
        public DatabaseType DatabaseType { get; set; }

        public string DatabaseName { get; set; }

        public string TableName { get; set; }

        public List<DBColumnInfo> ColumnInfos { get; set; }
    }

    public class DBColumnInfo
    {
        public string NameColumn { get; set; }

        public DBDataType TypeColumn { get; set; }

        public int LengthColumn { get; set; }

        public bool KeyColumn { get; set; }

        public bool NotNullColumn { get; set; }

        public bool AutoIncrementColumn { get; set; }

        public string InfoColumn { get; set; }

    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DBDataType
    {
        NVarChar = 0,
        DateTime = 1,
        Bit = 2,
        Int = 3,
        Float = 4,
        Double = 5,
        Money = 6,
        Image = 7
    }

    public static class DBDataTypeExtend
    {
        public static DBDataType ToDBDataType(this string str)
        {
            return (DBDataType)Enum.Parse(typeof(DBDataType), str, true);
        }
    }
}
