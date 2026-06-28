using System;
using System.Collections.Generic;

namespace Nikan.ViewModel.Citizens
{
	public class CitizenFeedbackDto
	{ 




		/// <summary>
		/// عنوان بازخورد
		/// </summary>
		public int FeedbackId { get; set; } 
		public string FeedbackDescription { get; set; } 
		/// <summary>
		/// بازخورد برای چه شهروندی بوده است؟
		/// </summary>
		public string  UserCode { get; set; }
	 
	 
	}


	public class CitizenFeedbackInfo
	{

		public int Id { get; set; }



		/// <summary>
		/// عنوان بازخورد
		/// </summary>
		public int FeedbackId { get; set; }
		public string  Feedback { get; set; }
		public string FeedbackDescription { get; set; }

		/// <summary>
		/// بازخورد برای چه شهروندی بوده است؟
		/// </summary>
		public int CitizenId { get; set; }
		public string  Citizen { get; set; }

		public Guid? UserCode { get; set; }



		/// <summary>
		/// توسط چه اپراتوی این بازخورد ثبت شده است
		/// </summary>
		public int? byUserId { get; set; }
		public string byUser { get; set; }

		public Guid? byUserCode { get; set; }
		public DateTime OnDate { get; set; }


	}

	public class PageFeedbackViewModel
	{

		public List<CitizenFeedbackInfo> Items { get; set; }

		public int TotalItems { get; set; }


	}
}
