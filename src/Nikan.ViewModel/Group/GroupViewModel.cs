using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Group
{

	public class GroupInfo
	{


		public int Id { get; set; }


		public int? ParentId { get; set; }
		public string  Parent { get; set; } 

		public int? MainGroupId { get; set; }
		public int? GroupCategory { get; set; }
		public DateTime? ExpireDate { get; set; }

		public string Code { get; set; }

		public string GroupName { get; set; }
		public bool AutoAddMembers { get; set; }
		public bool ShowToMembers { get; set; }
		public bool ShowToAddCitizen { get; set; }
		public int? MaxMembers { get; set; }
		public bool SpecialRules { get; set; }

		public bool Law_Gender { get; set; }
		public int? Law_AgeFrom { get; set; }
		public int? Law_AgeTo { get; set; }

		public bool Law_MariageStatus { get; set; }
		public bool Law_EducationLeve { get; set; }
		public bool Law_City { get; set; }
		public bool Law_JobGroup { get; set; }
		public bool IsActive { get; set; }
	 

		public bool? UseForServices { get; set; }

		public string ViewCssClass { get; set; }
		public string ViewIcon { get; set; }

		public int  CountCitizen { get; set; }


		/// <summary>
		/// آیا جز گرووهای شهرداری است
		/// </summary>
		public bool? MunicipalPersonnelGroup { get; set; }




		public int CountQueue { get; set; }


		public int? CreatedByUserId { get; set; }
		public string  CreatedByUser { get; set; }

		public bool? CanBuyFreeCard { get; set; }
		public DateTime CreationDate { get; set; }

	}


	public class GroupDto
	{

		public int? Id { get; set; }




		public int? ParentId { get; set; } 
		 
		public DateTime? ExpireDate { get; set; }

		public string Code { get; set; }

		public string GroupName { get; set; }
		public bool AutoAddMembers { get; set; }
		public bool ShowToMembers { get; set; }
		public bool ShowToAddCitizen { get; set; }
		public int? MaxMembers { get; set; }
		public int? Law_AgeFrom { get; set; }
		public int? Law_AgeTo { get; set; }


		public bool SpecialRules { get; set; }

		public bool Law_Gender { get; set; }

		public bool IsActive { get; set; }


		/// <summary>
		/// آیا جز گرووهای شهرداری است
		/// </summary>
		public bool? MunicipalPersonnelGroup { get; set; }
		public bool? CanBuyFreeCard { get; set; }




	}

	public class PagedGroupsViewModel
	{
		public int TotalItems { get; set; }
		public List<GroupInfo> Items { get; set; }

	}


	/// <summary>
	/// اطلاعات عضویت شهروند در گروههای شهروندی
	/// </summary>
	public class GroupCitizensInfo
	{

		public int Id { get; set; }
		public int GroupId { get; set; }
		public string Group { get; set; }
		public int CitizenId { get; set; }
		public string Citizen { get; set; }

		public Guid? UserCode { get; set; }


		public string NationCode { get; set; }

		public DateTime? CreationDate { get; set; }
		public int? AddByUserId { get; set; }
		public string AddByUser { get; set; }
	}



	/// <summary>
	/// صف های شهروندی
	/// </summary>
	public class CitizensQueueInfo
	{ 
		public int Id { get; set; }
		public int GroupId { get; set; }
		public string  Group { get; set; }
		public string NationCode { get; set; }
		public DateTime? CreationDate { get; set; }
		public int? AddByUserId { get; set; }
		public string  AddByUser { get; set; }

	}

	public class PagedCitizensQueueViewModel
	{
		public int TotalItems { get; set; }
		public List<CitizensQueueInfo> Items { get; set; }

	}



	public class GroupTransferModel
	{
		public int SourceGroupId { get; set; }
		public int DestinationGroupId { get; set; }
		public bool IsTransfer { get; set; }
		/// <summary>
		/// آیا صف ها را هم در نظر گرفته شود ؟
		/// </summary>
		public bool IsHasQueue { get; set; }

		/// <summary>
		/// در صورت در نظر گرفتن صف آیا از مبدا حذف شود ؟
		/// </summary>
		public bool IsTransferQueue { get; set; }

	}




}
