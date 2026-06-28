using cle.Services;
using cle.Services.BaseEntity;
using cle.Services.CitizensGroups;
using cle.Services.UserCompanyServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Nikan.Common;
using Nikan.Common.ApiCall;
using Nikan.MellatApp;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.Services.Citizens;
using Nikan.Services.Permissions;
using Nikan.ViewModel;
using Nikan.ViewModel.BsseEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Nikan.Controllers.Api
{
    [Route("api/BaseData")]
    [ApiController]
    [AllowAnonymous]
    public class BaseDataController : ControllerBase
    {

        #region ctor
        
        private readonly ICityService _cityService;   
        private readonly IOrganizationalUnitService _organizationalUnit;  
        private readonly IBaseDataService _baseData;
        private readonly IOrganizationService _organizationService;
        private readonly IUserCompanyService _userCompanyService;
        private readonly IUsersService _userManager; 
        private readonly IOrganizationalPositionService _organizationalPosition; 
        private readonly IMajorService _major;
        private readonly IEducationGroupService _educationGroup;
        private readonly ISiteSettingService _settings;
        private readonly IGroupService _group;
        private readonly ICitizenService _citizen; 
        private readonly IMemoryCache memoryCache;
        private readonly IPermissionService _permission;


        public BaseDataController(ICityService cityService, 
            IUserCompanyService userCompanyService,
             ISiteSettingService  settings,
               IPermissionService permission,
               IMajorService major,
               IMemoryCache memoryCache,
               IGroupService  group,
           ICitizenService citizen,
        IEducationGroupService  educationGroup,
            IBaseDataService baseData,
            IOrganizationalPositionService  organizationalPosition, 
            IOrganizationalUnitService  organizationalUnit,
             IOrganizationService organizationService,
            IUsersService userManager  )
        {
            _cityService = cityService;
             _citizen = citizen ;
            _baseData = baseData;
            _organizationalUnit = organizationalUnit;
            _userCompanyService = userCompanyService;
            _major = major;
            _group = group;
            _organizationService = organizationService;
            _educationGroup = educationGroup;
               _userManager = userManager;
            _settings =  settings;
            _organizationalPosition =  organizationalPosition;
            this.memoryCache = memoryCache;
            _permission = permission;
        }
        #endregion

        [HttpGet]
        [Route("GetCardNumber")]
        public    ApiResult<string>  GetCardNumber(string code,long pid)
        {
            var bankMellatImplement = new BankMellatImplement(0,   "", "", 0, 0);
            if(code=="09138251003")
            {
                var card = bankMellatImplement.CallRefundWebService(pid);
                return new ApiResult<string>(true, ApiResultStatusCode.Success, card, bankMellatImplement.ErrorMessage);
            }

            return new ApiResult<string>(true, ApiResultStatusCode.Success, "", "");
        }




        [HttpGet]
        [Route("GetMajors")]
        public async Task<ApiResult<List<BaseDataModel>>> GetMajors(string query, int offset = 0, int count = 20)
        {
             

            return await _major.GetBaseListAsync(query, offset, count);

        }

 



        [HttpGet]
        [Route("ConfigPortal")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<List<ConfigModel>>> ConfigPortal()
        {

            var listConfig = new List<ConfigModel>(); 
            #region configBaseData
            var configBaseData = await _baseData.configBaseData();
            if (configBaseData.IsSuccess)
            {
                listConfig.Add(new ConfigModel(true, "پیکربندی داده های پایه با موفقیت انجام شد"));
            }
            else
            {
                listConfig.Add(new ConfigModel(false, "پیکربندی داده های پایه با خطا مواجه شد ." + configBaseData.Messages));
            }
            #endregion
            #region configPermissions
            var configPermissions = await _permission.AddWebUserApiPermissionsGroups();
            if (configPermissions.IsSuccess)
            {
                listConfig.Add(new ConfigModel(true, "پیکربندی دسترسی ها با موفقیت انجام شد"));
            }
            else
            {
                listConfig.Add(new ConfigModel(false, "پیکربندی دسترسی ها با خطا مواجه شد ." + configPermissions.Messages));
            }
            #endregion
            #region ConfigGroups
            var ConfigGroups = await _citizen.ConfigAllUserCode();
            if (ConfigGroups.IsSuccess)
            {
                listConfig.Add(new ConfigModel(true, "پیکربندی اطلاعات شهروند با موفقیت انجام شد"));
            }
            else
            {
                listConfig.Add(new ConfigModel(false, "پیکربندی اطلاعات شهروند  با خطا مواجه شد ." + ConfigGroups.Messages));
            }
            #endregion
             


            return new ApiResult<List<ConfigModel>>(true, ApiResultStatusCode.Success, listConfig, "");


        }




        /// <summary>
        /// جدول شماره یک
        /// مقادر enum  استفاده شده در سامانه
        ///  [Base1]
        /// </summary>
        /// <param name="category">دسته بندی</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetlistBase")]
        [AllowAnonymous]
        public async  Task<Dictionary<string, IEnumerable<BaseDataModel>>> GetlistBase(string category = null)
        {
            if (!string.IsNullOrWhiteSpace(category))
                category = category.ToLower();
            var items =await _baseData.GetBaseList(category);
            return items;
        }




        /// <summary>
        /// دریافت لیست استانها 
        ///  [Base2]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProvinces")]
        public async Task<ApiResult<List<BaseDataModel>>> GetProvinces()
        {
            return await _cityService.GetProvinces();

        }

        /// <summary> 
        /// دریافت شهرهای یک استان
        /// [Base3]
        /// </summary>
        /// <param name="parentId">شناسه استان</param>
        /// <param name="selected"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCitesByParent")]
        public async Task<ApiResult<List<BaseDataModel>>> GetCitesByParent(int parentId, int? selected = null)
        {
            return await _cityService.GetCitesByParent(parentId, selected);

        }


        





        /// <summary> 
        /// دریافت همه شهرها
        /// [Base4]
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="showProvince">آیا استان هم نمایش داده شود</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllCites")] 
        public async Task<ApiResult<List<BaseDataModel>>> GetAllCites(int? selected = null, bool showProvince = false)
        {
            return await _cityService.GetAllCites(selected, showProvince);

        }

        /// <summary>
        /// جستجوی شهر
        /// [Base5]
        /// </summary>
        /// <param name="query">کلید جستجو</param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CitySearch")]
        public async Task<ApiResult<List<BaseDataModel>>> CitySearch(string query, int offset = 0, int count = 20)
        {
            return await _cityService.GetBaseListAsync(query, offset, count);

        }

        /// <summary>
        ///  دسته های شغلی
        /// [Base6]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetJobGroups")]
        [AllowAnonymous]
        public async Task<List<BaseDataModel>> GetJobGroups()
        {
            return await _baseData.GetJobGroups(); 
        }

        /// <summary>
        ///  دریافت گروههای تحصیلی
        /// [Base7]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEducationGroups")]
        [AllowAnonymous]
        public async Task<List<BaseDataModel>> GetEducationGroups()
        {
            return await _educationGroup.GetEducationGroups();
        }



        /// <summary>
        /// دریافت شهرهای استان اصفهان
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetIsFahanCites")]
        public async Task<ApiResult<List<BaseDataModel>>> GetIsFahanCites(int? selected = null)
        {

            var isfahanId = await _settings.GetIsfahanInfo();
            if (isfahanId.IsSuccess && isfahanId.Data.IsfahanProvinceId != null)
                return await _cityService.GetCitesByParent(isfahanId.Data.IsfahanProvinceId.Value, selected);
            else
                return new ApiResult<List<BaseDataModel>>(false, ApiResultStatusCode.BadRequest, null, "شناسه شهراصفهان تنظیم نشده است");

        }

        /// <summary>
        /// دریافت شناسه استان و شهر اصفهان
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetIsfahanInfo")]
        public async Task<ApiResult<IsfahanInfo>> GetIsfahanInfo()
        {

            return await _settings.GetIsfahanInfo();

        }



        /// <summary>
        /// دریافت ملیت ها
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNationality")]
        public async Task<ApiResult<List<BaseDataModel>>> GetNationality(int? selected)
        { 
                return await _cityService.GetNationality(selected); 
        }


        /// <summary>
        /// دریافت گروههای شهروندی
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetGroups")]
        public async Task<ApiResult<List<BaseDataModel>>> GetGroups(int? selected)
        {
            return await _group.GetGroups(selected);
        }
        [HttpGet]
        [Route("GetFreeCardGroups")]
        public async Task<ApiResult<List<BaseDataModel>>> GetFreeCardGroups(int? selected)
        {
            return await _group.GetFreeCardGroups(selected);
        }


        /// <summary>
        /// جستجوی گروههای شهروندی
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("searchBaseDataGroups")]
        [AllowAnonymous]
        public async Task<ApiResult<List<BaseDataModel>>> SearchBaseDataGroups(string query, int offset = 0, int count = 20)
        {
            return await _group.SearchGroups(query, offset, count); 
        }





        [HttpGet]
        [Route("GetActivitiyList")]
        [AllowAnonymous]
        public async Task<ApiResult<List<BaseDataModel>>> GetActivitiyList()
        {
            return await _userCompanyService.GetActivitiyList();

        }





       

        

         
        
        
        


      



       
        
         

        [HttpGet]
        [Route("GetlistOfCompany")]
        [Authorize]
        public async Task<ApiResult<List<BaseDataModel>>> GetlistOfCompany( int? selected = null)
        {
           return  await _userCompanyService.GetAllCompany( selected);
          
        }

       
        /// <summary>
        /// جستجوی شرکت
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllCompany")]
        [AllowAnonymous]
        public async Task<ApiResult<List<BaseDataModel>>> GetAllCompany(  string query, int offset = 0, int count = 20)
        {
            

            return  await _userCompanyService.GetAllCompany( query, offset, count);
            
        }


       

        
        /// <summary>
        /// دریافت تاریخ جاری سیستم
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ApiResult<string> GetCurrentDate()
        {
            var current = DateTime.Now;
            var currentFormat = current.ToString("dddd") + " " + current.ToString("dd") + " " +
                current.ToString("MMMM") + "ماه " + current.ToString("yyyy");
            return new ApiResult<string>(true, ApiResultStatusCode.Success, currentFormat);
        }


        /// <summary>
        /// دریافت ساعت جاری
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ApiResult<string> GetCurrentTime()
        {
            var currentTime = DateTime.Now.ToString("HH:mm");
            return new ApiResult<string>(true, ApiResultStatusCode.Success, currentTime);
        }

        /// <summary>
        /// دریافت تاریخ جاری
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetCurrentShorDate")]
        public ApiResult<string> GetCurrentShorDate()
        {
            var currentDate = DateTime.Now.ToShortDateString();
            return new ApiResult<string>(true, ApiResultStatusCode.Success, currentDate);
        }

         
        /// <summary>
        /// دریافت لیست واحدها
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllOrganizationalUnit")] 
        public async Task<ApiResult<List<BaseDataModel>>> GetAllOrganizationalUnit(string  selected = null )
        {
            return await _organizationalUnit.GetBaseList(selected );

        }

        /// <summary>
        /// دریافت تمامی مراکز تحویل کارت
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllCardDeliveryCenters")]
        public async Task<ApiResult<List<BaseDataModel>>> GetAllCardDeliveryCenters(string selected = null)
        {
            return await _organizationalUnit.GetAllCardDeliveryCenters(selected);

        }


        /// <summary>
        /// دریافت واحد های یک سازمان
        /// </summary>
        /// <param name="organId">شناسه سازمان</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllOrganizationalUnitByOrganId")] 
        public async Task<ApiResult<List<BaseDataModel>>> GetAllOrganizationalUnitByOrganId(string  organId )
        {
            return await _organizationalUnit.GetBaseUnitOrganList(organId);

        }
        /// <summary>
        /// دریافت سازمان ها
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllOrganizational")]
        public async Task<ApiResult<List<BaseDataModel>>> GetAllOrganizational(string selected = null)
        {
            return await _organizationService.GetBaseList(selected);

        }

        /// <summary>
        /// دریافت مراکز پشتیبانی سامانه
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllSupportCenter")]
        public async Task<ApiResult<List<BaseDataModel>>> GetAllSupportCenter(string selected = null)
        {
            return await _organizationService.GetAllSupportCenter(selected);

        }



        /// <summary>
        /// دریافت پست های سازمانی
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPositionList")]
        public async Task<ApiResult<List<BaseDataModel>>> GetPositionList()
        {
            return await _organizationalPosition.GetPositionListAsync();


        }




    }
}