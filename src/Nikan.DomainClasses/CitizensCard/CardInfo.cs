using System;
using System.ComponentModel.DataAnnotations;

namespace Nikan.DomainClasses.CitizensCard
{


	/// <summary>
	/// مشخصات کارت  
	/// </summary>
	public class CardInfo
	{


		[Key]
		public string CardInfoId { get; set; }
		
		/// <summary>
		/// نوع کارت
		/// </summary>
		public int CardTypeId { get; set; }
        public virtual CardType CardType  { get; set; }

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
		public User Operation { get; set; }
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
		public decimal VATForCardCost { get; set; }
		public decimal VATForPost { get; set; }

	}



}
