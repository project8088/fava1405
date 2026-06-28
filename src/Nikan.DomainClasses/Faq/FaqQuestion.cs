using Nikan.Common.GlobalEnum;
 
using System;
 
  

namespace Nikan.DomainClasses.Faq
{
    /// <summary>
    /// سوالات
    /// </summary>
    public class FaqQuestion   
    {
        public int Id { get; set; }
        public string Title { get; set; }



        public string Description { get; set; }


        #region QuestionGroupType
        public virtual FaqQuestionGroupType QuestionGroupType { get; set; }
        public virtual int? QuestionGroupTypeId { get; set; }
        #endregion 
        public virtual string TagNames { get; set; }
        public virtual int ViewCount { get; set; }
       
        /// <summary>
        /// پرسش و پاسخ های مربوط به هر واحد
        /// اجباری نیست
        /// </summary>
        public virtual string OrganizationalUnitId { get; set; }
        public virtual OrganizationalUnit OrganizationalUnit { get; set; }





        public virtual int?  ModifiedById { get; set; }
         public virtual User ModifiedBy  { get; set; }


        public virtual int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }



        public DateTime CreateOnDate { get; set; }

         public DateTime? ModifiedOnDate { get; set; }
        public bool IsActive { get; set; }
        public int IndexOrder { get; set; }
        public bool IsMainFaq { get; set; }

        public string Icon { get; set; }


        public bool IsDeleted { get; set; }


    }



}
