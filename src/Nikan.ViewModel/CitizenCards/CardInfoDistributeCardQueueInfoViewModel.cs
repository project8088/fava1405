using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.CitizensCard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.CitizenCards
{

	/// <summary>
	/// اضافه کردن دوره جدید
	/// </summary>
	public class CardInfo_DistributeCard_CoursesDto
	{
		 

		public int CourseNumber { get; set; }  
		public string Description { get; set; }

	}


	/// <summary>
	/// دوره توزیع کارت
	/// </summary>
	public class CardInfo_DistributeCard_CoursesViewModel
	{
		public int Id { get; set; }

		public int CourseNumber { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public int? OperationId { get; set; }
		public string  User { get; set; }

		public string Description { get; set; }

		public int CardQueueCount { get; set; }


		public bool  IsColsed { get; set; }


	}




	public class PagedCardInfoDistributeCardCoursesViewModel
	{

		public List<CardInfo_DistributeCard_CoursesViewModel> Items { get; set; }
		public int TotalItems { get; set; }


	}




	public class CardDistributionQueueDto
	{ 
		public long? Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string DefaultColor { get; set; }
		public bool IsActive { get; set; }
		public bool ISLock { get; set; }

		/// <summary>
		/// شناسه دوره
		/// </summary>
		public int CoursesId { get; set; }
		public int IndexOrder { get; set; }

		public int CardTypeId { get; set; }


		/// <summary>
		///لیست گروهها
		/// </summary>
		public List<int> GroupIds { get; set; }


	}

	public class CardInfo_DistributeCard_QueueInfoViewModel
	{


		public long Id { get; set; }

		public string Name { get; set; }

		public string GuidId { get; set; }
		public DateTime OnDate { get; set; }

		public string Description { get; set; }

		public bool IsActive { get; set; }

		public bool IsDeleted { get; set; }


		public int IndexOrder { get; set; }
		public int? OperationId { get; set; }
		public string  UserOperation { get; set; }

		public QueueInputTypeEnum QueueInputType { get; set; }

		public int? DeliveredByOperationId { get; set; }
		public string  DeliveredByOperation { get; set; }

		public int QueueStatues { get; set; }


		public string DeliveredDescription { get; set; }


		public DateTime? DeliveredOnDate { get; set; }

		public int? CardTypeId { get; set; }

		public bool IsLock { get; set; }

		public int PostTownType { get; set; }

		public string DefaultColor { get; set; }


		//public CardInfo_DistributeCard_Courses Course { get; set; }
		public int CourseId { get; set; }

		public int CardCount { get; set; }
		/// <summary>
		///لیست گروهها
		/// </summary>
		public List<int> GroupIds { get; set; }

		public IEnumerable<string> AllGroups { get; set; }
		public IEnumerable<BaseDataModel> Groups { get; set; }



	}

	/// <summary>
	/// اطلاعات کوتاه صف توزیع کارت
	/// </summary>
	public class DistributeCardQueueShortInfoViewModel
	{


		public long Id { get; set; }

		public string Name { get; set; } 
        public string DefaultColor { get; set; } 
		public bool IsActive { get; set; } 
		public int IndexOrder { get; set; } 
		public QueueInputTypeEnum QueueInputType { get; set; } 
	   
		public int? CardTypeId { get; set; }

		public bool IsLock { get; set; } 
	 
		public int CourseId { get; set; }

		public int CardCount { get; set; }
		  

	}




	public class PagedCardInfoDistributeCardQueue 
	{

		public List<CardInfo_DistributeCard_QueueInfoViewModel> Items { get; set; }
		public int TotalItems { get; set; } 
	}



	public class MiniSearchCitizensViewModel
	{

		public SearchCitizensTypeEnum? SearchCitizensType { get; set; }

	  
		public string Value { get; set; } 
		public int Count { get; set; } 
		public int CoursesId { get; set; }
		public int CoursesNumber { get; set; }

	}




	public class DeliveryQueueToOperatorViewModel
	{

		public string NationalCode { get; set; }


		 

		public long QueueId { get; set; } 
		 
		public string Description { get; set; }

		 

		public bool IsSendPost { get; set; }




	}






}
