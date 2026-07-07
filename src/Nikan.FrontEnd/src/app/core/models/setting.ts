export class SiteSettingViewModel {
  //[DisplayName("آدرس سایت")]
  public siteUrl!: string;
  // [DisplayName("نام سایت")]
  public siteName!: string;
  public fullSiteName!: string;
  // [DisplayName("کلمات کلیدی")]
  public siteKeywords!: string;
  // [DisplayName("توضیحات")]
  public siteDescription!: string;
  // [Display(Name = "سرویس دهنده ایمیل")]
  public mailServerUrl!: string;
  //  [Display(Name = "درگاه")]
  public mailServerPort!: string;
  //  [Display(Name = "شناسه ورود")]
  public mailServerUserName!: string;
  // [Display(Name = "کلمه عبور")]
  public mailServerPassword!: string;
  // [DisplayName("اطلاعات تماس")]
  public cellNumber!: string;
  public mobileNumber!: string;
  public addresss!: string;
  public smsNumber!: string;
  public telegramChannelId!: string;
  public faxNumber!: string;
  public emailAddress!: string;
  public footerText!: string;
  public businessHours!: string;
  public logoUrl!: string;
}
