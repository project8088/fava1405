using Kavenegar.Core.Models;
using Kavenegar.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nikan.Common
{
    public class SendSms
    {

        //private readonly string apiKey = "66482F6575502F47466D6B516F456136324A77424251554963517A774B434279";
        //private readonly string sender = "30004681088880";



        public string Fa2En(string str)
        {
            return str.Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9")
                //iphone numeric
                .Replace("٠", "0")
                .Replace("١", "1")
                .Replace("٢", "2")
                .Replace("٣", "3")
                .Replace("٤", "4")
                .Replace("٥", "5")
                .Replace("٦", "6")
                .Replace("٧", "7")
                .Replace("٨", "8")
                .Replace("٩", "9");
        }
        public async   Task<ApiResult<List<SendSMSResult>>> SendArray(List<string> to, List<string> msg, string localmessageid , string apiKey,string sender)
        {
            var mres = new ApiResult<List<SendSMSResult>>(true, ApiResultStatusCode.Success, new List<SendSMSResult>(), "پیامک با موفقیت ارسال شد");
             
            try
            { 

                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
                var resualt =await api.SendArray(sender, to, msg, localmessageid); 
                foreach (SendResult res in resualt)
                {
                    mres.Data.Add(GetSendResult(res));
                }
                
            }
            catch (Kavenegar.Core.Exceptions.ApiException er)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                mres.Messages ="خطای وب سرویس پیامک"+ er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;

            }
            catch (Kavenegar.Core.Exceptions.HttpException er)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد

                mres.Messages = "خطای وب سرویس پیامک" + er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;

            }

            return mres;

        }
        public async Task<ApiResult<List<SendSMSResult>>> SendArray(List<string> to, string msg, string localmessageid, string apiKey, string sender)
        {
            var mres = new ApiResult<List<SendSMSResult>>(true, ApiResultStatusCode.Success, new List<SendSMSResult>(), "پیامک با موفقیت ارسال شد");
           try
            {

                var msgList = new List<string>();
                foreach (var item in to)
                {
                    msgList.Add(msg);
                } 

                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
                var resualt =await api.SendArray(sender, to, msgList, localmessageid);
                foreach (SendResult res in resualt)
                {
                    mres.Data.Add(GetSendResult(res));
                }  
            }
            catch (Kavenegar.Core.Exceptions.ApiException er)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                mres.Messages = "خطای وب سرویس پیامک" + er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;


            }
            catch (Kavenegar.Core.Exceptions.HttpException er)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                mres.Messages = "خطای وب سرویس پیامک" + er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;

            }

            return mres;

        }
        public async Task<ApiResult<SendSMSResult>> Send(string to, string msg, string localmessageid, string apiKey, string sender)
        {

            var mres = new ApiResult<SendSMSResult>(true, ApiResultStatusCode.Success, new SendSMSResult(), "عملیات با موفقیت انجام گردید");

            try
            { 
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
                var res = await api.Send(sender, to, msg, localmessageid);
                mres.Data = GetSendResult(res); 
            }
            catch (Kavenegar.Core.Exceptions.ApiException er)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                mres.Messages = "خطای وب سرویس پیامک" + er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;

            }
            catch (Kavenegar.Core.Exceptions.HttpException er)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد

                mres.Messages = "خطای وب سرویس پیامک" + er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;

            }
            return mres;

        }
        public async Task<ApiResult<SendSMSResult>> VerifyLookup(string receptor, string token, TempleteNameEnum template, string apiKey )
        {
            if (!string.IsNullOrWhiteSpace(receptor))
            {
                receptor = Fa2En(receptor);
            }





            var mres = new ApiResult<SendSMSResult>(true, ApiResultStatusCode.Success, new SendSMSResult(), "عملیات با موفقیت انجام گردید");

            try
            { 
                //
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
                
                var res = await api.VerifyLookup(receptor, token, template.ToString(), VerifyLookupType.Sms );
                var smsLog = GetSendResult(res);
                smsLog.TempleteName = template.ToString();
                smsLog.Token1 = token;
                mres.Data=smsLog; 
            }
            catch (Kavenegar.Core.Exceptions.ApiException er)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.

                mres.Messages = "خطای وب سرویس پیامک" + er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;

            }
            catch (Kavenegar.Core.Exceptions.HttpException er)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد

                mres.Messages = "خطای وب سرویس پیامک" + er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;

            }

            return mres;

        }


        public async Task<ApiResult<SendSMSResult>>  VerifyLookup(string receptor, string token, string token2, string token3, TempleteNameEnum template, string apiKey)
        {
            var mres = new ApiResult<SendSMSResult>(true, ApiResultStatusCode.Success, new SendSMSResult(), "عملیات با موفقیت انجام گردید");
            if (!string.IsNullOrWhiteSpace(receptor))
            {
                receptor = Fa2En(receptor);
            }
            try
            {

                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
                var res =await api.VerifyLookup(receptor, token, token2, token3, template.ToString(), VerifyLookupType.Sms);
                var smsLog = GetSendResult(res);
                smsLog.TempleteName = template.ToString();
                smsLog.Token1 = token;
                smsLog.Token2 = token2;
                smsLog.Token3 = token3;
                mres.Data = smsLog;

            }
            catch (Kavenegar.Core.Exceptions.ApiException er)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.

                mres.Messages = "خطای وب سرویس پیامک" + er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;

            }
            catch (Kavenegar.Core.Exceptions.HttpException er)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد

                mres.Messages = "خطای وب سرویس پیامک" + er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;

            }

            return mres;

        }
        public async Task<ApiResult<SendSMSResult>> VerifyLookup(string receptor, string token, string token2, string token3, string token10, TempleteNameEnum template, string apiKey)
        {

            var mres = new ApiResult<SendSMSResult>(true, ApiResultStatusCode.Success, new SendSMSResult(), "عملیات با موفقیت انجام گردید");
            if (!string.IsNullOrWhiteSpace(receptor))
            {
                receptor = Fa2En(receptor);
            }
            try
            {
                token = token.Replace(" ", "");
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
                var res = await  api.VerifyLookup(receptor, token.Trim(), token2, token3, token10, template.ToString(), VerifyLookupType.Sms);
                var smsLog = GetSendResult(res);
                smsLog.TempleteName = template.ToString();
                smsLog.Token1 = token;
                smsLog.Token2 = token2;
                smsLog.Token3 = token3;
                smsLog.Token10 = token10;
                mres.Data = smsLog;

            }
            catch (Kavenegar.Core.Exceptions.ApiException er)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.

                mres.Messages = "خطای وب سرویس پیامک" + er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;
            }
            catch (Kavenegar.Core.Exceptions.HttpException er)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد

                mres.Messages = "خطای وب سرویس پیامک" + er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;
            }
            return mres;
        }
        public async Task<ApiResult<SendSMSResult>> VerifyLookup(string receptor, string token, string token2, string token3, string token10, string token20, TempleteNameEnum template, string apiKey)
        {

            var mres = new ApiResult<SendSMSResult>(true, ApiResultStatusCode.Success, new SendSMSResult(), "عملیات با موفقیت انجام گردید");
            if (!string.IsNullOrWhiteSpace(receptor))
            {
                receptor = Fa2En(receptor);
            }
            try
            {

                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
                var res = await api.VerifyLookup(receptor, token.Trim(), token2, token3, token10, token20, template.ToString(), VerifyLookupType.Sms);
                var smsLog = GetSendResult(res);
                smsLog.TempleteName = template.ToString();
                smsLog.Token1 = token;
                smsLog.Token2 = token2;
                smsLog.Token3 = token3;
                smsLog.Token10 = token10;
                smsLog.Token20 = token20;
                mres.Data = smsLog;

            }
            catch (Kavenegar.Core.Exceptions.ApiException er)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.

                mres.Messages = "خطای وب سرویس پیامک" + er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;
            }
            catch (Kavenegar.Core.Exceptions.HttpException er)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد

                mres.Messages = "خطای وب سرویس پیامک" + er.Message;
                mres.StatusCode = ApiResultStatusCode.ServerError;
                mres.IsSuccess = false;
            }

            return mres;
        }


        public SendSMSResult GetSendResult(SendResult res)
        {
            SendSMSResult smsLog = new SendSMSResult()
            {

                Cost = res.Cost,
                Messageid = res.Messageid,
                Message = res.Message,
                Receptor = res.Receptor,
                Sender = res.Sender,
                Status =(SendSMSReturnCode) res.Status,
                StatusText = res.StatusText,
                GregorianDate=res.GregorianDate,
              


            };
            return smsLog;
        }


        public async Task<string> RemainCredit(string apiKey)
        {
            var mres = "";
            
           
            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey); 
                var info = await api.AccountInfo(); 
                if (info != null)
                { 
                    mres= info.RemainCredit.ToString(); 
                }

            }
            catch (Kavenegar.Core.Exceptions.ApiException er)
            {

                mres = "-1";
            }
            catch (Kavenegar.Core.Exceptions.HttpException er)
            {
                mres = "-1";
            }
            catch (Exception er)
            {

                mres = "-10";

            }
            return mres;
        }







    }


    public class SendSMSResult
    {
        public SendSMSReturnCode Status { get; set; }
        public string Description { get; set; }
        public string Message { get; set; } 
        public string Token1 { get; set; } 
        public string Token2 { get; set; } 
        public string Token3 { get; set; } 
        public string Token10 { get; set; } 
        public string Token20 { get; set; } 
        public int Cost { get; set; }

        public long Messageid { get; set; }
       
        public DateTime GregorianDate { get; set; }
        public long Date { get; set; }
     
        public string Receptor { get; set; }
        public string Sender { get; set; }
       
        public string StatusText { get; set; } 

        public string TempleteName { get; set; }
        public List<string> MessageIdes { get; set; }
    }

    public class SMSCredit
    {
        public SendSMSReturnCode Status { get; set; }
        public string Description { get; set; }
        public string  Credit { get; set; }
    }

    public class SmsOption
    {
        public SmsOption()
        {

        }



        public string SmsUserName { get; set; }
        public string SmsPassword { get; set; }
        public string DomainName { get; set; }
        public string SenderNumber { get; set; }
        public string SmsToken { get; set; }

        /// <summary>
        /// تعداد  پیامک مجاز برای ارسال به یک شماره موبایل
        /// </summary>
        public int CountValidMobileNumber { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public bool SendSmsAfterRejectCitizenInformationInUpdateForm { get; set; }

        public bool OnlineAuthenticationAfterUpdateCitizenInfo { get; set; }


        public bool OnlineAuthentication { get; set; }


        public bool? SendSmsAfterAdminLogin { get; set; }


    }


    public enum SendSMSReturnCode
    {
        عملیات_با_موفقیت_صورت_گرفت = 1,
        پارامترهای_ورودی_صحیح_نمی_باشند = -1,
        اطلاعات_کاربری_صحیح_نمی_باشد = -2,
        شماره_فرستنده_صحیح_نمی_باشد = -3,
        حساب_کاربری_شما_غیرفعال_می_باشد = -4,
        حساب_کاربری_شما_منقضی_شده_است = -5,
        دﻓﻌﺎت_ﺑﺮداﺷﺖ_وﺟﻪ_ﺑﻴﺶ_از_ﺣﺪ_ﻣﺠﺎز_اﺳﺖ = -6,
        عدم_موجودی = -7,
        قیمت_پیامک_تعریف_نشده = -8,
        پنل_مدیریت_غیرفعال = -9,
        پنل_مدیریت_منقضی = -10,
        قیمت_پیامک_برای_مدیریت_مشخص_نشده = -11,
        سرویس_غیرفعال = -14,
        تعداد_حروف_غیر_مجاز_است =-15,
        خطا = -100,

    }
     
    
    public enum TempleteNameEnum
    {
        ChangeMobile,
        ForgotPassword,
        ManzelatHasCardAndConvertCard,
        ManzelatInProgress,
        ManzelatNoCard,
        ManzelatReview,
        MobileVerify, 
        ReviewPicture,
        RejectSabtAhval,
        smsinfoupdate,
        ResponseTicket,
        getcard,
        getcardmanzalatCenter,
        getcardmarkaziCenter,
        getcardboniadCenter,
        getByAddress,
        ReviewPictureAllCitizen,
        PersonalFormUpdate,
        editforcard,
        ConfirmCardNumber,
        adminlogin,


    }

}
