/****************************************************************************
*Copyright (c) 2022 Oceania All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：LP20210416002
*公司名称：Oceania
*命名空间：WEF.Core.Test.Models
*文件名： DBUserInfo
*版本号： V1.0.0.0
*唯一标识：d7f82391-4314-4b69-a0f7-a7402f2006d7
*当前的用户域：OCEANIA
*创建人： Mason.Wen
*电子邮箱：Mason.Wen@oceania-inc.com
*创建时间：2022/2/10 17:58:43
*描述：
*
*=====================================================================
*修改标记
*修改时间：2022/2/10 17:58:43
*修改人： Mason.Wen
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Data;
using System.Runtime.Serialization;

using WEF.Common;

namespace WEF.Core.Test.Models
{
    /// <summary>
    /// 实体类DBUserInfo
    /// </summary>
    [Serializable, DataContract, Table("OC_User_Info")]
	public partial class DBUserInfo : Entity
	{
		private static string m_tableName;
		/// <summary>
		/// 实体类DBUserInfo
		/// </summary>
		public DBUserInfo() : base("OC_User_Info") { m_tableName = "OC_User_Info"; }
		/// <summary>
		/// 实体类DBUserInfo
		/// <param name="tableName">表名</param>
		/// </summary>
		public DBUserInfo(string tableName) : base(tableName) { m_tableName = tableName; }

		#region Model
		private int _EmployeeNo;
		private string _ID;
		private string _RoleID;
		private string _Password;
		private string _LegalName;
		private string _DisplayName;
		private string _Department;
		private string _Team;
		private string _Position;
		private string _IsLeader;
		private int? _DirectManagerID;
		private string _DirectManager;
		private string _Company;
		private string _Location;
		private string _SeatNumber;
		private string _IdentityCard;
		private string _Nationality;
		private string _WorkType;
		private string _PaymentType;
		private int? _SocialSecurity;
		private float? _SocialSecurityScale;
		private int? _AccumulationFund;
		private float? _AccumulationFundScale;
		private int? _AttendanceRecord;
		private int? _OKRRecord;
		private string _Phone;
		private string _ExtNumber;
		private string _WxID;
		private string _PersonWX;
		private string _EMail;
		private string _PersonEmail;
		private string _BusinessQQ;
		private string _EmergencyContact1;
		private string _EmergencyContactRelation1;
		private string _EmergencyContactAddress1;
		private string _EmergencyContactNumber1;
		private string _EmergencyContact2;
		private string _EmergencyContactRelation2;
		private string _EmergencyContactAddress2;
		private string _EmergencyContactNumber2;
		private string _BankCard;
		private string _Contract;
		private string _Sex;
		private string _BirthdayType;
		private DateTime? _Birthday;
		private string _Birthplace;
		private string _Birthphone;
		private string _NativePlace;
		private string _MarriageType;
		private string _SpecialMedical;
		private string _PermanentResidenceAddress;
		private string _HomeAddress;
		private string _HomeNumber;
		private string _SHAddress;
		private string _SHStreet;
		private string _SHEmail;
		private string _NearSubway;
		private string _FamilyInfo;
		private string _EducationInfo;
		private string _WorkInfo;
		private string _FileInfo;
		private string _HeadPortrait;
		private DateTime? _HireDate;
		private DateTime? _TermDate;
		private DateTime? _PositiveDates;
		private string _ContractingCompany;
		private string _NumberCode;
		private string _CompanySkype;
		private string _Creater;
		private DateTime? _CreateTime;
		private string _Privilege;
		private DateTime? _LastLoginTime;
		private int? _RetainJobbutSuspendSalary;
		private DateTime? _WithoutPayDate;
		private DateTime? _ReinstatementDate;
		private float? _InitialAnnualLeave;
		private string _Status;
		private string _MobilePhone;
		private string _Institution;
		private string _DepartmentManagementList;
		private int? _AttendanceUser;
		private string _LeaveApplicationCC;
		private string _IdType;
		private string _WechatOpenID;
		private string _QQOpenID;
		private string _WechatTicket;
		/// <summary>
		/// EmployeeNo 
		/// </summary>
		[DataMember]
		public int EmployeeNo
		{
			get { return _EmployeeNo; }
			set
			{
				this.OnPropertyValueChange(_.EmployeeNo, _EmployeeNo, value);
				this._EmployeeNo = value;
			}
		}
		/// <summary>
		/// ID 
		/// </summary>
		[DataMember]
		public string ID
		{
			get { return _ID; }
			set
			{
				this.OnPropertyValueChange(_.ID, _ID, value);
				this._ID = value;
			}
		}
		/// <summary>
		/// RoleID 
		/// </summary>
		[DataMember]
		public string RoleID
		{
			get { return _RoleID; }
			set
			{
				this.OnPropertyValueChange(_.RoleID, _RoleID, value);
				this._RoleID = value;
			}
		}
		/// <summary>
		/// Password 
		/// </summary>
		[DataMember]
		public string Password
		{
			get { return _Password; }
			set
			{
				this.OnPropertyValueChange(_.Password, _Password, value);
				this._Password = value;
			}
		}
		/// <summary>
		/// LegalName 
		/// </summary>
		[DataMember]
		public string LegalName
		{
			get { return _LegalName; }
			set
			{
				this.OnPropertyValueChange(_.LegalName, _LegalName, value);
				this._LegalName = value;
			}
		}
		/// <summary>
		/// DisplayName 
		/// </summary>
		[DataMember]
		public string DisplayName
		{
			get { return _DisplayName; }
			set
			{
				this.OnPropertyValueChange(_.DisplayName, _DisplayName, value);
				this._DisplayName = value;
			}
		}
		/// <summary>
		/// Department 
		/// </summary>
		[DataMember]
		public string Department
		{
			get { return _Department; }
			set
			{
				this.OnPropertyValueChange(_.Department, _Department, value);
				this._Department = value;
			}
		}
		/// <summary>
		/// Team 
		/// </summary>
		[DataMember]
		public string Team
		{
			get { return _Team; }
			set
			{
				this.OnPropertyValueChange(_.Team, _Team, value);
				this._Team = value;
			}
		}
		/// <summary>
		/// Position 
		/// </summary>
		[DataMember]
		public string Position
		{
			get { return _Position; }
			set
			{
				this.OnPropertyValueChange(_.Position, _Position, value);
				this._Position = value;
			}
		}
		/// <summary>
		/// IsLeader 
		/// </summary>
		[DataMember]
		public string IsLeader
		{
			get { return _IsLeader; }
			set
			{
				this.OnPropertyValueChange(_.IsLeader, _IsLeader, value);
				this._IsLeader = value;
			}
		}
		/// <summary>
		/// DirectManagerID 
		/// </summary>
		[DataMember]
		public int? DirectManagerID
		{
			get { return _DirectManagerID; }
			set
			{
				this.OnPropertyValueChange(_.DirectManagerID, _DirectManagerID, value);
				this._DirectManagerID = value;
			}
		}
		/// <summary>
		/// DirectManager 
		/// </summary>
		[DataMember]
		public string DirectManager
		{
			get { return _DirectManager; }
			set
			{
				this.OnPropertyValueChange(_.DirectManager, _DirectManager, value);
				this._DirectManager = value;
			}
		}
		/// <summary>
		/// Company 
		/// </summary>
		[DataMember]
		public string Company
		{
			get { return _Company; }
			set
			{
				this.OnPropertyValueChange(_.Company, _Company, value);
				this._Company = value;
			}
		}
		/// <summary>
		/// Location 
		/// </summary>
		[DataMember]
		public string Location
		{
			get { return _Location; }
			set
			{
				this.OnPropertyValueChange(_.Location, _Location, value);
				this._Location = value;
			}
		}
		/// <summary>
		/// SeatNumber 
		/// </summary>
		[DataMember]
		public string SeatNumber
		{
			get { return _SeatNumber; }
			set
			{
				this.OnPropertyValueChange(_.SeatNumber, _SeatNumber, value);
				this._SeatNumber = value;
			}
		}
		/// <summary>
		/// IdentityCard 
		/// </summary>
		[DataMember]
		public string IdentityCard
		{
			get { return _IdentityCard; }
			set
			{
				this.OnPropertyValueChange(_.IdentityCard, _IdentityCard, value);
				this._IdentityCard = value;
			}
		}
		/// <summary>
		/// Nationality 
		/// </summary>
		[DataMember]
		public string Nationality
		{
			get { return _Nationality; }
			set
			{
				this.OnPropertyValueChange(_.Nationality, _Nationality, value);
				this._Nationality = value;
			}
		}
		/// <summary>
		/// WorkType 
		/// </summary>
		[DataMember]
		public string WorkType
		{
			get { return _WorkType; }
			set
			{
				this.OnPropertyValueChange(_.WorkType, _WorkType, value);
				this._WorkType = value;
			}
		}
		/// <summary>
		/// PaymentType 
		/// </summary>
		[DataMember]
		public string PaymentType
		{
			get { return _PaymentType; }
			set
			{
				this.OnPropertyValueChange(_.PaymentType, _PaymentType, value);
				this._PaymentType = value;
			}
		}
		/// <summary>
		/// SocialSecurity 
		/// </summary>
		[DataMember]
		public int? SocialSecurity
		{
			get { return _SocialSecurity; }
			set
			{
				this.OnPropertyValueChange(_.SocialSecurity, _SocialSecurity, value);
				this._SocialSecurity = value;
			}
		}
		/// <summary>
		/// SocialSecurityScale 
		/// </summary>
		[DataMember]
		public float? SocialSecurityScale
		{
			get { return _SocialSecurityScale; }
			set
			{
				this.OnPropertyValueChange(_.SocialSecurityScale, _SocialSecurityScale, value);
				this._SocialSecurityScale = value;
			}
		}
		/// <summary>
		/// AccumulationFund 
		/// </summary>
		[DataMember]
		public int? AccumulationFund
		{
			get { return _AccumulationFund; }
			set
			{
				this.OnPropertyValueChange(_.AccumulationFund, _AccumulationFund, value);
				this._AccumulationFund = value;
			}
		}
		/// <summary>
		/// AccumulationFundScale 
		/// </summary>
		[DataMember]
		public float? AccumulationFundScale
		{
			get { return _AccumulationFundScale; }
			set
			{
				this.OnPropertyValueChange(_.AccumulationFundScale, _AccumulationFundScale, value);
				this._AccumulationFundScale = value;
			}
		}
		/// <summary>
		/// AttendanceRecord 
		/// </summary>
		[DataMember]
		public int? AttendanceRecord
		{
			get { return _AttendanceRecord; }
			set
			{
				this.OnPropertyValueChange(_.AttendanceRecord, _AttendanceRecord, value);
				this._AttendanceRecord = value;
			}
		}
		/// <summary>
		/// OKRRecord 
		/// </summary>
		[DataMember]
		public int? OKRRecord
		{
			get { return _OKRRecord; }
			set
			{
				this.OnPropertyValueChange(_.OKRRecord, _OKRRecord, value);
				this._OKRRecord = value;
			}
		}
		/// <summary>
		/// Phone 
		/// </summary>
		[DataMember]
		public string Phone
		{
			get { return _Phone; }
			set
			{
				this.OnPropertyValueChange(_.Phone, _Phone, value);
				this._Phone = value;
			}
		}
		/// <summary>
		/// ExtNumber 
		/// </summary>
		[DataMember]
		public string ExtNumber
		{
			get { return _ExtNumber; }
			set
			{
				this.OnPropertyValueChange(_.ExtNumber, _ExtNumber, value);
				this._ExtNumber = value;
			}
		}
		/// <summary>
		/// WxID 
		/// </summary>
		[DataMember]
		public string WxID
		{
			get { return _WxID; }
			set
			{
				this.OnPropertyValueChange(_.WxID, _WxID, value);
				this._WxID = value;
			}
		}
		/// <summary>
		/// PersonWX 
		/// </summary>
		[DataMember]
		public string PersonWX
		{
			get { return _PersonWX; }
			set
			{
				this.OnPropertyValueChange(_.PersonWX, _PersonWX, value);
				this._PersonWX = value;
			}
		}
		/// <summary>
		/// EMail 
		/// </summary>
		[DataMember]
		public string EMail
		{
			get { return _EMail; }
			set
			{
				this.OnPropertyValueChange(_.EMail, _EMail, value);
				this._EMail = value;
			}
		}
		/// <summary>
		/// PersonEmail 
		/// </summary>
		[DataMember]
		public string PersonEmail
		{
			get { return _PersonEmail; }
			set
			{
				this.OnPropertyValueChange(_.PersonEmail, _PersonEmail, value);
				this._PersonEmail = value;
			}
		}
		/// <summary>
		/// BusinessQQ 
		/// </summary>
		[DataMember]
		public string BusinessQQ
		{
			get { return _BusinessQQ; }
			set
			{
				this.OnPropertyValueChange(_.BusinessQQ, _BusinessQQ, value);
				this._BusinessQQ = value;
			}
		}
		/// <summary>
		/// EmergencyContact1 
		/// </summary>
		[DataMember]
		public string EmergencyContact1
		{
			get { return _EmergencyContact1; }
			set
			{
				this.OnPropertyValueChange(_.EmergencyContact1, _EmergencyContact1, value);
				this._EmergencyContact1 = value;
			}
		}
		/// <summary>
		/// EmergencyContactRelation1 
		/// </summary>
		[DataMember]
		public string EmergencyContactRelation1
		{
			get { return _EmergencyContactRelation1; }
			set
			{
				this.OnPropertyValueChange(_.EmergencyContactRelation1, _EmergencyContactRelation1, value);
				this._EmergencyContactRelation1 = value;
			}
		}
		/// <summary>
		/// EmergencyContactAddress1 
		/// </summary>
		[DataMember]
		public string EmergencyContactAddress1
		{
			get { return _EmergencyContactAddress1; }
			set
			{
				this.OnPropertyValueChange(_.EmergencyContactAddress1, _EmergencyContactAddress1, value);
				this._EmergencyContactAddress1 = value;
			}
		}
		/// <summary>
		/// EmergencyContactNumber1 
		/// </summary>
		[DataMember]
		public string EmergencyContactNumber1
		{
			get { return _EmergencyContactNumber1; }
			set
			{
				this.OnPropertyValueChange(_.EmergencyContactNumber1, _EmergencyContactNumber1, value);
				this._EmergencyContactNumber1 = value;
			}
		}
		/// <summary>
		/// EmergencyContact2 
		/// </summary>
		[DataMember]
		public string EmergencyContact2
		{
			get { return _EmergencyContact2; }
			set
			{
				this.OnPropertyValueChange(_.EmergencyContact2, _EmergencyContact2, value);
				this._EmergencyContact2 = value;
			}
		}
		/// <summary>
		/// EmergencyContactRelation2 
		/// </summary>
		[DataMember]
		public string EmergencyContactRelation2
		{
			get { return _EmergencyContactRelation2; }
			set
			{
				this.OnPropertyValueChange(_.EmergencyContactRelation2, _EmergencyContactRelation2, value);
				this._EmergencyContactRelation2 = value;
			}
		}
		/// <summary>
		/// EmergencyContactAddress2 
		/// </summary>
		[DataMember]
		public string EmergencyContactAddress2
		{
			get { return _EmergencyContactAddress2; }
			set
			{
				this.OnPropertyValueChange(_.EmergencyContactAddress2, _EmergencyContactAddress2, value);
				this._EmergencyContactAddress2 = value;
			}
		}
		/// <summary>
		/// EmergencyContactNumber2 
		/// </summary>
		[DataMember]
		public string EmergencyContactNumber2
		{
			get { return _EmergencyContactNumber2; }
			set
			{
				this.OnPropertyValueChange(_.EmergencyContactNumber2, _EmergencyContactNumber2, value);
				this._EmergencyContactNumber2 = value;
			}
		}
		/// <summary>
		/// BankCard 
		/// </summary>
		[DataMember]
		public string BankCard
		{
			get { return _BankCard; }
			set
			{
				this.OnPropertyValueChange(_.BankCard, _BankCard, value);
				this._BankCard = value;
			}
		}
		/// <summary>
		/// Contract 
		/// </summary>
		[DataMember]
		public string Contract
		{
			get { return _Contract; }
			set
			{
				this.OnPropertyValueChange(_.Contract, _Contract, value);
				this._Contract = value;
			}
		}
		/// <summary>
		/// Sex 
		/// </summary>
		[DataMember]
		public string Sex
		{
			get { return _Sex; }
			set
			{
				this.OnPropertyValueChange(_.Sex, _Sex, value);
				this._Sex = value;
			}
		}
		/// <summary>
		/// BirthdayType 
		/// </summary>
		[DataMember]
		public string BirthdayType
		{
			get { return _BirthdayType; }
			set
			{
				this.OnPropertyValueChange(_.BirthdayType, _BirthdayType, value);
				this._BirthdayType = value;
			}
		}
		/// <summary>
		/// Birthday 
		/// </summary>
		[DataMember]
		public DateTime? Birthday
		{
			get { return _Birthday; }
			set
			{
				this.OnPropertyValueChange(_.Birthday, _Birthday, value);
				this._Birthday = value;
			}
		}
		/// <summary>
		/// Birthplace 
		/// </summary>
		[DataMember]
		public string Birthplace
		{
			get { return _Birthplace; }
			set
			{
				this.OnPropertyValueChange(_.Birthplace, _Birthplace, value);
				this._Birthplace = value;
			}
		}
		/// <summary>
		/// Birthphone 
		/// </summary>
		[DataMember]
		public string Birthphone
		{
			get { return _Birthphone; }
			set
			{
				this.OnPropertyValueChange(_.Birthphone, _Birthphone, value);
				this._Birthphone = value;
			}
		}
		/// <summary>
		/// NativePlace 
		/// </summary>
		[DataMember]
		public string NativePlace
		{
			get { return _NativePlace; }
			set
			{
				this.OnPropertyValueChange(_.NativePlace, _NativePlace, value);
				this._NativePlace = value;
			}
		}
		/// <summary>
		/// MarriageType 
		/// </summary>
		[DataMember]
		public string MarriageType
		{
			get { return _MarriageType; }
			set
			{
				this.OnPropertyValueChange(_.MarriageType, _MarriageType, value);
				this._MarriageType = value;
			}
		}
		/// <summary>
		/// SpecialMedical 
		/// </summary>
		[DataMember]
		public string SpecialMedical
		{
			get { return _SpecialMedical; }
			set
			{
				this.OnPropertyValueChange(_.SpecialMedical, _SpecialMedical, value);
				this._SpecialMedical = value;
			}
		}
		/// <summary>
		/// PermanentResidenceAddress 
		/// </summary>
		[DataMember]
		public string PermanentResidenceAddress
		{
			get { return _PermanentResidenceAddress; }
			set
			{
				this.OnPropertyValueChange(_.PermanentResidenceAddress, _PermanentResidenceAddress, value);
				this._PermanentResidenceAddress = value;
			}
		}
		/// <summary>
		/// HomeAddress 
		/// </summary>
		[DataMember]
		public string HomeAddress
		{
			get { return _HomeAddress; }
			set
			{
				this.OnPropertyValueChange(_.HomeAddress, _HomeAddress, value);
				this._HomeAddress = value;
			}
		}
		/// <summary>
		/// HomeNumber 
		/// </summary>
		[DataMember]
		public string HomeNumber
		{
			get { return _HomeNumber; }
			set
			{
				this.OnPropertyValueChange(_.HomeNumber, _HomeNumber, value);
				this._HomeNumber = value;
			}
		}
		/// <summary>
		/// SHAddress 
		/// </summary>
		[DataMember]
		public string SHAddress
		{
			get { return _SHAddress; }
			set
			{
				this.OnPropertyValueChange(_.SHAddress, _SHAddress, value);
				this._SHAddress = value;
			}
		}
		/// <summary>
		/// SHStreet 
		/// </summary>
		[DataMember]
		public string SHStreet
		{
			get { return _SHStreet; }
			set
			{
				this.OnPropertyValueChange(_.SHStreet, _SHStreet, value);
				this._SHStreet = value;
			}
		}
		/// <summary>
		/// SHEmail 
		/// </summary>
		[DataMember]
		public string SHEmail
		{
			get { return _SHEmail; }
			set
			{
				this.OnPropertyValueChange(_.SHEmail, _SHEmail, value);
				this._SHEmail = value;
			}
		}
		/// <summary>
		/// NearSubway 
		/// </summary>
		[DataMember]
		public string NearSubway
		{
			get { return _NearSubway; }
			set
			{
				this.OnPropertyValueChange(_.NearSubway, _NearSubway, value);
				this._NearSubway = value;
			}
		}
		/// <summary>
		/// FamilyInfo 
		/// </summary>
		[DataMember]
		public string FamilyInfo
		{
			get { return _FamilyInfo; }
			set
			{
				this.OnPropertyValueChange(_.FamilyInfo, _FamilyInfo, value);
				this._FamilyInfo = value;
			}
		}
		/// <summary>
		/// EducationInfo 
		/// </summary>
		[DataMember]
		public string EducationInfo
		{
			get { return _EducationInfo; }
			set
			{
				this.OnPropertyValueChange(_.EducationInfo, _EducationInfo, value);
				this._EducationInfo = value;
			}
		}
		/// <summary>
		/// WorkInfo 
		/// </summary>
		[DataMember]
		public string WorkInfo
		{
			get { return _WorkInfo; }
			set
			{
				this.OnPropertyValueChange(_.WorkInfo, _WorkInfo, value);
				this._WorkInfo = value;
			}
		}
		/// <summary>
		/// FileInfo 
		/// </summary>
		[DataMember]
		public string FileInfo
		{
			get { return _FileInfo; }
			set
			{
				this.OnPropertyValueChange(_.FileInfo, _FileInfo, value);
				this._FileInfo = value;
			}
		}
		/// <summary>
		/// HeadPortrait 
		/// </summary>
		[DataMember]
		public string HeadPortrait
		{
			get { return _HeadPortrait; }
			set
			{
				this.OnPropertyValueChange(_.HeadPortrait, _HeadPortrait, value);
				this._HeadPortrait = value;
			}
		}
		/// <summary>
		/// HireDate 
		/// </summary>
		[DataMember]
		public DateTime? HireDate
		{
			get { return _HireDate; }
			set
			{
				this.OnPropertyValueChange(_.HireDate, _HireDate, value);
				this._HireDate = value;
			}
		}
		/// <summary>
		/// TermDate 
		/// </summary>
		[DataMember]
		public DateTime? TermDate
		{
			get { return _TermDate; }
			set
			{
				this.OnPropertyValueChange(_.TermDate, _TermDate, value);
				this._TermDate = value;
			}
		}
		/// <summary>
		/// PositiveDates 
		/// </summary>
		[DataMember]
		public DateTime? PositiveDates
		{
			get { return _PositiveDates; }
			set
			{
				this.OnPropertyValueChange(_.PositiveDates, _PositiveDates, value);
				this._PositiveDates = value;
			}
		}
		/// <summary>
		/// ContractingCompany 
		/// </summary>
		[DataMember]
		public string ContractingCompany
		{
			get { return _ContractingCompany; }
			set
			{
				this.OnPropertyValueChange(_.ContractingCompany, _ContractingCompany, value);
				this._ContractingCompany = value;
			}
		}
		/// <summary>
		/// NumberCode 
		/// </summary>
		[DataMember]
		public string NumberCode
		{
			get { return _NumberCode; }
			set
			{
				this.OnPropertyValueChange(_.NumberCode, _NumberCode, value);
				this._NumberCode = value;
			}
		}
		/// <summary>
		/// CompanySkype 
		/// </summary>
		[DataMember]
		public string CompanySkype
		{
			get { return _CompanySkype; }
			set
			{
				this.OnPropertyValueChange(_.CompanySkype, _CompanySkype, value);
				this._CompanySkype = value;
			}
		}
		/// <summary>
		/// Creater 
		/// </summary>
		[DataMember]
		public string Creater
		{
			get { return _Creater; }
			set
			{
				this.OnPropertyValueChange(_.Creater, _Creater, value);
				this._Creater = value;
			}
		}
		/// <summary>
		/// CreateTime 
		/// </summary>
		[DataMember]
		public DateTime? CreateTime
		{
			get { return _CreateTime; }
			set
			{
				this.OnPropertyValueChange(_.CreateTime, _CreateTime, value);
				this._CreateTime = value;
			}
		}
		/// <summary>
		/// Privilege 
		/// </summary>
		[DataMember]
		public string Privilege
		{
			get { return _Privilege; }
			set
			{
				this.OnPropertyValueChange(_.Privilege, _Privilege, value);
				this._Privilege = value;
			}
		}
		/// <summary>
		/// LastLoginTime 
		/// </summary>
		[DataMember]
		public DateTime? LastLoginTime
		{
			get { return _LastLoginTime; }
			set
			{
				this.OnPropertyValueChange(_.LastLoginTime, _LastLoginTime, value);
				this._LastLoginTime = value;
			}
		}
		/// <summary>
		/// RetainJobbutSuspendSalary 
		/// </summary>
		[DataMember]
		public int? RetainJobbutSuspendSalary
		{
			get { return _RetainJobbutSuspendSalary; }
			set
			{
				this.OnPropertyValueChange(_.RetainJobbutSuspendSalary, _RetainJobbutSuspendSalary, value);
				this._RetainJobbutSuspendSalary = value;
			}
		}
		/// <summary>
		/// WithoutPayDate 
		/// </summary>
		[DataMember]
		public DateTime? WithoutPayDate
		{
			get { return _WithoutPayDate; }
			set
			{
				this.OnPropertyValueChange(_.WithoutPayDate, _WithoutPayDate, value);
				this._WithoutPayDate = value;
			}
		}
		/// <summary>
		/// ReinstatementDate 
		/// </summary>
		[DataMember]
		public DateTime? ReinstatementDate
		{
			get { return _ReinstatementDate; }
			set
			{
				this.OnPropertyValueChange(_.ReinstatementDate, _ReinstatementDate, value);
				this._ReinstatementDate = value;
			}
		}
		/// <summary>
		/// InitialAnnualLeave 
		/// </summary>
		[DataMember]
		public float? InitialAnnualLeave
		{
			get { return _InitialAnnualLeave; }
			set
			{
				this.OnPropertyValueChange(_.InitialAnnualLeave, _InitialAnnualLeave, value);
				this._InitialAnnualLeave = value;
			}
		}
		/// <summary>
		/// Status 
		/// </summary>
		[DataMember]
		public string Status
		{
			get { return _Status; }
			set
			{
				this.OnPropertyValueChange(_.Status, _Status, value);
				this._Status = value;
			}
		}
		/// <summary>
		/// MobilePhone 
		/// </summary>
		[DataMember]
		public string MobilePhone
		{
			get { return _MobilePhone; }
			set
			{
				this.OnPropertyValueChange(_.MobilePhone, _MobilePhone, value);
				this._MobilePhone = value;
			}
		}
		/// <summary>
		/// Institution 
		/// </summary>
		[DataMember]
		public string Institution
		{
			get { return _Institution; }
			set
			{
				this.OnPropertyValueChange(_.Institution, _Institution, value);
				this._Institution = value;
			}
		}
		/// <summary>
		/// DepartmentManagementList 部门管理列表（只有经理有数据）
		/// </summary>
		[DataMember]
		public string DepartmentManagementList
		{
			get { return _DepartmentManagementList; }
			set
			{
				this.OnPropertyValueChange(_.DepartmentManagementList, _DepartmentManagementList, value);
				this._DepartmentManagementList = value;
			}
		}
		/// <summary>
		/// AttendanceUser 
		/// </summary>
		[DataMember]
		public int? AttendanceUser
		{
			get { return _AttendanceUser; }
			set
			{
				this.OnPropertyValueChange(_.AttendanceUser, _AttendanceUser, value);
				this._AttendanceUser = value;
			}
		}
		/// <summary>
		/// LeaveApplicationCC 
		/// </summary>
		[DataMember]
		public string LeaveApplicationCC
		{
			get { return _LeaveApplicationCC; }
			set
			{
				this.OnPropertyValueChange(_.LeaveApplicationCC, _LeaveApplicationCC, value);
				this._LeaveApplicationCC = value;
			}
		}
		/// <summary>
		/// IdType 
		/// </summary>
		[DataMember]
		public string IdType
		{
			get { return _IdType; }
			set
			{
				this.OnPropertyValueChange(_.IdType, _IdType, value);
				this._IdType = value;
			}
		}
		/// <summary>
		/// WechatOpenID 微信OpenID
		/// </summary>
		[DataMember]
		public string WechatOpenID
		{
			get { return _WechatOpenID; }
			set
			{
				this.OnPropertyValueChange(_.WechatOpenID, _WechatOpenID, value);
				this._WechatOpenID = value;
			}
		}
		/// <summary>
		/// QQOpenID 企业QQ OpenID
		/// </summary>
		[DataMember]
		public string QQOpenID
		{
			get { return _QQOpenID; }
			set
			{
				this.OnPropertyValueChange(_.QQOpenID, _QQOpenID, value);
				this._QQOpenID = value;
			}
		}
		/// <summary>
		/// WechatTicket 微信二维码Ticket
		/// </summary>
		[DataMember]
		public string WechatTicket
		{
			get { return _WechatTicket; }
			set
			{
				this.OnPropertyValueChange(_.WechatTicket, _WechatTicket, value);
				this._WechatTicket = value;
			}
		}
		#endregion

		#region Method
		/// <summary>
		/// 获取实体中的主键列
		/// </summary>
		public override Field[] GetPrimaryKeyFields()
		{
			return new Field[] {
				_.ID};
		}
		/// <summary>
		/// 获取列信息
		/// </summary>
		public override Field[] GetFields()
		{
			return new Field[] {
				_.EmployeeNo,
				_.ID,
				_.RoleID,
				_.Password,
				_.LegalName,
				_.DisplayName,
				_.Department,
				_.Team,
				_.Position,
				_.IsLeader,
				_.DirectManagerID,
				_.DirectManager,
				_.Company,
				_.Location,
				_.SeatNumber,
				_.IdentityCard,
				_.Nationality,
				_.WorkType,
				_.PaymentType,
				_.SocialSecurity,
				_.SocialSecurityScale,
				_.AccumulationFund,
				_.AccumulationFundScale,
				_.AttendanceRecord,
				_.OKRRecord,
				_.Phone,
				_.ExtNumber,
				_.WxID,
				_.PersonWX,
				_.EMail,
				_.PersonEmail,
				_.BusinessQQ,
				_.EmergencyContact1,
				_.EmergencyContactRelation1,
				_.EmergencyContactAddress1,
				_.EmergencyContactNumber1,
				_.EmergencyContact2,
				_.EmergencyContactRelation2,
				_.EmergencyContactAddress2,
				_.EmergencyContactNumber2,
				_.BankCard,
				_.Contract,
				_.Sex,
				_.BirthdayType,
				_.Birthday,
				_.Birthplace,
				_.Birthphone,
				_.NativePlace,
				_.MarriageType,
				_.SpecialMedical,
				_.PermanentResidenceAddress,
				_.HomeAddress,
				_.HomeNumber,
				_.SHAddress,
				_.SHStreet,
				_.SHEmail,
				_.NearSubway,
				_.FamilyInfo,
				_.EducationInfo,
				_.WorkInfo,
				_.FileInfo,
				_.HeadPortrait,
				_.HireDate,
				_.TermDate,
				_.PositiveDates,
				_.ContractingCompany,
				_.NumberCode,
				_.CompanySkype,
				_.Creater,
				_.CreateTime,
				_.Privilege,
				_.LastLoginTime,
				_.RetainJobbutSuspendSalary,
				_.WithoutPayDate,
				_.ReinstatementDate,
				_.InitialAnnualLeave,
				_.Status,
				_.MobilePhone,
				_.Institution,
				_.DepartmentManagementList,
				_.AttendanceUser,
				_.LeaveApplicationCC,
				_.IdType,
				_.WechatOpenID,
				_.QQOpenID,
				_.WechatTicket};
		}
		/// <summary>
		/// 获取值信息
		/// </summary>
		public override object[] GetValues()
		{
			return new object[] {
				this._EmployeeNo,
				this._ID,
				this._RoleID,
				this._Password,
				this._LegalName,
				this._DisplayName,
				this._Department,
				this._Team,
				this._Position,
				this._IsLeader,
				this._DirectManagerID,
				this._DirectManager,
				this._Company,
				this._Location,
				this._SeatNumber,
				this._IdentityCard,
				this._Nationality,
				this._WorkType,
				this._PaymentType,
				this._SocialSecurity,
				this._SocialSecurityScale,
				this._AccumulationFund,
				this._AccumulationFundScale,
				this._AttendanceRecord,
				this._OKRRecord,
				this._Phone,
				this._ExtNumber,
				this._WxID,
				this._PersonWX,
				this._EMail,
				this._PersonEmail,
				this._BusinessQQ,
				this._EmergencyContact1,
				this._EmergencyContactRelation1,
				this._EmergencyContactAddress1,
				this._EmergencyContactNumber1,
				this._EmergencyContact2,
				this._EmergencyContactRelation2,
				this._EmergencyContactAddress2,
				this._EmergencyContactNumber2,
				this._BankCard,
				this._Contract,
				this._Sex,
				this._BirthdayType,
				this._Birthday,
				this._Birthplace,
				this._Birthphone,
				this._NativePlace,
				this._MarriageType,
				this._SpecialMedical,
				this._PermanentResidenceAddress,
				this._HomeAddress,
				this._HomeNumber,
				this._SHAddress,
				this._SHStreet,
				this._SHEmail,
				this._NearSubway,
				this._FamilyInfo,
				this._EducationInfo,
				this._WorkInfo,
				this._FileInfo,
				this._HeadPortrait,
				this._HireDate,
				this._TermDate,
				this._PositiveDates,
				this._ContractingCompany,
				this._NumberCode,
				this._CompanySkype,
				this._Creater,
				this._CreateTime,
				this._Privilege,
				this._LastLoginTime,
				this._RetainJobbutSuspendSalary,
				this._WithoutPayDate,
				this._ReinstatementDate,
				this._InitialAnnualLeave,
				this._Status,
				this._MobilePhone,
				this._Institution,
				this._DepartmentManagementList,
				this._AttendanceUser,
				this._LeaveApplicationCC,
				this._IdType,
				this._WechatOpenID,
				this._QQOpenID,
				this._WechatTicket};
		}
		#endregion

		#region _Field
		/// <summary>
		/// 字段信息
		/// </summary>
		public class _
		{
			/// <summary>
			/// OC_User_Info 
			/// </summary>
			public readonly static Field All = new Field("*", m_tableName);
			/// <summary>
			/// EmployeeNo 
			/// </summary>
			public readonly static Field EmployeeNo = new Field("EmployeeNo", m_tableName, DbType.Int32, 1, "EmployeeNo");
			/// <summary>
			/// ID 
			/// </summary>
			public readonly static Field ID = new Field("ID", m_tableName, "ID");
			/// <summary>
			/// RoleID 
			/// </summary>
			public readonly static Field RoleID = new Field("RoleID", m_tableName, "RoleID");
			/// <summary>
			/// Password 
			/// </summary>
			public readonly static Field Password = new Field("Password", m_tableName, "Password");
			/// <summary>
			/// LegalName 
			/// </summary>
			public readonly static Field LegalName = new Field("LegalName", m_tableName, "LegalName");
			/// <summary>
			/// DisplayName 
			/// </summary>
			public readonly static Field DisplayName = new Field("DisplayName", m_tableName, "DisplayName");
			/// <summary>
			/// Department 
			/// </summary>
			public readonly static Field Department = new Field("Department", m_tableName, "Department");
			/// <summary>
			/// Team 
			/// </summary>
			public readonly static Field Team = new Field("Team", m_tableName, "Team");
			/// <summary>
			/// Position 
			/// </summary>
			public readonly static Field Position = new Field("Position", m_tableName, "Position");
			/// <summary>
			/// IsLeader 
			/// </summary>
			public readonly static Field IsLeader = new Field("IsLeader", m_tableName, "IsLeader");
			/// <summary>
			/// DirectManagerID 
			/// </summary>
			public readonly static Field DirectManagerID = new Field("DirectManagerID", m_tableName, DbType.Int32, 1, "DirectManagerID");
			/// <summary>
			/// DirectManager 
			/// </summary>
			public readonly static Field DirectManager = new Field("DirectManager", m_tableName, "DirectManager");
			/// <summary>
			/// Company 
			/// </summary>
			public readonly static Field Company = new Field("Company", m_tableName, "Company");
			/// <summary>
			/// Location 
			/// </summary>
			public readonly static Field Location = new Field("Location", m_tableName, "Location");
			/// <summary>
			/// SeatNumber 
			/// </summary>
			public readonly static Field SeatNumber = new Field("SeatNumber", m_tableName, "SeatNumber");
			/// <summary>
			/// IdentityCard 
			/// </summary>
			public readonly static Field IdentityCard = new Field("IdentityCard", m_tableName, "IdentityCard");
			/// <summary>
			/// Nationality 
			/// </summary>
			public readonly static Field Nationality = new Field("Nationality", m_tableName, "Nationality");
			/// <summary>
			/// WorkType 
			/// </summary>
			public readonly static Field WorkType = new Field("WorkType", m_tableName, "WorkType");
			/// <summary>
			/// PaymentType 
			/// </summary>
			public readonly static Field PaymentType = new Field("PaymentType", m_tableName, "PaymentType");
			/// <summary>
			/// SocialSecurity 
			/// </summary>
			public readonly static Field SocialSecurity = new Field("SocialSecurity", m_tableName, DbType.Int32, 1, "SocialSecurity");
			/// <summary>
			/// SocialSecurityScale 
			/// </summary>
			public readonly static Field SocialSecurityScale = new Field("SocialSecurityScale", m_tableName, DbType.Single, 1, "SocialSecurityScale");
			/// <summary>
			/// AccumulationFund 
			/// </summary>
			public readonly static Field AccumulationFund = new Field("AccumulationFund", m_tableName, DbType.Int32, 1, "AccumulationFund");
			/// <summary>
			/// AccumulationFundScale 
			/// </summary>
			public readonly static Field AccumulationFundScale = new Field("AccumulationFundScale", m_tableName, DbType.Single, 1, "AccumulationFundScale");
			/// <summary>
			/// AttendanceRecord 
			/// </summary>
			public readonly static Field AttendanceRecord = new Field("AttendanceRecord", m_tableName, DbType.Int32, 1, "AttendanceRecord");
			/// <summary>
			/// OKRRecord 
			/// </summary>
			public readonly static Field OKRRecord = new Field("OKRRecord", m_tableName, DbType.Int32, 1, "OKRRecord");
			/// <summary>
			/// Phone 
			/// </summary>
			public readonly static Field Phone = new Field("Phone", m_tableName, "Phone");
			/// <summary>
			/// ExtNumber 
			/// </summary>
			public readonly static Field ExtNumber = new Field("ExtNumber", m_tableName, "ExtNumber");
			/// <summary>
			/// WxID 
			/// </summary>
			public readonly static Field WxID = new Field("WxID", m_tableName, "WxID");
			/// <summary>
			/// PersonWX 
			/// </summary>
			public readonly static Field PersonWX = new Field("PersonWX", m_tableName, "PersonWX");
			/// <summary>
			/// EMail 
			/// </summary>
			public readonly static Field EMail = new Field("EMail", m_tableName, "EMail");
			/// <summary>
			/// PersonEmail 
			/// </summary>
			public readonly static Field PersonEmail = new Field("PersonEmail", m_tableName, "PersonEmail");
			/// <summary>
			/// BusinessQQ 
			/// </summary>
			public readonly static Field BusinessQQ = new Field("BusinessQQ", m_tableName, "BusinessQQ");
			/// <summary>
			/// EmergencyContact1 
			/// </summary>
			public readonly static Field EmergencyContact1 = new Field("EmergencyContact1", m_tableName, "EmergencyContact1");
			/// <summary>
			/// EmergencyContactRelation1 
			/// </summary>
			public readonly static Field EmergencyContactRelation1 = new Field("EmergencyContactRelation1", m_tableName, "EmergencyContactRelation1");
			/// <summary>
			/// EmergencyContactAddress1 
			/// </summary>
			public readonly static Field EmergencyContactAddress1 = new Field("EmergencyContactAddress1", m_tableName, "EmergencyContactAddress1");
			/// <summary>
			/// EmergencyContactNumber1 
			/// </summary>
			public readonly static Field EmergencyContactNumber1 = new Field("EmergencyContactNumber1", m_tableName, "EmergencyContactNumber1");
			/// <summary>
			/// EmergencyContact2 
			/// </summary>
			public readonly static Field EmergencyContact2 = new Field("EmergencyContact2", m_tableName, "EmergencyContact2");
			/// <summary>
			/// EmergencyContactRelation2 
			/// </summary>
			public readonly static Field EmergencyContactRelation2 = new Field("EmergencyContactRelation2", m_tableName, "EmergencyContactRelation2");
			/// <summary>
			/// EmergencyContactAddress2 
			/// </summary>
			public readonly static Field EmergencyContactAddress2 = new Field("EmergencyContactAddress2", m_tableName, "EmergencyContactAddress2");
			/// <summary>
			/// EmergencyContactNumber2 
			/// </summary>
			public readonly static Field EmergencyContactNumber2 = new Field("EmergencyContactNumber2", m_tableName, "EmergencyContactNumber2");
			/// <summary>
			/// BankCard 
			/// </summary>
			public readonly static Field BankCard = new Field("BankCard", m_tableName, "BankCard");
			/// <summary>
			/// Contract 
			/// </summary>
			public readonly static Field Contract = new Field("Contract", m_tableName, "Contract");
			/// <summary>
			/// Sex 
			/// </summary>
			public readonly static Field Sex = new Field("Sex", m_tableName, "Sex");
			/// <summary>
			/// BirthdayType 
			/// </summary>
			public readonly static Field BirthdayType = new Field("BirthdayType", m_tableName, "BirthdayType");
			/// <summary>
			/// Birthday 
			/// </summary>
			public readonly static Field Birthday = new Field("Birthday", m_tableName, DbType.DateTime, 1, "Birthday");
			/// <summary>
			/// Birthplace 
			/// </summary>
			public readonly static Field Birthplace = new Field("Birthplace", m_tableName, "Birthplace");
			/// <summary>
			/// Birthphone 
			/// </summary>
			public readonly static Field Birthphone = new Field("Birthphone", m_tableName, "Birthphone");
			/// <summary>
			/// NativePlace 
			/// </summary>
			public readonly static Field NativePlace = new Field("NativePlace", m_tableName, "NativePlace");
			/// <summary>
			/// MarriageType 
			/// </summary>
			public readonly static Field MarriageType = new Field("MarriageType", m_tableName, "MarriageType");
			/// <summary>
			/// SpecialMedical 
			/// </summary>
			public readonly static Field SpecialMedical = new Field("SpecialMedical", m_tableName, "SpecialMedical");
			/// <summary>
			/// PermanentResidenceAddress 
			/// </summary>
			public readonly static Field PermanentResidenceAddress = new Field("PermanentResidenceAddress", m_tableName, "PermanentResidenceAddress");
			/// <summary>
			/// HomeAddress 
			/// </summary>
			public readonly static Field HomeAddress = new Field("HomeAddress", m_tableName, "HomeAddress");
			/// <summary>
			/// HomeNumber 
			/// </summary>
			public readonly static Field HomeNumber = new Field("HomeNumber", m_tableName, "HomeNumber");
			/// <summary>
			/// SHAddress 
			/// </summary>
			public readonly static Field SHAddress = new Field("SHAddress", m_tableName, "SHAddress");
			/// <summary>
			/// SHStreet 
			/// </summary>
			public readonly static Field SHStreet = new Field("SHStreet", m_tableName, "SHStreet");
			/// <summary>
			/// SHEmail 
			/// </summary>
			public readonly static Field SHEmail = new Field("SHEmail", m_tableName, "SHEmail");
			/// <summary>
			/// NearSubway 
			/// </summary>
			public readonly static Field NearSubway = new Field("NearSubway", m_tableName, "NearSubway");
			/// <summary>
			/// FamilyInfo 
			/// </summary>
			public readonly static Field FamilyInfo = new Field("FamilyInfo", m_tableName, "FamilyInfo");
			/// <summary>
			/// EducationInfo 
			/// </summary>
			public readonly static Field EducationInfo = new Field("EducationInfo", m_tableName, "EducationInfo");
			/// <summary>
			/// WorkInfo 
			/// </summary>
			public readonly static Field WorkInfo = new Field("WorkInfo", m_tableName, "WorkInfo");
			/// <summary>
			/// FileInfo 
			/// </summary>
			public readonly static Field FileInfo = new Field("FileInfo", m_tableName, "FileInfo");
			/// <summary>
			/// HeadPortrait 
			/// </summary>
			public readonly static Field HeadPortrait = new Field("HeadPortrait", m_tableName, "HeadPortrait");
			/// <summary>
			/// HireDate 
			/// </summary>
			public readonly static Field HireDate = new Field("HireDate", m_tableName, DbType.DateTime, 1, "HireDate");
			/// <summary>
			/// TermDate 
			/// </summary>
			public readonly static Field TermDate = new Field("TermDate", m_tableName, DbType.DateTime, 1, "TermDate");
			/// <summary>
			/// PositiveDates 
			/// </summary>
			public readonly static Field PositiveDates = new Field("PositiveDates", m_tableName, DbType.DateTime, 1, "PositiveDates");
			/// <summary>
			/// ContractingCompany 
			/// </summary>
			public readonly static Field ContractingCompany = new Field("ContractingCompany", m_tableName, "ContractingCompany");
			/// <summary>
			/// NumberCode 
			/// </summary>
			public readonly static Field NumberCode = new Field("NumberCode", m_tableName, "NumberCode");
			/// <summary>
			/// CompanySkype 
			/// </summary>
			public readonly static Field CompanySkype = new Field("CompanySkype", m_tableName, "CompanySkype");
			/// <summary>
			/// Creater 
			/// </summary>
			public readonly static Field Creater = new Field("Creater", m_tableName, "Creater");
			/// <summary>
			/// CreateTime 
			/// </summary>
			public readonly static Field CreateTime = new Field("CreateTime", m_tableName, DbType.DateTime, 1, "CreateTime");
			/// <summary>
			/// Privilege 
			/// </summary>
			public readonly static Field Privilege = new Field("Privilege", m_tableName, "Privilege");
			/// <summary>
			/// LastLoginTime 
			/// </summary>
			public readonly static Field LastLoginTime = new Field("LastLoginTime", m_tableName, DbType.DateTime, 1, "LastLoginTime");
			/// <summary>
			/// RetainJobbutSuspendSalary 
			/// </summary>
			public readonly static Field RetainJobbutSuspendSalary = new Field("RetainJobbutSuspendSalary", m_tableName, DbType.Int32, 1, "RetainJobbutSuspendSalary");
			/// <summary>
			/// WithoutPayDate 
			/// </summary>
			public readonly static Field WithoutPayDate = new Field("WithoutPayDate", m_tableName, DbType.DateTime, 1, "WithoutPayDate");
			/// <summary>
			/// ReinstatementDate 
			/// </summary>
			public readonly static Field ReinstatementDate = new Field("ReinstatementDate", m_tableName, DbType.DateTime, 1, "ReinstatementDate");
			/// <summary>
			/// InitialAnnualLeave 
			/// </summary>
			public readonly static Field InitialAnnualLeave = new Field("InitialAnnualLeave", m_tableName, DbType.Single, 1, "InitialAnnualLeave");
			/// <summary>
			/// Status 
			/// </summary>
			public readonly static Field Status = new Field("Status", m_tableName, "Status");
			/// <summary>
			/// MobilePhone 
			/// </summary>
			public readonly static Field MobilePhone = new Field("MobilePhone", m_tableName, "MobilePhone");
			/// <summary>
			/// Institution 
			/// </summary>
			public readonly static Field Institution = new Field("Institution", m_tableName, "Institution");
			/// <summary>
			/// DepartmentManagementList 部门管理列表（只有经理有数据）
			/// </summary>
			public readonly static Field DepartmentManagementList = new Field("DepartmentManagementList", m_tableName, "部门管理列表（只有经理有数据）");
			/// <summary>
			/// AttendanceUser 
			/// </summary>
			public readonly static Field AttendanceUser = new Field("AttendanceUser", m_tableName, DbType.Int32, 1, "AttendanceUser");
			/// <summary>
			/// LeaveApplicationCC 
			/// </summary>
			public readonly static Field LeaveApplicationCC = new Field("LeaveApplicationCC", m_tableName, "LeaveApplicationCC");
			/// <summary>
			/// IdType 
			/// </summary>
			public readonly static Field IdType = new Field("IdType", m_tableName, "IdType");
			/// <summary>
			/// WechatOpenID 微信OpenID
			/// </summary>
			public readonly static Field WechatOpenID = new Field("WechatOpenID", m_tableName, "微信OpenID");
			/// <summary>
			/// QQOpenID 企业QQ OpenID
			/// </summary>
			public readonly static Field QQOpenID = new Field("QQOpenID", m_tableName, "企业QQ OpenID");
			/// <summary>
			/// WechatTicket 微信二维码Ticket
			/// </summary>
			public readonly static Field WechatTicket = new Field("WechatTicket", m_tableName, "微信二维码Ticket");
		}
		#endregion


	}
	/// <summary>
	/// 实体类DBUserInfo操作类
	/// </summary>
	public partial class DBUserInfoRepository : BaseRepository<DBUserInfo>
	{
		/// <summary>
		/// DBUserInfo构造方法
		/// </summary>
		public DBUserInfoRepository() : base()
		{
			_db = new DBContext();
		}
		/// <summary>
		/// DBUserInfo构造方法
		/// </summary>
		public DBUserInfoRepository(DBContext dbContext) : base(dbContext)
		{
			_db = dbContext;
		}
		/// <summary>
		/// DBUserInfo构造方法
		/// <param name="connStrName">连接字符串中的名称</param>
		/// </summary>
		public DBUserInfoRepository(string connStrName) : base(connStrName)
		{
			_db = new DBContext(connStrName);
		}
		/// <summary>
		/// DBUserInfo构造方法
		/// <param name="dbType">数据库类型</param>
		/// <param name="connStr">连接字符串</param>
		/// </summary>
		public DBUserInfoRepository(DatabaseType dbType, string connStr) : base(dbType, connStr)
		{
			_db = new DBContext(dbType, connStr);
		}
	}

}
