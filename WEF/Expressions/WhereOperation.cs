/*****************************************************************************************************
 * 本代码版权归Wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * 所属域：WENLI-PC
 * 登录用户：yswenli
 * CLR版本：4.0.30319.17929
 * 唯一标识：9a4fe848-95cb-4ad2-ac1b-d757a6ea1cd0
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 命名空间：WEF.Expressions
 * 类名称：WhereClip
 * 文件名：WhereClip
 * 创建年份：2015
 * 创建时间：2015-09-29 16:35:12
 * 创建人：Wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using WEF.Common;

namespace WEF.Expressions
{
    /// <summary>
    /// 条件
    /// </summary>
    [Serializable]
    public class WhereOperation : Expression
    {
        /// <summary>
        /// All
        /// </summary>
        public readonly static WhereOperation All = new WhereOperation();

        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        public WhereOperation() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        public WhereOperation(string where)
            : base(where)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customWhereString"></param>
        /// <param name="parameters"></param>
        public WhereOperation(string customWhereString, params Parameter[] parameters)
            : base(customWhereString, parameters)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="oper"></param>
        public WhereOperation(Field field, object value, QueryOperator oper)
            : base(field, value, oper)
        {

        }


        #endregion

        #region 属性



        /// <summary>
        /// 返回  where
        /// </summary>
        public string Where
        {
            get
            {
                return this.ToString();
            }
        }


        /// <summary>
        /// WhereString    
        /// <example>
        /// where 1=1
        /// </example>
        /// </summary>
        public string WhereString
        {
            get
            {
                if (string.IsNullOrEmpty(this.expressionString))
                    return string.Empty;

                return string.Concat(" WHERE ", this.expressionString);
            }
        }


        #endregion

        #region 方法

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="whereString"></param>
        /// <returns></returns>
        public static implicit operator WhereOperation(string whereString)
        {
            return new WhereOperation(whereString);
        }


        /// <summary>
        /// 判断条件是否一样
        /// </summary>
        /// <param name="leftWhere"></param>
        /// <param name="rightWhere"></param>
        /// <returns></returns>
        public static bool Equals(WhereOperation leftWhere, WhereOperation rightWhere)
        {
            string leftWhereString = leftWhere.ToString();
            string rightWhereString = rightWhere.ToString();

            foreach (Parameter p in leftWhere.parameters)
            {
                leftWhereString.Replace(p.ParameterName, (p.ParameterValue == null) ? string.Empty : p.ParameterValue.ToString());
            }

            foreach (Parameter p in rightWhere.parameters)
            {
                rightWhereString.Replace(p.ParameterName, (p.ParameterValue == null) ? string.Empty : p.ParameterValue.ToString());
            }

            return (string.Compare(leftWhereString, rightWhereString, true) == 0);
        }




        /// <summary>
        /// 判断 WhereClip  是否为null
        /// </summary>
        /// <param name="whereClip"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(WhereOperation whereClip)
        {
            if ((null == whereClip) || string.IsNullOrEmpty(whereClip.expressionString))
                return true;
            return false;
        }


        public override string ToString()
        {
            if (string.IsNullOrEmpty(expressionString))
                return string.Empty;

            return string.Concat("(", expressionString, ")");
        }


        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;
            else if (obj is WhereOperation)
            {
                return obj.ToString().Equals(this.ToString());
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// And
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public WhereOperation And(WhereOperation where)
        {
            if (WhereOperation.IsNullOrEmpty(this) && WhereOperation.IsNullOrEmpty(where))
                return All;

            if (WhereOperation.IsNullOrEmpty(where))
                return this;
            if (WhereOperation.IsNullOrEmpty(this))
                return where;



            WhereOperation andwhere = new WhereOperation(string.Concat(this.Where, " AND ", where.Where));
            andwhere.parameters.AddRange(this.Parameters);
            andwhere.parameters.AddRange(where.Parameters);


            return andwhere;
        }

        /// <summary>
        /// Or
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public WhereOperation Or(WhereOperation where)
        {
            if (WhereOperation.IsNullOrEmpty(this) && WhereOperation.IsNullOrEmpty(where))
                return All;

            if (WhereOperation.IsNullOrEmpty(where))
                return this;
            if (WhereOperation.IsNullOrEmpty(this))
                return where;

            WhereOperation orwhere = new WhereOperation(string.Concat(this.Where, " OR ", where.Where));
            orwhere.parameters.AddRange(this.Parameters);
            orwhere.parameters.AddRange(where.Parameters);


            return orwhere;
        }


        #region 重载操作符


        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator true(WhereOperation right)
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator false(WhereOperation right)
        {
            return false;
        }



        /// <summary>
        /// And
        /// </summary>
        /// <param name="leftWhere"></param>
        /// <param name="rightWhere"></param>
        /// <returns></returns>
        public static WhereOperation operator &(WhereOperation leftWhere, WhereOperation rightWhere)
        {
            if (WhereOperation.IsNullOrEmpty(leftWhere))
                return rightWhere;

            return leftWhere.And(rightWhere);
        }

        /// <summary>
        /// Or
        /// </summary>
        /// <param name="leftWhere"></param>
        /// <param name="rightWhere"></param>
        /// <returns></returns>
        public static WhereOperation operator |(WhereOperation leftWhere, WhereOperation rightWhere)
        {
            if (WhereOperation.IsNullOrEmpty(leftWhere))
                return rightWhere;

            return leftWhere.Or(rightWhere);
        }

        /// <summary>
        /// not
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public static WhereOperation operator !(WhereOperation where)
        {
            if (IsNullOrEmpty(where))
            {
                return All;
            }
            return new WhereOperation(string.Concat(" NOT ", where.expressionString), where.parameters.ToArray());
        }


        /// <summary>
        /// EXISTS
        /// </summary>
        /// <param name="fromSection"></param>
        /// <returns></returns>
        public static WhereOperation Exists(Search fromSection)
        {
            return new WhereOperation(string.Concat(" EXISTS (", fromSection.SqlString, ") "), fromSection.Parameters.ToArray());
        }

        #endregion



        #endregion



    }
}
