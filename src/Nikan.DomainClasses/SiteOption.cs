using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.DomainClasses
{
    public class SiteOption  
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public SiteOptionCategoryEnum Category { get; set; }


    }



}
