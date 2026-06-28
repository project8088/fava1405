using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Citizens
{
    /// <summary>
    /// اعتبار سنجی شماره موبایل
    /// </summary>
    public class ValidatePhoneNumber
    {
          
        /// <summary>
        /// شماره موبایل شهروند
        /// </summary>
        public string MobileNumber { get; set; }
        /// <summary>
        /// کد ملی شهروند
        /// </summary>
        public string NationCode { get; set; }

       
    }
}
