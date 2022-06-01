using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using WEF;
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
            MySqlBatcherTest.Test();
            Console.ReadLine();



            var repository = new DBLoginfoRepository(DatabaseType.MySql, "server=192.168.11.18;user id=developer; password=Dev321!@#; Port=3306;database=oceaniajobmonitor; pooling=true;Convert Zero Datetime=True");

            var list1111 = new List<DBLoginfo>();
            list1111.Add(new DBLoginfo()
            {
                ID = Guid.NewGuid().ToString("N"),
                IP = "1111",
                Level = 1,
                Description = "1111",
                Created = DateTime.Now,
                Error = "22222",
                Operator = "test",
                Resolved = false,
                ServiceName = "test"
            });

            repository.BulkInsert(list1111);

            using (var tran = repository.CreateTransaction())
            {
                try
                {
                    var a1 = new DBLoginfo()
                    {
                        ID = Guid.NewGuid().ToString("N"),
                        IP = "2222",
                        Level = 1,
                        Description = "1111",
                        Created = DateTime.Now,
                        Error = "22222",
                        Operator = "test",
                        Resolved = false,
                        ServiceName = "test"
                    };

                    tran.Insert(a1);

                    var a2 = new DBLoginfo()
                    {
                        IP = "3333"
                    };

                    tran.Insert(a2);

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }

            var exception = repository.CreateTransaction().TryCommit((t) =>
            {
                var a1 = new DBLoginfo()
                {
                    ID = Guid.NewGuid().ToString("N"),
                    IP = "2222",
                    Level = 1,
                    Description = "1111",
                    Created = DateTime.Now,
                    Error = "22222",
                    Operator = "test",
                    Resolved = false,
                    ServiceName = "test"
                };

                t.Insert(a1);

                var a2 = new DBLoginfo()
                {
                    IP = "3333"
                };

                t.Insert(a2);
            });


            #region 传值填充更新

            var userInfoRepository = new DBUserInfoRepository(DatabaseType.SqlServer, "Data Source=LP20210416002\\MSSQLSERVER2014;Initial Catalog=Oceania_Test;Integrated Security=True");

            var userInfo = new UserInfo()
            {
                LegalName = "Mason.Wen1",
                DisplayName = "Mason.Wen1"
            };

            var user = userInfoRepository.Search().Where(q => q.DisplayName == "Mason Wen").First();

            user.FillFrom(userInfo, allowNull: false);
            //userInfo.FillModel(user);

            userInfoRepository.Update(user);

            #endregion


            var skuInfoRepository = new SkuInfoRepository(DatabaseType.SqlServer, "Data Source=LP20210416002\\MSSQLSERVER2014;Initial Catalog=Oceania_Test;Integrated Security=True");

            var search = skuInfoRepository.Search();

            //search = search.Where(b => b.SKU == "000a6d2fb6774cb8866ef7a6987b71e4");

            var where1 = new Where<SkuInfo>();

            var a = "a ";

            where1.And(b => b.ID == "00091ee8c4ec465993c7de57ad5cc6fa" && b.SKU == "000a6d2fb6774cb8866ef7a6987b71e4" && b.Created > DateTime.Now.AddDays(1));

            where1.And(b => b.Price == 1M);

            var list = search.Where(where1).ToList();

            Console.ReadLine();
        }
    }
}

