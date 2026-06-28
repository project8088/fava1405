
 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nikan.ViewModel
{
    public class OrganizationalPositionViewModel
    {
        
        
        [Display(Name = "شناسه سازمانی")]
        public int? Id { get; set; }


        [Display( Name = "پست سازمانی")]
        [Required(ErrorMessage ="*")]
        public string Name { get; set; }


        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }


        [Display(Name = "ترتیب نمایش")]
        public int IndexOrder { get; set; }



      



        [Display(Name = "وضعیت نمایش")]
        public bool IsActive { get; set; }
    }
}
