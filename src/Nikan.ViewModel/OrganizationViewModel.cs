 
using Nikan.Common.Resource;
using Nikan.DomainClasses;
 
using System;
using System.ComponentModel.DataAnnotations;

namespace Nikan.ViewModel
{
    public class OrganizationViewModel 
    { 
        public string Id { get; set; } 
        public string OrganizationName { get; set; } 
      
        public string Description { get; set; } 
       
        public string ThumbUrl { get; set; }


        public bool? CardDistributionCenters { get; set; }
        public bool? SupportCenters { get; set; }

        public int IndexOrder { get; set; } 
        
        public bool IsActive { get; set; }

        

    }

    public class OrganizationUnitGroups
    {
        public int  Id { get; set; }
        public int  GroupId { get; set; }
        public string  UnitId { get; set; }



    }

    public class OrganizationUnitGroupsInfo
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Group { get; set; }
        public string UnitId { get; set; }

        public string Unit { get; set; }

    }

}
