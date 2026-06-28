using System;
using System.Collections.Generic;

namespace Nikan.DomainClasses.CitizensCard
{
    /// <summary>
    /// مشخصات تخفیف
    /// </summary>
    public partial class  CardInfo_Discount
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
		public CardType CardType { get; set; }
		
		
		
		
		
		/// <summary>
		/// ایجاد تخفیف در چه تاریخی صورت گرفته است
		/// </summary>
		public  DateTime CreationDate { get; set; }
	
		/// <summary>
		/// این تخفیف توسط چه کاربری ایجاد شده است؟
		/// </summary>
		public int OperationId { get; set; } 
		public User Operation  { get; set; }



		public int? LastUpdateByUserId { get; set; }
		public User LastUpdateByUser { get; set; }





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
		public  DateTime?  StartDate { get; set; }
		
		
		
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

		public virtual ICollection<CardInfo_Discount_Group> CardInfo_Discount_Groups { get; set; } 
		public virtual ICollection<CardInfo_Discount_Center>  CardInfo_Discount_Center { get; set; }

	}








}
