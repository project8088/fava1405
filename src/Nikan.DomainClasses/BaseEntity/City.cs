

using Nikan.DomainClasses.Job;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.BaseEntity
{
    public class  City  
    {


        public City()
        {
            IsActive = true;
        }
        public City(int id, int? parentId,string code,string title, bool isactive=true)
        {
            Id = id;
            Title = title;
            Code = code;
            ParentId = parentId;
            IsActive = isactive;

        }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Title { get; set; }

        [StringLength(50)]
        public string Code { get; set; }
         

        #region Parent And children
        public int? ParentId { get; set; }
        public virtual City Parent { get; set; }
        public virtual ICollection<City> Childern { get; set; }
        #endregion

        public bool IsActive { get; set; }
 
        public int? PostCode { get; set; }

        public int? ItemLevel { get; set; }


        public int IndexOrder { get; set; }

         

    }

}