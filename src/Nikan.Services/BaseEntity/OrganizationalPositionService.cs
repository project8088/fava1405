
using Nikan.Common;
using Nikan.Common.Resource;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.BaseEntity;
using Nikan.DomainClasses.Faq;
using Nikan.ViewModel;
using Nikan.ViewModel.Faq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nikan.DataLayer.Context;

namespace cle.Services.BaseEntity
{
    

    public interface IOrganizationalPositionService
    {


        #region AddOrUpdate

        OrganizationalPosition AddOrUpdate(OrganizationalPositionViewModel model, out  List<GlobalError> errors);
        Task<CommandResult<OrganizationalPositionViewModel>> AddOrUpdateAsync(OrganizationalPositionViewModel model);
        #endregion
        #region Remove

        Task<CommandResult<bool>> RemoveAsync(int id );
        Task<CommandResult<bool>> DeleteAsync(int id );

        #endregion
     

        #region Get
        IEnumerable<OrganizationalPosition> GetAll();
        Task<List<OrganizationalPosition>> GetAllAsync();
        IEnumerable<OrganizationalPosition> GetAllActive(string OrgId);
        OrganizationalPosition GetItem(int id);
        OrganizationalPositionViewModel GetItemForEdit(int id);
        Task<OrganizationalPositionViewModel> GetItemForEditAsync(int id);
        List<SelectListItem> GetAllGroupForUser(int? selected);
        Task<List<SelectListItem>> GetAllGroupForAdminAsync(int? selected);
        #endregion

        void SaveChange();
        Task<ApiResult<List<BaseDataModel>>> GetPositionListAsync();
    }
    public class      OrganizationalPositionService : IOrganizationalPositionService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<OrganizationalPosition> _OrganizationalPositionRepository;

        public OrganizationalPositionService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _OrganizationalPositionRepository = _uow.Set<OrganizationalPosition>(); 
        }




        #region IOrganizationalPositionService Members
        public IEnumerable<OrganizationalPosition> GetAll()
        {
            var models = _OrganizationalPositionRepository 
               .OrderBy(g => g.IndexOrder)
               .ThenByDescending(g => g.Id);

            return models;
        }
        public async  Task<List<OrganizationalPosition>> GetAllAsync()
        {
            var list =await  _OrganizationalPositionRepository 
               .OrderBy(g => g.IndexOrder)
               .ThenByDescending(g => g.Id).ToListAsync();

            return list;
        }

        public async Task<List<SelectListItem>> GetAllGroupForAdminAsync(int? selected)
        {
            string[] errorMessages = { };
            try
            {
                var list33 = await _OrganizationalPositionRepository .ToListAsync();


                var list=  list33.Select(s => new SelectListItem()
                {
                    Text =  s.IsActive  ? s.Name : s.Name + "(غیرفعال)",
                    Value = s.Id.ToString(),
                    Disabled=!s.IsActive,
                    Selected = s.Id == selected
                }).ToList();


                return list;
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                 
            }

            return null;
        }

        public List<SelectListItem> GetAllGroupForUser(int? selected)
        {
            return _OrganizationalPositionRepository.Where(w=>w.IsActive  ).Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Id.ToString(),
                Selected = s.Id == selected
            }).ToList();
        }





      


        public IEnumerable<OrganizationalPosition> GetAllActive(string OrgId)
        {
             var models = _OrganizationalPositionRepository.Where(g =>   g.IsActive          )
                .OrderByDescending(g => g.Name)
                .ThenByDescending(g => g.Id);

            return models;
        }

         
        public OrganizationalPosition GetItem(int id)
        {
            var model = _OrganizationalPositionRepository.Find(id);
            return model;
        }

        public OrganizationalPositionViewModel GetItemForEdit(int id)
        {
            var model = _OrganizationalPositionRepository.Find(id);
            if (model == null)
                return null;
            var item = new OrganizationalPositionViewModel()
            {
               
               
                IndexOrder=model.IndexOrder,
                IsActive=model.IsActive, 
                Name=model.Name,
             
                Id=model.Id, 

            };
            return item;
        }

       public async Task<OrganizationalPositionViewModel> GetItemForEditAsync(int id) 
        {
            var model =await _OrganizationalPositionRepository.FindAsync(id);
            if (model == null)
                return null;
            var item = new OrganizationalPositionViewModel()
            {

               
                IndexOrder = model.IndexOrder,
                IsActive = model.IsActive,
                Name = model.Name,
              
                Id = model.Id,

            };
            return item;
        }









    public OrganizationalPosition AddOrUpdate(OrganizationalPositionViewModel model, out  List<GlobalError> errors)
        {
            errors = new List<GlobalError>();

            try
            {
                if (model.Id == null || model.Id.Value == 0)
                {
                    var item = new OrganizationalPosition()
                    {
                        
                        
                        IndexOrder = model.IndexOrder,
                        IsActive = model.IsActive, 
                        Name = model.Name, 
                      


                    };
                    _OrganizationalPositionRepository.Add(item);
                    SaveChange();
                    return item;
                }
                else
                {
                    var item = _OrganizationalPositionRepository.Find(model.Id);
                    
                    item.IndexOrder = model.IndexOrder;
                    item.IsActive = model.IsActive; 
                    item.Name = model.Name; 
                    _OrganizationalPositionRepository.Update(item);
                    SaveChange();
                    return item;
                }
            }
            catch (Exception er)
            {
                errors.Add(new GlobalError() { title = MessageResource.ErrorinAction, errortype = ErrorType.ServerError, message = er.Message, errorkey = "" });


            }

            return null;
           

        }





        public async Task<CommandResult<OrganizationalPositionViewModel>> AddOrUpdateAsync(OrganizationalPositionViewModel model )
        {
            var   errors = new List<GlobalError>();
            var res = new CommandResult<OrganizationalPositionViewModel>(true, model, errors);

            try
            {
                if (model.Id == null || model.Id.Value == 0)
                {
                    var item = new OrganizationalPosition()
                    {

                         
                        IndexOrder = model.IndexOrder,
                        IsActive = model.IsActive,
                        Name = model.Name,
                       
                       

                    };
                    _OrganizationalPositionRepository.Add(item);
                    SaveChange();

                    return res;
                }
                else
                {
                    var item = _OrganizationalPositionRepository.Find(model.Id);
                   
                    item.IndexOrder = model.IndexOrder;
                    item.IsActive = model.IsActive;
                    item.Name = model.Name;
                     

                    _OrganizationalPositionRepository.Update(item);
                    SaveChange();
                    return res;
                }
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                errors.Add(new GlobalError() { title = MessageResource.ErrorinAction, errortype = ErrorType.ServerError, message = er.Message, errorkey = "" });
                res.Errors = errors;

            }

            return res;


        }


        public async Task<CommandResult<bool>> RemoveAsync(int id )
        {
            var  errors = new List<GlobalError>();
            var item = _OrganizationalPositionRepository.Find(id);
            var res = new CommandResult<bool>(true, true, errors);

            if (item == null)
            {
                errors.Add(new GlobalError()
                {
                    message = MessageResource.NotFound,
                    errorkey = id.ToString(),
                    title = MessageResource.Delete,
                    errortype = ErrorType.NotFound

                });
                res.Errors = errors;
                res.IsSuccess = false;
                return res;


            }
            if (!CanDelete(id, out errors))
            {
                res.Errors = errors;
                res.IsSuccess = false;
                return res;
            }


            _OrganizationalPositionRepository.Remove(item);
            SaveChange();
            return res;
        }


         public async   Task<CommandResult<bool>> DeleteAsync(int id )
        {
           var  errors = new List<GlobalError>();
            var res = new CommandResult<bool>(true, true, errors);
            var item = _OrganizationalPositionRepository.Find(id);
            if (item == null)
            {
                errors.Add(new GlobalError()
                {
                    message = MessageResource.NotFound,
                    errorkey = id.ToString(),
                    title = MessageResource.Delete,
                    errortype = ErrorType.NotFound

                });
                res.Errors = errors;
                res.IsSuccess = false;
                return res;


            }
            if (!CanDelete(id, out errors))
            {
                res.Errors = errors;
                res.IsSuccess = false;
                return res;
            }
            
            _OrganizationalPositionRepository.Update(item);
            SaveChange();


            
            return res;
        }




        public void SaveChange()
        {
            _uow.SaveChanges();
        }

        #endregion




        public async Task<ApiResult<List<BaseDataModel>>> GetPositionListAsync()
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {
                res.Data = await _OrganizationalPositionRepository
                     .Where(w => w.IsActive)
                     .OrderByDescending(o => o.Name).Select
                     (s => new BaseDataModel()
                     {
                         Text = s.Name,
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





        #region private Methode

        private bool CanDelete(int id, out List<GlobalError> errors)
        {
            errors = new List<GlobalError>();
            bool flag = true;

            return flag;

        }

        #endregion



















    }

}
