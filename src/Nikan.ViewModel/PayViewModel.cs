using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel
{
    public class PayViewModel
    {
        

        public long OrderId { get; set; }  
        public long Amount { get; set; }

       
         
    }


    public class MellatModelDto
    {

        public string token { get; set; }

        public bool Isfree { get; set; }
        public string PgwSite { get; set; }
        public string RefId { get; set; }
        public long TransactionCode { get; set; }
        public long AmountTransaction { get; set; }

    }


    public class ResultMellatModelDto
    {

        public string RefId { get; set; }
        public string ResCode { get; set; }
        public long saleOrderId { get; set; }
        public long SaleReferenceId { get; set; }

    }

    /// <summary>
    /// برگشت بانک ملی
    /// </summary>
    public class CallbackRequestPayment
    {
        public string PrimaryAccNo { get; set; }
        public string HashedCardNo { get; set; }
        public long OrderId { get; set; }
        public string SwitchResCode { get; set; }
        public string ResCode { get; set; }
        public string Token { get; set; }
    }
    public class PurchaseResult
    {
        public string OrderId { get; set; }
        public string Token { get; set; }
        public string ResCode { get; set; }
        public VerifyResultData2 VerifyResultData { get; set; }
    }
    public class VerifyResultData
    {
        public int ResCode { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string RetrivalRefNo { get; set; }
        public string SystemTraceNo { get; set; }
        public string OrderId { get; set; }
    }
    public class VerifyResultData2
    {
        public bool Succeed { get; set; }
        public string ResCode { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string RetrivalRefNo { get; set; }
        public string SystemTraceNo { get; set; }
        public string OrderId { get; set; }
    }


}
