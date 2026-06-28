
using Nikan.Common;
using Nikan.Common.Resource;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses;
using Nikan.DomainClasses.BaseEntity;
using Nikan.DomainClasses.Faq;
using Nikan.ViewModel.Faq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Nikan.ViewModel.Ticket;
using System.Threading.Tasks;
using Nikan.ViewModel;

namespace cle.Services.Faq
{


    public interface ITicketSubjecteService
    {
        Task<ApiResult<TicketSubjectDto>> AddOrUpdate(TicketSubjectDto model);
        Task<ApiResult<List<TicketSubjectInfo>>> GetAll();
        Task<ApiResult<List<BaseDataModel>>> GetList();
        Task<ApiResult<List<BaseDataModel>>> GetList(string unitId);
        Task<ApiResult<TicketSubjectInfo>> GetSubject(string id);
        Task<ApiResult<string>> Remove(string id);
    }
    public class TicketSubjecteService : ITicketSubjecteService
    {

        private readonly IUnitOfWork _uow;
        private readonly DbSet<TicketSubject> _subject;

        public TicketSubjecteService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _subject = _uow.Set<TicketSubject>();
        }


        public async Task<ApiResult<TicketSubjectInfo>> GetSubject(string id)
        {

            var  res = new ApiResult<TicketSubjectInfo>(true, ApiResultStatusCode.Success, null);

            try
            {
                var sub  = await _subject.Select(subject=>  new TicketSubjectInfo()
                {
                    CreateOnDate = subject.CreateOnDate,
                    Id = subject.Id,
                    IsActive = subject.IsActive,
                    Title = subject.Title,
                    Description = subject.Description,
                    IndexOrder = subject.IndexOrder,
                    OrganizationalUnit = subject.OrganizationalUnit.Name,
                    OrganizationalUnitId = subject.OrganizationalUnitId

                }).FirstOrDefaultAsync(w => w.Id == id);
                if (sub == null)
                {
                    res.IsSuccess = false;
                    res.Messages = "  یافت نشد";
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    return res;
                } 

                res.Data = sub;
                 
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطای سیستمی :خطایی در واکشی   رخ داده است";
                res.StatusCode = ApiResultStatusCode.ServerError;
            }

            return res;

        }
        public async Task<ApiResult<List<TicketSubjectInfo>>> GetAll()
        {
            var res = new ApiResult<List<TicketSubjectInfo>>(true, ApiResultStatusCode.Success, new List<TicketSubjectInfo>());

            try
            {
                res.Data = await _subject.Where(m => m.IsDeleted !=true) 
           .Select(s => new TicketSubjectInfo()
          {
              CreateOnDate=s.CreateOnDate,
              Description=s.Description,
              Id=s.Id,
              IndexOrder=s.IndexOrder,
              IsActive=s.IsActive,
              Title=s.Title,
              OrganizationalUnit = s.OrganizationalUnit.Name,
              OrganizationalUnitId = s.OrganizationalUnitId,
              

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

        public async Task<ApiResult<List<BaseDataModel>>> GetList()
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {
                res.Data = await _subject.Where(m => m.IsDeleted != true && m.IsActive )
           .Select(s => new BaseDataModel()
           {
                Description=s.Description,
                Key=s.Id.ToString(),
                Text=s.Title

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


        public async Task<ApiResult<List<BaseDataModel>>> GetList(string unitId)
        {
            var res = new ApiResult<List<BaseDataModel>>(true, ApiResultStatusCode.Success, new List<BaseDataModel>());

            try
            {
                res.Data = await _subject.Where(m => m.IsDeleted != true && m.IsActive && m.OrganizationalUnitId== unitId) 
           .Select(s => new BaseDataModel()
           {
               Description = s.Description,
               Key = s.Id.ToString(),
               Text = s.Title

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



        public async Task<ApiResult<string>> Remove(string  id )
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "");

            try
            {
                var item = await _subject.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {

                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.ServerError;
                    res.Messages = " رکوردی یافت نشد ";
                }

                _subject.Remove(item);
                await _uow.SaveChangesAsync();
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.StatusCode = ApiResultStatusCode.ServerError;
                res.Messages = "خطایی رخ داده است ";
            }

            return res;
        }

        public async Task<ApiResult<TicketSubjectDto>> AddOrUpdate(TicketSubjectDto model)
        {

            var res = new ApiResult<TicketSubjectDto>(true, ApiResultStatusCode.Success, new TicketSubjectDto());

            try
            {

                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    var add = new TicketSubject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        IndexOrder = 0,
                        Description = model.Description,
                        Title = model.Title,
                        IsActive = model.IsActive,
                        CreateOnDate = DateTime.Now,
                        ModifiedOnDate = DateTime.Now,
                        OrganizationalUnitId=model.OrganizationalUnitId,
                        ThumbUrl=model.ThumbUrl,
                        


                    };


                    await _subject.AddAsync(add);
                    await _uow.SaveChangesAsync();
                    res.Data.Id = add.Id;

                }
                else
                {
                    var subject = await _subject.FirstOrDefaultAsync(w => w.Id == model.Id);
                    if (subject == null)
                    {
                        res.Data = null;
                        res.IsSuccess = false;
                        res.Messages = " یافت نشد";
                        res.StatusCode = ApiResultStatusCode.NotFound;
                        return res;

                    }


                    subject.Description = model.Description;
                    subject.Title = model.Title;
                    subject.IsActive = model.IsActive;
                    subject.OrganizationalUnitId = model.OrganizationalUnitId;
                    subject.ThumbUrl = model.ThumbUrl;
                    _subject.Update(subject);
                    await _uow.SaveChangesAsync();
                    res.Data.Id = subject.Id;

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
































    }

}
