using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel
{

    public class UploadFileResult
    {
        public string Guid { get; set; }
        public string  UploadUrl { get; set; } 
        public string Thumbnail { get; set; }

        public int ImportId { get; set; }
        public string ErrorMessage { get; set; }

    }



    public class AttachmentViewModel
    {
        public string Guid { get; set; }
        public IFormFile File { get; set; } 
        public string Caption { get; set; }
        public int GroupId { get; set; }



    }

    public class AttachmentInfo
    {
         
        
        public int Id { get; set; } 

        

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



        public virtual string  UserName { get; set; }
        public int? UserId { get; set; }

        public string FilePath { get; set; }


        public long DownloadsCount { get; set; }








    }

    public class AttachmentUserDocumentViewModel
    {
        public int  GroupId { get; set; }
        public IFormFile File { get; set; }
         
    }





}
