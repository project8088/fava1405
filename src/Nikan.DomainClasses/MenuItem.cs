
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nikan.DomainClasses.NewsItem
{
    public class MenuItem
    {

        public MenuItem()
        {
            CreatedOnDate = DateTime.Now;

        }


        
        public int Id { get; set; }
        
        [StringLength(50)] 
        public string MenuName { get; set; } 
        public string MenuPath { get; set; }
         

        public int? TabOrder { get; set; }
      
        public string IconFile { get; set; }
      

        public DateTime CreatedOnDate { get; set; }
        public DateTime? ModifiedOnDate { get; set; }

         
         


        public int? ParentId { get; set; }
        public MenuItem Parent { get; set; }

        

       
        public int?  CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }


        public int? LastModifiedByUserId { get; set; }
        public User  LastModifiedByUser { get; set; }

        public bool IsVisible { get; set; }
        public bool IsDeleted { get; set; }
        public bool OpenInNewPage { get; set; }

        public bool DisableLink { get; set; }


        public bool IsSystem { get; set; }




        public virtual ICollection<MenuItem> Children { get; set; }



    }



}
