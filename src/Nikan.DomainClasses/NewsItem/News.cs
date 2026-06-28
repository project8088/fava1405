using Nikan.Common.GlobalEnum;
using Nikan.DomainClasses.Citizens;
using Nikan.DomainClasses.UserCompanes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nikan.DomainClasses.NewsItem
{


 

    public class News
    {
        public int Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Code { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }
        public int Clicks { get; set; }


        public DateTime PublishDate { get; set; }


        public DateTime? EndDate { get; set; }



        public DateTime OnDate { get; set; }


        public string SeoDescription { get; set; }

        public string SeoTags { get; set; }

        public bool CommentIsActive { get; set; }


        public string AttachmentFileGroup { get; set; }
        public string AttachmentImageGroup { get; set; }
        public string ImageUrl { get; set; }

        public string ThumbnailUrl { get; set; }




        #region Foreign Key


        public int? CreatedByUserId { get; set; }
        public virtual User CreatedByUser { get; set; }

        public int? NewsGroupId { get; set; }
        public virtual NewsGroup NewsGroup { get; set; }



        [StringLength(200)]
        public string Slug { get; set; }


        [StringLength(3)]
        public string LanguageName { get; set; }



        #endregion

        public PageType PageType { get; set; }

        public bool IsSpecial { get; set; }

        public string OrganizationId { get; set; }
        public Organization Organization { get; set; }


        public bool IsPrivate { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }


        public int? IndexOrder { get; set; }



    }


    public class NewsForCitizen 
    {
        public int Id { get; set; }

        /// <summary>
        /// کدام اطلاعیه
        /// </summary>
        public News News { get; set; }
        public int NewsId { get; set; }

         
        /// <summary>
        /// برای این شهروند
        /// </summary>
        public int?  UserId { get; set; }
        public User User { get; set; }

        /// <summary>
        /// برای این گروه خبر
        /// </summary>
        public int? GroupId { get; set; }
        public Group Group { get; set; }



        /// <summary>
        /// در چه تاریخی ابلاغ شده است
        /// </summary>
        public DateTime OnDate { get; set; }


    }


    public class NewsReads
    {
        public int Id { get; set; }

        public News News { get; set; }
        public int NewsId { get; set; }


        public int? UserId { get; set; }
        public User User { get; set; }
         

        public string UserIP { get; set; }


        public DateTime OnDate { get; set; }

    }



    public class NewsGroup
    {


        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public virtual PageType PageType { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public ICollection<News> News { get; set; }


    }
    public class NewsComment
    {

        public int Id { get; set; }



        public virtual long? ReplyId { get; set; }
        /// <summary>
        /// gets or sets body of blog NewsItem's comment
        /// </summary>
        public virtual NewsComment Reply { get; set; }

        /// <summary>
        /// gets or sets NewsItem that this comment sent to it
        /// </summary>
        public virtual News NewsItem { get; set; }
        /// <summary>
        ///    gets or sets NewsItem'Id that this comment sent to it
        /// </summary>
        public virtual long NewsItemId { get; set; }


        public string CommentMessage { get; set; }
        public string EmailAddress { get; set; }

        public string FullName { get; set; }

        public bool? IsPublish { get; set; }


        public int? PublishByUserId { get; set; }
        public virtual User PublishByUser { get; set; }

        public DateTime? CreatedOnDate { get; set; }

        public virtual ICollection<NewsComment> Children { get; set; }

        public string UserIP { get; set; }


    }

    public class WebPage
    {

        public int Id { get; set; }
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }


        public string Body { get; set; }


        [StringLength(200)]
        public string Slug { get; set; }



        public int Clicks { get; set; }

        public string SeoTags { get; set; }
        public string SeoDescription { get; set; }


        public string AttachmentFileGroup { get; set; }
        public string AttachmentImageGroup { get; set; }


        public DateTime? LastModifiedOnDate { get; set; }

        public DateTime CreatedOnDate { get; set; }



        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }


        public int? ModifiedById { get; set; }
        public virtual User ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

    }


}
