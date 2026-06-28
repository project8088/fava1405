using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.CitizenCards
{
	public partial class  RequestFreeCardInfoViewModel
	{

		public string  Id { get; set; }

		/// <summary>
		/// عنوان تخفیف
		/// </summary>
		public string DiscountTitle { get; set; }


		/// <summary>
		/// این تخفیف برای چه کارتی تعریف شده است
		/// </summary>
		public int CardTypeId { get; set; }
		public string  CardType { get; set; }





		/// <summary>
		/// ایجاد تخفیف در چه تاریخی صورت گرفته است
		/// </summary>
		public DateTime CreationDate { get; set; }

		/// <summary>
		/// این تخفیف توسط چه کاربری ایجاد شده است؟
		/// </summary>
		public int? CreationById { get; set; }
		public string CreationBy { get; set; }



		public int? LastUpdateByUserId { get; set; }
		public string LastUpdateByUser { get; set; }


		public int? GroupId { get; set; }
		public string Group { get; set; }


		/// <summary>
		/// روش تحویل کارت
		/// </summary>
		public DeliverTypeEnum DeliverType { get; set; }



		/// <summary>
		/// مرکز تحویل کارت
		/// </summary>
		public string CenterID { get; set; }
		public string Center { get; set; }

		/// <summary>
		/// وضعیت بررسی تصویر پرسنلی
		/// </summary>
		public ImagerReviewStatusFormFreeCardEnum ImagerReviewStatusFormFreeCard { get; set; }



		public string FreeCardApplicantOrganization { get; set; }

		public string LetterNumber { get; set; }
		public string Description { get; set; }


		public string AttachmentGroup { get; set; }


		public bool? Accepted { get; set; }

		public int? AcceptedById { get; set; }
		public string AcceptedBy { get; set; }
		 

	}


	public class PagedRequestFreeCard 
	{
		public int TotalItems { get; set; }
		public List<RequestFreeCardInfoViewModel> Items { get; set; }

	}
	 
	public partial class RequestFreeCardDto
	{

		public string Id { get; set; }

		/// <summary>
		/// عنوان تخفیف
		/// </summary>
		public string DiscountTitle { get; set; }

 
		public int CardTypeId { get; set; }
	 

	   
		 
		public int? GroupId { get; set; }
	 

		/// <summary>
		/// روش تحویل کارت
		/// </summary>
		public DeliverTypeEnum DeliverType { get; set; }


 
		public string CenterID { get; set; }
		 

		/// <summary>
		/// وضعیت بررسی تصویر پرسنلی
		/// </summary>
		public ImagerReviewStatusFormFreeCardEnum ImagerReviewStatusFormFreeCard { get; set; }



		public string FreeCardApplicantOrganization { get; set; }

		public string LetterNumber { get; set; }
		public string Description { get; set; }


		public string AttachmentGroup { get; set; }


	  


	}


	public class RequestFreeCardCitizensViewModel
	{


		public string  Id { get; set; }

		/// <summary>
		/// شناسه شهروندی
		/// </summary>
		public int CitizenId { get; set; }




		public Guid? UserCode { get; set; }

		/// <summary>
		///
		/// جنسیت شهروند
		/// true آقا
		/// false خانم
		/// </summary>
		public bool Gender { get; set; }




		/// <summary>
		/// نام شهروند
		/// </summary>
		public string Citizen { get; set; }

 





		/// <summary>
		/// ملیت شهروند
		/// 0 ایرانی
		/// 1 غیرایرانی
		/// </summary>
		public int Nationality { get; set; }

		/// <summary>
		/// کد ملی شهروند
		/// </summary>
		public string NationCode { get; set; }



		/// <summary>
		/// وضعیت ثبت احوال شما
		/// </summary>
		public SabtStatusEnum SabtStatus { get; set; }



		/// <summary>
		/// تاریخ ثبت نام
		/// </summary>
		public DateTime CreationDate { get; set; }



		/// <summary>
		/// تاریخ تولد شهروند
		/// </summary>
		public DateTime? BirthDate { get; set; }

		public bool HasCard { get; set; }


		/// <summary>
		/// آیا تصویر شهروند بارگذاری شده است ؟
		/// 
		/// </summary>
		public bool PersonalPictureIsUploaded { get; set; }


		/// <summary>
		/// مسیر تصویر
		/// </summary>
		public string PersonalPictureUrl { get; set; }

		/// <summary>
		/// وضعیت تصویر پرسنلی
		/// </summary>
		public PersonalPictureEnum? PersonalPicture_Confirmed { get; set; }

	}

	public class PagedRequestFreeCardCitizens
	{
		public int TotalItems { get; set; }
		public List<RequestFreeCardCitizensViewModel> Items { get; set; }

	}
}
