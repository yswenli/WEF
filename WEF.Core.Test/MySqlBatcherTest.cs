/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Core.Test
*文件名： MySqlBatcherTest
*版本号： V1.0.0.0
*唯一标识：8680a2e8-9496-486a-a4d1-e16f56ca9274
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2022/3/7 9:49:58
*描述：
*
*=====================================================================
*修改标记
*修改时间：2022/3/7 9:49:58
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;

using WEF.Core.Test.Models;

namespace WEF.Core.Test
{
    /// <summary>
    /// mysql batch操作 test
    /// </summary>
    public static class MySqlBatcherTest
    {
        /// <summary>
        /// mysql batch操作 test
        /// </summary>
        public static void Test()
        {
            Console.WriteLine("mysql batch操作开始");
            var cnnStr = "server=54.177.54.64;user id=awswms; password=OcPda0708*sW#Pk; Port=3306;database=oceaniajobmonitor; pooling=true";
            var repository = new DBLoginfoRepository(DatabaseType.MySql, cnnStr);
            var list = new List<DBLoginfo>();
            for (int i = 0; i < 1000; i++)
            {
                var logInfo = new DBLoginfo()
                {
                    ID = Guid.NewGuid().ToString("N"),
                    Created = DateTime.Now,
                    Description = "test",
                    Error = "test",
                    IP = "127.0.0.1",
                    Level = 0,
                    Operator = "test",
                    Reason = "test",
                    Resolved = false,
                    ServiceName = "test",
                    Updated = DateTime.Now
                };
                list.Add(logInfo);
            }
            repository.BulkInsert(list);
            Console.WriteLine("操作完成");
            Console.ReadLine();
        }
    }
}
