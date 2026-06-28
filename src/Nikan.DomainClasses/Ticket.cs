using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.BaseEntity;
using Nikan.DomainClasses.UserCompanes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nikan.DomainClasses
{
    public class Ticket  
    {

        [Key]
        public string TicketId { get; set; }

        /// <summary>
        /// موضوع تیکت
        /// </summary>
        public string TicketSubjectId { get; set; }
        public virtual TicketSubject TicketSubject { get; set; }

        public Ticket()
        {
            OnDate = DateTime.Now; 
            TicketId = Guid.NewGuid().ToString();
            TicketStatus = TicketStatusEnum.جدید;
            TicketType = TicketTypeEnum.Other;
            TicketPriority = TicketPriorityEnum.کم;
        }

      

        public string Code { get; set; }



        [Required]
        [StringLength(100)]
        public string Title { get; set; }
       
    
        public DateTime  OnDate { get; set; }



        [StringLength(100)]
        public string FullName { get; set; }



         [StringLength(100)]
         public string Email { get; set; }
      
         [StringLength(100)]
         public string City { get; set; }


        public string NationCode { get; set; }




        [StringLength(20)]
        
        public string Phone { get; set; }


        public int? UserOwnerId { get; set; }

        public virtual User UserOwner { get; set; }
         


      
        public TicketStatusEnum TicketStatus { get; set; }

        
        public  TicketTypeEnum TicketType { get; set; }
     
        public TicketPriorityEnum  TicketPriority { get; set; }
       
        public  TicketChannelEnum TicketChannel { get; set; }


        public TicketSectionEnum TicketSection { get; set; }


        public bool IsColsed { get; set; }

        public int? ColsedById { get; set; } 
        public virtual User ColsedBy { get; set; }

        public DateTime? ColsedOnDate { get; set; }






        public bool IsArchive { get; set; }  

        public bool IsSolved { get; set; }

        public string OrganizationalUnitId { get; set; }
        public virtual OrganizationalUnit OrganizationalUnit { get; set; }



        public int? UserCompanyId { get; set; }
        public virtual UserCompany UserCompany { get; set; }




        public ICollection<TicketMessage> TicketMessages { get; set; }


        


    }

    public class TicketComments
    {
        public int Id { get; set; }
      
        
        public string TicketId { get; set; } 
        public virtual Ticket Ticket { get; set; }


        public int? UserOwnerId { get; set; } 
        public virtual User UserOwner { get; set; }


        public DateTime OnDate { get; set; }

        [StringLength(1000)]
        public string CommentText { get; set; }
        public bool IsPrivate { get; set; }


    }

    public class TicketActivity
    {
        public int Id { get; set; }
        public string TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        public DateTime OnDate { get; set; }


         public int PeriodOfMinutes { get; set; }


        public DateTime CreatedOnDate { get; set; }

        public int? UserOwnerId { get; set; }
        public virtual User UserOwner { get; set; }


        public string  Description {get;set;}
        public bool IsSubtractFromContract {get;set;}



    }


}
