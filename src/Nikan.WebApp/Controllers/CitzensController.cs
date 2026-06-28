using cle.Services;
using cle.Services.Citizens;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Nikan.Common;
using Nikan.Common.ApiCall;
using Nikan.Common.GlobalEnum;
using Nikan.Common.Utilities;
using Nikan.DataLayer.Context;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.Services.Citizens;
using Nikan.Services.Events;
using Nikan.Services.ExportCitizen;
using Nikan.Services.ImportFile;
using Nikan.Services.Permissions;
using Nikan.ViewModel;
using Nikan.ViewModel.Citizens;
using Nikan.ViewModel.Users;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nikan.WebApp.Controllers
{
    /// <summary>
    /// شهروند
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class CitzensController : Controller
    {

        #region Ctor

        private readonly IUsersService _usersService;
        public static IHostingEnvironment _environment;
        private readonly ICitizenService _citzene;
        private readonly ICitizenFamiliesService _family;
        private readonly IAppService _app;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly IUnitOfWork _uow;
        private readonly IAntiForgeryCookieService _antiforgery;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly ISmsInfoService _smsInfoService;
        private readonly ICitizenFeedbackService _citizenFeedback;
        private readonly ISiteSettingService _siteSettingService;
        private readonly ICitizenSummaryEducationServices _educationService;
        private readonly IEventService _event;
        private readonly IImportExcelFileService _importFile;
        private readonly IPermissionService _permission;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IMemoryCache _cache;
        private readonly IExportCitizenService _exportCitizen;




        public CitzensController(
              IUsersService userManager,
              IPermissionService permission,
              ICitizenService citzene,
              IExportCitizenService  exportCitizen,
               IImportExcelFileService importFile,
                IEventService siteevent,
               ICitizenFamiliesService family,
              ICitizenFeedbackService citizenFeedback,
              IAppService app,
              ICitizenSummaryEducationServices educationService,
              IHostingEnvironment environment,
            ISmsInfoService smsInfoService,
            ITokenStoreService tokenStoreService,
            ITokenFactoryService tokenFactoryService,
            IUnitOfWork uow,
            ISiteSettingService siteSettingService,
            IAntiForgeryCookieService antiforgery,
              IBackgroundJobClient backgroundJobClient,
              IMemoryCache memoryCache

            )
        {
            _citzene = citzene;
            _family = family;
            _citizenFeedback = citizenFeedback;
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));
            _environment = environment;
            _app = app;
            _permission = permission;
            _importFile = importFile;
            _siteSettingService = siteSettingService;
            _uow = uow;
            _event = siteevent;
            _uow.CheckArgumentIsNull(nameof(_uow));
            _smsInfoService = smsInfoService;
            _antiforgery = antiforgery;
            _antiforgery.CheckArgumentIsNull(nameof(antiforgery));
            _educationService = educationService;
            _tokenStoreService = tokenStoreService;
            _tokenStoreService.CheckArgumentIsNull(nameof(tokenStoreService));
            _tokenFactoryService = tokenFactoryService;
            _tokenFactoryService.CheckArgumentIsNull(nameof(tokenFactoryService));
            _backgroundJobClient = backgroundJobClient;
            _cache = memoryCache;
            _exportCitizen =  exportCitizen;

        }
        #endregion 
        #region ثبت نام شهروند
        /// <summary>
        /// لیست خدمات قابل ثبت نام
        /// [C1] 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAppRegisterList")]
        public async Task<ApiResult<List<AppServiceInfo>>> GetAppRegisterList()
        {

            return await _app.GetAppRegisterList();

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetAppRegisterListForMainPage")]
        public async Task<ApiResult<List<AppServiceInfo>>> GetAppRegisterListForMainPage()
        {
            return await _app.GetAppRegisterListForMainPage();

        }


        /// <summary>
        /// چک کردن اینکه ایا شهروند امکان ثبت نام را دارد یا نه؟
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ApiResult<PreRegisterResult>> CheckPossibilityCitizenRegistration(PreRegisterDto model)
        {
            return await _citzene.CheckCitizenRegister(model);
        }

        /// <summary>
        /// چک کردن شماره موبایل بابت ثبت نام یا تغییر شماره موبایل
        /// [C3]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ApiResult> CheckValidMobileNumberForCitzenRegister(CheckValidMobileNumberViewModel model)
        {
            return await _citzene.CheckValidMobileNumberForCitizenRegister(model.NewMobileNumber);

        }

        /// <summary>
        /// بررسی شماره موبایل جدید و دریافت کد تغییر به وسیله شهروند
        /// </summary>
        /// <param name="model">شماره موبایل جدید</param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Citizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> CheckValidMobileNumberAndGetVerfiyCodeForChangeMobileNumber(CheckValidMobileNumberViewModel model)
        {
            var res1 = await _citzene.CheckValidMobileNumberForCitizenRegister(model.NewMobileNumber);
            if (!res1.IsSuccess)
            {
                return res1;
            }

            res1 = new ApiResult(true, ApiResultStatusCode.Success, "کد تایید تغییر پیامک با موفقیت ارسال گردید");

            var mobileNumber = model.NewMobileNumber.Fa2En();

            try
            {
                //ارسال پیامک
                SendSms sms = new SendSms();
                //چک کن تا سه دقیقه قبل براش پیامک ارسال نشده باشه

                Random r = new Random();
                var code = "";
                var lastCode = await _smsInfoService.GetLastMobileValidationByMobileNumber(mobileNumber);
                if (lastCode.IsSuccess)
                {
                    var time = DateTime.Now.AddSeconds(-180);//دو دقیقه قبل
                    if (lastCode.Data.SendOnDate > time)
                    {
                        res1.Messages = "برای شما کد تایید ارسال شده است . لطفا منتظر دریافت کد تایید باشید ";
                        res1.IsSuccess = false;
                        res1.StatusCode = ApiResultStatusCode.BadRequest;
                        return res1;
                    }

                    code = lastCode.Data.Token1;
                }
                else
                {
                    code = r.Next(10000, 99999).ToString();
                }
                var smsOption = await _siteSettingService.GetSmsSettingForSend();
                var smsLog = await sms.VerifyLookup(mobileNumber, code, TempleteNameEnum.MobileVerify, smsOption.SmsToken);
                if (smsLog.IsSuccess)
                {
                    var citizenId = _usersService.GetCurrentUserId();
                    var saveSms = await _smsInfoService.Add(smsLog.Data, citizenId);
                    if (!saveSms.IsSuccess)
                    {
                        return new ApiResult(false, ApiResultStatusCode.BadRequest, saveSms.Messages);
                    }
                }
                else
                {
                    return new ApiResult(false, ApiResultStatusCode.BadRequest, smsLog.Messages);
                }

            }
            catch (Exception er)
            {

                res1.Messages = "خطایی در ارسال پیامک رخ داده است";
                res1.StatusCode = ApiResultStatusCode.BadRequest;
                return res1;
            }
            return res1;



           
        }


        /// <summary>
        /// اعتبارسنجی شماره موبایل
        /// ارسال پیامک
        ///  [C4]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "WebApiUser")] 
        public async Task<ApiResult> ValidatePhoneNumber(ValidatePhoneNumber model)
        {
            //ارسال پیامک
            var res1 = new ApiResult(true, ApiResultStatusCode.Success, "");

            var userId = _usersService.GetCurrentUserId(); 
            var canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.ارسال_پیامک_اعتبارسنجی_شماره_موبایل);
            if (!canAccess.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, "کاربر توسعه دهنده به این سرویس دسترسی ندارد", "کاربر توسعه دهنده به این سرویس دسترسی ندارد");


            if (string.IsNullOrWhiteSpace(model.MobileNumber))
            {
                res1.Messages = "شماره موبایل را وارد نمایید";
                res1.IsSuccess = false;
                res1.StatusCode = ApiResultStatusCode.BadRequest;
                return res1;

            }
            if (model.MobileNumber.Length != 11)
            {
                res1.Messages = "شماره موبایل را به صورت صحیح وارد نمایید";
                res1.IsSuccess = false;
                res1.StatusCode = ApiResultStatusCode.BadRequest;
                return res1;
            }
            model.MobileNumber = model.MobileNumber.Fa2En();
            if (!model.MobileNumber.StartsWith("09"))
            {
                res1.Messages = "شماره موبایل را به صورت صحیح وارد نمایید";
                res1.IsSuccess = false;
                res1.StatusCode = ApiResultStatusCode.BadRequest;
                return res1;
            }

            SendSms sms = new SendSms();
            Random r = new Random();
            var code = "";
            var lastCode = await _smsInfoService.GetLastMobileValidationByMobileNumber(model.MobileNumber);
            if (lastCode.IsSuccess)
            {
                //همیشه یه کد یکتا را به عنوان کد اعتبارسنجی ارسال کن
                code = lastCode.Data.Token1;
            }
            else
            {
                //تولید کد 5 رقمی
                code = r.Next(10000, 99999).ToString();
            }
            var smsOption = await _siteSettingService.GetSmsSettingForSend();
            if (smsOption == null)
            {
                res1.Messages = "تنظیمات پیامک انجام نشده است ";
                res1.IsSuccess = false;
                res1.StatusCode = ApiResultStatusCode.BadRequest;
                return res1;
            }

            if (string.IsNullOrWhiteSpace(smsOption.SmsToken))
            {
                res1.Messages = "تنظیمات پیامک انجام نشده است ";
                res1.IsSuccess = false;
                res1.StatusCode = ApiResultStatusCode.BadRequest;
                return res1;
            }




            var smsLog = await sms.VerifyLookup(model.MobileNumber, code, TempleteNameEnum.MobileVerify, smsOption.SmsToken);
            if (smsLog.IsSuccess)
            {
                var saveSms = await _smsInfoService.Add(smsLog.Data, null);
                if (!saveSms.IsSuccess)
                {
                    return new ApiResult(false, ApiResultStatusCode.BadRequest, saveSms.Messages);
                }
            }
            else
            {
                return new ApiResult(false, ApiResultStatusCode.BadRequest, smsLog.Messages);
            }


            return new ApiResult(true, ApiResultStatusCode.BadRequest, "پیامک با موفقیت ارسال گردید.");
        }


        /// <summary>
        /// ارسال مجدد کد تایید شماره موبایل
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ApiResult> ReSendVerfiyCode(ValidatePhoneNumber model)
        {
            //ارسال پیامک
            var res1 = new ApiResult(true, ApiResultStatusCode.Success, "");



            if (string.IsNullOrWhiteSpace(model.MobileNumber))
            {
                res1.Messages = "شماره موبایل را وارد نمایید";
                res1.IsSuccess = false;
                res1.StatusCode = ApiResultStatusCode.BadRequest;
                return res1;

            }
            if (model.MobileNumber.Length != 11)
            {
                res1.Messages = "شماره موبایل را به صورت صحیح وارد نمایید";
                res1.IsSuccess = false;
                res1.StatusCode = ApiResultStatusCode.BadRequest;
                return res1;
            }
            model.MobileNumber = model.MobileNumber.Fa2En();
            if (!model.MobileNumber.StartsWith("09"))
            {
                res1.Messages = "شماره موبایل را به صورت صحیح وارد نمایید";
                res1.IsSuccess = false;
                res1.StatusCode = ApiResultStatusCode.BadRequest;
                return res1;
            }

            SendSms sms = new SendSms();
            Random r = new Random();
            var code = "";
            var lastCode = await _smsInfoService.GetLastMobileValidationByMobileNumber(model.MobileNumber);
            if (lastCode.IsSuccess)
            {
                var time = DateTime.Now.AddSeconds(-180);//سه دقیقه قبل
                if (lastCode.Data.SendOnDate > time)
                {
                    res1.Messages = "برای شما کد تایید ارسال شده است . لطفا منتظر دریافت کد تایید باشید ";
                    res1.IsSuccess = false;
                    res1.StatusCode = ApiResultStatusCode.BadRequest;
                    return res1;
                }
                //همیشه یه کد یکتا را به عنوان کد اعتبارسنجی ارسال کن
                code = lastCode.Data.Token1;
            }
            else
            {
                //تولید کد 5 رقمی
                code = r.Next(10000, 99999).ToString();
            }
            var smsOption = await _siteSettingService.GetSmsSettingForSend();
            if (smsOption == null)
            {
                res1.Messages = "تنظیمات پیامک انجام نشده است ";
                res1.IsSuccess = false;
                res1.StatusCode = ApiResultStatusCode.BadRequest;
                return res1;
            }

            if (string.IsNullOrWhiteSpace(smsOption.SmsToken))
            {
                res1.Messages = "تنظیمات پیامک انجام نشده است ";
                res1.IsSuccess = false;
                res1.StatusCode = ApiResultStatusCode.BadRequest;
                return res1;
            }




            var smsLog = await sms.VerifyLookup(model.MobileNumber, code, TempleteNameEnum.MobileVerify, smsOption.SmsToken);
            if (smsLog.IsSuccess)
            {
                var saveSms = await _smsInfoService.Add(smsLog.Data, null);
                if (!saveSms.IsSuccess)
                {
                    return new ApiResult(false, ApiResultStatusCode.BadRequest, saveSms.Messages);
                }
            }
            else
            {
                return new ApiResult(false, ApiResultStatusCode.BadRequest, smsLog.Messages);
            }


            return new ApiResult(true, ApiResultStatusCode.BadRequest, "پیامک با موفقیت ارسال گردید.");
        }



        /// <summary>
        /// ثبت نام شهروند در سامانه
        /// [C5]
        ///  /api/citzens/CitizenRegister
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<UserInfoDto>> CitizenRegister([FromBody]  CitizensRegisterDto model)
        {

            var res = new ApiResult<UserInfoDto>(true, ApiResultStatusCode.Success, new UserInfoDto());
            try
            {
                if(model==null)
                {
                    res.Messages = "مدل ورودی معتبر نمی باشد";
                    res.IsSuccess = false;
                    return res;
                }


                if (model.ServiceId == null)
                    model.ServiceId = 0;//پروفایل شهروندی

                if (string.IsNullOrWhiteSpace(model.VerifyCode))
                {
                    res.Messages = "کد تایید شماره موبایل را وارد نمایید";
                    res.IsSuccess = false;
                    return res;
                }

                model.VerifyCode = model.VerifyCode.Fa2En();

                var lastCode = await _smsInfoService.GetLastMobileValidationByMobileNumber(model.MobileNumber);
                if (lastCode.IsSuccess)
                {
                    if (lastCode.Data.Token1 != model.VerifyCode)
                    {
                        res.Messages = "کد تایید شماره موبایل وارد شده صحیح نمی باشد.";
                        res.IsSuccess = false;
                        return res;
                    }
                }
                else
                {
                    res.Messages = "کد تایید شماره موبایلی برای شما ثبت نشده است";
                    res.IsSuccess = false;
                    return res;
                }

                model.NationCode = model.NationCode.Fa2En();

                var isregister = await _citzene.CitizenRegister(model,null);
                if (isregister.IsSuccess)
                {
                 
                    var user = isregister.Data;
                    if (model.Nationality == 0)
                        _backgroundJobClient.Enqueue(() => AddCitizenTodayExportList(user.Id));

                    _backgroundJobClient.Enqueue(() => UpdateCitizenGroupsJobs(model.NationCode));


                    var result = await _tokenFactoryService.CreateJwtTokensAsync(user);
                    await _tokenStoreService.AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, null);
                    await _uow.SaveChangesAsync();
                    _antiforgery.RegenerateAntiForgeryCookies(result.Claims);
                    //موقعی که شهروند ثبت نام کرد توکن و رفرش توکن را برگردون 
                    res.Data.access_token = result.AccessToken;
                    res.Data.refresh_token = result.RefreshToken;
                    res.Data.ServiceId = model.ServiceId;
                    var userIp = Request.HttpContext.Connection.RemoteIpAddress;
                    await _event.AddEvent(new ViewModel.Events.EventDto()
                    {
                        Description = user.DisplayName + "(" + model.ServiceId + ")",
                        UserId = user.Id,
                        ActionName = "CitizenRegister",
                        EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                        EventSection = Common.GlobalEnum.EventSectionEnum.ثبت_نام_شهروند,
                        EventType = Common.GlobalEnum.EventTypeEnum.Info,
                        OperationId = user.Id,
                        UserIp = userIp.ToString()

                    });
                }
                else
                {
                    res.Messages = isregister.Messages;
                    res.IsSuccess = false;
                }
                // HttpContext.Session.Remove(_captchaHashKey);
            }
            catch (Exception er)
            {
                res.Messages = "خطایی در ثبت نام رخ داده است";
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;
        }



        [Authorize(Roles = "WebApiUser")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<UserInfoDto>> CitizenRegisterByService([FromBody]  CitizensRegisterDto model)
        {

            var res = new ApiResult<UserInfoDto>(true, ApiResultStatusCode.Success, new UserInfoDto());
            try
            {
                if (model.ServiceId == 0 || model.ServiceId == null)
                {
                    res.Messages = "شناسه خدمت را مشخص کنید ";
                    res.IsSuccess = false;
                    return res;
                }

                var userId = _usersService.GetCurrentUserId();
                var canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.ثبت_نام_شهروند);
                if (!canAccess.IsSuccess)
                    return new ApiResult<UserInfoDto>(false, ApiResultStatusCode.NotAllowed,new UserInfoDto(), "کاربر توسعه دهنده به این سرویس دسترسی ندارد");



                var isregister = await _citzene.CitizenRegister(model, userId);
                if (isregister.IsSuccess)
                {
                    var user = isregister.Data;


                    if (model.Nationality == 0)
                        _backgroundJobClient.Enqueue(() => AddCitizenTodayExportList(user.Id));

                    _backgroundJobClient.Enqueue(() => UpdateCitizenGroupsJobs(model.NationCode));



                    var result = await _tokenFactoryService.CreateJwtTokensAsync(user);
                    await _tokenStoreService.AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, null);
                    await _uow.SaveChangesAsync();
                    _antiforgery.RegenerateAntiForgeryCookies(result.Claims);
                    res.Data.access_token = result.AccessToken;
                    res.Data.refresh_token = result.RefreshToken;
                    res.Data.ServiceId = model.ServiceId;
                    await _event.AddEvent(new ViewModel.Events.EventDto()
                    {
                        Description = user.DisplayName + "(" + model.ServiceId + ")",
                        UserId = user.Id,
                        ActionName = "CitizenRegister",
                        EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                        EventSection = Common.GlobalEnum.EventSectionEnum.ثبت_نام_شهروند,
                        EventType = Common.GlobalEnum.EventTypeEnum.Info,
                        OperationId = user.Id,


                    });
                }
                else
                {
                    res.Messages = isregister.Messages;
                    res.IsSuccess = false;
                }
                // HttpContext.Session.Remove(_captchaHashKey);
            }
            catch (Exception er)
            {
                res.Messages = "خطایی در ثبت نام رخ داده است";
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;
        }



        /// <summary>
        /// ثبت نام سریع شهروند 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "WebApiUser")] 
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<UserInfoDto>> QuickCitizenRegisterByService([FromBody]  QuickCitizensRegisterDto model)
        {

            var res = new ApiResult<UserInfoDto>(true, ApiResultStatusCode.Success, new UserInfoDto());
            try
            {
                var userId = _usersService.GetCurrentUserId();
                var canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.ثبت_نام_سریع_شهروند);
                if (!canAccess.IsSuccess)
                    return new ApiResult<UserInfoDto>(false, ApiResultStatusCode.NotAllowed, new UserInfoDto(), "کاربر توسعه دهنده به این سرویس دسترسی ندارد");


                var isregister = await _citzene.QuickCitizenRegister(model, userId);
                if (isregister.IsSuccess)
                {
                    var user = isregister.Data;

                    if (model.Nationality == 0)
                        _backgroundJobClient.Enqueue(() => AddCitizenTodayExportList(user.Id)); 

                    _backgroundJobClient.Enqueue(() => UpdateCitizenGroupsJobs(model.NationCode));


                    var result = await _tokenFactoryService.CreateJwtTokensAsync(user);
                    await _tokenStoreService.AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, null);
                    await _uow.SaveChangesAsync();
                    _antiforgery.RegenerateAntiForgeryCookies(result.Claims);
                    res.Data.access_token = result.AccessToken;
                    res.Data.refresh_token = result.RefreshToken;
                    res.Data.ServiceId = model.ServiceId;
                    res.Data.UserId = user.Id;
                    res.Data.UserName = user.Username;
                    res.Data.MobileNumber = user.MobileNumber;

                    res.Data.UserAccountState = user.UserAccountState;
                    res.Data.DisplayName = user.DisplayName;
                    await _event.AddEvent(new ViewModel.Events.EventDto()
                    {
                        Description = user.DisplayName + "(" + model.ServiceId + ")",
                        UserId = user.Id,
                        ActionName = "QuickCitizenRegisterByService",
                        EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                        EventSection = Common.GlobalEnum.EventSectionEnum.ثبت_نام_شهروند,
                        EventType = Common.GlobalEnum.EventTypeEnum.Info,
                        OperationId = user.Id,


                    });
                }
                else
                {
                    res.Messages = isregister.Messages;
                    res.IsSuccess = false;
                }
                // HttpContext.Session.Remove(_captchaHashKey);
            }
            catch (Exception er)
            {
                res.Messages = "خطایی در ثبت نام رخ داده است";
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;
        }



        [HttpGet]
        [Route("UpdateCitizenGroupsJobs")]
        [Authorize()]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task UpdateCitizenGroupsJobs(string nationcode)
        {

            await _citzene.UpdateCitizenGroups(nationcode);

        }


        [HttpGet]
        [Route("AddCitizenTodayExportList")]
        [Authorize()]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task AddCitizenTodayExportList(int citizenId)
        {
            await _exportCitizen.AddCitizenTodayExportList(citizenId);
        }


        #endregion
        #region ویرایش اطلاعات شهروند

        /// <summary>
        /// دریافت اطلاعات شهروند
        ///  [C6] 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "WebApiUser")]
        [Route("GetCitizenBaseInfo")]
        public async Task<ApiResult<CitizenBaseInfo>> GetCitizenBaseInfo(string   userCode)
        {
            

            var webApiuserId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(webApiuserId, Common.GlobalEnum.PermissionTypeEnum.دریافت_اطلاعات_کوتاه_شهروند);
            if (!canAccess.IsSuccess)
                return new ApiResult<CitizenBaseInfo>(false, ApiResultStatusCode.NotAllowed, new CitizenBaseInfo(), "کاربر توسعه دهنده به این سرویس دسترسی ندارد");

            return await _citzene.GetCitizenBaseInfo(userCode);

        }



        /// <summary>
        /// دریافت اطلاعات کاربر به وسیله شناسه Guid
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetCitizenBaseInfoByAdmin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CitizenBaseInfo>> GetCitizenBaseInfoByAdmin(string  userCode)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.جستجوی_شهروند);
            if (!canAccess.IsSuccess)
                return new ApiResult<CitizenBaseInfo>(false, ApiResultStatusCode.NotAllowed, null, "شما به جستجوی شهروند دسترسی ندارید");


            return await _citzene.GetCitizenBaseInfo(userCode); 
        }



        /// <summary>
        /// نمایش اطلاعات پایه برای خود شهروند
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetCitizenBaseInfoByCitizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CitizenBaseInfo>> GetCitizenBaseInfoByCitizen()
        {
            var  userId = _usersService.GetCurrentUserId();
            return await _citzene.GetCitizenBaseInfo(userId);

        }


        /// <summary>
        /// دریافت اطلاعات شناسنامه ایی
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetIdentityInformationByCitizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CitizenIdentityInformation>> GetIdentityInformationByCitizen()
        {
            var  userId = _usersService.GetCurrentUserId();
            return await _citzene.GetIdentityInformation(userId);

        }




        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetIdentityInformationByAdmin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CitizenIdentityInformation>> GetIdentityInformationByAdmin(string userCode)
        { 
            return await _citzene.GetIdentityInformation(userCode); 
        }



        /// <summary>
        /// ویرایش اطلاعات شناسنامه شهروند توسط خود شهروند
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Citizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<string>> UpdateIdentityInformationByCitizen(CitizenIdentityInformation model)
        {
            var userId = _usersService.GetCurrentUserId(); 
            var res = await _citzene.UpdateIdentityInformation(model, userId, userId); 
            var smsOption = await _siteSettingService.GetSmsSettingForOnlineAuthentication();
            if (smsOption == null)
                return res; 
            if (string.IsNullOrWhiteSpace(smsOption.SmsToken))
                return res;
            if (smsOption.OnlineAuthenticationAfterUpdateCitizenInfo)
            {
                await _event.AddEvent(new ViewModel.Events.EventDto()
                {
                    Description = "ویرایش اطلاعات شهروند و ارسال اطلاعات به سرویس احراز هویت",
                    UserId = userId,
                    ActionName = "UpdateIdentityInformationByCitizen",
                    EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                    EventSection = Common.GlobalEnum.EventSectionEnum.احراز_هویت,
                    EventType = Common.GlobalEnum.EventTypeEnum.Info,
                    OperationId = userId,


                });

                _backgroundJobClient.Enqueue(() => CitizenAuthenticationAfterUpdateIdentityInformation(userId, smsOption));

            }

            return res;
        }



        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<string>> UpdateIdentityInformationByAdmin(CitizenIdentityInformation model)
        {
          

            var userId = _usersService.GetCurrentUserId(); 
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.ویرایش_اطلاعات_شهروند);
            if (!canAccess.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, null, "شما به ویرایش اطلاعات شهروند دسترسی ندارید");
             


            if (model != null && !string.IsNullOrWhiteSpace(model.UserCode))
            {
                var res = await _citzene.UpdateIdentityInformation(model, model.UserCode, userId);
                var smsOption = await _siteSettingService.GetSmsSettingForOnlineAuthentication();
                if (smsOption == null)
                    return res;
                if (string.IsNullOrWhiteSpace(smsOption.SmsToken))
                    return res;
                if (smsOption.OnlineAuthenticationAfterUpdateCitizenInfo)
                {
                    await _event.AddEvent(new ViewModel.Events.EventDto()
                    {
                        Description = "ویرایش اطلاعات شهروند و ارسال اطلاعات به سرویس احراز هویت",
                        UserId = userId,
                        ActionName = "UpdateIdentityInformationByAdmin",
                        EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                        EventSection = Common.GlobalEnum.EventSectionEnum.احراز_هویت,
                        EventType = Common.GlobalEnum.EventTypeEnum.Info,
                        OperationId = userId,


                    });
                    _backgroundJobClient.Enqueue(() => CitizenAuthenticationAfterUpdateIdentityInformationByAdmin(model.UserCode, smsOption));

                }

            }
            else
            {
                return new ApiResult<string>(false, ApiResultStatusCode.BadRequest, "اطلاعات ارسالی نامعتبر می باشد", "اطلاعات ارسالی نامعتبر می باشد");

            }

            return new ApiResult<string>(true,ApiResultStatusCode.BadRequest,"اطلاعات با موفقیت ثبت شد و برای استعلام ارسال شد", "اطلاعات با موفقیت ثبت شد و برای استعلام ارسال شد");
        }



        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<string>> UpdateSabtStatus(UpdateSabtStatus model)
        {

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.تغییر_وضعیت_ثبت_احوال);
            if (!canAccess.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, null, "شما به تغییر وضعیت ثبت احوال شهروند دسترسی ندارید");
            return await _citzene.UpdateSabtStatus(model, userId);

        }

        [Authorize(Roles = "Card")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<string>> UpdateSabtStatusByCard(UpdateSabtStatus model)
        {
             

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.احراز_هویت_شهروند_توسط_اپراتور);
            if (!canAccess.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, "شما به احراز هویت شهروند دسترسی ندارید", "شما به احراز هویت شهروند دسترسی ندارید");
             

            return await _citzene.UpdateSabtStatus(model, userId);

        }



        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("CitizenAuthenticationAfterUpdateIdentityInformation")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task CitizenAuthenticationAfterUpdateIdentityInformation(int citizenId, SmsOption smsOption)
        {
           
            var getcitizen = await _citzene.GetShortCitizenInfo(citizenId);

            if (!getcitizen.IsSuccess)
            { 
                return ;
            }

            if (getcitizen.Data.SabtStatus == Common.GlobalEnum.SabtStatusEnum.تایید)
            { 
                return  ;
            }
            if (getcitizen.Data.Nationality != 0)
            {
                return;
            }
            var userId = _usersService.GetCurrentUserId();
            var call = new ItsaazApi(_cache);
            var token =await call.AutenticationApi();
            if (string.IsNullOrWhiteSpace(token))
            {

              
                await _event.AddEvent(new ViewModel.Events.EventDto()
                {
                    Description = "خطا در دریافت توکن سرویس استعلام" + call.ErrorMessage,
                    UserId = userId,
                    ActionName = "CitizenAuthenticationAfterUpdateIdentityInformation",
                    EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                    EventSection = Common.GlobalEnum.EventSectionEnum.خطای_سیستمی,
                    EventType = Common.GlobalEnum.EventTypeEnum.Error,
                    OperationId = userId
                });
                return;
            }

            try
            {
                DateTimeToPersianDateTimeConverter persianDate = new DateTimeToPersianDateTimeConverter("/", false);
                SendSms sms = new SendSms();
                var date = persianDate.toShamsiDateTime(getcitizen.Data.BirthDate.Value);
                var getCitizenInfo = await call.GetData(new GetDataParam() { BirthDate = date, Token = token, NationalCode = getcitizen.Data.NationCode });
                if (getCitizenInfo != null)
                {
                    getCitizenInfo.birthDate = date;
                    getCitizenInfo.nationalCode = getcitizen.Data.NationCode;
                    await _citzene.UpdateCitizenAfterAuthentication(getCitizenInfo,userId);

                    if ( getCitizenInfo.isMatch==false)
                    {
                        //ارسال پیامک به شهروندی بابت عدم صحت تاریخ تولد
                        if(smsOption.SendSmsAfterRejectCitizenInformationInUpdateForm)
                        {
                            var name = "کدملی"+ getcitizen.Data.NationCode; 
                            var mobile = getcitizen.Data.MobileNumber;
                            if (!string.IsNullOrWhiteSpace(mobile))
                            {
                                var smsLog = await sms.VerifyLookup(mobile, "کدملی", "", "", name, TempleteNameEnum.smsinfoupdate, smsOption.SmsToken);
                                if (smsLog.IsSuccess)
                                {
                                    smsLog.Data.Token2 = getcitizen.Data.NationCode;//دخیره کد ملی 
                                    var saveSms = await _smsInfoService.Add(smsLog.Data, getcitizen.Data.CitizenId);
                                }

                            }
                        }
                    }
                }
                else
                {
                   
                    await _event.AddEvent(new ViewModel.Events.EventDto()
                    {
                        Description = " خطا  در استعلام " + call.ErrorMessage,
                        UserId = userId,
                        ActionName = "CitizenAuthenticationAfterUpdateIdentityInformation",
                        EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                        EventSection = Common.GlobalEnum.EventSectionEnum.خطای_سیستمی,
                        EventType = Common.GlobalEnum.EventTypeEnum.Error,
                        OperationId = userId
                    });
                    return;


                }




            }
            catch (Exception er)
            {
                
            }
            return  ;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("CitizenAuthenticationAfterUpdateIdentityInformationByAdmin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task CitizenAuthenticationAfterUpdateIdentityInformationByAdmin(string  userCode, SmsOption smsOption)
        {

            var getcitizen = await _citzene.GetShortCitizenInfo(userCode);
           
            if (!getcitizen.IsSuccess)
            {
                return;
            }

            if (getcitizen.Data.SabtStatus == Common.GlobalEnum.SabtStatusEnum.تایید)
            {
                return;
            }
            if (getcitizen.Data.Nationality != 0)
            {
                return;
            }
            var userId = _usersService.GetCurrentUserId();
            var citizen = getcitizen.Data;
            var call = new ItsaazApi(_cache);
            var token =await call.AutenticationApi();
            if (string.IsNullOrWhiteSpace(token))
            {
               
                await _event.AddEvent(new ViewModel.Events.EventDto()
                {
                    Description = "خطا در دریافت توکن سرویس استعلام" + call.ErrorMessage,
                    UserId = citizen.CitizenId,
                    ActionName = "CitizenAuthenticationAfterUpdateIdentityInformation",
                    EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                    EventSection = Common.GlobalEnum.EventSectionEnum.خطای_سیستمی,
                    EventType = Common.GlobalEnum.EventTypeEnum.Error,
                    OperationId = userId
                });
                return;
            }

            try
            {
                DateTimeToPersianDateTimeConverter persianDate = new DateTimeToPersianDateTimeConverter("/", false);
                SendSms sms = new SendSms();
                var date = persianDate.toShamsiDateTime(citizen.BirthDate.Value);
                var getCitizenInfo = await call.GetData(new GetDataParam() { BirthDate = date, Token = token, NationalCode = getcitizen.Data.NationCode });
                if (getCitizenInfo != null)
                {
                    getCitizenInfo.birthDate = date;
                    getCitizenInfo.nationalCode = getcitizen.Data.NationCode;
                    await _citzene.UpdateCitizenAfterAuthentication(getCitizenInfo, userId);

                    if ( getCitizenInfo.isMatch==false)
                    {
                        //ارسال پیامک به شهروندی بابت عدم صحت تاریخ تولد
                        if (smsOption.SendSmsAfterRejectCitizenInformationInUpdateForm)
                        {
                            var name = "کدملی" + getcitizen.Data.NationCode;
                            var mobile = getcitizen.Data.MobileNumber;
                            if (!string.IsNullOrWhiteSpace(mobile))
                            {
                                var smsLog = await sms.VerifyLookup(mobile, "کدملی", "", "", name, TempleteNameEnum.smsinfoupdate, smsOption.SmsToken);
                                if (smsLog.IsSuccess)
                                {
                                    smsLog.Data.Token2 = getcitizen.Data.NationCode;//دخیره کد ملی 
                                    var saveSms = await _smsInfoService.Add(smsLog.Data, getcitizen.Data.CitizenId);
                                }

                            }
                        }
                    }
                }

                else
                {
                   
                    await _event.AddEvent(new ViewModel.Events.EventDto()
                    {
                        Description = "خطا  در استعلام" + call.ErrorMessage,
                        UserId = userId,
                        ActionName = "CitizenAuthenticationAfterUpdateIdentityInformation",
                        EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                        EventSection = Common.GlobalEnum.EventSectionEnum.خطای_سیستمی,
                        EventType = Common.GlobalEnum.EventTypeEnum.Error,
                        OperationId = userId
                    });
                    return;


                }

            }
            catch (Exception er)
            {

            }
            return;
        }


        /// <summary>
        /// ویرایش سایر اطلاعات توسط خود شهروند
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Citizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<string>> UpdateOtherBaseInfoByCitizen(EditOtherBaseInfoViewModel model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _citzene.UpdateOtherBaseInfoByCitizen(model, userId);

        }


        /// <summary>
        /// ویرایش اطلاعات پایه شهروند توسط مدیر
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<string>> UpdateCitizenByAdmin(EditCitizenViewModel model)
        {

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.ویرایش_اطلاعات_شهروند);
            if (!canAccess.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, null, "شما به ویرایش اطلاعات شهروند دسترسی ندارید");



            return await _citzene.UpdateCitizenByAdmin(model, userId);

        }

        [Authorize(Roles = "WebApiUser")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<string>> UpdateCitizen(EditCitizenViewModel model)
        {
           

            var webApiuserId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(webApiuserId, Common.GlobalEnum.PermissionTypeEnum.ویرایش_اطلاعات_شهروندی);
            if (!canAccess.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, "", "کاربر توسعه دهنده به این سرویس دسترسی ندارد");

             



            return await _citzene.UpdateCitizenByWebApiUser(model, webApiuserId);

        }



       








        /// <summary> 
        /// بارگذاری تصویر پرسنلی شهروند
        ///  [C8] 
        /// </summary>
        /// <param name="file"> 
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadPersonalPicture")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize(Roles = "Citizen")]
        public async Task<ApiResult<UploadFileResult>> UploadPersonalPicture(IFormFile file)
        {
            var response = new ApiResult<UploadFileResult>(false, ApiResultStatusCode.Success, new UploadFileResult());
            try
            {
                var user = await _usersService.GetCurrentUserAsync();
                var filesPath = _environment.WebRootPath + "/uploads/Resources/Citizens/" + user.UserCode.ToString();
                if (!System.IO.Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }
                if (file.Length > 0)
                {
                    if(file.Length> 204800)
                    {
                        response.IsSuccess = false;
                        response.Messages =  " حجم تصویر بیش از حد مجاز می باشد ";
                        return response;

                    }


                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    string[] supportedTypes = new string[3] { ".jpg", ".jpeg", ".png" };
                    if (!((IEnumerable<string>)supportedTypes).Contains<string>(extension.ToLower()))
                    {
                        response.IsSuccess = false;
                        response.Messages = "  پسوند تصویر ارسالی معتبر نمی باشد";
                        return response;
                    }



                    var vFileName = user.Username + ".jpg";
                    var path = "/uploads/Resources/Citizens/" + user.UserCode + $@"/{ vFileName}";
                    string FilePath = _environment.WebRootPath + path;
                    //var   vFilePath = Path.Combine(path, vFileName);
                    if (System.IO.File.Exists(FilePath))
                    {
                        System.IO.File.Delete(FilePath);
                    }
                    using (FileStream fs = System.IO.File.Create(FilePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    await _citzene.ChangeCitizenPicture(user.Id); //تصویر در حال بررسی شهروند
                    response.IsSuccess = true;
                    response.Data.UploadUrl = path;
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Messages = "خطا در آپلود فایل!";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = $"خطا در آپلود فایل: {ex}";
            }
            return response;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [Route("CopyPicture")]
        public async Task<ApiResult> CopyPicture()
        {
            //
            var res = new ApiResult(true, ApiResultStatusCode.Success, "");
            var msg = "";
            var count = 0;
            var notFound = 0;
            var errorCount = 0;
            //wwwroot\uploads\Resources\forcopyImageToServer\images
            string importPath = "/Uploads/Resources/forcopyImageToServer/images/";


            DirectoryInfo d = new DirectoryInfo(_environment.WebRootPath + importPath);
            var filePaths = d.GetFiles("*.jpg");

            if (filePaths.Any())
            {
                msg += " تعداد تصاویر یافت شده برابر است با  " + filePaths.Length.ToString();
                foreach (var file in filePaths)
                {
                    try
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file.FullName);
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            fileName = fileName.Fa2En();
                            var citizenInfo = await _citzene.GetShortCitizenInfoByNationCode(fileName);
                            if (citizenInfo.IsSuccess)
                            {

                                var filesPath = _environment.WebRootPath + "/uploads/Resources/Citizens/" + citizenInfo.Data.UserCode.ToString();
                                if (!System.IO.Directory.Exists(filesPath))
                                {
                                    Directory.CreateDirectory(filesPath);
                                }
                                var vFileName = citizenInfo.Data.NationCode + ".jpg";
                                var path = "/uploads/Resources/Citizens/" + citizenInfo.Data.UserCode.ToString() + $@"/{ vFileName}";
                                if (System.IO.File.Exists(_environment.WebRootPath + path))
                                {
                                    System.IO.File.Delete(_environment.WebRootPath + path);
                                }
                                file.CopyTo(_environment.WebRootPath + path);
                                await _citzene.ChangeCitizenPicture(citizenInfo.Data.CitizenId);
                                file.Delete();
                                count++;
                            }
                            else
                            {
                                notFound++;
                            }

                        }
                    }
                    catch (Exception er)
                    {
                        errorCount++;
                        continue;

                    }
                }

            }
            else
            {
                res.Messages = "تصویری در سرور یافت نشد";
                res.IsSuccess = false;
                return res;

            }
            msg += " تعداد خطای اجرا  " + errorCount + " خطا  ";
            msg += " تعداد شهروند یافت نشده  " + notFound + " شهروند  ";
            res.Messages = msg + "   " + "تعداد تصاویر منتقل شده برابر است با " + count.ToString() + " تصویر ";

            return res;

        }




        #endregion
        #region ویرایش اطلاعات کارت بانکی شهروند
        /// <summary>
        /// ثبت اطلاعات بانکی شهروند
        ///  [C9]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Operator,Citizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<string>> UpdateCitizenBankCardNumber(UpdateBankAccountCardNumberDto model)
        {
            if (model.CitizenId == null)
                model.CitizenId = _usersService.GetCurrentUserId();
            return await _citzene.UpdateCitizenBankCardNumber(model);

        }

        

        [HttpGet]
        [Route("GetCitizenBankCardNumber")]
        [Authorize(Roles = "Citizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<BankAccountCardNumberDto>> GetCitizenBankCardNumber()
        {
            int citizenId = this._usersService.GetCurrentUserId();
            ApiResult<BankAccountCardNumberDto> citizenBankCardNumber = await this._citzene.GetCitizenBankCardNumber(citizenId);
            return citizenBankCardNumber;
        }





        #endregion
        #region تکمیل اطلاعات شهروند
        /// <summary>
        /// ثبت یا ویرایش اطلاعات تکمیلی شهروند
        /// [C11]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Citizen")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> AddOrUpdateCitizenProfile(CitizenProfileDto model)
        {
            var citizenId = _usersService.GetCurrentUserId();
            return await _citzene.AddOrUpdateCitizenProfile(model, citizenId);
        }

        /// <summary>
        /// ثبت اطلاعات تکمیلی توسط شهروند
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> AddOrUpdateCitizenProfileByAdmin(CitizenProfileDto model)
        {
            if (model == null)
                return new ApiResult(false, ApiResultStatusCode.BadRequest, "مدل ورودی معتبر نمی باشد.");

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.ویرایش_اطلاعات_شهروند);
            if (!canAccess.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, null, "شما به ویرایش اطلاعات شهروند دسترسی ندارید");





            return await _citzene.AddOrUpdateCitizenProfileByAdmin(model);
        }


        /// <summary>
        /// دریافت اطلاعات تکمیلی شهروند
        /// [C12]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetCitizenProfileByCitizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CitizenProfileInfo>> GetCitizenProfileByCitizen()
        {
            var userId = _usersService.GetCurrentUserId();
            return await _citzene.GetCitizenProfile(userId);

        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetCitizenProfileByAdmin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CitizenProfileInfo>> GetCitizenProfileByAdmin(string   userCode)
        { 
            return await _citzene.GetCitizenProfile(userCode); 
        }







        /// <summary>
        /// دریافت اطلاعات تکمیلی شهروند
        /// </summary>
        /// <param name="userCode">شناسه کاربری</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "WebApiUser")]
        [Route("GetCitizenProfileByService")]
        public async Task<ApiResult<CitizenProfileInfo>> GetCitizenProfileByService(string userCode)
        {
            return await _citzene.GetCitizenProfile(userCode);
        }



        [Authorize(Roles = "WebApiUser")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult> UpdateCitizenProfileByService(CitizenProfileDto model)
        {
            if (model == null)
                return new ApiResult(false, ApiResultStatusCode.BadRequest, "مدل ورودی معتبر نمی باشد.");
            
            return await _citzene.AddOrUpdateCitizenProfileByAdmin(model);
        }















        [HttpGet]
        [Authorize(Roles = "WebApiUser")]
        [Route("GetShortCitizenInfo")]
        public async Task<ApiResult<ShortCitizenInfo>> GetShortCitizenInfo(int  userId)
        {
            
            var profile = await _citzene.GetShortCitizenInfo(userId);
            if (profile.IsSuccess)
            {
                var vFileName = profile.Data.NationCode + ".jpg";
                var path = "/uploads/Resources/Citizens/" + profile.Data.UserCode.ToString() + $@"/{ vFileName}";
                string FilePath = _environment.WebRootPath + path;
                if (System.IO.File.Exists(FilePath))
                {
                    profile.Data.PersonalPictureIsUploaded = true;
                    profile.Data.PersonalPictureUrl = path;
                }
                else
                {
                    profile.Data.PersonalPictureUrl = "assets/images/avatar.png";
                }
            }
            return profile;

        }
        /// <summary>
        /// دریافت کامل اطلاعات شهروند به همراه سوابق
        /// </summary>
        /// <param name="userCode">شناسه  کاربری شهروند</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "WebApiUser")]
        [Route("GetCitizenFullInfoByService")]
        public async Task<ApiResult<CitizenFullInfo>> GetCitizenFullInfoByService(string  userCode)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.دریافت_اطلاعات_کامل_شهروند);
            if (!canAccess.IsSuccess)
                return new ApiResult<CitizenFullInfo>(false, ApiResultStatusCode.NotAllowed, null, "کاربر توسعه دهنده به این سرویس دسترسی ندارد");
            return await _citzene.GetCitizenFullInfoByUserCode(userCode);
        }






        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetShortCitizenInfoByAdmin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<ShortCitizenInfo>> GetShortCitizenInfoByAdmin(string userCode)
        {
          
            var profile = await _citzene.GetShortCitizenInfo(userCode);
            if (profile.IsSuccess)
            {
                var vFileName = profile.Data.NationCode + ".jpg";
                var path = "/uploads/Resources/Citizens/" + profile.Data.UserCode.ToString() + $@"/{ vFileName}";
                string FilePath = _environment.WebRootPath + path;
                if (System.IO.File.Exists(FilePath))
                {
                    profile.Data.PersonalPictureIsUploaded = true;
                    profile.Data.PersonalPictureUrl = path;
                }
                else
                {
                    profile.Data.PersonalPictureUrl = "assets/images/avatar.png";
                }
            }
            return profile;

        }



        [HttpGet]
        [Authorize(Roles = "Card")]
        [Route("GetShortCitizenInfoByCard")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<ShortCitizenInfo>> GetShortCitizenInfoByCard(string userCode)
        {

            var profile = await _citzene.GetShortCitizenInfo(userCode);
            if (profile.IsSuccess)
            {
                var vFileName = profile.Data.NationCode + ".jpg";
                var path = "/uploads/Resources/Citizens/" + profile.Data.UserCode.ToString() + $@"/{ vFileName}";
                string FilePath = _environment.WebRootPath + path;
                if (System.IO.File.Exists(FilePath))
                {
                    profile.Data.PersonalPictureIsUploaded = true;
                    profile.Data.PersonalPictureUrl = path;
                }
                else
                {
                    profile.Data.PersonalPictureUrl = "assets/images/avatar.png";
                }
            }
            return profile;

        }







        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetShortCitizenInfoByCitizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<ShortCitizenInfo>> GetShortCitizenInfoByCitizen()
        {
            var  userId = _usersService.GetCurrentUserId();
            var profile = await _citzene.GetShortCitizenInfo(userId);
            if (profile.IsSuccess)
            {
                var vFileName = profile.Data.NationCode + ".jpg";
                var path = "/uploads/Resources/Citizens/" + profile.Data.UserCode.ToString() + $@"/{ vFileName}";
                string FilePath = _environment.WebRootPath + path;
                if (System.IO.File.Exists(FilePath))
                {
                    profile.Data.PersonalPictureIsUploaded = true;
                    profile.Data.PersonalPictureUrl = path;
                }
                else
                {
                    profile.Data.PersonalPictureUrl = "assets/images/avatar.png";
                }
            }
            return profile;

        }




        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetMyFullInfo")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CitizenFullInfo>> GetMyFullInfo()
        {
            var userId = _usersService.GetCurrentUserId();
            return await _citzene.GetCitizenFullInfo(userId);
        }





        /// <summary>
        /// دریافت اطلاعات کوتاه شهروندبه وسیله شناسه استعلام اطلاعات
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "WebApiUser")]
         //[AllowAnonymous]
        [Route("GetShortCitizenInfoByTicket")]
        public async Task<ApiResult<ShortCitizenInfo>> GetShortCitizenInfoByTicket(string ticket)
        {
            var webApiuserId =   _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(webApiuserId, Common.GlobalEnum.PermissionTypeEnum.دریافت_اطلاعات_شهروند_به_وسیله_توکن);
            if (!canAccess.IsSuccess)
                return new ApiResult<ShortCitizenInfo>(false, ApiResultStatusCode.NotAllowed, new ShortCitizenInfo(), "کاربر توسعه دهنده به این سرویس دسترسی ندارد");



            return await _citzene.GetShortCitizenInfoByTicket(ticket);
        }






        /// <summary>
        /// دریافت اطلاعات کامل شهروندی به وسیله شناسه کاربری
        /// </summary>
        /// <param name="userCode">شناسه کاربری</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("GetCitizenFullInfo")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CitizenFullInfo>> GetCitizenFullInfo(string userCode)
        {
            return await _citzene.GetCitizenFullInfoByUserCode(userCode);
        }

        #endregion
        #region چک کردن ثبت نام شهروند در سامانه
        /// <summary>
        /// چک کردن اینکه شهروند در سامانه ثبت نام کرده است یا نه
        /// </summary>
        /// <param name="model"></param>
        /// <returns>نتیجه ثبت نام و تاریخ ثبت نام</returns>
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<IsCitizenIsRegisterResult>> CheckRegisterCitizenByNtionCode(IsCitizenIsRegister model)
        {
            return await _citzene.CheckRegisterCitizenByNtionCode(model);
        }






        #endregion 
        #region خانواده شهروند
        /// <summary>
        ///  دریافت لیست خانواده شهروند
        ///  [C] 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetAllCitizenFamily")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<List<CitizenFamiliesInfo>>> GetAllCitizenFamily()
        {
            var userId = _usersService.GetCurrentUserId();
            return await _family.GetAllCitizenFamily(userId);

        }

        /// <summary>
        /// نمایش اطلاعات شهروند به همراه اعضای خانواده شهروند
        /// </summary>
        /// <param name="userCode">شناسه شهروند</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetAllCitizenFamilyByAdmin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CitizenAndFamilyList>> GetAllCitizenFamilyByAdmin(string  userCode)
        {
            return await _family.GetAllCitizenFamilyByAdmin(userCode);

        }


        /// <summary>
        /// اضافه کردن عضو جدید خانواده
        /// در صورتیکه عضو جدید قبلا در سامانه ثبت نام کرده باشد.
        /// [C] 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Citizen,Admin")]
        public async Task<ApiResult> AddFamilyMemberIfAny(CheckFamilyModel model)
        {
            var citizenId = _usersService.GetCurrentUserId();
            return await _family.AddFamily(model, citizenId);
        }

        /// <summary>
        /// دریافت جزئیات یک نسبت خانوادگی 
        /// </summary>
        /// <param name="familyId">شناسه شهروندی عضو</param>
        /// <param name="citizenId">شناسه شهروند خانواده</param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Citizen,Admin")]
        public async Task<ApiResult<CitizenFamiliesInfo>> GetCitizenFamily(int familyId, int? citizenId)
        {
            if (citizenId == null) citizenId = _usersService.GetCurrentUserId();
            return await _family.GetCitizenFamily(familyId, citizenId.Value);
        }

        /// <summary>
        /// اضافه کردن اطلاعات شهروند در صورتیکه نسبت قبلا ثبت نام نکرده باشد
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Citizen")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> AddFamilyMemberIfNotAny(AddFamilyCitizenDto model)
        {
            //ثبت اطلاعات حساب کاربری
            //ثبت اطلاعات شهروند
            //ثبت اطلاعات پروفایل
            //ثبت رابطه خانوادگی 
            var userId = _usersService.GetCurrentUserId();
            return await _family.AddFamilyMemberIfNotAny(model, userId);

        }


        /// <summary>
        /// ویرایش نسبت خانوادگی
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Citizen")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> UpdateFamilyMemberByCitizen(UpdateFamilyCitizenDto model)
        {

            if (model == null)
                return new ApiResult(false, ApiResultStatusCode.BadRequest, "مدل ورود نامعتبر می باشد");
            var userId = _usersService.GetCurrentUserId();
            return await _family.UpdateFamilyMemberByCitizen(model, userId);

        }


        /// <summary>
        /// حذف نسبت خانوادگی
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> RemoveFamily(DeleteFamilly model)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.خانواده_شهروند);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult(false, ApiResultStatusCode.NotAllowed, "شما به این قسمت دسترسی ندارید");
            }

            return await _family.Remove(model); 
        }


        /// <summary>
        ///  حذف عضو خانواده توسط خود شهروند
        /// </summary>
        /// <param name="id">شناسه عضو   خانواده</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("RemoveFamilyByCitizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> RemoveFamilyByCitizen(int id)
        { 
            var citizenId = _usersService.GetCurrentUserId();
            return await _family.RemoveByCitizen(id, citizenId);

        }



        /// <summary>
        /// تایید نسبت خانوادگی توسط شهروند
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Citizen")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> AcceptFamilyByCitizen(ReviewFamily model)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _family.AcceptFamilyByCitizen(model, userId);

        }


        /// <summary>
        /// تایید نسبت خانوادگی توسط
        ///  مدیر سامانه
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> ConfirmFamilyByAdmin(ConfirmFamily model)
        {

            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.خانواده_شهروند);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult(false, ApiResultStatusCode.NotAllowed,   "شما به این قسمت دسترسی ندارید");
            }

            return await _family.ConfirmFamilyByAdmin(model, userId);

        }

        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetMyFamilyBaseInfo")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CitizenBaseInfo>> GetMyFamilyBaseInfo(int familyId)
        {
            var myuserId = _usersService.GetCurrentUserId();
            return await _citzene.GetMyFamilyBaseInfo(myuserId, familyId);

        }


        /// <summary>
        /// جستجوی خانواده شهروند
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="name"></param>
        /// <param name="nationCode"></param>
        /// <param name="familyRelation"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchFamilyCitizens")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<PagedFamilyCitizenViewModel>> SearchFamilyCitizens(
            int? offset = 1, int? count = 20,
             DateTime? FromDate = null,
          DateTime? ToDate = null,
          string name = null,
          string nationCode = null,
           FamilyRelationshipsEnum? familyRelation = null,
            int? groupId = null
            )
        {


            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.خانواده_شهروند);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<PagedFamilyCitizenViewModel>(false, ApiResultStatusCode.NotAllowed,null, "شما به این قسمت دسترسی ندارید");
            }





            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;







            return await _family.SearchFamilyCitizens(offset.Value, count.Value, FromDate, ToDate,
                name, nationCode, familyRelation, groupId);

        }







        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetAllCitizenFamilyByFamily")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CitizenAndFamilyList>> GetAllCitizenFamilyByFamily()
        {
            var citizenId = _usersService.GetCurrentUserId();
            return await _family.GetAllCitizenFamilyByFamily(citizenId);

        }




        #endregion
        #region سوابق تحصیلی شهروند
        /// <summary>
        /// دریافت سابقه تحصیلی
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns> 
        [HttpGet]
        [Route("GetAllEducationByAdmin")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<List<karjoEducationDto>>> GetAllEducationByAdmin(int userId)
        {
            return await _educationService.GetAllEducation(userId);
        }


        [HttpGet]
        [Route("GetAllEducationByCitizen")]
        [Authorize(Roles = "Citizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<List<karjoEducationDto>>> GetAllEducationByCitizen()
        {
            var    userId = _usersService.GetCurrentUserId();
            return await _educationService.GetAllEducation(userId);

        }


        /// <summary>
        /// جزیئات یک سابقه تحصیلی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEducation")]
        [Authorize(Roles = "Citizen,Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<karjoEducationDto>> GetEducation(int id)
        {

            return await _educationService.GetItem(id);
        }

        /// <summary>
        /// دخیره سابقه تحصیلی
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Citizen,Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<karjoEducationDto>> SaveEducation(karjoEducationDto model)
        {
            if (model.CitizenId == null)
                model.CitizenId = _usersService.GetCurrentUserId();

            return await _educationService.AddOrUpdate(model, model.CitizenId.Value);
        }


        /// <summary>
        /// حذف سابقه تحصیلی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpDelete("[action]")]
        [Authorize(Roles = "Citizen,Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<string>> DeleteEducation(int id)
        {
            return await _educationService.Remove(id);
        }

        #endregion 
        #region جستجوی شهروند
        /// <summary>
        /// جستجوی شهروند
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate">تاریخ ثبت نام</param>
        /// <param name="ToDate">تا</param>
        /// <param name="name">نام یا نام خانوادگی</param>
        /// <param name="nationCode">کد ملی</param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchCitizens")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<PagedCitizenViewModel>> SearchCitizens(
            int? offset = 1, int? count = 20,
             DateTime? FromDate = null,
          DateTime? ToDate = null,
          string name = null,
          string nationCode = null,
          int? groupId = null
            )
        {

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.جستجوی_شهروند);
            if (!canAccess.IsSuccess)
                return new ApiResult<PagedCitizenViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به جستجوی شهروند دسترسی ندارید");



            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _citzene.SearchCitizens(offset.Value, count.Value, FromDate, ToDate,
                name, nationCode, groupId);

        }


        [HttpGet]
        [Route("SearchCitizenByCardUser")]
        [Authorize(Roles = "Admin,Card")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<ShortCitizenInfo>> SearchCitizenByCardUser(
          string nationCode,  
          DateTime? birthDate = null 
           )
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.جستجوی_شهروند_کارت);
            if (!canAccess.IsSuccess)
                return new ApiResult<ShortCitizenInfo>(false, ApiResultStatusCode.NotAllowed, null, "شما به جستجوی شهروند دسترسی ندارید");


            return await _citzene.SearchCitizenByCardUser( 
               nationCode,
                birthDate); 
        }




 
       
        [HttpGet]
        [Route("CitizenAdvancedSearch")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<PagedCitizenViewModel>> CitizenAdvancedSearch(
            int? offset = 1, int? count = 20,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          DateTime? birthDateFromDate = null,
          DateTime? birthDateToDate = null,
          string name = null,
           string lastname = null,
          string nationCode = null,
          string mobile = null,
          bool? gender = null,
          int? groupId = null,
          bool? hasFamily = null,
          bool? faceAuthentication = null,
          int? nationality = null,
          int? cityId = null, 
          int? region = null,
          SabtStatusEnum? sabtStatus = null,
          PersonalPictureEnum? pictureConfirmed = null,
          MaritalStatusEnum? mariageStatus = null,
          int? registerByService = null
            )
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.جستجوی_پیشرفته_شهروند);
            if (!canAccess.IsSuccess)
                return new ApiResult<PagedCitizenViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به جستجوی شهروند دسترسی ندارید");



            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _citzene.advancedSearch(offset.Value, count.Value, FromDate, ToDate, birthDateFromDate,
                birthDateToDate,
                name, lastname, nationCode, mobile, gender, nationality, cityId, null, region, sabtStatus, pictureConfirmed
                , mariageStatus, registerByService, groupId, hasFamily, faceAuthentication);

        }

        [HttpGet]
        [Route("CitizenAdvancedSearch_Export")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> CitizenAdvancedSearch_Export(
            int? offset = 1, int? count = 20,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          DateTime? birthDateFromDate = null,
          DateTime? birthDateToDate = null,
          string name = null,
           string lastname = null,
          string nationCode = null,
          string mobile = null,
          bool? gender = null,
          int? groupId = null,
          bool? hasFamily = null,
          bool? faceAuthentication = null,
          int? nationality = null,
          int? cityId = null,
          int? region = null,
          SabtStatusEnum? sabtStatus = null,
          PersonalPictureEnum? pictureConfirmed = null,
          MaritalStatusEnum? mariageStatus = null,
          int? registerByService = null
            )
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.جستجوی_پیشرفته_شهروند);
            if (!canAccess.IsSuccess)
                return BadRequest(new ApiResult(false, ApiResultStatusCode.NotAllowed, "شما به جستجوی پیشرفته شهروندان دسترسی ندارید"));

         canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.دریافت_خروجی_اکسل_شهروندان);
            if (!canAccess.IsSuccess)
                return BadRequest(  "شما به خروجی اکسل دسترسی ندارید" );
               // return StatusCode(StatusCodes.Status406NotAcceptable, new { message = "Invalid username or password" });


            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            var getlist = await _citzene.advancedSearch(offset.Value, count.Value, FromDate, ToDate, birthDateFromDate,
                birthDateToDate,
                name, lastname, nationCode, mobile, gender, nationality, cityId, null, region, sabtStatus, pictureConfirmed
                , mariageStatus, registerByService, groupId, hasFamily, faceAuthentication);

            var errorLine = 0;

            if (getlist.IsSuccess != true)
                return BadRequest(getlist.Messages);
            
            var userIp = Request.HttpContext.Connection.RemoteIpAddress;
            await _event.AddEvent(new ViewModel.Events.EventDto()
            {
               ActionName= "CitizenAdvancedSearch_Export",
               Code= 0,
               Description="دریافت خروجی اکسل از اطلاعات شهروندان",
               EventPriority=EventPriorityEnum.Important,
               EventSection=EventSectionEnum.خروجی_اکسل_شهروندان,
               EventType=EventTypeEnum.Warning,
               StrCode="",
               UserId= userId,
               OperationId = userId,
               UserIp= userIp.ToString()

            });


            //ثبت لاگ دریافت فایل اکسل
            try
            {
                var data = getlist.Data.Citizens;

                var workbook = new HSSFWorkbook();
                var sheet = workbook.CreateSheet();

                //(Optional) set the width of the columns
                sheet.SetColumnWidth(0, 20 * 100);//row
                sheet.SetColumnWidth(1, 20 * 100);//Gender
                sheet.SetColumnWidth(2, 20 * 100);//NationCode
                sheet.SetColumnWidth(3, 20 * 100);//FirstName
                sheet.SetColumnWidth(4, 20 * 100);//LastName
                sheet.SetColumnWidth(5, 20 * 100);//FatherName
                sheet.SetColumnWidth(6, 20 * 100);//BirthDate 
                sheet.SetColumnWidth(7, 20 * 100);//Age 
                sheet.SetColumnWidth(8, 20 * 100);//MobileNumber  
                sheet.SetColumnWidth(9, 20 * 100);//CreationDate
                sheet.SetColumnWidth(10, 20 * 100);//RegisterByService
                sheet.SetColumnWidth(11, 20 * 100);//SabtStatus 
                sheet.SetColumnWidth(12, 20 * 100);//PersonalPicture_Confirmed
                sheet.SetColumnWidth(13, 20 * 100);// Nationality 
                sheet.SetColumnWidth(14, 20 * 100);//Groups
                sheet.SetColumnWidth(15, 20 * 100);//FullAddress
                sheet.SetColumnWidth(16, 20 * 400);// PostalCode
                sheet.SetColumnWidth(17, 20 * 100);//LastUpdateOnDate

                // Gender  NationCode  FirstName  LastName  FatherName BirthDate Age
                // MobileNumber CreationDate RegisterByService SabtStatus
                // PersonalPicture_Confirmed Nationality Groups FullAddress PostalCode LastUpdateOnDate




                //Create a header row
                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("ردیف");
                headerRow.CreateCell(1).SetCellValue("جنسیت");
                headerRow.CreateCell(2).SetCellValue("کد ملی");
                headerRow.CreateCell(3).SetCellValue("نام");
                headerRow.CreateCell(4).SetCellValue("نام خانوادگی");
                headerRow.CreateCell(5).SetCellValue("نام پدر");
                headerRow.CreateCell(6).SetCellValue("تاریخ تولد");
                headerRow.CreateCell(7).SetCellValue("سن");
                headerRow.CreateCell(8).SetCellValue("شماره تماس");
                headerRow.CreateCell(9).SetCellValue("تاریخ ثبت نام");
                headerRow.CreateCell(10).SetCellValue("ثبت نام در سرویس");
                headerRow.CreateCell(11).SetCellValue("وضعیت ثبت احوال ");
                headerRow.CreateCell(12).SetCellValue("وضعیت تایید تصویر  ");
                headerRow.CreateCell(13).SetCellValue("ملیت");
                headerRow.CreateCell(14).SetCellValue("گروههای شهروندی");
                headerRow.CreateCell(15).SetCellValue("آدرس کامل");
                headerRow.CreateCell(16).SetCellValue("کد پستی");
                headerRow.CreateCell(17).SetCellValue(" تاریخ بروزرسانی  ");

                // Gender  NationCode  FirstName  LastName  FatherName BirthDate Age
                // MobileNumber CreationDate RegisterByService SabtStatus
                // PersonalPicture_Confirmed Nationality Groups FullAddress PostalCode LastUpdateOnDate


                //(Optional) freeze the header row so it is not scrolled
                sheet.CreateFreezePane(0, 1, 0, 1);

                int rowNumber = 1;
                DateTimeToPersianDateTimeConverter pdate = new DateTimeToPersianDateTimeConverter("-", false);

                foreach (var obj in data)
                {
                    errorLine = 1;
                    var row = sheet.CreateRow(rowNumber++);
                    row.CreateCell(0).SetCellValue(rowNumber);
                    row.CreateCell(1).SetCellValue(obj.Gender);
                    row.CreateCell(2).SetCellValue(obj.NationCode);
                    row.CreateCell(3).SetCellValue(obj.FirstName);
                    row.CreateCell(4).SetCellValue(obj.LastName);
                    row.CreateCell(5).SetCellValue(obj.FatherName);
                    errorLine = 2;
                    if (obj.BirthDate != null)
                    {
                        row.CreateCell(6).SetCellValue(pdate.toShamsiDateTime(obj.BirthDate.Value));
                    }
                    else
                    {
                        row.CreateCell(6).SetCellValue("");
                    }
                    errorLine = 4;
                    row.CreateCell(7).SetCellValue(obj.Age.ToString());
                    row.CreateCell(8).SetCellValue(obj.MobileNumber);
                    row.CreateCell(9).SetCellValue(pdate.toShamsiDateTime(obj.CreationDate));
                    errorLine = 9;
                    row.CreateCell(10).SetCellValue(obj.RegisterByService);
                    row.CreateCell(11).SetCellValue(obj.SabtStatus.ToString());
                    errorLine = 10;
                    if (obj.PersonalPicture_Confirmed != null)
                    {
                        row.CreateCell(12).SetCellValue(obj.PersonalPicture_Confirmed.ToString());
                    }
                    else
                    {
                        row.CreateCell(12).SetCellValue("");
                    }

                    errorLine = 20;
                    row.CreateCell(13).SetCellValue(obj.Nationality);
                    row.CreateCell(14).SetCellValue(String.Join(", ", obj.Groups));
                    errorLine = 22;
                    row.CreateCell(15).SetCellValue(obj.FullAddress);
                    row.CreateCell(16).SetCellValue(obj.PostalCode);

                    if (obj.LastUpdateOnDate != null)
                    {
                        row.CreateCell(17).SetCellValue(pdate.toShamsiDateTime(obj.LastUpdateOnDate.Value));
                    }
                    else
                    {
                        row.CreateCell(17).SetCellValue("");
                    }
                    errorLine = 50;


                }
                errorLine = 100;
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel", //MIME type of Excel files
                 "allCitizens.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user


            }
            catch (Exception er)
            {

                return BadRequest(er.Message+" "+ errorLine);
            }

          
        }

        #endregion  
        #region بازبینی تصاویر شهروندی
        /// <summary>
        /// جستجوی تصاویر شهروندان
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="pictureConfirmed"></param>
        /// <param name="nationCode"></param>
        /// <param name="hasCard"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchImageCitizens")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<PagedImageCitizenViewModel>> SearchImageCitizens(
           int? offset = 1, int? count = 20,
            DateTime? FromDate = null,
         DateTime? ToDate = null,
         PersonalPictureEnum? pictureConfirmed = null,
         string nationCode = null,
         bool? hasCard = null,
         int? groupId = null
           )
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.بازبینی_تصاویر);
            if (!canAccess.IsSuccess)
            {
                canAccess = await this._permission.CanAccessWebApi(userId, PermissionTypeEnum.بازبینی_تصاویر_کارت);
                if (!canAccess.IsSuccess)
                    return new ApiResult<PagedImageCitizenViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به بازبینی تصاویر شهروند دسترسی ندارید");
            }



            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;

            return await _citzene.SearchImageCitizens(offset.Value, count.Value, FromDate, ToDate,
                pictureConfirmed, nationCode, hasCard, groupId);

        }

        [HttpGet]
        [Route("SearchCitizensAuthentication")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<PagedCitizenViewModel>> SearchCitizensAuthentication(
           int? offset = 1, int? count = 20,
            DateTime? FromDate = null,
         DateTime? ToDate = null,
         string nationCode = null ,int? registerByService = null,
        SabtStatusEnum? sabtStatus = null

           )
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.جستجوی_شهروند);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<PagedCitizenViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به بازبینی تصاویر شهروند دسترسی ندارید");
            }



            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _citzene.SearchCitizensAuthentication(offset.Value, count.Value, FromDate, ToDate, nationCode, registerByService, sabtStatus);

        }

        [HttpGet]
        [Route("SearchImageCardCitizens")]
        [Authorize(Roles = "Admin,Card")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<PagedImageCitizenViewModel>> SearchImageCardCitizens(
          int? offset = 1, int? count = 20,
           DateTime? FromDate = null,
        DateTime? ToDate = null,
        PersonalPictureEnum? pictureConfirmed = null,
        string nationCode = null
          )
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.بازبینی_تصاویر);
            if (!canAccess.IsSuccess)
            {
                canAccess = await this._permission.CanAccessWebApi(userId, PermissionTypeEnum.بازبینی_تصاویر_کارت);
                if (!canAccess.IsSuccess)
                    return new ApiResult<PagedImageCitizenViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به بازبینی تصاویر شهروند دسترسی ندارید");
            }

            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _citzene.SearchImageCitizens(offset.Value, count.Value, FromDate, ToDate,
                pictureConfirmed, nationCode, true);

        }



        /// <summary>
        /// تایید تصویر پرسنلی
        /// </summary>
        /// <param name="citizenId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AcceptCitizenPicture")]
        [Authorize(Roles = "Admin,Operator,Card")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<string>> AcceptCitizenPicture(int citizenId)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.بازبینی_تصاویر);
            if (!canAccess.IsSuccess)
            {
                canAccess = await this._permission.CanAccessWebApi(userId, PermissionTypeEnum.بازبینی_تصاویر_کارت);
                if (!canAccess.IsSuccess)
                    return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, null, "شما به بازبینی تصاویر شهروند دسترسی ندارید");
            }

            return await _citzene.AcceptCitizenPicture(citizenId);

        }
        /// <summary>
        /// عدم تایید تصویر پرسنلی
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Operator,Card")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<ShortCitizenInfo>> RejectCitizenPicture(RejectCitizenPicture model)
        {
            
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.بازبینی_تصاویر);
            if (!canAccess.IsSuccess)
            {
                canAccess = await this._permission.CanAccessWebApi(userId, PermissionTypeEnum.بازبینی_تصاویر_کارت);
                if (!canAccess.IsSuccess)
                    return new ApiResult<ShortCitizenInfo>(false, ApiResultStatusCode.NotAllowed, null, "شما به بازبینی تصاویر شهروند دسترسی ندارید");
            }


           

            var reject = await _citzene.RejectCitizenPicture(model);
            if (reject.IsSuccess && model.SendSms)
            {
                var data = reject.Data;
                var name = " کد ملی " + data.NationCode;

                int count = await this._smsInfoService.CountSms(data.MobileNumber, TempleteNameEnum.ReviewPicture);
                if (count > 5)
                {
                    reject.Messages = " تعداد پیامک های ارسالی به شماره موبایل شما بیش از حد مجاز می باشد بعد از گذشت 24 ساعت مجددا اقدام نمائید. ";
                    reject.IsSuccess = false;
                    reject.StatusCode = ApiResultStatusCode.BadRequest;
                    return reject;
                }


                //ارسال پیامک عدم تایید
                SendSms sms = new SendSms();
                var smsOption = await _siteSettingService.GetSmsSettingForSend();
                var smsLog = await sms.VerifyLookup(data.MobileNumber, "پرسنلی", null, null, name.Trim(), model.Reason, TempleteNameEnum.ReviewPicture, smsOption.SmsToken);
                if (smsLog.IsSuccess)
                {

                    await _smsInfoService.Add(smsLog.Data, data.CitizenId);
                }
                else
                {
                    reject.Messages = "عدم تایید تصویر پرسنلی با موفقیت صورت گرفت.ولی پیامک برای شهروند ارسال نشد";
                }

            }

            return reject;
        }


        /// <summary>
        /// دریافت لیست پیامک های ارسال شده به شهروند جهت بررسی تصویر
        /// </summary>
        /// <param name="citizenId">شناسه شهروند</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCitizenRejectImageSmsList")]
        [Authorize(Roles = "Admin,Operator,Card")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<List<SmsInfoDto>>> GetCitizenRejectImageSmsList(int citizenId)
        {
            return await _smsInfoService.GetCitizenRejectImage(citizenId);

        }



        /// <summary>
        /// ویرایش تصویر شهروند توسط مدیریت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadPersonalPictureByAdmin")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize(Roles = "Admin,Card")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<UploadFileResult>> UploadPersonalPictureByAdmin([FromForm]UploadCitizenPicture model)
        {
            var response = new ApiResult<UploadFileResult>(false, ApiResultStatusCode.Success, new UploadFileResult());
            try
            {
                int userId = this._usersService.GetCurrentUserId();
                ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.بازبینی_تصاویر);
                if (!canAccess.IsSuccess)
                {
                    canAccess = await this._permission.CanAccessWebApi(userId, PermissionTypeEnum.بازبینی_تصاویر_کارت);
                    if (!canAccess.IsSuccess)
                        return new ApiResult<UploadFileResult>(false, ApiResultStatusCode.NotAllowed, null, "شما به بازبینی تصاویر شهروند دسترسی ندارید");
                }

                var citizenInfo = await _citzene.GetShortCitizenInfo(model.CitizenId);
                var filesPath = _environment.WebRootPath + "/uploads/Resources/Citizens/" + citizenInfo.Data.UserCode.ToString();
                if (!System.IO.Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }
                var file = model.File;
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    string[] supportedTypes = new string[3] { ".jpg", ".jpeg", ".png" };
                    if (!((IEnumerable<string>)supportedTypes).Contains<string>(extension.ToLower()))
                    {
                        response.IsSuccess = false;
                        response.Messages = "  پسوند تصویر ارسالی معتبر نمی باشد";
                        return response;
                    }


                    var vFileName = citizenInfo.Data.NationCode + ".jpg";
                    var path = "/uploads/Resources/Citizens/" + citizenInfo.Data.UserCode.ToString() + $@"/{ vFileName}";
                    string FilePath = _environment.WebRootPath + path;
                    //var   vFilePath = Path.Combine(path, vFileName);
                    if (System.IO.File.Exists(FilePath))
                    {
                        System.IO.File.Delete(FilePath);
                    }
                    using (FileStream fs = System.IO.File.Create(FilePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    await _citzene.ChangeCitizenPicture(model.CitizenId); //تصویر در حال بررسی شهروند
                    response.IsSuccess = true;
                    response.Data.UploadUrl = path;
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Messages = "خطا در آپلود فایل!";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = $"خطا در آپلود فایل: {ex}";
            }
            return response;
        }



        #endregion
        #region عضویت شهروند در گروههای شهروندی



        /// <summary>
        /// حذف یک شهروند از یک گروه
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<string>> RemoveCitizenFromGroup(GroupsCitizensDto model)
        {

            var userId = _usersService.GetCurrentUserId();
            return await _citzene.RemoveCitizenFromGroup(model, userId);

        }

        /// <summary>
        /// اضافه کردن یک شهروند یه گروه
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<string>> AddCitizenToGroup(GroupsCitizensDto model)
        {

            var userId = _usersService.GetCurrentUserId();
            return await _citzene.AddCitizenToGroup(model, userId);

        }


        /// <summary>
        /// شهروند در چه گرووهایی عضو می باشد
        /// </summary>
        /// <param name="userCode">شناسه  guid کاربر</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetGroupsCitizensInfo")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<List<GroupsCitizensInfo>>> GetGroupsCitizensInfo(string userCode)
        {
            return await _citzene.GetGroupsCitizensInfoByUserCode(userCode);


        }


        /// <summary>
        /// شهروند در چه گروههایی عضو است ؟
        /// </summary>
        /// <param name="userCode">شناسه  کاربری شهروندی</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "WebApiUser")]
        [Route("GetCitizenMemberGroups")]
        public async Task<ApiResult<List<GroupsCitizensInfo>>> GetCitizenMemberGroups(string  userCode)
        {
            var webApiuserId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(webApiuserId, Common.GlobalEnum.PermissionTypeEnum.دریافت_اطلاعات_عضویت_شهروند_در_گروههای_شهروندی);
            if (!canAccess.IsSuccess)
                return new ApiResult<List<GroupsCitizensInfo>>(false, ApiResultStatusCode.NotAllowed, new List<GroupsCitizensInfo>(), "کاربر توسعه دهنده به این سرویس دسترسی ندارد");

            return await _citzene.GetGroupsCitizensInfoByUserCode(userCode);

        }


        [HttpGet]
        [Authorize(Roles = "WebApiUser")]
        [Route("GetCitizenMemberGroupsByNationCode")]
        public async Task<ApiResult<List<GroupsCitizensInfo>>> GetCitizenMemberGroupsByNationCode(string nationCode)
        {
            var webApiuserId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(webApiuserId, Common.GlobalEnum.PermissionTypeEnum.دریافت_اطلاعات_عضویت_شهروند_در_گروههای_شهروندی);
            if (!canAccess.IsSuccess)
                return new ApiResult<List<GroupsCitizensInfo>>(false, ApiResultStatusCode.NotAllowed, new List<GroupsCitizensInfo>(), "کاربر توسعه دهنده به این سرویس دسترسی ندارد");



            return await _citzene.GetGroupsCitizensInfo(nationCode);

        }


        /// <summary>
        /// دریافت گروهها و صف هایی که شهروند داخل آن عضو است
        /// </summary>
        /// <param name="nationCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "WebApiUser")]
        [Route("GetCitizenGroupsAndQueues")]
        public async Task<ApiResult<CitizenGroupsAndQueues>> GetCitizenGroupsAndQueues(string nationCode)
        {
            var webApiuserId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(webApiuserId, Common.GlobalEnum.PermissionTypeEnum.دریافت_اطلاعات_عضویت_شهروند_در_گروههای_شهروندی);
            if (!canAccess.IsSuccess)
                return new ApiResult<CitizenGroupsAndQueues>(false, ApiResultStatusCode.NotAllowed, new CitizenGroupsAndQueues(), "کاربر توسعه دهنده به این سرویس دسترسی ندارد");
 
            return await _citzene.GetCitizenGroupsAndQueues(nationCode);

        }



        /// <summary>
        /// دریافت گروهها به صورت صفحه بندی شده 
        /// براساس شناسه گروه
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPagedGroupsCitizensInfo")]
        [Authorize(Roles = "Admin,Card")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<PagedCitizenGroupsViewModel>> GetPagedGroupsCitizensInfo( string  userCode,  int? offset = 1, int? count = 20           )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _citzene.GetGroupsCitizensInfo(userCode, offset.Value, count.Value);

        }

        #endregion
        #region  آدرس شهروند

        /// <summary>
        /// دریافت جزئیات یک آدرس
        /// </summary>
        /// <param name="addressid">شناسه آدرس</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GeAddress")]
        [Authorize(Roles = "Admin,Operator")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<AddressInfo>> GeAddress(int addressid)
        {
            return await _citzene.GeAddress(addressid);

        }


      



        /// <summary>
        /// دریافت اطلاعات ادرس منزل شهروند
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCitizenHomeAddress")]
        [Authorize(Roles = "Citizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<AddressInfo>> GetCitizenHomeAddress()
        {
            var userId = _usersService.GetCurrentUserId();
            return await _citzene.GetCitizenHomeAddress(userId);

        }


        [HttpGet]
        [Route("GetCitizenHomeAddressByAdmin")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<AddressInfo>> GetCitizenHomeAddressByAdmin(string   userCode)
        { 
            return await _citzene.GetCitizenHomeAddress(userCode);

        }

        /// <summary>
        /// دریافت جزئیات یک ادرس به وسیله خود شهروند
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCitizenAddress")]
        [Authorize(Roles = "Citizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<AddressInfo>> GetCitizenAddress(int id)
        {
            var userId = _usersService.GetCurrentUserId();
            return await _citzene.GeAddress(userId, id);

        }


        /// <summary>
        /// دریافت اطلاعات ادرس شهروند
        /// </summary>
        /// <param name="userCode">شناسه کاربری</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCitizenAddressInformation")]
        [Authorize(Roles = "WebApiUser")]
        public async Task<ApiResult<AddressInfo>> GetCitizenAddressInformation(string userCode)
        {
            var webApiuserId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(webApiuserId, Common.GlobalEnum.PermissionTypeEnum.دریافت_اطلاعات_آدرس_شهروند);
            if (!canAccess.IsSuccess)
                return new ApiResult<AddressInfo>(false, ApiResultStatusCode.NotAllowed, new AddressInfo(), "کاربر توسعه دهنده به این سرویس دسترسی ندارد");
            return await _citzene.GetCitizenHomeAddress(userCode);

        }

        /// <summary>
        /// دریافت اطلاعات ادرس شهروند
        /// </summary>
        /// <param >شناسه شهروند</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCitizenOfficeAddress")]
        [Authorize(Roles = "Citizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<AddressInfo>> GetCitizenOfficeAddress()
        {
            var userId = _usersService.GetCurrentUserId();
            return await _citzene.GetCitizenOfficeAddress(userId);

        }


        [HttpGet]
        [Route("GetCitizenOfficeAddressByAdmin")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<AddressInfo>> GetCitizenOfficeAddressByAdmin(string  userCode)
        { 
            return await _citzene.GetCitizenOfficeAddress(userCode);

        }



         




        /// <summary>
        /// اضافه یا ویرایش ادرس توسط شهروند
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Citizen")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<AddressDto>> AddOrUpdateCitizenAddressByCitizen(AddressDto model)
        {
            
            if (model.CitizenId == null)
            {
                model.CitizenId = _usersService.GetCurrentUserId();
            }

            return await _citzene.AddOrUpdateCitizenAddress(model, model.CitizenId.Value);
        }


        [Authorize(Roles = "Citizen")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<AddressDto>> AddOrUpdateCitizenAddressByCitizenForCardAddress(AddressDto model)
        {

            if (model == null)
            {
                return new ApiResult<AddressDto>(false, ApiResultStatusCode.BadRequest, null, "مدل ورودی نامعتبر می باشد");

            }
            if (string.IsNullOrWhiteSpace(model.Street))
            {
                return new ApiResult<AddressDto>(false, ApiResultStatusCode.BadRequest, null, "نام خیابان را وارد نمایید");

            }
            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                return new ApiResult<AddressDto>(false, ApiResultStatusCode.BadRequest, null, "شماره تلفن ثابت را وارد نمایید");

            }
            if (model.CitizenId == null)
            {
                model.CitizenId = _usersService.GetCurrentUserId();
            }
            var isfahan = await _siteSettingService.GetIsfahanInfo();
            if (isfahan.IsSuccess && isfahan.Data.IsfahanCityId != null)
            {
                model.CityId = isfahan.Data.IsfahanCityId.Value;
            }
            else
            {
                model.CityId = 7;
            }
            return await _citzene.AddOrUpdateCitizenAddress(model, model.CitizenId.Value);
        }



        /// <summary>
        /// ویرایش آدرس شهروند
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Operator")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult> AddOrUpdateCitizenAddress(AddressDto model)
        { 
            return await _citzene.AddOrUpdateCitizenAddress(model, model.UserCode);
        }

        /// <summary>
        /// حذف آدرس شهروند توسط مدیر
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveCitizenAddressByAdmin")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<ApiResult> RemoveCitizenAddressByAdmin(int id)
        {
            return await _citzene.RemoveCitizenAddressByAdmin(id);

        }



        /// <summary>
        /// ثبت ادرس به وسیله شهروند
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Citizen")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> AddCitizenAddress(AddressDto model)
        {
            model.CitizenId = _usersService.GetCurrentUserId();
            return await _citzene.AddCitizenAddress(model, model.CitizenId.Value);
        }



        /// <summary>
        /// اضافه کردن آدرس جدید به شهروند
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Operator")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> AddCitizenAddressByAdmin(AddressDto model)
        {
            if (model.CitizenId == null)
            {
                return new ApiResult(false, ApiResultStatusCode.BadRequest, "شناسه شهروند را وارد نمایید");
            }
            return await _citzene.AddCitizenAddress(model, model.CitizenId.Value);
        }

        #endregion 
        #region ویرایش شماره موبایل شهروند توسط خودشهروند

        /// <summary>
        ///  نمایش شماره موبایل شهروند
        ///  [C10]
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        [Route("GetCitizenMobileNumber")]
        [Authorize(Roles = "Citizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<UpdateMobileNumberDto>> GetCitizenMobileNumber()
        {
            var citizenId = _usersService.GetCurrentUserId();
            return await _citzene.GetCitizenMobileNumber(citizenId);

        }


        /// <summary>
        /// دریافت شماره موبایل توسط ادمین
        /// </summary>
        /// <param name="citizenId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCitizenMobileNumberByAmin")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<UpdateMobileNumberDto>> GetCitizenMobileNumberByAmin(int citizenId)
        { 
            return await _citzene.GetCitizenMobileNumber(citizenId); 
        }



        /// <summary>
        /// ارسال کد پیامک برای تغییر شماره موبایل شهروند در داخل خود پنل شهروند توسط خود شهروند
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Citizen")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> GetVerfiCodeByCitizen(CitizenVerifyMobileNumberDto model)
        {
            var res1 = new ApiResult(true, ApiResultStatusCode.Success, "پیامک با موفقیت ارسال گردید.");


            try
            {
                //ارسال پیامک
                SendSms sms = new SendSms();
                //چک کن تا سه دقیقه قبل براش پیامک ارسال نشده باشه

                Random r = new Random();
                var code = "";
                var lastCode = await _smsInfoService.GetLastMobileValidationByMobileNumber(model.NewMobileNumber);
                if (lastCode.IsSuccess)
                {
                    var time = DateTime.Now.AddSeconds(-180);//دو دقیقه قبل
                    if (lastCode.Data.SendOnDate > time)
                    {
                        res1.Messages = "برای شما کد تایید ارسال شده است . لطفا منتظر دریافت کد تایید باشید ";
                        res1.IsSuccess = false;
                        res1.StatusCode = ApiResultStatusCode.BadRequest;
                        return res1;
                    }

                    code = lastCode.Data.Token1;
                }
                else
                {
                    code = r.Next(10000, 99999).ToString();
                }
                var smsOption = await _siteSettingService.GetSmsSettingForSend();
                var smsLog = await sms.VerifyLookup(model.NewMobileNumber, code, TempleteNameEnum.MobileVerify, smsOption.SmsToken);
                if (smsLog.IsSuccess)
                {
                    var citizenId = _usersService.GetCurrentUserId();
                    var saveSms = await _smsInfoService.Add(smsLog.Data, citizenId);
                    if (!saveSms.IsSuccess)
                    {
                        return new ApiResult(false, ApiResultStatusCode.BadRequest, saveSms.Messages);
                    }
                }
                else
                {
                    return new ApiResult(false, ApiResultStatusCode.BadRequest, smsLog.Messages);
                }

            }
            catch (Exception er)
            {

                res1.Messages = "خطایی در ارسال پیامک رخ داده است";
                res1.StatusCode = ApiResultStatusCode.BadRequest;
                return res1;
            }
            return res1;



        }



        /// <summary>
        /// ویرایش شماره موبایل شهروند توسط خود شهروند
        ///  [C9]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Citizen")]
        [IgnoreAntiforgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("[action]")]
        public async Task<ApiResult<string>> UpdateCitizenMobileNumber(UpdateMobileNumberDto model)
        {

            model.CitizenId = _usersService.GetCurrentUserId();
            return await _citzene.UpdateCitizenMobileNumber(model);

        }



        [Authorize(Roles = "Admin")]
        [IgnoreAntiforgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("[action]")]
        public async Task<ApiResult<string>> UpdateCitizenMobileNumberByAdmin(UpdateMobileNumberDto model)
        {
            //ببین مدیر به ویرایش شماره موبایل دسترسی دارد؟

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.ویرایش_شماره_موبایل_شهروند);
            if (!canAccess.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed,   "شما به ویرایش شماره موبایل شهروند دسترسی ندارید", "شما به ویرایش شماره موبایل شهروند دسترسی ندارید");


            
            return await _citzene.UpdateCitizenMobileNumberByAdmin(model, userId); 
        }


        [Authorize(Roles = "Card")]
        [IgnoreAntiforgeryToken]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("[action]")]
        public async Task<ApiResult<string>> UpdateCitizenMobileNumberByCard(UpdateMobileNumberDto model)
        {
            //ببین مدیر به ویرایش شماره موبایل دسترسی دارد؟
         
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.ویرایش_شماره_موبایل_شهروند_اصفهان_کارت);
            if (!canAccess.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, "شما به ویرایش شماره موبایل شهروند دسترسی ندارید", "شما به ویرایش شماره موبایل شهروند دسترسی ندارید");


            return await _citzene.UpdateCitizenMobileNumberByAdmin(model, userId);
        }




        #endregion
        #region ویرایش آدرس ایمیل شهروند توسط خودشهروند

        /// <summary>
        ///  نمایش آدرس ایمیل شهروند
        ///  [C10]
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        [Route("GetCitizenEmail")]
        [Authorize(Roles = "Citizen")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<UpdateEmailAddressDto>> GetCitizenEmail()
        {
            var citizenId = _usersService.GetCurrentUserId();
            return await _citzene.GetCitizenEmail(citizenId);

        }



        /// <summary>
        /// ویرایش آدرس ایمیل شهروند توسط خود شهروند
        ///  [C9]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Citizen")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<string>> UpdateCitizenEmailAddress(UpdateEmailAddressDto model)
        {

            model.CitizenId = _usersService.GetCurrentUserId();
            return await _citzene.UpdateCitizenEmailAddress(model);

        }





        #endregion 
        #region ثبت بازخورد
        /// <summary>
        /// ثبت بازخورد شهروند
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Operator,Card")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> AddFeedbacke(CitizenFeedbackDto model)
        {

            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.بازخورد_ها);
            if (!canAccess.IsSuccess)
                return new ApiResult (false, ApiResultStatusCode.NotAllowed,  "شما به این قسمت دسترسی ندارید");


            return await _citizenFeedback.AddFeedbacke(model, userId);

        }

        /// <summary>
        /// دریافت همه بازخوردهای یک شهروند
        /// </summary>
        /// <param name="userCode">شناسه شهروند</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllCitizenFeedbacks")]
        [Authorize(Roles = "Admin,Operator,Card")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<List<CitizenFeedbackInfo>>> GetAllCitizenFeedbacks(string  userCode)
        {
         

            return await _citizenFeedback.GetAllCitizenFeedbacks(userCode);

        }

        /// <summary>
        /// دریافت همه عناوین بازخورد
        /// </summary>
        /// <param name="selected">انتخاب پیش فرض</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBaseListFeedbacke")]
        [AllowAnonymous]
       
        public async Task<ApiResult<List<BaseDataModel>>> GetBaseListFeedbacke(int? selected = null)
        {
          

            return await _citizenFeedback.GetBaseListFeedbacke(selected);

        }

        /// <summary>
        /// دریافت جزئیات یک بازخورد
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFeedback")]
        [Authorize(Roles = "Admin,Operator,Card")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CitizenFeedbackInfo>> GetFeedback(int id)
        {
            return await _citizenFeedback.GetFeedback(id);

        }

        /// <summary>
        /// جستجوی بازخورد
        /// </summary>
        /// <param name="offset">صفحه</param>
        /// <param name="count">تعداد</param>
        /// <param name="feedbackId">شناسه عنوان تیکت</param>
        /// <param name="FromDate">از تاریخ</param>
        /// <param name="ToDate">تا تاریخ</param>
        /// <param name="name">نام و نام خانوادگی شهروند</param>
        /// <param name="nationalCode">کد ملی شهروند</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Searchfeedbacks")]
        [Authorize(Roles = "Admin,Operator")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<PageFeedbackViewModel>> Searchfeedbacks(int? offset = 1,
            int? count = 20, int? feedbackId = null, DateTime? FromDate = null,
            DateTime? ToDate = null,
            string name = null, string nationalCode = null)
        {

            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.بازخورد_ها);
            if (!canAccess.IsSuccess)
                return new ApiResult<PageFeedbackViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");


            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;

            return await _citizenFeedback.Searchfeedbacks(offset.Value, count.Value, feedbackId,
                FromDate, ToDate, name, nationalCode);

        }

        /// <summary>
        /// حذف بازخورد شهروند
        /// </summary>
        /// <param name="id">حذف بازخورد</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Removefeedback")]
        [Authorize(Roles = "Admin,Operator")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<string>> Removefeedback(int id)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.بازخورد_ها);
            if (!canAccess.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");



            return await _citizenFeedback.Remove(id);

        }



        #endregion
        #region پیامک های ارسال شده به شهروند
        [HttpGet]
        [Route("GetCitizenPagedSmsList")]
        [Authorize(Roles = "Admin,Card")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<PagedSmsInfoViewModel>> GetCitizenPagedSmsList(
            string userCode, int? offset = 1,
            int? count = 20, DateTime? FromDate = null

            )
        {


            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _smsInfoService.GetCitizenPagedSmsListAsync(userCode, offset.Value, count.Value,
                FromDate);

        }
        #endregion
        #region بارگذاری شهروندان به صورت دسته ایی
        /// <summary>
        /// لیست فایل های وارد شده 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CitizenImportFileList")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<List<CitizenImportFileInfo>>> CitizenImportFileList()
        {

            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.ثبت_نام_دسته_ایی_شهروند);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<List<CitizenImportFileInfo>>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");
            }


            return await _citzene.CitizenImportFileList();

        }

        /// <summary>
        /// ورود شهروندان از طریق فایل اکسل
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ImportCitizenListFromExcel")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<UploadFileResult>> ImportCitizenListFromExcel(IFormFile file)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.ثبت_نام_دسته_ایی_شهروند);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<UploadFileResult>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");
            }

            var errorLine = "";
            var res = new ApiResult<UploadFileResult>(true, ApiResultStatusCode.Success, new UploadFileResult(), "");
            try
            {
                var fileName = "";
                string folderName = "ImportCitizen";
                var ticks = DateTime.Now.Ticks;
                var webRootPath = _environment.WebRootPath + "/uploads/Resources/ImportCitizen/" + ticks;
               
                string newPath = Path.Combine(webRootPath, folderName);
                var list = new List<CitizenExcelFileColumns>();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    fileName = file.FileName;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                        }
                        IRow headerRow = sheet.GetRow(0); //Get Header Row
                        int cellCount = headerRow.LastCellNum;
                        var cells = headerRow.Cells;
                        if (cellCount < 2)
                        {
                            res.IsSuccess = false;
                            res.Messages = "فایل اکسل نامعتبر می باشد";
                            res.StatusCode = ApiResultStatusCode.BadRequest;
                        }
                        else
                        {
                            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                            {
                                IRow row = sheet.GetRow(i);
                                if (row == null) continue;
                                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                              
                                var cell1 = row.GetCell(0);//NationalCode 
                                var cell2 = row.GetCell(1);//Gender
                                var cell3 = row.GetCell(2);//FirstName
                                var cell4 = row.GetCell(3);//LastName 
                                var cell5 = row.GetCell(4);//FatherName
                                var cell6 = row.GetCell(5);//BirthDate
                                var cell7 = row.GetCell(6);//Mobile
                                var cell8 = row.GetCell(7);//ServiceId
                                var cell9 = row.GetCell(8);//GroupId
                               
                                if (cell1 != null && cell2 != null && cell3 != null && cell4 != null && cell5 != null && cell6 != null && cell7 != null)
                                {

                                    string NationCode = cell1.ToString(); //cell1
                                    string Gender = cell2.ToString();   //cell2 
                                    string FirstName = cell3.ToString();  //cell3
                                    string LastName = cell4.ToString();//cell4
                                    string FatherName = cell5.ToString();//cell5
                                    string BirthDate = cell6.ToString();//cell6
                                    string Mobile = cell7.ToString();//cell7
                                    string ServiceId = "0";
                                    if (cell8 != null)
                                        ServiceId = cell8.ToString();//cell8 

                                    string groupId = "0";
                                    if (cell9 != null)
                                        groupId = cell9.ToString();//cell8 

                                    errorLine = NationCode;

                                    

                                    list.Add(new CitizenExcelFileColumns
                                    {
                                        BirthDate = BirthDate,
                                        FatherName = FatherName,
                                        FirstName = FirstName,
                                        ServiceId = ServiceId,
                                        Gender = Gender,
                                        LastName = LastName,
                                        NationCode = NationCode,
                                        Mobile = Mobile,
                                        GroupId = groupId
                                    });


                                }

                            }

                        }


                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.Messages = "فایل اکسل را انتخاب کنید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                }
                if (list.Any())
                {
                    
                    var addfile = await _citzene.AddCitizenImportFile(list, fileName, userId);
                    if (addfile.IsSuccess == false)
                    {
                        res.Messages = addfile.Messages;
                        res.IsSuccess = false;
                    }
                    else
                    {
                        res.Data.ImportId = addfile.Data.ImportId;
                    }

                }
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است. ممکن است فایل ورودی نامعتبر باشد"+ errorLine;
                res.StatusCode = ApiResultStatusCode.BadRequest;

            }
            return res;
        }


        [HttpGet]
        [Route("CitizenImportFileDetails")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<CitizenImportPagedList>> 
            CitizenImportFileDetails(int importId ,
            
            int? offset = 1, int? count = 20 ,  bool? isValidRow = null) 
        {
                if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _citzene.CitizenImportFileDetails(offset.Value, count.Value, importId, isValidRow);

        }


        /// <summary>
        /// حذف فایل اکسل
        /// </summary>
        /// <param name="importId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveCitizenImportFile")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<string>> RemoveCitizenImportFile(int importId)
        {
            return await _importFile.Remove(importId);

        }

        [HttpGet]
        [Route("ConfirmCitizenFile")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> ConfirmCitizenFile(int importId)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.ثبت_نام_دسته_ایی_شهروند);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult(false, ApiResultStatusCode.NotAllowed,   "شما به این قسمت دسترسی ندارید");
            }

            return await _citzene.ConfirmCitizenFile(importId, userId);
        }

        #endregion
        #region احراز هویت شهروند
        [HttpGet]
        [Route("GetCheckingCitizensDead")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> GetCheckingCitizensDead()
        {
            var bagrezvan = new BagRezvanService();
            var getlist= await _citzene.GetCheckingCitizensDead(50);
            if(getlist.IsSuccess)
            {
                var data = getlist.Data;
                if(data.Any())
                {
                    var list = data.Select(s => s.NationCode).ToList();
                    var checkList = bagrezvan.CheckNationCodes(list);
                    if (checkList!=null)
                    {
                        foreach (var item in checkList)
                        {
                            //شهروندی که فوتی هست به حالت فوتی در بیار
                            await _citzene.UpdateCheckDeathStateCitizen(item.NationalCode, item.Exist);
                        }
                        
                        await _citzene.DeleteCheckingCitizensDead(checkList);
                    }

                }
            }

            return new ApiResult(true, ApiResultStatusCode.Success, "");

        }
        [HttpGet]
        [Route("CheckIsDead")]
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> CheckIsDead(string nationCode)
        {
            var bagrezvan = new BagRezvanService();
            var checkitem = bagrezvan.IsDead(nationCode);
            if (checkitem != null)
            {
                //شهروندی که فوتی هست به حالت فوتی در بیار
                await _citzene.UpdateCheckDeathStateCitizen(nationCode, checkitem==true);
                return new ApiResult(true, ApiResultStatusCode.Success, "استعلام با موفقیت انجام گردید");
            }
            return new ApiResult(false, ApiResultStatusCode.Success, bagrezvan.ErrorMessage);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("CommitJob")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> CommitJob()
        { 

            RecurringJob.AddOrUpdate(() =>  GetCheckingCitizensDead(), Cron.Hourly);
            return new ApiResult(true, ApiResultStatusCode.Success, "");
        }

 
         



        [AllowAnonymous]
        [HttpGet]
        [Route("CommitAuthenticationCitizenJob")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> CommitAuthenticationCitizenJob()
        {
            RecurringJob.AddOrUpdate(() => RunAuthenticationCitizenJob(), Cron.Minutely);
            return new ApiResult(true, ApiResultStatusCode.Success, "");
        }
        [HttpGet]
        [Route("RunAuthenticationCitizenJob")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public async Task<ApiResult> RunAuthenticationCitizenJob()
        {
            var userId = _usersService.GetCurrentUserId();
            var getCitizen = await _exportCitizen.GetCitizenForAuthentication();
            if (getCitizen.IsSuccess)
            {
                var data = getCitizen.Data;
                if (data != null)
                {
                    var call = new ItsaazApi(_cache);
                    var token =await call.AutenticationApi();

                    if (string.IsNullOrWhiteSpace(token))
                    { 
                        await _event.AddEvent(new ViewModel.Events.EventDto()
                        {
                            Description = "خطا در دریافت توکن سرویس استعلام"+ call.ErrorMessage,
                            UserId = data.CitizenId,
                            ActionName = "CitizenAuthenticationAfterUpdateIdentityInformation",
                            EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                            EventSection = Common.GlobalEnum.EventSectionEnum.خطای_احراز_هویت,
                            EventType = Common.GlobalEnum.EventTypeEnum.Error,
                            OperationId = data.CitizenId
                        });
                        return new ApiResult(false, ApiResultStatusCode.Success, "");
                    }



                  if (!string.IsNullOrWhiteSpace(token))
                    {
                        try
                        {
                            DateTimeToPersianDateTimeConverter persianDate = new DateTimeToPersianDateTimeConverter("/", false);

                            var date = persianDate.toShamsiDateTime(data.BirthDate.Value);
                            var getCitizenInfo = await call.GetData(new GetDataParam() { BirthDate = date, Token = token, NationalCode = data.NationCode });
                            if (getCitizenInfo != null)
                            {
                                getCitizenInfo.birthDate = date;
                                getCitizenInfo.nationalCode = data.NationCode;
                                await _citzene.UpdateCitizenAfterAuthentication(getCitizenInfo, data.CitizenId);

                            }
                            else
                            {
                                
                                await _event.AddEvent(new ViewModel.Events.EventDto()
                                {
                                    Description = "خطا  در استعلام" + call.ErrorMessage,
                                    UserId = data.CitizenId,
                                    ActionName = "CitizenAuthenticationAfterUpdateIdentityInformation",
                                    EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                                    EventSection = Common.GlobalEnum.EventSectionEnum.خطای_احراز_هویت,
                                    EventType = Common.GlobalEnum.EventTypeEnum.Error,
                                    OperationId = data.CitizenId
                                });
                                return new ApiResult(false, ApiResultStatusCode.BadRequest, "خطا  در استعلام" + call.ErrorMessage);


                            }

                        }
                        catch (Exception er)
                        {

                        }
                    }


                }
            } 
            return new ApiResult(true, ApiResultStatusCode.Success, "");

        }

         
        #endregion 

        [HttpGet]
        [Route("configUserCode")]
        [AllowAnonymous] 
        public async Task<ApiResult> configUserCode(string code)
        {
            return await _citzene.UpdateUserCode(code);

        }

          






    }

}