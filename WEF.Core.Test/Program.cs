using System;

using WEF.Common;
using WEF.Core.Test.Models;
using WEF.Expressions;
using WEF.Models;

namespace WEF.Core.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "WEF.Core.Test";

            //var userInfoRepository = new DBUserInfoRepository(DatabaseType.MySql, "server=127.0.0.1;user id=root; password=yswenli; Port=3306;database=test; pooling=true");

            var skuInfoRepository = new SkuInfoRepository(DatabaseType.SqlServer, "Data Source=LP20210416002\\MSSQLSERVER2014;Initial Catalog=Oceania_Test;Integrated Security=True");

            var userInfo = new DBUserInfo()
            {
                UserName = "yswenli",
                NickName = "Mason",
                Gender = true,
                Created = DateTime.Now
            };

            var search = skuInfoRepository.Search();

            //search = search.Where(b => b.SKU == "000a6d2fb6774cb8866ef7a6987b71e4");

            var where1 = new Where<SkuInfo>();

            where1.And(b =>b.ID== "00091ee8c4ec465993c7de57ad5cc6fa" && b.SKU == "000a6d2fb6774cb8866ef7a6987b71e4");

            //search = search.Where(b => b.Price == 1M);

            where1.And(b => b.Price == 1M);

            var list= search.Where(where1).ToList();


            Console.ReadLine();
        }
    }
}
