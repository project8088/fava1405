 
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Ticket
{

    public class PagedTicketItemsViewModel
    { 
        public string TicketStatus { get; set; } = string.Empty;

        public List<TicketDetailsDto> Tickets { get; set; }

         public int TotalItems { get; set; }


    }

    public class PagedContactViewModel
    {
       

        public List<ContactDetailsDto> ContactList { get; set; }

        public int TotalItems { get; set; }


    }



    public class PagedContactItemsViewModel
    {

        public PagedContactItemsViewModel()
        {
            Paging = new PaginationSettings();
        }

        public string TicketStatus { get; set; } = string.Empty;

        public List<TicketDetailsDto> Tickets { get; set; }

        public PaginationSettings Paging { get; set; }


    }



}
