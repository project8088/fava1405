using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cle.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nikan.MellatApp;
using Nikan.Services;

namespace Nikan.WebApp.Controllers
{
    public class PayController : Controller
    {

        #region Ctor

        private readonly IUsersService _usersService; 
        private readonly ISiteSettingService _siteSettingService;
        private readonly ITransactionService _transactionService;


        public PayController( 
            ISiteSettingService siteSettingService,
            ITransactionService transactionService,
              IUsersService userManager

            )
        {
            _transactionService = transactionService;
            _siteSettingService = siteSettingService; 
            _usersService = userManager ?? throw new ArgumentNullException(nameof(_usersService));

        }
        #endregion




        [AllowAnonymous]
        [IgnoreAntiforgeryToken] 
        public async Task<IActionResult> Index()
        {
            var bankOption = await _siteSettingService.GetFinancialSettings();
            var bankMellatImplement = new BankMellatImplement(bankOption.Data.BankTerminalId.Value,
                bankOption.Data.BankUserName, bankOption.Data.BankPassword, bankOption.Data.BankCustomerId.Value, bankOption.Data.BankPaymentMethod.Value);
            long transactionid = 0;
            var user = "citizen";
            long saleReferenceId = -1;
            long saleOrderId = -1;

            try
            {
                if (!string.IsNullOrWhiteSpace(Request.Form["ResCode"]))
                {
                    if (!string.IsNullOrWhiteSpace(Request.Form["SaleOrderId"]))
                    {
                        long.TryParse(Request.Form["SaleOrderId"].ToString(), out saleOrderId);
                    }

                    

                    var ResCode = Request.Form["ResCode"].ToString();
                    if (ResCode == "0")
                    {
                        //پرداخت موفق
                       
                        long.TryParse(Request.Form["SaleReferenceId"].ToString(), out saleReferenceId);
                        var resVerify = bankMellatImplement.bpVerifyRequest(saleOrderId, saleOrderId, saleReferenceId);
                        if (resVerify.ResultCode == "0")
                        {
                            var resSettleData = bankMellatImplement.bpSettleData(saleOrderId, saleReferenceId);
                            if (resSettleData.ResultCode == "0")
                            {
                                var ress = await _transactionService.ResultTransaction(saleOrderId.ToString(), saleReferenceId.ToString(), Common.GlobalEnum.TransactionStateEnum.تایید_شده, resSettleData.Result);
                                transactionid = ress.Data.Id;
                                if (ress.Data.TransactionFor ==  Common.GlobalEnum.TransactionForEnum.تست_درگاه)
                                    user = "admin";
                            }
                            else
                            {
                                var ress = await _transactionService.ResultTransaction(saleOrderId.ToString(), saleReferenceId.ToString(), Common.GlobalEnum.TransactionStateEnum.پرداخت_نشده, resSettleData.Result);
                                transactionid = ress.Data.Id;
                                if (ress.Data.TransactionFor == Common.GlobalEnum.TransactionForEnum.تست_درگاه)
                                    user = "admin";
                            }
                        }
                        else
                        {
                            var ress = await _transactionService.ResultTransaction(saleOrderId.ToString(), saleReferenceId.ToString(), Common.GlobalEnum.TransactionStateEnum.پرداخت_نشده, resVerify.Result);
                            transactionid = ress.Data.Id;
                            if (ress.Data.TransactionFor == Common.GlobalEnum.TransactionForEnum.تست_درگاه)
                                user = "admin";
                        }

                    }
                    else if(saleOrderId!=-1)
                    {
                       var message=  bankMellatImplement.DesribtionStatusCode(int.Parse(ResCode)).Replace("_", " ") ;
                        var ress = await _transactionService.ResultTransaction(saleOrderId.ToString(), saleReferenceId.ToString(), Common.GlobalEnum.TransactionStateEnum.عدم_تایید, message);
                        transactionid = ress.Data.Id;
                        if (ress.Data.TransactionFor == Common.GlobalEnum.TransactionForEnum.تست_درگاه)
                            user = "admin";
                    }
                }
            }
            catch (Exception er)
            {

                ViewBag.ResCode = "خطا" + er.Message;
            }

            if (transactionid == 0)
            {
                Response.Redirect("/" + user + "/transaction-list");

            }
            else
            {
                Response.Redirect("/" + user + "/transaction-details/" + transactionid);
            }


            return View();
        }




    }
}