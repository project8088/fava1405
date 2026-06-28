using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.Citizens;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nikan.DomainClasses.CitizensCard
{


	/// <summary>
	/// مشخصات دوره توزیع کارت
	/// </summary>
	public class CardInfo_DistributeCard_Courses
	{


		public int Id { get; set; }
		public string GuidId { get; set; }

		public int CourseNumber { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public int? OperationId { get; set; }
		public User Operation { get; set; }

		public string Description { get; set; }

		//صفهای داخل این دوره
		public ICollection<CardInfo_DistributeCard_QueueInfo> CardInfo_DistributeCard_QueueInfo { get; set; }



	}



	/// <summary>
	/// مضخصات صف توزیع کارت
	/// </summary>
	public class  CardInfo_DistributeCard_QueueInfo
	{



		public long Id { get; set; }


		public string GuidId { get; set; }


		public string Name { get; set; }


		public DateTime OnDate { get; set; }

		public string Description { get; set; }

		public bool IsActive { get; set; }

		public bool IsDeleted { get; set; }


		public int IndexOrder { get; set; }
		public int? OperationId { get; set; }
		public User Operation { get; set; }

		public QueueInputTypeEnum QueueInputType { get; set; }

		public int? DeliveredByOperationId { get; set; }
		public User DeliveredByOperation { get; set; }

		public int QueueStatues { get; set; }


		public string DeliveredDescription { get; set; }



		public DateTime? DeliveredOnDate { get; set; }

		public int? CardTypeId { get; set; }

		public bool IsLock { get; set; }

		public int?  PostTownType { get; set; }

		public string DefaultColor { get; set; }


		public CardInfo_DistributeCard_Courses Course  { get; set; }
		public int  CourseId { get; set; }

		public virtual ICollection<CardInfo_DistributeCard> CardInfo_DistributeCard { get; set; }

		public virtual ICollection<CardInfo_DistributeCard_Queue_Groups> GroupsCitizens { get; set; }

	}



	/// <summary>
	/// گروهههای شهروندی داخل صف توزیع کارت
	/// </summary>
	public class CardInfo_DistributeCard_Queue_Groups
	{
		public int Id { get; set; }
		

		public Group Group { get; set; }
		public int GroupId { get; set; }


		public CardInfo_DistributeCard_QueueInfo QueueInfo  { get; set; } 
		public long QueueInfoId { get; set; }

	}


	/// <summary>
	/// صف توزیع کارت شهروندی
	/// </summary>
	public partial class CardInfo_DistributeCard
	{



		public long Id { get; set; }


		public string GuidId { get; set; }

		public long QueueInfoId { get; set; }
		public virtual CardInfo_DistributeCard_QueueInfo QueueInfo { get; set; }


		/// <summary>
		/// در چه تاریخی داخل صف قرار گرفته شده است ؟
		/// </summary>
		public  DateTime  OnDate { get; set; }
		public  bool?  IsPrinted { get; set; }
		
		/// <summary>
		/// به واسطه کدام گروه در این صف قرار گرفته است ؟
		/// </summary>
		public  int?  QueueByGroupId { get; set; }
		public Group QueueByGroup { get; set; }


		/// <summary>
		/// کارت شهروند
		/// </summary>
		public  int  CitizenCardInfoId { get; set; }
		public CitizensCard CitizenCardInfo  { get; set; }
		


		public string PrintCode { get; set; }
		public bool IsInconsistency { get; set; }
		 
	}




}
