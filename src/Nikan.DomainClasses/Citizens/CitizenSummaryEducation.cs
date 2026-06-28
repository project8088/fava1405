using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.Job;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.Citizens
{

	/// <summary>
	/// سوابق تحصیلی شهروند
	/// </summary>
    public class CitizenSummaryEducation
	{

		public int Id { get; set; } 
		[StringLength(50)]
		public string University { get; set; }

		//---------------------------------------
		[StringLength(50)]
		public string ThesisTitle { get; set; }

		//------------------------------------------------------------------
		public DateTime? DateOfStart { get; set; }
		//------------------------------------------------------------------
		public DateTime? DateOfEnd { get; set; }


		public string Average { get; set; }
		//------------------------------------------------------------------
		[StringLength(1000)]
		public string Note { get; set; }

		#region ForeignKey

		//------------------------------------------------------------------
		[StringLength(50)]
		public string UniversityLocation { get; set; }
		//------------------------------------------------------------------
		[StringLength(50)]
		public string EduOrientation { get; set; }
		//------------------------------------------------------------------
		public virtual Major Major { get; set; }
		public int? MajorId { get; set; }
		//------------------------------------------------------------------
		public virtual GradeEnum Grade { get; set; }

		//------------------------------------------------------------------
		public virtual TypeUniversity TypeUniversity { get; set; }

		[ForeignKey("Citizen")]
		public int CitizenId { get; set; }

		public virtual Citizen Citizen { get; set; }

		#endregion
	}






}
