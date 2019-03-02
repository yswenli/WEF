# WEF

WEF is database orm for C#. 

WEF 是C#数据库ORM

[![GitHub release](https://img.shields.io/github/release/yswenli/wef.svg)](https://github.com/yswenli/wef/releases)

WEF类似MEF上手简单，0学习成本。使用方便，按照sql书写习惯编写C#.NET代码

高性能，接近手写Sql

体积小（不到200kb，仅一个dll）

完美支持Sql Server(2000至最新版),MySql,Oracle,Access,Sqlite等数据库

支持大量Lambda表达式写法不需要像NHibernate的XML配置，不需要像MEF的各种数据库连接驱动

## 用法实例

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

## WEF数据库工具

WEF数据库工具是基于WEF的winform项目，可以快捷对数据库进行可视化操作的同时，高效生成基于WEF的ORM操作
   
   
   <img src="https://github.com/yswenli/WEF/blob/master/1.png?raw=true">
   
   
   <img src="https://github.com/yswenli/WEF/blob/master/2.png?raw=true">
   
   
   <img src="https://github.com/yswenli/WEF/blob/master/3.png?raw=true">
   
   
   <img src="https://github.com/yswenli/WEF/blob/master/4.png?raw=true">


   <img src="https://github.com/yswenli/WEF/blob/master/5.png?raw=true">


   <img src="https://github.com/yswenli/WEF/blob/master/6.png?raw=true">


   <img src="https://github.com/yswenli/WEF/blob/master/7.png?raw=true">


## WEF数据库工具生成代码测试

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




            var nut = ut.ConvertTo<User, SUser>();

            var nnut = nut.ConvertTo<SUser, User>();

            var ults = ur.GetList(1, 1000);

            r = ur.Delete(ut);



            var dlts = ur.GetList(1, 10000);
            ur.Deletes(dlts);

        }
    }
}



```
   
授权

WEF 采用LGPL开源协议：

如果您不对 WEF 程序代码进行任何修改，直接调用组件，可以以任意方式自由使用：开源、非开源、商业及非商业。

如果您对 WEF 程序代码进行任何的修改或者衍生，涉及修改部分的额外代码和衍生的代码都必须采用 LGPL 协议开放源代码。

无论您对 WEF 程序代码如何修改，都必须在程序文件头部声明版权信息的注释（包括压缩版）

Open Source (OSI) Logo

LGPL协议原文：GNU Lesser General Public License

商业授权

您可以将 WEF 程序直接使用在自己的商业或者非商业网站或者软件产品中。

您可以对 WEF 进行修改和美化，可以去除 WEF 版权注释或改变程序名称，无需公开您修改或美化过的 WEF 程序与界面。

商业授权每个公司只需要购买一次，而不限制产品域名。适用于 WEF 现有版本和所有后续版本，永久有效。

您享有反映和提出意见的优先权，相关意见将被作为首要考虑。

商业授权全文 | 授权价格：99元 | 支付方式：支付宝

联系Email/支付宝帐号: wenguoli_520@hotmail.com

