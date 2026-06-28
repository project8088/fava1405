using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.Job
{
    public  class Major
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }


        [StringLength(100)]
        [Required]
        public string Title { get; set; }
         

        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }


        public bool IsSystem { get; set; }


        [StringLength(50)]
        public string Code { get; set; }


        [StringLength(100)]
        public string SearchTitle { get; set; }

        public int IndexOrder { get; set; }
         
    }
}
