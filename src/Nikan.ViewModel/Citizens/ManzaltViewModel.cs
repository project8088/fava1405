using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Nikan.ViewModel.Citizens
{




    public class ManzalatBaseFormDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }


        public bool IsActive { get; set; }


        public bool? Gender { get; set; } 
        public int? MinAge { get; set; } 
        public int? MaxAge { get; set; }

        public int OrderIndex { get; set; }

        public string UploadDescription { get; set; }


    }



    /// <summary>
    /// فرم های قابل ثبت نام شهروند
    /// </summary>
    public class AvailableManzaltForms
	{


        public int Id { get; set; }


		/// <summary>
		/// عنوان طرح منزلت
		/// </summary>
		public string Title { get; set; }


		/// <summary>
		/// توضیحات و شرایط عضویت در این فرم
		/// </summary>
		public string Description { get; set; }
		 
		/// <summary>
		/// آیا شهروند این فرم را تکمیل کرده است ؟
		/// </summary>
		public bool CitizenIsRegisterThisForm { get; set; }

		/// <summary>
		/// شرح بررسی
		/// </summary>
		public string FormResultDescription { get; set; }

		/// <summary>
		/// نتیجه نهایی
		/// </summary>
		public ManzalatFormStatuseEnum? FormResult { get; set; }


		/// <summary>
		/// کدام فرم ؟
		/// </summary>
		public ManzalatFormTypeEnum ManzalatFormType { get; set; }


		 

	}


	public class AvailableManzaltFormsAndAddress
	{

		public bool HasAddress { get; set; }

		public bool HasRegister { get; set; }

		public ManzalatFormStatuseEnum FormStatuse { get; set; }
		public List<AvailableManzaltForms> Forms { get; set; }






	}






	public class ManzalatDto
	{

		public int? Id { get; set; }

        /// <summary>
        /// در چه فرمی ثبت نام کرده است
        /// </summary>
        public int ManzalatBaseFormId { get; set; }



        public bool? Chk_Janbazan_JesmiHarekati_NoWheelChair { get; set; }
        public Typ_JesmiHarekati_NoWheelChairEnum? Typ_Janbazan_JesmiHarekati_NoWheelChair { get; set; }
        public bool? Chk_Janbazan_JesmiHarekati_WheelChair { get; set; }
        public Typ_JesmiHarekati_WheelChairEnum? Typ_Janbazan_JesmiHarekati_WheelChair { get; set; }
        public bool? Chk_Janbazan_Zehni { get; set; }
        public Typ_ZehniEnum? Typ_Janbazan_Zehni { get; set; }
        public bool? Chk_Janbazan_AsabRavan { get; set; }
        public Typ_AsabRavanEnum? Typ_Janbazan_AsabRavan { get; set; }
        public bool? Chk_Janbazan_Binaei { get; set; }
        public Typ_BinaeiEnum? Typ_Janbazan_Binaei { get; set; }
        public bool? Chk_Janbazan_Shenavaei { get; set; }
        public Typ_ShenavaeiEnum? Typ_Janbazan_Shenavaei { get; set; }
        public bool? Chk_Janbazan_Sayer { get; set; }

        

        /// <summary>
        /// جسمی حرکتی غير ويلچري
        /// </summary>
        public bool? Chk_Maloulin_JesmiHarekati_NoWheelChair { get; set; }
        /// <summary>
        /// نوع جسمی حرکتی غير ويلچري
        /// </summary>
        public Typ_JesmiHarekati_NoWheelChairEnum? Typ_Maloulin_JesmiHarekati_NoWheelChair { get; set; }


        /// <summary>
        /// جسمی حرکتی ويلچري
        /// 
        /// </summary>
        public bool? Chk_Maloulin_JesmiHarekati_WheelChair { get; set; }

        /// <summary>
        /// نوع جسمی حرکتی ويلچري
        /// </summary>
        public Typ_JesmiHarekati_WheelChairEnum? Typ_Maloulin_JesmiHarekati_WheelChair { get; set; }

        /// <summary>
        ///  معلولين ذهنی
        ///  
        /// </summary>
        public bool? Chk_Maloulin_Zehni { get; set; }

        /// <summary>
        /// نوع
        ///  معلولين ذهنی
        /// </summary>
        public Typ_ZehniEnum? Typ_Maloulin_Zehni { get; set; }


        /// <summary>
        ///  اعصاب و روان
        /// </summary>
        public bool? Chk_Maloulin_AsabRavan { get; set; }

        /// <summary>
        /// نوع
        ///  اعصاب و روان
        /// </summary>
        public Typ_AsabRavanEnum? Typ_Maloulin_AsabRavan { get; set; }

        /// <summary>
        /// بینایی
        /// </summary>
        public bool? Chk_Maloulin_Binaei { get; set; }

        /// <summary>
        /// نوع
        /// بینایی
        /// </summary>
        public Typ_BinaeiEnum? Typ_Maloulin_Binaei { get; set; }

        /// <summary>
        ///  
        /// شنوایی
        /// </summary>
        public bool? Chk_Maloulin_Shenavaei { get; set; }

        /// <summary>
        /// نوع
        /// شنوایی
        /// </summary>
        public Typ_ShenavaeiEnum? Typ_Maloulin_Shenavaei { get; set; }


        /// <summary>
        /// سایر
        /// </summary>
        public bool? Chk_Maloulin_Sayer { get; set; }


       

        public Typ_ZananSarparastEnum? Typ_ZananSarparast { get; set; }






        public Typ_SpecialDiseasesEnum? Typ_SpecialDiseases { get; set; }
        public string SpecialDiseasesDenyDesc { get; set; }
        public bool? SpecialDiseasesResult { get; set; }








    }





    public class ManzalatViewModel
	{

		public int? Id { get; set; } 
		public int CitizenId { get; set; } 
		public bool? Chk_Maloulin { get; set; }
		public bool? Chk_Maloulin_JesmiHarekati_NoWheelChair { get; set; }
		public Typ_JesmiHarekati_NoWheelChairEnum? Typ_Maloulin_JesmiHarekati_NoWheelChair { get; set; }
		public bool? Chk_Maloulin_JesmiHarekati_WheelChair { get; set; }
		public Typ_JesmiHarekati_WheelChairEnum? Typ_Maloulin_JesmiHarekati_WheelChair { get; set; }
		public bool? Chk_Maloulin_Zehni { get; set; }
		public Typ_ZehniEnum? Typ_Maloulin_Zehni { get; set; }
		public bool? Chk_Maloulin_AsabRavan { get; set; }
		public Typ_AsabRavanEnum? Typ_Maloulin_AsabRavan { get; set; }
		public bool? Chk_Maloulin_Binaei { get; set; }
		public Typ_BinaeiEnum? Typ_Maloulin_Binaei { get; set; }
		public bool? Chk_Maloulin_Shenavaei { get; set; }
		public Typ_ShenavaeiEnum? Typ_Maloulin_Shenavaei { get; set; }
		public bool? Chk_Maloulin_Sayer { get; set; }
		public string Fu_Maloulin { get; set; }
		public bool? Chk_Janbazan { get; set; }
		public bool? Chk_Janbazan_JesmiHarekati_NoWheelChair { get; set; }
		public Typ_JesmiHarekati_NoWheelChairEnum? Typ_Janbazan_JesmiHarekati_NoWheelChair { get; set; }
		public bool? Chk_Janbazan_JesmiHarekati_WheelChair { get; set; }
		public Typ_JesmiHarekati_WheelChairEnum? Typ_Janbazan_JesmiHarekati_WheelChair { get; set; }
		public bool? Chk_Janbazan_Zehni { get; set; }
		public Typ_ZehniEnum? Typ_Janbazan_Zehni { get; set; }
		public bool? Chk_Janbazan_AsabRavan { get; set; }
		public Typ_AsabRavanEnum? Typ_Janbazan_AsabRavan { get; set; }
		public bool? Chk_Janbazan_Binaei { get; set; }
		public Typ_BinaeiEnum? Typ_Janbazan_Binaei { get; set; }
		public bool? Chk_Janbazan_Shenavaei { get; set; }
		public Typ_ShenavaeiEnum? Typ_Janbazan_Shenavaei { get; set; }
		public bool? Chk_Janbazan_Sayer { get; set; }
		public string Fu_Janbazan { get; set; }
		public bool? Chk_ZananSarparast { get; set; }
		public Typ_ZananSarparastEnum? Typ_ZananSarparast { get; set; }
		public string Fu_ZananSarparast { get; set; }
		public bool? Chk_Bazneshasteh { get; set; }
		public string Fu_Bazneshasteh { get; set; }
		public bool? Chk_Salmand { get; set; }
		public string Fu_Salmand { get; set; }
		public string BazneshastehDenyDesc { get; set; }
		public bool? BazneshastehResult { get; set; }
		public string JanbazanDenyDesc { get; set; }
		public bool? JanbazanResult { get; set; }
		public string MaloulinDenyDesc { get; set; }
		public bool? MaloulinResult { get; set; }
		public string SalmandDenyDesc { get; set; }
		public bool? SalmandResult { get; set; }
		public string ZananSarparastDenyDesc { get; set; }
		public bool? ZananSarparastResult { get; set; }

		public string CkeckOperation { get; set; }
		public int? CkeckOperationId { get; set; }

		public ManzalatFormStatuseEnum FormStatuse { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime? CheckDate { get; set; }
		public DateTime? LastUpdate { get; set; }
	}

	public class ManzalatShortCitizenInfo
	{
		/// <summary>
		/// شناسه شهروندی
		/// </summary>
		public int CitizenId { get; set; }




		public Guid? UserCode { get; set; }


		/// <summary>
		///
		/// جنسیت شهروند
		/// true آقا
		/// false خانم
		/// </summary>
		public bool Gender { get; set; }




		/// <summary>
		/// نام شهروند
		/// </summary>
		public string FirstName { get; set; }


		/// <summary>
		/// نام خانوادگی شهروند
		/// </summary>
		public string LastName { get; set; }


		/// <summary>
		/// نام پدر شهروند
		/// </summary>
		public string FatherName { get; set; }



		/// <summary>
		/// ملیت شهروند
		/// 0 ایرانی
		/// 1 غیرایرانی
		/// </summary>
		public int Nationality { get; set; }

		/// <summary>
		/// کد ملی شهروند
		/// </summary>
		public string NationCode { get; set; }


		public string  MariageStatus { get; set; }



		/// <summary>
		/// ثبت نام شده توسط کدام خدمت
		/// </summary>
		public string RegisterByService { get; set; }

		/// <summary>
		/// وضعیت ثبت احوال شما
		/// </summary>
		public SabtStatusEnum SabtStatus { get; set; }


		/// <summary>
		/// شماره موبایل شهروند
		/// </summary>
		public string Mobile { get; set; }



		/// <summary>
		/// تاریخ ثبت نام
		/// </summary>
		public DateTime CreationDate { get; set; }

		/// <summary>
		/// تاریخ ثبت نام شهروند
		/// </summary>
        public DateTime CitizenCreationDate { get; set; }



		public string CkeckOperation { get; set; }
		public int? CkeckOperationId { get; set; }


		/// <summary>
		/// تاریخ تولد شهروند
		/// </summary>
		public DateTime? BirthDate { get; set; }



		/// <summary>
		/// آیا تصویر شهروند بارگذاری شده است ؟
		/// 
		/// </summary>
		public bool PersonalPictureIsUploaded { get; set; }


		public bool DocumentUploaded { get; set; }







		/// <summary>
		/// مسیر تصویر
		/// </summary>
		public string PersonalPictureUrl { get; set; }

		/// <summary>
		/// وضعیت تصویر پرسنلی
		/// </summary>
		public PersonalPictureEnum? PersonalPicture_Confirmed { get; set; }

		public int Age { get; set; }


		 

		public string FullAddress { get; set; }

		public string PostalCode { get; set; }



		public List<string> CitizenGroups { get; set; }



		/// <summary>
		/// وضعیت فرم منزلت
		/// </summary>
		public ManzalatFormStatuseEnum FormStatuse { get; set; }


		public string FormTitle { get; set; }


		public bool InManzalatGroups { get; set; } 


		public string Typ_Maloulin_JesmiHarekati_NoWheelChair { get; set; }
	 
		public string Typ_Maloulin_JesmiHarekati_WheelChair { get; set; }
		 
		public string Typ_Maloulin_Zehni { get; set; }
		 
		public string Typ_Maloulin_AsabRavan { get; set; }
		 
		public string Typ_Maloulin_Binaei { get; set; }
	 
		public string Typ_Maloulin_Shenavaei { get; set; }
	 
	 
	 	public string Typ_Janbazan_JesmiHarekati_NoWheelChair { get; set; }
	 	public string Typ_Janbazan_JesmiHarekati_WheelChair { get; set; }
		 	 	
	 
		 
		public string Typ_ZananSarparast { get; set; }
		public string Typ_Janbazan_Binaei { get; set; }
		public string Typ_Janbazan_Shenavaei { get; set; }
		public string Typ_SpecialDiseases { get; set; } 
		public string Typ_Janbazan_AsabRavan { get; set; } 
		public string Typ_Janbazan_Zehni { get; set; }

	}


	public class PagedManzalatCitizenViewModel
	{
		public int TotalItems { get; set; }
		public List<ManzalatShortCitizenInfo> Citizens { get; set; }

	}


	public class CitizenManzaltForms
	{

        public int ManzalatRegisterId { get; set; }
        /// <summary>
        /// عنوان طرح منزلت
        /// </summary>
        public string Title { get; set; }
		 
		/// <summary>
		/// آیا شهروند این فرم را تکمیل کرده است ؟
		/// </summary>
		public bool CitizenIsRegisterThisForm { get; set; }

		/// <summary>
		/// شرح بررسی
		/// </summary>
		public string FormResultDescription { get; set; }

		/// <summary>
		/// نتیجه نهایی
		/// </summary>
		public bool? FormResult { get; set; }


		/// <summary>
		/// کدام فرم ؟
		/// </summary>
		public ManzalatFormTypeEnum ManzalatFormType { get; set; } 
		public string  Description { get; set; }
		public string FileUrl { get; set; }

         
		

	}




	public class CitizenReviewManzalatForm
	{
		public int CitizenId { get; set; }
		public Guid? UserCode { get; set; }
		public List<CitizenManzaltForms> ManzaltForms { get; set; }
        public List<ManzalatDocumentInfoViewModel> UpoladFiles { get; set; }
        public ManzalatShortCitizenInfo Citizen { get; set; }
	 
     }

    public class CitizenReviewManzalatFormItem
    {
        public int CitizenId { get; set; }
        public int FormBaseId { get; set; }
        public Guid? UserCode { get; set; }
        public CitizenManzaltForms  ManzaltForm { get; set; }
        public ManzalatDocumentInfoViewModel  UpoladFiles { get; set; }
		public bool  HasFiles { get; set; }
		public ManzalatShortCitizenInfo Citizen { get; set; }


    }













    public class ConfirmFormManzalatResult
	{
		public string Name { get; set; }
		public string MobileNumber { get; set; }
		public bool? FormResult { get; set; }
		public bool  HasCard { get; set; } 



	}
	public class ConfirmFormManzalat
	{

		/// <summary>
		/// شناسه شهروند
		/// </summary>
		public string UserCode { get; set; }

		/// <summary>
		/// کدام فرم ؟
		/// </summary>
		public ManzalatFormTypeEnum ManzalatFormType { get; set; }

		/// <summary>
		/// شرح بررسی
		/// </summary>
		public string FormResultDescription { get; set; }

		/// <summary>
		/// نتیجه نهایی
		/// </summary>
		public bool? FormResult { get; set; }


		/// <summary>
		/// آیا برای شهروند پیامک ارسال شود
		/// </summary>
		public bool SendSms { get; set; }

	}

    public class ManzalatDocumentInfoViewModel
    { 
        public int Id { get; set; }
        public string Title { get; set; }

        public string FileName { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; } 
        public DateTime AttachedOnDate { get; set; } 
        public string FilePath { get; set; } 
        public string ThumnailPath { get; set; } 
        public int OwnerId { get; set; } 
        public virtual string Owner { get; set; } 
        public virtual int ManzalatId { get; set; } 
        public virtual string DocumentGroupDescription { get; set; }


        public ManzalatFormStatuseEnum DocumentStatus { get; set; }
        public string Description { get; set; }


    }



}
