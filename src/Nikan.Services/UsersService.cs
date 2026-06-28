using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nikan.ViewModel.Users;
 
using Nikan.ViewModel.UserCompanes;
using Nikan.DomainClasses.UserCompanes;
using System.Linq;
using System.Collections.Generic;
using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.BaseEntity;
 
using System.Text;
using Nikan.DomainClasses.Permissions;
using Nikan.ViewModel.Permissions;

namespace Nikan.Services
{
    public interface IUsersService
    {

       

        /// <summary>
        /// ثبت نام اولیه شرکت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult<User>> CompanyRegisterAsync(CompanyRegister model);





        Task<string> GetSerialNumberAsync(int userId);
        Task<User> FindUserAsync(string username, string password);
        ValueTask<User> FindUserAsync(int userId);
        Task UpdateUserLastActivityDateAsync(int userId);
        ValueTask<User> GetCurrentUserAsync();
        int GetCurrentUserId();



        /// <summary>
        /// دریافت شناسه شرکت جاری
        /// </summary>
        /// <returns></returns>
        int GetCurrentUserCompanyId();

         




        
        Task<ApiResult<UserInfoDto>> AdminRegister(AdminRegister model,int userId);
        Task<ApiResult<List<UserInfoDto>>> GetAdminUser();
        Task<ApiResult<string>> ChangeCurrentUserPasswordAsync(ChangePasswordViewModel model);
        Task<ApiResult<string>> ChangeUserPasswordAsync(AdminChangePasswordViewModel model, int? userId);
        Task<ApiResult<UserInfoDto>> UpdateAdminAccount(UpdateAccount model, int userId);
        Task<ApiResult<UserInfoDto>> GetUserInfo(int userId);
        Task<User> FindUserAsync(string username);
        Task<ApiResult<UserInfoDto>> AddCompanyUser(CompanyUserRegister model,int companyId);
        Task<ApiResult<UserInfoDto>> UpdateCompanyUserAccount(UpdateCompanyAccount model);
        Task<ApiResult<List<UserInfoDto>>> GetCompanyUser(int companyId);
        bool IsAdmin();
        Task<ApiResult<PagedUsersViewModel>> SearchUsers(int pageNumber, int pageSize, DateTime? FromDate = null, 
            DateTime? ToDate = null, string username = null, int? roleId = null,userAccountStateEnum ? userAccountState = null);
        Task<ApiResult<List<UserInfoDto>>> GetWebApiUser();
        Task<ApiResult<PagedUsersViewModel>> SearchWebApiUserr(int pageNumber, int pageSize, DateTime? FromDate = null, DateTime? ToDate = null, string username = null);
        Task<ApiResult<UserInfoDto>> WebApiUserRegister(AdminRegister model, int userId);
        Task<ApiResult<UserAppServiceAccessDto>> AddUserAccessService(UserAppServiceAccessDto model, int userId);
        Task<ApiResult> DeleteUserAccessService(int userId, int serviceId);
        Task<ApiResult<List<UserAppServiceAccessDto>>> GetAllUserAppService(int userId);
        Task<ApiResult<string>> SetAllUserPasswordAsync();
        Task<ApiResult<List<UserInfoDto>>> GetCardUser();
        Task<ApiResult<PagedUsersViewModel>> SearchCardUser(int pageNumber, int pageSize, DateTime? FromDate = null, DateTime? ToDate = null, string username = null, int? roleId = null);
        Task<ApiResult> AddCardUser(CardUserRegister model, int userId);
        Task<ApiResult> DeleteCardUser(int userId, int operationId);
        Task<ApiResult<string>> UserCanAccessToService(int userId, int serviceId);
        Task<ApiResult<PagedUsersViewModel>> SearchAdminUser(int pageNumber, int pageSize, DateTime? FromDate = null, DateTime? ToDate = null, string username = null, int? roleId = null);
        Task<ApiResult> AddUserToPermissionGroup(UserPermissionGroupDto model, int userId);
        Task<ApiResult> RemoveUserPermissionGroup(UserPermissionGroupDto model, int userId);
        Task<ApiResult<List<UserPermissionGroupInfo>>> GetAllUserPermissionGroup(int userId);
        
    }

    public class UsersService : IUsersService
    {




        #region Ctor
        private readonly IUnitOfWork _uow;
        private readonly DbSet<User> _users;
        private readonly DbSet<UserAppServiceAccess> _usersAppServiceAccess;
        private readonly DbSet<UserCompany> _userCompany; 
        private readonly DbSet<UserRole> _userRole;
        private readonly DbSet<Role> _role; 
        private readonly ISecurityService _securityService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DbSet<PermissionGroup> _permissionGroup;
        private readonly DbSet<UserPermissionGroup> _userPermissionGroup;
        private readonly DbSet<Event> _event;





        public UsersService(
            IUnitOfWork uow,
            ISecurityService securityService,
            IHttpContextAccessor contextAccessor)
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));
            _usersAppServiceAccess = _uow.Set<UserAppServiceAccess>();
            _users = _uow.Set<User>();
            _role = _uow.Set<Role>();
            _userPermissionGroup = _uow.Set<UserPermissionGroup>();
            _permissionGroup = _uow.Set<PermissionGroup>();
            _userCompany = _uow.Set<UserCompany>();
            _userRole = _uow.Set<UserRole>();
            _event = _uow.Set<Event>();

            _securityService = securityService;
            _securityService.CheckArgumentIsNull(nameof(_securityService));

            _contextAccessor = contextAccessor;
            _contextAccessor.CheckArgumentIsNull(nameof(_contextAccessor));
        }

        #endregion


        public bool  IsAdmin()
        {
            var userId = GetCurrentUserId();
            var adminRole =   _role.FirstOrDefault(w => w.Name == CustomRoles.Admin);
            return _userRole.Any(w => w.UserId == userId && w.RoleId == adminRole.Id);
            
        }

  
        public ValueTask<User> FindUserAsync(int userId)
        {
            return _users.FindAsync(userId);
        }

        public async Task<ApiResult<UserInfoDto>> WebApiUserRegister(AdminRegister model, int userId)
        {

            var res = new ApiResult<UserInfoDto>(true, ApiResultStatusCode.Success, new UserInfoDto());
            if(model==null)
            {
                res.IsSuccess = false;
                res.Messages = "مدل ورودی نامعتبر می باشد.";
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;
            }


            if (model.AccessServiceId == null)
            {
                res.IsSuccess = false;
                res.Messages = "کاربر به چه خدماتی دسترسی دارد مشخص نمایید.";
                res.StatusCode = ApiResultStatusCode.BadRequest;
                return res;
            }

            

            try
            {
                if (await _users.AnyAsync(w => w.Username == model.UserName))
                {
                    res.IsSuccess = false;
                    res.Messages = "نام کاربری وارد شده تکراری می باشد.";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }

                 

                var passwordHash = _securityService.GetSha256Hash(model.Password);
                var user = new User
                {
                    DisplayName = model.DisplayName,
                    Username = model.UserName,
                    UserAccountState = userAccountStateEnum.فعال,
                    Password = passwordHash,
                    SerialNumber = Guid.NewGuid().ToString("N"),
                    CreatedOnDate = DateTime.Now,
                    EmailAddress = model.Email,
                    MobileNumber = model.MobileNumber,
                    OrganizationalUnitId = model.OrganizationalUnitId, 
                };

                var add = await _users.AddAsync(user);

                await _usersAppServiceAccess.AddAsync(new UserAppServiceAccess()
                {
                    AccessServiceId=model.AccessServiceId.Value,
                    User= user 
                });

                var guardRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.WebApiUser);
                var role = new UserRole() { RoleId = guardRole.Id, User = user };
                await _userRole.AddAsync(role);

                await _event.AddAsync(new Event()
                {
                    ActionName= "WebApiUserRegister",
                    Description="ثبت کاربر توسعه دهنده",
                    CreateDate=DateTime.Now,
                    EventPriority=EventPriorityEnum.Important,
                    EventSection=EventSectionEnum.ثبت_کاربر_توسعه_دهنده,
                    OperationId= userId,
                    EventType=EventTypeEnum.Info,
                    User= user, 
                });

                await _uow.SaveChangesAsync();
                res.Data.UserId = user.Id;

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }



        public async Task<ApiResult<UserAppServiceAccessDto>> AddUserAccessService(UserAppServiceAccessDto model,int userId)
        {
            var res = new ApiResult<UserAppServiceAccessDto>(true, ApiResultStatusCode.Success, new UserAppServiceAccessDto(), "نقش با موفقیت به کاربر اضافه گردید");
            try
            {

                if (await _usersAppServiceAccess.AnyAsync(w => w.AccessServiceId == model.ServiceId
              && w.UserId == model.UserId))
                {
                    res.IsSuccess = false;
                    res.Messages = "قبلا اضافه شده است";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }
                var activty = new UserAppServiceAccess()
                {
                    UserId = model.UserId,
                    AccessServiceId = model.ServiceId,


                };

                await _usersAppServiceAccess.AddAsync(activty);
                await _event.AddAsync(new Event()
                {
                    ActionName = "AddUserAccessService",
                    Description = "ثبت خدمات برای کاربر توسعه دهنده",
                    CreateDate = DateTime.Now,
                    EventPriority = EventPriorityEnum.Important,
                    EventSection = EventSectionEnum.ثبت_خدمات_برای_کاربر_توسعه_دهنده,
                    OperationId = userId,
                    EventType = EventTypeEnum.Info,
                    UserId = model.UserId,
                });


                await _uow.SaveChangesAsync();
                res.Data = model;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

        }
        public async Task<ApiResult> DeleteUserAccessService(int userId, int serviceId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت حذف شد");
            try
            {
                var usersAppServiceAccess = await _usersAppServiceAccess.FirstOrDefaultAsync(w => w.UserId == userId && w.AccessServiceId == serviceId);
                if (usersAppServiceAccess == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                _usersAppServiceAccess.Remove(usersAppServiceAccess);
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


        public async Task<ApiResult<List<UserAppServiceAccessDto>>> GetAllUserAppService(int userId)
        {
            var res = new ApiResult<List<UserAppServiceAccessDto>>(true, ApiResultStatusCode.Success, new List<UserAppServiceAccessDto>());

            try
            {
                res.Data = await _usersAppServiceAccess.Where(w => w.UserId == userId)
                     .Select
                      (s => new UserAppServiceAccessDto()
                      {
                          Service = s.AccessService.ServiceName,
                          ServiceId = s.AccessServiceId,
                          UserId = s.UserId,
                          UserName = s.User.Username
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


        public async Task<ApiResult<string>> UserCanAccessToService(int userId,int serviceId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "","");

            try
            {
                if (await _usersAppServiceAccess.AnyAsync(w => w.UserId == userId && w.AccessServiceId == serviceId))
                {

                    res.IsSuccess = true;
                    res.Messages = "کاربر به این سرویس دسترسی دارد";
                    res.StatusCode = ApiResultStatusCode.Success;

                }
                else
                {
                    res.IsSuccess = false;
                    res.Messages = "کاربر به این سرویس دسترسی ندارد";
                    res.StatusCode = ApiResultStatusCode.NotAllowed;
                }
                    
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }



        public async Task<ApiResult<UserInfoDto>> AdminRegister(AdminRegister model, int userId)
        {

            var res = new ApiResult<UserInfoDto>(true, ApiResultStatusCode.Success, new UserInfoDto());

            try
            {
                if (await _users.AnyAsync(w => w.Username == model.UserName))
                {
                    res.IsSuccess = false;
                    res.Messages = "نام کاربری وارد شده تکراری می باشد.";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }
                 


                var passwordHash = _securityService.GetSha256Hash(model.Password);
                var user = new User
                {
                    DisplayName = model.DisplayName,
                    Username = model.UserName,
                    UserAccountState = userAccountStateEnum.فعال,
                    Password = passwordHash,
                    SerialNumber = Guid.NewGuid().ToString("N"),
                    CreatedOnDate=DateTime.Now,
                    EmailAddress=model.Email,
                    MobileNumber=model.MobileNumber,
                    OrganizationalUnitId=model.OrganizationalUnitId,
                    
                    

                };

                var add = await _users.AddAsync(user);

                if(model.IsGuardRole)
                {
                   
                    var guardRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Card);
                    var role = new UserRole() { RoleId = guardRole.Id, User = user };
                    await _userRole.AddAsync(role);
                }
                else
                {
                    var adminRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Admin); 
                    var role = new UserRole() { RoleId = adminRole.Id, User = user };
                    await _userRole.AddAsync(role);
                }



                await _event.AddAsync(new Event()
                {
                    ActionName = "AdminRegister",
                    Description = "ثبت کاربر جدید",
                    CreateDate = DateTime.Now,
                    EventPriority = EventPriorityEnum.Important,
                    EventSection = EventSectionEnum.ثبت_کاربر_مدیریت,
                    OperationId = userId,
                    EventType = EventTypeEnum.Info,
                    User  = user,
                    
                });

                await _uow.SaveChangesAsync();
                res.Data.UserId = user.Id;
               
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }


        public async Task<ApiResult> AddCardUser(CardUserRegister model,int userId)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "دسترسی جدید با موفقیت ایجاد گردید");

            try
            {
                if(string.IsNullOrEmpty(model.NationCode))
                {
                    res.IsSuccess = false;
                    res.Messages = " کد ملی را وارد نمایید";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }
                model.NationCode = model.NationCode.Fa2En();
                var user = await _users.FirstOrDefaultAsync(w => w.Username == model.NationCode);
                if (user == null)
                {
                    res.IsSuccess = false;
                    res.Messages = " کاربری با کد ملی وارد شده در سامانه ثبت نام نکرده است.";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }
            
                var cardRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Card);
                if (await _userRole.AnyAsync(w => w.UserId == user.Id && w.RoleId == cardRole.Id))
                {
                    res.IsSuccess = false;
                    res.Messages = " برای کد ملی وارد شده دسترسی ایجاد شده است";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }
                else
                {
                    var role = new UserRole() { RoleId = cardRole.Id, UserId = user.Id };
                    await _userRole.AddAsync(role);



                    await _event.AddAsync(new Event()
                    {
                        ActionName = "AddCardUser",
                        Description = "ثبت کاربر مدیریت کارت",
                        CreateDate = DateTime.Now,
                        EventPriority = EventPriorityEnum.Important,
                        EventSection = EventSectionEnum.ثبت_کاربر_کارت,
                        OperationId = userId,
                        EventType = EventTypeEnum.Info,
                        User = user,

                    });







                    await _uow.SaveChangesAsync();
                }
               


            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }

        public async Task<ApiResult<UserInfoDto>> AddCompanyUser(CompanyUserRegister model,int companyId)
        {

            var res = new ApiResult<UserInfoDto>(true, ApiResultStatusCode.Success, new UserInfoDto());

            try
            {
                if (await _users.AnyAsync(w => w.Username == model.UserName))
                {
                    res.IsSuccess = false;
                    res.Messages = "نام کاربری وارد شده تکراری می باشد.";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }



                var passwordHash = _securityService.GetSha256Hash(model.Password);
                var user = new User
                {
                    DisplayName = model.DisplayName,
                    Username = model.UserName,
                    UserAccountState = userAccountStateEnum.فعال,
                    Password = passwordHash,
                    SerialNumber = Guid.NewGuid().ToString("N"),
                    CreatedOnDate = DateTime.Now,
                    EmailAddress = model.Email,
                    MobileNumber = model.MobileNumber,
                    UserCompanyId= companyId

                };

                var add = await _users.AddAsync(user); 
                var companyRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Company);
                var role = new UserRole() { RoleId = companyRole.Id, User = user };
                await _userRole.AddAsync(role);

                await _uow.SaveChangesAsync();
                res.Data.UserId = user.Id;

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }


        /// <summary>
        /// ویرایش اطلاعات کاربر ادمین
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResult<UserInfoDto>> UpdateAdminAccount(UpdateAccount model,int userId)
        {

            var res = new ApiResult<UserInfoDto>(true, ApiResultStatusCode.Success, new UserInfoDto());

            try
            {
                var user = await _users.FirstOrDefaultAsync(w => w.Id == model.UserId);
                if (user == null)
                {
                    res.IsSuccess = false;
                    res.Messages = " کاربری یافت نشد.";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }
                user.DisplayName = model.DisplayName;
                user.OrganizationalUnitId = model.OrganizationalUnitId;
                user.UserAccountState = model.UserAccountState;
                if (user.MobileNumber != model.MobileNumber)
                    user.MobileNumberVerification = false;

                if (user.EmailAddress != model.Email)
                    user.EmailVerification = false;

                user.EmailAddress = model.Email;
                user.MobileNumber = model.MobileNumber;

                _users.Update(user);


                await _event.AddAsync(new Event()
                {
                    ActionName = "UpdateAdminAccount",
                    Description = "ویرایش اطلاعات کاربر",
                    CreateDate = DateTime.Now,
                    EventPriority = EventPriorityEnum.Important,
                    EventSection = EventSectionEnum.ویرایش_اطلاعات_کاربر,
                    OperationId = userId,
                    EventType = EventTypeEnum.Info,
                    User = user,

                });






                await _uow.SaveChangesAsync();
                res.Data.UserId = user.Id;
                res.Data.DisplayName = user.DisplayName;
              

            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }

        public async Task<ApiResult<UserInfoDto>> UpdateCompanyUserAccount(UpdateCompanyAccount model)
        {

            var res = new ApiResult<UserInfoDto>(true, ApiResultStatusCode.Success, new UserInfoDto());

            try
            {
                var user = await _users.FirstOrDefaultAsync(w => w.Id == model.UserId);
                if (user == null)
                {
                    res.IsSuccess = false;
                    res.Messages = " کاربری یافت نشد.";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }
                user.DisplayName = model.FirstName + " " + model.LastName; 
                user.UserAccountState = model.UserAccountState;
                if (user.MobileNumber != model.MobileNumber)
                    user.MobileNumberVerification = false;

                if (user.EmailAddress != model.Email)
                    user.EmailVerification = false;

                user.EmailAddress = model.Email;
                user.MobileNumber = model.MobileNumber;

                _users.Update(user);
                await _uow.SaveChangesAsync();
                res.Data.UserId = user.Id;
                res.Data.DisplayName = user.DisplayName;


            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }



        public async Task<ApiResult<UserInfoDto>> GetUserInfo(int userId)
        {

            var res = new ApiResult<UserInfoDto>(true, ApiResultStatusCode.Success, new UserInfoDto());

            try
            {
              var user  = await _users.Where(w => w.Id == userId).Select(s => new UserInfoDto()
                {
                  CreatedOnDate=s.CreatedOnDate,
                  MobileNumber=s.MobileNumber,
                  OrganizationalUnit=s.OrganizationalUnit==null ? new BaseData() : new BaseData() {Key=s.OrganizationalUnitId,Text=s.OrganizationalUnit.Name },
                  Organization = s.OrganizationalUnit == null ? new BaseData() : new BaseData() { Key = s.OrganizationalUnit.OrganizationId, Text = s.OrganizationalUnit.Organization.OrganizationName },
                  DisplayName=s.DisplayName,
                  EmailAddress=s.EmailAddress,
                  EmailVerification=s.EmailVerification,
                  MobileNumberVerification=s.MobileNumberVerification,
                  UserAccountState=s.UserAccountState,
                  DeactivationDate = s.DeactivationDate,
                  UserId =s.Id,
                  UserName=s.Username,
                  Roles = s.UserRoles.Select(r => r.Role.Name).ToArray(),

              }).FirstOrDefaultAsync();
               
                
                
                
                
                if (user == null)
                {
                    res.IsSuccess = false;
                    res.Messages = " کاربری یافت نشد.";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }
                res.Data = user;


            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }


        public async Task<ApiResult<PagedUsersViewModel>> SearchUsers(
            int pageNumber, int pageSize,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string username = null,
            int? roleId = null ,
            userAccountStateEnum? userAccountState=null
            )
        {


            var res = new ApiResult<PagedUsersViewModel>(true, ApiResultStatusCode.Success, new PagedUsersViewModel());

            try
            {
                 var offset = (pageNumber ) * pageSize ;
                var query = _users.AsQueryable();
                if(!string.IsNullOrWhiteSpace(username))
                {
                    query = query.Where(w => w.Username == username);
                }

                if (FromDate != null)
                {
                    query = query.Where(w => w.CreatedOnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.CreatedOnDate <= ToDate);
                }
                if (roleId != null)
                {
                    query = query.Where(w => w.UserRoles.Any(a => a.RoleId == roleId));
                }

                if (userAccountState != null)
                {
                    query = query.Where(w => w.UserAccountState== userAccountState);
                }



                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new UserInfoDto()
                 {
                     DisplayName = s.DisplayName,
                     UserName = s.Username,
                     LastLoggedIn = s.LastLoggedIn,
                     CreatedOnDate = s.CreatedOnDate,
                     UserId = s.Id,
                     UserAccountState = s.UserAccountState,
                     Roles = s.UserRoles.Select(r => r.Role.Name).ToArray(),
                     EmailAddress = s.EmailAddress,
                     MobileNumber = s.MobileNumber,
                     EmailVerification = s.EmailVerification,
                     MobileNumberVerification = s.MobileNumberVerification,

                }).OrderByDescending(o=>o.UserId).Skip(offset).Take(pageSize).ToListAsync();




            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }


        public async Task<ApiResult<PagedUsersViewModel>> SearchCardUser(
           int pageNumber, int pageSize,
           DateTime? FromDate = null,
           DateTime? ToDate = null,
           string username = null,
           int? roleId = null
           )
        {


            var res = new ApiResult<PagedUsersViewModel>(true, ApiResultStatusCode.Success, new PagedUsersViewModel());

            try
            {
                 var offset = (pageNumber ) * pageSize ;


                var cardRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Card);
                var query = _users.Where(w => w.UserRoles.Any(t => t.RoleId == cardRole.Id));
                if (!string.IsNullOrWhiteSpace(username))
                {
                    query = query.Where(w => w.Username == username);
                }

                if (FromDate != null)
                {
                    query = query.Where(w => w.CreatedOnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.CreatedOnDate <= ToDate);
                }
                if (roleId != null)
                {
                    query = query.Where(w => w.UserRoles.Any(a => a.RoleId == roleId));
                }





                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new UserInfoDto()
                {
                    DisplayName = s.DisplayName,
                    UserName = s.Username,
                    LastLoggedIn = s.LastLoggedIn,
                    CreatedOnDate = s.CreatedOnDate,
                    UserId = s.Id,
                    UserAccountState = s.UserAccountState,
                    Roles = s.UserRoles.Select(r => r.Role.Name).ToArray(),
                    EmailAddress = s.EmailAddress,
                    MobileNumber = s.MobileNumber,
                    EmailVerification = s.EmailVerification,
                    MobileNumberVerification = s.MobileNumberVerification,

                }).OrderByDescending(o => o.UserId).Skip(offset).Take(pageSize).ToListAsync();




            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }






        public async Task<ApiResult<PagedUsersViewModel>> SearchAdminUser(
         int pageNumber, int pageSize,
         DateTime? FromDate = null,
         DateTime? ToDate = null,
         string username = null,
         int? roleId = null
         )
        {


            var res = new ApiResult<PagedUsersViewModel>(true, ApiResultStatusCode.Success, new PagedUsersViewModel());

            try
            {
                 var offset = (pageNumber ) * pageSize ;


                var adminRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Admin);
                var query = _users.Where(w =>w.IsSystem!=true &&  w.UserRoles.Any(a => a.RoleId == adminRole.Id));
                if (!string.IsNullOrWhiteSpace(username))
                {
                    query = query.Where(w => w.Username == username);
                }

                if (FromDate != null)
                {
                    query = query.Where(w => w.CreatedOnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.CreatedOnDate <= ToDate);
                }
                if (roleId != null)
                {
                    query = query.Where(w => w.UserRoles.Any(a => a.RoleId == roleId));
                }





                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new UserInfoDto()
                {
                    DisplayName = s.DisplayName,
                    UserName = s.Username,
                    LastLoggedIn = s.LastLoggedIn,
                    CreatedOnDate = s.CreatedOnDate,
                    UserId = s.Id,
                    UserAccountState = s.UserAccountState,
                    Roles = s.UserRoles.Select(r => r.Role.Name).ToArray(),
                    EmailAddress = s.EmailAddress,
                    MobileNumber = s.MobileNumber,
                    EmailVerification = s.EmailVerification,
                    MobileNumberVerification = s.MobileNumberVerification,

                }).OrderByDescending(o => o.UserId).Skip(offset).Take(pageSize).ToListAsync();




            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }














        public async Task<ApiResult<PagedUsersViewModel>> SearchWebApiUserr(
          int pageNumber, int pageSize,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          string username = null 
          )
        {


            var res = new ApiResult<PagedUsersViewModel>(true, ApiResultStatusCode.Success, new PagedUsersViewModel());

            try
            {
                var webApiUser = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.WebApiUser);


                 var offset = (pageNumber ) * pageSize ;
                var query = _users.Where(w => w.UserRoles.Any(a => a.RoleId == webApiUser.Id));
                if (!string.IsNullOrWhiteSpace(username))
                {
                    query = query.Where(w => w.Username == username);
                }

                if (FromDate != null)
                {
                    query = query.Where(w => w.CreatedOnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.CreatedOnDate <= ToDate);
                }
                
                     
                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new UserInfoDto()
                {
                    DisplayName = s.DisplayName,
                    UserName = s.Username,
                    LastLoggedIn = s.LastLoggedIn,
                    CreatedOnDate = s.CreatedOnDate,
                    UserId = s.Id,
                    UserAccountState = s.UserAccountState,
                    Roles = s.UserRoles.Select(r => r.Role.Name).ToArray(),
                    EmailAddress = s.EmailAddress,
                    MobileNumber = s.MobileNumber,
                    EmailVerification = s.EmailVerification,
                    MobileNumberVerification = s.MobileNumberVerification,

                }).Skip(offset).Take(pageSize).ToListAsync();




            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }



        public async Task<ApiResult<List<UserInfoDto>>> GetAdminUser()
        {


            var res = new ApiResult<List<UserInfoDto>>(true, ApiResultStatusCode.Success, new List<UserInfoDto>());

            try
            {

               //var cardRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Card);
                var adminRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Admin);

                res.Data = await _users.Where(w => w.UserRoles.Any(a =>a.RoleId == adminRole.Id) 
               // ||  w.UserRoles.Any(w => w.RoleId == cardRole.Id)
                 
                 
                 ).Select(s => new UserInfoDto()
                 {
                     DisplayName=s.DisplayName,
                     UserName=s.Username,
                     LastLoggedIn=s.LastLoggedIn,
                     CreatedOnDate=s.CreatedOnDate,
                     UserId=s.Id,
                     UserAccountState=s.UserAccountState, 
                     Roles = s.UserRoles.Select(r=>r.Role.Name).ToArray(),
                     EmailAddress=s.EmailAddress,
                     MobileNumber=s.MobileNumber,
                     EmailVerification=s.EmailVerification,
                     MobileNumberVerification=s.MobileNumberVerification,
                     

                 }).ToListAsync();



            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }

        public async Task<ApiResult<List<UserInfoDto>>> GetCardUser()
        {


            var res = new ApiResult<List<UserInfoDto>>(true, ApiResultStatusCode.Success, new List<UserInfoDto>());

            try
            {

                var cardRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Card);
                
                res.Data = await _users.Where(w => w.UserRoles.Any(a => a.RoleId == cardRole.Id)).Select(s => new UserInfoDto()
                 {
                     DisplayName = s.DisplayName,
                     UserName = s.Username,
                     LastLoggedIn = s.LastLoggedIn,
                     CreatedOnDate = s.CreatedOnDate,
                     UserId = s.Id,
                     UserAccountState = s.UserAccountState,
                     Roles = s.UserRoles.Select(r => r.Role.Name).ToArray(),
                     EmailAddress = s.EmailAddress,
                     MobileNumber = s.MobileNumber,
                     EmailVerification = s.EmailVerification,
                     MobileNumberVerification = s.MobileNumberVerification,


                 }).ToListAsync();



            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }

        public async Task<ApiResult> DeleteCardUser(int userId,int operationId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت حذف شد");
            try
            {

                var cardRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Card); 
                var role = await _userRole.FirstOrDefaultAsync(w => w.UserId == userId && w.RoleId == cardRole.Id);
                if (role == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "کاربری یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }
                await _event.AddAsync(new Event()
                {
                    ActionName = "DeleteCardUser",
                    Description = "حذف کاربر مدیریت کارت",
                    CreateDate = DateTime.Now,
                    EventPriority = EventPriorityEnum.Important,
                    EventSection = EventSectionEnum.حذف_کاربر_مدیریت_کارت,
                    OperationId = operationId,
                    EventType = EventTypeEnum.Info,
                    UserId = userId,

                });


                _userRole.Remove(role);
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


        public async Task<ApiResult<List<UserInfoDto>>> GetWebApiUser()
        {


            var res = new ApiResult<List<UserInfoDto>>(true, ApiResultStatusCode.Success, new List<UserInfoDto>());

            try
            {

                var webApiUser = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.WebApiUser);
              
                res.Data = await _users.Where(w => w.UserRoles.Any(f =>f.RoleId == webApiUser.Id) ).Select(s => new UserInfoDto()
                 {
                     DisplayName = s.DisplayName,
                     UserName = s.Username,
                     LastLoggedIn = s.LastLoggedIn,
                     CreatedOnDate = s.CreatedOnDate,
                     UserId = s.Id,
                     UserAccountState = s.UserAccountState,
                     Roles = s.UserRoles.Select(r => r.Role.Name).ToArray(),
                     EmailAddress = s.EmailAddress,
                     MobileNumber = s.MobileNumber,
                     EmailVerification = s.EmailVerification,
                     MobileNumberVerification = s.MobileNumberVerification,


                 }).ToListAsync();



            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }



        public async Task<ApiResult<List<UserInfoDto>>> GetCompanyUser(int companyId)
        {

            var res = new ApiResult<List<UserInfoDto>>(true, ApiResultStatusCode.Success, new List<UserInfoDto>());

            try
            {

              
                res.Data = await _users.Where(w => w.UserCompanyId== companyId).Select(s => new UserInfoDto()
                 {
                     DisplayName = s.DisplayName,
                     UserName = s.Username,
                     LastLoggedIn = s.LastLoggedIn,
                     CreatedOnDate = s.CreatedOnDate,
                     UserId = s.Id,
                     UserAccountState = s.UserAccountState,
                     Roles = s.UserRoles.Select(r => r.Role.Name).ToArray(),
                     EmailAddress = s.EmailAddress,
                     MobileNumber = s.MobileNumber,
                     EmailVerification = s.EmailVerification,
                     MobileNumberVerification = s.MobileNumberVerification,
                     CompanyId=s.UserCompanyId,
                     CompanyName=s.UserCompany.CompanyName
                     


                 }).ToListAsync();



            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }

        public async Task<ApiResult<User>> CompanyRegisterAsync(CompanyRegister model)
        {

            var res = new ApiResult<User>(true, ApiResultStatusCode.Success, new User());
            //چک شود نام کاربری قبلا ثیت نام نکرده باشد.
            try
            {
                if( await _users.AnyAsync(w=>w.Username==model.UserName))
                {
                    res.IsSuccess = false;
                    res.Messages = "نام کاربری وارد شده تکراری می باشد";
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;

                }


                var companyRole = await _role.FirstOrDefaultAsync(w => w.Name == CustomRoles.Company);


                var company = new UserCompany()
                {
                    CompanyName = model.CompanyName,
                    EnglishName = model.EnglishName,
                    CompanyRepresentative = model.CompanyRepresentative,
                    EstablishedYear = model.EstablishedYear,
                    TxtTinNo = model.TxtTinNo,
                    TxtRegNO = model.TxtRegNO,
                    CreatedOnDate = DateTime.Now,
                    MobileNumber = model.MobileNumber,
                    Email = model.Email,
                    UserCompanyAccountStatus = UserCompanyAccountStatusEnum.در_دست_بررسی,
                    RejectDesription="ثبت نام شما در دست بررسی می باشد"

                };

                await _userCompany.AddAsync(company);



                var passwordHash = _securityService.GetSha256Hash(model.Password); 
                var user = new User
                {
                    DisplayName = model.CompanyName,
                    Username = model.UserName,
                    UserAccountState = userAccountStateEnum.فعال,
                    Password = passwordHash,
                    SerialNumber = Guid.NewGuid().ToString("N"),
                    UserCompany= company,
                    CreatedOnDate=DateTime.Now,
                    MobileNumber=model.MobileNumber,
                    EmailAddress=model.Email

                };
                
                var add = await _users.AddAsync(user);
                if(companyRole != null)
                {
                    var role = new UserRole() { RoleId = companyRole.Id, User = user };
                    await _userRole.AddAsync(role);
                }

               
               
                await _uow.SaveChangesAsync();
               
                res.Data = user;
            }
            catch (Exception er)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی در ثبت به وجود آمده است";
            }

            return res;
        }



        public Task<User> FindUserAsync(string username )
        { 
            return _users.FirstOrDefaultAsync(x => x.Username == username  );
        }


        public string CalculateMD5Hash(string input)

        {

            try
            {
                // step 1, calculate MD5 hash from input

                var md5 = System.Security.Cryptography.MD5.Create();

                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

                byte[] hash = md5.ComputeHash(inputBytes);

                // step 2, convert byte array to hex string

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {

                    sb.Append(hash[i].ToString("X2"));

                }

                return sb.ToString();
            }
            catch (Exception er)
            {

                
            }

            return "";
        }


        public async Task<User> FindUserAsync(string username, string password)
        { 
            var passwordHash = _securityService.GetSha256Hash(password);
            var user = await _users.Include(i => i.UserCompany).FirstOrDefaultAsync(x =>
                x.Username == username && x.Password == passwordHash);
            return user;
        }


        public async Task<string> GetSerialNumberAsync(int userId)
        {
            var user = await FindUserAsync(userId);
            return user.SerialNumber;
        }

        public async Task UpdateUserLastActivityDateAsync(int userId)
        {
            var user = await FindUserAsync(userId);
            if (user.LastLoggedIn != null)
            {
                var updateLastActivityDate = TimeSpan.FromMinutes(2);
                var currentUtc = DateTime.UtcNow;
                var timeElapsed = currentUtc.Subtract(user.LastLoggedIn.Value);
                if (timeElapsed < updateLastActivityDate)
                {
                    return;
                }
            }
            user.LastLoggedIn = DateTime.UtcNow;
            await _uow.SaveChangesAsync();
        }

        public int GetCurrentUserId()
        {
            if (_contextAccessor.HttpContext == null)
                return 0;

            var claimsIdentity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userDataClaim = claimsIdentity?.FindFirst(ClaimTypes.UserData);
            var userId = userDataClaim?.Value;
            return string.IsNullOrWhiteSpace(userId) ? 0 : int.Parse(userId);


          

        }


        public int GetCurrentUserCompanyId()
        {
            var userId ="0";
               var claimsIdentity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
               var UserCompanyIdclaims =   claimsIdentity.Claims.Where(w => w.Type == "UserCompanyId").FirstOrDefault() ;
                userId = UserCompanyIdclaims?.Value;

            }
           
          
            return string.IsNullOrWhiteSpace(userId) ? 0 : int.Parse(userId); 
        }






        public ValueTask<User> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();
            return FindUserAsync(userId);
        }

        public async Task<ApiResult<string>> ChangeCurrentUserPasswordAsync(ChangePasswordViewModel model)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "کلمه عبور شما با موفقیت تغییر یافت");
            try
            {
                var userId = GetCurrentUserId();
                var user =await FindUserAsync(userId);
                if(user==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "کاربری یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;

                }

                var currentPasswordHash = _securityService.GetSha256Hash(model.OldPassword);
                if (user.Password != currentPasswordHash)
                {
                    res.IsSuccess = false;
                    res.Messages = "کلمه عبور فعلی صحیح نمی باشد.";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                }

                user.Password = _securityService.GetSha256Hash(model.NewPassword);

                await _event.AddAsync(new Event()
                {
                    ActionName = "ChangeCurrentUserPasswordAsync",
                    Description = "تغییر کلمه عبور",
                    CreateDate = DateTime.Now,
                    EventPriority = EventPriorityEnum.Important,
                    EventSection = EventSectionEnum.تغییر_کلمه_عبور_کاربر_جاری,
                    OperationId = userId,
                    EventType = EventTypeEnum.Info,
                    UserId = userId,

                });


                await _uow.SaveChangesAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.NotFound;

            }


            return res; 
           
        }




        public async Task<ApiResult<string>> ChangeUserPasswordAsync(AdminChangePasswordViewModel model,int? userId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "کلمه عبور  با موفقیت تغییر یافت");
            try
            { 
                var user = await FindUserAsync(model.UserId);
                if (user == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "کاربری یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;

                }

                if (userId == 0)
                    userId = null;



                user.Password = _securityService.GetSha256Hash(model.NewPassword);

                await _event.AddAsync(new Event()
                {
                    ActionName = "ChangeUserPasswordAsync",
                    Description = "تغییر کلمه عبور",
                    CreateDate = DateTime.Now,
                    EventPriority = EventPriorityEnum.Important,
                    EventSection = EventSectionEnum.تغییر_کلمه_عبور_کاربر,
                    OperationId = userId,
                    EventType = EventTypeEnum.Info,
                    UserId = model.UserId,

                });


                await _uow.SaveChangesAsync(); 

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.NotFound;

            }


            return res;

        }






        public async Task<ApiResult<string>> SetAllUserPasswordAsync()
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "کلمه عبور  با موفقیت تغییر یافت");
            try
            {
                var users = await _users.ToListAsync();
                foreach (var item in users)
                {
                    item.Password = _securityService.GetSha256Hash(item.Username);
                    _users.Update(item); 
                }

               
                await _uow.SaveChangesAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.NotFound;

            }


            return res;

        }


        #region User Group Permision
        public async Task<ApiResult> AddUserToPermissionGroup(UserPermissionGroupDto model,int userId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت اضافه گردید");
            try
            {
                if(await _userPermissionGroup.AnyAsync(w=>w.UserId==model.UserId && w.PermissionGroupId==model.PermissionGroupId ))
                {
                    res.IsSuccess = false;
                    res.Messages = "قبلا اضافه شده است";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;

                }


                var user = await _userPermissionGroup.AddAsync(new UserPermissionGroup()
                {
                    PermissionGroupId= model.PermissionGroupId,
                    UserId= model.UserId

                });


                await _event.AddAsync(new Event()
                {
                    ActionName = "AddUserToPermissionGroup",
                    Description = "اضافه کردن گروه دسترسی به کاربر",
                    CreateDate = DateTime.Now,
                    EventPriority = EventPriorityEnum.Important,
                    EventSection = EventSectionEnum.اضافه_کردن_گروه_دسترسی_به_کاربر,
                    OperationId = userId,
                    EventType = EventTypeEnum.Info,
                    UserId = model.UserId,
                    Code = model.PermissionGroupId

                });




                await _uow.SaveChangesAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.NotFound;

            }


            return res;

        }

        public async Task<ApiResult> RemoveUserPermissionGroup(UserPermissionGroupDto model,int userId)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت حذف گردید");
            try
            {

                var item = await _userPermissionGroup.FirstOrDefaultAsync(w => w.UserId == model.UserId && w.PermissionGroupId == model.PermissionGroupId);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.Data = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                _userPermissionGroup.Remove(item);

                await _event.AddAsync(new Event()
                {
                    ActionName = "RemoveUserPermissionGroup",
                    Description = "حذف کردن گروه دسترسی به کاربر",
                    CreateDate = DateTime.Now,
                    EventPriority = EventPriorityEnum.Important,
                    EventSection = EventSectionEnum.حذف_کردن_گروه_دسترسی_به_کاربر,
                    OperationId = userId,
                    EventType = EventTypeEnum.Info,
                    UserId = model.UserId,
                    Code= model.PermissionGroupId

                });

                await _uow.SaveChangesAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.NotFound;

            }


            return res;

        }
        public async Task<ApiResult<List<UserPermissionGroupInfo>>> GetAllUserPermissionGroup(int userId)
        {
            var res = new ApiResult<List<UserPermissionGroupInfo>>(true, ApiResultStatusCode.Success, new List<UserPermissionGroupInfo>());

            try
            {
                res.Data = await _userPermissionGroup.Where(w => w.UserId == userId)
                     .Select
                      (s => new UserPermissionGroupInfo()
                      {
                          
                          UserId = s.UserId,
                          UserName = s.User.Username,
                          Id=s.Id,
                          PermissionGroup=s.PermissionGroup.Name,
                          PermissionGroupId=s.PermissionGroupId,

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




        #endregion




    }
}
