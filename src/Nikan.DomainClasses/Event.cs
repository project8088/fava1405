using Nikan.Common.GlobalEnum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses
{
    public class Event
    {
        public int Id { get; set; }


        public DateTime  CreateDate { get; set; }
         

        public string ActionName { get; set; }
       
        public EventSectionEnum EventSection { get; set; }

        public EventPriorityEnum EventPriority { get; set; }


        public EventTypeEnum EventType  { get; set; }



        public int Code { get; set; }
        public string StrCode { get; set; }




        public int?  OperationId { get; set; }
        public User Operation { get; set; }




        

        public int? UserId { get; set; }
        public User User  { get; set; }
      
        public string WebSite { get; set; }

        public string Description { get; set; }

        public string UserIp { get; set; }

        public string JsonValue { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class ArchiveEvent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string ActionName { get; set; }

        public EventSectionEnum EventSection { get; set; }

        public EventPriorityEnum EventPriority { get; set; }

        public EventTypeEnum EventType { get; set; }

        public int Code { get; set; }

        public string StrCode { get; set; }

        public int? OperationId { get; set; }

        public User Operation { get; set; }

        public int? UserId { get; set; }

        public User User { get; set; }

        public string WebSite { get; set; }

        public string Description { get; set; }

        public string UserIp { get; set; }

        public string JsonValue { get; set; }

        public bool IsDeleted { get; set; }
    }










}
