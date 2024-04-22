using System;

using RiverLand.Common.Models.DataBase.TeJingCai;

using WEF.Common;
using WEF.Expressions;
using WEF.Test.Models;

namespace WEF.Standard.Test31
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var cnnstr = "Data Source=.;Initial Catalog=tejingcaiV2;Integrated Security=True";
            var dbarticleRepository = new DBArticleRepository(WEF.DatabaseType.SqlServer, cnnstr);

            var article = dbarticleRepository.Search().First();
            var articles = dbarticleRepository.Search().ToPagedList(1, 2, q => q.ID);
            dbarticleRepository.Update((q) => new { ID = "111", Content = "222" }, (q) => q.IsDeleted == false);



            #region 子查询

            var qs = dbarticleRepository.Search().Select(q => q.ID).Top();
            var sq = dbarticleRepository.Search().SubQuery(q => q.ID.SubQuery(qs, QueryOperator.Equal));
            var sqr = sq.ToList();

            var qs2 = dbarticleRepository.Search()
                .Join<DBUserInfo>((x, y) => x.CreatedBy == y.ID && y.IsDeleted == false && x.ID.In(sqr) && x.PraiseCount == 2, JoinType.LeftJoin)
                .Where(q => q.IsDeleted == false && q.Title == "12321" && q.ID.In(sqr))
                .Select(q => q.ID);
            var qs21 = qs2.ToPagedList(1, 10);
            var sq2 = dbarticleRepository.Search().Where(q => q.ID.SubQueryIn(qs2));
            var sqr2 = sq2.ToList();


            var qs3 = dbarticleRepository.Search().Exists<DBUserInfo>((x, y) => x.CreatedBy == y.ID);



            var subSearch = dbarticleRepository.Search().Select(q => q.ID).Top();
            var tr = dbarticleRepository.Search().Where(DBArticle._.ID.SubQueryEqual(subSearch)).First();


            var subSearch2 = dbarticleRepository.Search().Select(q => q.ID);
            var tr2 = dbarticleRepository.Search().Where(DBArticle._.ID.SubQueryIn(subSearch2)).First();


            var subSearch3 = dbarticleRepository.Search().Select(q => q.ID).Top();
            var subSearch31 = dbarticleRepository.Search().Select(q => q.ID);
            var tr3 = dbarticleRepository.Search().Where(q => q.ID.SubQuery(subSearch3, QueryOperator.Equal)).ToList();
            var tr31 = dbarticleRepository.Search().Where(q => q.ID.SubQueryIn(subSearch31)).ToList();
            var tr4 = dbarticleRepository.Search().Where(q => q.ID.SubQueryNotIn(subSearch31)).ToList();
            #endregion


            Console.WriteLine("Hello World!");
        }
    }
}
