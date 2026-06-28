using Nikan.Common.GlobalEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Nikan.DomainClasses.UserDocuments
{
   public class UserDocumentGroup 
    {


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }
        
        [StringLength(2000)]
        [Required]
        public string Title { get; set; } 
        public string Description { get; set; } 
        public int  IndexOrder { get; set; }

        public virtual bool IsActive { get; set; }
        public virtual bool IsDeleted { get; set; }

      

    }

    public class UserDocument
    {
        public UserDocument()
        {
            AttachedOnDate = DateTime.Now;
        }


        [Key]
        public int Id { get; set; } 


        [StringLength(200)]
        public string Title { get; set; }



        [StringLength(200)]
        public string FileName { get; set; }




        /// <summary>
        ///     sets or gets size of attachment
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        ///     sets or gets Extention of attachment
        /// </summary>
        [StringLength(50)]
        public string Extension { get; set; }

        
        public DateTime AttachedOnDate { get; set; }




        [StringLength(1000)]
        public string FilePath { get; set; }
        [StringLength(1000)]

        public string ThumnailPath { get; set; }






       
        public int OwnerId { get; set; }

        public virtual User  Owner { get; set; }


        public virtual UserDocumentGroup DocumentGroup { get; set; }
        public virtual int DocumentGroupId { get; set; } 

        public UserDocumentStatusEnum DocumentStatus { get; set; }
        public string Description { get; set; }

        public virtual bool IsDeleted { get; set; }

    }

}
