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
            Console.WriteLine("多次insert测试");

            var count = 10000;

            var list = new List<Rule>();

            for (int i = 2 * count; i < 3 * count; i++)
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

            var sw = Stopwatch.StartNew();

            rr.DBContext.Insert(list);

            sw.Stop();

            Console.WriteLine("多次insert测试完成,cost:" + sw.ElapsedMilliseconds);

            list.Clear();
            list = new List<Rule>();

            for (int i = 3 * count; i < count * 4; i++)
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

            var batch = rr.DBContext.CreateBatch();

            batch.InsertList(list);

            batch.Execute();

            sw.Stop();

            Console.WriteLine("批量insert测试完成,cost:" + sw.ElapsedMilliseconds);

            Console.ReadLine();

        }
    }
}
