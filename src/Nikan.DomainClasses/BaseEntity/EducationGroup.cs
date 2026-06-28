using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Nikan.DomainClasses.BaseEntity
{
    /// <summary>
    /// گروه شغلی
    /// </summary>
    public class EducationGroup
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; } 
        [StringLength(100)]
        [Required]
        public string Title { get; set; }


        public bool IsDeleted { get; set; }
    }
}
