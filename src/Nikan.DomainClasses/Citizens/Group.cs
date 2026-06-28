using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.Citizens
{
    /// <summary>
    /// گروههای شهروندی
    /// </summary>
    public class Group 
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Key]
		public int  Id { get; set; }
		
		
		public int? ParentId { get; set; }
		public Group Parent  { get; set; }



		public int? MainGroupId { get; set; }
		public int? GroupCategory { get; set; }
		public DateTime? ExpireDate { get; set; }

		public string Code { get; set; }

		public string GroupName { get; set; }
		public bool  AutoAddMembers { get; set; }
		public bool  ShowToMembers { get; set; }
		public bool  ShowToAddCitizen { get; set; }
		public int?  MaxMembers { get; set; }
        public bool  SpecialRules { get; set; }

		public bool Law_Gender { get; set; }
		public int?  Law_AgeFrom { get; set; }
		public int?  Law_AgeTo { get; set; }

		public bool  Law_MariageStatus { get; set; }
		public bool Law_EducationLeve { get; set; }
		public bool Law_City { get; set; }
		public bool Law_JobGroup { get; set; } 
		public bool IsActive { get; set; }

		public bool? CanBuyFreeCard { get; set; }




		public bool IsSystem { get; set; }



		public bool IsDeleted { get; set; } 
	
		public bool? UseForServices { get; set; }

		public string ViewCssClass { get; set; }
		public string ViewIcon { get; set; }

		/// <summary>
		/// آیا جز گرووهای شهرداری است
		/// </summary>
		public bool? MunicipalPersonnelGroup { get; set; }
		

		public int? CreatedByUserId { get; set; }
		public User CreatedByUser  { get; set; } 
		public DateTime CreationDate { get; set; }

		public virtual ICollection<GroupsCitizens> GroupsCitizens { get; set; }

		public virtual ICollection<CitizensQueue> CitizensQueue { get; set; }


	}


}
