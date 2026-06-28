using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.Citizens
{

    public class ManzalatBaseForm
    {
         
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int FormId { get; set; }


        public string  Title { get; set; }

        public string Description  { get; set; }


        public bool  IsActive { get; set; }


        public bool? Gender { get; set; }



        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }


        public int   OrderIndex { get; set; }

        public string UploadDescription { get; set; }

    }


    /// <summary>
    /// طرح منزلت
    /// </summary>
    public class Manzalat
	{



		public int Id { get; set; }

        public int? ManzalatBaseFormId { get; set; }
		public ManzalatBaseForm ManzalatBaseForm { get; set; }



		public int CitizenId { get; set; }
		public Citizen Citizen { get; set; }



        #region فرم معلولین
        //public bool? Chk_Maloulin { get; set; }

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
        //public string Fu_Maloulin { get; set; }

        #endregion
        #region جانبازان


       // public bool? Chk_Janbazan { get; set; }
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
        //public string Fu_Janbazan { get; set; }
        #endregion
        #region زنان سرپرست خانواده

       //  public bool? Chk_ZananSarparast { get; set; }
		public Typ_ZananSarparastEnum? Typ_ZananSarparast { get; set; }
        //public string Fu_ZananSarparast { get; set; }

        #endregion

        #region بازنشستگان
       // public bool? Chk_Bazneshasteh { get; set; }
        //public string Fu_Bazneshasteh { get; set; }
        #endregion

        #region سالمندان
        // public bool? Chk_Salmand { get; set; }
        //public string Fu_Salmand { get; set; }
        #endregion
        #region بیماریهای خاص
        // بیماران دیا لیزی ،تالاسمی ، هموفیلی،ام اس
         public Typ_SpecialDiseasesEnum? Typ_SpecialDiseases  { get; set; }
       
        #endregion
 


		public User CkeckOperation { get; set; }
		public int? CkeckOperationId { get; set; }
		
		public ManzalatFormStatuseEnum FormStatuse { get; set; }  
        public string  DenyDescription { get; set; }


		public DateTime CreationDate { get; set; }
		public DateTime? CheckDate { get; set; }
		public DateTime? LastUpdate { get; set; }
        /// <summary>
        /// مدارک ارسال شده
        /// </summary>
        public ICollection<ManzalatDocumentInfo> ManzalatDocumentInfo { get; set; }


    }

    public class ManzalatDocumentInfo
    {



        public int Id { get; set; }
        public string Title { get; set; }

        public string FileName { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }


        public DateTime AttachedOnDate { get; set; }

        public string FilePath { get; set; }

        public string ThumnailPath { get; set; }


       // public int OwnerId { get; set; }

        public virtual string Owner { get; set; }


        public virtual Manzalat Manzalat { get; set; }
        public virtual int ManzalatId { get; set; }

        public virtual string DocumentGroupDescription { get; set; }


        public ManzalatFormStatuseEnum DocumentStatus { get; set; }
        public string Description { get; set; }


        public bool IsDeleted { get; set; }


    }







}
