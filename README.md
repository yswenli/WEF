# WEF

WEF is based on the c # data entity framework supports MSQSqlServer, MySql, Orcalce, etc of conventional database and fast development, which integrates a large amount of data set under the development experience of tools, such as the Lambada without SQL query expression, add and delete, entity cloning, bulk and the parameters of the table, transaction, round of entities or stored procedures, SQL entities, etc.

WEF 是基于C#的数据实体框架，支持MSQSqlServer、MySql、Orcalce等等常规的数据库的快捷开发，其中集成了大量数据开发经验下的工具类集合，比如Lambada表达式查询、无sql的增删改查、实体克隆、批量、多表、事务、参数、SQL转实体或存储过程转实体等。

[![GitHub release](https://img.shields.io/github/release/yswenli/wef.svg)](https://github.com/yswenli/wef/releases)
[![NuGet version (SAEA)](https://img.shields.io/nuget/v/WEF.Standard.svg?style=flat-square)](https://www.nuget.org/packages?q=WEF.Standard)
[![License](https://img.shields.io/badge/license-Apache%202-4EB1BA.svg)](https://www.apache.org/licenses/LICENSE-2.0.html)

WEF类似MEF上手简单，0学习成本。使用方便，按照sql书写习惯编写C#.NET代码

高性能，接近手写Sql

体积小

完美支持Sql Server(2000至最新版),MySql,Oracle,Access,Sqlite等数据库

支持大量Lambda表达式写法不需要像NHibernate的XML配置，不需要像MEF的各种数据库连接驱动

```
dotnet add package WEF.Standard --version 6.24.7.1
dotnet add package WEF.Standard.MySQL --version 6.24.7.1
```

## 查询简例

```CSharp


db.Search<Area>()    //Model.table1类通过<a href="https://github.com/yswenli/WEF/tree/master/WEF.ModelGenerator">WEF数据库工具生成</a>

    .Select(d => new { d.id, d.price })
	
        //Sql：SELECT id,price FROM table1
		
    .Select<table2,table3>((a,b,c) => a.id, b.name, c.sex)
	
        //Sql：SELECT table1.id, table2.name, table3.sex
		
    .LeftJoin<table2>((a, b) => a.id == b.id)
	
        //Sql：LEFT JOIN Table2 ON table1.id = table2.id
		
    .Where(d => (d.id != 2 && d.name.In("com","net")) || d.sex != null)   
	
        //Sql：WHERE (id <> 2 AND name IN('com','net')) OR sex IS NOT NULL
		
    .GroupBy(d => new { d.name, d.sex })

        //Sql：GROUP BY name,sex
	
    .OrderBy(d => new { d.createTime, d.name })
	
        //Sql：ORDER BY createTime,name
		
    .Having(d => d.name != '')    
    
    //Sql：HAVING name <> ''
	
    .Top(5)    
    
    //取前5条数据
	
    .Page(10, 2)    
    
    //每页10条数据，取第2页
	
    .ToList();    
    
    //默认返回List<table1>，也可自定义Map类.ToList<T>();



```

## 多where条件拼接

```CSharp

            
    //Where条件拼接一：
    var dbTestRepository = new DBTestRepository();

    var where1 = new Where<DBTest>();
    where1.And(d => d.Operator != "");
    where1.And(d => d.Totallimit >= 0);

    var list1 = dbTestRepository.Search()
                    .Where(where1)
                    .Page(1, 2)
                    .ToList();


    //多表条件拼接
    var where2 = new Where<table>();
    where2.And(a => a.id == 1);
    where2.And<table2>((a, b) => b.id == 2);
    where2.And<table3>((a, c) => c.id == 3);

    var list2 = new DBContext().Search<table>()
                    .InnerJoin<table2>((a, b) => a.id == b.aid)
                    .InnerJoin<table3>((a, c) => a.id == c.aid)
                    .Where(where1)
                    .ToList();

    //上面的where还可以这样写：
    var where3 = new Where<table>();
    where3.And<table2, table3>((a, b, c) => a.id == 1 && b.id == 2 && c.id == 3);

```

## 执行sql

```CSharp
    #region 无实体sql操作，自定义参数            

    var dt1 = new DBContext().FromSql("select * from tb_task where taskid=@taskID").AddInParameter("@taskID", System.Data.DbType.String, 200, "10B676E5BC852464DE0533C5610ACC53").ToFirst<DBTask>();

    var count = new DBContext().Search<DBTask>().Where(b => b.Crc32.Avg() > 1).Count();

    var dt2=new DBContext().ToList<DBTask>("select * from tb_task");

    #endregion

```

## 聚合函数及Select

```CSharp
var count = giftopt.Search().Count(q => q.Supservicesku);

var sum = giftopt.Search().Select(b => b.Supporttype.Sum()).ToFirstDefault().Supporttype;

var avg = giftopt.Search().Select(b => b.Supporttype.Avg()).ToFirstDefault().Supporttype;

var select = giftopt.Search().LeftJoin<DBTask>((m, n) => m.Name == n.Name).Select<DBTask>((a, b) => new { a.Activename, b.Daylimit });

var select2 = giftopt.Search().LeftJoin2<DBTask>((m, n) => m.Name == n.Name).Select((a, b) => new { a.Activename, b.Daylimit });

//多表多关系的join
var giftopt = new DBGiftRepository(DatabaseType.SqlServer, "");
var select3 = giftopt.Search()
    .Join<DBTask>((m, n) => m.Name == n.Name, JoinType.InnerJoin)
    .Join<DBTask, DBUserPoint>((m, n) => m.Name == n.Uid, JoinType.LeftJoin)
    .Select<DBTask>((a, b) => new { a.Activename, b.Daylimit });
```
//多表join并多表取值分页示例
```CSharp
Repository.Search().Join<DBNotificationSetting>((x, y) => x.SettingID == y.ID, WEF.Common.JoinType.LeftJoin)
        .Where<DBNotificationSetting>((x, y) => x.ReceiverID == userId && x.IsDeleted != true && y.BusinessType == businessType && y.IsDeleted != true)
        .Select<DBNotificationSetting>((x, y) => new
        {
            ID = x.ID,
            Content = x.Content,
            Created = x.Created,
            UnRead = x.UnRead,
            Key = y.Key,
            BusinessType = y.BusinessType,
            Type = y.Type,
            Name = y.Name,
            Icon = y.Icon,
            BtnUrl = y.BtnUrl,
            BtnText = y.BtnText,
            Sender = x.Sender,
            SenderName = x.SenderName,
            SenderGender = x.SenderGender
        }).ToPagedList<NotificationListItem>(pageIndex, pageSize, orderBy, asc);
```
//多表聚合分组示例
```CSharp
Repository
    .Search()
    .LeftJoin<DBNotificationSetting>((x, y) => x.SettingID == y.ID)
    .Where(q => q.ReceiverID == userId && q.IsDeleted != true && q.UnRead == true)
    .GroupBy<DBNotificationSetting>((x, y) => y.BusinessType)
    .Select<DBNotificationSetting>((x, y) => new { BusinessType = y.BusinessType, Count = x.ID.Count() })
    .ToList<UnReadCountResult>();
```
## 事务

```CSharp
var repository = new DBOcWarehouseAreaRepository(DatabaseType.MySql, cnnStr);

using (var tran = repository.CreateTransaction())
{
     tran.Insert(area);
}

//或者
repository.CreateTransaction().TryCommit((tran)=>{
  //todo
});
            
```

## 获取多表集合
```CSharp
var tuple = new DBArticleRepository(WEF.DatabaseType.SqlServer, "Data Source=localhost;Initial Catalog=template;User Id=testuser;Password=testuser")
    .FromSql("select top 10 * from Article;select top 10 * from Comment")
    .ToMultipleList<DBArticle, DBComment>();

List<DBArticle> articleList = tuple.Item1;
List<DBComment> commentList = tuple.Item2;
```
## WEF数据库工具

WEF数据库工具是基于WEF的winform项目，可以快捷对数据库进行可视化操作的同时，高效生成基于WEF的ORM操作
   
   
   <img src="https://github.com/yswenli/WEF/blob/master/1.png?raw=true">
   
   
   <img src="https://github.com/yswenli/WEF/blob/master/2.png?raw=true">
   
   
   <img src="https://github.com/yswenli/WEF/blob/master/3.png?raw=true">
   
   
   <img src="https://github.com/yswenli/WEF/blob/master/4.png?raw=true">


   <img src="https://github.com/yswenli/WEF/blob/master/5.png?raw=true">


   <img src="https://github.com/yswenli/WEF/blob/master/6.png?raw=true">


   <img src="https://github.com/yswenli/WEF/blob/master/7.png?raw=true">


## WEF使用实例

```CSharp
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

using System.Data;
using System.Linq.Expressions;

using WEF.Common;
using WEF.Db;
using WEF.Expressions;
using WEF.Models;
using WEF.Standard.Test.Models;
using WEF.Test;
using WEF.Test.Models;

namespace WEF.Standard.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //MySqlBitTypeTest.Test1();
            ////MySqlBitTypeTest.Test3();
            //var nt1 = NotifyTest.GetList("76cf552c2b6f4dc6810994c4c015e2c8", "pro", 2, 1, 10);
            //var nt2 = NotifyTest.GetList("76cf552c2b6f4dc6810994c4c015e2c8", "pro", 2, 2, 10);
            //var nt3 = NotifyTest.GetList("76cf552c2b6f4dc6810994c4c015e2c8", "pro", 2, 3, 10);
            //var nt4 = NotifyTest.GetList("76cf552c2b6f4dc6810994c4c015e2c8", "pro", 2, 4, 10);


            List<DBArticle> articleList;
            List<DBComment> commentList;

            var cnnstr = "";

            #region get and update

            var dBCommentRepository = new DBCommentRepository(DatabaseType.SqlServer9, cnnstr);
            var dbclist = dBCommentRepository.Search()
                    .Where(q => q.IsDeleted != true)
                    .OrderBy(q => q.Created)
                    .Page(1, 10)
                    .ToList();

            var dbcommandResult = dBCommentRepository.Update(dbclist);

            #endregion


            #region 行转列测试

            //行转列测试
            var fdr = new DBFormdataRepository(WEF.DatabaseType.SqlServer9, cnnstr);


            var pivotInfo = new PivotInfo<DBFormdata>()
            {
                GroupBys = q => new { q.BatchNo, q.TemplateID, q.IsDeleted },
                ColumnNames = new List<string>() { "WorkNum", "TaskType", "FailReason" },
                TypeFieldName = q => q.FieldName,
                ValueFieldName = q => q.Value,
                WhereLambada = q => q.IsDeleted != true && q.BatchNo == "f465450ae51e4f2fa83b2a678e6804ea"
            };
            var pivotList1 = fdr.ToPivotList<DBFormdata, PivotObject>(pivotInfo,
                q => q.FailReason == "拒接",
                1,
                1000,
                q => q.WorkNum);


            //不指定查询字段和返回值
            var pivotInfo2 = new PivotInfo<DBFormdata>()
            {
                GroupBys = q => new { q.BatchNo, q.TemplateID, q.IsDeleted },
                TypeFieldName = q => q.FieldName,
                ValueFieldName = q => q.Value,
                WhereLambada = q => q.IsDeleted != true && q.BatchNo == "f465450ae51e4f2fa83b2a678e6804ea"
            };
            var pivotList2 = fdr.ToPivotList(pivotInfo2);


            //不指定查询字段和返回值，指定where
            var pivotInfo3 = new PivotInfo<DBFormdata>()
            {
                GroupBys = q => new { q.BatchNo, q.TemplateID, q.IsDeleted },
                TypeFieldName = q => q.FieldName,
                ValueFieldName = q => q.Value,
                WhereLambada = q => q.IsDeleted != true && q.BatchNo == "f465450ae51e4f2fa83b2a678e6804ea"
            };
            var pivotList3 = fdr.ToPivotList(pivotInfo2,
                new WhereExpression("ID=@ID", new Parameter("@ID", "123456", DbType.String, 50)));

            #endregion


            var dbarticleRepository = new DBArticleRepository(WEF.DatabaseType.SqlServer, cnnstr);

            var article = dbarticleRepository.Search().First();
            var articles = dbarticleRepository.Search().ToPagedList(1, 2, q => q.ID);
            dbarticleRepository.Update((q) => new { ID = "111", Content = "222" }, (q) => q.IsDeleted == false);



            #region 子查询

            var qs = dbarticleRepository.Search().Select(q => q.ID).Top();
            var sq = dbarticleRepository.Search().SubQuery(q => q.ID.SubQuery(qs, QueryOperator.Equal));
            var sqr = sq.ToList();

            var qs2 = dbarticleRepository.Search().Select(q => q.ID);
            var sq2 = dbarticleRepository.Search().SubQuery(q => q.ID.SubQueryIn(qs2));
            var sqr2 = sq.ToList();



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



            //转换字典
            var adic1 = dbarticleRepository.Search().ToDictionary(q => q.ID);
            var adic2 = dbarticleRepository.Search().ToDictionary(q => q.ID, q => q.Title);
            var adic3 = dbarticleRepository.Search().ToDictionary<string, string>(q => q.ID, q => q.Title);


            var tuple = dbarticleRepository.FromSql("select top 10 * from Article;select top 10 * from Comment")
                .ToMultipleList<DBArticle, DBComment>();

            articleList = tuple.Item1;
            commentList = tuple.Item2;

            var dic1111 = new Dictionary<string, dynamic>();

            var articlePagedList = dbarticleRepository.FromSql("select * from Article", 1, 10, "ID", true, dic1111)
                .ToPagedList<DBArticle>();

            var articlePagedList2 = dbarticleRepository.Search().ToPagedList(1, 10, "ID", true);

            int praiseCount = 12321;
            var yid = "abc";

            var aWhere = new Where<DBArticle>(x => x.Status == 1 && (x.IsDeleted.IsNull() || x.IsDeleted == false));
            aWhere.And<DBComment>((x, y) => y.PraiseCount == praiseCount && y.ID == yid);

            var aSection = dbarticleRepository.Search()
                .Join<DBComment>((x, y) => y.PageID == x.ID, JoinType.LeftJoin)
                .Join<DBComment>((x, y) => y.PageID == x.ID, JoinType.LeftJoin);

            aSection = aSection.Where(aWhere);

            aSection = aSection.Where(q => q.PublishDate > DateTime.Now);

            aSection = aSection.Select<DBComment>((x, y) => new { x.All, y.ID });
            var asList = aSection.ToPagedList<DBArticle>(1, 20, "Article.ID", true);

            var uja = dbarticleRepository.Update(new DBArticle() { ID = "1" },
                LeftJoinOn.Add<DBArticle, DBComment>((x, y) => x.ID == y.PageID),
                (x, y) => x.IsDeleted != true && y.IsDeleted != true && x.ID.IsNull());

            var articlePagedList3 = aSection.ToList();


            new BytesTest().Test2();

            BatchTest.Test();

            //MutiTablesTest.Test();

            //BatchTest2.Test();

            //new DBTicketOrderRepository().Search().Where(b => b.C_id == "123sdf4asdfsadfgrewfdg5498432165").OrderBy(b=>b.C_price).ToFirst();

            //Test4();

            //Test3();

            Console.ReadLine();

            var c_id = Guid.NewGuid().ToString("N");

            var dbtickerOrder = new DBTicketOrder()
            {
                C_id = c_id,
                C_supplier = 1,
                C_appId = "s",
                C_channelId = "22",
                C_sku = "sss",
                C_outTradeNo = "afefa",
                C_count = 1,
                C_phone = "1111",
                C_nonce = "asfew",
                C_timestamp = "asdfeaf",
                C_sign = "afefqwaf",
                C_amount = 19.80M,
                C_discountrate = 90F,
                C_activityName = "asdfasd",
                C_productName = "asdfed",
                C_resv1 = "hello",
                C_created = DateTime.Now
            };


            var c_price = (decimal)(((float)dbtickerOrder.C_amount) * dbtickerOrder.C_discountrate / 100F);

            dbtickerOrder.C_price = c_price;

            var ticketOrderRepository = new DBTicketOrderRepository();

            if (ticketOrderRepository.Insert(dbtickerOrder) > 0)
            {
                Console.WriteLine($"DBTicketOrder数据插入成功,{dbtickerOrder.C_price}");

                var newdbtickerOrder = ticketOrderRepository.GetDBTicketOrder(dbtickerOrder.C_id);

                Console.WriteLine($"DBTicketOrder数据查询成功,{newdbtickerOrder.C_price}");

                newdbtickerOrder.C_updated = DateTime.Now;

                newdbtickerOrder.C_resv1 = null;

                if (new DBTicketOrderRepository().Update(newdbtickerOrder) > 0)
                {
                    var newdbtickerOrder2 = ticketOrderRepository.GetDBTicketOrder(c_id);

                    Console.WriteLine($"DBTicketOrder数据查询成功,{newdbtickerOrder2.C_price}");
                }
            }

            ticketOrderRepository.Delete(c_id);

            Console.ReadLine();


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
                Description = "𠂆𠂆𠂆𠂆𠂆𠂆𠂆𠂆",
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

            var insertResult = new DBTaskRepository().Insert(tn);

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



            var tasks = new DBTaskRepository().Search().ToList();

            DBUserTaskRepository userTaskRepository = null;

            var ts1 = Task.Factory.StartNew(() =>
            {
                userTaskRepository = new DBUserTaskRepository();
            });

            Task.WaitAll(ts1);

            var useTasks1 = userTaskRepository.Search().Where(b => b.Isenabled).ToList();

            var useTasks2 = userTaskRepository.Search().Where(b => b.Userid == "6312124585351742" && b.Isenabled).ToList();


            for (int i = 0; i < 10000; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        userTaskRepository.Search().First();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
            }


            Console.ReadLine();

            new DBTaskRepository().Delete("");

            var giftopt = new DBGiftRepository(DatabaseType.SqlServer, "");

            var giftwhere = giftopt.Search().Where(b => b.Giftid.Contains("1"));

            giftwhere = giftwhere.Where(b => !b.Isdel && b.Isenabled); //不能连接


            giftwhere = giftwhere.OrderBy(b => b.Createtime).OrderByDescending(b => b.Giftid);//不能连接

            var glist = giftwhere.ToList();

            var glist2 = giftopt.Search().Where(b => b.Giftid.Like("1") && b.Isdel.IsNotNull()).ToList();

            var giftids = giftopt.Search().Page(1, 10).ToList().Select(b => b.Giftid).ToList();

            var count = giftopt.Search().Count(q => q.Supservicesku);

            var sum = giftopt.Search().Select(b => b.Supporttype.Sum()).ToFirstDefault().Supporttype;

            var avg = giftopt.Search().Select(b => b.Supporttype.Avg()).ToFirstDefault().Supporttype;

            var select = giftopt.Search().LeftJoin<DBTask>((m, n) => m.Name == n.Name).Select<DBTask>((a, b) => new { a.Activename, b.Daylimit });

            var select2 = giftopt.Search().LeftJoin2<DBTask>((m, n) => m.Name == n.Name).Select((a, b) => new { a.Activename, b.Daylimit });

            //不同表的join
            var select3 = giftopt.Search()
                .Join<DBTask>((m, n) => m.Name == n.Name, JoinType.InnerJoin)
                .Join<DBTask, DBUserPoint>((m, n) => m.Name == n.Uid, JoinType.LeftJoin)
                .Select<DBTask>((a, b) => new { a.Activename, b.Daylimit });

            var glist22 = giftopt.Search().Where(b => b.Giftid.In(giftids)).ToList();

            var gids = new List<string>();

            gids.Add("120100010219094341");
            gids.Add("030000050310180911");
            gids.Add("201706260157165");
            gids.Add("201706300150728");

            var glist3 = giftopt.Search().Where(b => b.Giftid.In(gids)).ToList();

            //多表join并多表取值分页示例：
            //Repository.Search().Join<DBNotificationSetting>((x, y) => x.SettingID == y.ID, WEF.Common.JoinType.LeftJoin)
            //        .Where<DBNotificationSetting>((x, y) => x.ReceiverID == userId && x.IsDeleted != true && y.BusinessType == businessType && y.IsDeleted != true)
            //        .Select<DBNotificationSetting>((x, y) => new
            //        {
            //            ID = x.ID,
            //            Content = x.Content,
            //            Created = x.Created,
            //            UnRead = x.UnRead,
            //            Key = y.Key,
            //            BusinessType = y.BusinessType,
            //            Type = y.Type,
            //            Name = y.Name,
            //            Icon = y.Icon,
            //            BtnUrl = y.BtnUrl,
            //            BtnText = y.BtnText,
            //            Sender = x.Sender,
            //            SenderName = x.SenderName,
            //            SenderGender = x.SenderGender
            //        }).ToPagedList<NotificationListItem>(pageIndex, pageSize, orderBy, asc);

            //多表聚合取值示例：

            new DBNotificationRepository()
                .Search()
                .LeftJoin<DBNotificationSetting>((x, y) => x.SettingID == y.ID)
                .Where(q => q.ReceiverID == "" && q.IsDeleted != true && q.UnRead == true)
                .GroupBy<DBNotificationSetting>((x, y) => y.BusinessType)
                .Select<DBNotificationSetting>((x, y) => new { BusinessType = y.BusinessType, Count = x.ID.Count() })
                .ToList();

            var articleGroupList = new DBArticleRepository(WEF.DatabaseType.SqlServer, cnnstr)
                .Search()
                .GroupBy(q => new { q.ID, q.Status, q.Created })
                .OrderBy(q => q.Created.Max())
                .OrderBy(q => new { q.ID, q.Status })
                .OrderByDescending(q => q.CreatedBy.Max())
                .Select(q => new { q.ID, q.Status, q.Created })
                .Where(q => q.ID != null)
                .Where(q => q.Status == 1)
                .ToList();

            #region where

            //不支持
            DBUserPointRepository tb_UserpointRepository = new DBUserPointRepository();

            var upWhere1 = tb_UserpointRepository.Search();

            var w1 = upWhere1.Where(b => b.Uid == "sss");

            var w2 = w1.Where(b => b.Points > 0);

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


            var upWhere2 = tb_UserpointRepository.Search().Where(eWhere1, eWhere2, eWhere3).ToList();


            //tb_UserpointRepository.Search().OrderBy()


            //Where条件拼接一：

            var dbTaskRepository = new DBTaskRepository();

            var where1 = new Where<DBTask>();
            where1.And(d => d.Operator != "");
            where1.And(d => d.Totallimit >= 0);

            var list1 = dbTaskRepository.Search()
                            .Where(where1)
                            .Page(1, 2)
                            .ToList();

            var list2 = dbTaskRepository.Search()
                            .Where(where1)
                            .Page(2, 2)
                            .ToList();

            where1.And<DBGift>((m, n) => m.Name.Like(n.Name));

            var list3 = dbTaskRepository.Search().LeftJoin<DBGift>((x, y) => x.Taskbuttontext == y.Supservicename)
                .Where(where1)
                .ToPagedList<User>(1, 10, "Name", true);

            //where条件拼接二：
            var where11 = new Where<DBTask>();
            where11.And(where1);


            //多表条件拼接

            //var where2 = new Where<table>();
            //where2.And(a => a.id == 1);
            //where2.And<table2>((a, b) => b.id == 2);
            //where2.And<table3>((a, c) => c.id == 3);

            //var list2 = new DBContext().Search<table>()
            //                .InnerJoin<table2>((a, b) => a.id == b.aid)
            //                .InnerJoin<table3>((a, c) => a.id == c.aid)
            //                .Where(where1)
            //                .ToList();

            //上面的where还可以这样写：
            //var where3 = new Where<table>();
            //where3.And<table2, table3>((a, b, c) => a.id == 1 && b.id == 2 && c.id == 3);

            #endregion





            var plist = tb_UserpointRepository.GetList(1, 100);

            #region mysql

            DBTaskRepository repository = new DBTaskRepository();

            var task = repository.GetList(1, 10);

            //var taskModel = task.ConvertTo<TaskModel>();

            #endregion


            #region 无实体sql操作，自定义参数            

            var dt1 = new DBContext().FromSql("select * from tb_task where taskid=@taskID").AddInParameter("@taskID", System.Data.DbType.String, 200, "10B676E5BC852464DE0533C5610ACC53").ToFirst<DBTask>();

            var count1 = new DBContext().Search<DBTask>().Where(b => b.Crc32.Avg() > 1).Where(" 1=1 ").Count();

            var dt2 = new DBContext().FromSql("select * from tb_task where taskid=@taskID").AddInParameter("@taskID", "10B676E5BC852464DE0533C5610ACC53").ToFirst<DBTask>();

            var count2 = new DBContext().Search<DBTask>().Where(b => b.Crc32.Avg() > 1).Where(" 1=1 ").Count();

            var dt3 = new DBContext().FromSql("select * from tb_task where taskid=@taskID").AddInParameterWithModel(new { taskID = "10B676E5BC852464DE0533C5610ACC53" }).ToFirst<DBTask>();

            var count3 = new DBContext().Search<DBTask>().Where(b => b.Crc32.Avg() > 1).Where(" 1=1 ").Count();

            #endregion


            string result = string.Empty;

            var entity = new ArticleKind();

            var entityRepository = new ArticleKindRepository();

            var pagedList = entityRepository.Search(entity).ToPagedList(1, 100, "ID", true);

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


            using (var batch = ur.DBContext.CreateBatch<User>())
            {
                batch.Insert(ut);
            }

            var nut = ut.ConvertTo<SUser>();

            var nut1 = ut.ConvertTo<SUser>();

            var nnut = nut.ConvertTo<User>();

            var ults = ur.GetList(1, 1000);

            r = ur.Delete(ut);



            #region tran

            var tran1 = ur.DBContext.BeginTransaction<User>();

            try
            {
                tran1.Insert(ut);

                var tb1 = new DBTaskRepository().GetList(1, 10);

                tran1.Update<DBTask>(tb1);
            }
            catch
            {
                tran1.Rollback();
            }
            finally
            {
                ur.DBContext.CloseTransaction(tran1);
            }

            //or
            using (var tran2 = ur.DBContext.BeginTransaction<User>())
            {
                tran2.Insert(ut);

                new DBTaskRepository().DBContext.Delete(tran2, new DBTask() { Taskid = "123" });

                tran2.Delete(ut);
            }

            //or
            ur.DBContext.BeginTransaction<User>().TryCommit((tran3) =>
            {

                tran3.Insert(ut);

                tran3.Delete(new DBTask() { Taskid = "123" });

                tran3.Delete(ut);
            });

            #endregion


            var dlts = ur.GetList(1, 10000);
            ur.Deletes(dlts);

        }


        public class TestA
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

        public class TestB
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


        static void Test3()
        {
            var list1 = new DBTaskRepository().Search().ToPagedList(1, 20, "begintime", true).Data.Select(b => b.Begintime).ToList();

            var list2 = new DBTaskRepository().Search().ToPagedList(1, 20, "begintime", false).Data.Select(b => b.Begintime).ToList();

            if (list1.First() == list2.First())
            {

            }
        }

        static void Test4()
        {
            UsersRepository usersRepository = new UsersRepository(DatabaseType.PostgreSQL, "PORT=5432;DATABASE=test;HOST=127.0.0.1;PASSWORD=yswenli;USER ID=postgres;");

            var id = Guid.NewGuid().GetHashCode();

            var r = usersRepository.Insert(new Users()
            {
                Id = id,
                Name = "chewang",
                Pwd = "12321",
                Created = DateTime.Now
            });

            var e = usersRepository.GetUsers(id);

            e.Name = "Chewang";

            usersRepository.Update(e);


            var l = usersRepository.Search().Where(b => b.Id > 0).OrderBy(b => b.Created).Page(1, 10).ToList();

            Console.ReadLine();

        }


    }

}

```

