using System;

using WEF.Common;
using WEF.Core.Test.Models;

namespace WEF.Core.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "WEF.Core.Test";

            //var userInfoRepository = new DBUserInfoRepository(DatabaseType.MySql, "server=127.0.0.1;user id=root; password=yswenli; Port=3306;database=test; pooling=true");

            var userInfoRepository = new DBUserInfoRepository(DatabaseType.SqlServer, "Data Source=192.168.11.77;Initial Catalog=OceaniaJobMonitor;User Id=Test77Ur;Password=Test77Ur");

            var userInfo = new DBUserInfo()
            {
                UserName = "yswenli",
                NickName = "Mason",
                Gender = true,
                Created = DateTime.Now
            };

            var list= userInfoRepository.DBContext.FromSql("select * from db", 1, 20).ToList<DBUserInfo>();

            var iResult = userInfoRepository.Insert(userInfo);

            Console.WriteLine($"Insert:{iResult}");

            var ui = userInfoRepository.Search().Where(b => b.UserName == "yswenli").First();

            if (ui != null)
            {
                Console.WriteLine($"Get:{SerializeHelper.Serialize(ui)}");
            }

            ui.NickName = "Mason.Wen";

            var uResult = userInfoRepository.Update(ui);

            Console.WriteLine($"Update:{uResult}");

            var dui = userInfoRepository.Search().Where(b => b.UserName == "yswenli").First();
            if (dui != null)
            {
                var dResult = userInfoRepository.Delete(dui);
                Console.WriteLine($"Delete:{dResult}");
            }

            Console.ReadLine();
        }
    }
}
