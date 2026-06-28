using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;

namespace Nikan.ViewModel.Citizens
{
    /// <summary>
    /// ثبت پروفایل شهروند
    /// </summary>
    public class CitizenProfileDto
    {


       
        public Guid? UserCode { get; set; }

        public int CitizenId { get; set; }
       

        /// <summary>
        /// کد پرسنلی 
        /// </summary>
        public string PersonnelCode { get; set; }
      

        /// <summary>
        /// شماره شناسنامه
        /// </summary>
        public string ShCode { get; set; }

        /// <summary>
        /// سریال شناسنامه
        /// </summary>
        public string ShSerial { get; set; }
        
        /// <summary>
        /// تاریخ صدور شناسنامه
        /// </summary>
        public DateTime? ShDate { get; set; }


        /// <summary>
        /// محل صدور شناسنامه
        /// </summary> 
        public int? ShCityId { get; set; }



        /// <summary>
        /// بخش صدور شناسنامه
        /// </summary>
        public string ShCitySection { get; set; }


        /// <summary>
        /// توضیحات شناسنامه
        /// </summary>
        public string ShNote { get; set; }



        /// <summary>
        /// تاریخ استخدام
        /// </summary>
        public DateTime? DateOfEmployeement { get; set; }


        /// <summary>
        /// وضعیت خدمت
        /// </summary>
        public SoldierStateEnum? SoldierState { get; set; }
        
        
        /// <summary>
        /// تاریخ پایان خدمت
        /// </summary>
        public DateTime? EndOfMilitary { get; set; }


        /// <summary>
        /// دین
        /// </summary>
        public CitizenProfileReligionEnum? Religion { get; set; }

        

         /// <summary>
         /// شهر محل تولد
         /// </summary>
        public int? CityOfBirthId { get; set; }
        /// <summary>
        /// بخش /روستا/دهستان محل تولد
        /// </summary>
        public string VillageOfBirth { get; set; }

        /// <summary>
        /// بخش محل تولد
        /// </summary>
        public string BirthCitySection { get; set; }




        /// <summary>
        /// تاریخ ازدواج
        /// </summary>
        public DateTime? DateOfMarriage { get; set; }

     
        /// <summary>
        /// پایه تحصیلی
        /// </summary>
        public string BaseEducation { get; set; }


        /// <summary>
        /// نام دانشگاه
        /// </summary>
        public string UniversityName { get; set; }


        /// <summary>
        /// معدل 
        /// </summary>
        public string AcademicGrade { get; set; }


        /// <summary>
        /// توضیحات اضافه
        /// </summary>
        public string AcademicNote { get; set; }

        /// <summary>
        /// تاریخ پایان تحصیلات
        /// </summary>
        public DateTime? EndOfEducation { get; set; }

        /// <summary>
        /// وضعیت تحصیلی
        /// </summary>
        public EducationStatuesEnum? EducationStatues { get; set; }



        /// <summary>
        /// شماره بیمه
        /// </summary>
        public string InsuranceNumber { get; set; }

       



    }

    /// <summary>
    /// نمایش اطلاعات پروفایل شهروند
    /// </summary>
    public class CitizenProfileInfo
    {

        

        public int CitizenId { get; set; }
        public virtual string   Citizen { get; set; }

        public Guid? UserCode { get; set; }


        public string PersonnelCode { get; set; }
        
        public string ShCode { get; set; }

       
        public string ShSerial { get; set; }
        public DateTime? ShDate { get; set; }


        /// <summary>
        /// محل صدور شناسنامه
        /// </summary>
        public BaseDataModel ProvinceShCity { get; set; }
        public int? ShCityId { get; set; }
        public string  ShCity  { get; set; }


        public BaseDataModel ProvinceCityOfBirth { get; set; }
        public int? CityOfBirthId { get; set; }
        public string  CityOfBirth  { get; set; }




        public string ShCitySection { get; set; }
        public string ShNote { get; set; }
        public DateTime? DateOfEmployeement { get; set; }


        public SoldierStateEnum? MilitaryStatus { get; set; }
        public DateTime? EndOfMilitary { get; set; }

        public CitizenProfileReligionEnum? Religion { get; set; }

        public string VillageOfBirth { get; set; }

      



        public DateTime? DateOfMarriage { get; set; }

        public string BirthCitySection { get; set; }

        public string BaseEducation { get; set; }

        public string UniversityName { get; set; }

        public string AcademicGrade { get; set; }
        public string AcademicNote { get; set; }


        public DateTime? EndOfEducation { get; set; }

        public EducationStatuesEnum? EducationStatues { get; set; }

        public string InsuranceNumber { get; set; }

        public bool BankCardNumber_Confirmed { get; set; }

        public string BankCardNumber { get; set; }



    }


    /// <summary>
    /// اطلاعات بانکی شهروندی
    /// </summary>
    public class BankAccountCardNumberDto
    {

        /// <summary>
        /// شناسه شهروند
        /// </summary>
        public int? CitizenId { get; set; }



        /// <summary>
        /// کد اعتبارسنجی تایید شماره کارت
        /// </summary>
        public string VerifyCode { get; set; }


        /// <summary>
        /// شماره کارت 16 رقمی
        /// </summary>
        public string CardNumber { get; set; }


        /// <summary>
        ///  شماره شبا
        /// </summary>
        public string ShabaNumber { get; set; }

        /// <summary>
        /// شماره موبایل شهروند
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// نام صاحب کارت
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// کد ملی شهروند
        /// </summary>
        public string OwnerNationCode { get; set; }


        /// <summary>
        /// تایید شماره کارت
        /// </summary>
        public bool? BankCardNumber_Confirmed { get; set; }



    }


    public class UpdateBankAccountCardNumberDto
    {


        /// <summary>
        /// شناسه شهروند
        /// </summary>
        public int? CitizenId { get; set; }  
        /// <summary>
        /// شماره کارت 16 رقمی
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// شماره شبا
        /// </summary>
        public string ShabaNumber { get; set; }



    }


    public class IsCitizenIsRegister
    { 
        public string NationCode { get; set; }
         
    }

    public class IsCitizenIsRegisterResult
    {
        /// <summary>
        /// آیا ثبت نام کرده است ؟
        /// </summary>
        public bool IsRegister { get; set; } 
         
        /// <summary>
        /// تاریخ ثبت نام
        /// </summary>
        public DateTime RegisterOnDate { get; set; }


        /// <summary>
        /// وضعیت تصویر پرسنلی
        /// </summary>
        public PersonalPictureEnum? PersonalPicture_Confirmed { get; set; }



        public string  MobileNumber { get; set; }


    }




    




    public class PagedImageCitizenViewModel
    {
        public int TotalItems { get; set; }
        public List<ImageCitizenInfo> Citizens { get; set; }

    }


    public class UpdateMobileNumberDto
    {



        /// <summary>
        /// شناسه شهروند
        /// </summary>
        public int? CitizenId { get; set; }



        public string UserCode { get; set; }


        /// <summary>
        ///شماره موبایل شهروند
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// آیا شماره موبایل تایید شده است ؟
        /// </summary>
        public bool IsConfirm { get; set; }

        /// <summary>
        ///  کد پیامک تایید شماره موبایل شهروند
        /// </summary>
        public string SmsActiveCode { get; set; }



    }



    public class CitizenVerifyMobileNumberDto
    { 
        /// <summary>
        ///شماره موبایل شهروند
        /// </summary>
        public string NewMobileNumber { get; set; }


    }

    public class CitizenSabtStatus
    {


        /// <summary>
        /// شناسه شهروند
        /// </summary>
        public int? CitizenId { get; set; }
        public SabtStatusEnum SabtStatus { get; set; }


    }


    public class UpdateEmailAddressDto
    {


        /// <summary>
        /// شناسه شهروند
        /// </summary>
        public int? CitizenId { get; set; }
     
        /// <summary>
        ///آدرس ایمیل شهروند
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        ///  کد   تایید ادرس ایمیل شهروند
        /// </summary>
        public string EmailActiveCode { get; set; }



    }




}
