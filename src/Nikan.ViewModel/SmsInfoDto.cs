using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel
{
    public class SmsInfoDto
    {
        public int Id { get; set; }
        public string MessageText { get; set; }
        public string Mobiles { get; set; }
        public long GroupListId { get; set; }

        public long MessageId { get; set; }

        public int SmsStatus { get; set; }

        public string StatusText { get; set; }
        public string Sender { get; set; }

        public DateTime SendOnDate { get; set; }
        public long Date { get; set; }


        public int Cost { get; set; }

        public string Token20 { get; set; }
        public string Token10 { get; set; }
        public string Token3 { get; set; }
        public string Token2 { get; set; }
        public string Token1 { get; set; }
        public string TempleteName { get; set; }

        public int? UserId { get; set; }
        public virtual string UserName { get; set; }

        public int? ContractCode { get; set; }
        public int? UserCompanyId { get; set; }
        public virtual string UserCompany { get; set; }


    }
    public class PagedSmsInfoViewModel
    {
         
        public List<SmsInfoDto> SmsList { get; set; } 
        public int TotalItems { get; set; }


    }

    

}
