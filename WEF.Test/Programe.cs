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
            Console.WriteLine("WEF使用实例");

            Console.WriteLine("-----------------------------");




            #region mysql

            DBTaskRepository repository = new DBTaskRepository();

            var task = repository.GetList(1, 10);

            var taskModel = task.ConvertTo<DBTask, TaskModel>();

            #endregion


            #region 无实体sql操作，自定义参数

            DBContext dbContext = new DBContext();

            var dt1 = dbContext.FromSql("select * from tb_task where taskid=@taskID").AddInParameter("@taskID", System.Data.DbType.String, 200, "10B676E5BC852464DE0533C5610ACC53").ToFirst<DBTask>();

            dbContext.Search<DBTask>().Sum();

            //dbContext.ExecuteNonQuery("");            

            //dbContext.FromSql("").ToList<DBTask>();

            #endregion


            string result = string.Empty;

            var entity = new Models.ArticleKind();

            var entityRepository = new Models.ArticleKindRepository();

            var pagedList = entityRepository.Search(entity).GetPagedList(1, 100, "ID", true);

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

            //ut.ConvertTo

            r = ur.Update(ut);

            #region search 

            var search = ur.Search().Where(b => b.NickName.Like("张*"));

            search = search.Where(b => !string.IsNullOrEmpty(b.ImUserID));

            var rlts = search.Page(1, 20).ToList();

            #endregion


            var batch = ur.DBContext.BeginBatchConnection();

            batch.Insert<User>(ut);

            batch.Execute();



            var nut = ut.ConvertTo<User, SUser>();

            var nut1 = ut.ConvertTo<User, SUser>();

            var nnut = nut.ConvertTo<SUser, User>();

            var ults = ur.GetList(1, 1000);

            r = ur.Delete(ut);



            #region tran

            var tran = ur.DBContext.BeginTransaction();

            tran.Insert<User>(ut);

            var tb1 = new DBTaskRepository().GetList(1, 10);

            //todo tb1

            tran.Update<DBTask>(tb1);

            ur.DBContext.CloseTransaction(tran);

            #endregion

            var dlts = ur.GetList(1, 10000);
            ur.Deletes(dlts);

        }
    }
}
