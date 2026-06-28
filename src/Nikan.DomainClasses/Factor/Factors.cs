using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.UserCompanes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.Factor
{

    public class FactorMaster 
    {

        /// <summary>
        /// شناسه فاکتور
        /// </summary>
        [Key]
        public int FactorId { get; set; }

        #region Seller
        [StringLength(150)]
        public string SellerName { get; set; } 

        /// <summary>
        /// شماره اقتصادی
        /// </summary>
        [StringLength(50)]
        public string SellerEconomicalNO { get; set; }


        /// <summary>
        /// شماره ثبت مخصوص افراد حقوقی
        /// </summary>
        [StringLength(50)]
        public string SellerRegNO { get; set; }
         
        [StringLength(500)]
        public string SellerAddress { get; set; }


        [StringLength(20)]
        public string SellerZipCode { get; set; }


        [StringLength(50)]
        public string SellerCity { get; set; }


        [StringLength(50)]
        public string SellerPhoneNumber { get; set; }

        [StringLength(50)]
        public string SellerFaxNumber { get; set; }


        /// <summary>
        /// شماره شبای فروشنده
        /// </summary>
        public string SellerSheba { get; set; }
      
        
        /// <summary>
        /// شماره کارت فروشنده
        /// </summary>
        [StringLength(16)]
        public string SellerCardNo { get; set; }
        
        /// <summary>
        /// شماره حساب فروشنده
        /// </summary>
        [StringLength(16)]
        public string SellerBankAccountNumber { get; set; }






        #endregion
        #region Buyer
        [StringLength(150)]
        public string BuyerName { get; set; }
         
        /// <summary>
        /// شماره اقتصادی
        /// </summary>
        [StringLength(50)]
        public string BuyerEconomicalNO { get; set; }


        /// <summary>
        /// شماره ثبت مخصوص افراد حقوقی
        /// </summary>
        [StringLength(50)]
        public string BuyerRegNO { get; set; }



        [StringLength(500)]
        public string BuyerAddress { get; set; }


        [StringLength(20)]
        public string BuyerZipCode { get; set; }


        [StringLength(50)]
        public string BuyerCity { get; set; }


        [StringLength(50)]
        public string BuyerPhoneNumber { get; set; }

        [StringLength(50)]
        public string BuyerFaxNumber { get; set; }

        #endregion 
        
        
        
        /// <summary>
        /// تاریخ ایجاد فاکتور
        /// </summary>
        public DateTime CrateOnDate { get; set; }

        /// <summary>
        /// شماره فاکتور
        /// </summary>
        [StringLength(100)]
        public string FactorNumber { get; set; }


        /// <summary>
        ///  تاریخ فاکتور
        /// </summary>
        public DateTime? FactorDate { get; set; }

       /// <summary>
       /// بارکد
       /// </summary>
        public string BarCode { get; set; }
       
        
         
    

        public virtual UserTransaction TransactionBy { get; set; }
        public long? TransactionById { get; set; }

        public virtual User CreatedBy { get; set; }
        public virtual int?  CreatedById { get; set; }

        //ایا فاکتور تایید شده
        public bool? IsConfirm { get; set; }

        public virtual ICollection<FactorDetail> FactorDetail { get; set; }

    }

    public class FactorDetail
    {
        public int Id { get; set; }
      
        
        [ForeignKey(nameof(FactorMasterId))]
        public virtual FactorMaster FactorMaster { get; set; }
        public int FactorMasterId { get; set; } 
      
        
        public string Description { get; set; } 
       
        public string Code { get; set; }
        
        public long UnitPrice { get; set; } 
        public int Count { get; set; } = 1; 
        public int Discountamount { get; set; } 
        public int VAT { get; set; } 
      
        
        /// <summary>
        /// حمع کل ردیف فاکتور در این ردیف قرار میگیرد 
        /// </summary>
        public long Total { get; set; } 


    }

    /// <summary>
    /// بدهکار طرف اول 
    /// بستانکار طرف دوم
    /// </summary>
    public class UserCredit
    {
        /// <summary>
        /// شناسه سند
        /// </summary>
        [Key]
        public virtual int Id { get; set; }



        /// <summary>
        /// این سند مالی مربوط به چه شرکتی است؟
        /// </summary>
        public int? UserCompanyId { get; set; }
        public virtual UserCompany UserCompany { get; set; }
       
        /// <summary>
        /// شماره اقتصادی طرف اول
        /// </summary>
        public string TxtTinNo { get; set; }





        /// <summary>
        /// این سند توسط چه فاکتوری پرداخت می شود؟
        /// </summary>
        [ForeignKey(nameof(FactorMasterId))]
        public virtual FactorMaster FactorMaster { get; set; }
        public int? FactorMasterId { get; set; }


        public decimal? AddedValueTaxPercent { get; set; }
        public int? AddedValueTaxMoneyAmount { get; set; }

        public int? MoneyUnitType { get; set; }

        public int TotalUnits { get; set; }


        /// <summary>
        /// این سند مالی مربوط به چه بهره برداری است؟
        /// </summary>
        public string BuyerOrganizationId { get; set; }
        public virtual Organization BuyerOrganization { get; set; }


        /// <summary>
        /// این سند مالی مربوط به چه بهره برداری است؟
        /// </summary>
        public string SellerOrganizationId { get; set; }
        public virtual Organization SellerOrganization { get; set; }

   

        #region Title
        /// <summary>
        /// عنوان سند
        /// </summary>
        [StringLength(200)]
        public virtual string Title { get; set; }
        #endregion



        #region Description
        /// <summary>
        /// شرح سند
        /// </summary>
        [StringLength(2000)]
        public virtual string Description { get; set; }
        #endregion 


         

        /// <summary>
        /// بدهکار=-1
        /// بستانکار=1
        /// </summary>
        public virtual TransactionTypeEnum TransactionType { get; set; }


        /// <summary>
        ///  بایت
        /// </summary>
        public virtual TransactionForEnum TransactionFor { get; set; }

       


        /// <summary>
        /// کل مبلغ تراکنش 
        /// </summary>
        public virtual long AmountTransaction { get; set; }


        /// <summary>
        /// بدون مالیات بر ارزش افزوده مبلغ تراکنش
        /// </summary>
        public long AmountTransactionWithoutTax { get; set; }

        /// <summary>
        /// تاریخ تراکنش
        /// </summary>
        public virtual DateTime TransactionOnDate { get; set; }

     

        /// <summary>
        /// وضعیت سند
        /// </summary>
        public virtual TransactionStateEnum TransactionState { get; set; }


       


        
        /// <summary>
        /// این سند توسط چه کاربری ثبت شده است
        /// </summary>
        public virtual User TransactionBy { get; set; }

        public virtual int?  TransactionById { get; set; }

     
  


        public virtual bool? IsDeleted { get; set; }



        

    }



}
