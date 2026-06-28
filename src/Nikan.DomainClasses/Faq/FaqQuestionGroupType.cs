 
using System;
using System.ComponentModel.DataAnnotations;

namespace Nikan.DomainClasses.Faq
{
    /// <summary>
    /// گروهبندی سوالات
    /// </summary>
    public class FaqQuestionGroupType
    {


        public int Id { get; set; }

        #region Title
        [StringLength(300)]
        [Required]
        public string Title { get; set; }
        #endregion
        #region Description
        public string Description { get; set; }
        #endregion

        public virtual int? ModifiedById { get; set; }
        public virtual User ModifiedBy { get; set; }


        public virtual int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

         
       
        /// <summary>
        /// پرسش و پاسخ های مربوط به هر واحد
        /// اجباری نیست
        /// </summary>
        public virtual string OrganizationalUnitId { get; set; }
        public virtual OrganizationalUnit OrganizationalUnit { get; set; }


       

        public DateTime CreateOnDate { get; set; }

        public DateTime? ModifiedOnDate { get; set; }

        public string Icon { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }
        public int IndexOrder { get; set; }




    }
}
