using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nikan.ViewModel
{
 


 


        /// <summary>
        /// سوابق تحصیلی کارجو
        /// </summary>
  public class karjoEducationDto
    { 
        public int? Id { get; set; } 
       
        public virtual string Grade { get; set; }
      
        public virtual GradeEnum GradeId { get; set; }


        public string University { get; set; }


        public DateTime? DateOfStart { get; set; }
        //------------------------------------------------------------------
        public DateTime? DateOfEnd { get; set; }

        public int? MajorId { get; set; }
        public virtual string Major { get; set; }

        public int? CitizenId { get; set; }


    }

  
   

  



}
