using Microsoft.AspNetCore.Http;
using Nikan.Common.GlobalEnum;
using Nikan.ViewModel.Group;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Citizens
{


    /// <summary>
    /// ثبت نام شهروند
    /// </summary>
    public class CitizensRegisterDto
    {



        /// <summary>
        /// کد ملی شهروند
        /// </summary>
        public string NationCode { get; set; }

        /// <summary>
        /// ملیت شهروند
        /// </summary>
        public int Nationality { get; set; }

        /// <summary>
        /// شماره موبایل
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// نام
        /// </summary>
        public string FirstName { get; set; }


        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string LastName { get; set; }


        /// <summary>
        /// وضعیت تحصیلی
        /// </summary>
        public EducationStatuesEnum? EducationStatues { get; set; }

        /// <summary>
        /// سطح تحصیلات
        /// </summary>
        public GradeEnum? EducationLevel { get; set; }

        /// <summary>
        ///جدول گروه تحصیلی
        /// گروه تحصیلی
        /// </summary>
        public int? EducationGroup { get; set; }


        /// <summary>
        /// رشته تحصیلی
        /// </summary>
        public string EducationTitle { get; set; }


        /// <summary>
        /// عنوان شغلی
        /// </summary>
        public string JobTitle { get; set; }


        /// <summary>
        /// جدول گروه شغلی
        /// کروه شغلی
        /// </summary>
        public int? JobGroup { get; set; }


        /// <summary>
        /// شماره تلفن منزل
        /// </summary>
        public string PhoneNumber { get; set; }


        /// <summary>
        /// شهرمحل زندگی
        /// </summary>
        public int? CityId { get; set; }


        /// <summary>
        /// منطقه
        /// </summary>
        public int? Region { get; set; }


        /// <summary>
        /// آدرس کامل
        /// </summary>
        public string FullAddress { get; set; }

        /// <summary>
        /// خیابان
        /// </summary>
        public string Street { get; set; }


        /// <summary>
        /// کوچه
        /// </summary>
        public string Alley { get; set; } 

        /// <summary>
        /// پلاک
        /// </summary>
        public string Plaque { get; set; }
        /// <summary>
        /// کدپستی
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// آدرس ایمیل
        /// </summary>
        public string EMail { get; set; }


        /// <summary>
        /// نام پدر
        /// </summary>
        public string FatherName { get; set; }


        /// <summary>
        /// تاریخ تولد
        /// </summary>
        public DateTime BirthDate { get; set; }


        /// <summary>
        /// جنسیت
        /// </summary>
        public bool Gender { get; set; }



        /// <summary>
        /// وضعیت تاهل
        /// </summary>
        public MaritalStatusEnum MariageStatus { get; set; }

        /// <summary>
        /// کلمه عبور
        /// </summary>
        public string Password { get; set; }



        /// <summary>
        /// سوال امنیتی
        /// </summary>
        public string PasswordQuestion { get; set; }


        /// <summary>
        /// پاسخ امنیتی
        /// </summary>
        public string PasswordAnswer { get; set; }


        /// <summary>
        ///  در چه سرویسی ثبت نام کرده است
        /// </summary>
        public int? ServiceId { get; set; }



        /// <summary>
        ///  کد اعتبارسنجی
        /// </summary>
        public string VerifyCode { get; set; }





    }

    public class QuickCitizensRegisterDto
    {

        /// <summary>
        /// جنسیت
        /// </summary>
        public bool Gender { get; set; }



        /// <summary>
        /// کد ملی شهروند
        /// </summary>
        public string NationCode { get; set; }

        /// <summary>
        /// ملیت شهروند
        /// </summary>
        public int Nationality { get; set; }

        /// <summary>
        /// شماره موبایل
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// نام
        /// </summary>
        public string FirstName { get; set; }


        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string LastName { get; set; }
         
  
        /// <summary>
        /// نام پدر
        /// </summary>
        public string FatherName { get; set; }


        /// <summary>
        /// تاریخ تولد
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// آدرس ایمیل
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// کلمه عبور
        /// </summary>
        public string Password { get; set; }
         

        /// <summary>
        ///  در چه سرویسی ثبت نام کرده است
        /// </summary>
        public int? ServiceId { get; set; }
         
    }

     
    public class PreRegisterResult
    {

        public bool  IsCanRegister { get; set; }
        public string  Description  { get; set; }




    }



        /// <summary>
        /// پیش ثبت نام
        /// </summary>
        public class PreRegisterDto
    {


        /// <summary>
        /// ملیت 
        /// 0 ایرانی
        /// 1 غیرایرانی
        /// </summary>
        public int Nationality { get; set; }
        /// <summary>
        /// کد ملی 
        /// یا شماره گذرنامه
        /// </summary>
        public string NationCode { get; set; }
        /// <summary>
        /// شماره موبایل
        /// </summary>
        public string MobileNumber { get; set; }

        
        /// <summary>
        /// شناسه سرویس
        /// </summary>
        public int? ServiceId { get; set; }

        



    }

     

    /// <summary>
    /// نتیجه ثبت نام شهروند
    /// </summary>
    public class CitizenRegisterResult
    {
        public int CitizenId { get; set; }
        public int UserId { get; set; }

        public string Redirect { get; set; }

    }



    /// <summary>
    /// اطلاعات شناسنامه ایی
    /// </summary>
    public class CitizenIdentityInformation 
    {

        /// <summary>
        /// شناسه شهروندی
        /// </summary>
        public int CitizenId { get; set; }



        /// <summary>
        /// شناسه شهروندی
        /// </summary>
        public string  UserCode { get; set; }

        /// <summary>
        ///
        /// جنسیت شهروند
        /// true آقا
        /// false خانم
        /// </summary>
        public bool Gender { get; set; }

 
       
         /// <summary>
         /// شماره شناسنامه
         /// </summary>
         public string IdentityId { get; set; }

        /// <summary>
        /// نام شهروند
        /// </summary>
        public string FirstName { get; set; }


        /// <summary>
        /// نام خانوادگی شهروند
        /// </summary>
        public string LastName { get; set; }


        /// <summary>
        /// نام پدر شهروند
        /// </summary>
        public string FatherName { get; set; }


       


        /// <summary>
        /// کد ملی شهروند
        /// </summary>
        public string NationCode { get; set; }


        

        /// <summary>
        /// تاریخ تولد شهروند
        /// </summary>
        public DateTime? BirthDate { get; set; }

        

 
        public DateTime? Date_SabtConfirm { get; set; }

        /// <summary>
        /// تاریخ ثبت نام
        /// </summary>
        public DateTime CreationDate { get; set; }


        public DateTime? LastUpdateOnDate { get; set; }



        /// <summary>
        /// وضعیت تایید هویت
        /// </summary>
        public SabtStatusEnum SabtStatus { get; set; }


        

    }




    public class UpdateSabtStatus
    {


        /// <summary>
        /// شناسه شهروندی
        /// </summary>
        public string UserCode { get; set; }


        public SabtStatusEnum SabtStatus { get; set; }


    }


    /// <summary>
    /// نمایش سایر اطلاعات شهروند
    /// </summary>
    public class EditOtherBaseInfoViewModel
    {


        public int CitizenId { get; set; }
        

        /// <summary>
        /// شماره موبایل شهروند
        /// </summary>
        public string MobileNumber { get; set; }


        /// <summary>
        /// وضعیت تاهل شهروند
        /// </summary>
        public MaritalStatusEnum MariageStatus { get; set; }



        /// <summary>
        /// وضعیت تحصیلی
        /// </summary>
        public EducationStatuesEnum? EducationStatues { get; set; }



        /// <summary>
        /// گروه تحصیلی
        /// </summary>
        public int? EducationGroup { get; set; }

        /// <summary>
        /// عنوان شغلی
        /// </summary>
        public string JobTitle { get; set; }


        /// <summary>
        /// گروه شغلی
        /// </summary>
        public int? JobGroup { get; set; }


        /// <summary>
        /// رشته تحصیلی
        /// </summary>
        public string EducationTitle { get; set; }

        /// <summary>
        /// سطح تحصیلات
        /// </summary>
        public GradeEnum? EducationLevel { get; set; }


        /// <summary>
        /// شماره تلفن منزل
        /// </summary>
        public string PhoneNumber { get; set; }


        /// <summary>
        /// استان و  شهر محل زندگی
        /// </summary>
        public int CityId { get; set; }


        /// <summary>
        /// منطقه
        /// </summary>
        public int? Region { get; set; }


        /// <summary>
        /// آدرس کامل محل زندگی
        /// </summary>
        public string FullAddress { get; set; }


        /// <summary>
        /// کد پستی محل زندگی
        /// </summary>
        public string PostalCode { get; set; }


        public string Street { get; set; }
        public string Alley { get; set; }

        public string Plaque { get; set; }



        /// <summary>
        /// آدرس ایمیل
        /// </summary>
        public string EMail { get; set; }


    }






    /// <summary>
    /// دریافت اطلاعات اولیه شهروند
    /// </summary>
    public class CitizenBaseInfo
    {
        /// <summary>
        /// شناسه شهروندی
        /// </summary>
        public int CitizenId { get; set; }


        public Guid? UserCode { get; set; }


        /// <summary>
        ///
        /// جنسیت شهروند
        /// true آقا
        /// false خانم
        /// </summary>
        public bool Gender { get; set; }


        /// <summary>
        /// نام کاربری شهروند
        /// </summary>
        public virtual string  User { get; set; }


        /// <summary>
        /// ملیت 
        /// 0 ایرانی
        /// 1 غیرایرانی
        /// </summary>
        public int Nationality { get; set; }


        /// <summary>
        /// نام شهروند
        /// </summary>
        public string FirstName { get; set; }


        /// <summary>
        /// نام خانوادگی شهروند
        /// </summary>
        public string LastName { get; set; }


        /// <summary>
        /// نام پدر شهروند
        /// </summary>
        public string FatherName { get; set; } 


        /// <summary>
        /// از طریق چه سرویسی ثبت نام کرده است
        /// </summary>
        public string  RegisterByService { get; set; }


        /// <summary>
        /// کد ملی شهروند
        /// </summary>
        public string NationCode { get; set; }


        /// <summary>
        /// وضعیت تاهل شهروند
        /// </summary>
        public MaritalStatusEnum? MariageStatus { get; set; }
        
        /// <summary>
        /// تاریخ تولد شهروند
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// شماره موبایل شهروند
        /// </summary>
        public string MobileNumber { get; set; }


       

        /// <summary>
        /// سطح تحصبلات شهروند
        /// </summary>
        public GradeEnum? EducationLevel { get; set; }


        /// <summary>
        /// گروه تحصیلی شهروند
        /// </summary>
        public int? EducationGroupId { get; set; }
        public string  EducationGroup { get; set; }

        /// <summary>
        /// وضعیت تحصیلی شهروند
        /// </summary>
        public EducationStatuesEnum? EducationStatues { get; set; }


        public string EducationField { get; set; }


        public int? JobGroupId { get; set; }
        public string  JobGroup { get; set; }


        public string JobTitle { get; set; }
       
        public int CityId { get; set; }
        
        
        
        /// <summary>
        /// شهر و استان محل تولد
        /// </summary>
        public BaseDataModel City { get; set; }
       

        /// <summary>
        /// منطقه
        /// </summary>
        public int? Region { get; set; }

        /// <summary>
        /// آدرس کامل
        /// </summary>
        public string FullAddress { get; set; }


        public string Street { get; set; }
        public string Alley { get; set; }
     
       
        /// <summary>
        /// تلفن ثابت
        /// </summary>
        public string Phone { get; set; }


        /// <summary>
        /// کدپستی
        /// </summary>
        public string PostalCode { get; set; }


        /// <summary>
        /// پلاک
        /// </summary>
        public string Plaque { get; set; }


        public string EMail { get; set; }


        public DateTime? Date_SabtConfirm { get; set; }
    
        /// <summary>
        /// تاریخ ثبت نام
        /// </summary>
        public DateTime CreationDate { get; set; }


        public DateTime? LastUpdateOnDate { get; set; }



        /// <summary>
        /// وضعیت تایید هویت
        /// </summary>
        public SabtStatusEnum  SabtStatus { get; set; }


        /// <summary>
        /// وضعیت تایید تصویر پرسنلی
        /// </summary>
        public PersonalPictureEnum? PersonalPicture_Confirmed { get; set; }
       
        
        /// <summary>
        /// دلیل رد تصویر پرسنلی
        /// </summary>
        public string PersonalPicture_DisapprovalReason { get; set; }

    }


    public class ShortCitizenInfo
    {
        /// <summary>
        /// شناسه شهروندی
        /// </summary>
        public int CitizenId { get; set; }
        public Guid? UserCode { get; set; }




        /// <summary>
        ///
        /// جنسیت شهروند
        /// true آقا
        /// false خانم
        /// </summary>
        public bool Gender { get; set; }

         



        /// <summary>
        /// نام شهروند
        /// </summary>
        public string FirstName { get; set; }


        /// <summary>
        /// نام خانوادگی شهروند
        /// </summary>
        public string LastName { get; set; }


        /// <summary>
        /// نام پدر شهروند
        /// </summary>
        public string FatherName { get; set; }

public string RequestId { get; set; }

        /// <summary>
        /// ملیت شهروند
        /// 0 ایرانی
        /// 1 غیرایرانی
        /// </summary>
        public int Nationality { get; set; }
         
         public int Age { get; set; }








        /// <summary>
        /// کد ملی شهروند
        /// </summary>
        public string NationCode { get; set; }

        /// <summary>
        /// ثبت نام شده توسط کدام خدمت
        /// </summary>
        public string RegisterByService { get; set; }






      public string AuthenticationByService { get; set; }








        /// <summary>
        /// وضعیت ثبت احوال شما
        /// </summary>
        public SabtStatusEnum SabtStatus { get; set; }


        /// <summary>
        /// شماره موبایل شهروند
        /// </summary>
        public string MobileNumber { get; set; }

      

        /// <summary>
        /// تاریخ ثبت نام
        /// </summary>
        public DateTime CreationDate { get; set; }
       
        

        /// <summary>
        /// تاریخ تولد شهروند
        /// </summary>
         public DateTime? BirthDate { get; set; }
       
        /// <summary>
        /// آخرین بروزرسانی اطلاعات
        /// </summary>
        public DateTime? LastUpdateOnDate { get; set; }
       


        
        /// <summary>
        /// آیا تصویر شهروند بارگذاری شده است ؟
        /// 
        /// </summary>
        public bool   PersonalPictureIsUploaded  { get; set; }



        public string FullAddress { get; set; }

        public string PostalCode { get; set; }
        /// <summary>
        /// مسیر تصویر
        /// </summary>
        public string    PersonalPictureUrl { get; set; }

        /// <summary>
        /// وضعیت تصویر پرسنلی
        /// </summary>
        public PersonalPictureEnum? PersonalPicture_Confirmed { get; set; }

        public   IEnumerable<string>  Groups { get; set; }
        /// <summary>
        /// مسیر برگشت
        /// </summary>
        public string ReturnUrl { get; set; }

    }


    /// <summary>
    /// اطلاعات تصاویر شهروند
    /// </summary>
    public class ImageCitizenInfo
    {
        /// <summary>
        /// شناسه شهروندی
        /// </summary>
        public int CitizenId { get; set; }




        public Guid? UserCode { get; set; }

        /// <summary>
        ///
        /// جنسیت شهروند
        /// true آقا
        /// false خانم
        /// </summary>
        public bool Gender { get; set; }




        /// <summary>
        /// نام شهروند
        /// </summary>
        public string FirstName { get; set; }


        /// <summary>
        /// نام خانوادگی شهروند
        /// </summary>
        public string LastName { get; set; }


       


        /// <summary>
        /// ملیت شهروند
        /// 0 ایرانی
        /// 1 غیرایرانی
        /// </summary>
        public int Nationality { get; set; }

        /// <summary>
        /// کد ملی شهروند
        /// </summary>
        public string NationCode { get; set; }

        

        /// <summary>
        /// وضعیت ثبت احوال شما
        /// </summary>
        public SabtStatusEnum SabtStatus { get; set; }


       
        /// <summary>
        /// تاریخ ثبت نام
        /// </summary>
        public DateTime CreationDate { get; set; }



        /// <summary>
        /// تاریخ تولد شهروند
        /// </summary>
        public DateTime? BirthDate { get; set; }


        public int Age { get; set; }

        /// <summary>
        /// آیا تصویر شهروند بارگذاری شده است ؟
        /// 
        /// </summary>
        public bool PersonalPictureIsUploaded { get; set; }


        /// <summary>
        /// مسیر تصویر
        /// </summary>
        public string PersonalPictureUrl { get; set; }

        /// <summary>
        /// وضعیت تصویر پرسنلی
        /// </summary>
        public PersonalPictureEnum? PersonalPicture_Confirmed { get; set; }

    }


    /// <summary>
    /// ویرایش اطلاعات شهروند
    /// </summary>
    public class EditCitizenViewModel
    {


        public int CitizenId { get; set; }



        public Guid?  UserCode { get; set; }


        /// <summary>
        /// جنسیت
        /// </summary>
        public bool Gender { get; set; }

        /// <summary>
        /// نام شهروند
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// نام و نام خانوادگی
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// نام پدر
        /// </summary>
        public string FatherName { get; set; }
        /// <summary>
        /// تاریخ تولد
        /// </summary>
        public DateTime? BirthDate { get; set; }


        /// <summary>
        /// شماره موبایل شهروند
        /// </summary>
        public string MobileNumber { get; set; }


        /// <summary>
        /// وضعیت تاهل شهروند
        /// </summary>
        public MaritalStatusEnum MariageStatus { get; set; }



        /// <summary>
        /// وضعیت تحصیلی
        /// </summary>
        public EducationStatuesEnum? EducationStatues { get; set; }



        /// <summary>
        /// گروه تحصیلی
        /// </summary>
        public int? EducationGroup { get; set; }

        /// <summary>
        /// عنوان شغلی
        /// </summary>
        public string JobTitle { get; set; }


        /// <summary>
        /// گروه شغلی
        /// </summary>
        public int? JobGroup { get; set; }


        /// <summary>
        /// رشته تحصیلی
        /// </summary>
        public string EducationTitle { get; set; }

        /// <summary>
        /// سطح تحصیلات
        /// </summary>
        public GradeEnum? EducationLevel { get; set; }


        /// <summary>
        /// شماره تلفن منزل
        /// </summary>
        public string PhoneNumber { get; set; }


        /// <summary>
        /// استان و  شهر محل زندگی
        /// </summary>
        public int CityId { get; set; }


        /// <summary>
        /// منطقه
        /// </summary>
        public int? Region { get; set; }


        /// <summary>
        /// آدرس کامل محل زندگی
        /// </summary>
        public string FullAddress { get; set; }


        /// <summary>
        /// کد پستی محل زندگی
        /// </summary>
        public string PostalCode { get; set; }


        public string Street { get; set; }
        public string Alley { get; set; }
      
        public string Plaque { get; set; }



        /// <summary>
        /// آدرس ایمیل
        /// </summary>
        public string EMail { get; set; }


    }

    /// <summary>
    /// رد تصویر شهروند
    /// </summary>
    public class RejectCitizenPicture
    {
        /// <summary>
        /// شناسه شهروندی
        /// </summary>
        public int CitizenId { get; set; }

        public bool SendSms { get; set; }


        /// <summary>
        /// دلیل رد تصویر شهروند
        /// </summary>
        public string Reason { get; set; }

    }


    public class CheckValidMobileNumberViewModel 
    { 
        /// <summary>
        ///  شماره موبایل جدید
        /// </summary>
        public string NewMobileNumber { get; set; }

    }


    public class PersonalPictureState
    {

        public int CitizenId { get; set; } 
        public string NationCode { get; set; } 
        public PersonalPictureEnum? PersonalPicture_Confirmed { get; set; }
    }

    /// <summary>
    /// اطلاعات کامل شهروند
    /// </summary>
    public class CitizenFullInfo
    {
        

        public int CitizenId { get; set; }
        public virtual string  User { get; set; }
        public virtual Guid? UserCode { get; set; }
        public virtual string AddByUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
     
        public string PersonalPictureUrl { get; set; }

        public string FatherName { get; set; }
        public int RegisterByServiceId { get; set; }
        public string  RegisterByService { get; set; }  
        public string NationCode { get; set; }
        public bool Gender { get; set; }
        public MaritalStatusEnum? MariageStatus { get; set; }
        public DateTime? BirthDate { get; set; }
        public string MobileNumber { get; set; }
        public GradeEnum? EducationLevel { get; set; }
        public int? EducationGroupId { get; set; }
        public string  EducationGroup { get; set; }
        public EducationStatuesEnum? EducationStatues { get; set; } 
        public string EducationField { get; set; }


        public int? JobGroupId { get; set; }
        public string  JobGroup { get; set; }


        public string JobTitle { get; set; } 
         
        public string EMail { get; set; }


        public DateTime? Date_SabtConfirm { get; set; }
        public DateTime CreationDate { get; set; }
        public SabtStatusEnum SabtStatus { get; set; }



        public int? NationId { get; set; }
        public string  Nation { get; set; }



        public DateTime? LastPictureUploadOnDate { get; set; }
        public PersonalPictureEnum? PersonalPicture_Confirmed { get; set; }
        public string PersonalPicture_DisapprovalReason { get; set; }

        public DateTime? LastUpdateOnDate { get; set; }

        public AddressInfo Address { get; set; }

        public CitizenHistory CitizenHistory { get; set; }
        


        public List<GroupCitizensInfo> Groups { get; set; }

    }


    public class UploadCitizenPicture
    {
        /// <summary>
        /// شناسه شهروندی
        /// </summary>
        public int CitizenId { get; set; } 
        public IFormFile File { get; set; }

    }

    public class PagedCitizenViewModel
    {
        public int TotalItems { get; set; }
        public List<ShortCitizenInfo> Citizens { get; set; }

    }

   
    public class CitizenHistory
    {
        public DateTime? RegsiterOnDate { get; set; }
        public DateTime? LastPictureUploadOnDate { get; set; }
        public bool?  PictureIsAccept { get; set; }
        public DateTime? SendToSabtAhval { get; set; } 
        public bool? SabtAhvalIsAccept { get; set; }

        public DateTime? BuyCard { get; set; }

        public string  CardDescription { get; set; }

        public string CardsendToPrintDescription { get; set; }


        public DateTime?  CardSendToPrint { get; set; }

        public DateTime? CardPrinted { get; set; }


        public DateTime? SendCardToCitizenAddress { get; set; }


        public string  CardRequstState { get; set; }

    }


    public class QueueCheckingCitizensDeadDto
    {
        public int Id { get; set; }
        public string NationCode { get; set; }
        public int Priority { get; set; }

    }

}
