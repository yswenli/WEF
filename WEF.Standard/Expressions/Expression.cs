/*****************************************************************************************************
 * 本代码版权归@wenli所有，All Rights Reserved (C) 2015-2022
 *****************************************************************************************************
 * CLR版本：4.0.30319.42000
 * 唯一标识：c631644d-ae98-49d3-97c6-20e2411f1569
 * 机器名称：WENLI-PC
 * 联系人邮箱：wenguoli_520@qq.com
 *****************************************************************************************************
 * 项目名称：$projectname$
 * 命名空间：WEF.Expressions
 * 类名称：Expression
 * 创建时间：2017/7/26 14:38:47
 * 创建人：wenli
 * 创建说明：
 *****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;

using WEF.Common;

namespace WEF.Expressions
{
    public enum WhereType
    {
        /// <summary>
        /// join where
        /// </summary>
        JoinWhere,
        /// <summary>
        /// 常规Where
        /// </summary>
        Where
    }

    /// <summary>
    /// 比较类型
    /// </summary>
    public enum QueryOperator : byte
    {
        /// <summary>
        /// ==
        /// </summary>
        Equal,

        /// <summary>
        /// &lt;&gt; 、 !=、不等于
        /// </summary>
        NotEqual,

        /// <summary>
        /// >
        /// </summary>
        Greater,

        /// <summary>
        /// &lt; 小于
        /// </summary>
        Less,

        /// <summary>
        /// >=
        /// </summary>
        GreaterOrEqual,

        /// <summary>
        /// &lt;= 小于等于
        /// </summary>
        LessOrEqual,

        /// <summary>
        /// LIKE
        /// </summary>
        Like,

        /// <summary>
        /// &
        /// </summary>
        BitwiseAND,

        /// <summary>
        /// |
        /// </summary>
        BitwiseOR,

        /// <summary>
        /// ^
        /// </summary>
        BitwiseXOR,

        /// <summary>
        /// ~
        /// </summary>
        BitwiseNOT,

        /// <summary>
        /// IS NULL
        /// </summary>
        IsNULL,

        /// <summary>
        /// IS NOT NULL
        /// </summary>
        IsNotNULL,

        /// <summary>
        ///  +
        /// </summary>
        Add,

        /// <summary>
        /// -
        /// </summary>
        Subtract,


        /// <summary>
        /// *
        /// </summary>
        Multiply,

        /// <summary>
        /// /
        /// </summary>
        Divide,

        /// <summary>
        /// %
        /// </summary>
        Modulo,
    }

    /// <summary>
    /// 参数
    /// </summary>
    [Serializable]
    public class Parameter
    {
        private string parameterName;
        private object parameterValue;
        private DbType parameterDbType;
        private int parameterSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        //public Parameter(string parameterName, object parameterValue) : this(parameterName, parameterValue, null, null) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <param name="parameterDbType"></param>
        /// <param name="parameterSize"></param>
        public Parameter(string parameterName, object parameterValue, DbType parameterDbType, int parameterSize)
        {
            this.parameterName = parameterName;
            this.parameterValue = parameterValue;
            this.parameterDbType = parameterDbType;
            this.parameterSize = parameterSize;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName
        {
            get
            {
                return parameterName;
            }
            set
            {
                parameterName = value;
            }
        }


        /// <summary>
        /// 参数值
        /// </summary>
        public object ParameterValue
        {
            get
            {
                return parameterValue;
            }
            set
            {
                parameterValue = value;
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public DbType ParameterDbType
        {
            get
            {
                return parameterDbType;
            }
            set
            {
                parameterDbType = value;
            }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public int ParameterSize
        {
            get
            {
                return parameterSize;
            }
            set
            {
                parameterSize = value;
            }
        }

    }




    /// <summary>
    /// 表达式
    /// </summary>
    [Serializable]
    public class Expression
    {
        /// <summary>
        /// 
        /// </summary>
        public string _expressionString = string.Empty;


        /// <summary>
        /// 参数
        /// </summary>
        protected List<Parameter> _parameters = new List<Parameter>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public Expression()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="expressionString"></param>
        public Expression(string expressionString)
        {
            this._expressionString = expressionString;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="expressionString"></param>
        /// <param name="parameters"></param>
        public Expression(string expressionString, params Parameter[] parameters)
        {
            if (!string.IsNullOrEmpty(expressionString))
            {
                this._expressionString = expressionString;

                if (null != parameters && parameters.Length > 0)
                    this._parameters.AddRange(parameters);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="oper"></param>
        public Expression(Field field, object value, QueryOperator oper)
            : this(field, value, oper, true)
        {

        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="oper"></param>
        /// <param name="isFieldBefore"></param>
        public Expression(Field field, object value, QueryOperator oper, bool isFieldBefore)
        {
            if (!Field.IsNullOrEmpty(field))
            {
                string valuestring = null;
                if (value is Expression)
                {
                    Expression expression = (Expression)value;
                    valuestring = expression.ToString();
                    _parameters.AddRange(expression.Parameters);
                }
                else if (value is Field)
                {
                    Field fieldValue = (Field)value;
                    valuestring = fieldValue.TableFieldName;
                }
                else
                {
                    valuestring = DataUtils.MakeUniqueKey(field);
                    var p = new Parameter(valuestring, value, field.ParameterDbType, field.ParameterSize);
                    _parameters.Add(p);
                }

                if (isFieldBefore)
                {
                    this._expressionString = string.Concat(field.TableFieldName, DataUtils.ToString(oper), valuestring);
                }
                else
                {
                    this._expressionString = string.Concat(valuestring, DataUtils.ToString(oper), field.TableFieldName);
                }
            }
        }

        /// <summary>
        /// 返回参数
        /// </summary>
        internal List<Parameter> Parameters
        {
            get
            {
                return _parameters;
            }
        }


        /// <summary>
        /// 返回组合字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _expressionString;
        }

    }
}
