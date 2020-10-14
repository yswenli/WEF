/****************************************************************************
*项目名称：WEF.Test
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：WEF.Test
*类 名 称：BatchTest
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/9/8 14:15:01
*描述：
*=====================================================================
*修改时间：2020/9/8 14:15:01
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WEF.Test.Models;

namespace WEF.Test
{
    class BatchTest
    {
        public static void Test()
        {
            Test1();

            //Test2();

        }

        public static void Test1()
        {
            Console.WriteLine("多次insert测试");

            var count = 100000;

            var list = new List<Rule>();

            for (int i = 0; i < count; i++)
            {
                list.Add(new Rule()
                {
                    Name = "test" + i,
                    Created = DateTime.Now,
                    Enabled = true,
                    Json = string.Empty,
                    RuleType = 1,
                    Score = 100,
                    Updated = DateTime.Now
                });
            }

            var rr = new RuleRepository();

            var ic = rr.ExecuteSQL("select count(id) from Rules").ToScalar<string>();

            rr.ExecuteSQL("TRUNCATE TABLE Rules").ToScalar();

            var sw = Stopwatch.StartNew();

            rr.DBContext.Insert(list);

            sw.Stop();

            Console.WriteLine("多次insert测试完成,cost:" + sw.ElapsedMilliseconds);

            list.Clear();

            for (int i = 1 * count; i < 2 * count; i++)
            {
                list.Add(new Rule()
                {
                    Name = "test" + i,
                    Created = DateTime.Now,
                    Enabled = true,
                    Json = string.Empty,
                    RuleType = 1,
                    Score = 100,
                    Updated = DateTime.Now
                });
            }

            Console.WriteLine("批量insert测试");

            sw = Stopwatch.StartNew();

            using (var batch = rr.DBContext.CreateBatch<Rule>())
            {
                batch.Insert(list);
            }

            sw.Stop();

            Console.WriteLine("批量insert测试完成,cost:" + sw.ElapsedMilliseconds);

            Console.ReadLine();

        }



        public static void Test2()
        {
            var sw = Stopwatch.StartNew();

            var list = new List<LiveLog>();

            for (int i = 0; i < 1000; i++)
            {
                list.Add(new LiveLog()
                {
                    ID = i,
                    Datetime = DateTime.Now
                });

            }

            var rr = new LiveLogRepository(DatabaseType.SqlServer9, "Data Source=.;Initial Catalog=student;Integrated Security=True");

            rr.DBContext.BulkInsert(list);

            sw.Stop();

            Console.WriteLine("批量insert测试完成,cost:" + sw.ElapsedMilliseconds);

            Console.ReadLine();
        }
    }
}
