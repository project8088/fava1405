using Nikan.Common.GlobalEnum;
using Nikan.ViewModel.UserCompanes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.ImportExcelFile
{

    public class ImportExcelFileInfo
    {


        public int Id { get; set; }
        public int CountRow { get; set; }
        public ImportExcelFileTypeEnum ImportExcelFileType { get; set; }
        public string ExportFileName { get; set; }
        public DateTime CreationDate { get; set; }


        public string ExportFilePath { get; set; }

        public int? ImportByUserId { get; set; }
        public string  ImportByUser { get; set; } 

        public int? ReviewByUserId { get; set; }
        public string ReviewByUser { get; set; }




        public bool? FileAccept { get; set; }
        public DateTime? ReviewOnData { get; set; }

        public int? UserCompanyId { get; set; }
        public string  UserCompany { get; set; }


    }

    public class ImportFileInfo
    {
        /// <summary>
        /// شناسه فایل
        /// </summary>
        public int ImportId { get; set; }

        /// <summary>
        /// نام فایل
        /// </summary>
        public string FileName { get; set; }




        public string GroupName { get; set; }



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
        public List<CompanyPersonnelInfo> PersonnelInfo { get; set; }
        public List<ImportFileGroupDetails> ImportFileGroupDetails { get; set; }


    }


    public class ImportFileGroupDetails
    {


        public int  Id { get; set; }
        /// <summary>
        /// شناسه فایل
        /// </summary>
        public int ImportId { get; set; } 
        public int? CitizenId { get; set; }
        public string  Citizen { get; set; } 
        public string NationCode { get; set; }



    }

    public class ImportFileGroupNationCodeInfo
    { 
        public string NationCode { get; set; } 
    }


}
