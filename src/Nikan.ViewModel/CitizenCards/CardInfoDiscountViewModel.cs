using System;
using System.Collections.Generic;

namespace Nikan.ViewModel.CitizenCards
{
    public class CardInfo_DiscountDto
    {


        public int? Id { get; set; }
        public string DiscountTitle { get; set; }
        public string AttachmentGroup { get; set; }
        public string Description { get; set; }
        public int CardTypeId { get; set; }
        public bool DiscountIsActive { get; set; }
        public int DiscountPercent { get; set; }
        public int PostalPercentInCity { get; set; }
        public int PostalPercentOutCity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool PostDeliveryPossibility { get; set; }
        public bool CenterDeliveryPossibility { get; set; }
        public int[] GroupIds { get; set; }
        public string[] CenterIds { get; set; }

    }

	public partial class CardInfo_DiscountViewModel
	{


		public int Id { get; set; }

		/// <summary>
		/// عنوان تخفیف
		/// </summary>
		public string DiscountTitle { get; set; }


		/// <summary>
		/// این تخفیف برای چه کارتی تعریف شده است
		/// </summary>
		public int CardTypeId { get; set; }
		public string CardType { get; set; }





		/// <summary>
		/// ایجاد تخفیف در چه تاریخی صورت گرفته است
		/// </summary>
		public DateTime CreationDate { get; set; }

		/// <summary>
		/// این تخفیف توسط چه کاربری ایجاد شده است؟
		/// </summary>
		public int ByUserId { get; set; }
		public string ByUser { get; set; }



		public int? LastUpdateByUserId { get; set; }
		public string LastUpdateByUser { get; set; }





		/// <summary>
		/// درصد تخفیف
		/// </summary>
		public int? DiscountPercent { get; set; }


		/// <summary>
		/// درصد تخفیف ارسال پستی داخل شهری
		/// </summary>
		public int? PostalPercentInCity { get; set; }
		/// <summary>
		/// درصد تخفیف ارسال پستی بیرون شهری
		/// </summary>
		public int? PostalPercentOutCity { get; set; }



		/// <summary>
		/// تاریخ شروع تخفیف
		/// </summary>
		public DateTime? StartDate { get; set; }



		/// <summary>
		/// تاریخ پایان تخفیف
		/// </summary>
		public DateTime? EndDate { get; set; }




		public bool PostDeliveryPossibility { get; set; }
		public bool CenterDeliveryPossibility { get; set; }
		public bool DiscountIsActive { get; set; }
		public string Description { get; set; }
		public string AttachmentGroup { get; set; }
		public bool IsDeleted { get; set; }

		public List<int> GroupIds { get; set; }
		public IEnumerable<BaseDataModel>  Groups { get; set; }


	}


	public class PagedDiscountCardList
	{
		public int TotalItems { get; set; }
		public List<CardInfo_DiscountViewModel> Items { get; set; }

	}


	public class AddDiscountCenterDto
    {
         
        public int DiscountId { get; set; }
        public string CenterId { get; set; }

    }


}
