using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.BaseEntity;
 
using Nikan.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nikan.DomainClasses.Citizens;

namespace cle.Services.BaseEntity
{
    public interface ICityService
    {
        Task<ApiResult<List<City>>> GetListByIdsAsync(int[] ids);
        Task<ApiResult<List<BaseDataModel>>> GetBaseListAsync(string query, int offset, int count);
        Task<ApiResult<List<BaseDataModel>>> GetBaseListCites(int? selected = null);
        Task<ApiResult<List<BaseDataModel>>> GetCitesByParent(int parentId, int? selected = null);

        Task<ApiResult<List<BaseDataModel>>> GetProvinces();

        Task<ApiResult<List<BaseDataModel>>> GetAllCites(int? selected = null, bool showProvince = false);
        Task<ApiResult<List<BaseDataModel>>> GetNationality(int? selected);
    }

    public class CityService: ICityService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<City> _cityRepository;
        private readonly DbSet<Nationality> _nationality;



        public CityService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _cityRepository = _uow.Set<City>();
            _nationality = _uow.Set<Nationality>();
        }


        public async Task<ApiResult<List<BaseDataModel>>> GetBaseListCites(int? selected = null)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {
                res.Data = await _cityRepository.Where(w => w.IsActive && w.ParentId != null)
                      .OrderByDescending(o => o.IndexOrder).ThenBy(o => o.Title).Select
                      (s => new BaseDataModel()
                      {
                          Text = s.Title,
                          Key = s.Id.ToString(),
                          ParentValue = s.ParentId.ToString(),
                          Selected = s.Id == selected,
                          ParentText = s.Parent.Title

                      })
                      .ToListAsync();
            }
            catch (Exception)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;


        }

        public async Task<ApiResult<List<BaseDataModel>>> GetCitesByParent(int parentId, int? selected = null)
        {

            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {
                res.Data = await _cityRepository.Where(w => w.IsActive && w.ParentId == parentId)
                       .Select
                       (s => new BaseDataModel()
                       {
                           Text = s.Title,
                           Key = s.Id.ToString(),
                           ParentValue = s.ParentId.ToString(),
                          // Selected = s.Id == selected,
                           ParentText = s.Parent.Title,
                       }).OrderBy(x => x.Text)
                       .ToListAsync();
            }
            catch (Exception)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

           
            


        }


   

        public async Task<ApiResult<List<BaseDataModel>>> GetBaseListAsync(string query, int offset, int count)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {
                res.Data = await _cityRepository
                      .Where(w => w.IsActive && w.ParentId != null && w.Title.Contains(query))
                      .OrderByDescending(o => o.IndexOrder).ThenBy(o => o.Title).Skip(offset).Take(count).Select
                      (s => new BaseDataModel()
                      {
                          Text = s.Title,
                          Key = s.Id.ToString(),
                          ParentValue = s.ParentId.ToString(),
                          ParentText = s.Parent.Title,

                      })
                      .ToListAsync();
            }
            catch (Exception)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;
            

        }


        public async Task<ApiResult<List<City>>> GetListByIdsAsync(int[] ids)
        {
            var res = new ApiResult<List<City>>(true, ApiResultStatusCode.Success, new List<City>());
            try
            {
                res.Data=await _cityRepository.Where(i => ids.Contains(i.Id)).ToListAsync();
            }
            catch (Exception)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;
          

        }



        public async Task<ApiResult<List<BaseDataModel>>> GetProvinces()
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {
                res.Data= await _cityRepository.Where(w => w.IsActive && w.ParentId == null)
                                        .OrderByDescending(o => o.IndexOrder).ThenBy(o => o.Title).Select
                                        (s => new BaseDataModel()
                                        {
                                            Text = s.Title,
                                            Key = s.Id.ToString(),
                                            ParentValue = s.ParentId.ToString(),
                                            Selected = false,
                                            ParentText = s.Parent.Title

                                        })
                                        .ToListAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError; 

            }

            return res;



        }

        public async Task<ApiResult<List<BaseDataModel>>> GetAllCites(int? selected = null,   bool showProvince = false)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());


            try
            {
                res.Data = await _cityRepository.Where(w => w.IsActive && w.ParentId != null)
                      .OrderByDescending(o => o.IndexOrder).ThenBy(o => o.Title).Select
                      (s => new BaseDataModel()
                      {
                          Text = showProvince ? s.Title + " ( " + s.Parent.Title + " ) " : s.Title,
                          Key = s.Id.ToString(),
                          ParentValue = s.ParentId.ToString(),
                         //Selected = s.Id == selected,
                          ParentText = s.Parent.Title,

                      })
                      .ToListAsync();
            }
            catch (Exception)
            {

                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;

           
           
        }

        #region دریافت ملیت ها

        public async Task<ApiResult<List<BaseDataModel>>> GetNationality(int? selected)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            { 
                res.Data = await _nationality.Where(w => w.IsActive).Select(s => new BaseDataModel()
                {
                    Disabled = !s.IsActive,
                    Selected = s.Id == selected,
                    Text = s.Name,
                    Key = s.Id.ToString(),
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


        #endregion 






    }
}
