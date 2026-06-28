 
using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cle.Services
{

    public interface IOrganizationalUnitService
    {
        Task<ApiResult<OrganizationalUnitDto>> AddOrUpdate(OrganizationalUnitDto model);
        Task<ApiResult<string>> Remove(string id);
        Task<ApiResult<List<OrganizationalUnitViewModel>>> GetAll(string orgId);
        Task<ApiResult<OrganizationalUnitViewModel>> GetItemForEdit(string id);
        /// <summary>
        /// دریافت لیست واحدها
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        Task<ApiResult<List<BaseDataModel>>> GetBaseList(string selected = null);
        Task<ApiResult<List<BaseDataModel>>> GetBaseUnitOrganList(string orgnId);
        Task<ApiResult<List<BaseDataModel>>> GetAllCardDeliveryCenters(string selected = null);
        Task<ApiResult<List<OrganizationUnitGroupsInfo>>> GetAllUnitGroups(string unitId);
        Task<ApiResult> RemoveUnitGroup(OrganizationUnitGroups model);
        Task<ApiResult> AddGroupToUnitGroup(OrganizationUnitGroups model);
    }
    public class OrganizationalUnitService : IOrganizationalUnitService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<OrganizationalUnit> _OrganizationalUnitRepository;
        private readonly DbSet<OrganizationalUnitGroups> _OrganizationalUnitGroups;




        public OrganizationalUnitService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _OrganizationalUnitRepository = _uow.Set<OrganizationalUnit>();
            _OrganizationalUnitGroups = _uow.Set<OrganizationalUnitGroups>();

        }


        public async Task<ApiResult<List<BaseDataModel>>> GetBaseList(string selected = null )
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {

                res.Data = await _OrganizationalUnitRepository.Where(g => g.IsDeleted != true 
                
                ).Select(s => new BaseDataModel()
                {
                    Description=s.Description,
                    Key=s.Id,
                    Text=s.Name,
                    ParentText=s.Organization.OrganizationName,
                    OrganizationId=s.OrganizationId,
                    ParentValue=s.OrganizationId,
                    Selected=s.Id== selected


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

        public async Task<ApiResult<List<BaseDataModel>>> GetAllCardDeliveryCenters(string selected = null)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {
                res.Data = await _OrganizationalUnitRepository.Where(g =>g.Organization.CardDistributionCenters==true  && g.IsDeleted != true

                ).Select(s => new BaseDataModel()
                {
                    Description = s.Description,
                    Key = s.Id,
                    Text = s.Name,
                    ParentText = s.Organization.OrganizationName,
                    OrganizationId = s.OrganizationId,
                    ParentValue = s.OrganizationId,
                    Selected = s.Id == selected


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




        public async Task<ApiResult<List<BaseDataModel>>> GetBaseUnitOrganList(string orgnId )
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {
                res.Data = await _OrganizationalUnitRepository.Where(g => g.IsDeleted != true
                && g.IsActive
                && g.OrganizationId== orgnId
                ).Select(s => new BaseDataModel()
                {
                    Description = s.Description,
                    Key = s.Id,
                    Text = s.Name,
                    ParentText = s.Organization.OrganizationName,
                    OrganizationId = s.OrganizationId,
                    ParentValue = s.OrganizationId,
                    

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



        public async Task<ApiResult<List<OrganizationalUnitViewModel>>> GetAll(string organId)
        {
            var res = new ApiResult<List<OrganizationalUnitViewModel>>(true, ApiResultStatusCode.Success, new List<OrganizationalUnitViewModel>());

            try
            {
                res.Data = await _OrganizationalUnitRepository.Where(g => g.IsDeleted != true
                && g.OrganizationId== organId 
                ).Select(s => new OrganizationalUnitViewModel()
                {
                    Id=s.Id,
                    
                    Description = s.Description,
                    IndexOrder = s.IndexOrder,
                    IsActive = s.IsActive,
                    IsDeleted = s.IsDeleted,
                    OrganizationId = s.OrganizationId,
                    ThumbUrl = s.ThumbUrl,
                    Name=s.Name,
                    Organization=s.Organization.OrganizationName 
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



        public async Task<ApiResult<OrganizationalUnitViewModel>> GetItemForEdit(string id)
        {
            var res = new ApiResult<OrganizationalUnitViewModel>(true, ApiResultStatusCode.Success, new OrganizationalUnitViewModel());

            try
            {

                var item = await _OrganizationalUnitRepository.Include(i=>i.Organization).FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                }

                res.Data = new OrganizationalUnitViewModel()
                {
                    Description = item.Description,
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    ThumbUrl = item.ThumbUrl,
                    IndexOrder = item.IndexOrder,
                    IsActive = item.IsActive,
                    OrganizationId = item.OrganizationId,
                    Organization=item.Organization.OrganizationName 
                   
                };

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";


            }

            return res;



        }



        public async Task<ApiResult<OrganizationalUnitDto>> AddOrUpdate(OrganizationalUnitDto model)
        {

            var res = new ApiResult<OrganizationalUnitDto>(true, ApiResultStatusCode.Success, new OrganizationalUnitDto());
            try
            {
                if (string.IsNullOrWhiteSpace(model.Id) )
                {
                    var item = new OrganizationalUnit
                    {
                        Description = model.Description,
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        ThumbUrl = model.ThumbUrl,
                        IndexOrder = model.IndexOrder,
                        OrganizationId = model.OrganizationId,
                        IsActive = model.IsActive==true,
                        


                    };


                    await _OrganizationalUnitRepository.AddAsync(item);
                    await _uow.SaveChangesAsync();

                }
                else
                {

                    var item = await _OrganizationalUnitRepository.FirstOrDefaultAsync(w => w.Id == model.Id);
                    if (item == null)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        res.Messages = "رکوردی یافت نشد";

                    }

                    item.Description = model.Description;
                    item.IndexOrder = model.IndexOrder;
                    item.IsActive = model.IsActive==true;
                    item.Name = model.Name;
                    item.OrganizationId = model.OrganizationId;
                    item.ThumbUrl = model.ThumbUrl;
                    _OrganizationalUnitRepository.Update(item);
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

         

        public async Task<ApiResult<string>> Remove(string id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "حذف واحد با موفقیت صورت گرفت");
            try
            {
                var item = await _OrganizationalUnitRepository.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                }



                item.IsDeleted = true;
                _OrganizationalUnitRepository.Update(item);
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




        #region  UnitGroup
        public async Task<ApiResult> AddGroupToUnitGroup(OrganizationUnitGroups model)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت اضافه گردید");
            try
            {
                if (await _OrganizationalUnitGroups.AnyAsync(w => w.OrganizationalUnitId == model.UnitId && w.GroupId == model.GroupId))
                {
                    res.IsSuccess = false;
                    res.Messages = "قبلا اضافه شده است";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;

                }


                var user = await _OrganizationalUnitGroups.AddAsync(new  OrganizationalUnitGroups()
                {
                    GroupId = model.GroupId,
                    OrganizationalUnitId = model.UnitId

                });

                await _uow.SaveChangesAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.NotFound;

            }


            return res;

        }
        public async Task<ApiResult> RemoveUnitGroup(OrganizationUnitGroups model)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "با موفقیت حذف گردید");
            try
            {

                var item = await _OrganizationalUnitGroups.FirstOrDefaultAsync(w => w.OrganizationalUnitId == model.UnitId 
                && w.GroupId == model.GroupId);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.Data = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                _OrganizationalUnitGroups.Remove(item);
                await _uow.SaveChangesAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.NotFound;

            }


            return res;

        }
        public async Task<ApiResult<List<OrganizationUnitGroupsInfo>>> GetAllUnitGroups(string unitId)
        {
            var res = new ApiResult<List<OrganizationUnitGroupsInfo>>(true, ApiResultStatusCode.Success, new List<OrganizationUnitGroupsInfo>());

            try
            {
                res.Data = await _OrganizationalUnitGroups.Where(w => w.OrganizationalUnitId == unitId)
                     .Select
                      (s => new OrganizationUnitGroupsInfo()
                      { 
                          Id = s.Id,
                          Group = s.Group.GroupName,
                          GroupId = s.GroupId,
                          Unit=s.OrganizationalUnit.Name,
                          UnitId=s.OrganizationalUnitId,
                          

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




        #endregion






    }




}