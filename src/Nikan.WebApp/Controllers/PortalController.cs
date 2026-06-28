using cle.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.Services.Events;
using Nikan.Services.Permissions;
using Nikan.ViewModel;
using Nikan.ViewModel.BsseEntity;
using Nikan.ViewModel.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nikan.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize(Policy = CustomRoles.Admin)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PortalController : Controller
    {

        #region Ctor

        private readonly IUsersService _usersService; 
        private readonly ISiteSettingService _siteSettingService;
        private readonly ISmsInfoService _smsService;
        private readonly IEventService _event;
        private readonly IPermissionService _permission;




        public static IHostingEnvironment _environment;
        public PortalController (
            ISiteSettingService siteSettingService,
             IPermissionService permission,
             ISmsInfoService smsService, 
             IHostingEnvironment environment,
              IUsersService userManager,
              IEventService siteevent

            )
        {
            _smsService = smsService;
            _environment = environment;
            _siteSettingService = siteSettingService;
            _event = siteevent;
            _permission = permission;
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));

        }
        #endregion



        #region تنظیمات سایت

        /// <summary>
        /// دریافت تنظیمات سایت
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("GetSettings")]
        public async Task<ApiResult<EditSettingViewModel>> GetSettings()
        {
            return await _siteSettingService.GetSettingsForEdit();

        }

        [HttpGet]
        [Route("GetSiteInfo")]
        [AllowAnonymous]
        public async Task<ApiResult<SiteInfo>> GetSiteInfo()
        {
            return await _siteSettingService.GetSiteInfo();

        }



        /// <summary>
        /// تنطیم تنطیمات سایت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<EditSettingViewModel>> UpdateSettings(EditSettingViewModel model)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.تنظیمات_سامانه);
            if (!canAccess.IsSuccess)
                return new ApiResult<EditSettingViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به تنظیمات سامانه دسترسی ندارید");


            return await _siteSettingService.UpdateSettings(model, userId);

        }

        #endregion  
        
        #region UploadImage
        [HttpPost]
        [Route("UploadImage")]
        [DisableRequestSizeLimit]
        [IgnoreAntiforgeryToken]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<string>> UploadImage(IFormFile file)
        {
            var response = new ApiResult<string>(false, ApiResultStatusCode.Success, "");
            try
            {

                var filesPath = _environment.WebRootPath + "/uploads/Resources/Images/Portal/Logo";
                if (!System.IO.Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }
                var fileSize = file.Length;
                if (fileSize > 0)
                {
                    var megLength = (fileSize / 1048576.0); //(i.e. 1024 * 1024): 
                    if (megLength > 1)
                    {
                        response.IsSuccess = false;
                        response.Messages = "حجم تصویر بیش از حد مجاز می باشد.";
                        return response;

                    }
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string extension = System.IO.Path.GetExtension(file.FileName);

                    var shortPath = "/uploads/Resources/Images/Portal/Logo/Logo.png";
                    string FilePath = _environment.WebRootPath + shortPath;

                    if (System.IO.File.Exists(FilePath))
                        System.IO.File.Delete(FilePath);

                    using (FileStream fs = System.IO.File.Create(FilePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();

                    }


                    response.Data = shortPath;
                    response.IsSuccess = true;


                }
                else
                {
                    response.IsSuccess = false;
                    response.Messages = "خطا در آپلود لوگو  !";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Messages = $"خطا در آپلود   لوگو: {ex}";

            }
            return response;
        }





        #endregion
         

        #region Sms
        


        [HttpGet]
        [Route("GetPagedSmsList")]
        public async Task<ApiResult<PagedSmsInfoViewModel>> GetPagedSmsList(
          int? offset = 1,
          int? count = 20,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          string mobileNumber = null,
          string smsText = null,
          string companyName = null,
          int? contractCode = null,
          bool? isArchive = false)
        {

            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.تنظیمات_پیامک);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<PagedSmsInfoViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");
            }

            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;


            return await this._smsService.GetPagedSmsListAsync(offset.Value, count.Value, FromDate, ToDate, 
                mobileNumber, smsText, companyName, contractCode, isArchive);
             
        }

        [HttpGet]
        [Route("SendToArchive")]
        public async Task<ApiResult> SendToArchive()
        {
            ApiResult archive = await this._smsService.TransToArchive();
            return archive;
        }



        /// <summary>
        /// میزان اعتبار پیامک به ریال جهت نمایش در داشبورد مدیریت
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCredit")]
        public async Task<ApiResult<string>> GetCredit()
        {
            SendSms sms = new SendSms();
            var smsSettings = await _siteSettingService.GetSmsSettingForSend();
            var Credit = 0;// sms.GetCredit(smsSettings);
            return new ApiResult<string>(true, ApiResultStatusCode.Success, Credit.ToString(), "میزان اعتبار پیامک به ریال");


        }

        #endregion

        #region تنظیمات پیامک
        [HttpGet]
        [Route("GeSmsSettings")]
        public async Task<ApiResult<SmsOption>> GeSmsSettings()
        {
            return await _siteSettingService.GetSmsSettings();

        }

        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<SmsOption>> UpdateSmsSettings(SmsOption model)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.تنظیمات_پیامک);
            if (!canAccess.IsSuccess)
                return new ApiResult<SmsOption>(false, ApiResultStatusCode.NotAllowed, null, "شما به تنظیمات پیامک دسترسی ندارید");



            return await _siteSettingService.UpdateSmsSettings(model);

        }

        #endregion
        #region رویدادها
        [HttpGet]
        [Route("GetTopEvent")]
        public async Task<ApiResult<List<EventsInfo>>> GetTopEvent(int? top = 50)
        {
            if (top == null) top = 50;
            return await _event.GetTopEvent(top.Value);

        }

        [HttpGet]
        [Route("GetCitizenTopEvent")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<List<EventsInfo>>> GetCitizenTopEvent(string userCode,  int? top = 50)
        {
            if (top == null) top = 50;
            return await _event.GetCitizenTopEvent(userCode, top.Value);

        }




        [HttpGet]
        [Route("GetEvent")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<EventsInfo>> GetEvent(int id)
        {
            return await _event.GetEvent(id);
        }


        #endregion 

        #region تنظیمات مالی
        [HttpGet]
        [Route("GetFinancialSettings")]
        public async Task<ApiResult<FinancialSettings>> GetFinancialSettings()
        {
            return await _siteSettingService.GetFinancialSettings();

        }


        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<FinancialSettings>> UpdateFinancialSettings(FinancialSettings model)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.تنظیمات_مالی);
            if (!canAccess.IsSuccess)
                return new ApiResult<FinancialSettings>(false, ApiResultStatusCode.NotAllowed, null, "شما به تنظیمات سامانه دسترسی ندارید");


            return await _siteSettingService.UpdateFinancialSettings(model);

        }

        #endregion


 
     

    
       
       





    }


}