using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.ExportCitizen
{
    public class ExportCitizensInfo
    {


        public int Id { get; set; }

        public int ExportNumber { get; set; } 


        public string  ExportBy { get; set; }
        public int? ExportById { get; set; }

        public Guid? ExportByUserCode { get; set; }



        public string ReceiveBy { get; set; }
        public int? ReceiveById { get; set; }


        public Guid? ReceiveByUserCode { get; set; }



        public ExportCitizenTypeEnum ExportType { get; set; }

        public int CountRow { get; set; } 

        public string ExportFileName { get; set; }


        public DateTime? SendOnDate { get; set; }


        public DateTime? CreationDate { get; set; }

        public DateTime? ReceiveOnDate { get; set; }

        public int? ExportedForeignId { get; set; }

        public int? AcceptCount { get; set; }
        public bool IsConfirmed { get; set; }



        public string GroupName { get; set; }






    }



    public class ExportedCitizensInfo
    {

        public int Id { get; set; }

       
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FatherName { get; set; }
        public string NationCode { get; set; }
        public bool Gender { get; set; }

        public SabtStatusEnum SabtStatus { get; set; }



        public DateTime? BirthDate { get; set; }
        public int CitizenId { get; set; }

        public Guid? UserCode { get; set; }

        public int Count { get; set; }
        public int ExportId { get; set; }

        public bool IsConfirmed { get; set; }

        public bool? Verified { get; set; }

        public DateTime? VerifyDate { get; set; }
        public DateTime CreationDate { get; set; }


       

    }

    public class PagedExportCitizenViewModel
    {

        public List<ExportCitizensInfo> Items { get; set; } 
        public int TotalItems { get; set; }


    }

    /// <summary>
    /// شهروندانی که در خروجی یک فایل ارسال شده است
    /// </summary>
    public class PagedExportedCitizenViewModel
    {

        public List<ExportedCitizensInfo> Items { get; set; }
        public int TotalItems { get; set; }


    }


    public class SendSmsForSabtAhvalCitizens
    {

        /// <summary>
        ///  لیست پیامک ها
        /// </summary>
        public List<int> Ids { get; set; }
        /// <summary>
        /// کد دوره
        /// </summary>
        public int ExportId { get; set; }


    }




    public class  ExportedCitizenForSabtAhvalDto
    {


        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; } 
        public int? GroupId { get; set; }

        public ExportCitizenTypeEnum ExportType { get; set; }




    }









}
