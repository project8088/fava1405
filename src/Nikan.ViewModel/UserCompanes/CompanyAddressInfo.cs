using Microsoft.AspNetCore.Http;
using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nikan.ViewModel.UserCompanes
{

    public class AdminCompanyRegisterResult
    {
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
    }
    public class AdminCompanyRegister2
    {


        public string CompanyName { get; set; }

 

    }

    public class AdminCompanyRegister
    {

        
        public string CompanyName { get; set; }


        
        public string EnglishName { get; set; }


        /// <summary>
        /// نام و نام خانوادگی نماینده شرکت
        /// </summary>
        
        public string CompanyRepresentative { get; set; }//نماینده  شرکت



        /// <summary>
        /// سال تاسیس شرکت
        /// </summary>
        
        public string EstablishedYear { get; set; }//  سال تاسیس




        /// <summary>
        /// شماره اقتصادی شرکت
        /// </summary>
        public string TxtTinNo { get; set; }


        /// <summary>
        /// شماره ثبت
        /// </summary>
        public string TxtRegNO { get; set; }
        /// <summary>
        /// شماره موبایل
        /// </summary>
        public string MobileNumber { get; set; }
        public string Email { get; set; }
         
        public string UserName { get; set; }

        /// <summary>
        /// کد قرارداد
        /// </summary>
        public int? ContractCode { get; set; }


        public string Password { get; set; }

    }

    public class CompanyLogo
    {


        public int? CompanyId { get; set; } 
        public virtual string CompanyName { get; set; } 
        public string ImageUrl { get; set; }


        public IFormFile file { get; set; }


    }
    public class CompanyShortInfo
    {


        public int? CompanyId { get; set; }
        public virtual string CompanyName { get; set; }
        public string ImageUrl { get; set; }

        /// <summary>
        /// شماره اقتصادی شرکت
        /// </summary>
        public string TxtTinNo { get; set; }
       
        
        public string ManagerName { get; set; }

        /// <summary>
        /// شماره ثبت
        /// </summary>
        public string TxtRegNO { get; set; }

        public string SlagUrl { get; set; }
       
        public UserCompanyStatusEnum UserCompanyStatus { get; set; }

        public DateTime CreatedOnDate { get; set; }
    }

    public class ChangeCompanyAccountStatus
    {


        public int CompanyId { get; set; }
        
        public string RejectDesription { get; set; }

        public UserCompanyAccountStatusEnum UserCompanyAccountStatus { get; set; }


        public bool SendSms { get; set; }
    }




    public class CompanySignatureInfo
    { 
        public int? CompanyId { get; set; }
        public virtual string CompanyName { get; set; }
        public string SignatureUrl { get; set; }

        public IFormFile file { get; set; }
    }


    public class CompanyContractInfo
    {
        public int? CompanyId { get; set; }
        public virtual string CompanyName { get; set; }
        public string ContractUrl { get; set; }


        public IFormFile file { get; set; }



    }



    public class CompanyAddressInfo
    {


        public int? CompanyId { get; set; }

        #region اطلاعات تماس

        [StringLength(50)]
        public string MobileNumber { get; set; }


        public string MobileNumber2 { get; set; }


        public string MobileNumber3 { get; set; }

        [StringLength(50)]
        public string CellNumber { get; set; }


        [StringLength(50)]
        public string CellNumber2 { get; set; }

        [StringLength(50)]
        public string CellNumber3 { get; set; }


        [StringLength(50)]
        public string SMSNumber { get; set; }
        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(100)]
        public string Website { get; set; }
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Telegram { get; set; }



        /// <summary>
        /// استان وشهرستان
        /// ایدی شهرستان باید دریافت شود
        /// </summary>
        public string City { get; set; }
        public int? CityId { get; set; }

        public BaseDataModel Province { get; set; }




        [StringLength(100)]
        public string ZipCode { get; set; }



        [StringLength(100)]
        public string Street { get; set; }

        [StringLength(1000)]
        public string FullAddress { get; set; }

        [StringLength(100)]
        public string Pelak { get; set; }
        #endregion


        public string Lat { get; set; }

        public string Lng { get; set; }


    }
    public class CompanyMainInfo
    {


        public int? CompanyId { get; set; }

        #region اطلاعات اصلی شرکت


        [StringLength(40)]
        public string SlagUrl { get; set; }


        public string Content { get; set; }



        /// <summary>
        /// شماره بیمه
        /// </summary>
        public string InsuranceNumber { get; set; }



        [StringLength(100)]
        public string CompanyRepresentative { get; set; }//نماینده  شرکت



        public int NumberOfEmployees { get; set; }


        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }

        #endregion

    }
   
    public class CompanyBaseInfo
    {


        public int? CompanyId { get; set; }

        [StringLength(500)]
        public string CompanyName { get; set; }

        [StringLength(40)]
        public string EnglishName { get; set; }


        [StringLength(10)]
        public string EstablishedYear { get; set; }//  سال تاسیس


        /// <summary>
        /// شماره اقتصادی شرکت
        /// </summary>
        public string TxtTinNo { get; set; }


        /// <summary>
        /// شماره ثبت
        /// </summary>
        public string TxtRegNO { get; set; }


        /// <summary>
        /// نوع/اتحادیه صنفی	
        /// </summary>
        public virtual CompanyOwnerTypeEnum CompanyOwnerType { get; set; }

        /// <summary>
        /// نوع پروانه فعالیت
        /// </summary>
        public virtual ActivityLicenseTypeEnum ActivityLicenseType { get; set; }

        /// <summary>
        /// نوع مجوز
        /// در صورت داشتن پروانه صنعتی
        /// </summary>
        public virtual ActivityLicenseEnum ActivityLicense { get; set; }


        /// <summary>
        /// تاریخ ثبت/پروانه کسب/تاریخ تاسیس	
        /// </summary>
        public DateTime? ActivityLicenseDate { get; set; }

        /// <summary>
        /// نوع فعالیت شرکت
        /// </summary>
        public FieldOfActivityEnum FieldOfActivity { get; set; }


        [StringLength(10)]
        public string ManagerNationCode { get; set; }

        public string ManagerName { get; set; }



        /// <summary>
        /// نوع زمین کارگاه
        /// </summary>
        public EarthConditionEnum EarthCondition { get; set; }
        /// <summary>
        /// مساحت زمین شرکت
        /// </summary>
        public decimal UnitArea { get; set; }
        /// <summary>
        /// مساحت فضای سبز
        /// </summary>
        public int AreaOfGreenSpace { get; set; }
        /// <summary>
        /// مساحت احداث شده
        /// </summary>
        public int BuildingArea { get; set; }
        /// <summary>
        /// مثراژ مجوز احداث
        /// </summary>
        public int BuildingLicenseArea { get; set; }


    }

    public class CompanyAdditionalInfo
    {


        public int  CompanyId { get; set; }

        #region اطلاعات تکمیل
         
        /// <summary>
        /// کد قرارداد
        /// </summary>
        public int? ContractCode { get; set; }

        /// <summary>
        /// تاریخ قرارداد
        /// </summary>
        public DateTime? ContractOnDate { get; set; }

        /// <summary>
        /// تاریخ قرارداد آب
        /// </summary>
        public DateTime? WaterContractOnDate { get; set; }

        /// <summary>
        /// شماره پرونده
        /// </summary>
        public string FileCode { get; set; }

        /// <summary>
        /// شناسه واریز قیض آب
        /// </summary>
        public string WaterDepositId { get; set; }


        /// <summary>
        /// کد تفصیلی آب
        /// </summary>
        public string WaterCode { get; set; }


        /// <summary>
        /// شناسه واریزی قبض شارژ
        /// </summary>
        public string ChargeDepositId { get; set; }


        /// <summary>
        /// کد تفصیلی شارژ
        /// </summary>
        public string ChargeCode { get; set; }


        /// <summary>
        /// شارژ معین کد
        /// </summary>
        public string ChargeMoeinCode { get; set; }

        /// <summary>
        /// حجم مخازن هوایی
        /// </summary>
        public int VolumeAirTanks { get; set; }


        /// <summary>
        /// این مشترک دارای واحد تجاری است ؟
        /// </summary>
        public bool IsBusinessUnit { get; set; }


        /// <summary>
        /// این شرکت در حال ساخت می باشد ؟
        /// </summary>
        public bool IsBuildingCompany { get; set; }

        /// <summary>
        /// قبض آب صادر شود ؟
        /// </summary>
        public bool IssueWaterBill { get; set; }

        /// <summary>
        /// قبض شارژ صادر شود ؟
        /// </summary>
        public bool IssueChargeBill { get; set; }

        #endregion


    }



    public class CompanyInfoDto
    {
        public int CompanyId { get; set; }
        [StringLength(500)]
        public string CompanyName { get; set; }

        [StringLength(40)]
        public string EnglishName { get; set; }


        [StringLength(10)]
        public string EstablishedYear { get; set; }//  سال تاسیس


        /// <summary>
        /// شماره اقتصادی شرکت
        /// </summary>
        public string TxtTinNo { get; set; }


        /// <summary>
        /// شماره ثبت
        /// </summary>
        public string TxtRegNO { get; set; }


        /// <summary>
        /// نوع/اتحادیه صنفی	
        /// </summary>
        public virtual CompanyOwnerTypeEnum CompanyOwnerType { get; set; }

        /// <summary>
        /// نوع پروانه فعالیت
        /// </summary>
        public virtual ActivityLicenseTypeEnum ActivityLicenseType { get; set; }

        /// <summary>
        /// نوع مجوز
        /// در صورت داشتن پروانه صنعتی
        /// </summary>
        public virtual ActivityLicenseEnum ActivityLicense { get; set; }


        /// <summary>
        /// تاریخ ثبت/پروانه کسب/تاریخ تاسیس	
        /// </summary>
        public DateTime? ActivityLicenseDate { get; set; }

        /// <summary>
        /// نوع فعالیت شرکت
        /// </summary>
        public FieldOfActivityEnum FieldOfActivity { get; set; }


        [StringLength(10)]
        public string ManagerNationCode { get; set; }

        public string ManagerName { get; set; }



        /// <summary>
        /// نوع زمین کارگاه
        /// </summary>
        public EarthConditionEnum EarthCondition { get; set; }
        /// <summary>
        /// مساحت زمین شرکت
        /// </summary>
        public decimal UnitArea { get; set; }
        /// <summary>
        /// مساحت فضای سبز
        /// </summary>
        public int AreaOfGreenSpace { get; set; }
        /// <summary>
        /// مساحت احداث شده
        /// </summary>
        public int BuildingArea { get; set; }
        /// <summary>
        /// مثراژ مجوز احداث
        /// </summary>
        public int BuildingLicenseArea { get; set; }

        #region اطلاعات اصلی شرکت


        [StringLength(40)]
        public string SlagUrl { get; set; }


        public string Content { get; set; }



        /// <summary>
        /// شماره بیمه
        /// </summary>
        public string InsuranceNumber { get; set; }



        [StringLength(100)]
        public string CompanyRepresentative { get; set; }//نماینده  شرکت







        #endregion
        #region اطلاعات تماس

        [StringLength(50)]
        public string MobileNumber { get; set; }

        public string MobileNumber2 { get; set; }
        public string MobileNumber3 { get; set; }



        [StringLength(50)]
        public string CellNumber { get; set; }

        public string CellNumber2 { get; set; }
     
        public string CellNumber3 { get; set; }


       public string Lat { get; set; }

        public string Lng { get; set; }





        [StringLength(50)]
        public string SMSNumber { get; set; }
        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(100)]
        public string Website { get; set; }
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Telegram { get; set; }



        /// <summary>
        /// استان وشهرستان
        /// ایدی شهرستان باید دریافت شود
        /// </summary>
        public string City { get; set; }
        public int? CityId { get; set; }


        [StringLength(100)]
        public string ZipCode { get; set; }

        public int NumberOfEmployees { get; set; }

        [StringLength(100)]
        public string Street { get; set; }

        [StringLength(1000)]
        public string FullAddress { get; set; }

        [StringLength(100)]
        public string Pelak { get; set; }
        #endregion



        #region اطلاعات تکمیل

       
        /// <summary>
        /// کد قرارداد
        /// </summary>
        public int? ContractCode { get; set; }

        public DateTime? ContractOnDate { get; set; }

        public DateTime? WaterContractOnDate { get; set; }

        /// <summary>
        /// شماره پرونده
        /// </summary>
        public string FileCode { get; set; }

        /// <summary>
        /// شناسه واریز قیض آب
        /// </summary>
        public string WaterDepositId { get; set; }


        /// <summary>
        /// کد تفصیلی آب
        /// </summary>
        public string WaterCode { get; set; }


        /// <summary>
        /// شناسه واریزی قبض شارژ
        /// </summary>
        public string ChargeDepositId { get; set; }


        /// <summary>
        /// کد تفصیلی شارژ
        /// </summary>
        public string ChargeCode { get; set; }


        /// <summary>
        /// شارژ معین کد
        /// </summary>
        public string ChargeMoeinCode { get; set; }


        public string WaterMoeinCode { get; set; }



        /// <summary>
        /// حجم مخازن هوایی
        /// </summary>
        public int VolumeAirTanks { get; set; }


        /// <summary>
        /// این مشترک دارای واحد تجاری است ؟
        /// </summary>
        public bool IsBusinessUnit { get; set; }


        /// <summary>
        /// این شرکت در حال ساخت می باشد ؟
        /// </summary>
        public bool IsBuildingCompany { get; set; }

        /// <summary>
        /// قبض آب صادر شود ؟
        /// </summary>
        public bool IssueWaterBill { get; set; }

        /// <summary>
        /// قبض شارژ صادر شود ؟
        /// </summary>
        public bool IssueChargeBill { get; set; }

        #endregion


        #region  اطلاعات ثبت نام در شهرک

        public bool LockEdit { get; set; }


        /// <summary>
        /// تاریخ تشکیل پرونده در شهرک
        /// </summary>
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// کد رهگیری ثبت نام
        /// </summary>
        public string RegistrationCode { get; set; }
        public UserCompanyStatusEnum UserCompanyStatus { get; set; }



        /// <summary>
        /// دلیل عدم تایید 
        /// </summary>
        public string RejectDesription { get; set; }

        /// <summary>
        /// وضعیت حساب کاربری شرکت
        /// </summary>
        public UserCompanyAccountStatusEnum UserCompanyAccountStatus { get; set; }


        #endregion

        #region  پیوست ها

        [StringLength(500)]
        public string ImageUrl { get; set; }

        [StringLength(500)]
        public string ThumbnailUrl { get; set; }

        public string ContractUrl { get; set; }
        public string SignatureUrl { get; set; }


        #endregion



        public List<UserCompanyActivitiyInfo> UserCompanyActivities { get; set; }


    }
    public class PagedCompaniesViewModel
    {
         
        public List<CompanyInfoDto> companies { get; set; }
         
        public int TotalItems { get; set; }


    }





}