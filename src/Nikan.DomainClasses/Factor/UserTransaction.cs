using Nikan.Common.GlobalEnum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nikan.DomainClasses.Factor
{
    /// <summary>
    /// تراکنش هالی مالی 
    /// </summary>
    public class UserTransaction
    {
         
        [Key]
        public long TransactionId { get; set; }

        [StringLength(64)]
        public string TerminalId { get; set; }

        public PaymentTypeEnum PaymentType { get; set; }

         
        [StringLength(64)]
        public string OrderId { get; set; }


        [StringLength(64)]
        public string TransactionBankReferenceId { get; set; }

        public long AmountTransaction { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }


        [StringLength(1000)]
        public string PaymentDescription { get; set; }
        public virtual DateTime TransactionOnDate { get; set; }


        /// <summary>
        /// وضعیت تراکنش
        /// </summary>
        public virtual TransactionStateEnum TransactionState { get; set; }
      

        /// <summary>
        /// توسط چه بانکی صورت گرفته است این تراکنش
        /// </summary>
        public virtual TransactionBankEnum TransactionBank { get; set; }
      




        public virtual TransactionForEnum TransactionFor  { get; set; }
      
        
        
        public virtual DateTime? AcceptationTransactionOnDate { get; set; }

        [StringLength(1000)]
        public string FileUrl { get; set; }

        [StringLength(1000)]
        public string OperationDescription { get; set; }


        public virtual User TransactionBy { get; set; }
        public virtual int? TransactionById { get; set; }




        public virtual User ReviewBy { get; set; }
        public virtual int? ReviewById { get; set; }

        #region اطلاعات حساب بانکی
        [StringLength(50)]
        public string BankName { get; set; }

        [StringLength(16)]
        public string BankAccountNumber { get; set; }
        public string BranchName { get; set; }
        #endregion

      



        public bool IsDeleted { get; set; }
      
      
       
    }



}
