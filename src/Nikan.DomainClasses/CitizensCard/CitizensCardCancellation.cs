using Nikan.Common.GlobalEnum;
using System;

namespace Nikan.DomainClasses.CitizensCard
{
    /// <summary>
    /// باطل کردن کارت
    /// </summary>
    public class CitizensCardCancellation
	{
		public int Id { get; set; }
		public int CitizenCardInfoId { get; set; }
		public CitizensCard CitizenCardInfo { get; set; }

		public int? CardCancellationByOperationId { get; set; }
		public User CardCancellationByOperation { get; set; }

		public string ReasonCardCancellation { get; set; }

		public DateTime CardCancellationOnDate { get; set; }


		public CardCancellationTypeEnum CardCancellationType { get; set; }


		public string Description { get; set; }

		 
		 
	}






}
