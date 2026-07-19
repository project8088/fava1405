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
using Nikan.Common.GlobalEnum;
using Nikan.DataLayer.Context;
using Nikan.MellatApp;
using Nikan.Services;
using Nikan.Services.BaseEntity;
using Nikan.Services.CitizenCards;
using Nikan.Services.Citizens;
using Nikan.Services.Permissions;
using Nikan.ViewModel;
using Nikan.ViewModel.CitizenCards;
using Nikan.ViewModel.Transactions;

namespace Nikan.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class OrderController : Controller
    {

        #region Ctor

        private readonly IUsersService _usersService;
        public static IHostingEnvironment _environment;
        private readonly ICitizenService _citzene; 
        private readonly ITokenStoreService _tokenStoreService; 
        private readonly IAntiForgeryCookieService _antiforgery;
        private readonly ITokenFactoryService _tokenFactoryService;
        private readonly ISmsInfoService _smsInfoService;
        private readonly ICitizenFeedbackService _citizenFeedback;
        private readonly ISiteSettingService _siteSettingService;
        private readonly ICardService _card;
        private readonly ITransactionService _transactionService;
        private readonly ICitizenCardService _citizencard;
        private readonly IPermissionService _permission;


        public OrderController(
              IUsersService userManager,
              ICitizenService citzene,
              ICitizenFeedbackService citizenFeedback,
              IAppService app,
            ICardService card,
           IPermissionService permission,
        ITransactionService  transactionService,
            IHostingEnvironment environment,
            ISmsInfoService smsInfoService,
            ITokenStoreService tokenStoreService,
            ITokenFactoryService tokenFactoryService,
            IUnitOfWork uow,
             ICitizenCardService citizencard,
            ISiteSettingService siteSettingService,
            IAntiForgeryCookieService antiforgery


            )
        {
            _citzene = citzene;
            _citizenFeedback = citizenFeedback;
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));
            _environment = environment;
            _siteSettingService = siteSettingService;
            _card = card;
            _transactionService = transactionService;
            _smsInfoService = smsInfoService;
            _antiforgery = antiforgery;
            _antiforgery.CheckArgumentIsNull(nameof(antiforgery));
            _citizencard = citizencard;
            _tokenStoreService = tokenStoreService;
            _tokenStoreService.CheckArgumentIsNull(nameof(tokenStoreService));
            _tokenFactoryService = tokenFactoryService;
            _tokenFactoryService.CheckArgumentIsNull(nameof(tokenFactoryService));
              _permission = permission;

        }
        #endregion 
        
        
        

        /// <summary>
        /// لیست کارتهای موجود برای خرید شهروند
        ///[Card2]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Citizen")] 
        [Route("ListAvailableCards")]
        public async Task<ApiResult<List<CardInfoViewModel>>> ListAvailableCards()
        { 
            return await _card.ListAvailableCards(); 
        }

        /// <summary>
        /// آیا شهروند می تواند این کارت را خرید کند یا نه ؟
        /// </summary>
        /// <param name="cardInfoId">شناسه کارت</param>
        /// <returns></returns>
         [HttpGet]
         [Authorize(Roles = "Citizen")]
         [Route("CheckCanOrderCard")]
       // [AllowAnonymous]
        public async Task<ApiResult<string>> CheckCanOrderCard(string cardInfoId)
        {
            var citizenId =   _usersService.GetCurrentUserId();  
            return await _card.CheckCanOrderCard(new CheckCanOrderCardDto() {CardInfoId= cardInfoId,CitizenId= citizenId });
        }

         
         
        /// <summary>
        /// دریافت همه تراکنش های شهروند
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="orderId"></param>
        /// <param name="referenceId"></param>
        /// <param name="transactionState"></param>
        /// <param name="transactionFor"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Citizen")]
        [Route("GetAllTransactionsForCitizens")]
        public async Task<ApiResult<PagedTransactionsViewModel>> GetAllTransactionsForCitizens
          (int? offset = 1, int? count = 20,
           DateTime? FromDate = null,
           DateTime? ToDate = null,
           string orderId = null,
           string referenceId = null,
           TransactionStateEnum? transactionState = null,
           TransactionForEnum? transactionFor = null
          )
        {
            var citzenId = _usersService.GetCurrentUserId();//دریافت شناسه شهروند جاری
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _transactionService.GetAll(offset.Value, count.Value, citzenId,   FromDate,
                 ToDate, orderId, referenceId, transactionState, transactionFor,null);
             
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetAllCitizenTransactions")]
        public async Task<ApiResult<PagedTransactionsViewModel>> GetAllCitizenTransactions 
          (
            int citizenId,
            int? offset = 1, int? count = 20,  
            string orderId = null,
            string referenceId = null 
            
          )
        {
            
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _transactionService.GetAll(offset.Value, count.Value, citizenId, null,
                 null, orderId, referenceId, null, null, null);

        }



        /// <summary>
        /// دریافت همه تراکنش های سمت پنل مدیریت
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="orderId"></param>
        /// <param name="referenceId"></param>
        /// <param name="transactionState"></param>
        /// <param name="transactionFor"></param>
        /// <param name="nationCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Card")]
        [Route("GetAllTransactions")]
        public async Task<ApiResult<PagedTransactionsViewModel>> GetAllTransactions
          (int? offset = 1, int? count = 20,
           DateTime? FromDate = null,
           DateTime? ToDate = null,
           string orderId = null,
           string referenceId = null,
           TransactionStateEnum? transactionState = null,
           TransactionForEnum? transactionFor = null,
            string nationCode = null
          )
        {
           
            if (offset == null || offset < 0) offset = 0;
            if (count == null || count < 1) count = 20;
            return await _transactionService.GetAll(offset.Value, count.Value, null, FromDate,
                 ToDate, orderId, referenceId, transactionState, transactionFor, nationCode);

        }






        /// <summary>
        /// جزئیات یک تراکنش
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTransaction")]
        [Authorize(Roles = "Admin,Citizen")]
        public async Task<ApiResult<TransactionInfo>> GetTransaction(long id)
        {
            var citzenId = _usersService.GetCurrentUserId();//دریافت شناسه شهروند جاری
            var getItem = await _transactionService.GetItem(id);
            if (getItem.IsSuccess)
            {
                if (getItem.Data.TransactionById == citzenId)
                    return getItem;
                //چک کن دسترسی به اطلاعات تراکنش دارد یا نه                
                ApiResult canAccess = await this._permission.CanAccess(citzenId, PermissionTypeEnum.تراکنش_های_مالی);
                if (!canAccess.IsSuccess)
                {
                    canAccess = await this._permission.CanAccessWebApi(citzenId, PermissionTypeEnum.تراکنش_های_مالی_کارت);
                    if (!canAccess.IsSuccess)
                        return new ApiResult<TransactionInfo>(false, ApiResultStatusCode.NotAllowed, null, "شما به جزئیات تراکنش  دسترسی ندارید");
                }
            }
            return getItem;
        }

        [HttpGet]
        [Route("CreateCardForCitizen")]
        [Authorize(Roles = "Admin")]
         //[AllowAnonymous]
        public async Task<ApiResult> CreateCardForCitizen(int id)
        {
            var citzenId = _usersService.GetCurrentUserId();//دریافت شناسه شهروند جاری
            return await _transactionService.CreateCardForCitizen(id, citzenId);

        }

        /// <summary>
        /// اطلاعات کارت برای خرید 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Citizen")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<OrderCardItem>>  CardPriceInfo(OrderAddressDto model)
        {
            if (model == null)
            {
                return new ApiResult<OrderCardItem>(false, ApiResultStatusCode.BadRequest,null, "مدل ورودی نامعتبر می باشد");

            }
            var citzenId = _usersService.GetCurrentUserId();//دریافت شناسه شهروند جاری
            var card=await    _card.GetCardInfoForBuy(model.CardInfoId, citzenId);
            if(!card.IsSuccess)
                return new ApiResult<OrderCardItem>(false, ApiResultStatusCode.BadRequest, null, card.Messages);

            var cardInfo = card.Data;
            var orderCard = new OrderCardItem()
            {
                AddressSend="",
                CardCost= cardInfo.CardCost,
                CardInfoId= cardInfo.CardInfoId,
                CardTypeTitle= cardInfo.CardType,
                DeliveringAddressId=model.AddressId,
                Description= cardInfo.BuyCardDescription,
                DoubleCardCost= cardInfo.DoubleCardCost,
                PostalCostInCity= cardInfo.PostalCostInCity,
                PostalCostOutCity= cardInfo.PostalCostOutCity,
                CenterDeliveryPossibility= cardInfo.CenterDeliveryPossibility,
                CenterList= cardInfo.CenterList,
                DiscountId= cardInfo.DiscountId,
                TotalDiscountAmount= cardInfo.TotalDiscountAmount,
                CardDiscountAmount= cardInfo.CardDiscountAmount,
                PostageDiscountAmount= cardInfo.PostageDiscountAmount,


            };

            return new ApiResult<OrderCardItem>(true, ApiResultStatusCode.Success, orderCard, "عملیات با موفقیت صورت گرفت");

        }

        [HttpGet]
        [Route("GetCitizenCardPriceInfo")]
        [Authorize(Roles = "Citizen")]
        public async Task<ApiResult<CardInfoViewModel>> GetCitizenCardPriceInfo(string cardInfoId)
        {
             
            var citzenId = _usersService.GetCurrentUserId();//دریافت شناسه شهروند جاری
            return await _card.GetCardInfoForBuy(cardInfoId, citzenId); 

        }


        [Authorize(Roles = "Citizen")]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<MellatModelDto>> BuyCardByCitizens(OrerCardDto model)
        {
            var res = new ApiResult<MellatModelDto>(true, ApiResultStatusCode.Success, new MellatModelDto());
            if (model == null)
            {
                res.IsSuccess = false;
                res.Messages = " مدل ارسالی معتبر نمی باشد ";
                return res;
            }

            if (model.CardInfoId == null)
            {
                res.IsSuccess = false;
                res.Messages = " شناسه کارت ارسالی معتبر نمی باشد ";
                return res;
            }
            


            try
            {
                var citzenId = _usersService.GetCurrentUserId();//دریافت شناسه شهروند جاری
                var getcard=await _card.GetCardInfoForBuy(model.CardInfoId, citzenId); 
                if (getcard.IsSuccess)
                {
                    var card = getcard.Data;
                    var amount = card.TotalAmount;
                    var orderId = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssf"));

                    if (amount < 1000)
                    {
                        
                        //کارت رایگان برای شهروند صادر شود
                        var addtransaction = await _transactionService.Add(new ViewModel.Transactions.TransactionDto()
                        {
                            AmountTransaction = amount,
                            Description = $" خرید با تخفیف 100 درصدی     {card.CardType}",
                         
                            TransactionById = citzenId,
                            TransactionFor = Common.GlobalEnum.TransactionForEnum.خرید_کارت,
                            OrderId = orderId.ToString(),
                           

                        },TransactionStateEnum.تایید_شده);
                        if (addtransaction.IsSuccess)
                        {
                            var orderCardInfo = new OrerCardDto()
                            {
                                CardInfoId = model.CardInfoId,
                                CitizenId = citzenId,
                                DeliveringAddressId = model.DeliveringAddressId,
                                TransactionId = addtransaction.Data.Id,
                                DiscountGroupId= getcard.Data.DiscountGroupId,
                                


                            };
                            var addCard = await _citizencard.OrderCardByCitizen(orderCardInfo,true);
                            if (addCard.IsSuccess)
                            {
                                res.IsSuccess = true;
                                res.Data.Isfree = true;
                                res.Messages = "کارت برای شما صادر گردید";
                                return res;
                            }
                            else
                            {
                                res.IsSuccess = false;
                                res.Messages = addCard.Messages;
                                return res;
                            }
                        }
                        else
                        {
                            res.IsSuccess = false;
                            res.Messages = addtransaction.Messages;
                            return res;
                        }
                         
                    }

                  
                   

                    var bankOption = await _siteSettingService.GetFinancialSettings();
                    if (bankOption.Data.BankTerminalId  == null)
                    {
                        res.IsSuccess = false;
                        res.Messages = "تنظیمات درگاه بانکی انجام نشده است";
                        return res;
                    }

                    var payment = new BankMellatImplement(bankOption.Data.BankTerminalId.Value,
                        bankOption.Data.BankUserName, bankOption.Data.BankPassword, bankOption.Data.BankCustomerId.Value, bankOption.Data.BankPaymentMethod.Value);

                    var result = payment.bpPayRequest(orderId, amount, "  خرید خدمات", bankOption.Data.CallBackUrl);
                    if (result != null)
                    {

                        if (result.ResultCode != "0")
                        {
                            res.IsSuccess = false;
                            res.Messages = result.Result;
                            return res;
                        }
                        else
                        {
                            var userId = _usersService.GetCurrentUserId();//دریافت شناسه کارجوی جاری
                            
                            //add transaction 
                            var addtransaction = await _transactionService.Add(new ViewModel.Transactions.TransactionDto()
                            {
                                AmountTransaction = amount,
                                Description = $"خرید     {card.CardType}",
                                TransactionById = userId,
                                TransactionFor = Common.GlobalEnum.TransactionForEnum.خرید_کارت,
                                OrderId = orderId.ToString(),  

                            });
                            if (addtransaction.IsSuccess)
                            {
                                var orderCardInfo = new OrerCardDto()
                                {
                                    CardInfoId = model.CardInfoId,
                                    CitizenId = userId,
                                    DeliveringAddressId = model.DeliveringAddressId,
                                    TransactionId = addtransaction.Data.Id,
                                    DiscountGroupId= getcard.Data.DiscountGroupId


                                };
                                var addCard = await _citizencard.OrderCardByCitizen(orderCardInfo);
                                if (addCard.IsSuccess)
                                {
                                    res.IsSuccess = true;
                                    res.Messages = result.Result;
                                    res.Data.RefId = result.RefId;
                                    res.Data.PgwSite = payment.PgwSite;
                                    res.Data.TransactionCode = orderId;
                                    res.Data.AmountTransaction = amount;
                                    return res;
                                }
                                else
                                {
                                    res.IsSuccess = false;
                                    res.Messages = addCard.Messages;
                                }
                            }
                            else
                            {
                                res.IsSuccess = false;
                                res.Messages = addtransaction.Messages;
                            }

                        }
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.Messages = getcard.Messages;

                }


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";

            }

            return res;
        }


        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        public async Task<ApiResult<MellatModelDto>> TestPay(PayViewModel model)
        {
            var res = new ApiResult<MellatModelDto>(true, ApiResultStatusCode.Success, new MellatModelDto());
            try
            {

                model.OrderId = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmssf"));

                var bankOption = await _siteSettingService.GetFinancialSettings();
                var payment = new BankMellatImplement(bankOption.Data.BankTerminalId.Value,
                    bankOption.Data.BankUserName, bankOption.Data.BankPassword, bankOption.Data.BankCustomerId.Value, bankOption.Data.BankPaymentMethod.Value);

                var result = payment.bpPayRequest(model.OrderId, model.Amount, " تست درگاه", bankOption.Data.CallBackUrl);
                if (result != null)
                {

                    if (result.ResultCode != "0")
                    {
                        res.IsSuccess = false;
                        res.Messages = result.Result;
                        return res;
                    }
                    else
                    {
                        var userId = _usersService.GetCurrentUserId();//دریافت شناسه کارجوی جاری

                        //add transaction 
                        var addtransaction = await _transactionService.Add(new ViewModel.Transactions.TransactionDto()
                        {
                            AmountTransaction = model.Amount,
                            Description = "تست درگاه پرداخت",
                            TransactionById = userId,
                            TransactionFor = Common.GlobalEnum.TransactionForEnum.تست_درگاه,
                            OrderId = model.OrderId.ToString(),


                        });
                        if (addtransaction.IsSuccess)
                        {
                            res.IsSuccess = true;
                            res.Messages = result.Result;
                            res.Data.RefId = result.RefId;
                            res.Data.PgwSite = payment.PgwSite;
                            res.Data.TransactionCode = model.OrderId;
                            res.Data.AmountTransaction = model.Amount;
                        }
                        else
                        {
                            res.IsSuccess = false;
                            res.Messages = addtransaction.Messages;
                        }


                    }

                }

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";



            }

            return res;
        }





        [HttpGet]
        [Route("CheckTransaction")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> CheckTransaction(int id)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "");
              
            try
            {
                var gettransaction = await _transactionService.GetItem(id);
                if (gettransaction.IsSuccess)
                {
                    var transaction = gettransaction.Data;
                    var transactionBankReferenceId = transaction.TransactionBankReferenceId;
                    var orderCode = transaction.OrderId;
                    if (string.IsNullOrWhiteSpace(transactionBankReferenceId))
                        return new ApiResult(false, ApiResultStatusCode.BadRequest, "کد پیگیری پرداخت ثبت نشده است");
                    long redId = 0;
                    if(!long.TryParse(transactionBankReferenceId,out redId))
                    {
                        return new ApiResult(false, ApiResultStatusCode.BadRequest, "کد پیگیری پرداخت نامعتبر می باشد");

                    }

                    if (string.IsNullOrWhiteSpace(orderCode))
                        return new ApiResult(false, ApiResultStatusCode.BadRequest, "شناسه واریز ثبت نشده است");
                    long orderId = 0;
                    if (!long.TryParse(orderCode, out orderId))
                    {
                        return new ApiResult(false, ApiResultStatusCode.BadRequest, "شناسه واریز   نامعتبر می باشد");

                    }




                    var bankOption = await _siteSettingService.GetFinancialSettings();
                    if (bankOption.Data.BankTerminalId == null)
                    {
                        res.IsSuccess = false;
                        res.Messages = "تنظیمات درگاه بانکی انجام نشده است";
                        return res;
                    }

                    var payment = new BankMellatImplement(bankOption.Data.BankTerminalId.Value,
                        bankOption.Data.BankUserName, bankOption.Data.BankPassword, bankOption.Data.BankCustomerId.Value, bankOption.Data.BankPaymentMethod.Value);

                    var result = payment.bpInquiryRequest(orderId, orderId, redId, bankOption.Data.BankTerminalId.Value, bankOption.Data.BankUserName, bankOption.Data.BankPassword);
                    if (result != null)
                    {

                        if (result.ResultCode != "0")
                        {
                            res.IsSuccess = false;
                            res.Messages = result.Result;
                            return res;
                        }
                        else
                        {
                            var userId = _usersService.GetCurrentUserId();//دریافت شناسه کارجوی جاری

                            //check transaction 
                            var addtransaction = await _transactionService.CheckTransaction(orderCode, transactionBankReferenceId, Common.GlobalEnum.TransactionStateEnum.تایید_شده );
                            if (addtransaction.IsSuccess)
                            {
                                res.IsSuccess = true;
                                res.Messages = "تراکنش با وضعیت تایید شده ثبت گردید";
                                return res;

                            }
                            else
                            {
                                res.IsSuccess = false;
                                res.Messages = addtransaction.Messages;
                            }

                        }
                    }
                }
                else
                {
                    res.IsSuccess = false;
                    res.Messages = gettransaction.Messages;

                }


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";

            }

            return res;
        }













    }
}