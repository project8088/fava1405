using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.BaseEntity;
using Nikan.DomainClasses.Citizens;
using Nikan.ViewModel.Citizens;
using System;
using System.Threading.Tasks;


namespace Nikan.Services.Citizens
{

    public interface IUserLoginTicketsService
    {
        Task<ApiResult<string>> CreateLoginTicket(int serviceId, int userId,string returnUrl = "");
        Task<ApiResult<string>> CreateLoginByTicketUrl(CreateAccessTicketDto model, int userId,  string returnUrl = "");
    }


    public  class UserLoginTicketsService: IUserLoginTicketsService
    {

        #region Ctor
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Citizen> _citizen;
        private readonly DbSet<GroupsCitizens> _citizenGroups;
        private readonly DbSet<AppServices> _appServices;
        private readonly DbSet<CitizenProfile> _profile;
        private readonly DbSet<SiteOption> _SiteOptions;
        private readonly DbSet<User> _users;
        private readonly DbSet<Role> _role;
        private readonly DbSet<UserRole> _userRole;
        private readonly DbSet<SmsInfo> _sms;
        private readonly DbSet<Nationality> _nationality;
        private readonly DbSet<UserLoginTickets> _userLoginTickets;



        private readonly ISecurityService _securityService;
        public UserLoginTicketsService(IUnitOfWork uow,
            ISecurityService securityService,

            IHttpContextAccessor contextAccessor)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _appServices = _uow.Set<AppServices>();
            _citizenGroups = _uow.Set<GroupsCitizens>();
            _citizen = _uow.Set<Citizen>();
            _profile = _uow.Set<CitizenProfile>();
            _SiteOptions = _uow.Set<SiteOption>();
            _users = _uow.Set<User>();
            _userRole = _uow.Set<UserRole>();
            _role = _uow.Set<Role>();
            _userLoginTickets = _uow.Set<UserLoginTickets>();
            _sms = _uow.Set<SmsInfo>();
            _nationality = _uow.Set<Nationality>();
            _securityService = securityService;
            _securityService.CheckArgumentIsNull(nameof(_securityService));
        }

        #endregion

         



      public async Task<ApiResult<string>> CreateLoginTicket(int serviceId,    int citizenId, string returnUrl = "")
        {

            var res = new ApiResult<string>(true, ApiResultStatusCode.Success,"", "تیکت دسترسی با موفقیت ایجاد گردید");
            try
            {
                if(citizenId == 0)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "شناسه شهروند را وارد نمایید";
                    return res;
                }
                var app =await _appServices.FirstOrDefaultAsync(w => w.Id == serviceId);
                if(app==null)
                { 

                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "سرویسی یافت نشد";
                    return res; 
                }
                var link = app.Link;
                if (app.IsLinkService)
                {
                    //لینک به سرویس خارجی
                    var ticket = Guid.NewGuid();
                    var item = new UserLoginTickets
                    {
                        AppServicesId = serviceId,
                        SourceId = 0,//پروفایل شهروندی
                        CreationDate = DateTime.Now,
                        UserId = citizenId,
                       // CreatedByUserId= citizenId,
                        UserTicket = ticket,
                        ReturnUrl= returnUrl,
                        
                        
                        
                    };
                    await _userLoginTickets.AddAsync(item);
                    await _uow.SaveChangesAsync();
                    link += "?ticket=" + ticket.ToString();
                } 
               
                res.Data = link; 
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "امکان ورود به خدمت مورد نظر در حال حاضر امکان پذیر نیست"; 
            }


            return res;

        }





        public async Task<ApiResult<string>> CreateLoginByTicketUrl(CreateAccessTicketDto model,  int userId ,    string returnUrl = "")
        {

            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "", "عملیات با موفقیت صورت گرفت");
            try
            {
                if (model.UserCode == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "شناسه کاربری شهروند را وارد نمایید";
                    return res;
                }
                var appService = await _appServices.AsNoTracking().FirstOrDefaultAsync(w => w.Id == model.ServiceId);
                if (appService==null)
                {

                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    res.Messages = "سرویسی یافت نشد";
                    return res;
                }
                if (!await _appServices.AsNoTracking().AnyAsync(w => w.Id == model.SourceServiceId))
                {

                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    res.Messages = "سرویسی یافت نشد";
                    return res;
                }

                var citizen = await _citizen.AsNoTracking().FirstOrDefaultAsync(w => w.UserCode == model.UserCode);

                if (citizen==null)
                {

                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }

                //لینک به سرویس خارجی
                var ticket = Guid.NewGuid();
                    var item = new UserLoginTickets
                    {
                        AppServicesId = model.ServiceId,
                        CreationDate = DateTime.Now,
                        UserId = citizen.CitizenId,
                        UserTicket = ticket,
                        ReturnUrl = returnUrl,
                        SourceId=model.SourceServiceId,
                        //ParamName1= appService.ParamName1,
                        //ParamName2= appService.ParamName2,
                        //ParamValue1= appService.ParamValue1,
                        //ParamValue2= appService.ParamValue2, 
                        //CreatedByUserId=userId,
                        
                    };
                    await _userLoginTickets.AddAsync(item);
                    await _uow.SaveChangesAsync();
                    
                if(string.IsNullOrWhiteSpace(appService.Link))
                {
                    var link  = appService.Link+  "?ticket=" + ticket.ToString();
                    res.Data = link;
                }
                else
                {
                    res.Data = ticket.ToString();
                }

               
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";
            }


            return res;

        }







    }
}
