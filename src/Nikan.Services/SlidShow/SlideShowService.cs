using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.ViewModel.SlidShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikan.Services.SlidShow
{
    public interface ISlideShowService
    {
        Task<ApiResult<SlideShowDto>> AddOrUpdate(SlideShowDto model, int userId);
        Task<ApiResult<List<SlideShowDto>>> GetAll(bool? isActive = null);
        Task<ApiResult<SlieShowInfo>> GetSlidShow(int id);
        Task<ApiResult<string>> Remove(int id);
    }
    public class SlideShowService : ISlideShowService
    {


        #region Ctor
        private readonly IUnitOfWork _uow;
        private readonly DbSet<SlideShow> _slideShow;

        public SlideShowService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _slideShow = _uow.Set<SlideShow>();
        }

        #endregion

        public async Task<ApiResult<List<SlideShowDto>>> GetAll( bool? isActive = null)
        {
            var res = new ApiResult<List<SlideShowDto>>(true, ApiResultStatusCode.Success, new List<SlideShowDto>());
            try
            {
                var query = _slideShow.AsQueryable();
                if (isActive.HasValue)
                {
                    query = query.Where(s => s.IsActive == isActive);
                }
                  

                res.Data = await query.Select(s => new SlideShowDto()
                {
                    Description = s.Description,
                    Caption=s.Caption,
                    Id=s.Id,
                    ImageUrl=s.ImageUrl,
                    IndexOrder=s.IndexOrder,
                    IsActive=s.IsActive,
                    Url=s.Url

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
        public async Task<ApiResult<SlieShowInfo>> GetSlidShow(int id)
        {
            var res = new ApiResult<SlieShowInfo>(true, ApiResultStatusCode.Success, new SlieShowInfo());
            try
            {
                var item = await _slideShow.Include(i => i.CreatedBy).FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;
                }


                res.Data = new SlieShowInfo()
                {
                    Description = item.Description,
                    Caption = item.Caption,
                    Id = item.Id,
                    ImageUrl = item.ImageUrl,
                    IndexOrder = item.IndexOrder,
                    IsActive = item.IsActive,
                    Url = item.Url,
                    CreatedBy=item.CreatedBy.DisplayName,
                    CreatedById=item.CreatedById,
                    CreatedOnDate=item.CreatedOnDate,
                    
                };



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }
        public async Task<ApiResult<SlideShowDto>> AddOrUpdate(SlideShowDto model, int userId)
        {

            var res = new ApiResult<SlideShowDto>(true, ApiResultStatusCode.Success, new SlideShowDto());
            try
            {
                if (model.Id == null)
                {
                    var item = new SlideShow
                    {
                        Caption=model.Caption,
                        CreatedById=userId,
                        CreatedOnDate=DateTime.Now,
                        Description=model.Description,
                        ImageUrl=model.ImageUrl,
                        IndexOrder=model.IndexOrder,
                        IsActive=model.IsActive,
                        Url=model.Url,
                        
                    };


                    await _slideShow.AddAsync(item);
                    await _uow.SaveChangesAsync();

                }
                else
                {

                    var item = await _slideShow.FirstOrDefaultAsync(w => w.Id == model.Id);
                    if (item == null)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        res.Messages = "رکوردی یافت نشد";
                        return res;

                    }

                    item.Caption = model.Caption;
                    item.Description = model.Description;
                    item.IsActive = model.IsActive;
                    item.ImageUrl = model.ImageUrl;
                    item.IndexOrder = model.IndexOrder;
                    item.Description = model.Description;
                    item.IsActive = model.IsActive;
                    item.Url = model.Url; 
                    _slideShow.Update(item);
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
        public async Task<ApiResult<string>> Remove(int id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            try
            {
                var item = await _slideShow.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                }
                 
                _slideShow.Remove(item);
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
         
    }
}
