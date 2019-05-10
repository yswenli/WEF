using System;
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

            var customerOperator = MongoDBFactory.Create<Account>();

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

            var list = customerOperator.Where(b => b.FirstName.Contains("l")).Skip(1).Take(10).OrderBy(b => b.Orders).ToList();

            var c = customerOperator.Where(b => b.FirstName.Contains("l")).FirstOrDefault();

            var c1 = customerOperator.GetById("5ccebadfb3b7bb38408bce24");

            Console.WriteLine("Update");

            c.FirstName = "guo li";

            customerOperator.Update(c);

            Console.WriteLine("Delete");

            customerOperator.Delete(c);

            customerOperator.DeleteAll();

            Console.ReadLine();
        }
    }
}
