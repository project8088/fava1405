

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nikan.Common.GlobalEnum
{


   

    public enum BaseDataEnum
    {
        /// <summary>
        /// زیان های خارجی
        /// </summary>
        language,
        /// <summary>
        ///  پست سازمانی
        /// </summary>
        organizationalposition,
        /// <summary>
        /// مقطع تحصیلی
        /// </summary>
        educationLevel,
        /// <summary>
        /// وضعیت فرصت شغلی
        /// </summary>
        jobStatus,
        /// <summary>
        /// وضعیت خدمت سربازی
        /// </summary>
        soldierState, 
        /// <summary>
        /// سطح تسلط به زبان های خارجی
        /// </summary>
        languageLevel,
        /// <summary>
        /// وضعیت تاهل
        /// </summary>
        maritalStatus,
        /// <summary>
        /// پیشوند اسم 
        /// </summary>
        namePrefix,
        /// <summary>
        /// اولویت تیکت
        /// </summary>
        ticketPriority,
        /// <summary>
        ///  نوع مجوز فعالیت شرکت
        /// </summary>
        activityLicenseType,
        /// <summary>
        ///  //وضعیت کاربری شرکت
        /// </summary>
        userCompanyAccountStatus,
        /// <summary>
        /// //وضعیت شرکت
        /// </summary>
        userCompanyStatus,


        /// <summary>
        /// //نوع زمین
        /// </summary>
        earthCondition,

        /// <summary>
        ///  //نوع شرکت
        /// </summary>
        companyOwnerType,

        /// <summary>
        /// //نوع مجوز
        /// </summary>
        activityLicense,
        /// <summary>
        /// مقطع تحصیلی برای شغل
        /// </summary>
        jobGrade,
        /// <summary>
        /// مقاطع تحصیلی
        /// </summary>
        eduGrade,
        /// <summary>
        /// حقوق
        /// </summary>
        salaryType,
        /// <summary>
        /// نوع ثبت نام حقیقی یا حقوقی
        /// </summary>
        userTypeRegister,
        /// <summary>
        /// نوع تماس 
        /// </summary>
        contactType,
        /// <summary>
        /// نتیجه تماس با کارجو بابت صدور معرفی نامه
        /// </summary>
        resultOfContactforJob,

        /// <summary>
        /// وضعیت تیکت
        /// </summary>
        ticketStatus,
        /// <summary>
        /// نوع قرارداد به کارگماری مددجو
        /// </summary>
        typeEmploymentContract,
        /// <summary>
        /// نوع مدرک آموزشی اخذ شده
        /// </summary>
        typeOfDocumentEnum,
        /// <summary>
        /// نوع آموزشگاههای اخذ مدرک
        /// </summary>
        amozeshOrganzationEnum,
        /// <summary>
        /// وضعیت حساب کاربری
        /// </summary>
        userAccountState,
        
        /// <summary>
        /// وضعیت پرسش و پاسخ
        /// </summary>
        questionStatus,
        
        
        /// <summary>
        /// نوع فعالیت شرکت
        /// </summary>
        companyFieldOfActivity,
        

       
        /// <summary>
        /// تراکنش بابت 
        /// </summary>
        transactionFor,
        /// <summary>
        /// نوع تراکنش
        /// </summary>
        transactionType,
        /// <summary>
        /// وضعیت تراکنش
        /// </summary>
        transactionState,
        /// <summary>
        /// وضعیت تحصیلی
        /// </summary>
        educationStatues,
        /// <summary>
        /// گروه شغلی
        /// </summary>
        jobGroup,
        /// <summary>
        /// سوالات امنیتی کلمه عبور
        /// </summary>
        passwordQuestion, 
        /// <summary>
        /// جانباز جسمی حرکتی
        /// </summary>
        typ_JesmiHarekati_WheelChair ,
        /// <summary>
        /// جانباز جسمی حرکتی غیرویلچری
        /// </summary>
        typ_JesmiHarekati_NoWheelChair ,
        /// <summary>
        /// اعصاب و روان
        /// </summary>
        typ_AsabRavan , 
        /// <summary>
        /// ذهنی
        /// </summary>
        typ_Zehni ,
        /// <summary>
        /// بینایی
        /// </summary>
        typ_Binaei ,
        /// <summary>
        /// شنوایی
        /// </summary>
        typ_Shenavaei ,
        /// <summary>
        /// زنان سرپرست خانواده
        /// </summary>
        typ_ZananSarparast, 
        /// <summary>
        /// وضعیت بررسی فرم منزلت 
        /// </summary>
        manzalatFormStatuse ,
        /// <summary>
        /// دلیل عدم تایید تصویر
        /// </summary>
        rejectCitizenPicture,
        /// <summary>
        /// دین
        /// </summary>
        religion,
        /// <summary>
        /// روابط خانوادگی
        /// </summary>
        familyRelationships,

        /// <summary>
        /// انواع فرم های طرح منزلت
        /// </summary>
        manzalatFormType,
        /// <summary>
        /// وضعیت درخواست کارت
        /// </summary>
        cardRequestStatus ,
        /// <summary>
        /// وضعیت مدزک شهروند
        /// </summary>
        userDocumentStatus,
        /// <summary>
        /// وضعیت ثبت احوال شهروند
        /// </summary>
        sabtStatus,
        /// <summary>
        /// وضعیت تصویر پرسنلی
        /// </summary>
        personalPicture,

        /// <summary>
        /// نوع ابطال کارت
        /// </summary>
        cardCancellationType,
        /// <summary>
        /// دلیل ابطال کارت
        /// </summary>
        cardCancellationItem,
        /// <summary>
        /// نوع تحویل کارت
        /// </summary>
        cardDeliverType,
        /// <summary>
        /// نوع صف
        /// </summary>
        queueInputType ,
        /// <summary>
        /// بیماریهای خاص
        /// </summary>
        typ_SpecialDiseases ,
        /// <summary>
        /// وضعیت بررسی تصویر برای صدور کارت رایگان
        /// </summary>
        imagerReviewStatusFormFreeCard 


    }


    
    public enum RoleEnum
    {

        GUEST=0
    }

    /// <summary>
    /// نوع ابطال کارت
    /// </summary>
    public enum CardCancellationTypeEnum
    {
        بدون_درخواست_مجدد = 0,
        با_درخواست_مجدد = 1

    }

    public enum RefundStateEnum
    {
        ثبت_شماره_کارت = 0,
        برگشت_هزینه = 1,
        بدون_پاسخ=2

    }

    public enum SearchCitizensTypeEnum
    {
        همه = 0,
        کد_ملی = 1,
        اصفهان_کارت = 2,
        شماره_موبایل = 3,
        شماره_تلفن = 4

    }


    public enum QueueInputTypeEnum
    {
        تحویل_پستی = 0,
        مرکز_تحویل = 1,
        صف_موقت = 2

    }





    /// <summary>
    /// وضعیت ثبت احوال
    /// </summary>
    public enum SabtStatusEnum
    {

        عدم_تایید = 0,
        تایید = 1,
        فوتی = 2,
        در_حال_استعلام = 3,
        استعلام_نشده = 4




    }


    /// <summary>
    /// وضعیت تصویر پرسنلی
    /// </summary>
    public enum PersonalPictureEnum
    { 

        عدم_تایید = 0,
        تایید_شده = 1,
        درحال_بررسی = 2,

    }
    public enum EventSectionEnum
    {
        //کاربران
        ورود_شهروند = 0,
        ثبت_نام_شهروند = 1,
        استعلام_شهروند = 2,
        ویرایش_اطلاعات_شهروند = 3,
        ثبت_کاربر_توسعه_دهنده = 4,
        ثبت_خدمات_برای_کاربر_توسعه_دهنده = 5,
        ثبت_کاربر_مدیریت = 6,
        ثبت_کاربر_کارت = 7,
        ویرایش_اطلاعات_کاربر = 8,
        حذف_کاربر_مدیریت_کارت = 9,
       تغییر_کلمه_عبور_کاربر_جاری = 10,
        تغییر_کلمه_عبور_کاربر = 11,
        اضافه_کردن_گروه_دسترسی_به_کاربر = 12,
        حذف_کردن_گروه_دسترسی_به_کاربر = 13,
        اضافه_کردن_نقش_به_کاربر = 14,
        حذف_نقش_از_کاربر = 15,
        استعلام_شهروند_تیکت = 16,
        عدم_احراز_شهروند = 17,
        تایید_اطلاعات_شناسنامه_ایی = 18,
        ویرایش_شماره_موبایل = 19,
        ویرایش_اطلاعات_شناسنامه_ایی = 20,
        ویرایش_وضعیت_ثبت_احوال = 21,

        خروجی_اکسل_شهروندان = 22,

        //تنظیمات مالی
        ویرایش_تنظیمات_سامانه = 30,



        //امور مالی

        //

        خطای_احراز_هویت = 900,
        احراز_هویت = 901,
        خطای_سیستمی =1000,


    }

    public enum EventTypeEnum
    {
        Info = 0,
        Error = 1,
        Warning = 2
    }


    public enum EventPriorityEnum
    {
        Normal = 0,
        Important = 1,
        Necessary = 2,
    }

    /// <summary>
    /// وضعیت درخواست کارت
    /// </summary>
    public enum CardRequestStatusEnum
    {
        /// <summary>
        /// درخواست بدون پرداخت هزینه
        /// </summary>
        درخواست_اولیه = 0,
        /// <summary>
        /// پرداخت هزینه انجام شده است
        /// </summary>
        درخواست_جدید = 1,
        ارسال_برای_چاپ = 2,
        چاپ_کارت = 4,


        ارسال_به_پست = 5,
        ارسال_به_مرکز_تحویل = 10,

        تحویل_داده_شد = 16,//6, 
        برگشت_داده_شده = 17, // 7, 
        باطل_شده = 19,


        وضعیت_نامعلوم = 20

    }


   




    /// <summary>
    /// وضعیت مدرک کاربران
    /// </summary>
    public enum UserDocumentStatusEnum
    {
        در_دست_بررسی = 0,
        تایید_شده = 1,
        عدم_تایید = 2,

    }

    public enum UserPermissionTypeEnum
    {
        مدیریت = 0,
        توسعه_دهندگان = 1,
        اصفهان_کارت = 2,

    }

    public enum ExportCitizenTypeEnum
    {
        Sabt = 0,
        BagRezvan = 1,
    }




    public enum ImportExcelFileTypeEnum
    {
        لیست_شهروندان_اشخاص_حقوقی = 0,
        گروه_شهروندی=1,
        لیست_استرداد = 2,
        شهروندان = 3


    }

    public enum PermissionCategoryEnum
    {

        مدیریت = 0,
        اطلاعات_پایه = 100, 
        مدیریت_سازمان = 200,
        مدیریت_کاربران = 300,
        مدیریت_شهروندان = 400,
        طرح_منزلت = 500,
        مدیریت_کارت = 600,
        اشخاص_حقوقی = 670,
        ثبت_احوال = 700,
        پشتیبانی = 800,
        مدیریت_محتوا = 900,  
        امورمالی = 1000,
        استرداد_هزینه = 1500,
        توسعه_دهندگان =2000,
        اصفهان_کارت = 5000,

      



    }

    public enum PermissionTypeEnum
    {


        //-- مدیریت = 0,
        داشبورد=0,
        //--اطلاعات_پایه = 100,
        گروههای_شهروندی = 100,
        خدمات_شهروندی = 101,
        تنظیمات_سامانه = 102,
        تنظیمات_پیامک = 103,
        //-- مدیریت_سازمان = 200,
        سازمانها = 201,
        //-- مدیریت_کاربران = 300,
        مدیریت_توسعه_دهندگان = 300,
        مدیریت_کاربران = 301,
        گروههای_کاربری = 302,
        مدیران_سامانه = 303,
        //-- مدیریت_شهروندان = 400,
        جستجوی_شهروند = 401,
        جستجوی_پیشرفته_شهروند = 402,
        بازبینی_تصاویر = 403,
        بازخورد_ها = 404,
        خانواده_شهروند = 405,
        استعلام_وضعیت_فوتی = 406,
        گروه_شهروندی = 407,
        صف_شهروندی = 408,
        ویرایش_اطلاعات_شهروند = 409,
        ثبت_نام_دسته_ایی_شهروند = 410,
        ویرایش_شماره_موبایل_شهروند = 411,
        تغییر_وضعیت_ثبت_احوال  = 412,
        دریافت_خروجی_اکسل_شهروندان = 413,
        //-- طرح_منزلت = 500,
        درخواست_کنندگان_منزلت = 501,
        بررسی_درخواست_منزلت = 502,
        تنظیمات_منزلت = 503,
        //-- مدیریت_کارت = 600,
        درخواست_کنندگان_کارت = 601,
        مشخصات_کارت = 602,
        خروجی_صدور_کارت = 603,
        //---اشخاص حقوقی---
        مدیریت_اشخاص_حقوقی = 671,
        //-- ثبت_احوال = 700,
        خروجی_ثبت_احوال = 701,

        احراز_هویت_شهروند = 702,  



        //-- پشتیبانی = 800,
        مدیریت_اطلاعیه = 801,
        مشاهده_تیکت = 802,
        ارسال_پاسخ_تیکت = 803,
        //-- مدیریت_محتوا = 900,
        مدیریت_خبر = 901,
        مدیریت_پرسش_و_پاسخ = 902,
        مدیریت_صفحات = 903,
        //--امور مالی
        تنظیمات_مالی = 1001,
        تراکنش_های_مالی = 1002,
        تست_پرداخت = 1003,
        //--استرداد هزینه
        بارگذاری_فایل_استرداد = 1501,
        لیست_دسترسی_استرداد = 1502,
        جستجوی_استرداد = 1503,
        تایید_برگشت_هزینه = 1504,
        ثبت_شماره_کارت = 1506,
        ویرایش_استرداد = 1507,
      
        
        //-- توسعه_دهندگان = 2000,
        لیست_خدمات = 2000,
        ثبت_نام_شهروند = 2001,
        دریافت_اطلاعات_کوتاه_شهروند = 2002,
        دریافت_اطلاعات_تکمیلی_شهروند = 2003,
        ویرایش_اطلاعات_شهروندی = 2004,
        ویرایش_کلمه_عبور_شهروندی = 2005,
        ایجاد_توکن_دسترسی = 2006,
        ارسال_پیامک_اعتبارسنجی_شماره_موبایل = 2007,
        ثبت_نام_سریع_شهروند = 2008,
        دریافت_اطلاعات_شهروند_به_وسیله_توکن = 2009, 
        دریافت_اطلاعات_عضویت_شهروند_در_گروههای_شهروندی = 2010,
        دریافت_اطلاعات_آدرس_شهروند = 2011,
        دریافت_اطلاعات_کامل_شهروند = 2012,
        دریافت_اطلاعات_ثبت_نام_کنندگان_طرح_منزلت = 2013,


        // اصفهان کارت
        مدیریت_کاربران_کارت = 5001,
        جستجوی_شهروند_کارت=5002,
        مشاهده_اطلاعات_شهروند = 5003,
        بازبینی_تصاویر_کارت = 5004,
        خروجی_صدور_کارت_کارت = 5005, 
        مدیریت_توزیع_کارت = 5006,
        تراکنش_های_مالی_کارت = 5007,
        تیکت_های_کارت = 5008,
        مدیریت_کارت = 5009,
        تنظیمات_کارت = 5010,
        ویرایش_شماره_موبایل_شهروند_اصفهان_کارت = 5011,
         
        احراز_هویت_شهروند_کارت = 5012,
        جستجوی_کارت_کارت = 5013,


        فایل_اکسل_خروجی_چاپ_کارت = 5014,


        ثبت_درخواست_کارت_رایگان_کارت = 5050,
        تایید_درخواست_کارت_رایگان_کارت = 5051,

        احراز_هویت_شهروند_توسط_اپراتور = 5055,









    }






    /// <summary>
    /// نوع فرم منزلت
    /// </summary>
    public enum ManzalatFormTypeEnum
    {
        جانبازان = 0,
        معلولین = 1,
        زنان_سرپرست_خانواده = 2,
        بازنشسته = 3,
        سالمند = 4,
        بیماران_خاص = 5,
        مادران_دارای_سه_فرزند = 6,
        دانش_آموزان_تحت_پوشش_کمیته_امداد_امام_خمینی_و_سازمان_بهزیستی = 7,

    }



    /// <summary>
    /// روابط خانوادگی
    /// </summary>
    public enum FamilyRelationshipsEnum
    {
        پدر = 0,
        مادر = 1,
        همسر = 2,
        فرزند = 3,
        برادر = 4,
        خواهر = 5,
    }




    /// <summary>
    /// دین شهروند
    /// </summary>
    public enum CitizenProfileReligionEnum
    {
        اسلام = 0,
        مسیحیت = 1,
        یهودیت = 2,
        زرتشت = 3,
        سایر = 4

    }




    public enum SiteOptionCategoryEnum
    {
        تنظیمات_پایه = 0,
        تنظیمات_مالی = 1,
        تنظیمات_پیامک = 2,
        تنظیمات_منزلت = 3,
        //درگاه_بانکی=3

    }


    #region Card
    /// <summary>
    /// روش درخواست کارت
    /// </summary>
    public enum DeliverTypeEnum
    {


        ///ارسال پستی
        پستی = 1,
        /// <summary>
        /// دریافت در مرکز
        /// </summary> 
        تحویل_در_مرکز = 2
    }


    public enum ImagerReviewStatusFormFreeCardEnum
    {
        همه=0,
        تاییده_شده =1,
        بارگذاری_شده=2, 
    }



    /// <summary>
    /// نوع درخواست کارت
    /// </summary>
    public enum CardRequestTypeEnum
    {
        درخواست_جدید = 0,
        درخواست_مجدد = 1,
        درخواست_المثنی = 2
    }



    /// <summary>
    /// نوع مجوز دسترسی به کارت
    /// </summary>
    public enum CardPermissionTypeEnum
    { 
        عدم_دسترسی = 0,
        دسترسی = 1,

    }


    #endregion

  

    public enum AddressTypeEnum
    {

        /// <summary>
        ///محل کار
        /// </summary> 
        محل_کار = 1,
        /// <summary>
        ///منزل
        /// </summary> 
        منزل = 2
    }



    /// <summary>
    /// وضعیت بررسی فرم منزلت
    /// </summary>
    public enum ManzalatFormStatuseEnum
    {
        در_حال_بررسی = 0,
        تایید = 1,
        عدم_تایید = 2,




    }
     

    public enum Typ_ZananSarparastEnum
    {
        همسر_فوت_شده = 0,
        از_کار_افتادگی_همسر = 1,
        طلاق = 2,
        سایر = 3

    }


    /// <summary>
    /// بیماریهای خاص
    ///  // بیماران  ، ، ،ام اس
    /// </summary>
    public enum Typ_SpecialDiseasesEnum
    {
        دیالیزی = 0,
        تالاسمی = 1,
        هموفیلی = 2,
        ام_اس = 3

    }






    public enum Typ_JesmiHarekati_WheelChairEnum
    {
        نخاعی=0,
        جسمی_حرکتی,
    }


    public enum Typ_JesmiHarekati_NoWheelChairEnum
    { 
        ضعیف = 0,
        متوسط,
        شدید
    }






    public enum Typ_AsabRavanEnum
    {
        ضعیف = 0,
        شدید
    }
    public enum Typ_ZehniEnum
    {
        ضعیف = 0,
        متوسط,
        شدید,
        اوتیسم
    }


    public enum Typ_BinaeiEnum
    {
        نابینا = 0,
        کم_بینا,

    }


    public enum Typ_ShenavaeiEnum
    {
        ناشنوا = 0,
        کم_شنوا,

    }


    

 


    /// <summary>
    /// وضعیت تحصیلی
    /// </summary>
    public enum EducationStatuesEnum
    {

        اتمام_تحصیل = 0,
        در_حین_تحصیل = 1,
        انصراف_از_تحصیل = 2,
        تحصیل_نکرده = 3

    }


    /// <summary>
    /// وضعیت تاهل
    /// </summary>
    public enum MaritalStatusEnum
    {
        مجرد = 0,
        متاهل = 1,
        همسر_فوت_شده = 2,
        سایر = 5
    }

   


   

    /// <summary>
    /// وضعیت خدمت
    /// </summary>
    public enum SoldierStateEnum
    {


        پایان_خدمت = 0,
        معافیت_دائم_پزشکی = 1,
        معافیت_دائم_غیر_پزشکی = 2,
        معافیت_خرید_خدمت = 3,
        معافیت_تحصیلی = 4,
        مشمول_به_خدمت = 5,
        عدم_مشمولیت = 6,
        موارد_خاص = 7,
        در_حین_خدمت = 8


    }
     
   




    /// <summary>
    /// نحوه محاسبه شارژ ماهانه شرکت ها
    /// </summary>
    public enum CalcChargeTypeEnum
    {
        مبلغ = 0,
        فرمول = 1
    }

    public enum FormulaForEnum
    {
        شارژ = 0,
        آب = 1
    }

    public enum TransactionForEnum
    {
        خرید_کارت = 0,
        پکیج_کنکور = 1,
        تست_درگاه =1000
    }

    public enum TransactionTypeEnum
    {
        /// <summary>
        /// بدهکار
        /// برداشت 
        /// </summary>
        بدهکار = -1,
        /// <summary>
        /// بستانکار- طلبکار
        /// واریز
        /// </summary>
        بستانکار = 1,
    }

    public enum TransactionBankEnum
    {
        ملت=0,
        ملی=1

    }

    
    public enum TransactionStateEnum
    {
       
        /// <summary>
        /// عدم تایید
        /// </summary>
        عدم_تایید =0,
        /// <summary>
        /// تایید شده
        /// </summary>
        تایید_شده =1,
        /// <summary>
        /// در دست بررسی
        /// </summary>
        در_دست_بررسی = 2,
        /// <summary>
        /// برگشت داده شده
        /// </summary>
        برگشت_داده_شده = 3,
        /// <summary>
        /// پرداخت  نشده
        /// </summary>
        پرداخت_نشده = 4
    }



    public enum PriceTypeEnum
    {
        دستگاه = 0,
        کیلو = 1,
        تن = 2,
        لیتر = 3,
        عدد = 4,
        متر = 5,
        سانتی_متر = 6,
        متر_مربع =7,
        نخ = 8,
        قطعه = 9,
        راس = 10,
        برنامه = 11,
        بسته = 12,
        کارتن = 13,
        شاخه = 14,
        پالت = 15,
        دوره = 16,
    }

    public enum PaymentTypeEnum
    {
        /// <summary>
        /// پرداخت به صورت آنلاین
        /// </summary>
        آنلاین = 0,
        /// <summary>
        /// پرداخت به صورت کارت به کارت
        /// </summary>
        دستگاه_پوز = 1,
        /// <summary>
        /// پرداخت به صورت واریز به حساب
        /// </summary>
        واریز_به_حساب = 2,
        /// <summary>
        /// پرداخت نقدی
        /// </summary>
        نقدی = 3,
    }

   


    public enum AssignmentTypeEnum
    {
        فروش_واحد_صنعتی=0,
        اجاره_واحد_صنعتی = 1,
        

    }


    public enum UnitSalesStatusEnum
    {
        جدید = 0,
        تایید = 1,
        عدم_تایید =2,
        ویرایش_شده = 3,

    }

    /// <summary>
    /// نوع فعالیت شرکت
    /// </summary>
    public enum FieldOfActivityEnum
    {


        صنعتی = 0,
        خدماتی = 1,
        بازرگانی = 2,
        کشاورزی = 3,
        کارگاهی = 4,
        توزیعی = 5,
        تولیدی = 6,

    }

    public enum ExitStatusEnum
    {
        
        جدید = 0, 
        تایید = 1, 
        عدم_تایید = 2,
        ارسال_مجدد = 3,
    }

    public enum ShowExitPermitEnum
    {
        همه=0,
        جدید=1,
        آرشیو=2,
        رد=3,
        تایید=4
    }


    /// <summary>
    /// وضعیت فرصت شغلی
    /// </summary>
    public enum JobStatusEnum
    {
        [Description("فعال")]
        Active = 0,
        [Description("منقضی شده")]
        Expired = 1,
        [Description("تکمیل شده")]
        Completed = 2
    }


    /// <summary>
    /// وضعیت بررسی رزومه
    /// </summary>
    public enum ReviewResumeStateEnum
    {
       
        ارسال_شده = 0,
        در_حال_بررسی = 1,
        پذیرفته_شده = 2, 
        رد_شده = 3
    }
    /// <summary>
    /// وضعیت نهایی به کارگماری کارجو
    /// </summary>
    public enum FinalStatusEnum
    { 
        پذیرفته_شده = 0,
        رد_شده = 1,
        پذیرش_آزمایشی = 2,

    }




    /// <summary>
    /// وضعیت شغلی کارجو
    /// </summary>
    public enum CurrentWorkStatus
    {
        شاغل = 0,
        بیکار_هستم = 1,
        در_حال_تحصیل_هستم = 2,

    }


    /// <summary>
    /// نوع ثبت نام
    /// حقیقی یا حقوقی
    /// </summary>
    public enum UserRegisterTypeEnum
    {
        /// <summary>
        /// شخصیت حقیقی
        /// </summary>
        حقیقی = 0,
        /// <summary>
        /// شخصیت حقوقی
        /// </summary>
        حقوقی = 1
    }

    

    
    /// <summary>
    /// پیشوند اسم
    /// </summary>
    public enum NamePrefixEnum
    {

        آقای = 0,
        خانم = 1,
        آقای_مهندس = 2,
        خانم_مهندس = 3,
        آقای_دکتر = 4,
        خانم_دکتر = 5,



    }
    public enum TicketPriorityEnum
    {
      کم=0,
      عادی=1,
      بالا=2,
      بحرانی=3

    }
    public enum PageType
    {
        Page = 0, 
        NewsList = 1,
        ArticleList = 2,
        Blog = 3,
        GalleryList = 4,
        Gallery = 5,
        Redirect = 6,
        Default = 7,
        Sitemap = 8,
        AdminPage = 9,
        Notifications = 10,

    }



    public enum TicketStatusEnum
    {
      
        جدید=0,
        در_دست_بررسی = 1,
        پاسخ_داده_شده = 2,
        منتظر_پاسخ_کاربر =3,
        پاسخ_توسط_کاربر = 4,

    }

    /// <summary>
    /// نوع مجوز فعالیت شرکت
    /// </summary>
    public enum ActivityLicenseTypeEnum
    {
        صنعتی=0,
        صنفی=1


    }
    /// <summary>
    /// نوع مجوز
    /// </summary>
    public enum ActivityLicenseEnum
    {
        کارت_شناسایی = 0,
        گواهی_فعالیت = 1,
        پروانه_بهره_برداری = 2,

    }

    /// <summary>
    /// نوع شرکت
    /// </summary>
    public enum CompanyOwnerTypeEnum
    {
        سهامی_عام=0,
        سهامی_خاص = 1,
        با_مسئولیت_محدود = 2,
        تعاونی = 3,
        تضامنی = 4,
         
    }

    /// <summary>
    /// نوع زمین
    /// </summary>
    public enum EarthConditionEnum
    {
        محصور=0,
        غیرمحصور = 1,
        در_حال_احداث = 2,

    }



    /// <summary>
    /// وضعیت شرکت
    /// درحال_احداث
    /// فعال
    /// راکد
    /// </summary>
    public enum UserCompanyStatusEnum
    {  
        در_حال_احداث = 0,
        فعال = 1,
        راکد = 2
    }
    /// <summary>
    /// وضعیت کاربری شرکت
    /// </summary>
    public enum UserCompanyAccountStatusEnum
    {
         
        در_دست_بررسی = 0,
        تایید_شده = 1,
        رد = 2,
        بلاک=3 
    }
    public enum userAccountStateEnum
    {


        غیر_فعال = 0,
        فعال = 1, 
        بلاک = 3
    }

    
    public enum JobGradeEnum
    {
        مهم_نیست = 0,
        زیردیپلم = 1,
        دیپلم = 20,
        دیپلم_به_بالا = 21,
        فوق_دیپلم = 22,
        لیسانس = 30,
        لیسانس_به_بالا = 31,
        فوق_لیسانس = 40,
        فوق_لیسانس_به_بالا = 41,
        دکتری =50

    }

    public enum GradeEnum
    {
        بیسواد = 0,
        ابتدايي = 1,
        راهنمايي = 2,
        متوسطه =3,


        دیپلم = 6,
        فوق_دیپلم = 7,
        لیسانس =8,
        فوق_لیسانس = 10,
        دکتری = 12,
        حوزوی = 15,
        فلوشیپ = 16,


    }


     
      
       

    public enum SalaryTypeEnum
    {
        اداره_کار = 0,
        توافقی = 1,

    }
    public enum KarjoStateEnum
    {
        درجریان = 0,
        راکد = 1,
        لیست_سیاه = 2

    }

     



    public enum ResumeStatusEnum
    {
        /// <summary>
        /// رزومه فعال باشد و در سایت هم نمایش داده شود
        /// </summary>
        رزومه_فعال_باشد = 0,
        /// <summary>
        /// رزومه فعال باشد ولی در سایت نمایش داده نشود
        /// </summary>
        در_سایت_نمایش_داده_نشود = 1,

        /// <summary>
        ///  غیرفعال کردن رزومه
        /// </summary>
        نیاز_به_شغل_ندارم = 2,



    }
    public enum TypeOfDocumentEnum
    {
        تاییده=0,
        گواهینامه

    }
    public enum AmozeshOrganzationEnum
    {
        آموزشگاههای_آزاد,
        فنی_و_حرفه_ایی = 1,
        میراث_فرهنگی = 2,
        شهرداری = 3,
        کمیته_امداد = 4,
        سایر = 5,

    }
    public enum TypeUniversity
    { 
        دولتی = 0,
        آزاد,
        پیام_نور,
        غیرانتفاعی,
        الکترونیکی,
        علمی_کاربردی,
        سایر


    }

    public enum LanguageLevel
    {
        مسلط = 0,
        مبتدی = 1,
        متوسط = 2,
        زبان_مادری = 3,


    }
    


    public enum LanguageTitle
    {
        انگلیسی = 1,
        فرانسوی = 2,
        آلمانی = 3,
        ترکی = 4,
        اسپانیایی = 5,
        ترکی_استانبولی = 6,
        عربی = 7,
        ایتالیایی = 8,
        ارمنی = 9,
        چینی = 10,
        روسی = 11,
        کره_ای = 12,
        هلندی = 13,
        نوروژی = 14,

    }
    


   


   


   

     
     


    public enum TicketTypeEnum
    {
        Other = 0,
        Problem = 1,
        Question = 2
    }

    public enum TicketChannelEnum
    {
        Other = 0,
        Phone = 1,
        Twitter = 2,
        Email = 3,
        Facebook = 4,
        Web = 5,
        Chat = 6,
        Forum = 7

    }
    public enum TicketSectionEnum
    {
        Ticket = 0,
        Faq = 1,
        Contact = 2,
        Feadback=3


    }

}
