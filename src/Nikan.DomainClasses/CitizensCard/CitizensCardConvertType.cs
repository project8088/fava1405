using System;
using System.ComponentModel.DataAnnotations;

namespace Nikan.DomainClasses.CitizensCard
{
    /// <summary>
    /// تبدیل کارت
    /// </summary>
    public class CitizensCardConvertType
	{
		[Key]
		public int Id { get; set; }
		public int CitizenCardInfoId { get; set; }
		public CitizensCard CitizenCardInfo { get; set; }

		  
		public int  ConvertType { get; set; }


		public int? ByOperationId { get; set; }
		public User ByOperation { get; set; }


		public DateTime CreationDate { get; set; }

		public DateTime? ApprovalOnDate { get; set; }


		public int? ApprovalByOperationId { get; set; }
		public User ApprovalByOperation { get; set; }


		public string Description { get; set; }

	}

 


}
