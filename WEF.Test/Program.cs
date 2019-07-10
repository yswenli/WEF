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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WEF.Common;
using WEF.Expressions;
using WEF.Models;

namespace WEF.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            #region test1

            var mdb = new DBContext(DatabaseType.MySql, "server=172.31.32.83;user id=EMUser; password=20190415; Port=3306;database=emdb; pooling=false");
            var a = mdb.FromSql("select 1").ToScalar();

            #endregion


            var db = new DBContext();

            var dt = db.FromSql("select * from tb_task").ToDataTable();

            var dttasks = dt.DataTableToEntityList<DBTask>();

            var taskModel = new TaskModel()
            {
                Crc32 = 123,
                TaskID = Guid.NewGuid().ToString("N"),
                BeginTime = DateTime.Now,
                BusinID = "asdfead24545",
                CompanyId = "54",
                CreateTime = DateTime.Now,
                DayLimit = 10,
                DayTimes = 100,
                Description = "adsfevadsfasdfsadf",
                EndTime = DateTime.Now,
                FlowType = "5687653",
                IsDel = false,
                IsEnabled = true,
                Name = "afeadfad545646546",
                Operator = "5435635321",
                PlatformID = "8423416534635",
                Point = 2,
                TaskMaxPoint = 100,
                TaskType = 3
            };

            var tn = taskModel.ConvertTo<DBTask>();

            var taa = taskModel.ConvertTo<TestA>();

            var aa = new TestA()
            {
                aa = DateTime.Now,
                Age = 10,
                //Created = "2019-04-22 22:53",
                id = "100001",
                Num = 10.235M,
                Num1 = 100
            };

            var bb = aa.ConvertTo<TestB>();

            var cc = bb.ConvertTo<TestA>();

            Console.WriteLine("WEF使用实例");

            Console.WriteLine("-----------------------------");

            var giftopt = new DBGiftRepository();

            var giftwhere = giftopt.Search().Where(b => b.Giftid.Contains("1"));

            giftwhere = giftwhere.Where(b => !b.Isdel && b.Isenabled); //不能连接


            giftwhere = giftwhere.OrderBy(b => b.Createtime).OrderByDescending(b => b.Giftid);//不能连接

            var glist = giftwhere.ToList();

            var glist2 = giftopt.Search().Where(b => b.Giftid.Like("1")).ToList();

            var giftids = giftopt.Search().Page(1, 10).ToList().Select(b => b.Giftid).ToList();

            var glist22 = giftopt.Search().Where(b => b.Giftid.In(giftids)).ToList();

            var gids = new List<string>();

            gids.Add("120100010219094341");
            gids.Add("030000050310180911");
            gids.Add("201706260157165");
            gids.Add("201706300150728");

            var glist3 = giftopt.Search().Where(b => b.Giftid.In(gids)).ToList();


            #region where

            //不支持
            DBUserPointRepository tb_UserpointRepository = new DBUserPointRepository();

            var upWhere1 = tb_UserpointRepository.Search();

            upWhere1 = upWhere1.Where(b => b.Uid == "sss");

            upWhere1 = upWhere1.Where(b => b.Points > 0);

            var up1 = upWhere1.First();

            //支持
            Expression<Func<DBUserPoint, bool>> eWhere1 = null;

            if (true)
            {
                eWhere1 = (b => b.Uid.IsNotNull());
            }

            Expression<Func<DBUserPoint, bool>> eWhere2 = (b => b.Points > 0);

            if (true)
            {
                eWhere2 = (b => b.Points > 0);
            }

            Expression<Func<DBUserPoint, bool>> eWhere3 = null;

            if (false)
            {
                eWhere3 = (b => b.Uid.Contains("1"));
            }


            var upWhere2 = tb_UserpointRepository.Search().Where(eWhere1, eWhere2, eWhere3);

            var up2 = upWhere2.ToList();

            #endregion


            var plist = tb_UserpointRepository.GetList(1, 100);

            #region mysql

            DBTaskRepository repository = new DBTaskRepository();

            var task = repository.GetList(1, 10);

            //var taskModel = task.ConvertTo<TaskModel>();

            #endregion


            #region 无实体sql操作，自定义参数            

            var dt1 = DBContext.Default.FromSql("select * from tb_task where taskid=@taskID").AddInParameter("@taskID", System.Data.DbType.String, 200, "10B676E5BC852464DE0533C5610ACC53").ToFirst<DBTask>();

            var count = DBContext.Default.Search<DBTask>().Where(b => b.Crc32.Avg() > 1).Where(" 1=1 ").Count();

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



            var nut = ut.ConvertTo<SUser>();

            var nut1 = ut.ConvertTo<SUser>();

            var nnut = nut.ConvertTo<User>();

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


        class TestA
        {
            public DateTime aa { get; set; }

            public string id
            {
                get; set;
            }

            public int? Age
            {
                get; set;
            }

            public string Created
            {
                get; set;
            }

            public decimal? Num
            {
                get; set;
            }

            public int Num1
            {
                get; set;
            }
        }

        class TestB
        {
            public int ID
            {
                get; set;
            }

            public string age
            {
                get; set;
            }

            public DateTime? created
            {
                get; set;
            }

            public int bb { get; set; }

            public int? Num
            {
                get; set;
            }

            public decimal? Num1
            {
                get; set;
            }
        }
    }
}
