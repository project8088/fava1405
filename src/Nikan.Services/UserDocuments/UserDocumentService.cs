using Microsoft.EntityFrameworkCore;
using Nikan.Common;
using Nikan.DataLayer.Context;
using Nikan.DomainClasses.UserDocuments;
using Nikan.ViewModel.UserDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikan.Services.UserDocuments
{
    public interface IUserDocumentService
    {
        Task<ApiResult<UserDocumentDto>> Add(UserDocumentDto model);
        Task<ApiResult<List<UserDocumentInfo>>> GetAllDocGrpupsAndUserDocuments(int userId);
        Task<ApiResult<UserDocumentInfo>> GetDocument(int id);
        Task<ApiResult<List<UserDocumentInfo>>> GetUserDocuments(int userId);
        Task<ApiResult<string>> Remove(int id);
    }
    public class UserDocumentService : IUserDocumentService
    {

        #region Field
        private readonly IUnitOfWork _uow;
        private readonly DbSet<UserDocument> _attachment;
        private readonly DbSet<UserDocumentGroup> _docGroups;

        #endregion
        #region Constractor

        public UserDocumentService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(_uow));
            _attachment = _uow.Set<UserDocument>();
            _docGroups = _uow.Set<UserDocumentGroup>();
        }
        #endregion



        public async Task<ApiResult<UserDocumentDto>> Add(UserDocumentDto model )
        {
            var res = new ApiResult<UserDocumentDto>(true, ApiResultStatusCode.Success, new UserDocumentDto());

            try
            {
                 
                var info = new UserDocument()
                {
                    
                    Extension = model.Extension,
                    FileName = model.FileName, 
                    Size = model.Size,
                    OwnerId = model.OwnerId,
                    ThumnailPath = "",
                    AttachedOnDate=DateTime.Now,
                    DocumentGroupId=model.DocumentGroupId,
                    FilePath=model.FilePath,
                    Title=model.Title,
                    DocumentStatus=Common.GlobalEnum.UserDocumentStatusEnum.در_دست_بررسی,
                    Description="", 
                };
                await _attachment.AddAsync(info);
                await _uow.SaveChangesAsync();
                model.Id = info.Id;
                res.Data = model; 
            }
            catch (Exception er)
            {
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";
            }

            return res;

        }


        public async Task<ApiResult<List<UserDocumentInfo>>> GetAllDocGrpupsAndUserDocuments(int userId)
        {
            var res = new ApiResult<List<UserDocumentInfo>>(true, ApiResultStatusCode.Success, new List<UserDocumentInfo>());

            try
            {

                var docGroups = await _docGroups
                    .Where(w => w.IsActive).ToListAsync();
                var query = _attachment.Where(w => w.IsDeleted != true && w.OwnerId == userId);
                var list = await query.Select(s => new UserDocumentInfo()
                {
                    Extension = s.Extension,
                    FileName = s.FileName,
                    FilePath = s.FilePath,
                    Id = s.Id,
                    Size = s.Size,
                    Owner = s.Owner == null ? "" : s.Owner.Username,
                    OwnerId = s.OwnerId,
                    DocumentGroupId = s.DocumentGroupId,
                    AttachedOnDate = s.AttachedOnDate,
                    Description = s.Description,
                    DocumentStatus = s.DocumentStatus,
                    Title = s.Title,
                    DocumentGroup = s.DocumentGroup.Title,
                    ThumnailPath = s.ThumnailPath,


                }).ToListAsync();




                foreach (var grp in docGroups)
                {
                    if(! list.Any(w=>w.DocumentGroupId==grp.Id))
                    {
                        list.Add(new UserDocumentInfo()
                        {
                            DocumentGroupId = grp.Id,
                            DocumentGroup= grp.Title,
                            DocumentGroupDescription=grp.Description

                        });
                    } 
                }

                res.Data = list;
            }
            catch (Exception er)
            {
                res.Data = null;
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";

            }
            return res;
        }



        public async Task<ApiResult<List<UserDocumentInfo>>> GetUserDocuments(int userId)
        {
            var res = new ApiResult<List<UserDocumentInfo>>(true, ApiResultStatusCode.Success, new List<UserDocumentInfo>());

            try
            {



                var query = _attachment.Where(w => w.IsDeleted != true && w.OwnerId == userId);
                res.Data = await query.Select(s => new UserDocumentInfo()
                { 
                    Extension = s.Extension,
                    FileName = s.FileName,
                    FilePath = s.FilePath,
                    Id = s.Id,
                    Size = s.Size,
                    Owner = s.Owner == null ? "" : s.Owner.Username,
                    OwnerId = s.OwnerId,
                    DocumentGroupId = s.DocumentGroupId,
                    AttachedOnDate = s.AttachedOnDate,
                    Description = s.Description,
                    DocumentStatus = s.DocumentStatus,
                    Title = s.Title,
                    DocumentGroup = s.DocumentGroup.Title,
                    ThumnailPath = s.ThumnailPath,


                }).ToListAsync();

            }
            catch (Exception er)
            {
                res.Data = null;
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";

            }
            return res;
        }

        public async Task<ApiResult<UserDocumentInfo>> GetDocument(int id)
        {
            var res = new ApiResult<UserDocumentInfo>(true, ApiResultStatusCode.Success, new UserDocumentInfo());

            try
            {
                var query = _attachment.Where(w => w.IsDeleted != true && w.Id == id);
                res.Data = await query.Select(s => new UserDocumentInfo()
                {

                     
                    Extension = s.Extension,
                    FileName = s.FileName,
                    FilePath = s.FilePath,
                    Id = s.Id,
                    Size = s.Size,
                    Owner = s.Owner == null ? "" : s.Owner.Username,
                    OwnerId = s.OwnerId,
                    DocumentGroupId = s.DocumentGroupId,
                    AttachedOnDate=s.AttachedOnDate,
                    Description=s.Description,
                    DocumentStatus=s.DocumentStatus,
                    Title=s.Title,
                    DocumentGroup=s.DocumentGroup.Title,
                    ThumnailPath=s.ThumnailPath,
                    

                }).FirstOrDefaultAsync();

            }
            catch (Exception er)
            {
                res.Data = null;
                res.IsSuccess = false;
                res.Messages = "خطایی رخ داده است";

            }
            return res;
        }

        public async Task<ApiResult<string>> Remove(int id)
        {
            var res = new ApiResult<string>(true, ApiResultStatusCode.Success, "حذف   با موفقیت صورت گرفت");
            try
            {
                var item = await _attachment.FirstOrDefaultAsync(w => w.Id == id);
                if (item == null)
                {
                    res.IsSuccess = false;
                    res.StatusCode = ApiResultStatusCode.BadRequest;
                    res.Messages = "رکوردی یافت نشد";
                    return res;

                }



                item.IsDeleted = true;
                _attachment.Update(item);
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



    }
}
