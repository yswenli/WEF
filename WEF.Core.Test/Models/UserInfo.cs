/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Core.Test.Models
*文件名： UserInfo
*版本号： V1.0.0.0
*唯一标识：34b9d040-11b8-407b-bfa3-4762f5536246
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2022/2/10 17:59:59
*描述：
*
*=====================================================================
*修改标记
*修改时间：2022/2/10 17:59:59
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Runtime.Serialization;

namespace WEF.Core.Test.Models
{
    /// <summary>
    /// 实体类UserInfo
    /// OC_User_Info
    /// </summary>
    [Serializable, DataContract]
	public partial class UserInfo
	{
		/// <summary>
		/// LegalName 
		/// </summary>
		[DataMember]
		public string LegalName
		{
			get;
			set;
		}
		/// <summary>
		/// DisplayName 
		/// </summary>
		[DataMember]
		public string DisplayName
		{
			get;
			set;
		}

		/// <summary>
		/// WorkInfo 
		/// </summary>
		[DataMember]
		public string Department
		{
			get;set;
		}
	}

}
