using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.BaseEntity;
using Nikan.DomainClasses.Job;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nikan.ViewModel.Jobs
{
    /// <summary>
    /// جهت ویرایش یا اضافه کردن
    /// </summary>
    public class QuickEmploymentInfoDto
    {



       public int? Id { get; set; }


      

        [StringLength(2000)] 
        public string Title { get; set; }
         
         
       
        public string Source { get; set; }




      
        public bool? Sex { get; set; }

       
        public int Number { get; set; }


       
        public string Note { get; set; }


     
        public string JobRowCode { get; set; }



        /// <summary>
        /// آیا بازنشیته باشد
        /// </summary>
        [Display(Name = "ایا بازنشسته باشد؟")]

        public bool? IsRetired { get; set; }

        [Display(Name = "سرویس دارد؟")]
        public bool? WorkService { get; set; }

        /// <summary>
        /// نوع حقوق پیشنهادی
        /// اداره کار-توافقی
        /// </summary>
        public SalaryTypeEnum SalaryType { get; set; }

       

        [Display(Name = "بیمه دارد؟")]
        public bool? WorkInsurance { get; set; }

       

        #region Skill 



        [Display(Name = "رشته تحصیلی")]
        public List<int> MajorIDs { get; set; }
        #endregion
        #region City



     
        public List<int> CityIDs { get; set; }




        #endregion
        #region  عنوان شغل

       
        public List<int> JobInfoIDs { get; set; }
        public int JobTitleId { get; set; }


        #endregion




        #region ساعت کاری

        public int? JobWorkTimeId { get; set; }





        #endregion
        [Display(Name = "مقطع تحصیلی")]
        public virtual JobGradeEnum? JobGrade { get; set; }

        [Display(Name = "حداقل سن")]
        public virtual int? JobAgeStart { get; set; }
        [Display(Name = "حداکثر سن")]
        public virtual int? JobAgeEnd { get; set; }


        


        #region   ExpireDate

    
        public DateTime ExpireDate { get; set; }


        #endregion

       
        public bool IsSpecial { get; set; }
      
        
        [Display(Name = "کارفرما")]
        public virtual int UserCompanyId { get; set; }



        [Display(Name = "درخواست کننده")]
        public string Applicant { get; set; }

        [Display(Name = "شماره تماس درخواست کننده")]
        public string ApplicantPhoneNumber { get; set; }





        [Display(Name = "سمت درخواست کننده")]
        public string ApplicantPost { get; set; }
        [Display(Name = "توضیحات (زمان و شرح مصاحبه)")]
        public string EmployerDescription { get; set; }

        [Display(Name = "عناوین شغلی مشابه")]
        public string[] SimilarJobTitles { get; set; }
        public string SimilarJobTitle { get; set; }
        //[IgnoreMap]
        [Display(Name = "مدت اعتبار سفارش نیرو")]
        public int? ExpireDay { get; set; }
        [Display(Name = "وضعیت تاهل")]
        public MaritalStatusEnum? MaritalStatus { get; set; }
        [Display(Name = "وضعیت پایان خدمت")]
        public SoldierStateEnum? militaryTrainingStatus { get; set; }
        [Display(Name = "عدم معافیت نظام پزشکی")]
        public bool? LackOfMedicalExemption { get; set; }
        [Display(Name = "کارجو بایستی سوابق تحصیلی داشته باشد")]
        public bool HasEducation { get; set; }



       



        [Display(Name = "ساعت کاری در 24 ساعت")]
        public int WorkTime { get; set; }
        [Display(Name = "حقوق از")]
        public int? SalaryRangeFrom { get; set; }
        [Display(Name = "حقوق تا")]
        public int? SalaryRangeTo { get; set; }
        
      
        public string WorkServicePath { get; set; }
        public string PlacementDescription { get; set; }








        #region آدرس محل کار

        /// <summary>
        /// شهر محل کار
        /// </summary>
        public int? LocationId { get; set; }
        public string Location { get; set; }

        /// <summary>
        /// آدرس دقیق موقعیت کاری
        /// </summary>
        public string JobFullAddress { get; set; }

        /// <summary>
        /// منطقه شغلی
        /// </summary>
        public int? JobArea { get; set; }

        #endregion







        public bool? Meal { get; set; }
        public string JobTitleStr { get; set; }
        public int? WorkExperienceMin { get; set; }
        public int? WorkExperienceMax { get; set; }



        public bool IsLockEdit { get; set; }
         

        public BaseDataModel LocationInfo { get; set; }
         

        public List<int> SkillIDs { get; set; }
        public IEnumerable<BaseDataModel> CityInfos { get; set; }
        public IEnumerable<BaseDataModel> MajorInfos { get; set; }
        public IEnumerable<BaseDataModel> SkillInfos { get; set; }



       

    }


    /// <summary>
    /// جهت نمایش جزئیات
    /// </summary>
    public class JobInfoViewDto
    {



        public int? Id { get; set; }




        [StringLength(2000)]
        public string Title { get; set; }







        public string UserCompanyTitle { get; set; }
        public string Source { get; set; }





        public bool? Sex { get; set; }


        public int Number { get; set; }



        public string Note { get; set; }



        public string JobRowCode { get; set; }



        /// <summary>
        /// آیا بازنشیته باشد
        /// </summary>
        [Display(Name = "ایا بازنشسته باشد؟")]

        public bool? IsRetired { get; set; }

        [Display(Name = "سرویس دارد؟")]
        public bool? WorkService { get; set; }

        /// <summary>
        /// نوع حقوق پیشنهادی
        /// اداره کار-توافقی
        /// </summary>
        public SalaryTypeEnum SalaryType { get; set; }



        [Display(Name = "بیمه دارد؟")]
        public bool? WorkInsurance { get; set; }



        #region Skill 



        [Display(Name = "رشته تحصیلی")]
        public List<int> MajorIDs { get; set; }
        #endregion
        #region City




        public List<int> CityIDs { get; set; }




        #endregion
        #region  عنوان شغل


        public List<int> JobInfoIDs { get; set; }
        public int JobTitleId { get; set; }


        #endregion




        #region ساعت کاری

        public int? JobWorkTimeId { get; set; }





        #endregion
        [Display(Name = "مقطع تحصیلی")]
        public virtual JobGradeEnum? JobGrade { get; set; }

        [Display(Name = "حداقل سن")]
        public virtual int? JobAgeStart { get; set; }
        [Display(Name = "حداکثر سن")]
        public virtual int? JobAgeEnd { get; set; }





        #region   ExpireDate


        public DateTime ExpireDate { get; set; }


        #endregion


        public bool IsSpecial { get; set; }


        [Display(Name = "کارفرما")]
        public virtual int UserCompanyId { get; set; }



        [Display(Name = "درخواست کننده")]
        public string Applicant { get; set; }

        [Display(Name = "شماره تماس درخواست کننده")]
        public string ApplicantPhoneNumber { get; set; }





        [Display(Name = "سمت درخواست کننده")]
        public string ApplicantPost { get; set; }
        [Display(Name = "توضیحات (زمان و شرح مصاحبه)")]
        public string EmployerDescription { get; set; }

        [Display(Name = "عناوین شغلی مشابه")]
        public string[] SimilarJobTitles { get; set; }
        public string SimilarJobTitle { get; set; }
        //[IgnoreMap]
        [Display(Name = "مدت اعتبار سفارش نیرو")]
        public int? ExpireDay { get; set; }
        [Display(Name = "وضعیت تاهل")]
        public MaritalStatusEnum? MaritalStatus { get; set; }
        [Display(Name = "وضعیت پایان خدمت")]
        public SoldierStateEnum? militaryTrainingStatus { get; set; }
        [Display(Name = "عدم معافیت نظام پزشکی")]
        public bool? LackOfMedicalExemption { get; set; }
        [Display(Name = "کارجو بایستی سوابق تحصیلی داشته باشد")]
        public bool HasEducation { get; set; }







        [Display(Name = "ساعت کاری در 24 ساعت")]
        public int WorkTime { get; set; }
        [Display(Name = "حقوق از")]
        public int? SalaryRangeFrom { get; set; }
        [Display(Name = "حقوق تا")]
        public int? SalaryRangeTo { get; set; }


        public string WorkServicePath { get; set; }
        public string PlacementDescription { get; set; }








        #region آدرس محل کار

        /// <summary>
        /// شهر محل کار
        /// </summary>
        public int? LocationId { get; set; }
        public string Location { get; set; }

        /// <summary>
        /// آدرس دقیق موقعیت کاری
        /// </summary>
        public string JobFullAddress { get; set; }

        /// <summary>
        /// منطقه شغلی
        /// </summary>
        public int? JobArea { get; set; }

        #endregion







        public bool? Meal { get; set; }
        public string JobTitleStr { get; set; }
        public int? WorkExperienceMin { get; set; }
        public int? WorkExperienceMax { get; set; }



        public bool IsLockEdit { get; set; }

        #region اطلاعات کارفرما

        public string CompanyMangerName { get; set; }

        public string CompanyMobileNumber { get; set; }

        public string CompanyCellNumber { get; set; }
        public string CompanyAddress { get; set; }



        #endregion





        public BaseDataModel LocationInfo { get; set; }




        public List<int> SkillIDs { get; set; }
        public IEnumerable<BaseDataModel> CityInfos { get; set; }
        public IEnumerable<BaseDataModel> MajorInfos { get; set; }
        public IEnumerable<BaseDataModel> SkillInfos { get; set; }





    }


    public class JobListDto
    {


       public int Id { get; set; }

        /// <summary>
        /// کد ردیف شغلی
        /// </summary>
        public string JobRowCode { get; set; }


        /// <summary>
        /// عنوان شغلی
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// تاریخ ثبت موقیعت شغلی
        /// </summary>
        public DateTime? CreatedOn { get; set; }


        /// <summary>
        /// تاریخ انقضای موقیت شغلی
        /// </summary>
        public DateTime ExpireDate { get; set; }


        /// <summary>
        /// کارگاه -کارفرما
        /// </summary>
        public string CompanyName { get; set; }

        public int CompanyId { get; set; }

        public bool? Gender { get; set; }


         

        public string JobStatus { get; set; }
        public JobStatusEnum JobStatusId { get; set; }



        #region آدرس محل کار
        public int? LocationId { get; set; }
        public string Location { get; set; }

        /// <summary>
        /// آدرس دقیق موقعیت کاری
        /// </summary>
        public string JobFullAddress { get; set; }

        /// <summary>
        /// منطقه شغلی
        /// </summary>
        public int? JobArea { get; set; }

        #endregion 



        /// <summary>
        /// آیا فیلدهای مهم فرصت شغلی قابل ویرایش هستند ؟
        /// </summary>
        public bool IsLockEdit { get; set; }


    }


    public class ChangeClosedDto
    {

        public int Id { get; set; }
        public JobStatusEnum JobStatus  { get; set; }

        public DateTime ExpireDate { get; set; }
        public string ClosedDescription { get; set; }
    }


    public class PagedJobsViewModel
    {

        public List<JobListDto> Jobs { get; set; } 
        public int TotalItems { get; set; }


    }






}
