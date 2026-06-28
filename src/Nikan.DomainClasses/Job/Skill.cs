using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Nikan.DomainClasses.Job
{
    /// <summary>
    /// تخصص ها و مهارتها
    /// </summary>
    public class Skill  
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; } 
        [Required]
        [StringLength(100)]
        public string Title { get; set; }


        public bool IsActive { get; set; }

        public int IndexOrder { get; set; } 


    }
}
