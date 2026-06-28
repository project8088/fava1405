using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.BaseEntity;
using Nikan.DomainClasses.NewsItem;
using Nikan.ViewModel.NewsItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services
{

    public  class WebPageService : IWebPageService
    {
        #region Field
        private readonly IUnitOfWork _uow;
        private readonly DbSet<WebPage> _page;
        
        #endregion
        #region Constractor

        public WebPageService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _page = _uow.Set<WebPage>();
            
            
        }
        #endregion



    
        public async Task<ApiResult<WebPageDto>> AddOrUpdate(WebPageDto model,int userId)
        {

            var res = new ApiResult<WebPageDto>(true, ApiResultStatusCode.Success, new WebPageDto());
            try
            {
                if(model.Id==null) 
                {
                    var item = new WebPage
                    {
                        Body=model.Body,
                        Clicks=0,
                        CreatedById= userId,
                        CreatedOnDate=DateTime.Now,
                        Description=model.Description,
                        LastModifiedOnDate=DateTime.Now,
                        ModifiedById=userId,
                        SeoDescription=model.SeoDescription,
                        SeoTags=model.SeoTags,
                        Slug=model.Slug,
                        Title=model.Title, 
                    };

                    item.AttachmentImageGroup = Guid.NewGuid().ToString();
                    item.AttachmentFileGroup = Guid.NewGuid().ToString();
                    await _page.AddAsync(item);
                    await _uow.SaveChangesAsync();

                }
                else
                {

                    var item =await _page.FirstOrDefaultAsync(w => w.Id == model.Id);
                    if(item==null)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        res.Messages = "رکوردی یافت نشد";

                    }

                    item.Title = model.Title;
                    item.Body = model.Body;  
                    item.Description = model.Description; 
                    item.SeoTags = model.SeoTags;
                    item.SeoDescription = model.SeoDescription;
                    item.ModifiedById = userId;
                    item.LastModifiedOnDate = DateTime.Now;
                    
                    item.Slug = model.Slug;
                    _page.Update(item);
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
        public async Task<ApiResult<WebPageInfo>> GetItems(int id, bool forEdit = true)
        {

            ApiResult<WebPageInfo> res = new ApiResult<WebPageInfo>(true, ApiResultStatusCode.Success, null);

            try
            {
                var item =await _page.Include(i=>i.CreatedBy).Include(i=>i.ModifiedBy).FirstOrDefaultAsync(w => w.Id == id); 
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "  یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                if (item.IsDeleted)
                {
                    res.IsSuccess = false;
                    res.Messages = "   حذف شده است";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                var data = new WebPageInfo()
                {
                    Body = item.Body,
                    Clicks = item.Clicks,
                    CreatedById = item.CreatedById,
                    CreatedBy = item.CreatedBy.DisplayName,
                    CreatedOnDate = item.CreatedOnDate,
                    Slug = item.Slug,
                    Title = item.Title,
                    Description = item.Description,
                    Id = item.Id,
                    LastModifiedOnDate = item.LastModifiedOnDate,
                    ModifiedBy = item.ModifiedBy == null ? "" : item.ModifiedBy.DisplayName,
                    ModifiedById = item.ModifiedById,
                    SeoDescription = item.SeoDescription,
                    SeoTags = item.SeoTags,


                };
                if(!forEdit)
                {
                    var click = item.Clicks + 1;
                    item.Clicks = click;
                    _page.Update(item);
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
        public async Task<ApiResult<WebPageInfo>> GetWebPageByPath(string path)
        {
            

            ApiResult<WebPageInfo> res = new ApiResult<WebPageInfo>(true, ApiResultStatusCode.Success, null);

            try
            {
                var item = await _page.Include(i => i.CreatedBy).Include(i => i.ModifiedBy).FirstOrDefaultAsync(w => w.Slug == path);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "  یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                if (item.IsDeleted)
                {
                    res.IsSuccess = false;
                    res.Messages = "   حذف شده است";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                var data = new WebPageInfo()
                {
                    Body = item.Body,
                    Clicks = item.Clicks,
                    CreatedById = item.CreatedById,
                    CreatedBy = item.CreatedBy.DisplayName,
                    CreatedOnDate = item.CreatedOnDate,
                    Slug = item.Slug,
                    Title = item.Title,
                    Description = item.Description,
                    Id = item.Id,
                    LastModifiedOnDate = item.LastModifiedOnDate,
                    ModifiedBy = item.ModifiedBy == null ? "" : item.ModifiedBy.DisplayName,
                    ModifiedById = item.ModifiedById,
                    SeoDescription = item.SeoDescription,
                    SeoTags = item.SeoTags,
                    AttachmentFileGroup=item.AttachmentFileGroup,
                    AttachmentImageGroup=item.AttachmentImageGroup


                };
                var click = item.Clicks + 1;
                item.Clicks = click;
                _page.Update(item);
                await _uow.SaveChangesAsync();



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

        public async Task<ApiResult<List<BaseData>>> GetAllPagePath()
        {

            var res = new ApiResult<List<BaseData>>(true, ApiResultStatusCode.Success, null); 
            try
            {
                var data =await _page.Where(w => w.IsDeleted != true).Select(s => new BaseData()
                {
                    Id=s.Id,
                    Description=s.Slug,
                    Key=s.Id.ToString(),
                    Text= s.Title

                }).ToListAsync();
                  
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

        public async Task<ApiResult<WebPageItemsViewModel>>
            GetPagedWebPageItemsAsync(
            int pageNumber, int pageSize,  
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string title = null
            )
        {
             var offset = (pageNumber ) * pageSize ;
            var res = new ApiResult<WebPageItemsViewModel>(true, ApiResultStatusCode.Success, null);

            try
            {
                var query =   _page.Where(w => w.IsDeleted!=true );
                 
                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(w => EF.Functions.Like(w.Title, "%[("+ title + ")]%"));
                }

                if (FromDate != null)
                {
                    query = query.Where(w => w.CreatedOnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.CreatedOnDate <= ToDate);
                }

                var list =await query.Select(s => new WebPageInfo()
                {
                    Body=s.Body,
                    Clicks=s.Clicks,
                    CreatedById=s.CreatedById,
                    CreatedBy=s.CreatedBy.DisplayName,
                    CreatedOnDate=s.CreatedOnDate,
                    Slug=s.Slug,
                    Title=s.Title,
                    Description=s.Description,
                    Id=s.Id,
                    LastModifiedOnDate=s.LastModifiedOnDate,
                    ModifiedBy=s.ModifiedBy==null ? "" :s.ModifiedBy.DisplayName,
                    ModifiedById=s.ModifiedById,
                    SeoDescription=s.SeoDescription,
                    SeoTags=s.SeoTags,  
                    AttachmentFileGroup=s.AttachmentFileGroup,
                    AttachmentImageGroup=s.AttachmentImageGroup
                }).Skip(offset).Take(pageSize).ToListAsync();

                res.Data = new WebPageItemsViewModel
                {
                    TotalItems = await query.CountAsync()  ,
                    WebPages = list
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



        public async Task<ApiResult> Remove(int id)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت حذف گردید");
            var item = await _page.FirstOrDefaultAsync(w => w.Id == id);
            if (item == null)
            {

                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.NotFound;
                res.Messages = "رکوردی یافت نشد";

            }

            item.IsDeleted = true;
            //نال کردن کلیه فرزندان

            _page.Update(item);
            await _uow.SaveChangesAsync();
            return res;
        }








    }

    public interface IWebPageService
    {
         
        
        
        Task<ApiResult<WebPageDto>> AddOrUpdate(WebPageDto model, int userId);
        Task<ApiResult<List<BaseData>>> GetAllPagePath();
        Task<ApiResult<WebPageInfo>> GetItems(int id, bool forEdit = true); 
        /// <summary>
        /// دریافت لیست
        /// </summary>
        /// <param name="pagetype">نوع محتول</param>
        /// <param name="pageNumber">شماره صفحه</param>
        /// <param name="pageSize">اندازه صفحه</param>
        /// <returns></returns>
        Task<ApiResult<WebPageItemsViewModel>>
            GetPagedWebPageItemsAsync( 
            int pageNumber, int pageSize,  
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string title = null 
            );
        Task<ApiResult<WebPageInfo>> GetWebPageByPath(string path);

        Task<ApiResult> Remove(int id);
        
    }






}
