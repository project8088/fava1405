using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nikan.ViewModel.Ticket
{
    public class TicketSubjectDto
    {
        public string Id { get; set; } 
        public string Title { get; set; } 
        public string Description { get; set; } 
        public string OrganizationalUnitId { get; set; }
        public string ThumbUrl { get; set; }


        public bool IsActive { get; set; }
         
    }


    public class TicketSubjectInfo
    {
        public string Id { get; set; } 
        public string Title { get; set; } 
        public string Description { get; set; } 
        public int IndexOrder { get; set; } 
        public bool IsActive { get; set; }
        public string OrganizationalUnitId { get; set; } 
        public virtual string  OrganizationalUnit { get; set; }


        public DateTime CreateOnDate { get; set; }
         


    }
    public class TicketChangeState
    {
        public string TicketId { get; set; }
        public bool IsClosed { get; set; }


    }

    public class TicketResultDto
    {
        public string TicketId { get; set; }
        public string MessageId { get; set; }
        public string Code { get; set; }
        public string MobileNumber { get; set; } 
        public bool SendSms { get; set; }

        public int? OwnerId { get; set; }


    }
    public class ContactDto2
    {

       
        public virtual string Subject { get; set; }

        

    }

    public class  ContactDto
    {

        public string Id { get; set; } 
        public virtual string Subject { get; set; } 
        
        public virtual string  Message { get; set; } 
        public string OrganizationalUnitId { get; set; } 
        public string Name { get; set; } 
        public string Email { get; set; }

        public string MobileNumber { get; set; }
        public int? UserId { get; set; }

        /// <summary>
        /// مخصوص ارسال تماس در صفحه پروفایل شرکت
        /// </summary>
        public int? CompanyId { get; set; }

    }


    public class SendUserTicketDto
    {


        public string Id { get; set; }

        /// <summary>
        /// موضوع
        /// </summary>
        [MaxLength(200)]
        public virtual string Subject { get; set; }


        /// <summary>
        /// شرح پیام
        /// </summary>
        [MaxLength(2000)]
        public virtual string TicketMessage { get; set; }

        public virtual TicketPriorityEnum Priority { get; set; }
      
        public string OrganizationalUnitId { get; set; }

        
        public string AttachmentGuid { get; set; }

        public string Name { get; set; }

        public string NationCode { get; set; }

        public string MobileNumber { get; set; }
        public int? UserId { get; set; }

        public string TicketSubjectId { get; set; }
         
    }

    public class TicketDetailsDto
    {

        public string Id { get; set; }
        /// <summary>
        /// موضوع
        /// </summary>
        [MaxLength(200)]
        public virtual string Subject { get; set; }


        /// <summary>
        /// شرح پیام
        /// </summary>
        [MaxLength(2000)]
        public virtual string TicketMessage { get; set; }


        /// <summary>
        /// کد رهگیری
        /// </summary>
        [MaxLength(200)]
        public virtual string Code { get; set; }

        [MaxLength(200)]
        public virtual string FullName { get; set; }

        [MaxLength(200)]
        public virtual string Email { get; set; }




        [MaxLength(200)]
        public virtual string City { get; set; }

        [MaxLength(50)]
        public virtual string NationalCode { get; set; }


        [MaxLength(50)]
        public virtual string MobileNumber { get; set; }


        public virtual string OwnerMobileNumber { get; set; }



        public virtual string Priority { get; set; }

        public virtual string TicketSection { get; set; }


        public virtual string TicketStatus { get; set; }


        public string OrganizationalUnitId { get; set; }

        public BaseDataModel OrganizationalUnit { get; set; }

         


        public string CreatedOn { get; set; }

        public int? OwnerId { get; set; }
        public string  OwnerUserName { get; set; }

        public virtual int? ColsedById { get; set; }

        public string ColsedByUserName { get; set; }


        public string ColsedOnDate { get; set; }

        public int MessageCount { get; set; }

        public bool IsColsed { get; set; }

        public bool IsSolved { get; set; }


        public List<ResponseTicketDto> ResponseTickets { get; set; }



    }


    public class ContactDetailsDto
    {

        public string Id { get; set; }
        
        public virtual string Subject { get; set; }

        public virtual string Message { get; set; }
        public string OrganizationalUnitId { get; set; }
        public BaseDataModel OrganizationalUnit { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string MobileNumber { get; set; }
        public int? UserId { get; set; }
        public string  UserName { get; set; }
        /// <summary>
        /// مخصوص ارسال تماس در صفحه پروفایل شرکت
        /// </summary>
        public int? CompanyId { get; set; }

        public string  CompanyName { get; set; }

    }





    public class ResponseTicketDto
    {

        public string TicketId { get; set; }
        public string MessageId { get; set; }

        public string Subject { get; set; }
        public string ResponseText { get; set; }
        public bool SendSms { get; set; }
        public bool Review { get; set; }
        public bool Solved { get; set; }
        public string MobileNumber { get; set; }

         

    

        public int? OwnerId { get; set; }

        public string OwnerDisplayName { get; set; }

        public string AttachmentGuid { get; set; }
        public DateTime ResponseTextOnDate { get; set; }

    }












}
