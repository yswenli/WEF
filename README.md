# WEF

this is database orm for C#. 

这是C #数据库ORM

WEF类似MEF上手简单，0学习成本。使用方便，按照sql书写习惯编写C#.NET代码

高性能，接近手写Sql

体积小（不到200kb，仅一个dll）

完美支持Sql Server(2000至最新版),MySql,Oracle,Access,Sqlite等数据库

支持大量Lambda表达式写法不需要像NHibernate的XML配置，不需要像MEF的各种数据库连接驱动

<h3>用法实例</h3>

db.From<Area>(tableName)    //Model.table1类通过<a href="https://github.com/yswenli/WEF/tree/master/WEF.ModelGenerator">WEF实体生成器生成</a>

    .Select(d => new { d.id, d.price })
	
        //Sql：SELECT id,price FROM table1
		
    //.Select<table2,table3>((a,b,c) => a.id, b.name, c.sex)
	
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


<h3>项目Console  </h3>

       static void Main(string[] args)
        {
            Console.WriteLine("WEF测试");

            Console.WriteLine("-----------------------------");

            string result = string.Empty;

            do
            {
                Test();
                Console.WriteLine("输入R继续,其他键退出");
                result = Console.ReadLine();
            }
            while (result.ToUpper() == "R");
        }


        static void Test()
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var fr = new AreaRepository();

            var a2 = new Area() { CName = "wenli" };

            Console.WriteLine("插入:{0}", a2.GetTableName());

            fr.Insert(a2);

            Console.ReadLine();

            var list = fr.GetList(1, 12);

            Console.WriteLine("查询:{0}", list.Count);
            Console.ReadLine();

            var sa2 = fr.GetSelectContext().Where(b => b.CName == "wenli").First();

            sa2.CName = "wenli520";

            fr.Update(sa2);

            var sa3 = fr.ExecuteSQL("select * from [Area] where [CName]='wenli520'").ToFirst<Area>();

            Console.WriteLine("更新:{0}", sa3.CName);

            Console.ReadLine();

            var r = fr.Delete(sa3);
            Console.WriteLine("移除:{0}", r);

            Console.ReadLine();

            fr = new AreaRepository();
            var flts = fr.GetList(2, 50);
            foreach (Area item in flts)
            {
                Console.WriteLine(string.Format("正在读取产品，code:{0},名称:{1}", item.EName, item.CName));
            }

            sw.Stop();
            Console.WriteLine(string.Format("处理完毕,用时：{0}毫秒", sw.Elapsed.TotalMilliseconds));
            Console.WriteLine("--------------");

            Console.ReadLine();

            sw.Restart();

            var flts2 = fr.GetSelectContext().Top(500).Page(10, 49).OrderBy(b => b.EName).ToList();
            foreach (Area item in flts2)
            {
                Console.WriteLine(string.Format("正在读取产品，code:{0},名称:{1}", item.CName, item.EName));
            }
            sw.Stop();
            Console.WriteLine(string.Format("处理完毕,用时：{0}毫秒", sw.Elapsed.TotalMilliseconds));

            Console.ReadLine();

            sw.Restart();
            var fn = fr.GetSelectContext().Where(b => b.ID == 10).First().CName;
            Console.WriteLine(fn);
            sw.Stop();
            Console.WriteLine(string.Format("处理完毕,用时：{0}毫秒", sw.Elapsed.TotalMilliseconds));

            var s = fr.ExecuteSQL("select * from [Area]").ToScalar();


            Console.WriteLine("-----------------------------");
        }
        
   
<h1>WEF实体代码生成器</h1>

使用实体代码生成器，简单高效完成实体类、实体数据库repository类
   
   
   <img src="https://github.com/yswenli/WEF/blob/master/WEF.ModelGenerator/0.jpg?raw=true">
   
   
   <img src="https://github.com/yswenli/WEF/blob/master/WEF.ModelGenerator/1.jpg?raw=true">
   
   
   <img src="https://github.com/yswenli/WEF/blob/master/WEF.ModelGenerator/2.jpg?raw=true">
   
   
   <img src="https://github.com/yswenli/WEF/blob/master/WEF.ModelGenerator/3.jpg?raw=true">
   
