 
using System;
using System.ComponentModel.DataAnnotations;

namespace Nikan.DomainClasses
{
    public class TicketMessage
    {
        [Key]
        public string TicketMessageId { get; set; }



        [Required]
        [StringLength(200)]
        public string Description { get; set; }


        public DateTime OnDate { get; set; }


        public string TicketId { get; set; }

        public virtual Ticket Ticket { get; set; } 
        public int? UserOwnerId { get; set; }

        public virtual User UserOwner { get; set; }

      
       
        public string AttachmentGuid { get; set; }




    }


}
