using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.Citizens
{

	/// <summary>
	/// هر شهروند یک حساب کاربری باید داشته باشد
	/// اطلاعات شهروند
	/// </summary>
	public class Citizen
	{


		[Key, ForeignKey("User")]
		public int CitizenId { get; set; }
		public virtual User User { get; set; }

		public Guid? UserCode { get; set; }


		//[Key, ForeignKey("Nation")]
		public int? NationId { get; set; }
		public Nationality Nation { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string FatherName { get; set; }
		public int RegisterByServiceId { get; set; }
		public AppServices RegisterByService { get; set; }

		public string IdentityId { get; set; }
		public string NationCode { get; set; }
		public bool Gender { get; set; }
		public MaritalStatusEnum? MariageStatus { get; set; }
		public DateTime? BirthDate { get; set; }
		public string Mobile { get; set; }

		public GradeEnum? EducationLevel { get; set; }
		public int? EducationGroupId { get; set; }
		public EducationGroup EducationGroup { get; set; }
		public EducationStatuesEnum? EducationStatues { get; set; }




		public string EducationField { get; set; }


		public int? JobGroupId { get; set; }
		public JobGroup JobGroup { get; set; }


		public string JobTitle { get; set; }



		public DateTime? Date_SabtConfirm { get; set; }
		
		
		
		/// <summary>
		/// اخرین تاریخ بررسی وضعیت حیات
		/// </summary>
		public DateTime? Date_LastReviewStateLife { get; set; }
		public DateTime CreationDate { get; set; }
		public SabtStatusEnum SabtStatus { get; set; }


		/// <summary>
		/// احرازهویت چهره آیا انجام شده است یا نه ؟
		/// </summary>
		public bool? FaceAuthentication { get; set; }
		public DateTime? FaceAuthenticationOnDate { get; set; }


		public User FaceAuthenticationBy { get; set; }
		public int? FaceAuthenticationById { get; set; }



		public DateTime? LastPictureUploadOnDate { get; set; }
		public PersonalPictureEnum? PersonalPicture_Confirmed { get; set; }
		public string PersonalPicture_DisapprovalReason { get; set; }



		public DateTime? LastUpdateOnDate { get; set; }


		public virtual CitizenProfile CitizenProfile { get; set; }


		public bool HasFamily { get; set; }
		public bool HasCard { get; set; }


		//یک شهروند مجموعه ایی از آدرس ها را دارد
		public virtual ICollection<Address> Address { get; set; }


		//یک شهروند در مجموعه ایی از گروهها عضو است
		public virtual ICollection<GroupsCitizens> GroupsCitizens { get; set; }



		//[NotMapped]
		//public virtual ICollection<Nikan.DomainClasses.CitizensCard.CitizensCard> CitizensCard { get; set; }


		 

		//	public virtual ICollection<CitizenFamily> CitizenFamily { get; set; }

	}


	/// <summary>
	/// شهروندان فوتی
	/// </summary>
	public class CitizensDead
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public DateTime? DeadOnDate { get; set; }
		public string Description { get; set; }
		public int? OperationId { get; set; }
		public User Operation { get; set; }
		public DateTime CreationDate { get; set; }
	}


	public class QueueCheckingCitizensDead
	{
		public int  Id { get; set; }
		public string NationCode { get; set; }
		public DateTime AddOnDate { get; set; } 
		public int Priority { get; set; } 

	}




	/// <summary>
	/// ملیت 
	/// </summary>
	public class Nationality
	{

		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Key]
		public int Id { get; set; } 
		public string  Name { get; set; } 
		public bool  IsActive { get; set; }


	}


	public class CitizensAuthentication
	{
		public int Id { get; set; }

		public Citizen Citizen { get; set; }

		public int CitizenId { get; set; }

		public DateTime OnDate { get; set; }

		public int? AddByUserId { get; set; }

		public User AddByUser { get; set; }


		public string  note1 { get; set; }
		public string note2 { get; set; }
		public string note3 { get; set; }
		 

		public SabtStatusEnum SabtStatus { get; set; }
	}










}
