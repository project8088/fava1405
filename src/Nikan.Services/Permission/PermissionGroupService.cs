using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.Permissions;
using Nikan.ViewModel;
using Nikan.ViewModel.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikan.Services.Permissions
{

    public interface IPermissionGroupService
    {
        Task<ApiResult<PermissionGroupDto>> AddOrUpdate(PermissionGroupDto model);
        Task<ApiResult<List<PermissionGroupDto>>> GetAll();
        Task<ApiResult<PermissionGroupDto>> GetGroup(int id);
        Task<ApiResult<List<BaseDataModel>>> GetGroups(int? selected);
        Task<ApiResult<string>> Remove(int Id);
    }
    public class PermissionGroupService : IPermissionGroupService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<PermissionGroup> _group;

        public PermissionGroupService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _group = _uow.Set<PermissionGroup>();
        }



 


        public async Task<ApiResult<List<BaseDataModel>>> GetGroups(int? selected)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {

                res.Data = await _group.Select(s => new BaseDataModel()
                { 
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


        public async Task<ApiResult<List<PermissionGroupDto>>> GetAll()
        {
            var res = new ApiResult<List<PermissionGroupDto>>(true, ApiResultStatusCode.Success, new List<PermissionGroupDto>());
            try
            {

                res.Data = await _group.Select(s => new PermissionGroupDto()
                {
                    Name = s.Name,
                    Id = s.Id,
                   
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
         
        public async Task<ApiResult<PermissionGroupDto>> AddOrUpdate(PermissionGroupDto model )
        {

            var res = new ApiResult<PermissionGroupDto>(true, ApiResultStatusCode.Success, new PermissionGroupDto());
            try
            {
                if (model.Id == null || model.Id.Value == 0)
                {
                    var item = new PermissionGroup()
                    {
                        Name=model.Name 
                    };
                    await _group.AddAsync(item);
                    await _uow.SaveChangesAsync();

                }
                else
                {

                    var item = _group.Find(model.Id);
                    if(item==null)
                    {
                        res.IsSuccess = false;
                        res.Messages = "رکوردی یافت نشد";
                        res.StatusCode = ApiResultStatusCode.ServerError;
                        return res;
                    }
                    item.Name = model.Name;  
                    _group.Update(item);
                    await _uow.SaveChangesAsync();
                }
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;


        }
 
        public async Task<ApiResult<string>> Remove(int Id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت حذف شده");
            try
            {

                var item = await _group.FirstOrDefaultAsync(w => w.Id == Id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

               
                _group.Remove(item);
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


        public async Task<ApiResult<PermissionGroupDto>> GetGroup(int id)
        {
            var res = new ApiResult<PermissionGroupDto>(true, ApiResultStatusCode.Success, new PermissionGroupDto());
            try
            {

                res.Data = await _group.Where(w=>w.Id== id).Select(s => new PermissionGroupDto()
                {
                    Name = s.Name,
                    Id = s.Id,

                }).FirstOrDefaultAsync(); 
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
