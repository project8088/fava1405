using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Transactions
{

    public class TransactionDto
    {
        public long  Id { get; set; } 
        public string OrderId { get; set; }  
        public long AmountTransaction { get; set; } 
        public string Description { get; set; }  
        public virtual TransactionForEnum TransactionFor { get; set; } 
        public virtual int? TransactionById { get; set; }


      

    }

    public class TransactionInfo
    {
        /// <summary>
        /// ایدی تراکنش
        /// </summary>
        public long Id { get; set; }


        /// <summary>
        ///شماره پیگیری
        /// </summary>
        public string OrderId { get; set; }


        /// <summary>
        /// شناسه بانکی
       /// </summary>
        public string TransactionBankReferenceId { get; set; }

        /// <summary>
        /// مبلغ
        /// </summary>
        public long AmountTransaction { get; set; }

         


        /// <summary>
        /// تاریخ تراکنش
        /// </summary>
        public virtual DateTime TransactionOnDate { get; set; }


        /// <summary>
        /// وضعیت تراکنش
        /// </summary>
        public virtual TransactionStateEnum TransactionState { get; set; }


        /// <summary>
        /// عنوان تراکنش
        /// </summary>
        public virtual TransactionForEnum TransactionFor { get; set; }


        /// <summary>
        /// تاریخ تایید تراکنش
        /// </summary>
        public virtual DateTime? AcceptationTransactionOnDate { get; set; }


        /// <summary>
        /// انجام دهنده تراکنش
        /// </summary>
        public virtual string  TransactionBy { get; set; }
        public virtual int? TransactionById { get; set; }

        public virtual string TransactionByName { get; set; }
        public string PaymentDescription { get; set; }

    }


    public class PagedTransactionsViewModel
    {
         
        public List<TransactionInfo> Transactions { get; set; } 
        public int TotalItems { get; set; }


    }





}
