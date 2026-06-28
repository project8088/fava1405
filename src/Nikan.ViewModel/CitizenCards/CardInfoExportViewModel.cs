using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.CitizenCards
{
    public class CardInfoExportViewModel
    {
		public int Id { get; set; } 
	 
		public int ExporterByUserId { get; set; }
		public string  ExporterByUser { get; set; }


		/// <summary>
		/// توسط چه کاربری شماره گذاری شده است
		/// </summary>
		public int? ImporterByUserId { get; set; }

		public int? Export_Number { get; set; }
		




		public string ImporterByUser { get; set; }

		/// <summary>
		/// تاریخ ارسال
		/// </summary>
		public DateTime? DateSend { get; set; }

		/// <summary>
		/// تاریخ دریافت
		/// </summary>
		public DateTime? DateReceive { get; set; }




		public bool IsSend { get; set; }

		public bool IsReceive { get; set; }


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

		public int  CountExport { get; set; }


	}
	public class PagedCardInfoExport
	{
		public int TotalItems { get; set; }
		public List<CardInfoExportViewModel> Items { get; set; }
	}

	/// <summary>
	/// ایجاد خروجی صدور کارت جدید
	/// </summary>
	public class NewExportCard
	{



	 
		public int Group_ID { get; set; }
		public int[] GroupIds { get; set; }



		 
		public int? Nationality { get; set; }


		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }


		 
		public int? CardTypeId { get; set; }

		 
		/// <summary>
		/// درخواست های مربوط به داخل شهر
		/// </summary>
		public bool? InTheCity { get; set; }




		public DeliverTypeEnum? DeliverType { get; set; }





	}




}
