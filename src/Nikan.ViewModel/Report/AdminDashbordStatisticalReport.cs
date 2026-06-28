using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Report
{
    public class AdminDashbordStatisticalReport
    { 


        /// <summary>
        /// تعداد کل تیکت ها
        /// </summary>
        public int AllTicketCount { get; set; }


        /// <summary>
        /// تیکت های بسته شده
        /// </summary>
        public int ClosedTicketCount { get; set; }


        /// <summary>
        /// تعداد تیکت های جدید
        /// </summary>
        public int NewTicketCount { get; set; }



        /// <summary>
        /// تعداد کل شهروندان
        /// </summary>
        public int AllCitizenCount { get; set; }

        /// <summary>
        ///  تعداد شهروندانی که ثبت نام کرده اند امروز
        /// </summary>
        public int CitizenTodayCount { get; set; }


        /// <summary>
        ///  تعدادی اتباع خارجی
        /// </summary>
        public int ForeignNationalsCount { get; set; }


        /// <summary>
        /// تعداد شهروندانی که تایید هویت شده اند
        /// </summary>
        public int AllAcceptCitizenCount { get; set; }

        /// <summary>
        /// تعدادی تصاویری که مورد تایید قرارگرفته اند
        /// </summary>
        public int AllAcceptCitizenPictureCount { get; set; }

        public int  DeathCitizen  { get; set; }

        /// <summary>
        /// تعداد گروههای شهروندی
        /// </summary>
        public int CitizenGroupCount { get; set; }



        public int ActiveCompany { get; set; }
        public int PersonelCount { get; set; } 
        public int StagnantCompany { get; set; } 
        public int BuildingCompany { get; set; }
         


        /// <summary>
        /// میزان اعتبار پیامک 
        /// </summary>
        public string SmsCredit { get; set; }

    }


    public class CardDashbordStatisticalReport
    {


        /// <summary>
        /// تعداد کل تیکت ها
        /// </summary>
        public int AllCardCount { get; set; }


        /// <summary>
        /// کارتها با وضعیت درخواست جدید
        /// </summary>
        public int CardNewRequest { get; set; }


        /// <summary>
        ///  تصاویر در حال بررسی
        /// </summary>
        public int PersonelPictureCount { get; set; }



        /// <summary>
        /// تعداد کل شهروندان
        /// </summary>
        public int DeliveredCards { get; set; }

        /// <summary>
        ///  تعداد شهروندانی که درخواست کارت داده اند امروز
        /// </summary> 
        public int CardNewRequestTodayCount { get; set; }

        /// <summary>
        ///  کارت های منزلت
        /// </summary>
        public int ManzalatCardCount { get; set; }


        /// <summary>
        /// کارت های شهروندی
        /// </summary>
        public int ShahrvaniCardCount { get; set; }

        
       public string TotalSeconds { get; set; }

          

    }


    public class BetweenDate
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }


    public class HiChartData
    {

        public int[] Data { get; set; }
        public string[] Categories { get; set; }
         
    }

}
