using Nikan.Common.GlobalEnum; 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nikan.ViewModel.NewsItems
{
    public class NewsDto
    {

        /// <summary>
        /// شناسه خبر
        /// </summary>
        public int? Id { get; set; }


        public string Title { get; set; }



        public string Description { get; set; }

        public string Body { get; set; }



        /// <summary>
        /// تاریخ انتشار خبر
        /// </summary>
        public DateTime? PublishDate { get; set; }




        /// <summary>
        /// توضیحات سئو
        /// </summary>
        public string SeoDescription { get; set; }


        /// <summary>
        /// تگهای سئو
        /// </summary>
        public string SeoTags { get; set; }


        /// <summary>
        /// آیا امکان ارسال دیدگاه وجود دارد ؟
        /// </summary>
        public bool CommentIsActive { get; set; }



        public string AttachmentFileGroup { get; set; }
        public string AttachmentImageGroup { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }






        public string NewsGroup { get; set; }


        /// <summary>
        /// آیا خبر ویژه است ؟
        /// </summary>
        public bool IsSpecial { get; set; }

        public string OrganizationId { get; set; }

        public bool IsActive { get; set; }

        public int? IndexOrder { get; set; }

        public int? NewsGroupId { get; set; }

    }
  
    
    public class NewsInfo
    {

        /// <summary>
        /// شناسه خبر
        /// </summary>
        public int? Id { get; set; }


        public int? IndexOrder { get; set; }
        /// <summary>
        /// عنوان خبر
        /// </summary>
        [Required]
        [StringLength(2000)]
        public string Title { get; set; }
      
        /// <summary>
        /// شماره اطلاعیه
        /// </summary>
        public string NotificationNumber { get; set; }

        /// <summary>
        /// توضیحات کوتاه
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///شرح کامل
        ///Html Editor
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// تعداد مشاهده خبر
        /// </summary>
        public int Clicks { get; set; }

        /// <summary>
        /// تاریخ انتشار خبر
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// پایان اطلاعیه 
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// تاریخ ایجاد خبر
        /// </summary>
        public DateTime OnDate { get; set; }

        /// <summary>
        /// توضیحات سئو
        /// </summary>
        public string SeoDescription { get; set; }


        /// <summary>
        /// تگهای سئو
        /// </summary>
        public string SeoTags { get; set; }


        /// <summary>
        /// آیا امکان ارسال دیدگاه وجود دارد ؟
        /// </summary>
        public bool CommentIsActive { get; set; }


        public string  AttachmentFileGroup { get; set; }
        public string AttachmentImageGroup { get; set; }
        public string ImageUrl { get; set; } 
        public string ThumbnailUrl { get; set; }



        #region Foreign Key


        public int? CreatedByUserId { get; set; }
        public string CreatedByUser { get; set; }

        public int? NewsGroupId { get; set; }
        public string NewsGroup { get; set; }


        /// <summary>
        /// مسیر دسترسی
        /// ویژه صفحات
        /// </summary>
        [StringLength(200)]
        public string Slug { get; set; }


        [StringLength(3)]
        public string LanguageName { get; set; }



        #endregion


        /// <summary>
        /// آیا خبر ویژه است ؟
        /// </summary>
        public bool IsSpecial { get; set; }

        public string OrganizationId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }


        /// <summary>
        /// این اطلاعیه خصوصی است
        /// مخصوص شرکت هایی که مشخص می شوند
        /// </summary>
        public bool IsPrivate { get; set; }


        public string Company { get; set; }

        public int CompanyId { get; set; }
    }

    public class NewsList
    {

        /// <summary>
        /// شناسه خبر
        /// </summary>
        public int? Id { get; set; }



        /// <summary>
        /// عنوان خبر
        /// </summary>
        [Required]
        [StringLength(2000)]
        public string Title { get; set; }

        /// <summary>
        /// شماره اطلاعیه
        /// </summary>
        public string NotificationNumber { get; set; }

        /// <summary>
        /// توضیحات کوتاه
        /// </summary>
        public string Description { get; set; }
        

        /// <summary>
        /// تعداد مشاهده خبر
        /// </summary>
        public int Clicks { get; set; }

        /// <summary>
        /// تاریخ انتشار خبر
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// پایان اطلاعیه 
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// تاریخ ایجاد خبر
        /// </summary>
        public DateTime OnDate { get; set; }

        /// <summary>
        /// توضیحات سئو
        /// </summary>
        public string SeoDescription { get; set; }


        
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }



        #region Foreign Key


        public int? CreatedByUserId { get; set; }
        public string CreatedByUser { get; set; }

        public int? NewsGroupId { get; set; }
        public string NewsGroup { get; set; }


        /// <summary>
        /// مسیر دسترسی
        /// ویژه صفحات
        /// </summary>
        [StringLength(200)]
        public string Slug { get; set; }


        [StringLength(3)]
        public string LanguageName { get; set; }



        #endregion


      
       
       

    }




    /// <summary>
    /// اطلاعیه
    /// </summary>
    public class NotificationDto
    {

        /// <summary>
        /// شناسه اطلاعیه
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// شماره اطلاعیه
        /// </summary>
        public string NotificationNumber { get; set; }

        public string Title { get; set; }



        public string Description { get; set; }

        public string Body { get; set; }



        /// <summary>
        /// شروع اطلاعیه
        /// </summary>
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// پایان اطلاعیه 
        /// </summary>
        public DateTime? EndDate { get; set; }

         
       public string ThumbnailUrl { get; set; }
        public string ImageUrl { get; set; }

        public string AttachmentFileGroup { get; set; }
        public string AttachmentImageGroup { get; set; }
        public bool IsActive { get; set; }
        /// <summary>
        /// این اطلاعیه خصوصی است
        /// مخصوص شرکت هایی که مشخص می شوند
        /// </summary>
        public bool IsPrivate { get; set; }
    }
     

    public class NotificationInfo
    {

        /// <summary>
        /// شناسه اطلاعیه
        /// </summary>
        public int? Id { get; set; }



        /// <summary>
        /// عنوان خبر
        /// </summary>
        [Required]
        [StringLength(2000)]
        public string Title { get; set; }

        /// <summary>
        /// شماره اطلاعیه
        /// </summary>
        public string NotificationNumber { get; set; }

        /// <summary>
        /// توضیحات کوتاه
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///شرح کامل
        ///Html Editor
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// تعداد مشاهده خبر
        /// </summary>
        public int Clicks { get; set; }

        /// <summary>
        /// تاریخ شروع اطلاعیه
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// پایان اطلاعیه 
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// تاریخ ایجاد خبر
        /// </summary>
        public DateTime OnDate { get; set; }






        #region Foreign Key


        public int? CreatedByUserId { get; set; }
        public string CreatedByUser { get; set; }







        #endregion



        public string AttachmentFileGroup { get; set; }
        public string AttachmentImageGroup { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// این اطلاعیه خصوصی است
        /// مخصوص شرکت هایی که مشخص می شوند
        /// </summary>
        public bool IsPrivate { get; set; }

        public bool IsActive { get; set; }


        public string Company { get; set; }

        public int CompanyId { get; set; }


        public List<NewsReadsInfo> NewsReads { get; set; }



    }


    public class NewsReadsInfo
    {
        public int Id { get; set; }

        public string  News { get; set; }
        public int NewsId { get; set; } 
        public int? UserId { get; set; }
        public string  User { get; set; }


        public int? UserCompanyId { get; set; }
        public string  UserCompany { get; set; }

        public string UserIP { get; set; } 
        public DateTime OnDate { get; set; }

    }



    public class GroupsNotifactionsDto
    {
        /// <summary>
        /// شناسه اطلاعیه
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// شناسه گروه
        /// </summary>
        public int GroupId { get; set; }

    }



    public class CitizensNotifactionsDto
    {
        /// <summary>
        /// شناسه اطلاعیه
        /// </summary>
        public int Id { get; set; } 
     
        /// <summary>
        /// کد ملی شهروند
        /// </summary>
        public string NationalCode { get; set; }

        /// <summary>
        /// شناسه گروه
        /// </summary>
        public int? GroupId { get; set; }






    }
    public class  CitizensNotifactions
    {

        public int Id { get; set; }

        /// <summary>
        /// شهروند
        /// </summary>
        public int? CitizenId { get; set; } 
        public string Citizen { get; set; }


        public int? GroupId { get; set; }
        public string Group { get; set; }

        /// <summary>
        /// عنوان اطلاعیه
        /// </summary>
        [Required]
        [StringLength(2000)]
        public string Title { get; set; }

        /// <summary>
        /// تاریخ اطلاعیه به شرکت
        /// </summary>
        public DateTime OnDate { get; set; }

    }


    public class NewsCommentDto
    {
        public int Id { get; set; }

        public string UserIP { get; set; }

        public string CommentMessage { get; set; }
        public string EmailAddress { get; set; }

        public string FullName { get; set; }


        public virtual long? ReplyId { get; set; }

        public bool? IsPublish { get; set; }
        public virtual long NewsItemId { get; set; }

        public DateTime? CreatedOnDate { get; set; }

    }
    public class PagedNewsItemsViewModel
    {

         
      

        public List<NewsInfo> News { get; set; }

        public int TotalItems { get; set; }


    }


  





    public class PublishCommen
    {
        public int CommentId { get; set; }

        public bool IsPublish { get; set; }

    }

    public class WebPageDto
    {

        public int? Id { get; set; }
      
        public string Title { get; set; }

       
        public string Description { get; set; } 

        public string Body { get; set; } 
        public string Slug { get; set; } 

        public string SeoTags { get; set; }
        public string SeoDescription { get; set; }

        


    }
    public class WebPageList
    {

        public int Id { get; set; }
       
        public string Title { get; set; }

       
        public string Description { get; set; }

         


        [StringLength(200)]
        public string Slug { get; set; }



        public int Clicks { get; set; }

       

        public DateTime? LastModifiedOnDate { get; set; }

        public DateTime CreatedOnDate { get; set; }



        public int? CreatedById { get; set; }
        public virtual string  CreatedBy { get; set; }


        public int? ModifiedById { get; set; }
        public virtual string  ModifiedBy { get; set; }



    }

    public class WebPageInfo
    {

        public int Id { get; set; }
       

        public string Title { get; set; }

         
        public string Description { get; set; }


        public string Body { get; set; }


        
        public string Slug { get; set; }



        public int Clicks { get; set; }

        public string SeoTags { get; set; }
        public string SeoDescription { get; set; }




        public DateTime? LastModifiedOnDate { get; set; }

        public DateTime CreatedOnDate { get; set; }



        public int? CreatedById { get; set; }
        public virtual string  CreatedBy { get; set; }


        public int? ModifiedById { get; set; }
        public virtual string  ModifiedBy { get; set; }


        public string AttachmentFileGroup { get; set; }
        public string AttachmentImageGroup { get; set; }


    }

    public class WebPageItemsViewModel
    {
         

        public List<WebPageInfo> WebPages { get; set; }

        public int TotalItems { get; set; }


    }
}
