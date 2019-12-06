using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WEF.NoSql;
using WEF.NoSqlTest.Model;

namespace WEF.NoSqlTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "MongoDBOperator.Test";

            MongoDBFactory.OnDisconnected += MongoDBFactory_OnDisconnected;

            MongoDBFactory.OnError += MongoDBFactory_OnError;

            var customerOperator = MongoDBFactory.Create<Account>();

            #region test

            var total = 1000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < total; i++)
            {
                MongoDBFactory.Create<Account>().Where(b => b.FirstName == "li9").FirstOrDefault();
            }

            stopwatch.Stop();

            Console.WriteLine($"mongodb 查询速度：{total * 1000 / stopwatch.ElapsedMilliseconds} 次/秒");
            Console.ReadLine();
            #endregion




            Parallel.For(0, 10, i =>
            {
                var customerOperator1 = MongoDBFactory.Create<Account>();

                var account = new Account();
                account.FirstName = "li" + i;
                account.LastName = "wen";
                account.Phone = "13800138000";
                account.Email = "wenguoli_520@qq.com";
                account.Created = DateTime.Now;
                account.HomeAddress = new Address
                {
                    Address1 = "上海",
                    Address2 = "徐汇",
                    PostCode = "210001",
                    City = "上海",
                    Country = "中国"
                };

                Console.WriteLine("Create");

                customerOperator1.Add(account);


            });

            Console.WriteLine("Read");

            var list = customerOperator.Where(b => b.FirstName.Contains("l")).OrderBy(b => b.Orders).Skip(1).Take(10).ToList();

            var c = customerOperator.Where(b => b.FirstName.Contains("l")).FirstOrDefault();

            var count = customerOperator.Count();

            var c1 = customerOperator.GetById("5ccebadfb3b7bb38408bce24");

            Console.WriteLine("Update");

            c.FirstName = "guo li";

            customerOperator.Update(c);

            Console.WriteLine("Delete");

            customerOperator.Delete(c);

            customerOperator.DeleteAll();

            Console.ReadLine();
        }

        private static void MongoDBFactory_OnError(string settingInfo, Exception ex)
        {
            Console.WriteLine($"settingInfo:{settingInfo} \r\nex:{ex.Message}");
        }

        private static void MongoDBFactory_OnDisconnected(string settingInfo)
        {
            Console.WriteLine($"settingInfo:{settingInfo}");
        }
    }
}
