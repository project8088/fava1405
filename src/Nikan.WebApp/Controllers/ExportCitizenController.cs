using cle.Services;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Nikan.Common;
using Nikan.Common.ApiCall;
using Nikan.Common.GlobalEnum;
using Nikan.Common.Utilities;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.Services.Citizens;
using Nikan.Services.Events;
using Nikan.Services.ExportCitizen;
using Nikan.Services.Permissions;
using Nikan.ViewModel.Citizens;
using Nikan.ViewModel.ExportCitizen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ExportCitizenController : Controller
    {

        #region Ctor

        private readonly IUsersService _usersService;
        public static IHostingEnvironment _environment;
        private readonly ICitizenService _citzene; 
        private readonly ISmsInfoService _smsInfoService;
        private readonly ISiteSettingService _siteSettingService; 
        private readonly IExportCitizenService _exportCitizen;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IMemoryCache _cache;
        private readonly IEventService _event;
        private readonly IPermissionService _permission;

        public ExportCitizenController(
            IUsersService userManager,
            ICitizenService citzene,
            IPermissionService permission,
            IHostingEnvironment environment,
            IBackgroundJobClient backgroundJobClient,
              IMemoryCache memoryCache,
            ISmsInfoService smsInfoService, 
              IEventService siteevent,
            ISiteSettingService siteSettingService,
            IExportCitizenService  exportCitizen

            )
        {
            _citzene = citzene;
            _exportCitizen =  exportCitizen;
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));
            _environment = environment; 
            _siteSettingService = siteSettingService; 
            _smsInfoService = smsInfoService;
            _backgroundJobClient = backgroundJobClient;
            _cache =  memoryCache;
            _event = siteevent;
            this._permission = permission;
        }
        #endregion

        /// <summary>
        /// دریافت لیست ثبت احوال
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllExportSabtAhval")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<PagedExportCitizenViewModel>> GetAllExportSabtAhval(
           int? offset = 1, int? count = 20,
            DateTime? FromDate = null,
         DateTime? ToDate = null
         , int? exportNumber = null
           )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _exportCitizen.GetAllExportSabtAhval(offset.Value, count.Value, FromDate, ToDate, exportNumber);

        }


        /// <summary>
        /// دریافت همه شهروندانی که در یک خروجی هستند
        /// </summary>
        /// <param name="exportId">شناسه خروجی</param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllCitizenExported")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<PagedExportedCitizenViewModel>> GetAllCitizenExported(
          int exportId,
           int? offset = 1, int? count = 20 )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _exportCitizen.GetAllCitizenExported(exportId,offset.Value, count.Value );

        }



        /// <summary>
        /// ارسال شهروندان به خروجی ثبت احوال
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<ExportCitizensInfo>> ExportCitizenForSabtAhval(ExportedCitizenForSabtAhvalDto model)
        {
           var  userId = _usersService.GetCurrentUserId();
            return await _exportCitizen.ExportCitizenForSabtAhval(model, userId);
        }




        /// <summary>
        /// حذف خروجی ثبت احوال
        /// </summary>
        /// <param name="id">شناسه خروجی</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("RemoveExport")]
        public async Task<ApiResult<string>> RemoveExport(int id)
        { 
            return await _exportCitizen.Remove(id); 
        }


        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        [IgnoreAntiforgeryToken]
        public async Task<ApiResult<string>> SendSabtAhvalCitizensSms([FromBody]SendSmsForSabtAhvalCitizens model)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success,"", "پیامک  گروهی با موفقیت ارسال شد");

            if (model == null)
                return new ApiResult<string>(false, ApiResultStatusCode.BadRequest, null, "");
            if (!model.Ids.Any())
                return new ApiResult<string>(false, ApiResultStatusCode.BadRequest, null, "هیچ ردیفی برای ارسال پیامک انتخاب نشده است");

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
            SendSms sms = new SendSms();

            var getcitizenList = await _citzene.GetShortCitizenInfoByIds(model.Ids);
            if(getcitizenList.IsSuccess)
            {
                var citizenList = getcitizenList.Data.Where(w=>w.SabtStatus==SabtStatusEnum.عدم_تایید);
                foreach (var item in citizenList)
                {
                    var name = item.FirstName + " " + item.LastName;
                    if (name.Length > 25)
                        name = name.Substring(0, 24); 
                    var mobile = item.MobileNumber; 
                    if (!string.IsNullOrWhiteSpace(mobile))
                    {
                        var smsLog = await sms.VerifyLookup(mobile, "کدملی", "", "", name, TempleteNameEnum.smsinfoupdate, smsOption.SmsToken);
                        if (smsLog.IsSuccess)
                        {
                            smsLog.Data.Token2 = item.NationCode;//دخیره کد ملی 
                            var saveSms = await _smsInfoService.Add(smsLog.Data, item.CitizenId);
                        }

                    }
                }
                return res;
            
            }
            else
            {
                return new ApiResult<string>(false, ApiResultStatusCode.BadRequest, getcitizenList.Messages, getcitizenList.Messages);


            }


        }


        [HttpGet]
        [Route("SendOnlineAuthentication")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> SendOnlineAuthentication(int exportId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت برای بررسی هویت ارسال گردید");
            var userId = _usersService.GetCurrentUserId();
            var getlist = await _exportCitizen.GetOnlineAuthentication(exportId);
            if (getlist.IsSuccess)
            {
                var list = getlist.Data;
                if (list.Any())
                {
                    CommitCitizenForAuthenticationJob(getlist.Data);
                }

                await _exportCitizen.Send(exportId, userId);

            }
            return res;

        }




     


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("CommitCitizenForAuthenticationJob")]
        public  void  CommitCitizenForAuthenticationJob(List<ExportedCitizensInfo> list )
        {
            try
            {
                var t = 1;
                foreach (var item in list)
                {

                    if(!string.IsNullOrWhiteSpace(item.NationCode))
                    {
                        item.NationCode = item.NationCode.Fa2En();
                        if (item.NationCode.Length == 10)
                        {
                            _backgroundJobClient.Schedule(() => SendCitizenForAuthenticationJobAsync(item), TimeSpan.FromSeconds(t * 40));

                        }
                        t++;
                    } 
                } 
            }
            catch (Exception er)
            {

                 
            }
           
           
        }



     



        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("SendCitizenForAuthenticationJobAsync")]
        public   async Task SendCitizenForAuthenticationJobAsync(ExportedCitizensInfo item)
        {


            var citizenSabtStatus = await _citzene.GetCitizenSabtStatus(item.NationCode);
            if (citizenSabtStatus.IsSuccess && citizenSabtStatus.Data.SabtStatus == Common.GlobalEnum.SabtStatusEnum.تایید)
                return; //اطلاعات شهروند تایید شده است استعلام مجدد نشود



            var userId = _usersService.GetCurrentUserId();
            var call = new ItsaazApi(_cache);
            var token =await call.AutenticationApi();
            if (string.IsNullOrWhiteSpace(token))
            {
             
                await _event.AddEvent(new ViewModel.Events.EventDto()
                {
                    Description = "خطا در دریافت توکن سرویس استعلام",
                    UserId = userId,
                    ActionName = "SendCitizenForAuthenticationJobAsync",
                    EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                    EventSection = Common.GlobalEnum.EventSectionEnum.خطای_سیستمی,
                    EventType = Common.GlobalEnum.EventTypeEnum.Error,
                    OperationId = userId 
                });
                return;
            }


            DateTimeToPersianDateTimeConverter persianDate = new DateTimeToPersianDateTimeConverter("/", false);
            if (item.BirthDate != null)
            { 
                var date = persianDate.toShamsiDateTime(item.BirthDate.Value);
                var getCitizenInfo = await call.GetData(new GetDataParam() { BirthDate = date, Token = token, NationalCode = item.NationCode });
                if (getCitizenInfo != null)
                {
                    getCitizenInfo.nationalCode = item.NationCode;
                    getCitizenInfo.birthDate = date;
                    await _citzene.UpdateCitizenAfterAuthentication(getCitizenInfo, userId);

                }
                else
                {
                    
                    await _event.AddEvent(new ViewModel.Events.EventDto()
                    {
                        Description = "خطا  در استعلام" + call.ErrorMessage,
                        UserId = userId,
                        ActionName = "SendCitizenForAuthenticationJobAsync",
                        EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                        EventSection = Common.GlobalEnum.EventSectionEnum.خطای_سیستمی,
                        EventType = Common.GlobalEnum.EventTypeEnum.Error,
                        OperationId = userId
                    });
                    return;


                }






            }
        }

        

        [HttpGet]
        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [Route("CitizenForAuthenticationByAdmin")]
        public async Task<ApiResult<ItsaazData>> CitizenForAuthenticationByAdmin(string nationCode, DateTime birthDate)
        {
            var res = new ApiResult<ItsaazData>(true, ApiResultStatusCode.Success, new ItsaazData(), "با موفقیت  احراز هویت  گردید");

            if(string.IsNullOrEmpty(nationCode))
            {
                res.Messages = "کد ملی را وارد نمایید";
                res.IsSuccess = false;
                return res;
            }

            nationCode = nationCode.Fa2En();
            if (nationCode.Length!=10)
            {
                res.Messages = "طول کد ملی باید 10 کارکتر باشد";
                res.IsSuccess = false;
                return res;
            }

            int userId = this._usersService.GetCurrentUserId();
            var call = new ItsaazApi(_cache);
            var token =await call.AutenticationApi();
            if(string.IsNullOrWhiteSpace(token))
            {
                res.Messages = call.ErrorMessage==null ? "عدم ارتباط با سرویس استعلام": call.ErrorMessage;
                res.IsSuccess = false;
                return res;
            }
            try
            {
                DateTimeToPersianDateTimeConverter persianDate = new DateTimeToPersianDateTimeConverter("/", false);

                var date = persianDate.toShamsiDateTime(birthDate);
                var getCitizenInfo = await call.GetData(new GetDataParam() { BirthDate = date, Token = token, NationalCode = nationCode });
                if (getCitizenInfo != null)
                {
                    getCitizenInfo.birthDate = date;
                    getCitizenInfo.nationalCode = nationCode;
                    await _citzene.UpdateCitizenAfterAuthentication(getCitizenInfo, userId);
                    res.Data = getCitizenInfo;
                }
                else
                {
                    res.Messages = call.ErrorMessage;
                    res.IsSuccess = false;
                    return res;
                }

            }
            catch (Exception er)
            {
                res.Messages = er.Message;
                res.IsSuccess = false;
            }
            return res;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Card")] 
        [Route("CitizenForAuthenticationByCitizenId")]
        public async Task<ApiResult<string>> CitizenForAuthenticationByCitizenId(int citizenId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success,"اطلاعات وارد شده صحیح می باشد", "با موفقیت  احراز هویت  گردید");

            if (citizenId==0)
            {
                res.Messages = "شناسه شهروندی وارد نمایید";
                res.IsSuccess = false;
                return res;
            }

            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.احراز_هویت_شهروند);
            if (!canAccess.IsSuccess)
            {
                canAccess = await this._permission.CanAccessWebApi(userId, PermissionTypeEnum.احراز_هویت_شهروند_کارت);
                if (!canAccess.IsSuccess)
                    return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, "شما به احراز هویت شهروند دسترسی ندارید", "شما به احراز هویت شهروند دسترسی ندارید");
            }

             


            var getcitizen = await _citzene.GetShortCitizenInfo(citizenId);

            if (!getcitizen.IsSuccess)
            {
                res.Messages = getcitizen.Messages;
                res.IsSuccess = false;
                return res;
            }

            if(getcitizen.Data.SabtStatus==Common.GlobalEnum.SabtStatusEnum.تایید) 
            {
                res.Messages = "اطلاعات شهروند قبلا تایید شده است و امکان استعلام مجدد وجود ندارد";
                res.IsSuccess = false;
                return res;
            }
            var call = new ItsaazApi(_cache);
            var token =await call.AutenticationApi();
            if (string.IsNullOrWhiteSpace(token))
            {
              

                res.Messages = call.ErrorMessage == null ? "عدم ارتباط با سرویس استعلام" : call.ErrorMessage;
                res.IsSuccess = false;
                await _event.AddEvent(new ViewModel.Events.EventDto()
                {
                    Description = "خطا در دریافت توکن سرویس استعلام",
                    UserId = userId,
                    ActionName = "CitizenForAuthenticationByCitizenId",
                    EventPriority = Common.GlobalEnum.EventPriorityEnum.Normal,
                    EventSection = Common.GlobalEnum.EventSectionEnum.خطای_سیستمی,
                    EventType = Common.GlobalEnum.EventTypeEnum.Error,
                    OperationId = userId
                });
                return res;
            }
            try
            {
                DateTimeToPersianDateTimeConverter persianDate = new DateTimeToPersianDateTimeConverter("/", false);

                var date = persianDate.toShamsiDateTime(getcitizen.Data.BirthDate.Value);
                var getCitizenInfo = await call.GetData(new GetDataParam() { BirthDate = date, Token = token, NationalCode = getcitizen.Data.NationCode });
                if (getCitizenInfo != null)
                {
                    getCitizenInfo.birthDate = date;
                    getCitizenInfo.nationalCode = getcitizen.Data.NationCode;
                    await _citzene.UpdateCitizenAfterAuthentication(getCitizenInfo, userId);
                    if (getCitizenInfo.isMatch == true)
                    {
                        res.Messages = "اطلاعات وارد شده صحیح می باشد";
                    }
                    else if (getCitizenInfo.isMatch == false)
                    {
                        res.Messages = "اطلاعات وارد شده صحیح نمی باشد";
                        res.IsSuccess = false;
                        return res;
                    }
                    else
                    {

                        res.Messages = " پاسخی از سمت سرویس استعلام دریافت نشد ";
                        res.IsSuccess = false;
                        return res;

                    }
                }
                else
                {
                    res.Messages = call.ErrorMessage;
                    res.IsSuccess = false;
                    return res;
                }

            }
            catch (Exception er)
            {
                res.Messages = er.Message;
                res.IsSuccess = false;
            }
            return res;
        }

    

        #region استعلام وضعیت فوتی

        [HttpGet]
        [Route("SendOnlineAuthenticationByBagRezvanService")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> SendOnlineAuthenticationByBagRezvanService(int exportId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت برای بررسی هویت ارسال گردید");
            var userId = _usersService.GetCurrentUserId();
            var getlist = await _exportCitizen.GetOnlineAuthenticationChekStateLife(exportId);
            if (getlist.IsSuccess)
            {
                var list = getlist.Data;
                if (list.Any())
                {
                    CommitCitizenForChekStateLifeJob(getlist.Data, exportId);
                }

                await _exportCitizen.Send(exportId, userId);

            }
            return res;

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("CommitCitizenForChekStateLifeJob")]
        public void CommitCitizenForChekStateLifeJob(List<ExportedCitizensInfo> list, int exportId)
        {
            try
            {
                var t = 1;
                foreach (var item in list)
                {

                    if (!string.IsNullOrWhiteSpace(item.NationCode))
                    {
                        item.NationCode = item.NationCode.Fa2En();
                        if (item.NationCode.Length == 10)
                        {
                            _backgroundJobClient.Schedule(() => SendCitizenForChekStateLifeJobAsync(item, exportId), TimeSpan.FromSeconds(t * 60));

                        }
                        t++;
                    }
                }
            }
            catch (Exception er)
            {


            }


        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("SendCitizenForChekStateLifeJobAsync")]
        public async Task SendCitizenForChekStateLifeJobAsync(ExportedCitizensInfo item, int exportId)
        {

            var bagrezvan = new BagRezvanService();
            var CheckNationCodes = new List<string>
            {
                item.NationCode
            };
            var checkList = bagrezvan.CheckNationCodes(CheckNationCodes);
            if (checkList != null)
            {
                foreach (var check in checkList)
                {
                    await _citzene.UpdateCheckDeathStateCitizen(item.NationCode, check.Exist, exportId);
                   
                } 
            }
        }
        #endregion 












    }
}