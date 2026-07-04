

  export enum BaseDataEnum {
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
    military,

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
    /// وضعیت فرم معرفی نامه
    /// </summary>
    introductionFormStatus,
    /// <summary>
    /// وضعیت پرسش و پاسخ
    /// </summary>
    questionStatus,
    /// <summary>
    /// وضعیت فعلی اشتغال کارجو
    /// </summary>
    currentWorkStatus,
    /// <summary>
    /// وضعیت بررسی رزومه
    /// </summary>
    reviewResumeState,
    /// <summary>
    /// وضعیت نهایی به کارگماری کارجو
    /// </summary>
    FinalStatus



  }

  /// <summary>
  /// وضعیت فرصت شغلی
  /// </summary>
  export enum JobStatusEnum {
   // [Description("فعال")]
        Active = 0,
   // [Description("منقضی شده")]
        Expired = 1,
   // [Description("تکمیل شده")]
        Completed = 2
  }


  /// <summary>
  /// وضعیت بررسی رزومه
/// </summary>
export enum ReviewResumeStateEnum {

    ارسال_شده = 0,
    در_حال_بررسی = 1,
    پذیرفته_شده = 2,
    رد_شده = 3
  }
  /// <summary>
  /// وضعیت نهایی به کارگماری کارجو
  /// </summary>
  export enum FinalStatusEnum {
    پذیرفته_شده = 0,
    رد_شده = 1,
    پذیرش_آزمایشی = 2,

  }




  /// <summary>
  /// وضعیت شغلی کارجو
/// </summary>
export enum CurrentWorkStatus {
    شاغل = 0,
    بیکار_هستم = 1,
    در_حال_تحصیل_هستم = 2,

  }


  /// <summary>
  /// نوع ثبت نام
  /// حقیقی یا حقوقی
/// </summary>
export enum UserRegisterTypeEnum {
    /// <summary>
    /// شخصیت حقیقی
    /// </summary>
    حقیقی = 0,
    /// <summary>
    /// شخصیت حقوقی
    /// </summary>
    حقوقی = 1
  }

export enum AddressTypeEnum {
    آدرس_محل_اقامت = 0,
    آدرس_محل_کار = 1
  }







  /// <summary>
  /// وضعیت سوال
/// </summary>
export enum QuestionStatusEnum {
    غیرفعال = 0,
    تایید_شده = 1,
    در_حال_بررسی = 2,
    حذف_شده = 3
  }
  /// <summary>
  /// پیشوند اسم
/// </summary>
export enum NamePrefixEnum {

    آقای = 0,
    خانم = 1,
    آقای_مهندس = 2,
    خانم_مهندس = 3,
    آقای_دکتر = 4,
    خانم_دکتر = 5,



}
export enum TicketPriorityEnum {
    کم = 0,
    عادی = 1,
    بالا = 2,
    بحرانی = 3

}
export enum PageType {
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


  }


export enum TicketStatusEnum {

    جدید = 0,
    در_دست_بررسی = 1,
    پاسخ_داده_شده = 2,
    منتظر_پاسخ_کاربر = 3,
    پاسخ_توسط_کاربر = 4,

  }

  /// <summary>
  /// نوع مجوز فعالیت شرکت
/// </summary>
export enum ActivityLicenseTypeEnum {
    صنعتی = 0,
    صنفی = 1


  }
  /// <summary>
  /// نوع مجوز
/// </summary>
export enum ActivityLicenseEnum {
    کارت_شناسایی = 0,
    گواهی_فعالیت = 1,
    پروانه_بهره_برداری = 2,

  }

  /// <summary>
  /// نوع شرکت
/// </summary>
export enum CompanyOwnerTypeEnum {
    سهامی_عام = 0,
    سهامی_خاص = 1,
    با_مسئولیت_محدود = 2,
    تعاونی = 3,
    تضامنی = 4,

  }

  /// <summary>
  /// نوع زمین
/// </summary>
export enum EarthConditionEnum {
    محصور = 0,
    غیرمحصور = 1,
    در_حال_احداث = 2,

  }



  /// <summary>
  /// وضعیت شرکت
  /// درحال_احداث
  /// فعال
  /// راکد
  /// </summary>
  export enum UserCompanyStatusEnum {


    در_حال_احداث = 0,
    فعال = 1,
    راکد = 2
  }
  /// <summary>
  /// وضعیت کاربری شرکت
  /// </summary>
  export enum UserCompanyAccountStatusEnum {

    در_دست_بررسی = 0,
    تایید_شده = 1,
    رد = 2,
    بلاک = 3


  }



  export enum JobGradeEnum {
    مهم_نیست = 0,
    زیردیپلم = 1,
    دیپلم = 20,
    دیپلم_به_بالا = 21,
    فوق_دیپلم = 22,
    لیسانس = 30,
    لیسانس_به_بالا = 31,
    فوق_لیسانس = 40,
    فوق_لیسانس_به_بالا = 41,
    دکتری = 50

  }
  export enum GradeEnum {
    بیسواد = 0,
    ابتدايي = 1,
    راهنمايي = 2,
    متوسطه = 3,

    دیپلم = 20,
    فوق_دیپلم = 22,
    لیسانس = 30,
    فوق_لیسانس = 40,
    دکتری = 50,


  }
  export enum MaritalStatusEnum {


    مجرد = 1,
    متاهل = 2,
    متارکه = 3,
    همسر_فوت_کرده = 4,
  }
  export enum SalaryTypeEnum {
    اداره_کار = 0,
    توافقی = 1,

  }
  export enum KarjoStateEnum {
    درجریان = 0,
    راکد = 1,
    لیست_سیاه = 2

  }





  export enum ResumeStatusEnum {
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
export enum TypeOfDocumentEnum {
    تاییده,
    گواهینامه

  }
export enum AmozeshOrganzationEnum {
    آموزشگاههای_آزاد,
    فنی_و_حرفه_ایی = 1,
    میراث_فرهنگی = 2,
    شهرداری = 3,
    کمیته_امداد = 4,
    سایر = 5,

  }
export enum TypeUniversity {
    دولتی = 0,
    آزاد,
    پیام_نور,
    غیرانتفاعی,
    الکترونیکی,
    علمی_کاربردی,
    سایر


  }

export enum LanguageLevel {
    مسلط = 0,
    مبتدی = 1,
    متوسط = 2,
    زبان_مادری = 3,


  }



export enum LanguageTitle {
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






export enum SoldierStateEnum {

    پایان_خدمت = 201,
    معافیت_کفالت = 202,
    معافیت_تحصیلی = 203,
    عدم_مشمولیت = 204,
    خدمت_نرفته = 205,
    معافیت_پزشکی = 1106,
    خرید_خدمت = 1107,
    مشمول = 1108,
    در_حال_خدمت = 1105,
  }


  /// <summary>
  /// نوع فعالیت شرکت
  /// </summary>
export enum FieldOfActivityEnum {

    صنعتی = 0,
    خدماتی = 1,
    بازرگانی = 2,
    کشاورزی = 3,
    کارگاهی = 4,
    توزیعی = 5,
    تولیدی = 6,

  }





export enum TicketTypeEnum {
    Other = 0,
    Problem = 1,
    Question = 2
  }

export enum TicketChannelEnum {
    Other = 0,
    Phone = 1,
    Twitter = 2,
    Email = 3,
    Facebook = 4,
    Web = 5,
    Chat = 6,
    Forum = 7

  }
export enum TicketSectionEnum {
    Ticket = 0,
    Faq = 1,
    Contact = 2,
    Feadback = 3


  }



export enum UnitSalesStatusEnum {
  جدید = 0,
  تایید = 1,
  عدم_تایید = 2,
  ویرایش_شده = 3,

}

