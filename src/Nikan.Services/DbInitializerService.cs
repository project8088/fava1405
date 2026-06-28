using System;
using System.Linq;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nikan.DomainClasses.BaseEntity;
using Nikan.DomainClasses.UserCompanes;
using System.Collections.Generic;
using Nikan.DomainClasses.Job;
using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.NewsItem;
 
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.Permissions;
using Nikan.DomainClasses.UserDocuments;

namespace Nikan.Services
{
    public interface IDbInitializerService
    {
        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Adds some default values to the Db
        /// </summary>
        void SeedData();

          void SetSettings();



    }

    public class DbInitializerService : IDbInitializerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ISecurityService _securityService;

        public DbInitializerService(
            IServiceScopeFactory scopeFactory,
            ISecurityService securityService)
        {
            _scopeFactory = scopeFactory;
            _scopeFactory.CheckArgumentIsNull(nameof(_scopeFactory));

            _securityService = securityService;
            _securityService.CheckArgumentIsNull(nameof(_securityService));
        }

        public void Initialize()
        {
           var serviceScope = _scopeFactory.CreateScope();
             var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();
        }

        public void SeedData()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    // Add default roles
                    var adminRole = new Role { Name = CustomRoles.Admin };
                    var userRole = new Role { Name = CustomRoles.User };
                    var citizenRole = new Role { Name = CustomRoles.Citizen };
                    var CompanyRole = new Role { Name = CustomRoles.Company };
                    var CardRole = new Role { Name = CustomRoles.Card };
                    var WebApiUserRole = new Role { Name = CustomRoles.WebApiUser };
                    if (!context.Roles.Any())
                    {
                        context.Add(adminRole);
                        context.Add(userRole);
                        context.Add(citizenRole);
                        context.Add(CompanyRole);
                        context.Add(CardRole);
                        context.Add(WebApiUserRole);

                        context.SaveChanges();
                    }

                    // Add Admin user
                    if (!context.Users.Any())
                    {
                        var adminUser = new User
                        {
                            Username = "Nikan",
                            DisplayName = "شرکت فناوری اطلاعات نیکان",
                            UserAccountState = userAccountStateEnum.فعال,
                            LastLoggedIn = null,
                            Password = _securityService.GetSha256Hash("123456"),
                            SerialNumber = Guid.NewGuid().ToString("N")
                        };
                        context.Add(adminUser);
                        context.SaveChanges();
                        context.Add(new UserRole { Role = adminRole, User = adminUser });
                        context.Add(new UserRole { Role = userRole, User = adminUser });
                        context.Add(new UserRole { Role = citizenRole, User = adminUser });
                        context.Add(new UserRole { Role = CompanyRole, User = adminUser });
                        context.Add(new UserRole { Role = CardRole, User = adminUser });



                        context.SaveChanges();
                    }
                }
            }
        }


        public void SetSettings()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                

                // How to add initial data to the DB directly
                using (var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    if (!context.SiteOption.Any())
                    {
                        context.SiteOption.AddRange(
                   //تنظیمات اصلی سایت

                  new SiteOption { Key = "SiteUrl", Value = "" },
                  new SiteOption { Key = "SiteName" },
                  new SiteOption { Key = "FullSiteName" },
                  new SiteOption { Key = "FooterText" },
                  new SiteOption { Key = "LogoUrl" },
                  //اطلاعات تماس

                  new SiteOption { Key = "Addresss" },
                  new SiteOption { Key = "SMSNumber" },
                  new SiteOption { Key = "TelegramChannelId" },
                  new SiteOption { Key = "FaxNumber" },
                  new SiteOption { Key = "CellNumber" },
                  new SiteOption { Key = "BusinessHours" },
                  new SiteOption { Key = "MobileNumber" },
                  new SiteOption { Key = "EmailAddress" },

                  //اطلاعات سئو
                  new SiteOption { Key = "SiteKeywords" },
                  new SiteOption { Key = "SiteDescription" },
                  //ایمیل
                  new SiteOption { Key = "MailServerUrl" },
                  new SiteOption { Key = "MailServerPort" },
                  new SiteOption { Key = "MailServerUserName" },
                  new SiteOption { Key = "MailServerPassword" },

                  //قوانین و مقررات

                  new SiteOption { Key = "NgoEthicalCharter" },
                  new SiteOption { Key = "VolunteersEthicalCharter" },
                  new SiteOption { Key = "SponsorsEthicalCharter" }


             );
                        context.SaveChanges();
                    }

                    if (!context.BaseData.Any())
                    {


                        context.BaseData.AddRange(
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

                   new BaseData { Text = "بیماران خاص", Key = "5", Category = BaseDataEnum.manzalatFormType.ToString(), LanguageName = "fa", Description = "عضویت در طرح منزلت" },
                   new BaseData { Text = "مادران دارای سه فرزند", Key = "6", Category = BaseDataEnum.manzalatFormType.ToString(), LanguageName = "fa", Description = "عضویت در طرح منزلت" }, 
                   new BaseData { Text = "دانش آموزان تحت پوشش کمیته امداد امام خمینی و سازمان بهزیستی", Key = "7", Category = BaseDataEnum.manzalatFormType.ToString(), LanguageName = "fa", Description = "عضویت در طرح منزلت" },



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
                    new BaseData { Text = "دیپلم", Key = "20", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "فوق دیپلم", Key = "25", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "لیسانس", Key = "30", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "فوق لیسانس", Key = "40", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "دکتری", Key = "50", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "فلوشیپ", Key = "60", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },
                    new BaseData { Text = "حوزوی", Key = "70", Category = BaseDataEnum.eduGrade.ToString(), LanguageName = "fa", Description = "سطح تحصیلات" },


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



                   //وضعیت تراکنش
                   new BaseData { Text = "در دست بررسی", Key = "0", Category = BaseDataEnum.transactionState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "عدم تایید", Key = "1", Category = BaseDataEnum.transactionState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "تایید شده", Key = "2", Category = BaseDataEnum.transactionState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "پرداخت شده", Key = "3", Category = BaseDataEnum.transactionState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "برگشت داده شده", Key = "4", Category = BaseDataEnum.transactionState.ToString(), LanguageName = "fa" },
                   new BaseData { Text = "پرداخت نشده", Key = "5", Category = BaseDataEnum.transactionState.ToString(), LanguageName = "fa" },


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
                   new BaseData { Text = "پاسخ توسط کاربر", Key = "4", Category = BaseDataEnum.ticketStatus.ToString(), LanguageName = "fa" }







  );

                        context.SaveChanges();
                    }
                    if (!context.City.Any())
                    {

                        var models = new List<City>();

                        var arr = new string[] { "هریس", "ورزقان", "میانه", "ملکان", "مرند", "مراغه", "کلیبر" };

                        var id = 100;
                        models.Add(new City()
                        {
                            Id = id,
                            IsActive = true,
                            Code = "041",
                            Title = "آذربایجان شرقی"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 100,
                                Code = "041",
                                Title = item.Trim()
                            });
                        }



                        id = 200;
                        arr = new string[] {"ارومیه","اشنویه","بوکان","پیرانشهر","تکاب","چالدران","خوی",
                                            "سردشت","سلماس","شاهین‌دژ","ماکو","مهاباد","میاندوآب","نقده"};

                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "044",
                            Title = "آذربایجان غربی"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 200,
                                Code = "044",
                                Title = item.Trim()
                            });
                        }





                        id = 300;
                        arr = new string[] {"اردبیل","بیله‌سوار","پارس‌آباد","خلخال","کوثر",
                                             "گِرمی","مِشگین‌شهر","نَمین","نیر"};




                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "045",
                            Title = "اردبیل"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 300,
                                Code = "045",
                                Title = item.Trim()
                            });
                        }


                        id = 400;
                        arr = new string[] {"آران و بیدگل","اردستان","اصفهان","برخوار و میمه","تیران و کرون","چادگان","خمینی‌شهر",
                                             "خوانسار","سمیرم","شهرضا","سمیرم سفلی","فریدن","فریدون‌شهر","فلاورجان","کاشان","گلپایگان",
                                             "لنجان","مبارکه","نائین","نجف‌آباد","نطنز"};

                        models.Add(new City(id, null, "031", "اصفهان"));
                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 400,
                                Code = "031",
                                Title = item.Trim()
                            });
                        }
                        id = 500;
                        arr = new string[] { "آبدانان", "ایلام", "ایوان", "دره‌شهر", "دهلران", "شیروان و چرداول", "مهران" };



                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "0841",
                            Title = "ایلام"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 500,
                                Code = "0841",
                                Title = item.Trim()
                            });
                        }

                        id = 600;
                        arr = new string[] { "بوشهر", "تنگستان", "جم", "دشتستان", "دشتی", "دیر", "دیلم", "کنگان", "گناوه" };




                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "0771",
                            Title = "بوشهر"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 600,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }


                        id = 700;

                        arr = new string[] {"اسلام‌شهر","پاکدشت","تهران","دماوند","رباط‌کریم","ری","ساوجبلاغ","شمیرانات","شهریار",
                                            "فیروزکوه","کرج","نظرآباد","ورامین"};


                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "021",
                            Title = "تهران"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 700,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }



                        id = 800;

                        arr = new string[] { "اردل", "بروجن", "شهرکرد", "فارسان", "کوهرنگ", "لردگان" };



                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "0381",
                            Title = "چهارمحال و بختیاری"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 800,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }
                        id = 1000;
                        arr = new string[] { "بیرجند", "درمیان", "سرایان", "سربیشه", "فردوس", "قائنات", "نهبندان" };


                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "0561",
                            Title = "خراسان جنوبی"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 1000,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }



                        id = 1100;
                        arr = new string[] {"بردسکن","تایباد","تربت جام","تربت حیدریه","چناران","خلیل‌آباد"
                                           ,"خواف","درگز","رشتخوار","سبزوار" ,"سرخس","فریمان" ,"قوچان","کاشمر" ,"کلات","گناباد" ,"مشهد","مه ولات" ,"نیشابور"};





                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "051",
                            Title = "خراسان رضوی"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 1100,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }

                        id = 1200;
                        arr = new string[] { "اسفراین", "بجنورد", "جاجرم", "شیروان", "فاروج", "مانه و سملقان" };


                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "0584",
                            Title = "خراسان شمالی"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 1200,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }

                        id = 1300;

                        arr = new string[] {"آبادان","امیدیه","اندیمشک","اهواز","ایذه","باغ‌ملک","بهبهان","خرمشهر",
              "دزفول","دشت آزادگان","رامشیر" ,"رامهرمز","شادگان","شوشتر","گتوند","لالی","مسجد سلیمان"
              ,"هندیجان","بندر ماهشهر"};




                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "0611",
                            Title = "خوزستان"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 1300,
                                Code = "0611",
                                Title = item.Trim()
                            });
                        }


                        id = 1400;

                        arr = new string[] { "ابهر", "ایجرود", "خدابنده", "خرمدره", "زنجان", "طارم", "ماه‌نشان" };




                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "024",
                            Title = "زنجان"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 1400,
                                Code = "024",
                                Title = item.Trim()
                            });
                        }


                        id = 1500;

                        arr = new string[] { "دامغان", "سمنان", "شاهرود", "گرمسار", "مهدی‌شهر" };


                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "023",
                            Title = "سمنان"
                        });



                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 1500,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }



                        id = 1600;
                        arr = new string[] {"ایرانشهر","چابهار","خاش","دلگان","زابل","زاهدان","زهک","سراوان",
           "سرباز","کنارک","نیک‌شهر" };





                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "054",
                            Title = "سیستان و بلوچستان"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 1600,
                                Code = "054",
                                Title = item.Trim()
                            });
                        }
                        id = 1700;

                        arr = new string[] {"آباده","ارسنجان","استهبان","اقلید","بوانات","پاسارگاد","جهرم",
           "خرم‌بید","خنج","داراب","زرین‌دشت","سپیدان","شیراز","فراشبند","فسا","فیروزآباد","قیر",
           "کارزین","کازرون","لارستان","لامِرد","مرودشت","ممسنی","مهر","نی‌ریز"};


                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "071",
                            Title = "فارس"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 1700,
                                Code = "071",
                                Title = item.Trim()
                            });
                        }

                        id = 1800;
                        arr = new string[] { "آبیک", "البرز", "بوئین‌زهرا", "تاکستان", "قزوین" };

                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "028",
                            Title = "قزوین"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 1800,
                                Code = "028",
                                Title = item.Trim()
                            });
                        }
                        id = 1900;

                        arr = new string[] { "قم", };


                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "025",
                            Title = "قم"
                        });
                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 1900,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }

                        id = 2000;

                        arr = new string[] {"بانه","بیجار","دیواندره","سروآباد","سقز","سنندج","قروه","کامیاران",
           "مریوان",};



                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "0871",
                            Title = "کردستان"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 2000,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }
                        id = 2100;

                        arr = new string[] {"بافت","بردسیر","بم","جیرفت","راور","رفسنجان","رودبار جنوب","زرند",
           "سیرجان","شهر بابک","عنبرآباد","قلعه گنج","کرمان","کوهبنان","کهنوج","منوجان"};



                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "0341",
                            Title = "کرمان"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 2100,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }
                        id = 2200;

                        arr = new string[] {"اسلام‌آباد غرب","پاوه","ثلاث باباجانی","جوانرود","دالاهو","روانسر",
           "سرپل ذهاب","سنقر","صحنه","قصر شیرین","کرمانشاه","کنگاور","گیلان غرب","هرسین"};



                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "083",
                            Title = "کرمانشاه"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 2200,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }

                        id = 2300;

                        arr = new string[] { "بویراحمد", "بهمئی", "دنا", "گچساران", "کهگیلویه", };



                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "074",
                            Title = "کهگیلویه و بویراحمد"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 2300,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }
                        id = 2400;
                        arr = new string[] {"آزادشهر","آق‌قلا","بندر گز","ترکمن","رامیان","علی‌آباد","کردکوی",
           "کلاله","گرگان","گنبد کاووس","مراوه‌تپه","مینودشت",};
                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "017",
                            Title = "گلستان"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 2400,
                                Code = "017",
                                Title = item.Trim()
                            });
                        }

                        id = 2500;

                        arr = new string[] {"آستارا","آستانه اشرفیه","اَملَش","بندر انزلی","رشت","رضوانشهر",
           "رودبار","رودسر","سیاهکل","شَفت","صومعه‌سرا","طوالش","فومَن","لاهیجان","لنگرود","ماسال",};




                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "0131",
                            Title = "گیلان"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 2500,
                                Code = "0131",
                                Title = item.Trim()
                            });
                        }

                        id = 2600;

                        arr = new string[] {"ازنا","الیگودرز","بروجرد","پل‌دختر","خرم‌آباد","دورود","دلفان","سلسله",
           "کوهدشت"};


                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "066",
                            Title = "لرستان"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 2600,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }
                        id = 2700;
                        arr = new string[] {"آمل","بابل","بابلسر","بهشهر","تنکابن","جویبار","چالوس","رامسر",
           "ساری","سوادکوه","قائم‌شهر","گلوگاه","محمودآباد","نور","نکا","نوشهر"};



                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "011",
                            Title = "مازندران"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 2700,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }
                        id = 2800;

                        arr = new string[] {"آشتیان","اراک","تفرش","خمین","دلیجان","زرندیه","ساوه","شازند",
           "کمیجان","محلات"};

                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "086",
                            Title = "مرکزی"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 2800,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }
                        id = 2900;

                        arr = new string[] {"ابوموسی","بستک","بندر عباس","بندر لنگه","جاسک","حاجی‌آباد",
           "شهرستان خمیر","رودان","قشم","کیش","عسلویه","گاوبندی","میناب" };



                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "0761",
                            Title = "هرمزگان"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 2900,
                                Code = "0771",
                                Title = item.Trim()
                            });
                        }

                        id = 3000;

                        arr = new string[] {"اسدآباد","بهار","تویسرکان",
               "رزن","کبودرآهنگ","ملایر","نهاوند","همدان", };



                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "081",
                            Title = "همدان"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 3000,
                                Code = "081",
                                Title = item.Trim()
                            });
                        }
                        id = 3100;

                        arr = new string[] {"ابرکوه","اردکان","بافق","تفت","خاتم",
               "صدوق","طبس","مهریز","مِیبُد","یزد"};



                        models.Add(new City()
                        {
                            IsActive = true,
                            Id = id,
                            Code = "035",
                            Title = "یزد"
                        });

                        foreach (var item in arr)
                        {
                            id++;
                            models.Add(new City()
                            {
                                Id = id,
                                ParentId = 3100,
                                Code = "035",
                                Title = item.Trim()
                            });
                        }



                        context.City.AddRange(models);
                        context.SaveChanges();



                    }
                    if (!context.Skill.Any())
                    {
                        var models = new List<Skill>();
                        string[] arr1 = new string[] {"GIS" ,"HSE","IT","MBA","PLC","آبدارچی","آرایشگر","آرایشگر زنانه","آشپز",
          "آشپز فست فود","آمارگیر","اتوکد کار","ادبیات فارسی","انباردار","اپتومتریست","اپراتور"
           ,"اپراتور CNC","اپراتور کامپیوتر","ایمنی","بازاریاب","بازاریاب بیمه","بازاریاب مواد غذایی",
          "بازرس جوش","بازنشسته","بازیگر","باغبان","برقکار","برقکار صنعتی"
                ,"برنامه نویس","برنامه نویس Asp.Net","برنامه نویس c#","برنامه نویس c++","برنامه نویس PHP",
          "برنامه نویس اندروید","برنامه نویس جاوا","برنامه نویس وب","برنامه نویس پایتون","بسته بند",
          "بهداشت حرفه ای","بهداشت خانواده"
                ,"بهداشت عمومی","بهداشت محیط","بهیار","بیوتکنولوژی","بیولوژیست","بیوشیمی","تایپیست","تحصیلدار",
          "تحلیلگر سیستم","تدوینگر","توزیع کننده","تکنسین اتاق عمل"
                ,"تراشکار","تعمیرکار","تکنسین","تکنسین الکترونیک","تکنسین برق","تکنسین بیهوشی","تکنسین تاسیسات",
          "تکنسین داروخانه","تکنسین درمانی","تکنسین شبکه","تکنسین عمران","تکنسین مکانیک"
,"تکنسین کامپیوتر","جامعه شناس","جوشکار","حراست","حسابدار","حسابدار ارشد","حسابدار صنعتی","حسابدار فروش",
          "حسابرس","خبرنگار","خیاط","داروساز"
,"دامپزشک","دستیار دندانپزشک","دندانپزشک","دیپلم الکترونیک","دیپلم برق","دیپلم برق صنعتی","دیپلم تجربی",
          "دیپلم حسابداری","دیپلم ریاضی","دیپلم فنی","دیپلم مکانیک","دیپلم کامپیوتر"
,"رئیس حسابداری","راننده","راننده بیل مکانیکی","راننده جرثقیل","راننده لیفتراک","راننده وانت",
          "راننده پایه یک","راننده کامیونت","روانشناس","ریاضی","زبان انگلیسی","زمین شناسی"
,"زیست شناسی","سرآشپز","سرایدار","سرپرست انبار","سرپرست فروش","سرپرست کارگاه","سرپرست کارگاه ساختمانی",
          "شیمی","صنایع غذایی","صنایع چوب","صندوقدار","طراح"
,"طراح دکوراسیون داخلی","طراح سه بعدی","طراح صنعتی","طراح لباس","طراح وب","علوم آزمایشگاهی","علوم اجتماعی",
          "علوم تربیتی","علوم سیاسی","عکاس","فروشنده","فوریتهای پزشکی"
,"فوق دیپلم الکترونیک","فوق دیپلم برق","فوق دیپلم حسابداری","فوق دیپلم شیمی","فوق دیپلم عمران",
          "فوق دیپلم معماری","فوق دیپلم مکانیک","فوق دیپلم مکانیک خودرو","فوق دیپلم کامپیوتر","فوق دیپلم گرافیک",
          "فیزیوتراپیست","فیزیک","فیلمبردار","ماساژور","ماما","مامور خرید"
           ,"مترجم","مترجم ترکی استانبولی","مترجم زبان آلمانی","مترجم زبان انگلیسی","مترجم زبان روسی","مترجم عربی",
          "مترجم فرانسه","مدارک پزشکی","مددکار اجتماعی","مدرس","مدرس ریاضی","مدرس زبان انگلیسی"
           ,"مدرس کامپیوتر","مدیر","مدیر it","مدیر آموزش","مدیر اجرایی","مدیر اداری","مدیر بازاریابی",
          "مدیر بازرگانی","مدیر برنامه ریزی","","",""
           ,"مدیر بازرگانی خارجی","مدیر تبلیغات","مدیر تضمین کیفیت","مدیر تولید","مدیر داخلی","مدیر رستوران",
          "مدیر روابط عمومی","مدیر سایت","مدیر عامل","مدیر فروش","مدیر فنی","مدیر مالی"
           ,"مدیر منابع انسانی","مدیر پروژه","مدیر کارخانه","مدیر کنترل","کیفیت   مدیریت","مدیریت آموزشی",
          "مدیریت اجرایی","مدیریت بازرگانی","مدیریت بیمه","مدیریت جهانگردی","مدیریت دولتی","مدیریت صنعتی"
           ,"مدیریت مالی","مربی","مربی مهد کودک","مسئول دفتر","مسئول فنی","مسئول فنی صنایع غذایی",
          "مسئول چیدمان","مشاور","مشاور املاک","مشاور تحصیلی","مشاور حقوقی","منشی","منشی آموزشگاه",
          "منشی دفتر وکالت","منشی مطب","مهماندار","مهندس - سایر","مهندس آب","مهندس آبیاری","مهندس ابزار دقیق",
          "مهندس اقتصاد کشاورزی","مهندس الکترونیک","مهندس انرژی","مهندس برق","مهندس برق قدرت","مهندس برق کنترل",
          "مهندس تاسیسات","مهندس حمل و نقل","مهندس دامپروری","مهندس رباتیک","مهندس رنگ","مهندس ساخت و تولید",
          "مهندس ساختمان","مهندس سازه","مهندس سخت افزار","مهندس سرامیک","مهندس شهرسازی","مهندس شیلات","مهندس شیمی",
          "مهندس صنایع","مهندس عمران","مهندس فرایند","مهندس فروش","مهندس فضای سبز","مهندس فناوری اطلاعات",
          "مهندس ماشینهای کشاورزی","مهندس متالورژی","مهندس مخابرات","مهندس معدن","مهندس معمار","مهندس مواد",
          "مهندس مکاترونیک","مهندس مکانیک","مهندس مکانیک جامدات","مهندس مکانیک خودرو","مهندس مکانیک سیالات",
          "مهندس ناظر","مهندس نرم افزار","مهندس نساجی","مهندس نفت","مهندس پایپینگ","مهندس پزشکی","مهندس پلیمر",
          "مهندس ژئوتکنیک","مهندس کامپیوتر","مهندس کشاورزی","مونتاژکار","میکروبیولوژی","ناخن کار","نجار","نسخه پیچ",
          "نصاب","نصاب کابینت","نظافتچی","نقاش","نقشه بردار","نقشه کش ساختمان","نقشه کش صنعتی",
          "نقشه کش معماری","نماینده علمی","نویسنده","نگهبان","نیروی خدماتی","هنر","هوافضا","وکیل","ویراستار",
          "ویزیتور","پرستار","پرسشگر","پزشک عمومی","پشتیبان","پشتیبان شبکه","پشتیبان نرم افزار","پژوهشگر","پیراپزشک",
          "پیک موتوری","ژئوفیزیک","ژنتیک","کارآموز","کارشناس - سایر","کارشناس آزمایشگاه","کارشناس آمار",
          "کارشناس آموزش","کارشناس اقتصاد","کارشناس بازرگانی","کارشناس بازرگانی خارجی","کارشناس بیمه",
          "کارشناس تبلیغات","کارشناس تربیت بدنی","کارشناس تضمین کیفیت","کارشناس تغذیه","کارشناس جغرافیا",
          "کارشناس حقوق","کارشناس خرید","کارشناس رادیولوژی","کارشناس روابط عمومی","کارشناس شبکه","کارشناس علوم دامی",
          "کارشناس فروش","کارشناس فنی","کارشناس فنی خودرو","کارشناس مالی","کارشناس محیط زیست","کارشناس منابع انسانی",
          "کارمند آژانس هواپیمایی","کارمند اداری","کارمند بیمه", "کارگر ساده",
          "کارگر ماهر","کافی من","کتابدار","کمک آشپز","کمک بهیار","کمک حسابدار","کمک حسابرس","کمک نقشه بردار",
          "کنترل پروژه","کنترل کیفیت","گرافیست","کار در منزل","Mysql","Sql Server","بانک اطلاعاتی Oracel","برنامه نویسی Android","برنامه نویسی ASP.NET","برنامه نویسی C#",
               "برنامه نویسی C++","برنامه نویسی Java","برنامه نویسی Javascript" ,"برنامه نویسی Perl","برنامه نویسی PHP","برنامه نویسی Python","نرم افزار Illustrator"
                ,"سیستم مدیریت محتوای Joomla","سیستم مدیریت محتوای Wordpress","مهارت شبکه","نرم افزار D Max","نرم افزار After Effect"
            ,"نرم افزار AutoCad","نرم افزار Catia","نرم افزار CorelDraw","نرم افزار DreamWeaver","نرم افزار Etabs","نرم افزار Flash"
            ,"نرم افزار Freehand","نرم افزار InDesign","نرم افزار Matlab"
            ,"نرم افزار Maya","نرم افزار Microsoft Access","نرم افزار Microsoft Excel","نرم افزار Microsoft Powerpoint","نرم افزار Microsoft Publisher",
           "نرم افزار Microsoft Project","نرم افزار Microsoft Word","نرم افزار Minitab","نرم افزار OneNote","نرم افزار Photoshop",
           "نرم افزار Primavera","نرم افزار SAS","نرم افزار Solidworks","نرم افزار SPSS","کار با Linux","کار با اینترنت","کد نویسی CSS"};
                        int id = 100;
                        foreach (var item in arr1)
                        {
                            id++;
                            if (item.Trim() != "")
                            {
                                models.Add(new Skill()
                                {
                                    Id = id,
                                    IsActive = true,
                                    Title = item.Trim()
                                });
                            }

                        }
                        context.Skill.AddRange(models);
                        context.SaveChanges();


                    }
                    if (!context.Major.Any())
                    {
                        var models = new List<Major>();
                        string[] arr1 = new string[] {"EMBA","GIS","HSE","ICT","ICT انتقال","ICT انتقال داده",
           "ICT برق","ICT سخت افزار","ICT شبکه","ICT مخابرات","ICT مدیریت","ICT کاربردها","IT",
           "IT امنیت اطلاعات","IT تجارت الکترونیک","IT سایر گرایش ها","IT سیستم های اطلاعاتی",
           "IT شبکه های کامپیوتری",
           "IT طراحی صفحات وب ","IT طراحی و تولید نرم افزار","IT مخابرات","IT مدیریت دانش",
           "IT مهندسی تکنولوژی",
           "IT مهندسی نرم افزار"
            ,"IT گرافیک","MBA","MBA استراتژی","MBA بازاریابی","MBA فناوری اطلاعات ","MBA مالی",
           "MBA مدیریت اجرایی",
           "MBA منابع انسانی"
            ,"MBA کارآفرینی","آب و هوا شناسی","آب و هوا شناسی جغرافیا","آب و هوا شناسی صنعت","آتش نشانی",
           "آتش نشانی آتش نشان"
            ,"آتش نشانی اطفای حریق ","آتش نشانی امداد و نجات","آتش نشانی ایمنی بهداشت",
           "آتش نشانی مدیریت حریق و حوادث","آزمایشگاه",
           "آزمایشگاه انگل شناسی","آزمایشگاه بالینی","آسیب شناسی","آسیب شناسی اجتماعی","آسیب شناسی اعتیاد",
           "آسیب شناسی جامعه", "آشپزی","آلودگی دریا","آمار","آمار واحتمال","آموزش ابتدایی",
           "آموزش ابتدایی تربیت مربی","آموزش ابتدایی دبیری","آموزش ابتدایی علوم اجتماعی",
           "آموزش ابتدایی علوم تربیتی","آموزش ابتدایی معلمان ","آموزش ابتدایی پیش دبستانی",
           "آموزش بهداشت و ارتقای سلامت","آموزش دینی و عربی","اتاق عمل","اتاق عمل علوم پزشکی",
           "اتاق عمل پیراپزشکی","ادیان و عرفان","ادیان و مذاهب","اعضای مصنوعی ارتوپدی فنی",
           "اعضای مصنوعی وسایل کمکی ","اقتصاد","اقتصاد امور گمرکی","اقتصاد بازرگانی","اقتصاد برنامه ریزی",
           "اقتصاد برنامه ریزی سیستمها","اقتصاد بورس","اقتصاد بیمه","اقتصاد بین الملل","اقتصاد توسعه روستایی",
           "اقتصاد حسابداری ","اقتصاد حمل و نقل","اقتصاد صنعتی","اقتصاد مالی","اقتصاد محض","اقتصاد محض",
           "اقتصاد محیط زیست","اقتصاد نظری","اقتصاد نفت و گاز","اقتصاد پول و بانکداری","اقتصاد کشاورزی ",
           "الهیات","الهیات ادیان و عرفان","الهیات تاریخ اسلام","الهیات تاریخ فرهنگ و تمدن ملل اسلامی",
           "الهیات تفسیرقرآن","الهیات دبیری","الهیات علوم حوزوی","الهیات علوم قرآن و حدیث",
           "الهیات فقه و مبانی حقوق اسلامی","الهیات فقه و معارف اسلامی ","الهیات فقه&zwnj; شافعی & zwnj;",
           "الهیات فلسفه ","الهیات فلسفه و حکمت اسلامی ","الهیات,"," قرآن ","امداد ",
           "امداد اطفا حریق صنعتی ","امداد"," امداد و سوانح ","امداد خدمات اجتماعی ",
           "امداد مدیریت عملیات ","امداد و سوانح ","امداد, پیشگیری از حریق و حوادث","امور دامی","امور گمرکی",
           "امور گمرکی بازرگانی","امور گمرکی روابط بین الملل","امور گمرکی مدیریت","اموراداری","انرژی",
           "اپتومتریست","اپتیک ","اپتیک, اپتوالکترونیک","اپتیک لیزر","ایرانشناسی",
           "ایرانشناسی فرهنگ،آداب و رسوم و میراث فرهنگی","ایرانشناسی موزه داری","ایمنی",
           "ایمنی بهداشت و محیط زیست", "ایمنی و آتش نشانی ",
           "ایمنی و آتش نشانی اطفاء حریق","ایمنی و بهداشت","بازرسی فنی","بازرگانی","باستان","باستان شناسی",
           "باغبانی","باغبانی گیاهان دارویی","بانکداری","بانکداری اقتصاد ","بانکداری اموربانکی",
           "بانکداری علوم بانکی","بانکداری مدیریت","برق","برق ابزار دقیق","برق الکتروتکنیک","برق الکترونیک",
           "برق شبکه های انتقال و توزیع","برق صنعتی","برق قدرت ","برق مخابرات","برق کنترل فرآیند",
           "برنامه ریزی و کنترل پروژه","بهداشت","بهداشت حرفه ای","بهداشت خانواده","بهداشت عمومی",
           "بهداشت مبارزه","بهداشت محیط","بهداشت مدارس ","بهیاری","بیمه","بینائی","بینایی سنجی","بیهوشی",
           "بیو فیزیک","بیوالکتریک","بیوتکنولوژی","بیولوژی دریا","تاریخ ","تاریخ انقلاب اسلامی",
           "تاریخ ایران اسلامی","تاریخ ایران دوره اسلامی","تاریخ ایران قبل از اسلام","تاریخ ایران و اسلام",
           "تاریخ ایران و باستان","تاریخ تاریخ تشیع","تاریخ تاریخ جهان","تاریخ دبیری","تاریخ عمومی ",
           "تاریخ فرهنگ و تمدن ملل اسلامی","تاریخ محض","تاریخ هنر","تدوین","تدوین سینما","تدوین فیلم سازی",
           "تربیت بدنی","تربیت بدنی آسیب شناسی","تربیت بدنی بدنسازی","تربیت بدنی بدنی ",
           "تربیت بدنی بیو مکانیک","تربیت بدنی دبیری","تربیت بدنی رفتار حرکتی","تربیت بدنی فیزیولوژی",
           "تربیت بدنی ماساژور","تربیت بدنی مدیریت ورزش","تربیت بدنی مدیریت ورزشی","تربیت بدنی مربی گری",
           "ترکی استانبولی","تعمیرات تجهیزات پزشکی ","تغذیه","تغذیه بیوشیمی ","تغذیه"," تبدیل مواد ",
           "تغذیه تکنولوژی ","تغذی رژیم ","تغذیه غلات ","تلویزیون ","توانبخشی ","توانبخشی ارتوپدی فنی ",
           "توانبخشی مشاوره ","توسعه روستایی","تولیدات دامی","تکنولوژی آبادانی روستا",
           "تکنولوژی ارتباطات و فناوری اطلاعات","جامعه شناسی","جغرافیا","جغرافیا برنامه ریزی روستایی",
           "جغرافیا برنامه ریزی شهری","جغرافیا کارتوگرافی","جغرافیای سیاسی ","جهانگردی",
           "جهانگردی بازاریابی","جهانگردی توریسم","جهانگردی هتلداری","جهانگردی گردشگری","جوشکاری","حسابداری",
           "حسابداری امور اداری","حسابداری بازرگانی","حسابداری بانکداری ","حسابداری دولتی",
           "حسابداری سایر گرایش ها","حسابداری صنعتی","حسابداری مالی","حسابداری ﺣﻘﻮﻕ ﻭ ﺩﺳﺘﻤﺰﺩ","حسابرسی",
           "حشره شناسی پزشکی و مبارزه با ناقلین","حقوق","حقوق الهیات","حقوق الهیات و معارف اسلامی ",
           "حقوق تجارت بین الملل","حقوق ثبت اسناد و املاک","حقوق جزا و جرم شناسی","حقوق خصوصی","حقوق علوم سیاسی",
           "حقوق علوم قرآن و حدیث","حقوق عمومی","حقوق فقه و حقوق","حقوق قضایی","حقوق مالکیت فکری ",
           "حقوق محیط زیست","حقوق مدنی","حقوق گمرکی","حمل و نقل ترافیک شهری","حوزه علمیه","داروسازی","دامپروری",
           "دامپروری تغذیه","دامپروری تغذیه دام و طیور","دامپروری علوم دامی ","دامپروری پرورش طیور",
           "دامپروری ژنتیک","دامپروری کشاورزی","دامپزشکی","دامپزشکی","دامپزشکی بهداشت ایمنی و مواد غذایی",
           "دامپزشکی دام بزرگ و طیور","دامپزشکی دام کوچک و طیور","دامپزشکی درمانی","دامپزشکی علوم آزمایشگاهی ",
           "دامپزشکی عمومی","دبیری شیمی","دریا","دریا دینامیک","دریا ناوبری","دریانوردی","دستیار دندانپزشک",
           "دندانپزشکی","رادیولو&zwj;ژی","راه آهن حمل و نقل ریلی ","راه آهن خط و سازه ریلی",
           "راهنمایی و مشاوره","راهنمایی و مشاوره روانشناسی","رباتیک","روابط عمومی","روانشناسی",
           "روانشناسی بالینی","روانشناسی تربیتی","روانشناسی صنعتی","روانشناسی عمومی ","روانشناسی مشاوره",
           "روانشناسی مشاوره خانواده","روانشناسی نظری","روانشناسی کودک و نوجوان",
           "روانشناسی کودکان استثنایی","روانشناسی گفتاردرمانی","روستا","ریاضی","ریاضی آمار",
           "ریاضی آموزش ","ریاضی دبیری ","ریاضی محض ","ریاضی نظری ","ریاضی و فیزیک ","ریاضی کاربردی ",
           "زبان اردو ","زبان اسپانیایی ","زبان ایتالیایی ","زبان روسی ","زبان شناسی ",
               "زبان و ادبیات آلمانی","زبان و ادبیات آلمانی آموزش","زبان و ادبیات آلمانی مترجمی",
               "زبان و ادبیات انگلیسی","زبان و ادبیات انگلیسی آموزش","زبان و ادبیات انگلیسی اسناد و مدارک",
               "زبان و ادبیات انگلیسی تربیت معلم","زبان و ادبیات انگلیسی دبیری",
               "زبان و ادبیات انگلیسی زبانشناسی","زبان و ادبیات انگلیسی مترجمی ","زبان و ادبیات انگلیسی نظری",
               "زبان و ادبیات عربی","زبان و ادبیات عربی الهیات و معارف اسلامی",
               "زبان و ادبیات عربی ترجمه و تدریس","زبان و ادبیات عربی دبیری","زبان و ادبیات عربی فقه و اصول",
               "زبان و ادبیات عربی محض","زبان و ادبیات فارسی","زبان و ادبیات فارسی تربیت معلم",
               "زبان و ادبیات فارسی دبیری ","زبان و ادبیات فارسی محض","زبان و ادبیات فارسی نویسندگی",
               "زبان و ادبیات فارسی ویراستاری","زبان و ادبیات فرانسه","زبان و ادبیات فرانسه آموزش",
               "زبان و ادبیات فرانسه مترجمی","زبان و ادبیات ژاپنی","زمین شناسی","زمین شناسی آبشناسی",
               "زمین شناسی اقتصادی ","زمین شناسی دبیری","زمین شناسی رسوب شناسی","زمین شناسی زیست محیطی",
               "زمین شناسی عمومی","زمین شناسی لرزه شناسی","زمین شناسی محض","زمین شناسی پترولوژی",
               "زمین شناسی چینه و فسیل","زمین شناسی ژئوفیزیک","زمین شناسی کاربردی ","زیست شناسی",
               "زیست شناسی آب و فاضلاب","زیست شناسی آلودگی محیط زیست","زیست شناسی بالینی","زیست شناسی بیو شیمی",
               "زیست شناسی بیوتکنولوژی","زیست شناسی بیوفیزیک","زیست شناسی جانوری","زیست شناسی دریا",
               "زیست شناسی علوم گیاهی ","زیست شناسی محیط زیست","زیست شناسی منابع طبیعی",
               "زیست شناسی میکروبیولوژی","زیست شناسی ژنتیک","زیست گیاهی","سینما","سینما ادبیات نمایشی",
               "سینما بازیگری","سینما تدوین","سینما طراحی صحنه و لباس ","سینما فیلم سازی",
               "سینما فیلم نامه نویسی","سینما مستند سازی و روابط عمومی","سینما کارگردانی",
               "سینما کارگردانی تئاتر و هنر","شنوایی","شنوایی سنجی","شنوایی شناسی","شهرسازی",
               "شهرسازی برنامه ریزی شهری ","شهرسازی برنامه ریزی شهری و منطقه ای","شهرسازی جغرافیا",
               "شهرسازی صنعت","شهرسازی طراحی","شهرسازی مدیریت شهری","شهرسازی نظری","شهرسازی هنر و معماری",
               "شیلات","شیمی","شیمی آزمایشگاه ","شیمی آلی","شیمی تجزیه","شیمی دبیری","شیمی سایر گرایش ها",
               "شیمی محض","شیمی مهندسی پلیمر","شیمی کاربردی","صنایع","صنایع ایمنی",
               "صنایع ایمنی صنعتی و محیط کار ","صنایع ایمنی و بهداشت ","صنایع برنامه ریزی و تحلیل سیستم ها ",
               "صنایع تولیدات صنعتی ","صنایع تکنولوژی ","صنایع دستی ","صنایع دستی سفال گری ",
               "صنایع دستی فرش دستباف ","صنایع دستی قلم زنی ","صنایع دستی نقش برجسته ","صنایع دستی نگارگری ",
               "صنایع دستی پژوهش در هنرهای سنتی","صنایع دستی چوب،سفال، دکوراسیون داخلی","صنایع دستی کتابت",
               "صنایع سایر گرایش ها","صنایع سیستم های اقتصادی و اجتماعی","صنایع شیمیایی","صنایع غذایی",
               "صنایع غذایی تکنولوژی","صنایع غذایی شیمی","صنایع غذایی لبنیات ","صنایع غذایی میکروبیولوژی",
               "صنایع غذایی کشاورزی","صنایع غذایی کنترل کیفیت","صنایع فلزی","صنایع مدیریت سیستم و بهره وری",
               "صنایع مدیریت کیفیت و بهره وری","صنایع کنترل و کیفیت","طبیعی","طبیعی آب و فاضلاب","طبیعی آبخیز ",
               "طبیعی آلودگی محیط زیست","طبیعی اکوتوریسم","طبیعی تکثیر و پرورش","طبیعی جنگلداری","طبیعی شیلات",
               "طبیعی محیط زیست","طبیعی مرتع و آبخیزداری","طبیعی منابع طبیعی","طبیعی چوب و کاغذ",
               "طبیعی کشاورزی ","طراحی","طراحی صنعتی","طراحی صنعتی ساخت و تولید","طراحی صنعتی نقشه کشی",
               "طراحی پارچه و لباس","طراحی پارچه و لباس بافندگی","طراحی پارچه و لباس دوخت",
               "طراحی پارچه و لباس مد","طراحی پارچه و لباس هنر","طراحی پارچه و لباس چاپ ","عرفان اسلامی",
               "علم اطلاعات و دانش شناسی","علوم آزمایشگاهی","علوم آزمایشگاهی میکروب شناسی","علوم اجتماعی",
               "علوم اجتماعی ارتباطات","علوم اجتماعی برنامه ریزی اجتماعی تعاون و رفاه",
               "علوم اجتماعی برنامه ریزی توسعه منطقه ای","علوم اجتماعی برنامه ریزی شهری",
               "علوم اجتماعی جمعیت شناسی ","علوم اجتماعی خدمات اجتماعی","علوم اجتماعی دبیری",
               "علوم اجتماعی روابط عمومی","علوم اجتماعی روزنامه نگاری","علوم اجتماعی زن و خانواده",
               "علوم اجتماعی سیاسی","علوم اجتماعی مددکاری","علوم اجتماعی مردم شناسی",
               "علوم اجتماعی مطالعات خانواده","علوم اجتماعی مطالعات زنان ","علوم اجتماعی مطالعات فرهنگی",
               "علوم اجتماعی پژوهش","علوم ارتباطات","علوم ارتباطات امور فرهنگی","علوم ارتباطات خبرنگاری",
               "علوم ارتباطات رسانه و مطالعات فرهنگی","علوم ارتباطات روابط بین الملل",
               "علوم ارتباطات روابط عمومی","علوم ارتباطات مدیریت","علوم ارتباطات مدیریت رسانه ",
               "علوم ارتباطات پژوهشگری","علوم انسانی","علوم تجربی","علوم تربیتی","علوم تربیتی آموزش ابتدایی",
               "علوم تربیتی آموزش و پرورش","علوم تربیتی اصلاح و تربیت","علوم تربیتی اموزش کودکان استثنایی",
               "علوم تربیتی برنامه ریزی آموزشی","علوم تربیتی برنامه ریزی درسی ",
               "علوم تربیتی تحقیقات آموزشی","علوم تربیتی تعلیم و تربیت","علوم تربیتی تکنولوژی آموزشی",
               "علوم تربیتی راهنمایی و مشاوره","علوم تربیتی روانشناسی تربیتی","علوم تربیتی فلسفه تعلیم و تربیت",
               "علوم تربیتی مدیریت","علوم تربیتی مطالعات خانواده","علوم تربیتی پیش دبستانی",
               "علوم تربیتی کودکان استثنایی","علوم تغذیه و رژیم درمانی ","علوم حوزه ","علوم سیاسی ",
               "علوم سیاسی اندیشه ","علوم سیاسی جامعه شناسی ","علوم سیاسی حقوق ","علوم سیاسی خاورمیانه ",
               "علوم سیاسی روابط بین الملل ","علوم سیاسی محض ","علوم سیاسی مسایل ایران ",
               "علوم سیاسی مطالعات منطقه خلیج فارس","علوم قرآنی","علوم قرآنی تفسیر قرآن","علوم قرآنی حوزوی",
               "علوم قضایی","علوم کامپیوتر","علوم کامپیوتر امنیت اطلاعات","علوم کامپیوتر سایر گرایش ها",
               "علوم کامپیوتر سیستم های هوشمند","علوم کامپیوتر سیستمهای اطلاعاتی ",
               "علوم کامپیوتر سیستمهای کامپیوتری","علوم کامپیوتر فناوری اطلاعات","علوم کامپیوتر محاسبات عددی",
               "علوم کامپیوتر مهندسی دانش","عمران","عمران GIS","عمران آب","عمران حمل و نقل","عمران خاک",
               "عمران راه ","عمران راه سازی","عمران راه و ساختمان","عمران زلزله شناسی","عمران زیرسازی",
               "عمران سازه","عمران سایر گرایش ها","عمران شهرسازی","عمران عمران","عمران محیط زیست",
               "عمران مدیریت پروژه ","عمران مهندسی و مدیریت ساخت","عمران نقشه برداری","عمران نقشه کشی",
               "عمران نقشه کشی ساختمان","عمران نقشه کشی صنعتی","عمران ژئو تکنیک","عکاسی","عکاسی تبلیغات",
               "عکاسی صنعتی","عکاسی فیلمسازی ","عکاسی هنر","فرش","فرش بافت و مرمت","فرش دستبافت",
               "فرش رنگرزی","فرش طراحی","فرهنگ و زبانهای باستانی","فضای سبز","فضای سبز باغبانی",
               "فضای سبز محیط زیست ","فضای سبز کشاورزی","فلسفه","فلسفه آموزشی","فلسفه اخلاق","فلسفه ادیان",
               "فلسفه اسلامی","فلسفه غرب","فلسفه محض","فلسفه معارف","فلسفه هنر ","فناوری اطلاعات",
               "فناوری اطلاعات امنیت اطلاعات","فناوری اطلاعات تجارت الکترونیک","فناوری اطلاعات سایر گرایش ها",
               "فناوری اطلاعات سلامت","فناوری اطلاعات سلامت","فناوری اطلاعات سیستم های اطلاعاتی",
               "فناوری اطلاعات شبکه های کامپیوتری","فناوری اطلاعات طراحی صفحات وب",
               "فناوری اطلاعات طراحی و تولید نرم افزار ","فناوری اطلاعات مخابرات","فناوری اطلاعات مدیریت دانش",
               "فناوری اطلاعات مهندسی تکنولوژی","فناوری اطلاعات مهندسی نرم افزار","فناوری اطلاعات و ارتباطات",
               "فناوری اطلاعات و ارتباطات انتقال","فناوری اطلاعات و ارتباطات انتقال داده",
               "فناوری اطلاعات و ارتباطات برق","فناوری اطلاعات و ارتباطات سخت افزار",
               "فناوری اطلاعات و ارتباطات شبکه ","فناوری اطلاعات و ارتباطات مخابرات",
               "فناوری اطلاعات و ارتباطات مدیریت","فناوری اطلاعات و ارتباطات کاربردها",
               "فناوری اطلاعات گرافیک","فنی و حرفه ای","فوتونیک","فوریت های پزشکی","فوریت های پزشکی مدیریت بحران",
               "فوریت های پزشکی پیرا پزشکی","فیزیوتراپی ","فیزیک ","فیزیک بنیادی ","فیزیک دبیری ",
               "فیزیک دریا ","فیزیک محض ","فیزیک نظری ","فیزیک هسته ای ","فیزیک کاربردی ","فیلمسازی ",
               "مامایی ","متالوژی","متالوژی استخراجی","متالوژی ذوب فلزات","متالوژی ریخته گری","متالوژی صنعتی",
               "مجسمه سازی","مخابرات","مدارک پزشکی","مددکاری","مددکاری اجتماعی ","مددکاری اجتماعی روانشناسی",
               "مددکاری خدمات اجتماعی","مددکاری سازمان زندان ها","مددکاری علوم پزشکی","مدیریت","مدیریت آموزشی",
               "مدیریت اجرایی","مدیریت امور اداری","مدیریت بازاریابی","مدیریت بازرگانی ","مدیریت بانکداری",
               "مدیریت برنامه ریزی خانواده","مدیریت بیمه","مدیریت جهانگردی","مدیریت خدمات بهداشتی درمانی",
               "مدیریت دفاعی","مدیریت دولتی","مدیریت سایر گرایشها","مدیریت صنعتی","مدیریت صنعتی تولید ",
               "مدیریت صنعتی مالی","مدیریت فرهنگی و هنری","مدیریت لجستیک بنادر","مدیریت مالی",
               "مدیریت و بازرگانی دریایی","مدیریت و کمیسر دریایی","مدیریت ورزشی","مدیریت پروژه",
               "مدیریت کارآفرینی","مربی پیش دبستانی ","مربی کودک","مرمت آثار تاریخی",
               "مرمت و احیای ابنیه تاریخی","مشاوره","مشاوره حقوقی","مصنوعی","مطالعات خانواده",
               "مطالعات زنان","معارف اسلامی","معارف اسلامی ادیان و عرفان ",
               "معارف اسلامی تاریخ فرهنگ و تمدن ملل اسلامی","معارف اسلامی حوزوی","معارف اسلامی دبیری",
               "معارف اسلامی علوم حدیث","معارف اسلامی علوم قرآن و حدیث","معدن","معدن استخراج","معدن اکتشاف",
               "معدن سنگ","معدن فرآوری ","معماری","معماری دریایی","معماری راه و ساختمان","معماری راه و شهرسازی",
               "معماری سایر گرایشها","معماری طراحی","معماری طراحی داخلی","معماری طراحی شهری","معماری مرمت",
               "معماری مرمت ابنیه ","معماری مرمت بنا","معماری معماری","معماری نقشه برداری","معماری نقشه کشی",
               "معماری هنر","معماری کشتی","مهندسی تکنولوژی جوشکاری","مهندسی تکنولوژی نرم افزار","مهندسی دریا",
               "مهندسی رباتیک ","مهندسی شیمی","مهندسی صنایع","مهندسی فضای سبز","مهندسی پروژه","مهندسی پزشکی",
               "مهندسی پزشکی بالینی","مهندسی پزشکی بیوالکتریک","مهندسی پزشکی بیومتریال","مهندسی پزشکی بیومواد",
               "مهندسی پزشکی بیومکانیک ","مهندسی پزشکی تجهیزات پزشکی ","مهندسی پزشکی ورزش ",
               "مهندسی پزشکی پیراپزشکی ","مواد ","مواد استخراج ","مواد جوشکاری ","مواد ذوب فلزات ",
               "مواد ریخته گری ","مواد سرامیک ","مواد صنعتی ","مواد متالوژی","مواد نانو","موزه داری",
               "موسیقی","موسیقی آهنگسازی","موسیقی آواز ایرانی","موسیقی موسیقی جهانی","مکاترونیک",
               "مکاترونیک طراحی روباتیک","مکاترونیک هوش مصنوعی ","مکاترونیک کامپیوتر",
               "مکاترونیک کنترل تولید و کیفیت","مکانیک","مکانیک تبدیل انرژی","مکانیک ابزار دقیق",
               "مکانیک بیومکانیک","مکانیک تاسیسات","مکانیک تراشکاری","مکانیک جامدات","مکانیک جوشکاری ",
               "مکانیک خودرو","مکانیک ساخت و تولید","مکانیک سایر گرایش ها","مکانیک سیالات","مکانیک طراحی صنعتی",
               "مکانیک طراحی کاربردی","مکانیک ماشین ابزار","مکانیک نقشه کشی صنعتی","ناظر چاپ","نانو شیمی ",
               "نساجی","نساجی شیمی","نساجی شیمی رنگ و علوم الیاف","نساجی طراحی دوخت","نساجی علمی کاربردی",
               "نساجی پوشاک","نظامی","نفت","نفت اکتشاف","نفت بهره برداری ","نفت حفاری","نفت مخازن","نفت پالایش",
               "نقاشی","نقاشی ایرانی","نقاشی عمومی","نقاشی موزه داری","نقاشی نگارگری","نقشه کشی صنعتی",
               "نیروی انتظامی ","هتلداری","هتلداری خدمات اجتماعی","هتلداری خدمات اقامتی","هتلداری مدیریت",
               "هسته ای","هسته ای راکتور","هسته ای پرتو پزشکی","هسته ای چرخه سوخت","هسته ای کاربرد پرتوها",
               "هنر ","هنر ارتباط تصویری","هنر انیمیشن","هنر تصویری","هنر صنایع دستی","هنر گرافیک",
               "هنرهای تجسمی","هوافضا","هوافضا آیرودینامیک","هوافضا دینامیک پرواز","هوافضا ساخت و تولید ",
               "هوانوردی","هوانوردی خلبانی","هواپیما","هواپیما تعمیرات و نگهداری","هواپیما صنعتی",
               "هواپیما هوانوردی","هوشبری","پتروشیمی","پرتودرمانی","پرستاری ",
               "پرستاری مراقبتهای ویژه","پرستاری پیراپزشکی","پرستاری کمک پرستار","پرستاری کودکان",
               "پروتز دندان","پرورش طیور","پزشکی","پزشکی بیولوژی",
               "پزشکی دکترای حرفه ای","پزشکی متخصص اطفال ","پزشکی پرتوپزشکی ","پزشکی پزشک عمومی ",
               "پلیمر ","پلیمر رنگ ","پلیمر شیمی ","پلیمر صنایع ","پژوهش هنر ","چاپ ","چاپ و نشر ",
               "چاپ گرافیک ","چینی","ژئو فیزیک","ژئو فیزیک زلزله شناسی","ژئومورفولوژی",
               "ژئومورفولوژی جغرافیای طبیعی","ژنتیک","ژنتیک سایر گرایشها","ژنتیک سلولی مولکولی","ژنتیک پزشکی",
               "کار و دانش ","کارآفرینی","کاردرمانی","کاردرمانی توانبخشی","کارشناس بهداشت عمومی",
               "کارشناسی حرفه ای مشاوره ژنتیک","کارگردانی","کامپیوتر","کامپیوتر بانک اطلاعات",
               "کامپیوتر تکنولوژی نرم افزار","کامپیوتر سایر گرایش ها ","کامپیوتر سخت افزار","کامپیوتر شبکه",
               "کامپیوتر معماری","کامپیوتر نرم افزار","کتابداری","کتابداری اطلاع رسانی","کتابداری علوم اجتماعی",
               "کتابداری مدیریت","کتابداری مدیریت اطلاعات","کتابداری پیراپزشکی ","کشاورزی","کشاورزی آب",
               "کشاورزی آب و خاک","کشاورزی آبیاری","کشاورزی اصلاح نباتات","کشاورزی اقتصاد","کشاورزی باغبانی",
               "کشاورزی ترویج و آموزش","کشاورزی تولیدات گیاهی","کشاورزی حشره شناسی ","کشاورزی خاک",
               "کشاورزی خاکشناسی","کشاورزی دامپروری","کشاورزی روستا","کشاورزی زراعت","کشاورزی صنایع غذایی",
               "کشاورزی علوم","کشاورزی علوم دامی","کشاورزی فضای سبز","کشاورزی ماشین آلات کشاورزی ",
               "کشاورزی محیط زیست","کشاورزی مدیریت و آبادانی روستاها","کشاورزی مرتع داری",
               "کشاورزی مرتع و آبخیزداری","کشاورزی مهندسی منابع آب","کشاورزی مکانیزاسیون","کشاورزی میوه کاری",
               "کشاورزی پرورش آبزیان دریایی","کشاورزی پرورش طیور","کشاورزی ژنتیک و اصلاح دام ",
               "کشاورزی گیاه پزشکی","کشاورزی گیاهان دارویی","کشاورزی گیاهان زینتی","کشتی سازی",
               "کشتی سازی هیدرودینامیک","کودکیاری","گرافیک","گرافیک کامپیوتری","گرافیک طراحی لباس",
               "گرافیک عکاسی ","گرافیک نقاشی","گرافیک چاپ","گردشگری","گفتار درمانی"
            ,"گفتار درمانی توانبخشی","گفتار درمانی مشاوره","گیاه پزشکی","گیاهان دارویی" };
                        int id = 100;
                        foreach (var item in arr1)
                        {
                            id++;
                            if (item.Trim() != "")
                            {
                                models.Add(new Major()
                                {
                                    Id = id,
                                    IsActive = true,
                                    IsDeleted = false,
                                    Title = item.Trim()
                                });
                            }

                        }

                        context.Major.AddRange(models);
                        context.SaveChanges();
                    }
                    if (!context.JobTitle.Any())
                    {

                        var models = new List<JobTitle>
                        {
                            new JobTitle() { Id = 100, Title = "فنی", IsActive = true, IndexOrder = 0, },
                            new JobTitle() { Id = 101, ParentId = 100, Title = "جوشکاری", IsActive = true, IndexOrder = 0, },
                            new JobTitle() { Id = 102, ParentId = 100, Title = "تاسیسات", IsActive = true, IndexOrder = 0, },
                            new JobTitle() { Id = 103, ParentId = 100, Title = "برقکار", IsActive = true, IndexOrder = 0, }
                        };
                        context.JobTitle.AddRange(models);
                        context.SaveChanges();


                    }
                    

                    if (!context.OrganizationalPosition.Any())
                    {

                        var models = new List<OrganizationalPosition>
                        {
                            new OrganizationalPosition() {     Name = "مدیرعامل", IsActive = true, IndexOrder = 0, },
                            new OrganizationalPosition() {     Name = "عضو هیات مدیره", IsActive = true, IndexOrder = 2, },
                            new OrganizationalPosition() {     Name = "رئیس هیات مدیره", IsActive = true, IndexOrder = 3, },
                            new OrganizationalPosition() {     Name = "مسئول دفتر", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "منشی", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "معاونت برنامه ریزی و انفورماتیک", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "معاونت مالی و اداری", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "معاونت اجرایی", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "معاونت مهندسی", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "مدیر امور جذب و استخدام", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "مدیر امور آموزش", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "مدیر امور رفاهی", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "مدیر امور کارکنان ", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "انباردار", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "نیروی خدمات", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "تاسیسات", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "حراست", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "کارمند", IsActive = true, IndexOrder = 1, },
                            new OrganizationalPosition() {     Name = "ترابری", IsActive = true, IndexOrder = 1, },

                        };
                        context.OrganizationalPosition.AddRange(models);
                        context.SaveChanges();


                    }
                    if (!context.UserCompanyFieldOfActivity.Any())
                    {
                        var models = new List<UserCompanyFieldOfActivity>();
                        string[] arr1 = new string[] {"کامپیوتر و نرم افزار","اینترنت ،فناوری و شبکه",
                            "رسانه، فرهنگی و نشریات",
               "تبلیغات و بازاریابی",  "آموزش","مهندسی و ساخت و ساز","تولید و صنایع",
               "دولتی","مالی و اعتباری","پزشکی، سلامت و بهداشت","حقوقی و قانون"
               ,"صنایع مواد غذایی و آشامیدنی","نساجی","چوب و کاغذ و مواد سلولزی",
                            "ماشین سازی و لوازم خانگی و فلزی","صنایع کوچک و کارگاهی",
                            "صنایع شیمیایی و پلاستیک","برق و الکترونیک","صنایع کانی و غیرفلزی"

               ,"رستوران و غذا ها","خدمات حمل و نقل","حسابرسی مشاوره مالیاتی و مدیریت مالی",
               "گردشگری و مسافرتی","بورس و اوراق بهادار","بیمه"};




                        int id = 100;
                        foreach (var item in arr1)
                        {
                            id++;
                            if (item.Trim() != "")
                            {
                                models.Add(new UserCompanyFieldOfActivity()
                                {
                                    Id = id,
                                    IsActive = true,
                                    Title = item.Trim(),

                                });
                            }

                        }

                        context.UserCompanyFieldOfActivity.AddRange(models);
                        context.SaveChanges();
                    }
                   
                    if (!context.MenuItem.Any())
                    {

                        var models = new List<MenuItem>
                        {
                            new MenuItem() {  MenuName="خانه",MenuPath="/", CreatedOnDate=DateTime.Now,IsSystem=true,IsVisible=true,   },
                            new MenuItem() {  MenuName="اخبار",MenuPath="/home/news-list", CreatedOnDate=DateTime.Now,IsSystem=true,IsVisible=true,   },
                            new MenuItem() {  MenuName="تماس با ما",MenuPath="/home/contact-us", CreatedOnDate=DateTime.Now,IsSystem=true,IsVisible=true,   },
                            new MenuItem() {  MenuName="پشتیبانی",MenuPath="/home/faq", CreatedOnDate=DateTime.Now,IsSystem=true,IsVisible=true,   },


                        };
                        context.MenuItem.AddRange(models);
                        context.SaveChanges();


                    }
                    if (!context.Feedback.Any())
                    {

                        var models = new List<Feedback>
                        {
                           new Feedback() { Id = 100,  FeedbackTitle="پیگیری کارت شهروندی"   },
                           new Feedback() { Id = 101,  FeedbackTitle="گزارش خطا در سامانه"   },
                           new Feedback() { Id = 102,  FeedbackTitle="درخواست عضویت در طرح منزلت"   },
                           new Feedback() { Id = 103,  FeedbackTitle="سوال یا درخواست مرتبط با سامانه"   },
                           new Feedback() { Id = 104,  FeedbackTitle=" پیگیری کارت منزلت"   },
                           new Feedback() { Id = 105,  FeedbackTitle="درخواست تغییر شماره موبایل"   },
                           new Feedback() { Id = 106,  FeedbackTitle="درخواست تغییر کلمه عبور"   },
                           new Feedback() { Id = 107,  FeedbackTitle="ثبت شماره كارت بانكي جهت عودت هزينه درخواست كارت"   },
                           new Feedback() { Id = 108,  FeedbackTitle="ثبت شماره كارت بانكي جهت عودت هزينه ارسال پستي كارت"   },
                           new Feedback() { Id = 109,  FeedbackTitle=" سایر"   }, 

                        };
                        context.Feedback.AddRange(models);
                        context.SaveChanges();


                    }
                    if (!context.Nationality.Any())
                    { 

                        var models = new List<Nationality>
                        {
                           new Nationality() { Id = 0,   Name  ="ایران" ,IsActive=true  },
                           new Nationality() { Id = 1,   Name  ="افغانستان" ,IsActive=true  },
                           new Nationality() { Id = 100, Name  ="عراق" ,IsActive=true  },
                           new Nationality() { Id = 101, Name  ="پاکستان" ,IsActive=true  },

                        };
                        context.Nationality.AddRange(models);
                        context.SaveChanges(); 
                    }
                    if (!context.UserDocumentGroup.Any())
                    {

                        var models = new List<UserDocumentGroup>
                        {
                           new UserDocumentGroup() { Id = 100,IndexOrder=0,   Title  ="اسکن کارت ملی" ,Description="لطفا اسکن کارت ملی جدید خود را بارگذاری نمایید",  IsActive=true  },
                           new UserDocumentGroup() { Id = 101,IndexOrder=0,   Title  ="اسکن آخرین مدرک تحصیلی" ,Description="لطفا اسکن آخرین مدرک تحصیلی خود را بارگذاری نمایید",  IsActive=true  },


                        };
                        context.UserDocumentGroup.AddRange(models);
                        context.SaveChanges();
                    }

                    if (!context.JobGroup.Any())
                    { 
                        var models = new List<JobGroup>
                        {
                           new JobGroup() { Id = 50,    Title="كارمند بخش دولتي"   },
                           new JobGroup() { Id = 51,    Title="كارمند بخش خصوصی"   },
                           new JobGroup() { Id = 52,    Title="نظامی"   },
                           new JobGroup() { Id = 53,    Title="آزاد"   },
                           new JobGroup() { Id = 54,    Title="بازنشسته"   },
                           new JobGroup() { Id = 55,    Title="محصل و دانشجو"   },
                           new JobGroup() { Id = 56,    Title="سرباز"   },
                           new JobGroup() { Id = 57,   Title="كارگر"   },
                           new JobGroup() { Id = 58,   Title="بيكار"   },
                           new JobGroup() { Id = 59,   Title="خانه دار"   },
                           new JobGroup() { Id = 60,   Title="ساير"   },

                        };
                        context.JobGroup.AddRange(models);
                        context.SaveChanges();
                        
                    }
                    if (!context.EducationGroup.Any())
                    {
                        var models = new List<EducationGroup>
                        {
                           new EducationGroup() { Id = 17,    Title="علوم هنر و زبان"   },
                           new EducationGroup() { Id = 18,    Title="علوم پزشکی"   },
                           new EducationGroup() { Id = 19,    Title="علوم انسانی و مدیریت"   },
                           new EducationGroup() { Id = 20,    Title="علوم فنی و مهندسی"   },
                           new EducationGroup() { Id = 21,    Title="علوم پایه"   }, 
                           new EducationGroup() { Id = 0,   Title="ساير"   },

                        };
                        context.EducationGroup.AddRange(models);
                        context.SaveChanges();
                      
         
         
                    }
                    if (!context.AppServices.Any())
                    {

                        var models = new List<AppServices>
                        {
                           new AppServices() { Id = 19,   ServiceName="طرح منزلت" ,Link="/User/Manzelat",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 20,   ServiceName="طرح دختران رو به دریا" ,Link="http://www.esfshahrvand.ir/user/profile",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 22,   ServiceName="طرح كارآفريني- توان 7" ,Link="http://tavan7.esfshahrvand.ir/home/userlogin",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 23,   ServiceName="رزرواسیون" ,Link="http://www.shahrvandticket.ir/returnforlogin.aspx",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 25,   ServiceName="آزمایش وب سرویس" ,Link="http://esfshahrvand.ir:8081/Default.cshtml",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 27,   ServiceName="خدمات شهروندی" ,Link="http://www.esfshahrvand.ir/user/profile",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 28,   ServiceName="کلاسهای آموزشی" ,Link="http://shahrvandedu.ir/webservice-auth/login",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 29,   ServiceName="درخواست عضویت در کتابخانه ها و سالن های مطالعه" ,Link="http://site17.douran.net/fa/registration/",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 30,   ServiceName="خرید کارت شهروندی" ,Link="/Order",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 33,   ServiceName="عضویت در کتابخانه" ,Link="/user/profile",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 35,   ServiceName="بليط همايش ازدواج" ,Link="http://www.edu.esfahanfarhang.ir/fa/lms.students/orientation_classes/91/",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 36,   ServiceName="همایش ها و رویدادها" ,Link="http://188.136.156.27",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 42,   ServiceName="EServiceDesk" ,Link="http://my.isfahan.ir/",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 43,   ServiceName="مشاوره کنکور ارشد" ,Link="/order/OrderPackage/info",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 45,   ServiceName="Club" ,Link="http://club.isfahan.ir/",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 46,   ServiceName="مسابقه شهر من" ,Link="http://188.136.156.18/ePoll04/r/uyooqps",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },
                           new AppServices() { Id = 47,   ServiceName="ShahrvandiAPP" ,Link="http://app.isfahan.ir/",IsMain=true,Priority=0,Terms="متن قوانین و مقررات" , CreationDate=DateTime.Now,HaveTerms=true,CssClass="",IsActive=true,  },


                        };
                        context.AppServices.AddRange(models);
                        context.SaveChanges();


                    }

                }
            }
        }


    }
}