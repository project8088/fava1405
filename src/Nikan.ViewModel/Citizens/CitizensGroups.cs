using Nikan.ViewModel.Group;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Citizens
{

	public class ShortGroupsCitizensInfo
	{

		public int Id { get; set; }

		/// <summary>
		/// شناسه گروه
		/// </summary>
		public int GroupId { get; set; }

		/// <summary>
		/// نام گروه
		/// </summary>
		public string Group { get; set; }


		/// <summary>
		/// شناسه شهروند
		/// </summary>
		public int CitizenId { get; set; }

	 
		/// <summary>
		/// تاریخ اضافه کردن شهروند به گروه
		/// </summary>
		public DateTime? CreationDate { get; set; }

		/// <summary>
		/// آیا جز گرووهای شهرداری است
		/// </summary>
		public bool? MunicipalPersonnelGroup { get; set; }


	}



	/// <summary>
	/// اطلاعات گروههای که شهروند در آن  عضو است
	/// </summary>
	public class GroupsCitizensInfo
	{ 

		public int Id { get; set; }
		
		/// <summary>
		/// شناسه گروه
		/// </summary>
		public int GroupId { get; set; }
		
		/// <summary>
		/// نام گروه
		/// </summary>
		public string  Group { get; set; }
		
		

		/// <summary>
		/// شناسه شهروند
		/// </summary>
		public int CitizenId { get; set; }
		
		/// <summary>
		/// شهروند
		/// </summary>
		public string Citizen { get; set; }
		
		
		/// <summary>
		/// تاریخ اضافه کردن شهروند به گروه
		/// </summary>
		public DateTime? CreationDate { get; set; }
		
		
		/// <summary>
		/// شناسه کاربر تایید کننده
		/// </summary>
		public int? AddByUserId { get; set; }

		/// <summary>
		/// آیا جز گرووهای شهرداری است
		/// </summary>
		public bool? MunicipalPersonnelGroup { get; set; }

		/// <summary>
		/// کاربر تایید کننده
		/// </summary>
		public string AddByUser { get; set; }

	}
	

    public class CitizenGroupsAndQueues
    {
        /// <summary>
        /// آیا شهروند ثبت نام کرده است ؟
        /// </summary>
        public bool IsRegistered { get; set; }


        /// <summary>
        /// لیست گروههایی که داخل آن عضو است
        /// </summary>
        public List<GroupsCitizensInfo> GroupList { get; set; }
        /// <summary>
        /// لیست صف هایی که داخل آن عضو است
        /// </summary>
        public List<CitizensQueueInfo>  QueueList { get; set; }

    }



	public class GroupsCitizensDto
	{
		public int GroupId { get; set; }

		public string UserCode { get; set; }

		public string NationCode { get; set; }

		public DateTime? ExpireDate { get; set; }
	}
	public class PagedCitizenGroupsViewModel
	{
		public int TotalItems { get; set; }
		public List<GroupsCitizensInfo> Items { get; set; }

	}




}
