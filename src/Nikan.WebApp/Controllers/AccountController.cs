using System.Security.Claims;
using System.Threading.Tasks;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.Services;
using Nikan.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nikan.ViewModel.Users;
using Nikan.ViewModel.UserCompanes;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;
using Nikan.Services.BaseEntity;
using cle.Services;
using Nikan.Services.Permissions;
using Nikan.Services.Events;
using Nikan.Services.Citizens;
using Nikan.ViewModel.Citizens;
using Hangfire;
using Nikan.Services.RateLimiter;



namespace Nikan.WebApp.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class AccountController : Controller
    {
        
        #region Ctor
        private readonly IAppService _app;
        private readonly IUsersService _usersService;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly IUnitOfWork _uow;
        private readonly ICitizenService _citizen;
        private readonly IAntiForgeryCookieService _antiforgery;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly ISmsInfoService _smsInfoService;
        private readonly ISiteSettingService _siteSettingService;
        private readonly IPermissionService _permission;
        private readonly IEventService _event;
        private readonly IBackgroundJobClient _backgroundJobClient;

        private readonly IMemoryRateLimiterService _rateLimitService;

        public AccountController(
            IUsersService usersService,
            ISmsInfoService  smsInfoService,
              IPermissionService permission,
               IEventService siteevent,
               IMemoryRateLimiterService rateLimitService,
            ICitizenService citizen,
            ITokenStoreService tokenStoreService,
            ITokenFactoryService tokenFactoryService
            , IAppService  app ,IUnitOfWork uow,
            ISiteSettingService siteSettingService,
              IBackgroundJobClient backgroundJobClient,
            IAntiForgeryCookieService antiforgery)
        {
            _usersService = usersService;
            _usersService.CheckArgumentIsNull(nameof(usersService));
            _event = siteevent;
            _tokenStoreService = tokenStoreService;
            _tokenStoreService.CheckArgumentIsNull(nameof(tokenStoreService));
            _siteSettingService = siteSettingService;
            _uow = uow;
            _citizen = citizen ;
            _app = app;
            _uow.CheckArgumentIsNull(nameof(_uow));
            _smsInfoService =  smsInfoService;
            _antiforgery = antiforgery;
            _antiforgery.CheckArgumentIsNull(nameof(antiforgery));
            _permission = permission;
            _tokenFactoryService = tokenFactoryService;
            _backgroundJobClient = backgroundJobClient;
            _tokenFactoryService.CheckArgumentIsNull(nameof(tokenFactoryService));
            _rateLimitService = rateLimitService;
        }
        #endregion




        /// <summary>
        /// چک کردن اطلاعات شهروند جهت ثبت نام
        /// [C2]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult> CheckCitizenRegister(PreRegisterDto model)
        {
            //چک کردن ثبت نام شهروند به همراه ارسال پیامک اعتبارسنجی
            var res = new ApiResult(true, ApiResultStatusCode.Success, "");
            if (model == null)
            {
                res.Messages = " اطلاعات ورود به سامانه را وارد نمایید";
                res.IsSuccess = false;
                return res;
            }
           

           


            if (string.IsNullOrWhiteSpace(model.MobileNumber))
            {
                res.Messages = "شماره موبایل را وارد نمایید";
                res.IsSuccess = false;
                return res;
            }

            model.MobileNumber = model.MobileNumber.Fa2En();

            if (!model.MobileNumber.StartsWith("09"))
            {
                res.Messages = "شماره موبایل را به صورت صحیح وارد نمایید";
                res.IsSuccess = false;
                return res;
            }

            if (model.MobileNumber.Length != 11)
            {
                res.Messages = "شماره موبایل را به صورت صحیح وارد نمایید";
                res.IsSuccess = false;
                return res;
            }



            

            var check = await _citizen.CheckCitizenRegister(model);
            if (check.IsSuccess)
            {

                if(check.Data.IsCanRegister)
                {
                    //send sms
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
                        res.Messages = "تنظیمات پیامک انجام نشده است ";
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                    if (string.IsNullOrWhiteSpace(smsOption.SmsToken))
                    {
                        res.Messages = "تنظیمات پیامک انجام نشده است ";
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.BadRequest;
                        return res;
                    }

                   


                    var smsLog = await sms.VerifyLookup(model.MobileNumber, code, TempleteNameEnum.MobileVerify, smsOption.SmsToken);
                    if (smsLog.IsSuccess)
                    {
                        smsLog.Data.Token2 = model.NationCode;//دخیره کد ملی 
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
                }
                else
                {

                    return new ApiResult(false, ApiResultStatusCode.BadRequest, check.Data.Description);
                }
               


            }
            return check;
        }


         
        

        /// <summary>
        /// ورود به سامانه
        /// [AC1]
        /// </summary>
        /// <param name="loginUser">اطلاعت ورود به سامانه</param>
        /// <returns></returns>
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<UserInfoDto>> Login([FromBody]  LosginModel loginUser)
        {
            var userIp = Request.HttpContext.Connection.RemoteIpAddress;
            var res = new ApiResult<UserInfoDto>(true, ApiResultStatusCode.Success, new UserInfoDto(), "");
            try
            {
                if (loginUser == null)
                {
                    res.Messages = " اطلاعات ورود به سامانه را وارد نمایید";
                    res.IsSuccess = false;
                    return res;
                }

 



                var user = await _usersService.FindUserAsync(loginUser.Username, loginUser.Password);

                if (user == null)
                {
                    res.Messages = "اطلاعات وارد شده صحیح نمی باشد";
                    res.IsSuccess = false;
                    return res;
                }

                
                if (user.UserAccountState==Common.GlobalEnum.userAccountStateEnum.بلاک)
                {
                    res.Messages = " حساب کاربری شما مسدود شده است.";
                    res.IsSuccess = false;
                    return res;
                }
                if (user.UserAccountState == Common.GlobalEnum.userAccountStateEnum.غیر_فعال)
                {
                    res.Messages = " حساب کاربری شما غیرفعال می باشد";
                    res.IsSuccess = false;
                    return res;
                }
                if(user.UserCompany!=null)
                {
                    var company = user.UserCompany;
                    if(company.UserCompanyAccountStatus==Common.GlobalEnum.UserCompanyAccountStatusEnum.بلاک)
                    {
                        res.Messages = " حساب کاربری شما مسدود شده است.";
                        res.IsSuccess = false;
                        return res;
                    }

                    res.Data.CompanyName = company.CompanyName;
                    res.Data.CompanyId = company.Id;
                    res.Data.UserCompanyAccountStatus = company.UserCompanyAccountStatus;
                    res.Data.RejectDesription = company.RejectDesription;



                }
                else if(user.IsAdmin)
                {
                    //منوهای ویژه پنل مدیریت
                    var permissions = await _permission.GetPermissionAdminMenuByUserId(user.Id);
                    res.Data.permissions = permissions.Data;
                    var smsOption = await _siteSettingService.GetSmsSettingForSend();

                    if (smsOption != null && smsOption.SendSmsAfterAdminLogin == true && !string.IsNullOrWhiteSpace(user.MobileNumber) )
                    {
                         
                        SendSms sms = new SendSms();
                        var smsLog = await sms.VerifyLookup(user.MobileNumber, user.Username,"","", DateTime.Now.ToShortTimeString(), userIp.ToString(), TempleteNameEnum.adminlogin, smsOption.SmsToken);
                        if (smsLog.IsSuccess)
                           await _smsInfoService.Add(smsLog.Data, user.Id); 
                        else
                        {
                            await _event.AddEvent(new ViewModel.Events.EventDto()
                            {
                                Description = smsLog.Messages,
                                UserId = user.Id,
                                ActionName = "AdminLogin",
                                EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                                EventSection = Common.GlobalEnum.EventSectionEnum.ورود_شهروند,
                                EventType = Common.GlobalEnum.EventTypeEnum.Error,
                                OperationId = user.Id,
                                UserIp = userIp.ToString()

                            });

                        }
                    }


                }
                else
                {
                    var permissions = await _permission.GetPermissionCardMenuByUserId(user.Id);
                    res.Data.permissions = permissions.Data;
                }


                var result = await _tokenFactoryService.CreateJwtTokensAsync(user);
                await _tokenStoreService.AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, null);
                
                await _uow.SaveChangesAsync();
                _antiforgery.RegenerateAntiForgeryCookies(result.Claims);
                res.Data.refresh_token = result.RefreshToken;
                res.Data.access_token = result.AccessToken;
                res.Data.DisplayName = user.DisplayName;
                res.Data.UserId = user.Id;
                if (loginUser.ServiceId == null)
                    loginUser.ServiceId = 0;//پروفایل شهروندی
                res.Data.ServiceId = loginUser.ServiceId;
                
                await _event.AddEvent(new ViewModel.Events.EventDto()
                {
                    Description = user.DisplayName + "(" + loginUser.ServiceId + ")",
                    UserId = user.Id,
                    ActionName = "Login",
                    EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                    EventSection = Common.GlobalEnum.EventSectionEnum.ورود_شهروند,
                    EventType = Common.GlobalEnum.EventTypeEnum.Info,
                    OperationId = user.Id,
                    UserIp = userIp.ToString()

                });

                _backgroundJobClient.Enqueue(() => UpdateCitizenGroupsJobs(loginUser.Username));





            }
            catch (System.Exception er)
            {
                res.Messages = "در حال حاضر امکان ورود به سامانه وجود ندارد";
                res.IsSuccess = false;
                
                await _event.AddEvent(new ViewModel.Events.EventDto()
                {
                    Description = loginUser.Username + "(" + loginUser.ServiceId + ")" + er.Message,
                    ActionName = "UserLogin",
                    EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                    EventSection = Common.GlobalEnum.EventSectionEnum.خطای_سیستمی,
                    EventType = Common.GlobalEnum.EventTypeEnum.Info,
                    UserIp = userIp.ToString()
                });

            }


            return res;

            
        }

        [HttpGet]
        [Route("UpdateCitizenGroupsJobs")]
        [Authorize()]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task UpdateCitizenGroupsJobs(string nationcode)
        {

            await _citizen.UpdateCitizenGroups(nationcode);

        }


        /// <summary>
        /// ورود کاربر روش دوم
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<UserInfoDto>> UserLogin([FromBody]  WebApiLosginModel loginUser)
        {

            var res = new ApiResult<UserInfoDto>(true, ApiResultStatusCode.Success, new UserInfoDto(), "");
            try
            {
                if (loginUser == null)
                {
                    res.Messages = " اطلاعات ورود به سامانه را وارد نمایید";
                    res.IsSuccess = false;
                    return res;
                }
                if (loginUser.ServiceId == null)
                {
                    res.Messages = " شناسه خدمات را مشخص نمایید";
                    res.IsSuccess = false;
                    return res;
                }
                var appservice =await _app.GetAppInfo(loginUser.ServiceId.Value);
                if(!appservice.IsSuccess)
                {
                    res.Messages = appservice.Messages;
                    res.IsSuccess = false;
                    res.StatusCode = appservice.StatusCode;
                    return res;
                }


                var user = await _usersService.FindUserAsync(loginUser.Username, loginUser.Password);
                if (user == null)
                {
                    //username
                    var u = await _usersService.FindUserAsync(loginUser.Username);
                    if (u == null)
                    {
                        res.Messages = " نام کاربری وارد شده صحیح نمی باشد";
                        res.IsSuccess = false;
                        return res;
                    }
                    else
                    {
                        res.Messages = "کلمه عبور وارد شده صحیح نمی باشد";
                        res.IsSuccess = false;
                        return res;

                    }

                }


               
                if (user.UserAccountState == Common.GlobalEnum.userAccountStateEnum.بلاک)
                {
                    res.Messages = " حساب کاربری شما مسدود شده است.";
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.NotAllowed;
                    return res;
                }
                if (user.UserAccountState == Common.GlobalEnum.userAccountStateEnum.غیر_فعال)
                {
                    res.Messages = " حساب کاربری شما غیرفعال می باشد";
                    res.StatusCode = ApiResultStatusCode.NotAllowed;
                    res.IsSuccess = false;
                    return res;
                }
                if (user.UserCompany != null)
                {
                    var company = user.UserCompany;
                    if (company.UserCompanyAccountStatus == Common.GlobalEnum.UserCompanyAccountStatusEnum.بلاک)
                    {
                        res.Messages = " حساب کاربری شما مسدود شده است.";
                        res.IsSuccess = false;
                        return res;
                    }
                    
                    res.Data.CompanyName = company.CompanyName;
                    res.Data.CompanyId = company.Id; 
                    res.Data.UserCompanyAccountStatus = company.UserCompanyAccountStatus;
                    res.Data.RejectDesription = company.RejectDesription; 
                }
                 


                var result = await _tokenFactoryService.CreateJwtTokensAsync(user);
                await _tokenStoreService.AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, null);

                await _uow.SaveChangesAsync();
                _antiforgery.RegenerateAntiForgeryCookies(result.Claims);
                res.Data.refresh_token = result.RefreshToken;
                res.Data.access_token = result.AccessToken;
                res.Data.DisplayName = user.DisplayName;
                res.Data.UserId = user.Id;
                res.Data.UserCode = user.UserCode;
                res.Data.UserName = user.Username;
                res.Data.UserAccountState = user.UserAccountState;
                res.Data.CreatedOnDate = user.CreatedOnDate;
                res.Data.ServiceId = loginUser.ServiceId;
                res.Data.ServiceName = appservice.Data.ServiceName;






                var userIp = Request.HttpContext.Connection.RemoteIpAddress;
                await _event.AddEvent(new ViewModel.Events.EventDto()
                {
                    Description = user.DisplayName + "(" + loginUser.ServiceId + ")",
                    UserId = user.Id,
                    ActionName = "UserLogin",
                    EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                    EventSection = Common.GlobalEnum.EventSectionEnum.ورود_شهروند,
                    EventType = Common.GlobalEnum.EventTypeEnum.Info,
                    OperationId = user.Id,
                    UserIp = userIp.ToString() 
                });
                _backgroundJobClient.Enqueue(() => UpdateCitizenGroupsJobs(loginUser.Username));

            }
            catch (System.Exception er)
            {
                res.Messages = "در حال حاضر امکان ورود به سامانه وجود ندارد";
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                var userIp = Request.HttpContext.Connection.RemoteIpAddress; 
                await _event.AddEvent(new ViewModel.Events.EventDto()
                {
                    Description = loginUser.Username + "(" + loginUser.ServiceId + ")" + er.Message, 
                    ActionName = "UserLogin",
                    EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                    EventSection = Common.GlobalEnum.EventSectionEnum.خطای_سیستمی,
                    EventType = Common.GlobalEnum.EventTypeEnum.Info, 
                    UserIp = userIp.ToString()
                });




            }


            return res;


        }

        /// <summary>
        /// ورود شهروند به سامانه به همراه دریافت گروههای شهروندی
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<UserInfoDto>> CitizenLoginAndGetGroups([FromBody]  WebApiLosginModel loginUser)
        {

            var res = new ApiResult<UserInfoDto>(true, ApiResultStatusCode.Success, new UserInfoDto(), "");
            try
            {
                if (loginUser == null)
                {
                    res.Messages = " اطلاعات ورود به سامانه را وارد نمایید";
                    res.IsSuccess = false;
                    return res;
                }
                if (loginUser.ServiceId == null)
                {
                    res.Messages = " شناسه خدمات را مشخص نمایید";
                    res.IsSuccess = false;
                    return res;
                }
                


                var user = await _usersService.FindUserAsync(loginUser.Username, loginUser.Password);

                if (user == null)
                {
                     
                    var u = await _usersService.FindUserAsync(loginUser.Username);
                    if (u == null)
                    {
                        res.Messages = " نام کاربری وارد شده صحیح نمی باشد";
                        res.IsSuccess = false;
                        return res;
                    }
                    else
                    {
                        res.Messages = "کلمه عبور وارد شده صحیح نمی باشد";
                        res.IsSuccess = false;
                        return res;

                    }

                }

                if (user.UserAccountState == Common.GlobalEnum.userAccountStateEnum.بلاک)
                {
                    res.Messages = " حساب کاربری شما مسدود شده است.";
                    res.IsSuccess = false;
                    return res;
                }
                if (user.UserAccountState == Common.GlobalEnum.userAccountStateEnum.غیر_فعال)
                {
                    res.Messages = " حساب کاربری شما غیرفعال می باشد";
                    res.IsSuccess = false;
                    return res;
                }
               



                var result = await _tokenFactoryService.CreateJwtTokensAsync(user);
                await _tokenStoreService.AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, null);

                await _uow.SaveChangesAsync();
                _antiforgery.RegenerateAntiForgeryCookies(result.Claims);
                res.Data.refresh_token = result.RefreshToken;
                res.Data.access_token = result.AccessToken;
                res.Data.DisplayName = user.DisplayName;
                res.Data.UserId = user.Id;
                res.Data.UserCode = user.UserCode;
                res.Data.UserName = user.Username;
                res.Data.UserAccountState = user.UserAccountState;
                res.Data.CreatedOnDate = user.CreatedOnDate;
                res.Data.ServiceId = loginUser.ServiceId;
                var getGroups= await _citizen.GetShortGroupsCitizensInfo(user.Id);
                if(getGroups.IsSuccess)
                {
                    res.Data.CitizenGroups = getGroups.Data;
                }


                var userIp = Request.HttpContext.Connection.RemoteIpAddress;
                await _event.AddEvent(new ViewModel.Events.EventDto()
                {
                    Description = user.DisplayName + "(" + loginUser.ServiceId + ")",
                    UserId = user.Id,
                    ActionName = "WebApiLoginModel",
                    EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                    EventSection = Common.GlobalEnum.EventSectionEnum.ورود_شهروند,
                    EventType = Common.GlobalEnum.EventTypeEnum.Info,
                    OperationId = user.Id,
                    UserIp = userIp.ToString()
                });

            }
            catch (System.Exception er)
            {
                res.Messages = "در حال حاضر امکان ورود به سامانه وجود ندارد";
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;

            }


            return res;


        }

        /// <summary>
        /// [AC2]
        /// رفرش توکن کاربر
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken([FromBody]Token model)
        {
            var refreshTokenValue = model.RefreshToken;
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                return BadRequest("refreshToken is not set.");
            }

            var token = await _tokenStoreService.FindTokenAsync(refreshTokenValue);
            if (token == null)
            {
                return Unauthorized();
            }

            var result = await _tokenFactoryService.CreateJwtTokensAsync(token.User);
            await _tokenStoreService.AddUserTokenAsync(token.User, result.RefreshTokenSerial, result.AccessToken, _tokenFactoryService.GetRefreshTokenSerial(refreshTokenValue));
            await _uow.SaveChangesAsync();

            _antiforgery.RegenerateAntiForgeryCookies(result.Claims);

            return Ok(new { access_token = result.AccessToken, refresh_token = result.RefreshToken });
        }


        /// <summary>
        /// خروج کاربر از سامانه
        /// [AC3]
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<bool> Logout(string refreshToken)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userIdValue = claimsIdentity.FindFirst(ClaimTypes.UserData)?.Value;

            // The Jwt implementation does not support "revoke OAuth token" (logout) by design.
            // Delete the user's tokens from the database (revoke its bearer token)
            await _tokenStoreService.RevokeUserBearerTokensAsync(userIdValue, refreshToken);
            await _uow.SaveChangesAsync();

            _antiforgery.DeleteAntiForgeryCookies();

            return true;
        }

       

        /// <summary>
        ///  [AC4]
        ///  آیا کاربر فعلی اعتبارسنجی شده است؟
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]"), HttpPost("[action]")]
        public bool IsAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }



       



        #region Company users

        /// <summary>
        /// اضافه کردن کاربر پنل شرکت 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<UserInfoDto>> AddCompanyUser([FromBody]  CompanyUserRegister model)
        {
            if(model.CompanyId == null)
                model.CompanyId = _usersService.GetCurrentUserCompanyId();

            if (model.CompanyId == 0)
                return new ApiResult<UserInfoDto>(false, ApiResultStatusCode.BadRequest, null, "شناسه شرکت مشخص نشده است");


            return await _usersService.AddCompanyUser(model, model.CompanyId.Value);

        }


        /// <summary>
        /// بروزرسانی اطلاعات شرکت توسط مدیر و شرکت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<UserInfoDto>> UpdateCompanyUserAccount(UpdateCompanyAccount model)
        { 
            return await _usersService.UpdateCompanyUserAccount(model); 
        }

        /// <summary>
        /// دریافت اطلاعات کاربر
        /// </summary>
        /// <param name="companyId">شناسه کاربری شرکت</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCompanyUser")]
        [Authorize(Roles = "Admin,Company")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<List<UserInfoDto>>> GetCompanyUser(int? companyId)
        {
            if(companyId==null)
              companyId = _usersService.GetCurrentUserCompanyId();
            return await _usersService.GetCompanyUser(companyId.Value);
        }

        #endregion 







      
      


        /// <summary>
        /// ثبت نام اولیه شرکت ها
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<UserInfoDto>> CompanyRegister([FromBody]  CompanyRegister model)
        {

            var res = new ApiResult<UserInfoDto>(true, ApiResultStatusCode.Success, new UserInfoDto());

            res.Messages = "امکان ثبت نام برای واحدهای حقوقی تعریف نشده است";
            res.IsSuccess = false;
            return res;



            if (model==null)
            {
                res.Messages = "مدل ورود خالی می باشد";
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;


            }
             

            var isregister = await _usersService.CompanyRegisterAsync(model);
            if (isregister.IsSuccess)
            {
                var user = isregister.Data;
                var result = await _tokenFactoryService.CreateJwtTokensAsync(user);
                await _tokenStoreService.AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, null);
                await _uow.SaveChangesAsync();
                _antiforgery.RegenerateAntiForgeryCookies(result.Claims);
                res.Data.access_token = result.AccessToken;
                res.Data.refresh_token = result.RefreshToken;

            }
            else
            {
                res.Messages = isregister.Messages;
                res.IsSuccess = false;
            }
            return res;

        }


        /// <summary>
        /// تغییر کلمه عبور کاربر جاری
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<string>> ChangeCurrentUserPassword([FromBody]ChangePasswordViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NewPassword) || string.IsNullOrWhiteSpace(model.OldPassword))
                return new ApiResult<string>(false, ApiResultStatusCode.BadRequest, "", "مقادیر ورودی را به صورت کامل وارد نمایید");

           return    await _usersService.ChangeCurrentUserPasswordAsync(model);
        }
         


        #region تغییر کلمه عبور

        /// <summary>
        /// تغییر کلمه عبور شهروند
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiResult<ForgotVerifyCodeViewModel>> SendSmsForgotPassword(UserForgotPasswordViewModel model)
        {
            var data = new ForgotVerifyCodeViewModel();
            var res = new ApiResult<ForgotVerifyCodeViewModel>(true,  ApiResultStatusCode.Success, data);
            /*
            SimpleCaptcha captcha = new SimpleCaptcha();

            var enteredCaptchaCode = model.UserEnteredCaptchaCode;
            if(string.IsNullOrWhiteSpace(enteredCaptchaCode))
            {
                res.Messages = " کد امنیتی را به صورت صحیح وارد نمائید";
                res.IsSuccess = false;
                return res;
            } 
            enteredCaptchaCode = enteredCaptchaCode.Fa2En();

            bool isHuman = captcha.Validate(enteredCaptchaCode, model.CaptchaId);
            if (isHuman == false)
            {
                res.Messages = " کد امنیتی را به صورت صحیح وارد نمائید";
                res.IsSuccess = false;
                return res;
            }
            */
            if (string.IsNullOrEmpty(model.UserName))
            {
                res.Messages = "نام کاربری را وارد نمایید";
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;
            }

            var user =await _usersService.FindUserAsync(model.UserName);
            if (user == null)
            {
                res.Messages = "اطلاعات وارد شده صحیح نمی باشد";
                res.IsSuccess = false;
                res.StatusCode =  ApiResultStatusCode.BadRequest;
                return res;
            }
            if (string.IsNullOrWhiteSpace(model.MobileNumber))
            {
                res.Messages = "شماره موبایل را وارد نمایید";
                res.IsSuccess = false;
                return res;
            }

            model.MobileNumber = model.MobileNumber.Fa2En();
            if (user.MobileNumber != model.MobileNumber)
            {
                res.Messages = "اطلاعات وارد شده صحیح نمی باشد";
                res.IsSuccess = false;
                res.StatusCode =  ApiResultStatusCode.BadRequest;
                return res;
            }

           

            var mobilenumber = user.MobileNumber.Remove(4, 3);
            mobilenumber = mobilenumber.Insert(4, "***");

            var smsOption = await _siteSettingService.GetSmsSettingForSend();
            if (smsOption == null)
            {
                res.Messages = "تنظیمات پیامک انجام نشده است ";
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;
            }

            if (string.IsNullOrWhiteSpace(smsOption.SmsToken))
            {
                res.Messages = "تنظیمات پیامک انجام نشده است ";
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;
            }

            Random r = new Random();
            var code = r.Next(10000, 99999).ToString();
            SendSms sms = new SendSms();
            var smsLog = await sms.VerifyLookup(model.MobileNumber, code, TempleteNameEnum.ForgotPassword, smsOption.SmsToken);
            
            
            if (smsLog.IsSuccess)
            {
                smsLog.Data.Token2 = model.UserName;//دخیره کد ملی 
                var saveSms = await _smsInfoService.Add(smsLog.Data, user.Id);
                if (!saveSms.IsSuccess)
                {
                    return new ApiResult<ForgotVerifyCodeViewModel>(false, ApiResultStatusCode.BadRequest,null, saveSms.Messages);
                }
            }
            else
            {
                return new ApiResult<ForgotVerifyCodeViewModel>(false, ApiResultStatusCode.BadRequest, null, "در حاضر امکان ارسال پیامک وجود ندارد");

            }

             
            data.MobileNumber = user.MobileNumber;
            data.UserId = user.Id;
            res.Data = data; 
             res.Messages = "پیامک تایید تغییر شماره موبایل به شماره همراه شما ارسال گردید";
            return res;



        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<SetForgotPasswordViewModel>> CheckForgotVerifyCode(ForgotVerifyCodeViewModel model)
        {
            var res = new ApiResult<SetForgotPasswordViewModel>(true,  ApiResultStatusCode.Success, new SetForgotPasswordViewModel() { UserId = model.UserId,VerifyCode=model.VerifyCode,MobileNumber=model.MobileNumber });

             
            var lastSMS =await _smsInfoService.GetLastForgotCodeByMobileNumber(model.MobileNumber);
            if (lastSMS.IsSuccess==false)
            {
                res.Messages = "خطا:پیامکی ذخیره نشده است";
                res.IsSuccess = false;
                return res;
            }

            model.VerifyCode = model.VerifyCode.Fa2En();
            if (lastSMS.Data.Token1 != model.VerifyCode)
            {

                res.Messages = " کد وارد شده صحیح نمی باشد.";
                res.IsSuccess = false;
                res.StatusCode =  ApiResultStatusCode.BadRequest;
                return res;

            }

            return res;
        }

        
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult> SetNewPassword(SetForgotPasswordViewModel model)
        {
            var res = new ApiResult(true,  ApiResultStatusCode.Success ,"کلمه عبور شما با موفقیت ثبت شد");
          
            var lastSMS = await _smsInfoService.GetLastForgotCodeByMobileNumber(model.MobileNumber);
            if (lastSMS.IsSuccess == false)
            {
                res.Messages = "خطا:اطلاعات ارسالی صحیح نمی باشد";
                res.IsSuccess = false;
                return res;
            }
            model.VerifyCode = model.VerifyCode.Fa2En();
            if (lastSMS.Data.Token1 != model.VerifyCode)
            {

                res.Messages = "اطلاعات ارسالی صحیح نمی باشد";
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;

            }

            model.Password = model.Password.Fa2En();
            var operationId = _usersService.GetCurrentUserId();//  ,operationId
            var isChange = await _usersService.ChangeUserPasswordAsync(
                new AdminChangePasswordViewModel()
                {
                    NewPassword=model.Password,
                    UserId=model.UserId,
                    
                    

                } , operationId);
            if (!isChange.IsSuccess)
            {
                res.Messages = isChange.Messages;
                res.IsSuccess = false;
                res.StatusCode =  ApiResultStatusCode.BadRequest;
            } 

            return res;
        }


     
    

        #endregion  









    }
}