/*
* 描述： 详细描述类能干什么
* 创建人：wenli
* 创建时间：2017/3/2 14:26:21
*/
/*
*修改人：wenli
*修改时间：2017/3/2 14:26:21
*修改内容：xxxxxxx
*/

using System;
using WEF.Expressions;
using WEF.Models;

namespace WEF.Test
{
    class Programe
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WEF测试");

            Console.WriteLine("-----------------------------");

            string result = string.Empty;

            var entity = new Models.ArticleKind();

            var entityRepository=new Models.ArticleKindRepository();

            var pagedList= entityRepository.Search(entity).GetPagedList(1, 100, "ID", true);

            do
            {
                Test2();

                Console.WriteLine("输入R继续,其他键退出");
                result = Console.ReadLine();
            }
            while (result.ToUpper() == "R");
        }

        static void Test2()
        {

            UserRepository ur = new UserRepository();

            var e = ur.Search().Where(b => b.NickName == "adsfasdfasdf").First();

            var ut = new User()
            {
                ID = Guid.NewGuid(),
                ImUserID = "",
                NickName = "张三三"
            };

            var r = ur.Insert(ut);

            var count = ur.Search().Count();

            ut.NickName = "李四四";

            r = ur.Update(ut);

            #region search 1

            Where<User> wults = new Where<User>();

            wults.And(new WhereClip(ut.GetFields()[0], "", QueryOperator.Less));

            wults.And(new WhereClip(ut.GetFields()[1], 2, QueryOperator.Like));

            var rlts = ur.Search().Where(wults).ToList();

            #endregion


            #region search 2

            var search = ur.Search().Where(b => b.NickName.Like("张*"));

            search = search.Where(b => !string.IsNullOrEmpty(b.ImUserID));

            rlts = search.Page(1, 20).ToList();

            #endregion


            var batch = ur.DBContext.BeginBatchConnection();

            batch.Insert<User>(ut);

            batch.Execute();



            var nut = ut.ConvertTo<User, SUser>();

            var nnut = nut.ConvertTo<SUser, User>();

            var ults = ur.GetList(1, 1000);

            r = ur.Delete(ut);



            var dlts = ur.GetList(1, 10000);
            ur.Deletes(dlts);

        }
    }
}
