/****************************************************************************
*Copyright (c) 2021 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Test
*文件名： MySqlBitTypeTest
*版本号： V1.0.0.0
*唯一标识：6738a661-57e8-4a3d-b837-f9cb29604bd9
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2021/12/6 14:55:52
*描述：
*
*=====================================================================
*修改标记
*修改时间：2021/12/6 14:55:52
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WEF.Models;
using WEF.Test.Models;

namespace WEF.Test
{
    public static class MySqlBitTypeTest
    {
        /// <summary>
        /// mysql bit tran 测试
        /// </summary>
        public static void Test1()
        {
            var cnnStr = "server=192.168.9.234;user id=root; password=12321; Port=3306;database=oceaniawms; pooling=true";

            DBOcWarehouseArea area = new DBOcWarehouseArea()
            {
                ID = Guid.NewGuid().ToString("N"),
                WarehouseCode = "aaaa",
                SortID = 0,
                Receivable = false,
                Storable = false,
                Pickable = true,
                Sendable = false,
                IsSecondHand = false,
                IsAbandoned = false,
                Detectable = false,
                IsTemporary = false,
                Repairable = false,
                CreateBy = "Mason",
                CreateTime = DateTime.Now
            };

            var repository = new DBOcWarehouseAreaRepository(DatabaseType.MySql, cnnStr);

            using (var tran = repository.CreateTransaction())
            {
                tran.Insert(area);
                tran.Commit();
            }
            Console.WriteLine("mysql bit tran 测试");
        }

        public static void Test2()
        {
            var cnnStr = "server=192.168.9.234;user id=root; password=12321; Port=3306;database=oceaniawms; pooling=true";

            DBWarehouseInboundData data = new DBWarehouseInboundData()
            {
                ID = Guid.NewGuid().ToString("N"),
                IID = Guid.NewGuid().ToString("N"),
                SKU = "21512-afas4-a",
                Company = "Oceania",
                BatchNumber = "55456513asfd",
                Amount = 10,
                ApplyQty = 111,
                ApplyBoxQty = 10,
                PerBoxQty = 11,
                Length = 10,
                Width = 10,
                Height = 10,
                Weight = 10,
                Regular = true,
                Dangerous = false,
                Fragile = true,
                NoneBox = false,
                SideUp = true,
                ExpireTime = DateTime.Now.AddYears(100),
                Status = 1
            };

            var repository = new DBWarehouseInboundDataRepository(DatabaseType.MySql, cnnStr);

            using (var tran = repository.CreateTransaction())
            {
                tran.Insert(data);
                tran.Commit();
            }
            Console.WriteLine("mysql bit tran 测试");
        }


        public static void Test3()
        {
            var cnnStr = "server=192.168.9.234;user id=root; password=12321; Port=3306;database=oceaniawms; pooling=true";

            var repository = new DBWarehouseInboundDataRepository(DatabaseType.MySql, cnnStr);

            using (var tran = repository.CreateTransaction())
            {
                var data = tran.Search<DBWarehouseInboundData>().Where(q => q.ID == "2f7a92eb536a11eca88d0050568338d8").First();
                data.ApplyQty = 22;
                var result = tran.Update(data);
                tran.Commit();
            }
            Console.WriteLine("mysql bit tran 测试");
        }
    }
}
