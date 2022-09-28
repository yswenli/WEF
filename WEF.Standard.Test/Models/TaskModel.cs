using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace WEF.Models
{
    #region << 版 本 注 释 >>
    /*---------------------------------------------------------------- 。 
    //
    // 文件名：TaskModel
    // 文件功能描述：
    //
    // 
    // 创建者：名字 ($wangwenjing$)
    // 时间：2019/4/15 17:01:21
    //
    // 修改人：
    // 时间：
    // 修改说明：
    //
    // 修改人：
    // 时间：
    // 修改说明：
    //
    // 版本：V1.0.0
    //----------------------------------------------------------------*/
    #endregion
    [DataContract, Serializable]
    public class TaskModel
    {
        /// <summary>
        /// 任务编号（特殊编码，唯一主键
        /// </summary>
        [DataMember]
        public string TaskID { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 支持平台（基金、证券或全平台）
        /// </summary>
        [DataMember]
        public string PlatformID { get; set; }

        /// <summary>
        /// 子业务（隶属于平台）
        /// </summary>
        [DataMember]
        public string BusinID { get; set; }

        /// <summary>
        /// 兑换所需的积分
        /// </summary>
        [DataMember]
        public decimal? Point { get; set; }

        /// <summary>
        /// 单用户每日可兑换次数（0为不限制）
        /// </summary>
        [DataMember]
        public decimal? DayTimes { get; set; }

        /// <summary>
        /// 单用户可累计兑换次数（0为不限制）
        /// </summary>
        [DataMember]
        public decimal? TotalTimes { get; set; }

        /// <summary>
        /// 每日可兑换次数
        /// </summary>
        [DataMember]
        public decimal? DayLimit { get; set; }

        /// <summary>
        /// 总成本
        /// </summary>
        [DataMember]
        public decimal? TotalLimit { get; set; }

        /// <summary>
        /// 可领取开始时间
        /// </summary>
        [DataMember]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 可领取结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 添加人员
        /// </summary>
        [DataMember]
        public string Operator { get; set; }

        /// <summary>
        /// 校验值
        /// </summary>
        [DataMember]
        public decimal? Crc32 { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [DataMember]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 是否有效标志位
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 逻辑删除标志位
        /// </summary>
        [DataMember]
        public bool IsDel { get; set; }


        // 任务状态（默认0.待审核）
        /// <summary>
        /// 任务状态 （1：待审核、2：已上架、3：已下架）(注：0代表查询所有)
        /// </summary>
        [DataMember]
        public int? TaskState { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        [DataMember]
        public string CompanyId { get; set; }


        /// <summary>
        /// 策略编号Guid
        /// </summary>
        [DataMember]
        public string TaskStrategyID { get; set; }

        /// <summary>
        /// 任务类型 1：一次任务，2：日常任务，3：月任务，4：年任务（生日等），5：活动(注：0代表查询所有)
        /// </summary>
        [DataMember]
        public int? TaskType { get; set; }

        /// <summary>
        /// 任务子类型 101：绑手机，102：首次交易，
        ///            201：签到，202：每日答题任务，
        ///            301：月交易满额，302：月定投满额，
        ///            401：生日，
        ///            (注：0代表查询所有)
        /// </summary>
        [DataMember]
        public int? TaskSubType { get; set; }


        /// <summary>
        /// 变动方向：0 查询所有，1 流入， 2 流出， 3 流入流出
        /// </summary>
        [DataMember]
        public string FlowType { get; set; }


        /// <summary>
        /// 任务的最小积分
        /// </summary>
        [DataMember]
        public decimal? TaskMinPoint { get; set; }

        /// <summary>
        /// 任务的最大积分
        /// </summary>
        [DataMember]
        public decimal? TaskMaxPoint { get; set; }

        /// <summary>
        /// 任务的奖励类型
        /// 0：积分，1：现金券，2：可变现金券，3：满减券
        /// </summary>
        [DataMember]
        public int? TaskRewardType { get; set; }
    }
}
