using System.Diagnostics;

using WEF.Standard.Mongo;
using WEF.Standard.Mongo.Test.Model;

public class Program
{
    static void Main(string[] args)
    {
        Console.Title = "WEF.Standard.Mongo.Test";

        var customerRepository = MongoDBFactory.Create<Account>();

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
            var customerRepository1 = MongoDBFactory.Create<Account>();

            var customerRepository2 = new BaseRepository<Account>(connectionString: "");

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

            customerRepository1.Add(account);

        });

        Console.WriteLine("Read");

        var list = customerRepository.Where(b => b.FirstName.Contains("l")).OrderBy(b => b.Orders).Skip(1).Take(10).ToList();

        var c = customerRepository.Where(b => b.FirstName.Contains("l")).FirstOrDefault();

        var count = customerRepository.Count();

        var c1 = customerRepository.GetById("5ccebadfb3b7bb38408bce24");

        Console.WriteLine("Update");

        c.FirstName = "li";

        customerRepository.Update(c);

        Console.WriteLine("Delete");

        customerRepository.Delete(c);

        customerRepository.DeleteAll();

        Console.ReadLine();
    }

}