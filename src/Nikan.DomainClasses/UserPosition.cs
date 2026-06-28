 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nikan.DomainClasses
{
    /// <summary>
    /// سمت های سازمانی
    /// </summary>
    public class UserPosition
    {
        public int Id { get; set; }

        #region Title
        [StringLength(50)]
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


        public bool IsActive { get; set; }

    }
}
