using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;

namespace Nikan.ViewModel.Citizens
{


    public class CitizenFamiliesInfo
    {

        public int Id { get; set; }

        public int CitizenId { get; set; }

        public Guid? UserCode { get; set; }


        public string  Citizen { get; set; }
        
        /// <summary>
        /// کد ملی شهروند
        /// </summary>
        public string NationCode { get; set; }


        /// <summary>
        /// شناسه شهروند نسبت
        /// </summary>
        public int FamilyCitizenId { get; set; }


        public Guid? FamilyUserCode { get; set; }




        /// <summary>
        /// نام و نام خانوادگی نسبت
        /// </summary>
        public string  FamilyCitizen { get; set; }


        /// <summary>
        /// کد ملی عضو خانواده
        /// </summary>
        public string FamilyNationCode { get; set; }


        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// توسط چه کاربری تایید شده است
        /// </summary>
        public string ConfirmerUser { get; set; }

        public int  FamilyAge { get; set; }

        public DateTime? ConfirmDate { get; set; }
      
        public DateTime? FamilyBirthDate { get; set; }

        public MaritalStatusEnum? FamilyMariageStatus { get; set; }

        public bool? Confirm { get; set; }


        public DateTime? AcceptDate { get; set; }
        public FamilyRelationshipsEnum FamilyRelation { get; set; }
      
        /// <summary>
        /// تحت تکلف است ؟
        /// </summary>
        public bool? UnderProtection { get; set; }

        /// <summary>
        /// مسیر تصویر عضو خانواده
        /// </summary>
        public string FamilyPictureUrl { get; set; }



        /// <summary>
        /// آیا جز وراث است ؟
        /// </summary>
        public bool? Heirs { get; set; }

    }

    public class CitizenFamiliesViewModel
    {
        /// <summary>
        /// شناسه شهروند
        /// </summary>
        public int CitizenId { get; set; }
        public int Nationality { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public int? RegisterByServiceId { get; set; }
        public string RegisterByServiceName { get; set; }
        public string NationCode { get; set; }
        public bool Gender { get; set; }

        /// <summary>
        /// نسبت 
        /// </summary>
        public FamilyRelationshipsEnum FamilyRelationship { get; set; }

        public DateTime BirthDate { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }


        public int EducationGroup { get; set; }
        public string EducationField { get; set; }
        public int JobGroup { get; set; }
        public string JobTitle { get; set; }
        public int City { get; set; }
        public string CityName { get; set; }
        public int Region { get; set; }
        public string FullAddress { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string EMail { get; set; }
        public string Scan_MelliCard { get; set; }
        public string Scan_PersonalPicture { get; set; }
        public PersonalPictureEnum? PersonalPicture_Confirmed { get; set; }
        public bool DataIsLock { get; set; }
        public string VerifyCode { get; set; }
        public bool CodeVerified { get; set; }
        public int? ConfirmedCenterID { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public DateTime? Date_SabtConfirm { get; set; }
        public DateTime? Date_LastEdit { get; set; }
        public string esfshahrvandCode { get; set; }
        public DateTime? esfshahrvandCode_Date { get; set; }
        public int? RegisteredCitizen { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreationDate { get; set; }
        


        public SabtStatusEnum? SabtStatus { get; set; }
         


    }


    public class CheckFamilyModel
    {
        /// <summary>
        /// کد ملی عضو خانواده
        /// </summary>
        public string NationCode { get; set; }

        /// <summary>
        /// تاریخ تولد
        /// </summary>
        public DateTime BirthDate { get; set; } 

        /// <summary>
        /// نسبت خانوادگی
        /// </summary>
        public  FamilyRelationshipsEnum FamilyRelation { get; set; }




    }

    public class AddFamilyCitizenDto
    {

        public FamilyRelationshipsEnum? FamilyRelation { get; set; }

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
        public DateTime BirthDate { get; set; }


        /// <summary>
        /// شماره موبایل شهروند
        /// </summary>
        public string Mobile { get; set; }


/// <summary>
/// کد ملی عضو جدید خانواده
/// </summary>

 public string NationCode { get; set; }










        /// <summary>
        /// وضعیت تاهل شهروند
        /// </summary>
        public MaritalStatusEnum MariageStatus { get; set; }



        /// <summary>
        /// وضعیت تحصیلی
        /// </summary>
        public EducationStatuesEnum EducationStatues { get; set; }



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


        /// <summary>
        /// آدرس ایمیل
        /// </summary>
        public string EMail { get; set; }


    }


    public class UpdateFamilyCitizenDto
    {

        public FamilyRelationshipsEnum? FamilyRelation { get; set; }


        public int? FamilyCitizenId { get; set; }


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
        public DateTime BirthDate { get; set; }


        /// <summary>
        /// شماره موبایل شهروند
        /// </summary>
        public string Mobile { get; set; }


        /// <summary>
        /// کد ملی عضو جدید خانواده
        /// </summary>

        public string NationCode { get; set; }
         


        /// <summary>
        /// وضعیت تاهل شهروند
        /// </summary>
        public MaritalStatusEnum MariageStatus { get; set; }



        /// <summary>
        /// وضعیت تحصیلی
        /// </summary>
        public EducationStatuesEnum EducationStatues { get; set; }



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
        /// شماره تلفن منزل
        /// </summary>
        public string PhoneNumber { get; set; }


        /// <summary>
        /// استان و  شهر محل زندگی
        /// </summary>
        public int CityId { get; set; }


        public string Street { get; set; }
        public string Alley { get; set; }
    
        public string Plaque { get; set; }
        

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


        /// <summary>
        /// آدرس ایمیل
        /// </summary>
        public string EMail { get; set; }


    }




    /// <summary>
    /// حذف عضو خانواده
    /// </summary>
    public class DeleteFamilly
    {

        /// <summary>
        /// شناسه شهروند
        /// </summary>
        public string  UserCode { get; set; }


        /// <summary>
        /// شناسه عضو خانوادگی
        /// </summary>
        public int FamillyId { get; set; }

        /// <summary>
        /// دلیل حذف نسبت خانوادگی
        /// </summary>
        public string  Description  { get; set; }


    }

    /// <summary>
    /// تایید یا عدم تایید نسبت خانوادگی
    /// </summary>
    public class ReviewFamily
    {


        /// <summary>
        /// شناسه شهروند درخواست دهنده
        /// </summary>
        public int CitizenId { get; set; }

         
        /// <summary>
        /// تایید یا عدم تایید
        /// </summary>
        public bool  IsAccept { get; set; }
         

    }
   

    /// <summary>
    /// تایید نسبت خانوادگی
    /// </summary>
    public class ConfirmFamily
    { 
        /// <summary>
        /// شناسه شهروند درخواست دهنده
        /// </summary>
        public string  UserCode { get; set; }


        /// <summary>
        /// شناسه عضو خانوادگی
        /// </summary>
        public string FamilyUserCode { get; set; }


        /// <summary>
        /// تایید یا عدم تایید توسط مدیر
        /// </summary>
        public bool IsAccept { get; set; }
    }

    public class ShortFamilyCitizenInfo
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




        public int FamilyCitizenId { get; set; }

        public Guid? FamilyUserCode { get; set; }

        /// <summary>
        /// نام شهروند
        /// </summary>
        public string FamilyFirstName { get; set; }


        public string FamilyLastName { get; set; }


        /// <summary>
        /// تاریخ تولد نسبت
        /// </summary>
        public DateTime? FamilyBirthDate { get; set; }


        /// <summary>
        /// نسبت خانوادگی
        /// </summary>
        public FamilyRelationshipsEnum FamilyRelation { get; set; }

        /// <summary>
        /// جنسیت نسبت
        /// </summary>
        public bool FamilyGender { get; set; }

        public string FamilyNationCode { get; set; }


        /// <summary>
        /// نام خانوادگی شهروند
        /// </summary>
        public string LastName { get; set; }


        /// <summary>
        /// نام پدر شهروند
        /// </summary>
        public string FatherName { get; set; }



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
        /// ثبت نام شده توسط کدام خدمت
        /// </summary>
        public string RegisterByService { get; set; }

        /// <summary>
        /// وضعیت ثبت احوال شما
        /// </summary>
        public SabtStatusEnum SabtStatus { get; set; }


        /// <summary>
        /// شماره موبایل شهروند
        /// </summary>
        public string Mobile { get; set; }



        /// <summary>
        /// تاریخ ثبت نام
        /// </summary>
        public DateTime CreationDate { get; set; }



        /// <summary>
        /// تاریخ تولد شهروند
        /// </summary>
        public DateTime? BirthDate { get; set; }

        




        public int Age { get; set; }


        public MaritalStatusEnum?  MariageStatus { get; set; }


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



    public class PagedFamilyCitizenViewModel
    {
        public int TotalItems { get; set; }
        public List<ShortFamilyCitizenInfo> Citizens { get; set; }

    }

    public class CitizenAndFamilyList
    {
        public int CitizenId { get; set; }
        public Guid? UserCode { get; set; }
        public ShortFamilyCitizenInfo Citizen { get; set; } 
        public List<CitizenFamiliesInfo> FamilyList { get; set; }


    }

}
