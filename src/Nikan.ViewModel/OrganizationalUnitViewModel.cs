 
using Nikan.Common.Resource;
using Nikan.DomainClasses;
 
using System;
using System.ComponentModel.DataAnnotations;

namespace Nikan.ViewModel
{
    public  class OrganizationalUnitViewModel 
    {
        
        public string Id { get; set; }

        public OrganizationalUnitViewModel()
        {
            this.ThumbUrl = "/images/blank-building.png";
           // Id = Guid.NewGuid().ToString();

        }


        [Display(ResourceType = typeof(ModelResource), Name = "OrganizationalUnitName")]
        public string Name { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = "Description")]
        public string Description { get; set; }





        [Display(ResourceType = typeof(ModelResource), Name = "OrganizationId")]
        
        public string OrganizationId { get; set; }
        public virtual string Organization { get; set; }




        [Display(ResourceType = typeof(ModelResource), Name = "IndexOrder")]
        public int IndexOrder { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = "IsDeleted")]
        public bool IsDeleted { get; set; }



        [Display(ResourceType = typeof(ModelResource), Name = "IsActive")]
        public bool IsActive { get; set; }


        [StringLength(255)]
        [Display(ResourceType = typeof(ModelResource), Name = "ThumbUrl")]
        public string ThumbUrl { get; set; }
  
    
    
    }


    public class OrganizationalUnitDto
    {
       
        public string Id { get; set; } 
      
        public string Name { get; set; } 
        public string Description { get; set; } 

        
        public string OrganizationId { get; set; } 
       
        public int IndexOrder { get; set; } 
        public bool? IsActive { get; set; } 
       
        public string ThumbUrl { get; set; }



    }



}
