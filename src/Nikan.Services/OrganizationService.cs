
 
using Nikan.Common;
using Nikan.Common.Resource;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace cle.Services 
{

    public interface IOrganizationService
    {
         

        Task<ApiResult<OrganizationViewModel>> AddOrUpdate(OrganizationViewModel model, int userOwnerId); 
        Task<ApiResult<string>> Remove(string id); 
        Task<ApiResult<List<OrganizationViewModel>>> GetAll();
        Task<ApiResult<OrganizationViewModel>> GetItemForEdit(string id);
        Task<ApiResult<List<BaseDataModel>>> GetBaseList(string selected = null);
        Task<ApiResult<List<BaseDataModel>>> GetAllSupportCenter(string selected = null);
    }
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Organization> _OrganizationRepository;
       
        public OrganizationService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _OrganizationRepository = _uow.Set<Organization>();
           



        }


        #region IOrganizationService Members

        public async Task<ApiResult<List<OrganizationViewModel>>> GetAll()
        {
            var res = new ApiResult<List<OrganizationViewModel>>(true, ApiResultStatusCode.Success, new List<OrganizationViewModel>());

            try
            {
                res.Data = await _OrganizationRepository.Where(g => g.IsDeleted != true).Select(s => new OrganizationViewModel()
                {
                    OrganizationName=s.OrganizationName,
                    

                    Description=s.Description,
                    IndexOrder=s.IndexOrder,
                    IsActive=s.IsActive, 
                    Id=s.OrganizationId,
                    ThumbUrl=s.ThumbUrl,
                    CardDistributionCenters=s.CardDistributionCenters,
                    SupportCenters=s.SupportCenters
                    
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


        public async Task<ApiResult<List<BaseDataModel>>> GetBaseList(string selected = null)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {
                res.Data = await _OrganizationRepository.Where(g => g.IsDeleted != true

                ).Select(s => new BaseDataModel()
                {
                    Description = s.Description,
                    Key = s.OrganizationId,
                    Text = s.OrganizationName, 
                    Selected = s.OrganizationId == selected


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

        public async Task<ApiResult<List<BaseDataModel>>> GetAllSupportCenter(string selected = null)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {
                res.Data = await _OrganizationRepository.Where(g =>g.SupportCenters==true && g.IsDeleted != true

                ).Select(s => new BaseDataModel()
                {
                    Description = s.Description,
                    Key = s.OrganizationId,
                    Text = s.OrganizationName,
                    Selected = s.OrganizationId == selected


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


        public async Task<ApiResult<OrganizationViewModel>> GetItemForEdit(string id) 
        {
            var res = new ApiResult<OrganizationViewModel>(true, ApiResultStatusCode.Success, new OrganizationViewModel());

            try
            {

                var item =await _OrganizationRepository.FirstOrDefaultAsync(w => w.OrganizationId == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                }

                res.Data = new OrganizationViewModel()
                {
                    Description = item.Description,
                    Id = item.OrganizationId,
                    OrganizationName = item.OrganizationName,
                    ThumbUrl = item.ThumbUrl,
                    CardDistributionCenters = item.CardDistributionCenters,
                     SupportCenters = item.SupportCenters,
                    IsActive = item.IsActive,
                    IndexOrder = item.IndexOrder,
                  

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

         
        public async Task<ApiResult<OrganizationViewModel>> AddOrUpdate(OrganizationViewModel model,int userOwnerId)
        {

            var res = new ApiResult<OrganizationViewModel>(true, ApiResultStatusCode.Success, new OrganizationViewModel());
            try
            {
               if (string.IsNullOrEmpty(model.Id))
                {
                    var newmodel = new Organization
                    {

                        Description = model.Description,
                        OrganizationId = Guid.NewGuid().ToString(),
                        OrganizationName = model.OrganizationName,
                        ThumbUrl = model.ThumbUrl,
                        UserOwnerId = userOwnerId,
                        IndexOrder = model.IndexOrder,
                        IsActive = model.IsActive,
                        SupportCenters=model.SupportCenters,
                        CardDistributionCenters=model.CardDistributionCenters,
                       


                    };

                    await _OrganizationRepository.AddAsync(newmodel);
                    await _uow.SaveChangesAsync();

                }
                else
                {

                    var item = await _OrganizationRepository.FirstOrDefaultAsync(w => w.OrganizationId == model.Id);
                    if (item == null)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        res.Messages = "رکوردی یافت نشد";

                    }

                    item.Description = model.Description;
                    item.IndexOrder = model.IndexOrder;
                    item.IsActive = model.IsActive;
                    item.OrganizationName = model.OrganizationName;

                    item.SupportCenters = model.SupportCenters;
                    item.CardDistributionCenters = model.CardDistributionCenters;



                    item.ThumbUrl = model.ThumbUrl; 
                    _OrganizationRepository.Update(item);
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

         

        public async Task<ApiResult<string>> Remove(string  id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            try
            {
                var item = await _OrganizationRepository.FirstOrDefaultAsync(w => w.OrganizationId == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                }


              
                item.IsDeleted = true;
                _OrganizationRepository.Update(item);
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



  

        public void SaveChange()
        {
            _uow.SaveChanges();
        }

        #endregion









        #region private Methode

        private bool CanDelete(string id, out List<GlobalError> errors)
        {
            errors = new List<GlobalError>();
            bool flag = true;

            return flag;

        }

        #endregion



















    }




}