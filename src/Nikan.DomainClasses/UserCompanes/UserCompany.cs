using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.UserCompanes
{
    public class UserCompany
    {
        [Key]
        public int Id { get; set; }
         
        public UserCompany()
        {
            LastModifiedOnDate = DateTime.Now;
        }





      


        #region اطلاعات اصلی شرکت


        [StringLength(500)]
        public string CompanyName { get; set; }

        [StringLength(50)] 
        public string DisplayName { get; set; }



        [StringLength(40)]
        public string EnglishName { get; set; }



        [StringLength(40)]
        public string SlagUrl { get; set; }


        public string Content { get; set; }
       
      

        /// <summary>
        /// شماره بیمه
        /// </summary>
        public string InsuranceNumber { get; set; }



        [StringLength(100)]
        public string CompanyRepresentative { get; set; }//نماینده  شرکت

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



        #endregion
        #region اطلاعات مدیریت

        [StringLength(10)]
        public string ManagerNationCode { get; set; }

        public string ManagerName { get; set; }
         

        #endregion
        #region  اطلاعات ثبت نام در شهرک

        /// <summary>
        /// تاریخ تشکیل پرونده در شهرک
        /// </summary>
        public DateTime? RegistrationDate { get; set; }
         
        /// <summary>
        /// کد رهگیری ثبت نام
        /// </summary>
        public string  RegistrationCode { get; set; }
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
        #region اطلاعات تماس

        [StringLength(50)]
        public string MobileNumber { get; set; }


        [StringLength(50)]
        public string MobileNumber2 { get; set; }


        [StringLength(50)]
        public string MobileNumber3 { get; set; }

        public string Lat { get; set; }

        public string Lng { get; set; }



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


        #endregion
        #region اطلاعات آدرس
         
        /// <summary>
        /// استان وشهرستان
        /// ایدی شهرستان باید دریافت شود
        /// </summary>
        public virtual City City { get; set; }
        public int? CityId { get; set; }

         
        [StringLength(100)]
        public string ZipCode { get; set; }
       
      

        [StringLength(100)]
        public string Street { get; set; }

        [StringLength(1000)]
        public string FullAddress { get; set; }

        [StringLength(100)]
        public string Pelak { get; set; }
        #endregion
         
      
        #region مشخصات محیطی شرکت
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
        #endregion
        #region اطلاعات تکمیل

        /// <summary>
        /// تعداد پرسنل شرکت
        /// </summary>
        public int NumberOfEmployees { get; set; }

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
         public string   WaterCode { get; set; }


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



        #region  پیوست ها

        [StringLength(500)]
        public string ImageUrl { get; set; }

        [StringLength(500)]
        public string ThumbnailUrl { get; set; }

        public string ContractUrl { get; set; }
        public string SignatureUrl { get; set; }


        #endregion


        public DateTime CreatedOnDate { get; set; }
        /// <summary>
        /// آخرین ویرایش اطلاعات شرکت
        /// </summary>
        public DateTime? LastModifiedOnDate { get; set; }


        public bool LockEdit { get; set; }
        public DateTime? LockOnDate { get; set; }


        public bool IsDeleted { get; set; }


        /// <summary>
        /// حوزه های فعالیت شرکت
        /// </summary>
        public virtual ICollection<UserCompanyActivities> UserCompanyActivities { get; set; }






    }


    /// <summary>
    /// پرسنل شرکت
    /// </summary>
    public class UserCompanyPersonel
    {

        public int Id { get; set; }
        public string PersonelCode { get; set; }

        /// <summary>
        /// شرکت
        /// </summary>
        public int? UserCompanyId { get; set; }
        public virtual UserCompany UserCompany { get; set; }

        /// <summary>
        /// پست سازمانی
        /// </summary>
        public int OrganizationalPositionId { get; set; }
        public virtual OrganizationalPosition OrganizationalPosition { get; set; }

        public virtual NamePrefixEnum NamePrefix { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [StringLength(50)]
        public string FatherName { get; set; }
        [StringLength(50)]
        public string NationCode { get; set; }
        [StringLength(50)]
        public string MobileNumber { get; set; }
        [StringLength(50)]
        public string CellNumber { get; set; }



        [StringLength(100)]
        public string Email { get; set; }

        public virtual City City { get; set; }
        public int? CityId { get; set; }

        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }



        [StringLength(100)]
        public string ZipCode { get; set; }



        [StringLength(100)]
        public string Street { get; set; }

        [StringLength(1000)]
        public string FullAddress { get; set; }

        [StringLength(100)]
        public string Pelak { get; set; }


        public string Office { get; set; }

        public string OfficePhoneNumber { get; set; }

        public DateTime? EmployeementOnDate { get; set; }

        public bool IsDeleted { get; set; }


        public string Biography { get; set; }
        public bool IsManagementMembers { get; set; }

        /// <summary>
        /// آیا دارای بیماری خاصی است ؟
        /// </summary>
        public bool HasSpecificDisease { get; set; }

        /// <summary>
        /// توضیحات بیماری
        /// </summary>
        public string DescriptionDisease { get; set; }

        



    }

    public class UserCompanyFieldOfActivity  
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }


        [Required]
        [StringLength(100)]
        public string Title { get; set; }


        public bool IsActive { get; set; }



    

    }

    public class UserCompanyActivities
    {
        public int Id { get; set; }

        public UserCompanyFieldOfActivity Activity { get; set; } 
        public int ActivityId { get; set; }


        public UserCompany UserCompany { get; set; }
        public int UserCompanyId { get; set; }



    }

     




}
