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

            var userInfoRepository = new DBUserInfoRepository(DatabaseType.MySql, "server=127.0.0.1;user id=root; password=yswenli; Port=3306;database=test; pooling=true");

            var userInfo = new DBUserInfo()
            {
                UserName = "yswenli",
                NickName = "Mason",
                Gender = true,
                Created = DateTime.Now
            };


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
