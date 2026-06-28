using cle.Services;
using cle.Services.Faq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.Services;
using Nikan.Services.BaseEntity;
 
using Nikan.Services.Permissions;
using Nikan.Services.UserDocuments;
using Nikan.ViewModel;
using Nikan.ViewModel.Permissions;
using Nikan.ViewModel.Ticket;
using Nikan.ViewModel.UserDocuments;
using Nikan.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace Nikan.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize()]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UsersController : Controller
    {


        #region Ctor
        private readonly IUsersService _usersService;
        private readonly ITokenStoreService _tokenStoreService; 
        private readonly IAntiForgeryCookieService _antiforgery;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly ISmsInfoService _smsInfoService;
        private readonly ISiteSettingService _siteSettingService;
        private readonly IPermissionService _permission;
        private readonly IPermissionGroupService _permissionGroup;
        private readonly IUserDocumentService _userDocument;
        private readonly IUserDocumentGroupService _userDocumentGroup;
        private readonly IRolesService _role;


        public UsersController(
            IUsersService usersService,
            ISmsInfoService smsInfoService,
              IPermissionService permission,
              IUserDocumentService userDocument,
              IRolesService role,
              IUserDocumentGroupService userDocumentGroup,
            ITokenStoreService tokenStoreService,
            ITokenFactoryService tokenFactoryService,
            IPermissionGroupService permissionGroup,
            ISiteSettingService siteSettingService,
            IAntiForgeryCookieService antiforgery)
        {
            _usersService = usersService;
            _usersService.CheckArgumentIsNull(nameof(usersService));
            _userDocument = userDocument;
            _userDocumentGroup = userDocumentGroup; 
            _tokenStoreService = tokenStoreService;
            _tokenStoreService.CheckArgumentIsNull(nameof(tokenStoreService));
            _siteSettingService = siteSettingService;
            _permissionGroup = permissionGroup; 
            _smsInfoService = smsInfoService;
            _antiforgery = antiforgery;
            _role = role;
            _antiforgery.CheckArgumentIsNull(nameof(antiforgery));
            _permission = permission;
            _tokenFactoryService = tokenFactoryService;
            _tokenFactoryService.CheckArgumentIsNull(nameof(tokenFactoryService));
        }
        #endregion



        #region Groups
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<PermissionGroupDto>> AddOrUpdateGroups([FromBody]PermissionGroupDto model)
        {

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.گروههای_کاربری);
            if (!canAccess.IsSuccess)
                return new ApiResult<PermissionGroupDto>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");








            if (model == null)
                return new ApiResult<PermissionGroupDto>(false, ApiResultStatusCode.BadRequest, null, "اطلاعات ورودی معتبر نمی باشد");

            if (string.IsNullOrWhiteSpace(model.Name))
                return new ApiResult<PermissionGroupDto>(false, ApiResultStatusCode.BadRequest, null, "نام گروه را وارد نمایید");






            return await _permissionGroup.AddOrUpdate(model);
        }


        [Route("RemoveGroups")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> RemoveGroups(int id)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.گروههای_کاربری);
            if (!canAccess.IsSuccess)
                return new ApiResult (false, ApiResultStatusCode.NotAllowed,  "شما به این قسمت دسترسی ندارید");








            return await _permissionGroup.Remove(id); 
        }


        [HttpGet]
        [Route("GetAllGroups")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<PermissionGroupDto>>> GetAllGroups()
        {

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.گروههای_کاربری);
            if (!canAccess.IsSuccess)
                return new ApiResult<List<PermissionGroupDto>>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");




            return await _permissionGroup.GetAll();

        }


        [HttpGet]
        [Route("GetGroup")]
        [Authorize(Roles = "Admin")]
        public async   Task<ApiResult<PermissionGroupDto>> GetGroup(int id)
        {
            return await _permissionGroup.GetGroup(id);

        }



        [HttpGet]
        [Route("GetAccessGroups")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<BaseDataModel>>> GetAccessGroups(int? selected)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.گروههای_کاربری);
            if (!canAccess.IsSuccess)
                return new ApiResult<List<BaseDataModel>>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");

            return await _permissionGroup.GetGroups(selected);

        }




        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public   async Task<ApiResult> AddUserToPermissionGroup(UserPermissionGroupDto model)
        {
            if (model == null)
                return new ApiResult (false, ApiResultStatusCode.BadRequest,  "اطلاعات ورودی معتبر نمی باشد");
            
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران);
            if (!canAccess.IsSuccess)
                return new ApiResult (false, ApiResultStatusCode.NotAllowed,  "شما به این قسمت دسترسی ندارید");










            return await _usersService.AddUserToPermissionGroup(model, userId);
        }



        [HttpGet]
        [Route("RemoveUserPermissionGroup")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> RemoveUserPermissionGroup(int permissionGroupId, int userId)
        {
            var operationId = _usersService.GetCurrentUserId();//  ,operationId

            
            var canAccess = await _permission.CanAccess( operationId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران);
            if (!canAccess.IsSuccess)
                return new ApiResult (false, ApiResultStatusCode.NotAllowed,   "شما به این قسمت دسترسی ندارید");

             



            return await _usersService.RemoveUserPermissionGroup(
                new UserPermissionGroupDto() {PermissionGroupId= permissionGroupId, UserId= userId }, operationId);

        }



        [HttpGet]
        [Route("GetAllUserPermissionGroup")]
        [Authorize(Roles = "Admin")]
        public async  Task<ApiResult<List<UserPermissionGroupInfo>>> GetAllUserPermissionGroup(int id)
        {
            return await _usersService.GetAllUserPermissionGroup(id);

        }





 
        #endregion


        #region UserPermissions 



        /// <summary>
        /// دسترسی کاربران وب سرویس
        /// </summary>
        /// <param name="userId">شناسه کاربری</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetWebApiPermissionList")]
        [Authorize(Roles = "Admin")]
        public async Task<Dictionary<string, IEnumerable<BaseDataModel>>> GetWebApiPermissionList(int userId)
        {
            var items = await _permission.GetWebApiPermissionList(userId);
            return items;
        }




        /// <summary>
        /// دریافت لیست مجوزهای یک گروه
        /// </summary>
        /// <param name="groupId">شناسه گروه کاربری</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPermissionList")]
        [Authorize(Roles = "Admin")]
        public async Task<Dictionary<string, IEnumerable<BaseDataModel>>> GetPermissionList(int groupId)
        {
            var items = await _permission.GetPermissionList(groupId);
            return items;
        } 
        /// <summary>
        /// دریافت لیست منوهای کاربر
        /// </summary>
        /// <param name="groupId">شناسه گروه</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPermissionAdminMenu")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<string>>> GetPermissionMenu(int groupId)
        {
            return await _permission.GetPermissionAdminMenuByGroupId(groupId);
        }



        



        /// <summary>
        /// اعمال مجوزهای یک کاربر
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult> AddPermissions(UserPermissions model)
        {

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران);
            if (!canAccess.IsSuccess)
                return new ApiResult(false, ApiResultStatusCode.NotAllowed, "شما به این قسمت دسترسی ندارید");



            return await _permission.AddPermissions(model);
        }



        /// <summary>
        /// اضافه کردن دسترسی به توسعه دهندگان
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult> AddWebApiUserPermissions(WebApiUserPermissions model)
        {

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران);
            if (!canAccess.IsSuccess)
                return new   ApiResult(false, ApiResultStatusCode.NotAllowed,   "شما به این قسمت دسترسی ندارید");








            return await _permission.AddWebApiUserPermissions(model);
        }



        #endregion



        #region Card

        [HttpGet]
        [Route("GetCardPermissionList")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<Dictionary<string, IEnumerable<BaseDataModel>>> GetCardPermissionList(int userId)
        {
            var items = await _permission.GetCardPermissionList(userId);
            return items;
        }

        [Authorize(Roles = "Admin,Card")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult> AddCardUserPermissions(WebApiUserPermissions model)
        {



            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccessWebApi(userId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران_کارت);
            if (!canAccess.IsSuccess)
                return new ApiResult<PermissionGroupDto>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");

             


            return await _permission.AddWebApiUserPermissions(model);
        }


        #endregion 
















        /// <summary>
        /// دریافت لیست کاربران مدیریتی
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAdminUser")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<List<UserInfoDto>>> GetAdminUser()
        {

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران);
            if (!canAccess.IsSuccess)
                return new ApiResult<List<UserInfoDto>>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");


            return await _usersService.GetAdminUser();
        }

        /// <summary>
        /// دریافت لیست کاربران مدیرت کارت
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCardUser")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<List<UserInfoDto>>> GetCardUser()
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران_کارت);
            if (!canAccess.IsSuccess)
                return new ApiResult<List<UserInfoDto>>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");



            return await _usersService.GetCardUser();
        }

        [HttpGet]
        [Route("DeleteCardUser")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult> DeleteCardUser(int userId)
        {
            var operationId = _usersService.GetCurrentUserId();//  ,operationId
            var canAccess = await _permission.CanAccess(operationId , Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران_کارت);
            if (!canAccess.IsSuccess)
                return new ApiResult (false, ApiResultStatusCode.NotAllowed,  "شما به این قسمت دسترسی ندارید");



            
            return await _usersService.DeleteCardUser(userId,operationId);
        }

        /// <summary>
        /// اضافه کردن کاربر مدیریت 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<UserInfoDto>> AdminRegister([FromBody]  AdminRegister model)
        {
            var operationId = _usersService.GetCurrentUserId();//  ,operationId
            return await _usersService.AdminRegister(model,operationId);

        }

        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult> AddCardUser([FromBody]  CardUserRegister model)
        {
            var operationId = _usersService.GetCurrentUserId();//  ,operationId
            return await _usersService.AddCardUser(model,operationId);

        }

        /// <summary>
        /// جستجوی اطلاعات کاربر سمت مدیریت
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="username"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchUsers")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<PagedUsersViewModel>> SearchUsers(
            int? offset = 1, int? count = 20,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
           string username = null,
            int? roleId = null ,
            userAccountStateEnum? userAccountState = null
           )
        {

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران);
            if (!canAccess.IsSuccess)
                return new ApiResult<PagedUsersViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");


            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;

            return await _usersService.SearchUsers(offset.Value, count.Value, FromDate, ToDate, username, roleId,userAccountState);
        }


        [HttpGet]
        [Route("SearchCardUser")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<PagedUsersViewModel>> SearchCardUser(
            int? offset = 1, int? count = 20,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
           string username = null,
            int? roleId = null 
           )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;

            return await _usersService.SearchCardUser(offset.Value, count.Value, FromDate, ToDate, username, roleId);
        }






        [HttpGet]
        [Route("SearchAminUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<PagedUsersViewModel>> SearchAminUser(
           int? offset = 1, int? count = 20,
           DateTime? FromDate = null,
           DateTime? ToDate = null,
          string username = null,
           int? roleId = null
          )
        {

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران);
            if (!canAccess.IsSuccess)
                return new ApiResult<PagedUsersViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");




            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;

            return await _usersService.SearchAdminUser(offset.Value, count.Value, FromDate, ToDate, username, roleId);
        }










        /// <summary>
        /// دریافت اطلاعات کاربر
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserAccountInfo")]
        public async Task<ApiResult<UserInfoDto>> GetUserAccountInfo(int userId)
        {
            return await _usersService.GetUserInfo(userId);
        }


        /// <summary>
        /// تغییر کلمه عبور کاربر
        /// توسط مدیریت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<string>> ChangeUserPassword(AdminChangePasswordViewModel model)
        {
            var operationId = _usersService.GetCurrentUserId();//  ,operationId
            return await _usersService.ChangeUserPasswordAsync(model ,operationId);
        }



        /// <summary>
        /// ویرایش اطلاعات کاربر مدیریت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<UserInfoDto>> UpdateAccount([FromBody]  UpdateAccount model)
        {
            var operationId = _usersService.GetCurrentUserId();//  ,operationId
            return await _usersService.UpdateAdminAccount(model,operationId);

        }




        #region UserDocGroups

        /// <summary>
        /// دریافت لیست گروههای مدارک
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDocGroupsBaseList")]
        [Authorize(Roles = "Citizen,Admin,Operator")]
        public async Task<ApiResult<List<BaseDataModel>>> GetDocGroupsBaseList(int? selected = null)
        {
            return await _userDocumentGroup.GetBaseListAsync(selected);
        }
         

        /// <summary>
        /// لیست مدارکی که شهروند قبلا بارگذاری کرده است
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserDocuments")]
        [Authorize(Roles = "Citizen,Admin,Operator")]
        public async Task<ApiResult<List<UserDocumentInfo>>> GetUserDocuments(int? userId)
        {
            if (userId == null) userId = _usersService.GetCurrentUserId();
            return await _userDocument.GetUserDocuments(userId.Value);
        }

        /// <summary>
        /// دریافت همه گروههای اسناد به همراه مدارک بارگذاری شده شهرند
        /// </summary>
        /// <param name="userId">شناسه شهروند</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllDocGrpupsAndUserDocuments")] 
        [Authorize(Roles = "Citizen,Admin,Operator")]
        public async Task<ApiResult<List<UserDocumentInfo>>> GetAllDocGrpupsAndUserDocuments(int? userId)
        {
            if (userId == null) userId = _usersService.GetCurrentUserId();
            return await _userDocument.GetAllDocGrpupsAndUserDocuments(userId.Value);
        }




        /// <summary>
        /// حذف مدرک توسط شهروند
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveUserDocument")]
        [Authorize(Roles = "Citizen,Admin,Operator")]
        public async Task<ApiResult<string>> RemoveUserDocument(int id)
        { 
            if(id==0)
            {
                return new ApiResult<string>(false, ApiResultStatusCode.BadRequest, "شناسه فایل ارسال نشده است", "شناسه فایل ارسال نشده است");
            }
            return await _userDocument.Remove(id);
        }

        #endregion 


        /// <summary>
        /// دریافت همه نقش ها
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllRols")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<List<BaseDataModel>>> GetAllRols()
        {
            return await _role.GetAllRols();
        }


        /// <summary>
        /// دریافت همه نقش های کاربر
        /// </summary>
        /// <param name="id">شناسه کاربری</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllUserRoles")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<List<UserAllRole>>> GetAllUserRoles(int id)
        {
            if (id == 0)
                return new ApiResult<List<UserAllRole>>(false, ApiResultStatusCode.BadRequest, null, "شناسه کاربر را مشخص نمایید");
            return await _role.GetAllUserRoles(id);
        }

        [HttpGet]
        [Route("DeleteUserRole")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult> DeleteUserRole(int userId, int roleId)
        {
            if (userId == 0 || roleId == 0)
                return new ApiResult(false, ApiResultStatusCode.BadRequest, "اطلاعات را به صورت کامل وارد نمایید");
            var operationId = _usersService.GetCurrentUserId();//  ,operationId
            return await _role.DeleteUserRoles(userId, roleId ,operationId);
        }


        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<UserAllRole>> AddUserRole([FromBody]UserAllRole model)
        {
            
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران);
            if (!canAccess.IsSuccess)
                return new ApiResult<UserAllRole>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");



            return await _role.AddUserRole(model , userId);
        }


        #region توسعه دهندگان
        /// <summary>
        /// کاربران وب سرویس 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetWebApiUsers")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<List<UserInfoDto>>> GetWebApiUsers()
        {
            return await _usersService.GetWebApiUser();
        }


        /// <summary>
        /// جستجوی کاربران
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchWebApiUsers")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<PagedUsersViewModel>> SearchWebApiUsers(
           int? offset = 1, int? count = 20,
           DateTime? FromDate = null,
           DateTime? ToDate = null,
          string username = null
          )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران);
            if (!canAccess.IsSuccess)
                return new ApiResult<PagedUsersViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");


            return await _usersService.SearchWebApiUserr(offset.Value, count.Value, FromDate, ToDate, username);
        }

        /// <summary>
        /// ثبت توسعه دهنده جدید
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<UserInfoDto>> WebApiUserRegister([FromBody]  AdminRegister model)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران);
            if (!canAccess.IsSuccess)
                return new ApiResult<UserInfoDto>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");



            var operationId = _usersService.GetCurrentUserId();//  ,operationId
            return await _usersService.WebApiUserRegister(model,operationId);

        }


        /// <summary>
        /// اضافه کردن خدمات قابل دسترس به کاربر
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<UserAppServiceAccessDto>> AddUserAccessService([FromBody]UserAppServiceAccessDto model)
        {
         

            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران);
            if (!canAccess.IsSuccess)
                return new ApiResult<UserAppServiceAccessDto>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");




            return await _usersService.AddUserAccessService(model, userId);
        }


        /// <summary>
        /// حذف دسترسی کاربر به خدمات
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteUserAccessService")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult> DeleteUserAccessService(int userId, int serviceId)
        {
            if (userId == 0 || serviceId == 0)
                return new ApiResult(false, ApiResultStatusCode.BadRequest, "اطلاعات را به صورت کامل وارد نمایید");

            return await _usersService.DeleteUserAccessService(userId, serviceId);
        }

        [HttpGet]
        [Route("GetAllUserAppService")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<List<UserAppServiceAccessDto>>> GetAllUserAppService(int id)
        {
            if (id == 0)
                return new ApiResult<List<UserAppServiceAccessDto>>(false, ApiResultStatusCode.BadRequest, null, "شناسه کاربر را مشخص نمایید");
            return await _usersService.GetAllUserAppService(id);
        }




        /// <summary>
        /// دریافت ادرس ای پی های مجاز
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetWebUserAccessRangeIp")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<List<WebUserAccessRangeIpInfo>>> GetWebUserAccessRangeIp(int userId)
        {
              return await _permission.GetWebUserAccessRangeIp(userId);
        }

        /// <summary>
        /// حذف ادرس IPP
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="IpId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DeleteUserAccessRangeIp")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult> DeleteUserAccessRangeIp(int userId, int IpId)
        {
            if (userId == 0 || IpId == 0)
                return new ApiResult(false, ApiResultStatusCode.BadRequest, "اطلاعات را به صورت کامل وارد نمایید"); 
            return await _permission.DeleteUserAccessRangeIp(userId, IpId);
        }






        /// <summary>
        /// اضافه کردن ادرس ایپی های مجاز
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult> AddWebUserAccessRangeIp(WebUserAccessRangeIpInfo model)
        {
            var userId = _usersService.GetCurrentUserId();
            var canAccess = await _permission.CanAccess(userId, Common.GlobalEnum.PermissionTypeEnum.مدیریت_کاربران);
            if (!canAccess.IsSuccess)
                return new ApiResult (false, ApiResultStatusCode.NotAllowed,  "شما به این قسمت دسترسی ندارید");




            return await _permission.AddWebUserAccessRangeIp(model);
        }








        #endregion


    }
}
