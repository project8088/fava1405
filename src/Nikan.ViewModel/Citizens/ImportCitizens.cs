using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Citizens
{
    public class CitizenImportExcelFileDetails
    {

        public int Id { get; set; } 
        public bool Gender { get; set; }
        public string NationCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public string FatherName { get; set; }
       
        public DateTime  BirthDate { get; set; }
      
        public string Mobile { get; set; }
        public int   ServiceId { get; set; }

        public int? GroupId { get; set; }


        public bool IsValidRow { get; set; }
        public string Description { get; set; }


    }


    public class CitizenImportFileInfo
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
        public List<CitizenImportExcelFileDetails> CitizenList { get; set; }

        public int  Count { get; set; }

    }

    public class CitizenExcelFileColumns
    {

        public string Gender { get; set; }
        public string NationCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; } 
        public string BirthDate { get; set; } 
        public string Mobile { get; set; }
        public string ServiceId { get; set; }

        public string GroupId { get; set; }

    }


    public class CitizenImportPagedList
    {
        /// <summary>
        /// شناسه فایل
        /// </summary>
        public int ImportId { get; set; }

        /// <summary>
        /// نام فایل
        /// </summary>
        public string FileName { get; set; }
        public string ImportBy { get; set; }
        /// <summary>
        /// در چه تاریخ بارگذاری شده است
        /// </summary>
        public DateTime OnDate { get; set; }

         
        public int TotalItems { get; set; }

        public List<CitizenImportExcelFileDetails> Items { get; set; }
        
    }







}
