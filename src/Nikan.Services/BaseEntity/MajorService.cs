using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.Job;
using Nikan.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services.BaseEntity
{
    public interface IMajorService
    {

        Task<List<Major>> GetListByIdsAsync(int[] ids);
        Task<ApiResult<List<BaseDataModel>>> GetBaseListAsync(int? selected = null);
        Task<ApiResult<List<BaseDataModel>>> GetBaseListAsync(string query, int offset = 0, int count = 20);
    }
    public class MajorService : IMajorService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Major> _majorRepository;
        public MajorService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _majorRepository = _uow.Set<Major>();
        }



        public async Task<ApiResult<List<BaseDataModel>>> GetBaseListAsync(int? selected = null)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {
                res.Data = await _majorRepository
                     .Where(w => w.IsActive)
                     .OrderByDescending(o => o.IndexOrder).Select
                     (s => new BaseDataModel()
                     {
                         Text = s.Title,
                         Key = s.Id.ToString(),
                         //Selected = s.Id == selected
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
        public async Task<ApiResult<List<BaseDataModel>>> GetBaseListAsync(string query, int offset = 0, int count = 20)
        {

            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {
                res.Data = await _majorRepository.Where(w => w.IsActive && w.Title.Contains(query))
                       .OrderByDescending(o => o.IndexOrder).Skip(offset).Take(count).Select
                       (s => new BaseDataModel()
                       {
                           Text = s.Title,
                           Key = s.Id.ToString(),

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



        public Task<List<Major>> GetListByIdsAsync(int[] ids)
        {
            return _majorRepository.Where(i => ids.Contains(i.Id)).ToListAsync();

        }










    }


}
