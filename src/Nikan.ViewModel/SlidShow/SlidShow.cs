using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.SlidShow
{
    public class SlideShowDto
    {

        public int? Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public int IndexOrder { get; set; }
        public bool IsActive { get; set; }
        

    }
    public class SlieShowInfo
    {

        public int Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public int IndexOrder { get; set; }
        public bool IsActive { get; set; }
        public string  CreatedBy { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOnDate { get; set; }



    }
}
