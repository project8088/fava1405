using Nikan.Common.GlobalEnum;
using Nikan.Common.Resource;
using Nikan.DomainClasses;
 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nikan.ViewModel
{
    public class TicketViewModel
    {
        public TicketViewModel()
        {
            OnDate = DateTime.Now; 
            TicketId = Guid.NewGuid().ToString();
            TicketStatus = TicketStatusEnum.جدید;
            TicketType = TicketTypeEnum.Other;
            TicketPriority = TicketPriorityEnum.کم;
        }


        [Key]
        
        public string TicketId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }


        public DateTime OnDate { get; set; }

        [StringLength(100)]
        public string Email { get; set; }


        [StringLength(100)]
        [Display(ResourceType = typeof(ModelResource), Name = "FullName")]
        public string FullName { get; set; }


        [StringLength(20)]

        public string Phone { get; set; }


        public int? UserOwnerId { get; set; }

        public virtual User UserOwner { get; set; }


        public TicketStatusEnum TicketStatus { get; set; }


        public TicketTypeEnum TicketType { get; set; }

        public TicketPriorityEnum TicketPriority { get; set; }

        public TicketChannelEnum TicketChannel { get; set; }


        public TicketSectionEnum TicketSection { get; set; }




        public string OrganizationalUnitId { get; set; }
        public virtual OrganizationalUnit OrganizationalUnit { get; set; }

        public ICollection<TicketMessage> TicketMessages { get; set; }


    }


    public class SendTicketViewModel
    {
        public SendTicketViewModel()
        {
            
           
            TicketType = TicketTypeEnum.Other;
          //  TicketPriority = TicketPriorityEnum.کم;
        }



        [Required(ErrorMessage = "(*)")]
        [StringLength(100)]
        [Display(ResourceType = typeof(ModelResource), Name = "TicketTitle")]
        public string Title { get; set; }

      
        
        
        [StringLength(100)]
        [Display(ResourceType = typeof(ModelResource), Name = "FullName")] 
        public string FullName { get; set; }





        [StringLength(100)]
        [EmailAddress(ErrorMessage = "لطفا آدرس ایمیل معتبری را وارد نمائید.")]
        [Display(ResourceType = typeof(ModelResource), Name = "TicketEmail")]
        public string Email { get; set; } 




        [StringLength(20)]
        [Display(ResourceType = typeof(ModelResource), Name = "TicketPhone")]
        public string Phone { get; set; }



        [Required(ErrorMessage = "(*)")]
        [DataType(DataType.MultilineText)]
       [StringLength(1000)]
        [Display(ResourceType = typeof(ModelResource), Name = "TicketMessage")]

        public string TicketMessage { get; set; }



        [Display(ResourceType = typeof(ModelResource), Name = "UserOwnerId")]

        public int? UserOwnerId { get; set; }


      



        [Display(ResourceType = typeof(ModelResource), Name = "TicketType")]
        public TicketTypeEnum TicketType { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = "TicketPriority")]
        [Required(ErrorMessage = "(*)")]
        public TicketPriorityEnum? TicketPriority { get; set; }


        [Display(ResourceType = typeof(ModelResource), Name = "TicketSection")]
        [Required(ErrorMessage = "(*)")]
        public TicketSectionEnum TicketSection { get; set; }

        
        public string AttachmentGuid { get; set; }



        [Display(ResourceType = typeof(ModelResource), Name = "OrganizationalUnitId")]
        [Required(ErrorMessage = "(*)")]
        public string OrganizationalUnitId { get; set; }
        public virtual OrganizationalUnit OrganizationalUnit { get; set; }

        public List<OrganizationalUnit> OrganizationalUnitList { get; set; }

    }

    public class AnswerTicketViewModel
    {
       

        
        public string TicketId { get; set; }

       
        public string AttachmentGuid { get; set; }



        [Required(ErrorMessage = "(*)")] 
        [Display(ResourceType = typeof(ModelResource), Name = "TicketMessage")]
        
        public string TicketMessage { get; set; }



        [Display(ResourceType = typeof(ModelResource), Name = "UserOwnerId")]

        public int  UserOwnerId { get; set; }



        public bool Solved { get; set; }





    }
    public class TicketCommentsDto
    {
        public int Id { get; set; }

        public string TicketId { get; set; }  
 
        public string CommentText { get; set; }

        /// <summary>
        /// متن کامنت برای کاربر نمایش داده نشود ؟
        /// خصوصی است
        /// </summary>
        public bool IsPrivate { get; set; }


    }


    public class TicketCommentsViewModel
    {
        public int Id { get; set; }
      
        public string TicketId { get; set; }

        public virtual string  TicketTitle { get; set; }


        public int? UserOwnerId { get; set; }
        public virtual string  UserOwner { get; set; }



        public DateTime OnDate { get; set; }



        /// <summary>
        /// متن کامنت
        /// </summary>
        [StringLength(1000)]
        [DataType(DataType.MultilineText)] 
        [Required(ErrorMessage = "(*)")] 
        public string CommentText { get; set; }

         /// <summary>
         /// متن کامنت برای کاربر نمایش داده نشود ؟
         /// خصوصی است
         /// </summary>
        public bool IsPrivate { get; set; }


    }

    public class TicketActivityViewModel
    {
        
        public int? Id { get; set; }

       
        public string TicketId { get; set; }
        public virtual string  TicketTitle { get; set; }

         
        /// <summary>
        /// چند دقیقه فعالیت داشته است 
        /// </summary>
        [Display(  Name = "دقیقه")]
        public int Minute { get; set; }
        
       


        [Display( Name = "در  تاریخ")]
        public DateTime OnDate { get; set; }



       
        /// <summary>
        /// توسط جه کاربری ثبت شده است 
        /// </summary>
        public int? UserOwnerId { get; set; }
        public virtual string  UserOwner { get; set; }


        [StringLength(2000)]
        [DataType(DataType.MultilineText)]  
        [Required(ErrorMessage = "(*)")] 
        public string Description { get; set; }

        /// <summary>
        /// جز وظابف من بوده است ؟
        /// </summary>
        public bool IsSubtractFromContract { get; set; }



    }
    public class TicketActivityDto
    { 
       
        public string TicketId { get; set; }
      

        /// <summary>
        /// چند دقیقه فعالیت داشته است 
        /// </summary> 
        public int Minute { get; set; }
         

        [Display(Name = "در  تاریخ")]
        public DateTime OnDate { get; set; }
         
       
        public string Description { get; set; }

        /// <summary>
        /// جز وظابف من بوده است ؟
        /// </summary>
        public bool IsSubtractFromContract { get; set; }



    }

}
