using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.DomainClasses.BaseEntity
{
    public class OrganizationalPosition
    {
       


        public int  Id { get; set; } 
        public string Name { get; set; }  
        public int IndexOrder { get; set; }

       
        public bool IsActive { get; set; }

          


    }
}
