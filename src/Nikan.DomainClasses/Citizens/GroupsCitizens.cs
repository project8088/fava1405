using System;

namespace Nikan.DomainClasses.Citizens
{
    /// <summary>
    /// عضویت شهروند در گروه 
    /// </summary>
    public class GroupsCitizens
	{

		public int Id { get; set; }
		public int GroupId { get; set; }
		public Group Group { get; set; }
		public int CitizenId { get; set; }
		public Citizen Citizen { get; set; }
		public DateTime? CreationDate { get; set; }
		public int? AddByUserId { get; set; }
		public User AddByUser { get; set; }

		public DateTime? ExpireDate { get; set; }


		public bool? IsDeleted { get; set; }
		public int? DeletedByUserId { get; set; }
		public User DeletedByUser { get; set; } 
		public DateTime? DeletedDate { get; set; }



	}
	/// <summary>
	/// صف های شهروندی
	/// </summary>
	public class CitizensQueue
	{


		public int Id { get; set; }
		public int GroupId { get; set; }
		public Group Group { get; set; }
		public string NationCode { get; set; }
		public DateTime? CreationDate { get; set; }
		public int? AddByUserId { get; set; }
		public User AddByUser { get; set; }

	}


	 

}
