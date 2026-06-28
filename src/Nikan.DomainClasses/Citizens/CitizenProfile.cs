using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.BaseEntity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.Citizens
{
    /// <summary>
    /// پروفایل شهروند
    /// </summary>
    public class CitizenProfile
	{


		[Key, ForeignKey("Citizen")]
		public int CitizenId { get; set; }
		public virtual Citizen Citizen { get; set; }

		[StringLength(100)]
		public string PersonnelCode { get; set; }
		[StringLength(100)]
		public string ShCode { get; set; }
		
		[StringLength(100)]
		public string ShSerial { get; set; }
		public DateTime? ShDate { get; set; }


		/// <summary>
		/// محل صدور شناسنامه
		/// </summary>
		public City ShCity { get; set; }
		public int? ShCityId { get; set; }
		public string ShCitySection { get; set; }
		public string ShNote { get; set; }
		public DateTime? DateOfEmployeement { get; set; }



		public SoldierStateEnum? SoldierState  { get; set; }
		public DateTime? EndOfMilitary { get; set; }

		public CitizenProfileReligionEnum? Religion { get; set; }

		public string VillageOfBirth { get; set; }

		public City CityOfBirth { get; set; }
		public int? CityOfBirthId { get; set; }



		public DateTime? DateOfMarriage { get; set; }

		public string BirthCitySection { get; set; }

		public string BaseEducation { get; set; }

		public string UniversityName { get; set; }

		public string AcademicGrade { get; set; }
		public string AcademicNote { get; set; }


		public DateTime? EndOfEducation { get; set; }

		public EducationStatuesEnum? EducationStatues { get; set; }

		public string InsuranceNumber { get; set; }

		public bool BankCardNumber_Confirmed { get; set; }

		public string BankCardNumber { get; set; } 
        public string ShabaNumber { get; set; }





	}


	 

	 




}
