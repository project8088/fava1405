using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.Citizens;
using Nikan.ViewModel;
using Nikan.ViewModel.Citizens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services.Citizens
{
    public interface ICitizenFeedbackService
    {
        Task<ApiResult> AddFeedbacke(CitizenFeedbackDto model, int userId);
        Task<ApiResult<List<CitizenFeedbackInfo>>> GetAllCitizenFeedbacks(string userCode);
        Task<ApiResult<List<BaseDataModel>>> GetBaseListFeedbacke(int? selected = null);
        Task<ApiResult<CitizenFeedbackInfo>> GetFeedback(int id);
        Task<ApiResult<string>> Remove(int id);
        Task<ApiResult<PageFeedbackViewModel>> Searchfeedbacks(int pageNumber, int pageSize, int? feedbackId, DateTime? FromDate = null, DateTime? ToDate = null, string name = null, string nationalCode = null);
    }
    public class CitizenFeedbackService : ICitizenFeedbackService
    {


        #region Ctor
        private readonly IUnitOfWork _uow;
        private readonly DbSet<CitizenFeedback> _citizenFeedback;
        private readonly DbSet<Feedback> _Feedback;
        private readonly DbSet<Citizen> _citizen;

        public CitizenFeedbackService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _citizenFeedback = _uow.Set<CitizenFeedback>();
            _Feedback = _uow.Set<Feedback>();
            _citizen = _uow.Set<Citizen>();


        }

        #endregion


        public async Task<ApiResult<PageFeedbackViewModel>> Searchfeedbacks(
         int pageNumber, int pageSize,
         int? feedbackId,
         DateTime? FromDate = null,
         DateTime? ToDate = null,
         string name = null,
         string nationalCode = null

         )
        {



            var res = new ApiResult<PageFeedbackViewModel>(true, ApiResultStatusCode.Success, new PageFeedbackViewModel());
            try
            {
                 var offset = (pageNumber ) * pageSize ;
                var query = _citizenFeedback.AsQueryable();


                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(w =>
                    EF.Functions.Like(w.Citizen.FirstName, "%" + name + "%")
                    ||
                      EF.Functions.Like(w.Citizen.LastName, "%" + name + "%")

                    );
                }

                if (!string.IsNullOrEmpty(nationalCode))
                {
                    query = query.Where(w => w.Citizen.NationCode == nationalCode);
                }

                if (feedbackId != null)
                {
                    query = query.Where(w => w.FeedbackId == feedbackId);
                }

                if (FromDate != null)
                {
                    query = query.Where(w => w.OnDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.OnDate <= ToDate);
                } 
                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items =  await query.Select(s => new CitizenFeedbackInfo()
                {
                    Id = s.Id,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    UserCode=s.Citizen.UserCode,
                    byUserCode = s.Operation.UserCode,
                    Feedback = s.Feedback.FeedbackTitle,
                    FeedbackDescription = s.FeedbackDescription,
                    FeedbackId = s.FeedbackId,
                    OnDate = s.OnDate,
                    byUser = s.Operation.DisplayName,
                    byUserId = s.OperationId,
                   

                }).OrderByDescending(o=>o.Id) .Skip(offset).Take(pageSize).ToListAsync();


            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }
            return res;



        }


        public async Task<ApiResult<List<CitizenFeedbackInfo>>> GetAllCitizenFeedbacks(string   userCode )
        {
            var res = new ApiResult<List<CitizenFeedbackInfo>>(true, ApiResultStatusCode.Success, new List<CitizenFeedbackInfo>());
            try
            {
                var guid = Guid.Empty;
                Guid.TryParse(userCode, out guid);
                var query = _citizenFeedback.Where(w => w.IsDeleted != true && w.Citizen.UserCode== guid); 

                res.Data = await query.Select(s => new CitizenFeedbackInfo()
                { 
                    Id = s.Id,
                    Citizen=s.Citizen.FirstName +" "+s.Citizen.LastName,
                    CitizenId=s.CitizenId,
                    Feedback=s.Feedback.FeedbackTitle,
                    FeedbackDescription=s.FeedbackDescription,
                    FeedbackId=s.FeedbackId,
                    OnDate=s.OnDate,
                    byUser = s.Operation.DisplayName,
                    byUserId = s.OperationId,
                    UserCode = s.Citizen.UserCode,
                    byUserCode = s.Operation.UserCode,
                }).OrderByDescending(o=>o.Id).ToListAsync();  
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;

            }

            return res;
        }

        public async Task<ApiResult<CitizenFeedbackInfo>> GetFeedback(int id)
        {
            var res = new ApiResult<CitizenFeedbackInfo>(true, ApiResultStatusCode.Success, new CitizenFeedbackInfo());
            try
            {
                var query = _citizenFeedback.Where(w => w.Id==id); 
                var data = await query.Select(s => new CitizenFeedbackInfo()
                {
                    Id = s.Id,
                    Citizen = s.Citizen.FirstName + " " + s.Citizen.LastName,
                    CitizenId = s.CitizenId,
                    Feedback = s.Feedback.FeedbackTitle,
                    FeedbackDescription = s.FeedbackDescription,
                    FeedbackId = s.FeedbackId,
                    OnDate = s.OnDate,
                    byUser = s.Operation.DisplayName,
                    byUserId = s.OperationId,
                    UserCode = s.Citizen.UserCode,
                    byUserCode = s.Operation.UserCode,
                }).FirstOrDefaultAsync();


                if (data == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
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

        public async Task<ApiResult> AddFeedbacke(CitizenFeedbackDto model, int userId)
        {

            var res = new ApiResult(true, ApiResultStatusCode.Success,"بازخورد با موفقیت ثبت شد");
            try
            {

                if (string.IsNullOrWhiteSpace(model.UserCode))
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "شناسه شهروند مشخص نشده است";
                    return res;
                }

                if (model.FeedbackId == 0)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = "عنوان بازخورد مشخص نشده است";
                    return res;
                }

                var userCode = Guid.Empty;
                Guid.TryParse(model.UserCode, out userCode);
                var citizen = await _citizen.AsNoTracking().FirstOrDefaultAsync(w => w.UserCode == userCode);
                if (citizen == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    res.Messages = "شهروندی یافت نشد";
                    return res;
                }

                var item = new CitizenFeedback
                    {
                        CitizenId= citizen.CitizenId,
                        FeedbackDescription=model.FeedbackDescription,
                        FeedbackId=model.FeedbackId,
                        OnDate=DateTime.Now,
                        OperationId= userId, 
                    }; 
                    await _citizenFeedback.AddAsync(item);
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
         
        public async Task<ApiResult<string>> Remove(int id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");
            try
            {
                var item = await _citizenFeedback.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                } 
                item.IsDeleted = true;
                _citizenFeedback.Update(item);
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
         
        public async Task<ApiResult<List<BaseDataModel>>> GetBaseListFeedbacke(int? selected = null)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {
                res.Data = await _Feedback.Where(w => w.IsDeleted != true)
                     .OrderBy(o => o.FeedbackTitle).Select
                      (s => new BaseDataModel()
                      {
                          Text = s.FeedbackTitle,
                          Key = s.Id.ToString(), 
                          Selected = s.Id == selected, 
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



    }

}
