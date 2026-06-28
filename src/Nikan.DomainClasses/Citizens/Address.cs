using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.BaseEntity;
using System;

namespace Nikan.DomainClasses.Citizens
{
    /// <summary>
    /// آدرس های شهروندی
    /// </summary>
    public class Address
	{

		public int Id { get; set; }
		public AddressTypeEnum AddressType { get; set; }
		public Citizen Citizen { get; set; }
		public int   CitizenId { get; set; }
		
		
		public City City { get; set; }
		public int  CityId { get; set; }
		
		
		
		public int? Region { get; set; }
		public string Street { get; set; }
		public string Alley { get; set; }
		public string PostalCode { get; set; }
		public string Plaque { get; set; }
		public string FullAddress { get; set; }
		public bool IsVerified { get; set; }
	
		public DateTime CreationDate { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime? LasteUpdateOnDate { get; set; }

		public bool IsActive { get; set; } 
		public string Phone { get; set; }

	}


	 

	 




}
