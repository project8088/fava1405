using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.Citizens;
using System;
using System.Collections.Generic;

namespace Nikan.DomainClasses.CitizensCard
{
	public partial class CardInfo_RequestFreeCard
	{

		public string   Id { get; set; }

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
		public DateTime CreationDate { get; set; }

		/// <summary>
		/// این تخفیف توسط چه کاربری ایجاد شده است؟
		/// </summary>
		public int? CreationById { get; set; }
		public User CreationBy { get; set; }



		public int? LastUpdateByUserId { get; set; }
		public User LastUpdateByUser { get; set; }


		public int? GroupId { get; set; }
		public Group Group { get; set; }


		/// <summary>
		/// روش تحویل کارت
		/// </summary>
		public DeliverTypeEnum DeliverType { get; set; }



		/// <summary>
		/// مرکز تحویل کارت
		/// </summary>
		public string CenterID { get; set; }
		public OrganizationalUnit Center { get; set; }

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
		public User AcceptedBy { get; set; }




		public virtual ICollection<CardInfo_RequestFreeCard_Citizens> CardInfo_RequestFreeCard_Citizens { get; set; }


	}


	public class CardInfo_RequestFreeCard_Citizens
	{

		public string Id { get; set; }

		public string RequestFreeCardId { get; set; }
		public CardInfo_RequestFreeCard RequestFreeCard { get; set; }

		public int? CitizenId { get; set; }
		public Citizen Citizen { get; set; } 

		 


	}





}
