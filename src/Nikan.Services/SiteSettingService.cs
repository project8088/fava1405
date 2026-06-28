using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.ViewModel.BsseEntity;


using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nikan.Common;
using Nikan.ViewModel;
using Nikan.Common.GlobalEnum;
using Newtonsoft.Json;

namespace cle.Services
{
    
    public interface ISiteSettingService
    {
        Task<ApiResult<EditSettingViewModel>> UpdateSettings(EditSettingViewModel settingsModel, int userId);
        Task<ApiResult<EditSettingViewModel>> GetSettingsForEdit();
        Task<ApiResult<SiteInfo>> GetSiteInfo();
        Task<ApiResult<string>> UpdateLogo(string url);
        Task<ApiResult<string>> GetSiteUrl();
        Task<ApiResult<FinancialSettings>> GetFinancialSettings();
        Task<ApiResult<FinancialSettings>> UpdateFinancialSettings(FinancialSettings settingsModel);
        Task<ApiResult<SmsOption>> GetSmsSettings();
        Task<ApiResult<SmsOption>> UpdateSmsSettings(SmsOption settingsModel);
        Task<SmsOption> GetSmsSettingForSend();
        Task<ApiResult<IsfahanInfo>> GetIsfahanInfo();
        Task<ApiResult<ManzalatSettings>> GetManzalatSettings();
        Task<ApiResult<ManzalatSettings>> UpdateManzalatSettings(ManzalatSettings settingsModel);
        Task<SmsOption> GetSmsSettingForOnlineAuthentication();
    }




    public class SiteSettingService : ISiteSettingService
    {

        #region Field
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SiteOption> _SiteOptions;
        private readonly DbSet<Event> _event;
        #endregion



        public SiteSettingService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _SiteOptions = _uow.Set<SiteOption>();
            _event = _uow.Set<Event>();
        }


        public async Task<ApiResult<EditSettingViewModel>> UpdateSettings(EditSettingViewModel settingsModel,int userId)
        {
            var category = Nikan.Common.GlobalEnum.SiteOptionCategoryEnum.تنظیمات_پایه;
            var res = new ApiResult<EditSettingViewModel>(true, ApiResultStatusCode.Success, new EditSettingViewModel());
            var currentSettings = await _SiteOptions.Where(w => w.Category == category).ToListAsync();
            try
            {

                if(settingsModel.IsfahanCityId==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شناسه شهر اصفهان را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                if (settingsModel.IsfahanProvinceId == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "شناسه استان اصفهان را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                #region Addresss
                if (currentSettings.FirstOrDefault(s => s.Key == "Addresss") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "Addresss").Value = settingsModel.Addresss?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "Addresss", Value = settingsModel.Addresss?.ToString(), Category = category });
                }
                #endregion 
                #region BusinessHours
                if (currentSettings.FirstOrDefault(s => s.Key == "BusinessHours") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "BusinessHours").Value = settingsModel.BusinessHours?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "BusinessHours", Value = settingsModel.BusinessHours?.ToString(), Category = category });
                }
                #endregion

                #region CellNumber
                if (currentSettings.FirstOrDefault(s => s.Key == "CellNumber") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "CellNumber").Value = settingsModel.CellNumber?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "CellNumber", Value = settingsModel.CellNumber?.ToString(), Category = category });
                }
                #endregion 

                if (currentSettings.FirstOrDefault(s => s.Key == "EmailAddress") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "EmailAddress").Value = settingsModel.EmailAddress?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "EmailAddress", Value = settingsModel.EmailAddress?.ToString(), Category = category });
                }



                if (currentSettings.FirstOrDefault(s => s.Key == "FaxNumber") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "FaxNumber").Value = settingsModel.FaxNumber?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "FaxNumber", Value = settingsModel.FaxNumber?.ToString(), Category = category });
                }



                if (currentSettings.FirstOrDefault(s => s.Key == "FooterText") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "FooterText").Value = settingsModel.FooterText?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "FooterText", Value = settingsModel.FooterText?.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "FullSiteName") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "FullSiteName").Value = settingsModel.FullSiteName?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "FullSiteName", Value = settingsModel.FullSiteName?.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "LogoUrl") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "LogoUrl").Value = settingsModel.LogoUrl?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "LogoUrl", Value = settingsModel.LogoUrl?.ToString(), Category = category });
                }



                if (currentSettings.FirstOrDefault(s => s.Key == "MailServerPassword") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "MailServerPassword").Value = settingsModel.MailServerPassword?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "MailServerPassword", Value = settingsModel.MailServerPassword?.ToString(), Category = category });
                }



                if (currentSettings.FirstOrDefault(s => s.Key == "MailServerPort") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "MailServerPort").Value = settingsModel.MailServerPort?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "MailServerPort", Value = settingsModel.MailServerPort?.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "MailServerUrl") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "MailServerUrl").Value = settingsModel.MailServerUrl?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "MailServerUrl", Value = settingsModel.MailServerUrl?.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "MailServerUserName") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "MailServerUserName").Value = settingsModel.MailServerUserName?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "MailServerUserName", Value = settingsModel.MailServerUserName?.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "MobileNumber") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "MobileNumber").Value = settingsModel.MobileNumber?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "MobileNumber", Value = settingsModel.MobileNumber?.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "SiteDescription") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "SiteDescription").Value = settingsModel.SiteDescription?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "SiteDescription", Value = settingsModel.SiteDescription?.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "SiteKeywords") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "SiteKeywords").Value = settingsModel.SiteKeywords?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "SiteKeywords", Value = settingsModel.SiteKeywords?.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "SiteName") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "SiteName").Value = settingsModel.SiteName?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "SiteName", Value = settingsModel.SiteName?.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "SiteUrl") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "SiteUrl").Value = settingsModel.SiteUrl?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "SiteUrl", Value = settingsModel.SiteUrl?.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "SMSNumber") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "SMSNumber").Value = settingsModel.SMSNumber?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "SMSNumber", Value = settingsModel.SMSNumber?.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "TelegramChannelId") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "TelegramChannelId").Value = settingsModel.TelegramChannelId?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "TelegramChannelId", Value = settingsModel.TelegramChannelId?.ToString(), Category = category });
                }



                if (currentSettings.FirstOrDefault(s => s.Key == "RegionCount") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "RegionCount").Value = settingsModel.RegionCount.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "RegionCount", Value = settingsModel.RegionCount.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "OnlineAuthenticationAfterUpdateCitizenInfo") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "OnlineAuthenticationAfterUpdateCitizenInfo").Value = settingsModel.OnlineAuthenticationAfterUpdateCitizenInfo.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "OnlineAuthenticationAfterUpdateCitizenInfo", Value = settingsModel.OnlineAuthenticationAfterUpdateCitizenInfo.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "OnlineAuthentication") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "OnlineAuthentication").Value = settingsModel.OnlineAuthentication.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "OnlineAuthentication", Value = settingsModel.OnlineAuthentication.ToString(), Category = category });
                }





                if (currentSettings.FirstOrDefault(s => s.Key == "IsfahanCityId") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "IsfahanCityId").Value = settingsModel.IsfahanCityId?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "IsfahanCityId", Value = settingsModel.IsfahanCityId?.ToString(), Category = category });
                }



                if (currentSettings.FirstOrDefault(s => s.Key == "ManzalatGroupId") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "ManzalatGroupId").Value = settingsModel.ManzalatGroupId?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "ManzalatGroupId", Value = settingsModel.ManzalatGroupId?.ToString(), Category = category });
                }









                if (currentSettings.FirstOrDefault(s => s.Key == "IsfahanProvinceId") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "IsfahanProvinceId").Value = settingsModel.IsfahanProvinceId.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "IsfahanProvinceId", Value = settingsModel.IsfahanProvinceId.ToString(), Category = category });
                }
                string output = JsonConvert.SerializeObject(settingsModel);

                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateSettings",
                    CreateDate = DateTime.Now,
                    Description = "ویرایش تنظیمات سامانه",
                    EventPriority = EventPriorityEnum.Normal,
                    EventSection = EventSectionEnum.ویرایش_تنظیمات_سامانه, 
                    EventType = EventTypeEnum.Info,
                    UserId = userId,
                    OperationId= userId,
                    JsonValue= output


                });

                await _uow.SaveChangesAsync();
                res.Data = settingsModel;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "";
                res.StatusCode = ApiResultStatusCode.ServerError;


            }


            return res;


        }
        public async Task<ApiResult<EditSettingViewModel>> GetSettingsForEdit()
        {
            var res = new ApiResult<EditSettingViewModel>(true, ApiResultStatusCode.Success, new EditSettingViewModel());

            var settings = await _SiteOptions.AsNoTracking().ToListAsync();
            try
            {
                var settingsViewModel = new EditSettingViewModel
                {
                    SiteUrl = settings.FirstOrDefault(option => option.Key == "SiteUrl")?.Value,
                    FullSiteName = settings.FirstOrDefault(option => option.Key == "FullSiteName")?.Value,
                    SiteName = settings.FirstOrDefault(option => option.Key == "SiteName")?.Value,
                    SiteKeywords = settings.FirstOrDefault(option => option.Key == "SiteKeywords")?.Value,
                    SiteDescription = settings.FirstOrDefault(option => option.Key == "SiteDescription")?.Value,
                    MailServerUrl = settings.FirstOrDefault(option => option.Key == "MailServerUrl")?.Value,
                    MailServerPort = settings.FirstOrDefault(option => option.Key == "MailServerPort")?.Value,
                    MailServerUserName = settings.FirstOrDefault(option => option.Key == "MailServerUserName")?.Value,
                    MailServerPassword = settings.FirstOrDefault(option => option.Key == "MailServerPassword")?.Value,
                    CellNumber = settings.FirstOrDefault(option => option.Key == "CellNumber")?.Value,
                    MobileNumber = settings.FirstOrDefault(option => option.Key == "MobileNumber")?.Value,
                    FaxNumber = settings.FirstOrDefault(option => option.Key == "FaxNumber")?.Value,
                    LogoUrl = settings.FirstOrDefault(option => option.Key == "LogoUrl")?.Value, 
                    FooterText = settings.FirstOrDefault(option => option.Key == "FooterText")?.Value,
                    Addresss = settings.FirstOrDefault(option => option.Key == "Addresss")?.Value,
                    BusinessHours = settings.FirstOrDefault(option => option.Key == "BusinessHours")?.Value,
                    EmailAddress = settings.FirstOrDefault(option => option.Key == "EmailAddress")?.Value,
                    SMSNumber = settings.FirstOrDefault(option => option.Key == "SMSNumber")?.Value,
                    TelegramChannelId = settings.FirstOrDefault(option => option.Key == "TelegramChannelId")?.Value,
                      

                };
                if (settings.FirstOrDefault(option => option.Key == "ManzalatGroupId") != null)
                {
                    settingsViewModel.ManzalatGroupId = int.Parse(settings.FirstOrDefault(option => option.Key == "ManzalatGroupId").Value);
                }
                if (settings.FirstOrDefault(option => option.Key == "IsfahanCityId") != null)
                {
                    settingsViewModel.IsfahanCityId = int.Parse(settings.FirstOrDefault(option => option.Key == "IsfahanCityId").Value);
                }
                if (settings.FirstOrDefault(option => option.Key == "IsfahanProvinceId") != null)
                {
                    settingsViewModel.IsfahanProvinceId = int.Parse(settings.FirstOrDefault(option => option.Key == "IsfahanProvinceId").Value);
                }
            
                if (settings.FirstOrDefault(option => option.Key == "RegionCount") != null)
                {
                    settingsViewModel.RegionCount = int.Parse(settings.FirstOrDefault(option => option.Key == "RegionCount").Value);
                }

                if (settings.FirstOrDefault(option => option.Key == "OnlineAuthenticationAfterUpdateCitizenInfo") != null)
                {
                    settingsViewModel.OnlineAuthenticationAfterUpdateCitizenInfo = bool.Parse(settings.FirstOrDefault(option => option.Key == "OnlineAuthenticationAfterUpdateCitizenInfo").Value);
                }

                if (settings.FirstOrDefault(option => option.Key == "OnlineAuthentication") != null)
                {
                    settingsViewModel.OnlineAuthentication = bool.Parse(settings.FirstOrDefault(option => option.Key == "OnlineAuthentication").Value);
                }




                res.Data = settingsViewModel;


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }



            return res;
        }



        public async Task<ApiResult<IsfahanInfo>> GetIsfahanInfo()
        {
            var res = new ApiResult<IsfahanInfo>(true, ApiResultStatusCode.Success, new IsfahanInfo());

            var settings = await _SiteOptions.AsNoTracking().ToListAsync();
            try
            {
               
                if (settings.FirstOrDefault(option => option.Key == "IsfahanCityId") != null)
                {
                    res.Data.IsfahanCityId = int.Parse(settings.FirstOrDefault(option => option.Key == "IsfahanCityId").Value);
                }
                if (settings.FirstOrDefault(option => option.Key == "IsfahanProvinceId") != null)
                {
                    res.Data.IsfahanProvinceId = int.Parse(settings.FirstOrDefault(option => option.Key == "IsfahanProvinceId").Value);
                }

              
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "تنظیمات استان و شهر اصفهان انجام نشده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }



            return res;
        }


        public async Task<ApiResult<ManzalatSettings>> GetManzalatSettings()
        {
            var res = new ApiResult<ManzalatSettings>(true, ApiResultStatusCode.Success, new ManzalatSettings());
            var category = Nikan.Common.GlobalEnum.SiteOptionCategoryEnum.تنظیمات_منزلت;
            var settings = await _SiteOptions.Where(w => w.Category == category).AsNoTracking().ToListAsync();

           
            try
            {

                

                if (settings.FirstOrDefault(option => option.Key == "MinAgeSalmand") != null)
                {
                    if (settings.FirstOrDefault(option => option.Key == "MinAgeSalmand").Value != null)
                    {
                        res.Data.MinAgeSalmand = int.Parse(settings.FirstOrDefault(option => option.Key == "MinAgeSalmand").Value);
                    }

                }

                if (settings.FirstOrDefault(option => option.Key == "MinAgeBazneshasteh") != null)
                {
                    if (settings.FirstOrDefault(option => option.Key == "MinAgeBazneshasteh").Value != null)
                    {
                        res.Data.MinAgeBazneshasteh = int.Parse(settings.FirstOrDefault(option => option.Key == "MinAgeBazneshasteh").Value);
                    }

                }

                if (settings.FirstOrDefault(option => option.Key == "JanbazanDescription") != null)
                {
                    res.Data.JanbazanDescription =  settings.FirstOrDefault(option => option.Key == "JanbazanDescription").Value ;
                }

                if (settings.FirstOrDefault(option => option.Key == "BazneshastehDescription") != null)
                {
                    res.Data.BazneshastehDescription = settings.FirstOrDefault(option => option.Key == "BazneshastehDescription").Value;
                }

                if (settings.FirstOrDefault(option => option.Key == "ZanSarparastDescription") != null)
                {
                    res.Data.ZanSarparastDescription = settings.FirstOrDefault(option => option.Key == "ZanSarparastDescription").Value;
                }
                if (settings.FirstOrDefault(option => option.Key == "SalmandDescription") != null)
                {
                    res.Data.SalmandDescription = settings.FirstOrDefault(option => option.Key == "SalmandDescription").Value;
                }
                if (settings.FirstOrDefault(option => option.Key == "MaloulinDescription") != null)
                {
                    res.Data.MaloulinDescription = settings.FirstOrDefault(option => option.Key == "MaloulinDescription").Value;
                }

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی در در دریافت تنظیمات رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }



            return res;
        }
        public async Task<ApiResult<ManzalatSettings>> UpdateManzalatSettings(ManzalatSettings settingsModel)
        {
            var category = Nikan.Common.GlobalEnum.SiteOptionCategoryEnum.تنظیمات_منزلت;
            var res = new ApiResult<ManzalatSettings>(true, ApiResultStatusCode.Success, new ManzalatSettings());
            var currentSettings = await _SiteOptions.Where(w => w.Category == category).ToListAsync();
            try
            {
                if (currentSettings.FirstOrDefault(s => s.Key == "MinAgeSalmand") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "MinAgeSalmand").Value = settingsModel.MinAgeSalmand.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "MinAgeSalmand", Value = settingsModel.MinAgeSalmand.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "MinAgeBazneshasteh") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "MinAgeBazneshasteh").Value = settingsModel.MinAgeBazneshasteh.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "MinAgeBazneshasteh", Value = settingsModel.MinAgeBazneshasteh.ToString(), Category = category });
                }




                if (currentSettings.FirstOrDefault(s => s.Key == "MaloulinDescription") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "MaloulinDescription").Value = settingsModel.MaloulinDescription?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "MaloulinDescription", Value = settingsModel.MaloulinDescription?.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "SalmandDescription") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "SalmandDescription").Value = settingsModel.SalmandDescription?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "SalmandDescription", Value = settingsModel.SalmandDescription?.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "ZanSarparastDescription") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "ZanSarparastDescription").Value = settingsModel.ZanSarparastDescription?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "ZanSarparastDescription", Value = settingsModel.ZanSarparastDescription?.ToString(), Category = category });
                }




                if (currentSettings.FirstOrDefault(s => s.Key == "BazneshastehDescription") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "BazneshastehDescription").Value = settingsModel.BazneshastehDescription?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "BazneshastehDescription", Value = settingsModel.BazneshastehDescription?.ToString(), Category = category });
                }



                







                if (currentSettings.FirstOrDefault(s => s.Key == "JanbazanDescription") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "JanbazanDescription").Value = settingsModel.JanbazanDescription.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "JanbazanDescription", Value = settingsModel.JanbazanDescription.ToString(), Category = category });
                }



                await _uow.SaveChangesAsync();
                res.Data = settingsModel;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "";
                res.StatusCode = ApiResultStatusCode.ServerError;


            }


            return res;


        }





        public async Task<ApiResult<SiteInfo>> GetSiteInfo()
        {
            var res = new ApiResult<SiteInfo>(true, ApiResultStatusCode.Success, new SiteInfo());

            var settings = await _SiteOptions.AsNoTracking().ToListAsync();
            try
            {
                var settingsViewModel = new SiteInfo
                {
                    SiteUrl = settings.FirstOrDefault(option => option.Key == "SiteUrl")?.Value,
                    FullSiteName = settings.FirstOrDefault(option => option.Key == "FullSiteName")?.Value,
                    SiteName = settings.FirstOrDefault(option => option.Key == "SiteName")?.Value,
                    CellNumber = settings.FirstOrDefault(option => option.Key == "CellNumber")?.Value,
                    MobileNumber = settings.FirstOrDefault(option => option.Key == "MobileNumber")?.Value,
                    FaxNumber = settings.FirstOrDefault(option => option.Key == "FaxNumber")?.Value,
                    LogoUrl = settings.FirstOrDefault(option => option.Key == "LogoUrl")?.Value,

                    FooterText = settings.FirstOrDefault(option => option.Key == "FooterText")?.Value,
                    Addresss = settings.FirstOrDefault(option => option.Key == "Addresss")?.Value,
                    BusinessHours = settings.FirstOrDefault(option => option.Key == "BusinessHours")?.Value,
                    EmailAddress = settings.FirstOrDefault(option => option.Key == "EmailAddress")?.Value,
                    SMSNumber = settings.FirstOrDefault(option => option.Key == "SMSNumber")?.Value,
                    TelegramChannelId = settings.FirstOrDefault(option => option.Key == "TelegramChannelId")?.Value,



                };

                res.Data = settingsViewModel;


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }



            return res;
        }



        public async Task<ApiResult<FinancialSettings>> GetFinancialSettings()
        {
            var res = new ApiResult<FinancialSettings>(true, ApiResultStatusCode.Success, new FinancialSettings());
            var category = Nikan.Common.GlobalEnum.SiteOptionCategoryEnum.تنظیمات_مالی;
            var settings = await _SiteOptions.Where(w => w.Category == category).AsNoTracking().ToListAsync();
        
            
            try
            {
                 
               
                if (settings.FirstOrDefault(option => option.Key == "BankCustomerId") != null)
                {
                    res.Data.BankCustomerId = long.Parse(settings.FirstOrDefault(option => option.Key == "BankCustomerId").Value);
                }

                if (settings.FirstOrDefault(option => option.Key == "RefundAmountDeficit") != null)
                {
                    if(settings.FirstOrDefault(option => option.Key == "RefundAmountDeficit").Value!=null)
                    {
                        res.Data.RefundAmountDeficit = int.Parse(settings.FirstOrDefault(option => option.Key == "RefundAmountDeficit").Value);
                    }
                   
                }

                if (settings.FirstOrDefault(option => option.Key == "BankPaymentMethod") != null)
                {
                    res.Data.BankPaymentMethod = int.Parse(settings.FirstOrDefault(option => option.Key == "BankPaymentMethod").Value);
                }

                if (settings.FirstOrDefault(option => option.Key == "BankUserName") != null)
                {
                    res.Data.BankUserName = settings.FirstOrDefault(option => option.Key == "BankUserName").Value;
                }

                if (settings.FirstOrDefault(option => option.Key == "CallBackUrl") != null)
                {
                    res.Data.CallBackUrl = settings.FirstOrDefault(option => option.Key == "CallBackUrl").Value;
                }

                if (settings.FirstOrDefault(option => option.Key == "BankPassword") != null)
                {
                    res.Data.BankPassword = settings.FirstOrDefault(option => option.Key == "BankPassword").Value;
                }

                if (settings.FirstOrDefault(option => option.Key == "BankTerminalId") != null)
                {
                    res.Data.BankTerminalId = long.Parse(settings.FirstOrDefault(option => option.Key == "BankTerminalId").Value);
                }

                if (settings.FirstOrDefault(option => option.Key == "RefundTerminalId") != null)
                {
                    res.Data.RefundTerminalId = long.Parse(settings.FirstOrDefault(option => option.Key == "RefundTerminalId").Value);
                }

                if (settings.FirstOrDefault(option => option.Key == "RefundUserName") != null)
                {
                    res.Data.RefundUserName = settings.FirstOrDefault(option => option.Key == "RefundUserName").Value;
                }

                if (settings.FirstOrDefault(option => option.Key == "RefundPassword") != null)
                {
                    res.Data.RefundPassword = settings.FirstOrDefault(option => option.Key == "RefundPassword").Value;
                }



                if (settings.FirstOrDefault(option => option.Key == "RefundPassword") != null)
                {
                    res.Data.RefundPassword = settings.FirstOrDefault(option => option.Key == "RefundPassword").Value;
                }

                if (settings.FirstOrDefault(option => option.Key == "RefundDeActiveDescription") != null)
                {
                    res.Data.RefundDeActiveDescription = settings.FirstOrDefault(option => option.Key == "RefundDeActiveDescription").Value;
                }


                if (settings.FirstOrDefault(option => option.Key == "RefundIsActive") != null)
                { 
                    res.Data.RefundIsActive = bool.Parse(settings.FirstOrDefault(option => option.Key == "RefundIsActive").Value);
                }




            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی در دریافت تنظیمات مالی";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }



            return res;
        }


        public async Task<ApiResult<SmsOption>> GetSmsSettings()
        {
            var res = new ApiResult<SmsOption>(true, ApiResultStatusCode.Success, new SmsOption());

            var settings = await _SiteOptions.AsNoTracking().Where(w => w.Category == Nikan.Common.GlobalEnum.SiteOptionCategoryEnum.تنظیمات_پیامک).ToListAsync();
            try
            {
                if (settings.FirstOrDefault(option => option.Key == "DomainName") != null)
                {
                    res.Data.DomainName = settings.FirstOrDefault(option => option.Key == "DomainName").Value;
                }
                if (settings.FirstOrDefault(option => option.Key == "SmsPassword") != null)
                {
                    res.Data.SmsPassword = settings.FirstOrDefault(option => option.Key == "SmsPassword").Value;
                }
                if (settings.FirstOrDefault(option => option.Key == "SenderNumber") != null)
                {
                    res.Data.SenderNumber = settings.FirstOrDefault(option => option.Key == "SenderNumber").Value;
                }

                if (settings.FirstOrDefault(option => option.Key == "SmsToken") != null)
                {
                    res.Data.SmsToken = settings.FirstOrDefault(option => option.Key == "SmsToken").Value;
                }

                if (settings.FirstOrDefault(option => option.Key == "SmsUserName") != null)
                {
                    res.Data.SmsUserName = settings.FirstOrDefault(option => option.Key == "SmsUserName").Value;
                }

                if (settings.FirstOrDefault(option => option.Key == "CountValidMobileNumber") != null)
                {
                    res.Data.CountValidMobileNumber = int.Parse(settings.FirstOrDefault(option => option.Key == "CountValidMobileNumber").Value);
                }

                if (settings.FirstOrDefault(option => option.Key == "SendSmsAfterRejectCitizenInformationInUpdateForm") != null)
                {
                    res.Data.SendSmsAfterRejectCitizenInformationInUpdateForm = bool.Parse(settings.FirstOrDefault(option => option.Key == "SendSmsAfterRejectCitizenInformationInUpdateForm").Value);
                }

                if (settings.FirstOrDefault(option => option.Key == "SendSmsAfterAdminLogin") != null)
                {
                    res.Data.SendSmsAfterAdminLogin = bool.Parse(settings.FirstOrDefault(option => option.Key == "SendSmsAfterAdminLogin").Value);
                }
                


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }



            return res;
        }


        public async Task<SmsOption> GetSmsSettingForSend()
        {
            var res = new SmsOption();
            var settings = await _SiteOptions.AsNoTracking().Where(w => w.Category == Nikan.Common.GlobalEnum.SiteOptionCategoryEnum.تنظیمات_پیامک).ToListAsync();
            try
            {
                if (settings.FirstOrDefault(option => option.Key == "DomainName") != null)
                {
                    res.DomainName = settings.FirstOrDefault(option => option.Key == "DomainName").Value;
                }
                if (settings.FirstOrDefault(option => option.Key == "SmsPassword") != null)
                {
                    res.SmsPassword = settings.FirstOrDefault(option => option.Key == "SmsPassword").Value;
                }
                if (settings.FirstOrDefault(option => option.Key == "SenderNumber") != null)
                {
                    res.SenderNumber = settings.FirstOrDefault(option => option.Key == "SenderNumber").Value;
                }
                if (settings.FirstOrDefault(option => option.Key == "SmsToken") != null)
                {
                    res.SmsToken = settings.FirstOrDefault(option => option.Key == "SmsToken").Value;
                }
                if (settings.FirstOrDefault(option => option.Key == "SmsUserName") != null)
                {
                    res.SmsUserName = settings.FirstOrDefault(option => option.Key == "SmsUserName").Value;
                }
                

                if (settings.FirstOrDefault(option => option.Key == "CountValidMobileNumber") != null)
                {
                    res.CountValidMobileNumber =int.Parse( settings.FirstOrDefault(option => option.Key == "CountValidMobileNumber").Value);
                }

                if (settings.FirstOrDefault(option => option.Key == "SendSmsAfterRejectCitizenInformationInUpdateForm") != null)
                {
                    res.SendSmsAfterRejectCitizenInformationInUpdateForm = bool.Parse(settings.FirstOrDefault(option => option.Key == "SendSmsAfterRejectCitizenInformationInUpdateForm").Value);
                }

                if (settings.FirstOrDefault(option => option.Key == "SendSmsAfterAdminLogin") != null)
                {
                    res.SendSmsAfterAdminLogin = bool.Parse(settings.FirstOrDefault(option => option.Key == "SendSmsAfterAdminLogin").Value);
                }
               


    }
            catch (Exception er)
            {


            }



            return res;
        }

        public async Task<SmsOption> GetSmsSettingForOnlineAuthentication()
        {
            var res = new SmsOption();
            var settings = await _SiteOptions.AsNoTracking().ToListAsync();
            try
            {
                 
                if (settings.FirstOrDefault(option => option.Key == "SmsToken") != null)
                {
                    res.SmsToken = settings.FirstOrDefault(option => option.Key == "SmsToken").Value;
                }
                

                if (settings.FirstOrDefault(option => option.Key == "CountValidMobileNumber") != null)
                {
                    res.CountValidMobileNumber = int.Parse(settings.FirstOrDefault(option => option.Key == "CountValidMobileNumber").Value);
                }

                if (settings.FirstOrDefault(option => option.Key == "SendSmsAfterRejectCitizenInformationInUpdateForm") != null)
                {
                    res.SendSmsAfterRejectCitizenInformationInUpdateForm = bool.Parse(settings.FirstOrDefault(option => option.Key == "SendSmsAfterRejectCitizenInformationInUpdateForm").Value);
                }


                if (settings.FirstOrDefault(option => option.Key == "OnlineAuthenticationAfterUpdateCitizenInfo") != null)
                {
                    res.OnlineAuthenticationAfterUpdateCitizenInfo = bool.Parse(settings.FirstOrDefault(option => option.Key == "OnlineAuthenticationAfterUpdateCitizenInfo").Value);
                }


                if (settings.FirstOrDefault(option => option.Key == "OnlineAuthentication") != null)
                {
                    res.OnlineAuthentication = bool.Parse(settings.FirstOrDefault(option => option.Key == "OnlineAuthentication").Value);
                }




            }
            catch (Exception er)
            {


            }



            return res;
        }


        public async Task<ApiResult<SmsOption>> UpdateSmsSettings(SmsOption settingsModel)
        {
            var category = Nikan.Common.GlobalEnum.SiteOptionCategoryEnum.تنظیمات_پیامک;
            var res = new ApiResult<SmsOption>(true, ApiResultStatusCode.Success, new SmsOption());
            var currentSettings = await _SiteOptions.Where(w => w.Category == category).ToListAsync();
            try
            {

                if (settingsModel.SendSmsAfterAdminLogin == null)
                    settingsModel.SendSmsAfterAdminLogin = false;

                if (currentSettings.FirstOrDefault(s => s.Key == "DomainName") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "DomainName").Value = settingsModel.DomainName?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "DomainName", Value = settingsModel.DomainName?.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "SmsPassword") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "SmsPassword").Value = settingsModel.SmsPassword?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "SmsPassword", Value = settingsModel.SmsPassword?.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "SenderNumber") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "SenderNumber").Value = settingsModel.SenderNumber?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "SenderNumber", Value = settingsModel.SenderNumber?.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "SmsUserName") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "SmsUserName").Value = settingsModel.SmsUserName?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "SmsUserName", Value = settingsModel.SmsUserName?.ToString(), Category = category });
                }



                if (currentSettings.FirstOrDefault(s => s.Key == "SmsToken") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "SmsToken").Value = settingsModel.SmsToken?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "SmsToken", Value = settingsModel.SmsToken?.ToString(), Category = category });
                }



              

               

 

                if (currentSettings.FirstOrDefault(s => s.Key == "CountValidMobileNumber") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "CountValidMobileNumber").Value = settingsModel.CountValidMobileNumber.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "CountValidMobileNumber", Value = settingsModel.CountValidMobileNumber.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "SendSmsAfterAdminLogin") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "SendSmsAfterAdminLogin").Value = settingsModel.SendSmsAfterAdminLogin.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "SendSmsAfterAdminLogin", Value = settingsModel.SendSmsAfterAdminLogin.ToString(), Category = category });
                }



                if (currentSettings.FirstOrDefault(s => s.Key == "SendSmsAfterRejectCitizenInformationInUpdateForm") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "SendSmsAfterRejectCitizenInformationInUpdateForm").Value = settingsModel.SendSmsAfterRejectCitizenInformationInUpdateForm.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "SendSmsAfterRejectCitizenInformationInUpdateForm", Value = settingsModel.SendSmsAfterRejectCitizenInformationInUpdateForm.ToString(), Category = category });
                }


 












                await _uow.SaveChangesAsync();
                res.Data = settingsModel;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "";
                res.StatusCode = ApiResultStatusCode.ServerError;


            }


            return res;


        }



        public async Task<ApiResult<FinancialSettings>> UpdateFinancialSettings(FinancialSettings settingsModel)
        {
            var category = Nikan.Common.GlobalEnum.SiteOptionCategoryEnum.تنظیمات_مالی;
            var res = new ApiResult<FinancialSettings>(true, ApiResultStatusCode.Success, new FinancialSettings());
            var currentSettings = await _SiteOptions.Where(w => w.Category == category).ToListAsync();
            try
            {



                if (currentSettings.FirstOrDefault(s => s.Key == "RefundAmountDeficit") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "RefundAmountDeficit").Value = settingsModel.RefundAmountDeficit?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "RefundAmountDeficit", Value = settingsModel.RefundAmountDeficit?.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "BankCustomerId") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "BankCustomerId").Value = settingsModel.BankCustomerId.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "BankCustomerId", Value = settingsModel.BankCustomerId.ToString(), Category = category });
                }

                 
                
                if (currentSettings.FirstOrDefault(s => s.Key == "BankPaymentMethod") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "BankPaymentMethod").Value = settingsModel.BankPaymentMethod.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "BankPaymentMethod", Value = settingsModel.BankPaymentMethod.ToString(), Category = category });
                }



                if (currentSettings.FirstOrDefault(s => s.Key == "BankUserName") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "BankUserName").Value = settingsModel.BankUserName?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "BankUserName", Value = settingsModel.BankUserName?.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "CallBackUrl") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "CallBackUrl").Value = settingsModel.CallBackUrl?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "CallBackUrl", Value = settingsModel.CallBackUrl?.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "BankPassword") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "BankPassword").Value = settingsModel.BankPassword?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "BankPassword", Value = settingsModel.BankPassword?.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "BankTerminalId") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "BankTerminalId").Value = settingsModel.BankTerminalId.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "BankTerminalId", Value = settingsModel.BankTerminalId.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "RefundTerminalId") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "RefundTerminalId").Value = settingsModel.RefundTerminalId.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "RefundTerminalId", Value = settingsModel.RefundTerminalId.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "RefundUserName") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "RefundUserName").Value = settingsModel.RefundUserName?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "RefundUserName", Value = settingsModel.RefundUserName?.ToString(), Category = category });
                }



                if (currentSettings.FirstOrDefault(s => s.Key == "RefundPassword") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "RefundPassword").Value = settingsModel.RefundPassword?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "RefundPassword", Value = settingsModel.RefundPassword?.ToString(), Category = category });
                }

                if (currentSettings.FirstOrDefault(s => s.Key == "RefundIsActive") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "RefundIsActive").Value = settingsModel.RefundIsActive.ToString() ;
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "RefundIsActive", Value = settingsModel.RefundIsActive.ToString(), Category = category });
                }


                if (currentSettings.FirstOrDefault(s => s.Key == "RefundDeActiveDescription") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "RefundDeActiveDescription").Value = settingsModel.RefundDeActiveDescription?.ToString();
                }
                else
                {
                    await _SiteOptions.AddAsync(new SiteOption() { Key = "RefundDeActiveDescription", Value = settingsModel.RefundDeActiveDescription?.ToString(), Category = category });
                }


                await _uow.SaveChangesAsync();
                res.Data = settingsModel;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "";
                res.StatusCode = ApiResultStatusCode.ServerError;


            }


            return res;


        }


        public async Task<ApiResult<string>> GetSiteUrl()
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");


            try
            {
                var row = await _SiteOptions.AsNoTracking().Where(w => w.Key == "SiteUrl").FirstOrDefaultAsync();
                if (row != null)
                {
                    res.Data = row.Value;
                }


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }



            return res;
        }





        public async Task<ApiResult<string>> UpdateLogo(string url)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            var currentSettings = await _SiteOptions.ToListAsync();
            try
            {
                if (currentSettings.FirstOrDefault(s => s.Key == "LogoUrl") != null)
                {
                    currentSettings.FirstOrDefault(s => s.Key == "LogoUrl").Value = url;
                }

                res.Data = url;
                await _uow.SaveChangesAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "";
                res.StatusCode = ApiResultStatusCode.ServerError;


            }
            return res;


        }









    }
}
