using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.DomainClasses.Citizens
{
    /// <summary>
    /// خروجی شهروندان
    /// </summary>
    public class ExportCitizens
    {


        public int Id { get; set; }

        /// <summary>
        /// شماره خروجی
        /// </summary>
        public int ExportNumber { get; set; }

        public User ExportBy { get; set; }
        public int? ExportById { get; set; }
        public int CountRow { get; set; }

       
         
        public ExportCitizenTypeEnum ExportType { get; set; }

        public string ExportFileName { get; set; }
        public string HistoryLog { get; set; }

        

        public DateTime? SendOnDate { get; set; } 
        public User ReceiveBy { get; set; }
        public int? ReceiveById { get; set; }

        public DateTime? ReceiveOnDate { get; set; }

        public int? ExportedForeignId { get; set; }

        public int? AcceptCount { get; set; }

        public DateTime? CreationDate { get; set; }
        public DateTime? ConfirmedData { get; set; }
        public bool?  IsDeleted { get; set; } 
        public string  Code { get; set; }

        /// <summary>
        /// استعلام مربوط به چه گروه شهروندی می باشد ؟
        /// </summary>
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }





        public ICollection<ExportedCitizens> ExportedCitizens { get; set; }

    }

   public class ExportedCitizens
    {
        public int Id { get; set; }
        public Citizen Citizen { get; set; }
        public int CitizenId { get; set; }


        public ExportCitizens Export  { get; set; }
        public int ExportId { get; set; } 

        public bool? Verified { get; set; }

        public DateTime? VerifyDate { get; set; }
        public DateTime CreationDate { get; set; }
         

        public int Count { get; set; } 

    }


}
