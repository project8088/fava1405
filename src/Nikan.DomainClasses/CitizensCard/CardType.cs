using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.CitizensCard
{
    /// <summary>
    /// نوع کارت
    /// </summary>
    public class CardType
	{

		 

		[Key]
		public int Id { get; set; } 
		/// <summary>
		/// عنوان نوع کارت
		/// </summary>
	 	public string  Title { get; set; }


		/// <summary>
		/// ترتیب نمایش
		/// </summary>
		public int  ViewOrder { get; set; } 


		/// <summary>
		/// مسیر تصویر پیش نماس تصویر
		/// </summary>
	    public string  ImageUrl { get; set; } 


		public string ViewIcon { get; set; }
		public bool IsActive { get; set; }
		public string Description { get; set; }

		public DateTime CreationDate { get; set; }

		public DateTime? LastUpdateDate { get; set; } 
		public string ExportQuery { get; set; }


		public virtual ICollection<CardInfo_Discount>  CardInfoDiscounts { get; set; }




	}

}
