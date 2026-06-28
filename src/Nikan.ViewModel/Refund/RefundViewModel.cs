using Nikan.Common.GlobalEnum;
using Nikan.ViewModel.Group;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Refund
{
    public class RefundDto
    {


        public int Id { get; set; }
        public string Description { get; set; }
        public string UnitName { get; set; }
        public string ClassName { get; set; }
        public int ImportId { get; set; }
        //public string NationalCode { get; set; } 
        public string LetterNumber { get; set; }
        public bool? CitizenAccess { get; set; }

        public int? CitizenId { get; set; }


    }


    public class ChangeRefundDto
    {


        public int Id { get; set; }
        public string Description { get; set; }
        public string UnitName { get; set; }
        public string ClassName { get; set; } 
        public string NationalCode { get; set; }
        public string LetterNumber { get; set; }
        public bool CitizenAccess { get; set; }

        public bool IsClosed { get; set; }

    }


    public class RefundInfo
    {




        public int Id { get; set; }
        public string Description { get; set; }
        public string UnitName { get; set; }

        public DateTime? OnDate { get; set; }
        public string ClassName { get; set; }
        public string AccessByCitizen { get; set; }
        public string NationalCode { get; set; }

        public int Count { get; set; }

        public int CountRefund { get; set; }



        public int? AccessByCitizenId { get; set; }
        public Guid? AccessByUserCode { get; set; }

        public string LetterNumber { get; set; }
        public bool CitizenAccess { get; set; }
        public bool IsClosed { get; set; }


    }



    public class ImportRefundList
    {


        public int RefundId { get; set; }
        public string OrderId { get; set; }
        public string TransactionCode { get; set; }
        public long RefundAmount { get; set; }
        public long TotalRefundAmount { get; set; }
        public string OwnerNationCode { get; set; }

         

        public string OwnerName { get; set; }

        public string OwnerMobileNumber { get; set; }


      

        public string RefundCardNumber { get; set; }

        public string TransactionXmlInfo { get; set; }



        public string AdminDescription { get; set; }

        public string OwnerBankCardNumber { get; set; }



        public string DeclarationCardNumber { get; set; }



        public DateTime? RefundOnDate { get; set; }
        public int? RefundByUserId { get; set; }
        public Guid? RefundByUserCode { get; set; }




        public string  RefundByUser  { get; set; }


        public string RefundRefCode { get; set; }

        public string Description { get; set; }
        public string OtherDescription { get; set; }
        public string LetterNumber { get; set; }

        


        /// <summary>
        /// وضعیت استرداد
        /// </summary>
        public RefundStateEnum? RefundState { get; set; }


        public int TransactionRefundImportId { get; set; }
        public bool? IsClosed { get; set; }
        public int? OwnerCitizenId { get; set; }

        public Guid? OwnerUserCode { get; set; }

    }

    public class PagedRefundViewModel
    { 
        public List<RefundInfo> Items { get; set; }
        public int TotalItems { get; set; } 

    }

    public class PagedRefundImportCitizenListViewModel
    {
        public List<ImportRefundList> Items { get; set; }
        public int TotalItems { get; set; }

    }

    /// <summary>
    /// جزئیات فایل ورودی استرداد
    /// </summary>
    public class RefundExcelFileColumns
    {

        public string OrderId { get; set; }
        public string SaleReferenceId { get; set; } 
        public string Description { get; set; }
        public string NationalCode { get; set; }
        public string OtherDescription { get; set; }
        public long TotalRefundAmount { get; set; }
        public long RefundAmount { get; set; }


    }


    public class RefundExcelFileDetails
    {

        public int Id { get; set; }
        public string OrderId { get; set; }
        public string SaleReferenceId { get; set; }
        public string Description { get; set; }
        public string NationalCode { get; set; }
        public int CitizenId { get; set; }

        public Guid? UserCode { get; set; }

        public string  Citizen  { get; set; }
        public string RefundCardNumber { get; set; }
        public string OtherDescription { get; set; }
        public long TotalRefundAmount { get; set; }
        public long RefundAmount { get; set; }


    }
    public class RefundImportFileInfo
    {
        /// <summary>
        /// شناسه فایل
        /// </summary>
        public int ImportId { get; set; }

        /// <summary>
        /// نام فایل
        /// </summary>
        public string FileName { get; set; }
         
        /// <summary>
        /// توسطه چه کسی بارگذاری شده است
        /// </summary>
        public string ImportBy { get; set; } 
        /// <summary>
        /// در چه تاریخ بارگذاری شده است
        /// </summary>
        public DateTime OnDate { get; set; } 

        /// <summary>
        /// آیا تایید شده است
        /// </summary>
        public bool? IsConfirm { get; set; }
        /// <summary>
        /// جزئیات فایل
        /// </summary>
        public List<RefundExcelFileDetails> RefundList { get; set; }
      


    }

    /// <summary>
    /// ویرایش استرداد هزینه
    /// </summary>
    public class ChangeTransactionRefund
    {
       

        public int RefundId { get; set; }
        public string OrderId { get; set; } 
        public string TransactionCode { get; set; }


        public long TotalRefundAmount { get; set; }
        public long RefundAmount { get; set; }


        public string RefundCardNumber { get; set; }

        

        public string Description { get; set; }
        public string OtherDescription { get; set; }


        public string AdminDescription { get; set; }


        public bool IsClosed { get; set; }

        

    }


    public class AddTransactionRefund
    {

        public int ImportId { get; set; }
       
        public string OrderId { get; set; }
        public string TransactionCode { get; set; }


        public long TotalRefundAmount { get; set; }
        public long RefundAmount { get; set; } 

        public string Description { get; set; }
        public string OtherDescription { get; set; }

        public string NationCode { get; set; }
        public string AdminDescription { get; set; }


        public bool IsClosed { get; set; }



    }

    public class RefundUserRegister
    {
        public string NationCode { get; set; }
    }


    public class UpdateRefundCardNumber
    {


        public int RefundId { get; set; }
        public string OwnerBankCardNumber { get; set; }

        public string AdminDescription { get; set; }



    }


    public class ReportRefund
    {


        /// <summary>
        /// تعداد
        /// </summary>
        public int CountRow { get; set; }
      
        /// <summary>
        /// جمع کل
        /// </summary>
        public long TotalAmount { get; set; }
      
        
       

         


      

        /// <summary>
        /// تعداد برگشت داده شده
        /// </summary>
        public int CountRefund { get; set; }


        /// <summary>
        /// برگشت داده شده
        /// </summary>
        public long AmountRefund { get; set; }



        /// <summary>
        /// مبلغ برگشت داده نشده
        /// </summary>
        public long AmountRemaining { get; set; }

        /// <summary>
        /// تعداد برگشت داده نشده
        /// </summary>
        public int CountRemaining { get; set; }

       

        public long AmountRefundCardNumber { get; set; }

        public int CountRefundCardNumber { get; set; }

          

    }




    public class PagedRefundAccessViewModel
    {
        public List<GroupCitizensInfo> Items { get; set; } 
        public int TotalItems { get; set; }
    }












}
