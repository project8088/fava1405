using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Events
{
    public class EventDto
    {
      
        public string ActionName { get; set; } 
        public EventSectionEnum EventSection { get; set; }

        public EventPriorityEnum EventPriority { get; set; } 
        public EventTypeEnum EventType { get; set; } 

        public int Code { get; set; }
        public string StrCode { get; set; } 

        public int? OperationId { get; set; } 
        public int? UserId { get; set; } 

        public string WebSite { get; set; }

        public string Description { get; set; }

        public string UserIp { get; set; }



    }
    public class EventsInfo
    {
        public int Id { get; set; }


        public DateTime CreateDate { get; set; }





        public string ActionName { get; set; }

        public string  EventSection { get; set; }

        public string EventPriority { get; set; }


        public string EventType { get; set; }



        public int Code { get; set; }
        public string StrCode { get; set; }




        public int? OperationId { get; set; }
        public string  Operation { get; set; }





        public int? UserId { get; set; }
        public string User { get; set; }

        public string WebSite { get; set; }

        public string Description { get; set; }

        public string UserIp { get; set; }

        public string JsonValue { get; set; }

    }

    public class PagedEventViewModel
    {
        public int TotalItems { get; set; }

        public List<EventsInfo> Items { get; set; }




    }










}
