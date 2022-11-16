/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF
*文件名： SearchT2
*版本号： V1.0.0.0
*唯一标识：8f7fc0aa-6742-4ed4-86c6-ed72a251f194
*当前的用户域：WALLE
*创建人： wenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/8/30 12:04:12
*描述：查询
*
*=================================================
*修改标记
*修改时间：2022/8/30 12:04:12
*修改人： yswen
*版本号： V1.0.0.0
*描述：查询
*
*****************************************************************************/
using System;
using System.Linq.Expressions;

using WEF.Db;
using WEF.Expressions;

namespace WEF
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class Search<T, T2> : Search<T>
        where T : Entity
        where T2 : Entity
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="database"></param>
        public Search(Database database) : base(database)
        {

        }

        /// <summary>
        /// Select
        /// </summary>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T, T2> Select(Expression<Func<T, T2, object>> lambdaSelect)
        {
            return (Search<T, T2>)AddSelect(ExpressionToOperation<T>.ToSelect(_tableName, lambdaSelect));
        }
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public class Search<T, T2, T3> : Search<T>
        where T : Entity
        where T2 : Entity
        where T3 : Entity
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="database"></param>
        public Search(Database database) : base(database)
        {

        }
        /// <summary>
        /// Select
        /// </summary>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T, T2, T3> Select(Expression<Func<T, T2, T3, object>> lambdaSelect)
        {
            return (Search<T, T2, T3>)AddSelect(ExpressionToOperation<T>.ToSelect(_tableName, lambdaSelect));
        }
    }
    /// <summary>
    /// 查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    public class Search<T, T2, T3, T4> : Search<T>
        where T : Entity
        where T2 : Entity
        where T3 : Entity
        where T4 : Entity
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="database"></param>
        public Search(Database database) : base(database)
        {

        }
        /// <summary>
        /// Select
        /// </summary>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T, T2, T3, T4> Select(Expression<Func<T, T2, T3, T4, object>> lambdaSelect)
        {
            return (Search<T, T2, T3, T4>)AddSelect(ExpressionToOperation<T>.ToSelect(_tableName, lambdaSelect));
        }
    }
    /// <summary>
    /// 查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    public class Search<T, T2, T3, T4, T5> : Search<T>
        where T : Entity
        where T2 : Entity
        where T3 : Entity
        where T4 : Entity
        where T5 : Entity
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="database"></param>
        public Search(Database database) : base(database)
        {

        }

        /// <summary>
        /// Select
        /// </summary>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T, T2, T3, T4, T5> Select(Expression<Func<T, T2, T3, T4, T5, object>> lambdaSelect)
        {
            return (Search<T, T2, T3, T4, T5>)AddSelect(ExpressionToOperation<T>.ToSelect(_tableName, lambdaSelect));
        }
    }
    /// <summary>
    /// 查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    public class Search<T, T2, T3, T4, T5, T6> : Search<T>
        where T : Entity
        where T2 : Entity
        where T3 : Entity
        where T4 : Entity
        where T5 : Entity
        where T6 : Entity
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="database"></param>
        public Search(Database database) : base(database)
        {

        }

        /// <summary>
        /// Select
        /// </summary>
        /// <param name="lambdaSelect"></param>
        /// <returns></returns>
        public Search<T, T2, T3, T4, T5, T6> Select(Expression<Func<T, T2, T3, T4, T5, T6, object>> lambdaSelect)
        {
            return (Search<T, T2, T3, T4, T5, T6>)AddSelect(ExpressionToOperation<T>.ToSelect(_tableName, lambdaSelect));
        }
    }
}
