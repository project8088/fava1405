using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.BaseEntity;
using Nikan.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services.BaseEntity
{
    public interface IEducationGroupService
    {

        Task<List<EducationGroup>> GetListByIdsAsync(int[] ids);
        Task<ApiResult<List<BaseDataModel>>> GetBaseListAsync(int? selected = null);
        Task<ApiResult<List<BaseDataModel>>> GetBaseListAsync(string query, int offset = 0, int count = 20);
        Task<List<BaseDataModel>> GetEducationGroups();


    }
    public class EducationGroupService : IEducationGroupService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<EducationGroup> _EducationGroupRepository;
        public EducationGroupService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _EducationGroupRepository = _uow.Set<EducationGroup>();
        }



        public async Task<ApiResult<List<BaseDataModel>>> GetBaseListAsync(int? selected = null)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {
                res.Data = await _EducationGroupRepository
                     .Where(w => w.IsDeleted!=true)
                     .OrderByDescending(o => o.Title).Select
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
                res.Data = await _EducationGroupRepository.Where(w => w.IsDeleted!=true && w.Title.Contains(query))
                       .OrderByDescending(o => o.Title).Skip(offset).Take(count).Select
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



        public Task<List<EducationGroup>> GetListByIdsAsync(int[] ids)
        {
            return _EducationGroupRepository.Where(i => ids.Contains(i.Id)).ToListAsync();

        }



        public Task<List<BaseDataModel>> GetEducationGroups()
        {
            return _EducationGroupRepository.Where(w => w.IsDeleted != true)
                       .OrderBy(o => o.Title).Select
                       (s => new BaseDataModel()
                       {

                           Text = s.Title,
                           Key = s.Id.ToString()
                       })
                       .ToListAsync();

        }






    }


}
