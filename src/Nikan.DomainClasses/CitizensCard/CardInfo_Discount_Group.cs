using Nikan.DomainClasses.Citizens;

namespace Nikan.DomainClasses.CitizensCard
{
	/// <summary>
	/// گروه تخفیف
	/// </summary>
	public   class  CardInfo_Discount_Group 
	{
		 

		public int Id { get; set; } 

		public int DiscountId { get; set; }
        public CardInfo_Discount Discount  { get; set; } 

		public int GroupId { get; set; }
		public Group Group { get; set; }



		
		public bool DiscountGroupIsActive { get; set; }

	  
	 
	}




}
