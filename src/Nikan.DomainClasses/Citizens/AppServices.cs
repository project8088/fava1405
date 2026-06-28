using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.Citizens
{
    /// <summary>
    /// خدمات
    /// </summary>
    public class AppServices
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Key]
		public int  Id { get; set; }

		public string ServiceName { get; set; }
		public AppServices Parent { get; set; }
		public int? ParentId { get; set; }
	 

		[StringLength(500)] 
		public string ServicePicture { get; set; }
		public int Priority { get; set; } 
		public string Description { get; set; } 
		public bool  IsLinkService { get; set; }
		public bool IsNeedAuthenticate { get; set; }

		/// <summary>
		/// نمایش در داشبور شهروندی
		/// </summary>
		public bool IsShowInDashbordCitizen { get; set; }




		public bool OpenInNewWindow { get; set; } 
		public string Link { get; set; } 
		[StringLength(100)]
		public string ParamName1 { get; set; }
		[StringLength(100)]
		public string ParamName2 { get; set; }
		[StringLength(100)]
		public string ParamValue1 { get; set; } 
		[StringLength(100)]
		public string ParamValue2 { get; set; } 
		public bool IsMain { get; set; } 
		public bool IsActive { get; set; } 
		public bool IsDeleted { get; set; }


		public DateTime CreationDate { get; set; }

	    public DateTime? ModifiedOnDate { get; set; }




		[StringLength(100)]
		public string CssClass { get; set; }

		[StringLength(100)]
		public string Icon { get; set; }

		public bool HaveTerms { get; set; }
		public string Terms { get; set; } 


		public User CreatedBy  { get; set; }
		public int? CreatedById { get; set; }




	}


	public class UserLoginTickets
	{


		public int Id { get; set; }
		public User User { get; set; }
		public int  UserId { get; set; }

		
	 

		/// <summary>
		/// شناسه سرویس مبدا
		/// ایجاد کننده تیکت دسترسی
		/// </summary>
		public int SourceId { get; set; }
		
		
		
		/// <summary>
		/// شناسه سرویس مقصد
		/// </summary>
		public AppServices AppServices { get; set; }
		public int AppServicesId { get; set; }



		public Guid UserTicket { get; set; }

		public DateTime CreationDate { get; set; }


		public DateTime?  ReturnDate { get; set; } 
		public string ReturnUrl { get; set; }

		[StringLength(100)]
		public string ParamName1 { get; set; }
		[StringLength(100)]
		public string ParamName2 { get; set; }
		[StringLength(100)]
		public string ParamValue1 { get; set; }
		[StringLength(100)]
		public string ParamValue2 { get; set; }

		public User CreatedByUser { get; set; }
		public int? CreatedByUserId { get; set; }



	}

	public class UserLoginTickets_Archive
	{


		 
		public int Id { get; set; }

		public int TicketId { get; set; }


		/// <summary>
		/// شناسه سرویس مبدا
		/// ایجاد کننده تیکت دسترسی
		/// </summary>
	 
		public int SourceId { get; set; }


		public User User { get; set; }
		public int UserId { get; set; }
		public AppServices AppServices { get; set; }
		public int AppServicesId { get; set; }
		public Guid UserTicket { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime? ReturnDate { get; set; }
		public string ReturnUrl { get; set; }

		[StringLength(100)]
		public string ParamName1 { get; set; }
		[StringLength(100)]
		public string ParamName2 { get; set; }
		[StringLength(100)]
		public string ParamValue1 { get; set; }
		[StringLength(100)]
		public string ParamValue2 { get; set; }


		public User CreatedByUser { get; set; }
		public int? CreatedByUserId { get; set; }



	}

}
