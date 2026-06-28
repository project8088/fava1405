using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.CitizenCards
{
	public class CardInfoDto
	{

		 
		public string CardInfoId { get; set; }

		/// <summary>
		/// نوع کارت
		/// </summary>
		public int CardTypeId { get; set; }
	 

		/// <summary>
		/// توضیحات خرید کارت
		/// </summary>
		public string BuyCardDescription { get; set; }
 

	 


		/// <summary>
		/// هزینه کارت
		/// </summary>
		public int CardCost { get; set; }

		/// <summary>
		/// هزینه کارت المثنی
		/// هزینه کارت المثنی
		/// </summary>
		public int DoubleCardCost { get; set; }

		/// <summary>
		/// هزینه پست داخلی
		/// </summary>
		public int PostalCostInCity { get; set; }


		/// <summary>
		/// هزینه پست خارج از شهر
		/// </summary>
		public int PostalCostOutCity { get; set; }


		/// <summary>
		/// آیا کارت فعال است
		/// </summary>
		public bool CardIsActive { get; set; } 
		public int VATForCardCost { get; set; }
		public int VATForPost { get; set; }

	}


	/// <summary>
	/// اطلاعات کامل یک کارت
	/// </summary>
	public class CardInfoViewModel
	{

		/// <summary>
		/// شناسه کارت
		/// </summary>
		public string CardInfoId { get; set; }


		/// <summary>
		/// نوع کارت
		/// </summary>
		public int CardTypeId { get; set; }
		public string CardType { get; set; }

		/// <summary>
		/// توضیحات خرید کارت
		/// </summary>
		public string BuyCardDescription { get; set; }

		/// <summary>
		/// تاریخ ایجاد کارت
		/// </summary>
		public DateTime? CreationDate { get; set; }

		/// <summary>
		/// ایجاد کارت توسط
		/// </summary>
		public string UserName { get; set; }
		public int OperationId { get; set; }


		/// <summary>
		/// هزینه کارت
		/// </summary>
		public int CardCost { get; set; }

		/// <summary>
		/// هزینه کارت المثنی
		/// هزینه کارت المثنی
		/// </summary>
		public int DoubleCardCost { get; set; }

		/// <summary>
		/// هزینه پست داخلی
		/// </summary>
		public int PostalCostInCity { get; set; }


		/// <summary>
		/// هزینه پست خارج از شهر
		/// </summary>
		public int PostalCostOutCity { get; set; }


		/// <summary>
		/// آیا کارت فعال است
		/// </summary>
		public bool CardIsActive { get; set; }
		public DateTime? StartFromDate { get; set; }
		public DateTime? ExpirationDate { get; set; }
		public string AttachmentGroup { get; set; }
		public int VATForCardCost { get; set; }
		public int VATForPost { get; set; }


		public int PostTotalAmount { get; set; }
		public int CardTotalAmount { get; set; }

		public bool  SetPrice { get; set; }
		public int TotalAmount { get; set; }
		public string TotalStr { get; set; }

	
		public int? DiscountId { get; set; } 
		public int? DiscountGroupId { get; set; } 
		public bool PostDeliveryPossibility { get; set; } 
		public bool CenterDeliveryPossibility { get; set; }

		public List<BaseDataModel> CenterList { get; set; }
		public int? CenterId { get; set; }

		/// <summary>
		/// مبلغ تخفیف پستی
		/// </summary>
		public int PostageDiscountAmount { get; set; }

		/// <summary>
		/// مبلغ تخفیف کارت
		/// </summary>
		public int CardDiscountAmount { get; set; }

		/// <summary>
		/// تخفیف کل
		/// </summary>
		public int TotalDiscountAmount { get; set; }

	}

	public class PagedCardInfoViewModel
	{

		public List<CardInfoViewModel> Items { get; set; }
		public int TotalItems { get; set; }


	}


}
