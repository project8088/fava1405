using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.Factor;
using System;
using System.Collections.Generic;

namespace Nikan.DomainClasses.CitizensCard
{
    /// <summary>
    /// مشخصات کارت شهروندی
    /// </summary>
    public class CitizensCard
	{
		 
		public int Id { get; set; }
		/// <summary>
		/// کد درخواست
		/// </summary>
		public string RequestCode { get; set; }
		

		/// <summary>
		/// این کارت برای چه شهروندی است؟
		/// </summary>
		public int CitizenId { get; set; }
        public Citizen Citizen  { get; set; }



		/// <summary>
		/// چه نوع کارتی خرید کرده است؟
		/// </summary>
		public string CardInfoId { get; set; }
        public CardInfo CardInfo { get; set; }


		/// <summary>
		/// تاریخ درخواست
		/// </summary>
		public DateTime RequestDate { get; set; }


		/// <summary>
		/// شناسه تخفیف
		/// </summary>
		public int? DiscountGroupId { get; set; }
        public CardInfo_Discount_Group DiscountGroup  { get; set; }





		/// <summary>
		/// شناسه تراکنش
		/// </summary>
		public long? TransactionId { get; set; }
		public UserTransaction Transaction  { get; set; }





		/// <summary>
		/// این درخواست توسط چه شهروندی صورت گرفته شده است
		/// </summary>
		public int?  RequestByCitizenId { get; set; }
		public Citizen RequestByCitizen { get; set; }

		/// <summary>
		/// روش تحویل کارت
		/// </summary>
		public DeliverTypeEnum DeliverType { get; set; }
	
		

		

		/// <summary>
		/// وضعیت درخواست کارت
		/// </summary>
		public CardRequestStatusEnum RequestStatuse { get; set; }
		
		/// <summary>
		/// توزیع کارت در تاریخ
		/// </summary>
        public  DateTime? DistributeCardOnDate { get; set; }


		/// <summary>
		/// تاریخ تحویل کارت به شهروند
		/// </summary>
		public  DateTime?  DeliveredOnDate { get; set; }
		
		/// <summary>
		/// توضیحات تحویل کارت به شهروند
		/// </summary>
		public string DeliveredDescription { get; set; }
		 
		 

		#region درخواست کارت
		/// <summary>
		/// آدرس تحویل کارت به شهروند
		/// </summary>
		public int? DeliveringAddressId { get; set; } 
		public Address DeliveringAddress { get; set; }


		/// <summary>
		/// مرکز تحویل کارت به شهروند
		/// </summary>
		public string DeliveringCenterId { get; set; }
		public OrganizationalUnit DeliveringCenter { get; set; }



		#endregion
		#region چاپ کارت
		/// <summary>
		/// تاریخ انقضای کارت
		/// </summary>
		public DateTime? CardExpirationDate { get; set; }

		/// <summary>
		/// تاریخ فعال سازی کارت
		/// </summary>
		public DateTime? CardActivationDate { get; set; }
		/// <summary>
		/// شماره کارت
		/// </summary>
		public string CardNumber { get; set; } 
		/// <summary>
		/// شماره کارت قبلی کارت
		/// </summary>
		public string PreCardNumber { get; set; }

		/// <summary>
		/// سریال کارت
		/// </summary>
		public string CardSerial { get; set; }
		#endregion

		#region تحویل کارت به شهروند
		/// <summary>
		/// تحویل کارت توسط چه شخصی بوده است
		/// </summary>
		public  int? DeliveredByOperationId { get; set; } 
		public User DeliveredByOperation { get; set; }

		/// <summary>
		/// بارکدپستی
		/// </summary>
		public string BarCode { get; set; }

		public  bool?  IsSetBarCode { get; set; }

		#endregion




		/// <summary>
		/// نوع کارت
		/// </summary>
		public CardRequestTypeEnum? CardRequestType { get; set; }
		
		
		/// <summary>
		/// توضیحات مدیر در مورد این کارت
		/// </summary>
		public string AdminDescription { get; set; }


		public ICollection<CardInfo_Export_Citizen> CardInfo_Export_Citizen { get; set; }




	}

}
