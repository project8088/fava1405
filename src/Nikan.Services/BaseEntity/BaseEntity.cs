using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.BaseEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nikan.ViewModel;
using Nikan.Common.GlobalEnum;

namespace cle.Services.BaseEntity
{
    public interface IBaseDataService
    {
         
        void SaveChange();

        #region Get


        Task<List<BaseData>> GetAll(string category = null);
        Task<Dictionary<string, IEnumerable<BaseDataModel>>> GetBaseList(string selected = null);
        Task<List<BaseDataModel>> GetBaseListAsync(string query, int offset, int count);
        Task<List<BaseDataModel>> GetJobGroups();
        Task<ApiResult> configBaseData();



        #endregion
    }
    public class BaseDataService : IBaseDataService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<BaseData> _BaseDataRepository;
        private readonly DbSet<JobGroup> _jobGroup;


        public BaseDataService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _BaseDataRepository = _uow.Set<BaseData>();
            _jobGroup = _uow.Set<JobGroup>();

        }


        #region IBaseDataService Members



        public async Task<List<BaseData>> GetAll(string category = null)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return await _BaseDataRepository.Where(g => g.Disabled != true)
               .OrderByDescending(g => g.IndexOrder).ToListAsync();

            }

            return await _BaseDataRepository.Where(g => g.Disabled != true
          && g.Category.ToLower() == category
           )
               .OrderByDescending(g => g.IndexOrder).ToListAsync();


        }


        public BaseData GetItem(int id)
        {
            var BaseData = _BaseDataRepository.Find(id);
            return BaseData;
        }



        public async Task< Dictionary<string, IEnumerable<BaseDataModel>>> GetBaseList(string selected = null)
        {


            var list = await _BaseDataRepository.ToListAsync();


            var items = list
                      .GroupBy(g => g.Category).Select(s => new EnumData()
                      {
                          category = s.Key,
                          enumList = s.Select(x =>
                          new BaseDataModel() {Key=x.Key ,Text=x.Text ,Description=x.Description }  ).OrderBy(o => o.Key)

                      }).ToDictionary(d => d.category, d => d.enumList);



            return items;

        }


        public async Task<ApiResult> configBaseData()
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, " با موفقیت انجام گردید");
            try
            {
                 var listRemove = await _BaseDataRepository.ToListAsync();
                    _BaseDataRepository.RemoveRange(listRemove);
                    var listAdd = new List<BaseData>()
                    {    
                   // زبان های خارجی
                   new BaseData { Text = "انگلیسی", Key = "1", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "فرانسوی", Key = "2", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "آلمانی", Key = "3", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "ترکی", Key = "4", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "اسپانیایی", Key = "5", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "ترکی استانبولی", Key = "6", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "عربی", Key = "7", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "ایتالیایی", Key = "8", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "ارمنی", Key = "9", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "چینی", Key = "10", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "روسی", Key = "11", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "کره ای", Key = "12", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "هلندی", Key = "13", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "نوروژی", Key = "14", Category = BaseDataEnum.language.ToString(), LanguageName = "fa" },



                   // روابط خانوادگی
                   new BaseData { Text = "پدر", Key = "0", Category = BaseDataEnum.familyRelationships.ToString(), LanguageName = "fa" ,Description="نسبت های خانوادگی"},
                   new BaseData { Text = "مادر", Key = "1", Category = BaseDataEnum.familyRelationships.ToString(), LanguageName = "fa", Description = "نسبت های خانوادگی" },
                   new BaseData { Text = "همسر", Key = "2", Category = BaseDataEnum.familyRelationships.ToString(), LanguageName = "fa", Description = "نسبت های خانوادگی" },
                   new BaseData { Text = "فرزند", Key = "3", Category = BaseDataEnum.familyRelationships.ToString(), LanguageName = "fa", Description = "نسبت های خانوادگی" },
                   new BaseData { Text = "برادر", Key = "4", Category = BaseDataEnum.familyRelationships.ToString(), LanguageName = "fa", Description = "نسبت های خانوادگی" },
                   new BaseData { Text = "خواهر", Key = "5", Category = BaseDataEnum.familyRelationships.ToString(), LanguageName = "fa", Description = "نسبت های خانوادگی" },
                    

                  
                   ///انواع فرم منزلت
                   new BaseData { Text = "جانبازان", Key = "0", Category = BaseDataEnum.manzalatFormType.ToString(), LanguageName = "fa" ,Description="عضویت در طرح منزلت"},
                   new BaseData { Text = "معلولین", Key = "1", Category = BaseDataEnum.manzalatFormType.ToString(), LanguageName = "fa", Description = "عضویت در طرح منزلت" },
                   new BaseData { Text = "زنان_سرپرست_خانواده", Key = "2", Category = BaseDataEnum.manzalatFormType.ToString(), LanguageName = "fa", Description = "عضویت در طرح منزلت" },
                   new BaseData { Text = "بازنشسته", Key = "3", Category = BaseDataEnum.manzalatFormType.ToString(), LanguageName = "fa", Description = "عضویت در طرح منزلت" },
                   new BaseData { Text = "سالمند", Key = "4", Category = BaseDataEnum.manzalatFormType.ToString(), LanguageName = "fa", Description = "عضویت در طرح منزلت" },



                   //وضعیت خدمت سربازی
                   new BaseData { Text = "پایان خدمت", Key = "0", Category = BaseDataEnum.soldierState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "معافیت دائم پزشکی", Key = "1", Category = BaseDataEnum.soldierState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "معافیت غیر پزشکی", Key = "2", Category = BaseDataEnum.soldierState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = " معافیت خرید خدمت", Key = "3", Category = BaseDataEnum.soldierState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = " معافیت تحصیلی", Key = "4", Category = BaseDataEnum.soldierState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "مشمول به خدمت", Key = "5", Category = BaseDataEnum.soldierState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "عدم مشمولیت", Key = "6", Category = BaseDataEnum.soldierState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "موارد خاص", Key = "7", Category = BaseDataEnum.soldierState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "در حین خدمت", Key = "8", Category = BaseDataEnum.soldierState.ToString(), LanguageName = "fa" },



                   // سطح تسلط به زبان های خارجی
                   new BaseData { Text = "مسلط", Key = "0", Category = BaseDataEnum.languageLevel.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "مبتدی", Key = "1", Category = BaseDataEnum.languageLevel.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "متوسط", Key = "2", Category = BaseDataEnum.languageLevel.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "زبان مادری", Key = "3", Category = BaseDataEnum.languageLevel.ToString(), LanguageName = "fa" },
                   // وضعیت تاهل
                   new BaseData { Text = "مجرد", Key = "0", Category = BaseDataEnum.maritalStatus.ToString(), LanguageName = "fa" ,Description="وضعیت تاهل"},
                   new BaseData { Text = "متاهل", Key = "1", Category = BaseDataEnum.maritalStatus.ToString(), LanguageName = "fa", Description = "وضعیت تاهل" },
                   new BaseData { Text = "همسر فوت کرده", Key = "2", Category = BaseDataEnum.maritalStatus.ToString(), LanguageName = "fa", Description = "وضعیت تاهل" },
                   new BaseData { Text = "سایر", Key = "5", Category = BaseDataEnum.maritalStatus.ToString(), LanguageName = "fa", Description = "وضعیت تاهل" },
                   // وضعیت تاهل
                   new BaseData { Text = "اتمام تحصیل", Key = "0", Category = BaseDataEnum.educationStatues.ToString(), LanguageName = "fa", Description = "وضعیت تحصیلی" },
                   new BaseData { Text = "در حین تحصیل", Key = "1", Category = BaseDataEnum.educationStatues.ToString(), LanguageName = "fa", Description = "وضعیت تحصیلی" },
                   new BaseData { Text = "انصراف از تحصیل", Key = "2", Category = BaseDataEnum.educationStatues.ToString(), LanguageName = "fa", Description = "وضعیت تحصیلی" },
                   new BaseData { Text = "تحصیل نکرده", Key = "3", Category = BaseDataEnum.educationStatues.ToString(), LanguageName = "fa", Description = "وضعیت تحصیلی" },


                   //پیشوند اسم 
                   new BaseData { Text = "آقای", Key = "0", Category = BaseDataEnum.namePrefix.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "خانم", Key = "1", Category = BaseDataEnum.namePrefix.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "آقای مهندس", Key = "2", Category = BaseDataEnum.namePrefix.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "خانم مهندس", Key = "3", Category = BaseDataEnum.namePrefix.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "آقای دکتر", Key = "4", Category = BaseDataEnum.namePrefix.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "خانم دکتر", Key = "5", Category = BaseDataEnum.namePrefix.ToString(), LanguageName = "fa" },
                   // اولویت تیکت
                   new BaseData { Text = "کم", Key = "0", Category = BaseDataEnum.ticketPriority.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "عادی", Key = "1", Category = BaseDataEnum.ticketPriority.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "بالا", Key = "2", Category = BaseDataEnum.ticketPriority.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "بحرانی", Key = "3", Category = BaseDataEnum.ticketPriority.ToString(), LanguageName = "fa" },
                   //نوع مجوز فعالیت شرکت
                   new BaseData { Text = "صنعتی", Key = "0", Category = BaseDataEnum.activityLicenseType.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "صنفی", Key = "1", Category = BaseDataEnum.activityLicenseType.ToString(), LanguageName = "fa" },
                   //وضعیت کاربری شرکت
                   new BaseData { Text = "در دست بررسی", Key = "0", Category = BaseDataEnum.userCompanyAccountStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "فعال", Key = "1", Category = BaseDataEnum.userCompanyAccountStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "راکد", Key = "2", Category = BaseDataEnum.userCompanyAccountStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "بلاک", Key = "3", Category = BaseDataEnum.userCompanyAccountStatus.ToString(), LanguageName = "fa" },
                   //وضعیت کاربری
                   new BaseData { Text = "غیر فعال", Key = "0", Category = BaseDataEnum.userAccountState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "فعال", Key = "1", Category = BaseDataEnum.userAccountState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "بلاک", Key = "3", Category = BaseDataEnum.userAccountState.ToString(), LanguageName = "fa" },

         


                   //وضعیت شرکت
                   new BaseData { Text = "در_حال_احداث", Key = "0", Category = BaseDataEnum.userCompanyStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "فعال", Key = "1", Category = BaseDataEnum.userCompanyStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "راکد", Key = "2", Category = BaseDataEnum.userCompanyStatus.ToString(), LanguageName = "fa" },

                   //نوع زمین
                   new BaseData { Text = "محصور", Key = "0", Category = BaseDataEnum.earthCondition.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "غیر محصور", Key = "1", Category = BaseDataEnum.earthCondition.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "در حال احداث", Key = "1", Category = BaseDataEnum.earthCondition.ToString(), LanguageName = "fa" },

                   //نوع شرکت
                   new BaseData { Text = "سهامی عام", Key = "0", Category = BaseDataEnum.companyOwnerType.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "سهامی خاص", Key = "1", Category = BaseDataEnum.companyOwnerType.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "با مسئولیت محدود", Key = "2", Category = BaseDataEnum.companyOwnerType.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "تعاونی", Key = "3", Category = BaseDataEnum.companyOwnerType.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "تضامنی", Key = "4", Category = BaseDataEnum.companyOwnerType.ToString(), LanguageName = "fa" },


                   //  نوع مجوز
                   new BaseData { Text = "کارت_شناسایی", Key = "0", Category = BaseDataEnum.activityLicense.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "گواهی_فعالیت", Key = "1", Category = BaseDataEnum.activityLicense.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "پروانه بهره برداری", Key = "2", Category = BaseDataEnum.activityLicense.ToString(), LanguageName = "fa" },

                   //مدرک برای شغل
                   new BaseData { Text = "مهم نیست", Key = "0", Category = BaseDataEnum.jobGrade.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "زیر دیپلم", Key = "1", Category = BaseDataEnum.jobGrade.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "دیپلم", Key = "20", Category = BaseDataEnum.jobGrade.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "دیپلم به بالا", Key = "21", Category = BaseDataEnum.jobGrade.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "فوق دیپلم", Key = "22", Category = BaseDataEnum.jobGrade.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "لیسانس", Key = "30", Category = BaseDataEnum.jobGrade.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "لیسانس به بالا", Key = "31", Category = BaseDataEnum.jobGrade.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "فوق لیسانس", Key = "40", Category = BaseDataEnum.jobGrade.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "فوق لیسانس به بالا", Key = "41", Category = BaseDataEnum.jobGrade.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "دکتری", Key = "50", Category = BaseDataEnum.jobGrade.ToString(), LanguageName = "fa" },
                   

                   
                   //مقاطع تحصیلی
                    new BaseData { Text = "بیسواد", Key = "0", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa" ,Description="سطح تحصیلات"},
                    new BaseData { Text = "ابتدايي", Key = "1", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "راهنمايي", Key = "2", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "متوسطه", Key = "3", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "دیپلم", Key = "6", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "فوق دیپلم", Key = "7", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "لیسانس", Key = "8", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "فوق لیسانس", Key = "10", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "دکتری", Key = "12", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "فلوشیپ", Key = "16", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "حوزوی", Key = "15", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },


                    //مقاطع تحصیلی
                    new BaseData { Text = "بیسواد", Key = "0", Category = BaseDataEnum.educationLevel.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "ابتدايي", Key = "1", Category = BaseDataEnum.educationLevel.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "راهنمايي", Key = "2", Category = BaseDataEnum.educationLevel.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "متوسطه", Key = "3", Category = BaseDataEnum.educationLevel.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "دیپلم", Key = "20", Category = BaseDataEnum.educationLevel.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "فوق دیپلم", Key = "25", Category = BaseDataEnum.educationLevel.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "لیسانس", Key = "30", Category = BaseDataEnum.educationLevel.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "فوق لیسانس", Key = "40", Category = BaseDataEnum.educationLevel.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "دکتری", Key = "50", Category = BaseDataEnum.educationLevel.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "فلوشیپ", Key = "60", Category = BaseDataEnum.educationLevel.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "حوزوی", Key = "70", Category = BaseDataEnum.educationLevel.ToString(), LanguageName = "fa" },
                    

                      //وضعیت فرصت شغلی
                   new BaseData { Text = "آموزشگاههای آزاد", Key = "0", Category = BaseDataEnum.amozeshOrganzationEnum.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "فنی و حرفه ایی", Key = "1", Category = BaseDataEnum.amozeshOrganzationEnum.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "میراث فرهنگی ", Key = "2", Category = BaseDataEnum.amozeshOrganzationEnum.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "شهرداری ", Key = "3", Category = BaseDataEnum.amozeshOrganzationEnum.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "کمیته امداد", Key = "4", Category = BaseDataEnum.amozeshOrganzationEnum.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "سایر", Key = "5", Category = BaseDataEnum.amozeshOrganzationEnum.ToString(), LanguageName = "fa" },

                   //  گروه شغلی
                   new BaseData { Text = "كارمند بخش دولتي", Key = "50", Category = BaseDataEnum.jobGroup.ToString(), LanguageName = "fa",Description="گروه شغلی" },
                   new BaseData { Text = "كارمند بخش خصوصی", Key = "51", Category = BaseDataEnum.jobGroup.ToString(), LanguageName = "fa", Description = "گروه شغلی" },
                   new BaseData { Text = "نظامی", Key = "52", Category = BaseDataEnum.jobGroup.ToString(), LanguageName = "fa", Description = "گروه شغلی" },
                   new BaseData { Text = "آزاد", Key = "53", Category = BaseDataEnum.jobGroup.ToString(), LanguageName = "fa", Description = "گروه شغلی" },
                   new BaseData { Text = "بازنشسته", Key = "54", Category = BaseDataEnum.jobGroup.ToString(), LanguageName = "fa", Description = "گروه شغلی" },
                   new BaseData { Text = "محصل و دانشجو", Key = "55", Category = BaseDataEnum.jobGroup.ToString(), LanguageName = "fa", Description = "گروه شغلی" },
                   new BaseData { Text = "سرباز", Key = "56", Category = BaseDataEnum.jobGroup.ToString(), LanguageName = "fa", Description = "گروه شغلی" },
                   new BaseData { Text = "كارگر", Key = "57", Category = BaseDataEnum.jobGroup.ToString(), LanguageName = "fa", Description = "گروه شغلی" },
                   new BaseData { Text = "بيكار", Key = "58", Category = BaseDataEnum.jobGroup.ToString(), LanguageName = "fa", Description = "گروه شغلی" },
                   new BaseData { Text = "خانه دار", Key = "59", Category = BaseDataEnum.jobGroup.ToString(), LanguageName = "fa", Description = "گروه شغلی" },
                   new BaseData { Text = "ساير", Key = "60", Category = BaseDataEnum.jobGroup.ToString(), LanguageName = "fa", Description = "گروه شغلی" },
                  


                   //وضعیت فرصت شغلی
                   new BaseData { Text = "فعال", Key = "0", Category = BaseDataEnum.jobStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "منقضی شده", Key = "1", Category = BaseDataEnum.jobStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "تکمیل شده", Key = "2", Category = BaseDataEnum.jobStatus.ToString(), LanguageName = "fa" },
                   //نوع گواهینامه
                   new BaseData { Text = "تاییده", Key = "0", Category = BaseDataEnum.typeOfDocumentEnum.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "گواهینامه", Key = "1", Category = BaseDataEnum.typeOfDocumentEnum.ToString(), LanguageName = "fa" },

                   // نوع حقوق
                   new BaseData { Text = "اداره کار", Key = "0", Category = BaseDataEnum.salaryType.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "توافقی", Key = "1", Category = BaseDataEnum.salaryType.ToString(), LanguageName = "fa" },

                     // نوع پرسش و پاسخ
                   new BaseData { Text = "غیر فعال", Key = "0", Category = BaseDataEnum.questionStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "تایید شده", Key = "1", Category = BaseDataEnum.questionStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "در حال بررسی", Key = "2", Category = BaseDataEnum.questionStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "حذف شده", Key = "3", Category = BaseDataEnum.questionStatus.ToString(), LanguageName = "fa" } ,
                          
                   //تراکنش بابت
                   new BaseData { Text = "خرید کارت", Key = "0", Category = BaseDataEnum.transactionFor.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "پکیج_کنکور", Key = "1", Category = BaseDataEnum.transactionFor.ToString(), LanguageName = "fa" },
                    new BaseData { Text = "تست درگاه", Key = "1000", Category = BaseDataEnum.transactionFor.ToString(), LanguageName = "fa" },

                   //نوع تراکنش 
                   new BaseData { Text = "بدهکار", Key = "-1", Category = BaseDataEnum.transactionType.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "بستانکار", Key = "1", Category = BaseDataEnum.transactionType.ToString(), LanguageName = "fa" },
                   // سوالات امنیتی
                   new BaseData { Text = "نام مدرسه ابتدایی شما چیست؟", Key = "نام مدرسه ابتدایی شما چیست؟", Category = BaseDataEnum.passwordQuestion.ToString(), LanguageName = "fa" ,Description="سوالات امنیتی"},
                   new BaseData { Text = "نام صمیمی ترین دوست شما چیست؟", Key = "نام صمیمی ترین دوست شما چیست؟", Category = BaseDataEnum.passwordQuestion.ToString(), LanguageName = "fa" ,  Description = "سوالات امنیتی" },
                   new BaseData { Text = "به کدام رنگ بیشتر علاقه دارید؟", Key = "به کدام رنگ بیشتر علاقه دارید؟", Category = BaseDataEnum.passwordQuestion.ToString(), LanguageName = "fa", Description = "سوالات امنیتی" },
                   new BaseData { Text = "کدام فصل از سال را دوست دارید؟", Key = "کدام فصل از سال را دوست دارید؟", Category = BaseDataEnum.passwordQuestion.ToString(), LanguageName = "fa", Description = "سوالات امنیتی" },

                   //وضعیت درخواست کارت
                   new BaseData { Text = "درخواست اولیه", Key = "0", Category = BaseDataEnum.cardRequestStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "درخواست جدید", Key = "1", Category = BaseDataEnum.cardRequestStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "ارسال برای چاپ", Key = "2", Category = BaseDataEnum.cardRequestStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "چاپ کارت", Key = "4", Category = BaseDataEnum.cardRequestStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "ارسال به پست", Key = "5", Category = BaseDataEnum.cardRequestStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "ارسال به مرکز تحویل", Key = "10", Category = BaseDataEnum.cardRequestStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "تحویل داده شد", Key = "16", Category = BaseDataEnum.cardRequestStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "برگشت داده شده", Key = "17", Category = BaseDataEnum.cardRequestStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "باطل شده", Key = "19", Category = BaseDataEnum.cardRequestStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "وضعیت نامعلوم", Key = "20", Category = BaseDataEnum.cardRequestStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                    //وضعیت مدرک شهروند
                   new BaseData { Text = "دردست بررسی", Key = "0", Category = BaseDataEnum.userDocumentStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "تایید شده", Key = "1", Category = BaseDataEnum.userDocumentStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "عدم تایید", Key = "2", Category = BaseDataEnum.userDocumentStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   //وضعیت شهروند
                   new BaseData { Text = "عدم تایید", Key = "0", Category = BaseDataEnum.sabtStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "تایید", Key = "1", Category = BaseDataEnum.sabtStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "فوتی", Key = "2", Category = BaseDataEnum.sabtStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "در حال استعلام", Key = "3", Category = BaseDataEnum.sabtStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },
                   new BaseData { Text = "استعلام نشده", Key = "4", Category = BaseDataEnum.sabtStatus.ToString(), LanguageName = "fa", Description = "وضعیت درخواست کارت" },

                   //وضعیت تصویر پرسنلی شهروند
                   new BaseData { Text = "تایید شده", Key = "1", Category = BaseDataEnum.personalPicture.ToString(), LanguageName = "fa", Description = "وضعیت تصویر پرسنلی" },
                   new BaseData { Text = "عدم تایید", Key = "0", Category = BaseDataEnum.personalPicture.ToString(), LanguageName = "fa", Description = "وضعیت تصویر پرسنلی" },
                   new BaseData { Text = "دردست بررسی", Key = "2", Category = BaseDataEnum.personalPicture.ToString(), LanguageName = "fa", Description = "وضعیت تصویر پرسنلی" },


                   //نوع ابطال کارت
                   new BaseData { Text = "بدون درخواست مجدد", Key = "1", Category = BaseDataEnum.cardCancellationType.ToString(), LanguageName = "fa", Description = "نوع ابطال کارت" },
                   new BaseData { Text = "با درخواست مجدد", Key = "0", Category = BaseDataEnum.cardCancellationType.ToString(), LanguageName = "fa", Description = "نوع ابطال کارت" },
                  //دلیل ابطال کارت
                   new BaseData { Text = "چاپ كارت با مشكل مواجعه شده است", Key = "چاپ كارت با مشكل مواجعه شده است", Category = BaseDataEnum.cardCancellationItem.ToString(), LanguageName = "fa", Description = "دلیل ابطال کارت" },
                   new BaseData { Text = "كارت به دليل مشكل فيزيكي توسط شهروند برگشت داده شده است", Key = "كارت به دليل مشكل فيزيكي توسط شهروند برگشت داده شده است", Category = BaseDataEnum.cardCancellationItem.ToString(), LanguageName = "fa", Description = "دلیل ابطال کارت" },
                   new BaseData { Text = "شهروند فوت شده است", Key = "شهروند فوت شده است", Category = BaseDataEnum.cardCancellationItem.ToString(), LanguageName = "fa", Description = "دلیل ابطال کارت" },
                   new BaseData { Text = "داري كارت مي باشد. واين درخواست اشتباها صادر شده است", Key = "داري كارت مي باشد. واين درخواست اشتباها صادر شده است", Category = BaseDataEnum.cardCancellationItem.ToString(), LanguageName = "fa", Description = "دلیل ابطال کارت" },
                   new BaseData { Text = "کارت توسط شهروند مفقود گردیده است", Key = "کارت توسط شهروند مفقود گردیده است", Category = BaseDataEnum.cardCancellationItem.ToString(), LanguageName = "fa", Description = "دلیل ابطال کارت" },
                   new BaseData { Text = "کارت توسط شهروند مخدوش گردیده است", Key = "کارت توسط شهروند مخدوش گردیده است", Category = BaseDataEnum.cardCancellationItem.ToString(), LanguageName = "fa", Description = "دلیل ابطال کارت" },
                   new BaseData { Text = "وضعیت درخواست کارت نامشخص می باشد .درخواست فعلی ابطال و درخواست جدید صادر می شود", Key = "وضعیت درخواست کارت نامشخص می باشد .درخواست فعلی ابطال و درخواست جدید صادر می شود", Category = BaseDataEnum.cardCancellationItem.ToString(), LanguageName = "fa", Description = "دلیل ابطال کارت" },
                      


                    //وضعیت تراکنش
                   new BaseData { Text = "عدم تایید", Key = "0", Category = BaseDataEnum.transactionState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "تایید شده", Key = "1", Category = BaseDataEnum.transactionState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "در دست بررسی", Key = "2", Category = BaseDataEnum.transactionState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "برگشت داده شده", Key = "3", Category = BaseDataEnum.transactionState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "پرداخت نشده", Key = "4", Category = BaseDataEnum.transactionState.ToString(), LanguageName = "fa" },





                  

                      // جسمی حرکتی غير ويلچري 
                   new BaseData { Text = "نخاعی", Key = "0", Category = BaseDataEnum.typ_JesmiHarekati_WheelChair.ToString(), LanguageName = "fa",Description= "جسمی حرکتی غير ويلچري" },
                   new BaseData { Text = "جسمی_حرکتی", Key = "1", Category = BaseDataEnum.typ_JesmiHarekati_WheelChair.ToString(), LanguageName = "fa", Description = "جسمی حرکتی غير ويلچري" },
                   //جسمی حرکتی ويلچري
                   new BaseData { Text = "ضعیف", Key = "0", Category = BaseDataEnum.typ_JesmiHarekati_WheelChair.ToString(), LanguageName = "fa", Description = "جسمی حرکتی ويلچري" },
                   new BaseData { Text = "متوسط", Key = "1", Category = BaseDataEnum.typ_JesmiHarekati_WheelChair.ToString(), LanguageName = "fa", Description = "جسمی حرکتی ويلچري" },
                   new BaseData { Text = "شدید", Key = "2", Category = BaseDataEnum.typ_JesmiHarekati_WheelChair.ToString(), LanguageName = "fa", Description = "جسمی حرکتی ويلچري" },
                   //جسمی حرکتی غير ويلچري
                   new BaseData { Text = "ضعیف", Key = "0", Category = BaseDataEnum.typ_JesmiHarekati_NoWheelChair.ToString(), LanguageName = "fa", Description = "جسمی حرکتی غير ويلچري" },
                   new BaseData { Text = "متوسط", Key = "1", Category = BaseDataEnum.typ_JesmiHarekati_NoWheelChair.ToString(), LanguageName = "fa", Description = "جسمی حرکتی غير ويلچري" },
                   new BaseData { Text = "شدید", Key = "2", Category = BaseDataEnum.typ_JesmiHarekati_NoWheelChair.ToString(), LanguageName = "fa", Description = "جسمی حرکتی غير ويلچري" },
                   //معلولين ذهنی
                   new BaseData { Text = "ضعیف", Key = "0", Category = BaseDataEnum.typ_Zehni.ToString(), LanguageName = "fa", Description = "معلولين ذهنی" },
                   new BaseData { Text = "متوسط", Key = "1", Category = BaseDataEnum.typ_Zehni.ToString(), LanguageName = "fa", Description = "معلولين ذهنی" },
                   new BaseData { Text = "شدید", Key = "2", Category = BaseDataEnum.typ_Zehni.ToString(), LanguageName = "fa", Description = "معلولين ذهنی" },
                   new BaseData { Text = "اوتیسم", Key = "3", Category = BaseDataEnum.typ_Zehni.ToString(), LanguageName = "fa", Description = "معلولين ذهنی" },
                   //بینایی
                   new BaseData { Text = "نابینا", Key = "0", Category = BaseDataEnum.typ_Binaei.ToString(), LanguageName = "fa", Description = "بینایی" },
                   new BaseData { Text = "کم بینا", Key = "1", Category = BaseDataEnum.typ_Binaei.ToString(), LanguageName = "fa", Description = "بینایی" },
                   //اعصاب و روان
                   new BaseData { Text = "ضعیف", Key = "0", Category = BaseDataEnum.typ_AsabRavan.ToString(), LanguageName = "fa", Description = "اعصاب و روان" },
                   new BaseData { Text = "شدید", Key = "1", Category = BaseDataEnum.typ_AsabRavan.ToString(), LanguageName = "fa", Description = "اعصاب و روان" },
                   //شنوایی
                   new BaseData { Text = "ناشنوا", Key = "0", Category = BaseDataEnum.typ_Shenavaei.ToString(), LanguageName = "fa", Description = "شنوایی" },
                   new BaseData { Text = "کم شنوا", Key = "1", Category = BaseDataEnum.typ_Shenavaei.ToString(), LanguageName = "fa", Description = "شنوایی" },
                   //زنان سرپرست خانواده 
                   new BaseData { Text = "همسر فوت شده", Key = "0", Category = BaseDataEnum.typ_ZananSarparast.ToString(), LanguageName = "fa", Description = "زنان سرپرست خانواده" },
                   new BaseData { Text = "از کار افتادگی همسر", Key = "1", Category = BaseDataEnum.typ_ZananSarparast.ToString(), LanguageName = "fa", Description = "زنان سرپرست خانواده" },
                   new BaseData { Text = "طلاق", Key = "2", Category = BaseDataEnum.typ_ZananSarparast.ToString(), LanguageName = "fa", Description = "زنان سرپرست خانواده" },
                   new BaseData { Text = "سایر", Key = "3", Category = BaseDataEnum.typ_ZananSarparast.ToString(), LanguageName = "fa", Description = "زنان سرپرست خانواده" },

                   new BaseData { Text = "دیالیزی", Key = "0", Category = BaseDataEnum.typ_SpecialDiseases.ToString(), LanguageName = "fa", Description = "بیماریهای خاص" },
                   new BaseData { Text = "تالاسمی", Key = "1", Category = BaseDataEnum.typ_SpecialDiseases.ToString(), LanguageName = "fa", Description = "بیماریهای خاص" },
                   new BaseData { Text = "هموفیلی", Key = "2", Category = BaseDataEnum.typ_SpecialDiseases.ToString(), LanguageName = "fa", Description = "بیماریهای خاص"},
                   new BaseData { Text = "ام اس ", Key = "3", Category = BaseDataEnum.typ_SpecialDiseases.ToString(), LanguageName = "fa", Description = "بیماریهای خاص" },


           
                   new BaseData { Text = "همه شهروندان", Key = "0", Category = BaseDataEnum.imagerReviewStatusFormFreeCard.ToString(), LanguageName = "fa", Description = "وضعیت تصویر برای صدور کارت رایگان" },
                   new BaseData { Text = "تصویر پرسنلی تایید شده", Key = "1", Category = BaseDataEnum.imagerReviewStatusFormFreeCard.ToString(), LanguageName = "fa", Description = "وضعیت تصویر برای صدور کارت رایگان" },
                   new BaseData { Text = "تصویر پرسنلی بارگذاری شده", Key = "2", Category = BaseDataEnum.imagerReviewStatusFormFreeCard.ToString(), LanguageName = "fa", Description = "وضعیت تصویر برای صدور کارت رایگان"},
                  



                   // دلیل عدم تایید تصویر شهروند
                   new BaseData { Text = "کیفیت نامناسب", Key = "کیفیت نامناسب", Category = BaseDataEnum.rejectCitizenPicture.ToString(), LanguageName = "fa", Description = "دلیل عدم تایید تصویر شهروند" },
                   new BaseData { Text = "اندازه نامناسب", Key = "اندازه نامناسب", Category = BaseDataEnum.rejectCitizenPicture.ToString(), LanguageName = "fa", Description = "دلیل عدم تایید تصویر شهروند" },
                   new BaseData { Text = "پوشش نامناسب", Key = "پوشش نامناسب", Category = BaseDataEnum.rejectCitizenPicture.ToString(), LanguageName = "fa", Description = "دلیل عدم تایید تصویر شهروند" },
                   new BaseData { Text = "عدم تطابق سن", Key = "عدم تطابق سن", Category = BaseDataEnum.rejectCitizenPicture.ToString(), LanguageName = "fa", Description = "دلیل عدم تایید تصویر شهروند" },
                   new BaseData { Text = "عدم تطابق  جنسیت", Key = "عدم تطابق  جنسیت  ", Category = BaseDataEnum.rejectCitizenPicture.ToString(), LanguageName = "fa", Description = "دلیل عدم تایید تصویر شهروند" },
                   new BaseData { Text = "تصویر نامناسب", Key = "تصویر نامناسب", Category = BaseDataEnum.rejectCitizenPicture.ToString(), LanguageName = "fa", Description = "دلیل عدم تایید تصویر شهروند" },
                   new BaseData { Text = "پس زمینه رنگی", Key = "پس زمینه رنگی", Category = BaseDataEnum.rejectCitizenPicture.ToString(), LanguageName = "fa", Description = "دلیل عدم تایید تصویر شهروند" },
                   new BaseData { Text = "نمایش ناقص چهره", Key = "اثر مهر در تصویر", Category = BaseDataEnum.rejectCitizenPicture.ToString(), LanguageName = "fa", Description = "دلیل عدم تایید تصویر شهروند" },
                   new BaseData { Text = "استفاده از کراوات", Key = "استفاده از کراوات", Category = BaseDataEnum.rejectCitizenPicture.ToString(), LanguageName = "fa", Description = "دلیل عدم تایید تصویر شهروند" },





                   //وضعیت بررسی فرم منزلت
                   new BaseData { Text = "در حال بررسی", Key = "0", Category = BaseDataEnum.manzalatFormStatuse.ToString(), LanguageName = "fa", Description = "وضعیت بررسی فرم منزلت" },
                   new BaseData { Text = "تایید", Key = "1", Category = BaseDataEnum.manzalatFormStatuse.ToString(), LanguageName = "fa", Description = "وضعیت بررسی فرم منزلت" },
                   new BaseData { Text = "عدم تایید", Key = "2", Category = BaseDataEnum.manzalatFormStatuse.ToString(), LanguageName = "fa", Description = "وضعیت بررسی فرم منزلت" },

                   //نوع تحویل کارت به شهروند
                   new BaseData { Text = "پستی", Key = "1", Category = BaseDataEnum.cardDeliverType.ToString(), LanguageName = "fa", Description = "نوع تحویل کارت به شهروند" },
                   new BaseData { Text = "تحویل در مرکز", Key = "2", Category = BaseDataEnum.cardDeliverType.ToString(), LanguageName = "fa", Description = "نوع تحویل کارت به شهروند" },
                   
                    //نوع صف ورودی توزیع کارت
                   new BaseData { Text = "پستی", Key = "0", Category = BaseDataEnum.queueInputType.ToString(), LanguageName = "fa", Description = "نوع صف ورودی توزیع کارت" },
                   new BaseData { Text = "تحویل در مرکز", Key = "1", Category = BaseDataEnum.queueInputType.ToString(), LanguageName = "fa", Description = "نوع صف ورودی توزیع کارت" },
                   new BaseData { Text = "صف موقت", Key = "2", Category = BaseDataEnum.queueInputType.ToString(), LanguageName = "fa", Description = "نوع صف ورودی توزیع کارت" },
                   





                   //دین شهروند
                   new BaseData { Text = "اسلام", Key = "0", Category = BaseDataEnum.religion.ToString(), LanguageName = "fa", Description = "دین" },
                   new BaseData { Text = "مسیحیت", Key = "1", Category = BaseDataEnum.religion.ToString(), LanguageName = "fa", Description = "دین" },
                   new BaseData { Text = "یهودیت", Key = "2", Category = BaseDataEnum.religion.ToString(), LanguageName = "fa", Description = "دین" },
                   new BaseData { Text = "زرتشت", Key = "3", Category = BaseDataEnum.religion.ToString(), LanguageName = "fa", Description = "دین" },
                   new BaseData { Text = "سایر", Key = "4", Category = BaseDataEnum.religion.ToString(), LanguageName = "fa", Description = "دین" },





                  //نوع فعالیت شرکت
                   new BaseData { Text = "صنعتی", Key = "0", Category = BaseDataEnum.companyFieldOfActivity.ToString(), Description = "وضعیت مجوز خروج بار", LanguageName = "fa" },
                   new BaseData { Text = "خدماتی", Key = "1", Category = BaseDataEnum.companyFieldOfActivity.ToString(), Description = "وضعیت مجوز خروج بار", LanguageName = "fa" },
                   new BaseData { Text = "بازرگانی", Key = "2", Category = BaseDataEnum.companyFieldOfActivity.ToString(), Description = "وضعیت مجوز خروج بار", LanguageName = "fa" },
                   new BaseData { Text = "کشاورزی", Key = "3", Category = BaseDataEnum.companyFieldOfActivity.ToString(), Description = "وضعیت مجوز خروج بار", LanguageName = "fa" },
                   new BaseData { Text = "کارگاهی", Key = "4", Category = BaseDataEnum.companyFieldOfActivity.ToString(), Description = "وضعیت مجوز خروج بار", LanguageName = "fa" },
                   new BaseData { Text = "توزیعی", Key = "5", Category = BaseDataEnum.companyFieldOfActivity.ToString(), Description = "وضعیت مجوز خروج بار", LanguageName = "fa" },
                   new BaseData { Text = "تولیدی", Key = "6", Category = BaseDataEnum.companyFieldOfActivity.ToString(), Description = "وضعیت مجوز خروج بار", LanguageName = "fa" },


                   //وضعیت تیکت 
                   new BaseData { Text = "جدید", Key = "0", Category = BaseDataEnum.ticketStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "در دست بررسی", Key = "1", Category = BaseDataEnum.ticketStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "پاسخ داده شده", Key = "2", Category = BaseDataEnum.ticketStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "منتظر پاسخ کاربر", Key = "3", Category = BaseDataEnum.ticketStatus.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "پاسخ توسط کاربر", Key = "4", Category = BaseDataEnum.ticketStatus.ToString(), LanguageName = "fa" },
                    
                    };
                    await _BaseDataRepository.AddRangeAsync(listAdd);
                    _uow.SaveChanges();

                
            }
            catch (Exception)
            {


            }

            return res;
        }

        public Task<List<BaseDataModel>> GetBaseListAsync(string query, int offset, int count)
        {
            return _BaseDataRepository
                       .Where(w =>   w.Text.Contains(query))
                       .OrderByDescending(o => o.IndexOrder).Skip(offset).Take(count).Select
                       (s => new BaseDataModel()
                       {

                           Text = s.Text,
                           Key = s.Key.ToString()
                       })
                       .ToListAsync();

        }


        public Task<List<BaseDataModel>> GetJobGroups()
        {
            return _jobGroup.Where(w => w.IsDeleted!=true)
                       .OrderBy(o => o.Title).Select
                       (s => new BaseDataModel()
                       {

                           Text = s.Title,
                           Key = s.Id.ToString()
                       })
                       .ToListAsync();

        }




        public void SaveChange()
        {
            _uow.SaveChanges();
        }

        #endregion









        #region private Methode

        private bool CanDelete(int id, out List<GlobalError> errors)
        {
            errors = new List<GlobalError>();
            bool flag = true;

            return flag;

        }

        #endregion



















    }


}