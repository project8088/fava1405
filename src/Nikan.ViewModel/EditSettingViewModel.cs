using Nikan.Common.Resource;
using System.ComponentModel.DataAnnotations;

namespace Nikan.ViewModel.BsseEntity
{

    public class EditSettingViewModel
    {


        //[DisplayName("آدرس سایت")]
        [Display(ResourceType = typeof(ModelResource), Name = "SiteUrl")]
        public string SiteUrl { get; set; }

        // [DisplayName("نام سایت")]
        [Display(ResourceType = typeof(ModelResource), Name = "SiteName")]
        public string SiteName { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = "FullSiteName")]
        public string FullSiteName { get; set; }





        // [DisplayName("کلمات کلیدی")]
        [Display(ResourceType = typeof(ModelResource), Name = "SiteKeywords")]
        [DataType(DataType.MultilineText)]
        public string SiteKeywords { get; set; }

        // [DisplayName("توضیحات")]
        [Display(ResourceType = typeof(ModelResource), Name = "SiteDescription")]
        [DataType(DataType.MultilineText)]
        public string SiteDescription { get; set; }

        // [Display(Name = "سرویس دهنده ایمیل")]
        [Display(ResourceType = typeof(ModelResource), Name = "MailServerUrl")]
        public string MailServerUrl { get; set; }

        //  [Display(Name = "درگاه")]
        [Display(ResourceType = typeof(ModelResource), Name = "MailServerPort")]
        public string MailServerPort { get; set; }

        //  [Display(Name = "شناسه ورود")]
        [Display(ResourceType = typeof(ModelResource), Name = "MailServerUserName")]
        public string MailServerUserName { get; set; }

        // [Display(Name = "کلمه عبور")]
        [Display(ResourceType = typeof(ModelResource), Name = "MailServerPassword")]
        public string MailServerPassword { get; set; }

        // [DisplayName("اطلاعات تماس")]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(ModelResource), Name = "CellNumber")]
        public string CellNumber { get; set; }


        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(ModelResource), Name = "MobileNumber")]
        public string MobileNumber { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(ModelResource), Name = "Addresss")]
        public string Addresss { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = "SMSNumber")]
        public string SMSNumber { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = "TelegramChannelId")]
        public string TelegramChannelId { get; set; }





        [Display(ResourceType = typeof(ModelResource), Name = "FaxNumber")]
        public string FaxNumber { get; set; }

        public string LogoUrl { get; set; }






        [Display(ResourceType = typeof(ModelResource), Name = "EmailAddress")]
        public string EmailAddress { get; set; }

        [Display(ResourceType = typeof(ModelResource), Name = "FooterText")]
        public string FooterText { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = "BusinessHours")]
        public string BusinessHours { get; set; }


        /// <summary>
        /// شناسه استان اصفهان
        /// </summary>
        public int? IsfahanProvinceId { get; set; }

        /// <summary>
        /// شناسه شهر اصفهان
        /// </summary>
        public int? IsfahanCityId { get; set; }


        public int? RegionCount { get; set; }


        public int? ManzalatGroupId { get; set; }


        public bool OnlineAuthenticationAfterUpdateCitizenInfo { get; set; }

        public bool OnlineAuthentication  { get; set; }













    }

    public class ManzalatSettings
    {
        public int MinAgeSalmand { get; set; }
        public int MinAgeBazneshasteh { get; set; }
        
        
        public string JanbazanDescription { get; set; }

        public string BazneshastehDescription { get; set; }

        public string ZanSarparastDescription { get; set; }

        public string SalmandDescription { get; set; }

        public string MaloulinDescription { get; set; }



    }


    public class SiteInfo
    {
        public string Addresss { get; set; }
        public string FooterText { get; set; }
        public string BusinessHours { get; set; }
        public string FaxNumber { get; set; }

        public string CellNumber { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }

        public string SiteName { get; set; }

        public string SMSNumber { get; set; }

        public string TelegramChannelId { get; set; }

        public string FullSiteName { get; set; }

        public string SiteUrl { get; set; }
        public string LogoUrl { get; set; }



    }

    public class SiteMetaTags
    {
        public string Keywords { get; set; }
        public string Description { get; set; }
    }
    public class SmtpSettings
    {
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
    }


    /// <summary>
    /// تنظیمات مالی
    /// </summary>
    public class FinancialSettings
    {


        /// <summary>
        /// شناسه مشتری
        /// شناسه بانکی مشتری
        /// </summary>
        public long? BankCustomerId { get; set; }

        /// <summary>
        /// نام کاربری
        /// </summary>
        public string BankUserName { get; set; }

        /// <summary>
        /// کلمه عبور
        /// </summary>
        public string BankPassword { get; set; }




        public string CallBackUrl { get; set; }


       public long? RefundTerminalId { get; set; }

        /// <summary>
        /// نام کاربری سرویس برگشت
        /// </summary>
        public string RefundUserName { get; set; }


        /// <summary>
        /// کلمه عبور سرویس برگشت
        /// </summary>
        public string RefundPassword { get; set; }


        /// <summary>
        /// شماره ترمینال بانکی
        /// </summary>
        public long? BankTerminalId { get; set; }
         

        /// <summary>
        /// 0 بدون شناسه پرداخت
        /// 1 باشناسه ثابت پرداخت
        /// 2 باشناسه پرداخت پرداخت شناور
        /// روش پرداخت بانکی
        /// </summary>
        public int? BankPaymentMethod { get; set; }

        public bool  RefundIsActive { get; set; }


        public string  RefundDeActiveDescription { get; set; }

        public int? RefundAmountDeficit { get; set; }

    }





    public class IsfahanInfo
    {


        /// <summary>
        /// شناسه استان اصفهان
        /// </summary>
        public int? IsfahanProvinceId { get; set; }

        /// <summary>
        /// شناسه شهر اصفهان
        /// </summary>
        public int? IsfahanCityId { get; set; }
    }






}