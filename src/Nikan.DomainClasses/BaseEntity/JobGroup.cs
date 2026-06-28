using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.BaseEntity
{
    /// <summary>
    /// گروههای شغلی
    /// </summary>
    public class JobGroup
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
