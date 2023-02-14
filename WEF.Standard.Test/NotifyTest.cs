/****************************************************************************
*Copyright (c) 2022 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.Standard.Test
*文件名： NotifyTest
*版本号： V1.0.0.0
*唯一标识：98a227dd-8928-49d6-a098-c1218930205a
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2022/11/29 14:29:50
*描述：
*
*=================================================
*修改标记
*修改时间：2022/11/29 14:29:50
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

using WEF.MvcPager;
using WEF.Standard.Test.Models;

using static System.Net.Mime.MediaTypeNames;

namespace WEF.Standard.Test
{
    internal static class NotifyTest
    {
        /// <summary>
        /// GetList
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dataName"></param>
        /// <param name="businessType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public static PagedList<NotificationListItem> GetList(string userId, string dataName, int businessType, int pageIndex = 1, int pageSize = 10, string orderBy = "Notification.Created", bool asc = false)
        {
            if (pageSize > 100) pageSize = 100;
            if (pageSize < 1) pageSize = 10;
            if (pageIndex < 1) pageIndex = 1;
            if (orderBy.IsNullOrEmpty() || orderBy == "string") orderBy = "Notification.Created";
            if (!orderBy.StartsWith("Notification.", StringComparison.InvariantCultureIgnoreCase))
            {
                orderBy = "Notification." + orderBy;
            }

            var conn = "";
            var respository = new DBNotificationRepository(WEF.DatabaseType.SqlServer, conn);

            var search = respository.Search().Join<DBNotificationSetting>((x, y) => x.SettingID == y.ID, WEF.Common.JoinType.LeftJoin);

            if (dataName.IsNotNullOrEmpty())
            {
                if (dataName.Equals("pro", true))
                {
                    search = search.Where<DBNotificationSetting>((x, y) => x.ReceiverID == userId && x.IsDeleted != true && y.BusinessType == businessType && y.IsDeleted != true && y.Key.StartsWith("医"));
                }
                else if (dataName.Equals("public", true))
                {
                    search = search.Where<DBNotificationSetting>((x, y) => x.ReceiverID == userId && x.IsDeleted != true && y.BusinessType == businessType && y.IsDeleted != true && y.Key.StartsWith("患"));
                }
                else
                {
                    search = search.Where<DBNotificationSetting>((x, y) => x.ReceiverID == userId && x.IsDeleted != true && y.BusinessType == businessType && y.IsDeleted != true);
                }
            }
            else
            {
                search = search.Where<DBNotificationSetting>((x, y) => x.ReceiverID == userId && x.IsDeleted != true && y.BusinessType == businessType && y.IsDeleted != true);
            }
            search = search.Select<DBNotificationSetting>((x, y) => new
            {
                ID = x.ID,
                Content = x.Content,
                Created = x.Created,
                UnRead = x.UnRead,
                Key = y.Key,
                BusinessType = y.BusinessType,
                Type = y.Type,
                Name = y.Name,
                Icon = y.Icon,
                BtnUrl = y.BtnUrl,
                BtnText = y.BtnText,
                Sender = x.Sender,
                SenderName = x.SenderName,
                SenderGender = x.SenderGender
            });
            var list= search.ToPagedList<NotificationListItem>(pageIndex, pageSize, orderBy, asc);
            return list;
        }
    }


    /// <summary>
    /// 通知项
    /// </summary>
    public class NotificationListItem
    {
        /// <summary>
        /// 通知id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 唯一值
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 业务大类 1一般消息，2 患者绑定，3咨询消息，4评论点赞，5钱包消息
        /// </summary>
        public int BusinessType { get; set; }
        /// <summary>
        /// 通知类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 通知名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图标样式
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 按钮标题
        /// </summary>
        public string BtnText { get; set; }
        /// <summary>
        /// 按钮地址
        /// </summary>
        public string BtnUrl { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 是否未读
        /// </summary>
        public bool UnRead { get; set; }
        /// <summary>
        /// 发送者
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// 发送者名称
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// 发送者性别
        /// </summary>
        public Boolean? SenderGender { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Created { get; set; }
    }

    public static class StringExtention
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool Equals(this string str, string txt, bool igonCase)
        {
            if (str.IsNullOrEmpty())
            {
                return false;
            }

            if (igonCase)
            {
                return str!.Equals(txt, StringComparison.InvariantCultureIgnoreCase);
            }

            return str!.Equals(txt);
        }
    }

}
