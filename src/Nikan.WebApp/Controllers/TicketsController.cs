using cle.Services;
using cle.Services.Faq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.ViewModel;
using Nikan.ViewModel.Ticket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nikan.Controllers.Api
{


        [Route("api/[controller]")]
        [ApiController] 
        [EnableCors("CorsPolicy")]
        [Authorize()]
        [ApiExplorerSettings(IgnoreApi = true)]
    public class TicketsController : Controller
        {
        #region Field

        private readonly IOrganizationalUnitService _unit;
        private readonly IOrganizationService _organization;
        private readonly ITicketService _ticket;
        private readonly IUsersService _usersService;
        private readonly ITicketSubjecteService _ticketSubjecteService;
        private readonly ISiteSettingService _siteSettingService;
        private readonly ISmsInfoService _smsService;

        #endregion
        #region Constractor
        /// <summary>
        /// سازنده
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="userManager"></param>
        /// <param name="ticket"></param>
        /// <param name="organization"></param>
        public TicketsController(IOrganizationalUnitService skill
           , IUsersService userManager
           , ITicketSubjecteService  ticketSubjecteService 
           , ITicketService ticket,
             ISiteSettingService siteSettingService,
             ISmsInfoService smsService
           , IOrganizationService organization

            )
        {
            _unit = skill;
            _smsService = smsService;
            _siteSettingService = siteSettingService;
            _organization = organization;
            _ticket = ticket; 
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));
            _ticketSubjecteService = ticketSubjecteService;
        }
        #endregion


        /// <summary>
        /// ارسال تیکت
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ApiResult<TicketResultDto>> SendUserTicket([FromBody]SendUserTicketDto data)
        { 
            data.UserId = _usersService.GetCurrentUserId();
            return await _ticket.AddTicket(data,Common.GlobalEnum.TicketSectionEnum.Ticket); 
        }

        /// <summary>
        /// ثبت تماس در قسمت تماس با ما
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ApiResult<TicketResultDto>> AddContact([FromBody]ContactDto  data)
        {
            if (data == null)
                return new ApiResult<TicketResultDto>(false, ApiResultStatusCode.ServerError, null, "مدل ورودی صحیح نمی باشد");

           data.UserId = _usersService.GetCurrentUserId();
            return  await _ticket.AddContact(data,Common.GlobalEnum.TicketSectionEnum.Contact);
        }



        /// <summary>
        /// ارسال پاسخ تیکت
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<TicketResultDto>> SendAnswerTicket([FromBody]ResponseTicketDto data)
        {


           
            var userId = _usersService.GetCurrentUserId(); 
            data.OwnerId = userId;
            var res = await _ticket.AddAnswer(data);
            if (res.IsSuccess && data.SendSms && !string.IsNullOrEmpty(res.Data.MobileNumber))
            {
                
                ////ارسال پیامک پاسخ
                Nikan.Common.SendSms sms = new SendSms();
                var smsOption = await _siteSettingService.GetSmsSettingForSend();
                if (smsOption == null)
                {
                    res.Messages = "تنظیمات پیامک انجام نشده است ";
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    return res;
                }

                var smsLog = await sms.VerifyLookup(res.Data.MobileNumber, res.Data.Code, TempleteNameEnum.ResponseTicket, smsOption.SmsToken);
                if (smsLog.IsSuccess)
                { 
                    var saveSms = await _smsService.Add(smsLog.Data, null); 
                }

            }


            return res;
        }


        /// <summary>
        /// وضعیت جدید تیکت
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize()]
        public async Task<ApiResult<TicketResultDto>> UpdateTicketsStatues([FromBody]TicketChangeState data)
        {
           
            var userId = _usersService.GetCurrentUserId(); 
            return await _ticket.UpdateTicketsStatues(data.TicketId, data.IsClosed, userId);

        }

        /// <summary>
        /// دریافت جزئیات یک تیکت
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetById")]
        public async Task<ApiResult<TicketDetailsDto>>   GetById(string id)
        {

            return await _ticket.GetTickets(id);

        }


        /// <summary>
        /// دریافت پاسخ های یک تیکت
        /// </summary>
        /// <param name="refCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAnswerTicket")]
        [AllowAnonymous]
        public async Task<ApiResult<ResponseTicketDto>> GetAnswerTicket(string refCode)
        {
            return await _ticket.GetAnswerTicket(refCode);

        }



        /// <summary>
        /// دریافت تمامی تیکت های کاربران
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserTicketsList")]
        public async Task<ApiResult<PagedTicketItemsViewModel>> GetUserTicketsList(int? userId = null)
        {
            if ( userId==null) 
                  userId = _usersService.GetCurrentUserId();
            
            return await _ticket.GetAllUserTicket(userId.Value);
        }


        [HttpGet]
        [Route("GetAdminTicketsList")]
        [Authorize(Policy = CustomRoles.Admin)] 
        public async Task<ApiResult<PagedTicketItemsViewModel>> GetAdminTicketsList(
            int offset=1, int count=20,
           TicketStatusEnum? ticketStatus = null,
            bool? Iscolsed = null,
            bool? IsSolved = null,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string ownerName = null,
            string title = null)
        {

           var  userId = _usersService.GetCurrentUserId(); 
            return await  _ticket.GetPagedTicketItemsAsync(userId,offset, count, ticketStatus,
                Iscolsed, IsSolved, FromDate, ToDate, ownerName, title);
        }


        [HttpGet]
        [Route("GetCardTicketsList")]
        [Authorize(Policy = CustomRoles.Card)]
        public async Task<ApiResult<PagedTicketItemsViewModel>> GetCardTicketsList(
          int offset = 1, int count = 20,
         TicketStatusEnum? ticketStatus = null,
          bool? Iscolsed = null,
          bool? IsSolved = null,
          DateTime? FromDate = null,
          DateTime? ToDate = null,
          string ownerName = null,
          string title = null)
        {

            var userId = _usersService.GetCurrentUserId();
            return await _ticket.GetPagedTicketItemsAsync(userId,offset, count, ticketStatus,
                Iscolsed, IsSolved, FromDate, ToDate, ownerName, title);
        }

        /// <summary>
        /// دریافت لیست تیکت های یک شرکت
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="ticketStatus"></param>
        /// <param name="Iscolsed"></param>
        /// <param name="IsSolved"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="ownerName"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCompanyTicketsList")]
        [Authorize(Policy = CustomRoles.Company)]
        public async Task<ApiResult<PagedTicketItemsViewModel>> GetCompanyTicketsList(
            int offset = 1, int count = 20,
           TicketStatusEnum? ticketStatus = null,
            bool? Iscolsed = null,
            bool? IsSolved = null,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string ownerName = null,
            string title = null)
        {
            var userId = _usersService.GetCurrentUserId();
            var companyId = _usersService.GetCurrentUserCompanyId();
            return await _ticket.GetPagedTicketItemsAsync(userId,offset, count, ticketStatus,
                Iscolsed, IsSolved, FromDate, ToDate, ownerName, title, companyId);
        }



        [HttpGet]
        [Route("GetContactList")] 
        public async Task<ApiResult<PagedContactViewModel>> GetContactList(
           int offset = 1, int count = 20,
            int? companyId = null,
           DateTime? FromDate = null,
           DateTime? ToDate = null, 
           string title = null)
        {


            return await _ticket.GetPagedContactAsync(offset, count, companyId,    FromDate, ToDate, title);
        }


        [HttpGet]
        [Route("GetComapnyContactList")] 
        public async Task<ApiResult<PagedContactViewModel>> GetComapnyContactList(
           int offset = 1, int count = 20, 
           DateTime? FromDate = null,
           DateTime? ToDate = null,
           string title = null)
        {

            var companyId = _usersService.GetCurrentUserCompanyId();
            return await _ticket.GetPagedContactAsync(offset, count, companyId, FromDate, ToDate, title);
        }


        [HttpGet]
        [Route("GetContactInfo")]
        public async Task<ApiResult<ContactDetailsDto>> GetContactInfo( string  id  )
        { 
            return await _ticket.GetContactAsync(id);
        }


        /// <summary>
        /// ثبت کامنت بر روی تیکت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize]
        public async Task<ApiResult<string>> AddComments([FromBody]TicketCommentsDto model)
        {
         
            var ownerId  = _usersService.GetCurrentUserId(); 
            return await _ticket.AddComments(model, ownerId);
        }


        /// <summary>
        /// دریافت کامنت های یک تیکت
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTicketComments")]
        public async Task<ApiResult<List<TicketCommentsViewModel>>> GetTicketComments(string ticketId)
        {
            var ownerId = _usersService.GetCurrentUserId();
            return await _ticket.GetTicketComments(ticketId, ownerId);
        }



        /// <summary>
        /// ثبت فعالیت روی تیکت
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<string>> AddActivity([FromBody]TicketActivityDto model)
        {
             var  ownerId = _usersService.GetCurrentUserId();
            return await _ticket.AddActivity(model, ownerId);
        }


        /// <summary>
        /// حذف کامنت از روی تیکت
        /// </summary>
        /// <param name="id">شناسه کامنت</param>
        /// <returns></returns>
        [HttpGet]
        [Route("RemoveComments")]
        public async Task<ApiResult<string>> RemoveComments(int id)
        {
            var ownerId = _usersService.GetCurrentUserId();
            return await _ticket.RemoveComments(id, ownerId);
        }



        [HttpGet]
        [Route("GetTicketActivity")]
        public async   Task<ApiResult<List<TicketActivityViewModel>>> GetTicketActivity(string ticketId)
        {
            return await _ticket.GetTicketActivity(ticketId);
        }

        [IgnoreAntiforgeryToken]
        [HttpGet("[action]")]
        public async Task<ApiResult<string>> RemoveActivity(int id)
        {
            var ownerId = _usersService.GetCurrentUserId();
            return await _ticket.RemoveActivity(id, ownerId);
        }


        #region ticketSubjecte

        [HttpGet]
        [Route("GetAllTicketSubject")]
        public async Task<ApiResult<List<TicketSubjectInfo>>> GetAllTicketSubject()
        {
            return await _ticketSubjecteService.GetAll();
        }


        [HttpGet]
        [Route("GetListTicketSubject")]
        [AllowAnonymous]
        public async Task<ApiResult<List<BaseDataModel>>> GetListTicketSubject()
        {

            return await _ticketSubjecteService.GetList();
        }


        [HttpGet]
        [Route("GetListTicketSubjectByUnitId")]
        [AllowAnonymous]
        public async Task<ApiResult<List<BaseDataModel>>> GetListTicketSubjectByUnitId(string unitId)
        {

            return await _ticketSubjecteService.GetList(unitId);
        }


        [HttpGet]
        [Route("GetTicketSubject")]
        public async Task<ApiResult<TicketSubjectInfo>> GetTicketSubject(string id)
        {

            return await _ticketSubjecteService.GetSubject(id);
        }


        [IgnoreAntiforgeryToken]
        [HttpGet("[action]")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<string>> RemoveTicketSubject(string id)
        {
             
            return await _ticketSubjecteService.Remove(id);
        }


        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [Authorize(Policy = CustomRoles.Admin)]
        public async Task<ApiResult<TicketSubjectDto>> AddOrUpdateTicketSubject(TicketSubjectDto model)
        { 
            return await _ticketSubjecteService.AddOrUpdate(model);
        }




        #endregion 


    }
}