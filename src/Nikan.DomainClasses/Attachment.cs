using System; 
using System.ComponentModel.DataAnnotations;
 

namespace Nikan.DomainClasses
{
    public class Attachment
    {

        public Attachment()
        {
            AttachedOn = DateTime.Now;
        }


        [Key]
        public int Id { get; set; }


        public virtual User User { get; set; }
        public int? UserId { get; set; }


        public string AttachmentGroup { get; set; }


        /// <summary>
        ///     sets or gets name for attachment
        /// </summary>
        public string FileName { get; set; }



        public string Caption { get; set; }

        /// <summary>
        ///     sets or gets type of attachment
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     sets or gets size of attachment
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        ///     sets or gets Extention of attachment
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        ///     sets or gets bytes of data
        /// </summary>
        /// <summary>
        ///     sets or gets Creation Date
        /// </summary>
        public DateTime AttachedOn { get; set; }

        /// <summary>
        ///     gets or sets counts of download this file
        /// </summary>
        public long DownloadsCount { get; set; }

        /// <summary>
        ///     gets or sets datetime that is modified
        /// </summary>
        public DateTime? ModifiedOn { get; set; }


        public int DurationMinute { get; set; }
        public int IndexOrder { get; set; }


        /// <summary>
        ///     gets or sets section that this file attached there
        /// </summary>

     

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public string FilePath { get; set; }

        public string ThumnailPath { get; set; }


        /// <summary>
        /// نمایش تصویر عمومی است
        /// </summary>
        public bool IsPublic { get; set; }






    }
  
}
