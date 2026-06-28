using cle.Services;
 
using cle.Services.UserCompanyServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nikan.Common;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.Services.CitizenCards;
using Nikan.Services.Citizens;
using Nikan.ViewModel.Report;
using System;
using System.Threading.Tasks;


namespace Nikan.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ReportController : Controller
    {
        #region Ctor

        private readonly IUsersService _usersService;
       
        private readonly ISiteSettingService _siteSettingService;
        private readonly ISmsInfoService _smsService;
        private readonly IUserCompanyService _userCompanyService;
        private readonly ITicketService _ticket;
        private readonly ICitizenService _citzene;
        private readonly ICitizenCardService _citzencard;


        public ReportController( 
            ISiteSettingService siteSettingService,
            ITicketService ticket,
            ICitizenService  citzene,
             ICitizenCardService citzencard,
             ISmsInfoService smsService,
             IUserCompanyService  userCompanyService,
        
              IUsersService userManager

            )
        {

            _citzene = citzene;
            _ticket = ticket;
            _smsService = smsService; 
            _siteSettingService = siteSettingService;
            _citzencard = citzencard;
              _userCompanyService = userCompanyService;
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));

        }
        #endregion



        /// <summary>
        /// داشبورد مدیریت 
        /// </summary>
        /// <returns></returns> 
        [HttpGet]
        [Route("GetStatisticalReport")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<AdminDashbordStatisticalReport>> GetStatisticalReport()
        {
            var res = new ApiResult<AdminDashbordStatisticalReport>(true, ApiResultStatusCode.Success, new AdminDashbordStatisticalReport(), "");

            try
            {
               

                var ticketCount =await _ticket.CountForReport();
                res.Data.NewTicketCount = ticketCount.Data.NewTicketCount;
                res.Data.AllTicketCount = ticketCount.Data.AllTicketCount;
                res.Data.ClosedTicketCount = ticketCount.Data.ClosedTicketCount;

              
                
                var citizenCount = await _citzene.CountForReport();
                res.Data.AllAcceptCitizenCount = citizenCount.Data.AllAcceptCitizenCount;
                res.Data.AllAcceptCitizenPictureCount = citizenCount.Data.AllAcceptCitizenPictureCount;
                res.Data.AllCitizenCount = citizenCount.Data.AllCitizenCount;
                res.Data.CitizenTodayCount = citizenCount.Data.CitizenTodayCount;
                res.Data.DeathCitizen = citizenCount.Data.DeathCitizen;
                res.Data.CitizenGroupCount = citizenCount.Data.CitizenGroupCount;

                res.Data.SmsCredit = "-2";
                var smsSettings= await _siteSettingService.GetSmsSettings();
              

                if (smsSettings.IsSuccess)
                {
                    SendSms sms = new SendSms();
                    var token = smsSettings.Data.SmsToken;
                    if(!string.IsNullOrWhiteSpace(token))
                        res.Data.SmsCredit = await sms.RemainCredit(token);

                }
               



            }
            catch (Exception er)
            {

                
            }



            return res;


        }


        [HttpGet]
        [Route("GetStatisticalCardReport")]
        [Authorize(Roles = "Admin,Card")]
        public async Task<ApiResult<CardDashbordStatisticalReport>> GetStatisticalCardReport()
        {
            var res = new ApiResult<CardDashbordStatisticalReport>(true, ApiResultStatusCode.Success, new CardDashbordStatisticalReport(), "");

            try
            { 
               return  await _citzencard.CountForReport();  
            }
            catch (Exception er)
            {


            }



            return res;


        }



        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<HiChartData>> GetCitizenRegisterChartReport(BetweenDate model)
        {
            return await _citzene.GetCitizenRegisterChartReport(model);
        }    
        


    }
}