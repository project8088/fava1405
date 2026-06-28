using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel.Citizens
{
   public class AppServiceInfo
    {

        public int ServiceId { get; set; } 
		public string ServiceName { get; set; }
		public string Parent { get; set; }
		public int? ParentId { get; set; } 
		 

		public string ImageUrl { get; set; }
		public int Priority { get; set; }
		public string Description { get; set; }
		public bool IsLinkService { get; set; }
		public bool IsNeedAuthenticate { get; set; }
		public bool OpenInNewWindow { get; set; }


		public bool IsShowInDashbordCitizen { get; set; }


		public string Link { get; set; }
		 
		public string ParamName1 { get; set; }
	 
		public string ParamName2 { get; set; }
		 
		public string ParamValue1 { get; set; }
		 
		public string ParamValue2 { get; set; }
		public bool IsMain { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }


		public DateTime CreationDate { get; set; }

		public DateTime? ModifiedOnDate { get; set; }




	 
		public string CssClass { get; set; }

	 
		public string Icon { get; set; }

		public bool HaveTerms { get; set; }
		public string Terms { get; set; }


		public string  CreatedBy { get; set; }
		public int? CreatedById { get; set; }


	}

	public class AppServicesDto
	{
		 


		public int? ServiceId { get; set; }


		/// <summary>
		/// نام سرویس
		/// </summary>
		public string ServiceName { get; set; }
		 

		/// <summary>
		/// سرویس والد
		/// </summary>
		public int? ParentId { get; set; }

	 
		/// <summary>
		/// تصویر سرویس
		/// </summary>
		public string ImageUrl { get; set; }


		/// <summary>
		/// الویت
		/// </summary>
		public int Priority { get; set; }

		/// <summary>
		/// توضیحات
		/// </summary>
		public string Description { get; set; }


		/// <summary>
		/// امکان لینک
		/// </summary>
		public bool IsLinkService { get; set; }


		/// <summary>
		/// نیاز به ثبت نام دارد
		/// </summary>
		public bool IsNeedAuthenticate { get; set; }


		/// <summary>
		/// در پنجره جدید باز شود
		/// </summary>
		public bool OpenInNewWindow { get; set; }
		
		
		/// <summary>
		/// لینک
		/// </summary>
		public string Link { get; set; }
		 

		/// <summary>
		/// پارامتر اول
		/// </summary>
		public string ParamName1 { get; set; }
	 

		/// <summary>
		/// پارامتر دومی
		/// </summary>
		public string ParamName2 { get; set; }
	 

		/// <summary>
		/// مقدار پارامتر اول
		/// </summary>
		public string ParamValue1 { get; set; }
		 

		/// <summary>
		/// مقدار پارامتر دوم
		/// </summary>
		public string ParamValue2 { get; set; }
	
		/// <summary>
		/// نمایش در تصویر اصلی
		/// </summary>
		public bool IsMain { get; set; }

	    public bool IsShowInDashbordCitizen { get; set; }



		/// <summary>
		/// فعال است ؟
		/// </summary>
		public bool IsActive { get; set; }
	 
	   
		/// <summary>
		/// css
		/// </summary>
		public string CssClass { get; set; }

	 /// <summary>
	 /// آیکن
	 /// </summary>
		public string Icon { get; set; }


		/// <summary>
		/// دارای قوانین و مقررات
		/// </summary>
		public bool HaveTerms { get; set; }


		/// <summary>
		/// قوانین و مقررات
		/// </summary>
		public string Terms { get; set; }

	 



	}

	public class PagedAppServiceViewModel
	{
		 
		public List<AppServiceInfo> Items { get; set; } 
		public int TotalItems { get; set; }


	}



	public class CreateAccessTicketDto
	{
		public Guid? UserCode { get; set; }
		public string ReturnUrl { get; set; }
		public int ServiceId { get; set; } 
		public int SourceServiceId { get; set; }



	}





}
