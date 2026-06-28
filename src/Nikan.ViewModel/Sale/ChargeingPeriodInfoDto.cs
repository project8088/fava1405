using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Sale
{
    /// <summary>
    /// تعریف دوره
    /// </summary>
    public class ChargeingPeriodInfoDto
    {



        /// <summary>
        /// شناسه دوره باید دریافت شود
        /// </summary>
      
        public int Id { get; set; }



        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        /// <summary>
        /// مهلت پرداخت
        /// </summary>
        public DateTime PaymentDeadLine { get; set; }




        /// <summary>
        /// طریقه محاسبه هزینه شارژ
        /// </summary>
        public CalcChargeTypeEnum CalcChargeType { get; set; }


        /// <summary>
        /// مبلغ شارژ روزانه
        /// </summary>
        public int DailyChargeAmount { get; set; }





        /// <summary>
        /// درصد مالیات
        /// </summary>
        public decimal Tax { get; set; }




        /// <summary>
        /// ضریب تجاری
        /// ضرب بر مقدار
        /// </summary>
        public decimal CommercialRatio { get; set; }

        /// <summary>
        /// جریمه برای بدهی دوره قبل محاسبه شود ؟
        /// </summary>
        public bool PenaltyForPeriodDebt { get; set; }

        /// <summary>
        /// درصد جریمه
        /// </summary>
        public decimal PenaltyPercentage { get; set; }

        /// <summary>
        /// مالیات برای جریمه ؟
        /// </summary>
        public bool TaxForPercentage { get; set; }

         

    }


    public class ChargeingPeriodDto
    {

        /// <summary>
        /// شناسه دوره باید دریافت شود
        /// </summary>

        public int Id { get; set; }



        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        /// <summary>
        /// مهلت پرداخت
        /// </summary>
        public DateTime PaymentDeadLine { get; set; }


        public int Days { get; set; }

        /// <summary>
        /// طریقه محاسبه هزینه شارژ
        /// </summary>
        public CalcChargeTypeEnum CalcChargeType { get; set; }


        /// <summary>
        /// مبلغ شارژ روزانه
        /// </summary>
        public int DailyChargeAmount { get; set; }





        /// <summary>
        /// درصد مالیات
        /// </summary>
        public decimal Tax { get; set; }




        /// <summary>
        /// ضریب تجاری
        /// ضرب بر مقدار
        /// </summary>
        public decimal CommercialRatio { get; set; }

        /// <summary>
        /// جریمه برای بدهی دوره قبل محاسبه شود ؟
        /// </summary>
        public bool PenaltyForPeriodDebt { get; set; }

        /// <summary>
        /// درصد جریمه
        /// </summary>
        public decimal PenaltyPercentage { get; set; }

        /// <summary>
        /// مالیات برای جریمه ؟
        /// </summary>
        public bool TaxForPercentage { get; set; }



        public int? CreatedByUserId { get; set; }

        public string  CreatedByUser { get; set; }

        public int? LastUpdateByUserId { get; set; }

        public string  LastUpdateByUser { get; set; }


        public DateTime? LastUpdateOnDate { get; set; }

        /// <summary>
        ///  آیا قبض مورد تایید است ؟
        /// </summary>
        public bool? IsConfirmed { get; set; }
        public int? ConfirmedByUserId { get; set; }
        public string  ConfirmedByUser { get; set; }
        public DateTime? ConfirmedOnDate { get; set; }

        /// <summary>
        /// آیا برای این دوره قبض ایجاد شده است ؟
        /// </summary>
        public bool? BillCreated { get; set; }

        /// <summary>
        /// توسط چه کاربری قبض تولید شده است ؟
        /// </summary>
        public int? BillCreatedByUserId { get; set; }
        public string  BillCreatedByUser { get; set; }


        public DateTime? BillCreatedOnDate { get; set; }
       public DateTime CreatedOn { get; set; }

        public string UserTransactionId { get; set; }
    }





    public class CompanyChargingBillsInfoDto
    {
        public int Id { get; set; }



        /// <summary>
        /// شناسه دوره باید دریافت شود
        /// </summary> 
        public int PeriodId { get; set; }

        /// <summary>
        /// شرکت
        /// </summary>
        public int UserCompanyId { get; set; }
        public string  UserCompany { get; set; }
         

        /// <summary>
        /// مبلغ شارژ
        /// </summary>
        public long ChargeAmount { get; set; }

        /// <summary>
        /// کد قرارداد
        /// </summary>
        public string ContractCode { get; set; } 
        /// <summary>
        /// شماره پرونده
        /// </summary>
        public string FileCode { get; set; }

        public decimal UnitArea { get; set; }

        public string UserTransactionId { get; set; }
        
        public TransactionStateEnum? TransactionState { get; set; }

    }


   


    public class FullBillInfoDto
    {
        public int BillId { get; set; }
        /// <summary>
        /// شرکت
        /// </summary>
        public int UserCompanyId { get; set; }
        public string UserCompany { get; set; }


     
        public long ChargeAmount { get; set; }


        public string UserTransactionId { get; set; }

        public TransactionStateEnum? TransactionState { get; set; }


        /// <summary>
        /// شناسه دوره باید دریافت شود
        /// </summary> 
        public int PeriodId { get; set; } 

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        /// <summary>
        /// مهلت پرداخت
        /// </summary>
        public DateTime PaymentDeadLine { get; set; }


        public int Days { get; set; }

        /// <summary>
        /// طریقه محاسبه هزینه شارژ
        /// </summary>
        public CalcChargeTypeEnum CalcChargeType { get; set; }


        /// <summary>
        /// مبلغ شارژ روزانه
        /// </summary>
        public int DailyChargeAmount { get; set; }
         
        /// <summary>
        /// درصد مالیات
        /// </summary>
        public decimal Tax { get; set; }
         
        /// <summary>
        /// ضریب تجاری
        /// ضرب بر مقدار
        /// </summary>
        public decimal CommercialRatio { get; set; }

        /// <summary>
        /// جریمه برای بدهی دوره قبل محاسبه شود ؟
        /// </summary>
        public bool PenaltyForPeriodDebt { get; set; }

        /// <summary>
        /// درصد جریمه
        /// </summary>
        public decimal PenaltyPercentage { get; set; }

        /// <summary>
        /// مالیات برای جریمه ؟
        /// </summary>
        public bool TaxForPercentage { get; set; }



        public int CreatedByUserId { get; set; }

        public string CreatedByUser { get; set; }

        public int? LastUpdateByUserId { get; set; } 
        public string LastUpdateByUser { get; set; }


        public DateTime? LastUpdateOnDate { get; set; }

        /// <summary>
        ///  آیا قبض مورد تایید است ؟
        /// </summary>
        public bool? IsConfirmed { get; set; }
        public int? ConfirmedByUserId { get; set; }
        public string ConfirmedByUser { get; set; }
        public DateTime? ConfirmedOnDate { get; set; }

        /// <summary>
        /// آیا برای این دوره قبض ایجاد شده است ؟
        /// </summary>
        public bool BillCreated { get; set; }

        /// <summary>
        /// توسط چه کاربری قبض تولید شده است ؟
        /// </summary>
        public int? BillCreatedByUserId { get; set; }
        public string BillCreatedByUser { get; set; }

        /// <summary>
        /// تاریخ ایجاد قبضخ>>
        /// </summary>
        public DateTime? BillCreatedOnDate { get; set; }

        /// <summary>
        /// تاریخ ایجاد دوره
        /// </summary>
        public DateTime CreatedOn { get; set; }
         
        /// <summary>
        /// کد قرارداد
        /// </summary>
        public string ContractCode { get; set; }
        /// <summary>
        /// شماره پرونده
        /// </summary>
        public string FileCode { get; set; }

        public decimal UnitArea { get; set; }


    }

    public class PagedCompaniesBillsDto
    {

        public List<FullBillInfoDto> FullBillInfo { get; set; }

        public int TotalItems { get; set; }


    }


    public class PagedBillsListDto
    {

        /// <summary>
        /// اطلاعات دوره
        /// </summary>
        public ChargeingPeriodDto ChargeingPeriodInfo { get; set; }
        /// <summary>
        /// اطلاعات قبض های صادر شده برای دوره
        /// </summary>
        public List<CompanyChargingBillsInfoDto> Bills  { get; set; }
        /// <summary>
        /// تعداد کل
        /// </summary>
        public int TotalItems { get; set; }


    }


    public class PagedChargeingPeriodListDto
    {

        /// <summary>
        /// لیست دوره ها
        /// </summary>
        public List<ChargeingPeriodDto> ChargeingPeriodInfo { get; set; }
        /// <summary>
        /// تعداد کل
        /// </summary>
        public int TotalItems { get; set; }


    }


}
