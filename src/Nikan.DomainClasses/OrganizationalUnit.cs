using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nikan.DomainClasses.AuditableEntity;
using Nikan.DomainClasses.Citizens;

namespace Nikan.DomainClasses
{
    /// <summary>
    /// واحد تحویل کارت
    /// </summary>
    public class OrganizationalUnit
    {
        public string Id { get; set; }

        public OrganizationalUnit()
        {
            this.ThumbUrl = "/images/blank-building.png";
            Id = Guid.NewGuid().ToString();

        }

        public string Name { get; set; }

 
        public string Description { get; set; }

        public string FullAddress { get; set; }

        public string OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

         

        public int IndexOrder { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }


        [StringLength(255)]
        public string ThumbUrl { get; set; }




        public ICollection<Ticket> Tickets { get; set; }
 


    }

    
    /// <summary>
    /// هر واحد سازمانی می تواندشامل یک یا چند گروه شهروندی باشد
    /// </summary>
    public class OrganizationalUnitGroups
    {

        public int Id { get; set; }

        public virtual OrganizationalUnit OrganizationalUnit { get;set;}
        public   string   OrganizationalUnitId { get; set; }

        public virtual Group Group { get; set; }
        public int GroupId { get; set; }

    }



}