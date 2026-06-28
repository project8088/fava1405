using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.BaseEntity;
using Nikan.DomainClasses.UserDocuments;
using Nikan.ViewModel;
using Nikan.ViewModel.UserDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services.BaseEntity
{
    public interface IUserDocumentGroupService
    {
        Task<ApiResult> AddOrUpdate(UserDocumentGroupDto model);
        Task<ApiResult<List<BaseDataModel>>> GetBaseListAsync(int? selected = null);
        Task<ApiResult<List<BaseDataModel>>> GetBaseListAsync(string query, int offset = 0, int count = 20);
        Task<ApiResult<UserDocumentGroupInfo>> GetGroupInfo(int serviceId);
        Task<ApiResult<string>> Remove(int id);
    }
    public class UserDocumentGroupService : IUserDocumentGroupService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserDocumentGroup> _docGroups;
        public UserDocumentGroupService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _docGroups = _uow.Set<UserDocumentGroup>();
        }




        public async Task<ApiResult<List<BaseDataModel>>> GetBaseListAsync(int? selected = null)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {
                res.Data = await _docGroups
                     .Where(w => w.IsActive )
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
                res.Data = await _docGroups.Where(w => w.IsActive  && w.Title.Contains(query))
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
        public async Task<ApiResult> AddOrUpdate(UserDocumentGroupDto model )
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success, "");
            try
            {
                if (model.Id == null)
                {

                    var maxId = await _docGroups.MaxAsync(m => m.Id);
                    var item = new UserDocumentGroup
                    {
                        Id = maxId + 1, 
                        Description = model.Description,
                        IndexOrder=model.IndexOrder,
                        IsActive=model.IsActive,
                        Title=model.Title,
                        
                    };


                    await _docGroups.AddAsync(item);
                    await _uow.SaveChangesAsync();

                }
                else
                {

                    var item = await _docGroups.FirstOrDefaultAsync(w => w.Id == model.Id);
                    if (item == null)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        res.Messages = "رکوردی یافت نشد";
                        return res;
                    }

                    item.IndexOrder = model.IndexOrder;
                    item.Description = model.Description;
                    item.IsActive = model.IsActive;
                    item.Title = model.Title;
                    _docGroups.Update(item);
                    await _uow.SaveChangesAsync();

                }



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

                return res;
            }


            return res;

        }
        public async Task<ApiResult<UserDocumentGroupInfo>> GetGroupInfo(int serviceId)
        {
            var res = new ApiResult<UserDocumentGroupInfo>(true, ApiResultStatusCode.Success, new UserDocumentGroupInfo(), "");
            try
            {
                var data = await _docGroups.Where(w => w.Id == serviceId).Select(s => new UserDocumentGroupInfo()
                {
                    Description=s.Description,
                    Id=s.Id,
                    IndexOrder=s.IndexOrder,
                    Title=s.Title,
                    IsActive=s.IsActive

                }).FirstOrDefaultAsync();
                if (data == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "اطلاعاتی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                }

                res.Data = data;
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;


        }
        public async Task<ApiResult<string>> Remove(int id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            try
            {
                var item = await _docGroups.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                } 
                item.IsDeleted = true;
                _docGroups.Update(item);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";
                return res;
            }


            return res;
        }





    }


}
