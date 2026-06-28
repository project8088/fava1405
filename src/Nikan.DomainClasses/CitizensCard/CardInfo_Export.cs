using System;
using System.Collections.Generic;

namespace Nikan.DomainClasses.CitizensCard
{
	public class CardInfo_Export
	{


		public int Id { get; set; }

		/// <summary>
		/// خروجی توسط چه کاربری انجام شده است
		/// </summary>
		public int ExporterByUserId { get; set; }
		public User ExporterByUser { get; set; }

		public int? ExportNumber { get; set; }





		/// <summary>
		/// توسط چه کاربری شماره گذاری شده است
		/// </summary>
		public int? ImporterByUserId { get; set; }
		public User ImporterByUser { get; set; }

		/// <summary>
		/// تاریخ ارسال
		/// </summary>
		public DateTime? DateSend { get; set; }

		/// <summary>
		/// تاریخ دریافت
		/// </summary>
		public DateTime? DateReceive { get; set; }

		/// <summary>
		/// تاریخ تایید
		/// </summary>
		public DateTime? ConfirmedData { get; set; }



		/// <summary>
		/// تاریخ ایجاد
		/// </summary>
		public DateTime CreationDate { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Description { get; set; }

		public string CitizenPictureFilePath { get; set; }

		public int CitizenPictureCount { get; set; }



		public ICollection<CardInfo_Export_Citizen> CardInfo_Export_Citizen { get; set; }


	}
	public class CardInfo_Export_Citizen 
	{


		public int Id { get; set; }


		public int CitizenCardInfoId { get; set; }
		public CitizensCard CitizenCardInfo { get; set; }


		public int? ExportCardId { get; set; }
		public CardInfo_Export ExportCard { get; set; } 
		public DateTime CreationDate { get; set; } 
		public bool IsDeleted { get; set; }
		 

	}



}
