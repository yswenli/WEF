# WEF

WEF is based on the c # data entity framework supports MSQSqlServer, MySql, Orcalce, etc of conventional database and fast development, which integrates a large amount of data set under the development experience of tools, such as the Lambada without SQL query expression, add and delete, entity cloning, bulk and the parameters of the table, transaction, round of entities or stored procedures, SQL entities, etc.

WEF 是基于C#的数据实体框架，支持MSQSqlServer、MySql、Orcalce等等常规的数据库的快捷开发，其中集成了大量数据开发经验下的工具类集合，比如Lambada表达式查询、无sql的增删改查、实体克隆、批量、多表、事务、参数、SQL转实体或存储过程转实体等。

[![GitHub release](https://img.shields.io/github/release/yswenli/wef.svg)](https://github.com/yswenli/wef/releases)

WEF类似MEF上手简单，0学习成本。使用方便，按照sql书写习惯编写C#.NET代码

高性能，接近手写Sql

体积小（不到200kb，仅一个dll）

完美支持Sql Server(2000至最新版),MySql,Oracle,Access,Sqlite等数据库

支持大量Lambda表达式写法不需要像NHibernate的XML配置，不需要像MEF的各种数据库连接驱动

## 查询简例

```CSharp

db.Search<Area>(tableName)    //Model.table1类通过<a href="https://github.com/yswenli/WEF/tree/master/WEF.ModelGenerator">WEF数据库工具生成</a>

    .Select(d => new { d.id, d.price })
	
        //Sql：SELECT id,price FROM table1
		
    .Select<table2,table3>((a,b,c) => a.id, b.name, c.sex)
	
        //Sql：SELECT table1.id, table2.name, table3.sex
		
    .LeftJoin<table2>((a, b) => a.id == b.id)
	
        //Sql：LEFT JOIN Table2 ON table1.id = table2.id
		
    .Where(d => (d.id != 2 && d.name.In("com","net")) || d.sex != null)   
	
        //Sql：WHERE (id <> 2 AND name IN('com','net')) OR sex IS NOT NULL
		
    .GroupBy(d => new { d.name, d.sex })    //Sql：GROUP BY name,sex
	
    .OrderBy(d => new { d.createTime, d.name })
	
        //Sql：ORDER BY createTime,name
		
    .Having(d => d.name != '')    //Sql：HAVING name <> ''
	
    .Top(5)    //取前5条数据
	
    .Page(10, 2)    //每页10条数据，取第2页
	
    .ToList();    //默认返回List<table1>，也可自定义Map类.ToList<T>();

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


```

