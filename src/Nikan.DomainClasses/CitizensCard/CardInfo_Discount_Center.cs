namespace Nikan.DomainClasses.CitizensCard
{
	public class CardInfo_Discount_Center
	{

		public int Id { get; set; }

		public int DiscountId { get; set; }
		public CardInfo_Discount Discount { get; set; }

		public string CenterID { get; set; }
		public OrganizationalUnit Center { get; set; }


		public bool CenterIsActive { get; set; }


	}




}
