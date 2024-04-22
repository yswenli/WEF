/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
*创建人： yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：fc2b3c60-82bd-4265-bf8c-051e512a1035
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************/
using System;

using WEF.Common;
using WEF.Db;
using WEF.Expressions;

namespace WEF.Provider
{

    /// <summary>
    /// Sql Server 2005以上
    /// </summary>
    public class SqlServer9Provider : SqlServerProvider
    {
        public SqlServer9Provider(string connectionString)
            : base(connectionString)
        {
            this.DatabaseType = DatabaseType.SqlServer9;
        }


        /// <summary>
        /// 创建分页查询
        /// </summary>
        /// <param name="fromSection"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public override Search CreatePageFromSection(Search fromSection, int startIndex, int endIndex)
        {
            Check.Require(startIndex, "startIndex", Check.GreaterThanOrEqual<int>(1));
            Check.Require(endIndex, "endIndex", Check.GreaterThanOrEqual<int>(1));
            Check.Require(startIndex <= endIndex, "startIndex must be less than endIndex!");
            Check.Require(fromSection, "fromSection", Check.NotNullOrEmpty);

            if (startIndex == 1 && endIndex == 1)
            {
                return base.CreatePageFromSection(fromSection, startIndex, endIndex);
            }

            if (OrderByOperation.IsNullOrEmpty(fromSection.OrderByClip))
            {
                foreach (Field f in fromSection.Fields)
                {
                    if (!f.PropertyName.Equals("*"))
                    {
                        fromSection.OrderBy(f.Asc);
                        break;
                    }
                }
            }

            Check.Require(!OrderByOperation.IsNullOrEmpty(fromSection.OrderByClip), "query.OrderByClip could not be null or empty!");

            if (fromSection.Fields.Count == 0)
            {
                fromSection.Select(Field.All);
            }

            fromSection.AddSelect(new Field(string.Concat("row_number() over(", fromSection.OrderByString, ") AS tmp_rowid")));
            fromSection.OrderBy(OrderByOperation.None);
            fromSection.TableName = string.Concat("(", fromSection.SqlString, ") AS tmp_table");
            fromSection.Parameters = fromSection.Parameters;
            fromSection.DistinctString = string.Empty;
            fromSection.PrefixString = string.Empty;
            fromSection.GroupBy(GroupByOperation.None);
            fromSection.Select(Field.All);
            fromSection.Where(new WhereExpression(string.Concat("tmp_rowid BETWEEN ", startIndex.ToString(), " AND ", endIndex.ToString())), true);
            fromSection.Join(JoinOn.None);
            return fromSection;
        }
    }
}
