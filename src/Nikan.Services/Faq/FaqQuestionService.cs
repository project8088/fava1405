 
using Nikan.Common;
using Nikan.Common.Resource;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.Faq;
using Nikan.ViewModel.Faq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cle.Services.Faq
{

    public interface IFaqQuestionService
    {
        #region AddOrUpdate
        /// <summary>
        /// اضافه یا ویرایش پرسش و پاسخ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ApiResult<FaqQuestionModel>> AddOrUpdate(FaqQuestionModel model, int userId);

        #endregion
        #region Remove

        Task<ApiResult<string>> Remove(int id );
       

        #endregion
       
        #region Get  
        /// <summary>
        /// دریافت تمامی پرسش و پاسخ ها
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        Task<ApiResult<List<faqDto>>> GetAllFaq(int? groupId, bool? isActive = null);

        /// <summary>
        /// دریافت جزئیات یک پرسش و پاسخ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResult<faqDto>> GetFaq(int id);


        #endregion
    }
    public class FaqQuestionService : IFaqQuestionService
    {


        #region Ctor
        private readonly IUnitOfWork _uow;
        private readonly DbSet<FaqQuestion> _faqQuestionRepository;

        public FaqQuestionService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _faqQuestionRepository = _uow.Set<FaqQuestion>();
        }

        #endregion 
    
        public async Task<ApiResult<List<faqDto>>> GetAllFaq(int? groupId,  bool? isActive =null)
        {
            var res = new ApiResult<List<faqDto>>(true, ApiResultStatusCode.Success, new List<faqDto>());
            try
            {
                var query = _faqQuestionRepository.Where(w => w.IsDeleted != true);
                if(isActive.HasValue)
                {
                    query = query.Where(s => s.IsActive == isActive);
                }
                if (groupId.HasValue)
                {
                    query = query.Where(s => s.QuestionGroupTypeId == groupId);
                }


                res.Data = await query.Select(s => new faqDto()
                {
                    Description = s.Description,
                    CreateOnDate=s.CreateOnDate,
                    Title=s.Title,
                    TagNames=s.TagNames,
                    ViewCount=s.ViewCount,
                    QuestionGroupTypeId=s.QuestionGroupTypeId,
                    QuestionGroup=s.QuestionGroupType.Title,
                    OrganizationId=s.OrganizationalUnitId,
                    Id=s.Id 

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

        public async Task<ApiResult<faqDto>> GetFaq(int id)
        {
            var res = new ApiResult<faqDto>(true, ApiResultStatusCode.Success, new faqDto());
            try
            {
                var faq = await _faqQuestionRepository.Include(i=>i.QuestionGroupType).FirstOrDefaultAsync(w => w.Id == id);
                if(faq==null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;
                }


                res.Data =  new faqDto()
                {
                    Description = faq.Description,
                    CreateOnDate = faq.CreateOnDate,
                    Title = faq.Title,
                    TagNames = faq.TagNames,
                    ViewCount = faq.ViewCount,
                    QuestionGroupTypeId = faq.QuestionGroupTypeId,
                    QuestionGroup = faq.QuestionGroupType.Title,
                    OrganizationId = faq.OrganizationalUnitId,
                    Id = faq.Id,
                    IsActive=faq.IsActive
                    

                } ;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }
         
        public async Task<ApiResult<FaqQuestionModel>> AddOrUpdate(FaqQuestionModel model,int userId )
        {

            var res = new ApiResult<FaqQuestionModel>(true, ApiResultStatusCode.Success, new FaqQuestionModel());
            try
            {
                if (model.Id == null)
                {
                    var item = new FaqQuestion
                    {
                        Icon=model.Icon,
                        Description=model.Description, 
                        CreateOnDate= DateTime.Now,
                        CreatedById=userId,
                        IndexOrder=model.IndexOrder,
                        QuestionGroupTypeId=model.QuestionGroupTypeId, 
                        IsActive=model.IsActive,
                        TagNames=model.TagNames,
                        Title=model.Title, 
                    };


                    await _faqQuestionRepository.AddAsync(item);
                    await _uow.SaveChangesAsync();

                }
                else
                {

                    var item = await _faqQuestionRepository.FirstOrDefaultAsync(w => w.Id == model.Id);
                    if (item == null)
                    {
                        res.IsSuccess = false;
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        res.Messages = "رکوردی یافت نشد";
                        return res;
                    }

                    item.Title = model.Title;
                    item.QuestionGroupTypeId = model.QuestionGroupTypeId;
                    item.IsActive = model.IsActive;
                    item.TagNames = model.TagNames;
                    item.Title = model.Title;
                    item.Description = model.Description;
                    item.Icon = model.Icon;
                    item.ModifiedById = userId;
                    item.ModifiedOnDate = DateTime.Now;
                    
                    _faqQuestionRepository.Update(item);
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


        public async Task<ApiResult<string>> Remove(int id) 
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            try
            {
                var item = await _faqQuestionRepository.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                }


                item.ModifiedOnDate = DateTime.Now;
                item.IsDeleted = true;
                _faqQuestionRepository.Update(item); 
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
