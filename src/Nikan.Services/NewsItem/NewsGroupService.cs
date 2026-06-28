using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.NewsItem;
using Nikan.ViewModel;
using Nikan.ViewModel.NewsItems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services 
{
    public interface INewsGroupService
    {
        /// <summary>
        /// اضافه یا ویرایش گروه خبر
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult<NewsGroupDto>> AddOrUpdate(NewsGroupDto model);
       
        
        
        /// <summary>
        /// دریافت همه گروههای خبری
        /// </summary>
        /// <returns></returns>
        Task<ApiResult<List<NewsGroupDto>>> GetAll();



        /// <summary>
        /// دریافت لیست گروههای خبری
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        Task<ApiResult<List<BaseDataModel>>> GetList(int? selected);
      
        
        /// <summary>
        /// دریافت اطلاعات یک گروه خبری
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<NewsGroupDto>> GetNewsGroup(int id);


        /// <summary>
        /// حذف منطقی گروه خبری
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ApiResult> Remove(int Id);
    }
    public class NewsGroupService: INewsGroupService
    {

        #region Field
        private readonly IUnitOfWork _uow;
        private readonly DbSet<News> _news;
        private readonly DbSet<NewsGroup> _newsGroup;
        #endregion
        #region Constractor

        public NewsGroupService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _news = _uow.Set<News>();
            _newsGroup = _uow.Set<NewsGroup>();

        }
        #endregion


        public async Task<ApiResult<NewsGroupDto>> AddOrUpdate(NewsGroupDto model)
        {
            var res = new ApiResult<NewsGroupDto>(true, ApiResultStatusCode.Success, new NewsGroupDto());
            try
            {
                if (model.Id == null)
                {
                    var add = new NewsGroup()
                    {
                        Description=model.Description,
                        ImageUrl=model.ImageUrl,
                        IsActive=model.IsActive==true,
                        PageType= Nikan.Common.GlobalEnum.PageType.NewsList,
                        Title=model.Title, 
                    };

                   await  _newsGroup.AddAsync(add);
                   await _uow.SaveChangesAsync();  
 

                }
                else
                {
                    var item =await _newsGroup.FirstOrDefaultAsync(w => w.Id == model.Id.Value);
                    if(item==null)
                    {
                        res.IsSuccess = false;
                        res.Messages = "رکوردی یافت نشد";
                        res.StatusCode = ApiResultStatusCode.NotFound;
                    }

                    item.IsActive = model.IsActive==true;
                    item.Title = model.Title;
                    item.Description = model.Description;
                    _newsGroup.Update(item);
                    await _uow.SaveChangesAsync(); 
                }

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }


        public async Task<ApiResult> Remove(int Id  )
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success,"با موفقیت حذف شده");
            try
            {
                 
                    var item = await _newsGroup.FirstOrDefaultAsync(w => w.Id == Id);
                    if (item == null)
                    {
                        res.IsSuccess = false;
                        res.Messages = "رکوردی یافت نشد";
                        res.StatusCode = ApiResultStatusCode.NotFound;
                    }

                    item.IsDeleted =true; 
                    _newsGroup.Update(item);
                    await _uow.SaveChangesAsync();
                

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }

        public async Task<ApiResult<List<NewsGroupDto>>> GetAll()
        {
            var res = new ApiResult<List<NewsGroupDto>>(true, ApiResultStatusCode.Success, new List<NewsGroupDto>());
            try
            {

                res.Data= await _newsGroup.Where(w => w.IsDeleted != true).Select(s => new NewsGroupDto()
                {
                    Id=s.Id,
                    IsDeleted=s.IsDeleted,
                    Description=s.Description,
                    ImageUrl=s.ImageUrl,
                    IsActive=s.IsActive,
                    Title=s.Title,
                    

                }).ToListAsync();
                    
                

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }


        public async Task<ApiResult<List<BaseDataModel>>> GetList(int? selected)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {

                res.Data= await _newsGroup.Where(w => w.IsDeleted != true).Select(s => new BaseDataModel()
                {
                    Description=s.Description,
                    Disabled=!s.IsActive,
                    Selected=s.Id== selected,
                    Text=s.Title,
                    Key=s.Id.ToString(),
                    


                }).ToListAsync();
                    
                

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }

        public async Task<ApiResult<NewsGroupDto>> GetNewsGroup(int id)
        {
            var res = new ApiResult<NewsGroupDto>(true, ApiResultStatusCode.Success, new NewsGroupDto());
            try
            {
                var item = await _newsGroup.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                }

                res.Data.Title = item.Title;
                res.Data.Id = item.Id;
                res.Data.ImageUrl = item.ImageUrl;
                res.Data.IsActive = item.IsActive;
                res.Data.Description = item.Description; 


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }



    }

}
