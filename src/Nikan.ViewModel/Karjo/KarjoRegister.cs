using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikan.ViewModel
{
    public class KarjoRegister
    {

        
        public bool? Gender { get; set; } 
       
        public string MobileNumber { get; set; } 
        public string Email { get; set; } 

        public string NationalCode { get; set; } 
       
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string FatherName { get; set; }

         
       public string Password { get; set; }

       
        
        public bool ApproveTermsCondition { get; set; }

    }
}
