using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cle.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.Services;
using Nikan.Services.Permissions;
using Nikan.ViewModel;

namespace Nikan.WebApp.Controllers
{
   
        [Route("api/[controller]")]
        [ApiController]
        //[Produces("application/json")]اگر بخواهیم همواره جیسون برگداند 
        [EnableCors("CorsPolicy")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Policy = CustomRoles.Admin)]
        public class OrganController : Controller
        {


        #region Field
        private readonly IOrganizationalUnitService _organizationalUnitService;
        private readonly IOrganizationService _OrganizationService; 
        private readonly IUsersService _usersService;
        private readonly IPermissionService _permission;
        #endregion
        #region Constractor

        public OrganController(
            IUsersService usersService,
            IOrganizationalUnitService  organizationalUnitService,
              IPermissionService permission
           , IOrganizationService OrganizationService)
        {
            _organizationalUnitService = organizationalUnitService; 
            _usersService = usersService;
            _OrganizationService = OrganizationService;
            this._permission = permission;


        }

        #endregion

        #region Organization
      
        
        
        /// <summary>
        /// اضافه یا ویرایش سازمان ها
        /// [Organ1]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<OrganizationViewModel>> AddOrUpdateOrganization(OrganizationViewModel model)
        {
          
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.سازمانها);
            if (!canAccess.IsSuccess)
            {
                 return new ApiResult<OrganizationViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");
            }

             

            return await _OrganizationService.AddOrUpdate(model, userId);
        }


        /// <summary>
        /// حذف سازمان
        /// [Organ2]
        /// </summary>
        /// <param name="id">شناسه سازمان</param>
        /// <returns></returns>
        [Route("RemoveOrganization")]
        [HttpGet]
        public async Task<ApiResult<string>> RemoveOrganization(string id)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.سازمانها);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");
            }
            return await _OrganizationService.Remove(id);

        }




        /// <summary>
        /// دریافت تمامی سازمان ها
        /// [Organ3]
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllOrganization")]
        public async Task<ApiResult<List<OrganizationViewModel>>> GetAllOrganization()
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.سازمانها);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<List<OrganizationViewModel>>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");
            }
            return await _OrganizationService.GetAll();

        }



        /// <summary>
        /// دریافت جزئیات سازمان
        ///  [Organ4]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetOrganizationForEdit")]
        [HttpGet]
        public async Task<ApiResult<OrganizationViewModel>> GetOrganizationForEdit(string id)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.سازمانها);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<OrganizationViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");
            }
            return await _OrganizationService.GetItemForEdit(id);

        }


        /// <summary>
        /// حذف واحد از سازمان
        /// [Organ5]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("RemoveUnit")]
        [HttpGet]
        public async Task<ApiResult<string>> RemoveUnit(string id)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.سازمانها);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<string>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");
            }
            return await _organizationalUnitService.Remove(id);

        }


        /// <summary>
        /// اضافه یا ویرایش واحد از سازمان
        /// [Organ6]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<OrganizationalUnitDto>> AddOrUpdateUnit(OrganizationalUnitDto model)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.سازمانها);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<OrganizationalUnitDto>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");
            }
            return await _organizationalUnitService.AddOrUpdate(model);
        }



        /// <summary>
        /// دریافت اطلاعات یک واحد
        /// [Organ7]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetUnitForEdit")]
        [HttpGet]
        public async Task<ApiResult<OrganizationalUnitViewModel>> GetUnitForEdit(string id)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.سازمانها);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<OrganizationalUnitViewModel>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");
            }

            return await _organizationalUnitService.GetItemForEdit(id);

        }

        /// <summary>
        /// دریافت همه واحد های یک سازمان
        /// [Organ8]
        /// </summary>
        /// <param name="orgId">شناسه سازمان</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllUnit")]
        public async Task<ApiResult<List<OrganizationalUnitViewModel>>> GetAllUnit(string orgId)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.سازمانها);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult<List<OrganizationalUnitViewModel>>(false, ApiResultStatusCode.NotAllowed, null, "شما به این قسمت دسترسی ندارید");
            }


            return await _organizationalUnitService.GetAll(orgId);

        }


        #endregion


        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> AddGroupToUnitGroup(OrganizationUnitGroups model)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.سازمانها);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult(false, ApiResultStatusCode.NotAllowed,   "شما به این قسمت دسترسی ندارید");
            }

            if (model == null)
                return new ApiResult<OrganizationUnitGroups>(false, ApiResultStatusCode.BadRequest, null, "اطلاعات ورودی معتبر نمی باشد");
            return await _organizationalUnitService.AddGroupToUnitGroup(model);
        }



        [HttpGet]
        [Route("RemoveUnitGroup")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> RemoveUnitGroup(int groupId, string  unitId)
        {
            int userId = this._usersService.GetCurrentUserId();
            ApiResult canAccess = await this._permission.CanAccess(userId, PermissionTypeEnum.سازمانها);
            if (!canAccess.IsSuccess)
            {
                return new ApiResult(false, ApiResultStatusCode.NotAllowed, "شما به این قسمت دسترسی ندارید");
            }

            return await _organizationalUnitService.RemoveUnitGroup(new OrganizationUnitGroups() {  UnitId = unitId,  GroupId = groupId });

        }



        [HttpGet]
        [Route("GetAllUnitGroups")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<OrganizationUnitGroupsInfo>>> GetAllUnitGroups(string  id)
        {
            return await _organizationalUnitService.GetAllUnitGroups(id);

        }



    }
}