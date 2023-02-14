/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Test
*文件名： BytesTest
*版本号： V1.0.0.0
*唯一标识：2394a390-c37c-4e3e-9dce-e3fef123a20c
*当前的用户域：WALLE
*创建人： wenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/8/17 11:06:25
*描述：
*
*=================================================
*修改标记
*修改时间：2022/8/17 11:06:25
*修改人： yswenli
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

namespace WEF.Test
{
    /// <summary>
    /// 字节类型测试
    /// </summary>
    public class BytesTest
    {
        string _cnnStr;

        /// <summary>
        /// 字节类型测试
        /// </summary>
        public BytesTest()
        {
            _cnnStr = "";
        }

        public void Test()
        {
            var r = new DBFileDataRepository(DatabaseType.SqlServer, _cnnStr);

            var bytes = new byte[1024];

            new Random().NextBytes(bytes);

            var e = new DBFileData()
            {
                Created = DateTime.Now,
                CreatedBy = "1",
                Data = bytes,
                ID = Guid.NewGuid().ToString("N"),
                IsDeleted = false,
                MD5 = "111",
                Size = 1024
            };

            var d = r.Insert(e);
        }

        public void Test2()
        {
            var r = new DBFileDataRepository(DatabaseType.SqlServer, _cnnStr);

            r.Search().LeftJoin<DBUserPoint>((a, b) => a.MD5 == b.Uid).Select<DBUserPoint>((a, b) => new { a.MD5, b.Points }).ToList();
        }
    }
}
