using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 

namespace Nikan.DomainClasses.BaseEntity
{
    public class TicketSubject 
    {
        public string Id { get; set; }

        public TicketSubject()
        {
          
            Id = Guid.NewGuid().ToString();

        }

        public string Title { get; set; }

 
        public string Description { get; set; }


        /// <summary>
        /// این موضوع مربوط به کدام واحد می باشد
        /// </summary>
        public string OrganizationalUnitId { get; set; }
        public virtual OrganizationalUnit OrganizationalUnit { get; set; }



       


        public int IndexOrder { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }


        [StringLength(255)]
        public string ThumbUrl { get; set; }


        public DateTime?  ModifiedOnDate { get; set; }
        public DateTime CreateOnDate { get; set; }


        public ICollection<Ticket> Tickets { get; set; }


    }
}