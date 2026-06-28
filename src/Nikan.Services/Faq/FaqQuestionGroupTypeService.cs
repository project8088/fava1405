
using Nikan.Common;
using Nikan.Common.Resource;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.Faq;
using Nikan.ViewModel;
using Nikan.ViewModel.Faq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cle.Services.Faq
{
    

    public interface IFaqQuestionGroupTypeService
    {

      
        Task<ApiResult<FaqQuestionGroupTypeDto>> AddOrUpdate(FaqQuestionGroupTypeDto model, int userId);
 

        /// <summary>
        /// حذف منطقی یک گروه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<string>> Remove(int id );
      

       

         /// <summary>
         /// دریافت لیست گروههای پرسش و پاسخ
         /// </summary>
         /// <param name="selected"></param>
         /// <returns></returns>
        Task<ApiResult<List<BaseDataModel>>> GetFaqGroups(int? selected); 
       

        

        /// <summary>
        /// دریافت لیست گروههای پرسش و پاسخ
        /// </summary>
        /// <returns></returns>
        Task<List<BaseDataModel>> GetAllGroupForAdmin();



        /// <summary>
        /// دریافت لیست گروههای پرسش و پاسخ برای کاربر
        /// </summary>
        /// <returns></returns>
        Task<List<BaseDataModel>> GetAllGroupForUser();
        Task<ApiResult<List<FaqQuestionGroupTypeDto>>> GetAllFaqGroups();
    }
    public class FaqQuestionGroupTypeService : IFaqQuestionGroupTypeService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<FaqQuestionGroupType> _faqQuestionGroupTypeRepository;

        public FaqQuestionGroupTypeService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _faqQuestionGroupTypeRepository = _uow.Set<FaqQuestionGroupType>(); 
        }




        #region IFaqQuestionGroupTypeService Members
       
   
         
        public async Task<ApiResult<List<BaseDataModel>>> GetFaqGroups(int? selected)
        { 
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());
            try
            {

                res.Data = await _faqQuestionGroupTypeRepository.Where(w => w.IsDeleted != true).Select(s => new BaseDataModel()
                {
                    Description = s.Description,
                    Disabled = !s.IsActive,
                    Selected = s.Id == selected,
                    Text = s.Title,
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


        public async Task<ApiResult<List<FaqQuestionGroupTypeDto>>> GetAllFaqGroups()
        {
            var res = new ApiResult<List<FaqQuestionGroupTypeDto>>(true, ApiResultStatusCode.Success, new List<FaqQuestionGroupTypeDto>());
            try
            {

                res.Data = await _faqQuestionGroupTypeRepository.Where(w => w.IsDeleted != true).Select(s => new FaqQuestionGroupTypeDto()
                {
                    Description=s.Description,
                    Icon=s.Icon,
                    Id=s.Id,
                    IndexOrder=s.IndexOrder,
                    IsActive=s.IsActive,
                    Title=s.Title
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


        public async Task<List<BaseDataModel>> GetAllGroupForAdmin()
        {
            return await  _faqQuestionGroupTypeRepository.Select(s => new BaseDataModel()
            {
                Text=s.IsActive? s.Title :   s.Title  + "(غیرفعال)",
                Key=s.Id.ToString()  
            }).ToListAsync(); 
        }

        public async Task<List<BaseDataModel>> GetAllGroupForUser()
        {
            return await _faqQuestionGroupTypeRepository.Where(w=>w.IsActive && w.IsDeleted !=true).Select(s => new BaseDataModel()
            {
                Text = s.Title,
                Key = s.Id.ToString(),
                
            }).ToListAsync();
        }

         


       


        public async Task<ApiResult<FaqQuestionGroupTypeDto>> AddOrUpdate(FaqQuestionGroupTypeDto model,int userId )
        {

            var res = new ApiResult<FaqQuestionGroupTypeDto>(true, ApiResultStatusCode.Success, new FaqQuestionGroupTypeDto());
            try
            {
                if (model.Id == null || model.Id.Value == 0)
                {
                    var item = new FaqQuestionGroupType()
                    {
                        CreatedById = userId,
                        Description = model.Description,
                        Icon = model.Icon,
                        IndexOrder = model.IndexOrder,
                        IsActive = model.IsActive, 
                        Title = model.Title, 
                        CreateOnDate = DateTime.Now,
                      

                    };
                    await _faqQuestionGroupTypeRepository.AddAsync(item);
                    await _uow.SaveChangesAsync();
                    
                }
                else
                {
                    var item = _faqQuestionGroupTypeRepository.Find(model.Id);
                    item.Description = model.Description;
                    item.Icon = model.Icon;
                    item.IndexOrder = model.IndexOrder;
                    item.IsActive = model.IsActive; 
                    item.Title = model.Title; 
                    item.ModifiedById =userId;
                    item.ModifiedOnDate = DateTime.Now;
                    _faqQuestionGroupTypeRepository.Update(item); 
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

                var item = await _faqQuestionGroupTypeRepository.FirstOrDefaultAsync(w => w.Id == Id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    return res;
                }

                item.IsDeleted = true;
                _faqQuestionGroupTypeRepository.Update(item);
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


     


      

        #endregion









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
