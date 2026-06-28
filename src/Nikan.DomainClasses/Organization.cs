 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nikan.DomainClasses
{
    public class Organization
    {

        public Organization()
        {
            this.ThumbUrl = "/images/blank-building.png";
            OrganizationId = Guid.NewGuid().ToString();

        }

        [Key]
        public string OrganizationId { get; set; }

        [Required]
        [StringLength(100)]
        public string OrganizationName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }



        [StringLength(255)]
        public string ThumbUrl { get; set; }

 
        //refer to Application User
        public int? UserOwnerId { get; set; }

        public virtual User UserOwner  { get; set; }
        public int IndexOrder { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public string  BanerUrl { get; set; }



        public bool? CardDistributionCenters { get; set; }
        public bool? SupportCenters { get; set; }


        public virtual ICollection<OrganizationalUnit> OrganizationalUnit { get; set; }



    }
}
