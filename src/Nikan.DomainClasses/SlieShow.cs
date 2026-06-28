using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.DomainClasses
{

    public class SlideShow
    {

        public int Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public int IndexOrder { get; set; }
        public bool IsActive { get; set; }
        public User CreatedBy { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedOnDate { get; set; }



    }
}
