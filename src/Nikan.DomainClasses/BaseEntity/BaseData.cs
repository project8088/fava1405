using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nikan.DomainClasses.BaseEntity
{


    public class BaseData
    {
        

        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public string LanguageName { get; set; }

        public string Description { get; set; }

        public bool? Disabled { get; set; }
        public bool? Selected { get; set; }

        public int IndexOrder { get; set; }


    }
}
