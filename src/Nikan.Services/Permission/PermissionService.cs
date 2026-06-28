using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.Permissions;
using Nikan.ViewModel;
using Nikan.ViewModel.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services.Permissions
{
    public interface IPermissionService
    {
        Task<ApiResult> AddPermissions(UserPermissions model);
        Task<Dictionary<string, IEnumerable<BaseDataModel>>> GetPermissionList(int userId);
        Task<ApiResult<List<string>>> GetPermissionAdminMenuByGroupId(int userId);
        Task<ApiResult> AddWebUserApiPermissionsGroups();
        Task<Dictionary<string, IEnumerable<BaseDataModel>>> GetWebApiPermissionList(int userId);
        Task<ApiResult> AddWebApiUserPermissions(WebApiUserPermissions model);
        Task<ApiResult> CanAccessWebApi(int userId, PermissionTypeEnum permissionType);
        Task<ApiResult<List<string>>> GetPermissionAdminMenuByUserId(int userId);
        Task<ApiResult> AddWebUserAccessRangeIp(WebUserAccessRangeIpInfo model);
        Task<ApiResult<List<WebUserAccessRangeIpInfo>>> GetWebUserAccessRangeIp(int userId);
        Task<ApiResult> DeleteUserAccessRangeIp(int userId, int IpId);
        Task<Dictionary<string, IEnumerable<BaseDataModel>>> GetCardPermissionList(int userId);
        Task<ApiResult<List<string>>> GetPermissionCardMenuByUserId(int userId);
        Task<ApiResult> CanAccess(int userId, PermissionTypeEnum permissionType);
    }

    public class PermissionService : IPermissionService
    {
        #region Field
        private readonly IUnitOfWork _uow;
        private readonly DbSet< Nikan.DomainClasses.Permissions.Permission> _permission;
        private readonly DbSet<UserPermission> _userPermission;
        private readonly DbSet<WebUserPermission> _webuserPermission;
        private readonly DbSet<PermissionGroup> _group;
        private readonly DbSet<WebUserAccessRangeIp> _rangeIp; 
        private readonly DbSet<UserPermissionGroup> _usergroup;


        #endregion
        #region Constractor

        public PermissionService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _permission = _uow.Set<Nikan.DomainClasses.Permissions.Permission>();
            _userPermission = _uow.Set<UserPermission>();
            _webuserPermission = _uow.Set<WebUserPermission>();
            _group = _uow.Set<PermissionGroup>();
            _usergroup = _uow.Set<UserPermissionGroup>();
            _rangeIp = _uow.Set<WebUserAccessRangeIp>();

        }
        #endregion

        public async Task<Dictionary<string, IEnumerable<BaseDataModel>>> GetPermissionList(int groupId)
        {
            var userpermissionList =await _userPermission.Where(w =>
             w.PermissionGroupId == groupId).Select(s => s.PermissionId).ToListAsync();
            var list = await _permission.Where(w=>w.UserPermissionType == UserPermissionTypeEnum.مدیریت).ToListAsync(); 
            var items = list
                      .GroupBy(g => g.Category).Select(s => new PermissionList()
                      {
                          Category = s.Key.ToString(),
                          Permissions = s.Select(x =>
                          new BaseDataModel() { 
                              Key =((int) x.PermissionType).ToString(), 
                              Text = x.Title.ToString() ,
                              Selected= userpermissionList.Contains(((int)x.PermissionType))


                          }).OrderBy(o => o.Key)

                      }).ToDictionary(d => d.Category, d => d.Permissions);



            return items;

        }

        public async Task<Dictionary<string, IEnumerable<BaseDataModel>>> GetWebApiPermissionList(int userId)
        {
            var userpermissionList = await _webuserPermission.Where(w =>
              w.UserId == userId).Select(s => s.PermissionId).ToListAsync();
            var list = await _permission.Where(w => w.UserPermissionType == UserPermissionTypeEnum.توسعه_دهندگان).ToListAsync();
            var items = list
                      .GroupBy(g => g.Category).Select(s => new PermissionList()
                      {
                          Category = s.Key.ToString(),
                          Permissions = s.Select(x =>
                          new BaseDataModel()
                          {
                              Key = ((int)x.PermissionType).ToString(),
                              Text = x.Title.ToString(),
                              Selected = userpermissionList.Contains(((int)x.PermissionType))


                          }).OrderBy(o => o.Key)

                      }).ToDictionary(d => d.Category, d => d.Permissions);



            return items;

        }

        public async Task<Dictionary<string, IEnumerable<BaseDataModel>>> GetCardPermissionList(int userId)
        {
            var userpermissionList = await _webuserPermission.Where(w =>
              w.UserId == userId).Select(s => s.PermissionId).ToListAsync();
            var list = await _permission.Where(w => w.UserPermissionType == UserPermissionTypeEnum.اصفهان_کارت).ToListAsync();
            var items = list
                      .GroupBy(g => g.Category).Select(s => new PermissionList()
                      {
                          Category = s.Key.ToString(),
                          Permissions = s.Select(x =>
                          new BaseDataModel()
                          {
                              Key = ((int)x.PermissionType).ToString(),
                              Text = x.Title.ToString(),
                              Selected = userpermissionList.Contains(((int)x.PermissionType))


                          }).OrderBy(o => o.Key)

                      }).ToDictionary(d => d.Category, d => d.Permissions);



            return items;

        }

        public async Task<ApiResult<List<string>>> GetPermissionAdminMenuByUserId(int userId)
        {

            var list = new List<string>();
            var res = new ApiResult<List<string>>(true, ApiResultStatusCode.Success, new List<string>(), "");
            try
            {
                //ببین این کاربر داخل چه گروههایی است
                var groupIds = await _usergroup.Where(w => w.UserId == userId).Select(s => s.PermissionGroupId).ToListAsync();

                if(groupIds.Any())
                {


                    var userpermissionList = await _userPermission.Include(w => w.Permission).Where(w => groupIds.Contains(w.PermissionGroupId)).ToListAsync();


                    if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.اطلاعات_پایه))
                        list.Add("basedata");


                    if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.مدیریت_سازمان))
                        list.Add("organ");

                    if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.مدیریت_کاربران))
                        list.Add("usermanagement");

                    if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.مدیریت_شهروندان))
                        list.Add("citizens");

                    if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.طرح_منزلت))
                        list.Add("manzalat");


                    if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.مدیریت_کارت))
                        list.Add("card");

                    if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.اشخاص_حقوقی))
                        list.Add("companymanagement");

                    if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.ثبت_احوال))
                        list.Add("sabtahval");


                    if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.امورمالی))
                        list.Add("financial");

                   


                    if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.مدیریت_محتوا))
                        list.Add("contentmanagement");

                    if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.پشتیبانی))
                        list.Add("supportmanagement");


                    if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.استرداد_هزینه))
                        list.Add("refundmanagement");




                }

                var cardpermissionList = await _webuserPermission.Include(w => w.Permission).Where(w => w.UserId == userId).ToListAsync();
                //دسترسی اصفهان کارت
                if (cardpermissionList.Any())
                {

                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.مدیریت_کاربران_کارت))
                        list.Add("cardmanageusers");


                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.جستجوی_شهروند_کارت))
                        list.Add("cardsearchcitizen");


                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.مشاهده_اطلاعات_شهروند))
                        list.Add("cardviewcitizen");

                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.بازبینی_تصاویر_کارت))
                        list.Add("cardcitizenspictures");

                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.خروجی_صدور_کارت_کارت))
                        list.Add("cardexport");

                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.مدیریت_توزیع_کارت))
                        list.Add("carddistribute");



                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.تراکنش_های_مالی_کارت))
                        list.Add("cardtrasaction");


                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.تیکت_های_کارت))
                        list.Add("cardticket");


                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.مدیریت_کارت))
                        list.Add("cardmanage");



                }




                res.Data = list;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";
            }
            return res;
        }

        public async Task<ApiResult<List<string>>> GetPermissionCardMenuByUserId(int userId)
        {

            var list = new List<string>();
            var res = new ApiResult<List<string>>(true, ApiResultStatusCode.Success, new List<string>(), "");
            try
            {
                //ببین این کاربر داخل چه گروههایی است
                


               var cardpermissionList = await _webuserPermission.Include(w => w.Permission).Where(w => w.UserId== userId).ToListAsync();

                //دسترسی اصفهان کارت
                if (cardpermissionList.Any())
                {

                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.مدیریت_کاربران_کارت))
                        list.Add("cardmanageusers");


                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.جستجوی_شهروند_کارت))
                        list.Add("cardsearchcitizen");


                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.مشاهده_اطلاعات_شهروند))
                        list.Add("cardviewcitizen");

                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.بازبینی_تصاویر_کارت))
                        list.Add("cardcitizenspictures");

                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.خروجی_صدور_کارت_کارت))
                        list.Add("cardexport");

                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.مدیریت_توزیع_کارت))
                        list.Add("carddistribute");



                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.تراکنش_های_مالی_کارت))
                        list.Add("cardtrasaction");


                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.تیکت_های_کارت))
                        list.Add("cardticket");

                     if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.مدیریت_کارت))
                        list.Add("cardmanage");


                    if (cardpermissionList.Any(w => w.Permission.PermissionType == PermissionTypeEnum.تنظیمات_کارت))
                        list.Add("cardsettings");

                }



                res.Data = list;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";
            }
            return res;
        }




        public async Task<ApiResult<List<string>>> GetPermissionAdminMenuByGroupId(int groupId)
        {
          
            var list = new List<string>();
            var res = new ApiResult<List<string>>(true, ApiResultStatusCode.Success,new List<string>(),"" );
            try
            {
                var userpermissionList = await _userPermission.Include(w=>w.Permission).Where(w => w.PermissionGroupId == groupId).ToListAsync();

                if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.اطلاعات_پایه))
                    list.Add("basedata");


                if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.مدیریت_سازمان))
                    list.Add("organ");

                if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.مدیریت_کاربران))
                    list.Add("usermanagement");

                if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.مدیریت_شهروندان))
                    list.Add("citizens");

                if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.طرح_منزلت))
                    list.Add("manzalat");


                if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.مدیریت_کارت))
                    list.Add("card");

                if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.اشخاص_حقوقی))
                    list.Add("companymanagement");

                if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.ثبت_احوال))
                    list.Add("sabtahval");




                if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.مدیریت_محتوا))
                    list.Add("contentmanagement");

                if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.پشتیبانی))
                    list.Add("supportmanagement");



                if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.امورمالی))
                    list.Add("financial");


                if (userpermissionList.Any(w => w.Permission.Category == Common.GlobalEnum.PermissionCategoryEnum.استرداد_هزینه))
                    list.Add("refundmanagement");






                res.Data = list;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";
            } 
            return res; 
        }
      
        
        
        public async Task<ApiResult> AddPermissions(UserPermissions model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "دسترسی با موفقعیت اعمال گردید");
            try
            {
                var allPersmissions =await _userPermission.Where(w => w.PermissionGroupId == model.GroupId).ToListAsync();
                _userPermission.RemoveRange(allPersmissions); 
                foreach (var persmission in model.Permissions)
                { 
                    var item = new UserPermission
                    {
                        PermissionId= persmission,
                        PermissionGroupId=model.GroupId, 
                    }; 
                    await _userPermission.AddAsync(item);
                } 
                await _uow.SaveChangesAsync(); 
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            } 

            return res;
        }


        public async Task<ApiResult> AddWebApiUserPermissions(WebApiUserPermissions model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "دسترسی با موفقعیت اعمال گردید");
            try
            {

                var allPersmissions = await _webuserPermission.Where(w => w.UserId == model.UserId).ToListAsync();
                _webuserPermission.RemoveRange(allPersmissions);
                foreach (var persmission in model.Permissions)
                {
                    var item = new WebUserPermission
                    {
                        PermissionId = persmission,
                        UserId = model.UserId,
                    };
                    await _webuserPermission.AddAsync(item);
                }
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }

            return res;
        }
         
        public async Task<ApiResult> AddWebUserApiPermissionsGroups()
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "دسترسی با موفقعیت اعمال گردید");
            try
            {

                 
                    var models = new List<Permission>
                        {

                           

                           new Permission() { Id = 0,
                                Title = "داشبورد",
                                PermissionType = PermissionTypeEnum.داشبورد ,
                                Category =  PermissionCategoryEnum.مدیریت },
                            //-----اطلاعات_پایه
                            new Permission() { Id = 100,
                                Title = "گروههای شهروندی",
                                PermissionType = PermissionTypeEnum.گروههای_شهروندی ,
                                Category =  PermissionCategoryEnum.اطلاعات_پایه },

                             new Permission() { Id = 101,
                                Title = "خدمات شهروندی",
                                PermissionType = PermissionTypeEnum.خدمات_شهروندی ,
                                Category =  PermissionCategoryEnum.اطلاعات_پایه },
                               new Permission() { Id = 102,
                                Title = "تنظیمات_سامانه",
                                PermissionType = PermissionTypeEnum.تنظیمات_سامانه ,
                                Category =  PermissionCategoryEnum.اطلاعات_پایه },

                                new Permission() { Id = 103,
                                Title = "تنظیمات پیامک",
                                PermissionType = PermissionTypeEnum.تنظیمات_پیامک ,
                                Category =  PermissionCategoryEnum.اطلاعات_پایه },

                                //---سازمانها
                            new Permission() { Id = 201,
                                Title = "سازمانها",
                                PermissionType = PermissionTypeEnum.سازمانها ,
                                Category =  PermissionCategoryEnum.مدیریت_سازمان
                          },
                                 //-- مدیریت_کاربران = 300,

                                new Permission() { Id = 300,
                                Title = "مدیریت توسعه دهندگان",
                                PermissionType = PermissionTypeEnum.مدیریت_توسعه_دهندگان ,
                                Category =  PermissionCategoryEnum.مدیریت_کاربران },

                                     new Permission() { Id = 301,
                                Title = "مدیریت کاربران",
                                PermissionType = PermissionTypeEnum.مدیریت_کاربران ,
                                Category =  PermissionCategoryEnum.مدیریت_کاربران },

                               new Permission() { Id = 302,
                                Title = "گروههای کاربری",
                                PermissionType = PermissionTypeEnum.گروههای_کاربری ,
                                Category =  PermissionCategoryEnum.مدیریت_کاربران },

                               new Permission() { Id = 303,
                                Title = "مدیران سامانه",
                                PermissionType = PermissionTypeEnum.مدیران_سامانه ,
                                Category =  PermissionCategoryEnum.مدیریت_کاربران },


                     
                                 //-- مدیریت_شهروندان = 400,
                                new Permission() { Id = 401,
                                Title = "جستجوی شهروند",
                                PermissionType = PermissionTypeEnum.جستجوی_شهروند ,
                                Category =  PermissionCategoryEnum.مدیریت_شهروندان },


                               new Permission() { Id = 402,
                                Title = "جستجوی پیشرفته شهروند",
                                PermissionType = PermissionTypeEnum.جستجوی_پیشرفته_شهروند ,
                                Category =  PermissionCategoryEnum.مدیریت_شهروندان },

                              new Permission() { Id = 403,
                                Title = "بازبینی تصاویر",
                                PermissionType = PermissionTypeEnum.بازبینی_تصاویر ,
                                Category =  PermissionCategoryEnum.مدیریت_شهروندان },

                                               new Permission() { Id = 404,
                                Title = "بازخورد ها",
                                PermissionType = PermissionTypeEnum.بازخورد_ها ,
                                Category =  PermissionCategoryEnum.مدیریت_شهروندان },

                              new Permission() { Id = 405,   Title = "خانواده شهروند",
                               PermissionType = PermissionTypeEnum.خانواده_شهروند ,  Category =  PermissionCategoryEnum.مدیریت_شهروندان },

                                new Permission() { Id = 406,   Title = "استعلام وضعیت فوتی",
                                PermissionType = PermissionTypeEnum.استعلام_وضعیت_فوتی ,  Category =  PermissionCategoryEnum.مدیریت_شهروندان },

                                  new Permission() { Id = 407,   Title = "گروه شهروندی",
                                  PermissionType = PermissionTypeEnum.گروه_شهروندی ,  Category =  PermissionCategoryEnum.مدیریت_شهروندان },

                                    new Permission() { Id = 408,   Title = "صف شهروندی",
                                  PermissionType = PermissionTypeEnum.صف_شهروندی , 
                                        Category =  PermissionCategoryEnum.مدیریت_شهروندان },

                                     new Permission() { Id = 409,   Title = "ویرایش اطلاعات شهروند",
                                  PermissionType = PermissionTypeEnum.ویرایش_اطلاعات_شهروند ,
                                        Category =  PermissionCategoryEnum.مدیریت_شهروندان },




                                new Permission() { Id = 410,
                                Title = "ثبت نام دسته ایی شهروند",
                                PermissionType = PermissionTypeEnum.ثبت_نام_دسته_ایی_شهروند ,
                                Category =  PermissionCategoryEnum.مدیریت_شهروندان },

                               new Permission() { Id = 411,
                                Title = "ویرایش شماره موبایل شهروند",
                                PermissionType = PermissionTypeEnum.ویرایش_شماره_موبایل_شهروند ,
                                Category =  PermissionCategoryEnum.مدیریت_شهروندان },
                              

                                new Permission() { Id = 412,
                                Title = "تغییر وضعیت ثبت احوال شهروند",
                                PermissionType = PermissionTypeEnum.تغییر_وضعیت_ثبت_احوال ,
                                Category =  PermissionCategoryEnum.مدیریت_شهروندان },


                                new Permission() { Id = 413,
                                Title = "دریافت خروجی اکسل از اطلاعات شهروندان",
                                PermissionType = PermissionTypeEnum.دریافت_خروجی_اکسل_شهروندان ,
                                Category =  PermissionCategoryEnum.مدیریت_شهروندان },

//-----------------------------------------------------------------------------------------------
                               new Permission() { Id = 501,   Title = "درخواست کنندگان منزلت",
                               PermissionType = PermissionTypeEnum.درخواست_کنندگان_منزلت ,  Category =  PermissionCategoryEnum.طرح_منزلت },

                               new Permission() { Id = 502,   Title = " بررسی درخواست منزلت ",
                               PermissionType = PermissionTypeEnum.بررسی_درخواست_منزلت , 
                                   Category =  PermissionCategoryEnum.طرح_منزلت },

                                new Permission() { Id = 503,   Title = "تنظیمات منزلت",
                               PermissionType = PermissionTypeEnum.تنظیمات_منزلت ,
                                   Category =  PermissionCategoryEnum.طرح_منزلت },
                                //-----------------------------------------------------------------------------------------------
                               new Permission() { Id = 601,   Title = "درخواست کنندگان منزلت",
                               PermissionType = PermissionTypeEnum.درخواست_کنندگان_کارت , 
                                   Category =  PermissionCategoryEnum.مدیریت_کارت },

                               new Permission() { Id = 602,   Title = "مشخصات کارت",
                               PermissionType = PermissionTypeEnum.مشخصات_کارت ,
                                   Category =  PermissionCategoryEnum.مدیریت_کارت },

                                new Permission() { Id = 603,   Title = "خروجی صدور کارت",
                               PermissionType = PermissionTypeEnum.خروجی_صدور_کارت ,
                                   Category =  PermissionCategoryEnum.مدیریت_کارت },
                                 //--------------------------مدیریت اشخاص حقوقی---------------------------------------------------------------------
                               new Permission() { Id = 671,   Title = "مدیریت اشخاص حقوقی",
                               PermissionType = PermissionTypeEnum.مدیریت_اشخاص_حقوقی ,
                                   Category =  PermissionCategoryEnum.اشخاص_حقوقی },
                               //--------------------------خروجی_ثبت_احوال---------------------------------------------------------------------
                               new Permission() { Id = 701,   Title = "خروجی ثبت احوال  ",
                               PermissionType = PermissionTypeEnum.خروجی_ثبت_احوال , 
                                   Category =  PermissionCategoryEnum.ثبت_احوال },


                                new Permission() { Id = 702,   Title = "  احراز هویت شهروند ",
                               PermissionType = PermissionTypeEnum.احراز_هویت_شهروند ,
                                   Category =  PermissionCategoryEnum.ثبت_احوال },




                               //--------------------------پشتیبانی---------------------------------------------------------------------
                               new Permission() { Id = 801,   Title = "مدیریت اطلاعیه",
                               PermissionType = PermissionTypeEnum.مدیریت_اطلاعیه , 
                                   Category =  PermissionCategoryEnum.پشتیبانی },

                                 new Permission() { Id = 802,   Title = "مشاهده تیکت ",
                               PermissionType = PermissionTypeEnum.مشاهده_تیکت , 
                                     Category =  PermissionCategoryEnum.پشتیبانی },

                                   new Permission() { Id = 803,   Title = "ارسال پاسخ تیکت",
                               PermissionType = PermissionTypeEnum.ارسال_پاسخ_تیکت ,  
                                       Category =  PermissionCategoryEnum.پشتیبانی },
   


                            
                       //--------------------------مدیریت خبر---------------------------------------------------------------------
                            
                            new Permission() { Id = 901,   Title = "مدیریت راهنمای توسعه دهندگان",
                               PermissionType = PermissionTypeEnum.مدیریت_خبر ,  Category =  PermissionCategoryEnum.مدیریت_محتوا },

                               new Permission() { Id = 902,   Title = "مدیریت پرسش و پاسخ",
                               PermissionType = PermissionTypeEnum.مدیریت_پرسش_و_پاسخ ,  Category =  PermissionCategoryEnum.مدیریت_محتوا },

                               new Permission() { Id = 903,   Title = "مدیریت صفحات",
                                PermissionType = PermissionTypeEnum.مدیریت_صفحات ,  Category =  PermissionCategoryEnum.مدیریت_محتوا },




                                //--------------------------پشتیبانی---------------------------------------------------------------------
                               new Permission() { Id = 1001,   Title = "تنظیمات مالی",
                               PermissionType = PermissionTypeEnum.تنظیمات_مالی ,
                                   Category =  PermissionCategoryEnum.امورمالی },

                                 new Permission() { Id = 1002,   Title = "تراکنش های مالی",
                               PermissionType = PermissionTypeEnum.تراکنش_های_مالی ,
                                     Category =  PermissionCategoryEnum.امورمالی },

                                  new Permission() { Id = 1003,   Title = "تست پرداخت",
                               PermissionType = PermissionTypeEnum.تست_پرداخت ,
                                     Category =  PermissionCategoryEnum.امورمالی },


                               //--------------------------پشتیبانی---------------------------------------------------------------------
                               new Permission() { Id = 1501,   Title = "بارگذاری فایل استرداد",
                               PermissionType = PermissionTypeEnum.بارگذاری_فایل_استرداد ,
                                   Category =  PermissionCategoryEnum.استرداد_هزینه },

                                 new Permission() { Id = 1502,   Title = " لیست دسترسی استرداد ",
                               PermissionType = PermissionTypeEnum.لیست_دسترسی_استرداد ,
                                     Category =  PermissionCategoryEnum.استرداد_هزینه },

                                   new Permission() { Id = 1503,   Title = "جستجوی استرداد",
                               PermissionType = PermissionTypeEnum.جستجوی_استرداد ,
                                   Category =  PermissionCategoryEnum.استرداد_هزینه },

                                 new Permission() { Id = 1504,   Title = "تایید برگشت هزینه",
                               PermissionType = PermissionTypeEnum.تایید_برگشت_هزینه ,
                                     Category =  PermissionCategoryEnum.استرداد_هزینه },
                                  
                        
                        new Permission() { Id = 1506,   Title = "ثبت شماره کارت",
                               PermissionType = PermissionTypeEnum.ثبت_شماره_کارت ,
                                   Category =  PermissionCategoryEnum.استرداد_هزینه },

                                 new Permission() { Id = 1507,   Title = "ویرایش استرداد",
                               PermissionType = PermissionTypeEnum.ویرایش_استرداد ,
                                     Category =  PermissionCategoryEnum.استرداد_هزینه },

        


                         //توسعه دهندگان---------------------------------------------------------
                            new Permission() { Id = 2000,
                                Title = "لیست خدمات",
                                PermissionType = PermissionTypeEnum.لیست_خدمات ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},

                                new Permission() { Id = 2001,
                                Title = "ثبت نام شهروند",
                                PermissionType = PermissionTypeEnum.ثبت_نام_شهروند ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},


                               new Permission() { Id = 2002,
                                Title = "دریافت اطلاعات کوتاه شهروند",
                                PermissionType = PermissionTypeEnum.دریافت_اطلاعات_کوتاه_شهروند ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},

                              new Permission() { Id = 2003,
                                Title = "دریافت اطلاعات تکمیلی شهروند",
                                PermissionType = PermissionTypeEnum.دریافت_اطلاعات_تکمیلی_شهروند ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},

                                new Permission() { Id = 2004,
                                Title = "ویرایش اطلاعات شهروندی",
                                PermissionType = PermissionTypeEnum.ویرایش_اطلاعات_شهروندی ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},


                                 new Permission() { Id = 2005,
                                Title = "ویرایش کلمه عبور شهروندی",
                                PermissionType = PermissionTypeEnum.ویرایش_کلمه_عبور_شهروندی ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},


                                new Permission() { Id = 2006,
                                Title = "ایجاد توکن دسترسی",
                                PermissionType = PermissionTypeEnum.ایجاد_توکن_دسترسی ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},
                              
                                new Permission() { Id = 2007,
                                Title = "ارسال پیامک اعتبارسنجی شماره موبایل",
                                PermissionType = PermissionTypeEnum.ارسال_پیامک_اعتبارسنجی_شماره_موبایل ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},

                               
                                new Permission() { Id = 2008,
                                Title = "ثبت نام سریع شهروند",
                                PermissionType = PermissionTypeEnum.ثبت_نام_سریع_شهروند ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},


                                new Permission() { Id = 2009,
                                Title = "دریافت اطلاعات شهروند به وسیله توکن",
                                PermissionType = PermissionTypeEnum.دریافت_اطلاعات_شهروند_به_وسیله_توکن ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},


                                new Permission() { Id = 2010,
                                Title = "دریافت اطلاعات عضویت شهروند در گروههای شهروندی",
                                PermissionType = PermissionTypeEnum.دریافت_اطلاعات_عضویت_شهروند_در_گروههای_شهروندی ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},


                                new Permission() { Id = 2011,
                                Title = "دریافت اطلاعات آدرس شهروند",
                                PermissionType = PermissionTypeEnum.دریافت_اطلاعات_آدرس_شهروند ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},


                                new Permission() { Id = 2012,
                                Title = "دریافت اطلاعات کامل شهروند",
                                PermissionType = PermissionTypeEnum.دریافت_اطلاعات_کامل_شهروند ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},

                                new Permission() { Id = 2013,
                                Title = "دریافت اطلاعات ثبت نام کنندگان طرح منزلت",
                                PermissionType = PermissionTypeEnum.دریافت_اطلاعات_ثبت_نام_کنندگان_طرح_منزلت ,
                                Category =  PermissionCategoryEnum.توسعه_دهندگان ,UserPermissionType=UserPermissionTypeEnum.توسعه_دهندگان},
                           







                                //---------------------------------------------------------------------------------------------
                                new Permission() { Id = 5001,
                                Title = "مدیریت کاربران ",
                                PermissionType = PermissionTypeEnum.مدیریت_کاربران_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت},

                                new Permission() { Id = 5002,
                                Title = " جستجوی شهروند ",
                                PermissionType = PermissionTypeEnum.جستجوی_شهروند_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت},

                                 new Permission() { Id = 5003,
                                Title = "مشاهده اطلاعات شهروند",
                                PermissionType = PermissionTypeEnum.مشاهده_اطلاعات_شهروند ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت},

                                new Permission() { Id = 5004,
                                Title = "بازبینی تصاویر کارت",
                                PermissionType = PermissionTypeEnum.بازبینی_تصاویر_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت},


                                new Permission() { Id = 5005,
                                Title = "خروجی صدور کارت ",
                                PermissionType = PermissionTypeEnum.خروجی_صدور_کارت_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت},



                                new Permission() { Id = 5006,
                                Title = "مدیریت توزیع کارت",
                                PermissionType = PermissionTypeEnum.مدیریت_توزیع_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت},


                                new Permission() { Id = 5007,
                                Title = "تراکنش های مالی کارت",
                                PermissionType = PermissionTypeEnum.تراکنش_های_مالی_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت},



                                new Permission() { Id = 5008,
                                Title = "مدیریت تیکت شهروندان",
                                PermissionType = PermissionTypeEnum.تیکت_های_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت},


                                new Permission() { Id = 5009,
                                Title = "مدیریت کارت شهروندی",
                                PermissionType = PermissionTypeEnum.مدیریت_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت},



                                new Permission() { Id = 5010,
                                Title = "تنظیمات کارت",
                                PermissionType = PermissionTypeEnum.تنظیمات_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,  
                                UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت
                                 },

                                new Permission() { Id = 5011,
                                Title = "ویرایش شماره موبایل شهروند",
                                PermissionType = PermissionTypeEnum.ویرایش_شماره_موبایل_شهروند_اصفهان_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,  
                                UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت
                                 },


                                new Permission() { Id = 5012,
                                 Title = " احراز هویت شهروند  ",
                                PermissionType = PermissionTypeEnum.احراز_هویت_شهروند_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,  
                                UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت
                                 },

                                 new Permission() { Id = 5013,
                                 Title = "جستجوی کارت ",
                                PermissionType = PermissionTypeEnum.جستجوی_کارت_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,  
                                UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت
                                 },
                                
                        new Permission() { Id = 5014,
                                 Title = " خروجی چاپ کارت به صورت اکسل ",
                                PermissionType = PermissionTypeEnum.فایل_اکسل_خروجی_چاپ_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,  
                                UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت
                                 },



                                 new Permission() { Id = 5050,
                                Title = " ثبت درخواست صدور کارت رایگان   ",
                                PermissionType = PermissionTypeEnum.ثبت_درخواست_کارت_رایگان_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,
                                UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت
                                 },



                                 new Permission() { Id = 5051,
                                Title = " تایید درخواست صدور  کارت رایگان  ",
                                PermissionType = PermissionTypeEnum.تایید_درخواست_کارت_رایگان_کارت ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,
                                UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت
                                 },




                                 new Permission() { Id = 5055,
                                Title = " احراز هویت شهروند توسط اپراتور  ",
                                PermissionType = PermissionTypeEnum.احراز_هویت_شهروند_توسط_اپراتور ,
                                Category =  PermissionCategoryEnum.اصفهان_کارت ,
                                UserPermissionType=UserPermissionTypeEnum.اصفهان_کارت
                                 },



                        };


                    foreach (var item in models)
                    {
                        if (! await _permission.AnyAsync(w => w.Id == item.Id))
                        {
                            await _permission.AddAsync(item);
                        }
                    } 
                    await _uow.SaveChangesAsync();

                
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است" + er.Message;

            }

            return res;
        }


        public async Task<ApiResult> CanAccess(int userId, PermissionTypeEnum permissionType)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "به این سرویس دسترسی دارد");
            try
            {
                var permissionId = (int)permissionType;
                var userGroups =await _usergroup.AsNoTracking().Where(w => w.UserId == userId).ToListAsync();

                if (!userGroups.Any())
                {
                    res.IsSuccess = false;
                    res.Messages = "شما به این سرویس دسترسی ندارید";
                    return res;
                }


                var groupIds = userGroups.Select(s => s.PermissionGroupId).ToList();

                var perList = await _userPermission.Include(p=>p.Permission).Where(w => groupIds.Contains( w.PermissionGroupId )).ToListAsync();

                if(perList.Any(w=>w.Permission.PermissionType== permissionType))
                {
                    res.IsSuccess = true;
                    res.Messages = "شما به این سرویس دسترسی دارید";
                    return res;

                }

                res.IsSuccess = false;
                res.Messages = "شما به این سرویس دسترسی ندارید";
                return res;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است" + er.Message;

            }

            return res;
        }


        public async Task<ApiResult> CanAccessWebApi(int userId, PermissionTypeEnum permissionType)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "به این سرویس دسترسی دارد");
            try
            {
                var permissionId = (int)permissionType;
                if (await _webuserPermission.AnyAsync(w => w.UserId == userId && w.PermissionId == permissionId))
                {
                    res.IsSuccess = true;
                    res.Messages = "به این سرویس دسترسی دارد";
                    return res;
                }

                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.UnAuthorized;
                res.Messages = "کاربر توسعه دهنده به این سرویس دسترسی نداد";
                return res;


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است" + er.Message;

            }

            return res;
        }




        public async Task<ApiResult> AddWebUserAccessRangeIp(WebUserAccessRangeIpInfo model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "آدرس ای پی با موفقیت اضافه گردید");
            try
            {

                await _rangeIp.AddAsync(new WebUserAccessRangeIp()
                {
                    End=model.End,
                    Start=model.Start,
                    UserId=model.UserId

                });
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }

            return res;
        }
        public async Task<ApiResult<List<WebUserAccessRangeIpInfo>>> GetWebUserAccessRangeIp(int userId)
        {
            var res = new ApiResult<List<WebUserAccessRangeIpInfo>>(true, ApiResultStatusCode.Success, new List<WebUserAccessRangeIpInfo>());

            try
            {
                res.Data = await _rangeIp.Where(w => w.UserId == userId)
                     .Select
                      (s => new WebUserAccessRangeIpInfo()
                      {
                           Start =s.Start,
                           End = s.End,
                           UserId = s.UserId,
                           UserName = s.User.Username,
                           Id=s.Id
                      })
                      .ToListAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }
        public async Task<ApiResult> DeleteUserAccessRangeIp(int userId, int IpId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت حذف شد");
            try
            {
                var usersAppServiceAccess = await _rangeIp.FirstOrDefaultAsync(w => w.UserId == userId && w.Id == IpId);
                if (usersAppServiceAccess == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                _rangeIp.Remove(usersAppServiceAccess);
                await _uow.SaveChangesAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }









    }

}
