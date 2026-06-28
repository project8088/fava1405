using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.Citizens
{
    /// <summary>
    /// ثبت بازخوردهای شهروندی
    /// </summary>
    public class CitizenFeedback
	{

		public int Id { get; set; } 
		/// <summary>
		/// عنوان بازخورد
		/// </summary>
		public int FeedbackId { get; set; }
		public virtual Feedback Feedback { get; set; } 
		public string FeedbackDescription { get; set; } 

		/// <summary>
		/// بازخورد برای چه شهروندی بوده است؟
		/// </summary>
		public int CitizenId { get; set; }
		public virtual Citizen Citizen { get; set; } 

		
		
	    [ForeignKey("OperationId")]
		public virtual User Operation { get; set; } 

		/// <summary>
		/// توسط چه اپراتوی این بازخورد ثبت شده است
		/// </summary>
		public int? OperationId { get; set; } 

		public bool IsDeleted { get; set; }
		public DateTime OnDate { get; set; } 



	}


	/// <summary>
	/// لیست عناوین بازخورد
	/// </summary>
	public class Feedback
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Key]
		public int Id { get; set; }
		public string FeedbackTitle { get; set; }

		public bool IsDeleted { get; set; }
	}





	public class Card
	{
		public Card()
		{
			Sides = new Collection<Side>();
			 
		}

		[Key]
		[Required]
		public virtual int CardId { get; set; }

		[Required]
		public virtual Stage Stage { get; set; }

		[Required]
		[ForeignKey("CardId")]
		public virtual ICollection<Side> Sides { get; set; }
	}

	public class Side
	{
		public Side()
		{
			Stage = Stage.ONE;
		}

		[Key]
		[Required]
		public virtual int SideId { get; set; }

		[Required]
		public virtual Stage Stage { get; set; }

		[Required]
		public int CardId { get; set; }

		[ForeignKey("CardId")]
		public virtual Card Card { get; set; }

	}

	public class Stage
	{
		// Zero
		public static readonly Stage ONE = new Stage(new TimeSpan(0, 0, 0), "ONE");
		// Ten seconds
		public static readonly Stage TWO = new Stage(new TimeSpan(0, 0, 10), "TWO");

		public static IEnumerable<Stage> Values
		{
			get
			{
				yield return ONE;
				yield return TWO;
			}

		}

		public int StageId { get; set; }
		private readonly TimeSpan span;
		public string Title { get; set; }

		Stage(TimeSpan span, string title)
		{
			this.span = span;
			this.Title = title;
		}

		public TimeSpan Span { get { return span; } }
	}
}
