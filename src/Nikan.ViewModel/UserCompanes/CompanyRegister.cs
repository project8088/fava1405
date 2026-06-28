using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nikan.ViewModel.UserCompanes
{

 




    public class CompanyRegister
    {








         
        public string CompanyName { get; set; }


       
        public string EnglishName { get; set; }


        /// <summary>
        /// نام و نام خانوادگی نماینده شرکت
        /// </summary> 
        public string CompanyRepresentative { get; set; }//نماینده  شرکت



        /// <summary>
        /// سال تاسیس شرکت
        /// </summary> 
        public string EstablishedYear { get; set; }//  سال تاسیس




        /// <summary>
        /// شماره اقتصادی شرکت
        /// </summary>
        public string TxtTinNo { get; set; }


        /// <summary>
        /// شماره ثبت
        /// </summary>
        public string TxtRegNO { get; set; } 
        /// <summary>
        /// شماره موبایل
        /// </summary>
       public string MobileNumber { get; set; } 
        public string Email { get; set; } 
        
        public string UserName { get; set; } 

       
        public string Password { get; set; }

    }

    public class AddUserCompanyActivities 
    {  
        public int ActivityId { get; set; } 
        public int? UserCompanyId { get; set; }
         
    }

    public class  UserCompanyActivitiyInfo
    {
        public int Id { get; set; }
        public int ActivityId { get; set; } 
        public string  Activity  { get; set; }
        public int  UserCompanyId { get; set; }

        public string CompanyName { get; set; }


    }

}
