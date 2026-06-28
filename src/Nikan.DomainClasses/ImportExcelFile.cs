using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.UserCompanes;
using System;
using System.Collections.Generic;
 

namespace Nikan.DomainClasses
{
    public class ImportExcelFile
    {


        public int Id { get; set; } 
        public int CountRow { get; set; } 
        public ImportExcelFileTypeEnum ImportExcelFileType { get; set; } 
        public string ExportFileName { get; set; } 
        public DateTime CreationDate { get; set; }


        public string ExportFilePath { get; set; }

        public int? ImportByUserId { get; set; } 
        public User ImportByUser { get; set; }


        public int? UserCompanyId { get; set; }
        public UserCompany UserCompany { get; set; } 
        public int? ReviewByUserId { get; set; }
        public User ReviewByUser { get; set; }

        /// <summary>
        /// برای چه گروهی بارگذاری شده است ؟
        /// </summary>
        public int? GroupId { get; set; }
        public  Group Group { get; set; }




        public bool? IsConfirmed { get; set; }
        public DateTime? ReviewOnData { get; set; }


        public virtual ICollection<CompanyPersonnel> CompanyPersonnels { get; set; }
        public virtual ICollection<ImportExcelFileDetails> ImportExcelFileDetails { get; set; }

        public bool IsDeleted { get; set; }

    }

    public class ImportExcelFileDetails
    {

        public int Id { get; set; }

        public ImportExcelFile ImportExcelFile { get; set; }
        public int ImportExcelFileId { get; set; }


        public int? CitizenId { get; set; }
        public Citizen  Citizen { get; set; }

        public bool Gender { get; set; }
        public string NationCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Mobile { get; set; }
        public int? ServiceId { get; set; }

        public int? GroupId { get; set; }
         
        public bool IsValidRow { get; set; }
        public string Description { get; set; }

    }
     

}
