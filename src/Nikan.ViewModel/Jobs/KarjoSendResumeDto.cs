using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Jobs
{
    /// <summary>
    /// ارسال رزومه برای موقعیت شغلی
    /// </summary>
    public class KarjoSendResumeDto
    {
        public int JobInfoId { get; set; }

        public int? UserId { get; set; }

    }

    public class KarjoSendResumeList
    {

        public int SendResumeId { get; set; }

        public int JobInfoId { get; set; }

        public string JobTitle { get; set; }

        public int? UserId { get; set; }
        public bool Gender { get; set; }

        
        public string FirstName { get; set; }

         
        public string LastName { get; set; }


      
        public string NationalCode { get; set; }


        public string MobileNumber { get; set; }




    }

    public class PagedSendResumeViewModel
    {

        public List<KarjoSendResumeList> KarjoSendResume { get; set; }
        public int TotalItems { get; set; }


    }

}
