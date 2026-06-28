using Microsoft.AspNetCore.Http;
using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.CitizenCards
{

	#region نوع کارت

	/// <summary>
	/// نوع کارت
	/// </summary>
	public class CardTypeViewModel
	{
		public int Id { get; set; }
		/// <summary>
		/// عنوان نوع کارت
		/// </summary>
		public string Title { get; set; }


		/// <summary>
		/// ترتیب نمایش
		/// </summary>
		public int ViewOrder { get; set; }


		/// <summary>
		/// مسیر تصویر پیش نماس تصویر
		/// </summary>
		public string ImageUrl { get; set; }


		public string ViewIcon { get; set; }
		public bool IsActive { get; set; }
		public string Description { get; set; }

		public DateTime CreationDate { get; set; }

		public DateTime? LastUpdateDate { get; set; }

		public string  Operation { get; set; }
		public int OperationId { get; set; }

		public string ExportQuery { get; set; }



	}


	#endregion









	public class OrderCard
	{

		 /// <summary>
		 /// کدام کارت را درخواست کرده است
		 /// </summary>
		public string CardId { get; set; }

		public int CitizenId { get; set; }

		 
		public int? CenterId { get; set; }

		public int AddressId { get; set; }
		public int AddressType { get; set; }
		public int DeliveryType { get; set; }

	 
		public string Address { get; set; }



		 
		public string PostalCode { get; set; }


		public int DiscountId { get; set; }
		public int DiscountGroupId { get; set; }


	 
		public bool PostDeliveryPossibility { get; set; }



		 
		public bool CenterDeliveryPossibility { get; set; }

		 
		public int? CityId { get; set; }
	 
		/// <summary>
		/// true post
		/// false center
		/// </summary>
		public bool TypePossibility { get; set; }

		 
		public string BuyCardDescription { get; set; }

	 
		public string SendCardDescription { get; set; }

		public string CardTypeTitle { get; set; }



	}




	public class CitizensCardDto
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
		public string Citizen { get; set; }


		/// <summary>
		/// چه نوع کارتی خرید کرده است؟
		/// </summary>
		public string CardInfoId { get; set; }
		public string CardInfo { get; set; }


		/// <summary>
		/// تاریخ درخواست
		/// </summary>
		public DateTime RequestDate { get; set; }


		/// <summary>
		/// شناسه تخفیف
		/// </summary>
		public int? DiscountGroupId { get; set; }
	 



		/// <summary>
		/// شناسه تراکنش
		/// </summary>
		public long? TransactionId { get; set; }
		 

		/// <summary>
		/// این درخواست توسط چه شهروندی صورت گرفته شده است
		/// </summary>
		public int? RequestByCitizenId { get; set; }
		 
		/// <summary>
		/// روش تحویل کارت
		/// </summary>
		public DeliverTypeEnum DeliverType { get; set; }



		/// <summary>
		/// وضعیت درخواست کارت
		/// </summary>
		public CardRequestStatusEnum RequestStatuse { get; set; }

		/// <summary>
		/// تاریخ تحویل کارت به شهروند
		/// </summary>
		public DateTime? DeliveredOnDate { get; set; }

		/// <summary>
		/// توضیحات تحویل کارت به شهروند
		/// </summary>
		public string DeliveredDescription { get; set; }









		#region درخواست کارت
		/// <summary>
		/// آدرس تحویل کارت به شهروند
		/// </summary>
		public int? DeliveringAddressId { get; set; }
		 
		/// <summary>
		/// مرکز تحویل کارت به شهروند
		/// </summary>
		public string DeliveringCenterId { get; set; }
		 
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
		public int? DeliveredByOperationId { get; set; }
	 	/// <summary>
		/// بارکدپستی
		/// </summary>
		public string BarCode { get; set; }

		public bool? IsSetBarCode { get; set; }

		#endregion




		/// <summary>
		/// نوع کارت
		/// </summary>
		public CardRequestTypeEnum? CardRequestType { get; set; }


		/// <summary>
		/// توضیحات مدیر در مورد این کارت
		/// </summary>
		public string AdminDescription { get; set; }



	}
 
	
	public class CheckCanOrderCardDto
	{
		public string CardInfoId { get; set; }
		public  int CitizenId { get; set; }
		 

	}
	public class OrderCardItem
	{
		public string CardInfoId { get; set; }
		public string CardTypeTitle { get; set; }

		public string RequestCode { get; set; }

		public string Description { get; set; }
		public int CardCost { get; set; }
		public int DoubleCardCost { get; set; }
		public int PostalCostInCity { get; set; }
		public int PostalCostOutCity { get; set; }


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

		public int VATForCardCost { get; set; }
		public int VATForPost { get; set; }



		public int DeliveringAddressId { get; set; }


		//هزینه نهایی کارت
		public int TotalCardCost { get; set; }
		/// <summary>
		/// مبلغ تخفیف کارت
		/// </summary>
		public int TotalCardPercent { get; set; }

		/// <summary>
		/// هزینه نهایی پست
		/// </summary>
		public int TotalPostCost { get; set; }
		/// <summary>
		/// مبلغ تخفیف ارسال
		/// </summary>
		public int TotalPostPercent { get; set; }

		//جمع ارزش افزدوه
		public int TotalVAT { get; set; }

		public int TotalCost { get; set; }

		/// <summary>
		/// مبلغ کل + ارزش افزوده
		/// </summary>
		public int TotalCostWithVAT { get; set; }


		public string TotalCostStr { get; set; }

		public string AddressSend { get; set; }

		public int? DiscountId { get; set; }
		public int? DiscountGroupId { get; set; }
		public bool PostDeliveryPossibility { get; set; }
		public bool CenterDeliveryPossibility { get; set; }

		public List<BaseDataModel> CenterList { get; set; }
		public int? CenterId { get; set; }


	}

	/// <summary>
	/// اطلاعات ادرس دریافت کارت
	/// </summary>
	public class OrderAddressDto
	{

		public int  AddressId { get; set; }

		public string CardInfoId { get; set; }

	}




	/// <summary>
	/// اطلاعات کامل خرید کارت
	/// </summary>
	public class CitizensCardInfo
	{



		public int Id { get; set; }



		public int CitizenCardInfoId { get; set; }





		/// <summary>
		/// کد درخواست
		/// </summary>
		public string RequestCode { get; set; }


		/// <summary>
		/// این کارت برای چه شهروندی است؟
		/// </summary>
		public int CitizenId { get; set; }
		public string Citizen { get; set; }
		public Guid? UserCode { get; set; }


		public string FirstName { get; set; }
		
	   public string Gender { get; set; }
		
		
		
		
		public string LastName { get; set; }
		public string Mobile { get; set; }


		public SabtStatusEnum SabtStatus { get; set; }

		public PersonalPictureEnum? PersonalPicture_Confirmed { get; set; }



		/// <summary>
		/// کد ملی شهروند
		/// </summary>
		public string NationCode { get; set; }



		/// <summary>
		/// چه نوع کارتی خرید کرده است؟
		/// </summary>
		public string CardInfoId { get; set; }
		public string CardTitle { get; set; }
		public int  CardTypeId { get; set; }




		public IEnumerable<string> Groups { get; set; }

		public string ImageUrl { get; set; }



		/// <summary>
		/// تاریخ درخواست
		/// </summary>
		public DateTime RequestDate { get; set; }


		/// <summary>
		/// شناسه تخفیف
		/// </summary>
		public int? DiscountGroupId { get; set; }
		public string DiscountGroup { get; set; }

       public string DiscountTitle { get; set; }



	   public bool 	HasDiscount { get; set; }




		/// <summary>
		/// این درخواست توسط چه شهروندی صورت گرفته شده است
		/// </summary>
		public int? RequestByCitizenId { get; set; }
		public string RequestByCitizen { get; set; }

		/// <summary>
		/// روش تحویل کارت
		/// </summary>
		public DeliverTypeEnum DeliverType { get; set; }



		/// <summary>
		/// وضعیت درخواست کارت
		/// </summary>
		public CardRequestStatusEnum RequestStatuse { get; set; }

		/// <summary>
		/// تاریخ تحویل کارت به شهروند
		/// </summary>
		public DateTime? DeliveredOnDate { get; set; }

		/// <summary>
		/// توضیحات تحویل کارت به شهروند
		/// </summary>
		public string DeliveredDescription { get; set; }


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


	 
	 
		public decimal VATForCardCost { get; set; }
		public decimal VATForPost { get; set; }


		 
		

		#region درخواست کارت
		/// <summary>
		/// آدرس تحویل کارت به شهروند
		/// </summary>
		public int? DeliveringAddressId { get; set; }
		public string DeliveringAddress { get; set; }






		/// <summary>
		/// مرکز تحویل کارت به شهروند
		/// </summary>
		public string DeliveringCenterId { get; set; }
		public string DeliveringCenter { get; set; }


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
		public int? DeliveredByOperationId { get; set; }
		public string DeliveredByOperation { get; set; }
		public Guid? DeliveredByUserCode{ get; set; }



		/// <summary>
		/// بارکدپستی
		/// </summary>
		public string BarCode { get; set; }

		public bool? IsSetBarCode { get; set; }

		#endregion




		/// <summary>
		/// نوع کارت
		/// </summary>
		public CardRequestTypeEnum? CardRequestType { get; set; }


		/// <summary>
		/// توضیحات مدیر در مورد این کارت
		/// </summary>
		public string AdminDescription { get; set; }
		
		public long? TransactionId { get; set; }

		public string TransactionBankReferenceId { get; set; }


		public long? AmountTransaction { get; set; }





		 
		public AddressTypeEnum AddressType { get; set; }
		 

		public string  City { get; set; }
		 

		public int? Region { get; set; }
		public string Street { get; set; }
		public string Alley { get; set; }
		public string PostalCode { get; set; }
		public string Plaque { get; set; }
		public string FullAddress { get; set; } 
		public string Phone { get; set; }














		public virtual DateTime? TransactionOnDate { get; set; }


	}

	/// <summary>
	/// فرمت خروجی صدور کارت جهت چاپ کارت
	/// </summary>
	public class  FormatExportCardInfo
	{


		public int Id { get; set; }



		public int CitizenCardInfoId { get; set; }





		/// <summary>
		/// کد درخواست
		/// </summary>
		public string RequestCode { get; set; }


		/// <summary>
		/// این کارت برای چه شهروندی است؟
		/// </summary>
		public int CitizenId { get; set; }


		/// <summary>
		/// کد ملی شهروند
		/// </summary>
		public string NationCode { get; set; }



		public string CitizenFirstName { get; set; } 
		public string CitizenLastName { get; set; }

		public string CitizenFullName { get; set; }


		public string ExpaireDate { get; set; }

		public string ManzelatStatus { get; set; }

		public string IsSalmand { get; set; }

		public string IsMaloul { get; set; }



		public string IsBimar { get; set; }
		public string IsMadar { get; set; }



		public string IsZanSarperst { get; set; }

		public string IsJanbaz { get; set; }


		public string IsBazneshasteh { get; set; } 
	    public string TotalResult { get; set; }





	}

	public class PagedCitizensCardViewModel
	{
		public int TotalItems { get; set; }
		public List<CitizensCardInfo> Items { get; set; }

	}




	public class PagedCitizensCardForSendPrintCard
	{
		public int TotalItems { get; set; }
		public List<FormatExportCardInfo> Items { get; set; }

	}


	public class ImportExcelCardNumber
	{
		public string NationCode { get; set; }
		public string CardNumber { get; set; }
		public bool  CardCancellation { get; set; }
		public string CardSerial { get; set; }

		public DateTime? CardActivationDate { get; set; }

		public DateTime? CardExpirationDate { get; set; }

	}
	public class ImportExcelFileCardNumber
	{
		public int ExportId { get; set; }
		public string FileUrl { get; set; }
		public IFormFile file { get; set; }

	}


	/// <summary>
	/// اطلاعات خرید کارت
	/// </summary>
	public class OrerCardDto
	{

		public int DeliveringAddressId { get; set; }
		public string CardInfoId { get; set; }



		public int CitizenId { get; set; }
	
		public long TransactionId { get; set; }


		public int? DiscountGroupId { get; set; }

	}



	/// <summary>
	/// برگشت کارت
	/// </summary>
	public class BackCardDto
	{
		public int Id { get; set; }
		public string CenterId { get; set; }
		public string Reason { get; set; } 
		public bool  SendSms { get; set; } 
		public DateTime BackCardOnDate { get; set; } 
	}

	/// <summary>
	/// تحویل کارت
	/// </summary>
	public class DeliveredCardDto
	{
		public int Id { get; set; } 
		public string DeliveredDescription { get; set; } 
		public DateTime DeliveredOnDate { get; set; }
	}


	/// <summary>
	/// ابطال کارت
	/// </summary>
	public class CardCancellationDto
	{

		public int Id { get; set; }
		public int CardCancellationType { get; set; }
		public string CardCancellationItemId { get; set; }
		public bool SendSms { get; set; }
		public string Description { get; set; }

		public DateTime CardCancellationOnDate { get; set; }



	}

	/// <summary>
	/// مضخصات کارتهای داخل صف توزیع کارت
	/// </summary>
	public class CitizenCardsInQueueInfo
	{


		public int RequestId { get; set; }

		public long QueueId { get; set; }


		public int CourseId { get; set; }


		public long PrintId { get; set; }


		/// <summary>
		/// کد درخواست
		/// </summary>
		public string RequestCode { get; set; }

		/// <summary>
		/// شماره دوره
		/// </summary>
       public int CourseNumber { get; set; }

		/// <summary>
		/// این کارت برای چه شهروندی است؟
		/// </summary>
		public int CitizenId { get; set; }
		public string Citizen { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Mobile { get; set; }
		public string QueueName { get; set; }

		public SabtStatusEnum SabtStatus { get; set; }

		public PersonalPictureEnum? PersonalPicture_Confirmed { get; set; }



		/// <summary>
		/// کد ملی شهروند
		/// </summary>
		public string NationCode { get; set; }



		/// <summary>
		/// چه نوع کارتی خرید کرده است؟
		/// </summary>
		public string CardInfoId { get; set; }
		public string CardTitle { get; set; }

		public string City { get; set; } 
		public int? Region { get; set; }
		public string Street { get; set; }
		public string Alley { get; set; }
		public string PostalCode { get; set; }
		public string Plaque { get; set; }
		public string FullAddress { get; set; }
		public string Phone { get; set; }

		public string ImageUrl { get; set; }



		/// <summary>
		/// تاریخ درخواست
		/// </summary>
		public DateTime RequestDate { get; set; }


		public DateTime QueueOnDate { get; set; }

		/// <summary>
		/// شناسه تخفیف
		/// </summary>
		public int? DiscountGroupId { get; set; }
		public string DiscountGroup { get; set; }




		/// <summary>
		/// این درخواست توسط چه شهروندی صورت گرفته شده است
		/// </summary>
		public int? RequestByCitizenId { get; set; }
		public string RequestByCitizen { get; set; }

		/// <summary>
		/// روش تحویل کارت
		/// </summary>
		public DeliverTypeEnum DeliverType { get; set; }



		/// <summary>
		/// وضعیت درخواست کارت
		/// </summary>
		public CardRequestStatusEnum RequestStatuse { get; set; }

		/// <summary>
		/// تاریخ تحویل کارت به شهروند
		/// </summary>
		public DateTime? DeliveredOnDate { get; set; }

		/// <summary>
		/// توضیحات تحویل کارت به شهروند
		/// </summary>
		public string DeliveredDescription { get; set; }


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




		public decimal VATForCardCost { get; set; }
		public decimal VATForPost { get; set; }





		#region درخواست کارت
		/// <summary>
		/// آدرس تحویل کارت به شهروند
		/// </summary>
		public int? DeliveringAddressId { get; set; }
		public string DeliveringAddress { get; set; }


		/// <summary>
		/// مرکز تحویل کارت به شهروند
		/// </summary>
		public string DeliveringCenterId { get; set; }
		public string DeliveringCenter { get; set; }


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
		public int? DeliveredByOperationId { get; set; }
		public string DeliveredByOperation { get; set; }

		/// <summary>
		/// بارکدپستی
		/// </summary>
		public string BarCode { get; set; }

		public bool? IsSetBarCode { get; set; }

		#endregion





		/// <summary>
		/// نوع کارت
		/// </summary>
		public CardRequestTypeEnum? CardRequestType { get; set; }


		/// <summary>
		/// توضیحات مدیر در مورد این کارت
		/// </summary>
		public string AdminDescription { get; set; }

		public long? TransactionId { get; set; }

		public string TransactionBankReferenceId { get; set; }


		public long? AmountTransaction { get; set; }


		public virtual DateTime? TransactionOnDate { get; set; }


	}

	public class PagedCitizenCardsInQueueInfo
	{
		public int TotalItems { get; set; }
		public List<CitizenCardsInQueueInfo> Items { get; set; }

	}

	public class PrintqueueInfoViewModel
	{
		public int? GroupId { get; set; }
		public long QueueId { get; set; }
		public string QueueName { get; set; }
		public string DefaultColor { get; set; }



	}

	public class SendCardToQueueDto
	{

		/// <summary>
		/// شناسه کارت
		/// </summary>
		public int Id { get; set; } 

		public int  CitizenId { get; set; }
		public int  DeliverType { get; set; }

		public int CardTypeId { get; set; }

        public int CourseId { get; set; }

		public string DeliveringCenterId { get; set; }


	}
	
	
	public class PrintForPostViewModel
	{
		public long Id { get; set; }

		public string RowId { get; set; }

		public string NationalCode { get; set; }


		public int CityId { get; set; }

		public string City { get; set; }

		public string Location { get; set; }


		public string Address { get; set; }


		public string Name { get; set; }


		public string ZipCode { get; set; }


		public string CellNumber { get; set; }


		public string Mobile { get; set; }


		public string QueueName { get; set; }



		public string GroupName { get; set; }

		public string CardNumber { get; set; }

		public string BarCode { get; set; }




	}

}
