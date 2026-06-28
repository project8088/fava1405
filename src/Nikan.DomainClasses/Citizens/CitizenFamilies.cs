using Nikan.Common.GlobalEnum;
using System;

namespace Nikan.DomainClasses.Citizens
{
    /// <summary>
    /// خانواده شهروند
    /// </summary>
    public class CitizenFamily
	{



		public int Id { get; set; }
		 
		public int CitizenId { get; set; }
		public Citizen Citizen { get; set; } 


		/// <summary>
		/// با چه شهروندی نسبت دارد
		/// </summary>
		public int? FamilyCitizenId { get; set; }
	    public Citizen FamilyCitizen { get; set; }
		 
		/// <summary>
		/// چه نسبتی دارد؟
		/// </summary>
		public FamilyRelationshipsEnum FamilyRelation { get; set; }


		public DateTime CreationDate { get; set; } 
		
	
		/// <summary>
		/// آیا شهروند رابطه را تایید کرده است ؟
		/// </summary>
		public bool? AcceptRelative { get; set; }
		public DateTime? AcceptDate { get; set; }

		/// <summary>
		/// آیا مورد تایید است
		/// </summary>
		public bool? Confirm  { get; set; }


		public DateTime? ConfirmDate { get; set; }
		/// <summary>
		/// چه کاربری این نسبت را تایید کرده است 
		///توسط مدیر این نسبت باید تایید شود
		/// </summary>
		public int? ConfirmerUserId { get; set; }
		public User ConfirmerUser  { get; set; }


		public bool? UnderProtection { get; set; }
		public bool? Heirs { get; set; }

		public bool IsDeleted { get; set; }


		public string  ReasonFordeleting { get; set; }


	}









}
