using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.Services;
using Nikan.Services.Citizens;
using Nikan.Services.Permissions;
using Nikan.ViewModel;
using Nikan.ViewModel.Citizens;

namespace Nikan.WebApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class AppServiceCitizenController : Controller
    {

        #region Ctor
        private readonly IAppService _app;
        private readonly IUsersService _usersService;
        private readonly IUserLoginTicketsService _usersTickets;
        private readonly IPermissionService _permission;
      

        public AppServiceCitizenController(IUsersService usersService, IPermissionService permission,
            IUserLoginTicketsService  usersTickets, IAppService  app)
        {
            _app = app;
            _permission =  permission;
            _usersService = usersService;
            _usersTickets = usersTickets;
        }
        #endregion 

        /// <summary>
        /// دریافت مشخصات یک سرویس
        /// [app1]
        /// </summary>
        /// <param name="id">شناسه سرویس</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAppInfo")]
        public async Task<ApiResult<AppServiceInfo>> GetAppInfo(int id)
        {
            if (id == 0)
                return new ApiResult<AppServiceInfo>(false, ApiResultStatusCode.BadRequest, null, "شناسه سرویس را وارد نمایید");

            return await _app.GetAppInfo(id);

        }



        /// <summary>
        /// اضافه یا ویرایش کردن یک سرویس
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> AddOrUpdate(AppServicesDto model)
        {
            if (model == null)
                return new ApiResult(false, ApiResultStatusCode.ServerError,   "مدل ورودی خالی است");
         
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.خدمات_شهروندی);
            if (!canAccess.IsSuccess)
                return new ApiResult(false, ApiResultStatusCode.NotAllowed,   "شما به خدمات دسترسی ندارید");


            return await _app.AddOrUpdate(model, userId);
        }


        /// <summary>
        /// حذف خدمت 
        /// </summary>
        /// <param name="id">شناسه خدمت</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Remove")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> Remove (int id)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.خدمات_شهروندی);
            if (!canAccess.IsSuccess)
                return new ApiResult(false, ApiResultStatusCode.NotAllowed, "شما به خدمات دسترسی ندارید");


            return await _app.Remove(id); 
        }


        /// <summary>
        /// دریافت همه خدمات
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllApp")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<PagedAppServiceViewModel>> GetAllApp(int? offset = 0, int? count = 20, string serviceName = null )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _app.GetAllApp(offset.Value, count.Value, serviceName);
        }



        /// <summary>
        /// فراخوانی سرویس توسط شهروند
        /// </summary>
        /// <param name="serviceId">شناسه سرویس</param>
        /// <param name="returnUrl"> مسیر برگشت شهروند  </param>
        /// <returns></returns>
        [Authorize(Roles = "Citizen")]
        [HttpGet]
        [Route("CallCitizenService")]
        public   async Task<ApiResult<string>> CallCitizenService(int serviceId,string returnUrl="")
        {
            var userId = _usersService.GetCurrentUserId();
            return await _usersTickets.CreateLoginTicket(serviceId, userId, returnUrl);

        }




        /// <summary>
        /// ایجاد توکن دسترسی به اطلاعات شهروند
        /// </summary>
        /// <param name="model">اطلاعات دسترسی</param>
        /// <returns></returns>
        [Authorize(Roles = "WebApiUser")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<string>> CreateAccessTicket(CreateAccessTicketDto model)
        {
            var userId = _usersService.GetCurrentUserId();
          
            //ببین این  کاربر حق ایجاد توکن دسترسی را دارد ؟
            var canAccess = await  _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.ایجاد_توکن_دسترسی);
            if (!canAccess.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, "کاربر توسعه دهنده به این سرویس دسترسی ندارد", "کاربر توسعه دهنده به این سرویس دسترسی ندارد");

            //ببین این کاربر   به این سرویس دسترسی دارد ؟
            var canAccessService = await _usersService.UserCanAccessToService(userId , model.ServiceId);
            if (!canAccessService.IsSuccess)
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, "کاربر توسعه دهنده به این خدمت دسترسی ندارد", "کاربر توسعه دهنده به این خدمت دسترسی ندارد");


            return await _usersTickets.CreateLoginByTicketUrl(model,  userId,model.ReturnUrl);

        }

      




        /// <summary>
        /// داشتن منوهای داشبورد
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAppDashbordList")]
        [Authorize(Roles = "Citizen")]
        public async Task<ApiResult<List<AppServiceInfo>>> GetAppDashbordList()
        {
            var userId = _usersService.GetCurrentUserId();
            return await _app.GetAppDashbordList(userId);
        }


        /// <summary>
        /// دریافت همه خدمات شهروندی
        /// خدمات فعال
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBaseListAppService")]
        [AllowAnonymous]
        public async Task<ApiResult<List<BaseDataModel>>> GetBaseListAppService()
        { 
            return await _app.GetBaseListAppService();
        }


         




    }
}