using Nikan.Common;
using Nikan.Common.GlobalEnum;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.NewsItem;
using Nikan.ViewModel.NewsItems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nikan.DomainClasses.Citizens;

namespace Nikan.Services
{

    public class NewsService : INewsService
    {
        #region Field
        private readonly IUnitOfWork _uow;
        private readonly DbSet<News> _news;
        private readonly DbSet<NewsComment> _newsComments;
        private readonly DbSet<NewsForCitizen> _CitizensNotifactions;
        private readonly DbSet<NewsReads> _newsReads;
        private readonly DbSet<Citizen> _citizen;
        #endregion
        #region Constractor

        public NewsService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _news = _uow.Set<News>();
            _citizen = _uow.Set<Citizen>();

            _newsComments = _uow.Set<NewsComment>();
            _CitizensNotifactions = _uow.Set<NewsForCitizen>();
            _newsReads = _uow.Set<NewsReads>();

        }
        #endregion




        public async Task<ApiResult<NewsDto>> AddOrUpdateNews(NewsDto model, int userId)
        {

            var res = new ApiResult<NewsDto>(true, ApiResultStatusCode.Success, new NewsDto());
            try
            {
                if (model.Id == null)
                {





                    var item = new News
                    {
                        OnDate = DateTime.Now,
                        Title = model.Title,
                        Body = model.Body,
                        Clicks = 0,
                        CommentIsActive = model.CommentIsActive,
                        CreatedByUserId = userId,
                        Description = model.Description,
                        IsActive = model.IsActive,
                        ImageUrl = model.ImageUrl,
                        ThumbnailUrl = model.ThumbnailUrl,
                        IsSpecial = model.IsSpecial,
                        LanguageName = "fa",
                        NewsGroupId = model.NewsGroupId,
                        IndexOrder=model.IndexOrder,

                        PageType = PageType.NewsList,
                        SeoTags = model.SeoTags,
                        SeoDescription = model.SeoDescription,
                        PublishDate = model.PublishDate.Value,

                    };
                    if (string.IsNullOrWhiteSpace(model.AttachmentFileGroup))
                        item.AttachmentFileGroup = Guid.NewGuid().ToString();

                    if (string.IsNullOrWhiteSpace(model.AttachmentImageGroup))
                        item.AttachmentImageGroup = Guid.NewGuid().ToString();


                    await _news.AddAsync(item);
                    await _uow.SaveChangesAsync();

                }
                else
                {

                    var item = await _news.FirstOrDefaultAsync(w => w.Id == model.Id);
                    if (item == null)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        res.Messages = "رکوردی یافت نشد";

                    }

                    item.Title = model.Title;
                    item.Body = model.Body;
                    item.CommentIsActive = model.CommentIsActive;
                    item.Description = model.Description;
                    item.IsActive = model.IsActive;
                    item.ImageUrl = model.ImageUrl;
                    item.ThumbnailUrl = model.ThumbnailUrl;
                    item.IsSpecial = model.IsSpecial;
                    item.LanguageName = "fa";
                    item.NewsGroupId = model.NewsGroupId;
                    item.SeoTags = model.SeoTags;
                    item.IndexOrder = model.IndexOrder;
                    item.SeoDescription = model.SeoDescription;
                    item.PublishDate = model.PublishDate.Value;
                    if (string.IsNullOrWhiteSpace(item.AttachmentFileGroup))
                        item.AttachmentFileGroup = Guid.NewGuid().ToString();

                    if (string.IsNullOrWhiteSpace(item.AttachmentImageGroup))
                        item.AttachmentImageGroup = Guid.NewGuid().ToString();
                    _news.Update(item);
                    await _uow.SaveChangesAsync();

                }



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            }


            return res;
        }


        public async Task<ApiResult<NotificationDto>> AddOrUpdateNotifications(NotificationDto model, int userId)
        {

            var res = new ApiResult<NotificationDto>(true, ApiResultStatusCode.Success, new NotificationDto());
            try
            {
                if (model.Id == null)
                {



                    var item = new News
                    {
                        OnDate = DateTime.Now,
                        Title = model.Title,
                        Body = model.Body,
                        Clicks = 0,
                        CreatedByUserId = userId,
                        Description = model.Description,
                        IsActive = model.IsActive,
                        ImageUrl = model.ImageUrl,
                        IsPrivate = model.IsPrivate,
                        ThumbnailUrl = model.ThumbnailUrl,
                        LanguageName = "fa",
                        PageType = PageType.Notifications,
                        PublishDate = model.PublishDate.Value,
                        EndDate = model.EndDate.Value,
                        Code = model.NotificationNumber

                    };

                    if (string.IsNullOrWhiteSpace(model.AttachmentFileGroup))
                        item.AttachmentFileGroup = Guid.NewGuid().ToString();

                    if (string.IsNullOrWhiteSpace(model.AttachmentImageGroup))
                        item.AttachmentImageGroup = Guid.NewGuid().ToString();

                    await _news.AddAsync(item);
                    await _uow.SaveChangesAsync();
                    res.Data.Id = item.Id;

                }
                else
                {

                    var item = await _news.FirstOrDefaultAsync(w => w.Id == model.Id);
                    if (item == null)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        res.Messages = "رکوردی یافت نشد";

                    }

                    item.Title = model.Title;
                    item.Body = model.Body;
                    item.Description = model.Description;
                    item.IsActive = model.IsActive;
                    item.ImageUrl = model.ImageUrl;
                    item.ThumbnailUrl = model.ThumbnailUrl;

                    item.Code = model.NotificationNumber;
                    item.LanguageName = "fa";

                    item.EndDate = model.EndDate.Value;
                    item.PublishDate = model.PublishDate.Value;

                    item.IsPrivate = model.IsPrivate;


                    if (string.IsNullOrWhiteSpace(item.AttachmentFileGroup))
                        item.AttachmentFileGroup = Guid.NewGuid().ToString();

                    if (string.IsNullOrWhiteSpace(item.AttachmentImageGroup))
                        item.AttachmentImageGroup = Guid.NewGuid().ToString();


                    _news.Update(item);
                    await _uow.SaveChangesAsync();
                    res.Data.Id = item.Id;

                }



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            }


            return res;
        }


        public async Task<ApiResult> AddCitizensNotifactions(CitizensNotifactionsDto model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت ثبت شد");
            try
            {


                if(string.IsNullOrWhiteSpace(model.NationalCode) && model.GroupId==null )
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "گروه شهروندی یا کد ملی شهروند را وارد نمایید";
                    return res;
                }
                int? userId = null;
                if (!string.IsNullOrWhiteSpace(model.NationalCode))
                {
                    var user = await _citizen.FirstOrDefaultAsync(w => w.NationCode == model.NationalCode);
                    if (user == null)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.ServerError;
                        res.Messages = "شهروندی با کد ملی وارد شده یافت نشد";
                        return res;
                    }
                    userId = user.CitizenId;
                }

                var item = new NewsForCitizen
                {
                    OnDate = DateTime.Now, 
                    NewsId = model.Id,
                    GroupId=model.GroupId,
                    UserId= userId
                };
                await _CitizensNotifactions.AddAsync(item);
                await _uow.SaveChangesAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }

        public async Task<ApiResult> AddGroupNotifactions(GroupsNotifactionsDto model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت ثبت شد");
            try
            {
                var item = new NewsForCitizen
                {
                    OnDate = DateTime.Now,
                    GroupId = model.GroupId,
                    NewsId = model.Id,
                };
                await _CitizensNotifactions.AddAsync(item);
                await _uow.SaveChangesAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }



        public async Task<ApiResult> AddComments(NewsCommentDto model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "نظر شما با موفقیت ثبت شد");
            try
            {
                var item = new NewsComment
                {
                    FullName = model.FullName,
                    NewsItemId = model.NewsItemId,
                    UserIP = model.UserIP,
                    ReplyId = model.ReplyId,
                    CommentMessage = model.CommentMessage,
                    EmailAddress = model.EmailAddress,
                    CreatedOnDate = DateTime.Now

                };
                await _newsComments.AddAsync(item);
                await _uow.SaveChangesAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }




        public async Task<ApiResult<List<NewsCommentDto>>> GetComments(int newsid)
        {
            var res = new ApiResult<List<NewsCommentDto>>(true, ApiResultStatusCode.Success, new List<NewsCommentDto>());
            try
            {
                res.Data = await _newsComments.Where(w => w.NewsItemId == newsid).Select(s => new NewsCommentDto()
                {
                    CommentMessage = s.CommentMessage,
                    EmailAddress = s.EmailAddress,
                    FullName = s.FullName,
                    NewsItemId = s.NewsItemId,
                    ReplyId = s.ReplyId,
                    UserIP = s.UserIP,
                    Id = s.Id,
                    IsPublish = s.IsPublish,
                    CreatedOnDate = s.CreatedOnDate,


                }).ToListAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }
        public async Task<ApiResult<List<NewsCommentDto>>> GetPublishComments(int newsid)
        {
            var res = new ApiResult<List<NewsCommentDto>>(true, ApiResultStatusCode.Success, new List<NewsCommentDto>());
            try
            {
                res.Data = await _newsComments.Where(w => w.NewsItemId == newsid
                && w.IsPublish == true

                ).Select(s => new NewsCommentDto()
                {
                    CommentMessage = s.CommentMessage,
                    EmailAddress = s.EmailAddress,
                    FullName = s.FullName,
                    NewsItemId = s.NewsItemId,
                    ReplyId = s.ReplyId,
                    UserIP = s.UserIP,
                    Id = s.Id,
                    IsPublish = s.IsPublish,
                    CreatedOnDate = s.CreatedOnDate,

                }).ToListAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }

        public async Task<ApiResult> PublishComment(int commentId, bool IsPublish, int? PublishByUserId)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success);
            try
            {
                if (PublishByUserId == 0)
                    PublishByUserId = null;
                var item = await _newsComments.FirstOrDefaultAsync(w => w.Id == commentId);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    res.Messages = "رکوردی یافت نشد";
                    return res;
                }
                item.IsPublish = IsPublish;
                item.PublishByUserId = PublishByUserId;
                _newsComments.Update(item);
                await _uow.SaveChangesAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }

        public async Task<ApiResult> RemoveNews(int id)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت حذف گردید");
            var item = await _news.FirstOrDefaultAsync(w => w.Id == id);
            if (item == null)
            {

                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.NotFound;
                res.Messages = "رکوردی یافت نشد";
                return res;

            }

            item.IsDeleted = true;
            _news.Update(item);
            await _uow.SaveChangesAsync();
            return res;
        }


        public async Task<ApiResult> RemoveCitizensNotifactions(int id)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت حذف گردید");
            var item = await _CitizensNotifactions.FirstOrDefaultAsync(w => w.Id == id);
            if (item == null)
            {

                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.NotFound;
                res.Messages = "رکوردی یافت نشد";
                return res;

            }
            _CitizensNotifactions.Remove(item);
            await _uow.SaveChangesAsync();
            return res;
        }

        public async Task<ApiResult<NewsInfo>> GetNews(int id, bool forEdit = true)
        {

            ApiResult<NewsInfo> res = new ApiResult<NewsInfo>(true, ApiResultStatusCode.Success, null);

            try
            {
                var news = await _news.Include(i => i.NewsGroup).FirstOrDefaultAsync(w => w.Id == id);
                if (news == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "خبری یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                if (news.IsDeleted)
                {
                    res.IsSuccess = false;
                    res.Messages = "خبر  حذف شده است";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                var data = new NewsInfo()
                {
                    OrganizationId = news.OrganizationId,
                    Id = news.Id,
                    IndexOrder=news.IndexOrder,
                    NewsGroup = news.NewsGroup.Title,
                    OnDate = news.OnDate,
                    Title = news.Title,
                    Body = news.Body,
                    Clicks = news.Clicks,
                    CommentIsActive = news.CommentIsActive,
                    CreatedByUserId = news.CreatedByUserId,
                    AttachmentFileGroup = news.AttachmentFileGroup,
                    AttachmentImageGroup = news.AttachmentImageGroup,
                    Description = news.Description,
                    IsActive = news.IsActive,
                    IsPrivate = news.IsPrivate,
                    ImageUrl = news.ImageUrl,
                    ThumbnailUrl = news.ThumbnailUrl,
                    IsSpecial = news.IsSpecial,
                    LanguageName = news.LanguageName,
                    NewsGroupId = news.NewsGroupId,
                    SeoTags = news.SeoTags,
                    SeoDescription = news.SeoDescription,
                    PublishDate = news.PublishDate,
                    Slug = news.Slug,
                    NotificationNumber = news.Code,
                    EndDate = news.EndDate,




                };
                if (!forEdit)
                {
                    var click = news.Clicks + 1;
                    news.Clicks = click;
                    _news.Update(news);
                    await _uow.SaveChangesAsync();
                }


                res.Data = data;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }


        public async Task<ApiResult<NotificationInfo>> GetNotification(int id, int? companyId, int? userId, string userIp = "", bool forEdit = true)
        {

            ApiResult<NotificationInfo> res = new ApiResult<NotificationInfo>(true, ApiResultStatusCode.Success, null);

            try
            {

                var news = await _news.Include(i => i.NewsGroup).FirstOrDefaultAsync(w => w.Id == id);
                if (news == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "  یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                if (news.IsDeleted)
                {
                    res.IsSuccess = false;
                    res.Messages = "   حذف شده است";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                var data = new NotificationInfo()
                {

                    Id = news.Id,
                    OnDate = news.OnDate,
                    Title = news.Title,
                    Body = news.Body,
                    Clicks = news.Clicks,
                    CreatedByUserId = news.CreatedByUserId,
                    Description = news.Description,
                    IsActive = news.IsActive,
                    PublishDate = news.PublishDate,
                    NotificationNumber = news.Code,
                    EndDate = news.EndDate,
                    AttachmentFileGroup = news.AttachmentFileGroup,
                    AttachmentImageGroup = news.AttachmentImageGroup,
                    ImageUrl = news.ImageUrl,
                    ThumbnailUrl = news.ThumbnailUrl,
                    IsPrivate = news.IsPrivate,

                };
                if (!forEdit)
                {
                    var click = news.Clicks + 1;
                    news.Clicks = click;
                    _news.Update(news);
                    if (companyId != null && companyId != 0)
                    {
                        //ثبت بازدید توسط شرکت
                        await _newsReads.AddAsync(new NewsReads()
                        {
                            NewsId = news.Id,
                            OnDate = DateTime.Now, 
                            UserId = userId,
                            UserIP = userIp,
                        });
                    }

                    await _uow.SaveChangesAsync();

                }


                res.Data = data;

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }





        public async Task<ApiResult<PagedNewsItemsViewModel>> GetPagedNewsItemsAsync(PageType pagetype,
            int pageNumber, int pageSize, int? groupId = null,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string title = null


            )
        {

             var offset = (pageNumber ) * pageSize ;
            var res = new ApiResult<PagedNewsItemsViewModel>(true, ApiResultStatusCode.Success, null);

            try
            {
                var query = _news.Where(w => w.PageType == pagetype && w.IsActive && w.IsDeleted != true);
                if (groupId.HasValue)
                {
                    query = query.Where(w => w.NewsGroupId == groupId.Value);
                }

                if (!string.IsNullOrEmpty(title) && title != "null")
                {
                    query = query.Where(w => EF.Functions.Like(w.Title, "%[(" + title + ")]%"));
                }

                if (FromDate != null)
                {
                    query = query.Where(w => w.OnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.OnDate <= ToDate);
                }

                var list = await query.Select(news => new NewsInfo()
                {
                    Id = news.Id,
                    OnDate = news.OnDate,
                    Title = news.Title,
                    Body = news.Body,
                    Clicks = news.Clicks,
                    CommentIsActive = news.CommentIsActive,
                    CreatedByUserId = news.CreatedByUserId,
                    AttachmentFileGroup = news.AttachmentFileGroup,
                    AttachmentImageGroup = news.AttachmentImageGroup,
                    Description = news.Description,
                    IsActive = news.IsActive,
                    ImageUrl = news.ImageUrl,
                    ThumbnailUrl = news.ThumbnailUrl,
                    IsSpecial = news.IsSpecial,
                    LanguageName = news.LanguageName,
                    NewsGroupId = news.NewsGroupId,
                    SeoTags = news.SeoTags,
                    SeoDescription = news.SeoDescription,
                    PublishDate = news.PublishDate,
                    Slug = news.Slug,
                    NewsGroup = news.NewsGroup == null ? "" : news.NewsGroup.Title,
                    OrganizationId = news.OrganizationId,
                    NotificationNumber = news.Code,
                    EndDate = news.EndDate,
                    IsPrivate = news.IsPrivate,


                }).Skip(offset).Take(pageSize).ToListAsync();

                res.Data = new PagedNewsItemsViewModel
                {
                    TotalItems = await query.CountAsync(),
                    News = list
                };


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }




        public async Task<ApiResult<List<CitizensNotifactions>>> GetCitizenNotifications(int newsId)
        {


            var res = new ApiResult<List<CitizensNotifactions>>(true, ApiResultStatusCode.Success, new List<CitizensNotifactions>());
            try
            {
                var query = _CitizensNotifactions.Where(w => w.NewsId == newsId);
                res.Data = await query.Select(news => new CitizensNotifactions()
                {
                    Id = news.Id,
                    OnDate = news.OnDate,
                    Title = news.News.Title, 
                    CitizenId = news.UserId ,
                    GroupId=news.GroupId,
                    Group=news.Group==null ? "" :news.Group.GroupName,
                    Citizen=news.User==null ? "" :news.User.DisplayName,
                    
                    
                  

                }).ToListAsync();




            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }




        public async Task<ApiResult<List<NewsList>>> GetMostVisitedNews(PageType pagetype, int pageSize)
        {

            var res = new ApiResult<List<NewsList>>(true, ApiResultStatusCode.Success, new List<NewsList>());

            try
            {
                var query = _news.Where(w => w.PageType == pagetype && w.IsActive && w.IsDeleted != true);
                res.Data = await query.Select(news => new NewsList()
                {
                    Id = news.Id,
                    OnDate = news.OnDate,
                    Title = news.Title,
                    Clicks = news.Clicks,
                    CreatedByUserId = news.CreatedByUserId,
                    Description = news.Description,
                    ImageUrl = news.ImageUrl,
                    ThumbnailUrl = news.ThumbnailUrl,
                    LanguageName = news.LanguageName,
                    NewsGroupId = news.NewsGroupId,
                    SeoDescription = news.SeoDescription,
                    PublishDate = news.PublishDate,
                    Slug = news.Slug,
                    NewsGroup = news.NewsGroup == null ? "" : news.NewsGroup.Title,
                    NotificationNumber = news.Code,
                    EndDate = news.EndDate,
                }).OrderByDescending(w => w.Clicks).Take(pageSize).ToListAsync();



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }



        public async Task<ApiResult<List<NewsList>>> GetLastNewsAsync(PageType pagetype, int top)
        {

            var res = new ApiResult<List<NewsList>>(true, ApiResultStatusCode.Success, new List<NewsList>());

            try
            {
                var query = _news.Where(w => w.PageType == pagetype && w.IsDeleted != true).OrderBy(o => o.IndexOrder);

                res.Data = await query.Select(news => new NewsList()
                {
                    Id = news.Id,

                    OnDate = news.OnDate,
                    Title = news.Title,

                    Clicks = news.Clicks,

                    CreatedByUserId = news.CreatedByUserId,

                    Description = news.Description,

                    ImageUrl = news.ImageUrl,
                    ThumbnailUrl = news.ThumbnailUrl,

                    LanguageName = news.LanguageName,
                    NewsGroupId = news.NewsGroupId,

                    SeoDescription = news.SeoDescription,
                    PublishDate = news.PublishDate,
                    Slug = news.Slug,
                    NewsGroup = news.NewsGroup == null ? "" : news.NewsGroup.Title,
                    EndDate = news.EndDate,
                    NotificationNumber = news.Code,

                }).Take(top).ToListAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }



        public async Task<ApiResult<List<NewsList>>> GetLastNotifications(int top, int citizenId)
        {

            var res = new ApiResult<List<NewsList>>(true, ApiResultStatusCode.Success, new List<NewsList>());

            try
            {

                var listNewsIds = await _CitizensNotifactions.Where(w =>

                w.News.IsDeleted != true && w.News.IsPrivate == true && w.UserId == citizenId).Select(s => s.NewsId).ToListAsync();


                var query = _news.Where(w => w.PageType == PageType.Notifications && w.IsDeleted != true
                && (w.IsPrivate == false || (listNewsIds.Contains(w.Id)))
                ).OrderByDescending(o => o.Id);

                res.Data = await query.Select(news => new NewsList()
                {
                    Id = news.Id, 
                    OnDate = news.OnDate,
                    Title = news.Title, 
                    Clicks = news.Clicks, 
                    CreatedByUserId = news.CreatedByUserId, 
                    Description = news.Description, 
                    ImageUrl = news.ImageUrl,
                    ThumbnailUrl = news.ThumbnailUrl, 
                    LanguageName = news.LanguageName,
                    NewsGroupId = news.NewsGroupId, 
                    SeoDescription = news.SeoDescription,
                    PublishDate = news.PublishDate,
                    Slug = news.Slug,
                    NewsGroup = news.NewsGroup == null ? "" : news.NewsGroup.Title,
                    EndDate = news.EndDate,
                    NotificationNumber = news.Code,

                }).Take(top).ToListAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }




    }

    public interface INewsService
    {



        /// <summary>
        /// اضافه کردن دیدگاه برای یک خبر
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult> AddComments(NewsCommentDto model);


        /// <summary>
        /// اضافه یا ویرایش یک خبر
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult<NewsDto>> AddOrUpdateNews(NewsDto model, int userId);


        /// <summary>
        /// انتشار دیدگاه
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="IsPublish"></param>
        /// <returns></returns>
        Task<ApiResult> PublishComment(int commentId, bool IsPublish, int? PublishByUserId);




        Task<ApiResult<List<NewsCommentDto>>> GetComments(int newsid);



        /// <summary>
        /// دریافت جزئیات یک خبر
        /// </summary>
        /// <param name="id">شناسه خبر</param>
        /// <param name="forEdit">برای ویرایش است ؟</param>
        /// <returns></returns>
        Task<ApiResult<NewsInfo>> GetNews(int id, bool forEdit = true);

        /// <summary>
        /// دریافت لیست
        /// </summary>
        /// <param name="pagetype">نوع محتول</param>
        /// <param name="pageNumber">شماره صفحه</param>
        /// <param name="pageSize">اندازه صفحه</param>
        /// <returns></returns>
        Task<ApiResult<PagedNewsItemsViewModel>>
            GetPagedNewsItemsAsync(PageType pagetype,
            int pageNumber, int pageSize, int? groupId = null,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string title = null
            );

        /// <summary>
        /// حذف یک خبر
        /// حذف منطقی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult> RemoveNews(int id);
        Task<ApiResult<List<NewsList>>> GetLastNewsAsync(PageType pagetype, int top);
        Task<ApiResult<List<NewsCommentDto>>> GetPublishComments(int newsid);
        Task<ApiResult<NotificationDto>> AddOrUpdateNotifications(NotificationDto model, int userId);
        Task<ApiResult<NotificationInfo>> GetNotification(int id, int? companyId, int? userId, string userIp = "", bool forEdit = true);
        Task<ApiResult<List<NewsList>>> GetMostVisitedNews(PageType pagetype, int pageSize);
        Task<ApiResult<List<CitizensNotifactions>>> GetCitizenNotifications(int newsId);
        Task<ApiResult> RemoveCitizensNotifactions(int id);
        Task<ApiResult> AddCitizensNotifactions(CitizensNotifactionsDto model);
        Task<ApiResult<List<NewsList>>> GetLastNotifications(int top, int companyId);
        Task<ApiResult> AddGroupNotifactions(GroupsNotifactionsDto model);
    }






}
