using Nikan.Common.GlobalEnum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nikan.DomainClasses.CitizensCard
{
	/// <summary>
	/// برگشت کارت
	/// </summary>
	public class CitizensCardBackCard
	{
		[Key]
		public int  Id { get; set; }
		public int CitizenCardInfoId { get; set; } 
		public CitizensCard CitizenCardInfo  { get; set; }

		public string  ReasonBackDescription { get; set; }

		public string DeliveringCenterId { get; set; }
		public OrganizationalUnit DeliveringCenter { get; set; }


		public CardRequestStatusEnum PreRequestStatuse { get; set; }


		public int? BackCardByOperationId { get; set; }
		public User BackCardByOperation { get; set; }


		public DateTime BackCardOnDate { get; set; }
		 
		 
	}

 


}
