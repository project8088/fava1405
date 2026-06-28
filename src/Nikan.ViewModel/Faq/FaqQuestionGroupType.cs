using Nikan.Common.Resource;
using Nikan.DomainClasses;
 
using System;
using System.ComponentModel.DataAnnotations;

namespace Nikan.ViewModel.Faq
{
    /// <summary>
    /// گروهبندی سوالات
    /// </summary>
    public class FaqQuestionGroupTypeViewModel 
    {

       
        public int? Id { get; set; }
        
        
        
        public string Title { get; set; }
        
        public string Description { get; set; }
       

        public virtual int? ModifiedById { get; set; }
        public virtual User ModifiedBy { get; set; }


        public virtual int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }




        [Display(ResourceType = typeof(ModelResource), Name = "OrganizationalUnitId")]
        public virtual string OrganizationalUnitId { get; set; }
        public virtual OrganizationalUnit OrganizationalUnit { get; set; }




        [Display(ResourceType = typeof(ModelResource), Name = "CreateOnDate")]
        public DateTime CreateOnDate { get; set; }

        public DateTime? ModifiedOnDate { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = "Icon")]
        public string Icon { get; set; }



       
        public bool  IsDeleted { get; set; }


       [Display(ResourceType = typeof(ModelResource), Name = "IsActive")]
         public bool IsActive { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = "IndexOrder")]
        public int IndexOrder { get; set; }



    }


    public class FaqQuestionGroupTypeDto
    {


        public int? Id { get; set; }



        public string Title { get; set; }

        public string Description { get; set; }

         

       
        public string Icon { get; set; } 

        
        public bool IsActive { get; set; } 
        
        public int IndexOrder { get; set; }



    }



}
