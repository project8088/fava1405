using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cle.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nikan.Common;
using Nikan.MellatApp;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.Services.Citizens;
using Nikan.Services.Refund;
using Nikan.ViewModel.Refund;

namespace Nikan.WebApp.Controllers
{
    /// <summary>
    /// سرویس برگشت هزینه
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TransactionRefundController : Controller
    {

        #region Ctor

        private readonly IUsersService _usersService;
        public static IHostingEnvironment _environment;
        private readonly ICitizenService _citzene; 
        private readonly ISmsInfoService _smsInfoService;
        private readonly ICitizenFeedbackService _citizenFeedback;
        private readonly ISiteSettingService _siteSettingService;
        private readonly IRefundService _refund;
        private readonly ITransactionService _transactionService; 
        public TransactionRefundController(
              IUsersService userManager,
              ICitizenService citzene, 
            IRefundService  refund,
            ITransactionService transactionService,
            IHostingEnvironment environment,
            ISmsInfoService smsInfoService, 
            ISiteSettingService siteSettingService 
           

            )
        {
            _citzene = citzene; 
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));
            _environment = environment;
            _siteSettingService = siteSettingService;
            _refund =  refund;
            _transactionService = transactionService;
            _smsInfoService = smsInfoService; 
        }
        #endregion

       


       


        /// <summary>
        /// جزئیات لیست عودت های کارشناسان مراکز
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="nationCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("OperationImportDetailsList")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<PagedRefundImportCitizenListViewModel>> OperationImportDetailsList(
         int? offset = 1, int? count = 20,
         DateTime? FromDate = null,
         DateTime? ToDate = null,
         string nationCode = null
           )
        {
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;

            var userId = _usersService.GetCurrentUserId(); 

            return await _refund.OperationImportDetailsList(offset.Value, count.Value, userId, FromDate,
                 ToDate, nationCode);

        }



        



       
     


        /// <summary>
        /// عدم تایید برگشت هزینه
        /// </summary>
        /// <param name="refundId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DisagreementRefund")]
        [Authorize(Roles = "Admin")]
        public virtual async Task<ApiResult> DisagreementRefund(int refundId)
        {
          
            var result = new ApiResult<string>(false, ApiResultStatusCode.Success, "", "");
            var userId = _usersService.GetCurrentUserId();

            return await _refund.DisagreementRefund(refundId, userId);
             
        }





    }
}