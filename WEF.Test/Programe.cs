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
using System.Diagnostics;
using System.Linq;
using System.Text;
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
    }
}
