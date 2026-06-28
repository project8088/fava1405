using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.ViewModel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nikan.Services.Events
{
    public interface IEventService
    {
        Task<ApiResult> AddEvent(EventDto model);
        Task<ApiResult<PagedEventViewModel>> GetAllEvent(int pageNumber, int pageSize, DateTime? FromDate = null, DateTime? ToDate = null, string actionName = null);
        Task<ApiResult<List<EventsInfo>>> GetCitizenTopEvent(string userCode, int top);
        Task<ApiResult<EventsInfo>> GetEvent(int id);
        Task<ApiResult<List<EventsInfo>>> GetTopEvent(int top);
        Task<ApiResult> RemoveEvent(int id);
    }


    public class EventService : IEventService
    {

        #region Field
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Event> _event;

       
        #endregion
        #region Constractor

        public EventService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _event = _uow.Set<Event>();
           



        }
        #endregion

        public async Task<ApiResult<PagedEventViewModel>> GetAllEvent(int pageNumber, int pageSize,
            DateTime? FromDate = null,
            DateTime? ToDate = null,
            string actionName = null 
             )
        {
            var res = new ApiResult<PagedEventViewModel>(true, ApiResultStatusCode.Success, new PagedEventViewModel(), "");
            try
            {

                 var offset = (pageNumber ) * pageSize ;
                var query = _event.AsQueryable();

                if (!string.IsNullOrEmpty(actionName))
                {
                    query = query.Where(w => EF.Functions.Like(w.ActionName, "%" + actionName + "%"));
                }


                

                if (FromDate != null)
                {
                    query = query.Where(w => w.CreateDate >= FromDate);
                }
                if (ToDate != null)
                {
                    query = query.Where(w => w.CreateDate <= ToDate);
                }

                res.Data.TotalItems = await query.CountAsync();
                res.Data.Items = await query.Select(s => new EventsInfo()
                {
                    ActionName=s.ActionName,
                    Description=s.Description,
                    CreateDate=s.CreateDate,
                    Code=s.Code,
                    Id=s.Id,
                    EventPriority=s.EventPriority.ToString(),
                    EventSection=s.EventSection.ToString().Replace("_", " "),
                    EventType=s.EventType.ToString(),
                    Operation=s.Operation.Username,
                    OperationId=s.OperationId,
                    StrCode=s.StrCode,
                    UserId=s.UserId,
                    User=s.User.Username,
                    UserIp=s.UserIp,
                    WebSite=s.WebSite,
                    
                     

                }).Skip(offset).Take(pageSize).ToListAsync();




            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }


        public async Task<ApiResult<List<EventsInfo>>> GetCitizenTopEvent(string userCode,  int top)
        {
            var res = new ApiResult<List<EventsInfo>>(true, ApiResultStatusCode.Success, new List<EventsInfo>(), "");
            try
            {
                var code = Guid.NewGuid();
                Guid.TryParse(userCode ,out code);

                res.Data = await _event.Where(w =>w.User.UserCode== code &&  w.IsDeleted != true).Select(s => new EventsInfo()
                {
                    ActionName = s.ActionName,
                    Description = s.Description,
                    CreateDate = s.CreateDate,
                    Code = s.Code,
                    Id = s.Id,
                    EventPriority = s.EventPriority.ToString(),
                    EventSection = s.EventSection.ToString().Replace("_", " "),
                    EventType = s.EventType.ToString(),
                    Operation = s.Operation.Username,
                    OperationId = s.OperationId,
                    StrCode = s.StrCode,
                    UserId = s.UserId,
                    User = s.User.Username,
                    UserIp = s.UserIp,
                    WebSite = s.WebSite,
                }).OrderByDescending(o => o.Id).Take(top).ToListAsync();

            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }


        public async Task<ApiResult<List<EventsInfo>>> GetTopEvent(int top )
        {
            var res = new ApiResult<List<EventsInfo>>(true, ApiResultStatusCode.Success, new List<EventsInfo>(), "");
            try
            {

               
                res.Data = await _event.Where(w=>w.IsDeleted!=true).Select(s => new EventsInfo()
                {
                    ActionName = s.ActionName,
                    Description = s.Description,
                    CreateDate = s.CreateDate,
                    Code = s.Code,
                    Id = s.Id,
                    EventPriority = s.EventPriority.ToString(),
                    EventSection = s.EventSection.ToString().Replace("_", " "),
                    EventType = s.EventType.ToString(),
                    Operation = s.Operation.Username,
                    OperationId = s.OperationId,
                    StrCode = s.StrCode,
                    UserId = s.UserId,
                    User = s.User.Username,
                    UserIp = s.UserIp,
                    WebSite = s.WebSite, 
                }).OrderByDescending(o=>o.Id).Take(top).ToListAsync();
                 
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }



        public async Task<ApiResult<EventsInfo>> GetEvent(int id)
        {
            var res = new ApiResult<EventsInfo>(true, ApiResultStatusCode.Success, new EventsInfo(), "");
            try
            {



                var item = await _event.Where(w => w.Id == id).Select(s => new EventsInfo()
                {
                    ActionName = s.ActionName,
                    Description = s.Description,
                    CreateDate = s.CreateDate,
                    Code = s.Code,
                    Id = s.Id,
                    EventPriority = s.EventPriority.ToString(),
                    EventSection = s.EventSection.ToString().Replace("_", " "),
                    EventType = s.EventType.ToString(),
                    Operation = s.Operation.Username,
                    OperationId = s.OperationId,
                    StrCode = s.StrCode,
                    UserId = s.UserId,
                    User = s.User.Username,
                    UserIp = s.UserIp,
                    WebSite = s.WebSite,
                    JsonValue=s.JsonValue,

                    

                }).FirstOrDefaultAsync();
                if (item == null)
                {

                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.NotFound;
                    res.Messages = "رکوردی یافت نشد";
                    return res;
                }
               
                res.Data = item;



            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است";

            }



            return res;
        }


        public async Task<ApiResult> AddEvent(EventDto model)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "ثبت رویداد با موفقیت ایجاد گردید");
            try
            {

                if (model.OperationId == 0)
                    model.OperationId = null;

                if (model.UserId == 0)
                    model.UserId = null;




                var item = new Event
                {
                    OperationId=model.OperationId,
                    UserId=model.UserId,
                    WebSite=model.WebSite,
                    UserIp=model.UserIp,
                    Description=model.Description,
                    CreateDate=DateTime.Now,
                    Code=model.Code,
                    ActionName=model.ActionName,
                    EventPriority=model.EventPriority,
                    EventSection=model.EventSection,
                    EventType=model.EventType,
                    StrCode=model.StrCode,
                    JsonValue=""
                    

                };
                 
                await _event.AddAsync(item);
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


     
    
        public async Task<ApiResult> RemoveEvent(int id)
        {
            var res = new ApiResult(true, ApiResultStatusCode.Success, "با موفقیت حذف گردید");
            var item = await _event.FirstOrDefaultAsync(w => w.Id == id);
            if (item == null)
            {

                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.NotFound;
                res.Messages = "رکوردی یافت نشد";
                return res;
            }

            item.IsDeleted = true;
            _event.Update(item);
            await _uow.SaveChangesAsync();
            return res;
        }

     

    }
}
